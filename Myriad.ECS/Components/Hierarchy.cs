using Myriad.ECS.Command;

namespace Myriad.ECS.Components;

/// <summary>
/// A list of child entities that will be destroyed when this entity is destroyed
/// </summary>
/// <param name="children"></param>
public readonly struct ChildEntities(List<Entity> children)
    : IDisposableComponent
{
    /// <summary>
    /// A list of entities that will be destroyed when this component is disposed
    /// </summary>
    public readonly List<Entity> Children = children;

    /// <inheritdoc />
    public void Dispose(ref LazyCommandBuffer buffer)
    {
        // Delete the children, checking if they're alive before doing so:
        // 1. This prevents deleting children which are phantoms, which are probably being cleared up in other ways
        // 2. This prevents getting the buffer (forcing it to be created) for no reason
        foreach (var entity in Children)
            if (entity.IsAlive())
                buffer.CommandBuffer.Delete(entity);
        Children.Clear();
    }
}