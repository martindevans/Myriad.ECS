using Myriad.ECS.Command;
using Myriad.ECS.IDs;
using Myriad.ECS.Locks;
using Myriad.ECS.Queries;
using Myriad.ECS.Worlds;
using Myriad.ECS.Worlds.Archetypes;

namespace Myriad.ECS.Tests.Queries;

[TestClass]
public class ExecuteChunkJoinTests
{
    private static void BasicCrossJoinTest(int leftCount, int rightCount)
    {
        var totalCount = leftCount * rightCount;

        var w = new WorldBuilder().Build();

        var cmd = new CommandBuffer(w);
        for (var i = 0; i < leftCount; i++)
            cmd.Create().Set(new Component0()).Set(new ComponentInt32(i));
        for (var i = 0; i < rightCount; i++)
            cmd.Create().Set(new Component1()).Set(new ComponentInt32(i));
        cmd.Playback().Dispose();

        var left = new QueryBuilder().Include<Component0, ComponentInt32>().Build(w);
        var right = new QueryBuilder().Include<Component1, ComponentInt32>().Build(w);

        var pairs = new List<(int, int)>();
        var count = w.ExecuteChunkJoin(new RecordingIntJoin(pairs), left, right);
        Assert.AreEqual(totalCount, count);

        Assert.HasCount(totalCount, pairs.Distinct());

        var pairsSet = pairs.ToHashSet();
        for (var i = 0; i < leftCount; i++)
        for (var j = 0; j < rightCount; j++)
            Assert.Contains((i, j), pairsSet);
    }
    
    [TestMethod]
    public void CrossJoinProducesExpectedValues_Small()
    {
        BasicCrossJoinTest(3, 3);
    }

    [TestMethod]
    public void CrossJoinProducesExpectedValues_LeftLargeArchetype()
    {
        BasicCrossJoinTest(1100, 3);
    }

    [TestMethod]
    public void CrossJoinProducesExpectedValues_RightLargeArchetype()
    {
        BasicCrossJoinTest(3, 1200);
    }

    [TestMethod]
    public void CrossJoinProducesExpectedValues_BothLargeArchetype()
    {
        BasicCrossJoinTest(1050, 1060);
    }

    [TestMethod]
    public void ExecuteChunkJoinMultipleArchetypes()
    {
        var w = new WorldBuilder().Build();

        var b = new CommandBuffer(w);
        for (var i = 0; i < 2000; i++)
            b.Create().Set(new ComponentInt32(i));
        for (var i = 0; i < 1500; i++)
            b.Create().Set(new ComponentInt32(10000 + i)).Set(new ComponentInt64(i));
        for (var i = 0; i < 1000; i++)
            b.Create().Set(new ComponentFloat(i));
        for (var i = 0; i < 225; i++)
            b.Create().Set(new ComponentFloat(10000 + i)).Set(new ComponentByte((byte)i));
        b.Playback().Dispose();

        var left = new QueryBuilder().Include<ComponentInt32>().Build(w);
        var right = new QueryBuilder().Include<ComponentFloat>().Build(w);

        var q = new CountingJoin();
        var count = w.ExecuteChunkJoin(ref q, left, right);

        var totalCount = (2000 + 1500) * (1000 + 225);
        Assert.AreEqual(totalCount, count);
        Assert.AreEqual(ExpectedChunkPairCount(left, right), q.PairCount);
        Assert.AreEqual(q.PairCount, q.SeenPairs.Count);
    }

    [TestMethod]
    public void ExecuteChunkJoinSelfJoinBlocksSharedArchetypesOnce()
    {
        var manager = new RecordingSafetyManager();
        var w = new WorldBuilder().WithSafetySystem(manager).Build();

        var b = new CommandBuffer(w);
        for (var i = 0; i < 1500; i++)
            b.Create().Set(new ComponentInt32(i));
        b.Playback().Dispose();
        manager.Clear();

        var q = new QueryBuilder().Include<ComponentInt32>().Build(w);

        var count = w.ExecuteChunkJoin(new CountingJoin(), q, q);

        Assert.AreEqual(1500L * 1500L, count);

        // Passing the same query on both sides must not double-block the shared archetypes.
        foreach (var match in q.GetArchetypes())
            Assert.AreEqual(1, manager.CountOf(match.Archetype));
        Assert.AreEqual(q.GetArchetypes().Count, manager.TotalCalls);
    }

    [TestMethod]
    public void ExecuteChunkJoinOverlappingQueriesBlockEachArchetypeOnce()
    {
        var manager = new RecordingSafetyManager();
        var w = new WorldBuilder().WithSafetySystem(manager).Build();

        var b = new CommandBuffer(w);
        for (var i = 0; i < 1000; i++)
            b.Create().Set(new ComponentInt32(i));
        for (var i = 0; i < 500; i++)
            b.Create().Set(new ComponentInt32(10000 + i)).Set(new ComponentFloat(i));
        b.Playback().Dispose();
        manager.Clear();

        // The second archetype matches both queries and must only be blocked once.
        var left = new QueryBuilder().Include<ComponentInt32>().Build(w);
        var right = new QueryBuilder().Include<ComponentInt32>().Include<ComponentFloat>().Build(w);

        var count = w.ExecuteChunkJoin(new CountingJoin(), left, right);

        Assert.AreEqual(1500L * 500L, count);

        var seen = new HashSet<Archetype>();
        foreach (var match in left.GetArchetypes())
            seen.Add(match.Archetype);
        foreach (var match in right.GetArchetypes())
            seen.Add(match.Archetype);

        foreach (var archetype in seen)
            Assert.AreEqual(1, manager.CountOf(archetype));
        Assert.AreEqual(seen.Count, manager.TotalCalls);
    }

    [TestMethod]
    public void ExecuteChunkJoinEmptySideReturnsZeroWithoutBlocking()
    {
        var manager = new RecordingSafetyManager();
        var w = new WorldBuilder().WithSafetySystem(manager).Build();

        var b = new CommandBuffer(w);
        for (var i = 0; i < 1000; i++)
            b.Create().Set(new ComponentInt32(i));
        b.Playback().Dispose();
        manager.Clear();

        // Left matches no archetypes at all.
        var left = new QueryBuilder().Include<ComponentInt16>().Build(w);
        var right = new QueryBuilder().Include<ComponentInt32>().Build(w);

        var count = w.ExecuteChunkJoin<InstantFailJoin>(left, right);

        Assert.AreEqual(0L, count);
        Assert.AreEqual(0, manager.TotalCalls);
    }

    [TestMethod]
    public void ExecuteChunkJoinEmptyArchetypeReturnsZero()
    {
        var w = new WorldBuilder().Build();

        var b = new CommandBuffer(w);
        for (var i = 0; i < 10; i++)
            b.Create().Set(new ComponentInt16((short)i));
        for (var i = 0; i < 100; i++)
            b.Create().Set(new ComponentInt32(i));
        b.Playback().Dispose();

        // Destroy all of the entities in the Int16 archetype, leaving the archetype matching but empty.
        var d = new CommandBuffer(w);
        d.Delete(new QueryBuilder().Include<ComponentInt16>().Build(w));
        d.Playback().Dispose();

        var left = new QueryBuilder().Include<ComponentInt16>().Build(w);
        var right = new QueryBuilder().Include<ComponentInt32>().Build(w);

        Assert.IsTrue(left.GetArchetypes().Count > 0, "Expected the empty archetype to still match the query");
        Assert.AreEqual(0, left.Count());

        var count = w.ExecuteChunkJoin<InstantFailJoin>(left, right);

        Assert.AreEqual(0L, count);
    }

    [TestMethod]
    public void ExecuteChunkJoinThrowsWhenQueriesAreFromDifferentWorlds()
    {
        var w1 = new WorldBuilder().Build();
        var w2 = new WorldBuilder().Build();

        var q1 = new QueryBuilder().Include<ComponentInt32>().Build(w1);
        var q2 = new QueryBuilder().Include<ComponentInt32>().Build(w2);

        Assert.Throws<ArgumentException>(() => w1.ExecuteChunkJoin<InstantFailJoin>(q1, q2));
        Assert.Throws<ArgumentException>(() => w1.ExecuteChunkJoin<InstantFailJoin>(q2, q1));
    }

    [TestMethod]
    public void ExecuteChunkJoinOverloadsReturnSameCount()
    {
        var w = new WorldBuilder().Build();

        var b = new CommandBuffer(w);
        for (var i = 0; i < 1500; i++)
            b.Create().Set(new ComponentInt32(i));
        for (var i = 0; i < 700; i++)
            b.Create().Set(new ComponentFloat(i));
        b.Playback().Dispose();

        var left = new QueryBuilder().Include<ComponentInt32>().Build(w);
        var right = new QueryBuilder().Include<ComponentFloat>().Build(w);

        var expected = 1500L * 700L;
        var q = new CountingJoin();

        Assert.AreEqual(expected, w.ExecuteChunkJoin(ref q, left, right));
        Assert.AreEqual(expected, w.ExecuteChunkJoin(new CountingJoin(), left, right));
    }

    private static int ExpectedChunkPairCount(QueryDescription left, QueryDescription right)
    {
        var count = 0;
        foreach (var l in left.GetArchetypes())
            foreach (var r in right.GetArchetypes())
                count += l.Archetype.ChunkCount * r.Archetype.ChunkCount;
        return count;
    }

    private sealed class RecordingSafetyManager
        : IWorldArchetypeSafetyManager
    {
        private readonly Dictionary<Archetype, int> _counts = new();

        public int TotalCalls { get; private set; }

        public void Clear()
        {
            _counts.Clear();
            TotalCalls = 0;
        }

        public int CountOf(Archetype archetype)
        {
            return _counts.GetValueOrDefault(archetype);
        }

        public void Block(Archetype archetype)
        {
            TotalCalls++;
            _counts[archetype] = _counts.GetValueOrDefault(archetype) + 1;
        }

        public void Block(Archetype archetype, ReadOnlySpan<ComponentID> ids)
        {
            Block(archetype);
        }
    }

    private class CountingJoin
        : IChunkJoinQuery
    {
        public int PairCount;
        public long TotalEntities;
        public readonly HashSet<(long, long)> SeenPairs = new();

        public CountingJoin()
        {
            PairCount = 0;
            TotalEntities = 0;
        }

        public void Execute(ChunkHandle left, ChunkHandle right)
        {
            PairCount++;
            TotalEntities += (long)left.EntityCount * right.EntityCount;
            SeenPairs.Add((left.Entities.Span[0].ID.UniqueID(), right.Entities.Span[0].ID.UniqueID()));
        }
    }

    private struct RecordingIntJoin
        : IChunkJoinQuery
    {
        public readonly List<(int, int)> Pairs;

        public RecordingIntJoin(List<(int, int)> pairs)
        {
            Pairs = pairs;
        }

        public void Execute(ChunkHandle left, ChunkHandle right)
        {
            Assert.IsTrue(left.HasComponent<Component0>());
            Assert.IsTrue(right.HasComponent<Component1>());
            Assert.IsFalse(left.HasComponent<Component1>());
            Assert.IsFalse(right.HasComponent<Component0>());

            var l = left.GetComponentSpan<ComponentInt32>();
            var r = right.GetComponentSpan<ComponentInt32>();

            foreach (var litem in l)
                foreach (var ritem in r)
                    Pairs.Add((litem.Value, ritem.Value));
        }
    }

    private struct InstantFailJoin
        : IChunkJoinQuery
    {
        public void Execute(ChunkHandle left, ChunkHandle right)
        {
            Assert.Fail();
        }
    }
}
