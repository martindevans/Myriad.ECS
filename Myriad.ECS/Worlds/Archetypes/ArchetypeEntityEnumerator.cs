using System.Collections;
using Myriad.ECS.Worlds.Chunks;

namespace Myriad.ECS.Worlds.Archetypes;

public readonly struct ArchetypeEntityEnumerable
{
    public Archetype Archetype { get; }

    public ArchetypeEntityEnumerable(Archetype archetype)
    {
        Archetype = archetype;
    }

    public ArchetypeEntityEnumerator GetEnumerator()
    {
        return new ArchetypeEntityEnumerator(Archetype);
    }

    internal int Count()
    {
        var count = 0;
        foreach (var entity in this)
            count++;
        return count;
    }
}

public struct ArchetypeEntityEnumerator
{
    private List<Chunk>.Enumerator _chunksEnumerator;
    private int _entityIndex = -1;

    private Chunk? _chunk;

    internal ArchetypeEntityEnumerator(Archetype archetype)
    {
        _chunksEnumerator = archetype.GetChunkEnumerator();
    }

    public readonly Entity Current => _chunk!.Entities[_entityIndex];

    private bool NextChunk()
    {
        if (!_chunksEnumerator.MoveNext())
            return false;

        _chunk = _chunksEnumerator.Current;
        _entityIndex = 0;
        return true;
    }

    public bool MoveNext()
    {
        _entityIndex++;
        if (_chunk != null && _entityIndex < _chunk.EntityCount)
            return true;

        if (!NextChunk())
            return false;

        return true;
    }

    // ReSharper disable once UnusedMember.Global (Justification: used by enumerator)
    public void Dispose()
    {
        _chunksEnumerator.Dispose();
    }
}