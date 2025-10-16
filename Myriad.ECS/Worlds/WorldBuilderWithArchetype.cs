using Myriad.ECS.IDs;
using System.Diagnostics.CodeAnalysis;

namespace Myriad.ECS.Worlds;

/* dotcover disable */

public sealed partial class WorldBuilder
{
    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
    
    public WorldBuilder WithArchetype<T0>()
        where T0 : IComponent
    {
        var set = new HashSet<ComponentID>(1)
        {
            ComponentID<T0>.ID
        };


        AddArchetype(set);

        return this;
    }

    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
    
    public WorldBuilder WithArchetype<T0, T1>()
        where T0 : IComponent
        where T1 : IComponent
    {
        var set = new HashSet<ComponentID>(2)
        {
            ComponentID<T0>.ID
        };

        if (!set.Add(ComponentID<T1>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T1).Name}");

        AddArchetype(set);

        return this;
    }

    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public WorldBuilder WithArchetype<T0, T1, T2>()
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
    {
        var set = new HashSet<ComponentID>(3)
        {
            ComponentID<T0>.ID
        };

        if (!set.Add(ComponentID<T1>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T1).Name}");
        if (!set.Add(ComponentID<T2>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T2).Name}");

        AddArchetype(set);

        return this;
    }

    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public WorldBuilder WithArchetype<T0, T1, T2, T3>()
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
    {
        var set = new HashSet<ComponentID>(4)
        {
            ComponentID<T0>.ID
        };

        if (!set.Add(ComponentID<T1>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T1).Name}");
        if (!set.Add(ComponentID<T2>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T2).Name}");
        if (!set.Add(ComponentID<T3>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T3).Name}");

        AddArchetype(set);

        return this;
    }

    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public WorldBuilder WithArchetype<T0, T1, T2, T3, T4>()
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
    {
        var set = new HashSet<ComponentID>(5)
        {
            ComponentID<T0>.ID
        };

        if (!set.Add(ComponentID<T1>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T1).Name}");
        if (!set.Add(ComponentID<T2>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T2).Name}");
        if (!set.Add(ComponentID<T3>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T3).Name}");
        if (!set.Add(ComponentID<T4>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T4).Name}");

        AddArchetype(set);

        return this;
    }

    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public WorldBuilder WithArchetype<T0, T1, T2, T3, T4, T5>()
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
    {
        var set = new HashSet<ComponentID>(6)
        {
            ComponentID<T0>.ID
        };

        if (!set.Add(ComponentID<T1>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T1).Name}");
        if (!set.Add(ComponentID<T2>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T2).Name}");
        if (!set.Add(ComponentID<T3>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T3).Name}");
        if (!set.Add(ComponentID<T4>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T4).Name}");
        if (!set.Add(ComponentID<T5>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T5).Name}");

        AddArchetype(set);

        return this;
    }

    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public WorldBuilder WithArchetype<T0, T1, T2, T3, T4, T5, T6>()
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
    {
        var set = new HashSet<ComponentID>(7)
        {
            ComponentID<T0>.ID
        };

        if (!set.Add(ComponentID<T1>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T1).Name}");
        if (!set.Add(ComponentID<T2>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T2).Name}");
        if (!set.Add(ComponentID<T3>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T3).Name}");
        if (!set.Add(ComponentID<T4>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T4).Name}");
        if (!set.Add(ComponentID<T5>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T5).Name}");
        if (!set.Add(ComponentID<T6>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T6).Name}");

        AddArchetype(set);

        return this;
    }

    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public WorldBuilder WithArchetype<T0, T1, T2, T3, T4, T5, T6, T7>()
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
    {
        var set = new HashSet<ComponentID>(8)
        {
            ComponentID<T0>.ID
        };

        if (!set.Add(ComponentID<T1>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T1).Name}");
        if (!set.Add(ComponentID<T2>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T2).Name}");
        if (!set.Add(ComponentID<T3>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T3).Name}");
        if (!set.Add(ComponentID<T4>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T4).Name}");
        if (!set.Add(ComponentID<T5>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T5).Name}");
        if (!set.Add(ComponentID<T6>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T6).Name}");
        if (!set.Add(ComponentID<T7>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T7).Name}");

        AddArchetype(set);

        return this;
    }

    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public WorldBuilder WithArchetype<T0, T1, T2, T3, T4, T5, T6, T7, T8>()
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
        var set = new HashSet<ComponentID>(9)
        {
            ComponentID<T0>.ID
        };

        if (!set.Add(ComponentID<T1>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T1).Name}");
        if (!set.Add(ComponentID<T2>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T2).Name}");
        if (!set.Add(ComponentID<T3>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T3).Name}");
        if (!set.Add(ComponentID<T4>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T4).Name}");
        if (!set.Add(ComponentID<T5>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T5).Name}");
        if (!set.Add(ComponentID<T6>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T6).Name}");
        if (!set.Add(ComponentID<T7>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T7).Name}");
        if (!set.Add(ComponentID<T8>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T8).Name}");

        AddArchetype(set);

        return this;
    }

    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public WorldBuilder WithArchetype<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>()
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
        var set = new HashSet<ComponentID>(10)
        {
            ComponentID<T0>.ID
        };

        if (!set.Add(ComponentID<T1>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T1).Name}");
        if (!set.Add(ComponentID<T2>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T2).Name}");
        if (!set.Add(ComponentID<T3>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T3).Name}");
        if (!set.Add(ComponentID<T4>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T4).Name}");
        if (!set.Add(ComponentID<T5>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T5).Name}");
        if (!set.Add(ComponentID<T6>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T6).Name}");
        if (!set.Add(ComponentID<T7>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T7).Name}");
        if (!set.Add(ComponentID<T8>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T8).Name}");
        if (!set.Add(ComponentID<T9>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T9).Name}");

        AddArchetype(set);

        return this;
    }

    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public WorldBuilder WithArchetype<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>()
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
        var set = new HashSet<ComponentID>(11)
        {
            ComponentID<T0>.ID
        };

        if (!set.Add(ComponentID<T1>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T1).Name}");
        if (!set.Add(ComponentID<T2>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T2).Name}");
        if (!set.Add(ComponentID<T3>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T3).Name}");
        if (!set.Add(ComponentID<T4>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T4).Name}");
        if (!set.Add(ComponentID<T5>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T5).Name}");
        if (!set.Add(ComponentID<T6>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T6).Name}");
        if (!set.Add(ComponentID<T7>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T7).Name}");
        if (!set.Add(ComponentID<T8>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T8).Name}");
        if (!set.Add(ComponentID<T9>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T9).Name}");
        if (!set.Add(ComponentID<T10>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T10).Name}");

        AddArchetype(set);

        return this;
    }

    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public WorldBuilder WithArchetype<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>()
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
        var set = new HashSet<ComponentID>(12)
        {
            ComponentID<T0>.ID
        };

        if (!set.Add(ComponentID<T1>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T1).Name}");
        if (!set.Add(ComponentID<T2>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T2).Name}");
        if (!set.Add(ComponentID<T3>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T3).Name}");
        if (!set.Add(ComponentID<T4>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T4).Name}");
        if (!set.Add(ComponentID<T5>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T5).Name}");
        if (!set.Add(ComponentID<T6>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T6).Name}");
        if (!set.Add(ComponentID<T7>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T7).Name}");
        if (!set.Add(ComponentID<T8>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T8).Name}");
        if (!set.Add(ComponentID<T9>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T9).Name}");
        if (!set.Add(ComponentID<T10>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T10).Name}");
        if (!set.Add(ComponentID<T11>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T11).Name}");

        AddArchetype(set);

        return this;
    }

    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public WorldBuilder WithArchetype<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>()
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
        var set = new HashSet<ComponentID>(13)
        {
            ComponentID<T0>.ID
        };

        if (!set.Add(ComponentID<T1>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T1).Name}");
        if (!set.Add(ComponentID<T2>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T2).Name}");
        if (!set.Add(ComponentID<T3>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T3).Name}");
        if (!set.Add(ComponentID<T4>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T4).Name}");
        if (!set.Add(ComponentID<T5>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T5).Name}");
        if (!set.Add(ComponentID<T6>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T6).Name}");
        if (!set.Add(ComponentID<T7>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T7).Name}");
        if (!set.Add(ComponentID<T8>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T8).Name}");
        if (!set.Add(ComponentID<T9>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T9).Name}");
        if (!set.Add(ComponentID<T10>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T10).Name}");
        if (!set.Add(ComponentID<T11>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T11).Name}");
        if (!set.Add(ComponentID<T12>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T12).Name}");

        AddArchetype(set);

        return this;
    }

    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public WorldBuilder WithArchetype<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>()
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
        var set = new HashSet<ComponentID>(14)
        {
            ComponentID<T0>.ID
        };

        if (!set.Add(ComponentID<T1>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T1).Name}");
        if (!set.Add(ComponentID<T2>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T2).Name}");
        if (!set.Add(ComponentID<T3>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T3).Name}");
        if (!set.Add(ComponentID<T4>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T4).Name}");
        if (!set.Add(ComponentID<T5>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T5).Name}");
        if (!set.Add(ComponentID<T6>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T6).Name}");
        if (!set.Add(ComponentID<T7>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T7).Name}");
        if (!set.Add(ComponentID<T8>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T8).Name}");
        if (!set.Add(ComponentID<T9>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T9).Name}");
        if (!set.Add(ComponentID<T10>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T10).Name}");
        if (!set.Add(ComponentID<T11>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T11).Name}");
        if (!set.Add(ComponentID<T12>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T12).Name}");
        if (!set.Add(ComponentID<T13>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T13).Name}");

        AddArchetype(set);

        return this;
    }

    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public WorldBuilder WithArchetype<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>()
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
        var set = new HashSet<ComponentID>(15)
        {
            ComponentID<T0>.ID
        };

        if (!set.Add(ComponentID<T1>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T1).Name}");
        if (!set.Add(ComponentID<T2>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T2).Name}");
        if (!set.Add(ComponentID<T3>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T3).Name}");
        if (!set.Add(ComponentID<T4>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T4).Name}");
        if (!set.Add(ComponentID<T5>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T5).Name}");
        if (!set.Add(ComponentID<T6>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T6).Name}");
        if (!set.Add(ComponentID<T7>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T7).Name}");
        if (!set.Add(ComponentID<T8>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T8).Name}");
        if (!set.Add(ComponentID<T9>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T9).Name}");
        if (!set.Add(ComponentID<T10>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T10).Name}");
        if (!set.Add(ComponentID<T11>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T11).Name}");
        if (!set.Add(ComponentID<T12>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T12).Name}");
        if (!set.Add(ComponentID<T13>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T13).Name}");
        if (!set.Add(ComponentID<T14>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T14).Name}");

        AddArchetype(set);

        return this;
    }

    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public WorldBuilder WithArchetype<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>()
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
        var set = new HashSet<ComponentID>(16)
        {
            ComponentID<T0>.ID
        };

        if (!set.Add(ComponentID<T1>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T1).Name}");
        if (!set.Add(ComponentID<T2>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T2).Name}");
        if (!set.Add(ComponentID<T3>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T3).Name}");
        if (!set.Add(ComponentID<T4>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T4).Name}");
        if (!set.Add(ComponentID<T5>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T5).Name}");
        if (!set.Add(ComponentID<T6>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T6).Name}");
        if (!set.Add(ComponentID<T7>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T7).Name}");
        if (!set.Add(ComponentID<T8>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T8).Name}");
        if (!set.Add(ComponentID<T9>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T9).Name}");
        if (!set.Add(ComponentID<T10>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T10).Name}");
        if (!set.Add(ComponentID<T11>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T11).Name}");
        if (!set.Add(ComponentID<T12>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T12).Name}");
        if (!set.Add(ComponentID<T13>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T13).Name}");
        if (!set.Add(ComponentID<T14>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T14).Name}");
        if (!set.Add(ComponentID<T15>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T15).Name}");

        AddArchetype(set);

        return this;
    }

    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public WorldBuilder WithArchetype<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>()
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
        where T16 : IComponent
    {
        var set = new HashSet<ComponentID>(17)
        {
            ComponentID<T0>.ID
        };

        if (!set.Add(ComponentID<T1>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T1).Name}");
        if (!set.Add(ComponentID<T2>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T2).Name}");
        if (!set.Add(ComponentID<T3>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T3).Name}");
        if (!set.Add(ComponentID<T4>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T4).Name}");
        if (!set.Add(ComponentID<T5>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T5).Name}");
        if (!set.Add(ComponentID<T6>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T6).Name}");
        if (!set.Add(ComponentID<T7>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T7).Name}");
        if (!set.Add(ComponentID<T8>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T8).Name}");
        if (!set.Add(ComponentID<T9>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T9).Name}");
        if (!set.Add(ComponentID<T10>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T10).Name}");
        if (!set.Add(ComponentID<T11>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T11).Name}");
        if (!set.Add(ComponentID<T12>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T12).Name}");
        if (!set.Add(ComponentID<T13>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T13).Name}");
        if (!set.Add(ComponentID<T14>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T14).Name}");
        if (!set.Add(ComponentID<T15>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T15).Name}");
        if (!set.Add(ComponentID<T16>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T16).Name}");

        AddArchetype(set);

        return this;
    }

    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public WorldBuilder WithArchetype<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>()
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
        where T16 : IComponent
        where T17 : IComponent
    {
        var set = new HashSet<ComponentID>(18)
        {
            ComponentID<T0>.ID
        };

        if (!set.Add(ComponentID<T1>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T1).Name}");
        if (!set.Add(ComponentID<T2>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T2).Name}");
        if (!set.Add(ComponentID<T3>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T3).Name}");
        if (!set.Add(ComponentID<T4>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T4).Name}");
        if (!set.Add(ComponentID<T5>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T5).Name}");
        if (!set.Add(ComponentID<T6>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T6).Name}");
        if (!set.Add(ComponentID<T7>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T7).Name}");
        if (!set.Add(ComponentID<T8>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T8).Name}");
        if (!set.Add(ComponentID<T9>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T9).Name}");
        if (!set.Add(ComponentID<T10>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T10).Name}");
        if (!set.Add(ComponentID<T11>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T11).Name}");
        if (!set.Add(ComponentID<T12>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T12).Name}");
        if (!set.Add(ComponentID<T13>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T13).Name}");
        if (!set.Add(ComponentID<T14>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T14).Name}");
        if (!set.Add(ComponentID<T15>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T15).Name}");
        if (!set.Add(ComponentID<T16>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T16).Name}");
        if (!set.Add(ComponentID<T17>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T17).Name}");

        AddArchetype(set);

        return this;
    }

    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public WorldBuilder WithArchetype<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>()
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
        where T16 : IComponent
        where T17 : IComponent
        where T18 : IComponent
    {
        var set = new HashSet<ComponentID>(19)
        {
            ComponentID<T0>.ID
        };

        if (!set.Add(ComponentID<T1>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T1).Name}");
        if (!set.Add(ComponentID<T2>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T2).Name}");
        if (!set.Add(ComponentID<T3>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T3).Name}");
        if (!set.Add(ComponentID<T4>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T4).Name}");
        if (!set.Add(ComponentID<T5>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T5).Name}");
        if (!set.Add(ComponentID<T6>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T6).Name}");
        if (!set.Add(ComponentID<T7>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T7).Name}");
        if (!set.Add(ComponentID<T8>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T8).Name}");
        if (!set.Add(ComponentID<T9>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T9).Name}");
        if (!set.Add(ComponentID<T10>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T10).Name}");
        if (!set.Add(ComponentID<T11>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T11).Name}");
        if (!set.Add(ComponentID<T12>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T12).Name}");
        if (!set.Add(ComponentID<T13>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T13).Name}");
        if (!set.Add(ComponentID<T14>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T14).Name}");
        if (!set.Add(ComponentID<T15>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T15).Name}");
        if (!set.Add(ComponentID<T16>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T16).Name}");
        if (!set.Add(ComponentID<T17>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T17).Name}");
        if (!set.Add(ComponentID<T18>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T18).Name}");

        AddArchetype(set);

        return this;
    }

    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public WorldBuilder WithArchetype<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>()
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
        where T16 : IComponent
        where T17 : IComponent
        where T18 : IComponent
        where T19 : IComponent
    {
        var set = new HashSet<ComponentID>(20)
        {
            ComponentID<T0>.ID
        };

        if (!set.Add(ComponentID<T1>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T1).Name}");
        if (!set.Add(ComponentID<T2>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T2).Name}");
        if (!set.Add(ComponentID<T3>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T3).Name}");
        if (!set.Add(ComponentID<T4>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T4).Name}");
        if (!set.Add(ComponentID<T5>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T5).Name}");
        if (!set.Add(ComponentID<T6>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T6).Name}");
        if (!set.Add(ComponentID<T7>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T7).Name}");
        if (!set.Add(ComponentID<T8>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T8).Name}");
        if (!set.Add(ComponentID<T9>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T9).Name}");
        if (!set.Add(ComponentID<T10>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T10).Name}");
        if (!set.Add(ComponentID<T11>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T11).Name}");
        if (!set.Add(ComponentID<T12>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T12).Name}");
        if (!set.Add(ComponentID<T13>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T13).Name}");
        if (!set.Add(ComponentID<T14>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T14).Name}");
        if (!set.Add(ComponentID<T15>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T15).Name}");
        if (!set.Add(ComponentID<T16>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T16).Name}");
        if (!set.Add(ComponentID<T17>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T17).Name}");
        if (!set.Add(ComponentID<T18>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T18).Name}");
        if (!set.Add(ComponentID<T19>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T19).Name}");

        AddArchetype(set);

        return this;
    }

    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public WorldBuilder WithArchetype<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>()
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
        where T16 : IComponent
        where T17 : IComponent
        where T18 : IComponent
        where T19 : IComponent
        where T20 : IComponent
    {
        var set = new HashSet<ComponentID>(21)
        {
            ComponentID<T0>.ID
        };

        if (!set.Add(ComponentID<T1>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T1).Name}");
        if (!set.Add(ComponentID<T2>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T2).Name}");
        if (!set.Add(ComponentID<T3>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T3).Name}");
        if (!set.Add(ComponentID<T4>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T4).Name}");
        if (!set.Add(ComponentID<T5>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T5).Name}");
        if (!set.Add(ComponentID<T6>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T6).Name}");
        if (!set.Add(ComponentID<T7>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T7).Name}");
        if (!set.Add(ComponentID<T8>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T8).Name}");
        if (!set.Add(ComponentID<T9>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T9).Name}");
        if (!set.Add(ComponentID<T10>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T10).Name}");
        if (!set.Add(ComponentID<T11>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T11).Name}");
        if (!set.Add(ComponentID<T12>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T12).Name}");
        if (!set.Add(ComponentID<T13>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T13).Name}");
        if (!set.Add(ComponentID<T14>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T14).Name}");
        if (!set.Add(ComponentID<T15>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T15).Name}");
        if (!set.Add(ComponentID<T16>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T16).Name}");
        if (!set.Add(ComponentID<T17>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T17).Name}");
        if (!set.Add(ComponentID<T18>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T18).Name}");
        if (!set.Add(ComponentID<T19>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T19).Name}");
        if (!set.Add(ComponentID<T20>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T20).Name}");

        AddArchetype(set);

        return this;
    }

    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public WorldBuilder WithArchetype<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>()
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
        where T16 : IComponent
        where T17 : IComponent
        where T18 : IComponent
        where T19 : IComponent
        where T20 : IComponent
        where T21 : IComponent
    {
        var set = new HashSet<ComponentID>(22)
        {
            ComponentID<T0>.ID
        };

        if (!set.Add(ComponentID<T1>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T1).Name}");
        if (!set.Add(ComponentID<T2>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T2).Name}");
        if (!set.Add(ComponentID<T3>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T3).Name}");
        if (!set.Add(ComponentID<T4>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T4).Name}");
        if (!set.Add(ComponentID<T5>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T5).Name}");
        if (!set.Add(ComponentID<T6>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T6).Name}");
        if (!set.Add(ComponentID<T7>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T7).Name}");
        if (!set.Add(ComponentID<T8>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T8).Name}");
        if (!set.Add(ComponentID<T9>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T9).Name}");
        if (!set.Add(ComponentID<T10>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T10).Name}");
        if (!set.Add(ComponentID<T11>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T11).Name}");
        if (!set.Add(ComponentID<T12>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T12).Name}");
        if (!set.Add(ComponentID<T13>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T13).Name}");
        if (!set.Add(ComponentID<T14>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T14).Name}");
        if (!set.Add(ComponentID<T15>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T15).Name}");
        if (!set.Add(ComponentID<T16>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T16).Name}");
        if (!set.Add(ComponentID<T17>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T17).Name}");
        if (!set.Add(ComponentID<T18>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T18).Name}");
        if (!set.Add(ComponentID<T19>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T19).Name}");
        if (!set.Add(ComponentID<T20>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T20).Name}");
        if (!set.Add(ComponentID<T21>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T21).Name}");

        AddArchetype(set);

        return this;
    }

    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public WorldBuilder WithArchetype<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>()
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
        where T16 : IComponent
        where T17 : IComponent
        where T18 : IComponent
        where T19 : IComponent
        where T20 : IComponent
        where T21 : IComponent
        where T22 : IComponent
    {
        var set = new HashSet<ComponentID>(23)
        {
            ComponentID<T0>.ID
        };

        if (!set.Add(ComponentID<T1>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T1).Name}");
        if (!set.Add(ComponentID<T2>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T2).Name}");
        if (!set.Add(ComponentID<T3>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T3).Name}");
        if (!set.Add(ComponentID<T4>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T4).Name}");
        if (!set.Add(ComponentID<T5>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T5).Name}");
        if (!set.Add(ComponentID<T6>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T6).Name}");
        if (!set.Add(ComponentID<T7>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T7).Name}");
        if (!set.Add(ComponentID<T8>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T8).Name}");
        if (!set.Add(ComponentID<T9>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T9).Name}");
        if (!set.Add(ComponentID<T10>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T10).Name}");
        if (!set.Add(ComponentID<T11>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T11).Name}");
        if (!set.Add(ComponentID<T12>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T12).Name}");
        if (!set.Add(ComponentID<T13>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T13).Name}");
        if (!set.Add(ComponentID<T14>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T14).Name}");
        if (!set.Add(ComponentID<T15>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T15).Name}");
        if (!set.Add(ComponentID<T16>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T16).Name}");
        if (!set.Add(ComponentID<T17>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T17).Name}");
        if (!set.Add(ComponentID<T18>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T18).Name}");
        if (!set.Add(ComponentID<T19>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T19).Name}");
        if (!set.Add(ComponentID<T20>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T20).Name}");
        if (!set.Add(ComponentID<T21>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T21).Name}");
        if (!set.Add(ComponentID<T22>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T22).Name}");

        AddArchetype(set);

        return this;
    }

    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public WorldBuilder WithArchetype<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>()
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
        where T16 : IComponent
        where T17 : IComponent
        where T18 : IComponent
        where T19 : IComponent
        where T20 : IComponent
        where T21 : IComponent
        where T22 : IComponent
        where T23 : IComponent
    {
        var set = new HashSet<ComponentID>(24)
        {
            ComponentID<T0>.ID
        };

        if (!set.Add(ComponentID<T1>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T1).Name}");
        if (!set.Add(ComponentID<T2>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T2).Name}");
        if (!set.Add(ComponentID<T3>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T3).Name}");
        if (!set.Add(ComponentID<T4>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T4).Name}");
        if (!set.Add(ComponentID<T5>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T5).Name}");
        if (!set.Add(ComponentID<T6>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T6).Name}");
        if (!set.Add(ComponentID<T7>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T7).Name}");
        if (!set.Add(ComponentID<T8>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T8).Name}");
        if (!set.Add(ComponentID<T9>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T9).Name}");
        if (!set.Add(ComponentID<T10>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T10).Name}");
        if (!set.Add(ComponentID<T11>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T11).Name}");
        if (!set.Add(ComponentID<T12>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T12).Name}");
        if (!set.Add(ComponentID<T13>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T13).Name}");
        if (!set.Add(ComponentID<T14>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T14).Name}");
        if (!set.Add(ComponentID<T15>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T15).Name}");
        if (!set.Add(ComponentID<T16>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T16).Name}");
        if (!set.Add(ComponentID<T17>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T17).Name}");
        if (!set.Add(ComponentID<T18>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T18).Name}");
        if (!set.Add(ComponentID<T19>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T19).Name}");
        if (!set.Add(ComponentID<T20>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T20).Name}");
        if (!set.Add(ComponentID<T21>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T21).Name}");
        if (!set.Add(ComponentID<T22>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T22).Name}");
        if (!set.Add(ComponentID<T23>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T23).Name}");

        AddArchetype(set);

        return this;
    }

    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public WorldBuilder WithArchetype<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>()
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
        where T16 : IComponent
        where T17 : IComponent
        where T18 : IComponent
        where T19 : IComponent
        where T20 : IComponent
        where T21 : IComponent
        where T22 : IComponent
        where T23 : IComponent
        where T24 : IComponent
    {
        var set = new HashSet<ComponentID>(25)
        {
            ComponentID<T0>.ID
        };

        if (!set.Add(ComponentID<T1>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T1).Name}");
        if (!set.Add(ComponentID<T2>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T2).Name}");
        if (!set.Add(ComponentID<T3>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T3).Name}");
        if (!set.Add(ComponentID<T4>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T4).Name}");
        if (!set.Add(ComponentID<T5>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T5).Name}");
        if (!set.Add(ComponentID<T6>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T6).Name}");
        if (!set.Add(ComponentID<T7>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T7).Name}");
        if (!set.Add(ComponentID<T8>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T8).Name}");
        if (!set.Add(ComponentID<T9>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T9).Name}");
        if (!set.Add(ComponentID<T10>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T10).Name}");
        if (!set.Add(ComponentID<T11>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T11).Name}");
        if (!set.Add(ComponentID<T12>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T12).Name}");
        if (!set.Add(ComponentID<T13>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T13).Name}");
        if (!set.Add(ComponentID<T14>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T14).Name}");
        if (!set.Add(ComponentID<T15>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T15).Name}");
        if (!set.Add(ComponentID<T16>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T16).Name}");
        if (!set.Add(ComponentID<T17>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T17).Name}");
        if (!set.Add(ComponentID<T18>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T18).Name}");
        if (!set.Add(ComponentID<T19>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T19).Name}");
        if (!set.Add(ComponentID<T20>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T20).Name}");
        if (!set.Add(ComponentID<T21>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T21).Name}");
        if (!set.Add(ComponentID<T22>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T22).Name}");
        if (!set.Add(ComponentID<T23>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T23).Name}");
        if (!set.Add(ComponentID<T24>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T24).Name}");

        AddArchetype(set);

        return this;
    }

    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public WorldBuilder WithArchetype<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25>()
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
        where T16 : IComponent
        where T17 : IComponent
        where T18 : IComponent
        where T19 : IComponent
        where T20 : IComponent
        where T21 : IComponent
        where T22 : IComponent
        where T23 : IComponent
        where T24 : IComponent
        where T25 : IComponent
    {
        var set = new HashSet<ComponentID>(26)
        {
            ComponentID<T0>.ID
        };

        if (!set.Add(ComponentID<T1>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T1).Name}");
        if (!set.Add(ComponentID<T2>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T2).Name}");
        if (!set.Add(ComponentID<T3>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T3).Name}");
        if (!set.Add(ComponentID<T4>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T4).Name}");
        if (!set.Add(ComponentID<T5>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T5).Name}");
        if (!set.Add(ComponentID<T6>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T6).Name}");
        if (!set.Add(ComponentID<T7>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T7).Name}");
        if (!set.Add(ComponentID<T8>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T8).Name}");
        if (!set.Add(ComponentID<T9>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T9).Name}");
        if (!set.Add(ComponentID<T10>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T10).Name}");
        if (!set.Add(ComponentID<T11>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T11).Name}");
        if (!set.Add(ComponentID<T12>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T12).Name}");
        if (!set.Add(ComponentID<T13>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T13).Name}");
        if (!set.Add(ComponentID<T14>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T14).Name}");
        if (!set.Add(ComponentID<T15>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T15).Name}");
        if (!set.Add(ComponentID<T16>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T16).Name}");
        if (!set.Add(ComponentID<T17>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T17).Name}");
        if (!set.Add(ComponentID<T18>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T18).Name}");
        if (!set.Add(ComponentID<T19>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T19).Name}");
        if (!set.Add(ComponentID<T20>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T20).Name}");
        if (!set.Add(ComponentID<T21>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T21).Name}");
        if (!set.Add(ComponentID<T22>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T22).Name}");
        if (!set.Add(ComponentID<T23>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T23).Name}");
        if (!set.Add(ComponentID<T24>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T24).Name}");
        if (!set.Add(ComponentID<T25>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T25).Name}");

        AddArchetype(set);

        return this;
    }

    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public WorldBuilder WithArchetype<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26>()
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
        where T16 : IComponent
        where T17 : IComponent
        where T18 : IComponent
        where T19 : IComponent
        where T20 : IComponent
        where T21 : IComponent
        where T22 : IComponent
        where T23 : IComponent
        where T24 : IComponent
        where T25 : IComponent
        where T26 : IComponent
    {
        var set = new HashSet<ComponentID>(27)
        {
            ComponentID<T0>.ID
        };

        if (!set.Add(ComponentID<T1>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T1).Name}");
        if (!set.Add(ComponentID<T2>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T2).Name}");
        if (!set.Add(ComponentID<T3>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T3).Name}");
        if (!set.Add(ComponentID<T4>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T4).Name}");
        if (!set.Add(ComponentID<T5>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T5).Name}");
        if (!set.Add(ComponentID<T6>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T6).Name}");
        if (!set.Add(ComponentID<T7>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T7).Name}");
        if (!set.Add(ComponentID<T8>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T8).Name}");
        if (!set.Add(ComponentID<T9>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T9).Name}");
        if (!set.Add(ComponentID<T10>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T10).Name}");
        if (!set.Add(ComponentID<T11>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T11).Name}");
        if (!set.Add(ComponentID<T12>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T12).Name}");
        if (!set.Add(ComponentID<T13>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T13).Name}");
        if (!set.Add(ComponentID<T14>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T14).Name}");
        if (!set.Add(ComponentID<T15>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T15).Name}");
        if (!set.Add(ComponentID<T16>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T16).Name}");
        if (!set.Add(ComponentID<T17>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T17).Name}");
        if (!set.Add(ComponentID<T18>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T18).Name}");
        if (!set.Add(ComponentID<T19>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T19).Name}");
        if (!set.Add(ComponentID<T20>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T20).Name}");
        if (!set.Add(ComponentID<T21>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T21).Name}");
        if (!set.Add(ComponentID<T22>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T22).Name}");
        if (!set.Add(ComponentID<T23>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T23).Name}");
        if (!set.Add(ComponentID<T24>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T24).Name}");
        if (!set.Add(ComponentID<T25>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T25).Name}");
        if (!set.Add(ComponentID<T26>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T26).Name}");

        AddArchetype(set);

        return this;
    }

    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public WorldBuilder WithArchetype<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27>()
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
        where T16 : IComponent
        where T17 : IComponent
        where T18 : IComponent
        where T19 : IComponent
        where T20 : IComponent
        where T21 : IComponent
        where T22 : IComponent
        where T23 : IComponent
        where T24 : IComponent
        where T25 : IComponent
        where T26 : IComponent
        where T27 : IComponent
    {
        var set = new HashSet<ComponentID>(28)
        {
            ComponentID<T0>.ID
        };

        if (!set.Add(ComponentID<T1>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T1).Name}");
        if (!set.Add(ComponentID<T2>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T2).Name}");
        if (!set.Add(ComponentID<T3>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T3).Name}");
        if (!set.Add(ComponentID<T4>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T4).Name}");
        if (!set.Add(ComponentID<T5>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T5).Name}");
        if (!set.Add(ComponentID<T6>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T6).Name}");
        if (!set.Add(ComponentID<T7>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T7).Name}");
        if (!set.Add(ComponentID<T8>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T8).Name}");
        if (!set.Add(ComponentID<T9>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T9).Name}");
        if (!set.Add(ComponentID<T10>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T10).Name}");
        if (!set.Add(ComponentID<T11>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T11).Name}");
        if (!set.Add(ComponentID<T12>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T12).Name}");
        if (!set.Add(ComponentID<T13>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T13).Name}");
        if (!set.Add(ComponentID<T14>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T14).Name}");
        if (!set.Add(ComponentID<T15>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T15).Name}");
        if (!set.Add(ComponentID<T16>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T16).Name}");
        if (!set.Add(ComponentID<T17>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T17).Name}");
        if (!set.Add(ComponentID<T18>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T18).Name}");
        if (!set.Add(ComponentID<T19>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T19).Name}");
        if (!set.Add(ComponentID<T20>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T20).Name}");
        if (!set.Add(ComponentID<T21>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T21).Name}");
        if (!set.Add(ComponentID<T22>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T22).Name}");
        if (!set.Add(ComponentID<T23>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T23).Name}");
        if (!set.Add(ComponentID<T24>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T24).Name}");
        if (!set.Add(ComponentID<T25>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T25).Name}");
        if (!set.Add(ComponentID<T26>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T26).Name}");
        if (!set.Add(ComponentID<T27>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T27).Name}");

        AddArchetype(set);

        return this;
    }

    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public WorldBuilder WithArchetype<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28>()
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
        where T16 : IComponent
        where T17 : IComponent
        where T18 : IComponent
        where T19 : IComponent
        where T20 : IComponent
        where T21 : IComponent
        where T22 : IComponent
        where T23 : IComponent
        where T24 : IComponent
        where T25 : IComponent
        where T26 : IComponent
        where T27 : IComponent
        where T28 : IComponent
    {
        var set = new HashSet<ComponentID>(29)
        {
            ComponentID<T0>.ID
        };

        if (!set.Add(ComponentID<T1>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T1).Name}");
        if (!set.Add(ComponentID<T2>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T2).Name}");
        if (!set.Add(ComponentID<T3>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T3).Name}");
        if (!set.Add(ComponentID<T4>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T4).Name}");
        if (!set.Add(ComponentID<T5>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T5).Name}");
        if (!set.Add(ComponentID<T6>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T6).Name}");
        if (!set.Add(ComponentID<T7>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T7).Name}");
        if (!set.Add(ComponentID<T8>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T8).Name}");
        if (!set.Add(ComponentID<T9>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T9).Name}");
        if (!set.Add(ComponentID<T10>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T10).Name}");
        if (!set.Add(ComponentID<T11>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T11).Name}");
        if (!set.Add(ComponentID<T12>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T12).Name}");
        if (!set.Add(ComponentID<T13>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T13).Name}");
        if (!set.Add(ComponentID<T14>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T14).Name}");
        if (!set.Add(ComponentID<T15>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T15).Name}");
        if (!set.Add(ComponentID<T16>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T16).Name}");
        if (!set.Add(ComponentID<T17>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T17).Name}");
        if (!set.Add(ComponentID<T18>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T18).Name}");
        if (!set.Add(ComponentID<T19>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T19).Name}");
        if (!set.Add(ComponentID<T20>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T20).Name}");
        if (!set.Add(ComponentID<T21>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T21).Name}");
        if (!set.Add(ComponentID<T22>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T22).Name}");
        if (!set.Add(ComponentID<T23>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T23).Name}");
        if (!set.Add(ComponentID<T24>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T24).Name}");
        if (!set.Add(ComponentID<T25>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T25).Name}");
        if (!set.Add(ComponentID<T26>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T26).Name}");
        if (!set.Add(ComponentID<T27>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T27).Name}");
        if (!set.Add(ComponentID<T28>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T28).Name}");

        AddArchetype(set);

        return this;
    }

    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public WorldBuilder WithArchetype<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29>()
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
        where T16 : IComponent
        where T17 : IComponent
        where T18 : IComponent
        where T19 : IComponent
        where T20 : IComponent
        where T21 : IComponent
        where T22 : IComponent
        where T23 : IComponent
        where T24 : IComponent
        where T25 : IComponent
        where T26 : IComponent
        where T27 : IComponent
        where T28 : IComponent
        where T29 : IComponent
    {
        var set = new HashSet<ComponentID>(30)
        {
            ComponentID<T0>.ID
        };

        if (!set.Add(ComponentID<T1>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T1).Name}");
        if (!set.Add(ComponentID<T2>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T2).Name}");
        if (!set.Add(ComponentID<T3>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T3).Name}");
        if (!set.Add(ComponentID<T4>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T4).Name}");
        if (!set.Add(ComponentID<T5>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T5).Name}");
        if (!set.Add(ComponentID<T6>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T6).Name}");
        if (!set.Add(ComponentID<T7>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T7).Name}");
        if (!set.Add(ComponentID<T8>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T8).Name}");
        if (!set.Add(ComponentID<T9>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T9).Name}");
        if (!set.Add(ComponentID<T10>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T10).Name}");
        if (!set.Add(ComponentID<T11>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T11).Name}");
        if (!set.Add(ComponentID<T12>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T12).Name}");
        if (!set.Add(ComponentID<T13>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T13).Name}");
        if (!set.Add(ComponentID<T14>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T14).Name}");
        if (!set.Add(ComponentID<T15>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T15).Name}");
        if (!set.Add(ComponentID<T16>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T16).Name}");
        if (!set.Add(ComponentID<T17>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T17).Name}");
        if (!set.Add(ComponentID<T18>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T18).Name}");
        if (!set.Add(ComponentID<T19>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T19).Name}");
        if (!set.Add(ComponentID<T20>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T20).Name}");
        if (!set.Add(ComponentID<T21>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T21).Name}");
        if (!set.Add(ComponentID<T22>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T22).Name}");
        if (!set.Add(ComponentID<T23>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T23).Name}");
        if (!set.Add(ComponentID<T24>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T24).Name}");
        if (!set.Add(ComponentID<T25>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T25).Name}");
        if (!set.Add(ComponentID<T26>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T26).Name}");
        if (!set.Add(ComponentID<T27>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T27).Name}");
        if (!set.Add(ComponentID<T28>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T28).Name}");
        if (!set.Add(ComponentID<T29>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T29).Name}");

        AddArchetype(set);

        return this;
    }

    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public WorldBuilder WithArchetype<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30>()
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
        where T16 : IComponent
        where T17 : IComponent
        where T18 : IComponent
        where T19 : IComponent
        where T20 : IComponent
        where T21 : IComponent
        where T22 : IComponent
        where T23 : IComponent
        where T24 : IComponent
        where T25 : IComponent
        where T26 : IComponent
        where T27 : IComponent
        where T28 : IComponent
        where T29 : IComponent
        where T30 : IComponent
    {
        var set = new HashSet<ComponentID>(31)
        {
            ComponentID<T0>.ID
        };

        if (!set.Add(ComponentID<T1>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T1).Name}");
        if (!set.Add(ComponentID<T2>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T2).Name}");
        if (!set.Add(ComponentID<T3>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T3).Name}");
        if (!set.Add(ComponentID<T4>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T4).Name}");
        if (!set.Add(ComponentID<T5>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T5).Name}");
        if (!set.Add(ComponentID<T6>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T6).Name}");
        if (!set.Add(ComponentID<T7>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T7).Name}");
        if (!set.Add(ComponentID<T8>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T8).Name}");
        if (!set.Add(ComponentID<T9>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T9).Name}");
        if (!set.Add(ComponentID<T10>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T10).Name}");
        if (!set.Add(ComponentID<T11>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T11).Name}");
        if (!set.Add(ComponentID<T12>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T12).Name}");
        if (!set.Add(ComponentID<T13>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T13).Name}");
        if (!set.Add(ComponentID<T14>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T14).Name}");
        if (!set.Add(ComponentID<T15>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T15).Name}");
        if (!set.Add(ComponentID<T16>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T16).Name}");
        if (!set.Add(ComponentID<T17>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T17).Name}");
        if (!set.Add(ComponentID<T18>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T18).Name}");
        if (!set.Add(ComponentID<T19>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T19).Name}");
        if (!set.Add(ComponentID<T20>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T20).Name}");
        if (!set.Add(ComponentID<T21>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T21).Name}");
        if (!set.Add(ComponentID<T22>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T22).Name}");
        if (!set.Add(ComponentID<T23>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T23).Name}");
        if (!set.Add(ComponentID<T24>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T24).Name}");
        if (!set.Add(ComponentID<T25>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T25).Name}");
        if (!set.Add(ComponentID<T26>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T26).Name}");
        if (!set.Add(ComponentID<T27>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T27).Name}");
        if (!set.Add(ComponentID<T28>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T28).Name}");
        if (!set.Add(ComponentID<T29>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T29).Name}");
        if (!set.Add(ComponentID<T30>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T30).Name}");

        AddArchetype(set);

        return this;
    }

    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public WorldBuilder WithArchetype<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31>()
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
        where T16 : IComponent
        where T17 : IComponent
        where T18 : IComponent
        where T19 : IComponent
        where T20 : IComponent
        where T21 : IComponent
        where T22 : IComponent
        where T23 : IComponent
        where T24 : IComponent
        where T25 : IComponent
        where T26 : IComponent
        where T27 : IComponent
        where T28 : IComponent
        where T29 : IComponent
        where T30 : IComponent
        where T31 : IComponent
    {
        var set = new HashSet<ComponentID>(32)
        {
            ComponentID<T0>.ID
        };

        if (!set.Add(ComponentID<T1>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T1).Name}");
        if (!set.Add(ComponentID<T2>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T2).Name}");
        if (!set.Add(ComponentID<T3>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T3).Name}");
        if (!set.Add(ComponentID<T4>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T4).Name}");
        if (!set.Add(ComponentID<T5>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T5).Name}");
        if (!set.Add(ComponentID<T6>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T6).Name}");
        if (!set.Add(ComponentID<T7>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T7).Name}");
        if (!set.Add(ComponentID<T8>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T8).Name}");
        if (!set.Add(ComponentID<T9>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T9).Name}");
        if (!set.Add(ComponentID<T10>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T10).Name}");
        if (!set.Add(ComponentID<T11>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T11).Name}");
        if (!set.Add(ComponentID<T12>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T12).Name}");
        if (!set.Add(ComponentID<T13>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T13).Name}");
        if (!set.Add(ComponentID<T14>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T14).Name}");
        if (!set.Add(ComponentID<T15>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T15).Name}");
        if (!set.Add(ComponentID<T16>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T16).Name}");
        if (!set.Add(ComponentID<T17>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T17).Name}");
        if (!set.Add(ComponentID<T18>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T18).Name}");
        if (!set.Add(ComponentID<T19>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T19).Name}");
        if (!set.Add(ComponentID<T20>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T20).Name}");
        if (!set.Add(ComponentID<T21>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T21).Name}");
        if (!set.Add(ComponentID<T22>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T22).Name}");
        if (!set.Add(ComponentID<T23>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T23).Name}");
        if (!set.Add(ComponentID<T24>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T24).Name}");
        if (!set.Add(ComponentID<T25>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T25).Name}");
        if (!set.Add(ComponentID<T26>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T26).Name}");
        if (!set.Add(ComponentID<T27>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T27).Name}");
        if (!set.Add(ComponentID<T28>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T28).Name}");
        if (!set.Add(ComponentID<T29>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T29).Name}");
        if (!set.Add(ComponentID<T30>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T30).Name}");
        if (!set.Add(ComponentID<T31>.ID))
            throw new ArgumentException($"Duplicate component type: {typeof(T31).Name}");

        AddArchetype(set);

        return this;
    }

}

