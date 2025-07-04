using Myriad.ECS.Components;
using Myriad.ECS.IDs;
using Myriad.ECS.Worlds;

namespace Myriad.ECS.Paths;

/// <summary>
/// A path through a set of related entities
/// </summary>
public readonly partial struct Path
    : IPath
{
    private readonly ReadOnlyMemory<IStep> _steps;

    /// <summary>
    /// Create a new path
    /// </summary>
    /// <param name="steps"></param>
    public Path(ReadOnlyMemory<IStep> steps)
    {
        _steps = steps;
    }

    /// <summary>
    /// Create a new path
    /// </summary>
    /// <param name="steps"></param>
    public Path(params IStep[] steps)
    {
        _steps = steps;
    }

    /// <summary>
    /// Try to follow this path, starting from an entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public Entity? TryFollow(Entity entity)
    {
        foreach (var stage in _steps.Span)
            if (!stage.TryFollow(ref entity))
                return default;

        if (!entity.IsAlive())
            return default;

        return entity;
    }

    /// <summary>
    /// A step in the path
    /// </summary>
    public interface IStep
    {
        /// <summary>
        /// Try to follow this step
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool TryFollow(ref Entity entity);
    }

    /// <summary>
    /// Try to follow a relational component
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class FollowRelation<T>
        : IStep
        where T : IEntityRelationComponent
    {
        private static readonly ComponentID C = ComponentID<T>.ID;

        /// <inheritdoc />
        public bool TryFollow(ref Entity entity)
        {
            // Try to get entity info for this entity
            var dummy = default(EntityInfo);
            ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

            // Can't follow a path through a dead entity!
            if (isDead)
                return false;

            // Check if component is present
            if (!entityInfo.Chunk.Archetype.Components.Contains(C))
                return false;

            // Follow link
            entity = entityInfo.GetRow(entity.ID).GetMutable<T>(C).Target;
            return true;
        }
    }

    /// <summary>
    /// Static helpers for <see cref="Nested{T}"/>
    /// </summary>
    public static class Nested
    {
        /// <summary>
        /// Create a new nested path wrapper
        /// </summary>
        /// <param name="nested"></param>
        /// <returns></returns>
        public static Nested<T> Create<T>(T nested)
            where T : IPath
        {
            return new Nested<T>(nested);
        }
    }

    /// <summary>
    /// Follow another path
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class Nested<T>
        : IStep
        where T : IPath
    {
        private readonly T _path;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public Nested(T path)
        {
            _path = path;
        }

        /// <inheritdoc />
        public bool TryFollow(ref Entity entity)
        {
            var follow = _path.TryFollow(entity);
            if (!follow.HasValue)
                return false;

            entity = follow.Value;
            return true;
        }
    }
}