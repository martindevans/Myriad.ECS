﻿using System.Diagnostics;
using Myriad.ECS.Registry;

namespace Myriad.ECS.IDs;

[DebuggerDisplay("{Type} ({Value})")]
public readonly record struct ComponentID
    : IIDNumber<ComponentID>
{
    public int Value { get; }
    public Type Type => ComponentRegistry.Get(this);

    private ComponentID(int value)
    {
        Value = value;
    }

    public ComponentID Next()
    {
        return new ComponentID(checked(Value + 1));
    }

    public int CompareTo(ComponentID other)
    {
        return Value.CompareTo(other.Value);
    }
}

internal static class ComponentID<T>
    where T : IComponent
{
    public static readonly ComponentID ID = ComponentRegistry.Get<T>();
}