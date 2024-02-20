using Myriad.ECS.Allocations;
using Myriad.ECS.Collections;
using Myriad.ECS.IDs;
using Myriad.ECS.Worlds;
using Myriad.ECS.Worlds.Archetypes;

namespace Myriad.ECS.Command;

public sealed class CommandBuffer(World World)
{
    private uint _version;
    private uint _nextBufferedEntityId;

    private readonly SortedList<uint, SortedList<ComponentID, BaseComponentSetter>> _bufferedSets = [];
    private readonly SortedList<Entity, SortedList<ComponentID, BaseComponentSetter>> _entitySets = [];
    private readonly SortedList<Entity, OrderedListSet<ComponentID>> _removes = [];
    private readonly OrderedListSet<Entity> _modified = [ ];
    private readonly List<Entity> _deletes = [ ];

    private readonly OrderedListSet<ComponentID> _tempComponentIdSet = [ ];

    public Resolver Playback()
    {
        // Create a "resolver" that can be used to resolve entity IDs
        var resolver = Pool<Resolver>.Get();
        resolver.Configure(this);

        // Create buffered entities.
        _tempComponentIdSet.Clear();
        foreach (var (bufEntId, components) in _bufferedSets)
        {
            // Build a set of components on this new entity
            _tempComponentIdSet.Clear();
            foreach (var compId in components.Keys)
                _tempComponentIdSet.Add(compId);

            // Allocate an entity for it
            var (entity, slot) = World.CreateEntity(_tempComponentIdSet);

            // Store the new ID in the resolver so it can be retrieved later
            resolver.Lookup.Add(bufEntId, entity);

            // Write the components into the entity
            foreach (var setter in components.Values)
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
                    foreach (var (id, _) in sets)
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
                    foreach (var (_, set) in sets)
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

        // Update the version of this buffer, invalidating all buffered entities for further modification
        unchecked { _version++; }

        // Return the resolver
        return resolver;
    }

    public BufferedEntity Create()
    {
        var set = Pool<SortedList<ComponentID, BaseComponentSetter>>.Get();
        set.Clear();

        var id = checked(_nextBufferedEntityId++);
        _bufferedSets.Add(id, set);
        return new BufferedEntity(id, this);
    }

    private void SetBuffered<T>(uint id, T value, bool allowDuplicates = false)
        where T : IComponent
    {
        if (!_bufferedSets.TryGetValue(id, out var set))
            throw new InvalidOperationException("Unknown entity ID in SetBuffered");

        // Remove previous setter, if this was one
        if (set.Remove(ComponentID<T>.ID, out var prevSetter))
        {
            if (!allowDuplicates)
                throw new InvalidOperationException("Cannot set the same component twice onto a buffered entity");
            prevSetter.ReturnToPool();
        }

        // Add a setter to the list
        var setter = GenericComponentSetter<T>.Get(value);

        set.Add(setter.ID, setter);
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
            ObjectDisposedException.ThrowIf(Parent == null, typeof(Resolver));
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