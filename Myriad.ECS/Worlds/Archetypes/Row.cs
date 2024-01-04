using Myriad.ECS.Worlds.Chunks;

namespace Myriad.ECS.Worlds.Archetypes;

internal readonly struct Row(Entity entity, int index, Chunk chunk, int chunkIndex)
{
    public int ChunkIndex { get; } = chunkIndex;

    public ref T GetMutable<T>()
        where T : IComponent
    {
        return ref chunk.GetMutable<T>(entity, index);
    }

    public ref readonly T GetImmutable<T>()
        where T : IComponent
    {
        return ref chunk.GetImmutable<T>(entity, index);
    }
}