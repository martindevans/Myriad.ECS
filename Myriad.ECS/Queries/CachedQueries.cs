using Myriad.ECS.Queries;
using Myriad.ECS.IDs;
using Standart.Hash.xxHash;
using System.Runtime.InteropServices;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedParameter.Global
// ReSharper disable LoopCanBeConvertedToQuery
// ReSharper disable CheckNamespace
// ReSharper disable ArrangeAccessorOwnerBody

namespace Myriad.ECS.Worlds;

public partial class World
{
    // Cache of all queries with 1 included components. Tuple elements are:
    // - hash of sorted component IDs
    // - sorted array of component IDs
    // - the query itself
    private readonly List<(uint, int[], QueryDescription)> _queryCache1 = [ ];

    /// <summary>
    /// Get a query that finds entities which include all of the given types. This query
    /// will be shared with other requests for the same set of types.
    /// </summary>
    /// <returns>A query that finds entities which include all of the given types</returns>
    private QueryDescription GetCachedQuery<T0>()
        where T0 : IComponent
    {
        // Accumulate all components in ascending order
        Span<int> orderedComponents = [
            ComponentID<T0>.ID.Value,
        ];

        // Calculate hash of components, for fast rejection
        var hash = xxHash32.ComputeHash(MemoryMarshal.Cast<int, byte>(orderedComponents), 42);

        // Find query that matches these types
        foreach (var (h, c, q) in _queryCache1)
        {
            // Early exit on hash check.
            if (h != hash)
                continue;

            // Since the sequences are sorted by component ID these should be identical
            if (!orderedComponents.SequenceEqual(c))
                continue;

            return q;
        }

        // Didn't find one, create it now
        var query = new QueryBuilder()
            .Include<T0>()
            .Build(this);

        // Add query to the cache
        _queryCache1.Add((hash, orderedComponents.ToArray(), query));

        return query;
    }

    // Cache of all queries with 2 included components. Tuple elements are:
    // - hash of sorted component IDs
    // - sorted array of component IDs
    // - the query itself
    private readonly List<(uint, int[], QueryDescription)> _queryCache2 = [ ];

    /// <summary>
    /// Get a query that finds entities which include all of the given types. This query
    /// will be shared with other requests for the same set of types.
    /// </summary>
    /// <returns>A query that finds entities which include all of the given types</returns>
    private QueryDescription GetCachedQuery<T0, T1>()
        where T0 : IComponent
            where T1 : IComponent
    {
        // Accumulate all components in ascending order
        Span<int> orderedComponents = [
            ComponentID<T0>.ID.Value,
            ComponentID<T1>.ID.Value,
        ];
        orderedComponents.Sort();

        // Calculate hash of components, for fast rejection
        var hash = xxHash32.ComputeHash(MemoryMarshal.Cast<int, byte>(orderedComponents), 42);

        // Find query that matches these types
        foreach (var (h, c, q) in _queryCache2)
        {
            // Early exit on hash check.
            if (h != hash)
                continue;

            // Since the sequences are sorted by component ID these should be identical
            if (!orderedComponents.SequenceEqual(c))
                continue;

            return q;
        }

        // Didn't find one, create it now
        var query = new QueryBuilder()
            .Include<T0>()
            .Include<T1>()
            .Build(this);

        // Add query to the cache
        _queryCache2.Add((hash, orderedComponents.ToArray(), query));

        return query;
    }

    // Cache of all queries with 3 included components. Tuple elements are:
    // - hash of sorted component IDs
    // - sorted array of component IDs
    // - the query itself
    private readonly List<(uint, int[], QueryDescription)> _queryCache3 = [ ];

    /// <summary>
    /// Get a query that finds entities which include all of the given types. This query
    /// will be shared with other requests for the same set of types.
    /// </summary>
    /// <returns>A query that finds entities which include all of the given types</returns>
    private QueryDescription GetCachedQuery<T0, T1, T2>()
        where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
    {
        // Accumulate all components in ascending order
        Span<int> orderedComponents = [
            ComponentID<T0>.ID.Value,
            ComponentID<T1>.ID.Value,
            ComponentID<T2>.ID.Value,
        ];
        orderedComponents.Sort();

        // Calculate hash of components, for fast rejection
        var hash = xxHash32.ComputeHash(MemoryMarshal.Cast<int, byte>(orderedComponents), 42);

        // Find query that matches these types
        foreach (var (h, c, q) in _queryCache3)
        {
            // Early exit on hash check.
            if (h != hash)
                continue;

            // Since the sequences are sorted by component ID these should be identical
            if (!orderedComponents.SequenceEqual(c))
                continue;

            return q;
        }

        // Didn't find one, create it now
        var query = new QueryBuilder()
            .Include<T0>()
            .Include<T1>()
            .Include<T2>()
            .Build(this);

        // Add query to the cache
        _queryCache3.Add((hash, orderedComponents.ToArray(), query));

        return query;
    }

    // Cache of all queries with 4 included components. Tuple elements are:
    // - hash of sorted component IDs
    // - sorted array of component IDs
    // - the query itself
    private readonly List<(uint, int[], QueryDescription)> _queryCache4 = [ ];

    /// <summary>
    /// Get a query that finds entities which include all of the given types. This query
    /// will be shared with other requests for the same set of types.
    /// </summary>
    /// <returns>A query that finds entities which include all of the given types</returns>
    private QueryDescription GetCachedQuery<T0, T1, T2, T3>()
        where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
    {
        // Accumulate all components in ascending order
        Span<int> orderedComponents = [
            ComponentID<T0>.ID.Value,
            ComponentID<T1>.ID.Value,
            ComponentID<T2>.ID.Value,
            ComponentID<T3>.ID.Value,
        ];
        orderedComponents.Sort();

        // Calculate hash of components, for fast rejection
        var hash = xxHash32.ComputeHash(MemoryMarshal.Cast<int, byte>(orderedComponents), 42);

        // Find query that matches these types
        foreach (var (h, c, q) in _queryCache4)
        {
            // Early exit on hash check.
            if (h != hash)
                continue;

            // Since the sequences are sorted by component ID these should be identical
            if (!orderedComponents.SequenceEqual(c))
                continue;

            return q;
        }

        // Didn't find one, create it now
        var query = new QueryBuilder()
            .Include<T0>()
            .Include<T1>()
            .Include<T2>()
            .Include<T3>()
            .Build(this);

        // Add query to the cache
        _queryCache4.Add((hash, orderedComponents.ToArray(), query));

        return query;
    }

    // Cache of all queries with 5 included components. Tuple elements are:
    // - hash of sorted component IDs
    // - sorted array of component IDs
    // - the query itself
    private readonly List<(uint, int[], QueryDescription)> _queryCache5 = [ ];

    /// <summary>
    /// Get a query that finds entities which include all of the given types. This query
    /// will be shared with other requests for the same set of types.
    /// </summary>
    /// <returns>A query that finds entities which include all of the given types</returns>
    private QueryDescription GetCachedQuery<T0, T1, T2, T3, T4>()
        where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
    {
        // Accumulate all components in ascending order
        Span<int> orderedComponents = [
            ComponentID<T0>.ID.Value,
            ComponentID<T1>.ID.Value,
            ComponentID<T2>.ID.Value,
            ComponentID<T3>.ID.Value,
            ComponentID<T4>.ID.Value,
        ];
        orderedComponents.Sort();

        // Calculate hash of components, for fast rejection
        var hash = xxHash32.ComputeHash(MemoryMarshal.Cast<int, byte>(orderedComponents), 42);

        // Find query that matches these types
        foreach (var (h, c, q) in _queryCache5)
        {
            // Early exit on hash check.
            if (h != hash)
                continue;

            // Since the sequences are sorted by component ID these should be identical
            if (!orderedComponents.SequenceEqual(c))
                continue;

            return q;
        }

        // Didn't find one, create it now
        var query = new QueryBuilder()
            .Include<T0>()
            .Include<T1>()
            .Include<T2>()
            .Include<T3>()
            .Include<T4>()
            .Build(this);

        // Add query to the cache
        _queryCache5.Add((hash, orderedComponents.ToArray(), query));

        return query;
    }

    // Cache of all queries with 6 included components. Tuple elements are:
    // - hash of sorted component IDs
    // - sorted array of component IDs
    // - the query itself
    private readonly List<(uint, int[], QueryDescription)> _queryCache6 = [ ];

    /// <summary>
    /// Get a query that finds entities which include all of the given types. This query
    /// will be shared with other requests for the same set of types.
    /// </summary>
    /// <returns>A query that finds entities which include all of the given types</returns>
    private QueryDescription GetCachedQuery<T0, T1, T2, T3, T4, T5>()
        where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
    {
        // Accumulate all components in ascending order
        Span<int> orderedComponents = [
            ComponentID<T0>.ID.Value,
            ComponentID<T1>.ID.Value,
            ComponentID<T2>.ID.Value,
            ComponentID<T3>.ID.Value,
            ComponentID<T4>.ID.Value,
            ComponentID<T5>.ID.Value,
        ];
        orderedComponents.Sort();

        // Calculate hash of components, for fast rejection
        var hash = xxHash32.ComputeHash(MemoryMarshal.Cast<int, byte>(orderedComponents), 42);

        // Find query that matches these types
        foreach (var (h, c, q) in _queryCache6)
        {
            // Early exit on hash check.
            if (h != hash)
                continue;

            // Since the sequences are sorted by component ID these should be identical
            if (!orderedComponents.SequenceEqual(c))
                continue;

            return q;
        }

        // Didn't find one, create it now
        var query = new QueryBuilder()
            .Include<T0>()
            .Include<T1>()
            .Include<T2>()
            .Include<T3>()
            .Include<T4>()
            .Include<T5>()
            .Build(this);

        // Add query to the cache
        _queryCache6.Add((hash, orderedComponents.ToArray(), query));

        return query;
    }

    // Cache of all queries with 7 included components. Tuple elements are:
    // - hash of sorted component IDs
    // - sorted array of component IDs
    // - the query itself
    private readonly List<(uint, int[], QueryDescription)> _queryCache7 = [ ];

    /// <summary>
    /// Get a query that finds entities which include all of the given types. This query
    /// will be shared with other requests for the same set of types.
    /// </summary>
    /// <returns>A query that finds entities which include all of the given types</returns>
    private QueryDescription GetCachedQuery<T0, T1, T2, T3, T4, T5, T6>()
        where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
    {
        // Accumulate all components in ascending order
        Span<int> orderedComponents = [
            ComponentID<T0>.ID.Value,
            ComponentID<T1>.ID.Value,
            ComponentID<T2>.ID.Value,
            ComponentID<T3>.ID.Value,
            ComponentID<T4>.ID.Value,
            ComponentID<T5>.ID.Value,
            ComponentID<T6>.ID.Value,
        ];
        orderedComponents.Sort();

        // Calculate hash of components, for fast rejection
        var hash = xxHash32.ComputeHash(MemoryMarshal.Cast<int, byte>(orderedComponents), 42);

        // Find query that matches these types
        foreach (var (h, c, q) in _queryCache7)
        {
            // Early exit on hash check.
            if (h != hash)
                continue;

            // Since the sequences are sorted by component ID these should be identical
            if (!orderedComponents.SequenceEqual(c))
                continue;

            return q;
        }

        // Didn't find one, create it now
        var query = new QueryBuilder()
            .Include<T0>()
            .Include<T1>()
            .Include<T2>()
            .Include<T3>()
            .Include<T4>()
            .Include<T5>()
            .Include<T6>()
            .Build(this);

        // Add query to the cache
        _queryCache7.Add((hash, orderedComponents.ToArray(), query));

        return query;
    }

    // Cache of all queries with 8 included components. Tuple elements are:
    // - hash of sorted component IDs
    // - sorted array of component IDs
    // - the query itself
    private readonly List<(uint, int[], QueryDescription)> _queryCache8 = [ ];

    /// <summary>
    /// Get a query that finds entities which include all of the given types. This query
    /// will be shared with other requests for the same set of types.
    /// </summary>
    /// <returns>A query that finds entities which include all of the given types</returns>
    private QueryDescription GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7>()
        where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
    {
        // Accumulate all components in ascending order
        Span<int> orderedComponents = [
            ComponentID<T0>.ID.Value,
            ComponentID<T1>.ID.Value,
            ComponentID<T2>.ID.Value,
            ComponentID<T3>.ID.Value,
            ComponentID<T4>.ID.Value,
            ComponentID<T5>.ID.Value,
            ComponentID<T6>.ID.Value,
            ComponentID<T7>.ID.Value,
        ];
        orderedComponents.Sort();

        // Calculate hash of components, for fast rejection
        var hash = xxHash32.ComputeHash(MemoryMarshal.Cast<int, byte>(orderedComponents), 42);

        // Find query that matches these types
        foreach (var (h, c, q) in _queryCache8)
        {
            // Early exit on hash check.
            if (h != hash)
                continue;

            // Since the sequences are sorted by component ID these should be identical
            if (!orderedComponents.SequenceEqual(c))
                continue;

            return q;
        }

        // Didn't find one, create it now
        var query = new QueryBuilder()
            .Include<T0>()
            .Include<T1>()
            .Include<T2>()
            .Include<T3>()
            .Include<T4>()
            .Include<T5>()
            .Include<T6>()
            .Include<T7>()
            .Build(this);

        // Add query to the cache
        _queryCache8.Add((hash, orderedComponents.ToArray(), query));

        return query;
    }

    // Cache of all queries with 9 included components. Tuple elements are:
    // - hash of sorted component IDs
    // - sorted array of component IDs
    // - the query itself
    private readonly List<(uint, int[], QueryDescription)> _queryCache9 = [ ];

    /// <summary>
    /// Get a query that finds entities which include all of the given types. This query
    /// will be shared with other requests for the same set of types.
    /// </summary>
    /// <returns>A query that finds entities which include all of the given types</returns>
    private QueryDescription GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8>()
        where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
            where T8 : IComponent
    {
        // Accumulate all components in ascending order
        Span<int> orderedComponents = [
            ComponentID<T0>.ID.Value,
            ComponentID<T1>.ID.Value,
            ComponentID<T2>.ID.Value,
            ComponentID<T3>.ID.Value,
            ComponentID<T4>.ID.Value,
            ComponentID<T5>.ID.Value,
            ComponentID<T6>.ID.Value,
            ComponentID<T7>.ID.Value,
            ComponentID<T8>.ID.Value,
        ];
        orderedComponents.Sort();

        // Calculate hash of components, for fast rejection
        var hash = xxHash32.ComputeHash(MemoryMarshal.Cast<int, byte>(orderedComponents), 42);

        // Find query that matches these types
        foreach (var (h, c, q) in _queryCache9)
        {
            // Early exit on hash check.
            if (h != hash)
                continue;

            // Since the sequences are sorted by component ID these should be identical
            if (!orderedComponents.SequenceEqual(c))
                continue;

            return q;
        }

        // Didn't find one, create it now
        var query = new QueryBuilder()
            .Include<T0>()
            .Include<T1>()
            .Include<T2>()
            .Include<T3>()
            .Include<T4>()
            .Include<T5>()
            .Include<T6>()
            .Include<T7>()
            .Include<T8>()
            .Build(this);

        // Add query to the cache
        _queryCache9.Add((hash, orderedComponents.ToArray(), query));

        return query;
    }


}


