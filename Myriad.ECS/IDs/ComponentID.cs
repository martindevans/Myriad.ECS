using Myriad.ECS.Registry;

namespace Myriad.ECS.IDs;

public readonly record struct ComponentID
    : IIDNumber<ComponentID>
{
    public int Value { get; }

    private ComponentID(int value)
    {
        Value = value;
    }

    public static ComponentID First()
    {
        return new ComponentID(1);
    }

    public static ComponentID Next(ComponentID value)
    {
        return new ComponentID(checked(value.Value + 1));
    }
}

internal static class ComponentID<T>
    where T : IComponent
{
    public static readonly ComponentID ID = ComponentRegistry.Get<T>();
}