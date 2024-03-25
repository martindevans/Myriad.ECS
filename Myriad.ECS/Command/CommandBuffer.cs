using System.Buffers;
using Myriad.ECS.Allocations;
using Myriad.ECS.Collections;
using Myriad.ECS.Extensions;
using Myriad.ECS.IDs;
using Myriad.ECS.Worlds;
using Myriad.ECS.Worlds.Archetypes;

namespace Myriad.ECS.Command;

public sealed partial class CommandBuffer(World World)
{
    private uint _version;

    private readonly List<BufferedEntityData> _bufferedSets = [ ];

    // Keep track of a fix number of aggregation nodes. The root node (0) is the node for a new entity
    // with no components. Every nodes stores a list of "edges" leading to other nodes. Edges indicate
    // the addition of that component to the entity. Buffered entities keep track of their node ID. Every
    // buffered entity with the same node ID therefore has the same archetype. Except for node=-1, which
    // indicates unknown.
    private int _aggregateNodesCount;
    private readonly BufferedAggregateNode[] _bufferedAggregateNodes = new BufferedAggregateNode[512];

    private readonly SortedList<Entity, EntityModificationData> _entityModifications = [ ];
    private readonly List<Entity> _deletes = [ ];

    private readonly OrderedListSet<ComponentID> _tempComponentIdSet = new();

    #region playback
    public Resolver Playback()
    {
        // Create a "resolver" that can be used to resolve entity IDs
        var resolver = Pool<Resolver>.Get();
        resolver.Configure(this);

        // Create buffered entities.
        CreateBufferedEntities(resolver);

        // Delete entities
        foreach (var delete in _deletes)
            World.DeleteImmediate(delete);
        _deletes.Clear();

        // Structural changes (add/remove components)
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

                // Get a row handle for the entity, moving it to a new archetype first if necessary
                Row row;
                if (moveRequired)
                {
                    // Get the new archetype we're moving to
                    var newArchetype = World.GetOrCreateArchetype(_tempComponentIdSet, hash);

                    // Migrate the entity across
                    row = World.MigrateEntity(entity, newArchetype);
                }
                else
                {
                    row = World.GetRow(entity);
                }

                // Run all setters
                if (mod.Sets != null)
                {
                    foreach (var (_, set) in mod.Sets.Enumerable())
                    {
                        set.Write(row);
                        set.ReturnToPool();
                    }

                    // Recycle
                    mod.Sets.Clear();
                    Pool.Return(mod.Sets);
                }
            }
        }
        _entityModifications.Clear();
        _tempComponentIdSet.Clear();
        _aggregateNodesCount = 0;

        // Update the version of this buffer, invalidating all buffered entities for further modification
        unchecked { _version++; }

        // Return the resolver
        return resolver;
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
                {
                    setter.Write(slot);
                    setter.ReturnToPool();
                }

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
        var set = Pool<SortedList<ComponentID, BaseComponentSetter>>.Get();
        set.Clear();

        // Store this entity in the collection of entities
        // Put it in aggregate node 0 (i.e. no components)
        var id = (uint)(_bufferedSets.Count);
        _bufferedSets.Add(new BufferedEntityData(id, set, 0));
        
        return new BufferedEntity(id, this);
    }

    private void SetBuffered<T>(uint id, T value, bool allowDuplicates = false)
        where T : IComponent
    {
        if (id >= _bufferedSets.Count)
            throw new InvalidOperationException("Unknown entity ID in SetBuffered");

        var bufferedData = _bufferedSets[(int)id];
        var setters = bufferedData.Setters;

        // Try to find this component in the set
        var key = ComponentID<T>.ID;
        var index =  setters.IndexOfKey(key);
        if (index != -1)
        {
            if (!allowDuplicates)
                throw new InvalidOperationException("Cannot set the same component twice onto a buffered entity");

            // Remove and recycle the old setter
            var prevSetter = setters.Values[index];
            prevSetter.ReturnToPool();

            // overwrite it with new setter
            var newSetter = GenericComponentSetter<T>.Get(value);
#if NET6_0_OR_GREATER
            setters.SetValueAtIndex(index, newSetter);
#else
            setters[key] = newSetter;
#endif
        }
        else
        {
            // Add a setter to the set
            var setter = GenericComponentSetter<T>.Get(value);
            setters.Add(setter.ID, setter);

            // Update node id. Skip it if it's in node -1, once an entity is
            // marked as node -1 it's been opted out of aggregation.
            if (bufferedData.Node != -1)
            {
                bufferedData.Node = _bufferedAggregateNodes[bufferedData.Node].GetNodeIndex(key, _bufferedAggregateNodes, ref _aggregateNodesCount);
                _bufferedSets[(int)id] = bufferedData;
            }
        }
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
        var mod = GetModificationData(entity, true, false);

        // Create a setter and store it in the list (recycling the old one, if it's there)
        var setter = GenericComponentSetter<T>.Get(value);
        if (mod.Sets!.Remove(setter.ID, out var old))
            old.ReturnToPool();
        mod.Sets!.Add(setter.ID, setter);

        // Remove it from the "remove" set. In case it was previously removed
        mod.Removes?.Remove(setter.ID);
    }

    /// <summary>
    /// Remove a component attached to an entity
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entity"></param>
    public void Remove<T>(Entity entity)
        where T : IComponent
    {
        var mod = GetModificationData(entity, false, true);

        // Add a remover to the list
        var id = ComponentID<T>.ID;
        mod.Removes!.Add(id);

        // Remove it from the setters, if it's there
        if (mod.Sets != null && mod.Sets.Remove(id, out var setter))
            setter.ReturnToPool();
    }

    /// <summary>
    /// Delete an entity
    /// </summary>
    /// <param name="entity"></param>
    public void Delete(Entity entity)
    {
        _deletes.Add(entity);

        if (_entityModifications.Remove(entity, out var mod))
        {
            if (mod.Sets != null)
            {
                foreach (var (_, setter) in mod.Sets.Enumerable())
                    setter.ReturnToPool();
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
        var idx = _entityModifications.IndexOfKey(entity);

        // Add it if it's missing
        if (idx == -1)
        {
            var mod = new EntityModificationData(
                ensureSet ? Pool<SortedList<ComponentID, BaseComponentSetter>>.Get() : null,
                ensureRemove ? Pool<OrderedListSet<ComponentID>>.Get() : null
            );
            mod.Sets?.Clear();
            mod.Removes?.Clear();

            _entityModifications.Add(entity, mod);

            return mod;
        }
        else
        {
            // It was found, but do we need to modify it
            var mod = _entityModifications.Values[idx];

            var overwrite = false;
            if (mod.Sets == null && ensureSet)
            {
                mod.Sets = Pool<SortedList<ComponentID, BaseComponentSetter>>.Get();
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
    /// <param name="Id">ID of this buffered entity, used in resolver to get actual entity</param>
    /// <param name="Setters">All setters to be run on this entity</param>
    /// <param name="Node">The "Node ID" of this entity, all buffered entities with the same node ID are in the same archetype (except -1)</param>
    private record struct BufferedEntityData(uint Id, SortedList<ComponentID, BaseComponentSetter> Setters, int Node);

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
                    var idx = componentIds.Slice(0, _edgeCount).IndexOf(component.Value);
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

    private record struct EntityModificationData(SortedList<ComponentID, BaseComponentSetter>? Sets, OrderedListSet<ComponentID>? Removes);
}