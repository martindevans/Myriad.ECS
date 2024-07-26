namespace Myriad.ECS.Components;

/// <summary>
/// A relation component can be added to a command buffer, along with an associated buffered entity. When the
/// buffered entity is created it will be automatically resolved and added to this component.
/// </summary>
public interface IEntityRelationComponent : IComponent
{
    /// <summary>
    /// The target entity of this relationship
    /// </summary>
    public Entity Target { get; set; }
}