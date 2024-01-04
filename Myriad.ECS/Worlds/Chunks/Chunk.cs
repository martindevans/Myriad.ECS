using Myriad.ECS.Worlds.Archetypes;
using System.Runtime.CompilerServices;
using Myriad.ECS.IDs;

namespace Myriad.ECS.Worlds.Chunks;

public sealed class Chunk
{
    private readonly int _chunkIndex;

    // Map from component ID (index) to index in chunk
    private readonly int[] _componentIndexLookup;
    
    private int _entityCount;

    private readonly Dictionary<Entity, int> _indexLookup = new();
    private readonly Entity[] _entities;
    private readonly Array[] _components;

    internal Chunk(int size, int[] componentIndexLookup, IReadOnlyList<Type> componentTypes, int componentCount, int chunkIndex)
    {
        _componentIndexLookup = componentIndexLookup;
        _chunkIndex = chunkIndex;
        _entities = new Entity[size];

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

    public ref readonly T GetImmutable<T>(Entity entity, int chunkIndex)
        where T : IComponent
    {
        return ref GetMutable<T>(entity, chunkIndex);
    }

    public ref T GetMutable<T>(Entity entity)
        where T : IComponent
    {
        var index = _indexLookup[entity];
        return ref GetMutable<T>(entity, index);
    }

    public ref readonly T GetImmutable<T>(Entity entity)
        where T : IComponent
    {
        var index = _indexLookup[entity];
        return ref GetImmutable<T>(entity, index);
    }
    #endregion

    internal Row? TryAddEntity(Entity entity)
    {
        if (_entityCount == _entities.Length)
            return null;

        // Use the next free slot
        var index = _entityCount++;

        // Occupy this row
        _entities[index] = entity;
        _indexLookup.Add(entity, index);
        return new Row(entity, index, this, _chunkIndex);
    }

    internal Row GetRow(Entity entity)
    {
        if (!_indexLookup.TryGetValue(entity, out var index))
            throw new ArgumentException("entity is not in this chunk", nameof(entity));

        return new Row(entity, index, this, _chunkIndex);
    }

    public bool RemoveEntity(Entity entity)
    {
        if (!_indexLookup.Remove(entity, out var index))
            return false;

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
        _entities[index] = lastEntity;
        _entities[lastEntityIndex] = default;
        _indexLookup[lastEntity] = index;

        // Copy components into place
        foreach (var component in _components)
        {
            Array.Copy(component, lastEntityIndex, component, index, 1);
            Array.Clear(component, lastEntityIndex, 1);
        }

        return true;
    }
}