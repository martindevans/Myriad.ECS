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

    public FrozenOrderedListSet(IReadOnlySet<TItem> items)
    {
        _items = new OrderedListSet<TItem>(items);
    }

    public FrozenOrderedListSet(OrderedListSet<TItem> items)
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
        return _items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    #endregion

    public bool Contains(TItem item)
    {
        return _items.Contains(item);
    }

    public bool IsProperSubsetOf(IEnumerable<TItem> other)
    {
        return _items.IsProperSubsetOf(other);
    }

    public bool IsProperSupersetOf(IEnumerable<TItem> other)
    {
        return _items.IsProperSupersetOf(other);
    }

    public bool IsSubsetOf(IEnumerable<TItem> other)
    {
        return _items.IsSubsetOf(other);
    }

    public bool IsSupersetOf(IEnumerable<TItem> other)
    {
        return _items.IsSupersetOf(other);
    }

    public bool Overlaps(IEnumerable<TItem> other)
    {
        return _items.Overlaps(other);
    }

    public bool SetEquals(IEnumerable<TItem> other)
    {
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
}