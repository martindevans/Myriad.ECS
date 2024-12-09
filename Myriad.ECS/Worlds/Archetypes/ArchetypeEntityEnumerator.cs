using Myriad.ECS.Worlds.Chunks;

namespace Myriad.ECS.Worlds.Archetypes;

/// <summary>
/// Enumerable of all the entities in a single archetype
/// </summary>
public readonly struct ArchetypeEntityEnumerable
{
    /// <summary>
    /// The <see cref="Archetype"/> this enumerable is over
    /// </summary>
    public Archetype Archetype { get; }

    /// <summary>
    /// Create a new enumerable for the given archetype
    /// </summary>
    /// <param name="archetype"></param>
    public ArchetypeEntityEnumerable(Archetype archetype)
    {
        Archetype = archetype;
    }

    /// <summary>
    /// Get an enumerator from this enumerable
    /// </summary>
    /// <returns></returns>
    public ArchetypeEntityEnumerator GetEnumerator()
    {
        return new ArchetypeEntityEnumerator(Archetype);
    }

    internal int Count()
    {
        var count = 0;
        foreach (var _ in this)
            count++;
        return count;
    }
}

/// <summary>
/// Enumerator over the entities in an archetype
/// </summary>
public struct ArchetypeEntityEnumerator
    : IDisposable
{
    private List<Chunk>.Enumerator _chunksEnumerator;
    private int _entityIndex = -1;

    private Chunk? _chunk;

    internal ArchetypeEntityEnumerator(Archetype archetype)
    {
        _chunksEnumerator = archetype.GetChunkEnumerator();
    }

    /// <summary>
    /// Get the current item from this enumerator
    /// </summary>
    public readonly Entity Current => _chunk!.Entities.Span[_entityIndex];

    private bool NextChunk()
    {
        if (!_chunksEnumerator.MoveNext())
            return false;

        _chunk = _chunksEnumerator.Current;
        _entityIndex = 0;
        return true;
    }

    /// <summary>
    /// Move to the next item
    /// </summary>
    /// <returns></returns>
    public bool MoveNext()
    {
        _entityIndex++;
        if (_chunk != null && _entityIndex < _chunk.EntityCount)
            return true;

        if (!NextChunk())
            return false;

        return true;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _chunksEnumerator.Dispose();
    }
}