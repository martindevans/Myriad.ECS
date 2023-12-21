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
        return new ComponentID(0);
    }

    public static ComponentID Next(ComponentID value)
    {
        return new ComponentID(checked(value.Value + 1));
    }
}