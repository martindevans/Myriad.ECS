using System.Diagnostics;

namespace Myriad.ECS.IDs;

[DebuggerDisplay("{Type} ({Value})")]
public readonly record struct ComponentID
    : IComparable<ComponentID>
{
    internal const int SpecialBits = 1;
    internal const int IsPhantomComponentMask = 1;

    public int Value { get; }
    public Type Type => ComponentRegistry.Get(this);

    /// <summary>
    /// Indicates if this component implements <see cref="IsPhantomComponent"/>
    /// </summary>
    public bool IsPhantomComponent => (Value & IsPhantomComponentMask) == IsPhantomComponentMask;

    internal ComponentID(int value)
    {
        Value = value;
    }

    public int CompareTo(ComponentID other)
    {
        return Value.CompareTo(other.Value);
    }

    public override string ToString()
    {
        var p = IsPhantomComponent ? " (phantom)" : "";
        return $"C{Value}{p}";
    }
}

internal static class ComponentID<T>
    where T : IComponent
{
    public static readonly ComponentID ID = ComponentRegistry.Get<T>();
}