﻿using Myriad.ECS.IDs;
using System.Diagnostics.CodeAnalysis;

namespace Myriad.ECS.Registry;

/// <summary>
/// Store a lookup from component type to unique 32 bit ID.
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public sealed class ComponentRegistry
    : BaseRegistry<IComponent, ComponentID>
{
    [ExcludeFromCodeCoverage]
    private ComponentRegistry()
    {
    }
}