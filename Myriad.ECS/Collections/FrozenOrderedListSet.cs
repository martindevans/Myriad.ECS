using System.Collections;

namespace Myriad.ECS.Collections;

public class FrozenOrderedListSet<TItem>
    : IReadOnlySet<TItem>
    where TItem : IComparable<TItem>, IEquatable<TItem>
{
    private readonly OrderedListSet<TItem> _items;
    public int Count => _items.Count;

    #region constructors
    public FrozenOrderedListSet()
    {
        _items = [ ];
    }

    public FrozenOrderedListSet(List<TItem> items)
    {
        _items = new OrderedListSet<TItem>(items);
    }

    public FrozenOrderedListSet(IReadOnlySet<TItem> items)
    {
        _items = new OrderedListSet<TItem>(items);
    }

    public FrozenOrderedListSet(OrderedListSet<TItem> items)
    {
        _items = new OrderedListSet<TItem>(items);
    }

    public FrozenOrderedListSet(FrozenOrderedListSet<TItem> items)
    {
        _items = new OrderedListSet<TItem>(items);
    }
    #endregion

    #region GetEnumerator
    public List<TItem>.Enumerator GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    IEnumerator<TItem> IEnumerable<TItem>.GetEnumerator()
    {
        // ReSharper disable once HeapView.BoxingAllocation
        return _items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        // ReSharper disable once HeapView.BoxingAllocation
        return GetEnumerator();
    }
    #endregion

    public bool Contains(TItem item)
    {
        return _items.Contains(item);
    }

    #region IsProperSubsetOf
    public bool IsProperSubsetOf(IEnumerable<TItem> other)
    {
        if (other is OrderedListSet<TItem> ols)
            return _items.IsProperSubsetOf(ols);
        if (other is FrozenOrderedListSet<TItem> fols)
            return _items.IsProperSubsetOf(fols);

        return _items.IsProperSubsetOf(other);
    }

    public bool IsProperSubsetOf(OrderedListSet<TItem> other)
    {
        return _items.IsProperSubsetOf(other);
    }

    public bool IsProperSubsetOf(FrozenOrderedListSet<TItem> other)
    {
        return _items.IsProperSubsetOf(other._items);
    }
    #endregion

    #region IsProperSupersetOf
    public bool IsProperSupersetOf(IEnumerable<TItem> other)
    {
        if (other is OrderedListSet<TItem> ols)
            return _items.IsProperSupersetOf(ols);
        if (other is FrozenOrderedListSet<TItem> fols)
            return _items.IsProperSupersetOf(fols);

        return _items.IsProperSupersetOf(other);
    }

    public bool IsProperSupersetOf(OrderedListSet<TItem> other)
    {
        return _items.IsProperSupersetOf(other);
    }

    public bool IsProperSupersetOf(FrozenOrderedListSet<TItem> other)
    {
        return _items.IsProperSupersetOf(other._items);
    }
    #endregion

    #region IsSubsetOf
    public bool IsSubsetOf(IEnumerable<TItem> other)
    {
        if (other is OrderedListSet<TItem> ols)
            return _items.IsSubsetOf(ols);
        if (other is FrozenOrderedListSet<TItem> fols)
            return _items.IsSubsetOf(fols);

        return _items.IsSubsetOf(other);
    }

    public bool IsSubsetOf(OrderedListSet<TItem> other)
    {
        return _items.IsSubsetOf(other);
    }

    public bool IsSubsetOf(FrozenOrderedListSet<TItem> other)
    {
        return _items.IsSubsetOf(other._items);
    }
    #endregion

    #region IsSupersetOf
    public bool IsSupersetOf(IEnumerable<TItem> other)
    {
        if (other is OrderedListSet<TItem> ols)
            return _items.IsSupersetOf(ols);
        if (other is FrozenOrderedListSet<TItem> fols)
            return _items.IsSupersetOf(fols);

        return _items.IsSupersetOf(other);
    }

    public bool IsSupersetOf(OrderedListSet<TItem> other)
    {
        return _items.IsSupersetOf(other);
    }

    public bool IsSupersetOf(FrozenOrderedListSet<TItem> other)
    {
        return _items.IsSupersetOf(other._items);
    }
    #endregion

    #region Overlaps
    public bool Overlaps(IEnumerable<TItem> other)
    {
        if (other is OrderedListSet<TItem> ols)
            return _items.Overlaps(ols);
        if (other is FrozenOrderedListSet<TItem> fols)
            return _items.Overlaps(fols);

        return _items.Overlaps(other);
    }

    public bool Overlaps(OrderedListSet<TItem> other)
    {
        return _items.Overlaps(other);
    }

    public bool Overlaps(FrozenOrderedListSet<TItem> other)
    {
        return _items.Overlaps(other._items);
    }
    #endregion

    #region SetEquals
    public bool SetEquals(IEnumerable<TItem> other)
    {
        if (other is OrderedListSet<TItem> ols)
            return _items.SetEquals(ols);
        if (other is FrozenOrderedListSet<TItem> fols)
            return _items.SetEquals(fols);

        return _items.SetEquals(other);
    }

    public bool SetEquals(OrderedListSet<TItem> other)
    {
        return _items.SetEquals(other);
    }

    public bool SetEquals(FrozenOrderedListSet<TItem> other)
    {
        return _items.SetEquals(other._items);
    }
    #endregion
}