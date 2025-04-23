using Myriad.ECS.Command;
using Myriad.ECS.Components;
using Myriad.ECS.IDs;
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
        var e = eb.Resolve();
        resolver.Dispose();

        // Is the entity valid
        Assert.IsTrue(e.Exists());
        Assert.IsFalse(e.IsPhantom());

        // Delete it
        cmd.Delete(e);
        cmd.Playback().Dispose();

        // Is the entity valid but no longer alive
        Assert.IsTrue(e.Exists());
        Assert.IsTrue(e.IsPhantom());

        // Delete it again
        cmd.Delete(e);
        cmd.Playback().Dispose();

        // Entity should no longer exist at all
        Assert.IsFalse(e.Exists());
    }

    [TestMethod]
    public void CanRemoveNonPhantomComponents()
    {
        var w = new WorldBuilder().Build();

        // Create an entity with a phantom component
        var cmd = new CommandBuffer(w);
        var eb = cmd.Create().Set(new TestPhantom0()).Set(new TestPhantom1()).Set(new ComponentInt32(42)).Set(new ComponentFloat(42));
        var resolver = cmd.Playback();
        var e = eb.Resolve();
        resolver.Dispose();

        // Is the entity valid
        Assert.IsTrue(e.Exists());
        Assert.IsFalse(e.IsPhantom());

        // Delete it
        cmd.Delete(e);
        cmd.Playback().Dispose();

        // Is the entity valid but no longer alive
        Assert.IsTrue(e.Exists());
        Assert.IsTrue(e.IsPhantom());
        Assert.IsTrue(e.HasComponent<ComponentInt32>());

        // Remove one of the non-phantom components
        cmd.Remove<ComponentInt32>(e);
        cmd.Playback().Dispose();

        // Is the entity valid but no longer alive
        Assert.IsTrue(e.Exists());
        Assert.IsTrue(e.IsPhantom());
        Assert.IsFalse(e.HasComponent<ComponentInt32>());
    }

    [TestMethod]
    public void AutoDeletePhantom()
    {
        var w = new WorldBuilder().Build();

        // Create an entity with a phantom component
        var cmd = new CommandBuffer(w);
        var eb = cmd.Create().Set(new TestPhantom0()).Set(new TestPhantom1()).Set(new ComponentInt32(42)).Set(new ComponentFloat(42));
        var resolver = cmd.Playback();
        var e = eb.Resolve();
        resolver.Dispose();

        // Is the entity valid
        Assert.IsTrue(e.Exists());
        Assert.IsFalse(e.IsPhantom());

        // Delete it
        cmd.Delete(e);
        cmd.Playback().Dispose();

        // Is the entity valid but no longer alive
        Assert.IsTrue(e.Exists());
        Assert.IsTrue(e.IsPhantom());
        Assert.IsTrue(e.HasComponent<ComponentInt32>());

        // Remove one phantom component
        cmd.Remove<TestPhantom1>(e);
        cmd.Playback().Dispose();

        // Is the entity valid but no longer alive
        Assert.IsTrue(e.Exists());
        Assert.IsTrue(e.IsPhantom());
        Assert.IsFalse(e.HasComponent<TestPhantom1>());

        // Remove one more phantom component, this is the final one so it should be auto deleted.
        cmd.Remove<TestPhantom0>(e);
        cmd.Playback().Dispose();

        // Entity should no longer exist at all
        Assert.IsFalse(e.Exists());
    }

    [TestMethod]
    public void AddPhantomComponentAndDelete()
    {
        var w = new WorldBuilder().Build();

        // Create an entity without a phantom component
        var cmd = new CommandBuffer(w);
        var eb = cmd.Create().Set(new ComponentFloat(42));
        var resolver = cmd.Playback();
        var e = eb.Resolve();
        resolver.Dispose();

        // Is the entity valid
        Assert.IsTrue(e.Exists());
        Assert.IsFalse(e.IsPhantom());

        // Add a phantom component and then delete it
        cmd.Set(e, new TestPhantom0());
        cmd.Delete(e);
        cmd.Playback().Dispose();

        // Is the entity valid but no longer alive
        Assert.IsTrue(e.Exists());
        Assert.IsTrue(e.IsPhantom());
        Assert.IsTrue(e.HasComponent<ComponentFloat>());
    }

    [TestMethod]
    public void DeleteAndAddPhantomComponent()
    {
        var w = new WorldBuilder().Build();

        // Create an entity without a phantom component
        var cmd = new CommandBuffer(w);
        var eb = cmd.Create().Set(new ComponentFloat(42));
        var resolver = cmd.Playback();
        var e = eb.Resolve();
        resolver.Dispose();

        // Is the entity valid
        Assert.IsTrue(e.Exists());
        Assert.IsFalse(e.IsPhantom());

        // Delete it and also add a phantom component
        cmd.Delete(e);
        cmd.Set(e, new TestPhantom0());
        cmd.Playback().Dispose();

        // Is the entity valid but no longer alive
        Assert.IsTrue(e.Exists());
        Assert.IsTrue(e.IsPhantom());
        Assert.IsTrue(e.HasComponent<ComponentFloat>());
    }

    [TestMethod]
    public void SimultaneousRemoveAndDelete()
    {
        var w = new WorldBuilder().Build();

        // Create an entity with a phantom component
        var cmd = new CommandBuffer(w);
        var eb = cmd.Create().Set(new ComponentFloat(42)).Set(new TestPhantom0());
        var resolver = cmd.Playback();
        var e = eb.Resolve();
        resolver.Dispose();

        // Delete entity and remove phantom in one go
        cmd.Remove<TestPhantom0>(e);
        cmd.Delete(e);
        cmd.Playback().Dispose();

        Assert.IsFalse(e.IsAlive());
        Assert.IsFalse(e.Exists());
        Assert.IsFalse(e.IsPhantom());
    }

    [TestMethod]
    public void PhantomNotification()
    {
        var w = new WorldBuilder().Build();

        Assert.IsTrue(ComponentID<PhantomNotifier>.ID.IsPhantomNotifierComponent);
        Assert.IsFalse(ComponentID<PhantomNotifier>.ID.IsPhantomComponent);

        // Create an entity with a phantom component
        var list = new List<EntityId>();
        var cmd = new CommandBuffer(w);
        var eb = cmd.Create().Set(new TestPhantom0()).Set(new TestPhantom1()).Set(new PhantomNotifier { CalledWith = list });
        var resolver = cmd.Playback();
        var e = eb.Resolve();
        resolver.Dispose();

        // Is the entity valid
        Assert.IsTrue(e.Exists());
        Assert.IsFalse(e.IsPhantom());
        Assert.AreEqual(0, list.Count);

        // Delete it
        cmd.Delete(e);
        cmd.Playback().Dispose();

        // Is the entity valid but no longer alive
        Assert.IsTrue(e.Exists());
        Assert.IsTrue(e.IsPhantom());
        Assert.AreEqual(1, list.Count);
        Assert.AreEqual(e.ID, list.Single());

        // Remove a component, triggering a migration
        cmd.Remove<TestPhantom1>(e);
        cmd.Playback().Dispose();

        // Check that it did not receive a second notification
        Assert.IsTrue(e.Exists());
        Assert.IsTrue(e.IsPhantom());
        Assert.AreEqual(1, list.Count);
        Assert.AreEqual(e.ID, list.Single());

        // Delete it again
        cmd.Delete(e);
        cmd.Playback().Dispose();

        // Entity should no longer exist at all
        Assert.IsFalse(e.Exists());
        Assert.AreEqual(1, list.Count);
        Assert.AreEqual(e.ID, list.Single());
    }

    [TestMethod]
    public void NoNotification()
    {
        var w = new WorldBuilder().Build();

        Assert.IsTrue(ComponentID<PhantomNotifier>.ID.IsPhantomNotifierComponent);
        Assert.IsFalse(ComponentID<PhantomNotifier>.ID.IsPhantomComponent);

        // Create an entity **without** a phantom component
        var list = new List<EntityId>();
        var cmd = new CommandBuffer(w);
        var eb = cmd.Create().Set(new ComponentInt32()).Set(new PhantomNotifier { CalledWith = list });
        var resolver = cmd.Playback();
        var e = eb.Resolve();
        resolver.Dispose();

        // Is the entity valid
        Assert.IsTrue(e.Exists());
        Assert.IsFalse(e.IsPhantom());
        Assert.AreEqual(0, list.Count);

        // Delete it
        cmd.Delete(e);
        cmd.Playback().Dispose();

        // Is the entity dead
        Assert.IsFalse(e.Exists());
        Assert.IsFalse(e.IsPhantom());
        Assert.AreEqual(0, list.Count);

        // Delete it again
        cmd.Delete(e);
        cmd.Playback().Dispose();
    }
}