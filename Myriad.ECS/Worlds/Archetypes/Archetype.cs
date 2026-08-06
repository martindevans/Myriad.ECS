using System.Buffers;
using Myriad.ECS.Collections;
using Myriad.ECS.Command;
using Myriad.ECS.Components;
using Myriad.ECS.IDs;
using Myriad.ECS.Worlds.Chunks;
using System.Diagnostics;
using static Myriad.ECS.Worlds.Chunks.Chunk;

namespace Myriad.ECS.Worlds.Archetypes;

/// <summary>
/// An archetype contains all entities which share exactly the same set of components.
/// </summary>
public sealed partial class Archetype
{
    /// <summary>
    /// Number of entities in a single chunk
    /// </summary>
    internal const int CHUNK_SIZE = 1024;

    /// <summary>
    /// How many empty chunks to keep as spares
    /// </summary>
    private const int CHUNK_HOT_SPARES = 4;

    /// <summary>
    /// The world which this archetype belongs to
    /// </summary>
    public World World { get; }

    /// <summary>
    /// The components of entities in this archetype
    /// </summary>
    public FrozenOrderedListSet<ComponentID> Components { get; }

    /// <summary>
    /// A bloom filter of all the components in this archetype
    /// </summary>
    internal readonly BloomFilter32x512 ComponentsBloomFilter;

    /// <summary>
    /// The hash of all components IDs in this archetype
    /// </summary>
    internal ArchetypeHash Hash { get; }

    /// <summary>
    /// Map from component ID (index) to index in chunk
    /// </summary>
    private readonly int[] _componentIndexLookup;

    /// <summary>
    /// All chunks in this archetype
    /// </summary>
    private readonly List<Chunk> _chunks = [ ];

    /// <summary>
    /// A list of chunks which might have space to put an entity in
    /// </summary>
    private readonly List<Chunk> _chunksWithSpace = [ ];

    /// <summary>
    /// A list of empty chunks that have been removed from this archetype
    /// </summary>
    private readonly Stack<Chunk> _spareChunks = new(CHUNK_HOT_SPARES);

    private readonly ComponentID[] _componentIDs;
    private readonly Type[] _componentTypes;
    private readonly ArchetypeComponentDisposal? _disposer;
    private readonly ArchetypePhantomComponentNotifier? _phantomNotifier;

    /// <summary>
    /// The archetype that entities should be moved to when deleted. Initialised on first use, null until then.
    /// </summary>
    private Archetype? _phantomDestination;

    /// <summary>
    /// The total number of entities in this archetype
    /// </summary>
    public int EntityCount { get; private set; }

    /// <summary>
    /// Get the number of chunks in this archetype with entities
    /// </summary>
    public int ChunkCount => _chunks.Count;

    /// <summary>
    /// Indicates if any of the components in this Archetype implement <see cref="IPhantomComponent"/>;
    /// </summary>
    public bool HasPhantomComponents { get; }

    /// <summary>
    /// Indicates if any of the components in this Archetype is <see cref="Phantom"/>
    /// </summary>
    public bool IsPhantom { get; }

    /// <summary>
    /// Indicates if any of the components in this Archetype implement <see cref="IEntityRelationComponent"/>
    /// </summary>
    public bool HasRelationComponents { get; }

    /// <summary>
    /// Indicates if any of the components in this Archetype implement <see cref="IDisposableComponent"/>
    /// </summary>
    public bool HasDisposableComponents { get; }

    /// <summary>
    /// Indicates if any of the components in this Archetype implement <see cref="IPhantomNotifierComponent"/>
    /// </summary>
    public bool HasPhantomNotifierComponents { get; }

    private static long _nextId;
    /// <summary>
    /// Globally Unique ID for this archetype
    /// </summary>
    public long ArchetypeId { get; }

    internal Archetype(World world, FrozenOrderedListSet<ComponentID> components)
    {
        ArchetypeId = Interlocked.Increment(ref _nextId);

        World = world;
        Components = components;
        ComponentsBloomFilter = components.ToBloomFilter();

        // Create arrays to fills in below
        _componentTypes = new Type[components.Count];
        _componentIDs = new ComponentID[components.Count];

        // Calculate archetype hash and also keep track of the max component ID ever seen
        var maxComponentId = int.MinValue;
        foreach (var component in components)
        {
            Hash = Hash.Toggle(component);
            if (component.Value > maxComponentId)
                maxComponentId = component.Value;
        }

        // Build an array where the number at a given index is the index of the component with that ID
        _componentIndexLookup = maxComponentId == int.MinValue ? [ ] : new int[maxComponentId + 1];
        Array.Fill(_componentIndexLookup, -1);
        var idx = 0;
        foreach (var component in components)
        {
            _componentTypes[idx] = component.Type;
            _componentIndexLookup[component.Value] = idx;
            _componentIDs[idx] = component;

            idx++;
        }

        // Gather flags for special components
        foreach (var component in components)
        {
            IsPhantom |= component == ComponentID<Phantom>.ID;
            HasPhantomComponents |= component.IsPhantomComponent;
            HasRelationComponents |= component.IsRelationComponent;
            HasDisposableComponents |= component.IsDisposableComponent;
            HasPhantomNotifierComponents |= component.IsPhantomNotifierComponent;
        }

        // Create a disposer if it's needed
        if (HasDisposableComponents)
            _disposer = new ArchetypeComponentDisposal(components);

        // Create a notifier if it's needed
        if (HasPhantomNotifierComponents && !IsPhantom)
            _phantomNotifier = new ArchetypePhantomComponentNotifier(components);
    }

    internal void Dispose(ref LazyCommandBuffer buffer)
    {
        // Wait for multithreaded access to this archetype
        Block();

        DisposeAllDisposableComponents(ref buffer);
    }

    private void DisposeAllDisposableComponents(ref LazyCommandBuffer buffer)
    {
        if (_disposer != null)
            foreach (var chunk in _chunks)
                for (var i = 0; i < chunk.EntityCount; i++)
                    _disposer.DisposeEntity(ref buffer, chunk, i);
    }

    internal ref EntityInfo CreateEntity(out EntityId entity, bool block)
    {
        // Wait for multithreaded access to this archetype
        if (block)
            Block();

        // Allocate an entity in the world
        ref var info = ref World.AllocateEntity(out entity);

        // Add it to this archetype, find a row to put components into
        AddEntity(entity, ref info, block:false);

        return ref info;
    }

    /// <summary>
    /// Delete every Entity in this archetype
    /// </summary>
    /// <param name="lazy">Lazy command buffer to use</param>
    /// <param name="blockSrc">Whether to block on the source archetype</param>
    /// <param name="blockDst">Whether to block on the destination archetype</param>
    internal void Clear(ref LazyCommandBuffer lazy, bool blockSrc, bool blockDst)
    {
        // Wait for multithreaded access to this archetype
        if (blockSrc)
            Block();

        if (HasPhantomComponents && !IsPhantom)
        {
            // Get the destination archetype for entities which are becoming phantoms and cache it
            if (_phantomDestination == null)
            {
                var c = new OrderedListSet<ComponentID>(Components)
                {
                    ComponentID<Phantom>.ID
                };
                _phantomDestination = World.GetOrCreateArchetype(c);
            }
            
            // Block on the destination
            if (blockDst)
                _phantomDestination.Block();

            // Migrate all entities in all chunks to the new archetype. Doing this does all of the bookeeping like chunk management and entity count.
            // This could be better, at the moment it just does the work on a per-entity basis, instead of doing it all in one batch.
            while (_chunks.Count > 0)
            {
                var chunk = _chunks[^1];

                while (chunk.EntityCount > 0)
                {
                    var entity = chunk.EntityIds.Span[^1];
                    ref var info = ref World.GetEntityInfo(entity);

                    MigrateTo(entity, ref info, _phantomDestination, ref lazy, blockSrc:false, blockDst:false);
                }
            }
        }
        else
        {
            // Dispose all disposables on any entity in this archetype
            if (HasDisposableComponents)
                DisposeAllDisposableComponents(ref lazy);

            // Clear all the chunks
            foreach (var chunk in _chunks)
                chunk.Clear();

            // Move some chunks to hot spares and then delete the rest
            foreach (var chunk in _chunks)
            {
                if (_spareChunks.Count < CHUNK_HOT_SPARES)
                    _spareChunks.Push(chunk);
                else
                    break;
            }
            _chunksWithSpace.Clear();
            _chunks.Clear();

            // Done! No entities left.
            EntityCount = 0;
        }

        Debug.Assert(EntityCount == 0);
    }

    /// <summary>
    /// Find a chunk with space and add the given entity to it.
    /// </summary>
    /// <param name="entity">Entity to add to a chunk</param>
    /// <param name="info">Info will be mutated to point to the new location</param>
    /// <param name="block"></param>
    /// <returns></returns>
    internal void AddEntity(EntityId entity, ref EntityInfo info, bool block)
    {
        // Wait for multithreaded access to this archetype
        if (block)
            Block();

        // Increase archetype entity count
        EntityCount++;

        // Trim chunks with space collection to remove items
        _chunksWithSpace.RemoveAll(static c => c.EntityCount == CHUNK_SIZE);

        // If there's one with space, use it
        if (_chunksWithSpace.Count > 0)
        {
            var chunk = _chunksWithSpace[0];
            chunk.AddEntity(entity, ref info);

            // If the chunk is now full, remove it from the "chunks with space" set
            if (chunk.EntityCount == CHUNK_SIZE)
                _chunksWithSpace.RemoveAt(0);

            return;
        }

        // No space in any chunks, create a new chunk
        var newChunk = AllocateChunk();
        _chunks.Add(newChunk);
        _chunksWithSpace.Add(newChunk);

        // The chunk obviously has space, so this cannot fail!
        newChunk.AddEntity(entity, ref info);
    }

    private Chunk AllocateChunk()
    {
        return _spareChunks.Count > 0 ? _spareChunks.Pop() : new Chunk(this, CHUNK_SIZE, _componentIndexLookup, _componentTypes, _componentIDs);
    }
    
    internal void RemoveEntity(EntityInfo info, ref LazyCommandBuffer lazy, bool block)
    {
        // Wait for multithreaded access to this archetype
        if (block)
            Block();

        // Run disposal for all IDisposableComponent components
        if (HasDisposableComponents)
            _disposer?.DisposeEntity(ref lazy, info);

        // Remove the entity from the chunk, component data is lost after this point
        info.Chunk.RemoveEntity(info);

        // Execute handler for when an entity is removed from a chunk
        HandleChunkEntityRemoved(info.Chunk);
    }

    internal void MigrateTo(EntityId entity, ref EntityInfo info, Archetype to, ref LazyCommandBuffer lazy, bool blockSrc, bool blockDst)
    {
        // Wait for multithreaded access to this archetype
        if (blockSrc)
            Block();
        if (blockDst)
            to.Block();

        //// Early exit if we're migrating to where we already are!
        //if (to == this)
        //    return info.GetRow(entity);
        Debug.Assert(to != this);

        // Handle disposable components which are being removed
        _disposer?.DisposeRemoved(ref lazy, info, to);

        // Inform entity it is becoming a phantom
        if (to.IsPhantom)
            _phantomNotifier?.Notify(entity, info);

        // Do the actual copying
        // We already blocked on src and dst archetypes just above.
        var chunk = info.Chunk;
        chunk.MigrateTo(entity, ref info, to, destBlock:false);

        // Execute handler for when an entity is removed from a chunk
        HandleChunkEntityRemoved(chunk);
    }

    private void HandleChunkEntityRemoved(Chunk chunk)
    {
        // Decrease archetype entity count
        EntityCount--;

        switch (chunk.EntityCount)
        {
            // If the chunk is empty remove it from this archetype entirely
            case 0:
            {
                _chunksWithSpace.Remove(chunk);
                _chunks.Remove(chunk);
                if (_spareChunks.Count < CHUNK_HOT_SPARES)
                    _spareChunks.Push(chunk);
                break;
            }

            // If the chunk was previously full and now isn't, add it to the set of chunks with space
            case CHUNK_SIZE - 1:
                _chunksWithSpace.Add(chunk);
                break;
        }
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Hash.GetHashCode();
    }

    internal IReadOnlyList<Chunk> Chunks => _chunks;

    //[MustDisposeResource]
    internal List<Chunk>.Enumerator GetChunkEnumerator()
    {
        return _chunks.GetEnumerator();
    }

    internal bool SetEquals(OrderedListSet<ComponentID> query)
    {
        return Components.SetEquals(query);
    }

    internal bool SetEquals<TV>(Dictionary<ComponentID, TV> query)
    {
        return Components.SetEquals(query);
    }

    /// <summary>
    /// Get an enumerable of all entities in this <see cref="Archetype"/>, in an arbitrary order.
    /// </summary>
    public ArchetypeEntityEnumerable Entities => new(this);

    /// <summary>
    /// Block on multithreaded access to this archetype to finish
    /// </summary>
    public void Block()
    {
        World.LockManager.Block(this);
    }

    /// <summary>
    /// Block on multithreaded access to the given components in this archetype to finish
    /// </summary>
    public void Block(ReadOnlySpan<ComponentID> ids)
    {
        World.LockManager.Block(this, ids);
    }

    /// <summary>
    /// Check if this archetype contains the given component
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public bool HasComponent<T>()
        where T : IComponent
    {
        return HasComponent(ComponentID<T>.ID);
    }

    /// <summary>
    /// Check if this archetype contains the given component
    /// </summary>
    /// <returns></returns>
    public bool HasComponent(ComponentID id)
    {
        var idx = id.Value;
        return idx > 0
            && idx < _componentIndexLookup.Length
            && _componentIndexLookup[idx] != -1;
    }

    #region sorting

    /// <summary>
    /// Sort this archetype. This is a structural change!
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TKeyMapper"></typeparam>
    internal void Sort<TKey, TKeyMapper>(TKeyMapper mapper, bool block)
        where TKey : unmanaged, IComparable<TKey>
        where TKeyMapper : IKeyMapper<TKey>
    {
        // Wait for multithreaded access
        if (block)
            Block();

        // No need to sort an empty archetype
        if (EntityCount == 0)
            return;

        // if there's only one chunk, sort it directly
        if (_chunks.Count == 1)
        {
            _chunks[0].Sort<TKey, TKeyMapper>(mapper, block:false);
            return;
        }
        
        // Allocate a slot for each chunk
        using var spans = new SortableSpans<TKey>(_chunks);

        // Fill and sort each chunk span
        spans.Sort(mapper);
        
        // Clear all chunks from this archetype, we'll recreate them after
        _chunks.Clear();
        _chunksWithSpace.Clear();

        // Do a K-Way merge over all chunk
        KWayChunkMerge(spans.Span);

        // Rebuild the "chunks with space" cache
        foreach (var chunk in _chunks)
            if (chunk.EntityCount != CHUNK_SIZE)
                _chunksWithSpace.Add(chunk);
        
        // Add empty chunks to the "hot spares" cache
        foreach (var sortableSpan in spans.Span)
        {
            if (_spareChunks.Count >= CHUNK_HOT_SPARES)
                break;

            if (sortableSpan.Chunk.EntityCount == 0)
                _spareChunks.Push(sortableSpan.Chunk);
        }
    }

    private void KWayChunkMerge<TKey>(Span<SortableSpan<TKey>> spans)
        where TKey : unmanaged, IComparable<TKey>
    {
        var filling = default(Chunk);

        while (true)
        {
            var bestSpan = -1;
            var bestIndex = -1;
            TKey? bestKey = null;

            // Find smallest head item across all chunks
            for (var i = 0; i < spans.Length; i++)
            {
                // Skip empty chunks
                ref var span = ref spans[i];
                if (span.Remaining == 0)
                    continue;

                // Get the smallest item from this span
                ref var item = ref span.Head;

                // Update tracker if it is the best item
                if (bestSpan == -1 || item.Key.CompareTo(bestKey!.Value) < 0)
                {
                    bestSpan = i;
                    bestIndex = item.OriginalIndex;
                    bestKey = item.Key;
                }
            }

            // Everything consumed
            if (bestSpan == -1)
                break;

            // todo: add check here - if the best item is the first in it's chunk check if the end item is small enough that we can transfer
            //       the entire chunk with no copying.
            
            // Need another output chunk
            if (filling == null || filling.EntityCount == CHUNK_SIZE)
            {
                filling = AllocateChunk();
                _chunks.Add(filling);
            }

            // Do the actual entity copy
            ref var source = ref spans[bestSpan];
            Copy(
                source.Chunk,
                bestIndex,
                filling
            );

            source.Consumed++;
        }

        void Copy(Chunk sourceChunk, int sourceIndex, Chunk destChunk)
        {
            // Add the entity to the dest chunk
            var entity = sourceChunk.Entities.Span[sourceIndex];
            ref var info = ref World.GetEntityInfo(entity);
            destChunk.AddEntity(entity, ref info);

            // Copy the components
            sourceChunk.CopyComponents(sourceIndex, destChunk, info.RowIndex);

            // Clear from source row
            sourceChunk.ClearComponents(sourceIndex);
        }
}

    /// <summary>
    /// A collection of sortable spans, one per chunk
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    private struct SortableSpans<TKey>
        : IDisposable
        where TKey : unmanaged, IComparable<TKey>
    {
        private readonly int _count;
        private SortableSpan<TKey>[] _spansArr;

        public Span<SortableSpan<TKey>> Span => _spansArr.AsSpan(0, _count);

        public SortableSpans(List<Chunk> chunks)
        {
            _count = chunks.Count;
            _spansArr = ArrayPool<SortableSpan<TKey>>.Shared.Rent(_count);

            for (var i = 0; i < chunks.Count; i++)
                _spansArr[i] = new SortableSpan<TKey>(chunks[i]);
        }

        public void Dispose()
        {
            if (_spansArr == null)
                throw new ObjectDisposedException("Already disposed");

            for (var i = 0; i < _count; i++)
                _spansArr[i].Dispose();

            ArrayPool<SortableSpan<TKey>>.Shared.Return(_spansArr, clearArray:true);
            _spansArr = null!;
        }

        public void Sort<TKeyMapper>(TKeyMapper mapper)
            where TKeyMapper : IKeyMapper<TKey>
        {
            foreach (ref var item in Span)
                item.Sort(mapper);
        }
    }

    /// <summary>
    /// A sortable span for a chunk
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    private struct SortableSpan<TKey>
        : IDisposable
        where TKey : unmanaged, IComparable<TKey>
    {
        public Chunk Chunk { get; }
        public Sortable<TKey>[] Array { get; }

        public Span<Sortable<TKey>> Span => Array.AsSpan(Consumed, Remaining);
        public ref Sortable<TKey> Head => ref Span[0];
        public ref Sortable<TKey> Tail => ref Span[^1];
        
        public int Consumed { get; set; }
        public int Remaining => Chunk.EntityCount - Consumed;

        public SortableSpan(Chunk chunk, Sortable<TKey>[] array)
        {
            Chunk = chunk;
            Array = array;
        }

        public SortableSpan(Chunk chunk)
        {
            Chunk = chunk;
            Array = ArrayPool<Sortable<TKey>>.Shared.Rent(chunk.EntityCount);
        }

        public void Dispose()
        {
            ArrayPool<Sortable<TKey>>.Shared.Return(Array);
        }

        public void Sort<TKeyMapper>(TKeyMapper mapper)
            where TKeyMapper : IKeyMapper<TKey>
        {
            Chunk.SortKeyBuffer(mapper, Span, block:false);
        }
    }
    #endregion
}