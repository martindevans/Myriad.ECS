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
        var resolver = buffer.Playback();

        // Auto attach C17 to an entities with C0
        // Auto detach C17 from an entities without C0
        var buffer2 = new CommandBuffer(world);
        var state = new SystemState<Component0, Component17>(world, _ => new Component17());
        state.Update(buffer2);
        buffer2.Playback().Dispose();

        // Check it was attached to e0
        Assert.IsTrue(resolver[e0].HasComponent<Component0>(world));
        Assert.IsTrue(resolver[e0].HasComponent<Component17>(world));

        // Check e1 was untouched
        Assert.IsTrue(resolver[e1].HasComponent<Component1>(world));
        Assert.IsFalse(resolver[e1].HasComponent<Component17>(world));

        // Check it was removed from e2
        Assert.IsTrue(resolver[e2].HasComponent<Component2>(world));
        Assert.IsFalse(resolver[e2].HasComponent<Component17>(world));
    }
}