using Myriad.ECS.IDs;
using Myriad.ECS.Queries.Filters;

namespace Myriad.ECS.Registry;

/// <summary>
/// Store a lookup from filter type to unique 32 bit ID.
/// </summary>
public sealed class FilterRegistry
    : BaseRegistry<IQueryFilter, FilterID>
{
    private FilterRegistry()
    {
    }
}