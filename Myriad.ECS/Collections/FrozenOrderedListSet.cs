using System.Collections;

namespace Myriad.ECS.Collections;

public class FrozenOrderedListSet<TItem>
    : IReadOnlySet<TItem>
    where TItem : IComparable<TItem>, IEquatable<TItem>
{
    private readonly OrderedListSet<TItem> _items;
    public int Count => _items.Count;

    #region constructors
    internal FrozenOrderedListSet()
    {
        _items = [ ];
    }

    internal FrozenOrderedListSet(List<TItem> items)
    {
        _items = new OrderedListSet<TItem>(items);
    }

    internal FrozenOrderedListSet(HashSet<TItem> items)
    {
        _items = new OrderedListSet<TItem>(items);
    }

    internal FrozenOrderedListSet(IReadOnlySet<TItem> items)
    {
        _items = new OrderedListSet<TItem>(items);
    }

    internal FrozenOrderedListSet(OrderedListSet<TItem> items)
    {
        _items = new OrderedListSet<TItem>(items);
    }

    internal FrozenOrderedListSet(FrozenOrderedListSet<TItem> items)
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
        return other switch
        {
            OrderedListSet<TItem> ols => _items.IsProperSubsetOf(ols),
            FrozenOrderedListSet<TItem> fols => _items.IsProperSubsetOf(fols),
            _ => _items.IsProperSubsetOf(other)
        };
    }

    internal bool IsProperSubsetOf(OrderedListSet<TItem> other)
    {
        return _items.IsProperSubsetOf(other);
    }

    public bool IsProperSubsetOf(FrozenOrderedListSet<TItem> other)
    {
        return _items.IsProperSubsetOf(other._items);
    }
    #endregion

    #region IsProperSupersetOf
    bool IReadOnlySet<TItem>.IsProperSupersetOf(IEnumerable<TItem> other)
    {
        return other switch
        {
            OrderedListSet<TItem> ols => _items.IsProperSupersetOf(ols),
            FrozenOrderedListSet<TItem> fols => _items.IsProperSupersetOf(fols),
            _ => _items.IsProperSupersetOf(other)
        };
    }

    internal bool IsProperSupersetOf(OrderedListSet<TItem> other)
    {
        return _items.IsProperSupersetOf(other);
    }

    public bool IsProperSupersetOf(FrozenOrderedListSet<TItem> other)
    {
        return _items.IsProperSupersetOf(other._items);
    }
    #endregion

    #region IsSubsetOf
    bool IReadOnlySet<TItem>.IsSubsetOf(IEnumerable<TItem> other)
    {
        return other switch
        {
            OrderedListSet<TItem> ols => _items.IsSubsetOf(ols),
            FrozenOrderedListSet<TItem> fols => _items.IsSubsetOf(fols),
            _ => _items.IsSubsetOf(other)
        };
    }

    internal bool IsSubsetOf(OrderedListSet<TItem> other)
    {
        return _items.IsSubsetOf(other);
    }

    public bool IsSubsetOf(FrozenOrderedListSet<TItem> other)
    {
        return _items.IsSubsetOf(other._items);
    }
    #endregion

    #region IsSupersetOf
    bool IReadOnlySet<TItem>.IsSupersetOf(IEnumerable<TItem> other)
    {
        return other switch
        {
            OrderedListSet<TItem> ols => _items.IsSupersetOf(ols),
            FrozenOrderedListSet<TItem> fols => _items.IsSupersetOf(fols),
            _ => _items.IsSupersetOf(other)
        };
    }

    internal bool IsSupersetOf(OrderedListSet<TItem> other)
    {
        return _items.IsSupersetOf(other);
    }

    public bool IsSupersetOf(FrozenOrderedListSet<TItem> other)
    {
        return _items.IsSupersetOf(other._items);
    }
    #endregion

    #region Overlaps
    bool IReadOnlySet<TItem>.Overlaps(IEnumerable<TItem> other)
    {
        return other switch
        {
            OrderedListSet<TItem> ols => _items.Overlaps(ols),
            FrozenOrderedListSet<TItem> fols => _items.Overlaps(fols),
            _ => _items.Overlaps(other)
        };
    }

    internal bool Overlaps(OrderedListSet<TItem> other)
    {
        return _items.Overlaps(other);
    }

    public bool Overlaps(FrozenOrderedListSet<TItem> other)
    {
        return _items.Overlaps(other._items);
    }
    #endregion

    #region SetEquals
    bool IReadOnlySet<TItem>.SetEquals(IEnumerable<TItem> other)
    {
        return other switch
        {
            OrderedListSet<TItem> ols => _items.SetEquals(ols),
            FrozenOrderedListSet<TItem> fols => _items.SetEquals(fols),
            _ => _items.SetEquals(other)
        };
    }

    internal bool SetEquals(OrderedListSet<TItem> other)
    {
        return _items.SetEquals(other);
    }

    public bool SetEquals(FrozenOrderedListSet<TItem> other)
    {
        return _items.SetEquals(other._items);
    }
    #endregion
}