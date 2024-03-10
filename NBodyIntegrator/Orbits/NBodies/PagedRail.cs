using Myriad.ECS;
using Myriad.ECS.Allocations;

namespace NBodyIntegrator.Orbits.NBodies;

/// <summary>
/// Stores data in a form specifically designed for efficient use by nbody orbit rails. Only ever
/// add to end and remove from start. Data is stored in pages, which are returned to a pool once used.
/// </summary>
/// <typeparam name="TData"></typeparam>
public class PagedRail<TData>
    : IComponent
{
    public const int PageSize = 1024;

    private readonly LocalPool<Page> _pool = new(2);
    private readonly List<Page> _pages = [ ];

    /// <summary>
    /// Total count of items in the rail
    /// </summary>
    public int ItemCount { get; private set; }

    /// <summary>
    /// Incremented every time a page is added
    /// </summary>
    public ulong PageEpoch { get; private set; }

    public void Add(TData item)
    {
        if (_pages.Count == 0 || _pages[^1].IsCursorAtEnd())
        {
            var p = _pool.Get();
            p.Clear();
            p.ID = PageEpoch++;
            _pages.Add(p);
        }

        _pages[^1].Add(item);

        ItemCount++;
    }

    public void RemoveFirst()
    {
        if (_pages.Count == 0)
            return;

        var p = _pages[0];
        p.RemoveFirst();

        if (p.IsEmpty())
        {
            _pages.RemoveAt(0);
            _pool.Return(p);
        }

        ItemCount--;
    }

    public TData First()
    {
        return _pages[0][0];
    }

    public TData Second()
    {
        var p0 = _pages[0];
        if (p0.Count > 1)
            return p0[1];

        var p1 = _pages[1];
        return p1[0];
    }

    public TData Last()
    {
        return _pages[^1][^1];
    }

    private class Page
    {
        private readonly TData[] _data = new TData[PageSize];

        private int _first;
        private int _end;

        public int Count { get; private set; }
        public ulong ID { get; set; }

        public void Clear()
        {
            _first = 0;
            _end = 0;
            Count = 0;
        }

        public bool IsCursorAtEnd()
        {
            return _end == _data.Length;
        }

        public bool IsEmpty()
        {
            return Count == 0;
        }

        public void Add(TData item)
        {
            if (IsCursorAtEnd())
                throw new InvalidOperationException("Cannot add to full page");

            _data[_end++] = item;
            Count++;
        }

        public void RemoveFirst()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Cannot remove from empty page");

            _first++;
            Count--;
        }

        public TData this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                    throw new IndexOutOfRangeException();
                return _data[index + _first];
            }
        }

        public ReadOnlySpan<TData> GetSpan()
        {
            return _data.AsSpan(_first, Count);
        }

        public void Keep(int keep)
        {
            if (keep > Count)
                throw new ArgumentOutOfRangeException(nameof(keep), "Cannot keep more items than count");
            if (_first + keep > _data.Length)
                throw new ArgumentOutOfRangeException(nameof(keep), "Cannot keep more items than data page size");

            Count = keep;
            _end = _first + keep;
        }
    }

    public PageSpanEnumerator GetEnumerator()
    {
        return new PageSpanEnumerator(this);
    }

    public struct PageSpanEnumerator(PagedRail<TData> pagedRail)
    {
        private int _page = -1;

        public readonly ReadOnlySpan<TData> Current => pagedRail._pages[_page].GetSpan();

        public bool MoveNext()
        {
            _page++;

            if (_page >= pagedRail._pages.Count)
                return false;

            return true;
        }
    }

    /// <summary>
    /// Keep counting until the predicate returns false
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public int CountUntilFalse<T>(T value, Func<TData, T, bool> predicate)
    {
        var counter = 0;
        foreach (var page in _pages)
        {
            foreach (var item in page.GetSpan())
            {
                if (predicate(item, value))
                    counter++;
                else
                    return counter;
            }
        }

        return counter;
    }

    /// <summary>
    /// Keep this many data points (discard all others)
    /// </summary>
    /// <param name="keep"></param>
    public void Keep(int keep)
    {
        if (keep > ItemCount)
            throw new ArgumentOutOfRangeException(nameof(keep), "Cannot keep more items than there are");

        var discard = 0;
        var sum = 0;
        for (var i = 0; i < _pages.Count; i++)
        {
            if (_pages[i].Count + sum <= keep)
            {
                // We haven't yet counted up enough items to exceed the "keep" counter, just add to the sum
                sum += _pages[i].Count;
            }
            else
            {
                // This page needs trimming, and every page after it needs discarding
                discard = _pages.Count - i - 1;
                _pages[i].Keep(keep - sum);
                break;
            }
        }

        // Remove however many pages need discarding
        while (discard > 0)
        {
            var last = _pages[^1];
            _pages.RemoveAt(_pages.Count - 1);

            last.Clear();
            _pool.Return(last);

            discard--;
        }

        // Update final item count
        ItemCount = keep;
    }
}