using Myriad.ECS.Command;
using Myriad.ECS.Components;
using Myriad.ECS.Worlds;
using Myriad.ECS.Worlds.Chunks;

namespace Myriad.ECS.Tests;

[TestClass]
public class ChunkSortTests
{
    [TestMethod]
    public void Sort()
    {
        var world = new WorldBuilder().Build();

        // Create entities with random integers
        var rng = new Random(3724);
        var cmd = new CommandBuffer(world);
        for (var i = 0; i < 1000; i++)
            cmd.Create().Set(new ComponentInt32(rng.Next())).Set(new SelfReference());
        cmd.Playback().Dispose();

        // Sort the chunk by int
        world.Archetypes[0].Chunks[0].Sort<int, ComponentInt32KeyMapper>(new ComponentInt32KeyMapper());

        // Check that the entities are correct
        var prev = int.MinValue;
        foreach (var (e, ci, sr) in world.Query<ComponentInt32, SelfReference>())
        {
            // Are the values actually sorted
            Assert.IsGreaterThanOrEqualTo(prev, ci.Ref.Value);
            prev = ci.Ref.Value;
            
            // Did all components get moved? we check this with the "self reference" entity which stores it's own ID
            Assert.AreEqual(sr.Ref.Target, e);

            if (e.ID.ID == 1)
            {
                
            }

            // Check that the world reference is correct
            ref var info = ref world.GetEntityInfo(e);
            var e2 = info.Chunk.Entities.Span[info.RowIndex];
            Assert.AreEqual(e, e2);
        }
    }

    private struct ComponentInt32KeyMapper
        : Chunk.IKeyMapper<int>
    {
        public int MapKey(Chunk chunk, int index)
        {
            return chunk.GetRef<ComponentInt32>(index).Value;
        }
    }
}