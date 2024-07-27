using Myriad.ECS.Command;

namespace Myriad.ECS.Components;

/// <summary>
/// Automatically has Dispose() called when this component is destroyed. Either because the Entity
/// was destroyed or because the component was removed.
/// </summary>
public interface IDisposableComponent
    : IComponent
{
    /// <summary>
    /// Dispose this component
    /// </summary>
    /// <param name="buffer">May be used to enqueue more work as a result of this disposal</param>
    public void Dispose(ref LazyCommandBuffer buffer);
}