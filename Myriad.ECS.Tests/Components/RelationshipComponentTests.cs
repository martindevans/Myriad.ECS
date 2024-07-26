using Myriad.ECS.Command;
using Myriad.ECS.Worlds;

namespace Myriad.ECS.Tests.Components;

[TestClass]
public class RelationshipComponentTests
{
    [TestMethod]
    public void BindBuffered()
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
    public void BindUnbuffered()
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
}