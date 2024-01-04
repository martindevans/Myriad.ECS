using Myriad.ECS.Worlds.Archetypes;

namespace Myriad.ECS.Worlds;

internal struct EntityInfo
{
    /// <summary>
    /// The current version of this entity
    /// </summary>
    public uint Version;

    /// <summary>
    /// The archetype which contains this entity
    /// </summary>
    public Archetype Archetype;

    /// <summary>
    /// The chunk index in the archetype which contains this entity
    /// </summary>
    public int ChunkIndex;

    /// <summary>
    /// The row in the chunk which contains this entity
    /// </summary>
    public int RowIndex;
}