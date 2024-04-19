using Myriad.ECS.Command;
using Myriad.ECS.Components;
using Myriad.ECS.Worlds;

namespace Myriad.ECS.Tests.Systems;

[TestClass]
public class DisposableComponentSystemTests
{
    [TestMethod]
    public void DoNotDisposeWhileAlive()
    {
        var w = new WorldBuilder().Build();

        // Create an entity with a disposable test component. boxed integer will be incrmented every time it is disposed
        var box = new BoxedInt();
        var cmd = new CommandBuffer(w);
        cmd.Create().Set(new ComponentInt32(7)).Set(new TestDisposable(box));
        cmd.Playback().Dispose();

        var sys = new DisposableComponentSystem<int, TestDisposable>(w);
        sys.Update(0);

        Assert.AreEqual(0, box.Value);
    }

    [TestMethod]
    public void DisposeWhenDead()
    {
        var w = new WorldBuilder().Build();

        // Create an entity with a disposable test component. boxed integer will be incrmented every time it is disposed
        var box = new BoxedInt();
        var cmd = new CommandBuffer(w);
        var eb = cmd.Create().Set(new ComponentInt32(7)).Set(new TestDisposable(box));
        using var resolver = cmd.Playback();
        var entity = eb.Resolve(resolver);

        cmd.Delete(entity);
        cmd.Playback().Dispose();

        Assert.IsTrue(entity.IsPhantom(w));

        var sys = new DisposableComponentSystem<int, TestDisposable>(w);
        sys.Update(0);

        Assert.AreEqual(1, box.Value);
        Assert.IsFalse(entity.IsPhantom(w));
    }

    [TestMethod]
    public void DisposeWhenDisposed()
    {
        var w = new WorldBuilder().Build();

        // Create an entity with a disposable test component. boxed integer will be incrmented every time it is disposed
        var box = new BoxedInt();
        var cmd = new CommandBuffer(w);
        var eb = cmd.Create().Set(new ComponentInt32(7)).Set(new TestDisposable(box));
        using var resolver = cmd.Playback();
        var entity = eb.Resolve(resolver);

        var sys = new DisposableComponentSystem<int, TestDisposable>(w);
        sys.Dispose();

        Assert.AreEqual(1, box.Value);
        Assert.IsFalse(entity.IsPhantom(w));
        Assert.IsFalse(entity.HasComponent<TestDisposable>(w));
    }
}