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
    public void SingleEntity()
    {
        var w = new WorldBuilder().Build();
        var c = new CommandBuffer(w);
        var eb = c.Create()
            .Set(new Vector2LocalTransform { Transform = new(Vector2.One) })
            .Set(new Vector2WorldTransform());

        using var _ = c.Playback();
        var e = eb.Resolve();

        // Run update and check
        new TransformAddIntegers(w).Update(new GameTime());
        Assert.AreEqual(Vector2.One, e.GetComponentRef<Vector2LocalTransform>().Transform.Value);
        Assert.AreEqual(Vector2.One, e.GetComponentRef<Vector2WorldTransform>().Transform.Value);

        // Change local transform
        e.GetComponentRef<Vector2LocalTransform>().Transform = new Vector2Transform(new(3, 4));

        // Run update and check
        new TransformAddIntegers(w).Update(new GameTime());
        Assert.AreEqual(new Vector2(3, 4), e.GetComponentRef<Vector2LocalTransform>().Transform.Value);
        Assert.AreEqual(new Vector2(3, 4), e.GetComponentRef<Vector2WorldTransform>().Transform.Value);
    }

    [TestMethod]
    public void SingleParent()
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
    public void SingleParentWithNoLocalTransform()
    {
        var w = new WorldBuilder().Build();
        var c = new CommandBuffer(w);

        var eb1 = c.Create()
                   .Set(new Vector2WorldTransform { Transform = new(Vector2.UnitX) });

        var eb2 = c.Create()
                   .Set(new Vector2LocalTransform { Transform = new(Vector2.UnitY) })
                   .Set(new Vector2WorldTransform())
                   .Set(new TransformParent(), eb1);

        using var _ = c.Playback();
        var parent = eb1.Resolve();
        var child = eb2.Resolve();

        new TransformAddIntegers(w).Update(new GameTime());

        Assert.AreEqual(Vector2.UnitX, parent.GetComponentRef<Vector2WorldTransform>().Transform.Value);

        Assert.AreEqual(Vector2.UnitY, child.GetComponentRef<Vector2LocalTransform>().Transform.Value);
        Assert.AreEqual(Vector2.One, child.GetComponentRef<Vector2WorldTransform>().Transform.Value);
    }

    [TestMethod]
    public void DestroyedParent()
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

        c.Delete(parent);
        c.Playback().Dispose();

        new TransformAddIntegers(w).Update(new GameTime());

        Assert.AreEqual(Vector2.UnitY, child.GetComponentRef<Vector2LocalTransform>().Transform.Value);
        Assert.AreEqual(Vector2.UnitY, child.GetComponentRef<Vector2WorldTransform>().Transform.Value);
    }

    [TestMethod]
    public void DestroyedPhantomParent()
    {
        var w = new WorldBuilder().Build();
        var c = new CommandBuffer(w);

        var eb1 = c.Create()
                   .Set(new Vector2LocalTransform { Transform = new(Vector2.UnitX) })
                   .Set(new Vector2WorldTransform { Transform = new(new(10, 20)) })
                   .Set(new TestPhantom0());

        var eb2 = c.Create()
                   .Set(new Vector2LocalTransform { Transform = new(Vector2.UnitY) })
                   .Set(new Vector2WorldTransform())
                   .Set(new TransformParent(), eb1);

        using var _ = c.Playback();
        var parent = eb1.Resolve();
        var child = eb2.Resolve();

        c.Delete(parent);
        c.Playback().Dispose();

        new TransformAddIntegers(w).Update(new GameTime());

        Assert.AreEqual(Vector2.UnitY, child.GetComponentRef<Vector2LocalTransform>().Transform.Value);
        Assert.AreEqual(Vector2.UnitY, child.GetComponentRef<Vector2WorldTransform>().Transform.Value);
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

    [TestMethod]
    public void MultipleChainedChildrenInLoop()
    {
        var w = new WorldBuilder().Build();
        var c = new CommandBuffer(w);

        var eb1 = c.Create()
                   .Set(new Vector2LocalTransform { Transform = new(Vector2.UnitX) })
                   .Set(new Vector2WorldTransform());

        var eb2 = c.Create()
                   .Set(new Vector2LocalTransform { Transform = new(Vector2.UnitX) })
                   .Set(new Vector2WorldTransform());

        var eb3 = c.Create()
                   .Set(new Vector2LocalTransform { Transform = new(Vector2.UnitX) })
                   .Set(new Vector2WorldTransform());

        eb1.Set(new TransformParent(), eb3);
        eb2.Set(new TransformParent(), eb1);
        eb3.Set(new TransformParent(), eb2);

        using var _ = c.Playback();
        var e1 = eb1.Resolve();
        var e2 = eb2.Resolve();
        var e3 = eb3.Resolve();

        var sys = new TransformAddIntegers(w);
        sys.Update(new GameTime());
        Assert.AreEqual(1, sys.LoopCount);

        // Since we have a loop it's indeterminate what the final transform will actually be (i.e. which is treated as the root).
        // However, all of the local transforms are the same (UnitX), so all we need to do is make sure the result is 3 * UnitX
        // for one of them.

        Assert.AreEqual(Vector2.UnitX, e1.GetComponentRef<Vector2LocalTransform>().Transform.Value);
        Assert.AreEqual(Vector2.UnitX, e2.GetComponentRef<Vector2LocalTransform>().Transform.Value);
        Assert.AreEqual(Vector2.UnitX, e3.GetComponentRef<Vector2LocalTransform>().Transform.Value);

        var wtransforms = new[]
        {
            e1.GetComponentRef<Vector2WorldTransform>().Transform.Value,
            e2.GetComponentRef<Vector2WorldTransform>().Transform.Value,
            e3.GetComponentRef<Vector2WorldTransform>().Transform.Value,
        };

        // One of the entities will have been treated as the root, find which
        var root = Array.IndexOf(wtransforms, Vector2.UnitX);

        // Check they're all offset the appropriate amount from the root (whichever that is)
        Assert.AreEqual(1 * Vector2.UnitX, wtransforms[(root + 0) % wtransforms.Length]);
        Assert.AreEqual(2 * Vector2.UnitX, wtransforms[(root + 1) % wtransforms.Length]);
        Assert.AreEqual(3 * Vector2.UnitX, wtransforms[(root + 2) % wtransforms.Length]);
    }

    private class TransformAddIntegers
        : BaseUpdateTransformHierarchySystem<GameTime, Vector2Transform, Vector2LocalTransform, Vector2WorldTransform>
    {
        public int LoopCount;

        public TransformAddIntegers(World world)
            : base(world)
        {
        }

        protected override void LoopDetected(Entity entity)
        {
            LoopCount++;
            base.LoopDetected(entity);
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