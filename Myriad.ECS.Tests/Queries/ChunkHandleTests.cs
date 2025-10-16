using Myriad.ECS.Components;
using Myriad.ECS.IDs;
using Myriad.ECS.Queries;
using Myriad.ECS.Worlds;
using System;

namespace Myriad.ECS.Tests.Queries;

[TestClass]
public class ChunkHandleTests
{
    [TestMethod]
    public void ChunkHandle_Basics()
    {
        var w = new WorldBuilder().Build();
        TestHelpers.SetupRandomEntities(w, count: 10_000).Playback().Dispose();

        w.Query((ChunkHandle ch, Span<ComponentInt32> ci) =>
        {
            Assert.AreEqual(ch.EntityCount, ci.Length);
            Assert.IsNotNull(ch.Archetype);
            Assert.IsTrue(ch.Archetype.Components.Contains(ComponentID<ComponentInt32>.ID));
        });
    }

    [TestMethod]
    public void ChunkHandle_Danger()
    {
        var w = new WorldBuilder().Build();
        TestHelpers.SetupRandomEntities(w, count: 10_000).Playback().Dispose();

        w.Query((ChunkHandle ch, Span<ComponentInt32> ci) =>
        {
            var danger = ch.Danger();

            Assert.IsNotNull(danger.GetEntityArray());
            Assert.IsTrue(danger.GetEntityArray().AsSpan(0, ch.EntityCount).SequenceEqual(ch.Entities.Span));

            Assert.IsTrue(danger.GetComponentArray<ComponentInt32>().AsSpan(0, ch.EntityCount).SequenceEqual(ci));
        });
    }
}