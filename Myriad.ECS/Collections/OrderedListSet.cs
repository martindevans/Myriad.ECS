using System.Collections;
using Myriad.ECS.Extensions;

namespace Myriad.ECS.Collections;

/// <summary>
/// A set built out of an ordered list. This allows allocation free enumeration of the set.
/// </summary>
/// <typeparam name="TItem"></typeparam>
internal class OrderedListSet<TItem>
    : IReadOnlySet<TItem>
    where TItem : IComparable<TItem>, IEquatable<TItem>
{
    private readonly List<TItem> _items = [ ];
    public int Count => _items.Count;

    #region constructors
    public OrderedListSet()
    {
    }

    // ReSharper disable once ParameterTypeCanBeEnumerable.Local
    public OrderedListSet(List<TItem> items)
    {
        _items = [..items];
    }

    // ReSharper disable once ParameterTypeCanBeEnumerable.Local (Justification: the fact this is a set is important, it means there are definitely no duplicates)
    internal OrderedListSet(IReadOnlySet<TItem> items)
    {
        _items.AddRange(items);
        _items.Sort();
    }

    internal OrderedListSet(HashSet<TItem> items)
    {
        _items.AddRange(items);
        _items.Sort();
    }

    public OrderedListSet(OrderedListSet<TItem> items)
    {
        _items = [..items._items];
    }
    #endregion

    #region mutate
    public bool Add(TItem item)
    {
        var index = _items.BinarySearch(item);
        if (index >= 0)
            return false;

        _items.Insert(~index, item);
        return true;
    }

    internal void UnionWith<TSet>(TSet items)
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
        return _items.BinarySearch(item) >= 0;
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

    #region IsSupersetOf
    public bool IsSupersetOf(IEnumerable<TItem> other)
    {
        if (other.TryGetNonEnumeratedCount(out var otherCount) && otherCount > Count)
            return false;

        foreach (var item in other)
            if (_items.BinarySearch(item) < 0)
                return false;

        return true;
    }

    public bool IsSupersetOf(OrderedListSet<TItem> other)
    {
        if (other.Count > Count)
            return false;

        foreach (var item in other._items)
            if (_items.BinarySearch(item) < 0)
                return false;

        return true;
    }
    #endregion

    #region Overlaps
    public bool Overlaps(IEnumerable<TItem> other)
    {
        if (Count == 0)
            return false;
        if (other.TryGetNonEnumeratedCount(out var count) && count == 0)
            return false;

        foreach (var item in other)
            if (Contains(item))
                return true;

        return false;
    }

    public bool Overlaps(OrderedListSet<TItem> other)
    {
        if (Count == 0)
            return false;
        if (other.Count == 0)
            return false;

        foreach (var item in other._items)
            if (Contains(item))
                return true;

        return false;
    }
    #endregion

    #region SetEquals
    public bool SetEquals(IEnumerable<TItem> other)
    {
        // Try to get the count, if possible. This allows a possible early exit without any work.
        if (other.TryGetNonEnumeratedCount(out var count))
            if (Count != count)
                return false;

        // Ensure every item in other is in set
        count = 0;
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
    #endregion
}