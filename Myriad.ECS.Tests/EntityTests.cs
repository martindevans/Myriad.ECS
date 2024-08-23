using Myriad.ECS.Worlds;
using Myriad.ECS.Command;
using Myriad.ECS.IDs;

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
        var entity = eb.Resolve();

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
        var entity1 = eb1.Resolve();
        var entity2 = eb2.Resolve();

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
        var entity1 = eb1.Resolve();
        var entity2 = eb2.Resolve();

        var id1 = entity1.UniqueID();
        var id2 = entity2.UniqueID();

        Assert.AreNotEqual(id1, id2);
    }

    [TestMethod]
    public void GetComponent()
    {
        var w = new WorldBuilder().Build();
        var b = new CommandBuffer(w);

        var e = b.Create()
                 .Set(new ComponentInt16(7));
        using var resolver = b.Playback();
        var entity = e.Resolve();

        ref var c = ref entity.GetComponentRef<ComponentInt16>(w);
        Assert.AreEqual(7, c.Value);
    }

    [TestMethod]
    public void GetBoxedComponent()
    {
        var w = new WorldBuilder().Build();
        var b = new CommandBuffer(w);

        var e = b.Create()
                 .Set(new ComponentInt16(7));
        using var resolver = b.Playback();
        var entity = e.Resolve();

        var c = (ComponentInt16)entity.GetBoxedComponent(w, ComponentID<ComponentInt16>.ID)!;
        Assert.AreEqual(7, c.Value);

        Assert.IsNull(entity.GetBoxedComponent(w, ComponentID<ComponentInt32>.ID));

        b.Delete(entity);
        b.Playback().Dispose();

        Assert.IsNull(entity.GetBoxedComponent(w, ComponentID<ComponentInt16>.ID));
        Assert.IsNull(entity.GetBoxedComponent(w, ComponentID<ComponentInt32>.ID));
    }
}