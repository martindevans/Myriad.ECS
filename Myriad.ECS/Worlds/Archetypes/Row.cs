using Myriad.ECS.Worlds.Chunks;

namespace Myriad.ECS.Worlds.Archetypes;

internal readonly record struct Row
{
    public EntityId Entity { get; }
    public int RowIndex { get; }
    public Chunk Chunk { get; }

    internal Row(EntityId entity, int rowIndex, Chunk chunk)
    {
        Entity = entity;
        RowIndex = rowIndex;
        Chunk = chunk;
    }

    public ref T GetMutable<T>()
        where T : IComponent
    {
        return ref Chunk.GetRef<T>(Entity, RowIndex);
    }
}