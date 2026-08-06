using Myriad.ECS.Worlds;

namespace Myriad.ECS.Sorting;

/// <summary>
/// Compare entities based on their Chunk ID. This will have the effect of grouping together entities by their
/// chunk ID, which may improve locality.
/// </summary>
public readonly struct EntityChunkComparer
    : IComparer<EntityId>
{
    private readonly World _world;

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityChunkComparer"/> struct.
    /// </summary>
    /// <param name="world">
    /// The <see cref="World"/> instance used to provide context for entity chunk comparisons.
    /// </param>
    public EntityChunkComparer(World world)
    {
        _world = world;
    }

    /// <inheritdoc />
    public int Compare(EntityId x, EntityId y)
    {
        EntityInfo dummy = default;
        ref var xInfo = ref _world.GetEntityInfo(x, ref dummy, out var xDead);
        ref var yInfo = ref _world.GetEntityInfo(y, ref dummy, out var yDead);

        var xId = xDead ? long.MaxValue : xInfo.Chunk.ChunkId;
        var yId = yDead ? long.MaxValue : yInfo.Chunk.ChunkId;
        
        return xId.CompareTo(yId);
    }
}