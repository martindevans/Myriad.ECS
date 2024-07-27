namespace Myriad.ECS.Components;

/// <summary>
/// Automatically has Dispose() called when this component is destroyed. Either because the Entity
/// was destroyed or because the component was removed.
/// </summary>
public interface IDisposableComponent
    : IComponent, IDisposable;