using Myriad.ECS.Command;
using Myriad.ECS.Queries;
using Myriad.ECS.Worlds;

namespace Myriad.ECS.Tests.Queries;

[TestClass]
public class EnumerableQueryTests
{
    [TestMethod]
    public void MatchNothing()
    {
        var w = new WorldBuilder().Build();

        var cmd = new CommandBuffer(w);
        cmd.Create().Set(new Component0());
        cmd.Playback().Dispose();

        var q = new QueryBuilder().Include<Component1>().Build(w);

        // Enumerate entities that don't exist
        foreach (var _ in w.Query(q))
            Assert.Fail("Shouldn't find anything");
    }

    [TestMethod]
    public void MatchNothingWithOneComponent()
    {
        var w = new WorldBuilder().Build();

        var cmd = new CommandBuffer(w);
        cmd.Create().Set(new Component0());
        cmd.Playback().Dispose();

        // Enumerate entities that don't exist
        foreach (var _ in w.Query<Component1>())
            Assert.Fail("Shouldn't find anything");
    }
}