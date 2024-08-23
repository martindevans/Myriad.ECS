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

        public uint Version { get; private set; }
        public int Count => Lookup.Count;
        public World World => Parent!.World;

        internal void Configure(CommandBuffer buffer)
        {
            Lookup.Clear();
            Parent = buffer;
            Version = buffer._version;
        }

        public void Dispose()
        {
            if (Parent == null)
                throw new ObjectDisposedException(nameof(Resolver));

            Parent = null;
            Lookup.Clear();

            Pool.Return(this);
        }

        public Entity this[int index] => Lookup.Values[index];
    }
}