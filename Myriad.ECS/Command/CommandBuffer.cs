using System.Buffers;
using System.Diagnostics;
using Myriad.ECS.Allocations;
using Myriad.ECS.Collections;
using Myriad.ECS.Components;
using Myriad.ECS.Extensions;
using Myriad.ECS.IDs;
using Myriad.ECS.Worlds;
using Myriad.ECS.Worlds.Archetypes;

namespace Myriad.ECS.Command;

public sealed partial class CommandBuffer(World _world)
{
    private uint _version;

    public World World => _world;

    /// <summary>
    /// Collection of all components to be set onto entities
    /// </summary>
    private readonly ComponentSetterCollection _setters = new();

    private readonly List<BufferedEntityData> _bufferedSets = [ ];

    // Keep track of a fixed number of aggregation nodes. The root node (0) is the node for a new entity
    // with no components. Nodes store a list of "edges" leading to other nodes. Edges indicate
    // the addition of that component to the entity. Buffered entities keep track of their node ID. Every
    // buffered entity with the same node ID therefore has the same archetype. Except for node=-1, which
    // indicates unknown.
    private int _aggregateNodesCount;
    private readonly BufferedAggregateNode[] _bufferedAggregateNodes = new BufferedAggregateNode[512];

    private readonly SortedList<Entity, EntityModificationData> _entityModifications = [ ];
    private readonly List<Entity> _deletes = [ ];
    private readonly OrderedListSet<Entity> _maybeAddingPhantomComponent = new();

    private readonly OrderedListSet<ComponentID> _tempComponentIdSet = new();

    private readonly BufferedRelationBinder _bufferedRelationBindings = new();
    private readonly UnbufferedRelationBinder _unbufferedRelationBindings = new();

    #region playback
    public Resolver Playback()
    {
        // Create a "resolver" that can be used to resolve entity IDs
        var resolver = Pool<Resolver>.Get();
        resolver.Configure(this);

        // Create buffered entities.
        CreateBufferedEntities(resolver);

        // Lazy command buffer accumulates any changes caused by applying this command buffer
        var lazy = new LazyCommandBuffer(World);
        
        // Delete entities, this must occur before structural changes because it may trigger new structural changes
        // by adding a new phantom component.
        DeleteEntities(ref lazy);

        // Structural changes (add/remove components)
        ApplyStructuralChanges(ref lazy);

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
            World.ReturnPooledCommandBuffer(lazyBuffer);
        }

        // Return the resolver
        return resolver;
    }

    private void DeleteEntities(ref LazyCommandBuffer lazy)
    {
        foreach (var delete in _deletes)
        {
            if (!delete.Exists(World))
                continue;

            var archetype = World.GetArchetype(delete);
            if (archetype is { IsPhantom: false, HasPhantomComponents: true } || IsAddingPhantomComponent(delete))
            {
                // It has phantom components and isn't yet a phantom. Add a Phantom component.
                InternalSet(delete, new Phantom());
            }
            else
            {
                World.DeleteImmediate(delete, ref lazy);

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
                foreach (var (key, _) in mod.Sets.Enumerable())
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
            foreach (var (entity, mod) in _entityModifications.Enumerable())
            {
                var currentArchetype = World.GetArchetype(entity);

                // Set all of the current archetype components
                _tempComponentIdSet.Clear();
                _tempComponentIdSet.UnionWith(currentArchetype.Components);
                var moveRequired = false;

                // Calculate the hash and component set of the new archetype
                var hash = currentArchetype.Hash;
                if (mod.Sets != null)
                {
                    foreach (var (id, _) in mod.Sets.Enumerable())
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
                    World.DeleteImmediate(entity, ref lazy);
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
                        row = World.MigrateEntity(entity, newArchetype, ref lazy);
                    }
                    else
                    {
                        row = World.GetRow(entity);
                    }

                    // Run all setters
                    if (mod.Sets != null)
                        foreach (var (_, set) in mod.Sets.Enumerable())
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

                var (entity, slot) = archetype.CreateEntity();

                // Store the new ID in the resolver so it can be retrieved later
                resolver.Lookup.Add(bufferedData.Id, entity);

                // Write the components into the entity
                foreach (var (_, setter) in components.Enumerable())
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

        // Build a set of components on this new entity
        _tempComponentIdSet.Clear();
        foreach (var (compId, _) in entityData.Setters.Enumerable())
            _tempComponentIdSet.Add(compId);

        // Get the archetype
        var archetype = World.GetOrCreateArchetype(_tempComponentIdSet);

        // If the node ID is positive, cache it
        if (entityData.Node >= 0)
            archetypeLookup[entityData.Node] = archetype;

        return archetype;
    }
    #endregion

    public BufferedEntity Create()
    {
        // Ensure the root aggregation node exists
        if (_aggregateNodesCount == 0)
        {
            _aggregateNodesCount++;
            _bufferedAggregateNodes[0] = new BufferedAggregateNode();
        }

        // Get a set to hold all of the component setters
        var set = Pool<SortedList<ComponentID, ComponentSetterCollection.SetterId>>.Get();
        set.Clear();

        // Store this entity in the collection of entities
        // Put it in aggregate node 0 (i.e. no components)
        var id = (uint)_bufferedSets.Count;
        _bufferedSets.Add(new BufferedEntityData(id, set));
        
        return new BufferedEntity(id, this);
    }

    private void SetBuffered<T>(uint id, T value, bool allowDuplicates = false)
        where T : IComponent
    {
        Debug.Assert(id < _bufferedSets.Count, "Unknown entity ID in SetBuffered");

        if (typeof(T) == typeof(Phantom))
            throw new InvalidOperationException("Cannot manually attach `Phantom` component to an entity");

        var bufferedData = _bufferedSets[(int)id];
        var setters = bufferedData.Setters;

        // Try to find this component in the set
        var key = ComponentID<T>.ID;
        var index =  setters.IndexOfKey(key);
        if (index != -1)
        {
            if (!allowDuplicates)
                throw new InvalidOperationException("Cannot set the same component twice onto a buffered entity");

            _setters.Overwrite(setters.Values[index], value);
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

    private void SetBuffered<T>(uint id, T value, BufferedEntity relation, bool allowDuplicates = false)
        where T : IEntityRelationComponent
    {
        if (relation._buffer != this)
            throw new ArgumentException("Target of relation must be BufferedEntity from the same CommandBuffer", nameof(relation));

        SetBuffered(id, value, allowDuplicates);
        _bufferedRelationBindings.Create<T>(new BufferedEntity(id, this), relation);
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

    private EntityModificationData GetModificationData(Entity entity, bool ensureSet, bool ensureRemove)
    {
        // Get the index of this entity in the modifications lookup
        var idx = _entityModifications.IndexOfKey(entity);

        // Add it if it's missing
        if (idx == -1)
        {
            var mod = new EntityModificationData(
                ensureSet ? Pool<SortedList<ComponentID, ComponentSetterCollection.SetterId>>.Get() : null,
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
            var mod = _entityModifications.Values[idx];

            var overwrite = false;
            if (mod.Sets == null && ensureSet)
            {
                mod.Sets = Pool<SortedList<ComponentID, ComponentSetterCollection.SetterId>>.Get();
                overwrite = true;
            }

            if (mod.Removes == null && ensureRemove)
            {
                mod.Removes = Pool<OrderedListSet<ComponentID>>.Get();
                overwrite = true;
            }

            if (overwrite)
            {
#if NET8_0_OR_GREATER
                _entityModifications.SetValueAtIndex(idx, mod);
#else
                _entityModifications[entity] = mod;
#endif
            }

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
        public SortedList<ComponentID, ComponentSetterCollection.SetterId> Setters { get; }

        /// <summary>The "Node ID" of this entity, all buffered entities with the same node ID are in the same archetype (except -1)</summary>
        public int Node { get; set; }

        /// <summary>
        /// Data about a new entity being created
        /// </summary>
        /// <param name="id">ID of this buffered entity, used in resolver to get actual entity</param>
        /// <param name="setters">All setters to be run on this entity</param>
        public BufferedEntityData(uint id, SortedList<ComponentID, ComponentSetterCollection.SetterId> setters)
        {
            Id = id;
            Setters = setters;
        }
    }

    private struct BufferedAggregateNode
    {
        private const int MaxEdges = 16;

        private int _edgeCount;
        private unsafe fixed int _componentIdBuffer[MaxEdges];
        private unsafe fixed int _nodeIdBuffer[MaxEdges];

        public int GetNodeIndex(ComponentID component, BufferedAggregateNode[] nodesArr, ref int nodesCount)
        {
            unsafe
            {
                fixed (int* componentIdBufferPtr = _componentIdBuffer)
                fixed (int* nodeIdBufferPtr = _nodeIdBuffer)
                {
                    var componentIds = new Span<int>(componentIdBufferPtr, MaxEdges);
                    var nodeIds = new Span<int>(nodeIdBufferPtr, MaxEdges);

                    // Find the index of the edge for this component
                    var idx = componentIds[.._edgeCount].IndexOf(component.Value);
                    if (idx >= 0)
                        return nodeIds[idx];

                    // Not found...

                    // If the buffers are full return node -1. This is the "no particular group" node.
                    if (_edgeCount == MaxEdges)
                        return -1;

                    // If the node array itself is full return -1. This is the "no particular group" node.
                    if (nodesCount == nodesArr.Length)
                        return -1;

                    // Create a new node
                    nodesArr[nodesCount] = new BufferedAggregateNode();
                    var newNodeId = nodesCount++;

                    // Create an edge point to a new node
                    componentIds[_edgeCount] = component.Value;
                    nodeIds[_edgeCount] = newNodeId;
                    _edgeCount++;

                    
                    return newNodeId;
                }
            }
        }
    }

    private record struct EntityModificationData(SortedList<ComponentID, ComponentSetterCollection.SetterId>? Sets, OrderedListSet<ComponentID>? Removes);
}