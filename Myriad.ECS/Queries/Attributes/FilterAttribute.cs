using Myriad.ECS.Queries.Predicates;

namespace Myriad.ECS.Queries.Attributes;

[AttributeUsage(AttributeTargets.Struct)]
public sealed class FilterAttribute<TQ>
    : Attribute
    where TQ : IQueryPredicate
{
    public Type Type => typeof(TQ);
}