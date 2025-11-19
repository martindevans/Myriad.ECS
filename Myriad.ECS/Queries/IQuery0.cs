using Myriad.ECS.Queries;

namespace Myriad.ECS.Queries
{
    /// <summary>
    /// Interface for query handlers which operate over entities
    /// </summary>
    public interface IQuery
    {
        /// <summary>
        /// Execute this query handler for an entity
        /// </summary>
        /// <param name="e"></param>
        public void Execute(Entity e);
    }

    /// <summary>
    /// Interface for query handlers which operate over entire chunks of entities
    /// </summary>
    public interface IChunkQuery
    {
        /// <summary>
        /// Execute this query handler for a chunk of entities
        /// </summary>
        /// <param name="chunk"></param>
        /// <param name="e"></param>
        public void Execute(ChunkHandle chunk, ReadOnlySpan<Entity> e);
    }
}

namespace Myriad.ECS.Worlds
{
    public partial class World
    {
        /// <summary>
        /// Execute a query, selecting entities which match the given <see cref="QueryDescription"/>
        /// </summary>
        /// <typeparam name="TQ"></typeparam>
        /// <param name="q"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public int Execute<TQ>(
            TQ q,
            QueryDescription query
        )
            where TQ : IQuery
        {
            var archetypes = query.GetArchetypes();

            var count = 0;
            foreach (var archetypeMatch in archetypes)
            {
                var archetype = archetypeMatch.Archetype;
                archetype.Block();

                count += archetype.EntityCount;

                var chunks = archetype.Chunks;
                for (var c = chunks.Count - 1; c >= 0; c--)
                {
                    var chunk = chunks[c];
                    var entities = chunk.Entities.Span;

                    for (var i = entities.Length - 1; i >= 0; i--)
                        q.Execute(entities[i]);
                }
            }

            return count;
        }
    }

    public partial class World
    {
        /// <summary>
        /// Execute a query, selecting entities which match the given <see cref="QueryDescription"/>
        /// </summary>
        /// <typeparam name="TQ"></typeparam>
        /// <param name="q"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public int ExecuteChunk<TQ>(
            TQ q,
            QueryDescription query
        )
            where TQ : IChunkQuery
        {
            var archetypes = query.GetArchetypes();

            var count = 0;
            foreach (var archetypeMatch in archetypes)
            {
                var archetype = archetypeMatch.Archetype;
                archetype.Block();

                count += archetype.EntityCount;

                var chunks = archetype.Chunks;
                for (var c = chunks.Count - 1; c >= 0; c--)
                {
                    var chunk = chunks[c];
                    var entities = chunk.Entities.Span;

                    q.Execute(new ChunkHandle(chunk), entities);
                }
            }

            return count;
        }
    }
}