using Myriad.ECS.IDs;
using Myriad.ECS.Registry;

namespace Myriad.ECS;

/// <summary>
/// Marker interface for components
/// </summary>
public interface IComponent;

/// <summary>
/// Marker interface for components
/// </summary>
public interface IComponent<TSelf>
    : IComponent
    where TSelf : IComponent<TSelf>
{
    public static ComponentID ID => ComponentRegistry.Get<TSelf>();
}