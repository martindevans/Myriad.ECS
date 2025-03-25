using Myriad.ECS.Command;
using Myriad.ECS.Components;
using Myriad.ECS.IDs;
using Myriad.ECS.Worlds;

namespace Myriad.ECS.Tests.Components;

[TestClass]
public class RelationshipComponentTests
{
    private static Entity CreateEntity(World world)
    {
        var cmd = new CommandBuffer(world);

        var buffered = cmd.Create();
        using var resolver = cmd.Playback();

        return buffered.Resolve();
    }

    [TestMethod]
    public void ComponentIdFlag()
    {
        Assert.IsFalse(ComponentID<ComponentInt32>.ID.IsRelationComponent);
        Assert.IsTrue(ComponentID<Relational1>.ID.IsRelationComponent);
    }

    [TestMethod]
    public void BindBufferedMixup()
    {
        var world = new WorldBuilder().Build();
        var buffer1 = new CommandBuffer(world);
        var buffer2 = new CommandBuffer(world);

        // Create entity in buffer 1
        var ab = buffer1.Create().Set(new ComponentInt32(17));

        // Try to bind a relation in buffer 2
        Assert.ThrowsException<ArgumentException>(() =>
        {
            buffer2.Create().Set(new Relational1(), ab);
        });
    }

    [TestMethod]
    public void BindBufferedMixup2()
    {
        var world = new WorldBuilder().Build();
        var buffer1 = new CommandBuffer(world);
        var buffer2 = new CommandBuffer(world);

        var entity = CreateEntity(world);

        // Create a buffered entity in cmd buffer 1
        var b1 = buffer1.Create();

        // Bind a relationship in buffer 2
        Assert.ThrowsException<ArgumentException>(() =>
        {
            buffer2.Set(entity, new Relational1(), b1);
        });
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

        var a = ab.Resolve();
        var b = bb.Resolve();
        var c = cb.Resolve();

        Assert.AreEqual(a, c.GetComponentRef<Relational1>().Target);
        Assert.AreEqual(b, c.GetComponentRef<Relational2>().Target);
    }

    [TestMethod]
    public void BindUnbufferedBuffered()
    {
        var world = new WorldBuilder().Build();
        var buffer = new CommandBuffer(world);

        // Create an entity
        var cb = buffer.Create();
        using var resolver1 = buffer.Playback();
        var c = cb.Resolve();

        // Add relationships to existing entity
        var ab = buffer.Create().Set(new ComponentInt32(17));
        var bb = buffer.Create().Set(new ComponentInt32(18));
        buffer.Set(c, new Relational1(), ab);
        buffer.Set(c, new Relational2(default, 42), bb);
        using var resolver2 = buffer.Playback();

        var a = ab.Resolve();
        var b = bb.Resolve();

        Assert.AreEqual(a, c.GetComponentRef<Relational1>().Target);
        Assert.AreEqual(b, c.GetComponentRef<Relational2>().Target);
    }

    [TestMethod]
    public void BindBufferedUnbuffered()
    {
        var world = new WorldBuilder().Build();
        var buffer = new CommandBuffer(world);

        // Create an entity
        var ab = buffer.Create();
        using var resolver1 = buffer.Playback();
        var a = ab.Resolve();

        // Create a new entity, pointing at the existing one
        var bb = buffer.Create().Set(new Relational1(), a);
        using var resolver2 = buffer.Playback();
        var b = bb.Resolve();

        Assert.AreEqual(a, b.GetComponentRef<Relational1>().Target);
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
        var a = ab.Resolve();
        var b = bb.Resolve();

        // Add relationship
        buffer.Set(a, new Relational1(), b);
        buffer.Set(b, new Relational1(), a);
        buffer.Playback().Dispose();

        Assert.AreEqual(b, a.GetComponentRef<Relational1>().Target);
        Assert.AreEqual(a, b.GetComponentRef<Relational1>().Target);
    }

    [TestMethod]
    public void BindSelfReference()
    {
        var world = new WorldBuilder().Build();
        var buffer = new CommandBuffer(world);

        var eb = buffer.Create();
        eb.Set(new SelfReference());

        using var _ = buffer.Playback();

        var e = eb.Resolve();

        Assert.AreEqual(e, e.GetComponentRef<SelfReference>().Target);
    }

    [TestMethod]
    public void BindSelfReferenceToOtherBuffered()
    {
        var world = new WorldBuilder().Build();
        var buffer = new CommandBuffer(world);

        var eb1 = buffer.Create();
        var eb2 = buffer.Create();

        Assert.ThrowsException<InvalidOperationException>(() =>
        {
            eb1.Set(new SelfReference(), eb2);
        });
    }

    [TestMethod]
    public void BindSelfReferenceToOther()
    {
        var world = new WorldBuilder().Build();
        var buffer = new CommandBuffer(world);

        var eb1 = buffer.Create();
        using var _ = buffer.Playback();
        var e1 = eb1.Resolve();

        var eb2 = buffer.Create();

        Assert.ThrowsException<InvalidOperationException>(() =>
        {
            eb2.Set(new SelfReference(), e1);
        });
    }
}