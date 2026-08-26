using Myriad.ECS.Allocations;
using Myriad.ECS.Collections;
using Myriad.ECS.Command;
using Myriad.ECS.Queries;
using System.Runtime.CompilerServices;

namespace Myriad.ECS.Queries
{
    /// <summary>
    /// A cross join on two chunks
    /// </summary>
    public interface IChunkJoinQuery
    {
        /// <summary>
        /// Apply a cross join to two chunks of entities
        /// </summary>
        public void Execute(ChunkHandle left, ChunkHandle right);
    }
}

namespace Myriad.ECS.Worlds
{
    public partial class World
    {
        /// <summary>
        /// Execute a query which does a cross join on entire chunks.
        /// </summary>
        /// <param name="left">Specification for "left" chunks</param>
        /// <param name="right">Specification for "right" chunks</param>
        /// <returns>The total number of entities processed (product of the count of both chunks)</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long ExecuteChunkJoin<TQ>(QueryDescription left, QueryDescription right)
            where TQ : struct, IChunkJoinQuery
        {
            var q = default(TQ);
            return ExecuteChunkJoin(ref q, left, right);
        }

        /// <summary>
        /// Execute a query which does a cross join on entire chunks.
        /// </summary>
        /// <param name="q"></param>
        /// <param name="left">Specification for "left" chunks</param>
        /// <param name="right">Specification for "right" chunks</param>
        /// <returns>The total number of entities processed (product of the count of both chunks)</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long ExecuteChunkJoin<TQ>(TQ q, QueryDescription left, QueryDescription right)
            where TQ :  IChunkJoinQuery
        {
            return ExecuteChunkJoin(ref q, left, right);
        }

        /// <summary>
        /// Execute a query which does a cross join on entire chunks.
        /// </summary>
        /// <param name="q"></param>
        /// <param name="left">Specification for "left" chunks</param>
        /// <param name="right">Specification for "right" chunks</param>
        /// <returns>The total number of entities processed (product of the count of both chunks)</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long ExecuteChunkJoin<TQ>(ref TQ q, QueryDescription left, QueryDescription right)
            where TQ : IChunkJoinQuery
        {
            if (left.World != this)
                throw new ArgumentException("Join query query must be from this world", nameof(left));
            if (right.World != this)
                throw new ArgumentException("Join query query must be from this world", nameof(right));

            // Early out to skip blocking if possible
            if (!left.Any() || !right.Any())
                return 0;

            // Get all matched archetypes
            var leftArchetypes = left.GetArchetypes();
            var rightArchetypes = right.GetArchetypes();

            // Block on all archetypes involved so we don't have to later
            using var rentalSet = Pool<OrderedListSet<long>>.Rent();
            rentalSet.Value.Clear();
            var blocker = new Blocker(left.World, rentalSet.Value);
            foreach (var item in leftArchetypes)
                blocker.Block(item.Archetype);
            foreach (var item in rightArchetypes)
                blocker.Block(item.Archetype);

            // Do the actual cross join
            var count = 0L;
            foreach (var leftMatch in leftArchetypes)
            {
                var leftArchetype = leftMatch.Archetype;
                if (leftArchetype.EntityCount == 0)
                    continue;

                foreach (var rightMatch in rightArchetypes)
                {
                    var rightArchetype = rightMatch.Archetype;
                    if (rightArchetype.EntityCount == 0)
                        continue;

                    using var leftChunks = leftArchetype.GetChunkEnumerator();
                    while (leftChunks.MoveNext())
                    {
                        var leftChunk = leftChunks.Current;

                        using var rightChunks = rightArchetype.GetChunkEnumerator();
                        while (rightChunks.MoveNext())
                        {
                            var rightChunk = rightChunks.Current;
                            q.Execute(new ChunkHandle(leftChunk), new ChunkHandle(rightChunk));

                            count += leftChunk.EntityCount * rightChunk.EntityCount;
                        }
                    }
                    
                }
            }

            return count;
        }
    }
}