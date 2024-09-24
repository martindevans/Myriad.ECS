using Myriad.ECS.Command;
using Myriad.ECS.Queries;
using Myriad.ECS.Worlds;

namespace Myriad.ECS.Tests.Queries;

[TestClass]
public class StructQueryTests
{
    [TestMethod]
    public void MatchesWithManagedObject()
    {
        var w = new WorldBuilder().Build();

        var cb = new CommandBuffer(w);
        cb.Create().Set(new ComponentInt64(1));
        cb.Create().Set(new ComponentFloat(2));
        cb.Create().Set(new ComponentObject(new object()));
        cb.Playback().Dispose();

        var l = new List<Entity>();
        w.Execute<PutEntitiesInList1<ComponentObject>, ComponentObject>(new PutEntitiesInList1<ComponentObject>(l));

        Assert.AreEqual(1, l.Count);
    }

    [TestMethod]
    public void Query0MatchesEntities()
    {
        var w = new WorldBuilder().Build();

        var cb = new CommandBuffer(w);
        cb.Create().Set(new ComponentInt64(1));
        cb.Create().Set(new ComponentFloat(2));
        cb.Create().Set(new ComponentObject(new object()));
        cb.Playback().Dispose();

        var q = new QueryBuilder().Include<ComponentObject>().Build(w);

        var l = new List<Entity>();
        w.ExecuteChunk(new PutEntitiesInList0(l), q);

        Assert.AreEqual(1, l.Count);
    }

    private readonly struct PutEntitiesInList0(List<Entity> entities)
        : IChunkQuery
    {
        public readonly List<Entity> Entities = entities;

        public void Execute(ChunkHandle chunk, ReadOnlySpan<Entity> e)
        {
            Entities.AddRange(e);

            Assert.AreEqual(e.Length, chunk.Entities.Length);
            Assert.AreEqual(e.Length, chunk.EntityCount);
        }
    }

    private readonly struct PutEntitiesInList1<T>(List<Entity> entities)
        : IQuery<T>
        where T : IComponent
    {
        public readonly List<Entity> Entities = entities;

        public void Execute(Entity e, ref T t0)
        {
            Entities.Add(e);
        }
    }
}