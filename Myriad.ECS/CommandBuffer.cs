using Myriad.ECS.Worlds;
using Myriad.ParallelTasks;

namespace Myriad.ECS;

public sealed class CommandBuffer(World World)
{
    private readonly int _version;

    public Future<Resolver> Playback()
    {
        throw new NotImplementedException();
    }

    public BufferedEntity Create()
    {
        throw new NotImplementedException();
    }

    public void Set<T>(Entity entity)
        where T : IComponent
    {
        throw new NotImplementedException();
    }

    public void Remove<T>(Entity entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(Entity entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(BufferedEntity entity)
    {
        throw new NotImplementedException();
    }

    public readonly record struct BufferedEntity
    {
        private readonly int _id;
        private readonly int _version;
        private readonly CommandBuffer _buffer;

        public BufferedEntity(int id, CommandBuffer buffer)
        {
            _id = id;
            _buffer = buffer;
            _version = buffer._version;
        }

        public BufferedEntity Delete()
        {
            throw new NotImplementedException();
        }

        public BufferedEntity Remove<T>()
        {
            throw new NotImplementedException();
        }

        public BufferedEntity Set<T>(T value)
            where T : IComponent
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Resolve this buffered Entity into the real Entity that was constructed
        /// </summary>
        /// <param name="resolver"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Entity? Resolve(Resolver resolver)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Provides a way to resolve created entities. Must be disposed before the command buffer can be used again
    /// </summary>
    public struct Resolver
        : IDisposable
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}