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
        foreach (var entity in Children)
            if (entity.IsAlive())
                buffer.CommandBuffer.Delete(entity);
        Children.Clear();
    }
}