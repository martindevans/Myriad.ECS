using Myriad.ECS.Command;
using Myriad.ECS.Worlds;
using Myriad.ECS.Worlds.Archetypes;

namespace Myriad.ECS.Tests;

/// <summary>
/// Tests targeting an IndexOutOfRangeException in Chunk.AddEntity caused by stale
/// entries in the Archetype._chunksWithSpace list.
///
/// Root cause:
/// When a chunk reaches CHUNK_SIZE entities it is still left in _chunksWithSpace (only the
/// NEXT AddEntity call trims full chunks). If an entity is then removed, HandleChunkEntityRemoved
/// sees EntityCount == CHUNK_SIZE-1 and adds the chunk to _chunksWithSpace *again*, creating a
/// duplicate. When the chunk empties to 0 it is pushed to _spareChunks and
/// _chunksWithSpace.Remove removes only one of the two copies, leaving a stale entry.
/// The stale entry is later used to add up to CHUNK_SIZE more entities to the spare chunk
/// (without adding it to _chunks). When those entities fill the spare chunk the stale entry
/// is cleaned up by RemoveAll, _spareChunks is popped, and AddEntity is called on a chunk
/// that is already full → IndexOutOfRangeException.
/// </summary>
[TestClass]
public class ChunkOverflowTests
{
    // Use the actual internal constant so this stays in sync with the implementation.
    private const int ChunkSize = Archetype.CHUNK_SIZE;

    /// <summary>
    /// Fill exactly one chunk, delete every entity, then create one more entity than a chunk
    /// can hold. The stale _chunksWithSpace entry causes the spare chunk to be overfilled.
    /// </summary>
    [TestMethod]
    public void ChunkOverflow_FillDeleteAllThenCreateChunkSizePlusOne()
    {
        var world = new WorldBuilder().Build();
        var buffer = new CommandBuffer(world);

        // Fill exactly one chunk with Component1 entities.
        for (var i = 0; i < ChunkSize; i++)
            buffer.Create().Set(new Component1());
        buffer.Playback().Dispose();

        Assert.AreEqual(ChunkSize, world.Archetypes.Sum(a => a.EntityCount));

        // Delete every entity.
        foreach (var (e, _) in world.Query<Component1>())
            buffer.Delete(e);
        buffer.Playback().Dispose();

        Assert.AreEqual(0, world.Archetypes.Sum(a => a.EntityCount));

        // Create CHUNK_SIZE + 1 entities in the same archetype.
        // Without the fix this triggers an IndexOutOfRangeException in Chunk.AddEntity.
        for (var i = 0; i <= ChunkSize; i++)
            buffer.Create().Set(new Component1());
        buffer.Playback().Dispose();

        Assert.AreEqual(ChunkSize + 1, world.Archetypes.Sum(a => a.EntityCount));
    }

    /// <summary>
    /// Variant that exercises the bug via multiple create/delete cycles so that the
    /// spare chunk pool grows and the stale-entry corruption accumulates.
    /// </summary>
    [TestMethod]
    public void ChunkOverflow_MultipleFillDeleteCycles()
    {
        var world = new WorldBuilder().Build();
        var buffer = new CommandBuffer(world);

        for (var cycle = 0; cycle < 3; cycle++)
        {
            // Fill one chunk completely.
            for (var i = 0; i < ChunkSize; i++)
                buffer.Create().Set(new Component1());
            buffer.Playback().Dispose();

            // Delete all entities.
            foreach (var (e, _) in world.Query<Component1>())
                buffer.Delete(e);
            buffer.Playback().Dispose();
        }

        // After several cycles the spare pool and stale _chunksWithSpace entries can be in a
        // state where adding CHUNK_SIZE+1 entities will attempt to use an already-full spare.
        for (var i = 0; i <= ChunkSize; i++)
            buffer.Create().Set(new Component1());
        buffer.Playback().Dispose();

        Assert.AreEqual(ChunkSize + 1, world.Archetypes.Sum(a => a.EntityCount));
    }

    /// <summary>
    /// Fill a chunk, remove one entity (this produces the first duplicate in
    /// _chunksWithSpace), then remove the rest. The stale entry must not prevent
    /// the archetype from accepting a full chunk's worth of new entities plus one more.
    /// </summary>
    [TestMethod]
    public void ChunkOverflow_RemoveOneFirstThenAll()
    {
        var world = new WorldBuilder().Build();
        var buffer = new CommandBuffer(world);

        // Fill exactly one chunk.
        for (var i = 0; i < ChunkSize; i++)
            buffer.Create().Set(new Component1());
        buffer.Playback().Dispose();

        // Remove a single entity first – this triggers the CHUNK_SIZE-1 case in
        // HandleChunkEntityRemoved and appends a duplicate into _chunksWithSpace.
        Entity oneEntity = default;
        foreach (var (e, _) in world.Query<Component1>())
        {
            oneEntity = e;
            break;
        }
        buffer.Delete(oneEntity);
        buffer.Playback().Dispose();

        // Remove all remaining entities.
        foreach (var (e, _) in world.Query<Component1>())
            buffer.Delete(e);
        buffer.Playback().Dispose();

        Assert.AreEqual(0, world.Archetypes.Sum(a => a.EntityCount));

        // Create CHUNK_SIZE + 1 new entities.
        for (var i = 0; i <= ChunkSize; i++)
            buffer.Create().Set(new Component1());
        buffer.Playback().Dispose();

        Assert.AreEqual(ChunkSize + 1, world.Archetypes.Sum(a => a.EntityCount));
    }

    /// <summary>
    /// Exercises the bug via archetype migration: fill an archetype, migrate all entities
    /// to a different archetype (emptying the original), then fill the original past one
    /// chunk.
    /// </summary>
    [TestMethod]
    public void ChunkOverflow_MigrationEmptiesChunkThenRefill()
    {
        var world = new WorldBuilder().Build();
        var buffer = new CommandBuffer(world);

        // Create CHUNK_SIZE entities with two components so they land in archetype A.
        for (var i = 0; i < ChunkSize; i++)
            buffer.Create().Set(new Component1()).Set(new Component2());
        buffer.Playback().Dispose();

        // Migrate all entities to archetype B (Component1 only) by removing Component2.
        // This empties archetype A's chunk and pushes it to spares.
        foreach (var (e, _, _) in world.Query<Component1, Component2>())
            buffer.Remove<Component2>(e);
        buffer.Playback().Dispose();

        // Delete all entities.
        foreach (var (e, _) in world.Query<Component1>())
            buffer.Delete(e);
        buffer.Playback().Dispose();

        Assert.AreEqual(0, world.Archetypes.Sum(a => a.EntityCount));

        // Recreate CHUNK_SIZE + 1 entities in the original archetype.
        for (var i = 0; i <= ChunkSize; i++)
            buffer.Create().Set(new Component1()).Set(new Component2());
        buffer.Playback().Dispose();

        Assert.AreEqual(ChunkSize + 1, world.Archetypes.Sum(a => a.EntityCount));
    }

    /// <summary>
    /// Verifies that the world's internal entity count remains consistent after
    /// fill-and-drain cycles across archetypes.
    /// </summary>
    [TestMethod]
    public void ChunkOverflow_EntityCountConsistentAfterChurn()
    {
        var world = new WorldBuilder().Build();
        var buffer = new CommandBuffer(world);

        for (var cycle = 0; cycle < 5; cycle++)
        {
            // Add a full chunk worth.
            for (var i = 0; i < ChunkSize; i++)
                buffer.Create().Set(new Component1());
            buffer.Playback().Dispose();

            // Delete all.
            foreach (var (e, _) in world.Query<Component1>())
                buffer.Delete(e);
            buffer.Playback().Dispose();

            Assert.AreEqual(0, world.Archetypes.Sum(a => a.EntityCount),
                $"Entity count should be 0 after cycle {cycle}");
        }

        // Now add CHUNK_SIZE + 1 – all should succeed without exception.
        for (var i = 0; i <= ChunkSize; i++)
            buffer.Create().Set(new Component1());
        buffer.Playback().Dispose();

        Assert.AreEqual(ChunkSize + 1, world.Archetypes.Sum(a => a.EntityCount));
    }
}
