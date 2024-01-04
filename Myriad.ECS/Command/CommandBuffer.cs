using Myriad.ECS.Allocations;
using Myriad.ECS.IDs;
using Myriad.ECS.Worlds;
using Myriad.ECS.Worlds.Archetypes;
using Myriad.ParallelTasks;

namespace Myriad.ECS.Command;

public sealed class CommandBuffer(World World)
{
    private uint _version;
    private uint _nextBufferedEntityId;

    private readonly Dictionary<uint, HashSet<BaseComponentSetter>> _bufferedSets = [];
    private readonly Dictionary<Entity, List<BaseComponentSetter>> _entitySets = [];
    private readonly Dictionary<Entity, List<BaseComponentRemove>> _removes = [];
    private readonly HashSet<Entity> _modified = [ ];
    private readonly List<Entity> _deletes = [ ];

    public Future<Resolver> Playback()
    {
        // Create a "resolver" that can be used to resolve entity IDs
        var resolver = Pool<Resolver>.Get();
        resolver.Configure(this);

        // Create buffered entities. First borrow a set to re-use for all operations.
        using (var compList = Pool<HashSet<ComponentID>>.Rent())
        {
            compList.Value.Clear();

            foreach (var (id, components) in _bufferedSets)
            {
                // Build a set of components on this new entity
                compList.Value.Clear();
                foreach (var setter in components)
                    compList.Value.Add(setter.ID);

                // Allocate an entity for it
                var (entity, slot) = World.CreateEntity(compList.Value);

                // Store the new ID in the resolver so it can be retrieved later
                resolver.Lookup.Add(id, entity);

                // Write the components into the entity
                foreach (var setter in components)
                {
                    setter.Write(slot);
                    setter.ReturnToPool();
                }

                // Recycle
                components.Clear();
                Pool<HashSet<BaseComponentSetter>>.Return(components);
            }
        }

        // Delete entities
        foreach (var delete in _deletes)
            World.DeleteImmediate(delete);

        // Structural changes (add/remove components)
        if (_modified.Count > 0)
        {
            // Borrow a set to use for holding component IDs
            var newArchetypeSet = Pool<HashSet<ComponentID>>.Get();

            // Calculate the new archetype for the entity
            foreach (var entity in _modified)
            {
                var currentArchetype = World.GetArchetype(entity);

                // Set all of the current archetype components
                newArchetypeSet.Clear();
                newArchetypeSet.UnionWith(currentArchetype.Components);
                var moveRequired = false;

                // Calculate the hash and component set of the new archetype
                var hash = currentArchetype.Hash;
                if (_entitySets.TryGetValue(entity, out var sets))
                {
                    foreach (var set in sets)
                    {
                        if (!newArchetypeSet.Contains(set.ID))
                        {
                            hash = hash.Toggle(set.ID);
                            newArchetypeSet.Add(set.ID);
                            moveRequired = true;
                        }
                    }
                }
                if (_removes.TryGetValue(entity, out var removes))
                {
                    foreach (var remove in removes)
                    {
                        if (currentArchetype.Components.Contains(remove.ID))
                        {
                            hash = hash.Toggle(remove.ID);
                            moveRequired = true;
                        }
                    }
                }

                // Get a row handle for the entity, moving it to a new archetype first if necessary
                Row row;
                if (moveRequired)
                {
                    // Get the new archetype we're moving to
                    var newArchetype = World.GetOrCreateArchetype(newArchetypeSet, hash);

                    throw new NotImplementedException("move from old to new, get new row");
                }
                else
                {
                    row = World.GetRow(entity);
                }

                // Run all setters
                if (sets != null)
                {
                    foreach (var set in sets)
                    {
                        set.Write(row);
                        set.ReturnToPool();
                    }

                    // Recycle
                    sets.Clear();
                    Pool<List<BaseComponentSetter>>.Return(sets);
                }
            }
        }

        // Cleanup
        foreach (var (_, list) in _removes)
        {
            list.Clear();
            Pool<List<BaseComponentRemove>>.Return(list);
        }

        // Update the version of this buffer, invalidating all buffered entities for further modification
        unchecked { _version++; }

        return new Future<Resolver>(resolver);
    }

    public BufferedEntity Create()
    {
        var set = Pool<HashSet<BaseComponentSetter>>.Get();
        set.Clear();

        var id = checked(_nextBufferedEntityId++);
        _bufferedSets.Add(id, set);
        return new BufferedEntity(id, this);
    }

    private void SetBuffered<T>(uint id, T value, bool allowDuplicates = false)
        where T : IComponent
    {
        if (!_bufferedSets.TryGetValue(id, out var set))
        {
            set = Pool<HashSet<BaseComponentSetter>>.Get();
            set.Clear();

            _bufferedSets.Add(id, set);
        }

        // Add a setter to the list
        var setter = GenericComponentSetter<T>.Get(value);

        if (!set.Add(setter))
        {

            if (allowDuplicates)
            {
                set.Remove(setter);
                set.Add(setter);
            }
            else
                throw new InvalidOperationException("Cannot set the same component twice onto a buffered entity");
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
        if (!_entitySets.TryGetValue(entity, out var list))
        {
            list = Pool<List<BaseComponentSetter>>.Get();
            list.Clear();

            _entitySets.Add(entity, list);
        }

        // Add a setter to the list
        var setter = GenericComponentSetter<T>.Get(value);
        list.Add(setter);

        _modified.Add(entity);
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
        if (!_removes.TryGetValue(entity, out var list))
        {
            list = Pool<List<BaseComponentRemove>>.Get();
            list.Clear();

            _removes.Add(entity, list);
        }

        // Add a remover to the list
        list.Add(GenericComponentRemove<T>.Instance);

        _modified.Add(entity);
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
        private readonly uint _version;
        private readonly CommandBuffer _buffer;

        public BufferedEntity(uint id, CommandBuffer buffer)
        {
            _id = id;
            _buffer = buffer;
            _version = buffer._version;
        }

        private void Check()
        {
            if (_version != _buffer._version)
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
            if (resolver.Lookup == null)
                throw new ObjectDisposedException("Resolver has already been disposed");
            if (resolver.Parent != _buffer)
                throw new InvalidOperationException("Cannot use a resolver from one CommandBuffer with BufferedEntity from another");

            return resolver.Lookup[_id];
        }
    }

    /// <summary>
    /// Provides a way to resolve created entities. Must be disposed before the command buffer can be used again
    /// </summary>
    public sealed class Resolver
        : IDisposable
    {
        internal Dictionary<uint, Entity> Lookup { get; } = [];
        internal CommandBuffer? Parent { get; private set; }

        internal void Configure(CommandBuffer parent)
        {
            Lookup.Clear();
            Parent = parent;
        }

        public void Dispose()
        {
            ObjectDisposedException.ThrowIf(Parent == null, typeof(Resolver));
            Parent = null;
            Lookup.Clear();
        }

        public Entity Resolve(BufferedEntity bufferedEntity)
        {
            return bufferedEntity.Resolve(this);
        }
    }
}