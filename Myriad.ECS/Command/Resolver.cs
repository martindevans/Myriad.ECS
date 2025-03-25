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
        internal SortedList<uint, EntityId> Lookup { get; } = [];
        internal CommandBuffer? Parent { get; private set; }

        internal uint Version { get; private set; }

        /// <summary>
        /// Get the number of entities in this <see cref="Resolver"/>
        /// </summary>
        public int Count => Lookup.Count;

        /// <summary>
        /// The <see cref="World"/> this resolver is for.
        /// </summary>
        public World World => Parent!.World;

        internal void Configure(CommandBuffer buffer)
        {
            Lookup.Clear();
            Parent = buffer;
            Version = buffer._version;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (Parent == null)
                throw new ObjectDisposedException(nameof(Resolver));

            unchecked
            {
                Version--;
            }

            Parent = null;
            Lookup.Clear();

            Pool.Return(this);
        }

        /// <summary>
        /// Get the nth item in this <see cref="Resolver"/>. Items are an arbitrary order.
        /// </summary>
        /// <param name="index"></param>
        public Entity this[int index] => Lookup.Values[index].ToEntity(World);
    }
}