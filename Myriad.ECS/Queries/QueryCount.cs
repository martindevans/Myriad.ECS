using Myriad.ECS.IDs;
using Myriad.ECS.Queries;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedParameter.Global
// ReSharper disable LoopCanBeConvertedToQuery
// ReSharper disable CheckNamespace
// ReSharper disable ArrangeAccessorOwnerBody

namespace Myriad.ECS.Worlds;

public partial class World
{
    /// <summary>
    /// Count how many entities match this query
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public int Count(QueryDescription query)
    {
        return query.Count();
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    public int Count<T0>()
        where T0 : IComponent
    {
        var query = GetCachedQuery<T0, T1>();
        return query.Count();
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    public int Count<T0>(ref QueryDescription? query)
        where T0 : IComponent
    {
        QueryBuilder? builder = null;
        if (query != null)
        {
            var c0 = ComponentID<T0>.ID;
            if (!query.IsIncluded(c0))
            {
                builder ??= query.ToBuilder();
                builder.Include(c0);
            }

            if (builder != null)
                query = builder.Build(query.World);
        }
        else
        {
            query = GetCachedQuery<T0>();
        }

        return query.Count();
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    public int Count<T0, T1>()
        where T0 : IComponent
        where T1 : IComponent
    {
        var query = GetCachedQuery<T0, T1>();
        return query.Count();
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <param name="query">If this is null it will be initialised with a query based on the type parameters. If non-null it will
    /// be modified to ensure it **includes** all type parameters (and the new query will be placed into this ref).</param>
    /// <returns></returns>
    public int Count<T0, T1>(ref QueryDescription? query)
        where T0 : IComponent
        where T1 : IComponent
    {
        QueryBuilder? builder = null;
        if (query != null)
        {
            var c0 = ComponentID<T0>.ID;
            if (!query.IsIncluded(c0))
            {
                builder ??= query.ToBuilder();
                builder.Include(c0);
            }

            var c1 = ComponentID<T1>.ID;
            if (!query.IsIncluded(c1))
            {
                builder ??= query.ToBuilder();
                builder.Include(c1);
            }

            if (builder != null)
                query = builder.Build(query.World);
        }
        else
        {
            query = GetCachedQuery<T0, T1>();
        }

        return query.Count();
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    public int Count<T0, T1, T2>()
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
    {
        var query = GetCachedQuery<T0, T1>();
        return query.Count();
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    public int Count<T0, T1, T2>(ref QueryDescription? query)
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
    {
        QueryBuilder? builder = null;
        if (query != null)
        {
            var c0 = ComponentID<T0>.ID;
            if (!query.IsIncluded(c0))
            {
                builder ??= query.ToBuilder();
                builder.Include(c0);
            }

            var c1 = ComponentID<T1>.ID;
            if (!query.IsIncluded(c1))
            {
                builder ??= query.ToBuilder();
                builder.Include(c1);
            }

            var c2 = ComponentID<T2>.ID;
            if (!query.IsIncluded(c2))
            {
                builder ??= query.ToBuilder();
                builder.Include(c2);
            }

            if (builder != null)
                query = builder.Build(query.World);
        }
        else
        {
            query = GetCachedQuery<T0, T1, T2>();
        }

        return query.Count();
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    public int Count<T0, T1, T2, T3>()
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
    {
        var query = GetCachedQuery<T0, T1>();
        return query.Count();
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    public int Count<T0, T1, T2, T3>(ref QueryDescription? query)
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
    {
        QueryBuilder? builder = null;
        if (query != null)
        {
            var c0 = ComponentID<T0>.ID;
            if (!query.IsIncluded(c0))
            {
                builder ??= query.ToBuilder();
                builder.Include(c0);
            }

            var c1 = ComponentID<T1>.ID;
            if (!query.IsIncluded(c1))
            {
                builder ??= query.ToBuilder();
                builder.Include(c1);
            }

            var c2 = ComponentID<T2>.ID;
            if (!query.IsIncluded(c2))
            {
                builder ??= query.ToBuilder();
                builder.Include(c2);
            }

            var c3 = ComponentID<T3>.ID;
            if (!query.IsIncluded(c3))
            {
                builder ??= query.ToBuilder();
                builder.Include(c3);
            }

            if (builder != null)
                query = builder.Build(query.World);
        }
        else
        {
            query = GetCachedQuery<T0, T1, T2, T3>();
        }

        return query.Count();
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    public int Count<T0, T1, T2, T3, T4>()
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
    {
        var query = GetCachedQuery<T0, T1>();
        return query.Count();
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    public int Count<T0, T1, T2, T3, T4>(ref QueryDescription? query)
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
    {
        QueryBuilder? builder = null;
        if (query != null)
        {
            var c0 = ComponentID<T0>.ID;
            if (!query.IsIncluded(c0))
            {
                builder ??= query.ToBuilder();
                builder.Include(c0);
            }

            var c1 = ComponentID<T1>.ID;
            if (!query.IsIncluded(c1))
            {
                builder ??= query.ToBuilder();
                builder.Include(c1);
            }

            var c2 = ComponentID<T2>.ID;
            if (!query.IsIncluded(c2))
            {
                builder ??= query.ToBuilder();
                builder.Include(c2);
            }

            var c3 = ComponentID<T3>.ID;
            if (!query.IsIncluded(c3))
            {
                builder ??= query.ToBuilder();
                builder.Include(c3);
            }

            var c4 = ComponentID<T4>.ID;
            if (!query.IsIncluded(c4))
            {
                builder ??= query.ToBuilder();
                builder.Include(c4);
            }

            if (builder != null)
                query = builder.Build(query.World);
        }
        else
        {
            query = GetCachedQuery<T0, T1, T2, T3, T4>();
        }

        return query.Count();
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    public int Count<T0, T1, T2, T3, T4, T5>()
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
    {
        var query = GetCachedQuery<T0, T1>();
        return query.Count();
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    public int Count<T0, T1, T2, T3, T4, T5>(ref QueryDescription? query)
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
    {
        QueryBuilder? builder = null;
        if (query != null)
        {
            var c0 = ComponentID<T0>.ID;
            if (!query.IsIncluded(c0))
            {
                builder ??= query.ToBuilder();
                builder.Include(c0);
            }

            var c1 = ComponentID<T1>.ID;
            if (!query.IsIncluded(c1))
            {
                builder ??= query.ToBuilder();
                builder.Include(c1);
            }

            var c2 = ComponentID<T2>.ID;
            if (!query.IsIncluded(c2))
            {
                builder ??= query.ToBuilder();
                builder.Include(c2);
            }

            var c3 = ComponentID<T3>.ID;
            if (!query.IsIncluded(c3))
            {
                builder ??= query.ToBuilder();
                builder.Include(c3);
            }

            var c4 = ComponentID<T4>.ID;
            if (!query.IsIncluded(c4))
            {
                builder ??= query.ToBuilder();
                builder.Include(c4);
            }

            var c5 = ComponentID<T5>.ID;
            if (!query.IsIncluded(c5))
            {
                builder ??= query.ToBuilder();
                builder.Include(c5);
            }

            if (builder != null)
                query = builder.Build(query.World);
        }
        else
        {
            query = GetCachedQuery<T0, T1, T2, T3, T4, T5>();
        }

        return query.Count();
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    public int Count<T0, T1, T2, T3, T4, T5, T6>()
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
    {
        var query = GetCachedQuery<T0, T1>();
        return query.Count();
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    public int Count<T0, T1, T2, T3, T4, T5, T6>(ref QueryDescription? query)
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
    {
        QueryBuilder? builder = null;
        if (query != null)
        {
            var c0 = ComponentID<T0>.ID;
            if (!query.IsIncluded(c0))
            {
                builder ??= query.ToBuilder();
                builder.Include(c0);
            }

            var c1 = ComponentID<T1>.ID;
            if (!query.IsIncluded(c1))
            {
                builder ??= query.ToBuilder();
                builder.Include(c1);
            }

            var c2 = ComponentID<T2>.ID;
            if (!query.IsIncluded(c2))
            {
                builder ??= query.ToBuilder();
                builder.Include(c2);
            }

            var c3 = ComponentID<T3>.ID;
            if (!query.IsIncluded(c3))
            {
                builder ??= query.ToBuilder();
                builder.Include(c3);
            }

            var c4 = ComponentID<T4>.ID;
            if (!query.IsIncluded(c4))
            {
                builder ??= query.ToBuilder();
                builder.Include(c4);
            }

            var c5 = ComponentID<T5>.ID;
            if (!query.IsIncluded(c5))
            {
                builder ??= query.ToBuilder();
                builder.Include(c5);
            }

            var c6 = ComponentID<T6>.ID;
            if (!query.IsIncluded(c6))
            {
                builder ??= query.ToBuilder();
                builder.Include(c6);
            }

            if (builder != null)
                query = builder.Build(query.World);
        }
        else
        {
            query = GetCachedQuery<T0, T1, T2, T3, T4, T5, T6>();
        }

        return query.Count();
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    public int Count<T0, T1, T2, T3, T4, T5, T6, T7>()
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
    {
        var query = GetCachedQuery<T0, T1>();
        return query.Count();
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    public int Count<T0, T1, T2, T3, T4, T5, T6, T7>(ref QueryDescription? query)
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
    {
        QueryBuilder? builder = null;
        if (query != null)
        {
            var c0 = ComponentID<T0>.ID;
            if (!query.IsIncluded(c0))
            {
                builder ??= query.ToBuilder();
                builder.Include(c0);
            }

            var c1 = ComponentID<T1>.ID;
            if (!query.IsIncluded(c1))
            {
                builder ??= query.ToBuilder();
                builder.Include(c1);
            }

            var c2 = ComponentID<T2>.ID;
            if (!query.IsIncluded(c2))
            {
                builder ??= query.ToBuilder();
                builder.Include(c2);
            }

            var c3 = ComponentID<T3>.ID;
            if (!query.IsIncluded(c3))
            {
                builder ??= query.ToBuilder();
                builder.Include(c3);
            }

            var c4 = ComponentID<T4>.ID;
            if (!query.IsIncluded(c4))
            {
                builder ??= query.ToBuilder();
                builder.Include(c4);
            }

            var c5 = ComponentID<T5>.ID;
            if (!query.IsIncluded(c5))
            {
                builder ??= query.ToBuilder();
                builder.Include(c5);
            }

            var c6 = ComponentID<T6>.ID;
            if (!query.IsIncluded(c6))
            {
                builder ??= query.ToBuilder();
                builder.Include(c6);
            }

            var c7 = ComponentID<T7>.ID;
            if (!query.IsIncluded(c7))
            {
                builder ??= query.ToBuilder();
                builder.Include(c7);
            }

            if (builder != null)
                query = builder.Build(query.World);
        }
        else
        {
            query = GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7>();
        }

        return query.Count();
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    public int Count<T0, T1, T2, T3, T4, T5, T6, T7, T8>()
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
        var query = GetCachedQuery<T0, T1>();
        return query.Count();
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    public int Count<T0, T1, T2, T3, T4, T5, T6, T7, T8>(ref QueryDescription? query)
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
        QueryBuilder? builder = null;
        if (query != null)
        {
            var c0 = ComponentID<T0>.ID;
            if (!query.IsIncluded(c0))
            {
                builder ??= query.ToBuilder();
                builder.Include(c0);
            }

            var c1 = ComponentID<T1>.ID;
            if (!query.IsIncluded(c1))
            {
                builder ??= query.ToBuilder();
                builder.Include(c1);
            }

            var c2 = ComponentID<T2>.ID;
            if (!query.IsIncluded(c2))
            {
                builder ??= query.ToBuilder();
                builder.Include(c2);
            }

            var c3 = ComponentID<T3>.ID;
            if (!query.IsIncluded(c3))
            {
                builder ??= query.ToBuilder();
                builder.Include(c3);
            }

            var c4 = ComponentID<T4>.ID;
            if (!query.IsIncluded(c4))
            {
                builder ??= query.ToBuilder();
                builder.Include(c4);
            }

            var c5 = ComponentID<T5>.ID;
            if (!query.IsIncluded(c5))
            {
                builder ??= query.ToBuilder();
                builder.Include(c5);
            }

            var c6 = ComponentID<T6>.ID;
            if (!query.IsIncluded(c6))
            {
                builder ??= query.ToBuilder();
                builder.Include(c6);
            }

            var c7 = ComponentID<T7>.ID;
            if (!query.IsIncluded(c7))
            {
                builder ??= query.ToBuilder();
                builder.Include(c7);
            }

            var c8 = ComponentID<T8>.ID;
            if (!query.IsIncluded(c8))
            {
                builder ??= query.ToBuilder();
                builder.Include(c8);
            }

            if (builder != null)
                query = builder.Build(query.World);
        }
        else
        {
            query = GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8>();
        }

        return query.Count();
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    public int Count<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>()
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
        var query = GetCachedQuery<T0, T1>();
        return query.Count();
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    public int Count<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(ref QueryDescription? query)
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
        QueryBuilder? builder = null;
        if (query != null)
        {
            var c0 = ComponentID<T0>.ID;
            if (!query.IsIncluded(c0))
            {
                builder ??= query.ToBuilder();
                builder.Include(c0);
            }

            var c1 = ComponentID<T1>.ID;
            if (!query.IsIncluded(c1))
            {
                builder ??= query.ToBuilder();
                builder.Include(c1);
            }

            var c2 = ComponentID<T2>.ID;
            if (!query.IsIncluded(c2))
            {
                builder ??= query.ToBuilder();
                builder.Include(c2);
            }

            var c3 = ComponentID<T3>.ID;
            if (!query.IsIncluded(c3))
            {
                builder ??= query.ToBuilder();
                builder.Include(c3);
            }

            var c4 = ComponentID<T4>.ID;
            if (!query.IsIncluded(c4))
            {
                builder ??= query.ToBuilder();
                builder.Include(c4);
            }

            var c5 = ComponentID<T5>.ID;
            if (!query.IsIncluded(c5))
            {
                builder ??= query.ToBuilder();
                builder.Include(c5);
            }

            var c6 = ComponentID<T6>.ID;
            if (!query.IsIncluded(c6))
            {
                builder ??= query.ToBuilder();
                builder.Include(c6);
            }

            var c7 = ComponentID<T7>.ID;
            if (!query.IsIncluded(c7))
            {
                builder ??= query.ToBuilder();
                builder.Include(c7);
            }

            var c8 = ComponentID<T8>.ID;
            if (!query.IsIncluded(c8))
            {
                builder ??= query.ToBuilder();
                builder.Include(c8);
            }

            var c9 = ComponentID<T9>.ID;
            if (!query.IsIncluded(c9))
            {
                builder ??= query.ToBuilder();
                builder.Include(c9);
            }

            if (builder != null)
                query = builder.Build(query.World);
        }
        else
        {
            query = GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>();
        }

        return query.Count();
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    public int Count<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>()
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
        var query = GetCachedQuery<T0, T1>();
        return query.Count();
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    public int Count<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(ref QueryDescription? query)
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
        QueryBuilder? builder = null;
        if (query != null)
        {
            var c0 = ComponentID<T0>.ID;
            if (!query.IsIncluded(c0))
            {
                builder ??= query.ToBuilder();
                builder.Include(c0);
            }

            var c1 = ComponentID<T1>.ID;
            if (!query.IsIncluded(c1))
            {
                builder ??= query.ToBuilder();
                builder.Include(c1);
            }

            var c2 = ComponentID<T2>.ID;
            if (!query.IsIncluded(c2))
            {
                builder ??= query.ToBuilder();
                builder.Include(c2);
            }

            var c3 = ComponentID<T3>.ID;
            if (!query.IsIncluded(c3))
            {
                builder ??= query.ToBuilder();
                builder.Include(c3);
            }

            var c4 = ComponentID<T4>.ID;
            if (!query.IsIncluded(c4))
            {
                builder ??= query.ToBuilder();
                builder.Include(c4);
            }

            var c5 = ComponentID<T5>.ID;
            if (!query.IsIncluded(c5))
            {
                builder ??= query.ToBuilder();
                builder.Include(c5);
            }

            var c6 = ComponentID<T6>.ID;
            if (!query.IsIncluded(c6))
            {
                builder ??= query.ToBuilder();
                builder.Include(c6);
            }

            var c7 = ComponentID<T7>.ID;
            if (!query.IsIncluded(c7))
            {
                builder ??= query.ToBuilder();
                builder.Include(c7);
            }

            var c8 = ComponentID<T8>.ID;
            if (!query.IsIncluded(c8))
            {
                builder ??= query.ToBuilder();
                builder.Include(c8);
            }

            var c9 = ComponentID<T9>.ID;
            if (!query.IsIncluded(c9))
            {
                builder ??= query.ToBuilder();
                builder.Include(c9);
            }

            var c10 = ComponentID<T10>.ID;
            if (!query.IsIncluded(c10))
            {
                builder ??= query.ToBuilder();
                builder.Include(c10);
            }

            if (builder != null)
                query = builder.Build(query.World);
        }
        else
        {
            query = GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>();
        }

        return query.Count();
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    public int Count<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>()
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
        var query = GetCachedQuery<T0, T1>();
        return query.Count();
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    public int Count<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(ref QueryDescription? query)
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
        QueryBuilder? builder = null;
        if (query != null)
        {
            var c0 = ComponentID<T0>.ID;
            if (!query.IsIncluded(c0))
            {
                builder ??= query.ToBuilder();
                builder.Include(c0);
            }

            var c1 = ComponentID<T1>.ID;
            if (!query.IsIncluded(c1))
            {
                builder ??= query.ToBuilder();
                builder.Include(c1);
            }

            var c2 = ComponentID<T2>.ID;
            if (!query.IsIncluded(c2))
            {
                builder ??= query.ToBuilder();
                builder.Include(c2);
            }

            var c3 = ComponentID<T3>.ID;
            if (!query.IsIncluded(c3))
            {
                builder ??= query.ToBuilder();
                builder.Include(c3);
            }

            var c4 = ComponentID<T4>.ID;
            if (!query.IsIncluded(c4))
            {
                builder ??= query.ToBuilder();
                builder.Include(c4);
            }

            var c5 = ComponentID<T5>.ID;
            if (!query.IsIncluded(c5))
            {
                builder ??= query.ToBuilder();
                builder.Include(c5);
            }

            var c6 = ComponentID<T6>.ID;
            if (!query.IsIncluded(c6))
            {
                builder ??= query.ToBuilder();
                builder.Include(c6);
            }

            var c7 = ComponentID<T7>.ID;
            if (!query.IsIncluded(c7))
            {
                builder ??= query.ToBuilder();
                builder.Include(c7);
            }

            var c8 = ComponentID<T8>.ID;
            if (!query.IsIncluded(c8))
            {
                builder ??= query.ToBuilder();
                builder.Include(c8);
            }

            var c9 = ComponentID<T9>.ID;
            if (!query.IsIncluded(c9))
            {
                builder ??= query.ToBuilder();
                builder.Include(c9);
            }

            var c10 = ComponentID<T10>.ID;
            if (!query.IsIncluded(c10))
            {
                builder ??= query.ToBuilder();
                builder.Include(c10);
            }

            var c11 = ComponentID<T11>.ID;
            if (!query.IsIncluded(c11))
            {
                builder ??= query.ToBuilder();
                builder.Include(c11);
            }

            if (builder != null)
                query = builder.Build(query.World);
        }
        else
        {
            query = GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>();
        }

        return query.Count();
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    public int Count<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>()
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
        var query = GetCachedQuery<T0, T1>();
        return query.Count();
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    public int Count<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(ref QueryDescription? query)
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
        QueryBuilder? builder = null;
        if (query != null)
        {
            var c0 = ComponentID<T0>.ID;
            if (!query.IsIncluded(c0))
            {
                builder ??= query.ToBuilder();
                builder.Include(c0);
            }

            var c1 = ComponentID<T1>.ID;
            if (!query.IsIncluded(c1))
            {
                builder ??= query.ToBuilder();
                builder.Include(c1);
            }

            var c2 = ComponentID<T2>.ID;
            if (!query.IsIncluded(c2))
            {
                builder ??= query.ToBuilder();
                builder.Include(c2);
            }

            var c3 = ComponentID<T3>.ID;
            if (!query.IsIncluded(c3))
            {
                builder ??= query.ToBuilder();
                builder.Include(c3);
            }

            var c4 = ComponentID<T4>.ID;
            if (!query.IsIncluded(c4))
            {
                builder ??= query.ToBuilder();
                builder.Include(c4);
            }

            var c5 = ComponentID<T5>.ID;
            if (!query.IsIncluded(c5))
            {
                builder ??= query.ToBuilder();
                builder.Include(c5);
            }

            var c6 = ComponentID<T6>.ID;
            if (!query.IsIncluded(c6))
            {
                builder ??= query.ToBuilder();
                builder.Include(c6);
            }

            var c7 = ComponentID<T7>.ID;
            if (!query.IsIncluded(c7))
            {
                builder ??= query.ToBuilder();
                builder.Include(c7);
            }

            var c8 = ComponentID<T8>.ID;
            if (!query.IsIncluded(c8))
            {
                builder ??= query.ToBuilder();
                builder.Include(c8);
            }

            var c9 = ComponentID<T9>.ID;
            if (!query.IsIncluded(c9))
            {
                builder ??= query.ToBuilder();
                builder.Include(c9);
            }

            var c10 = ComponentID<T10>.ID;
            if (!query.IsIncluded(c10))
            {
                builder ??= query.ToBuilder();
                builder.Include(c10);
            }

            var c11 = ComponentID<T11>.ID;
            if (!query.IsIncluded(c11))
            {
                builder ??= query.ToBuilder();
                builder.Include(c11);
            }

            var c12 = ComponentID<T12>.ID;
            if (!query.IsIncluded(c12))
            {
                builder ??= query.ToBuilder();
                builder.Include(c12);
            }

            if (builder != null)
                query = builder.Build(query.World);
        }
        else
        {
            query = GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>();
        }

        return query.Count();
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    public int Count<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>()
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
        var query = GetCachedQuery<T0, T1>();
        return query.Count();
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    public int Count<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(ref QueryDescription? query)
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
        QueryBuilder? builder = null;
        if (query != null)
        {
            var c0 = ComponentID<T0>.ID;
            if (!query.IsIncluded(c0))
            {
                builder ??= query.ToBuilder();
                builder.Include(c0);
            }

            var c1 = ComponentID<T1>.ID;
            if (!query.IsIncluded(c1))
            {
                builder ??= query.ToBuilder();
                builder.Include(c1);
            }

            var c2 = ComponentID<T2>.ID;
            if (!query.IsIncluded(c2))
            {
                builder ??= query.ToBuilder();
                builder.Include(c2);
            }

            var c3 = ComponentID<T3>.ID;
            if (!query.IsIncluded(c3))
            {
                builder ??= query.ToBuilder();
                builder.Include(c3);
            }

            var c4 = ComponentID<T4>.ID;
            if (!query.IsIncluded(c4))
            {
                builder ??= query.ToBuilder();
                builder.Include(c4);
            }

            var c5 = ComponentID<T5>.ID;
            if (!query.IsIncluded(c5))
            {
                builder ??= query.ToBuilder();
                builder.Include(c5);
            }

            var c6 = ComponentID<T6>.ID;
            if (!query.IsIncluded(c6))
            {
                builder ??= query.ToBuilder();
                builder.Include(c6);
            }

            var c7 = ComponentID<T7>.ID;
            if (!query.IsIncluded(c7))
            {
                builder ??= query.ToBuilder();
                builder.Include(c7);
            }

            var c8 = ComponentID<T8>.ID;
            if (!query.IsIncluded(c8))
            {
                builder ??= query.ToBuilder();
                builder.Include(c8);
            }

            var c9 = ComponentID<T9>.ID;
            if (!query.IsIncluded(c9))
            {
                builder ??= query.ToBuilder();
                builder.Include(c9);
            }

            var c10 = ComponentID<T10>.ID;
            if (!query.IsIncluded(c10))
            {
                builder ??= query.ToBuilder();
                builder.Include(c10);
            }

            var c11 = ComponentID<T11>.ID;
            if (!query.IsIncluded(c11))
            {
                builder ??= query.ToBuilder();
                builder.Include(c11);
            }

            var c12 = ComponentID<T12>.ID;
            if (!query.IsIncluded(c12))
            {
                builder ??= query.ToBuilder();
                builder.Include(c12);
            }

            var c13 = ComponentID<T13>.ID;
            if (!query.IsIncluded(c13))
            {
                builder ??= query.ToBuilder();
                builder.Include(c13);
            }

            if (builder != null)
                query = builder.Build(query.World);
        }
        else
        {
            query = GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>();
        }

        return query.Count();
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    public int Count<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>()
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
        var query = GetCachedQuery<T0, T1>();
        return query.Count();
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    public int Count<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(ref QueryDescription? query)
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
        QueryBuilder? builder = null;
        if (query != null)
        {
            var c0 = ComponentID<T0>.ID;
            if (!query.IsIncluded(c0))
            {
                builder ??= query.ToBuilder();
                builder.Include(c0);
            }

            var c1 = ComponentID<T1>.ID;
            if (!query.IsIncluded(c1))
            {
                builder ??= query.ToBuilder();
                builder.Include(c1);
            }

            var c2 = ComponentID<T2>.ID;
            if (!query.IsIncluded(c2))
            {
                builder ??= query.ToBuilder();
                builder.Include(c2);
            }

            var c3 = ComponentID<T3>.ID;
            if (!query.IsIncluded(c3))
            {
                builder ??= query.ToBuilder();
                builder.Include(c3);
            }

            var c4 = ComponentID<T4>.ID;
            if (!query.IsIncluded(c4))
            {
                builder ??= query.ToBuilder();
                builder.Include(c4);
            }

            var c5 = ComponentID<T5>.ID;
            if (!query.IsIncluded(c5))
            {
                builder ??= query.ToBuilder();
                builder.Include(c5);
            }

            var c6 = ComponentID<T6>.ID;
            if (!query.IsIncluded(c6))
            {
                builder ??= query.ToBuilder();
                builder.Include(c6);
            }

            var c7 = ComponentID<T7>.ID;
            if (!query.IsIncluded(c7))
            {
                builder ??= query.ToBuilder();
                builder.Include(c7);
            }

            var c8 = ComponentID<T8>.ID;
            if (!query.IsIncluded(c8))
            {
                builder ??= query.ToBuilder();
                builder.Include(c8);
            }

            var c9 = ComponentID<T9>.ID;
            if (!query.IsIncluded(c9))
            {
                builder ??= query.ToBuilder();
                builder.Include(c9);
            }

            var c10 = ComponentID<T10>.ID;
            if (!query.IsIncluded(c10))
            {
                builder ??= query.ToBuilder();
                builder.Include(c10);
            }

            var c11 = ComponentID<T11>.ID;
            if (!query.IsIncluded(c11))
            {
                builder ??= query.ToBuilder();
                builder.Include(c11);
            }

            var c12 = ComponentID<T12>.ID;
            if (!query.IsIncluded(c12))
            {
                builder ??= query.ToBuilder();
                builder.Include(c12);
            }

            var c13 = ComponentID<T13>.ID;
            if (!query.IsIncluded(c13))
            {
                builder ??= query.ToBuilder();
                builder.Include(c13);
            }

            var c14 = ComponentID<T14>.ID;
            if (!query.IsIncluded(c14))
            {
                builder ??= query.ToBuilder();
                builder.Include(c14);
            }

            if (builder != null)
                query = builder.Build(query.World);
        }
        else
        {
            query = GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>();
        }

        return query.Count();
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    public int Count<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>()
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
        var query = GetCachedQuery<T0, T1>();
        return query.Count();
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    public int Count<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(ref QueryDescription? query)
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
        QueryBuilder? builder = null;
        if (query != null)
        {
            var c0 = ComponentID<T0>.ID;
            if (!query.IsIncluded(c0))
            {
                builder ??= query.ToBuilder();
                builder.Include(c0);
            }

            var c1 = ComponentID<T1>.ID;
            if (!query.IsIncluded(c1))
            {
                builder ??= query.ToBuilder();
                builder.Include(c1);
            }

            var c2 = ComponentID<T2>.ID;
            if (!query.IsIncluded(c2))
            {
                builder ??= query.ToBuilder();
                builder.Include(c2);
            }

            var c3 = ComponentID<T3>.ID;
            if (!query.IsIncluded(c3))
            {
                builder ??= query.ToBuilder();
                builder.Include(c3);
            }

            var c4 = ComponentID<T4>.ID;
            if (!query.IsIncluded(c4))
            {
                builder ??= query.ToBuilder();
                builder.Include(c4);
            }

            var c5 = ComponentID<T5>.ID;
            if (!query.IsIncluded(c5))
            {
                builder ??= query.ToBuilder();
                builder.Include(c5);
            }

            var c6 = ComponentID<T6>.ID;
            if (!query.IsIncluded(c6))
            {
                builder ??= query.ToBuilder();
                builder.Include(c6);
            }

            var c7 = ComponentID<T7>.ID;
            if (!query.IsIncluded(c7))
            {
                builder ??= query.ToBuilder();
                builder.Include(c7);
            }

            var c8 = ComponentID<T8>.ID;
            if (!query.IsIncluded(c8))
            {
                builder ??= query.ToBuilder();
                builder.Include(c8);
            }

            var c9 = ComponentID<T9>.ID;
            if (!query.IsIncluded(c9))
            {
                builder ??= query.ToBuilder();
                builder.Include(c9);
            }

            var c10 = ComponentID<T10>.ID;
            if (!query.IsIncluded(c10))
            {
                builder ??= query.ToBuilder();
                builder.Include(c10);
            }

            var c11 = ComponentID<T11>.ID;
            if (!query.IsIncluded(c11))
            {
                builder ??= query.ToBuilder();
                builder.Include(c11);
            }

            var c12 = ComponentID<T12>.ID;
            if (!query.IsIncluded(c12))
            {
                builder ??= query.ToBuilder();
                builder.Include(c12);
            }

            var c13 = ComponentID<T13>.ID;
            if (!query.IsIncluded(c13))
            {
                builder ??= query.ToBuilder();
                builder.Include(c13);
            }

            var c14 = ComponentID<T14>.ID;
            if (!query.IsIncluded(c14))
            {
                builder ??= query.ToBuilder();
                builder.Include(c14);
            }

            var c15 = ComponentID<T15>.ID;
            if (!query.IsIncluded(c15))
            {
                builder ??= query.ToBuilder();
                builder.Include(c15);
            }

            if (builder != null)
                query = builder.Build(query.World);
        }
        else
        {
            query = GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>();
        }

        return query.Count();
    }

}

