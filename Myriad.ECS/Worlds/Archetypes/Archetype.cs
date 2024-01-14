using System.Collections;
using System.Collections.Frozen;
using Myriad.ECS.IDs;
using Myriad.ECS.Registry;
using Myriad.ECS.Worlds.Chunks;

namespace Myriad.ECS.Worlds.Archetypes;

public sealed partial class Archetype
{
    private const int CHUNK_SIZE = 1024;

    public World World { get; }

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
    /// Get an enumerator over all chunks
    /// </summary>
    internal List<Chunk>.Enumerator Chunks => _chunks.GetEnumerator();

    /// <summary>
    /// A list of chunks which might have space to put an entity in
    /// </summary>
    private readonly List<Chunk> _chunksWithSpace = [ ];

    private readonly ComponentID[] _componentIDs;
    private readonly Type[] _componentTypes;

    public Archetype(World world, FrozenSet<ComponentID> components)
    {
        World = world;
        Components = components;

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
            _componentTypes[idx] = ComponentRegistry.Get(component);
            _componentIndexLookup[component.Value] = idx;
            _componentIDs[idx] = component;

            idx++;
        }
    }

    /// <summary>
    /// Find a chunk with space and add the given entity to it.
    /// </summary>
    /// <param name="entity">Entity to add to a chunk</param>
    /// <param name="info">Info will be mutated to point to the new location</param>
    /// <returns></returns>
    internal Row AddEntity(Entity entity, ref EntityInfo info)
    {
        // Iterate over all candidate chunks, searching for one with space for an entity.
        // Remove any from the list that turn out not to have space
        for (var i = _chunksWithSpace.Count - 1; i >= 0; i--)
        {
            var chunk = _chunksWithSpace[i];

            // Try to add this entity to the chunk
            var row = _chunksWithSpace[i].TryAddEntity(entity, ref info);

            // Remove this chunk if it is full
            if (row == null || chunk.EntityCount == CHUNK_SIZE)
                _chunksWithSpace.RemoveAt(i);

            // Return the row if one was assigned
            if (row != null)
                return row.Value;
        }

        // No space in any chunks, create a new chunk
        var newChunk = new Chunk(this, CHUNK_SIZE, _componentIndexLookup, _componentTypes, _componentIDs);
        _chunks.Add(newChunk);
        _chunksWithSpace.Add(newChunk);

        // The chunk obviously has space, so this cannot fail!
        return newChunk.TryAddEntity(entity, ref info)!.Value;
    }

    internal void RemoveEntity(EntityInfo info)
    {
        var chunk = info.Chunk;

        if (!chunk.RemoveEntity(info))
            throw new NotImplementedException("entity was not in expected chunk");

        // If the chunk was previously full and now isn't, add it to the set of chunks with space
        if (chunk.EntityCount == CHUNK_SIZE - 1)
            _chunksWithSpace.Add(chunk);
    }

    internal Row MigrateTo(Entity entity, ref EntityInfo info, Archetype to)
    {
        var chunk = info.Chunk;
        var row = chunk.MigrateTo(entity, ref info, to);

        // If the chunk was previously full and now isn't, add it to the set of chunks with space
        if (chunk.EntityCount == CHUNK_SIZE - 1)
            _chunksWithSpace.Add(chunk);

        return row;
    }

    public List<Chunk>.Enumerator GetEnumerator()
    {
        return _chunks.GetEnumerator();
    }
}