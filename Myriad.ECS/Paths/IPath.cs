namespace Myriad.ECS.Paths;

/// <summary>
/// A path through a set of related entities
/// </summary>
public interface IPath
{
    /// <summary>
    /// Try to follow this path, starting from an entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Entity? TryFollow(Entity entity);
}