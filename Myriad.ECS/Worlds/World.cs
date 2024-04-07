using Myriad.ECS.Collections;
using Myriad.ECS.IDs;
using Myriad.ECS.Queries;
using Myriad.ECS.Worlds.Archetypes;

namespace Myriad.ECS.Worlds;

public sealed partial class World
{
    private readonly List<Archetype> _archetypes = [];
    private readonly Dictionary<ArchetypeHash, List<Archetype>> _archetypesByHash = [ ];

    // Keep track of dead entities so their ID can be re-used
    private readonly List<Entity> _deadEntities = [ ];
    private int _nextEntityId = 1;

    private readonly SegmentedList<EntityInfo> _entities = new(1024);

    public IReadOnlyList<Archetype> Archetypes => _archetypes;

    internal World()
    {
    }

    internal void DeleteImmediate(Entity delete)
    {
        // Get the entityinfo for this entity
        ref var entityInfo = ref _entities[delete.ID];

        // Check this is still a valid entity reference. Early exit if the entity
        // is already dead.
        if (entityInfo.Version != delete.Version)
            return;

        // Increment version, this will invalid the handle
        entityInfo.Version++;

        // Notify archetype this entity is dead
        entityInfo.Chunk.Archetype.RemoveEntity(entityInfo);

        // Store this ID for re-use later
        _deadEntities.Add(delete);
    }

    internal Archetype GetArchetype(Entity entity)
    {
        if (entity.ID < 0 || entity.ID >= _entities.TotalCapacity)
            throw new ArgumentException("Invalid entity ID", nameof(entity));

        var info = _entities[entity.ID];
        if (info.Version != entity.Version)
            throw new ArgumentException("Entity is not alive", nameof(entity));

        return info.Chunk.Archetype;
    }

    /// <summary>
    /// Get the current version for a given entity ID
    /// </summary>
    /// <param name="entityId"></param>
    /// <returns>The entity ID, or zero if the entity does not exist</returns>
    internal uint GetVersion(int entityId)
    {
        if (entityId <= 0 || entityId >= _entities.TotalCapacity)
            return 0;
        return _entities[entityId].Version;
    }

    /// <summary>
    /// Find an archetype with the given set of components, using a precomputed archetype hash.
    /// </summary>
    /// <param name="components"></param>
    /// <param name="hash"></param>
    /// <returns></returns>
    internal Archetype GetOrCreateArchetype(OrderedListSet<ComponentID> components, ArchetypeHash hash)
    {
        // Get list of all archetypes with this hash
        if (!_archetypesByHash.TryGetValue(hash, out var candidates))
        {
            candidates = [ ];
            _archetypesByHash.Add(hash, candidates);
        }

        // Check if any of the candidates are the one we need
        foreach (var archetype in candidates)
            if (archetype.SetEquals(components))
                return archetype;

        // Didn't find one, create the new archetype
        var a = new Archetype(this, new FrozenOrderedListSet<ComponentID>(components));

        // Add it to the relevant lists
        _archetypes.Add(a);
        candidates.Add(a);

        return a;
    }

    internal Archetype GetOrCreateArchetype(OrderedListSet<ComponentID> components)
    {
        // Build archetype hash to accelerate querying
        var hash = ArchetypeHash.Create(components);

        return GetOrCreateArchetype(components, hash);
    }

    internal Row MigrateEntity(Entity entity, Archetype to)
    {
        ref var info = ref GetEntityInfo(entity);
        return info.Chunk.Archetype.MigrateTo(entity, ref info, to);
    }

    internal ref EntityInfo AllocateEntity(out Entity entity)
    {
        if (_deadEntities.Count > 0)
        {
            var prev = _deadEntities[^1];
            _deadEntities.RemoveAt(_deadEntities.Count - 1);

            // Add 2 to the version number, instead of 1. Zero is not a valid entity id, so we want
            // to make sure that when the version overflows (after creating and destroying the same
            // entity 4294967296 times) we don't end up back at zero. Since we always add two we'll
            // skip straight from `uint.MaxValue => 1`. This saves checking and branching every time.
            var v = unchecked(prev.Version + 2);
            entity = new Entity(prev.ID, v);
        }
        else
        {
            // Allocate a new ID. This **must not** overflow!
            entity = new Entity(checked(_nextEntityId++), 1);

            // Check if the collection of all entities needs to grow
            if (entity.ID >= _entities.TotalCapacity)
                _entities.Grow();
        }

        // Update the version
        ref var slot = ref _entities[entity.ID];
        slot.Version = entity.Version;

        return ref slot;
    }

    /// <summary>
    /// Count how many entities match this query
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public int Count(QueryDescription query)
    {
        var count = 0;
        foreach (var archetype in query.GetArchetypes())
            count += archetype.Archetype.EntityCount;

        return count;
    }





    //todo: temp API?
    public ref T GetComponentRef<T>(Entity entity)
        where T : IComponent
    {
        if (!entity.Exists(this))
            throw new ArgumentException("entity is not alive", nameof(entity));

        // Get the entityinfo for this entity
        ref var entityInfo = ref _entities[entity.ID];

        return ref entityInfo.Chunk.GetRef<T>(entity);
    }

    public bool HasComponent<T>(Entity entity)
        where T : IComponent
    {
        return GetComponentSet(entity).Contains(ComponentID<T>.ID);
    }

    private FrozenOrderedListSet<ComponentID> GetComponentSet(Entity entity)
    {
        if (!entity.Exists(this))
            throw new ArgumentException("entity is not alive", nameof(entity));

        // Get the entityinfo for this entity
        var entityInfo = _entities[entity.ID];

        // Get the set of components for this archetype
        return entityInfo.Chunk.Archetype.Components;
    }

    internal Row GetRow(Entity entity)
    {
        var info = GetEntityInfo(entity);
        return new Row(entity, info.RowIndex, info.Chunk);
    }

    internal ref EntityInfo GetEntityInfo(Entity entity)
    {
        if (!entity.Exists(this))
            throw new ArgumentException("entity is not alive", nameof(entity));

        // Get the entityinfo for this entity
        return ref _entities[entity.ID];
    }
}