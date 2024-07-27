using Myriad.ECS.Command;
using Myriad.ECS.IDs;
using Myriad.ECS.Worlds;

namespace Myriad.ECS.Tests.Components;

[TestClass]
public class RelationshipComponentTests
{
    [TestMethod]
    public void ComponentIdFlag()
    {
        Assert.IsFalse(ComponentID<ComponentInt32>.ID.IsRelationComponent);
        Assert.IsTrue(ComponentID<Relational1>.ID.IsRelationComponent);
    }

    [TestMethod]
    public void BindBufferedBuffered()
    {
        var world = new WorldBuilder().Build();
        var buffer = new CommandBuffer(world);

        var ab = buffer.Create().Set(new ComponentInt32(17));
        var bb = buffer.Create().Set(new ComponentInt32(18));
        var cb = buffer.Create().Set(new Relational1(), ab).Set(new Relational2(default, 42), bb);

        using var resolver = buffer.Playback();

        var a = ab.Resolve(resolver);
        var b = bb.Resolve(resolver);
        var c = cb.Resolve(resolver);

        Assert.AreEqual(a, c.GetComponentRef<Relational1>(world).Target);
        Assert.AreEqual(b, c.GetComponentRef<Relational2>(world).Target);
    }

    [TestMethod]
    public void BindUnbufferedBuffered()
    {
        var world = new WorldBuilder().Build();
        var buffer = new CommandBuffer(world);

        // Create an entity
        var cb = buffer.Create();
        using var resolver1 = buffer.Playback();
        var c = cb.Resolve(resolver1);

        // Add relationships to existing entity
        var ab = buffer.Create().Set(new ComponentInt32(17));
        var bb = buffer.Create().Set(new ComponentInt32(18));
        buffer.Set(c, new Relational1(), ab);
        buffer.Set(c, new Relational2(default, 42), bb);
        using var resolver2 = buffer.Playback();

        var a = ab.Resolve(resolver2);
        var b = bb.Resolve(resolver2);

        Assert.AreEqual(a, c.GetComponentRef<Relational1>(world).Target);
        Assert.AreEqual(b, c.GetComponentRef<Relational2>(world).Target);
    }

    [TestMethod]
    public void BindBufferedUnbuffered()
    {
        var world = new WorldBuilder().Build();
        var buffer = new CommandBuffer(world);

        // Create an entity
        var ab = buffer.Create();
        using var resolver1 = buffer.Playback();
        var a = ab.Resolve(resolver1);

        // Create a new entity, pointing at the existing one
        var bb = buffer.Create().Set(new Relational1(), a);
        using var resolver2 = buffer.Playback();
        var b = bb.Resolve(resolver2);

        Assert.AreEqual(a, b.GetComponentRef<Relational1>(world).Target);
    }

    [TestMethod]
    public void BindUnbufferedUnbuffered()
    {
        var world = new WorldBuilder().Build();
        var buffer = new CommandBuffer(world);

        // Create two entities
        var ab = buffer.Create();
        var bb = buffer.Create();
        using var resolver1 = buffer.Playback();
        var a = ab.Resolve(resolver1);
        var b = bb.Resolve(resolver1);

        // Add relationship
        buffer.Set(a, new Relational1(), b);
        buffer.Set(b, new Relational1(), a);
        buffer.Playback().Dispose();

        Assert.AreEqual(b, a.GetComponentRef<Relational1>(world).Target);
        Assert.AreEqual(a, b.GetComponentRef<Relational1>(world).Target);
    }
}