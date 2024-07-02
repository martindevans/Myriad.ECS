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
        var query = GetCachedQuery<T0>();
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
    /// <returns></returns>
    public int Count<T0, T1, T2>()
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
    {
        var query = GetCachedQuery<T0, T1, T2>();
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
        var query = GetCachedQuery<T0, T1, T2, T3>();
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
        var query = GetCachedQuery<T0, T1, T2, T3, T4>();
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
        var query = GetCachedQuery<T0, T1, T2, T3, T4, T5>();
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
        var query = GetCachedQuery<T0, T1, T2, T3, T4, T5, T6>();
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
        var query = GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7>();
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
        var query = GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8>();
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
        var query = GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>();
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
        var query = GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>();
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
        var query = GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>();
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
        var query = GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>();
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
        var query = GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>();
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
        var query = GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>();
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
        var query = GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>();
        return query.Count();
    }

}

