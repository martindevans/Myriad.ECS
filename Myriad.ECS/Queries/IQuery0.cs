using Myriad.ECS.Queries;

namespace Myriad.ECS.Queries
{
    public interface IQuery
    {
        public void Execute(Entity e);
    }

    public interface IChunkQuery
    {
        public void Execute(ChunkHandle chunk, ReadOnlySpan<Entity> e);
    }
}

namespace Myriad.ECS.Worlds
{
    public partial class World
    {
        public int Execute<TQ>(
            TQ q,
            QueryDescription query
        )
            where TQ : IQuery
        {
            var archetypes = query.GetArchetypes();
            if (archetypes.Count == 0)
                return 0;

            var count = 0;
            foreach (var archetypeMatch in archetypes)
            {
                var archetype = archetypeMatch.Archetype;
                if (archetype.EntityCount == 0)
                    continue;

                count += archetype.EntityCount;

                var chunks = archetype.Chunks;
                for (var c = chunks.Count - 1; c >= 0; c--)
                {
                    var chunk = chunks[c];

                    var entities = chunk.Entities.Span;
                    if (entities.Length == 0)
                        continue;

                    for (var i = entities.Length - 1; i >= 0; i--)
                        q.Execute(entities[i]);
                }
            }

            return count;
        }
    }

    public partial class World
    {
        public int ExecuteChunk<TQ>(
            TQ q,
            QueryDescription query
        )
            where TQ : IChunkQuery
        {
            var archetypes = query.GetArchetypes();
            if (archetypes.Count == 0)
                return 0;

            var count = 0;
            foreach (var archetypeMatch in archetypes)
            {
                var archetype = archetypeMatch.Archetype;
                if (archetype.EntityCount == 0)
                    continue;

                count += archetype.EntityCount;

                var chunks = archetype.Chunks;
                for (var c = chunks.Count - 1; c >= 0; c--)
                {
                    var chunk = chunks[c];

                    var entities = chunk.Entities.Span;
                    if (entities.Length == 0)
                        continue;

                    q.Execute(new ChunkHandle(chunk), entities);
                }
            }

            return count;
        }
    }
}