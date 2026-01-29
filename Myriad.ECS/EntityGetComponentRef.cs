using Myriad.ECS.Collections;
using Myriad.ECS.Worlds;
using System.Diagnostics.CodeAnalysis;
using Myriad.ECS.IDs;

namespace Myriad.ECS;

/* dotcover disable */

// ReSharper disable RedundantTypeArgumentsOfMethod

public readonly partial struct EntityId
{
    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    
    public RefTuple<T0, T1> GetComponentRef<T0, T1>(World world)
        where T0 : IComponent
        where T1 : IComponent
    {
        ref var entityInfo = ref world.GetEntityInfo(this);

        return entityInfo.Chunk.GetRefTuple<T0, T1>(
            entityInfo.RowIndex,
            ComponentID<T0>.ID,
            ComponentID<T1>.ID
        );
    }

    /// <summary>
    /// Try to get a tuple of several components, returns false if the entity does not exist or if any of the components
    /// are missing.
    /// </summary>
    
    public bool TryGetComponentRef<T0, T1>(World world, out RefTuple<T0, T1> output)
        where T0 : IComponent
        where T1 : IComponent
    {
        // Try to get entity info ref, returns ref to dummy if not
        EntityInfo dummy = default;
        ref var entityInfo = ref world.GetEntityInfo(this, ref dummy, out var isNotExists);

        // If it doesn't exist it doesn't have the components!
        if (isNotExists)
        {
            output = default;
            return false;
        }

        // Get a cached list of components that has been sorted
        var components = SortedListOfComponents<T0, T1>.Components;

        // Check if they are all in the components set
        var hasComponents = entityInfo.Chunk.Archetype.Components.IsSupersetOfSortedSpan(components.Span);
        if (!hasComponents)
        {
            output = default;
            return false;
        }

        // Get the components
        output = entityInfo.Chunk.GetRefTuple<T0, T1>(
            entityInfo.RowIndex,
            ComponentID<T0>.ID,
            ComponentID<T1>.ID
        );
        return true;
    }

    /// <summary>
    /// Check if this entity contains a tuple of several components
    /// </summary>
    
    public bool HasComponent<T0, T1>(World world)
        where T0 : IComponent
        where T1 : IComponent
    {
        // Try to get entity info ref, returns ref to dummy if not
        EntityInfo dummy = default;
        ref var entityInfo = ref world.GetEntityInfo(this, ref dummy, out var isNotExists);

        // If it doesn't exist it doesn't have the components!
        if (isNotExists)
            return false;

        // Get a cached list of components that has been sorted
        var components = SortedListOfComponents<T0, T1>.Components;

        // Check if they are all in the components set
        return entityInfo.Chunk.Archetype.Components.IsSupersetOfSortedSpan(components.Span);
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public RefTuple<T0, T1, T2> GetComponentRef<T0, T1, T2>(World world)
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
    {
        ref var entityInfo = ref world.GetEntityInfo(this);

        return entityInfo.Chunk.GetRefTuple<T0, T1, T2>(
            entityInfo.RowIndex,
            ComponentID<T0>.ID,
            ComponentID<T1>.ID,
            ComponentID<T2>.ID
        );
    }

    /// <summary>
    /// Try to get a tuple of several components, returns false if the entity does not exist or if any of the components
    /// are missing.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool TryGetComponentRef<T0, T1, T2>(World world, out RefTuple<T0, T1, T2> output)
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
    {
        // Try to get entity info ref, returns ref to dummy if not
        EntityInfo dummy = default;
        ref var entityInfo = ref world.GetEntityInfo(this, ref dummy, out var isNotExists);

        // If it doesn't exist it doesn't have the components!
        if (isNotExists)
        {
            output = default;
            return false;
        }

        // Get a cached list of components that has been sorted
        var components = SortedListOfComponents<T0, T1, T2>.Components;

        // Check if they are all in the components set
        var hasComponents = entityInfo.Chunk.Archetype.Components.IsSupersetOfSortedSpan(components.Span);
        if (!hasComponents)
        {
            output = default;
            return false;
        }

        // Get the components
        output = entityInfo.Chunk.GetRefTuple<T0, T1, T2>(
            entityInfo.RowIndex,
            ComponentID<T0>.ID,
            ComponentID<T1>.ID,
            ComponentID<T2>.ID
        );
        return true;
    }

    /// <summary>
    /// Check if this entity contains a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool HasComponent<T0, T1, T2>(World world)
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
    {
        // Try to get entity info ref, returns ref to dummy if not
        EntityInfo dummy = default;
        ref var entityInfo = ref world.GetEntityInfo(this, ref dummy, out var isNotExists);

        // If it doesn't exist it doesn't have the components!
        if (isNotExists)
            return false;

        // Get a cached list of components that has been sorted
        var components = SortedListOfComponents<T0, T1, T2>.Components;

        // Check if they are all in the components set
        return entityInfo.Chunk.Archetype.Components.IsSupersetOfSortedSpan(components.Span);
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public RefTuple<T0, T1, T2, T3> GetComponentRef<T0, T1, T2, T3>(World world)
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
    {
        ref var entityInfo = ref world.GetEntityInfo(this);

        return entityInfo.Chunk.GetRefTuple<T0, T1, T2, T3>(
            entityInfo.RowIndex,
            ComponentID<T0>.ID,
            ComponentID<T1>.ID,
            ComponentID<T2>.ID,
            ComponentID<T3>.ID
        );
    }

    /// <summary>
    /// Try to get a tuple of several components, returns false if the entity does not exist or if any of the components
    /// are missing.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool TryGetComponentRef<T0, T1, T2, T3>(World world, out RefTuple<T0, T1, T2, T3> output)
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
    {
        // Try to get entity info ref, returns ref to dummy if not
        EntityInfo dummy = default;
        ref var entityInfo = ref world.GetEntityInfo(this, ref dummy, out var isNotExists);

        // If it doesn't exist it doesn't have the components!
        if (isNotExists)
        {
            output = default;
            return false;
        }

        // Get a cached list of components that has been sorted
        var components = SortedListOfComponents<T0, T1, T2, T3>.Components;

        // Check if they are all in the components set
        var hasComponents = entityInfo.Chunk.Archetype.Components.IsSupersetOfSortedSpan(components.Span);
        if (!hasComponents)
        {
            output = default;
            return false;
        }

        // Get the components
        output = entityInfo.Chunk.GetRefTuple<T0, T1, T2, T3>(
            entityInfo.RowIndex,
            ComponentID<T0>.ID,
            ComponentID<T1>.ID,
            ComponentID<T2>.ID,
            ComponentID<T3>.ID
        );
        return true;
    }

    /// <summary>
    /// Check if this entity contains a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool HasComponent<T0, T1, T2, T3>(World world)
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
    {
        // Try to get entity info ref, returns ref to dummy if not
        EntityInfo dummy = default;
        ref var entityInfo = ref world.GetEntityInfo(this, ref dummy, out var isNotExists);

        // If it doesn't exist it doesn't have the components!
        if (isNotExists)
            return false;

        // Get a cached list of components that has been sorted
        var components = SortedListOfComponents<T0, T1, T2, T3>.Components;

        // Check if they are all in the components set
        return entityInfo.Chunk.Archetype.Components.IsSupersetOfSortedSpan(components.Span);
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public RefTuple<T0, T1, T2, T3, T4> GetComponentRef<T0, T1, T2, T3, T4>(World world)
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
    {
        ref var entityInfo = ref world.GetEntityInfo(this);

        return entityInfo.Chunk.GetRefTuple<T0, T1, T2, T3, T4>(
            entityInfo.RowIndex,
            ComponentID<T0>.ID,
            ComponentID<T1>.ID,
            ComponentID<T2>.ID,
            ComponentID<T3>.ID,
            ComponentID<T4>.ID
        );
    }

    /// <summary>
    /// Try to get a tuple of several components, returns false if the entity does not exist or if any of the components
    /// are missing.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool TryGetComponentRef<T0, T1, T2, T3, T4>(World world, out RefTuple<T0, T1, T2, T3, T4> output)
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
    {
        // Try to get entity info ref, returns ref to dummy if not
        EntityInfo dummy = default;
        ref var entityInfo = ref world.GetEntityInfo(this, ref dummy, out var isNotExists);

        // If it doesn't exist it doesn't have the components!
        if (isNotExists)
        {
            output = default;
            return false;
        }

        // Get a cached list of components that has been sorted
        var components = SortedListOfComponents<T0, T1, T2, T3, T4>.Components;

        // Check if they are all in the components set
        var hasComponents = entityInfo.Chunk.Archetype.Components.IsSupersetOfSortedSpan(components.Span);
        if (!hasComponents)
        {
            output = default;
            return false;
        }

        // Get the components
        output = entityInfo.Chunk.GetRefTuple<T0, T1, T2, T3, T4>(
            entityInfo.RowIndex,
            ComponentID<T0>.ID,
            ComponentID<T1>.ID,
            ComponentID<T2>.ID,
            ComponentID<T3>.ID,
            ComponentID<T4>.ID
        );
        return true;
    }

    /// <summary>
    /// Check if this entity contains a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool HasComponent<T0, T1, T2, T3, T4>(World world)
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
    {
        // Try to get entity info ref, returns ref to dummy if not
        EntityInfo dummy = default;
        ref var entityInfo = ref world.GetEntityInfo(this, ref dummy, out var isNotExists);

        // If it doesn't exist it doesn't have the components!
        if (isNotExists)
            return false;

        // Get a cached list of components that has been sorted
        var components = SortedListOfComponents<T0, T1, T2, T3, T4>.Components;

        // Check if they are all in the components set
        return entityInfo.Chunk.Archetype.Components.IsSupersetOfSortedSpan(components.Span);
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public RefTuple<T0, T1, T2, T3, T4, T5> GetComponentRef<T0, T1, T2, T3, T4, T5>(World world)
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
    {
        ref var entityInfo = ref world.GetEntityInfo(this);

        return entityInfo.Chunk.GetRefTuple<T0, T1, T2, T3, T4, T5>(
            entityInfo.RowIndex,
            ComponentID<T0>.ID,
            ComponentID<T1>.ID,
            ComponentID<T2>.ID,
            ComponentID<T3>.ID,
            ComponentID<T4>.ID,
            ComponentID<T5>.ID
        );
    }

    /// <summary>
    /// Try to get a tuple of several components, returns false if the entity does not exist or if any of the components
    /// are missing.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool TryGetComponentRef<T0, T1, T2, T3, T4, T5>(World world, out RefTuple<T0, T1, T2, T3, T4, T5> output)
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
    {
        // Try to get entity info ref, returns ref to dummy if not
        EntityInfo dummy = default;
        ref var entityInfo = ref world.GetEntityInfo(this, ref dummy, out var isNotExists);

        // If it doesn't exist it doesn't have the components!
        if (isNotExists)
        {
            output = default;
            return false;
        }

        // Get a cached list of components that has been sorted
        var components = SortedListOfComponents<T0, T1, T2, T3, T4, T5>.Components;

        // Check if they are all in the components set
        var hasComponents = entityInfo.Chunk.Archetype.Components.IsSupersetOfSortedSpan(components.Span);
        if (!hasComponents)
        {
            output = default;
            return false;
        }

        // Get the components
        output = entityInfo.Chunk.GetRefTuple<T0, T1, T2, T3, T4, T5>(
            entityInfo.RowIndex,
            ComponentID<T0>.ID,
            ComponentID<T1>.ID,
            ComponentID<T2>.ID,
            ComponentID<T3>.ID,
            ComponentID<T4>.ID,
            ComponentID<T5>.ID
        );
        return true;
    }

    /// <summary>
    /// Check if this entity contains a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool HasComponent<T0, T1, T2, T3, T4, T5>(World world)
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
    {
        // Try to get entity info ref, returns ref to dummy if not
        EntityInfo dummy = default;
        ref var entityInfo = ref world.GetEntityInfo(this, ref dummy, out var isNotExists);

        // If it doesn't exist it doesn't have the components!
        if (isNotExists)
            return false;

        // Get a cached list of components that has been sorted
        var components = SortedListOfComponents<T0, T1, T2, T3, T4, T5>.Components;

        // Check if they are all in the components set
        return entityInfo.Chunk.Archetype.Components.IsSupersetOfSortedSpan(components.Span);
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public RefTuple<T0, T1, T2, T3, T4, T5, T6> GetComponentRef<T0, T1, T2, T3, T4, T5, T6>(World world)
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
    {
        ref var entityInfo = ref world.GetEntityInfo(this);

        return entityInfo.Chunk.GetRefTuple<T0, T1, T2, T3, T4, T5, T6>(
            entityInfo.RowIndex,
            ComponentID<T0>.ID,
            ComponentID<T1>.ID,
            ComponentID<T2>.ID,
            ComponentID<T3>.ID,
            ComponentID<T4>.ID,
            ComponentID<T5>.ID,
            ComponentID<T6>.ID
        );
    }

    /// <summary>
    /// Try to get a tuple of several components, returns false if the entity does not exist or if any of the components
    /// are missing.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool TryGetComponentRef<T0, T1, T2, T3, T4, T5, T6>(World world, out RefTuple<T0, T1, T2, T3, T4, T5, T6> output)
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
    {
        // Try to get entity info ref, returns ref to dummy if not
        EntityInfo dummy = default;
        ref var entityInfo = ref world.GetEntityInfo(this, ref dummy, out var isNotExists);

        // If it doesn't exist it doesn't have the components!
        if (isNotExists)
        {
            output = default;
            return false;
        }

        // Get a cached list of components that has been sorted
        var components = SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6>.Components;

        // Check if they are all in the components set
        var hasComponents = entityInfo.Chunk.Archetype.Components.IsSupersetOfSortedSpan(components.Span);
        if (!hasComponents)
        {
            output = default;
            return false;
        }

        // Get the components
        output = entityInfo.Chunk.GetRefTuple<T0, T1, T2, T3, T4, T5, T6>(
            entityInfo.RowIndex,
            ComponentID<T0>.ID,
            ComponentID<T1>.ID,
            ComponentID<T2>.ID,
            ComponentID<T3>.ID,
            ComponentID<T4>.ID,
            ComponentID<T5>.ID,
            ComponentID<T6>.ID
        );
        return true;
    }

    /// <summary>
    /// Check if this entity contains a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool HasComponent<T0, T1, T2, T3, T4, T5, T6>(World world)
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
    {
        // Try to get entity info ref, returns ref to dummy if not
        EntityInfo dummy = default;
        ref var entityInfo = ref world.GetEntityInfo(this, ref dummy, out var isNotExists);

        // If it doesn't exist it doesn't have the components!
        if (isNotExists)
            return false;

        // Get a cached list of components that has been sorted
        var components = SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6>.Components;

        // Check if they are all in the components set
        return entityInfo.Chunk.Archetype.Components.IsSupersetOfSortedSpan(components.Span);
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public RefTuple<T0, T1, T2, T3, T4, T5, T6, T7> GetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7>(World world)
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
    {
        ref var entityInfo = ref world.GetEntityInfo(this);

        return entityInfo.Chunk.GetRefTuple<T0, T1, T2, T3, T4, T5, T6, T7>(
            entityInfo.RowIndex,
            ComponentID<T0>.ID,
            ComponentID<T1>.ID,
            ComponentID<T2>.ID,
            ComponentID<T3>.ID,
            ComponentID<T4>.ID,
            ComponentID<T5>.ID,
            ComponentID<T6>.ID,
            ComponentID<T7>.ID
        );
    }

    /// <summary>
    /// Try to get a tuple of several components, returns false if the entity does not exist or if any of the components
    /// are missing.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool TryGetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7>(World world, out RefTuple<T0, T1, T2, T3, T4, T5, T6, T7> output)
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
    {
        // Try to get entity info ref, returns ref to dummy if not
        EntityInfo dummy = default;
        ref var entityInfo = ref world.GetEntityInfo(this, ref dummy, out var isNotExists);

        // If it doesn't exist it doesn't have the components!
        if (isNotExists)
        {
            output = default;
            return false;
        }

        // Get a cached list of components that has been sorted
        var components = SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6, T7>.Components;

        // Check if they are all in the components set
        var hasComponents = entityInfo.Chunk.Archetype.Components.IsSupersetOfSortedSpan(components.Span);
        if (!hasComponents)
        {
            output = default;
            return false;
        }

        // Get the components
        output = entityInfo.Chunk.GetRefTuple<T0, T1, T2, T3, T4, T5, T6, T7>(
            entityInfo.RowIndex,
            ComponentID<T0>.ID,
            ComponentID<T1>.ID,
            ComponentID<T2>.ID,
            ComponentID<T3>.ID,
            ComponentID<T4>.ID,
            ComponentID<T5>.ID,
            ComponentID<T6>.ID,
            ComponentID<T7>.ID
        );
        return true;
    }

    /// <summary>
    /// Check if this entity contains a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool HasComponent<T0, T1, T2, T3, T4, T5, T6, T7>(World world)
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
    {
        // Try to get entity info ref, returns ref to dummy if not
        EntityInfo dummy = default;
        ref var entityInfo = ref world.GetEntityInfo(this, ref dummy, out var isNotExists);

        // If it doesn't exist it doesn't have the components!
        if (isNotExists)
            return false;

        // Get a cached list of components that has been sorted
        var components = SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6, T7>.Components;

        // Check if they are all in the components set
        return entityInfo.Chunk.Archetype.Components.IsSupersetOfSortedSpan(components.Span);
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8> GetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8>(World world)
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
        ref var entityInfo = ref world.GetEntityInfo(this);

        return entityInfo.Chunk.GetRefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8>(
            entityInfo.RowIndex,
            ComponentID<T0>.ID,
            ComponentID<T1>.ID,
            ComponentID<T2>.ID,
            ComponentID<T3>.ID,
            ComponentID<T4>.ID,
            ComponentID<T5>.ID,
            ComponentID<T6>.ID,
            ComponentID<T7>.ID,
            ComponentID<T8>.ID
        );
    }

    /// <summary>
    /// Try to get a tuple of several components, returns false if the entity does not exist or if any of the components
    /// are missing.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool TryGetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8>(World world, out RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8> output)
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
        // Try to get entity info ref, returns ref to dummy if not
        EntityInfo dummy = default;
        ref var entityInfo = ref world.GetEntityInfo(this, ref dummy, out var isNotExists);

        // If it doesn't exist it doesn't have the components!
        if (isNotExists)
        {
            output = default;
            return false;
        }

        // Get a cached list of components that has been sorted
        var components = SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8>.Components;

        // Check if they are all in the components set
        var hasComponents = entityInfo.Chunk.Archetype.Components.IsSupersetOfSortedSpan(components.Span);
        if (!hasComponents)
        {
            output = default;
            return false;
        }

        // Get the components
        output = entityInfo.Chunk.GetRefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8>(
            entityInfo.RowIndex,
            ComponentID<T0>.ID,
            ComponentID<T1>.ID,
            ComponentID<T2>.ID,
            ComponentID<T3>.ID,
            ComponentID<T4>.ID,
            ComponentID<T5>.ID,
            ComponentID<T6>.ID,
            ComponentID<T7>.ID,
            ComponentID<T8>.ID
        );
        return true;
    }

    /// <summary>
    /// Check if this entity contains a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool HasComponent<T0, T1, T2, T3, T4, T5, T6, T7, T8>(World world)
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
        // Try to get entity info ref, returns ref to dummy if not
        EntityInfo dummy = default;
        ref var entityInfo = ref world.GetEntityInfo(this, ref dummy, out var isNotExists);

        // If it doesn't exist it doesn't have the components!
        if (isNotExists)
            return false;

        // Get a cached list of components that has been sorted
        var components = SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8>.Components;

        // Check if they are all in the components set
        return entityInfo.Chunk.Archetype.Components.IsSupersetOfSortedSpan(components.Span);
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> GetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(World world)
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
        ref var entityInfo = ref world.GetEntityInfo(this);

        return entityInfo.Chunk.GetRefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            entityInfo.RowIndex,
            ComponentID<T0>.ID,
            ComponentID<T1>.ID,
            ComponentID<T2>.ID,
            ComponentID<T3>.ID,
            ComponentID<T4>.ID,
            ComponentID<T5>.ID,
            ComponentID<T6>.ID,
            ComponentID<T7>.ID,
            ComponentID<T8>.ID,
            ComponentID<T9>.ID
        );
    }

    /// <summary>
    /// Try to get a tuple of several components, returns false if the entity does not exist or if any of the components
    /// are missing.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool TryGetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(World world, out RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> output)
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
        // Try to get entity info ref, returns ref to dummy if not
        EntityInfo dummy = default;
        ref var entityInfo = ref world.GetEntityInfo(this, ref dummy, out var isNotExists);

        // If it doesn't exist it doesn't have the components!
        if (isNotExists)
        {
            output = default;
            return false;
        }

        // Get a cached list of components that has been sorted
        var components = SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>.Components;

        // Check if they are all in the components set
        var hasComponents = entityInfo.Chunk.Archetype.Components.IsSupersetOfSortedSpan(components.Span);
        if (!hasComponents)
        {
            output = default;
            return false;
        }

        // Get the components
        output = entityInfo.Chunk.GetRefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            entityInfo.RowIndex,
            ComponentID<T0>.ID,
            ComponentID<T1>.ID,
            ComponentID<T2>.ID,
            ComponentID<T3>.ID,
            ComponentID<T4>.ID,
            ComponentID<T5>.ID,
            ComponentID<T6>.ID,
            ComponentID<T7>.ID,
            ComponentID<T8>.ID,
            ComponentID<T9>.ID
        );
        return true;
    }

    /// <summary>
    /// Check if this entity contains a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool HasComponent<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(World world)
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
        // Try to get entity info ref, returns ref to dummy if not
        EntityInfo dummy = default;
        ref var entityInfo = ref world.GetEntityInfo(this, ref dummy, out var isNotExists);

        // If it doesn't exist it doesn't have the components!
        if (isNotExists)
            return false;

        // Get a cached list of components that has been sorted
        var components = SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>.Components;

        // Check if they are all in the components set
        return entityInfo.Chunk.Archetype.Components.IsSupersetOfSortedSpan(components.Span);
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> GetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(World world)
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
        ref var entityInfo = ref world.GetEntityInfo(this);

        return entityInfo.Chunk.GetRefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            entityInfo.RowIndex,
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
            ComponentID<T10>.ID
        );
    }

    /// <summary>
    /// Try to get a tuple of several components, returns false if the entity does not exist or if any of the components
    /// are missing.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool TryGetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(World world, out RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> output)
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
        // Try to get entity info ref, returns ref to dummy if not
        EntityInfo dummy = default;
        ref var entityInfo = ref world.GetEntityInfo(this, ref dummy, out var isNotExists);

        // If it doesn't exist it doesn't have the components!
        if (isNotExists)
        {
            output = default;
            return false;
        }

        // Get a cached list of components that has been sorted
        var components = SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.Components;

        // Check if they are all in the components set
        var hasComponents = entityInfo.Chunk.Archetype.Components.IsSupersetOfSortedSpan(components.Span);
        if (!hasComponents)
        {
            output = default;
            return false;
        }

        // Get the components
        output = entityInfo.Chunk.GetRefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            entityInfo.RowIndex,
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
            ComponentID<T10>.ID
        );
        return true;
    }

    /// <summary>
    /// Check if this entity contains a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool HasComponent<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(World world)
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
        // Try to get entity info ref, returns ref to dummy if not
        EntityInfo dummy = default;
        ref var entityInfo = ref world.GetEntityInfo(this, ref dummy, out var isNotExists);

        // If it doesn't exist it doesn't have the components!
        if (isNotExists)
            return false;

        // Get a cached list of components that has been sorted
        var components = SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.Components;

        // Check if they are all in the components set
        return entityInfo.Chunk.Archetype.Components.IsSupersetOfSortedSpan(components.Span);
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> GetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(World world)
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
        ref var entityInfo = ref world.GetEntityInfo(this);

        return entityInfo.Chunk.GetRefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            entityInfo.RowIndex,
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
            ComponentID<T11>.ID
        );
    }

    /// <summary>
    /// Try to get a tuple of several components, returns false if the entity does not exist or if any of the components
    /// are missing.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool TryGetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(World world, out RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> output)
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
        // Try to get entity info ref, returns ref to dummy if not
        EntityInfo dummy = default;
        ref var entityInfo = ref world.GetEntityInfo(this, ref dummy, out var isNotExists);

        // If it doesn't exist it doesn't have the components!
        if (isNotExists)
        {
            output = default;
            return false;
        }

        // Get a cached list of components that has been sorted
        var components = SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.Components;

        // Check if they are all in the components set
        var hasComponents = entityInfo.Chunk.Archetype.Components.IsSupersetOfSortedSpan(components.Span);
        if (!hasComponents)
        {
            output = default;
            return false;
        }

        // Get the components
        output = entityInfo.Chunk.GetRefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            entityInfo.RowIndex,
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
            ComponentID<T11>.ID
        );
        return true;
    }

    /// <summary>
    /// Check if this entity contains a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool HasComponent<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(World world)
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
        // Try to get entity info ref, returns ref to dummy if not
        EntityInfo dummy = default;
        ref var entityInfo = ref world.GetEntityInfo(this, ref dummy, out var isNotExists);

        // If it doesn't exist it doesn't have the components!
        if (isNotExists)
            return false;

        // Get a cached list of components that has been sorted
        var components = SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.Components;

        // Check if they are all in the components set
        return entityInfo.Chunk.Archetype.Components.IsSupersetOfSortedSpan(components.Span);
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> GetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(World world)
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
        ref var entityInfo = ref world.GetEntityInfo(this);

        return entityInfo.Chunk.GetRefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            entityInfo.RowIndex,
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
            ComponentID<T12>.ID
        );
    }

    /// <summary>
    /// Try to get a tuple of several components, returns false if the entity does not exist or if any of the components
    /// are missing.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool TryGetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(World world, out RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> output)
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
        // Try to get entity info ref, returns ref to dummy if not
        EntityInfo dummy = default;
        ref var entityInfo = ref world.GetEntityInfo(this, ref dummy, out var isNotExists);

        // If it doesn't exist it doesn't have the components!
        if (isNotExists)
        {
            output = default;
            return false;
        }

        // Get a cached list of components that has been sorted
        var components = SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.Components;

        // Check if they are all in the components set
        var hasComponents = entityInfo.Chunk.Archetype.Components.IsSupersetOfSortedSpan(components.Span);
        if (!hasComponents)
        {
            output = default;
            return false;
        }

        // Get the components
        output = entityInfo.Chunk.GetRefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            entityInfo.RowIndex,
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
            ComponentID<T12>.ID
        );
        return true;
    }

    /// <summary>
    /// Check if this entity contains a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool HasComponent<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(World world)
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
        // Try to get entity info ref, returns ref to dummy if not
        EntityInfo dummy = default;
        ref var entityInfo = ref world.GetEntityInfo(this, ref dummy, out var isNotExists);

        // If it doesn't exist it doesn't have the components!
        if (isNotExists)
            return false;

        // Get a cached list of components that has been sorted
        var components = SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.Components;

        // Check if they are all in the components set
        return entityInfo.Chunk.Archetype.Components.IsSupersetOfSortedSpan(components.Span);
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> GetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(World world)
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
        ref var entityInfo = ref world.GetEntityInfo(this);

        return entityInfo.Chunk.GetRefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            entityInfo.RowIndex,
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
            ComponentID<T13>.ID
        );
    }

    /// <summary>
    /// Try to get a tuple of several components, returns false if the entity does not exist or if any of the components
    /// are missing.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool TryGetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(World world, out RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> output)
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
        // Try to get entity info ref, returns ref to dummy if not
        EntityInfo dummy = default;
        ref var entityInfo = ref world.GetEntityInfo(this, ref dummy, out var isNotExists);

        // If it doesn't exist it doesn't have the components!
        if (isNotExists)
        {
            output = default;
            return false;
        }

        // Get a cached list of components that has been sorted
        var components = SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.Components;

        // Check if they are all in the components set
        var hasComponents = entityInfo.Chunk.Archetype.Components.IsSupersetOfSortedSpan(components.Span);
        if (!hasComponents)
        {
            output = default;
            return false;
        }

        // Get the components
        output = entityInfo.Chunk.GetRefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            entityInfo.RowIndex,
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
            ComponentID<T13>.ID
        );
        return true;
    }

    /// <summary>
    /// Check if this entity contains a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool HasComponent<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(World world)
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
        // Try to get entity info ref, returns ref to dummy if not
        EntityInfo dummy = default;
        ref var entityInfo = ref world.GetEntityInfo(this, ref dummy, out var isNotExists);

        // If it doesn't exist it doesn't have the components!
        if (isNotExists)
            return false;

        // Get a cached list of components that has been sorted
        var components = SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.Components;

        // Check if they are all in the components set
        return entityInfo.Chunk.Archetype.Components.IsSupersetOfSortedSpan(components.Span);
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> GetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(World world)
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
        ref var entityInfo = ref world.GetEntityInfo(this);

        return entityInfo.Chunk.GetRefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            entityInfo.RowIndex,
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
            ComponentID<T14>.ID
        );
    }

    /// <summary>
    /// Try to get a tuple of several components, returns false if the entity does not exist or if any of the components
    /// are missing.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool TryGetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(World world, out RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> output)
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
        // Try to get entity info ref, returns ref to dummy if not
        EntityInfo dummy = default;
        ref var entityInfo = ref world.GetEntityInfo(this, ref dummy, out var isNotExists);

        // If it doesn't exist it doesn't have the components!
        if (isNotExists)
        {
            output = default;
            return false;
        }

        // Get a cached list of components that has been sorted
        var components = SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.Components;

        // Check if they are all in the components set
        var hasComponents = entityInfo.Chunk.Archetype.Components.IsSupersetOfSortedSpan(components.Span);
        if (!hasComponents)
        {
            output = default;
            return false;
        }

        // Get the components
        output = entityInfo.Chunk.GetRefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            entityInfo.RowIndex,
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
            ComponentID<T14>.ID
        );
        return true;
    }

    /// <summary>
    /// Check if this entity contains a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool HasComponent<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(World world)
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
        // Try to get entity info ref, returns ref to dummy if not
        EntityInfo dummy = default;
        ref var entityInfo = ref world.GetEntityInfo(this, ref dummy, out var isNotExists);

        // If it doesn't exist it doesn't have the components!
        if (isNotExists)
            return false;

        // Get a cached list of components that has been sorted
        var components = SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.Components;

        // Check if they are all in the components set
        return entityInfo.Chunk.Archetype.Components.IsSupersetOfSortedSpan(components.Span);
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> GetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(World world)
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
        ref var entityInfo = ref world.GetEntityInfo(this);

        return entityInfo.Chunk.GetRefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            entityInfo.RowIndex,
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
            ComponentID<T15>.ID
        );
    }

    /// <summary>
    /// Try to get a tuple of several components, returns false if the entity does not exist or if any of the components
    /// are missing.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool TryGetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(World world, out RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> output)
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
        // Try to get entity info ref, returns ref to dummy if not
        EntityInfo dummy = default;
        ref var entityInfo = ref world.GetEntityInfo(this, ref dummy, out var isNotExists);

        // If it doesn't exist it doesn't have the components!
        if (isNotExists)
        {
            output = default;
            return false;
        }

        // Get a cached list of components that has been sorted
        var components = SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.Components;

        // Check if they are all in the components set
        var hasComponents = entityInfo.Chunk.Archetype.Components.IsSupersetOfSortedSpan(components.Span);
        if (!hasComponents)
        {
            output = default;
            return false;
        }

        // Get the components
        output = entityInfo.Chunk.GetRefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            entityInfo.RowIndex,
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
            ComponentID<T15>.ID
        );
        return true;
    }

    /// <summary>
    /// Check if this entity contains a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool HasComponent<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(World world)
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
        // Try to get entity info ref, returns ref to dummy if not
        EntityInfo dummy = default;
        ref var entityInfo = ref world.GetEntityInfo(this, ref dummy, out var isNotExists);

        // If it doesn't exist it doesn't have the components!
        if (isNotExists)
            return false;

        // Get a cached list of components that has been sorted
        var components = SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.Components;

        // Check if they are all in the components set
        return entityInfo.Chunk.Archetype.Components.IsSupersetOfSortedSpan(components.Span);
    }

}

public readonly partial record struct Entity
{
    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    
    public RefTuple<T0, T1> GetComponentRef<T0, T1>()
        where T0 : IComponent
        where T1 : IComponent
    {
        return ID.GetComponentRef<T0, T1>(World);
    }

    /// <summary>
    /// Try to get a tuple of several components, returns false if the entity does not exist or if any of the components
    /// are missing.
    /// </summary>
    
    public bool TryGetComponentRef<T0, T1>(out RefTuple<T0, T1> output)
        where T0 : IComponent
        where T1 : IComponent
    {
        return ID.TryGetComponentRef<T0, T1>(World, out output);
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    
    public bool HasComponent<T0, T1>()
        where T0 : IComponent
        where T1 : IComponent
    {
        return ID.HasComponent<T0, T1>(World);
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public RefTuple<T0, T1, T2> GetComponentRef<T0, T1, T2>()
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
    {
        return ID.GetComponentRef<T0, T1, T2>(World);
    }

    /// <summary>
    /// Try to get a tuple of several components, returns false if the entity does not exist or if any of the components
    /// are missing.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool TryGetComponentRef<T0, T1, T2>(out RefTuple<T0, T1, T2> output)
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
    {
        return ID.TryGetComponentRef<T0, T1, T2>(World, out output);
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool HasComponent<T0, T1, T2>()
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
    {
        return ID.HasComponent<T0, T1, T2>(World);
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public RefTuple<T0, T1, T2, T3> GetComponentRef<T0, T1, T2, T3>()
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
    {
        return ID.GetComponentRef<T0, T1, T2, T3>(World);
    }

    /// <summary>
    /// Try to get a tuple of several components, returns false if the entity does not exist or if any of the components
    /// are missing.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool TryGetComponentRef<T0, T1, T2, T3>(out RefTuple<T0, T1, T2, T3> output)
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
    {
        return ID.TryGetComponentRef<T0, T1, T2, T3>(World, out output);
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool HasComponent<T0, T1, T2, T3>()
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
    {
        return ID.HasComponent<T0, T1, T2, T3>(World);
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public RefTuple<T0, T1, T2, T3, T4> GetComponentRef<T0, T1, T2, T3, T4>()
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
    {
        return ID.GetComponentRef<T0, T1, T2, T3, T4>(World);
    }

    /// <summary>
    /// Try to get a tuple of several components, returns false if the entity does not exist or if any of the components
    /// are missing.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool TryGetComponentRef<T0, T1, T2, T3, T4>(out RefTuple<T0, T1, T2, T3, T4> output)
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
    {
        return ID.TryGetComponentRef<T0, T1, T2, T3, T4>(World, out output);
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool HasComponent<T0, T1, T2, T3, T4>()
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
    {
        return ID.HasComponent<T0, T1, T2, T3, T4>(World);
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public RefTuple<T0, T1, T2, T3, T4, T5> GetComponentRef<T0, T1, T2, T3, T4, T5>()
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
    {
        return ID.GetComponentRef<T0, T1, T2, T3, T4, T5>(World);
    }

    /// <summary>
    /// Try to get a tuple of several components, returns false if the entity does not exist or if any of the components
    /// are missing.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool TryGetComponentRef<T0, T1, T2, T3, T4, T5>(out RefTuple<T0, T1, T2, T3, T4, T5> output)
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
    {
        return ID.TryGetComponentRef<T0, T1, T2, T3, T4, T5>(World, out output);
    }

    /// <summary>
    /// Get a tuple of several components
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
        return ID.HasComponent<T0, T1, T2, T3, T4, T5>(World);
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public RefTuple<T0, T1, T2, T3, T4, T5, T6> GetComponentRef<T0, T1, T2, T3, T4, T5, T6>()
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
    {
        return ID.GetComponentRef<T0, T1, T2, T3, T4, T5, T6>(World);
    }

    /// <summary>
    /// Try to get a tuple of several components, returns false if the entity does not exist or if any of the components
    /// are missing.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool TryGetComponentRef<T0, T1, T2, T3, T4, T5, T6>(out RefTuple<T0, T1, T2, T3, T4, T5, T6> output)
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
    {
        return ID.TryGetComponentRef<T0, T1, T2, T3, T4, T5, T6>(World, out output);
    }

    /// <summary>
    /// Get a tuple of several components
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
        return ID.HasComponent<T0, T1, T2, T3, T4, T5, T6>(World);
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public RefTuple<T0, T1, T2, T3, T4, T5, T6, T7> GetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7>()
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
    {
        return ID.GetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7>(World);
    }

    /// <summary>
    /// Try to get a tuple of several components, returns false if the entity does not exist or if any of the components
    /// are missing.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool TryGetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7>(out RefTuple<T0, T1, T2, T3, T4, T5, T6, T7> output)
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
    {
        return ID.TryGetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7>(World, out output);
    }

    /// <summary>
    /// Get a tuple of several components
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
        return ID.HasComponent<T0, T1, T2, T3, T4, T5, T6, T7>(World);
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8> GetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8>()
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
        return ID.GetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8>(World);
    }

    /// <summary>
    /// Try to get a tuple of several components, returns false if the entity does not exist or if any of the components
    /// are missing.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool TryGetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8>(out RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8> output)
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
        return ID.TryGetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8>(World, out output);
    }

    /// <summary>
    /// Get a tuple of several components
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
        return ID.HasComponent<T0, T1, T2, T3, T4, T5, T6, T7, T8>(World);
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> GetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>()
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
        return ID.GetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(World);
    }

    /// <summary>
    /// Try to get a tuple of several components, returns false if the entity does not exist or if any of the components
    /// are missing.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool TryGetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(out RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> output)
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
        return ID.TryGetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(World, out output);
    }

    /// <summary>
    /// Get a tuple of several components
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
        return ID.HasComponent<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(World);
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> GetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>()
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
        return ID.GetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(World);
    }

    /// <summary>
    /// Try to get a tuple of several components, returns false if the entity does not exist or if any of the components
    /// are missing.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool TryGetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(out RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> output)
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
        return ID.TryGetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(World, out output);
    }

    /// <summary>
    /// Get a tuple of several components
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
        return ID.HasComponent<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(World);
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> GetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>()
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
        return ID.GetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(World);
    }

    /// <summary>
    /// Try to get a tuple of several components, returns false if the entity does not exist or if any of the components
    /// are missing.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool TryGetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(out RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> output)
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
        return ID.TryGetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(World, out output);
    }

    /// <summary>
    /// Get a tuple of several components
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
        return ID.HasComponent<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(World);
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> GetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>()
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
        return ID.GetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(World);
    }

    /// <summary>
    /// Try to get a tuple of several components, returns false if the entity does not exist or if any of the components
    /// are missing.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool TryGetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(out RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> output)
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
        return ID.TryGetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(World, out output);
    }

    /// <summary>
    /// Get a tuple of several components
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
        return ID.HasComponent<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(World);
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> GetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>()
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
        return ID.GetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(World);
    }

    /// <summary>
    /// Try to get a tuple of several components, returns false if the entity does not exist or if any of the components
    /// are missing.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool TryGetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(out RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> output)
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
        return ID.TryGetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(World, out output);
    }

    /// <summary>
    /// Get a tuple of several components
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
        return ID.HasComponent<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(World);
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> GetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>()
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
        return ID.GetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(World);
    }

    /// <summary>
    /// Try to get a tuple of several components, returns false if the entity does not exist or if any of the components
    /// are missing.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool TryGetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(out RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> output)
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
        return ID.TryGetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(World, out output);
    }

    /// <summary>
    /// Get a tuple of several components
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
        return ID.HasComponent<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(World);
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    public RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> GetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>()
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
        return ID.GetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(World);
    }

    /// <summary>
    /// Try to get a tuple of several components, returns false if the entity does not exist or if any of the components
    /// are missing.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool TryGetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(out RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> output)
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
        return ID.TryGetComponentRef<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(World, out output);
    }

    /// <summary>
    /// Get a tuple of several components
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
        return ID.HasComponent<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(World);
    }

}

