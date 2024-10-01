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
        var entity = eb.Resolve();

        // Delete entity
        cmd.Delete(entity);
        cmd.Playback().Dispose();

        // Check the entity was disposed
        Assert.AreEqual(1, box.Value);
        Assert.IsFalse(entity.IsPhantom());
        Assert.IsFalse(entity.IsAlive());
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
        var entity = eb.Resolve();

        // Delete entity
        cmd.Delete(entity);
        cmd.Playback().Dispose();

        // Entity should be a phantom, so disposal should not yet have run
        Assert.AreEqual(0, box.Value);
        Assert.IsTrue(entity.IsPhantom());
        Assert.IsFalse(entity.IsAlive());

        // Delete it for real this time
        cmd.Delete(entity);
        cmd.Playback().Dispose();

        // Check the entity was disposed
        Assert.AreEqual(1, box.Value);
        Assert.IsFalse(entity.IsPhantom());
        Assert.IsFalse(entity.IsAlive());
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
        var entity = eb.Resolve();

        // Remove component
        cmd.Remove<TestDisposable>(entity);
        cmd.Playback().Dispose();

        // Check the component was disposed
        Assert.AreEqual(1, box.Value);
        Assert.IsFalse(entity.IsPhantom());
        Assert.IsTrue(entity.IsAlive());
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
        var a = ab.Resolve();
        var b = bb.Resolve();
        resolver.Dispose();

        cmd.Delete(b);
        cmd.Playback().Dispose();

        Assert.IsFalse(a.IsAlive());
        Assert.IsFalse(b.IsAlive());
        Assert.AreEqual(1, box.Value);
    }

    [TestMethod]
    public void LeakFromCommandBufferSetDeleteConflict()
    {
        var w = new WorldBuilder().Build();

        // boxed int will be incremented when dispose happens
        var box = new BoxedInt();

        var cmd = new CommandBuffer(w);

        // Create an entity
        var be = cmd.Create();
        var resolver1 = cmd.Playback();
        var entity = be.Resolve();
        resolver1.Dispose();

        // Delete the entity
        cmd.Delete(entity);

        // Enqueue attachment of a disposal component... but that entity is already going to be deleted!
        cmd.Set(entity, new TestDisposable(box));

        // Nothing has been disposed yet
        Assert.AreEqual(0, box.Value);

        // Execute the buffer
        cmd.Playback().Dispose();

        // Ensure the component was disposed, even though it wasn't ever really even applied!
        Assert.AreEqual(1, box.Value);
    }

    [TestMethod]
    public void LeakFromCommandBufferSetDoubleDeleteConflict()
    {
        var w = new WorldBuilder().Build();

        // boxed int will be incremented when dispose happens
        var box = new BoxedInt();

        var cmd = new CommandBuffer(w);

        // Create an entity
        var be = cmd.Create();
        var resolver1 = cmd.Playback();
        var entity = be.Resolve();
        resolver1.Dispose();

        // Enqueue deletion of entity in one command buffer
        cmd.Delete(entity);

        // Setup a disposable in another buffer
        var cmd2 = new CommandBuffer(w);
        cmd2.Set(entity, new TestDisposable(box));

        // Delete entity
        cmd.Playback().Dispose();

        // Nothing has been disposed yet
        Assert.AreEqual(0, box.Value);

        // Execute the second buffer
        cmd2.Playback().Dispose();

        // Ensure the component was disposed, even though it wasn't ever really even applied!
        Assert.AreEqual(1, box.Value);
    }

    [TestMethod]
    public void LeakFromCommandBufferSetSetConflict()
    {
        var w = new WorldBuilder().Build();

        // boxed int will be incremented when dispose happens
        var box1 = new BoxedInt();
        var box2 = new BoxedInt();

        var cmd = new CommandBuffer(w);

        // Create an entity
        var be = cmd.Create();
        var resolver1 = cmd.Playback();
        var entity = be.Resolve();
        resolver1.Dispose();

        // Enqueue attachment of a disposal component, twice
        cmd.Set(entity, new TestDisposable(box1));
        cmd.Set(entity, new TestDisposable(box2));

        // Nothing has been disposed yet
        Assert.AreEqual(0, box1.Value);
        Assert.AreEqual(0, box2.Value);

        // Execute the buffer
        cmd.Playback().Dispose();

        // Ensure the first one was disposed
        Assert.AreEqual(1, box1.Value);
        Assert.AreEqual(0, box2.Value);
    }

    [TestMethod]
    public void LeakFromCommandBufferBufferedSetSetConflict()
    {
        var w = new WorldBuilder().Build();

        // boxed int will be incremented when dispose happens
        var box1 = new BoxedInt();
        var box2 = new BoxedInt();

        var cmd = new CommandBuffer(w);

        // Create an entity
        var be = cmd.Create();

        // Enqueue attachment of a disposal component, twice. To an entity that doesn't exist yet.
        be.Set(new TestDisposable(box1));
        be.Set(new TestDisposable(box2), overwrite:true);

        // Nothing has been disposed yet
        Assert.AreEqual(0, box1.Value);
        Assert.AreEqual(0, box2.Value);

        // Execute the buffer
        cmd.Playback().Dispose();

        // Ensure the first one was disposed
        Assert.AreEqual(1, box1.Value);
        Assert.AreEqual(0, box2.Value);
    }
}