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
    public void OneRelationalStep()
    {
        // Create A -> B -> C
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