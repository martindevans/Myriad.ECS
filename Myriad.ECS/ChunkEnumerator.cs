using System.Collections;

namespace Myriad.ECS;

public struct ChunkEnumerator
    : IEnumerator<Chunk>
{
    public bool MoveNext()
    {
        throw new NotImplementedException();
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }

    public Chunk Current { get; }

    object IEnumerator.Current => Current;

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}