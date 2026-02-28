using Myriad.ECS.Systems;
using Myriad.ECS.Worlds;

namespace Myriad.ECS.Tests.Systems;

[TestClass]
public class CommandBufferSystems
{
    [TestMethod]
    public void EarlyBufferSystem()
    {
        var world = new WorldBuilder().Build();
        var sys = new EarlyCommandBufferSystem<object>(world);

        var list = new List<Entity>();
        var buffered = sys.Buffer.Create();
        buffered.DelayedResolve(list);

        Assert.AreEqual(0, list.Count);
        sys.BeforeUpdate(new());
        Assert.AreEqual(1, list.Count);
        sys.Update(new());
        Assert.AreEqual(1, list.Count);
    }

    [TestMethod]
    public void UpdateBufferSystem()
    {
        var world = new WorldBuilder().Build();
        var sys = new CommandBufferSystem<object>(world);

        var list = new List<Entity>();
        var buffered = sys.Buffer.Create();
        buffered.DelayedResolve(list);

        Assert.AreEqual(0, list.Count);
        sys.Update(new());
        Assert.AreEqual(1, list.Count);
    }

    [TestMethod]
    public void LateBufferSystem()
    {
        var world = new WorldBuilder().Build();
        var sys = new LateCommandBufferSystem<object>(world);

        var list = new List<Entity>();
        var buffered = sys.Buffer.Create();
        buffered.DelayedResolve(list);

        Assert.AreEqual(0, list.Count);
        sys.Update(new());
        Assert.AreEqual(0, list.Count);
        sys.AfterUpdate(new());
        Assert.AreEqual(1, list.Count);
    }
}