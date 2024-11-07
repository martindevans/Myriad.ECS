using Myriad.ECS.Command;
using Myriad.ECS.Components;
using Myriad.ECS.IDs;
using Myriad.ECS.Queries;
using Myriad.ECS.Worlds;

namespace Myriad.ECS.Tests.Queries;

[TestClass]
public class ExecuteChunkTests
{
    [TestMethod]
    public void ExecuteChunkNone()
    {
        var w = new WorldBuilder().Build();
        TestHelpers.SetupRandomEntities(w, 0, 10000).Playback().Dispose();

        var count = w.ExecuteChunk<InstantFail, Component0>();
        Assert.AreEqual(0, count);
    }

    [TestMethod]
    public void ExecuteChunkNoneRefQuery()
    {
        var w = new WorldBuilder().Build();
        TestHelpers.SetupRandomEntities(w, 0, 10000).Playback().Dispose();

        QueryDescription? q = null;
        var count = w.ExecuteChunk<InstantFail, Component0>(ref q);
        Assert.AreEqual(0, count);

        Assert.IsNotNull(q);

        Assert.AreEqual(1, q.Include.Count);
        Assert.IsTrue(q.Include.Contains(ComponentID<Component0>.ID));

        Assert.AreEqual(1, q.Exclude.Count);
        Assert.IsTrue(q.Exclude.Contains(ComponentID<Phantom>.ID));

        Assert.AreEqual(0, q.ExactlyOneOf.Count);
        Assert.AreEqual(0, q.AtLeastOneOf.Count);
    }

    [TestMethod]
    public void ExecuteChunkNoneRefAction()
    {
        var w = new WorldBuilder().Build();
        TestHelpers.SetupRandomEntities(w, 0, 10000).Playback().Dispose();

        InstantFail i;
        var count = w.ExecuteChunk<InstantFail, Component0>(i);
        Assert.AreEqual(0, count);
    }

    [TestMethod]
    public void ExecuteChunkSetInt()
    {
        var w = new WorldBuilder().Build();
        TestHelpers.SetupRandomEntities(w, count:500000).Playback().Dispose();

        // Destroy all of these specific entities, to ensure at least one archetype matches the query but is empty
        var c = new CommandBuffer(w);
        foreach (var (e, _, _) in w.Query<ComponentInt32, ComponentByte>())
            c.Delete(e);
        c.Playback().Dispose();
        Assert.AreEqual(0, w.Count<ComponentInt32, ComponentByte>());

        // Set all entities with ComponentInt32 but without ComponentFloat
        var q0 = new QueryBuilder().Include<ComponentInt32>().Exclude<ComponentFloat>().Build(w);
        var q1 = q0;
        var count = w.ExecuteChunk<SetValueToOne, ComponentInt32>(new SetValueToOne(), ref q1);
        Console.WriteLine(count);

        // Since we supplied a non-null query, it should not have been modified
        Assert.AreSame(q0, q1);

        // Check all entities
        foreach (var (e, i) in w.Query<ComponentInt32>())
        {
            if (e.HasComponent<ComponentFloat>())
                Assert.AreEqual(0, i.Ref.Value);
            else
                Assert.AreEqual(1, i.Ref.Value);
        }
    }

    private struct InstantFail
        : IChunkQuery<Component0>
    {
        public void Execute(ChunkHandle chunk, ReadOnlySpan<Entity> e, Span<Component0> t0)
        {
            Assert.Fail();
        }
    }

    private struct SetValueToOne
        : IChunkQuery<ComponentInt32>
    {
        public void Execute(ChunkHandle chunk, ReadOnlySpan<Entity> e, Span<ComponentInt32> t0)
        {
            for (var i = 0; i < t0.Length; i++)
                t0[i].Value = 1;
        }
    }
}