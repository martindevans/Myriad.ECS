using Myriad.ECS.Command;
using Myriad.ECS.Components;
using Myriad.ECS.Worlds;

namespace Myriad.ECS.Tests.Components;

[TestClass]
public class ShardingTests
{
    [TestMethod]
    public void ShardedCreatesMoreArchetypes()
    {
        var w = new WorldBuilder().Build();

        var c = new CommandBuffer(w);

        for (var i = 0; i < 1024; i++)
        {
            c.Create()
             .Set(new ComponentInt64(i))
             .AddSharding(i);
        }

        c.Playback().Dispose();

        // All entities are the same (except for sharding). So check that
        // there's more than one archetype, to check that sharding did something.
        Assert.IsTrue(w.Archetypes.Count > 1);
    }

    [TestMethod]
    public void ShardedMigratesToMoreArchetypes()
    {
        var w = new WorldBuilder().Build();

        var c = new CommandBuffer(w);

        // Create identical entities
        var buffered = new List<CommandBuffer.BufferedEntity>();
        for (var i = 0; i < 1024; i++)
            buffered.Add(c.Create().Set(new ComponentInt64(i)));

        using var resolver = c.Playback();
        Assert.IsTrue(w.Archetypes.Count == 1);

        // Add sharding
        for (var i = 0; i < buffered.Count; i++)
            c.AddSharding(buffered[i].Resolve(resolver), i);
        c.Playback().Dispose();

        // All entities are the same (except for sharding). So check that
        // there's more than one archetype, to check that sharding did something.
        Assert.IsTrue(w.Archetypes.Count > 1);
    }

    [TestMethod]
    public void RemoveShardingMigratesEntities()
    {
        var w = new WorldBuilder().Build();

        var c = new CommandBuffer(w);

        // Create identical entities with sharding
        var buffered = new List<CommandBuffer.BufferedEntity>();
        for (var i = 0; i < 1024; i++)
            buffered.Add(c.Create().Set(new ComponentInt64(i)).AddSharding(i));

        // There should be many archetypes with entities
        Assert.IsFalse(w.Archetypes.Count(a => a.EntityCount != 0) == 1);

        // Remove sharding from all entities
        using var resolver = c.Playback();
        foreach (var bufferedEntity in buffered)
            c.RemoveSharding(resolver.Resolve(bufferedEntity));
        c.Playback().Dispose();

        // Check that all but one archetype is empty
        Assert.IsTrue(w.Archetypes.Count(a => a.EntityCount != 0) == 1);
    }
}