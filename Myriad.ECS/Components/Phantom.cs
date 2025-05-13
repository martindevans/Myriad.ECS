namespace Myriad.ECS.Components;

/// <summary>
/// <para>
/// A phantom component acts like a normal component until the entity is destroyed. At
/// that point instead of being destroyed the entity will automatically have a
/// <see cref="Phantom"/> component added.
/// </para>
/// <para>
/// If an entity with a <see cref="Phantom"/> entity is destroyed, it will
/// actually be destroyed. It will also automatically be destroyed if it has no more
/// <see cref="IPhantomComponent"/> components attached.
/// </para>
/// </summary>
public interface IPhantomComponent : IComponent;

/// <summary>
/// This component receives a notification when the entity it is attached to becomes a phantom.
/// </summary>
/// <remarks>Note that this component is <b>not</b> an <see cref="IPhantomComponent"/>.</remarks>
public interface IPhantomNotifierComponent : IComponent
{
    /// <summary>
    /// Called when the entity this component is attached to becomes a phantom.
    /// </summary>
    /// <param name="self">A reference to the entity this component is attached to</param>
    void OnBecomePhantom(EntityId self);
}

/// <summary>
/// <para>
/// Indicates that the entity this is attached to is a "phantom". Phantom entities
/// are automatically excluded from queries and must be specifically requested.
/// </para>
/// <para>An entity will automatically become a phantom if it is destroyed, but still has
/// <see cref="IPhantomComponent"/> components attached.
/// </para>
/// <para>
/// If an entity with a <see cref="Phantom"/> component is destroyed, it will
/// actually be destroyed. It will automatically be destroyed if it has no more
/// <see cref="IPhantomComponent"/> components attached.
/// </para>
/// </summary>
public struct Phantom : IComponent;