using Myriad.ECS.Queries;
using Myriad.ECS.Worlds;

namespace Myriad.ECS.Tests.Queries;

[TestClass]
public class ParallelQueryTests
{
    private static void SetupRandomEntities(World world, int count = 1_000_000)
    {
        TestHelpers.SetupRandomEntities(world, 5, count).Playback().Dispose();
    }

    [TestMethod]
    public void IncrementValues()
    {
        var w = new WorldBuilder().Build();
        SetupRandomEntities(w, count:100_000);

        // Increment just the int32s
        for (var i = 0; i < 128; i++)
        {
            w.QueryParallel((ref ComponentInt32 i) =>
            {
                i.Value++;
            });
        }

        // check they're 128
        foreach (var (_, v) in w.Query<ComponentInt32>())
            Assert.AreEqual(128, v.Ref.Value);

        // check they're 128 in a different way
        foreach (var item in w.Query<ComponentInt32>())
            Assert.AreEqual(128, item.Item0.Value);

        // check they're 128 in a different way
        w.Query((Entity _, ref ComponentInt32 c) => Assert.AreEqual(128, c.Value));
        w.QueryParallel((Entity _, ref ComponentInt32 c) => Assert.AreEqual(128, c.Value));

        // check they're 128 in a different way
        w.Query(128, (int data, Entity _, ref ComponentInt32 c) => Assert.AreEqual(data, c.Value));
        w.QueryParallel(128, (int data, Entity _, ref ComponentInt32 c) => Assert.AreEqual(data, c.Value));

        // check they're 128 in a different way
        w.Query(128, (int data, ref ComponentInt32 c) => Assert.AreEqual(data, c.Value));
        w.QueryParallel(128, (int data, ref ComponentInt32 c) => Assert.AreEqual(data, c.Value));

        // Check everything else is 0
        foreach (var (_, v) in w.Query<ComponentByte>())
            Assert.AreEqual(0, v.Ref.Value);
        foreach (var (_, v) in w.Query<ComponentInt16>())
            Assert.AreEqual(0, v.Ref.Value);
        foreach (var (_, v) in w.Query<ComponentFloat>())
            Assert.AreEqual(0, v.Ref.Value);
        foreach (var (_, v) in w.Query<ComponentInt64>())
            Assert.AreEqual(0, v.Ref.Value);
    }

    [TestMethod]
    public void ChunkIncrementValues()
    {
        var w = new WorldBuilder().Build();
        SetupRandomEntities(w, count:150_000);

        // Increment just the int32s
        for (var i = 0; i < 128; i++)
            w.ExecuteChunkParallel<IncrementInt, ComponentInt32>(new IncrementInt());

        // check they're 128
        foreach (var (_, v) in w.Query<ComponentInt32>())
            Assert.AreEqual(128, v.Ref.Value);

        // check they're 128 in a different way
        foreach (var e in w.Query(new QueryBuilder().Include<ComponentInt32>().Build(w)))
            Assert.AreEqual(128, e.GetComponentRef<ComponentInt32>().Value);

        // Check everything else is 0
        foreach (var (_, v) in w.Query<ComponentByte>())
            Assert.AreEqual(0, v.Ref.Value);
        foreach (var (_, v) in w.Query<ComponentInt16>())
            Assert.AreEqual(0, v.Ref.Value);
        foreach (var (_, v) in w.Query<ComponentFloat>())
            Assert.AreEqual(0, v.Ref.Value);
        foreach (var (_, v) in w.Query<ComponentInt64>())
            Assert.AreEqual(0, v.Ref.Value);
    }

    [TestMethod]
    public void ChunkExceptions()
    {
        var w = new WorldBuilder().Build();
        SetupRandomEntities(w, 10_000);

        // Increment just the int32s
        Assert.ThrowsException<AggregateException>(() =>
        {
            w.ExecuteChunkParallel<SometimesThrowOtherwiseIncrement, ComponentInt32>(new SometimesThrowOtherwiseIncrement());
        });
    }

    [TestMethod]
    public void ParallelExceptions()
    {
        var w = new WorldBuilder().Build();
        SetupRandomEntities(w, 10_000);

        // Increment just the int32s
        Assert.ThrowsException<AggregateException>(() =>
        {
            w.ExecuteParallel<SometimesThrowOtherwiseIncrement, ComponentInt32>(new SometimesThrowOtherwiseIncrement());
        });
    }

    [TestMethod]
    public void NoWork()
    {
        var w = new WorldBuilder().Build();
        SetupRandomEntities(w, 10_000);

        w.QueryParallel((Entity _, ref ComponentInt32 c, ref Component16 x) => Assert.Fail());

        var q = new QueryBuilder().Include<ComponentInt32, Component17>().Build(w);
        Assert.AreEqual(0, w.ExecuteChunkParallel<IncrementInt, ComponentInt32>(new IncrementInt(), q));

        Assert.AreEqual(0, w.ExecuteParallel<IncrementInt, ComponentInt32>(new IncrementInt(), q));
    }

    private readonly struct IncrementInt
        : IChunkQuery<ComponentInt32>, IQuery<ComponentInt32>
    {
        public void Execute(ChunkHandle chunk, Span<ComponentInt32> t0)
        {
            for (var i = 0; i < t0.Length; i++)
                Execute(chunk.Entities.Span[i], ref t0[i]);
        }

        public void Execute(Entity e, ref ComponentInt32 t0)
        {
            t0.Value++;
        }
    }

    private readonly struct SometimesThrowOtherwiseIncrement
        : IChunkQuery<ComponentInt32>, IQuery<ComponentInt32>
    {
        public void Execute(ChunkHandle chunk, Span<ComponentInt32> t0)
        {
            for (var i = 0; i < t0.Length; i++)
            {
                if (i == 21)
                    throw new Exception("Expected");
                t0[i].Value++;
            }
        }

        public void Execute(Entity e, ref ComponentInt32 t0)
        {
            if (e.ID.ID % 3 == 0)
                throw new Exception("Expected");
            t0.Value++;
        }
    }
}
