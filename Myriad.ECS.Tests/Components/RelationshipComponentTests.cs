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

    [TestMethod]
    public void TypedReferenceWithComponent()
    {
        var world = new WorldBuilder().Build();
        var buffer = new CommandBuffer(world);

        // Create entity with component
        var ab = buffer.Create().Set(new ComponentInt32(17)).Set(new TestPhantom0());

        // Create another entity, relating to it
        var bb = buffer.Create().Set(new TypedRefTest<ComponentInt32>(), ab);

        // Get the entities
        using var p = buffer.Playback();
        var a = ab.Resolve();
        var b = bb.Resolve();

        // Get the relation
        ref var relation = ref b.GetComponentRef<TypedRefTest<ComponentInt32>>();
        Assert.AreEqual(a, relation.Target);

        // Check status
        CheckTypedRef(a, ref relation, world, true, true, false, 17);

        // Make a structural change
        buffer.Set(a, new Component11());
        buffer.Playback().Dispose();

        // Check status again, this will internally refresh the cache
        CheckTypedRef(a, ref relation, world, true, true, false, 17);

        // Delete the relation target
        buffer.Delete(a);
        buffer.Playback().Dispose();

        // Check yet again, the entity should be a phantom now
        CheckTypedRef(a, ref relation, world, true, true, true, 17);

        // Delete it again, so it no longer exists at all
        buffer.Delete(a);
        buffer.Playback().Dispose();

        // Check one final time
        CheckTypedRef(a, ref relation, world, false, false, false);
    }

    [TestMethod]
    public void TypedReferenceChangeTarget()
    {
        var world = new WorldBuilder().Build();
        var buffer = new CommandBuffer(world);

        // Create two entities with component and one without
        var xb = buffer.Create().Set(new ComponentInt32(17)).Set(new TestPhantom0());
        var yb = buffer.Create().Set(new ComponentInt32(18)).Set(new TestPhantom0());
        var zb = buffer.Create()                            .Set(new TestPhantom0());

        // Create another entity, relating to X
        var ab = buffer.Create().Set(new TypedRefTest<ComponentInt32>(), xb);

        // Get the entities
        using var p = buffer.Playback();
        var x = xb.Resolve();
        var y = yb.Resolve();
        var z = zb.Resolve();
        var a = ab.Resolve();

        // Get the relation
        ref var relation = ref a.GetComponentRef<TypedRefTest<ComponentInt32>>();
        Assert.AreEqual(x, relation.Target);

        // Check status
        CheckTypedRef(x, ref relation, world, true, true, false, 17);

        // Change target to another entity with the correct component
        relation.Target = y;

        // Check status
        CheckTypedRef(y, ref relation, world, true, true, false, 18);

        // Change target to another entity without the correct component
        relation.Target = z;

        // Check status
        CheckTypedRef(z, ref relation, world, false, true, false);
    }

    private static void CheckTypedRef(Entity entity, ref TypedRefTest<ComponentInt32> relation, World world, bool has, bool exists, bool phantom, int expected = default)
    {
        // It's cached, so check twice
        CheckTypedRefOnce(entity, ref relation, world, has, exists, phantom, expected);
        CheckTypedRefOnce(entity, ref relation, world, has, exists, phantom, expected);
    }

    private static void CheckTypedRefOnce(Entity entity, ref TypedRefTest<ComponentInt32> relation, World world, bool has, bool exists, bool phantom, int expected)
    {
        var defaultRefComponent = new ComponentInt32(3);

        var result = relation.Ref.TryGetComponentRef(world);

        Assert.AreEqual(entity, relation.Ref.Target);
        Assert.AreEqual(has, result.HasComponent);
        Assert.AreEqual(exists, result.Exists);
        Assert.AreEqual(phantom, result.IsPhantom);

        if (has)
        {
            Assert.AreNotEqual(defaultRefComponent, result.Components.Item0);
            Assert.AreEqual(expected, result.Components.Item0.Value);
        }
    }

    private struct TypedRefTest<TC>
        : IEntityRelationComponent
        where TC : IComponent
    {
        public TypedEntityReference<TC> Ref;

        private Entity _entity;
        public Entity Target
        {
            get => _entity;
            set
            {
                _entity = value;
                Ref.Target = _entity.ID;
            }
        }
    }
}