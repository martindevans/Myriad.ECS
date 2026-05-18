using Myriad.ECS.IDs;
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

    /// <summary>
    /// Get a mutable reference to a component on this entity
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public readonly ref T GetMutable<T>()
        where T : IComponent
    {
        return ref Chunk.GetRef<T>(RowIndex);
    }

    /// <summary>
    /// Get a mutable reference to a component on this entity
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public readonly ref T GetMutable<T>(ComponentID id)
        where T : IComponent
    {
        return ref Chunk.GetRef<T>(RowIndex, id);
    }
}