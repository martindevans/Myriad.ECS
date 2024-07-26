using Myriad.ECS.Allocations;
using Myriad.ECS.Worlds;

namespace Myriad.ECS.Command;

public sealed partial class CommandBuffer
{
    /// <summary>
    /// Provides a way to resolve created entities. Must be disposed once finished with!
    /// </summary>
    public sealed class Resolver
        : IDisposable
    {
        internal SortedList<uint, Entity> Lookup { get; } = [];
        internal CommandBuffer? Parent { get; private set; }
        private uint _version;

        public int Count => Lookup.Count;

        public World World => Parent!.World;

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

            Pool.Return(this);
        }

        public Entity Resolve(BufferedEntity bufferedEntity)
        {
            if (_version != bufferedEntity.Version)
                throw new InvalidOperationException("Cannot use a resolver from one CommandBuffer with BufferedEntity from another generation of the same buffer");

            return bufferedEntity.Resolve(this);
        }

        public Entity this[int index] => Lookup.Values[index];

        public Entity this[BufferedEntity entity] => Resolve(entity);
    }
}