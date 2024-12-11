using Myriad.ECS.Components;

namespace Myriad.ECS.Command;

public sealed partial class CommandBuffer
{
    /// <summary>
    /// An entity that has been created in a command buffer, but not yet created. Can be used to add components.
    /// </summary>
    public readonly record struct BufferedEntity
    {
        private readonly uint _id;
        private readonly uint _version;

        internal readonly CommandBuffer _buffer;
        private readonly Resolver _resolver;

        /// <summary>
        /// Get the <see cref="CommandBuffer"/> which this <see cref="BufferedEntity"/> is from.
        /// </summary>
        public CommandBuffer CommandBuffer
        {
            get
            {
                CheckIsMutable();
                return _buffer;
            }
        }

        internal BufferedEntity(uint id, CommandBuffer buffer, Resolver resolver)
        {
            _id = id;
            _buffer = buffer;
            _resolver = resolver;

            _version = buffer._version;
        }

        private void CheckIsMutable()
        {
            if (_version != _buffer._version)
                throw new InvalidOperationException("Cannot use `BufferedEntity` after `CommandBuffer` has been played");
        }

        /// <summary>
        /// Add a component to this entity
        /// </summary>
        /// <typeparam name="T">The type of component to add</typeparam>
        /// <param name="value">The value of the component to add</param>
        /// <param name="duplicateMode">Indicates how duplicates sets of this component for this entity in this buffer should be handled</param>
        /// <returns>this buffered entity</returns>
        public BufferedEntity Set<T>(T value, DuplicateSet duplicateMode = DuplicateSet.Throw)
            where T : IComponent
        {
            CheckIsMutable();
            _buffer.SetBuffered(_id, value, duplicateMode);
            return this;
        }

        /// <summary>
        /// Add a relational component to this entity
        /// </summary>
        /// <typeparam name="T">The type of component to add</typeparam>
        /// <param name="value">The value of the component to add</param>
        /// <param name="relation">When the command buffer is played back the target entity will automatically be resolved and set into the relational component</param>
        /// <param name="duplicateMode">Indicates how duplicates sets of this component for this entity in this buffer should be handled</param>
        /// <returns>this buffered entity</returns>
        public BufferedEntity Set<T>(T value, BufferedEntity relation, DuplicateSet duplicateMode = DuplicateSet.Throw)
            where T : IEntityRelationComponent
        {
            CheckIsMutable();
            _buffer.SetBuffered(_id, value, relation, duplicateMode);
            return this;
        }

        /// <summary>
        /// Add a relational component to this entity
        /// </summary>
        /// <typeparam name="T">The type of component to add</typeparam>
        /// <param name="value">The value of the component to add</param>
        /// <param name="relation"></param>
        /// <param name="duplicateMode">Indicates how duplicates sets of this component for this entity in this buffer should be handled</param>
        /// <returns>this buffered entity</returns>
        public BufferedEntity Set<T>(T value, Entity relation, DuplicateSet duplicateMode = DuplicateSet.Throw)
            where T : IEntityRelationComponent
        {
            CheckIsMutable();
            value.Target = relation;
            _buffer.SetBuffered(_id, value, duplicateMode);
            return this;
        }

        /// <summary>
        /// Resolve this buffered Entity into the real Entity that was constructed
        /// </summary>
        /// <returns></returns>
        public Entity Resolve()
        {
            if (_resolver.Parent == null)
                throw new ObjectDisposedException("Resolver has already been disposed");
            if (_resolver.Parent != _buffer)
                throw new InvalidOperationException("Cannot use a resolver from one CommandBuffer with BufferedEntity from another");
            if (_resolver.Version != _version)
                throw new ObjectDisposedException("Resolver has already been disposed");

            return _resolver.Lookup[_id].ToEntity(_resolver.World);
        }
    }
}