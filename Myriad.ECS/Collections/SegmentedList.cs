namespace Myriad.ECS.Collections;

/// <summary>
/// A list which stores data in "segments", this removes the need for copying data when the list grows.
/// </summary>
/// <typeparam name="TItem"></typeparam>
internal class SegmentedList<TItem>
{
    /// <summary>
    /// How many items are stored within a single segment
    /// </summary>
    public int SegmentCapacity { get; }

    /// <summary>
    /// Total capacity in all segments
    /// </summary>
    public int TotalCapacity => SegmentCapacity * _segments.Count;

    private readonly List<TItem[]> _segments = [];

    public SegmentedList(int segmentCapacity)
    {
        SegmentCapacity = segmentCapacity;
        Grow();
    }

    /// <summary>
    /// Get the item with the given index (mutable)
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public ref TItem this[int index]
    {
        get
        {
            var (rowIndex, segment) = GetSegment(index);
            return ref segment[rowIndex];
        }
    }

    /// <summary>
    /// Get the segment and index within the segment for the item with the given index
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    /// <exception cref="IndexOutOfRangeException"></exception>
    private (int, TItem[]) GetSegment(int index)
    {
        var segIndex = index / SegmentCapacity;
        var rowIndex = index - segIndex * SegmentCapacity;

        return (rowIndex, _segments[segIndex]);
    }

    /// <summary>
    /// Add another segment
    /// </summary>
    public void Grow()
    {
        _segments.Add(new TItem[SegmentCapacity]);
    }
}