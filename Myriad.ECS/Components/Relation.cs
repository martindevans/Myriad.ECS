namespace Myriad.ECS.Components;

/// <summary>
/// Sets the target of an entity relationship
/// </summary>
public interface IEntityRelationSetter
    : IComponent
{
    /// <summary>
    /// Set the target
    /// </summary>
    public Entity Target { set; }
}

/// <summary>
/// A relation component can be added to a command buffer, along with an associated buffered entity. When the
/// buffered entity is created it will be automatically resolved and added to this component.
/// </summary>
public interface IEntityRelationComponent
    : IEntityRelationSetter
{
    /// <summary>
    /// The target entity of this relationship
    /// </summary>
#pragma warning disable CS0108, CS0114 // keyword `new` required
    public Entity Target { get; }
#pragma warning restore CS0108, CS0114
}

/// <summary>
/// A relation component can be added to a command buffer, along with an associated buffered entity. When the
/// buffered entity is created it will be automatically resolved and added to this component.
/// </summary>
public interface IEntityIdRelationComponent
    : IEntityRelationSetter
{
    /// <summary>
    /// The target entity of this relationship
    /// </summary>
    public EntityId TargetId { get; }
}

/// <summary>
/// Contains a reference to the entity it is attached to
/// </summary>
public struct SelfReference
    : IEntityRelationComponent
{
    /// <summary>
    /// The entity this component is attached to
    /// </summary>
    public Entity Target { get; set; }
}