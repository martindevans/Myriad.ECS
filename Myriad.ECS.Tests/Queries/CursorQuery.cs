using Myriad.ECS.Command;
using Myriad.ECS.Queries;
using Myriad.ECS.Worlds;
using Myriad.ECS.Worlds.Archetypes;

namespace Myriad.ECS.Tests.Queries;

[TestClass]
public class CursorQuery
{
    [TestMethod]
    public void BasicCursor()
    {
        var w = new WorldBuilder().Build();

        // Create 3 chunks worth of entities, 
        var c = new CommandBuffer(w);
        for (var i = 0; i < Archetype.CHUNK_SIZE * 3; i++)
            c.Create().Set(new ComponentInt32());

        // Plus some other unrelated entities
        for (var i = 0; i < Archetype.CHUNK_SIZE; i++)
            c.Create().Set(new Component0());
        c.Playback().Dispose();

        // Create a cursor with budget to process one chunk
        var cursor = new Cursor
        {
            EntityBudget = Archetype.CHUNK_SIZE,
        };

        // Execute query with cursor
        QueryDescription? q = null;
        var inc = new Increment();
        Assert.AreEqual(Archetype.CHUNK_SIZE, w.Execute<Increment, ComponentInt32>(ref inc, ref q, cursor));

        // Check that exactly 1 chunk of entities has been processed
        var incremented = 0;
        foreach (var (e, ci32) in w.Query<ComponentInt32>())
            if (ci32.Ref.Value > 0)
                incremented++;
        Assert.AreEqual(Archetype.CHUNK_SIZE, incremented);

        // Now run it twice again, with the cursor
        Assert.AreEqual(Archetype.CHUNK_SIZE, w.Execute<Increment, ComponentInt32>(ref inc, ref q, cursor));
        Assert.AreEqual(Archetype.CHUNK_SIZE, w.Execute<Increment, ComponentInt32>(ref inc, ref q, cursor));

        // Check that all entities have been processed exactly once
        foreach (var (_, ci32) in w.Query<ComponentInt32>())
            Assert.AreEqual(1, ci32.Ref.Value);
    }

    private struct Increment
        : IQuery<ComponentInt32>
    {
        public void Execute(Entity e, ref ComponentInt32 t0)
        {
            t0.Value++;
        }
    }
}