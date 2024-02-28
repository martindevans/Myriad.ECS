using Myriad.ECS.Collections;
using Myriad.ECS.IDs;
using Myriad.ECS.Queries;
using Myriad.ECS.Registry;
using Myriad.ECS.Worlds;
using Myriad.ECS.Worlds.Archetypes;

namespace Myriad.ECS.Tests;

[TestClass]
public class QueryTests
{
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
        Assert.IsNull(a.Single().AtLeastOne);
        Assert.IsNull(a.Single().ExactlyOne);
    }

    [TestMethod]
    public void IncludeMatchCaching()
    {
        var w = new WorldBuilder()
           .WithArchetype<ComponentInt32>()
           .WithArchetype(typeof(ComponentFloat))
           .Build();

        var q = new QueryBuilder()
           .Include<ComponentFloat>()
           .Build(w);

        // Match once, check it matches one archetype
        var a = q.GetArchetypes();
        Assert.IsNotNull(a);
        Assert.AreEqual(1, a.Count);
        Assert.IsNull(a.Single().AtLeastOne);
        Assert.IsNull(a.Single().ExactlyOne);

        // Add an archetype to the world
        var c = new OrderedListSet<ComponentID>(new HashSet<ComponentID> { ComponentRegistry.Get<ComponentInt32>(), ComponentRegistry.Get<ComponentFloat>() });
        w.GetOrCreateArchetype(c, ArchetypeHash.Create(c));

        var b = q.GetArchetypes();
        Assert.IsNotNull(b);
        Assert.AreEqual(2, b.Count);
        Assert.IsTrue(a.All(x => x.Archetype.Components.Contains(ComponentRegistry.Get<ComponentFloat>())));
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

        Assert.IsTrue(a.All(x => x.Archetype.Components.Contains(ComponentRegistry.Get<ComponentFloat>())));
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

        var single = a.Single();
        Assert.IsNull(single.AtLeastOne);
        Assert.IsNull(single.ExactlyOne);
        Assert.IsTrue(single.Archetype.Components.Contains(ComponentRegistry.Get<ComponentFloat>()));
        Assert.IsFalse(single.Archetype.Components.Contains(ComponentRegistry.Get<ComponentInt32>()));
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
            Assert.IsTrue(match.ExactlyOne == ComponentRegistry.Get<ComponentInt32>() || match.ExactlyOne == ComponentRegistry.Get<ComponentFloat>());
            Assert.IsTrue(match.AtLeastOne == null);
            Assert.IsTrue(match.Archetype.Components.Count == 1);
        }
    }

    [TestMethod]
    public void AtLeastOne()
    {
        var w = new WorldBuilder()
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

            Assert.IsTrue(match.Archetype.Components.Contains(ComponentRegistry.Get<ComponentInt32>())
                       || match.Archetype.Components.Contains(ComponentRegistry.Get<ComponentFloat>()));
        }
    }
}