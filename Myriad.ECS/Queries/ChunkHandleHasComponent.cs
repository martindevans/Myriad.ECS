using Myriad.ECS.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Myriad.ECS.Queries;

/* dotcover disable */

public readonly ref partial struct ChunkHandle
{

    /// <summary>
    /// Check if this chunk contains a tuple of several components
    /// </summary>
    
    public bool HasComponent<T0, T1>()
        where T0 : IComponent
        where T1 : IComponent
    {
        // Get a cached list of components that has been sorted
        var components = SortedListOfComponents<T0, T1>.Components;

        // Check if they are all in the component set
        return Archetype.Components.IsSupersetOfSortedSpan(components.Span);
    }

    /// <summary>
    /// Check if this chunk contains a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool HasComponent<T0, T1, T2>()
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
    {
        // Get a cached list of components that has been sorted
        var components = SortedListOfComponents<T0, T1, T2>.Components;

        // Check if they are all in the component set
        return Archetype.Components.IsSupersetOfSortedSpan(components.Span);
    }

    /// <summary>
    /// Check if this chunk contains a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool HasComponent<T0, T1, T2, T3>()
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
    {
        // Get a cached list of components that has been sorted
        var components = SortedListOfComponents<T0, T1, T2, T3>.Components;

        // Check if they are all in the component set
        return Archetype.Components.IsSupersetOfSortedSpan(components.Span);
    }

    /// <summary>
    /// Check if this chunk contains a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool HasComponent<T0, T1, T2, T3, T4>()
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
    {
        // Get a cached list of components that has been sorted
        var components = SortedListOfComponents<T0, T1, T2, T3, T4>.Components;

        // Check if they are all in the component set
        return Archetype.Components.IsSupersetOfSortedSpan(components.Span);
    }

    /// <summary>
    /// Check if this chunk contains a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool HasComponent<T0, T1, T2, T3, T4, T5>()
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
    {
        // Get a cached list of components that has been sorted
        var components = SortedListOfComponents<T0, T1, T2, T3, T4, T5>.Components;

        // Check if they are all in the component set
        return Archetype.Components.IsSupersetOfSortedSpan(components.Span);
    }

    /// <summary>
    /// Check if this chunk contains a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool HasComponent<T0, T1, T2, T3, T4, T5, T6>()
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
    {
        // Get a cached list of components that has been sorted
        var components = SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6>.Components;

        // Check if they are all in the component set
        return Archetype.Components.IsSupersetOfSortedSpan(components.Span);
    }

    /// <summary>
    /// Check if this chunk contains a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool HasComponent<T0, T1, T2, T3, T4, T5, T6, T7>()
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
    {
        // Get a cached list of components that has been sorted
        var components = SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6, T7>.Components;

        // Check if they are all in the component set
        return Archetype.Components.IsSupersetOfSortedSpan(components.Span);
    }

    /// <summary>
    /// Check if this chunk contains a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool HasComponent<T0, T1, T2, T3, T4, T5, T6, T7, T8>()
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
        // Get a cached list of components that has been sorted
        var components = SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8>.Components;

        // Check if they are all in the component set
        return Archetype.Components.IsSupersetOfSortedSpan(components.Span);
    }

    /// <summary>
    /// Check if this chunk contains a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool HasComponent<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>()
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
        // Get a cached list of components that has been sorted
        var components = SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>.Components;

        // Check if they are all in the component set
        return Archetype.Components.IsSupersetOfSortedSpan(components.Span);
    }

    /// <summary>
    /// Check if this chunk contains a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool HasComponent<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>()
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
        // Get a cached list of components that has been sorted
        var components = SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.Components;

        // Check if they are all in the component set
        return Archetype.Components.IsSupersetOfSortedSpan(components.Span);
    }

    /// <summary>
    /// Check if this chunk contains a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool HasComponent<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>()
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
        // Get a cached list of components that has been sorted
        var components = SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.Components;

        // Check if they are all in the component set
        return Archetype.Components.IsSupersetOfSortedSpan(components.Span);
    }

    /// <summary>
    /// Check if this chunk contains a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool HasComponent<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>()
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
        // Get a cached list of components that has been sorted
        var components = SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.Components;

        // Check if they are all in the component set
        return Archetype.Components.IsSupersetOfSortedSpan(components.Span);
    }

    /// <summary>
    /// Check if this chunk contains a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool HasComponent<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>()
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
        // Get a cached list of components that has been sorted
        var components = SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.Components;

        // Check if they are all in the component set
        return Archetype.Components.IsSupersetOfSortedSpan(components.Span);
    }

    /// <summary>
    /// Check if this chunk contains a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool HasComponent<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>()
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
        // Get a cached list of components that has been sorted
        var components = SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.Components;

        // Check if they are all in the component set
        return Archetype.Components.IsSupersetOfSortedSpan(components.Span);
    }

    /// <summary>
    /// Check if this chunk contains a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool HasComponent<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>()
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
        // Get a cached list of components that has been sorted
        var components = SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.Components;

        // Check if they are all in the component set
        return Archetype.Components.IsSupersetOfSortedSpan(components.Span);
    }
}

