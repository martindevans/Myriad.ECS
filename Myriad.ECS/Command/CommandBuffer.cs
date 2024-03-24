using System.Buffers;
using Myriad.ECS.Allocations;
using Myriad.ECS.Collections;
using Myriad.ECS.Extensions;
using Myriad.ECS.IDs;
using Myriad.ECS.Worlds;
using Myriad.ECS.Worlds.Archetypes;

namespace Myriad.ECS.Command;

public sealed class CommandBuffer(World World)
{
    private uint _version;

    private readonly List<BufferedEntityData> _bufferedSets = [ ];

    private int _aggregateNodesCount;
    private readonly BufferedAggregateNode[] _aggregateNodes = new BufferedAggregateNode[512];

    private readonly SortedList<Entity, SortedList<ComponentID, BaseComponentSetter>> _entitySets = [ ];
    private readonly SortedList<Entity, OrderedListSet<ComponentID>> _removes = [ ];
    private readonly OrderedListSet<Entity> _modified = new();
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
        if (_modified.Count > 0)
        {
            // Calculate the new archetype for the entity
            foreach (var entity in _modified)
            {
                var currentArchetype = World.GetArchetype(entity);

                // Set all of the current archetype components
                _tempComponentIdSet.Clear();
                _tempComponentIdSet.UnionWith(currentArchetype.Components);
                var moveRequired = false;

                // Calculate the hash and component set of the new archetype
                var hash = currentArchetype.Hash;
                if (_entitySets.TryGetValue(entity, out var sets))
                {
                    foreach (var (id, _) in sets.Enumerable())
                    {
                        if (_tempComponentIdSet.Add(id))
                        {
                            hash = hash.Toggle(id);
                            moveRequired = true;
                        }
                    }
                }
                if (_removes.TryGetValue(entity, out var removes))
                {
                    foreach (var remove in removes)
                    {
                        if (_tempComponentIdSet.Remove(remove))
                        {
                            hash = hash.Toggle(remove);
                            moveRequired = true;
                        }
                    }
                    removes.Clear();
                    Pool<OrderedListSet<ComponentID>>.Return(removes);
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
                if (sets != null)
                {
                    foreach (var (_, set) in sets.Enumerable())
                    {
                        set.Write(row);
                        set.ReturnToPool();
                    }

                    // Recycle
                    sets.Clear();
                    Pool.Return(sets);
                }
            }
        }
        _modified.Clear();
        _entitySets.Clear();
        _removes.Clear();
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
            _aggregateNodes[0] = new BufferedAggregateNode();
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
                bufferedData.Node = _aggregateNodes[bufferedData.Node].GetNodeIndex(key, _aggregateNodes, ref _aggregateNodesCount);
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
        if (!_entitySets.TryGetValue(entity, out var set))
        {
            set = Pool<SortedList<ComponentID, BaseComponentSetter>>.Get();
            set.Clear();

            _entitySets.Add(entity, set);
        }

        // Create a setter and store it in the list (recycling the old one, if it's there)
        var setter = GenericComponentSetter<T>.Get(value);
        if (set.Remove(setter.ID, out var old))
            old.ReturnToPool();
        set.Add(setter.ID, setter);

        // Mark this as modified. If already modified it's possible there's an earlier remove operation for this
        // component, which now needs to be removed.
        if (!_modified.Add(entity))
            if (_removes.TryGetValue(entity, out var removes))
                removes.Remove(setter.ID);
    }

    /// <summary>
    /// Remove a component attached to an entity
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entity"></param>
    public void Remove<T>(Entity entity)
        where T : IComponent
    {
        // Get a list of "remove" operations for this entity
        if (!_removes.TryGetValue(entity, out var set))
        {
            set = Pool<OrderedListSet<ComponentID>>.Get();
            set.Clear();

            _removes.Add(entity, set);
        }

        // Add a remover to the list
        var id = ComponentID<T>.ID;
        set.Add(id);

        // Add this entity to the modified set. If it was already there it's possible that
        // there were some _sets_ for this component. Remove them.
        if (!_modified.Add(entity))
            if (_entitySets.TryGetValue(entity, out var sets))
                if (sets.Remove(id, out var setter))
                    setter.ReturnToPool();
    }

    /// <summary>
    /// Delete an entity
    /// </summary>
    /// <param name="entity"></param>
    public void Delete(Entity entity)
    {
        _deletes.Add(entity);
        _entitySets.Remove(entity);
        _removes.Remove(entity);
    }

    /// <summary>
    /// Bulk delete entities
    /// </summary>
    /// <param name="entities"></param>
    public void Delete(List<Entity> entities)
    {
        _deletes.AddRange(entities);

        foreach (var entity in entities)
        {
            _entitySets.Remove(entity);
            _removes.Remove(entity);
        }
    }

    /// <summary>
    /// An entity that has been created in a command buffer, but not yet created. Can be used to add components.
    /// </summary>
    public readonly record struct BufferedEntity
    {
        private readonly uint _id;
        internal readonly uint Version;
        private readonly CommandBuffer _buffer;

        public BufferedEntity(uint id, CommandBuffer buffer)
        {
            _id = id;
            _buffer = buffer;
            Version = buffer._version;
        }

        private void Check()
        {
            if (Version != _buffer._version)
                throw new InvalidOperationException("Cannot use `BufferedEntity` after CommandBuffer has been played");
        }

        public BufferedEntity Set<T>(T value, bool overwrite = false)
            where T : IComponent
        {
            Check();
            _buffer.SetBuffered(_id, value, overwrite);
            return this;
        }

        /// <summary>
        /// Resolve this buffered Entity into the real Entity that was constructed
        /// </summary>
        /// <param name="resolver"></param>
        /// <returns></returns>
        public Entity Resolve(Resolver resolver)
        {
            if (resolver.Parent == null)
                throw new ObjectDisposedException("Resolver has already been disposed");
            if (resolver.Parent != _buffer)
                throw new InvalidOperationException("Cannot use a resolver from one CommandBuffer with BufferedEntity from another");

            return resolver.Lookup[_id];
        }
    }

    private record struct BufferedEntityData(uint Id, SortedList<ComponentID, BaseComponentSetter> Setters, int Node)
        : IComparable<BufferedEntityData>
    {
        public readonly int CompareTo(BufferedEntityData other)
        {
            return Node.CompareTo(other.Node);
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

    /// <summary>
    /// Provides a way to resolve created entities. Must be disposed once finished with!
    /// </summary>
    public sealed class Resolver
        : IDisposable
    {
        internal SortedList<uint, Entity> Lookup { get; } = [];
        internal CommandBuffer? Parent { get; private set; }
        private uint _version;

        internal void Configure(CommandBuffer parent)
        {
            Lookup.Clear();
            Parent = parent;
            _version = parent._version;
        }

        public void Dispose()
        {
            if (Parent == null)
                throw new ObjectDisposedException(nameof(Resolver));

            Parent = null;
            Lookup.Clear();

            Pool<Resolver>.Return(this);
        }

        public Entity Resolve(BufferedEntity bufferedEntity)
        {
            if (_version != bufferedEntity.Version)
                throw new InvalidOperationException("Cannot use a resolver from one CommandBuffer with BufferedEntity from another generation of the same buffer");

            return bufferedEntity.Resolve(this);
        }
    }
}