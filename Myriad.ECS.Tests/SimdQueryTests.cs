using System.Numerics;
using Myriad.ECS.Command;
using Myriad.ECS.Queries;
using Myriad.ECS.Worlds;

namespace Myriad.ECS.Tests;

[TestClass]
public class SimdQueryTests
{
    [TestMethod]
    public void AddItems()
    {
        var w = new WorldBuilder()
               .WithArchetype<ComponentInt32>()
               .Build();

        var cmd = new CommandBuffer(w);
        for (var i = 0; i < 23; i++)
            cmd.Create().Set(new ComponentInt32(i)).Set(new ComponentInt64(i));
        cmd.Playback().Dispose();

        w.ExecuteVectorChunk<AddInts, ComponentInt32, int>(new AddInts());

        foreach (var (_, i32, i64) in w.Query<ComponentInt32, ComponentInt64>())
            Assert.AreEqual(i64.Ref.Value * 2, i32.Ref.Value);
    }

    private struct AddInts
        : IVectorChunkQuery1<int>
    {
        public readonly void Execute(Span<Vector<int>> t0, int pad)
        {
            for (var i = 0; i < t0.Length; i++)
                t0[i] += t0[i];
        }
    }
}