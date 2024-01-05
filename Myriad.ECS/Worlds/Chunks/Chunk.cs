using Myriad.ECS.Worlds.Archetypes;
using System.Runtime.CompilerServices;
using Myriad.ECS.IDs;

namespace Myriad.ECS.Worlds.Chunks;

public sealed class Chunk
{
    public Archetype Archetype { get; }

    private readonly World _world;
    private readonly int _chunkIndex;

    // Map from component ID (index) to index in chunk
    private readonly int[] _componentIndexLookup;

    /// <summary>
    /// Map from index to component ID
    /// </summary>
    private readonly IReadOnlyList<ComponentID> _componentIdLookup;
    
    private int _entityCount;

    private readonly Entity[] _entities;
    private readonly Array[] _components;

    public int EntityCount => _entityCount;

    internal Chunk(World world, Archetype archetype, int size, int[] componentIndexLookup, IReadOnlyList<Type> componentTypes, IReadOnlyList<ComponentID> ids, int componentCount, int chunkIndex)
    {
        _world = world;
        Archetype = archetype;
        _componentIndexLookup = componentIndexLookup;
        _chunkIndex = chunkIndex;
        _entities = new Entity[size];
        _componentIdLookup = ids;

        _components = new Array[componentCount];
        for (var i = 0; i < _components.Length; i++)
            _components[i] = ArrayFactory.Create(componentTypes[i], size);
    }

    #region get component
    public ref T GetMutable<T>(Entity entity, int rowIndex)
        where T : IComponent
    {
        if (_entities[rowIndex] != entity)
            throw new InvalidOperationException("Mismatched entities in chunk");

        var component = ComponentID<T>.ID;
        var componentArray = _components[_componentIndexLookup[component.Value]];
        var typedArray = Unsafe.As<T[]>(componentArray);
        return ref typedArray[rowIndex];
    }

    public ref T GetMutable<T>(Entity entity)
        where T : IComponent
    {
        var index = _world.GetEntityInfo(entity).RowIndex;
        return ref GetMutable<T>(entity, index);
    }
    #endregion

    internal Row? TryAddEntity(Entity entity, ref EntityInfo info)
    {
        if (_entityCount == _entities.Length)
            return null;

        // Use the next free slot
        var index = _entityCount++;

        // Occupy this row
        _entities[index] = entity;

        // Update global entity info to refer to this location
        info.RowIndex = index;
        info.Chunk = this;

        return new Row(entity, index, this);
    }

    internal Row GetRow(Entity entity, EntityInfo info)
    {
        if (!ReferenceEquals(info.Chunk, this))
            throw new ArgumentException("entity is not in this chunk", nameof(entity));

        return new Row(entity, info.RowIndex, this);
    }

    internal bool RemoveEntity(Entity entity, EntityInfo info)
    {
        var index = info.RowIndex;

        // No work to do if there are no entites
        _entityCount -= 1;
        if (_entityCount == 0)
        {
            _entities[index] = default;
            return true;
        }

        // Swap the top entity into place
        var lastEntity = _entities[_entityCount];
        var lastEntityIndex = _entityCount;
        ref var lastInfo = ref _world.GetEntityInfo(lastEntity);
        _entities[index] = lastEntity;
        _entities[lastEntityIndex] = default;
        lastInfo.RowIndex = index;

        // Copy top entity components into place
        foreach (var component in _components)
        {
            Array.Copy(component, lastEntityIndex, component, index, 1);
            Array.Clear(component, lastEntityIndex, 1);
        }

        return true;
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
        RemoveEntity(entity, oldInfo);

        return destRow;
    }
}