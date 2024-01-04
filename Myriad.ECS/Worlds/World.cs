using System.Collections.Frozen;
using Myriad.ECS.Collections;
using Myriad.ECS.IDs;
using Myriad.ECS.Worlds.Archetypes;

namespace Myriad.ECS.Worlds;

public sealed partial class World
{
    private readonly List<Archetype> _archetypes = [ ];

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
        ref var entityInfo = ref _entities.GetMutable(delete.ID);

        // Check this is still a valid entity reference
        if (entityInfo.Version != delete.Version)
            return;

        // Increment version, this will invalid the handle
        entityInfo.Version++;

        // Notify archetype this entity is dead
        entityInfo.Archetype.RemoveEntity(delete, entityInfo.ChunkIndex);

        // Store this ID for re-use later
        _deadEntities.Add(delete);


    }

    internal Archetype GetArchetype(Entity entity)
    {
        if (entity.ID < 0 || entity.ID >= _entities.TotalCapacity)
            throw new ArgumentException("Invalid entity ID", nameof(entity));

        var info = _entities.GetImmutable(entity.ID);
        if (info.Version != entity.Version)
            throw new ArgumentException("Entity is not alive", nameof(entity));

        return info.Archetype;
    }

    /// <summary>
    /// Get the current version for a given entity ID
    /// </summary>
    /// <param name="entityId"></param>
    /// <returns></returns>
    internal uint GetVersion(int entityId)
    {
        if (entityId < 0 || entityId >= _entities.TotalCapacity)
            return 0;
        return _entities.GetImmutable(entityId).Version;
    }

    /// <summary>
    /// Find an archetype with the given set of components, using a precomputed archetype hash.
    /// </summary>
    /// <param name="components"></param>
    /// <param name="hash"></param>
    /// <returns></returns>
    internal Archetype GetOrCreateArchetype(IReadOnlySet<ComponentID> components, ArchetypeHash hash)
    {
        // Check all archetypes
        foreach (var archetype in _archetypes)
        {
            // Fast approcimate check (false positives possible, false negatives not)
            if (archetype.Hash != hash)
                continue;

            // Full/slow check
            if (archetype.Components.SetEquals(components))
                return archetype;
        }

        // No luck, create a new archetype
        var a = new Archetype(this, components.ToFrozenSet());
        _archetypes.Add(a);
        return a;
    }

    private Archetype GetOrCreateArchetype(IReadOnlySet<ComponentID> components, out ArchetypeHash hash)
    {
        // Build archetype hash to accelerate querying
        hash = new ArchetypeHash();
        foreach (var component in components)
            hash = hash.Toggle(component);

        return GetOrCreateArchetype(components, hash);
    }

    internal (Entity entity, Row slot) CreateEntity(IReadOnlySet<ComponentID> components)
    {
        // Find an ID to use for this new entity
        ref var entityInfo = ref AllocateEntity(out var entity);

        // Find the archetype for this entity
        entityInfo.Archetype = GetOrCreateArchetype(components, out _);

        // Add this entity to the archetype
        var row = entityInfo.Archetype.AddEntity(entity);
        entityInfo.ChunkIndex = row.ChunkIndex;

        // Job done!
        return (entity, row);
        
        ref EntityInfo AllocateEntity(out Entity entity)
        {
            if (_deadEntities.Count > 0)
            {
                var prev = _deadEntities[^1];
                _deadEntities.RemoveAt(_deadEntities.Count - 1);
                entity = new Entity(prev.ID, unchecked(prev.Version + 1));

                // Check if the version has rolled over and make sure we're _not_ using version 0
                if (entity.Version == 0)
                    entity = new Entity(entity.ID, 1);
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
            ref var slot = ref _entities.GetMutable(entity.ID);
            slot.Version = entity.Version;

            return ref slot;
        }
    }





    //public Future<Empty> Query<TQ>(TQ query)
    //    where TQ : struct, IQuery
    //{

    //}

    //todo: use source generation to generate a special extension method for every single query.



    //todo: temp API?
    public ref T GetComponentRef<T>(Entity entity)
        where T : IComponent
    {
        if (!entity.IsAlive(this))
            throw new ArgumentException("entity is not alive", nameof(entity));

        // Get the entityinfo for this entity
        ref var entityInfo = ref _entities.GetMutable(entity.ID);

        return ref entityInfo.Archetype.GetChunk(entityInfo.ChunkIndex).GetMutable<T>(entity);
    }

    public bool HasComponent<T>(Entity entity)
        where T : IComponent
    {
        if (!entity.IsAlive(this))
            throw new ArgumentException("entity is not alive", nameof(entity));

        // Get the entityinfo for this entity
        ref var entityInfo = ref _entities.GetMutable(entity.ID);

        // Check if this archetype contains the component
        return entityInfo.Archetype.Components.Contains(ComponentID<T>.ID);
    }

    internal Row GetRow(Entity entity)
    {
        var info = GetEntityInfo(entity);

        return info.Archetype.GetRow(entity, info.ChunkIndex);
    }

    internal EntityInfo GetEntityInfo(Entity entity)
    {
        if (!entity.IsAlive(this))
            throw new ArgumentException("entity is not alive", nameof(entity));

        // Get the entityinfo for this entity
        return _entities.GetMutable(entity.ID);
    }
}