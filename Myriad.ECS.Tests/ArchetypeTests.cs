using Myriad.ECS.Command;
using Myriad.ECS.IDs;
using Myriad.ECS.Queries;
using Myriad.ECS.Tests.Queries;
using Myriad.ECS.Worlds;

namespace Myriad.ECS.Tests;

[TestClass]
public class ArchetypeTests
{
    [TestMethod]
    public void EnumerateManyEntities()
    {
        var w = new WorldBuilder()
               .WithArchetype<ComponentInt32>()
               .Build();

        var cb = new CommandBuffer(w);
        for (var i = 0; i < 10000; i++)
        {
            var e = cb.Create();
            e.Set(new ComponentInt32(i)).Set(new ComponentInt64(i));
            if (i % 7 == 0)
                e.Set(new ComponentFloat(i));
        }
        cb.Playback().Dispose();

        foreach (var archetype in w.Archetypes)
        {
            Assert.AreEqual(archetype.EntityCount, archetype.Entities.Count());
        }

        var count = 0;
        foreach (var (_, i32, i64) in w.Query<ComponentInt32, ComponentInt64>())
        {
            count++;
            Assert.AreEqual(i32.Ref.Value, (int)i64.Ref.Value);
        }

        Assert.AreEqual(10000, count);
    }

    [TestMethod]
    public void ArchetypeHashesDiffer()
    {
        var w = new WorldBuilder().Build();
        TestHelpers.SetupRandomEntities(w).Playback().Dispose();

        var h0 = w.Archetypes[0].GetHashCode();
        var h1 = w.Archetypes[1].GetHashCode();

        Assert.AreNotEqual(h0, h1);
    }

    [TestMethod]
    public void ChunkFlags()
    {
        var w = new WorldBuilder().Build();
        TestHelpers.SetupRandomEntities(w, uniqueComponents:4, count:10_000).Playback().Dispose();

        // Set a flag on chunks
        foreach (var archetype in w.Archetypes)
            if (archetype.Components.Contains(ComponentID<ComponentInt32>.ID))
                foreach (var chunk in archetype.Chunks)
                    new ChunkHandle(chunk).SetBit<HasInt32>(true);

        // Check that flag
        foreach (var archetype in w.Archetypes)
        {
            var flag = archetype.Components.Contains(ComponentID<ComponentInt32>.ID);
            foreach (var chunk in archetype.Chunks)
                Assert.AreEqual(flag, new ChunkHandle(chunk).GetBit<HasInt32>());
        }

        // Set another flag
        foreach (var archetype in w.Archetypes)
        {
            var even = archetype.ArchetypeId % 2 == 0;
            foreach (var chunk in archetype.Chunks)
                chunk.SetFlag<IdIsEven>(even);
        }

        // Check both flags
        foreach (var archetype in w.Archetypes)
        {
            var flag = archetype.Components.Contains(ComponentID<ComponentInt32>.ID);
            var even = archetype.ArchetypeId % 2 == 0;

            foreach (var chunk in archetype.Chunks)
            {
                Assert.AreEqual(flag, chunk.GetFlag<HasInt32>());
                Assert.AreEqual(even, chunk.GetFlag<IdIsEven>());
            }
        }
    }

    private struct HasInt32 : IChunkBitFlag;
    private struct IdIsEven : IChunkBitFlag;
}