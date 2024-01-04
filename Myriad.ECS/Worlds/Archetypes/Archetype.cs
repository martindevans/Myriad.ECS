using System.Collections.Frozen;
using Myriad.ECS.IDs;
using Myriad.ECS.Registry;
using Myriad.ECS.Worlds.Chunks;

namespace Myriad.ECS.Worlds.Archetypes;

public sealed class Archetype
{
    private const int CHUNK_SIZE = 1024;

    private readonly World _world;
    public FrozenSet<ComponentID> Components { get; }
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

    private readonly Type[] _componentTypes;

    public Archetype(World world, FrozenSet<ComponentID> components)
    {
        _world = world;
        Components = components;

        // Calculate archetype hash and also keep track of the max component ID ever seen
        var maxComponentId = int.MinValue;
        _componentTypes = new Type[components.Count];
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
            _componentTypes[idx] = ComponentRegistry.Get(component);
            _componentIndexLookup[component.Value] = idx++;
        }
    }

    internal Row AddEntity(Entity entity)
    {
        // Iterate over all candidate chunks, searching for one with space for an entity.
        // Remove any from the list that turn out not to have space
        for (var i = _chunksWithSpace.Count - 1; i >= 0; i--)
        {
            var row = _chunksWithSpace[i].TryAddEntity(entity);
            if (row != null)
                return row.Value;

            _chunksWithSpace.RemoveAt(i);
        }

        // Create a new chunk
        var chunk = new Chunk(CHUNK_SIZE, _componentIndexLookup, _componentTypes, Components.Count, _chunks.Count);
        _chunks.Add(chunk);
        _chunksWithSpace.Add(chunk);

        return chunk.TryAddEntity(entity)!.Value;
    }

    internal void RemoveEntity(Entity entity, int chunkIndex)
    {
        if (!_chunks[chunkIndex].RemoveEntity(entity))
            throw new NotImplementedException("entity was not in expected chunk");
    }

    internal Chunk GetChunk(int chunkIndex)
    {
        return _chunks[chunkIndex];
    }

    internal Row GetRow(Entity entity, int chunkIndex)
    {
        return _chunks[chunkIndex].GetRow(entity);
    }
}