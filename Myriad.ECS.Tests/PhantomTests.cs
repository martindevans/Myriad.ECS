using Myriad.ECS.Command;
using Myriad.ECS.Components;
using Myriad.ECS.Worlds;

namespace Myriad.ECS.Tests;

[TestClass]
public class PhantomTests
{
    [TestMethod]
    public void CannotExplicitlyAttachPhantomBuffered()
    {
        var w = new WorldBuilder().Build();

        // Create an entity with a phantom component
        var cmd = new CommandBuffer(w);

        var eb = cmd.Create().Set(new TestPhantom0()).Set(new ComponentInt32(42));

        Assert.ThrowsException<InvalidOperationException>(() =>
        {
            eb.Set(new Phantom());
        });
    }

    [TestMethod]
    public void CannotExplicitlyAttachPhantom()
    {
        var w = new WorldBuilder().Build();

        // Create an entity with a phantom component
        var cmd = new CommandBuffer(w);

        cmd.Create().Set(new TestPhantom0()).Set(new ComponentInt32(42));
        var e = cmd.Playback()[0];

        Assert.ThrowsException<InvalidOperationException>(() =>
        {
            cmd.Set(e, new Phantom());
        });
    }

    [TestMethod]
    public void CannotExplicitlyRemovePhantom()
    {
        var w = new WorldBuilder().Build();

        // Create an entity with a phantom component
        var cmd = new CommandBuffer(w);

        cmd.Create().Set(new TestPhantom0()).Set(new ComponentInt32(42));
        var e = cmd.Playback()[0];

        // Delete it, so it becomes a phantom
        cmd.Delete(e);
        cmd.Playback().Dispose();

        Assert.ThrowsException<InvalidOperationException>(() =>
        {
            cmd.Remove<Phantom>(e);
        });
    }

    [TestMethod]
    public void DeletePhantom()
    {
        var w = new WorldBuilder().Build();

        // Create an entity with a phantom component
        var cmd = new CommandBuffer(w);
        var eb = cmd.Create().Set(new TestPhantom0()).Set(new ComponentInt32(42));
        var resolver = cmd.Playback();
        var e = resolver[eb];
        resolver.Dispose();

        // Is the entity valid
        Assert.IsTrue(e.Exists(w));
        Assert.IsFalse(e.IsPhantom(w));

        // Delete it
        cmd.Delete(e);
        cmd.Playback().Dispose();

        // Is the entity valid but no longer alive
        Assert.IsTrue(e.Exists(w));
        Assert.IsTrue(e.IsPhantom(w));

        // Delete it again
        cmd.Delete(e);
        cmd.Playback().Dispose();

        // Entity should no longer exist at all
        Assert.IsFalse(e.Exists(w));
    }

    [TestMethod]
    public void CanRemoveNonPhantomComponents()
    {
        var w = new WorldBuilder().Build();

        // Create an entity with a phantom component
        var cmd = new CommandBuffer(w);
        var eb = cmd.Create().Set(new TestPhantom0()).Set(new TestPhantom1()).Set(new ComponentInt32(42)).Set(new ComponentFloat(42));
        var resolver = cmd.Playback();
        var e = resolver[eb];
        resolver.Dispose();

        // Is the entity valid
        Assert.IsTrue(e.Exists(w));
        Assert.IsFalse(e.IsPhantom(w));

        // Delete it
        cmd.Delete(e);
        cmd.Playback().Dispose();

        // Is the entity valid but no longer alive
        Assert.IsTrue(e.Exists(w));
        Assert.IsTrue(e.IsPhantom(w));
        Assert.IsTrue(w.HasComponent<ComponentInt32>(e));

        // Remove one of the non-phantom components
        cmd.Remove<ComponentInt32>(e);
        cmd.Playback().Dispose();

        // Is the entity valid but no longer alive
        Assert.IsTrue(e.Exists(w));
        Assert.IsTrue(e.IsPhantom(w));
        Assert.IsFalse(w.HasComponent<ComponentInt32>(e));
    }

    [TestMethod]
    public void AutoDeletePhantom()
    {
        var w = new WorldBuilder().Build();

        // Create an entity with a phantom component
        var cmd = new CommandBuffer(w);
        var eb = cmd.Create().Set(new TestPhantom0()).Set(new TestPhantom1()).Set(new ComponentInt32(42)).Set(new ComponentFloat(42));
        var resolver = cmd.Playback();
        var e = resolver[eb];
        resolver.Dispose();

        // Is the entity valid
        Assert.IsTrue(e.Exists(w));
        Assert.IsFalse(e.IsPhantom(w));

        // Delete it
        cmd.Delete(e);
        cmd.Playback().Dispose();

        // Is the entity valid but no longer alive
        Assert.IsTrue(e.Exists(w));
        Assert.IsTrue(e.IsPhantom(w));
        Assert.IsTrue(w.HasComponent<ComponentInt32>(e));

        // Remove one phantom component
        cmd.Remove<TestPhantom1>(e);
        cmd.Playback().Dispose();

        // Is the entity valid but no longer alive
        Assert.IsTrue(e.Exists(w));
        Assert.IsTrue(e.IsPhantom(w));
        Assert.IsFalse(w.HasComponent<TestPhantom1>(e));

        // Remove one more phantom component, this is the final one so it should be auto deleted.
        cmd.Remove<TestPhantom0>(e);
        cmd.Playback().Dispose();

        // Entity should no longer exist at all
        Assert.IsFalse(e.Exists(w));
    }

    [TestMethod]
    public void AddPhantomComponentAndDelete()
    {
        var w = new WorldBuilder().Build();

        // Create an entity without a phantom component
        var cmd = new CommandBuffer(w);
        var eb = cmd.Create().Set(new ComponentFloat(42));
        var resolver = cmd.Playback();
        var e = resolver[eb];
        resolver.Dispose();

        // Is the entity valid
        Assert.IsTrue(e.Exists(w));
        Assert.IsFalse(e.IsPhantom(w));

        // Add a phantom component and then delete it
        cmd.Set(e, new TestPhantom0());
        cmd.Delete(e);
        cmd.Playback().Dispose();

        // Is the entity valid but no longer alive
        Assert.IsTrue(e.Exists(w));
        Assert.IsTrue(e.IsPhantom(w));
        Assert.IsTrue(w.HasComponent<ComponentFloat>(e));
    }

    [TestMethod]
    public void DeleteAndAddPhantomComponent()
    {
        var w = new WorldBuilder().Build();

        // Create an entity without a phantom component
        var cmd = new CommandBuffer(w);
        var eb = cmd.Create().Set(new ComponentFloat(42));
        var resolver = cmd.Playback();
        var e = resolver[eb];
        resolver.Dispose();

        // Is the entity valid
        Assert.IsTrue(e.Exists(w));
        Assert.IsFalse(e.IsPhantom(w));

        // Delete it and also add a phantom component
        cmd.Delete(e);
        cmd.Set(e, new TestPhantom0());
        cmd.Playback().Dispose();

        // Is the entity valid but no longer alive
        Assert.IsTrue(e.Exists(w));
        Assert.IsTrue(e.IsPhantom(w));
        Assert.IsTrue(w.HasComponent<ComponentFloat>(e));
    }
}