using Myriad.ECS;
using Myriad.ECS.Allocations;
using NBodyIntegrator.Units;

namespace NBodyIntegrator.Orbits.NBodies;

public struct PagedRail2
{
    /// <summary>
    /// A list of all pages in this rail
    /// </summary>
    public List<Page> Pages;

    /// <summary>
    /// The total number of items in pages
    /// </summary>
    public int ItemCount;

    /// <summary>
    /// Incremented every time a page is added or removed
    /// </summary>
    public ulong Epoch;
}

/// <summary>
/// Stores data in a form specifically designed for efficient use by nbody orbit rails. Only ever
/// add to end and remove from start. Data is stored in pages, which are returned to a pool once used.
/// </summary>
public class PagedRail
    : IComponent
{
    #region fields and properties
    private readonly LocalPool<Page> _pool = new(2);
    private readonly List<Page> _pages = [ ];

    /// <summary>
    /// Total count of items in the rail
    /// </summary>
    public int ItemCount { get; private set; }

    /// <summary>
    /// Incremented every time a page is added or removed
    /// </summary>
    public ulong Epoch { get; private set; }

    /// <summary>
    /// Total duration from first data point to the last
    /// </summary>
    public Seconds Duration
    {
        get
        {
            if (ItemCount < 2)
                return new Seconds(0);

            var a = _pages[0].GetSpanTimes()[0];
            var b = _pages[^1].GetSpanTimes()[^1];
            return new Seconds(b - a);
        }
    }
    #endregion

    public PagedRail(Metre3 position, Metre3 velocity, double time)
    {
        // Create a new page
        var page = _pool.Get();
        page.Init(1, Epoch++);
        _pages.Add(page);

        // Add that single datapoint to it
        page.GetSpanPositions()[0] = position;
        page.GetSpanVelocities()[0] = velocity;
        page.GetSpanTimes()[0] = time;
        ItemCount++;
    }

    /// <summary>
    /// Create a new page and mark it as full of data.
    /// </summary>
    /// <returns>The span for this page, which _must_ be filled with data</returns>
    public MutStateSpanTuple AddPage()
    {
        // Create a new page
        var page = _pool.Get();
        page.Init(Page.PageSize, Epoch++);
        _pages.Add(page);

        // Act as if this page is full of data (because it's about to be)
        ItemCount += Page.PageSize;

        return new MutStateSpanTuple
        {
            Positions = page.GetSpanPositions(),
            Velocities = page.GetSpanVelocities(),
            Timestamps = page.GetSpanTimes(),
        };
    }

    /// <summary>
    /// Remove the entire first page if it can be removed.
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public bool TryTrimStart(double time)
    {
        if (_pages.Count < 2)
            return false;

        // Get the first and second page
        var p0 = _pages[0];
        var p0span = _pages[0].GetSpanTimes();
        var p1span = _pages[1].GetSpanTimes();

        // We need all the points in page 0 to be in the past _and_ at least one point in the next page
        // to be in the past before we can remove a page!
        if (p0span[^1] > time || p1span[0] > time)
            return false;

        // Remove and recycle first page
        _pages.RemoveAt(0);
        _pool.Return(p0);
        return true;
    }

    /// <summary>
    /// Get the last state in the rail
    /// </summary>
    /// <returns></returns>
    public (Metre3 pos, Metre3 vel, double time) LastState()
    {
        var page = _pages[^1];
        return (
            page.GetSpanPositions()[^1],
            page.GetSpanVelocities()[^1],
            page.GetSpanTimes()[^1]
        );
    }

    #region GetEnumerator
    public PageSpanEnumerator GetEnumerator()
    {
        return new PageSpanEnumerator(this);
    }

    public struct PageSpanEnumerator(PagedRail pagedRail)
    {
        private int _page = -1;

        public readonly StateSpanTuple Current => new()
        {
            Positions = pagedRail._pages[_page].GetSpanPositions(),
            Velocities = pagedRail._pages[_page].GetSpanVelocities(),
            Timestamps = pagedRail._pages[_page].GetSpanTimes(),
        };

        public bool MoveNext()
        {
            _page++;

            if (_page >= pagedRail._pages.Count)
                return false;

            return true;
        }
    }

    public ref struct StateSpanTuple
    {
        public ReadOnlySpan<Metre3> Positions;
        public ReadOnlySpan<Metre3> Velocities;
        public ReadOnlySpan<double> Timestamps;
    }

    public ref struct MutStateSpanTuple
    {
        public Span<Metre3> Positions;
        public Span<Metre3> Velocities;
        public Span<double> Timestamps;
    }
    #endregion

    /// <summary>
    /// Count how many items are before the given timestamp
    /// </summary>
    /// <param name="timestamp"></param>
    /// <returns></returns>
    private int CountItemsBefore(double timestamp)
    {
        var counter = 0;
        foreach (var page in _pages)
        {
            foreach (var item in page.GetSpanTimes())
            {
                if (item < timestamp)
                    counter++;
                else
                    return counter;
            }
        }

        return counter;
    }

    /// <summary>
    /// Keep all items before the given timestamp (discard all others)
    /// </summary>
    /// <param name="timestamp"></param>
    public void KeepBefore(double timestamp)
    {
        var keep = CountItemsBefore(timestamp);

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
            _pool.Return(last);
            discard--;
        }

        // Update final item count
        ItemCount = keep;
    }

    
}

public class Page
{
    public const int PageSize = 256;

    private readonly Metre3[] _dataPositions = new Metre3[PageSize];
    private readonly Metre3[] _dataVelocities = new Metre3[PageSize];
    private readonly double[] _dataTimestamps = new double[PageSize];

    private int _first;
    private int _end;

    public int Count => _end - _first;
    public ulong ID { get; private set; }

    public void Init(int count, ulong id)
    {
        if (count == 0)
            throw new ArgumentOutOfRangeException(nameof(count), "Cannot init a page with zero items");
        if (count > PageSize)
            throw new ArgumentOutOfRangeException(nameof(count), "Cannot init a page with zero items");

        Array.Clear(_dataPositions);
        Array.Clear(_dataVelocities);
        Array.Clear(_dataTimestamps);

        _first = 0;
        _end = count;
        ID = id;
    }

    public Span<Metre3> GetSpanPositions()
    {
        return _dataPositions.AsSpan(_first, Count);
    }

    public Span<Metre3> GetSpanVelocities()
    {
        return _dataVelocities.AsSpan(_first, Count);
    }

    public Span<double> GetSpanTimes()
    {
        return _dataTimestamps.AsSpan(_first, Count);
    }

    public void Keep(int keep)
    {
        if (keep > Count)
            throw new ArgumentOutOfRangeException(nameof(keep), "Cannot keep more items than count");
        if (_first + keep > PageSize)
            throw new ArgumentOutOfRangeException(nameof(keep), "Cannot keep more items than data page size");

        _end = _first + keep;
    }
}