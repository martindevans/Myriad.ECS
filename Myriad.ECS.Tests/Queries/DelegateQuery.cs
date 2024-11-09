using Myriad.ECS.Components;
using Myriad.ECS.Queries;
using Myriad.ECS.Worlds;

namespace Myriad.ECS.Tests.Queries;

[TestClass]
public class DelegateQuery
{
    [TestMethod]
    public void RefQueryEntity()
    {
        var w = new WorldBuilder().Build();
        TestHelpers.SetupRandomEntities(w, count:10_000).Playback().Dispose();

        var q = default(QueryDescription);

        // Set all to entity ID
        w.Query((Entity e, ref ComponentInt32 i) =>
        {
            i.Value = e.ID.ID;
        }, ref q);

        // Check all are correct
        foreach (var (e, i) in w.Query<ComponentInt32>())
        {
            Assert.AreEqual(e.ID.ID, i.Ref.Value);
        }

        // Check the returned query is correct
        Assert.IsNotNull(q);
        Assert.IsTrue(q.IsIncluded<ComponentInt32>());
        Assert.AreEqual(1, q.Include.Count);
        Assert.IsTrue(q.IsExcluded<Phantom>());
        Assert.AreEqual(1, q.Exclude.Count);
        Assert.AreEqual(0, q.ExactlyOneOf.Count);
        Assert.AreEqual(0, q.AtLeastOneOf.Count);
    }

    [TestMethod]
    public void RefQueryEntityWithData()
    {
        var w = new WorldBuilder().Build();
        TestHelpers.SetupRandomEntities(w, count: 10_000).Playback().Dispose();

        var q = default(QueryDescription);

        // Set all to entity ID
        w.Query(9, (int data, Entity e, ref ComponentInt32 i) =>
        {
            Assert.AreEqual(9, data);
            i.Value = e.ID.ID;
        }, ref q);

        // Check all are correct
        foreach (var (e, i) in w.Query<ComponentInt32>())
        {
            Assert.AreEqual(e.ID.ID, i.Ref.Value);
        }

        // Check the returned query is correct
        Assert.IsNotNull(q);
        Assert.IsTrue(q.IsIncluded<ComponentInt32>());
        Assert.AreEqual(1, q.Include.Count);
        Assert.IsTrue(q.IsExcluded<Phantom>());
        Assert.AreEqual(1, q.Exclude.Count);
        Assert.AreEqual(0, q.ExactlyOneOf.Count);
        Assert.AreEqual(0, q.AtLeastOneOf.Count);
    }

    [TestMethod]
    public void RefQueryNoEntity()
    {
        var w = new WorldBuilder().Build();
        TestHelpers.SetupRandomEntities(w, count: 10_000).Playback().Dispose();

        var q = default(QueryDescription);

        // Set all to 123
        w.Query((ref ComponentInt32 i) =>
        {
            i.Value = 123;
        }, ref q);

        // Check all are correct
        foreach (var (_, i) in w.Query<ComponentInt32>())
        {
            Assert.AreEqual(123, i.Ref.Value);
        }

        // Check the returned query is correct
        Assert.IsNotNull(q);
        Assert.IsTrue(q.IsIncluded<ComponentInt32>());
        Assert.AreEqual(1, q.Include.Count);
        Assert.IsTrue(q.IsExcluded<Phantom>());
        Assert.AreEqual(1, q.Exclude.Count);
        Assert.AreEqual(0, q.ExactlyOneOf.Count);
        Assert.AreEqual(0, q.AtLeastOneOf.Count);
    }

    [TestMethod]
    public void RefQueryNoEntityWithData()
    {
        var w = new WorldBuilder().Build();
        TestHelpers.SetupRandomEntities(w, count: 10_000).Playback().Dispose();

        var q = default(QueryDescription);

        // Set all to 123
        w.Query(123, (int data, ref ComponentInt32 i) =>
        {
            i.Value = data;
        }, ref q);

        // Check all are correct
        foreach (var (_, i) in w.Query<ComponentInt32>())
        {
            Assert.AreEqual(123, i.Ref.Value);
        }

        // Check the returned query is correct
        Assert.IsNotNull(q);
        Assert.IsTrue(q.IsIncluded<ComponentInt32>());
        Assert.AreEqual(1, q.Include.Count);
        Assert.IsTrue(q.IsExcluded<Phantom>());
        Assert.AreEqual(1, q.Exclude.Count);
        Assert.AreEqual(0, q.ExactlyOneOf.Count);
        Assert.AreEqual(0, q.AtLeastOneOf.Count);
    }

    [TestMethod]
    public void QueryNoEntity()
    {
        var w = new WorldBuilder().Build();
        TestHelpers.SetupRandomEntities(w, count: 10_000).Playback().Dispose();

        // Set all to 123
        w.Query((ref ComponentInt32 i) =>
        {
            i.Value = 123;
        });

        // Check all are correct
        foreach (var (_, i) in w.Query<ComponentInt32>())
        {
            Assert.AreEqual(123, i.Ref.Value);
        }
    }

    [TestMethod]
    public void QueryNoEntityWithData()
    {
        var w = new WorldBuilder().Build();
        TestHelpers.SetupRandomEntities(w, count: 10_000).Playback().Dispose();

        // Set all to 142
        w.Query(142, (int data, ref ComponentInt32 i) =>
        {
            i.Value = data;
        });

        // Check all are correct
        foreach (var (_, i) in w.Query<ComponentInt32>())
        {
            Assert.AreEqual(142, i.Ref.Value);
        }
    }
}