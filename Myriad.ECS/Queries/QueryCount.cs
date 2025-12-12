using Myriad.ECS.Queries;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedParameter.Global
// ReSharper disable LoopCanBeConvertedToQuery
// ReSharper disable CheckNamespace
// ReSharper disable ArrangeAccessorOwnerBody

namespace Myriad.ECS.Worlds;

public partial class World
{
    /// <summary>
    /// Count how many entities exist
    /// </summary>
    /// <returns></returns>
    public int Count()
    {
        var count = 0;
        foreach (var archetype in _archetypes)
            count += archetype.EntityCount;
        return count;
    }

    /// <summary>
    /// Count how many entities match this query
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Count(QueryDescription query)
    {
        return query.Count();
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Count<T0>()
        where T0 : IComponent
    {
        QueryDescription? q = null;
        return Count<T0>(ref q);
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <param name="cache">If null, will be set the the query. If not null, will be used to determine the count.</param>
    /// <returns></returns>
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Count<T0>(ref QueryDescription? cache)
        where T0 : IComponent
    {
        cache ??= GetCachedQuery<T0>();
        return Count(cache);
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Count<T0, T1>()
        where T0 : IComponent
        where T1 : IComponent
    {
        QueryDescription? q = null;
        return Count<T0, T1>(ref q);
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <param name="cache">If null, will be set the the query. If not null, will be used to determine the count.</param>
    /// <returns></returns>
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Count<T0, T1>(ref QueryDescription? cache)
        where T0 : IComponent
        where T1 : IComponent
    {
        cache ??= GetCachedQuery<T0, T1>();
        return Count(cache);
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    [ExcludeFromCodeCoverage]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Count<T0, T1, T2>()
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
    {
        QueryDescription? q = null;
        return Count<T0, T1, T2>(ref q);
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <param name="cache">If null, will be set the the query. If not null, will be used to determine the count.</param>
    /// <returns></returns>
    [ExcludeFromCodeCoverage]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Count<T0, T1, T2>(ref QueryDescription? cache)
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
    {
        cache ??= GetCachedQuery<T0, T1, T2>();
        return Count(cache);
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    [ExcludeFromCodeCoverage]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Count<T0, T1, T2, T3>()
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
    {
        QueryDescription? q = null;
        return Count<T0, T1, T2, T3>(ref q);
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <param name="cache">If null, will be set the the query. If not null, will be used to determine the count.</param>
    /// <returns></returns>
    [ExcludeFromCodeCoverage]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Count<T0, T1, T2, T3>(ref QueryDescription? cache)
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
    {
        cache ??= GetCachedQuery<T0, T1, T2, T3>();
        return Count(cache);
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    [ExcludeFromCodeCoverage]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Count<T0, T1, T2, T3, T4>()
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
    {
        QueryDescription? q = null;
        return Count<T0, T1, T2, T3, T4>(ref q);
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <param name="cache">If null, will be set the the query. If not null, will be used to determine the count.</param>
    /// <returns></returns>
    [ExcludeFromCodeCoverage]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Count<T0, T1, T2, T3, T4>(ref QueryDescription? cache)
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
    {
        cache ??= GetCachedQuery<T0, T1, T2, T3, T4>();
        return Count(cache);
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    [ExcludeFromCodeCoverage]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Count<T0, T1, T2, T3, T4, T5>()
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
    {
        QueryDescription? q = null;
        return Count<T0, T1, T2, T3, T4, T5>(ref q);
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <param name="cache">If null, will be set the the query. If not null, will be used to determine the count.</param>
    /// <returns></returns>
    [ExcludeFromCodeCoverage]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Count<T0, T1, T2, T3, T4, T5>(ref QueryDescription? cache)
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
    {
        cache ??= GetCachedQuery<T0, T1, T2, T3, T4, T5>();
        return Count(cache);
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    [ExcludeFromCodeCoverage]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Count<T0, T1, T2, T3, T4, T5, T6>()
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
    {
        QueryDescription? q = null;
        return Count<T0, T1, T2, T3, T4, T5, T6>(ref q);
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <param name="cache">If null, will be set the the query. If not null, will be used to determine the count.</param>
    /// <returns></returns>
    [ExcludeFromCodeCoverage]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Count<T0, T1, T2, T3, T4, T5, T6>(ref QueryDescription? cache)
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
    {
        cache ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6>();
        return Count(cache);
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    [ExcludeFromCodeCoverage]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        QueryDescription? q = null;
        return Count<T0, T1, T2, T3, T4, T5, T6, T7>(ref q);
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <param name="cache">If null, will be set the the query. If not null, will be used to determine the count.</param>
    /// <returns></returns>
    [ExcludeFromCodeCoverage]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Count<T0, T1, T2, T3, T4, T5, T6, T7>(ref QueryDescription? cache)
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
    {
        cache ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7>();
        return Count(cache);
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    [ExcludeFromCodeCoverage]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        QueryDescription? q = null;
        return Count<T0, T1, T2, T3, T4, T5, T6, T7, T8>(ref q);
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <param name="cache">If null, will be set the the query. If not null, will be used to determine the count.</param>
    /// <returns></returns>
    [ExcludeFromCodeCoverage]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Count<T0, T1, T2, T3, T4, T5, T6, T7, T8>(ref QueryDescription? cache)
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
        cache ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8>();
        return Count(cache);
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    [ExcludeFromCodeCoverage]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        QueryDescription? q = null;
        return Count<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(ref q);
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <param name="cache">If null, will be set the the query. If not null, will be used to determine the count.</param>
    /// <returns></returns>
    [ExcludeFromCodeCoverage]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Count<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(ref QueryDescription? cache)
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
        cache ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>();
        return Count(cache);
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    [ExcludeFromCodeCoverage]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        QueryDescription? q = null;
        return Count<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(ref q);
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <param name="cache">If null, will be set the the query. If not null, will be used to determine the count.</param>
    /// <returns></returns>
    [ExcludeFromCodeCoverage]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Count<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(ref QueryDescription? cache)
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
        cache ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>();
        return Count(cache);
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    [ExcludeFromCodeCoverage]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        QueryDescription? q = null;
        return Count<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(ref q);
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <param name="cache">If null, will be set the the query. If not null, will be used to determine the count.</param>
    /// <returns></returns>
    [ExcludeFromCodeCoverage]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Count<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(ref QueryDescription? cache)
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
        cache ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>();
        return Count(cache);
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    [ExcludeFromCodeCoverage]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        QueryDescription? q = null;
        return Count<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(ref q);
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <param name="cache">If null, will be set the the query. If not null, will be used to determine the count.</param>
    /// <returns></returns>
    [ExcludeFromCodeCoverage]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Count<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(ref QueryDescription? cache)
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
        cache ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>();
        return Count(cache);
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    [ExcludeFromCodeCoverage]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        QueryDescription? q = null;
        return Count<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(ref q);
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <param name="cache">If null, will be set the the query. If not null, will be used to determine the count.</param>
    /// <returns></returns>
    [ExcludeFromCodeCoverage]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Count<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(ref QueryDescription? cache)
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
        cache ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>();
        return Count(cache);
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    [ExcludeFromCodeCoverage]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        QueryDescription? q = null;
        return Count<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(ref q);
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <param name="cache">If null, will be set the the query. If not null, will be used to determine the count.</param>
    /// <returns></returns>
    [ExcludeFromCodeCoverage]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Count<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(ref QueryDescription? cache)
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
        cache ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>();
        return Count(cache);
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <returns></returns>
    [ExcludeFromCodeCoverage]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        QueryDescription? q = null;
        return Count<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(ref q);
    }

    /// <summary>
    /// Count how many entities exist which include all given components
    /// </summary>
    /// <param name="cache">If null, will be set the the query. If not null, will be used to determine the count.</param>
    /// <returns></returns>
    [ExcludeFromCodeCoverage]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Count<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(ref QueryDescription? cache)
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
        cache ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>();
        return Count(cache);
    }

}

