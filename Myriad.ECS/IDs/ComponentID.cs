using System.Diagnostics;

namespace Myriad.ECS.IDs;

/// <summary>
/// Unique numeric ID for a type which implements IComponent
/// </summary>
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

    /// <inheritdoc />
    public int CompareTo(ComponentID other)
    {
        return Value.CompareTo(other.Value);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        var p = IsPhantomComponent ? " (phantom)" : "";
        return $"C{Value}{p}";
    }

    /// <summary>
    /// Get the component ID for the given type
    /// </summary>
    /// <param name="type"></param>
    /// <exception cref="ArgumentException">Thrown if <see cref="type"/> does not implement <see cref="IComponent"/></exception>
    /// <returns></returns>
    public static ComponentID Get(Type type)
    {
        return ComponentRegistry.Get(type);
    }
}

/// <summary>
/// Retrieve the component ID for a type
/// </summary>
/// <typeparam name="T"></typeparam>
public static class ComponentID<T>
    where T : IComponent
{
    public static readonly ComponentID ID = ComponentRegistry.Get<T>();
}