using System.Reflection;
using Myriad.ECS.Command;
using Myriad.ECS.Queries;
using Myriad.ECS.Queries.Attributes;
using Myriad.ECS.Registry;
using Myriad.ECS.Worlds;

namespace Myriad.ECS.Tests;

[TestClass]
public class UnitTest1
{
    public UnitTest1()
    {
        // Register all the component we need
        ComponentRegistry.Register(Assembly.GetExecutingAssembly());
    }

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
        var e1 = be1.Resolve(resolver);
        Assert.IsNotNull(e1);
        var e2 = be2.Resolve(resolver);
        Assert.IsNotNull(e2);

        // Run query
        var queryDesc = MultiplyAdd.QueryBuilder.Build(world);
        var queryResult = world.Execute(queryDesc, new MultiplyAdd(4));
    }
}

[All<ComponentFloat, ComponentInt32>]
[None<ComponentByte>]
[ExactlyOneOf<ComponentFloat, ComponentInt16>]
[AtLeastOneOf<ComponentFloat, ComponentInt16>]
public readonly partial struct MultiplyAdd(float factor)
    : IQueryWR<ComponentFloat, ComponentInt32>
{
    public void Execute(Entity e, ref ComponentFloat f, in ComponentInt32 i)
    {
        f.Value += i.Value * factor;
    }

    //todo: auto generate this from attributes?
    public static QueryBuilder QueryBuilder { get; } = new QueryBuilder()
        .Include<ComponentFloat>()
        .Include<ComponentInt32>()
        .Exclude<ComponentByte>();
}