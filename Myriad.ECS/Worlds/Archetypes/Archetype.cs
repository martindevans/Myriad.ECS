using System.Collections.Frozen;
using Myriad.ECS.IDs;
using Myriad.ECS.Registry;
using Myriad.ECS.Worlds.Chunks;

namespace Myriad.ECS.Worlds.Archetypes;

public sealed partial class Archetype
{
    /// <summary>
    /// Number of entities in a single chunk
    /// </summary>
    private const int CHUNK_SIZE = 1024;

    /// <summary>
    /// How many empty chunks to keep as spares
    /// </summary>
    private const int CHUNK_HOT_SPARES = 4;

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
    /// A list of chunks which might have space to put an entity in
    /// </summary>
    private readonly List<Chunk> _chunksWithSpace = [ ];

    /// <summary>
    /// A list of empty chunks that have been removed from this archetype
    /// </summary>
    private readonly Stack<Chunk> _spareChunks = new(CHUNK_HOT_SPARES);

    private readonly ComponentID[] _componentIDs;
    private readonly Type[] _componentTypes;

    public int EntityCount { get; private set; }

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
        var newChunk = _spareChunks.Count > 0 ? _spareChunks.Pop() : new Chunk(this, CHUNK_SIZE, _componentIndexLookup, _componentTypes, _componentIDs);
        _chunks.Add(newChunk);
        _chunksWithSpace.Add(newChunk);

        // Increase archetype entity count
        EntityCount++;

        // The chunk obviously has space, so this cannot fail!
        return newChunk.TryAddEntity(entity, ref info)!.Value;
    }

    internal void RemoveEntity(EntityInfo info)
    {
        var chunk = info.Chunk;

        if (!chunk.RemoveEntity(info))
            throw new NotImplementedException("entity was not in expected chunk");

        // Decrease archetype entity count
        EntityCount--;

        // Execute handler for when an entity is removed from a chunk
        HandleChunkEntityRemoved(chunk);
    }

    internal Row MigrateTo(Entity entity, ref EntityInfo info, Archetype to)
    {
        var chunk = info.Chunk;
        var row = chunk.MigrateTo(entity, ref info, to);

        // Decrease archetype entity count
        EntityCount--;

        // Execute handler for when an entity is removed from a chunk
        HandleChunkEntityRemoved(chunk);

        return row;
    }

    private void HandleChunkEntityRemoved(Chunk chunk)
    {
        switch (chunk.EntityCount)
        {
            // If the chunk is empty remove it from this archetype entirely
            case 0:
            {
                _chunksWithSpace.Remove(chunk);
                _chunks.Remove(chunk);
                if (_spareChunks.Count < 4)
                    _spareChunks.Push(chunk);
                break;
            }

            // If the chunk was previously full and now isn't, add it to the set of chunks with space
            case CHUNK_SIZE - 1:
                _chunksWithSpace.Add(chunk);
                break;
        }
    }

    internal IReadOnlyList<Chunk> Chunks => _chunks;
}