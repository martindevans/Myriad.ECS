using Myriad.ECS.Queries;
using Myriad.ECS.Queries.Predicates;

namespace Myriad.ECS.Tests;

public record struct ComponentByte(byte Value) : IComponent;
public record struct ComponentInt16(short Value) : IComponent;
public record struct ComponentFloat(float Value) : IComponent;
public record struct ComponentInt32(int Value) : IComponent;
public record struct ComponentInt64(long Value) : IComponent;

public readonly struct PredicatePositiveFloat
    : IQueryPredicate<ComponentFloat>
{
    public bool Execute(Entity e, in ComponentFloat t0)
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
    public bool Execute(Entity e, in ComponentFloat t0)
    {
        return t0.Value < 0;
    }

    public void ConfigureQueryBuilder(QueryBuilder q)
    {
        q.Include<ComponentFloat>();
    }
}