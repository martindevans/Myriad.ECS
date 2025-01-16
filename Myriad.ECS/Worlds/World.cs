using System.Collections.Concurrent;
using Myriad.ECS.Collections;
using Myriad.ECS.Command;
using Myriad.ECS.IDs;
using Myriad.ECS.Queries;
using Myriad.ECS.Threading;
using Myriad.ECS.Worlds.Archetypes;

namespace Myriad.ECS.Worlds;

/// <summary>
/// A world contains all entities.
/// </summary>
public sealed partial class World
    : IDisposable
{
    internal IThreadPool ThreadPool { get; }

    private readonly List<Archetype> _archetypes = [ ];
    private readonly Dictionary<ArchetypeHash, List<Archetype>> _archetypesByHash = [ ];

    // Keep track of dead entities so their ID can be re-used
    private readonly List<EntityId> _deadEntities = [ ];
    private int _nextEntityId = 1;

    private readonly SegmentedList<EntityInfo> _entities = new(1024);

    /// <summary>
    /// Get a list of all archetypes in this <see cref="World"/>
    /// </summary>
    public IReadOnlyList<Archetype> Archetypes => _archetypes;
    internal int ArchetypesCount => _archetypes.Count;

    private readonly ConcurrentBag<CommandBuffer> _commandBufferPool = [ ];

    internal World(IThreadPool pool)
    {
        ThreadPool = pool;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        var lazy = new LazyCommandBuffer(this);

        foreach (var archetype in _archetypes)
            archetype.Dispose(ref lazy);

        if (lazy.TryGetBuffer(out var buffer))
            buffer.Playback().Dispose();
    }

    /// <summary>
    /// Get a <see cref="CommandBuffer"/> from the pool or create a new one
    /// </summary>
    /// <returns></returns>
    internal CommandBuffer GetPooledCommandBuffer()
    {
        if (!_commandBufferPool.TryTake(out var buffer))
            buffer = new CommandBuffer(this);
        return buffer;
    }

    /// <summary>
    /// Return a <see cref="CommandBuffer"/> to the internal pool
    /// </summary>
    /// <param name="buffer"></param>
    internal void ReturnPooledCommandBuffer(CommandBuffer buffer)
    {
        //todo: buffer.Clear(); -- this must be implemented before this method can be made public!

        if (_commandBufferPool.Count < 32)
            _commandBufferPool.Add(buffer);
    }

    #region bulk write
    /// <summary>
    /// Overwrite the value of a specific component on every entity which matches the given query
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="item"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    public int Overwrite<T>(T item, QueryDescription? query = null)
        where T : IComponent
    {
        return Overwrite(item, ref query);
    }

    /// <summary>
    /// Overwrite the value of a specific component on every entity which matches the given query
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="item"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    public int Overwrite<T>(T item, ref QueryDescription? query)
        where T : IComponent
    {
        query ??= GetCachedQuery<T>();
        return query.Overwrite(item);
    }
    #endregion

    internal void DeleteImmediate(EntityId delete, ref LazyCommandBuffer lazy)
    {
        // Get the entityinfo for this entity
        ref var entityInfo = ref _entities[delete.ID];

        // Check this is still a valid entity reference. Early exit if the entity
        // is already dead.
        if (entityInfo.Version != delete.Version)
            return;

        // Notify archetype this entity is dead
        entityInfo.Chunk.Archetype.RemoveEntity(entityInfo, ref lazy);

        // Increment version, this will invalid the handle
        entityInfo.Version++;

        // Store this ID for re-use later
        _deadEntities.Add(delete);
    }

    internal void DeleteImmediate(Archetype archetype, ref LazyCommandBuffer lazy)
    {
        // Mark all of the IDs as dead (as long as they haven't become phantoms)
        if (archetype is { HasPhantomComponents: false, IsPhantom: false })
        {
            _deadEntities.EnsureCapacity(_deadEntities.Count + archetype.EntityCount);
            foreach (var entity in archetype.Entities)
            {
                // Get the entityinfo for this entity
                ref var entityInfo = ref _entities[entity.ID.ID];

                // Increment version, this will invalidate the handle
                entityInfo.Version++;

                // Store this ID for re-use later
                _deadEntities.Add(entity.ID);
            }
        }

        // Clear the archetype
        archetype.Clear(ref lazy);
    }

    internal Archetype GetArchetype(EntityId entity)
    {
        if (entity.ID < 0 || entity.ID >= _entities.TotalCapacity)
            throw new ArgumentException("Invalid entity ID", nameof(entity));

        return GetEntityInfo(entity).Chunk.Archetype;
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

    #region Get/Create Archetype
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
        var a = new Archetype(this, FrozenOrderedListSet<ComponentID>.Create(components));

        // Add it to the relevant lists
        _archetypes.Add(a);
        candidates.Add(a);

        return a;
    }

    internal Archetype GetOrCreateArchetype<TV>(Dictionary<ComponentID, TV> components, ArchetypeHash hash)
    {
        // Get list of all archetypes with this hash
        if (!_archetypesByHash.TryGetValue(hash, out var candidates))
        {
            candidates = [];
            _archetypesByHash.Add(hash, candidates);
        }

        // Check if any of the candidates are the one we need
        foreach (var archetype in candidates)
            if (archetype.SetEquals(components))
                return archetype;

        // Didn't find one, create the new archetype
        var set = FrozenOrderedListSet<ComponentID>.Create(components);
        var a = new Archetype(this, set);

        // Add it to the relevant lists
        _archetypes.Add(a);
        candidates.Add(a);

        return a;
    }

    internal Archetype GetOrCreateArchetype(OrderedListSet<ComponentID> components)
    {
        return GetOrCreateArchetype(components, ArchetypeHash.Create(components));
    }

    internal Archetype GetOrCreateArchetype<TV>(Dictionary<ComponentID, TV> components)
    {
        return GetOrCreateArchetype(components, ArchetypeHash.Create(components));
    }
    #endregion

    internal Row MigrateEntity(EntityId entity, Archetype to, ref LazyCommandBuffer lazy)
    {
        ref var info = ref GetEntityInfo(entity);
        return info.Chunk.Archetype.MigrateTo(entity, ref info, to, ref lazy);
    }

    internal ref EntityInfo AllocateEntity(out EntityId entity)
    {
        if (_deadEntities.Count > 0)
        {
            var prev = _deadEntities[^1];
            _deadEntities.RemoveAt(_deadEntities.Count - 1);

            var v = unchecked(prev.Version + 1);

            // Ensure ID 0 is not assigned even after wrapping around 2^32 entities
            if (v == 0)
                v += 1;

            entity = new EntityId(prev.ID, v);
        }
        else
        {
            // Allocate a new ID. This **must not** overflow!
            entity = new EntityId(checked(_nextEntityId++), 1);

            // Check if the collection of all entities needs to grow
            if (entity.ID >= _entities.TotalCapacity)
                _entities.Grow();
        }

        // Update the version
        ref var slot = ref _entities[entity.ID];
        slot.Version = entity.Version;

        return ref slot;
    }

    internal Row GetRow(EntityId entity)
    {
        var info = GetEntityInfo(entity);
        return new Row(entity, info.RowIndex, info.Chunk);
    }

    internal ref EntityInfo GetEntityInfo(EntityId entity)
    {
        ref var info = ref _entities[entity.ID];

        if (info.Version != entity.Version)
            throw new ArgumentException("entity is not alive", nameof(entity));

        return ref info;
    }
}