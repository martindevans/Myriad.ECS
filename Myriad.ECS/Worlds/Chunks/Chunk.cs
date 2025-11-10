using System.Diagnostics;
using System.Runtime.CompilerServices;
using Myriad.ECS.Allocations;
using Myriad.ECS.Collections;
using Myriad.ECS.IDs;
using Myriad.ECS.Worlds.Archetypes;

namespace Myriad.ECS.Worlds.Chunks;

internal sealed class Chunk
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
    private readonly Array[] _components;

    /// <summary>
    /// Get the number of entities currently in this chunk
    /// </summary>
    public int EntityCount { get; private set; }

    /// <summary>
    /// Get all of the entities in this chunk
    /// </summary>
    public ReadOnlyMemory<Entity> Entities => _entities.AsMemory(0, EntityCount);

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
        _componentIdLookup = ids;

        _components = new Array[componentTypes.Length];
        for (var i = 0; i < _components.Length; i++)
            _components[i] = ArrayFactory.Create(componentTypes[i], size);
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
        return ref GetRef<T>(rowIndex);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public RefT<T> GetRefT<T>(EntityId entityId, int rowIndex)
        where T : IComponent
    {
        Debug.Assert(_entities[rowIndex].ID == entityId, "Mismatched entities in chunk");

#if NET6_0_OR_GREATER
        return new RefT<T>(ref GetRef<T>(rowIndex));
#else
        return new RefT<T>(GetComponentArray<T>(), rowIndex);
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

    #region add/remove entity
    // Note that these must be called only from Archetype! The Archetype needs to do some bookeeping on create/destroy.

    internal void Clear()
    {
        Debug.Assert(!Archetype.HasPhantomComponents);

        // Clear out the components. This prevents chunks holding 
        // onto references to dead managed components, and keeping them in memory.
        foreach (var component in _components)
            Array.Clear(component, 0, component.Length);

        // Not strictly necessary, clean up all the IDs so they're default instead of some invalid value.
        Array.Clear(_entities, 0, _entities.Length);

        EntityCount = 0;
    }

    internal Row AddEntity(EntityId entity, ref EntityInfo info)
    {
        // It is safe to only debug assert here. It should never happen if Myriad is working
        // correctly. If it does somehow go wrong you'll get an index out of range exception
        // below so it still fails in a sensible way.
        Debug.Assert(EntityCount < _entities.Length, "Cannot add entity to full chunk");

        // Use the next free slot
        var index = EntityCount++;

        // Occupy this row
        _entities[index] = entity.ToEntity(Archetype.World);

        // Update global entity info to refer to this location
        info.RowIndex = index;
        info.Chunk = this;

        return new Row(entity, index, this);
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
            _entities[lastEntityIndex] = default;
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

    internal Row MigrateTo(EntityId entity, ref EntityInfo info, Archetype to)
    {
        // Copy current entity info so we can use it later
        var oldInfo = info;

        // Get a reference to the row currently storing this entity
        var srcRow = info.GetRow(entity);

        // Move the entity to the new archetype
        var destRow = to.AddEntity(entity, ref info);
        var destChunk = destRow.Chunk;

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
            Array.Copy(srcArr, srcRow.RowIndex, destArr, destRow.RowIndex, 1);
        }

        // Remove the entity from this chunk (using the old saved info)
        RemoveEntity(oldInfo);

        return destRow;
    }
    #endregion

    internal Entity[] GetEntityArray()
    {
        return _entities;
    }
}