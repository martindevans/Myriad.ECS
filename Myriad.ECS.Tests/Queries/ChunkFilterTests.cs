using Myriad.ECS.Command;
using Myriad.ECS.Components;
using Myriad.ECS.Queries;
using Myriad.ECS.Worlds;

namespace Myriad.ECS.Tests.Queries;

[TestClass]
public class ChunkFilterTests
{
    [TestMethod]
    public void ExecuteChunkFilterSkipsExcludedChunks()
    {
        // Setup a world with 10k entities, each with a ComponentInt32
        var w = new WorldBuilder().Build();
        var c = new CommandBuffer(w);
        for (var i = 0; i < 10_000; i++)
            c.Create().Set(new ComponentInt32(0));
        c.Playback().Dispose();
        Assert.AreEqual(10_000, w.Count<ComponentInt32>());

        // Run a chunk query that only operates on chunks with an even ID, incrementing the value
        var q = new IncrementValues();
        var f = new EvenIdChunkFilter();
        var query = (QueryDescription?)null;
        var count = w.ExecuteChunk<IncrementValues, EvenIdChunkFilter, ComponentInt32>(ref q, ref f, ref query);

        // Run a normal chunk query, asserting that the value is 1 where it should be, and 0 everywhere else
        var state = new CheckState();
        var check = new CheckValues(state);
        w.ExecuteChunk<CheckValues, ComponentInt32>(check);

        Assert.IsTrue(state.EvenChunks > 0, "Expected at least one even-ID chunk");
        Assert.IsTrue(state.OddChunks > 0, "Expected at least one odd-ID chunk");
        Assert.AreEqual(state.EntitiesInEvenChunks, count);
    }

    /// <summary>
    /// Only operate on chunks with an even ID
    /// </summary>
    private readonly struct EvenIdChunkFilter
        : IChunkFilter
    {
        public bool Exclude(in ChunkHandle handle)
        {
            return handle.ChunkId % 2 != 0;
        }
    }

    private struct IncrementValues
        : IChunkQuery<ComponentInt32>
    {
        public void Execute(ChunkHandle chunk, Span<ComponentInt32> t0)
        {
            for (var i = 0; i < t0.Length; i++)
                t0[i].Value++;
        }
    }

    private sealed class CheckState
    {
        public int EvenChunks;
        public int OddChunks;
        public int EntitiesInEvenChunks;
    }

    private struct CheckValues
        : IChunkQuery<ComponentInt32>
    {
        private readonly CheckState _state;

        public CheckValues(CheckState state)
        {
            _state = state;
        }

        public void Execute(ChunkHandle chunk, Span<ComponentInt32> t0)
        {
            var expected = chunk.ChunkId % 2 == 0 ? 1 : 0;

            if (chunk.ChunkId % 2 == 0)
            {
                _state.EvenChunks++;
                _state.EntitiesInEvenChunks += t0.Length;
            }
            else
            {
                _state.OddChunks++;
            }

            for (var i = 0; i < t0.Length; i++)
                Assert.AreEqual(expected, t0[i].Value, $"Chunk {chunk.ChunkId} at index {i}");
        }
    }
}
