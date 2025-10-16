using Myriad.ECS.Collections;
using Myriad.ECS.Command;
using Myriad.ECS.Components;
using Myriad.ECS.IDs;
using Myriad.ECS.Queries;
using Myriad.ECS.Tests.Queries;
using Myriad.ECS.Worlds;
using Myriad.ECS.Worlds.Archetypes;

namespace Myriad.ECS.Tests;

[TestClass]
public class QueryDescriptionTests
{
    [TestMethod]
    public void IsIncluded()
    {
        var w = new WorldBuilder().Build();

        var q = new QueryBuilder()
               .Include<ComponentFloat>()
               .Build(w);

        Assert.IsTrue(q.IsIncluded<ComponentFloat>());
        Assert.IsTrue(q.IsIncluded(typeof(ComponentFloat)));
        Assert.IsFalse(q.IsIncluded<ComponentInt32>());
        Assert.IsFalse(q.IsIncluded(typeof(ComponentInt32)));
    }

    [TestMethod]
    public void IsExcluded()
    {
        var w = new WorldBuilder().Build();

        var q = new QueryBuilder()
               .Exclude<ComponentFloat>()
               .Build(w);

        Assert.IsTrue(q.IsExcluded<ComponentFloat>());
        Assert.IsTrue(q.IsExcluded(typeof(ComponentFloat)));
        Assert.IsFalse(q.IsExcluded<ComponentInt32>());
        Assert.IsFalse(q.IsExcluded(typeof(ComponentInt32)));
    }

    [TestMethod]
    public void IsExactlyOneOf()
    {
        var w = new WorldBuilder().Build();

        var q = new QueryBuilder()
               .ExactlyOneOf<ComponentFloat>()
               .ExactlyOneOf<ComponentByte>()
               .Build(w);

        Assert.IsTrue(q.IsExactlyOneOf<ComponentFloat>());
        Assert.IsTrue(q.IsExactlyOneOf(typeof(ComponentFloat)));
        Assert.IsFalse(q.IsExactlyOneOf<ComponentInt32>());
        Assert.IsFalse(q.IsExactlyOneOf(typeof(ComponentInt32)));
    }

    [TestMethod]
    public void IsAtLeastOneOf()
    {
        var w = new WorldBuilder().Build();

        var q = new QueryBuilder()
               .AtLeastOneOf<ComponentFloat>()
               .AtLeastOneOf<ComponentByte>()
               .Build(w);

        Assert.IsTrue(q.IsAtLeastOneOf<ComponentFloat>());
        Assert.IsTrue(q.IsAtLeastOneOf(typeof(ComponentFloat)));
        Assert.IsFalse(q.IsAtLeastOneOf<ComponentInt32>());
        Assert.IsFalse(q.IsAtLeastOneOf(typeof(ComponentInt32)));
    }

    [TestMethod]
    public void IncludeMatchNone()
    {
        var w = new WorldBuilder()
               .WithArchetype<ComponentInt32>()
               .Build();

        var q = new QueryBuilder()
           .Include<ComponentFloat>()
           .Build(w);

        var a = q.GetArchetypes();

        Assert.IsNotNull(a);
        Assert.AreEqual(0, a.Count);
    }

    [TestMethod]
    public void IncludeMatchNoneNonGeneric()
    {
        var w = new WorldBuilder()
           .WithArchetype<ComponentInt32>()
           .Build();

        var q = new QueryBuilder()
           .Include(typeof(ComponentFloat))
           .Build(w);

        var a = q.GetArchetypes();

        Assert.IsNotNull(a);
        Assert.AreEqual(0, a.Count);
    }

    [TestMethod]
    public void IncludeMatchOne()
    {
        var w = new WorldBuilder()
           .WithArchetype<ComponentInt32>()
           .WithArchetype<ComponentFloat>()
           .Build();

        var q = new QueryBuilder()
           .Include<ComponentFloat>()
           .Build(w);

        var a = q.GetArchetypes();

        Assert.IsNotNull(a);
        Assert.AreEqual(1, a.Count);
        Assert.IsNull(a.LINQ().Single().AtLeastOne);
        Assert.IsNull(a.LINQ().Single().ExactlyOne);
    }

    [TestMethod]
    public void IncludeMatchCaching()
    {
        var w = new WorldBuilder()
           .WithArchetype<ComponentInt32>()
           .WithArchetype(typeof(ComponentFloat))
           .Build();

        // Query that matches just one of the archetypes in the world
        var q = new QueryBuilder()
           .Include<ComponentFloat>()
           .Build(w);

        // Match once, check it matches one archetype
        var a = q.GetArchetypes();
        Assert.IsNotNull(a);
        Assert.AreEqual(1, a.Count);
        Assert.IsNull(a.LINQ().Single().AtLeastOne);
        Assert.IsNull(a.LINQ().Single().ExactlyOne);

        // Add an archetype to the world that the query should match
        var c1 = new OrderedListSet<ComponentID>(new HashSet<ComponentID> { ComponentID<ComponentInt32>.ID, ComponentID<ComponentFloat>.ID });
        w.GetOrCreateArchetype(c1, ArchetypeHash.Create(c1));

        // Check it now matches 2 archetypes
        var b = q.GetArchetypes();
        Assert.IsNotNull(b);
        Assert.AreEqual(2, b.Count);
        Assert.IsTrue(a.LINQ().All(x => x.Archetype.Components.Contains(ComponentID<ComponentFloat>.ID)));

        // Add an archetype to the world that the query should NOT match
        var c2 = new OrderedListSet<ComponentID>(new HashSet<ComponentID> { ComponentID<ComponentInt32>.ID, ComponentID<ComponentByte>.ID });
        w.GetOrCreateArchetype(c2, ArchetypeHash.Create(c2));

        // Check it now matches 2 archetypes
        var c = q.GetArchetypes();
        Assert.IsNotNull(c);
        Assert.AreEqual(2, c.Count);
        Assert.IsTrue(c.LINQ().All(x => x.Archetype.Components.Contains(ComponentID<ComponentFloat>.ID)));
    }

    [TestMethod]
    public void IncludeMatchMultiple()
    {
        var w = new WorldBuilder()
           .WithArchetype<ComponentInt32>()
           .WithArchetype<ComponentFloat>()
           .WithArchetype<ComponentFloat, ComponentInt32>()
           .Build();

        var q = new QueryBuilder()
           .Include<ComponentFloat>()
           .Build(w);

        var a = q.GetArchetypes();

        Assert.IsNotNull(a);
        Assert.AreEqual(2, a.Count);

        Assert.IsTrue(a.LINQ().All(x => x.Archetype.Components.Contains(ComponentID<ComponentFloat>.ID)));
    }

    [TestMethod]
    public void IncludeExclude()
    {
        var w = new WorldBuilder()
           .WithArchetype<ComponentInt32>()
           .WithArchetype<ComponentFloat>()
           .WithArchetype<ComponentFloat, ComponentInt32>()
           .Build();

        var q = new QueryBuilder()
            .Include<ComponentFloat>()
            .Exclude<ComponentInt32>()
            .Build(w);

        var a = q.GetArchetypes();

        Assert.IsNotNull(a);
        Assert.AreEqual(1, a.Count);

        var single = a.LINQ().Single();
        Assert.IsNull(single.AtLeastOne);
        Assert.IsNull(single.ExactlyOne);
        Assert.IsTrue(single.Archetype.Components.Contains(ComponentID<ComponentFloat>.ID));
        Assert.IsFalse(single.Archetype.Components.Contains(ComponentID<ComponentInt32>.ID));
    }

    [TestMethod]
    public void ExactlyOne()
    {
        var w = new WorldBuilder()
           .WithArchetype<ComponentInt16>()
           .WithArchetype<ComponentInt32>()
           .WithArchetype<ComponentFloat>()
           .WithArchetype<ComponentFloat, ComponentInt32>()
           .Build();

        var q = new QueryBuilder()
                .ExactlyOneOf<ComponentFloat>()
                .ExactlyOneOf<ComponentInt32>()
                .Build(w);

        var matches = q.GetArchetypes();

        Assert.IsNotNull(matches);
        Assert.AreEqual(2, matches.Count);

        foreach (var match in matches)
        {
            Assert.IsNotNull(match);
            Assert.IsTrue(match.ExactlyOne == ComponentID<ComponentInt32>.ID || match.ExactlyOne == ComponentID<ComponentFloat>.ID);
            Assert.IsTrue(match.AtLeastOne == null);
            Assert.IsTrue(match.Archetype.Components.Count == 1);
        }
    }

    [TestMethod]
    public void AtLeastOne()
    {
        var w = new WorldBuilder()
           .WithArchetype<ComponentInt32>()
           .WithArchetype<ComponentInt32>()
           .WithArchetype<ComponentFloat>()
           .WithArchetype<ComponentFloat, ComponentInt32>()
           .WithArchetype<ComponentInt16, ComponentInt64>()
           .Build();

        var q = new QueryBuilder()
            .AtLeastOneOf<ComponentFloat>()
            .AtLeastOneOf<ComponentInt32>()
            .Build(w);

        var matches = q.GetArchetypes();

        Assert.IsNotNull(matches);
        Assert.AreEqual(3, matches.Count);

        foreach (var match in matches)
        {
            Assert.IsNotNull(match);
            Assert.IsTrue(match.ExactlyOne == null);

            Assert.IsTrue(match.Archetype.Components.Contains(ComponentID<ComponentInt32>.ID)
                       || match.Archetype.Components.Contains(ComponentID<ComponentFloat>.ID));
        }
    }

    [TestMethod]
    public void ExcludePhantoms()
    {
        var w = new WorldBuilder()
               .WithArchetype<ComponentInt32>()
               .WithArchetype<ComponentInt32, Phantom>()
               .WithArchetype<ComponentFloat>()
               .WithArchetype<ComponentFloat, ComponentInt32>()
               .WithArchetype<ComponentInt16, ComponentInt64>()
               .Build();

        var q = new QueryBuilder()
               .Include<ComponentInt32>()
               .Build(w);

        var matches = q.GetArchetypes();

        Assert.IsNotNull(matches);
        Assert.AreEqual(2, matches.Count);

        foreach (var match in matches)
        {
            Assert.IsNotNull(match);
            Assert.IsTrue(match.ExactlyOne == null);

            Assert.IsTrue(match.Archetype.Components.Contains(ComponentID<ComponentInt32>.ID));
        }
    }

    [TestMethod]
    public void IncludePhantoms()
    {
        var w = new WorldBuilder()
               .WithArchetype<ComponentInt32>()
               .WithArchetype<ComponentInt32, Phantom>()
               .WithArchetype<ComponentFloat>()
               .WithArchetype<ComponentFloat, ComponentInt32>()
               .WithArchetype<ComponentInt16, ComponentInt64>()
               .Build();

        var q = new QueryBuilder()
               .Include<ComponentInt32>()
               .Include<Phantom>()
               .Build(w);

        var matches = q.GetArchetypes();

        Assert.IsNotNull(matches);
        Assert.AreEqual(1, matches.Count);

        foreach (var match in matches)
        {
            Assert.IsNotNull(match);
            Assert.IsTrue(match.ExactlyOne == null);

            Assert.IsTrue(match.Archetype.Components.Contains(ComponentID<ComponentInt32>.ID));
        }
    }

    [TestMethod]
    public void First_ThrowsNoMatch()
    {
        var w = new WorldBuilder()
           .Build();

        var q = new QueryBuilder()
               .Include<Component0>()
               .Build(w);

        Assert.ThrowsException<InvalidOperationException>(() => q.First());
    }

    [TestMethod]
    public void First_MatchSingle()
    {
        var w = new WorldBuilder()
           .Build();

        var q = new QueryBuilder()
               .Include<Component0>()
               .Build(w);

        var c = new CommandBuffer(w);
        var eb = c.Create().Set(new Component0());
        using var _ = c.Playback();
        var e = eb.Resolve();

        Assert.AreEqual(e, q.First());
    }

    [TestMethod]
    public void First_MatchMultiple()
    {
        var w = new WorldBuilder()
           .Build();

        var q = new QueryBuilder()
               .Include<Component0>()
               .Build(w);

        var c = new CommandBuffer(w);
        var eb1 = c.Create().Set(new Component0());
        var eb2 = c.Create().Set(new Component0());
        using var _ = c.Playback();
        var e1 = eb1.Resolve();
        var e2 = eb2.Resolve();

        Assert.IsTrue(new[] { e1, e2 }.Contains(q.First()));
    }

    [TestMethod]
    public void FirstOrDefault_NullNoMatch()
    {
        var w = new WorldBuilder()
           .Build();

        var q = new QueryBuilder()
               .Include<Component0>()
               .Build(w);

        Assert.IsNull(q.FirstOrDefault());
    }

    [TestMethod]
    public void FirstOrDefault_MatchSingle()
    {
        var w = new WorldBuilder()
           .Build();

        var q = new QueryBuilder()
               .Include<Component0>()
               .Build(w);

        var c = new CommandBuffer(w);
        var eb = c.Create().Set(new Component0());
        using var _ = c.Playback();
        var e = eb.Resolve();

        Assert.AreEqual(e, q.FirstOrDefault());
    }

    [TestMethod]
    public void FirstOrDefault_SkipsEmptyArchetypes()
    {
        // Create some random entities
        var w = new WorldBuilder().Build();
        TestHelpers.SetupRandomEntities(w).Playback().Dispose();

        // Create a query for entities to count
        var qc = new QueryBuilder()
               .Include<Component0>()
               .Build(w);

        // Get count before deleting
        var count1 = qc.Count();

        // Get the archetypes which this query matches, we'll delete all entities in the
        // first archetype
        var archetypes = qc.GetArchetypes();

        // Create a query for entities to delete
        var qdb = new QueryBuilder();
        foreach (var archetypeComponent in archetypes.LINQ().First().Archetype.Components)
            qdb.Include(archetypeComponent);
        var qd = qdb.Build(w);

        // Delete them
        var deleted = qd.Count();
        Assert.AreNotEqual(0, deleted);

        // Now get the first matched
        Assert.IsNotNull(qc.FirstOrDefault());
    }

    [TestMethod]
    public void FirstOrDefault_MatchMultiple()
    {
        var w = new WorldBuilder()
           .Build();

        var q = new QueryBuilder()
               .Include<Component0>()
               .Build(w);

        var c = new CommandBuffer(w);
        var eb1 = c.Create().Set(new Component0());
        var eb2 = c.Create().Set(new Component0());
        using var _ = c.Playback();
        var e1 = eb1.Resolve();
        var e2 = eb2.Resolve();

        Assert.IsTrue(new[] { e1, e2 }.Contains(q.FirstOrDefault()!.Value));
    }

    [TestMethod]
    public void SingleOrDefault_NullNoMatch()
    {
        var w = new WorldBuilder()
           .Build();

        var q = new QueryBuilder()
               .Include<Component0>()
               .Build(w);

        Assert.IsNull(q.SingleOrDefault());
    }

    [TestMethod]
    public void SingleOrDefault_ThrowsMultipleMatch()
    {
        var w = new WorldBuilder()
           .Build();

        var q = new QueryBuilder()
               .Include<Component0>()
               .Build(w);

        var c = new CommandBuffer(w);
        c.Create().Set(new Component0());
        c.Create().Set(new Component0());
        c.Playback().Dispose();

        Assert.ThrowsException<InvalidOperationException>(() => q.SingleOrDefault());
    }

    [TestMethod]
    public void SingleOrDefault_MatchSingle()
    {
        var w = new WorldBuilder()
           .Build();

        var q = new QueryBuilder()
               .Include<Component0>()
               .Build(w);

        var c = new CommandBuffer(w);
        var eb = c.Create().Set(new Component0());
        using var _ = c.Playback();
        var e = eb.Resolve();

        Assert.AreEqual(e, q.SingleOrDefault()!.Value);
    }

    [TestMethod]
    public void Single_ThrowsMultipleMatch()
    {
        var w = new WorldBuilder()
           .Build();

        var q = new QueryBuilder()
               .Include<Component0>()
               .Build(w);

        var c = new CommandBuffer(w);
        c.Create().Set(new Component0());
        c.Create().Set(new Component0());
        c.Playback().Dispose();

        Assert.ThrowsException<InvalidOperationException>(() => q.Single());
    }

    [TestMethod]
    public void Single_ThrowsNoMatch()
    {
        var w = new WorldBuilder()
           .Build();

        var q = new QueryBuilder()
               .Include<Component0>()
               .Build(w);

        Assert.ThrowsException<InvalidOperationException>(() => q.Single());
    }

    [TestMethod]
    public void Single_MatchSingle()
    {
        var w = new WorldBuilder()
           .Build();

        var q = new QueryBuilder()
               .Include<Component0>()
               .Build(w);

        var c = new CommandBuffer(w);
        var eb = c.Create().Set(new Component0());
        using var _ = c.Playback();
        var e = eb.Resolve();

        Assert.AreEqual(e, q.Single());
    }

    [TestMethod]
    public void Any_True()
    {
        var w = new WorldBuilder()
           .Build();

        var q = new QueryBuilder()
               .Include<Component0>()
               .Build(w);

        var c = new CommandBuffer(w);
        var eb = c.Create().Set(new Component0());
        using var _ = c.Playback();
        var e = eb.Resolve();

        Assert.IsTrue(q.Any());
    }

    [TestMethod]
    public void Any_False()
    {
        var w = new WorldBuilder()
           .Build();

        var q = new QueryBuilder()
               .Include<Component0>()
               .Build(w);

        Assert.IsFalse(q.Any());
    }

    [TestMethod]
    public void Contains_True()
    {
        var w = new WorldBuilder()
           .Build();

        var q = new QueryBuilder()
               .Include<Component0>()
               .Build(w);

        var c = new CommandBuffer(w);
        var eb = c.Create().Set(new Component0());
        using var _ = c.Playback();
        var e = eb.Resolve();

        Assert.IsTrue(q.Contains(e));
    }

    [TestMethod]
    public void Contains_False()
    {
        var w = new WorldBuilder()
           .Build();

        var q = new QueryBuilder()
               .Include<Component0>()
               .Build(w);

        var c = new CommandBuffer(w);
        var eb = c.Create();
        using var _ = c.Playback();
        var e = eb.Resolve();

        Assert.IsFalse(q.Contains(e));
    }

    [TestMethod]
    public void RandomOrDefault_NullNoMatch()
    {
        var w = new WorldBuilder()
           .Build();

        var q = new QueryBuilder()
               .Include<Component0>()
               .Build(w);

        var rng = new Random(123);

        Assert.IsNull(q.RandomOrDefault(rng));
    }

    [TestMethod]
    public void Random_ThrowNoMatch()
    {
        var w = new WorldBuilder()
           .Build();

        var q = new QueryBuilder()
               .Include<Component0>()
               .Build(w);

        var rng = new Random(123);

        Assert.ThrowsException<InvalidOperationException>(() =>
        {
            q.Random(rng);
        });
    }

    [TestMethod]
    public void Random_MatchSingle()
    {
        var w = new WorldBuilder()
           .Build();

        var q = new QueryBuilder()
               .Include<Component0>()
               .Build(w);

        var c = new CommandBuffer(w);
        var eb = c.Create().Set(new Component0());
        using var _ = c.Playback();
        var e = eb.Resolve();

        var r = new Random(123);

        Assert.AreEqual(e, q.RandomOrDefault(r));
    }

    [TestMethod]
    public void Random_MatchRandom()
    {
        var w = new WorldBuilder()
           .Build();

        var q = new QueryBuilder()
               .Include<ComponentInt32>()
               .Build(w);

        var c = new CommandBuffer(w);
        for (var i = 0; i < 10000; i++)
            c.Create().Set(new ComponentInt32(i));
        for (var i = 0; i < 10000; i++)
            c.Create().Set(new ComponentInt32(i)).Set(new Component0());
        for (var i = 0; i < 10000; i++)
            c.Create().Set(new ComponentInt32(i)).Set(new Component1());
        using var resolver = c.Playback();
        var entities = new List<Entity>();
        for (var i = 0; i < resolver.Count; i++)
            entities.Add(resolver[i]);

        var r = new Random(123);

        for (var i = 0; i < 1000; i++)
            Assert.IsTrue(entities.Contains(q.RandomOrDefault(r)!.Value));
    }

    [TestMethod]
    public void IsCountGreaterThan_True()
    {
        var w = new WorldBuilder().Build();
        TestHelpers.SetupRandomEntities(w, count: 10_000).Playback().Dispose();

        var q = new QueryBuilder().Include<Component0>().Include<Component1>().Build(w);

        // Count entities with a query
        var actual = 0;
        w.Query((ChunkHandle h, Span<Component0> _) =>
        {
            actual += h.EntityCount;
        }, q);

        // Check count
        Assert.AreEqual(actual, q.Count());

        // Check threshold
        Assert.IsTrue(q.IsCountGreaterThan(actual - 1));
        Assert.IsTrue(q.IsCountGreaterThan(0));
    }

    [TestMethod]
    public void IsCountGreaterThan_False()
    {
        var w = new WorldBuilder().Build();
        TestHelpers.SetupRandomEntities(w, count: 10_000).Playback().Dispose();

        var q = new QueryBuilder().Include<Component0>().Include<Component1>().Build(w);

        // Count entities with a query
        var actual = 0;
        w.Query((ChunkHandle h, Span<Component0> _) =>
        {
            actual += h.EntityCount;
        }, q);

        // Check count
        Assert.AreEqual(actual, q.Count());

        // Check threshold
        Assert.IsFalse(q.IsCountGreaterThan(actual + 1));

        var q2 = new QueryBuilder()
                .Include<Component0>()
                .Include<NotUsedComponent>().Build(w);

        Assert.IsFalse(q2.IsCountGreaterThan(0));
        Assert.IsFalse(q2.Any());
    }

    private struct NotUsedComponent : IComponent;

    [TestMethod]
    public void CountChunks()
    {
        var w = new WorldBuilder().Build();
        TestHelpers.SetupRandomEntities(w, count: 10_000).Playback().Dispose();

        var q = new QueryBuilder().Include<Component0>().Include<Component1>().Build(w);

        // Count chunks with a query
        var actual = 0;
        w.Query((ChunkHandle h, Span<Component0> _) =>
        {
            actual++;
        }, q);

        // Check count
        Assert.AreEqual(actual, q.CountChunks());
    }
}