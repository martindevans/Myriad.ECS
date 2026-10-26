using Myriad.ECS.Command;
using Myriad.ECS.Sorting;
using Myriad.ECS.Worlds;
using Myriad.ECS.Worlds.Archetypes;

namespace Myriad.ECS.Tests;

[TestClass]
public class EntityChunkComparerTests
{
    /// <summary>
    /// Create <paramref name="count"/> entities with a <see cref="Component1"/> and return their IDs.
    /// </summary>
    private static EntityId[] CreateEntities(World world, int count)
    {
        var buffer = new CommandBuffer(world);
        for (var i = 0; i < count; i++)
            buffer.Create().Set(new Component1());
        using var resolver = buffer.Playback();

        return Enumerable.Range(0, count).Select(a => resolver[a].ID).ToArray();
    }

    /// <summary>
    /// Get the chunk ID an entity currently lives in, or <see cref="long.MaxValue"/> if the entity is dead.
    /// </summary>
    private static long ChunkIdOf(World world, EntityId id)
    {
        EntityInfo dummy = default;
        ref var info = ref world.GetEntityInfo(id, ref dummy, out var isDead);
        return isDead ? long.MaxValue : info.Chunk.ChunkId;
    }

    [TestMethod]
    public void SameChunk_CompareIsZero()
    {
        var world = new WorldBuilder().Build();
        var ids = CreateEntities(world, 2);
        var comparer = new EntityChunkComparer(world);

        Assert.AreEqual(0, comparer.Compare(ids[0], ids[1]));
        Assert.AreEqual(0, comparer.Compare(ids[1], ids[0]));
        Assert.AreEqual(0, comparer.Compare(ids[0], ids[0]));
    }

    [TestMethod]
    public void DifferentChunks_CompareMatchesChunkIdOrdering()
    {
        var world = new WorldBuilder().Build();
        var ids = CreateEntities(world, Archetype.CHUNK_SIZE * 3);
        var comparer = new EntityChunkComparer(world);

        var chunkIds = ids.Select(id => ChunkIdOf(world, id)).ToArray();

        for (var i = 0; i < ids.Length; i++)
        {
            for (var j = 0; j < ids.Length; j++)
            {
                var expected = Math.Sign(chunkIds[i].CompareTo(chunkIds[j]));
                var actual = Math.Sign(comparer.Compare(ids[i], ids[j]));
                Assert.AreEqual(expected, actual, $"Compare({ids[i]}, {ids[j]}) should follow chunk ID ordering");
            }
        }
    }

    [TestMethod]
    public void Sort_GroupsEntitiesByChunk()
    {
        var world = new WorldBuilder().Build();
        var ids = CreateEntities(world, Archetype.CHUNK_SIZE * 3);
        var comparer = new EntityChunkComparer(world);

        Array.Sort(ids, comparer);

        // Chunk IDs must be non-decreasing after the sort, which means all entities of a
        // chunk end up contiguous.
        var prev = ChunkIdOf(world, ids[0]);
        foreach (var id in ids)
        {
            var current = ChunkIdOf(world, id);
            Assert.IsTrue(prev <= current, "sorted IDs must be grouped by chunk ID");
            prev = current;
        }

        // All three chunks must still be represented.
        var distinctChunks = ids.Select(id => ChunkIdOf(world, id)).Distinct().Count();
        Assert.AreEqual(3, distinctChunks);
    }

    [TestMethod]
    public void DeadEntity_SortsAfterLiveEntities()
    {
        var world = new WorldBuilder().Build();
        var ids = CreateEntities(world, 4);
        var dead = ids[0];

        var buffer = new CommandBuffer(world);
        buffer.Delete(dead.ToEntity(world));
        buffer.Playback().Dispose();

        var comparer = new EntityChunkComparer(world);

        // Dead entities sort after every live entity.
        Assert.IsTrue(comparer.Compare(dead, ids[1]) > 0);
        Assert.IsTrue(comparer.Compare(ids[1], dead) < 0);

        EntityId[] all = [dead, ids[1], ids[2], ids[3]];
        Array.Sort(all, comparer);
        Assert.AreEqual(dead, all[^1]);
    }

    [TestMethod]
    public void BothDead_CompareIsZero()
    {
        var world = new WorldBuilder().Build();
        var ids = CreateEntities(world, 2);

        var buffer = new CommandBuffer(world);
        buffer.Delete(ids[0].ToEntity(world));
        buffer.Delete(ids[1].ToEntity(world));
        buffer.Playback().Dispose();

        var comparer = new EntityChunkComparer(world);
        Assert.AreEqual(0, comparer.Compare(ids[0], ids[1]));
    }

    [TestMethod]
    public void UnknownEntityId_TreatedAsDead()
    {
        var world = new WorldBuilder().Build();
        var ids = CreateEntities(world, 2);
        var unknown = new EntityId(int.MaxValue, 1);
        var comparer = new EntityChunkComparer(world);

        // An ID that was never allocated compares as if it were dead.
        Assert.IsTrue(comparer.Compare(unknown, ids[0]) > 0);
        Assert.IsTrue(comparer.Compare(ids[0], unknown) < 0);
        Assert.AreEqual(0, comparer.Compare(unknown, new EntityId(int.MaxValue, 5)));
    }
}
