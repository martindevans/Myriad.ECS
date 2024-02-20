using System.Collections;

namespace Myriad.ECS.Collections;

/// <summary>
/// A set built out of an ordered list. This allowed allocation free numeration of the set.
/// </summary>
/// <typeparam name="TItem"></typeparam>
public class OrderedListSet<TItem>
    : IReadOnlySet<TItem>
    where TItem : IComparable<TItem>, IEquatable<TItem>
{
    private readonly List<TItem> _items = [ ];
    public int Count => _items.Count;

    #region constructors
    public OrderedListSet()
    {
    }

    public OrderedListSet(IReadOnlySet<TItem> items)
    {
        _items.AddRange(items);
        _items.Sort();
    }

    public OrderedListSet(OrderedListSet<TItem> items)
    {
        _items = items._items.ToList();
    }
    #endregion

    public bool Add(TItem item)
    {
        var index = _items.BinarySearch(item);
        if (index >= 0)
            return false;

        _items.Insert(~index, item);
        return true;
    }

    public void UnionWith<TSet>(TSet items)
        where TSet : IReadOnlySet<TItem>
    {
        if (_items.Count == 0)
        {
            _items.AddRange(items);
            _items.Sort();
        }
        else
        {
            foreach (var item in items)
                Add(item);
        }
    }

    public bool Remove(TItem item)
    {
        var index = _items.BinarySearch(item);
        if (index < 0)
            return false;

        _items.RemoveAt(index);
        return true;
    }

    public void Clear()
    {
        _items.Clear();
    }

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
        var index = _items.BinarySearch(item);
        return index >= 0;
    }

    public bool IsProperSubsetOf(IEnumerable<TItem> other)
    {
        throw new NotImplementedException();
    }

    public bool IsProperSupersetOf(IEnumerable<TItem> other)
    {
        throw new NotImplementedException();
    }

    public bool IsSubsetOf(IEnumerable<TItem> other)
    {
        throw new NotImplementedException();
    }

    public bool IsSupersetOf(IEnumerable<TItem> other)
    {
        foreach (var item in other)
            if (_items.BinarySearch(item) < 0)
                return false;
        return true;
    }

    public bool Overlaps(IEnumerable<TItem> other)
    {
        foreach (var item in other)
            if (Contains(item))
                return true;

        return false;
    }

    public bool Overlaps<TList>(TList other)
        where TList : IEnumerable<TItem>
    {
        foreach (var item in other)
            if (Contains(item))
                return true;

        return false;
    }

    public bool SetEquals(IEnumerable<TItem> other)
    {
        // Ensure every item in other is in set
        var count = 0;
        foreach (var item in other)
        {
            count++;
            if (_items.BinarySearch(item) < 0)
                return false;
        }

        // Check that there are exactly the same number of items in both
        if (Count != count)
            return false;

        return true;
    }

    public bool SetEquals(OrderedListSet<TItem> other)
    {
        if (Count != other.Count)
            return false;

        for (var i = other._items.Count - 1; i >= 0; i--)
            if (!_items[i].Equals(other._items[i]))
                return false;

        return true;
    }
}