using System.Reflection;
using Myriad.ECS.Queries;
using Myriad.ECS.Queries.Attributes;
using Myriad.ECS.Queries.Filters;
using Myriad.ECS.Registry;

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
        return;
        //todo: TestMethod1

        // Create a world
        var world = new World();

        // Create 2 entities with a command buffer and delete one before it's even created
        var cmd = new CommandBuffer(world);
        var be1 = cmd.Create()
                     .Set(new ComponentFloat(123))
                     .Set(new ComponentInt32(456));
        var be2 = cmd.Create()
                     .Set(new ComponentFloat(0))
                     .Set(new ComponentInt32(1))
                     .Delete();

        // Play that buffer back
        var future = cmd.Playback();

        // Resolve the IDs of the entities
        using var resolver = future.Block();
        var e1 = be1.Resolve(resolver);
        Assert.IsNotNull(e1);
        var e2 = be2.Resolve(resolver);
        Assert.IsNull(e2);

        // Queue up a query
        //todo:var query = world.Query(new MultiplyAdd(4));

        //todo:query.Block();
    }
}

[All<ComponentFloat, ComponentInt32>]
[None<ComponentByte>]
[ExactlyOneOf<ComponentFloat, ComponentInt16>]
[AtLeastOneOf<ComponentFloat, ComponentInt16>]
[Filter<FloatValueGreaterThanIntValue>]
public readonly partial struct MultiplyAdd(float factor)
    : IQueryWR<ComponentFloat, ComponentInt32>
{
    public void Execute(Entity e, ref ComponentFloat f, in ComponentInt32 i)
    {
        f.Value += i.Value * factor;
    }
}

public readonly partial struct FloatValueGreaterThanIntValue
    : IQueryFilter<ComponentFloat, ComponentInt32>
{
    public bool Filter(Entity e, in ComponentFloat f, in ComponentInt32 i)
    {
        return f.Value > i.Value;
    }

    public void ConfigureQueryBuilder(QueryBuilder q)
    {
        q.Include<ComponentFloat>();
        q.Include<ComponentInt32>();
    }
}