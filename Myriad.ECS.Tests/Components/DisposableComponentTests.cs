using Myriad.ECS.Command;
using Myriad.ECS.Worlds;

namespace Myriad.ECS.Tests.Components;

[TestClass]
public class DisposableComponentTests
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

        cmd.Playback().Dispose();

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

        // Delete entity
        cmd.Delete(entity);
        cmd.Playback().Dispose();

        // Check the entity was disposed
        Assert.AreEqual(1, box.Value);
        Assert.IsFalse(entity.IsPhantom(w));
        Assert.IsFalse(entity.IsAlive(w));
    }

    [TestMethod]
    public void CombinedPhantomDispose()
    {
        var w = new WorldBuilder().Build();

        // Create an entity with a disposable test component. boxed integer will be incrmented every time it is disposed
        var box = new BoxedInt();
        var cmd = new CommandBuffer(w);
        var eb = cmd.Create().Set(new TestDisposablePhantom(box));
        using var resolver = cmd.Playback();
        var entity = eb.Resolve(resolver);

        // Delete entity
        cmd.Delete(entity);
        cmd.Playback().Dispose();

        // Entity should be a phantom, so disposal should not yet have run
        Assert.AreEqual(0, box.Value);
        Assert.IsTrue(entity.IsPhantom(w));
        Assert.IsFalse(entity.IsAlive(w));

        // Delete it for real this time
        cmd.Delete(entity);
        cmd.Playback().Dispose();

        // Check the entity was disposed
        Assert.AreEqual(1, box.Value);
        Assert.IsFalse(entity.IsPhantom(w));
        Assert.IsFalse(entity.IsAlive(w));
    }

    [TestMethod]
    public void DisposeWhenRemoved()
    {
        var w = new WorldBuilder().Build();

        // Create an entity with a disposable test component. boxed integer will be incrmented every time it is disposed
        var box = new BoxedInt();
        var cmd = new CommandBuffer(w);
        var eb = cmd.Create().Set(new ComponentInt32(7)).Set(new TestDisposable(box));
        using var resolver = cmd.Playback();
        var entity = eb.Resolve(resolver);

        // Remove component
        cmd.Remove<TestDisposable>(entity);
        cmd.Playback().Dispose();

        // Check the component was disposed
        Assert.AreEqual(1, box.Value);
        Assert.IsFalse(entity.IsPhantom(w));
        Assert.IsTrue(entity.IsAlive(w));
    }

    [TestMethod]
    public void DisposeWhenWorldDisposed()
    {
        var w = new WorldBuilder().Build();

        // Create an entity with a disposable test component. boxed integer will be incrmented every time it is disposed
        var box = new BoxedInt();
        var cmd = new CommandBuffer(w);
        cmd.Create().Set(new ComponentInt32(7)).Set(new TestDisposable(box));
        cmd.Create().Set(new TestDisposable(box));
        cmd.Create().Set(new Component0()).Set(new TestDisposable(box));
        cmd.Playback().Dispose();

        // Destroy world
        w.Dispose();

        // Check the component was disposed 3 times
        Assert.AreEqual(3, box.Value);
    }

    [TestMethod]
    public void DisposableMakesBufferedChanges()
    {
        var w = new WorldBuilder().Build();

        // Create an entity with a disposable test component. boxed integer will be incrmented every time it is disposed
        var box = new BoxedInt();
        var cmd = new CommandBuffer(w);

        // Create two entities. Entity b is setup to delet entity a when it is disposed
        var ab = cmd.Create().Set(new TestDisposable(box));
        var bb = cmd.Create().Set(new TestDisposableParent(), ab);
        var resolver = cmd.Playback();
        var a = ab.Resolve(resolver);
        var b = bb.Resolve(resolver);
        resolver.Dispose();

        cmd.Delete(b);
        cmd.Playback().Dispose();

        Assert.IsFalse(a.IsAlive(w));
        Assert.IsFalse(b.IsAlive(w));
        Assert.AreEqual(1, box.Value);
    }
}