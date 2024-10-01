using Myriad.ECS.Worlds.Archetypes;
using Myriad.ECS.Worlds.Chunks;

namespace Myriad.ECS.Worlds;

internal struct EntityInfo
{
    /// <summary>
    /// The current version of this entity
    /// </summary>
    public uint Version;

    /// <summary>
    /// The chunk in the archetype which contains this entity
    /// </summary>
    public Chunk Chunk;

    /// <summary>
    /// The row in the chunk which contains this entity
    /// </summary>
    public int RowIndex;

    public readonly Row GetRow(EntityId entity)
    {
        return new Row(entity, RowIndex, Chunk);
    }
}