using Myriad.ECS.IDs;
using System.Diagnostics.CodeAnalysis;

namespace Myriad.ECS.Collections;

/* dotcover disable */


/// <summary>
/// Get a precalculated span of components IDs from generic type. Returned span is deduplicated.
/// </summary>

internal static class SortedListOfComponents<T0, T1>
    where T0 : IComponent
        where T1 : IComponent
{
    // ReSharper disable once StaticMemberInGenericType
    public static readonly ReadOnlyMemory<ComponentID> Components;

    static SortedListOfComponents()
    {
        // Create sorted array of components
        var components = new[]
        {
            ComponentID<T0>.ID,
            ComponentID<T1>.ID,
        };
        Array.Sort(components);

        // Deduplicate components
        var insertIndex = 1;
        for (var i = 1; i < components.Length; i++)
        {
            if (components[i].Value != components[insertIndex - 1].Value)
            {
                components[insertIndex] = components[i];
                insertIndex++;
            }
        }

        Components = components.AsMemory(0, insertIndex);
    }
}


/// <summary>
/// Get a precalculated span of components IDs from generic type. Returned span is deduplicated.
/// </summary>
[ExcludeFromCodeCoverage]
internal static class SortedListOfComponents<T0, T1, T2>
    where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
{
    // ReSharper disable once StaticMemberInGenericType
    public static readonly ReadOnlyMemory<ComponentID> Components;

    static SortedListOfComponents()
    {
        // Create sorted array of components
        var components = new[]
        {
            ComponentID<T0>.ID,
            ComponentID<T1>.ID,
            ComponentID<T2>.ID,
        };
        Array.Sort(components);

        // Deduplicate components
        var insertIndex = 1;
        for (var i = 1; i < components.Length; i++)
        {
            if (components[i].Value != components[insertIndex - 1].Value)
            {
                components[insertIndex] = components[i];
                insertIndex++;
            }
        }

        Components = components.AsMemory(0, insertIndex);
    }
}


/// <summary>
/// Get a precalculated span of components IDs from generic type. Returned span is deduplicated.
/// </summary>
[ExcludeFromCodeCoverage]
internal static class SortedListOfComponents<T0, T1, T2, T3>
    where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
{
    // ReSharper disable once StaticMemberInGenericType
    public static readonly ReadOnlyMemory<ComponentID> Components;

    static SortedListOfComponents()
    {
        // Create sorted array of components
        var components = new[]
        {
            ComponentID<T0>.ID,
            ComponentID<T1>.ID,
            ComponentID<T2>.ID,
            ComponentID<T3>.ID,
        };
        Array.Sort(components);

        // Deduplicate components
        var insertIndex = 1;
        for (var i = 1; i < components.Length; i++)
        {
            if (components[i].Value != components[insertIndex - 1].Value)
            {
                components[insertIndex] = components[i];
                insertIndex++;
            }
        }

        Components = components.AsMemory(0, insertIndex);
    }
}


/// <summary>
/// Get a precalculated span of components IDs from generic type. Returned span is deduplicated.
/// </summary>
[ExcludeFromCodeCoverage]
internal static class SortedListOfComponents<T0, T1, T2, T3, T4>
    where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
{
    // ReSharper disable once StaticMemberInGenericType
    public static readonly ReadOnlyMemory<ComponentID> Components;

    static SortedListOfComponents()
    {
        // Create sorted array of components
        var components = new[]
        {
            ComponentID<T0>.ID,
            ComponentID<T1>.ID,
            ComponentID<T2>.ID,
            ComponentID<T3>.ID,
            ComponentID<T4>.ID,
        };
        Array.Sort(components);

        // Deduplicate components
        var insertIndex = 1;
        for (var i = 1; i < components.Length; i++)
        {
            if (components[i].Value != components[insertIndex - 1].Value)
            {
                components[insertIndex] = components[i];
                insertIndex++;
            }
        }

        Components = components.AsMemory(0, insertIndex);
    }
}


/// <summary>
/// Get a precalculated span of components IDs from generic type. Returned span is deduplicated.
/// </summary>
[ExcludeFromCodeCoverage]
internal static class SortedListOfComponents<T0, T1, T2, T3, T4, T5>
    where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
{
    // ReSharper disable once StaticMemberInGenericType
    public static readonly ReadOnlyMemory<ComponentID> Components;

    static SortedListOfComponents()
    {
        // Create sorted array of components
        var components = new[]
        {
            ComponentID<T0>.ID,
            ComponentID<T1>.ID,
            ComponentID<T2>.ID,
            ComponentID<T3>.ID,
            ComponentID<T4>.ID,
            ComponentID<T5>.ID,
        };
        Array.Sort(components);

        // Deduplicate components
        var insertIndex = 1;
        for (var i = 1; i < components.Length; i++)
        {
            if (components[i].Value != components[insertIndex - 1].Value)
            {
                components[insertIndex] = components[i];
                insertIndex++;
            }
        }

        Components = components.AsMemory(0, insertIndex);
    }
}


/// <summary>
/// Get a precalculated span of components IDs from generic type. Returned span is deduplicated.
/// </summary>
[ExcludeFromCodeCoverage]
internal static class SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6>
    where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
{
    // ReSharper disable once StaticMemberInGenericType
    public static readonly ReadOnlyMemory<ComponentID> Components;

    static SortedListOfComponents()
    {
        // Create sorted array of components
        var components = new[]
        {
            ComponentID<T0>.ID,
            ComponentID<T1>.ID,
            ComponentID<T2>.ID,
            ComponentID<T3>.ID,
            ComponentID<T4>.ID,
            ComponentID<T5>.ID,
            ComponentID<T6>.ID,
        };
        Array.Sort(components);

        // Deduplicate components
        var insertIndex = 1;
        for (var i = 1; i < components.Length; i++)
        {
            if (components[i].Value != components[insertIndex - 1].Value)
            {
                components[insertIndex] = components[i];
                insertIndex++;
            }
        }

        Components = components.AsMemory(0, insertIndex);
    }
}


/// <summary>
/// Get a precalculated span of components IDs from generic type. Returned span is deduplicated.
/// </summary>
[ExcludeFromCodeCoverage]
internal static class SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6, T7>
    where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
{
    // ReSharper disable once StaticMemberInGenericType
    public static readonly ReadOnlyMemory<ComponentID> Components;

    static SortedListOfComponents()
    {
        // Create sorted array of components
        var components = new[]
        {
            ComponentID<T0>.ID,
            ComponentID<T1>.ID,
            ComponentID<T2>.ID,
            ComponentID<T3>.ID,
            ComponentID<T4>.ID,
            ComponentID<T5>.ID,
            ComponentID<T6>.ID,
            ComponentID<T7>.ID,
        };
        Array.Sort(components);

        // Deduplicate components
        var insertIndex = 1;
        for (var i = 1; i < components.Length; i++)
        {
            if (components[i].Value != components[insertIndex - 1].Value)
            {
                components[insertIndex] = components[i];
                insertIndex++;
            }
        }

        Components = components.AsMemory(0, insertIndex);
    }
}


/// <summary>
/// Get a precalculated span of components IDs from generic type. Returned span is deduplicated.
/// </summary>
[ExcludeFromCodeCoverage]
internal static class SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8>
    where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
        where T8 : IComponent
{
    // ReSharper disable once StaticMemberInGenericType
    public static readonly ReadOnlyMemory<ComponentID> Components;

    static SortedListOfComponents()
    {
        // Create sorted array of components
        var components = new[]
        {
            ComponentID<T0>.ID,
            ComponentID<T1>.ID,
            ComponentID<T2>.ID,
            ComponentID<T3>.ID,
            ComponentID<T4>.ID,
            ComponentID<T5>.ID,
            ComponentID<T6>.ID,
            ComponentID<T7>.ID,
            ComponentID<T8>.ID,
        };
        Array.Sort(components);

        // Deduplicate components
        var insertIndex = 1;
        for (var i = 1; i < components.Length; i++)
        {
            if (components[i].Value != components[insertIndex - 1].Value)
            {
                components[insertIndex] = components[i];
                insertIndex++;
            }
        }

        Components = components.AsMemory(0, insertIndex);
    }
}


/// <summary>
/// Get a precalculated span of components IDs from generic type. Returned span is deduplicated.
/// </summary>
[ExcludeFromCodeCoverage]
internal static class SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
    where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
        where T8 : IComponent
        where T9 : IComponent
{
    // ReSharper disable once StaticMemberInGenericType
    public static readonly ReadOnlyMemory<ComponentID> Components;

    static SortedListOfComponents()
    {
        // Create sorted array of components
        var components = new[]
        {
            ComponentID<T0>.ID,
            ComponentID<T1>.ID,
            ComponentID<T2>.ID,
            ComponentID<T3>.ID,
            ComponentID<T4>.ID,
            ComponentID<T5>.ID,
            ComponentID<T6>.ID,
            ComponentID<T7>.ID,
            ComponentID<T8>.ID,
            ComponentID<T9>.ID,
        };
        Array.Sort(components);

        // Deduplicate components
        var insertIndex = 1;
        for (var i = 1; i < components.Length; i++)
        {
            if (components[i].Value != components[insertIndex - 1].Value)
            {
                components[insertIndex] = components[i];
                insertIndex++;
            }
        }

        Components = components.AsMemory(0, insertIndex);
    }
}


/// <summary>
/// Get a precalculated span of components IDs from generic type. Returned span is deduplicated.
/// </summary>
[ExcludeFromCodeCoverage]
internal static class SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
    where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
        where T8 : IComponent
        where T9 : IComponent
        where T10 : IComponent
{
    // ReSharper disable once StaticMemberInGenericType
    public static readonly ReadOnlyMemory<ComponentID> Components;

    static SortedListOfComponents()
    {
        // Create sorted array of components
        var components = new[]
        {
            ComponentID<T0>.ID,
            ComponentID<T1>.ID,
            ComponentID<T2>.ID,
            ComponentID<T3>.ID,
            ComponentID<T4>.ID,
            ComponentID<T5>.ID,
            ComponentID<T6>.ID,
            ComponentID<T7>.ID,
            ComponentID<T8>.ID,
            ComponentID<T9>.ID,
            ComponentID<T10>.ID,
        };
        Array.Sort(components);

        // Deduplicate components
        var insertIndex = 1;
        for (var i = 1; i < components.Length; i++)
        {
            if (components[i].Value != components[insertIndex - 1].Value)
            {
                components[insertIndex] = components[i];
                insertIndex++;
            }
        }

        Components = components.AsMemory(0, insertIndex);
    }
}


/// <summary>
/// Get a precalculated span of components IDs from generic type. Returned span is deduplicated.
/// </summary>
[ExcludeFromCodeCoverage]
internal static class SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
    where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
        where T8 : IComponent
        where T9 : IComponent
        where T10 : IComponent
        where T11 : IComponent
{
    // ReSharper disable once StaticMemberInGenericType
    public static readonly ReadOnlyMemory<ComponentID> Components;

    static SortedListOfComponents()
    {
        // Create sorted array of components
        var components = new[]
        {
            ComponentID<T0>.ID,
            ComponentID<T1>.ID,
            ComponentID<T2>.ID,
            ComponentID<T3>.ID,
            ComponentID<T4>.ID,
            ComponentID<T5>.ID,
            ComponentID<T6>.ID,
            ComponentID<T7>.ID,
            ComponentID<T8>.ID,
            ComponentID<T9>.ID,
            ComponentID<T10>.ID,
            ComponentID<T11>.ID,
        };
        Array.Sort(components);

        // Deduplicate components
        var insertIndex = 1;
        for (var i = 1; i < components.Length; i++)
        {
            if (components[i].Value != components[insertIndex - 1].Value)
            {
                components[insertIndex] = components[i];
                insertIndex++;
            }
        }

        Components = components.AsMemory(0, insertIndex);
    }
}


/// <summary>
/// Get a precalculated span of components IDs from generic type. Returned span is deduplicated.
/// </summary>
[ExcludeFromCodeCoverage]
internal static class SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
    where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
        where T8 : IComponent
        where T9 : IComponent
        where T10 : IComponent
        where T11 : IComponent
        where T12 : IComponent
{
    // ReSharper disable once StaticMemberInGenericType
    public static readonly ReadOnlyMemory<ComponentID> Components;

    static SortedListOfComponents()
    {
        // Create sorted array of components
        var components = new[]
        {
            ComponentID<T0>.ID,
            ComponentID<T1>.ID,
            ComponentID<T2>.ID,
            ComponentID<T3>.ID,
            ComponentID<T4>.ID,
            ComponentID<T5>.ID,
            ComponentID<T6>.ID,
            ComponentID<T7>.ID,
            ComponentID<T8>.ID,
            ComponentID<T9>.ID,
            ComponentID<T10>.ID,
            ComponentID<T11>.ID,
            ComponentID<T12>.ID,
        };
        Array.Sort(components);

        // Deduplicate components
        var insertIndex = 1;
        for (var i = 1; i < components.Length; i++)
        {
            if (components[i].Value != components[insertIndex - 1].Value)
            {
                components[insertIndex] = components[i];
                insertIndex++;
            }
        }

        Components = components.AsMemory(0, insertIndex);
    }
}


/// <summary>
/// Get a precalculated span of components IDs from generic type. Returned span is deduplicated.
/// </summary>
[ExcludeFromCodeCoverage]
internal static class SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
    where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
        where T8 : IComponent
        where T9 : IComponent
        where T10 : IComponent
        where T11 : IComponent
        where T12 : IComponent
        where T13 : IComponent
{
    // ReSharper disable once StaticMemberInGenericType
    public static readonly ReadOnlyMemory<ComponentID> Components;

    static SortedListOfComponents()
    {
        // Create sorted array of components
        var components = new[]
        {
            ComponentID<T0>.ID,
            ComponentID<T1>.ID,
            ComponentID<T2>.ID,
            ComponentID<T3>.ID,
            ComponentID<T4>.ID,
            ComponentID<T5>.ID,
            ComponentID<T6>.ID,
            ComponentID<T7>.ID,
            ComponentID<T8>.ID,
            ComponentID<T9>.ID,
            ComponentID<T10>.ID,
            ComponentID<T11>.ID,
            ComponentID<T12>.ID,
            ComponentID<T13>.ID,
        };
        Array.Sort(components);

        // Deduplicate components
        var insertIndex = 1;
        for (var i = 1; i < components.Length; i++)
        {
            if (components[i].Value != components[insertIndex - 1].Value)
            {
                components[insertIndex] = components[i];
                insertIndex++;
            }
        }

        Components = components.AsMemory(0, insertIndex);
    }
}


/// <summary>
/// Get a precalculated span of components IDs from generic type. Returned span is deduplicated.
/// </summary>
[ExcludeFromCodeCoverage]
internal static class SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
    where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
        where T8 : IComponent
        where T9 : IComponent
        where T10 : IComponent
        where T11 : IComponent
        where T12 : IComponent
        where T13 : IComponent
        where T14 : IComponent
{
    // ReSharper disable once StaticMemberInGenericType
    public static readonly ReadOnlyMemory<ComponentID> Components;

    static SortedListOfComponents()
    {
        // Create sorted array of components
        var components = new[]
        {
            ComponentID<T0>.ID,
            ComponentID<T1>.ID,
            ComponentID<T2>.ID,
            ComponentID<T3>.ID,
            ComponentID<T4>.ID,
            ComponentID<T5>.ID,
            ComponentID<T6>.ID,
            ComponentID<T7>.ID,
            ComponentID<T8>.ID,
            ComponentID<T9>.ID,
            ComponentID<T10>.ID,
            ComponentID<T11>.ID,
            ComponentID<T12>.ID,
            ComponentID<T13>.ID,
            ComponentID<T14>.ID,
        };
        Array.Sort(components);

        // Deduplicate components
        var insertIndex = 1;
        for (var i = 1; i < components.Length; i++)
        {
            if (components[i].Value != components[insertIndex - 1].Value)
            {
                components[insertIndex] = components[i];
                insertIndex++;
            }
        }

        Components = components.AsMemory(0, insertIndex);
    }
}


/// <summary>
/// Get a precalculated span of components IDs from generic type. Returned span is deduplicated.
/// </summary>
[ExcludeFromCodeCoverage]
internal static class SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
    where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
        where T8 : IComponent
        where T9 : IComponent
        where T10 : IComponent
        where T11 : IComponent
        where T12 : IComponent
        where T13 : IComponent
        where T14 : IComponent
        where T15 : IComponent
{
    // ReSharper disable once StaticMemberInGenericType
    public static readonly ReadOnlyMemory<ComponentID> Components;

    static SortedListOfComponents()
    {
        // Create sorted array of components
        var components = new[]
        {
            ComponentID<T0>.ID,
            ComponentID<T1>.ID,
            ComponentID<T2>.ID,
            ComponentID<T3>.ID,
            ComponentID<T4>.ID,
            ComponentID<T5>.ID,
            ComponentID<T6>.ID,
            ComponentID<T7>.ID,
            ComponentID<T8>.ID,
            ComponentID<T9>.ID,
            ComponentID<T10>.ID,
            ComponentID<T11>.ID,
            ComponentID<T12>.ID,
            ComponentID<T13>.ID,
            ComponentID<T14>.ID,
            ComponentID<T15>.ID,
        };
        Array.Sort(components);

        // Deduplicate components
        var insertIndex = 1;
        for (var i = 1; i < components.Length; i++)
        {
            if (components[i].Value != components[insertIndex - 1].Value)
            {
                components[insertIndex] = components[i];
                insertIndex++;
            }
        }

        Components = components.AsMemory(0, insertIndex);
    }
}


