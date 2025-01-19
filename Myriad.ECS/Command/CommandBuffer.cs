using System.Buffers;
using System.Diagnostics;
using Myriad.ECS.Allocations;
using Myriad.ECS.Collections;
using Myriad.ECS.Components;
using Myriad.ECS.IDs;
using Myriad.ECS.Queries;
using Myriad.ECS.Worlds;
using Myriad.ECS.Worlds.Archetypes;

namespace Myriad.ECS.Command;

/// <summary>
/// Buffers up modifications to entities and replays them all at once.
/// </summary>
public sealed partial class CommandBuffer
{
    private uint _version;

    /// <summary>
    /// The <see cref="World"/> this <see cref="CommandBuffer"/> is modifying
    /// </summary>
    public World World { get; }

    /// <summary>
    /// Collection of all components to be set onto entities
    /// </summary>
    private readonly ComponentSetterCollection _setters = new();

    private readonly List<BufferedEntityData> _bufferedSets = [ ];

    private readonly Dictionary<Entity, EntityModificationData> _entityModifications = [ ];
    private readonly List<Entity> _deletes = [ ];
    private readonly List<QueryDescription> _archetypeDeletes = [ ];
    private readonly OrderedListSet<Entity> _maybeAddingPhantomComponent = [ ];

    private readonly OrderedListSet<ComponentID> _tempComponentIdSet = [ ];

    private readonly BufferedRelationBinder _bufferedRelationBindings = new();
    private readonly UnbufferedRelationBinder _unbufferedRelationBindings = new();

    private Resolver _nextResolver;

    /// <summary>
    /// Create a new <see cref="CommandBuffer"/> for the given <see cref="World"/>
    /// </summary>
    /// <param name="world"></param>
    public CommandBuffer(World world)
    {
        World = world;

        _nextResolver = Pool<Resolver>.Get();
        _nextResolver.Configure(this);
    }

    #region playback
    /// <summary>
    /// Apply all of the operations in this buffer to the <see cref="World"/>
    /// </summary>
    /// <returns></returns>
    public Resolver Playback()
    {
        // Use this resolver for this playback
        var resolver = _nextResolver;

        // Create buffered entities.
        CreateBufferedEntities(resolver);

        // Lazy command buffer accumulates any changes caused by applying this command buffer
        var lazy = new LazyCommandBuffer(World);
        
        // Delete entities, this must occur before structural changes because it may trigger new structural changes
        // by adding a new phantom component.
        DeleteEntities(ref lazy);

        // Structural changes (add/remove components)
        ApplyStructuralChanges(ref lazy);

        // Dispose all disposable components which were enqueued but were never attached to an Entity
        _setters.DisposeAllOverwritten(ref lazy);

        // Clear all temporary state
        _maybeAddingPhantomComponent.Clear();
        _setters.Clear();
        _entityModifications.Clear();
        _tempComponentIdSet.Clear();
        _aggregateNodesCount = 0;

        // Update the version of this buffer, invalidating all buffered entities for further modification
        unchecked { _version++; }

        // Apply all late-bound relationships (this requires using the resolver, so must be done after the version bump)
        _bufferedRelationBindings.Apply(resolver);
        _bufferedRelationBindings.Clear();
        _unbufferedRelationBindings.Apply(resolver);
        _unbufferedRelationBindings.Clear();

        // Apply any changes caused by these changes
        if (lazy.TryGetBuffer(out var lazyBuffer))
        {
            lazyBuffer.Playback().Dispose();
            World.ReturnCommandBuffer(lazyBuffer);
        }

        // Create a resolver ready to use in the future
        _nextResolver = Pool<Resolver>.Get();
        _nextResolver.Configure(this);

        // Return the resolver
        return resolver;
    }

    private void DeleteEntities(ref LazyCommandBuffer lazy)
    {
        foreach (var query in _archetypeDeletes)
        {
            foreach (var match in query.GetArchetypes())
            {
                if (match.Archetype.EntityCount == 0)
                    continue;

                World.DeleteImmediate(match.Archetype, ref lazy);
            }
        }
        _archetypeDeletes.Clear();

        foreach (var delete in _deletes)
        {
            // If there are any modifications enqueue for this entity, delete them
            if (_entityModifications.TryGetValue(delete, out var mods))
                _setters.Dispose(mods.Sets, ref lazy);

            // Skip deleted entities
            if (!delete.Exists())
                continue;

            var archetype = World.GetArchetype(delete.ID);
            if (archetype is { IsPhantom: false, HasPhantomComponents: true } || IsAddingPhantomComponent(delete))
            {
                // It has phantom components and isn't yet a phantom. Add a Phantom component.
                InternalSet(delete, new Phantom());
            }
            else
            {
                World.DeleteImmediate(delete.ID, ref lazy);

                // Return objects to pools
                if (_entityModifications.Remove(delete, out var mod))
                {
                    if (mod.Sets != null)
                    {
                        mod.Sets.Clear();
                        Pool.Return(mod.Sets);
                    }

                    if (mod.Removes != null)
                    {
                        mod.Removes.Clear();
                        Pool.Return(mod.Removes);
                    }
                }
            }
        }

        _deletes.Clear();

        // Check if this entity should not be deleted, because a phantom component is being added
        bool IsAddingPhantomComponent(Entity entity)
        {
            if (_maybeAddingPhantomComponent.Contains(entity) && _entityModifications.TryGetValue(entity, out var mod) && mod.Sets != null)
                foreach (var key in mod.Sets.Keys)
                    if (key.IsPhantomComponent)
                        return true;

            return false;
        }
    }

    private void ApplyStructuralChanges(ref LazyCommandBuffer lazy)
    {
        if (_entityModifications.Count > 0)
        {
            // Calculate the new archetype for the entity
            foreach (var (entity, mod) in _entityModifications)
            {
                // Skip entities that have been deleted since this was enqueued
                if (!entity.Exists())
                {
                    _setters.Dispose(mod.Sets, ref lazy);
                    continue;
                }

                var currentArchetype = World.GetArchetype(entity.ID);

                // Set all of the current archetype components
                _tempComponentIdSet.Clear();
                _tempComponentIdSet.UnionWith(currentArchetype.Components);
                var moveRequired = false;

                // Calculate the hash and component set of the new archetype
                var hash = currentArchetype.Hash;
                if (mod.Sets != null)
                {
                    foreach (var id in mod.Sets.Keys)
                    {
                        if (_tempComponentIdSet.Add(id))
                        {
                            hash = hash.Toggle(id);
                            moveRequired = true;
                        }
                    }
                }
                if (mod.Removes != null)
                {
                    foreach (var remove in mod.Removes)
                    {
                        if (_tempComponentIdSet.Remove(remove))
                        {
                            hash = hash.Toggle(remove);
                            moveRequired = true;
                        }
                    }

                    // Recycle remove set
                    mod.Removes.Clear();
                    Pool.Return(mod.Removes);
                }

                // If it's already a phantom then it will be autodeleted if the last phantom component has been removed.
                var autodelete = currentArchetype.IsPhantom && !_tempComponentIdSet.Any(static a => a.IsPhantomComponent);
                if (autodelete)
                {
                    World.DeleteImmediate(entity.ID, ref lazy);
                }
                else
                {
                    // Get a row handle for the entity, moving it to a new archetype first if necessary
                    Row row;
                    if (moveRequired)
                    {
                        // Get the new archetype we're moving to
                        var newArchetype = World.GetOrCreateArchetype(_tempComponentIdSet, hash);

                        // Migrate the entity across
                        row = World.MigrateEntity(entity.ID, newArchetype, ref lazy);
                    }
                    else
                    {
                        row = World.GetRow(entity.ID);
                    }

                    // Run all setters
                    if (mod.Sets != null)
                        foreach (var set in mod.Sets.Values)
                            _setters.Write(set, row);
                }

                // Recycle setters
                if (mod.Sets != null)
                {
                    mod.Sets.Clear();
                    Pool.Return(mod.Sets);
                }
            }
        }
    }

    private void CreateBufferedEntities(Resolver resolver)
    {
        _tempComponentIdSet.Clear();

        // Keep a map from node ID -> archetype. This means we only need to calculate it once
        // per node ID.
        var archetypeLookup = ArrayPool<Archetype>.Shared.Rent(_aggregateNodesCount);
        Array.Clear(archetypeLookup, 0, archetypeLookup.Length);
        try
        {
            for (var i = 0; i < _bufferedSets.Count; i++)
            {
                var bufferedData = _bufferedSets[i];
                var components = bufferedData.Setters;

                var archetype = GetArchetype(bufferedData, archetypeLookup);

                var slot = archetype.CreateEntity();

                // Store the new ID in the resolver so it can be retrieved later
                resolver.Lookup.Add(bufferedData.Id, slot.Entity);

                // Write the components into the entity
                foreach (var setter in components.Values)
                    _setters.Write(setter, slot);

                // Recycle
                components.Clear();
                Pool.Return(components);
            }

            _bufferedSets.Clear();
            _tempComponentIdSet.Clear();
        }
        finally
        {
            ArrayPool<Archetype>.Shared.Return(archetypeLookup);
        }
    }

    private Archetype GetArchetype(BufferedEntityData entityData, Archetype?[] archetypeLookup)
    {
        // Check the cache
        if (entityData.Node >= 0)
        {
            var a = archetypeLookup[entityData.Node];
            if (a != null)
                return a;
        }

        // Get the archetype
        var archetype = World.GetOrCreateArchetype(entityData.Setters);

        // If the node ID is positive, cache it
        if (entityData.Node >= 0)
            archetypeLookup[entityData.Node] = archetype;

        return archetype;
    }
    #endregion

    #region clear
    /// <summary>
    /// Clear this <see cref="CommandBuffer"/>
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public void Clear()
    {
        // We can't actually make any changes, but we do still need the lazy buffer
        var lazy = new LazyCommandBuffer(World);

        _setters.ClearAndDispose(ref lazy);

        for (var i = 0; i < _bufferedSets.Count; i++)
        {
            var setters = _bufferedSets[i].Setters;
            setters.Clear();
            Pool.Return(setters);
        }
        _bufferedSets.Clear();

        foreach (var (_, data) in _entityModifications)
        {
            if (data.Removes != null)
            {
                data.Removes.Clear();
                Pool.Return(data.Removes);
            }

            if (data.Sets != null)
            {
                data.Sets.Clear();
                Pool.Return(data.Sets);
            }
        }
        _entityModifications.Clear();

        _aggregateNodesCount = 0;

        _deletes.Clear();
        _archetypeDeletes.Clear();
        _maybeAddingPhantomComponent.Clear();
        _tempComponentIdSet.Clear();
        _bufferedRelationBindings.Clear();
        _unbufferedRelationBindings.Clear();

        _nextResolver.Dispose();
        _nextResolver = Pool<Resolver>.Get();
        _nextResolver.Configure(this);

        if (lazy.TryGetBuffer(out var cmd))
            cmd.Clear();
    }
    #endregion

    /// <summary>
    /// Create a new <see cref="Entity"/> in the world.
    /// </summary>
    /// <returns></returns>
    public BufferedEntity Create()
    {
        // Ensure the root aggregation node exists
        if (_aggregateNodesCount == 0)
        {
            _aggregateNodesCount++;
            _bufferedAggregateNodes[0] = new BufferedAggregateNode();
        }

        // Get a set to hold all of the component setters
        var set = Pool<Dictionary<ComponentID, ComponentSetterCollection.SetterId>>.Get();
        set.Clear();

        // Store this entity in the collection of entities
        // Put it in aggregate node 0 (i.e. no components)
        var id = (uint)_bufferedSets.Count;
        _bufferedSets.Add(new BufferedEntityData(id, set));
        
        return new BufferedEntity(id, this, _nextResolver);
    }

    private void SetBuffered<T>(uint id, T value, DuplicateSet duplicateMode)
        where T : IComponent
    {
        Debug.Assert(id < _bufferedSets.Count, "Unknown entity ID in SetBuffered");

        if (typeof(T) == typeof(Phantom))
            throw new InvalidOperationException("Cannot manually attach `Phantom` component to an entity");

        var bufferedData = _bufferedSets[(int)id];
        var setters = bufferedData.Setters;

        var key = ComponentID<T>.ID;

        if (setters.TryGetValue(key, out var existing))
        {
            switch (duplicateMode)
            {
                case DuplicateSet.Overwrite:
                    _setters.Overwrite(existing, value);
                    break;
                case DuplicateSet.Discard:
                    if (key.IsDisposableComponent)
                        _setters.Discard(value);
                    break;
                case DuplicateSet.Throw:
                    throw new InvalidOperationException("Cannot set the same component twice onto a buffered entity");

                /* dotcover disable */
                default:
                    throw new ArgumentOutOfRangeException(nameof(duplicateMode), duplicateMode, null);
                /* dotcover enable */
            }
        }
        else
        {
            // Add to global collection of setters
            var setterIndex = _setters.Add(value);

            // Store the index in the per-entity collection
            setters.Add(key, setterIndex);

            // Update node id. Skip it if it's in node -1, once an entity is
            // marked as node -1 it's been opted out of aggregation.
            if (bufferedData.Node != -1)
            {
                bufferedData.Node = _bufferedAggregateNodes[bufferedData.Node].GetNodeIndex(key, _bufferedAggregateNodes, ref _aggregateNodesCount);
                _bufferedSets[(int)id] = bufferedData;
            }
        }
    }

    private void SetBuffered<T>(uint id, T value, BufferedEntity relation, DuplicateSet duplicateMode)
        where T : IEntityRelationComponent
    {
        if (relation._buffer != this)
            throw new ArgumentException("Target of relation must be BufferedEntity from the same CommandBuffer", nameof(relation));

        SetBuffered(id, value, duplicateMode);
        _bufferedRelationBindings.Create<T>(new BufferedEntity(id, this, _nextResolver), relation);
    }

    /// <summary>
    /// Add or overwrite a component attached to an entity
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entity"></param>
    /// <param name="value"></param>
    public void Set<T>(Entity entity, T value)
        where T : IComponent
    {
        if (typeof(T) == typeof(Phantom))
            throw new InvalidOperationException("Cannot manually attach `Phantom` component to an entity");

        InternalSet(entity, value);
    }

    /// <summary>
    /// Add or overwrite a component attached to an entity
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entity"></param>
    /// <param name="value"></param>
    /// <param name="relation">When this buffer is played back the given buffered entity will be set into the component</param>
    public void Set<T>(Entity entity, T value, BufferedEntity relation)
        where T : IEntityRelationComponent
    {
        InternalSet(entity, value, relation);
    }

    /// <summary>
    /// Add or overwrite a component attached to an entity
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entity"></param>
    /// <param name="value"></param>
    /// <param name="relation"></param>
    public void Set<T>(Entity entity, T value, Entity relation)
        where T : IEntityRelationComponent
    {
        value.Target = relation;
        InternalSet(entity, value);
    }

    private void InternalSet<T>(Entity entity, T value)
        where T : IComponent
    {
        var mod = GetModificationData(entity, true, false);

        // Create a setter and store it in the list (recycling the old one, if it's there)
        var id = ComponentID<T>.ID;
        if (mod.Sets!.TryGetValue(id, out var existing))
        {
            _setters.Overwrite(existing, value);
        }
        else
        {
            var index = _setters.Add(value);
            mod.Sets!.Add(id, index);
        }

        // Check if this is a phantom component being added
        if (id.IsPhantomComponent)
            _maybeAddingPhantomComponent.Add(entity);

        // Remove it from the "remove" set. In case it was previously removed
        mod.Removes?.Remove(id);
    }

    private void InternalSet<T>(Entity entity, T value, BufferedEntity relation)
        where T : IEntityRelationComponent
    {
        if (relation._buffer != this)
            throw new ArgumentException("Target of relation must be BufferedEntity from the same CommandBuffer", nameof(relation));

        InternalSet(entity, value);
        _unbufferedRelationBindings.Create<T>(entity, relation);
    }

    /// <summary>
    /// Remove a component attached to an entity
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entity"></param>
    public void Remove<T>(Entity entity)
        where T : IComponent
    {
        if (typeof(T) == typeof(Phantom))
            throw new InvalidOperationException("Cannot remove `Phantom` component from an entity");

        var mod = GetModificationData(entity, false, true);

        // Add a remover to the list
        var id = ComponentID<T>.ID;
        mod.Removes!.Add(id);

        // Remove it from the setters, if it's there
        mod.Sets?.Remove(id);
    }

    /// <summary>
    /// Delete an entity
    /// </summary>
    /// <param name="entity"></param>
    public void Delete(Entity entity)
    {
        _deletes.Add(entity);
    }

    /// <summary>
    /// Bulk delete entities
    /// </summary>
    /// <param name="entities"></param>
    public void Delete(List<Entity> entities)
    {
        _deletes.EnsureCapacity(_deletes.Count + entities.Capacity);

        foreach (var entity in entities)
            Delete(entity);
    }

    /// <summary>
    /// Bulk delete all entities which match the given query
    /// </summary>
    /// <param name="entities"></param>
    public void Delete(QueryDescription entities)
    {
        if (entities.World != World)
            throw new ArgumentException("Cannot use QueryDescription from one World with CommandBuffer for another World");

        _archetypeDeletes.Add(entities);
    }

    private EntityModificationData GetModificationData(Entity entity, bool ensureSet, bool ensureRemove)
    {
        // Add it if it's missing
        if (!_entityModifications.TryGetValue(entity, out var existing))
        {
            var mod = new EntityModificationData(
                ensureSet ? Pool<Dictionary<ComponentID, ComponentSetterCollection.SetterId>>.Get() : null,
                ensureRemove ? Pool<OrderedListSet<ComponentID>>.Get() : null
            );
            mod.Sets?.Clear();
            mod.Removes?.Clear();

            _entityModifications.Add(entity, mod);

            return mod;
        }
        else
        {
            // Found it, now modify it (if necessary)
            var mod = existing;

            var overwrite = false;
            if (mod.Sets == null && ensureSet)
            {
                mod.Sets = Pool<Dictionary<ComponentID, ComponentSetterCollection.SetterId>>.Get();
                overwrite = true;
            }

            if (mod.Removes == null && ensureRemove)
            {
                mod.Removes = Pool<OrderedListSet<ComponentID>>.Get();
                overwrite = true;
            }

            if (overwrite)
                _entityModifications[entity] = mod;

            return mod;
        }
    }

    /// <summary>
    /// Data about a new entity being created
    /// </summary>
    private record struct BufferedEntityData
    {
        /// <summary>ID of this buffered entity, used in resolver to get actual entity</summary>
        public uint Id { get; }

        /// <summary>All setters to be run on this entity</summary>
        public Dictionary<ComponentID, ComponentSetterCollection.SetterId> Setters { get; }

        /// <summary>The "Node ID" of this entity, all buffered entities with the same node ID are in the same archetype (except -1)</summary>
        public int Node { get; set; }

        /// <summary>
        /// Data about a new entity being created
        /// </summary>
        /// <param name="id">ID of this buffered entity, used in resolver to get actual entity</param>
        /// <param name="setters">All setters to be run on this entity</param>
        public BufferedEntityData(uint id, Dictionary<ComponentID, ComponentSetterCollection.SetterId> setters)
        {
            Id = id;
            Setters = setters;
        }
    }

    private record struct EntityModificationData(Dictionary<ComponentID, ComponentSetterCollection.SetterId>? Sets, OrderedListSet<ComponentID>? Removes);

    /// <summary>
    /// Indicates how multiple Set operations enqueued for the same entity in this buffer should that be handled
    /// </summary>
    public enum DuplicateSet
    {
        /// <summary>
        /// The later set value should overwrite the earlier one.<br />
        /// <code>Set(A); Set(B);</code>
        /// Would result in `B`
        /// </summary>
        Overwrite,

        /// <summary>
        /// The later set value should be discarded.<br />
        /// <code>Set(A); Set(B);</code>
        /// Would result in `A`
        /// </summary>
        Discard,

        /// <summary>
        /// The later set value should throw an exception.
        /// </summary>
        Throw,
    }
}