namespace Myriad.ECS.Collections;

public class FrozenOrderedListSet<TItem>
    where TItem : struct, IComparable<TItem>, IEquatable<TItem>
{
    private readonly OrderedListSet<TItem> _items;
    public int Count => _items.Count;

    #region constructors
    private FrozenOrderedListSet(OrderedListSet<TItem> dangerousItems)
    {
        _items = dangerousItems;
    }

    internal static FrozenOrderedListSet<TItem> Create(List<TItem> items)
    {
        return new FrozenOrderedListSet<TItem>(new OrderedListSet<TItem>(items));
    }

    internal static FrozenOrderedListSet<TItem> Create(HashSet<TItem> items)
    {
        return new FrozenOrderedListSet<TItem>(new OrderedListSet<TItem>(items));
    }

    internal static FrozenOrderedListSet<TItem> Create(OrderedListSet<TItem> items)
    {
        return new FrozenOrderedListSet<TItem>(new OrderedListSet<TItem>(items));
    }

    internal static FrozenOrderedListSet<TItem> Create<TV>(Dictionary<TItem, TV> items)
    {
        var set = new OrderedListSet<TItem>();
        set.AddRange(items.Keys);
        return new FrozenOrderedListSet<TItem>(set);
    }
    #endregion

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
    public List<TItem>.Enumerator GetEnumerator()
    {
        // ReSharper disable once NotDisposedResourceIsReturned
        return _items.GetEnumerator();
    }
    #endregion

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
    internal bool IsSupersetOf(OrderedListSet<TItem> other)
    {
        return _items.IsSupersetOf(other);
    }

    public bool IsSupersetOf(FrozenOrderedListSet<TItem> other)
    {
        return IsSupersetOf(other._items);
    }
    #endregion

    #region Overlaps
    internal bool Overlaps(OrderedListSet<TItem> other)
    {
        return _items.Overlaps(other);
    }

    public bool Overlaps(FrozenOrderedListSet<TItem> other)
    {
        return Overlaps(other._items);
    }
    #endregion

    #region SetEquals
    internal bool SetEquals(OrderedListSet<TItem> other)
    {
        return _items.SetEquals(other);
    }

    public bool SetEquals(FrozenOrderedListSet<TItem> other)
    {
        return SetEquals(other._items);
    }

    public bool SetEquals<TV>(Dictionary<TItem, TV> other)
    {
        return _items.SetEquals(other);
    }
    #endregion
}