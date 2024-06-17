using Myriad.ECS.Command;
using Myriad.ECS.Worlds;

namespace Myriad.ECS.Tests;

[TestClass]
public class ArchetypeTests
{
    [TestMethod]
    public void EnumerateEntities()
    {
        var w = new WorldBuilder()
            .WithArchetype<ComponentInt32>()
            .Build();

        var cb = new CommandBuffer(w);
        cb.Create().Set(new ComponentInt64(1));
        cb.Create().Set(new ComponentFloat(2));
        cb.Create().Set(new ComponentObject(new object()));
        var e3 = cb.Create().Set(new ComponentFloat(3)).Set(new ComponentInt64(4));
        using var r = cb.Playback();
        var entity3 = r.Resolve(e3);

        foreach (var archetype in w.Archetypes)
        {
            Assert.AreEqual(archetype.EntityCount, archetype.Entities.Count());
        }
    }
}