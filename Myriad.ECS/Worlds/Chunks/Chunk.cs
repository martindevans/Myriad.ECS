using System.Runtime.CompilerServices;
using Myriad.ECS.Allocations;
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
    private readonly IReadOnlyList<ComponentID> _componentIdLookup;

    private readonly Entity[] _entities;
    private readonly Array[] _components;

    /// <summary>
    /// Get the number of entities currently in this chunk
    /// </summary>
    public int EntityCount { get; private set; }

    /// <summary>
    /// Get all of the entities in this chunk
    /// </summary>
    public ReadOnlySpan<Entity> Entities => _entities.AsSpan(0, EntityCount);

    internal Chunk(Archetype archetype, int size, int[] componentIndexLookup, IReadOnlyList<Type> componentTypes, IReadOnlyList<ComponentID> ids)
    {
        Archetype = archetype;
        _componentIndexLookup = componentIndexLookup;
        _entities = new Entity[size];
        _componentIdLookup = ids;

        _components = new Array[componentTypes.Count];
        for (var i = 0; i < _components.Length; i++)
            _components[i] = ArrayFactory.Create(componentTypes[i], size);
    }

    #region get component
    public ref T GetRef<T>(Entity entity)
        where T : IComponent
    {
        var index = Archetype.World.GetEntityInfo(entity).RowIndex;
        return ref GetRef<T>(entity, index);
    }

    internal ref T GetRef<T>(Entity entity, int rowIndex)
        where T : IComponent
    {
        if (_entities[rowIndex] != entity)
            throw new InvalidOperationException("Mismatched entities in chunk");
        return ref GetSpan<T>()[rowIndex];
    }

    internal Span<T> GetSpan<T>()
        where T : IComponent
    {
        return GetSpan<T>(ComponentID<T>.ID);
    }

    internal Span<T> GetSpan<T>(ComponentID id)
        where T : IComponent
    {
        var componentArray = _components[_componentIndexLookup[id.Value]];
        var typedArray = Unsafe.As<T[]>(componentArray);
        return typedArray.AsSpan(0, EntityCount);
    }
    #endregion

    internal Row GetRow(Entity entity, EntityInfo info)
    {
        if (!ReferenceEquals(info.Chunk, this))
            throw new ArgumentException("entity is not in this chunk", nameof(entity));

        return new Row(entity, info.RowIndex, this);
    }

    #region add/remove entity
    internal Row AddEntity(Entity entity, ref EntityInfo info)
    {
        if (EntityCount == _entities.Length)
            throw new InvalidOperationException("Cannot add entity to full chunk");

        // Use the next free slot
        var index = EntityCount++;

        // Occupy this row
        _entities[index] = entity;

        // Update global entity info to refer to this location
        info.RowIndex = index;
        info.Chunk = this;

        return new Row(entity, index, this);
    }

    internal void RemoveEntity(EntityInfo info)
    {
        var index = info.RowIndex;

        // No work to do if there are no other entites
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
            ref var lastInfo = ref Archetype.World.GetEntityInfo(lastEntity);
            _entities[index] = lastEntity;
            _entities[lastEntityIndex] = default;
            lastInfo.RowIndex = index;

            // Copy top entity components into place
            foreach (var component in _components)
            {
                Array.Copy(component, lastEntityIndex, component, index, 1);
                Array.Clear(component, lastEntityIndex, 1);
            }
        }
    }

    internal Row MigrateTo(Entity entity, ref EntityInfo info, Archetype to)
    {
        // Copy current entity info so we can use it later
        var oldInfo = info;

        // Get a reference to the row currently storing this entity
        var srcRow = GetRow(entity, info);

        // Move the entity to the new archetype
        var destRow = to.AddEntity(entity, ref info);
        var destChunk = destRow.Chunk;

        // Copy across everything that exists in the destination archetype
        for (var i = 0; i < _components.Length; i++)
        {
            var id = _componentIdLookup[i].Value;

            // Check if the component is out of range (in which case it's not in the dest)
            if (id >= destChunk._componentIndexLookup.Length)
                continue;

            // Check if it's explicitly -1 (in which case it's not in the dest)
            if (destChunk._componentIndexLookup[id] == -1)
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
}