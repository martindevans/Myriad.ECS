using Myriad.ECS.Command;
using Myriad.ECS.Systems;
using Myriad.ECS.Worlds;

namespace Myriad.ECS.Tests.Systems;

[TestClass]
public class SystemStateTests
{
    [TestMethod]
    public void AttachState()
    {
        var world = new WorldBuilder().Build();
        var buffer = new CommandBuffer(world);

        var e0 = buffer.Create().Set(new Component0());
        var e1 = buffer.Create().Set(new Component1());
        var e2 = buffer.Create().Set(new Component2()).Set(new Component17());
        using var resolver = buffer.Playback();

        // Auto attach C17 to an entities with C0
        // Auto detach C17 from an entities without C0
        var buffer2 = new CommandBuffer(world);
        var state = new FactorySystemState<Component0, Component17>(world, _ => new Component17());
        state.Update(buffer2);
        buffer2.Playback().Dispose();

        // Check it was attached to e0
        Assert.IsTrue(e0.Resolve().HasComponent<Component0>());
        Assert.IsTrue(e0.Resolve().HasComponent<Component17>());

        // Check e1 was untouched
        Assert.IsTrue(e1.Resolve().HasComponent<Component1>());
        Assert.IsFalse(e1.Resolve().HasComponent<Component17>());

        // Check it was removed from e2
        Assert.IsTrue(e2.Resolve().HasComponent<Component2>());
        Assert.IsFalse(e2.Resolve().HasComponent<Component17>());
    }
}