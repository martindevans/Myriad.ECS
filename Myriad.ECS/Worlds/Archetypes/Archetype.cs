using Myriad.ECS.Collections;
using Myriad.ECS.Components;
using Myriad.ECS.IDs;
using Myriad.ECS.Worlds.Chunks;

namespace Myriad.ECS.Worlds.Archetypes;

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

    public int EntityCount { get; private set; }

    /// <summary>
    /// Indicates if any of the components in this Archetype implement <see cref="IPhantomComponent"/>;
    /// </summary>
    public bool HasPhantomComponents { get; }

    /// <summary>
    /// Indicates if any of the components in this Archetype is <see cref="Phantom"/>
    /// </summary>
    public bool IsPhantom { get; }

    internal Archetype(World world, FrozenOrderedListSet<ComponentID> components)
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

        // Check if this archetype has any phantom components or if any of the components are Phantom
        foreach (var component in components)
        {
            IsPhantom |= component == ComponentID<Phantom>.ID;
            HasPhantomComponents |= component.IsPhantomComponent;
        }
    }

    internal (Entity entity, Row slot) CreateEntity()
    {
        // Allocate an entity in the world
        ref var info = ref World.AllocateEntity(out var entity);

        // Add it to this archetype, find a row to put components into
        var row = AddEntity(entity, ref info);

        return (entity, row);
    }

    /// <summary>
    /// Find a chunk with space and add the given entity to it.
    /// </summary>
    /// <param name="entity">Entity to add to a chunk</param>
    /// <param name="info">Info will be mutated to point to the new location</param>
    /// <returns></returns>
    internal Row AddEntity(Entity entity, ref EntityInfo info)
    {
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

    internal void RemoveEntity(EntityInfo info)
    {
        info.Chunk.RemoveEntity(info);

        // Decrease archetype entity count
        EntityCount--;

        // Execute handler for when an entity is removed from a chunk
        HandleChunkEntityRemoved(info.Chunk);
    }

    internal Row MigrateTo(Entity entity, ref EntityInfo info, Archetype to)
    {
        // Early exit if we're migrating to where we already are!
        if (to == this)
            return info.GetRow(entity);

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

    public ArchetypeEntityEnumerable Entities => new ArchetypeEntityEnumerable(this);
}