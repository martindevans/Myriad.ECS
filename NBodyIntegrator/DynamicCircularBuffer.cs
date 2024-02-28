namespace NBodyIntegrator;

/// <summary>
/// Manages a circular buffer of buffer elements
/// </summary>
public struct DynamicCircularBuffer
{
    /// <summary>
    /// Number of items in the circular buffer
    /// </summary>
    public int Count { get; private set; }

    /// <summary>
    /// Index of the first item in the buffer
    /// </summary>
    public int Start { get; private set; }

    /// <summary>
    /// Index one past the last item in the buffer
    /// </summary>
    public int End { get; private set; }

    /// <summary>
    /// Incremented every time the rail is modified (may overflow)
    /// </summary>
    public uint Epoch { get; private set; }

    public bool IsFull(int bufferLength)
    {
        return bufferLength == Count;
    }

    /// <summary>
    /// Return the index to assign a new item to the buffer
    /// </summary>
    /// <returns>The index to write data into, or null if there is no space in the buffer</returns>
    public int? TryAdd(int bufferLength)
    {
        if (IsFull(bufferLength))
            return null;

        // Insert the new item
        var idx = End;
        End = (End + 1) % bufferLength;
        Count++;

        unchecked
        {
            Epoch++;
        }

        return idx;
    }

    /// <summary>
    /// Remove the item from the start of the buffer and return the index of the item just returned
    /// </summary>
    /// <param name="bufferLength"></param>
    /// <returns></returns>
    public int? TryRemove(int bufferLength)
    {
        if (Count == 0)
            return default;

        var idx = Start;
        Start = (Start + 1) % bufferLength;
        Count--;

        unchecked
        {
            Epoch++;
        }

        return idx;
    }

    /// <summary>
    /// Get the index of the item at a given circular index
    /// </summary>
    /// <param name="bufferLength"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public readonly int? IndexAt(int bufferLength, int index)
    {
        if (index < 0)
            return default;
        if (index >= Count)
            return default;

        return (Start + index) % bufferLength;
    }
}