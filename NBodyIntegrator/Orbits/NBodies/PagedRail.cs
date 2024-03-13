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
    public readonly RailPoint LastState()
    {
        var page = Pages[^1];
        return new(
            page.GetSpanPositions()[^1],
            page.GetSpanVelocities()[^1],
            page.GetSpanTimes()[^1]
        );
    }

    /// <summary>
    /// Search for the two states either side of the given point in time
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public (RailPoint before, RailPoint after)? NearestStates(double time)
    {
        for (var i = 0; i < Pages.Count; i++)
        {
            var page = Pages[i];
            var start = page.GetSpanTimes()[0];
            var end = page.GetSpanTimes()[^1];

            // Check if the relevant datapoint is somewhere in this page
            if (time >= start && time < end)
            {
                for (var j = 1; j < page.Count; j++)
                {
                    var a = page.GetSpanTimes()[j - 1];
                    var b = page.GetSpanTimes()[j];

                    if (time >= a && time < b)
                        return (page.GetState(j - 1), page.GetState(j));
                }

                return null;
            }

            // Maybe the relevant points are the last point in this page, and the first point in the next page
            if (i < Pages.Count - 1)
            {
                var nextPage = Pages[i + 1];
                var nextFirst = nextPage.GetSpanTimes()[0];

                if (time >= end && time < nextFirst)
                {
                    return (
                        page.GetState(^1),
                        nextPage.GetState(0)
                    );
                }
            }
        }

        return null;
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
    public const int PageSize = 64;

    private readonly Metre3[] _dataPositions = new Metre3[PageSize];
    private readonly Metre3[] _dataVelocities = new Metre3[PageSize];
    private readonly double[] _dataTimestamps = new double[PageSize];

    public int Count { get; set; }
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

        Count = count;
        ID = id;
    }

    public Span<Metre3> GetSpanPositions()
    {
        return _dataPositions.AsSpan(0, Count);
    }

    public Span<Metre3> GetSpanVelocities()
    {
        return _dataVelocities.AsSpan(0, Count);
    }

    public Span<double> GetSpanTimes()
    {
        return _dataTimestamps.AsSpan(0, Count);
    }

    public void Keep(int keep)
    {
        if (keep > Count)
            throw new ArgumentOutOfRangeException(nameof(keep), "Cannot keep more items than count");
        if (keep > PageSize)
            throw new ArgumentOutOfRangeException(nameof(keep), "Cannot keep more items than data page size");

        Count = keep;
    }

    public RailPoint GetState(Index index)
    {
        var idx = index.GetOffset(Count);
        if (idx < 0 || idx >= Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        return new RailPoint(
            _dataPositions[idx],
            _dataVelocities[idx],
            _dataTimestamps[idx]
        );
    }
}

public readonly record struct RailPoint(Metre3 Position, Metre3 Velocity, double Time);