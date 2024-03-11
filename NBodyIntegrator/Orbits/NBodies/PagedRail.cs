using Myriad.ECS;
using NBodyIntegrator.Units;

namespace NBodyIntegrator.Orbits.NBodies;

/// <summary>
/// A orbital rail made out of "pages" of data.
/// </summary>
public struct PagedRail
    : IComponent
{
    /// <summary>
    /// A list of all pages in this rail
    /// </summary>
    public List<OrbitRailPage> Pages;

    /// <summary>
    /// Incremented every time a page is added or removed
    /// </summary>
    public ulong Epoch;

    /// <summary>
    /// Total duration from first data point to the last
    /// </summary>
    public readonly Seconds Duration
    {
        get
        {
            if (Pages.Count == 0)
                return new Seconds(0);

            var a = Pages[0].GetSpanTimes()[0];
            var b = Pages[^1].GetSpanTimes()[^1];
            return new Seconds(b - a);
        }
    }

    /// <summary>
    /// Get the last state in the rail
    /// </summary>
    /// <returns></returns>
    public readonly (Metre3 pos, Metre3 vel, double time) LastState()
    {
        var page = Pages[^1];
        return (
            page.GetSpanPositions()[^1],
            page.GetSpanVelocities()[^1],
            page.GetSpanTimes()[^1]
        );
    }
}

/// <summary>
/// A "page" of data in an orbit rail
/// </summary>
public class OrbitRailPage
{
    /// <summary>
    /// Maximum size of a page (some pages may be smaller)
    /// </summary>
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