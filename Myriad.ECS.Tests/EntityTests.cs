using Myriad.ECS.Worlds;
using System;
using Myriad.ECS.Command;

namespace Myriad.ECS.Tests;

[TestClass]
public class EntityTests
{
    [TestMethod]
    public void DefaultEntityIsNotAlive()
    {
        var w = new WorldBuilder().Build();
        Assert.IsFalse(default(Entity).Exists(w));
        Assert.IsFalse(default(Entity).IsAlive(w));
        Assert.IsFalse(default(Entity).IsPhantom(w));
    }

    [TestMethod]
    public void CompareDefaultEntity()
    {
        Assert.AreEqual(0, default(Entity).CompareTo(default));
    }

    [TestMethod]
    public void CompareEntityWithSelf()
    {
        var w = new WorldBuilder().Build();
        var b = new CommandBuffer(w);

        var eb = b.Create();
        using var resolver = b.Playback();
        var entity = resolver.Resolve(eb);

        Assert.AreEqual(0, entity.CompareTo(entity));
    }

    [TestMethod]
    public void CompareEntityWithAnother()
    {
        var w = new WorldBuilder().Build();
        var b = new CommandBuffer(w);

        var eb1 = b.Create();
        var eb2 = b.Create();
        using var resolver = b.Playback();
        var entity1 = resolver.Resolve(eb1);
        var entity2 = resolver.Resolve(eb2);

        var c1 = entity1.CompareTo(entity2);
        var c2 = entity2.CompareTo(entity1);

        Assert.AreNotEqual(c1, c2);
        Assert.AreNotEqual(0, c1);
        Assert.AreNotEqual(0, c2);
    }

    [TestMethod]
    public void EntityUniqueID()
    {
        var w = new WorldBuilder().Build();
        var b = new CommandBuffer(w);

        var eb1 = b.Create();
        var eb2 = b.Create();
        using var resolver = b.Playback();
        var entity1 = resolver.Resolve(eb1);
        var entity2 = resolver.Resolve(eb2);

        var id1 = entity1.UniqueID();
        var id2 = entity2.UniqueID();

        Assert.AreNotEqual(id1, id2);
    }
}