using Myriad.ECS.Command;
using Myriad.ECS.Queries;
using Myriad.ECS.Worlds;

namespace Myriad.ECS.Tests.Queries;

[TestClass]
public class CollectQueryTests
{
    [TestMethod]
    public void CollectMatched()
    {
        var w = new WorldBuilder().Build();

        // Create entities, all with ComponentInt64, some with ComponentFloat
        var cb = new CommandBuffer(w);
        for (var i = 0; i < 128; i++)
        {
            cb.Create().Set(new ComponentInt64(1));
            if (i % 2 == 0)
                cb.Create().Set(new ComponentFloat(2));
        }
        cb.Playback().Dispose();

        // Collect entities with ComponentInt64, without ComponentFloat
        var filter = new QueryBuilder().Include<ComponentInt64>().Exclude<ComponentFloat>().Build(w);
        var listWithComponents = new List<(Entity, ComponentInt64)>();
        var listOnlyEntities = new List<Entity>();
        var listOnlyEntitiesGeneric = new List<Entity>();
        w.Collect(listWithComponents, filter);
        w.Collect(listOnlyEntities, filter);
        w.Collect<ComponentInt64>(listOnlyEntitiesGeneric, filter);

        // Check list is the right size
        Assert.AreEqual(filter.Count(), listWithComponents.Count);
        Assert.AreEqual(filter.Count(), listOnlyEntitiesGeneric.Count);
        Assert.IsTrue(listOnlyEntities.SequenceEqual(listOnlyEntitiesGeneric));

        // Check all the expected things are in the list
        foreach (var (e, c64) in w.Query<ComponentInt64>(filter))
        {
            var item = listWithComponents.Find((tup) => tup.Item1 == e);
            Assert.AreEqual(c64.Ref.Value, item.Item2.Value);
        }
    }
}