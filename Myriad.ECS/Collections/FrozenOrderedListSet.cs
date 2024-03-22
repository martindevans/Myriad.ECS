using System.Collections;

namespace Myriad.ECS.Collections;

public class FrozenOrderedListSet<TItem>
    where TItem : IComparable<TItem>, IEquatable<TItem>
{
    private readonly OrderedListSet<TItem> _items;
    public int Count => _items.Count;

    #region constructors
    internal FrozenOrderedListSet()
    {
        _items = new();
    }

    internal FrozenOrderedListSet(List<TItem> items)
    {
        _items = new OrderedListSet<TItem>(items);
    }

    internal FrozenOrderedListSet(HashSet<TItem> items)
    {
        _items = new OrderedListSet<TItem>(items);
    }

    internal FrozenOrderedListSet(OrderedListSet<TItem> items)
    {
        _items = new OrderedListSet<TItem>(items);
    }

    internal FrozenOrderedListSet(FrozenOrderedListSet<TItem> items)
    {
        _items = new OrderedListSet<TItem>(items._items);
    }
    #endregion

    internal IReadOnlyCollection<TItem> LINQ()
    {
        return _items.LINQ();
    }

    #region GetEnumerator
    public List<TItem>.Enumerator GetEnumerator()
    {
        return _items.GetEnumerator();
    }
    #endregion

    public bool Contains(TItem item)
    {
        return _items.Contains(item);
    }

    #region IsProperSubsetOf
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