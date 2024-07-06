using Myriad.ECS.Command;
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
}