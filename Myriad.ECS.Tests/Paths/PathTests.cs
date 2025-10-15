using Myriad.ECS.Command;
using Myriad.ECS.Paths;
using Myriad.ECS.Queries;
using Myriad.ECS.Worlds;
using Path = Myriad.ECS.Paths.Path;

namespace Myriad.ECS.Tests.Paths;

[TestClass]
public class PathTests
{
    [TestMethod]
    public void NoSteps()
    {
        var world = new WorldBuilder().Build();
        var cmd = new CommandBuffer(world);
        var eb = cmd.Create();
        cmd.Playback();
        var e = eb.Resolve();

        var path = new Path();
        Assert.AreEqual(e, path.TryFollow(e));
    }

    [TestMethod]
    public void StartFromDeadEntity()
    {
        // Create 2 entities, with a link from 1 to 2
        var world = new WorldBuilder().Build();
        var cmd = new CommandBuffer(world);
        var eb1 = cmd.Create();
        var eb2 = cmd.Create();
        eb1.Set<Relational1>(new(), eb2);
        using var _ = cmd.Playback();
        var e1 = eb1.Resolve();
        var e2 = eb2.Resolve();

        // Delete e1
        cmd.Delete(e1);
        using var __ = cmd.Playback();

        // Now ensure following a path from e1 fails
        var path = new Path([
            new Path.Follow<GetEntityMapper, Relational1>(new GetEntityMapper())
        ]);
        Assert.AreEqual(default, path.TryFollow(e1));
    }

    [TestMethod]
    public void EndAtDeadEntity()
    {
        // Create 2 entities, with a link from 1 to 2
        var world = new WorldBuilder().Build();
        var cmd = new CommandBuffer(world);
        var eb1 = cmd.Create();
        var eb2 = cmd.Create();
        eb1.Set<Relational1>(new(), eb2);
        using var _ = cmd.Playback();
        var e1 = eb1.Resolve();
        var e2 = eb2.Resolve();

        // Delete e2
        cmd.Delete(e2);
        using var __ = cmd.Playback();

        // Now ensure following a path from e1 fails
        var path = new Path([
            new Path.Follow<GetEntityMapper, Relational1>(new GetEntityMapper())
        ]);
        Assert.AreEqual(default, path.TryFollow(e1));
    }

    [TestMethod]
    public void FollowWithMapper()
    {
        // Create 2 entities, with a link from 1 to 2
        var world = new WorldBuilder().Build();
        var cmd = new CommandBuffer(world);
        var eb1 = cmd.Create();
        var eb2 = cmd.Create();
        eb1.Set<Relational1>(new(), eb2);
        cmd.Playback();
        var e1 = eb1.Resolve();
        var e2 = eb2.Resolve();

        // Follow path from 1 to 2
        var path = new Path([
            new Path.Follow<GetEntityMapper, Relational1>(new GetEntityMapper())
        ]);
        Assert.AreEqual(e2, path.TryFollow(e1));

        // Fail to follow path from 2
        Assert.AreEqual(default, path.TryFollow(e2));
    }

    private struct GetEntityMapper
        : IQueryMap<Entity, Relational1>
    {
        public Entity Execute(Entity e, ref Relational1 t0)
        {
            return t0.Target;
        }
    }

    [TestMethod]
    public void StartFromDeadEntity2()
    {
        // Create 2 entities, with a link from 1 to 2
        var world = new WorldBuilder().Build();
        var cmd = new CommandBuffer(world);
        var eb1 = cmd.Create();
        var eb2 = cmd.Create();
        eb1.Set<Relational1>(new(), eb2);
        eb1.Set<Component0>(new());
        using var _ = cmd.Playback();
        var e1 = eb1.Resolve();
        var e2 = eb2.Resolve();

        // Delete e1
        cmd.Delete(e1);
        using var __ = cmd.Playback();

        // Now ensure following a path from e1 fails
        var path = new Path([
            new Path.Follow<GetEntityMapper2, Relational1, Component0>(new GetEntityMapper2())
        ]);
        Assert.AreEqual(default, path.TryFollow(e1));
    }

    [TestMethod]
    public void EndAtDeadEntity2()
    {
        // Create 2 entities, with a link from 1 to 2
        var world = new WorldBuilder().Build();
        var cmd = new CommandBuffer(world);
        var eb1 = cmd.Create();
        var eb2 = cmd.Create();
        eb1.Set<Relational1>(new(), eb2);
        eb1.Set<Component0>(new());
        using var _ = cmd.Playback();
        var e1 = eb1.Resolve();
        var e2 = eb2.Resolve();

        // Delete e2
        cmd.Delete(e2);
        using var __ = cmd.Playback();

        // Now ensure following a path from e1 fails
        var path = new Path([
            new Path.Follow<GetEntityMapper2, Relational1, Component0>(new GetEntityMapper2())
        ]);
        Assert.AreEqual(default, path.TryFollow(e1));
    }

    [TestMethod]
    public void FollowWithMapper2()
    {
        // Create 2 entities, with a link from 1 to 2
        var world = new WorldBuilder().Build();
        var cmd = new CommandBuffer(world);
        var eb1 = cmd.Create();
        var eb2 = cmd.Create();
        eb1.Set<Relational1>(new(), eb2);
        eb1.Set<Component0>(new());
        cmd.Playback();
        var e1 = eb1.Resolve();
        var e2 = eb2.Resolve();

        // Follow path from 1 to 2
        var path = new Path([
            new Path.Follow<GetEntityMapper2, Relational1, Component0>(new GetEntityMapper2())
        ]);
        Assert.AreEqual(e2, path.TryFollow(e1));

        // Fail to follow path from 2
        Assert.AreEqual(default, path.TryFollow(e2));
    }

    private struct GetEntityMapper2
        : IQueryMap<Entity, Relational1, Component0>
    {
        public Entity Execute(Entity e, ref Relational1 t0, ref Component0 t1)
        {
            return t0.Target;
        }
    }

    [TestMethod]
    public void OneRelationalStep()
    {
        // Create A -> B (Relation1) & A -> C (Relational2)
        var world = new WorldBuilder().Build();
        var cmd = new CommandBuffer(world);
        var eb1 = cmd.Create();
        var eb2 = cmd.Create();
        var eb3 = cmd.Create();
        eb1.Set(new Relational1(), eb2);
        eb1.Set(new Relational2(), eb3);
        cmd.Playback();
        var e1 = eb1.Resolve();
        var e2 = eb2.Resolve();

        // Follow A -> B
        var path = new Path(
            new Path.FollowRelation<Relational1>()
        );
        var end = path.TryFollow(e1);
        Assert.AreEqual(e2, end);

        // Follow A -> B with specialised relational path
        var pathR = new Path<Relational1>();
        var endR = pathR.TryFollow(e1);
        Assert.AreEqual(e2, endR);

        // Try to follow a link that doesn't exist
        var pathR2 = new Path<Relational3>();
        var endR2 = pathR2.TryFollow(e1);
        Assert.AreEqual(default, endR2);
    }

    [TestMethod]
    public void TwoRelationalSteps()
    {
        // Create A -> B -> C
        var world = new WorldBuilder().Build();
        var cmd = new CommandBuffer(world);
        var eb1 = cmd.Create();
        var eb2 = cmd.Create();
        var eb3 = cmd.Create();
        eb1.Set(new Relational1(), eb2);
        eb2.Set(new Relational2(), eb3);
        cmd.Playback();
        var e1 = eb1.Resolve();
        var e2 = eb2.Resolve();
        var e3 = eb3.Resolve();

        // Create A -> B -> C
        var path = new Path(
            new Path.FollowRelation<Relational1>(),
            new Path.FollowRelation<Relational2>()
        );
        var end = path.TryFollow(e1);
        Assert.AreEqual(e3, end);

        // Follow A -> B -> C with specialised relational path
        var pathR = new Path<Relational1, Relational2>();
        var endR = pathR.TryFollow(e1);
        Assert.AreEqual(e3, endR);
    }

    [TestMethod]
    public void TwoRelationalSteps_MissingRelation()
    {
        // Create A -> B -> C
        var world = new WorldBuilder().Build();
        var cmd = new CommandBuffer(world);
        var eb1 = cmd.Create();
        var eb2 = cmd.Create();
        var eb3 = cmd.Create();
        eb1.Set(new Relational1(), eb2);
        eb2.Set(new Relational2(), eb3);
        cmd.Playback();
        var e1 = eb1.Resolve();
        var e2 = eb2.Resolve();
        var e3 = eb3.Resolve();

        // Follow A -> B -> C
        var path = new Path(
            new Path.FollowRelation<Relational1>(),
            new Path.FollowRelation<Relational1>()
        );

        // We followed R1, but B -> C doesn't have that relation
        var end = path.TryFollow(e1);
        Assert.IsFalse(end.HasValue);

        // Follow with specialised relational path
        var pathR = new Path<Relational1, Relational1>();
        var endR = pathR.TryFollow(e1);
        Assert.IsFalse(endR.HasValue);
    }

    [TestMethod]
    public void TwoRelationalSteps_StartDeadEntity()
    {
        // Create A -> B -> C
        var world = new WorldBuilder().Build();
        var cmd = new CommandBuffer(world);
        var eb1 = cmd.Create();
        var eb2 = cmd.Create();
        var eb3 = cmd.Create();
        eb1.Set(new Relational1(), eb2);
        eb2.Set(new Relational2(), eb3);
        cmd.Playback();
        var e1 = eb1.Resolve();
        var e2 = eb2.Resolve();
        var e3 = eb3.Resolve();

        // Delete A
        cmd.Delete(e1);
        cmd.Playback();

        // Follow A -> B -> C
        // Nest steps to mix things up a bit
        var path = new Path(
            Path.Nested.Create(new Path<Relational1>()),
            Path.Nested.Create(new Path<Relational2>())
        );

        // Check that it failed
        var end = path.TryFollow(e1);
        Assert.IsFalse(end.HasValue);

        // Follow with specialised relational path
        var pathR = new Path<Relational1, Relational2>();
        var endR = pathR.TryFollow(e1);
        Assert.IsFalse(endR.HasValue);
    }

    [TestMethod]
    public void TwoRelationalSteps_MidDeadEntity()
    {
        // Create A -> B -> C
        var world = new WorldBuilder().Build();
        var cmd = new CommandBuffer(world);
        var eb1 = cmd.Create();
        var eb2 = cmd.Create();
        var eb3 = cmd.Create();
        eb1.Set(new Relational1(), eb2);
        eb2.Set(new Relational2(), eb3);
        cmd.Playback();
        var e1 = eb1.Resolve();
        var e2 = eb2.Resolve();
        var e3 = eb3.Resolve();

        // Delete B
        cmd.Delete(e2);
        cmd.Playback();

        // Follow A -> B -> C
        var path = new Path(
            new Path.FollowRelation<Relational1>(),
            new Path.FollowRelation<Relational2>()
        );

        // Check that it failed
        var end = path.TryFollow(e1);
        Assert.IsFalse(end.HasValue);

        // Follow with specialised relational path
        var pathR = new Path<Relational1, Relational2>();
        var endR = pathR.TryFollow(e1);
        Assert.IsFalse(endR.HasValue);
    }

    [TestMethod]
    public void TwoRelationalSteps_FinalDeadEntity()
    {
        // Create A -> B -> C
        var world = new WorldBuilder().Build();
        var cmd = new CommandBuffer(world);
        var eb1 = cmd.Create();
        var eb2 = cmd.Create();
        var eb3 = cmd.Create();
        eb1.Set(new Relational1(), eb2);
        eb2.Set(new Relational2(), eb3);
        cmd.Playback();
        var e1 = eb1.Resolve();
        var e2 = eb2.Resolve();
        var e3 = eb3.Resolve();

        // Delete B
        cmd.Delete(e3);
        cmd.Playback();

        // Follow A -> B -> C
        // Nest steps to mix things up a bit
        var path = new Path(
            Path.Nested.Create(new Path<Relational1>()),
            Path.Nested.Create(new Path<Relational2>())
        );

        // Check that it failed
        var end = path.TryFollow(e1);
        Assert.IsFalse(end.HasValue);

        // Follow with specialised relational path
        var pathR = new Path<Relational1, Relational2>();
        var endR = pathR.TryFollow(e1);
        Assert.IsFalse(endR.HasValue);
    }

    [TestMethod]
    public void PredicateStep()
    {
        // Create A
        var world = new WorldBuilder().Build();
        var cmd = new CommandBuffer(world);
        var eb1 = cmd.Create().Set(new Component0());
        cmd.Playback();
        var e1 = eb1.Resolve();

        // Follow A (with predicate)
        var pathTrue = new Path(new Path.Predicate<MapReturnConst<Component0>, Component0>(new MapReturnConst<Component0>(true)));
        var pathFalse = new Path(new Path.Predicate<MapReturnConst<Component0>, Component0>(new MapReturnConst<Component0>(false)));

        Assert.AreEqual(e1, pathTrue.TryFollow(e1));
        Assert.AreEqual(default, pathFalse.TryFollow(e1));
    }

    [TestMethod]
    public void PredicateStep2()
    {
        // Create A
        var world = new WorldBuilder().Build();
        var cmd = new CommandBuffer(world);
        var eb1 = cmd.Create().Set(new Component0()).Set(new Component1());
        cmd.Playback();
        var e1 = eb1.Resolve();

        // Follow A (with predicate)
        var pathTrue = new Path(new Path.Predicate<MapReturnConst<Component0, Component1>, Component0, Component1>(new MapReturnConst<Component0, Component1>(true)));
        var pathFalse = new Path(new Path.Predicate<MapReturnConst<Component0, Component1>, Component0, Component1>(new MapReturnConst<Component0, Component1>(false)));

        Assert.AreEqual(e1, pathTrue.TryFollow(e1));
        Assert.AreEqual(default, pathFalse.TryFollow(e1));
    }

    [TestMethod]
    public void PredicateStep_MissingComponent()
    {
        // Create A
        var world = new WorldBuilder().Build();
        var cmd = new CommandBuffer(world);
        var eb1 = cmd.Create().Set(new Component0());
        cmd.Playback();
        var e1 = eb1.Resolve();

        // Follow A (with predicate)
        var pathTrue = new Path(new Path.Predicate<MapReturnConst<Component1>, Component1>(new MapReturnConst<Component1>(true)));
        var pathFalse = new Path(new Path.Predicate<MapReturnConst<Component1>, Component1>(new MapReturnConst<Component1>(false)));

        Assert.AreEqual(default, pathTrue.TryFollow(e1));
        Assert.AreEqual(default, pathFalse.TryFollow(e1));
    }

    [TestMethod]
    public void PredicateStep_MissingComponent2()
    {
        // Create A
        var world = new WorldBuilder().Build();
        var cmd = new CommandBuffer(world);
        var eb1 = cmd.Create().Set(new Component0());
        cmd.Playback();
        var e1 = eb1.Resolve();

        // Follow A (with predicate)
        var pathTrue = new Path(new Path.Predicate<MapReturnConst<Component0, Component1>, Component0, Component1>(new MapReturnConst<Component0, Component1>(true)));
        var pathFalse = new Path(new Path.Predicate<MapReturnConst<Component0, Component1>, Component0, Component1>(new MapReturnConst<Component0, Component1>(false)));

        Assert.AreEqual(default, pathTrue.TryFollow(e1));
        Assert.AreEqual(default, pathFalse.TryFollow(e1));
    }

    [TestMethod]
    public void PredicateStep_Dead()
    {
        // Create A
        var world = new WorldBuilder().Build();
        var cmd = new CommandBuffer(world);
        var eb1 = cmd.Create().Set(new Component0());
        cmd.Playback();
        var e1 = eb1.Resolve();
        cmd.Delete(e1);
        cmd.Playback();

        // Follow A (with predicate)
        var pathTrue = new Path(new Path.Predicate<MapReturnConst<Component0>, Component0>(new MapReturnConst<Component0>(true)));
        var pathFalse = new Path(new Path.Predicate<MapReturnConst<Component0>, Component0>(new MapReturnConst<Component0>(false)));

        Assert.AreEqual(default, pathTrue.TryFollow(e1));
        Assert.AreEqual(default, pathFalse.TryFollow(e1));
    }

    [TestMethod]
    public void PredicateStep_Dead2()
    {
        // Create A
        var world = new WorldBuilder().Build();
        var cmd = new CommandBuffer(world);
        var eb1 = cmd.Create().Set(new Component0()).Set(new Component1());
        cmd.Playback();
        var e1 = eb1.Resolve();
        cmd.Delete(e1);
        cmd.Playback();

        // Follow A (with predicate)
        var pathTrue = new Path(new Path.Predicate<MapReturnConst<Component0, Component1>, Component0, Component1>(new MapReturnConst<Component0, Component1>(true)));
        var pathFalse = new Path(new Path.Predicate<MapReturnConst<Component0, Component1>, Component0, Component1>(new MapReturnConst<Component0, Component1>(false)));

        Assert.AreEqual(default, pathTrue.TryFollow(e1));
        Assert.AreEqual(default, pathFalse.TryFollow(e1));
    }

    private readonly struct MapReturnConst<T>
        : IQueryMap<bool, T>
        where T : IComponent
    {
        private readonly bool _value;

        public MapReturnConst(bool value)
        {
            _value = value;
        }

        public bool Execute(Entity e, ref T t0)
        {
            return _value;
        }
    }

    private readonly struct MapReturnConst<T0, T1>
        : IQueryMap<bool, T0, T1>
        where T1 : IComponent
        where T0 : IComponent
    {
        private readonly bool _value;

        public MapReturnConst(bool value)
        {
            _value = value;
        }

        public bool Execute(Entity e, ref T0 t0, ref T1 t1)
        {
            return _value;
        }
    }
}