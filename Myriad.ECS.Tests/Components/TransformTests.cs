using System.Numerics;
using Myriad.ECS.Command;
using Myriad.ECS.Components;
using Myriad.ECS.Systems;
using Myriad.ECS.Worlds;

namespace Myriad.ECS.Tests.Components;

[TestClass]
public class TransformTests
{
    [TestMethod]
    public void SingleEntityTransform()
    {
        var w = new WorldBuilder().Build();
        var c = new CommandBuffer(w);
        var eb = c.Create()
            .Set(new Vector2LocalTransform { Transform = new(Vector2.One) })
            .Set(new Vector2WorldTransform());

        using var _ = c.Playback();
        var e = eb.Resolve();

        new TransformAddIntegers(w).Update(new GameTime());

        Assert.AreEqual(Vector2.One, e.GetComponentRef<Vector2LocalTransform>().Transform.Value);
        Assert.AreEqual(Vector2.One, e.GetComponentRef<Vector2WorldTransform>().Transform.Value);
    }

    [TestMethod]
    public void SingleParentEntityTransform()
    {
        var w = new WorldBuilder().Build();
        var c = new CommandBuffer(w);

        var eb1 = c.Create()
                  .Set(new Vector2LocalTransform { Transform = new(Vector2.UnitX) })
                  .Set(new Vector2WorldTransform());

        var eb2 = c.Create()
                   .Set(new Vector2LocalTransform { Transform = new(Vector2.UnitY) })
                   .Set(new Vector2WorldTransform())
                   .Set(new TransformParent(), eb1);

        using var _ = c.Playback();
        var parent = eb1.Resolve();
        var child = eb2.Resolve();

        new TransformAddIntegers(w).Update(new GameTime());

        Assert.AreEqual(Vector2.UnitX, parent.GetComponentRef<Vector2LocalTransform>().Transform.Value);
        Assert.AreEqual(Vector2.UnitX, parent.GetComponentRef<Vector2WorldTransform>().Transform.Value);

        Assert.AreEqual(Vector2.UnitY, child.GetComponentRef<Vector2LocalTransform>().Transform.Value);
        Assert.AreEqual(Vector2.One, child.GetComponentRef<Vector2WorldTransform>().Transform.Value);
    }

    [TestMethod]
    public void TwoChildrenParentEntityTransform()
    {
        var w = new WorldBuilder().Build();
        var c = new CommandBuffer(w);

        var eb1 = c.Create()
                   .Set(new Vector2LocalTransform { Transform = new(Vector2.UnitX) })
                   .Set(new Vector2WorldTransform());

        var eb2 = c.Create()
                   .Set(new Vector2LocalTransform { Transform = new(Vector2.UnitY) })
                   .Set(new Vector2WorldTransform())
                   .Set(new TransformParent(), eb1);

        var eb3 = c.Create()
                   .Set(new Vector2LocalTransform { Transform = new(-Vector2.UnitY) })
                   .Set(new Vector2WorldTransform())
                   .Set(new TransformParent(), eb1);

        using var _ = c.Playback();
        var parent = eb1.Resolve();
        var child2 = eb2.Resolve();
        var child3 = eb3.Resolve();

        new TransformAddIntegers(w).Update(new GameTime());

        Assert.AreEqual(Vector2.UnitX, parent.GetComponentRef<Vector2LocalTransform>().Transform.Value);
        Assert.AreEqual(Vector2.UnitX, parent.GetComponentRef<Vector2WorldTransform>().Transform.Value);

        Assert.AreEqual(Vector2.UnitY, child2.GetComponentRef<Vector2LocalTransform>().Transform.Value);
        Assert.AreEqual(Vector2.One, child2.GetComponentRef<Vector2WorldTransform>().Transform.Value);

        Assert.AreEqual(-Vector2.UnitY, child3.GetComponentRef<Vector2LocalTransform>().Transform.Value);
        Assert.AreEqual(new Vector2(1, -1), child3.GetComponentRef<Vector2WorldTransform>().Transform.Value);
    }

    [TestMethod]
    public void MultipleChainedChildren()
    {
        var w = new WorldBuilder().Build();
        var c = new CommandBuffer(w);

        var eb1 = c.Create()
                   .Set(new Vector2LocalTransform { Transform = new(Vector2.UnitX) })
                   .Set(new Vector2WorldTransform());

        var eb2 = c.Create()
                   .Set(new Vector2LocalTransform { Transform = new(Vector2.UnitY) })
                   .Set(new Vector2WorldTransform())
                   .Set(new TransformParent(), eb1);

        var eb3 = c.Create()
                   .Set(new Vector2LocalTransform { Transform = new(2 * Vector2.UnitY) })
                   .Set(new Vector2WorldTransform())
                   .Set(new TransformParent(), eb2);

        using var _ = c.Playback();
        var parent = eb1.Resolve();
        var child2 = eb2.Resolve();
        var child3 = eb3.Resolve();

        new TransformAddIntegers(w).Update(new GameTime());

        Assert.AreEqual(Vector2.UnitX, parent.GetComponentRef<Vector2LocalTransform>().Transform.Value);
        Assert.AreEqual(Vector2.UnitX, parent.GetComponentRef<Vector2WorldTransform>().Transform.Value);

        Assert.AreEqual(Vector2.UnitY, child2.GetComponentRef<Vector2LocalTransform>().Transform.Value);
        Assert.AreEqual(Vector2.One, child2.GetComponentRef<Vector2WorldTransform>().Transform.Value);

        Assert.AreEqual(2 * Vector2.UnitY, child3.GetComponentRef<Vector2LocalTransform>().Transform.Value);
        Assert.AreEqual(new Vector2(1, 3), child3.GetComponentRef<Vector2WorldTransform>().Transform.Value);
    }

    private class TransformAddIntegers
        : BaseUpdateTransformHierarchySystem<GameTime, Vector2Transform, Vector2LocalTransform, Vector2WorldTransform>
    {
        public TransformAddIntegers(World world)
            : base(world)
        {
        }
    }

    private struct Vector2Transform
        : ITransform<Vector2Transform>
    {
        public Vector2 Value;

        public Vector2Transform(Vector2 value)
        {
            Value = value;
        }

        public Vector2Transform Compose(Vector2Transform child)
        {
            return new Vector2Transform { Value = Value + child.Value };
        }
    }

    private struct Vector2LocalTransform
        : ILocalTransform<Vector2Transform>
    {
        public Vector2Transform Transform { get; set; }
    }

    private struct Vector2WorldTransform
        : IWorldTransform<Vector2Transform>
    {
        public Vector2Transform Transform { get; set; }
        public int Phase { get; set; }
    }
}