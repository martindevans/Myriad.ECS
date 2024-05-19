using Myriad.ECS.Queries;
using Myriad.ECS.IDs;
using Myriad.ECS.xxHash;
using Myriad.ECS.Collections;
using System.Runtime.InteropServices;

#if NETSTANDARD2_1
using Myriad.ECS.Extensions;
#endif

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedParameter.Global
// ReSharper disable LoopCanBeConvertedToQuery
// ReSharper disable CheckNamespace
// ReSharper disable ArrangeAccessorOwnerBody

namespace Myriad.ECS.Worlds;

public partial class World
{
    // Cache of all queries with 1 included components. Tuple elements are:
    // - component ID
    // - the query itself
    private readonly SortedList<int, QueryDescription> _queryCache1 = [ ];

    /// <summary>
    /// Get a query that finds entities which include all of the given types. This query
    /// will be shared with other requests for the same set of types.
    /// </summary>
    /// <returns>A query that finds entities which include all of the given types</returns>
    private QueryDescription GetCachedQuery<T0>()
        where T0 : IComponent
    {
        var component = ComponentID<T0>.ID.Value;

        // Find query that matches this types
        if (_queryCache1.TryGetValue(component, out var q))
            return q;

        // Didn't find one, create it now
        var query = new QueryBuilder()
            .Include<T0>()
            .Build(this);

        // Add query to the cache
        _queryCache1.Add(component, query);

        return query;
    }

    // Cache of all queries with 2 included components. Tuple elements are:
    // - component IDs combined together
    // - the query itself
    private readonly SortedList<long, QueryDescription> _queryCache2 = [ ];

    /// <summary>
    /// Get a query that finds entities which include all of the given types. This query
    /// will be shared with other requests for the same set of types.
    /// </summary>
    /// <returns>A query that finds entities which include all of the given types</returns>
    private QueryDescription GetCachedQuery<T0, T1>()
        where T0 : IComponent
        where T1 : IComponent
    {
        var u = new Union64
        {
            I0 = ComponentID<T0>.ID.Value,
            I1 = ComponentID<T1>.ID.Value,
        };

        // Sort them into order
        if (u.I0 > u.I1)
            (u.I0, u.I1) = (u.I1, u.I0);

        var key = u.Long;

        // Find query that matches this types
        if (_queryCache2.TryGetValue(key, out var q))
            return q;

        // Didn't find one, create it now
        var query = new QueryBuilder()
            .Include<T0>()
            .Include<T1>()
            .Build(this);

        // Add query to the cache
        _queryCache2.Add(key, query);

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
        var byteSpan = MemoryMarshal.Cast<int, byte>(orderedComponents);
        var hash = xxHash32.ComputeHash(byteSpan, seed: 42);

        // Find query that matches these types
        foreach (var (h, c, q) in _queryCache3)
        {
            // Early exit on hash check.
            if (h != hash)
                continue;

            // Since the sequences are sorted by component ID these should be identical
            // Comparing two int spans should be very fast (SIMD accelerated)
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
        var byteSpan = MemoryMarshal.Cast<int, byte>(orderedComponents);
        var hash = xxHash32.ComputeHash(byteSpan, seed: 42);

        // Find query that matches these types
        foreach (var (h, c, q) in _queryCache4)
        {
            // Early exit on hash check.
            if (h != hash)
                continue;

            // Since the sequences are sorted by component ID these should be identical
            // Comparing two int spans should be very fast (SIMD accelerated)
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
        var byteSpan = MemoryMarshal.Cast<int, byte>(orderedComponents);
        var hash = xxHash32.ComputeHash(byteSpan, seed: 42);

        // Find query that matches these types
        foreach (var (h, c, q) in _queryCache5)
        {
            // Early exit on hash check.
            if (h != hash)
                continue;

            // Since the sequences are sorted by component ID these should be identical
            // Comparing two int spans should be very fast (SIMD accelerated)
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
        var byteSpan = MemoryMarshal.Cast<int, byte>(orderedComponents);
        var hash = xxHash32.ComputeHash(byteSpan, seed: 42);

        // Find query that matches these types
        foreach (var (h, c, q) in _queryCache6)
        {
            // Early exit on hash check.
            if (h != hash)
                continue;

            // Since the sequences are sorted by component ID these should be identical
            // Comparing two int spans should be very fast (SIMD accelerated)
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
        var byteSpan = MemoryMarshal.Cast<int, byte>(orderedComponents);
        var hash = xxHash32.ComputeHash(byteSpan, seed: 42);

        // Find query that matches these types
        foreach (var (h, c, q) in _queryCache7)
        {
            // Early exit on hash check.
            if (h != hash)
                continue;

            // Since the sequences are sorted by component ID these should be identical
            // Comparing two int spans should be very fast (SIMD accelerated)
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
        var byteSpan = MemoryMarshal.Cast<int, byte>(orderedComponents);
        var hash = xxHash32.ComputeHash(byteSpan, seed: 42);

        // Find query that matches these types
        foreach (var (h, c, q) in _queryCache8)
        {
            // Early exit on hash check.
            if (h != hash)
                continue;

            // Since the sequences are sorted by component ID these should be identical
            // Comparing two int spans should be very fast (SIMD accelerated)
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
        var byteSpan = MemoryMarshal.Cast<int, byte>(orderedComponents);
        var hash = xxHash32.ComputeHash(byteSpan, seed: 42);

        // Find query that matches these types
        foreach (var (h, c, q) in _queryCache9)
        {
            // Early exit on hash check.
            if (h != hash)
                continue;

            // Since the sequences are sorted by component ID these should be identical
            // Comparing two int spans should be very fast (SIMD accelerated)
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


    // Cache of all queries with 10 included components. Tuple elements are:
    // - hash of sorted component IDs
    // - sorted array of component IDs
    // - the query itself
    private readonly List<(uint, int[], QueryDescription)> _queryCache10 = [ ];

    /// <summary>
    /// Get a query that finds entities which include all of the given types. This query
    /// will be shared with other requests for the same set of types.
    /// </summary>
    /// <returns>A query that finds entities which include all of the given types</returns>
    private QueryDescription GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>()
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
        where T8 : IComponent
        where T9 : IComponent
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
            ComponentID<T9>.ID.Value,
        ];
        orderedComponents.Sort();

        // Calculate hash of components, for fast rejection
        var byteSpan = MemoryMarshal.Cast<int, byte>(orderedComponents);
        var hash = xxHash32.ComputeHash(byteSpan, seed: 42);

        // Find query that matches these types
        foreach (var (h, c, q) in _queryCache10)
        {
            // Early exit on hash check.
            if (h != hash)
                continue;

            // Since the sequences are sorted by component ID these should be identical
            // Comparing two int spans should be very fast (SIMD accelerated)
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
            .Include<T9>()
            .Build(this);

        // Add query to the cache
        _queryCache10.Add((hash, orderedComponents.ToArray(), query));

        return query;
    }


    // Cache of all queries with 11 included components. Tuple elements are:
    // - hash of sorted component IDs
    // - sorted array of component IDs
    // - the query itself
    private readonly List<(uint, int[], QueryDescription)> _queryCache11 = [ ];

    /// <summary>
    /// Get a query that finds entities which include all of the given types. This query
    /// will be shared with other requests for the same set of types.
    /// </summary>
    /// <returns>A query that finds entities which include all of the given types</returns>
    private QueryDescription GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>()
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
        where T8 : IComponent
        where T9 : IComponent
        where T10 : IComponent
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
            ComponentID<T9>.ID.Value,
            ComponentID<T10>.ID.Value,
        ];
        orderedComponents.Sort();

        // Calculate hash of components, for fast rejection
        var byteSpan = MemoryMarshal.Cast<int, byte>(orderedComponents);
        var hash = xxHash32.ComputeHash(byteSpan, seed: 42);

        // Find query that matches these types
        foreach (var (h, c, q) in _queryCache11)
        {
            // Early exit on hash check.
            if (h != hash)
                continue;

            // Since the sequences are sorted by component ID these should be identical
            // Comparing two int spans should be very fast (SIMD accelerated)
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
            .Include<T9>()
            .Include<T10>()
            .Build(this);

        // Add query to the cache
        _queryCache11.Add((hash, orderedComponents.ToArray(), query));

        return query;
    }


    // Cache of all queries with 12 included components. Tuple elements are:
    // - hash of sorted component IDs
    // - sorted array of component IDs
    // - the query itself
    private readonly List<(uint, int[], QueryDescription)> _queryCache12 = [ ];

    /// <summary>
    /// Get a query that finds entities which include all of the given types. This query
    /// will be shared with other requests for the same set of types.
    /// </summary>
    /// <returns>A query that finds entities which include all of the given types</returns>
    private QueryDescription GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>()
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
        where T8 : IComponent
        where T9 : IComponent
        where T10 : IComponent
        where T11 : IComponent
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
            ComponentID<T9>.ID.Value,
            ComponentID<T10>.ID.Value,
            ComponentID<T11>.ID.Value,
        ];
        orderedComponents.Sort();

        // Calculate hash of components, for fast rejection
        var byteSpan = MemoryMarshal.Cast<int, byte>(orderedComponents);
        var hash = xxHash32.ComputeHash(byteSpan, seed: 42);

        // Find query that matches these types
        foreach (var (h, c, q) in _queryCache12)
        {
            // Early exit on hash check.
            if (h != hash)
                continue;

            // Since the sequences are sorted by component ID these should be identical
            // Comparing two int spans should be very fast (SIMD accelerated)
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
            .Include<T9>()
            .Include<T10>()
            .Include<T11>()
            .Build(this);

        // Add query to the cache
        _queryCache12.Add((hash, orderedComponents.ToArray(), query));

        return query;
    }


    // Cache of all queries with 13 included components. Tuple elements are:
    // - hash of sorted component IDs
    // - sorted array of component IDs
    // - the query itself
    private readonly List<(uint, int[], QueryDescription)> _queryCache13 = [ ];

    /// <summary>
    /// Get a query that finds entities which include all of the given types. This query
    /// will be shared with other requests for the same set of types.
    /// </summary>
    /// <returns>A query that finds entities which include all of the given types</returns>
    private QueryDescription GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>()
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
        where T8 : IComponent
        where T9 : IComponent
        where T10 : IComponent
        where T11 : IComponent
        where T12 : IComponent
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
            ComponentID<T9>.ID.Value,
            ComponentID<T10>.ID.Value,
            ComponentID<T11>.ID.Value,
            ComponentID<T12>.ID.Value,
        ];
        orderedComponents.Sort();

        // Calculate hash of components, for fast rejection
        var byteSpan = MemoryMarshal.Cast<int, byte>(orderedComponents);
        var hash = xxHash32.ComputeHash(byteSpan, seed: 42);

        // Find query that matches these types
        foreach (var (h, c, q) in _queryCache13)
        {
            // Early exit on hash check.
            if (h != hash)
                continue;

            // Since the sequences are sorted by component ID these should be identical
            // Comparing two int spans should be very fast (SIMD accelerated)
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
            .Include<T9>()
            .Include<T10>()
            .Include<T11>()
            .Include<T12>()
            .Build(this);

        // Add query to the cache
        _queryCache13.Add((hash, orderedComponents.ToArray(), query));

        return query;
    }


    // Cache of all queries with 14 included components. Tuple elements are:
    // - hash of sorted component IDs
    // - sorted array of component IDs
    // - the query itself
    private readonly List<(uint, int[], QueryDescription)> _queryCache14 = [ ];

    /// <summary>
    /// Get a query that finds entities which include all of the given types. This query
    /// will be shared with other requests for the same set of types.
    /// </summary>
    /// <returns>A query that finds entities which include all of the given types</returns>
    private QueryDescription GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>()
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
        where T8 : IComponent
        where T9 : IComponent
        where T10 : IComponent
        where T11 : IComponent
        where T12 : IComponent
        where T13 : IComponent
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
            ComponentID<T9>.ID.Value,
            ComponentID<T10>.ID.Value,
            ComponentID<T11>.ID.Value,
            ComponentID<T12>.ID.Value,
            ComponentID<T13>.ID.Value,
        ];
        orderedComponents.Sort();

        // Calculate hash of components, for fast rejection
        var byteSpan = MemoryMarshal.Cast<int, byte>(orderedComponents);
        var hash = xxHash32.ComputeHash(byteSpan, seed: 42);

        // Find query that matches these types
        foreach (var (h, c, q) in _queryCache14)
        {
            // Early exit on hash check.
            if (h != hash)
                continue;

            // Since the sequences are sorted by component ID these should be identical
            // Comparing two int spans should be very fast (SIMD accelerated)
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
            .Include<T9>()
            .Include<T10>()
            .Include<T11>()
            .Include<T12>()
            .Include<T13>()
            .Build(this);

        // Add query to the cache
        _queryCache14.Add((hash, orderedComponents.ToArray(), query));

        return query;
    }


    // Cache of all queries with 15 included components. Tuple elements are:
    // - hash of sorted component IDs
    // - sorted array of component IDs
    // - the query itself
    private readonly List<(uint, int[], QueryDescription)> _queryCache15 = [ ];

    /// <summary>
    /// Get a query that finds entities which include all of the given types. This query
    /// will be shared with other requests for the same set of types.
    /// </summary>
    /// <returns>A query that finds entities which include all of the given types</returns>
    private QueryDescription GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>()
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
        where T8 : IComponent
        where T9 : IComponent
        where T10 : IComponent
        where T11 : IComponent
        where T12 : IComponent
        where T13 : IComponent
        where T14 : IComponent
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
            ComponentID<T9>.ID.Value,
            ComponentID<T10>.ID.Value,
            ComponentID<T11>.ID.Value,
            ComponentID<T12>.ID.Value,
            ComponentID<T13>.ID.Value,
            ComponentID<T14>.ID.Value,
        ];
        orderedComponents.Sort();

        // Calculate hash of components, for fast rejection
        var byteSpan = MemoryMarshal.Cast<int, byte>(orderedComponents);
        var hash = xxHash32.ComputeHash(byteSpan, seed: 42);

        // Find query that matches these types
        foreach (var (h, c, q) in _queryCache15)
        {
            // Early exit on hash check.
            if (h != hash)
                continue;

            // Since the sequences are sorted by component ID these should be identical
            // Comparing two int spans should be very fast (SIMD accelerated)
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
            .Include<T9>()
            .Include<T10>()
            .Include<T11>()
            .Include<T12>()
            .Include<T13>()
            .Include<T14>()
            .Build(this);

        // Add query to the cache
        _queryCache15.Add((hash, orderedComponents.ToArray(), query));

        return query;
    }


    // Cache of all queries with 16 included components. Tuple elements are:
    // - hash of sorted component IDs
    // - sorted array of component IDs
    // - the query itself
    private readonly List<(uint, int[], QueryDescription)> _queryCache16 = [ ];

    /// <summary>
    /// Get a query that finds entities which include all of the given types. This query
    /// will be shared with other requests for the same set of types.
    /// </summary>
    /// <returns>A query that finds entities which include all of the given types</returns>
    private QueryDescription GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>()
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
        where T8 : IComponent
        where T9 : IComponent
        where T10 : IComponent
        where T11 : IComponent
        where T12 : IComponent
        where T13 : IComponent
        where T14 : IComponent
        where T15 : IComponent
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
            ComponentID<T9>.ID.Value,
            ComponentID<T10>.ID.Value,
            ComponentID<T11>.ID.Value,
            ComponentID<T12>.ID.Value,
            ComponentID<T13>.ID.Value,
            ComponentID<T14>.ID.Value,
            ComponentID<T15>.ID.Value,
        ];
        orderedComponents.Sort();

        // Calculate hash of components, for fast rejection
        var byteSpan = MemoryMarshal.Cast<int, byte>(orderedComponents);
        var hash = xxHash32.ComputeHash(byteSpan, seed: 42);

        // Find query that matches these types
        foreach (var (h, c, q) in _queryCache16)
        {
            // Early exit on hash check.
            if (h != hash)
                continue;

            // Since the sequences are sorted by component ID these should be identical
            // Comparing two int spans should be very fast (SIMD accelerated)
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
            .Include<T9>()
            .Include<T10>()
            .Include<T11>()
            .Include<T12>()
            .Include<T13>()
            .Include<T14>()
            .Include<T15>()
            .Build(this);

        // Add query to the cache
        _queryCache16.Add((hash, orderedComponents.ToArray(), query));

        return query;
    }


}


