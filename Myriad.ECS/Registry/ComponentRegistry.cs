using Myriad.ECS.IDs;

namespace Myriad.ECS.Registry;

/// <summary>
/// Store a lookup from component type to unique 32 bit ID.
/// </summary>
public sealed class ComponentRegistry
    : BaseRegistry<IComponent, ComponentID>
{
    private ComponentRegistry()
    {
    }
}