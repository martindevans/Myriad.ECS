using Myriad.ECS.Command;
using Myriad.ECS.Components;
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

    [TestMethod]
    public void ExecuteWithQueryDescription()
    {
        var w = new WorldBuilder().Build();
        TestHelpers.SetupRandomEntities(w, count: 10_000).Playback().Dispose();

        w.Execute<SetComponentInt32, ComponentInt32>();

        // Check all have been set
        foreach (var (_, i) in w.Query<ComponentInt32>())
        {
            Assert.AreEqual(42, i.Ref.Value);
        }
    }

    [TestMethod]
    public void ExecuteWithRefQueryDescription()
    {
        var w = new WorldBuilder().Build();
        TestHelpers.SetupRandomEntities(w, count: 10_000).Playback().Dispose();

        var q = default(QueryDescription);
        w.Execute<SetComponentInt32, ComponentInt32>(ref q);

        // Check all have been set
        foreach (var (_, i) in w.Query<ComponentInt32>())
            Assert.AreEqual(42, i.Ref.Value);

        // Check the returned query is correct
        Assert.IsNotNull(q);
        Assert.IsTrue(q.IsIncluded<ComponentInt32>());
        Assert.AreEqual(1, q.Include.Count);
        Assert.IsTrue(q.IsExcluded<Phantom>());
        Assert.AreEqual(1, q.Exclude.Count);
        Assert.AreEqual(0, q.ExactlyOneOf.Count);
        Assert.AreEqual(0, q.AtLeastOneOf.Count);
    }

    private readonly struct PutEntitiesInList0(List<Entity> entities)
        : IChunkQuery
    {
        public readonly List<Entity> Entities = entities;

        public void Execute(ChunkHandle chunk, ReadOnlySpan<Entity> e)
        {
            Entities.AddRange(e);

            Assert.IsNotNull(chunk.Archetype);
            Assert.AreEqual(e.Length, chunk.Entities.Length);
            Assert.AreEqual(e.Length, chunk.EntityCount);

            Assert.IsFalse(chunk.HasComponent<ComponentInt64>());
            Assert.IsFalse(chunk.HasComponent<ComponentFloat>());
            Assert.IsFalse(chunk.HasComponent<ComponentInt32>());

            Assert.IsFalse(chunk.HasComponent<ComponentInt64, ComponentFloat>());
            Assert.IsFalse(chunk.HasComponent<ComponentInt64, ComponentFloat, ComponentInt32>());

            Assert.IsTrue(chunk.HasComponent<ComponentObject>());

            Assert.AreEqual(1, chunk.GetComponentSpan<ComponentObject>().Length);
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

    private readonly struct SetComponentInt32
        : IQuery<ComponentInt32>
    {
        public void Execute(Entity e, ref ComponentInt32 t0)
        {
            t0.Value = 42;
        }
    }
}