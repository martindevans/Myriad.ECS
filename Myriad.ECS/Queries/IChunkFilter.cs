namespace Myriad.ECS.Queries;

/// <summary>
/// Allows filtering out of chunks in a query, skipping processing
/// </summary>
public interface IChunkFilter
{
    /// <summary>
    /// Determine if the given chunk should be excluded from the query
    /// </summary>
    /// <param name="handle"></param>
    /// <returns></returns>
    public bool Exclude(in ChunkHandle handle);
}

/// <summary>
/// Default filter, exclude no chunks.
/// </summary>
public readonly struct ExcludeNoneFilter
    : IChunkFilter
{
    /// <inheritdoc />
    public bool Exclude(in ChunkHandle handle)
    {
        return false;
    }
}