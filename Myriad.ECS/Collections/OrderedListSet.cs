using System.Runtime.InteropServices;
using Myriad.ECS.Extensions;
using Myriad.ECS.IDs;

namespace Myriad.ECS.Collections;

/// <summary>
/// A set built out of an ordered list. This allows allocation free enumeration of the set.
/// </summary>
/// <typeparam name="TItem"></typeparam>
internal class OrderedListSet<TItem>
    where TItem : struct, IComparable<TItem>, IEquatable<TItem>
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

    /// <summary>
    /// Copy this set to a new frozen set
    /// </summary>
    /// <returns></returns>
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
    public bool SetEquals(HashSet<TItem> other)
    {
        // Try to get the count, if possible. This allows a possible early exit without any work.
        if (other.Count != Count)
            return false;

        // Ensure every item in this is in other
        foreach (var item in _items)
            if (!other.Contains(item))
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

        // Add a specialization for ComponentID. This allows it to be compared with fast SIMD equality
        // instead of calling the equality implementation for every item individually.
        if (typeof(TItem) == typeof(ComponentID))
        {
            var aa = MemoryMarshal.Cast<TItem, int>(a);
            var bb = MemoryMarshal.Cast<TItem, int>(b);
            return aa.SequenceEqual(bb);
        }

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
    internal IReadOnlyCollection<TItem> LINQ()
    {
        return _items;
    }

    internal TItem Single()
    {
        return _items.Count switch
        {
            0 => throw new InvalidOperationException("Cannot get single item, there are 0 items"),
            1 => _items[0],
            _ => throw new InvalidOperationException($"Cannot get single item, there are {_items.Count} items"),
        };
    }

    internal bool Any(Func<TItem, bool> predicate)
    {
        foreach (var item in _items)
            if (predicate(item))
                return true;

        return false;
    }
    #endregion
}