namespace Myriad.ECS.Extensions;

internal static class ReorderBuffer
{
    /// <summary>
    /// Apply a re-order in place using a data mover. The mover encapsulates how to move data, the indices
    /// specify where each index is moving to.
    /// </summary>
    /// <typeparam name="TMove"></typeparam>
    /// <typeparam name="TDataIndex"></typeparam>
    /// <param name="mover"></param>
    /// <param name="indices"></param>
    public static void ApplyReorderInPlace<TMove, TDataIndex>(this TMove mover, Span<TDataIndex> indices)
        where TMove : struct, IDataMove
        where TDataIndex : struct, IDataIndex
    {
        Span<bool> visited = stackalloc bool[indices.Length];
        visited.Clear();
        
        for (var i = 0; i < indices.Length; i++)
        {
            // Check if this index has already been visited/processed
            if (visited[i])
                continue;

            // Start a new cycle
            var currentIdx = i;

            // Save this item to temp
            mover.SaveToTemporary(i);

            // Follow the cycle
            while (true)
            {
                var nextIdx = indices[currentIdx].OriginalIndex;

                // Mark the current index as visited
                visited[currentIdx] = true;

                // If the cycle loops back to the start, insert the saved element
                if (nextIdx == i)
                {
                    mover.RestoreFromTemporary(currentIdx);
                    break;
                }

                // Move the element to its correct position
                mover.Move(nextIdx, currentIdx);
                currentIdx = nextIdx;
            }
        }
    }

    public interface IDataMove
    {
        /// <summary>
        /// Move data from one index to another
        /// </summary>
        /// <param name="indexFrom"></param>
        /// <param name="indexTo"></param>
        void Move(int indexFrom, int indexTo);

        /// <summary>
        /// Save data at the given index to the temporary slot. Only one thing will
        /// ever be placed into temporary storage at a time.
        /// </summary>
        /// <param name="indexFrom"></param>
        void SaveToTemporary(int indexFrom);

        /// <summary>
        /// Restore from temporary storage to the given index.
        /// </summary>
        /// <param name="indexTo"></param>
        void RestoreFromTemporary(int indexTo);
    }

    public interface IDataIndex
    {
        /// <summary>
        /// Get the original index of this data
        /// </summary>
        int OriginalIndex { get; }
    }
}