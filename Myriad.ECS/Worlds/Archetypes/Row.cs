using Myriad.ECS.Worlds.Chunks;

namespace Myriad.ECS.Worlds.Archetypes;

internal readonly record struct Row(Entity Entity, int RowIndex, Chunk Chunk)
{
    public ref T GetMutable<T>()
        where T : IComponent
    {
        return ref Chunk.GetMutable<T>(Entity, RowIndex);
    }
}