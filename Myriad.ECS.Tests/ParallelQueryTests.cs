using Myriad.ECS.Command;
using Myriad.ECS.Worlds;

namespace Myriad.ECS.Tests;

[TestClass]
public class ParallelQueryTests
{
    [TestMethod]
    public void IncrementValues()
    {
        var w = new WorldBuilder().Build();
        var b = new CommandBuffer(w);

        // Create some random entities
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
}