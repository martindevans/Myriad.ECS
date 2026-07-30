using Myriad.ECS.Allocations;
using Myriad.ECS.Collections;
using Myriad.ECS.IDs;
using Myriad.ECS.Worlds.Archetypes;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Myriad.ECS.Worlds.Chunks;

internal sealed partial class Chunk
{
    /// <summary>
    /// The archetype which contains this chunk
    /// </summary>
    public Archetype Archetype { get; }

    // Map from component ID (index) to index in chunk
    private readonly int[] _componentIndexLookup;

    /// <summary>
    /// Map from index to component ID
    /// </summary>
    private readonly ReadOnlyMemory<ComponentID> _componentIdLookup;

    private readonly Entity[] _entities;
    private readonly EntityId[] _entityIds;
    private readonly Array[] _components;

    private uint[]? _bits;

    /// <summary>
    /// Get the number of entities currently in this chunk
    /// </summary>
    public int EntityCount { get; private set; }

    /// <summary>
    /// Get all of the entities in this chunk
    /// </summary>
    public ReadOnlyMemory<Entity> Entities => _entities.AsMemory(0, EntityCount);

    /// <summary>
    /// Get all of the entities in this chunk
    /// </summary>
    public ReadOnlyMemory<EntityId> EntityIds => _entityIds.AsMemory(0, EntityCount);

    private static long _nextId;
    /// <summary>
    /// Globally Unique ID for this chunk
    /// </summary>
    public long ChunkId { get; }

    internal Chunk(Archetype archetype, int size, int[] componentIndexLookup, ReadOnlySpan<Type> componentTypes, ReadOnlyMemory<ComponentID> ids)
    {
        ChunkId = Interlocked.Increment(ref _nextId);

        Archetype = archetype;
        _componentIndexLookup = componentIndexLookup;
        _entities = new Entity[size];
        _entityIds = new EntityId[size];
        _componentIdLookup = ids;

        // Allocate component arrays. Each chunk is one larger than it needs to be, this slot
        // is used as temporary storage when moving entities.
        _components = new Array[componentTypes.Length];
        for (var i = 0; i < _components.Length; i++)
            _components[i] = ArrayFactory.Create(componentTypes[i], size + 1);
    }

    #region get component
    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    //public ref T GetRef<T>(Entity entity)
    //    where T : IComponent
    //{
    //    var index = Archetype.World.GetEntityInfo(entity).RowIndex;
    //    return ref GetRef<T>(entity, index);
    //}

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal ref T GetRef<T>(EntityId entityId, int rowIndex)
        where T : IComponent
    {
        Debug.Assert(_entities[rowIndex].ID == entityId, "Mismatched entities in chunk");
        Debug.Assert(_entityIds[rowIndex] == entityId, "Mismatched entities in chunk");
        return ref GetRef<T>(rowIndex);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public RefT<T> GetRefT<T>(EntityId entityId, int rowIndex)
        where T : IComponent
    {
        Debug.Assert(_entities[rowIndex].ID == entityId, "Mismatched entities in chunk");
        Debug.Assert(_entityIds[rowIndex] == entityId, "Mismatched entities in chunk");

#if NET6_0_OR_GREATER
        return new RefT<T>(ref GetRef<T>(rowIndex));
#else
        return new RefT<T>(GetComponentArray<T>(), rowIndex);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public RefT<T> GetRefT<T>(int rowIndex, ComponentID id)
        where T : IComponent
    {
#if NET6_0_OR_GREATER
        return new RefT<T>(ref GetRef<T>(rowIndex, id));
#else
        return new RefT<T>(GetComponentArray<T>(id), rowIndex);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal ref T GetRef<T>(int rowIndex)
        where T : IComponent
    {
        return ref GetRef<T>(rowIndex, ComponentID<T>.ID);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal ref T GetRef<T>(int rowIndex, ComponentID id)
        where T : IComponent
    {
        return ref GetSpan<T>(id)[rowIndex];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal Span<T> GetSpan<T>()
        where T : IComponent
    {
        return GetSpan<T>(ComponentID<T>.ID);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal Span<T> GetSpan<T>(ComponentID id)
        where T : IComponent
    {
        return GetComponentArray<T>(id).AsSpan(0, EntityCount);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal T[] GetComponentArray<T>()
        where T : IComponent
    {
        return GetComponentArray<T>(ComponentID<T>.ID);
    }

    /// <summary>
    /// Get the component array, providing the component ID if it is known.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="id"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal T[] GetComponentArray<T>(ComponentID id)
        where T : IComponent
    {
        return (GetComponentArray(id) as T[])!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal Array GetComponentArray(ComponentID id)
    {
        return _components[_componentIndexLookup[id.Value]];
    }
    #endregion

    #region bit flags
    /// <summary>
    /// Get the value of the given chunk flag bit
    /// </summary>
    /// <typeparam name="TFlag"></typeparam>
    /// <returns></returns>
    public bool GetFlag<TFlag>()
        where TFlag : IChunkBitFlag
    {
        var index = ChunkBitFlagID<TFlag>.ID.Value;

        // If the word is out of range the bit can't possibly be set
        var wordIndex = index >> 5;
        if (_bits == null || wordIndex > _bits.Length)
            return false;

        // Get the bit
        var bitOffset = index & 31;
        return (_bits[wordIndex] & (1u << bitOffset)) != 0;
    }

    /// <summary>
    /// Set the value of the given chunk flag bit
    /// </summary>
    /// <typeparam name="TFlag"></typeparam>
    /// <param name="value"></param>
    public void SetFlag<TFlag>(bool value)
        where TFlag : IChunkBitFlag
    {
        var index = ChunkBitFlagID<TFlag>.ID.Value;

        // If index is out of range grow the array now
        var wordIndex = index >> 5;
        if (_bits == null || _bits.Length < wordIndex)
        {
            var bits2 = new uint[wordIndex + 1];
            _bits?.AsSpan().CopyTo(bits2.AsSpan());
            _bits = bits2;
        }

        // Set/clear the bit in the array
        var bitOffset = index & 31;
        var mask = 1u << bitOffset;
        if (value)
            _bits[wordIndex] |= mask;
        else
            _bits[wordIndex] &= ~mask;
    }
    #endregion

    #region add/remove entity
    // Note that these must be called only from Archetype! The Archetype needs to do some bookeeping on create/destroy.

    internal void Clear()
    {
        Debug.Assert(!Archetype.HasPhantomComponents);

        // Clear out the components. This prevents chunks holding 
        // onto references to dead managed components, and keeping them in memory.
        foreach (var component in _components)
            Array.Clear(component, 0, component.Length);

        // Clean up all the IDs so they're default instead of some invalid value. This is
        // necessary in case anything is holding on to a reference to the chunk.
        Array.Clear(_entities, 0, _entities.Length);
        Array.Clear(_entityIds, 0, _entityIds.Length);

        EntityCount = 0;
    }

    internal void AddEntity(EntityId entity, ref EntityInfo info)
    {
        // It is safe to only debug assert here. It should never happen if Myriad is working
        // correctly. If it does somehow go wrong you'll get an index out of range exception
        // below so it still fails in a sensible way.
        Debug.Assert(EntityCount < _entities.Length, "Cannot add entity to full chunk");

        // Use the next free slot
        var index = EntityCount++;

        // Occupy this row
        _entities[index] = entity.ToEntity(Archetype.World);
        _entityIds[index] = entity;

        // Update global entity info to refer to this location
        info.RowIndex = index;
        info.Chunk = this;
    }

    internal void RemoveEntity(EntityInfo info)
    {
        var index = info.RowIndex;

        // Clear out the components. This prevents chunks holding 
        // onto references to dead managed components, and keeping them in memory.
        foreach (var component in _components)
            Array.Clear(component, index, 1);

        // No work to do if there are no other entities
        EntityCount -= 1;
        if (EntityCount == 0)
        {
            _entities[index] = default;
            _entityIds[index] = default;
            return;
        }

        // If we did not just delete the top entity into place then swap the top
        // entity down into this slot to keep the chunk continuous.
        if (index != EntityCount)
        {
            var lastEntity = _entities[EntityCount];
            var lastEntityIndex = EntityCount;
            ref var lastInfo = ref Archetype.World.GetEntityInfo(lastEntity.ID);
            _entities[index] = lastEntity;
            _entityIds[index] = lastEntity.ID;
            _entities[lastEntityIndex] = default;
            _entityIds[lastEntityIndex] = default;
            lastInfo.RowIndex = index;

            // Copy top entity components into place
            foreach (var component in _components)
            {
                Array.Copy(component, lastEntityIndex, component, index, 1);

                // Clear out the components we just moved. This prevents chunks holding 
                // onto references to dead managed components, and keeping them in memory.
                Array.Clear(component, lastEntityIndex, 1);
            }
        }
    }

    internal void MigrateTo(EntityId entity, ref EntityInfo info, Archetype to, bool destBlock)
    {
        // Copy current entity info so we can use it later
        var oldInfo = info;

        // Move the entity to the new archetype
        to.AddEntity(entity, ref info, destBlock);
        var destChunk = info.Chunk;

        // Copy across everything that exists in the destination archetype
        var componentIdLookupSpan = _componentIdLookup.Span;
        for (var i = 0; i < _components.Length; i++)
        {
            var id = componentIdLookupSpan[i].Value;

            // Check if the component is not in the destination, in which case just don't copy it
            if (id >= destChunk._componentIndexLookup.Length || destChunk._componentIndexLookup[id] == -1)
                continue;

            // Get the two arrays
            var srcArr = _components[i];
            var destArr = destChunk._components[destChunk._componentIndexLookup[id]];

            // Copy!
            Array.Copy(srcArr, oldInfo.RowIndex, destArr, info.RowIndex, 1);
        }

        // Remove the entity from this chunk (using the old saved info)
        RemoveEntity(oldInfo);
    }
    #endregion

    internal Entity[] GetEntityArray()
    {
        return _entities;
    }

    internal EntityId[] GetEntityIdArray()
    {
        return _entityIds;
    }

    #region sort

    private void CopyComponents(int indexFrom, int indexTo)
    {
        // Copy top entity components into place
        foreach (var component in _components)
            Array.Copy(component, indexFrom, component, indexTo, 1);
    }

    private void SetEntityAtIndex(int index, Entity entity)
    {
        // Overwrite the entity
        _entities[index] = entity;
        _entityIds[index] = entity;

        // Update the world
        ref var info = ref Archetype.World.GetEntityInfo(entity);
        info.RowIndex = index;
    }

    private void ClearComponents(int index)
    {
        foreach (var component in _components)
            Array.Clear(component, index, 1);
    }

    /// <summary>
    /// Sort this chunk by a key (derived from components)
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TKeyMapper"></typeparam>
    internal void Sort<TKey, TKeyMapper>(TKeyMapper mapper, bool block = true)
        where TKey : unmanaged, IComparable<TKey>
        where TKeyMapper : IKeyMapper<TKey>
    {
        Sort(
            mapper,
            stackalloc Sortable<TKey>[EntityCount],
            block
        );
    }
    
    /// <summary>
    /// Sort this chunk by a key (derived from components). Using caller provided temporary memory.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TKeyMapper"></typeparam>
    /// <param name="mapper"></param>
    /// <param name="reorder">Reorder buffer, must be exactly chunk size</param>
    /// <param name="block"></param>
    /// <exception cref="ArgumentException"></exception>
    internal void Sort<TKey, TKeyMapper>(TKeyMapper mapper, Span<Sortable<TKey>> reorder, bool block = true)
        where TKey : unmanaged, IComparable<TKey>
        where TKeyMapper : IKeyMapper<TKey>
    {
        if (reorder.Length != EntityCount)
            throw new ArgumentException("Reorder buffer is incorrect length (must be chunksize)");
        
        // Wait on multithreaded access to the archetype
        if (block)
            Archetype.Block();
        
        // Build span of entities with key
        for (var i = 0; i < EntityCount; i++)
            reorder[i] = new Sortable<TKey>(i, mapper.MapKey(this, i));

        // Sort the span based on the key
        reorder.Sort();

        // Now apply the reorder buffer
        new EntityMover(this).ApplyReorderInPlace(reorder);
        
        // Clear the temporary slot, one beyond the end of the valid slice of the array
        ClearComponents(_entities.Length);
    }

    internal interface IKeyMapper<out TKey>
    {
        public TKey MapKey(Chunk chunk, int index);
    }

    internal readonly struct Sortable<TKey>
        : ReorderBuffer.IDataIndex, IComparable<Sortable<TKey>>
        where TKey : unmanaged, IComparable<TKey>
    {
        public int OriginalIndex { get; }
        public readonly TKey Key;

        public Sortable(int originalIndex, TKey key)
        {
            OriginalIndex = originalIndex;
            Key = key;
        }

        public int CompareTo(Sortable<TKey> other)
        {
            // Compare the keys, fall back to comparing indices if they're the same.
            // This makes the unstable span sort stable!
            var cmp = Key.CompareTo(other.Key);
            return cmp != 0
                ? cmp
                : OriginalIndex.CompareTo(other.OriginalIndex);
        }
    }

    private struct EntityMover
        : ReorderBuffer.IDataMove
    {
        private readonly Chunk _chunk;
        
        private Entity _tempEntity;
        private readonly int _tempIdx;

        public EntityMover(Chunk chunk)
        {
            _chunk = chunk;
            _tempIdx = _chunk._entities.Length;
        }

        public void Move(int indexFrom, int indexTo)
        {
            _chunk.CopyComponents(indexFrom, indexTo);
            _chunk.SetEntityAtIndex(indexTo, _chunk._entities[indexFrom]);
        }

        public void SaveToTemporary(int indexFrom)
        {
            // Save the entity
            _tempEntity = _chunk._entities[indexFrom];
            
            // Copy components to the very last slot of the array
            _chunk.CopyComponents(indexFrom, _chunk._entities.Length);
        }

        public void RestoreFromTemporary(int indexTo)
        {
            // Copy components from the very last slot
            _chunk.CopyComponents(_tempIdx, indexTo);
            
            // Restore the entity
            _chunk.SetEntityAtIndex(indexTo, _tempEntity);
        }
    }

    #endregion
}