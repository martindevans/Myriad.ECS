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
    // Cache of all queries with 1 included components.
    private readonly SortedList<int, QueryDescription> _queryCache1 = [ ];
    private readonly ReaderWriterLockSlim _lock1 = new();

    /// <summary>
    /// Get a query that finds entities which include all of the given types. This query
    /// will be shared with other requests for the same set of types.
    /// </summary>
    /// <returns>A query that finds entities which include all of the given types</returns>
    private QueryDescription GetCachedQuery<T0>()
        where T0 : IComponent
    {
        var component = ComponentID<T0>.ID.Value;

        _lock1.EnterReadLock();
        try
        {
            // Find query that matches this types
            if (_queryCache1.TryGetValue(component, out var q))
                return q;
        }
        finally
        {
            _lock1.ExitReadLock();
        }

        _lock1.EnterWriteLock();
        try
        {
            // Didn't find one, create it now
            var query = new QueryBuilder()
                .Include<T0>()
                .Build(this);

            // Add query to the cache
            _queryCache1[component] = query;

            return query;
        }
        finally
        {
            _lock1.ExitWriteLock();
        }
    }

    // Cache of all queries with 2 included components. Key is the 2 component IDs combined together
    private readonly SortedList<long, QueryDescription> _queryCache2 = [ ];
    private readonly ReaderWriterLockSlim _lock2 = new();

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

        _lock2.EnterReadLock();
        try
        {
            // Find query that matches this types
            if (_queryCache2.TryGetValue(key, out var q))
                return q;
        }
        finally
        {
            _lock2.ExitReadLock();
        }

        _lock2.EnterWriteLock();
        try
        {
            // Didn't find one, create it now
            var query = new QueryBuilder()
                .Include<T0>()
                .Include<T1>()
                .Build(this);

            // Add query to the cache
            _queryCache2[key] = query;

            return query;
        }
        finally
        {
            _lock2.ExitWriteLock();
        }
    }


    // Cache of all queries with 3 included components. Tuple elements are:
    // - hash of sorted component IDs
    // - sorted array of component IDs
    // - the query itself
    private readonly SortedList<ulong, List<(int[], QueryDescription)>> _queryCache3 = [ ];

    private readonly ReaderWriterLockSlim _lock3 = new();

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
        var hash = xxHash64.ComputeHash(byteSpan, seed: 42);

        _lock3.EnterReadLock();
        try
        {
            // Get the list of items with this hash
            if (_queryCache3.TryGetValue(hash, out var list))
            {
                // Find query that matches these types
                foreach (var (c, q) in list)
                {
                    // Since the sequences are sorted by component ID these should be identical
                    // Comparing two int spans should be very fast (SIMD accelerated)
                    if (!orderedComponents.SequenceEqual(c))
                        continue;

                    return q;
                }
            }
        }
        finally
        {
            _lock3.ExitReadLock();
        }

        // Didn't find query, create it now
        _lock3.EnterWriteLock();
        try
        {
            // Get or create list of items with this hash
            if (!_queryCache3.TryGetValue(hash, out var list))
            {
                list = [ ];
                _queryCache3.Add(hash, list);
            }
            
            // Create query
            var query = new QueryBuilder()
                .Include<T0>()
                .Include<T1>()
                .Include<T2>()
                .Build(this);

            // Add query to the cache
            list.Add((orderedComponents.ToArray(), query));

            return query;
        }
        finally
        {
            _lock3.ExitWriteLock();
        }
    }


    // Cache of all queries with 4 included components. Tuple elements are:
    // - hash of sorted component IDs
    // - sorted array of component IDs
    // - the query itself
    private readonly SortedList<ulong, List<(int[], QueryDescription)>> _queryCache4 = [ ];

    private readonly ReaderWriterLockSlim _lock4 = new();

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
        var hash = xxHash64.ComputeHash(byteSpan, seed: 42);

        _lock4.EnterReadLock();
        try
        {
            // Get the list of items with this hash
            if (_queryCache4.TryGetValue(hash, out var list))
            {
                // Find query that matches these types
                foreach (var (c, q) in list)
                {
                    // Since the sequences are sorted by component ID these should be identical
                    // Comparing two int spans should be very fast (SIMD accelerated)
                    if (!orderedComponents.SequenceEqual(c))
                        continue;

                    return q;
                }
            }
        }
        finally
        {
            _lock4.ExitReadLock();
        }

        // Didn't find query, create it now
        _lock4.EnterWriteLock();
        try
        {
            // Get or create list of items with this hash
            if (!_queryCache4.TryGetValue(hash, out var list))
            {
                list = [ ];
                _queryCache4.Add(hash, list);
            }
            
            // Create query
            var query = new QueryBuilder()
                .Include<T0>()
                .Include<T1>()
                .Include<T2>()
                .Include<T3>()
                .Build(this);

            // Add query to the cache
            list.Add((orderedComponents.ToArray(), query));

            return query;
        }
        finally
        {
            _lock4.ExitWriteLock();
        }
    }


    // Cache of all queries with 5 included components. Tuple elements are:
    // - hash of sorted component IDs
    // - sorted array of component IDs
    // - the query itself
    private readonly SortedList<ulong, List<(int[], QueryDescription)>> _queryCache5 = [ ];

    private readonly ReaderWriterLockSlim _lock5 = new();

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
        var hash = xxHash64.ComputeHash(byteSpan, seed: 42);

        _lock5.EnterReadLock();
        try
        {
            // Get the list of items with this hash
            if (_queryCache5.TryGetValue(hash, out var list))
            {
                // Find query that matches these types
                foreach (var (c, q) in list)
                {
                    // Since the sequences are sorted by component ID these should be identical
                    // Comparing two int spans should be very fast (SIMD accelerated)
                    if (!orderedComponents.SequenceEqual(c))
                        continue;

                    return q;
                }
            }
        }
        finally
        {
            _lock5.ExitReadLock();
        }

        // Didn't find query, create it now
        _lock5.EnterWriteLock();
        try
        {
            // Get or create list of items with this hash
            if (!_queryCache5.TryGetValue(hash, out var list))
            {
                list = [ ];
                _queryCache5.Add(hash, list);
            }
            
            // Create query
            var query = new QueryBuilder()
                .Include<T0>()
                .Include<T1>()
                .Include<T2>()
                .Include<T3>()
                .Include<T4>()
                .Build(this);

            // Add query to the cache
            list.Add((orderedComponents.ToArray(), query));

            return query;
        }
        finally
        {
            _lock5.ExitWriteLock();
        }
    }


    // Cache of all queries with 6 included components. Tuple elements are:
    // - hash of sorted component IDs
    // - sorted array of component IDs
    // - the query itself
    private readonly SortedList<ulong, List<(int[], QueryDescription)>> _queryCache6 = [ ];

    private readonly ReaderWriterLockSlim _lock6 = new();

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
        var hash = xxHash64.ComputeHash(byteSpan, seed: 42);

        _lock6.EnterReadLock();
        try
        {
            // Get the list of items with this hash
            if (_queryCache6.TryGetValue(hash, out var list))
            {
                // Find query that matches these types
                foreach (var (c, q) in list)
                {
                    // Since the sequences are sorted by component ID these should be identical
                    // Comparing two int spans should be very fast (SIMD accelerated)
                    if (!orderedComponents.SequenceEqual(c))
                        continue;

                    return q;
                }
            }
        }
        finally
        {
            _lock6.ExitReadLock();
        }

        // Didn't find query, create it now
        _lock6.EnterWriteLock();
        try
        {
            // Get or create list of items with this hash
            if (!_queryCache6.TryGetValue(hash, out var list))
            {
                list = [ ];
                _queryCache6.Add(hash, list);
            }
            
            // Create query
            var query = new QueryBuilder()
                .Include<T0>()
                .Include<T1>()
                .Include<T2>()
                .Include<T3>()
                .Include<T4>()
                .Include<T5>()
                .Build(this);

            // Add query to the cache
            list.Add((orderedComponents.ToArray(), query));

            return query;
        }
        finally
        {
            _lock6.ExitWriteLock();
        }
    }


    // Cache of all queries with 7 included components. Tuple elements are:
    // - hash of sorted component IDs
    // - sorted array of component IDs
    // - the query itself
    private readonly SortedList<ulong, List<(int[], QueryDescription)>> _queryCache7 = [ ];

    private readonly ReaderWriterLockSlim _lock7 = new();

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
        var hash = xxHash64.ComputeHash(byteSpan, seed: 42);

        _lock7.EnterReadLock();
        try
        {
            // Get the list of items with this hash
            if (_queryCache7.TryGetValue(hash, out var list))
            {
                // Find query that matches these types
                foreach (var (c, q) in list)
                {
                    // Since the sequences are sorted by component ID these should be identical
                    // Comparing two int spans should be very fast (SIMD accelerated)
                    if (!orderedComponents.SequenceEqual(c))
                        continue;

                    return q;
                }
            }
        }
        finally
        {
            _lock7.ExitReadLock();
        }

        // Didn't find query, create it now
        _lock7.EnterWriteLock();
        try
        {
            // Get or create list of items with this hash
            if (!_queryCache7.TryGetValue(hash, out var list))
            {
                list = [ ];
                _queryCache7.Add(hash, list);
            }
            
            // Create query
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
            list.Add((orderedComponents.ToArray(), query));

            return query;
        }
        finally
        {
            _lock7.ExitWriteLock();
        }
    }


    // Cache of all queries with 8 included components. Tuple elements are:
    // - hash of sorted component IDs
    // - sorted array of component IDs
    // - the query itself
    private readonly SortedList<ulong, List<(int[], QueryDescription)>> _queryCache8 = [ ];

    private readonly ReaderWriterLockSlim _lock8 = new();

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
        var hash = xxHash64.ComputeHash(byteSpan, seed: 42);

        _lock8.EnterReadLock();
        try
        {
            // Get the list of items with this hash
            if (_queryCache8.TryGetValue(hash, out var list))
            {
                // Find query that matches these types
                foreach (var (c, q) in list)
                {
                    // Since the sequences are sorted by component ID these should be identical
                    // Comparing two int spans should be very fast (SIMD accelerated)
                    if (!orderedComponents.SequenceEqual(c))
                        continue;

                    return q;
                }
            }
        }
        finally
        {
            _lock8.ExitReadLock();
        }

        // Didn't find query, create it now
        _lock8.EnterWriteLock();
        try
        {
            // Get or create list of items with this hash
            if (!_queryCache8.TryGetValue(hash, out var list))
            {
                list = [ ];
                _queryCache8.Add(hash, list);
            }
            
            // Create query
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
            list.Add((orderedComponents.ToArray(), query));

            return query;
        }
        finally
        {
            _lock8.ExitWriteLock();
        }
    }


    // Cache of all queries with 9 included components. Tuple elements are:
    // - hash of sorted component IDs
    // - sorted array of component IDs
    // - the query itself
    private readonly SortedList<ulong, List<(int[], QueryDescription)>> _queryCache9 = [ ];

    private readonly ReaderWriterLockSlim _lock9 = new();

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
        var hash = xxHash64.ComputeHash(byteSpan, seed: 42);

        _lock9.EnterReadLock();
        try
        {
            // Get the list of items with this hash
            if (_queryCache9.TryGetValue(hash, out var list))
            {
                // Find query that matches these types
                foreach (var (c, q) in list)
                {
                    // Since the sequences are sorted by component ID these should be identical
                    // Comparing two int spans should be very fast (SIMD accelerated)
                    if (!orderedComponents.SequenceEqual(c))
                        continue;

                    return q;
                }
            }
        }
        finally
        {
            _lock9.ExitReadLock();
        }

        // Didn't find query, create it now
        _lock9.EnterWriteLock();
        try
        {
            // Get or create list of items with this hash
            if (!_queryCache9.TryGetValue(hash, out var list))
            {
                list = [ ];
                _queryCache9.Add(hash, list);
            }
            
            // Create query
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
            list.Add((orderedComponents.ToArray(), query));

            return query;
        }
        finally
        {
            _lock9.ExitWriteLock();
        }
    }


    // Cache of all queries with 10 included components. Tuple elements are:
    // - hash of sorted component IDs
    // - sorted array of component IDs
    // - the query itself
    private readonly SortedList<ulong, List<(int[], QueryDescription)>> _queryCache10 = [ ];

    private readonly ReaderWriterLockSlim _lock10 = new();

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
        var hash = xxHash64.ComputeHash(byteSpan, seed: 42);

        _lock10.EnterReadLock();
        try
        {
            // Get the list of items with this hash
            if (_queryCache10.TryGetValue(hash, out var list))
            {
                // Find query that matches these types
                foreach (var (c, q) in list)
                {
                    // Since the sequences are sorted by component ID these should be identical
                    // Comparing two int spans should be very fast (SIMD accelerated)
                    if (!orderedComponents.SequenceEqual(c))
                        continue;

                    return q;
                }
            }
        }
        finally
        {
            _lock10.ExitReadLock();
        }

        // Didn't find query, create it now
        _lock10.EnterWriteLock();
        try
        {
            // Get or create list of items with this hash
            if (!_queryCache10.TryGetValue(hash, out var list))
            {
                list = [ ];
                _queryCache10.Add(hash, list);
            }
            
            // Create query
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
            list.Add((orderedComponents.ToArray(), query));

            return query;
        }
        finally
        {
            _lock10.ExitWriteLock();
        }
    }


    // Cache of all queries with 11 included components. Tuple elements are:
    // - hash of sorted component IDs
    // - sorted array of component IDs
    // - the query itself
    private readonly SortedList<ulong, List<(int[], QueryDescription)>> _queryCache11 = [ ];

    private readonly ReaderWriterLockSlim _lock11 = new();

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
        var hash = xxHash64.ComputeHash(byteSpan, seed: 42);

        _lock11.EnterReadLock();
        try
        {
            // Get the list of items with this hash
            if (_queryCache11.TryGetValue(hash, out var list))
            {
                // Find query that matches these types
                foreach (var (c, q) in list)
                {
                    // Since the sequences are sorted by component ID these should be identical
                    // Comparing two int spans should be very fast (SIMD accelerated)
                    if (!orderedComponents.SequenceEqual(c))
                        continue;

                    return q;
                }
            }
        }
        finally
        {
            _lock11.ExitReadLock();
        }

        // Didn't find query, create it now
        _lock11.EnterWriteLock();
        try
        {
            // Get or create list of items with this hash
            if (!_queryCache11.TryGetValue(hash, out var list))
            {
                list = [ ];
                _queryCache11.Add(hash, list);
            }
            
            // Create query
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
            list.Add((orderedComponents.ToArray(), query));

            return query;
        }
        finally
        {
            _lock11.ExitWriteLock();
        }
    }


    // Cache of all queries with 12 included components. Tuple elements are:
    // - hash of sorted component IDs
    // - sorted array of component IDs
    // - the query itself
    private readonly SortedList<ulong, List<(int[], QueryDescription)>> _queryCache12 = [ ];

    private readonly ReaderWriterLockSlim _lock12 = new();

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
        var hash = xxHash64.ComputeHash(byteSpan, seed: 42);

        _lock12.EnterReadLock();
        try
        {
            // Get the list of items with this hash
            if (_queryCache12.TryGetValue(hash, out var list))
            {
                // Find query that matches these types
                foreach (var (c, q) in list)
                {
                    // Since the sequences are sorted by component ID these should be identical
                    // Comparing two int spans should be very fast (SIMD accelerated)
                    if (!orderedComponents.SequenceEqual(c))
                        continue;

                    return q;
                }
            }
        }
        finally
        {
            _lock12.ExitReadLock();
        }

        // Didn't find query, create it now
        _lock12.EnterWriteLock();
        try
        {
            // Get or create list of items with this hash
            if (!_queryCache12.TryGetValue(hash, out var list))
            {
                list = [ ];
                _queryCache12.Add(hash, list);
            }
            
            // Create query
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
            list.Add((orderedComponents.ToArray(), query));

            return query;
        }
        finally
        {
            _lock12.ExitWriteLock();
        }
    }


    // Cache of all queries with 13 included components. Tuple elements are:
    // - hash of sorted component IDs
    // - sorted array of component IDs
    // - the query itself
    private readonly SortedList<ulong, List<(int[], QueryDescription)>> _queryCache13 = [ ];

    private readonly ReaderWriterLockSlim _lock13 = new();

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
        var hash = xxHash64.ComputeHash(byteSpan, seed: 42);

        _lock13.EnterReadLock();
        try
        {
            // Get the list of items with this hash
            if (_queryCache13.TryGetValue(hash, out var list))
            {
                // Find query that matches these types
                foreach (var (c, q) in list)
                {
                    // Since the sequences are sorted by component ID these should be identical
                    // Comparing two int spans should be very fast (SIMD accelerated)
                    if (!orderedComponents.SequenceEqual(c))
                        continue;

                    return q;
                }
            }
        }
        finally
        {
            _lock13.ExitReadLock();
        }

        // Didn't find query, create it now
        _lock13.EnterWriteLock();
        try
        {
            // Get or create list of items with this hash
            if (!_queryCache13.TryGetValue(hash, out var list))
            {
                list = [ ];
                _queryCache13.Add(hash, list);
            }
            
            // Create query
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
            list.Add((orderedComponents.ToArray(), query));

            return query;
        }
        finally
        {
            _lock13.ExitWriteLock();
        }
    }


    // Cache of all queries with 14 included components. Tuple elements are:
    // - hash of sorted component IDs
    // - sorted array of component IDs
    // - the query itself
    private readonly SortedList<ulong, List<(int[], QueryDescription)>> _queryCache14 = [ ];

    private readonly ReaderWriterLockSlim _lock14 = new();

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
        var hash = xxHash64.ComputeHash(byteSpan, seed: 42);

        _lock14.EnterReadLock();
        try
        {
            // Get the list of items with this hash
            if (_queryCache14.TryGetValue(hash, out var list))
            {
                // Find query that matches these types
                foreach (var (c, q) in list)
                {
                    // Since the sequences are sorted by component ID these should be identical
                    // Comparing two int spans should be very fast (SIMD accelerated)
                    if (!orderedComponents.SequenceEqual(c))
                        continue;

                    return q;
                }
            }
        }
        finally
        {
            _lock14.ExitReadLock();
        }

        // Didn't find query, create it now
        _lock14.EnterWriteLock();
        try
        {
            // Get or create list of items with this hash
            if (!_queryCache14.TryGetValue(hash, out var list))
            {
                list = [ ];
                _queryCache14.Add(hash, list);
            }
            
            // Create query
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
            list.Add((orderedComponents.ToArray(), query));

            return query;
        }
        finally
        {
            _lock14.ExitWriteLock();
        }
    }


    // Cache of all queries with 15 included components. Tuple elements are:
    // - hash of sorted component IDs
    // - sorted array of component IDs
    // - the query itself
    private readonly SortedList<ulong, List<(int[], QueryDescription)>> _queryCache15 = [ ];

    private readonly ReaderWriterLockSlim _lock15 = new();

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
        var hash = xxHash64.ComputeHash(byteSpan, seed: 42);

        _lock15.EnterReadLock();
        try
        {
            // Get the list of items with this hash
            if (_queryCache15.TryGetValue(hash, out var list))
            {
                // Find query that matches these types
                foreach (var (c, q) in list)
                {
                    // Since the sequences are sorted by component ID these should be identical
                    // Comparing two int spans should be very fast (SIMD accelerated)
                    if (!orderedComponents.SequenceEqual(c))
                        continue;

                    return q;
                }
            }
        }
        finally
        {
            _lock15.ExitReadLock();
        }

        // Didn't find query, create it now
        _lock15.EnterWriteLock();
        try
        {
            // Get or create list of items with this hash
            if (!_queryCache15.TryGetValue(hash, out var list))
            {
                list = [ ];
                _queryCache15.Add(hash, list);
            }
            
            // Create query
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
            list.Add((orderedComponents.ToArray(), query));

            return query;
        }
        finally
        {
            _lock15.ExitWriteLock();
        }
    }


    // Cache of all queries with 16 included components. Tuple elements are:
    // - hash of sorted component IDs
    // - sorted array of component IDs
    // - the query itself
    private readonly SortedList<ulong, List<(int[], QueryDescription)>> _queryCache16 = [ ];

    private readonly ReaderWriterLockSlim _lock16 = new();

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
        var hash = xxHash64.ComputeHash(byteSpan, seed: 42);

        _lock16.EnterReadLock();
        try
        {
            // Get the list of items with this hash
            if (_queryCache16.TryGetValue(hash, out var list))
            {
                // Find query that matches these types
                foreach (var (c, q) in list)
                {
                    // Since the sequences are sorted by component ID these should be identical
                    // Comparing two int spans should be very fast (SIMD accelerated)
                    if (!orderedComponents.SequenceEqual(c))
                        continue;

                    return q;
                }
            }
        }
        finally
        {
            _lock16.ExitReadLock();
        }

        // Didn't find query, create it now
        _lock16.EnterWriteLock();
        try
        {
            // Get or create list of items with this hash
            if (!_queryCache16.TryGetValue(hash, out var list))
            {
                list = [ ];
                _queryCache16.Add(hash, list);
            }
            
            // Create query
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
            list.Add((orderedComponents.ToArray(), query));

            return query;
        }
        finally
        {
            _lock16.ExitWriteLock();
        }
    }


}


