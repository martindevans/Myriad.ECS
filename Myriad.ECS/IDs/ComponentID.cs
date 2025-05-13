using System.Diagnostics;
using Myriad.ECS.Components;

namespace Myriad.ECS.IDs;

/// <summary>
/// Unique numeric ID for a type which implements IComponent
/// </summary>
[DebuggerDisplay("{Type} ({Value})")]
public readonly record struct ComponentID
    : IComparable<ComponentID>
{
    internal const int SpecialBitsCount = 4;
    internal const int SpecialBitsMask  = ~(~0 << SpecialBitsCount);
    internal const int IsPhantomComponentMask         = 0b0001;
    internal const int IsRelationComponentMask        = 0b0010;
    internal const int IsDisposableComponentMask      = 0b0100;
    internal const int IsPhantomNotifierComponentMask = 0b1000;

    /// <summary>
    /// Get the raw value of this ID
    /// </summary>
    public int Value { get; }

    /// <summary>
    /// The <see cref="System.Type"/> of the component this ID is for
    /// </summary>
    public Type Type => ComponentRegistry.Get(this);

    /// <summary>
    /// Indicates if this component implements <see cref="IPhantomComponent"/>
    /// </summary>
    public bool IsPhantomComponent => (Value & IsPhantomComponentMask) == IsPhantomComponentMask;

    /// <summary>
    /// Indicates if this component implements <see cref="IEntityRelationComponent"/>
    /// </summary>
    public bool IsRelationComponent => (Value & IsRelationComponentMask) == IsRelationComponentMask;

    /// <summary>
    /// Indicates if this component implements <see cref="IDisposableComponent"/>
    /// </summary>
    public bool IsDisposableComponent => (Value & IsDisposableComponentMask) == IsDisposableComponentMask;

    /// <summary>
    /// Indicates if this component implements <see cref="IPhantomNotifierComponent"/>
    /// </summary>
    public bool IsPhantomNotifierComponent => (Value & IsPhantomNotifierComponentMask) == IsPhantomNotifierComponentMask;

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
        var d = IsDisposableComponent ? " (dispose)" : "";
        return $"C{Value}{p}{d}";
    }

    /// <summary>
    /// Get the component ID for the given type
    /// </summary>
    /// <param name="type"></param>
    /// <exception cref="ArgumentException">Thrown if 'type' does not implement <see cref="IComponent"/></exception>
    /// <returns></returns>
    public static ComponentID Get(Type type)
    {
        return ComponentRegistry.Get(type);
    }
}

/// <summary>
/// Retrieve the component ID for a type
/// </summary>
/// <typeparam name="TComponent"></typeparam>
public static class ComponentID<TComponent>
    where TComponent : IComponent
{
    /// <summary>
    /// The ID for <typeparamref name="TComponent" />
    /// </summary>
    public static readonly ComponentID ID = ComponentRegistry.Get<TComponent>();
}