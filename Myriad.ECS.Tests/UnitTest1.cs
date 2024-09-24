using Myriad.ECS.Command;
using Myriad.ECS.Queries;
using Myriad.ECS.Worlds;

namespace Myriad.ECS.Tests;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestMethod1()
    {
        // Create a world
        var world = new WorldBuilder().Build();

        // Create 2 entities with a command buffer
        var cmd = new CommandBuffer(world);
        var be1 = cmd.Create()
                     .Set(new ComponentFloat(123))
                     .Set(new ComponentInt32(456));
        var be2 = cmd.Create()
                     .Set(new ComponentFloat(0))
                     .Set(new ComponentInt32(1))
                     .Set(new ComponentByte(255));

        // Play that buffer back
        using var resolver = cmd.Playback();

        // Resolve the IDs of the entities
        var e1 = be1.Resolve();
        Assert.IsNotNull(e1);
        var e2 = be2.Resolve();
        Assert.IsNotNull(e2);

        // Run query
        var queryDesc = MultiplyAdd.QueryBuilder.Build(world);
        var queryResult = world.Execute<MultiplyAdd, ComponentFloat, ComponentInt32>(new MultiplyAdd(4), queryDesc);

        // Query enumerable style
        foreach (var values in world.Query<ComponentFloat, ComponentInt32>(queryDesc))
            values.Item0.Value += values.Item1.Value * 4;
    }
}

public readonly partial struct MultiplyAdd(float factor)
    : IQuery<ComponentFloat, ComponentInt32>
{
    public void Execute(Entity e, ref ComponentFloat f, ref ComponentInt32 i)
    {
        f.Value += i.Value * factor;
    }

    //todo: auto generate this from attributes?
    public static QueryBuilder QueryBuilder { get; } = new QueryBuilder()
        .Include<ComponentFloat>()
        .Include<ComponentInt32>()
        .Exclude<ComponentByte>();
}