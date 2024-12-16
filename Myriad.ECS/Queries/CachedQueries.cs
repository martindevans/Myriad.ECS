using Myriad.ECS.Queries;
using Myriad.ECS.IDs;
using Myriad.ECS.xxHash;
using Myriad.ECS.Collections;
using Myriad.ECS.Components;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;

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
    internal static FrozenOrderedListSet<ComponentID> ExcludePhantom = FrozenOrderedListSet<ComponentID>.Create(
        (ReadOnlySpan<ComponentID>)[ ComponentID<Phantom>.ID ]
    );

    // Cache of all queries with 1 included components.
    private readonly Dictionary<int, QueryDescription> _queryCache1 = [ ];
    private readonly ReaderWriterLockSlim _lock1 = new();

    private QueryDescription GetCachedQuery(ComponentID id0)
    {
        // Try to find query
        _lock1.EnterReadLock();
        try
        {
            // Find query that matches this types
            if (_queryCache1.TryGetValue(id0.Value, out var q))
                return q;
        }
        finally
        {
            _lock1.ExitReadLock();
        }

        // Didn't find one, create it now
        _lock1.EnterWriteLock();
        try
        {
            var query = new QueryDescription(
                this,
                FrozenOrderedListSet<ComponentID>.Create(stackalloc ComponentID[] { id0 }),
                QueryBuilder.SetWithJustPhantom,
                FrozenOrderedListSet<ComponentID>.Empty,
                FrozenOrderedListSet<ComponentID>.Empty
            );

            // Add query to the cache
            _queryCache1[id0.Value] = query;

            return query;
        }
        finally
        {
            _lock1.ExitWriteLock();
        }
    }

    /// <summary>
    /// Get a query that finds entities which include all of the given types. This query
    /// will be shared with other requests for the same set of types.
    /// </summary>
    /// <returns>A query that finds entities which include all of the given types</returns>
    public QueryDescription GetCachedQuery<T0>()
        where T0 : IComponent
    {
        return GetCachedQuery(ComponentID<T0>.ID);
    }

    // Cache of all queries with 2 included components. Key is the 2 component IDs combined together
    private readonly Dictionary<long, QueryDescription> _queryCache2 = [ ];
    private readonly ReaderWriterLockSlim _lock2 = new();

    private QueryDescription GetCachedQuery(ComponentID id0, ComponentID id1)
    {
        var u = new Union64
        {
            I0 = Math.Min(id0.Value, id1.Value),
            I1 = Math.Max(id0.Value, id1.Value),
        };
        var key = u.Long;

        // Find query that matches these types
        _lock2.EnterReadLock();
        try
        {
            if (_queryCache2.TryGetValue(key, out var q))
                return q;
        }
        finally
        {
            _lock2.ExitReadLock();
        }

        // Didn't find one, create it now
        _lock2.EnterWriteLock();
        try
        {
            var query = new QueryDescription(
                this,
                FrozenOrderedListSet<ComponentID>.Create(stackalloc ComponentID[] { id0, id1 }),
                QueryBuilder.SetWithJustPhantom,
                FrozenOrderedListSet<ComponentID>.Empty,
                FrozenOrderedListSet<ComponentID>.Empty
            );

            // Add query to the cache
            _queryCache2[key] = query;

            return query;
        }
        finally
        {
            _lock2.ExitWriteLock();
        }
    }

    /// <summary>
    /// Get a query that finds entities which include all of the given types. This query
    /// will be shared with other requests for the same set of types.
    /// </summary>
    /// <returns>A query that finds entities which include all of the given types</returns>
    public QueryDescription GetCachedQuery<T0, T1>()
        where T0 : IComponent
        where T1 : IComponent
    {
        return GetCachedQuery(ComponentID<T0>.ID, ComponentID<T1>.ID);
    }


    // Cache of all queries with 3 included components. 
    // Key is the hash of the sorted component IDs.
    // The value has a list of tuples, containing:
    // - The actual components for this item (sorted)
    // - The query itself
    private readonly Dictionary<ulong, List<(int[], QueryDescription)>> _queryCache3 = [ ];

    private readonly ReaderWriterLockSlim _lock3 = new();

    
    private QueryDescription GetCachedQuery(ComponentID id0, ComponentID id1, ComponentID id2)
    {
        // Accumulate all components in ascending order
        Span<int> orderedComponents = stackalloc int[] {
            id0.Value,
            id1.Value,
            id2.Value,
        };
        orderedComponents.Sort();

        // Calculate hash of components, for fast rejection
        var byteSpan = MemoryMarshal.Cast<int, byte>(orderedComponents);
        var hash = xxHash64.ComputeHash(byteSpan, seed: 42);

        _lock3.EnterReadLock();
        try
        {
            // Hash collisions are possible but very unlikely. Use the hash to get a list of all queries with this hash
            // and then perform a linear search over the list to find a query with exactly the same set of components.
            // The list of components has been sorted into order (and is stored in order) so we can just do a direct span
            // equality check, which should be very fast (SIMD accelerated).
            if (_queryCache3.TryGetValue(hash, out var list))
                foreach (var (c, q) in list)
                    if (orderedComponents.SequenceEqual(c))
                        return q;
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
            
            var query = new QueryDescription(
                this,
                FrozenOrderedListSet<ComponentID>.Create(stackalloc ComponentID[] {
                    id0,
                    id1,
                    id2,
                }),
                QueryBuilder.SetWithJustPhantom,
                FrozenOrderedListSet<ComponentID>.Empty,
                FrozenOrderedListSet<ComponentID>.Empty
            );

            // Add query to the cache
            list.Add((orderedComponents.ToArray(), query));

            return query;
        }
        finally
        {
            _lock3.ExitWriteLock();
        }
    }

    /// <summary>
    /// Get a query that finds entities which include all of the given types. This query
    /// will be shared with other requests for the same set of types.
    /// </summary>
    /// <returns>A query that finds entities which include all of the given types</returns>
    
    public QueryDescription GetCachedQuery<T0, T1, T2>()
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
    {
        return GetCachedQuery(
            ComponentID<T0>.ID,
            ComponentID<T1>.ID,
            ComponentID<T2>.ID
        );
    }


    // Cache of all queries with 4 included components. 
    // Key is the hash of the sorted component IDs.
    // The value has a list of tuples, containing:
    // - The actual components for this item (sorted)
    // - The query itself
    private readonly Dictionary<ulong, List<(int[], QueryDescription)>> _queryCache4 = [ ];

    private readonly ReaderWriterLockSlim _lock4 = new();

    [ExcludeFromCodeCoverage]
    private QueryDescription GetCachedQuery(ComponentID id0, ComponentID id1, ComponentID id2, ComponentID id3)
    {
        // Accumulate all components in ascending order
        Span<int> orderedComponents = stackalloc int[] {
            id0.Value,
            id1.Value,
            id2.Value,
            id3.Value,
        };
        orderedComponents.Sort();

        // Calculate hash of components, for fast rejection
        var byteSpan = MemoryMarshal.Cast<int, byte>(orderedComponents);
        var hash = xxHash64.ComputeHash(byteSpan, seed: 42);

        _lock4.EnterReadLock();
        try
        {
            // Hash collisions are possible but very unlikely. Use the hash to get a list of all queries with this hash
            // and then perform a linear search over the list to find a query with exactly the same set of components.
            // The list of components has been sorted into order (and is stored in order) so we can just do a direct span
            // equality check, which should be very fast (SIMD accelerated).
            if (_queryCache4.TryGetValue(hash, out var list))
                foreach (var (c, q) in list)
                    if (orderedComponents.SequenceEqual(c))
                        return q;
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
            
            var query = new QueryDescription(
                this,
                FrozenOrderedListSet<ComponentID>.Create(stackalloc ComponentID[] {
                    id0,
                    id1,
                    id2,
                    id3,
                }),
                QueryBuilder.SetWithJustPhantom,
                FrozenOrderedListSet<ComponentID>.Empty,
                FrozenOrderedListSet<ComponentID>.Empty
            );

            // Add query to the cache
            list.Add((orderedComponents.ToArray(), query));

            return query;
        }
        finally
        {
            _lock4.ExitWriteLock();
        }
    }

    /// <summary>
    /// Get a query that finds entities which include all of the given types. This query
    /// will be shared with other requests for the same set of types.
    /// </summary>
    /// <returns>A query that finds entities which include all of the given types</returns>
    [ExcludeFromCodeCoverage]
    public QueryDescription GetCachedQuery<T0, T1, T2, T3>()
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
    {
        return GetCachedQuery(
            ComponentID<T0>.ID,
            ComponentID<T1>.ID,
            ComponentID<T2>.ID,
            ComponentID<T3>.ID
        );
    }


    // Cache of all queries with 5 included components. 
    // Key is the hash of the sorted component IDs.
    // The value has a list of tuples, containing:
    // - The actual components for this item (sorted)
    // - The query itself
    private readonly Dictionary<ulong, List<(int[], QueryDescription)>> _queryCache5 = [ ];

    private readonly ReaderWriterLockSlim _lock5 = new();

    [ExcludeFromCodeCoverage]
    private QueryDescription GetCachedQuery(ComponentID id0, ComponentID id1, ComponentID id2, ComponentID id3, ComponentID id4)
    {
        // Accumulate all components in ascending order
        Span<int> orderedComponents = stackalloc int[] {
            id0.Value,
            id1.Value,
            id2.Value,
            id3.Value,
            id4.Value,
        };
        orderedComponents.Sort();

        // Calculate hash of components, for fast rejection
        var byteSpan = MemoryMarshal.Cast<int, byte>(orderedComponents);
        var hash = xxHash64.ComputeHash(byteSpan, seed: 42);

        _lock5.EnterReadLock();
        try
        {
            // Hash collisions are possible but very unlikely. Use the hash to get a list of all queries with this hash
            // and then perform a linear search over the list to find a query with exactly the same set of components.
            // The list of components has been sorted into order (and is stored in order) so we can just do a direct span
            // equality check, which should be very fast (SIMD accelerated).
            if (_queryCache5.TryGetValue(hash, out var list))
                foreach (var (c, q) in list)
                    if (orderedComponents.SequenceEqual(c))
                        return q;
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
            
            var query = new QueryDescription(
                this,
                FrozenOrderedListSet<ComponentID>.Create(stackalloc ComponentID[] {
                    id0,
                    id1,
                    id2,
                    id3,
                    id4,
                }),
                QueryBuilder.SetWithJustPhantom,
                FrozenOrderedListSet<ComponentID>.Empty,
                FrozenOrderedListSet<ComponentID>.Empty
            );

            // Add query to the cache
            list.Add((orderedComponents.ToArray(), query));

            return query;
        }
        finally
        {
            _lock5.ExitWriteLock();
        }
    }

    /// <summary>
    /// Get a query that finds entities which include all of the given types. This query
    /// will be shared with other requests for the same set of types.
    /// </summary>
    /// <returns>A query that finds entities which include all of the given types</returns>
    [ExcludeFromCodeCoverage]
    public QueryDescription GetCachedQuery<T0, T1, T2, T3, T4>()
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
    {
        return GetCachedQuery(
            ComponentID<T0>.ID,
            ComponentID<T1>.ID,
            ComponentID<T2>.ID,
            ComponentID<T3>.ID,
            ComponentID<T4>.ID
        );
    }


    // Cache of all queries with 6 included components. 
    // Key is the hash of the sorted component IDs.
    // The value has a list of tuples, containing:
    // - The actual components for this item (sorted)
    // - The query itself
    private readonly Dictionary<ulong, List<(int[], QueryDescription)>> _queryCache6 = [ ];

    private readonly ReaderWriterLockSlim _lock6 = new();

    [ExcludeFromCodeCoverage]
    private QueryDescription GetCachedQuery(ComponentID id0, ComponentID id1, ComponentID id2, ComponentID id3, ComponentID id4, ComponentID id5)
    {
        // Accumulate all components in ascending order
        Span<int> orderedComponents = stackalloc int[] {
            id0.Value,
            id1.Value,
            id2.Value,
            id3.Value,
            id4.Value,
            id5.Value,
        };
        orderedComponents.Sort();

        // Calculate hash of components, for fast rejection
        var byteSpan = MemoryMarshal.Cast<int, byte>(orderedComponents);
        var hash = xxHash64.ComputeHash(byteSpan, seed: 42);

        _lock6.EnterReadLock();
        try
        {
            // Hash collisions are possible but very unlikely. Use the hash to get a list of all queries with this hash
            // and then perform a linear search over the list to find a query with exactly the same set of components.
            // The list of components has been sorted into order (and is stored in order) so we can just do a direct span
            // equality check, which should be very fast (SIMD accelerated).
            if (_queryCache6.TryGetValue(hash, out var list))
                foreach (var (c, q) in list)
                    if (orderedComponents.SequenceEqual(c))
                        return q;
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
            
            var query = new QueryDescription(
                this,
                FrozenOrderedListSet<ComponentID>.Create(stackalloc ComponentID[] {
                    id0,
                    id1,
                    id2,
                    id3,
                    id4,
                    id5,
                }),
                QueryBuilder.SetWithJustPhantom,
                FrozenOrderedListSet<ComponentID>.Empty,
                FrozenOrderedListSet<ComponentID>.Empty
            );

            // Add query to the cache
            list.Add((orderedComponents.ToArray(), query));

            return query;
        }
        finally
        {
            _lock6.ExitWriteLock();
        }
    }

    /// <summary>
    /// Get a query that finds entities which include all of the given types. This query
    /// will be shared with other requests for the same set of types.
    /// </summary>
    /// <returns>A query that finds entities which include all of the given types</returns>
    [ExcludeFromCodeCoverage]
    public QueryDescription GetCachedQuery<T0, T1, T2, T3, T4, T5>()
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
    {
        return GetCachedQuery(
            ComponentID<T0>.ID,
            ComponentID<T1>.ID,
            ComponentID<T2>.ID,
            ComponentID<T3>.ID,
            ComponentID<T4>.ID,
            ComponentID<T5>.ID
        );
    }


    // Cache of all queries with 7 included components. 
    // Key is the hash of the sorted component IDs.
    // The value has a list of tuples, containing:
    // - The actual components for this item (sorted)
    // - The query itself
    private readonly Dictionary<ulong, List<(int[], QueryDescription)>> _queryCache7 = [ ];

    private readonly ReaderWriterLockSlim _lock7 = new();

    [ExcludeFromCodeCoverage]
    private QueryDescription GetCachedQuery(ComponentID id0, ComponentID id1, ComponentID id2, ComponentID id3, ComponentID id4, ComponentID id5, ComponentID id6)
    {
        // Accumulate all components in ascending order
        Span<int> orderedComponents = stackalloc int[] {
            id0.Value,
            id1.Value,
            id2.Value,
            id3.Value,
            id4.Value,
            id5.Value,
            id6.Value,
        };
        orderedComponents.Sort();

        // Calculate hash of components, for fast rejection
        var byteSpan = MemoryMarshal.Cast<int, byte>(orderedComponents);
        var hash = xxHash64.ComputeHash(byteSpan, seed: 42);

        _lock7.EnterReadLock();
        try
        {
            // Hash collisions are possible but very unlikely. Use the hash to get a list of all queries with this hash
            // and then perform a linear search over the list to find a query with exactly the same set of components.
            // The list of components has been sorted into order (and is stored in order) so we can just do a direct span
            // equality check, which should be very fast (SIMD accelerated).
            if (_queryCache7.TryGetValue(hash, out var list))
                foreach (var (c, q) in list)
                    if (orderedComponents.SequenceEqual(c))
                        return q;
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
            
            var query = new QueryDescription(
                this,
                FrozenOrderedListSet<ComponentID>.Create(stackalloc ComponentID[] {
                    id0,
                    id1,
                    id2,
                    id3,
                    id4,
                    id5,
                    id6,
                }),
                QueryBuilder.SetWithJustPhantom,
                FrozenOrderedListSet<ComponentID>.Empty,
                FrozenOrderedListSet<ComponentID>.Empty
            );

            // Add query to the cache
            list.Add((orderedComponents.ToArray(), query));

            return query;
        }
        finally
        {
            _lock7.ExitWriteLock();
        }
    }

    /// <summary>
    /// Get a query that finds entities which include all of the given types. This query
    /// will be shared with other requests for the same set of types.
    /// </summary>
    /// <returns>A query that finds entities which include all of the given types</returns>
    [ExcludeFromCodeCoverage]
    public QueryDescription GetCachedQuery<T0, T1, T2, T3, T4, T5, T6>()
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
    {
        return GetCachedQuery(
            ComponentID<T0>.ID,
            ComponentID<T1>.ID,
            ComponentID<T2>.ID,
            ComponentID<T3>.ID,
            ComponentID<T4>.ID,
            ComponentID<T5>.ID,
            ComponentID<T6>.ID
        );
    }


    // Cache of all queries with 8 included components. 
    // Key is the hash of the sorted component IDs.
    // The value has a list of tuples, containing:
    // - The actual components for this item (sorted)
    // - The query itself
    private readonly Dictionary<ulong, List<(int[], QueryDescription)>> _queryCache8 = [ ];

    private readonly ReaderWriterLockSlim _lock8 = new();

    [ExcludeFromCodeCoverage]
    private QueryDescription GetCachedQuery(ComponentID id0, ComponentID id1, ComponentID id2, ComponentID id3, ComponentID id4, ComponentID id5, ComponentID id6, ComponentID id7)
    {
        // Accumulate all components in ascending order
        Span<int> orderedComponents = stackalloc int[] {
            id0.Value,
            id1.Value,
            id2.Value,
            id3.Value,
            id4.Value,
            id5.Value,
            id6.Value,
            id7.Value,
        };
        orderedComponents.Sort();

        // Calculate hash of components, for fast rejection
        var byteSpan = MemoryMarshal.Cast<int, byte>(orderedComponents);
        var hash = xxHash64.ComputeHash(byteSpan, seed: 42);

        _lock8.EnterReadLock();
        try
        {
            // Hash collisions are possible but very unlikely. Use the hash to get a list of all queries with this hash
            // and then perform a linear search over the list to find a query with exactly the same set of components.
            // The list of components has been sorted into order (and is stored in order) so we can just do a direct span
            // equality check, which should be very fast (SIMD accelerated).
            if (_queryCache8.TryGetValue(hash, out var list))
                foreach (var (c, q) in list)
                    if (orderedComponents.SequenceEqual(c))
                        return q;
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
            
            var query = new QueryDescription(
                this,
                FrozenOrderedListSet<ComponentID>.Create(stackalloc ComponentID[] {
                    id0,
                    id1,
                    id2,
                    id3,
                    id4,
                    id5,
                    id6,
                    id7,
                }),
                QueryBuilder.SetWithJustPhantom,
                FrozenOrderedListSet<ComponentID>.Empty,
                FrozenOrderedListSet<ComponentID>.Empty
            );

            // Add query to the cache
            list.Add((orderedComponents.ToArray(), query));

            return query;
        }
        finally
        {
            _lock8.ExitWriteLock();
        }
    }

    /// <summary>
    /// Get a query that finds entities which include all of the given types. This query
    /// will be shared with other requests for the same set of types.
    /// </summary>
    /// <returns>A query that finds entities which include all of the given types</returns>
    [ExcludeFromCodeCoverage]
    public QueryDescription GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7>()
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
    {
        return GetCachedQuery(
            ComponentID<T0>.ID,
            ComponentID<T1>.ID,
            ComponentID<T2>.ID,
            ComponentID<T3>.ID,
            ComponentID<T4>.ID,
            ComponentID<T5>.ID,
            ComponentID<T6>.ID,
            ComponentID<T7>.ID
        );
    }


    // Cache of all queries with 9 included components. 
    // Key is the hash of the sorted component IDs.
    // The value has a list of tuples, containing:
    // - The actual components for this item (sorted)
    // - The query itself
    private readonly Dictionary<ulong, List<(int[], QueryDescription)>> _queryCache9 = [ ];

    private readonly ReaderWriterLockSlim _lock9 = new();

    [ExcludeFromCodeCoverage]
    private QueryDescription GetCachedQuery(ComponentID id0, ComponentID id1, ComponentID id2, ComponentID id3, ComponentID id4, ComponentID id5, ComponentID id6, ComponentID id7, ComponentID id8)
    {
        // Accumulate all components in ascending order
        Span<int> orderedComponents = stackalloc int[] {
            id0.Value,
            id1.Value,
            id2.Value,
            id3.Value,
            id4.Value,
            id5.Value,
            id6.Value,
            id7.Value,
            id8.Value,
        };
        orderedComponents.Sort();

        // Calculate hash of components, for fast rejection
        var byteSpan = MemoryMarshal.Cast<int, byte>(orderedComponents);
        var hash = xxHash64.ComputeHash(byteSpan, seed: 42);

        _lock9.EnterReadLock();
        try
        {
            // Hash collisions are possible but very unlikely. Use the hash to get a list of all queries with this hash
            // and then perform a linear search over the list to find a query with exactly the same set of components.
            // The list of components has been sorted into order (and is stored in order) so we can just do a direct span
            // equality check, which should be very fast (SIMD accelerated).
            if (_queryCache9.TryGetValue(hash, out var list))
                foreach (var (c, q) in list)
                    if (orderedComponents.SequenceEqual(c))
                        return q;
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
            
            var query = new QueryDescription(
                this,
                FrozenOrderedListSet<ComponentID>.Create(stackalloc ComponentID[] {
                    id0,
                    id1,
                    id2,
                    id3,
                    id4,
                    id5,
                    id6,
                    id7,
                    id8,
                }),
                QueryBuilder.SetWithJustPhantom,
                FrozenOrderedListSet<ComponentID>.Empty,
                FrozenOrderedListSet<ComponentID>.Empty
            );

            // Add query to the cache
            list.Add((orderedComponents.ToArray(), query));

            return query;
        }
        finally
        {
            _lock9.ExitWriteLock();
        }
    }

    /// <summary>
    /// Get a query that finds entities which include all of the given types. This query
    /// will be shared with other requests for the same set of types.
    /// </summary>
    /// <returns>A query that finds entities which include all of the given types</returns>
    [ExcludeFromCodeCoverage]
    public QueryDescription GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8>()
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
        return GetCachedQuery(
            ComponentID<T0>.ID,
            ComponentID<T1>.ID,
            ComponentID<T2>.ID,
            ComponentID<T3>.ID,
            ComponentID<T4>.ID,
            ComponentID<T5>.ID,
            ComponentID<T6>.ID,
            ComponentID<T7>.ID,
            ComponentID<T8>.ID
        );
    }


    // Cache of all queries with 10 included components. 
    // Key is the hash of the sorted component IDs.
    // The value has a list of tuples, containing:
    // - The actual components for this item (sorted)
    // - The query itself
    private readonly Dictionary<ulong, List<(int[], QueryDescription)>> _queryCache10 = [ ];

    private readonly ReaderWriterLockSlim _lock10 = new();

    [ExcludeFromCodeCoverage]
    private QueryDescription GetCachedQuery(ComponentID id0, ComponentID id1, ComponentID id2, ComponentID id3, ComponentID id4, ComponentID id5, ComponentID id6, ComponentID id7, ComponentID id8, ComponentID id9)
    {
        // Accumulate all components in ascending order
        Span<int> orderedComponents = stackalloc int[] {
            id0.Value,
            id1.Value,
            id2.Value,
            id3.Value,
            id4.Value,
            id5.Value,
            id6.Value,
            id7.Value,
            id8.Value,
            id9.Value,
        };
        orderedComponents.Sort();

        // Calculate hash of components, for fast rejection
        var byteSpan = MemoryMarshal.Cast<int, byte>(orderedComponents);
        var hash = xxHash64.ComputeHash(byteSpan, seed: 42);

        _lock10.EnterReadLock();
        try
        {
            // Hash collisions are possible but very unlikely. Use the hash to get a list of all queries with this hash
            // and then perform a linear search over the list to find a query with exactly the same set of components.
            // The list of components has been sorted into order (and is stored in order) so we can just do a direct span
            // equality check, which should be very fast (SIMD accelerated).
            if (_queryCache10.TryGetValue(hash, out var list))
                foreach (var (c, q) in list)
                    if (orderedComponents.SequenceEqual(c))
                        return q;
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
            
            var query = new QueryDescription(
                this,
                FrozenOrderedListSet<ComponentID>.Create(stackalloc ComponentID[] {
                    id0,
                    id1,
                    id2,
                    id3,
                    id4,
                    id5,
                    id6,
                    id7,
                    id8,
                    id9,
                }),
                QueryBuilder.SetWithJustPhantom,
                FrozenOrderedListSet<ComponentID>.Empty,
                FrozenOrderedListSet<ComponentID>.Empty
            );

            // Add query to the cache
            list.Add((orderedComponents.ToArray(), query));

            return query;
        }
        finally
        {
            _lock10.ExitWriteLock();
        }
    }

    /// <summary>
    /// Get a query that finds entities which include all of the given types. This query
    /// will be shared with other requests for the same set of types.
    /// </summary>
    /// <returns>A query that finds entities which include all of the given types</returns>
    [ExcludeFromCodeCoverage]
    public QueryDescription GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>()
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
        return GetCachedQuery(
            ComponentID<T0>.ID,
            ComponentID<T1>.ID,
            ComponentID<T2>.ID,
            ComponentID<T3>.ID,
            ComponentID<T4>.ID,
            ComponentID<T5>.ID,
            ComponentID<T6>.ID,
            ComponentID<T7>.ID,
            ComponentID<T8>.ID,
            ComponentID<T9>.ID
        );
    }


    // Cache of all queries with 11 included components. 
    // Key is the hash of the sorted component IDs.
    // The value has a list of tuples, containing:
    // - The actual components for this item (sorted)
    // - The query itself
    private readonly Dictionary<ulong, List<(int[], QueryDescription)>> _queryCache11 = [ ];

    private readonly ReaderWriterLockSlim _lock11 = new();

    [ExcludeFromCodeCoverage]
    private QueryDescription GetCachedQuery(ComponentID id0, ComponentID id1, ComponentID id2, ComponentID id3, ComponentID id4, ComponentID id5, ComponentID id6, ComponentID id7, ComponentID id8, ComponentID id9, ComponentID id10)
    {
        // Accumulate all components in ascending order
        Span<int> orderedComponents = stackalloc int[] {
            id0.Value,
            id1.Value,
            id2.Value,
            id3.Value,
            id4.Value,
            id5.Value,
            id6.Value,
            id7.Value,
            id8.Value,
            id9.Value,
            id10.Value,
        };
        orderedComponents.Sort();

        // Calculate hash of components, for fast rejection
        var byteSpan = MemoryMarshal.Cast<int, byte>(orderedComponents);
        var hash = xxHash64.ComputeHash(byteSpan, seed: 42);

        _lock11.EnterReadLock();
        try
        {
            // Hash collisions are possible but very unlikely. Use the hash to get a list of all queries with this hash
            // and then perform a linear search over the list to find a query with exactly the same set of components.
            // The list of components has been sorted into order (and is stored in order) so we can just do a direct span
            // equality check, which should be very fast (SIMD accelerated).
            if (_queryCache11.TryGetValue(hash, out var list))
                foreach (var (c, q) in list)
                    if (orderedComponents.SequenceEqual(c))
                        return q;
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
            
            var query = new QueryDescription(
                this,
                FrozenOrderedListSet<ComponentID>.Create(stackalloc ComponentID[] {
                    id0,
                    id1,
                    id2,
                    id3,
                    id4,
                    id5,
                    id6,
                    id7,
                    id8,
                    id9,
                    id10,
                }),
                QueryBuilder.SetWithJustPhantom,
                FrozenOrderedListSet<ComponentID>.Empty,
                FrozenOrderedListSet<ComponentID>.Empty
            );

            // Add query to the cache
            list.Add((orderedComponents.ToArray(), query));

            return query;
        }
        finally
        {
            _lock11.ExitWriteLock();
        }
    }

    /// <summary>
    /// Get a query that finds entities which include all of the given types. This query
    /// will be shared with other requests for the same set of types.
    /// </summary>
    /// <returns>A query that finds entities which include all of the given types</returns>
    [ExcludeFromCodeCoverage]
    public QueryDescription GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>()
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
        return GetCachedQuery(
            ComponentID<T0>.ID,
            ComponentID<T1>.ID,
            ComponentID<T2>.ID,
            ComponentID<T3>.ID,
            ComponentID<T4>.ID,
            ComponentID<T5>.ID,
            ComponentID<T6>.ID,
            ComponentID<T7>.ID,
            ComponentID<T8>.ID,
            ComponentID<T9>.ID,
            ComponentID<T10>.ID
        );
    }


    // Cache of all queries with 12 included components. 
    // Key is the hash of the sorted component IDs.
    // The value has a list of tuples, containing:
    // - The actual components for this item (sorted)
    // - The query itself
    private readonly Dictionary<ulong, List<(int[], QueryDescription)>> _queryCache12 = [ ];

    private readonly ReaderWriterLockSlim _lock12 = new();

    [ExcludeFromCodeCoverage]
    private QueryDescription GetCachedQuery(ComponentID id0, ComponentID id1, ComponentID id2, ComponentID id3, ComponentID id4, ComponentID id5, ComponentID id6, ComponentID id7, ComponentID id8, ComponentID id9, ComponentID id10, ComponentID id11)
    {
        // Accumulate all components in ascending order
        Span<int> orderedComponents = stackalloc int[] {
            id0.Value,
            id1.Value,
            id2.Value,
            id3.Value,
            id4.Value,
            id5.Value,
            id6.Value,
            id7.Value,
            id8.Value,
            id9.Value,
            id10.Value,
            id11.Value,
        };
        orderedComponents.Sort();

        // Calculate hash of components, for fast rejection
        var byteSpan = MemoryMarshal.Cast<int, byte>(orderedComponents);
        var hash = xxHash64.ComputeHash(byteSpan, seed: 42);

        _lock12.EnterReadLock();
        try
        {
            // Hash collisions are possible but very unlikely. Use the hash to get a list of all queries with this hash
            // and then perform a linear search over the list to find a query with exactly the same set of components.
            // The list of components has been sorted into order (and is stored in order) so we can just do a direct span
            // equality check, which should be very fast (SIMD accelerated).
            if (_queryCache12.TryGetValue(hash, out var list))
                foreach (var (c, q) in list)
                    if (orderedComponents.SequenceEqual(c))
                        return q;
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
            
            var query = new QueryDescription(
                this,
                FrozenOrderedListSet<ComponentID>.Create(stackalloc ComponentID[] {
                    id0,
                    id1,
                    id2,
                    id3,
                    id4,
                    id5,
                    id6,
                    id7,
                    id8,
                    id9,
                    id10,
                    id11,
                }),
                QueryBuilder.SetWithJustPhantom,
                FrozenOrderedListSet<ComponentID>.Empty,
                FrozenOrderedListSet<ComponentID>.Empty
            );

            // Add query to the cache
            list.Add((orderedComponents.ToArray(), query));

            return query;
        }
        finally
        {
            _lock12.ExitWriteLock();
        }
    }

    /// <summary>
    /// Get a query that finds entities which include all of the given types. This query
    /// will be shared with other requests for the same set of types.
    /// </summary>
    /// <returns>A query that finds entities which include all of the given types</returns>
    [ExcludeFromCodeCoverage]
    public QueryDescription GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>()
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
        return GetCachedQuery(
            ComponentID<T0>.ID,
            ComponentID<T1>.ID,
            ComponentID<T2>.ID,
            ComponentID<T3>.ID,
            ComponentID<T4>.ID,
            ComponentID<T5>.ID,
            ComponentID<T6>.ID,
            ComponentID<T7>.ID,
            ComponentID<T8>.ID,
            ComponentID<T9>.ID,
            ComponentID<T10>.ID,
            ComponentID<T11>.ID
        );
    }


    // Cache of all queries with 13 included components. 
    // Key is the hash of the sorted component IDs.
    // The value has a list of tuples, containing:
    // - The actual components for this item (sorted)
    // - The query itself
    private readonly Dictionary<ulong, List<(int[], QueryDescription)>> _queryCache13 = [ ];

    private readonly ReaderWriterLockSlim _lock13 = new();

    [ExcludeFromCodeCoverage]
    private QueryDescription GetCachedQuery(ComponentID id0, ComponentID id1, ComponentID id2, ComponentID id3, ComponentID id4, ComponentID id5, ComponentID id6, ComponentID id7, ComponentID id8, ComponentID id9, ComponentID id10, ComponentID id11, ComponentID id12)
    {
        // Accumulate all components in ascending order
        Span<int> orderedComponents = stackalloc int[] {
            id0.Value,
            id1.Value,
            id2.Value,
            id3.Value,
            id4.Value,
            id5.Value,
            id6.Value,
            id7.Value,
            id8.Value,
            id9.Value,
            id10.Value,
            id11.Value,
            id12.Value,
        };
        orderedComponents.Sort();

        // Calculate hash of components, for fast rejection
        var byteSpan = MemoryMarshal.Cast<int, byte>(orderedComponents);
        var hash = xxHash64.ComputeHash(byteSpan, seed: 42);

        _lock13.EnterReadLock();
        try
        {
            // Hash collisions are possible but very unlikely. Use the hash to get a list of all queries with this hash
            // and then perform a linear search over the list to find a query with exactly the same set of components.
            // The list of components has been sorted into order (and is stored in order) so we can just do a direct span
            // equality check, which should be very fast (SIMD accelerated).
            if (_queryCache13.TryGetValue(hash, out var list))
                foreach (var (c, q) in list)
                    if (orderedComponents.SequenceEqual(c))
                        return q;
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
            
            var query = new QueryDescription(
                this,
                FrozenOrderedListSet<ComponentID>.Create(stackalloc ComponentID[] {
                    id0,
                    id1,
                    id2,
                    id3,
                    id4,
                    id5,
                    id6,
                    id7,
                    id8,
                    id9,
                    id10,
                    id11,
                    id12,
                }),
                QueryBuilder.SetWithJustPhantom,
                FrozenOrderedListSet<ComponentID>.Empty,
                FrozenOrderedListSet<ComponentID>.Empty
            );

            // Add query to the cache
            list.Add((orderedComponents.ToArray(), query));

            return query;
        }
        finally
        {
            _lock13.ExitWriteLock();
        }
    }

    /// <summary>
    /// Get a query that finds entities which include all of the given types. This query
    /// will be shared with other requests for the same set of types.
    /// </summary>
    /// <returns>A query that finds entities which include all of the given types</returns>
    [ExcludeFromCodeCoverage]
    public QueryDescription GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>()
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
        return GetCachedQuery(
            ComponentID<T0>.ID,
            ComponentID<T1>.ID,
            ComponentID<T2>.ID,
            ComponentID<T3>.ID,
            ComponentID<T4>.ID,
            ComponentID<T5>.ID,
            ComponentID<T6>.ID,
            ComponentID<T7>.ID,
            ComponentID<T8>.ID,
            ComponentID<T9>.ID,
            ComponentID<T10>.ID,
            ComponentID<T11>.ID,
            ComponentID<T12>.ID
        );
    }


    // Cache of all queries with 14 included components. 
    // Key is the hash of the sorted component IDs.
    // The value has a list of tuples, containing:
    // - The actual components for this item (sorted)
    // - The query itself
    private readonly Dictionary<ulong, List<(int[], QueryDescription)>> _queryCache14 = [ ];

    private readonly ReaderWriterLockSlim _lock14 = new();

    [ExcludeFromCodeCoverage]
    private QueryDescription GetCachedQuery(ComponentID id0, ComponentID id1, ComponentID id2, ComponentID id3, ComponentID id4, ComponentID id5, ComponentID id6, ComponentID id7, ComponentID id8, ComponentID id9, ComponentID id10, ComponentID id11, ComponentID id12, ComponentID id13)
    {
        // Accumulate all components in ascending order
        Span<int> orderedComponents = stackalloc int[] {
            id0.Value,
            id1.Value,
            id2.Value,
            id3.Value,
            id4.Value,
            id5.Value,
            id6.Value,
            id7.Value,
            id8.Value,
            id9.Value,
            id10.Value,
            id11.Value,
            id12.Value,
            id13.Value,
        };
        orderedComponents.Sort();

        // Calculate hash of components, for fast rejection
        var byteSpan = MemoryMarshal.Cast<int, byte>(orderedComponents);
        var hash = xxHash64.ComputeHash(byteSpan, seed: 42);

        _lock14.EnterReadLock();
        try
        {
            // Hash collisions are possible but very unlikely. Use the hash to get a list of all queries with this hash
            // and then perform a linear search over the list to find a query with exactly the same set of components.
            // The list of components has been sorted into order (and is stored in order) so we can just do a direct span
            // equality check, which should be very fast (SIMD accelerated).
            if (_queryCache14.TryGetValue(hash, out var list))
                foreach (var (c, q) in list)
                    if (orderedComponents.SequenceEqual(c))
                        return q;
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
            
            var query = new QueryDescription(
                this,
                FrozenOrderedListSet<ComponentID>.Create(stackalloc ComponentID[] {
                    id0,
                    id1,
                    id2,
                    id3,
                    id4,
                    id5,
                    id6,
                    id7,
                    id8,
                    id9,
                    id10,
                    id11,
                    id12,
                    id13,
                }),
                QueryBuilder.SetWithJustPhantom,
                FrozenOrderedListSet<ComponentID>.Empty,
                FrozenOrderedListSet<ComponentID>.Empty
            );

            // Add query to the cache
            list.Add((orderedComponents.ToArray(), query));

            return query;
        }
        finally
        {
            _lock14.ExitWriteLock();
        }
    }

    /// <summary>
    /// Get a query that finds entities which include all of the given types. This query
    /// will be shared with other requests for the same set of types.
    /// </summary>
    /// <returns>A query that finds entities which include all of the given types</returns>
    [ExcludeFromCodeCoverage]
    public QueryDescription GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>()
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
        return GetCachedQuery(
            ComponentID<T0>.ID,
            ComponentID<T1>.ID,
            ComponentID<T2>.ID,
            ComponentID<T3>.ID,
            ComponentID<T4>.ID,
            ComponentID<T5>.ID,
            ComponentID<T6>.ID,
            ComponentID<T7>.ID,
            ComponentID<T8>.ID,
            ComponentID<T9>.ID,
            ComponentID<T10>.ID,
            ComponentID<T11>.ID,
            ComponentID<T12>.ID,
            ComponentID<T13>.ID
        );
    }


    // Cache of all queries with 15 included components. 
    // Key is the hash of the sorted component IDs.
    // The value has a list of tuples, containing:
    // - The actual components for this item (sorted)
    // - The query itself
    private readonly Dictionary<ulong, List<(int[], QueryDescription)>> _queryCache15 = [ ];

    private readonly ReaderWriterLockSlim _lock15 = new();

    [ExcludeFromCodeCoverage]
    private QueryDescription GetCachedQuery(ComponentID id0, ComponentID id1, ComponentID id2, ComponentID id3, ComponentID id4, ComponentID id5, ComponentID id6, ComponentID id7, ComponentID id8, ComponentID id9, ComponentID id10, ComponentID id11, ComponentID id12, ComponentID id13, ComponentID id14)
    {
        // Accumulate all components in ascending order
        Span<int> orderedComponents = stackalloc int[] {
            id0.Value,
            id1.Value,
            id2.Value,
            id3.Value,
            id4.Value,
            id5.Value,
            id6.Value,
            id7.Value,
            id8.Value,
            id9.Value,
            id10.Value,
            id11.Value,
            id12.Value,
            id13.Value,
            id14.Value,
        };
        orderedComponents.Sort();

        // Calculate hash of components, for fast rejection
        var byteSpan = MemoryMarshal.Cast<int, byte>(orderedComponents);
        var hash = xxHash64.ComputeHash(byteSpan, seed: 42);

        _lock15.EnterReadLock();
        try
        {
            // Hash collisions are possible but very unlikely. Use the hash to get a list of all queries with this hash
            // and then perform a linear search over the list to find a query with exactly the same set of components.
            // The list of components has been sorted into order (and is stored in order) so we can just do a direct span
            // equality check, which should be very fast (SIMD accelerated).
            if (_queryCache15.TryGetValue(hash, out var list))
                foreach (var (c, q) in list)
                    if (orderedComponents.SequenceEqual(c))
                        return q;
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
            
            var query = new QueryDescription(
                this,
                FrozenOrderedListSet<ComponentID>.Create(stackalloc ComponentID[] {
                    id0,
                    id1,
                    id2,
                    id3,
                    id4,
                    id5,
                    id6,
                    id7,
                    id8,
                    id9,
                    id10,
                    id11,
                    id12,
                    id13,
                    id14,
                }),
                QueryBuilder.SetWithJustPhantom,
                FrozenOrderedListSet<ComponentID>.Empty,
                FrozenOrderedListSet<ComponentID>.Empty
            );

            // Add query to the cache
            list.Add((orderedComponents.ToArray(), query));

            return query;
        }
        finally
        {
            _lock15.ExitWriteLock();
        }
    }

    /// <summary>
    /// Get a query that finds entities which include all of the given types. This query
    /// will be shared with other requests for the same set of types.
    /// </summary>
    /// <returns>A query that finds entities which include all of the given types</returns>
    [ExcludeFromCodeCoverage]
    public QueryDescription GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>()
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
        return GetCachedQuery(
            ComponentID<T0>.ID,
            ComponentID<T1>.ID,
            ComponentID<T2>.ID,
            ComponentID<T3>.ID,
            ComponentID<T4>.ID,
            ComponentID<T5>.ID,
            ComponentID<T6>.ID,
            ComponentID<T7>.ID,
            ComponentID<T8>.ID,
            ComponentID<T9>.ID,
            ComponentID<T10>.ID,
            ComponentID<T11>.ID,
            ComponentID<T12>.ID,
            ComponentID<T13>.ID,
            ComponentID<T14>.ID
        );
    }


    // Cache of all queries with 16 included components. 
    // Key is the hash of the sorted component IDs.
    // The value has a list of tuples, containing:
    // - The actual components for this item (sorted)
    // - The query itself
    private readonly Dictionary<ulong, List<(int[], QueryDescription)>> _queryCache16 = [ ];

    private readonly ReaderWriterLockSlim _lock16 = new();

    [ExcludeFromCodeCoverage]
    private QueryDescription GetCachedQuery(ComponentID id0, ComponentID id1, ComponentID id2, ComponentID id3, ComponentID id4, ComponentID id5, ComponentID id6, ComponentID id7, ComponentID id8, ComponentID id9, ComponentID id10, ComponentID id11, ComponentID id12, ComponentID id13, ComponentID id14, ComponentID id15)
    {
        // Accumulate all components in ascending order
        Span<int> orderedComponents = stackalloc int[] {
            id0.Value,
            id1.Value,
            id2.Value,
            id3.Value,
            id4.Value,
            id5.Value,
            id6.Value,
            id7.Value,
            id8.Value,
            id9.Value,
            id10.Value,
            id11.Value,
            id12.Value,
            id13.Value,
            id14.Value,
            id15.Value,
        };
        orderedComponents.Sort();

        // Calculate hash of components, for fast rejection
        var byteSpan = MemoryMarshal.Cast<int, byte>(orderedComponents);
        var hash = xxHash64.ComputeHash(byteSpan, seed: 42);

        _lock16.EnterReadLock();
        try
        {
            // Hash collisions are possible but very unlikely. Use the hash to get a list of all queries with this hash
            // and then perform a linear search over the list to find a query with exactly the same set of components.
            // The list of components has been sorted into order (and is stored in order) so we can just do a direct span
            // equality check, which should be very fast (SIMD accelerated).
            if (_queryCache16.TryGetValue(hash, out var list))
                foreach (var (c, q) in list)
                    if (orderedComponents.SequenceEqual(c))
                        return q;
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
            
            var query = new QueryDescription(
                this,
                FrozenOrderedListSet<ComponentID>.Create(stackalloc ComponentID[] {
                    id0,
                    id1,
                    id2,
                    id3,
                    id4,
                    id5,
                    id6,
                    id7,
                    id8,
                    id9,
                    id10,
                    id11,
                    id12,
                    id13,
                    id14,
                    id15,
                }),
                QueryBuilder.SetWithJustPhantom,
                FrozenOrderedListSet<ComponentID>.Empty,
                FrozenOrderedListSet<ComponentID>.Empty
            );

            // Add query to the cache
            list.Add((orderedComponents.ToArray(), query));

            return query;
        }
        finally
        {
            _lock16.ExitWriteLock();
        }
    }

    /// <summary>
    /// Get a query that finds entities which include all of the given types. This query
    /// will be shared with other requests for the same set of types.
    /// </summary>
    /// <returns>A query that finds entities which include all of the given types</returns>
    [ExcludeFromCodeCoverage]
    public QueryDescription GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>()
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
        return GetCachedQuery(
            ComponentID<T0>.ID,
            ComponentID<T1>.ID,
            ComponentID<T2>.ID,
            ComponentID<T3>.ID,
            ComponentID<T4>.ID,
            ComponentID<T5>.ID,
            ComponentID<T6>.ID,
            ComponentID<T7>.ID,
            ComponentID<T8>.ID,
            ComponentID<T9>.ID,
            ComponentID<T10>.ID,
            ComponentID<T11>.ID,
            ComponentID<T12>.ID,
            ComponentID<T13>.ID,
            ComponentID<T14>.ID,
            ComponentID<T15>.ID
        );
    }


}


