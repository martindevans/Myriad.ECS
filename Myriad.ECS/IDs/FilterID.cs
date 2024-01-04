namespace Myriad.ECS.IDs;

public readonly record struct FilterID
    : IIDNumber<FilterID>
{
    public int Value { get; }

    private FilterID(int value)
    {
        Value = value;
    }

    public static FilterID First()
    {
        return new FilterID(1);
    }

    public static FilterID Next(FilterID value)
    {
        return new FilterID(checked(value.Value + 1));
    }
}