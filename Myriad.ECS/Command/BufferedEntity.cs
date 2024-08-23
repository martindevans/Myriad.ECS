﻿using Myriad.ECS.Components;

namespace Myriad.ECS.Command;

public sealed partial class CommandBuffer
{
    /// <summary>
    /// An entity that has been created in a command buffer, but not yet created. Can be used to add components.
    /// </summary>
    public readonly record struct BufferedEntity
    {
        private readonly uint _id;
        internal readonly uint Version;
        internal readonly CommandBuffer _buffer;
        private readonly Resolver _resolver;

        public BufferedEntity(uint id, CommandBuffer buffer, Resolver resolver)
        {
            _id = id;
            _buffer = buffer;
            _resolver = resolver;

            Version = buffer._version;
        }

        private void CheckIsMutable()
        {
            if (Version != _buffer._version)
                throw new InvalidOperationException("Cannot use `BufferedEntity` after `CommandBuffer` has been played");
        }

        /// <summary>
        /// Add a component to this entity
        /// </summary>
        /// <typeparam name="T">The type of component to add</typeparam>
        /// <param name="value">The value of the component to add</param>
        /// <param name="overwrite">If this component has already been added to this entity it will either be overwritten or an exception will be thrown, depending on this parameter</param>
        /// <returns>this buffered entity</returns>
        public BufferedEntity Set<T>(T value, bool overwrite = false)
            where T : IComponent
        {
            CheckIsMutable();
            _buffer.SetBuffered(_id, value, overwrite);
            return this;
        }

        /// <summary>
        /// Add a relational component to this entity
        /// </summary>
        /// <typeparam name="T">The type of component to add</typeparam>
        /// <param name="value">The value of the component to add</param>
        /// <param name="relation">When the command buffer is played back the target entity will automatically be resolved and set into the relational component</param>
        /// <param name="overwrite">If this component has already been added to this entity it will either be overwritten or an exception will be thrown, depending on this parameter</param>
        /// <returns>this buffered entity</returns>
        public BufferedEntity Set<T>(T value, BufferedEntity relation, bool overwrite = false)
            where T : IEntityRelationComponent
        {
            CheckIsMutable();
            _buffer.SetBuffered(_id, value, relation, overwrite);
            return this;
        }

        /// <summary>
        /// Add a relational component to this entity
        /// </summary>
        /// <typeparam name="T">The type of component to add</typeparam>
        /// <param name="value">The value of the component to add</param>
        /// <param name="relation"></param>
        /// <param name="overwrite">If this component has already been added to this entity it will either be overwritten or an exception will be thrown, depending on this parameter</param>
        /// <returns>this buffered entity</returns>
        public BufferedEntity Set<T>(T value, Entity relation, bool overwrite = false)
            where T : IEntityRelationComponent
        {
            CheckIsMutable();
            value.Target = relation;
            _buffer.SetBuffered(_id, value, overwrite);
            return this;
        }

        /// <summary>
        /// Resolve this buffered Entity into the real Entity that was constructed
        /// </summary>
        /// <param name="resolver"></param>
        /// <returns></returns>
        public Entity Resolve()
        {
            if (_resolver.Parent == null)
                throw new ObjectDisposedException("Resolver has already been disposed");
            if (_resolver.Parent != _buffer)
                throw new InvalidOperationException("Cannot use a resolver from one CommandBuffer with BufferedEntity from another");
            if (_resolver.Version != Version)
                throw new ObjectDisposedException("Resolver has already been disposed");

            return _resolver.Lookup[_id];
        }
    }
}