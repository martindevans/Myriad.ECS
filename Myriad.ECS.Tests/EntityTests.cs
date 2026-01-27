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
    public void ImplicitConversionToEntityId()
    {
        var w = new WorldBuilder().Build();
        var b = new CommandBuffer(w);

        var eb = b.Create();
        using var resolver = b.Playback();
        var entity = eb.Resolve();

        // Convert to ID in all the various ways
        var id1 = entity.ID;            // Property
        var id2 = (EntityId)entity;     // Explicit
        EntityId id3 = entity;          // Implicit

        Assert.AreEqual(id1, id2);
        Assert.AreEqual(id1, id3);

        // Round trip the ID
        Assert.AreEqual(id1, (EntityId)(ulong)id1);
    }

    [TestMethod]
    public void RoundtripRawEntityId()
    {
        var w = new WorldBuilder().Build();
        var b = new CommandBuffer(w);

        var eb = b.Create();
        using var resolver = b.Playback();
        var entity = eb.Resolve();

        // Round trip the ID
        Assert.AreEqual(entity.ID, (EntityId)(ulong)entity.ID);
    }

    [TestMethod]
    public void EntityIdEquality()
    {
        var w = new WorldBuilder().Build();
        var b = new CommandBuffer(w);

        var eb1 = b.Create();
        var eb2 = b.Create();
        using var resolver = b.Playback();
        var entity1 = eb1.Resolve();
        var entity2 = eb2.Resolve();

        var id1 = entity1.ID;
        var id2 = entity2.ID;

        // ReSharper disable EqualExpressionComparison
        Assert.IsTrue(id1 == id1);
        Assert.IsFalse(id1 != id1);

        Assert.IsTrue(id1 != id2);
        Assert.IsFalse(id1 == id2);

        Assert.IsTrue(id1.Equals((object)id1));
        Assert.IsFalse(id1.Equals((object)id2));
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
    public void TryGetComponentsTuple_AliveSuccess()
    {
        var w = new WorldBuilder().Build();
        var b = new CommandBuffer(w);

        var e = b.Create().Set(new ComponentInt16(7)).Set(new Component1()).Set(new ComponentInt32());
        using var resolver = b.Playback();
        var entity = e.Resolve();

        Assert.AreEqual(3, entity.ComponentTypes.Count);
        Assert.IsTrue(entity.ComponentTypes.Contains(ComponentID<ComponentInt16>.ID));

        Assert.IsTrue(entity.TryGetComponentRef<ComponentInt16, ComponentInt32>(out var tuple));
        var (t1_e, t1_16, t1_32) = tuple;
        var (t2_16, t2_32) = tuple;

        Assert.AreEqual(entity, t1_e);
        Assert.AreEqual(7, t1_16.Ref.Value);
        Assert.AreEqual(7, t2_16.Ref.Value);
    }

    [TestMethod]
    public void TryGetComponentsTuple_AliveFail()
    {
        var w = new WorldBuilder().Build();
        var b = new CommandBuffer(w);

        var e = b.Create().Set(new ComponentInt16(7)).Set(new Component1()).Set(new ComponentInt32());
        using var resolver = b.Playback();
        var entity = e.Resolve();

        Assert.AreEqual(3, entity.ComponentTypes.Count);
        Assert.IsTrue(entity.ComponentTypes.Contains(ComponentID<ComponentInt16>.ID));

        Assert.IsFalse(entity.TryGetComponentRef<ComponentInt16, Component13>(out var tuple));
    }

    [TestMethod]
    public void TryGetComponentsTuple_AliveMissingComponent()
    {
        var w = new WorldBuilder().Build();
        var b = new CommandBuffer(w);

        var e = b.Create().Set(new ComponentInt16(7)).Set(new Component1()).Set(new ComponentInt32());
        using var resolver = b.Playback();
        var entity = e.Resolve();

        Assert.AreEqual(3, entity.ComponentTypes.Count);
        Assert.IsTrue(entity.ComponentTypes.Contains(ComponentID<ComponentInt16>.ID));

        Assert.IsFalse(entity.TryGetComponentRef<ComponentInt16, ComponentInt32, Component13>(out var tuple));
    }

    [TestMethod]
    public void TryGetComponentsTuple_Dead()
    {
        var w = new WorldBuilder().Build();
        var b = new CommandBuffer(w);

        var e = b.Create().Set(new ComponentInt16(7)).Set(new Component1()).Set(new ComponentInt32());
        using var resolver = b.Playback();
        var entity = e.Resolve();

        Assert.AreEqual(3, entity.ComponentTypes.Count);
        Assert.IsTrue(entity.ComponentTypes.Contains(ComponentID<ComponentInt16>.ID));

        b.Delete(entity);
        b.Playback().Dispose();

        Assert.IsFalse(entity.TryGetComponentRef<ComponentInt16, ComponentInt32>(out var tuple));
    }

    [TestMethod]
    public void TryGetComponent_HasComponent()
    {
        var w = new WorldBuilder().Build();
        var b = new CommandBuffer(w);

        // Create an entity that will become phantom when deleted
        var e = b.Create().Set(new ComponentInt16(7)).Set(new TestPhantom0());
        using var resolver = b.Playback();
        var entity = e.Resolve();

        // Get the component
        Assert.IsTrue(entity.TryGetComponentRef<ComponentInt16>(out var item1));
        Assert.AreEqual(7, item1.Item0.Value);

        // Destroy it so it becomes a phantom
        b.Delete(entity);
        b.Playback().Dispose();

        // Should still work
        Assert.IsTrue(entity.TryGetComponentRef<ComponentInt16>(out var item2));
        Assert.AreEqual(7, item2.Item0.Value);

        // Destroy it so it no longer exists
        b.Delete(entity);
        b.Playback().Dispose();

        // Should no longer work
        Assert.IsFalse(entity.TryGetComponentRef<ComponentInt16>(out var item3));
    }

    [TestMethod]
    public void TryGetComponent_MissingComponent()
    {
        var w = new WorldBuilder().Build();
        var b = new CommandBuffer(w);

        // Create an entity that will become phantom when deleted
        var e = b.Create().Set(new ComponentInt16(7)).Set(new TestPhantom0());
        using var resolver = b.Playback();
        var entity = e.Resolve();

        // Get the component
        Assert.IsFalse(entity.TryGetComponentRef<ComponentInt32>(out var item1));
    }

    [TestMethod]
    public void TryGetComponent_NonExist()
    {
        var entity = default(Entity);

        // Get the component
        Assert.IsFalse(entity.TryGetComponentRef<ComponentInt32>(out var item1));

        // Get it boxed
        Assert.IsNull(entity.GetBoxedComponent(ComponentID<ComponentInt32>.ID));
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