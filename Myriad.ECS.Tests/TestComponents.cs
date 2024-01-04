using Myriad.ECS.Queries;
using Myriad.ECS.Queries.Predicates;

namespace Myriad.ECS.Tests;

public partial record struct ComponentByte(byte Value) : IComponent;
public partial record struct ComponentInt16(short Value) : IComponent;
public partial record struct ComponentFloat(float Value) : IComponent;
public partial record struct ComponentInt32(int Value) : IComponent;
public partial record struct ComponentInt64(long Value) : IComponent;

public readonly struct PredicatePositiveFloat
    : IQueryPredicate<ComponentFloat>
{
    public bool Execute(Entity e, ref readonly ComponentFloat t0)
    {
        return t0.Value > 0;
    }

    public void ConfigureQueryBuilder(QueryBuilder q)
    {
        q.Include<ComponentFloat>();
    }
}

public readonly struct PredicateNegativeFloat
    : IQueryPredicate<ComponentFloat>
{
    public bool Execute(Entity e, ref readonly ComponentFloat t0)
    {
        return t0.Value < 0;
    }

    public void ConfigureQueryBuilder(QueryBuilder q)
    {
        q.Include<ComponentFloat>();
    }
}