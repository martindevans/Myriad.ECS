using Myriad.ECS.Collections;
using Myriad.ECS.Command;
using Myriad.ECS.Components;
using Myriad.ECS.IDs;
using Myriad.ECS.Queries;
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
    public void FirstOrDefault()
    {
        var w = new WorldBuilder()
            .Build();
        var buffer = new CommandBuffer(w);

        var q = new QueryBuilder()
            .Include<Component0>()
            .Build(w);

        var notFound = q.FirstOrDefault();
        Assert.IsNull(notFound);

        var e = buffer.Create().Set(new Component0());
        using var resolver = buffer.Playback();

        var found = q.FirstOrDefault();
        Assert.IsNotNull(found);
        Assert.AreEqual(found, e.Resolve());
    }

    [TestMethod]
    public void Single()
    {
        var w = new WorldBuilder()
            .Build();
        var buffer = new CommandBuffer(w);

        var q = new QueryBuilder()
            .Include<Component0>()
            .Build(w);

        Assert.ThrowsException<InvalidOperationException>(() => q.Single(), "Expected throw for no matches.");

        var e0 = buffer.Create().Set(new Component0());
        using var resolver0 = buffer.Playback();

        var found = q.Single();
        Assert.AreEqual(found, e0.Resolve());

        var e1 = buffer.Create().Set(new Component0()).Set(new Component1());
        buffer.Playback();

        Assert.ThrowsException<InvalidOperationException>(() => q.Single(), "Expected throw due to multiple matched entities.");
    }
}