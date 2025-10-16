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
        Assert.IsFalse(default(Entity).Exists());
        Assert.IsFalse(default(Entity).IsAlive());
        Assert.IsFalse(default(Entity).IsPhantom());
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

        Assert.AreNotEqual(entity1.ToString(), entity2.ToString());
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
    public void HasComponent()
    {
        var w = new WorldBuilder().Build();
        var b = new CommandBuffer(w);

        var e = b.Create().Set(new ComponentInt16(7));
        using var resolver = b.Playback();
        var entity = e.Resolve();

        Assert.IsTrue(entity.HasComponent<ComponentInt16>());
        Assert.IsFalse(entity.HasComponent<ComponentInt32>());

        Assert.IsFalse(entity.HasComponent<ComponentInt16, ComponentInt32>());
        Assert.IsFalse(entity.HasComponent<ComponentInt32, ComponentInt32>());
        Assert.IsTrue(entity.HasComponent<ComponentInt16, ComponentInt16>());
        Assert.IsFalse(entity.HasComponent<ComponentInt16, ComponentInt32, ComponentInt64>());
    }

    [TestMethod]
    public void HasComponentNoEntity()
    {
        var w = new WorldBuilder().Build();

        // Create an invalid entity
        var entity = new Entity(new EntityId(-23, 4), w);

        Assert.IsFalse(entity.HasComponent<ComponentInt16>());
        Assert.IsFalse(entity.HasComponent<ComponentInt32>());
        Assert.IsFalse(entity.HasComponent<ComponentInt16, ComponentInt32>());
        Assert.IsFalse(entity.HasComponent<ComponentInt16, ComponentInt32, ComponentInt64>());
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

        ref var c = ref entity.GetComponentRef<ComponentInt16>();
        Assert.AreEqual(7, c.Value);
    }

    [TestMethod]
    public void GetComponents()
    {
        var w = new WorldBuilder().Build();
        var b = new CommandBuffer(w);

        var e = b.Create().Set(new ComponentInt16(7));
        using var resolver = b.Playback();
        var entity = e.Resolve();

        Assert.AreEqual(1, entity.ComponentTypes.Count);
        Assert.IsTrue(entity.ComponentTypes.Contains(ComponentID<ComponentInt16>.ID));
    }

    [TestMethod]
    public void GetComponentsTuple()
    {
        var w = new WorldBuilder().Build();
        var b = new CommandBuffer(w);

        var e = b.Create().Set(new ComponentInt16(7)).Set(new Component1()).Set(new ComponentInt32());
        using var resolver = b.Playback();
        var entity = e.Resolve();

        Assert.AreEqual(3, entity.ComponentTypes.Count);
        Assert.IsTrue(entity.ComponentTypes.Contains(ComponentID<ComponentInt16>.ID));

        var tuple = entity.GetComponentRef<ComponentInt16, ComponentInt32>();
        var (t1_e, t1_16, t1_32) = tuple;
        var (t2_16, t2_32) = tuple;

        Assert.AreEqual(entity, t1_e);
        Assert.AreEqual(7, t1_16.Ref.Value);
        Assert.AreEqual(7, t2_16.Ref.Value);
    }

    [TestMethod]
    public void GetComponentDead()
    {
        var w = new WorldBuilder().Build();
        var b = new CommandBuffer(w);

        var e = b.Create().Set(new ComponentInt16(7));
        var resolver = b.Playback();
        var entity = e.Resolve();
        resolver.Dispose();

        b.Delete(entity);
        b.Playback().Dispose();

        Assert.ThrowsException<ArgumentException>(() =>
        {
            var c = entity.ComponentTypes.Count;
        });
    }

    [TestMethod]
    public void GetBoxedComponents()
    {
        var w = new WorldBuilder().Build();
        var b = new CommandBuffer(w);

        var e = b.Create().Set(new ComponentInt16(7));
        using var resolver = b.Playback();
        var entity = e.Resolve();

        Assert.AreEqual(1, entity.BoxedComponents.Length);
        Assert.AreEqual(new ComponentInt16(7), (ComponentInt16)entity.BoxedComponents[0]);
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

        var c = (ComponentInt16)entity.GetBoxedComponent(ComponentID<ComponentInt16>.ID)!;
        Assert.AreEqual(7, c.Value);

        Assert.IsNull(entity.GetBoxedComponent(ComponentID<ComponentInt32>.ID));

        b.Delete(entity);
        b.Playback().Dispose();

        Assert.IsNull(entity.GetBoxedComponent(ComponentID<ComponentInt16>.ID));
        Assert.IsNull(entity.GetBoxedComponent(ComponentID<ComponentInt32>.ID));
    }
}