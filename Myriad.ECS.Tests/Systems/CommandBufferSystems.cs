using Myriad.ECS.Command;
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

        var sub = new TestSubscriber();
        sys.Subscribe(sub);

        var list = new List<Entity>();
        var buffered = sys.Buffer.Create();
        buffered.DelayedResolve(list);
        sub.Expect(buffered);

        Assert.AreEqual(0, list.Count);
        sys.Update(new());
        Assert.AreEqual(1, list.Count);

        sys.Unsubscribe(sub);
        sub.AllowCallback(false);

        sys.Buffer.Create();
        sys.Update(new());
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

    private class TestSubscriber
        : ICommandBufferSystemSubscriber
    {
        private readonly List<CommandBuffer.BufferedEntity> _expected = [ ];
        private bool _allowed = true;

        public void Expect(CommandBuffer.BufferedEntity buffered)
        {
            _expected.Add(buffered);
        }

        public void AllowCallback(bool allow = true)
        {
            _allowed = allow;
        }

        public void OnCommandbufferPlayback(CommandBuffer.Resolver resolver)
        {
            if (!_allowed)
                Assert.Fail("Callback not allowed");

            foreach (var bufferedEntity in _expected)
            {
                var entity = bufferedEntity.Resolve();
                Assert.IsTrue(entity.IsAlive());
            }
        }
    }
}