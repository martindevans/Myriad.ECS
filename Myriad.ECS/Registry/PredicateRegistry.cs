using Myriad.ECS.IDs;
using System.Diagnostics.CodeAnalysis;
using Myriad.ECS.Queries.Predicates;

namespace Myriad.ECS.Registry;

/// <summary>
/// Store a lookup from filter type to unique 32 bit ID.
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public sealed class PredicateRegistry
    : BaseRegistry<IQueryPredicate, FilterID>
{
    [ExcludeFromCodeCoverage]
    private PredicateRegistry()
    {
    }
}