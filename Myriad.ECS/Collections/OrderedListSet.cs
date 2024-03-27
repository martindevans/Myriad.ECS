using System.Collections;
using System.Runtime.InteropServices;
using Myriad.ECS.Extensions;
using Myriad.ECS.IDs;

namespace Myriad.ECS.Collections;

/// <summary>
/// A set built out of an ordered list. This allows allocation free enumeration of the set.
/// </summary>
/// <typeparam name="TItem"></typeparam>
internal class OrderedListSet<TItem>
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
        _items.EnsureCapacity(items.Count);
        foreach (var item in items)
            Add(item);
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

    public OrderedListSet(FrozenOrderedListSet<TItem> items)
    {
        _items.EnsureCapacity(items.Count);
        foreach (var item in items)
            _items.Add(item);
    }
    #endregion

    internal IReadOnlyCollection<TItem> LINQ()
    {
        return _items;
    }

    #region add
    public bool Add(TItem item)
    {
        var index = _items.BinarySearch(item);
        if (index >= 0)
            return false;

        _items.Insert(~index, item);
        return true;
    }
    #endregion

    #region unionwith
    internal void UnionWith(FrozenOrderedListSet<TItem> items)
    {
        if (_items.Count == 0)
        {
            _items.EnsureCapacity(items.Count);
            foreach (var item in items)
                _items.Add(item);
        }
        else
        {
            _items.EnsureCapacity(_items.Count + items.Count);
            foreach (var item in items)
                Add(item);
        }
    }
    #endregion

    public void IntersectWith(FrozenOrderedListSet<TItem> other)
    {
        for (var i = _items.Count - 1; i >= 0; i--)
            if (!other.Contains(_items[i]))
                _items.RemoveAt(i);
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

    public FrozenOrderedListSet<TItem> Freeze()
    {
        return new FrozenOrderedListSet<TItem>(this);
    }

    #region GetEnumerator
    public List<TItem>.Enumerator GetEnumerator()
    {
        return _items.GetEnumerator();
    }
    #endregion

    public bool Contains(TItem item)
    {
        return _items.BinarySearch(item) >= 0;
    }

    //todo: other set methods when needed

    //public bool IsProperSubsetOf(IEnumerable<TItem> other)
    //{
    //    throw new NotImplementedException();
    //}

    //public bool IsProperSubsetOf(OrderedListSet<TItem> other)
    //{
    //    throw new NotImplementedException();
    //}

    //public bool IsProperSupersetOf(IEnumerable<TItem> other)
    //{
    //    throw new NotImplementedException();
    //}

    //public bool IsProperSupersetOf(OrderedListSet<TItem> other)
    //{
    //    throw new NotImplementedException();
    //}

    //public bool IsSubsetOf(IEnumerable<TItem> other)
    //{
    //    throw new NotImplementedException();
    //}

    //public bool IsSubsetOf(OrderedListSet<TItem> other)
    //{
    //    throw new NotImplementedException();
    //}

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

#if NET6_0_OR_GREATER
        var a = CollectionsMarshal.AsSpan(_items);
        var b = CollectionsMarshal.AsSpan(other._items);
        return a.SequenceEqual(b);
#else
        for (var i = other._items.Count - 1; i >= 0; i--)
            if (!_items[i].Equals(other._items[i]))
                return false;

        return true;
#endif
    }
#endregion

    #region LINQ
    internal TItem Single()
    {
        return _items.Single();
    }
    #endregion
}