using Myriad.ECS.Queries;
using Myriad.ECS.Registry;

namespace Myriad.ECS.Tests;

[TestClass]
public class QueryTests
{
    [TestMethod]
    public void IncludeMatchNone()
    {
        var w = new World()
        {
            _archetypes =
            [
                new Archetype
                {
                    Components = [ComponentRegistry.Get<ComponentInt32>()]
                },
            ]
        };

        var qb = new QueryBuilder()
           .Include<ComponentFloat>();

        var q = qb.Build(w);

        var a = q.GetArchetypes();

        Assert.IsNotNull(a);
        Assert.AreEqual(0, a.Count);
    }

    [TestMethod]
    public void IncludeMatchOne()
    {
        var w = new World()
        {
            _archetypes =
            [
                new Archetype { Components = [ComponentRegistry.Get<ComponentInt32>()] },
                new Archetype { Components = [ComponentRegistry.Get<ComponentFloat>()] },
            ]
        };

        var qb = new QueryBuilder()
           .Include<ComponentFloat>();

        var q = qb.Build(w);

        var a = q.GetArchetypes();

        Assert.IsNotNull(a);
        Assert.AreEqual(1, a.Count);
        Assert.IsNull(a.Single().AtLeastOne);
        Assert.IsNull(a.Single().ExactlyOne);
    }

    [TestMethod]
    public void IncludeMatchMultiple()
    {
        var w = new World()
        {
            _archetypes =
            [
                new Archetype { Components = [ComponentRegistry.Get<ComponentInt32>()] },
                new Archetype { Components = [ComponentRegistry.Get<ComponentFloat>()] },
                new Archetype { Components = [ComponentRegistry.Get<ComponentFloat>(), ComponentRegistry.Get<ComponentInt32>()] },
            ]
        };

        var qb = new QueryBuilder()
           .Include<ComponentFloat>();

        var q = qb.Build(w);

        var a = q.GetArchetypes();

        Assert.IsNotNull(a);
        Assert.AreEqual(2, a.Count);

        Assert.IsTrue(a.All(x => x.Archetype.Components.Contains(ComponentRegistry.Get<ComponentFloat>())));
    }

    [TestMethod]
    public void IncludeExclude()
    {
        var w = new World()
        {
            _archetypes =
            [
                new Archetype { Components = [ComponentRegistry.Get<ComponentInt32>()] },
                new Archetype { Components = [ComponentRegistry.Get<ComponentFloat>()] },
                new Archetype { Components = [ComponentRegistry.Get<ComponentFloat>(), ComponentRegistry.Get<ComponentInt32>()] },
            ]
        };

        var qb = new QueryBuilder()
            .Include<ComponentFloat>()
            .Exclude<ComponentInt32>();

        var q = qb.Build(w);

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
        var w = new World()
        {
            _archetypes =
            [
                new Archetype { Components = [ComponentRegistry.Get<ComponentInt32>()] },
                new Archetype { Components = [ComponentRegistry.Get<ComponentFloat>()] },
                new Archetype { Components = [ComponentRegistry.Get<ComponentFloat>(), ComponentRegistry.Get<ComponentInt32>()] },
                new Archetype { Components = [ComponentRegistry.Get<ComponentInt16>()] },
            ]
        };

        var qb = new QueryBuilder()
                .ExactlyOneOf<ComponentFloat>()
                .ExactlyOneOf<ComponentInt32>();

        var q = qb.Build(w);

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
        var w = new World()
        {
            _archetypes =
            [
                new Archetype { Components = [ComponentRegistry.Get<ComponentInt32>()] },
                new Archetype { Components = [ComponentRegistry.Get<ComponentFloat>()] },
                new Archetype { Components = [ComponentRegistry.Get<ComponentFloat>(), ComponentRegistry.Get<ComponentInt32>()] },
                new Archetype { Components = [ComponentRegistry.Get<ComponentInt16>(), ComponentRegistry.Get<ComponentInt64>()] },
            ]
        };

        var qb = new QueryBuilder()
                .AtLeastOneOf<ComponentFloat>()
                .AtLeastOneOf<ComponentInt32>();

        var q = qb.Build(w);

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