using Myriad.ECS.Components;
using Myriad.ECS.Queries;
using Myriad.ECS.Worlds;

namespace Myriad.ECS.Tests;

[TestClass]
public class CachedQueryTests
{
    private static void Check(QueryDescription? q, params Type[] components)
    {
        Assert.IsNotNull(q);
        Assert.AreEqual(components.Length, q.Include.Count);

        foreach (var component in components)
            Assert.IsTrue(q.IsIncluded(component));

        Assert.AreEqual(1, q.Exclude.Count);
        Assert.IsTrue(q.IsExcluded<Phantom>());
    }

    [TestMethod]
    public void GetQuery1()
    {
        var w = new WorldBuilder().Build();

        // Run two queries, which should return identical cached query objects
        var q1 = default(QueryDescription);
        w.Query<Component0>(ref q1);
        var q2 = default(QueryDescription);
        w.Query<Component0>(ref q2);

        // Run another query with different types, should be a different query object
        var q3 = default(QueryDescription);
        w.Query<Component1>(ref q3);

        Assert.AreSame(q1, q2);
        Assert.AreNotSame(q1, q3);
        Check(q1, typeof(Component0));
        Check(q3, typeof(Component1));
    }

    [TestMethod]
    public void GetQuery2()
    {
        var w = new WorldBuilder().Build();

        // Run two queries, which should return identical cached query objects
        var q1 = default(QueryDescription);
        w.Query<Component0, Component1>(ref q1);
        var q2 = default(QueryDescription);
        w.Query<Component1, Component0>(ref q2);

        // Run another query with different types, should be a different query object
        var q3 = default(QueryDescription);
        w.Query<Component0, Component2>(ref q3);

        Assert.AreSame(q1, q2);
        Assert.AreNotSame(q1, q3);
        Check(q1, typeof(Component0), typeof(Component1));
        Check(q3, typeof(Component0), typeof(Component2));
    }

    [TestMethod]
    public void GetQuery3()
    {
        var w = new WorldBuilder().Build();

        // Run two queries, which should return identical cached query objects
        var q1 = default(QueryDescription);
        w.Query<Component0, Component1, Component2>(ref q1);
        var q2 = default(QueryDescription);
        w.Query<Component2, Component1, Component0>(ref q2);

        // Run another query with different types, should be a different query object
        var q3 = default(QueryDescription);
        w.Query<Component0, Component2, Component10>(ref q3);

        Assert.AreSame(q1, q2);
        Assert.AreNotSame(q1, q3);
        Check(q1, typeof(Component0), typeof(Component1), typeof(Component2));
        Check(q3, typeof(Component0), typeof(Component2), typeof(Component10));
    }
}