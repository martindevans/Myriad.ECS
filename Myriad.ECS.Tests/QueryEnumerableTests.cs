using Myriad.ECS.Command;
using Myriad.ECS.Queries;
using Myriad.ECS.Worlds;

namespace Myriad.ECS.Tests;

[TestClass]
public class QueryEnumerableTests
{
    [TestMethod]
    public void MatchNothingInEmptyWorld()
    {
        var w = new WorldBuilder()
           .WithArchetype<ComponentInt32>()
           .Build();

        var q = new QueryBuilder()
               .Include<ComponentInt64>()
               .Include<ComponentFloat>()
               .Build(w);

        foreach (var _ in w.Query<ComponentInt64, ComponentFloat>(q))
            Assert.Fail("should not match anything");
    }

    [TestMethod]
    public void MatchNothingInWorld()
    {
        var w = new WorldBuilder()
               .WithArchetype<ComponentInt32>()
               .Build();

        var cb = new CommandBuffer(w);
        cb.Create().Set(new ComponentInt64());
        cb.Create().Set(new ComponentFloat());
        using var r = cb.Playback();

        var q = new QueryBuilder()
               .Include<ComponentInt64>()
               .Include<ComponentFloat>()
               .Build(w);

        foreach (var _ in w.Query<ComponentInt64, ComponentFloat>(q))
            Assert.Fail("should not match anything");
    }

    [TestMethod]
    public void MatchEntityInWorld()
    {
        var w = new WorldBuilder()
               .WithArchetype<ComponentInt32>()
               .Build();

        var cb = new CommandBuffer(w);
        cb.Create().Set(new ComponentInt64(1));
        cb.Create().Set(new ComponentFloat(2));
        var e3 = cb.Create().Set(new ComponentFloat(3)).Set(new ComponentInt64(4));
        using var r = cb.Playback();
        var entity3 = r.Resolve(e3);

        var count = 0;
        foreach (var e in w.Query<ComponentInt64, ComponentFloat>())
        {
            Assert.AreEqual(entity3, e.Entity);
            Assert.AreEqual(4, e.Item0.Value);
            Assert.AreEqual(3, e.Item1.Value);
            count++;
        }

        Assert.AreEqual(1, count);
    }

    [TestMethod]
    public void MatchMultipleEntitiesInWorld()
    {
        var w = new WorldBuilder()
               .WithArchetype<ComponentInt32>()
               .Build();

        var cb = new CommandBuffer(w);
        cb.Create().Set(new ComponentFloat(1)).Set(new ComponentInt64(2));
        cb.Create().Set(new ComponentFloat(3)).Set(new ComponentInt64(6));
        cb.Create().Set(new ComponentFloat(4)).Set(new ComponentInt64(8));
        using var r = cb.Playback();

        var q = new QueryBuilder()
               .Include<ComponentInt64>()
               .Include<ComponentFloat>()
               .Build(w);

        var count = 0;
        foreach (var e in w.Query<ComponentInt64, ComponentFloat>(q))
        {
            Assert.AreEqual(e.Item1.Value * 2, e.Item0.Value);
            count++;
        }

        Assert.AreEqual(3, count);
    }

    [TestMethod]
    public void MatchManyEntitiesInWorld()
    {
        var w = new WorldBuilder()
               .WithArchetype<ComponentInt32>()
               .Build();

        // Create lots of random entities, keeping track of how many _should_ match the query
        var expectedIndices = new HashSet<int>();
        var rng = new Random(235);
        var cb = new CommandBuffer(w);
        for (var i = 0; i < 100_000; i++)
        {
            var entity = cb.Create();

            var hasFloat = false;
            var hasInt32 = false;
            var hasInt64 = false;
            for (var j = 0; j < 5; j++)
            {
                switch (rng.Next(0, 5))
                {
                    case 0: entity.Set(new ComponentByte((byte)i), true); break;
                    case 1: entity.Set(new ComponentInt16((short)i), true); break;

                    case 2:
                        entity.Set(new ComponentFloat(i), true);
                        hasFloat = true;
                        break;

                    case 3:
                        entity.Set(new ComponentInt32(i), true);
                        hasInt32 = true;
                        break;

                    case 4:
                        entity.Set(new ComponentInt64(i), true);
                        hasInt64 = true;
                        break;
                }
            }

            if (hasFloat && hasInt32 && hasInt64)
                expectedIndices.Add(i);
        }
        using var r = cb.Playback();

        var actualIndices = new HashSet<int>();
        foreach (var (_, f, i32, i64) in w.Query<ComponentFloat, ComponentInt32, ComponentInt64>())
        {
            Assert.AreEqual(f.Ref.Value, i32.Ref.Value);
            Assert.AreEqual(f.Ref.Value, i64.Ref.Value);
            actualIndices.Add(i32.Ref.Value);
        }

        Assert.IsTrue(expectedIndices.SetEquals(actualIndices));
    }
}