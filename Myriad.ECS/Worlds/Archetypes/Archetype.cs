using System.Diagnostics;
using Myriad.ECS.Collections;
using Myriad.ECS.Command;
using Myriad.ECS.Components;
using Myriad.ECS.IDs;
using Myriad.ECS.Worlds.Chunks;

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
    internal readonly BloomFilter32x384 ComponentsBloomFilter;

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
    /// The archetype that entities should be moved to when deleted.
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
    /// Indicates if any of the components in this Archetype im[lement <see cref="IEntityRelationComponent"/>
    /// </summary>
    public bool HasRelationComponents { get; }

    /// <summary>
    /// Indicates if any of the components in this Archetype im[lement <see cref="IDisposableComponent"/>
    /// </summary>
    public bool HasDisposableComponents { get; }

    /// <summary>
    /// Indicates if any of the components in this Archetype im[lement <see cref="IPhantomNotifierComponent"/>
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

    internal Row CreateEntity()
    {
        // Wait for multithreaded access to this archetype
        Block();

        // Allocate an entity in the world
        ref var info = ref World.AllocateEntity(out var entity);

        // Add it to this archetype, find a row to put components into
        return AddEntity(entity, ref info);
    }

    /// <summary>
    /// Delete every Entity in this archetype
    /// </summary>
    /// <param name="lazy"></param>
    /// <exception cref="NotImplementedException"></exception>
    internal void Clear(ref LazyCommandBuffer lazy)
    {
        // Wait for multithreaded access to this archetype
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

            // Migrate all entities in all chunks to the new archetype. Doing this does all of the bookeeping like chunk management and entity count.
            // This could be better, at the moment it just does the work on a per-entity basis, instead of doing it all in one batch.
            while (_chunks.Count > 0)
            {
                var chunk = _chunks[^1];

                while (chunk.EntityCount > 0)
                {
                    var entity = chunk.Entities.Span[^1].ID;
                    ref var info = ref World.GetEntityInfo(entity);

                    MigrateTo(entity, ref info, _phantomDestination, ref lazy);
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
    /// <returns></returns>
    internal Row AddEntity(EntityId entity, ref EntityInfo info)
    {
        // Wait for multithreaded access to this archetype
        Block();

        // Increase archetype entity count
        EntityCount++;

        // Trim chunks with space collection to remove items
        _chunksWithSpace.RemoveAll(static c => c.EntityCount == CHUNK_SIZE);

        // If there's one with space, use it
        if (_chunksWithSpace.Count > 0)
            return _chunksWithSpace[0].AddEntity(entity, ref info);

        // No space in any chunks, create a new chunk
        var newChunk = _spareChunks.Count > 0 ? _spareChunks.Pop() : new Chunk(this, CHUNK_SIZE, _componentIndexLookup, _componentTypes, _componentIDs);
        _chunks.Add(newChunk);
        _chunksWithSpace.Add(newChunk);

        // The chunk obviously has space, so this cannot fail!
        return newChunk.AddEntity(entity, ref info);
    }

    internal void RemoveEntity(EntityInfo info, ref LazyCommandBuffer lazy)
    {
        // Wait for multithreaded access to this archetype
        Block();

        // Run disposal for all IDisposableComponent components
        if (HasDisposableComponents)
            _disposer?.DisposeEntity(ref lazy, info);

        // Remove the entity from the chunk, component data is lost after this point
        info.Chunk.RemoveEntity(info);

        // Execute handler for when an entity is removed from a chunk
        HandleChunkEntityRemoved(info.Chunk);
    }

    internal Row MigrateTo(EntityId entity, ref EntityInfo info, Archetype to, ref LazyCommandBuffer lazy)
    {
        // Wait for multithreaded access to this archetype
        Block();

        //// Early exit if we're migrating to where we already are!
        //if (to == this)
        //    return info.GetRow(entity);
        Debug.Assert(to != this);

        // Handle disposable components which are being removed
        _disposer?.DisposeRemoved(ref lazy, info, to.Components);

        // Inform entity it is becoming a phantom
        if (to.IsPhantom)
            _phantomNotifier?.Notify(entity, info);

        // Do the actual copying
        var chunk = info.Chunk;
        var row = chunk.MigrateTo(entity, ref info, to);

        // Execute handler for when an entity is removed from a chunk
        HandleChunkEntityRemoved(chunk);

        return row;
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
}