using Myriad.ECS.Command;
using Myriad.ECS.Queries;
using Myriad.ECS.Worlds;

namespace Myriad.ECS.Tests.Queries;

[TestClass]
public class ParallelQueryTests
{
    private void SetupRandomEntities(World world)
    {
        var b = new CommandBuffer(world);
        var r = new Random(3452);
        for (var i = 0; i < 1_000_000; i++)
        {
            var eb = b.Create();

            for (var j = 0; j < 5; j++)
            {
                switch (r.Next(0, 5))
                {
                    case 0: eb.Set(new ComponentByte(0), true); break;
                    case 1: eb.Set(new ComponentInt16(0), true); break;
                    case 2: eb.Set(new ComponentFloat(0), true); break;
                    case 3: eb.Set(new ComponentInt32(0), true); break;
                    case 4: eb.Set(new ComponentInt64(0), true); break;
                }
            }
        }

        b.Playback().Dispose();
    }

    [TestMethod]
    public void IncrementValues()
    {
        var w = new WorldBuilder().Build();
        SetupRandomEntities(w);

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
        SetupRandomEntities(w);

        // Increment just the int32s
        for (var i = 0; i < 128; i++)
            w.ExecuteChunkParallel<IncrementInt, ComponentInt32>(new IncrementInt());

        // check they're 128
        foreach (var (_, v) in w.Query<ComponentInt32>())
            Assert.AreEqual(128, v.Ref.Value);

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
        SetupRandomEntities(w);

        // Increment just the int32s
        Assert.ThrowsException<AggregateException>(() =>
        {
            w.ExecuteChunkParallel<SometimesThrowOtherwiseIncrement, ComponentInt32>(new SometimesThrowOtherwiseIncrement());
        });
    }

    private readonly struct IncrementInt
        : IChunkQuery<ComponentInt32>
    {
        public void Execute(ChunkHandle chunk, ReadOnlySpan<Entity> e, Span<ComponentInt32> t0)
        {
            for (var i = 0; i < t0.Length; i++)
                t0[i].Value++;
        }
    }

    private readonly struct SometimesThrowOtherwiseIncrement
        : IChunkQuery<ComponentInt32>
    {
        public void Execute(ChunkHandle chunk, ReadOnlySpan<Entity> e, Span<ComponentInt32> t0)
        {
            for (var i = 0; i < t0.Length; i++)
            {
                if (i == 21)
                    throw new Exception("Expected");
                t0[i].Value++;
            }
        }
    }
}
