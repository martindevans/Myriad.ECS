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
    private readonly LocalPool<Page> _pool = new(2);
    private readonly List<Page> _pages = [ ];

    public int Count { get; private set; }

    public void Add(TData item)
    {
        if (_pages.Count == 0 || _pages[^1].IsCursorAtEnd())
        {
            var p = _pool.Get();
            p.Clear();
            _pages.Add(p);
        }

        _pages[^1].Add(item);

        Count++;
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

        Count--;
    }

    public TData First()
    {
        return _pages[0].First();
    }

    public TData Second()
    {
        var p0 = _pages[0];
        if (p0.Count > 1)
            return p0.Second();

        var p1 = _pages[1];
        return p1.First();
    }

    public TData Last()
    {
        return _pages[^1].Last();
    }

    private class Page
    {
        public const int PageSize = 128;

        private readonly TData[] _data = new TData[PageSize];

        private int _first;
        private int _end;
        private int _count;

        public int Count => _count;

        public void Clear()
        {
            _first = 0;
            _end = 0;
            _count = 0;
        }

        public bool IsCursorAtEnd()
        {
            return _end == _data.Length;
        }

        public bool IsEmpty()
        {
            return _count == 0;
        }

        public void Add(TData item)
        {
            if (IsCursorAtEnd())
                throw new InvalidOperationException("Cannot add to full page");

            _data[_end++] = item;
            _count++;
        }

        public void RemoveFirst()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Cannot remove from empty page");

            _first++;
            _count--;
        }

        public TData First()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Cannot get first from empty page");

            return _data[_first];
        }

        public TData Second()
        {
            if (Count < 2)
                throw new InvalidOperationException("Cannot get second from page with less than 2 items");

            return _data[_first + 1];
        }

        public TData Last()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Cannot get first from empty page");

            return _data[_end - 1];
        }
    }
}