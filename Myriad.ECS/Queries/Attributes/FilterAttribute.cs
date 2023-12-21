using Myriad.ECS.Queries.Filters;

namespace Myriad.ECS.Queries.Attributes;

[AttributeUsage(AttributeTargets.Struct)]
public sealed class FilterAttribute<TQ>
    : Attribute
    where TQ : IQueryFilter
{
    public Type Type => typeof(TQ);
}