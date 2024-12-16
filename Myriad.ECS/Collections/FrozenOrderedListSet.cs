namespace Myriad.ECS.Collections;

/// <summary>
/// A frozen (i.e. completely immutable) set of objects.
/// </summary>
/// <typeparam name="TItem"></typeparam>
public class FrozenOrderedListSet<TItem>
    where TItem : struct, IComparable<TItem>, IEquatable<TItem>
{
    /// <summary>
    /// An empty frozen set
    /// </summary>
    public static readonly FrozenOrderedListSet<TItem> Empty = new(new OrderedListSet<TItem>());

    private readonly OrderedListSet<TItem> _items;

    /// <summary>
    /// Get the number of items in this set
    /// </summary>
    public int Count => _items.Count;

    #region constructors
    private FrozenOrderedListSet(OrderedListSet<TItem> dangerousItems)
    {
        _items = dangerousItems;
    }

    internal static FrozenOrderedListSet<TItem> Create(List<TItem> items)
    {
        if (items.Count == 0)
            return Empty;

        return new FrozenOrderedListSet<TItem>(new OrderedListSet<TItem>(items));
    }

    internal static FrozenOrderedListSet<TItem> Create(ReadOnlySpan<TItem> items)
    {
        if (items.Length == 0)
            return Empty;

        return new FrozenOrderedListSet<TItem>(new OrderedListSet<TItem>(items));
    }

    internal static FrozenOrderedListSet<TItem> Create(HashSet<TItem> items)
    {
        if (items.Count == 0)
            return Empty;

        return new FrozenOrderedListSet<TItem>(new OrderedListSet<TItem>(items));
    }

    internal static FrozenOrderedListSet<TItem> Create(OrderedListSet<TItem> items)
    {
        if (items.Count == 0)
            return Empty;

        return new FrozenOrderedListSet<TItem>(new OrderedListSet<TItem>(items));
    }

    internal static FrozenOrderedListSet<TItem> Create<TV>(Dictionary<TItem, TV> items)
    {
        if (items.Count == 0)
            return Empty;

        var set = new OrderedListSet<TItem>();
        set.AddRange(items.Keys);
        return new FrozenOrderedListSet<TItem>(set);
    }
    #endregion

    /// <summary>
    /// Copy this set to the given list
    /// </summary>
    /// <param name="dest"></param>
    public void CopyTo(List<TItem> dest)
    {
        _items.CopyTo(dest);
    }

    /// <summary>
    /// Get a collection which can be queried by LINQ (only use this for tests)
    /// </summary>
    /// <returns></returns>
    internal IReadOnlyCollection<TItem> LINQ()
    {
        return _items.LINQ();
    }

    #region GetEnumerator
    /// <summary>
    /// Get an enumerator over the items in this set
    /// </summary>
    /// <returns></returns>
    public List<TItem>.Enumerator GetEnumerator()
    {
        // ReSharper disable once NotDisposedResourceIsReturned
        return _items.GetEnumerator();
    }
    #endregion

    /// <summary>
    /// Check if this set contains the given item
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool Contains(TItem item)
    {
        return _items.Contains(item);
    }

    //todo: other set methods when needed

    //#region IsProperSubsetOf
    //internal bool IsProperSubsetOf(OrderedListSet<TItem> other)
    //{
    //    return _items.IsProperSubsetOf(other);
    //}

    //public bool IsProperSubsetOf(FrozenOrderedListSet<TItem> other)
    //{
    //    return _items.IsProperSubsetOf(other._items);
    //}
    //#endregion

    //#region IsProperSupersetOf
    //internal bool IsProperSupersetOf(OrderedListSet<TItem> other)
    //{
    //    return _items.IsProperSupersetOf(other);
    //}

    //public bool IsProperSupersetOf(FrozenOrderedListSet<TItem> other)
    //{
    //    return _items.IsProperSupersetOf(other._items);
    //}
    //#endregion

    //#region IsSubsetOf
    //internal bool IsSubsetOf(OrderedListSet<TItem> other)
    //{
    //    return _items.IsSubsetOf(other);
    //}

    //public bool IsSubsetOf(FrozenOrderedListSet<TItem> other)
    //{
    //    return _items.IsSubsetOf(other._items);
    //}
    //#endregion

    #region IsSupersetOf
    /// <summary>
    /// Check if this set is a superset of another set. i.e. contains all the items in the other set.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    internal bool IsSupersetOf(OrderedListSet<TItem> other)
    {
        return _items.IsSupersetOf(other);
    }

    /// <summary>
    /// Check if this set is a superset of another set. i.e. contains all the items in the other set.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool IsSupersetOf(FrozenOrderedListSet<TItem> other)
    {
        return IsSupersetOf(other._items);
    }
    #endregion

    #region Overlaps
    /// <summary>
    /// Check if this set overlaps another set. i.e. contains at least one item which is in the other set.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    internal bool Overlaps(OrderedListSet<TItem> other)
    {
        return _items.Overlaps(other);
    }

    /// <summary>
    /// Check if this set overlaps another set. i.e. contains at least one item which is in the other set.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Overlaps(FrozenOrderedListSet<TItem> other)
    {
        return Overlaps(other._items);
    }
    #endregion

    #region SetEquals
    /// <summary>
    /// Check if this set contains exactly the same items as another set
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    internal bool SetEquals(OrderedListSet<TItem> other)
    {
        return _items.SetEquals(other);
    }

    /// <summary>
    /// Check if this set contains exactly the same items as another set
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool SetEquals(FrozenOrderedListSet<TItem> other)
    {
        return SetEquals(other._items);
    }

    /// <summary>
    /// Check if this set contains exactly the same items as another set
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool SetEquals<TV>(Dictionary<TItem, TV> other)
    {
        return _items.SetEquals(other);
    }
    #endregion
}