using Myriad.ECS.Command;
using Myriad.ECS.Components;
using Myriad.ECS.Worlds;
using Myriad.ECS.Worlds.Chunks;

namespace Myriad.ECS.Tests;

[TestClass]
public class ArchetypeSortTests
{
    [TestMethod]
    public void Sort()
    {
        var world = new WorldBuilder().Build();

        // Create entities with ComponentInt32 values descending
        var cmd = new CommandBuffer(world);
        for (var i = 0; i < 10000; i++)
            cmd.Create().Set(new ComponentInt32(-i)).Set(new SelfReference());
        cmd.Playback().Dispose();

        // Sort by ComponentInt32 ascending
        world.Archetypes[0].Sort<int, ComponentInt32Key>(new ComponentInt32Key(), block:false);

        //todo: testing this is tricky - query order is (intentionally) undefined, even after a sort.
    }

    private struct ComponentInt32Key
        : Chunk.IKeyMapper<int>
    {
        //todo: keymapper can't accept a Chunk, that's not a public type!
        public int MapKey(Chunk chunk, int index)
        {
            return chunk.GetRef<ComponentInt32>(index).Value;
        }
    }
}