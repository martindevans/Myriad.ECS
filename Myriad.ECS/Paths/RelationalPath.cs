using System.Diagnostics.CodeAnalysis;
using Myriad.ECS.Components;
using Myriad.ECS.IDs;
using Myriad.ECS.Worlds;

// ReSharper disable UnusedType.Global

namespace Myriad.ECS.Paths;


/// <summary>
/// Construct a path that follows a path entirely of relational components
/// </summary>
/// <typeparam name="T0"></typeparam>

public readonly struct Path<T0>
    : IPath
    where T0 : IEntityRelationComponent
{
    /// <inheritdoc />
    public Entity? TryFollow(Entity entity)
    {
        if (!TryFollow<T0>(ref entity))
            return null;

        if (!entity.IsAlive())
            return default;

        return entity;
    }

    private static bool TryFollow<T>(ref Entity entity)
        where T : IEntityRelationComponent
    {
        // Try to get entity info for this entity
        var dummy = default(EntityInfo);
        ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

        // Can't follow a path through a dead entity!
        if (isDead)
            return false;

        // Check if the component is present
        if (!entityInfo.Chunk.Archetype.Components.Contains(ComponentID<T>.ID))
            return false;

        // Follow link
        entity = entity.GetComponentRef<T>().Target;
        return true;
    }
}


/// <summary>
/// Construct a path that follows a path entirely of relational components
/// </summary>
/// <typeparam name="T0"></typeparam>
/// <typeparam name="T1"></typeparam>

public readonly struct Path<T0, T1>
    : IPath
    where T0 : IEntityRelationComponent
    where T1 : IEntityRelationComponent
{
    /// <inheritdoc />
    public Entity? TryFollow(Entity entity)
    {
        if (!TryFollow<T0>(ref entity))
            return null;
        if (!TryFollow<T1>(ref entity))
            return null;

        if (!entity.IsAlive())
            return default;

        return entity;
    }

    private static bool TryFollow<T>(ref Entity entity)
        where T : IEntityRelationComponent
    {
        // Try to get entity info for this entity
        var dummy = default(EntityInfo);
        ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

        // Can't follow a path through a dead entity!
        if (isDead)
            return false;

        // Check if the component is present
        if (!entityInfo.Chunk.Archetype.Components.Contains(ComponentID<T>.ID))
            return false;

        // Follow link
        entity = entity.GetComponentRef<T>().Target;
        return true;
    }
}


/// <summary>
/// Construct a path that follows a path entirely of relational components
/// </summary>
/// <typeparam name="T0"></typeparam>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
[ExcludeFromCodeCoverage]
public readonly struct Path<T0, T1, T2>
    : IPath
    where T0 : IEntityRelationComponent
    where T1 : IEntityRelationComponent
    where T2 : IEntityRelationComponent
{
    /// <inheritdoc />
    public Entity? TryFollow(Entity entity)
    {
        if (!TryFollow<T0>(ref entity))
            return null;
        if (!TryFollow<T1>(ref entity))
            return null;
        if (!TryFollow<T2>(ref entity))
            return null;

        if (!entity.IsAlive())
            return default;

        return entity;
    }

    private static bool TryFollow<T>(ref Entity entity)
        where T : IEntityRelationComponent
    {
        // Try to get entity info for this entity
        var dummy = default(EntityInfo);
        ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

        // Can't follow a path through a dead entity!
        if (isDead)
            return false;

        // Check if the component is present
        if (!entityInfo.Chunk.Archetype.Components.Contains(ComponentID<T>.ID))
            return false;

        // Follow link
        entity = entity.GetComponentRef<T>().Target;
        return true;
    }
}


/// <summary>
/// Construct a path that follows a path entirely of relational components
/// </summary>
/// <typeparam name="T0"></typeparam>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
/// <typeparam name="T3"></typeparam>
[ExcludeFromCodeCoverage]
public readonly struct Path<T0, T1, T2, T3>
    : IPath
    where T0 : IEntityRelationComponent
    where T1 : IEntityRelationComponent
    where T2 : IEntityRelationComponent
    where T3 : IEntityRelationComponent
{
    /// <inheritdoc />
    public Entity? TryFollow(Entity entity)
    {
        if (!TryFollow<T0>(ref entity))
            return null;
        if (!TryFollow<T1>(ref entity))
            return null;
        if (!TryFollow<T2>(ref entity))
            return null;
        if (!TryFollow<T3>(ref entity))
            return null;

        if (!entity.IsAlive())
            return default;

        return entity;
    }

    private static bool TryFollow<T>(ref Entity entity)
        where T : IEntityRelationComponent
    {
        // Try to get entity info for this entity
        var dummy = default(EntityInfo);
        ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

        // Can't follow a path through a dead entity!
        if (isDead)
            return false;

        // Check if the component is present
        if (!entityInfo.Chunk.Archetype.Components.Contains(ComponentID<T>.ID))
            return false;

        // Follow link
        entity = entity.GetComponentRef<T>().Target;
        return true;
    }
}


/// <summary>
/// Construct a path that follows a path entirely of relational components
/// </summary>
/// <typeparam name="T0"></typeparam>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
/// <typeparam name="T3"></typeparam>
/// <typeparam name="T4"></typeparam>
[ExcludeFromCodeCoverage]
public readonly struct Path<T0, T1, T2, T3, T4>
    : IPath
    where T0 : IEntityRelationComponent
    where T1 : IEntityRelationComponent
    where T2 : IEntityRelationComponent
    where T3 : IEntityRelationComponent
    where T4 : IEntityRelationComponent
{
    /// <inheritdoc />
    public Entity? TryFollow(Entity entity)
    {
        if (!TryFollow<T0>(ref entity))
            return null;
        if (!TryFollow<T1>(ref entity))
            return null;
        if (!TryFollow<T2>(ref entity))
            return null;
        if (!TryFollow<T3>(ref entity))
            return null;
        if (!TryFollow<T4>(ref entity))
            return null;

        if (!entity.IsAlive())
            return default;

        return entity;
    }

    private static bool TryFollow<T>(ref Entity entity)
        where T : IEntityRelationComponent
    {
        // Try to get entity info for this entity
        var dummy = default(EntityInfo);
        ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

        // Can't follow a path through a dead entity!
        if (isDead)
            return false;

        // Check if the component is present
        if (!entityInfo.Chunk.Archetype.Components.Contains(ComponentID<T>.ID))
            return false;

        // Follow link
        entity = entity.GetComponentRef<T>().Target;
        return true;
    }
}


/// <summary>
/// Construct a path that follows a path entirely of relational components
/// </summary>
/// <typeparam name="T0"></typeparam>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
/// <typeparam name="T3"></typeparam>
/// <typeparam name="T4"></typeparam>
/// <typeparam name="T5"></typeparam>
[ExcludeFromCodeCoverage]
public readonly struct Path<T0, T1, T2, T3, T4, T5>
    : IPath
    where T0 : IEntityRelationComponent
    where T1 : IEntityRelationComponent
    where T2 : IEntityRelationComponent
    where T3 : IEntityRelationComponent
    where T4 : IEntityRelationComponent
    where T5 : IEntityRelationComponent
{
    /// <inheritdoc />
    public Entity? TryFollow(Entity entity)
    {
        if (!TryFollow<T0>(ref entity))
            return null;
        if (!TryFollow<T1>(ref entity))
            return null;
        if (!TryFollow<T2>(ref entity))
            return null;
        if (!TryFollow<T3>(ref entity))
            return null;
        if (!TryFollow<T4>(ref entity))
            return null;
        if (!TryFollow<T5>(ref entity))
            return null;

        if (!entity.IsAlive())
            return default;

        return entity;
    }

    private static bool TryFollow<T>(ref Entity entity)
        where T : IEntityRelationComponent
    {
        // Try to get entity info for this entity
        var dummy = default(EntityInfo);
        ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

        // Can't follow a path through a dead entity!
        if (isDead)
            return false;

        // Check if the component is present
        if (!entityInfo.Chunk.Archetype.Components.Contains(ComponentID<T>.ID))
            return false;

        // Follow link
        entity = entity.GetComponentRef<T>().Target;
        return true;
    }
}


/// <summary>
/// Construct a path that follows a path entirely of relational components
/// </summary>
/// <typeparam name="T0"></typeparam>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
/// <typeparam name="T3"></typeparam>
/// <typeparam name="T4"></typeparam>
/// <typeparam name="T5"></typeparam>
/// <typeparam name="T6"></typeparam>
[ExcludeFromCodeCoverage]
public readonly struct Path<T0, T1, T2, T3, T4, T5, T6>
    : IPath
    where T0 : IEntityRelationComponent
    where T1 : IEntityRelationComponent
    where T2 : IEntityRelationComponent
    where T3 : IEntityRelationComponent
    where T4 : IEntityRelationComponent
    where T5 : IEntityRelationComponent
    where T6 : IEntityRelationComponent
{
    /// <inheritdoc />
    public Entity? TryFollow(Entity entity)
    {
        if (!TryFollow<T0>(ref entity))
            return null;
        if (!TryFollow<T1>(ref entity))
            return null;
        if (!TryFollow<T2>(ref entity))
            return null;
        if (!TryFollow<T3>(ref entity))
            return null;
        if (!TryFollow<T4>(ref entity))
            return null;
        if (!TryFollow<T5>(ref entity))
            return null;
        if (!TryFollow<T6>(ref entity))
            return null;

        if (!entity.IsAlive())
            return default;

        return entity;
    }

    private static bool TryFollow<T>(ref Entity entity)
        where T : IEntityRelationComponent
    {
        // Try to get entity info for this entity
        var dummy = default(EntityInfo);
        ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

        // Can't follow a path through a dead entity!
        if (isDead)
            return false;

        // Check if the component is present
        if (!entityInfo.Chunk.Archetype.Components.Contains(ComponentID<T>.ID))
            return false;

        // Follow link
        entity = entity.GetComponentRef<T>().Target;
        return true;
    }
}


/// <summary>
/// Construct a path that follows a path entirely of relational components
/// </summary>
/// <typeparam name="T0"></typeparam>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
/// <typeparam name="T3"></typeparam>
/// <typeparam name="T4"></typeparam>
/// <typeparam name="T5"></typeparam>
/// <typeparam name="T6"></typeparam>
/// <typeparam name="T7"></typeparam>
[ExcludeFromCodeCoverage]
public readonly struct Path<T0, T1, T2, T3, T4, T5, T6, T7>
    : IPath
    where T0 : IEntityRelationComponent
    where T1 : IEntityRelationComponent
    where T2 : IEntityRelationComponent
    where T3 : IEntityRelationComponent
    where T4 : IEntityRelationComponent
    where T5 : IEntityRelationComponent
    where T6 : IEntityRelationComponent
    where T7 : IEntityRelationComponent
{
    /// <inheritdoc />
    public Entity? TryFollow(Entity entity)
    {
        if (!TryFollow<T0>(ref entity))
            return null;
        if (!TryFollow<T1>(ref entity))
            return null;
        if (!TryFollow<T2>(ref entity))
            return null;
        if (!TryFollow<T3>(ref entity))
            return null;
        if (!TryFollow<T4>(ref entity))
            return null;
        if (!TryFollow<T5>(ref entity))
            return null;
        if (!TryFollow<T6>(ref entity))
            return null;
        if (!TryFollow<T7>(ref entity))
            return null;

        if (!entity.IsAlive())
            return default;

        return entity;
    }

    private static bool TryFollow<T>(ref Entity entity)
        where T : IEntityRelationComponent
    {
        // Try to get entity info for this entity
        var dummy = default(EntityInfo);
        ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

        // Can't follow a path through a dead entity!
        if (isDead)
            return false;

        // Check if the component is present
        if (!entityInfo.Chunk.Archetype.Components.Contains(ComponentID<T>.ID))
            return false;

        // Follow link
        entity = entity.GetComponentRef<T>().Target;
        return true;
    }
}


/// <summary>
/// Construct a path that follows a path entirely of relational components
/// </summary>
/// <typeparam name="T0"></typeparam>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
/// <typeparam name="T3"></typeparam>
/// <typeparam name="T4"></typeparam>
/// <typeparam name="T5"></typeparam>
/// <typeparam name="T6"></typeparam>
/// <typeparam name="T7"></typeparam>
/// <typeparam name="T8"></typeparam>
[ExcludeFromCodeCoverage]
public readonly struct Path<T0, T1, T2, T3, T4, T5, T6, T7, T8>
    : IPath
    where T0 : IEntityRelationComponent
    where T1 : IEntityRelationComponent
    where T2 : IEntityRelationComponent
    where T3 : IEntityRelationComponent
    where T4 : IEntityRelationComponent
    where T5 : IEntityRelationComponent
    where T6 : IEntityRelationComponent
    where T7 : IEntityRelationComponent
    where T8 : IEntityRelationComponent
{
    /// <inheritdoc />
    public Entity? TryFollow(Entity entity)
    {
        if (!TryFollow<T0>(ref entity))
            return null;
        if (!TryFollow<T1>(ref entity))
            return null;
        if (!TryFollow<T2>(ref entity))
            return null;
        if (!TryFollow<T3>(ref entity))
            return null;
        if (!TryFollow<T4>(ref entity))
            return null;
        if (!TryFollow<T5>(ref entity))
            return null;
        if (!TryFollow<T6>(ref entity))
            return null;
        if (!TryFollow<T7>(ref entity))
            return null;
        if (!TryFollow<T8>(ref entity))
            return null;

        if (!entity.IsAlive())
            return default;

        return entity;
    }

    private static bool TryFollow<T>(ref Entity entity)
        where T : IEntityRelationComponent
    {
        // Try to get entity info for this entity
        var dummy = default(EntityInfo);
        ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

        // Can't follow a path through a dead entity!
        if (isDead)
            return false;

        // Check if the component is present
        if (!entityInfo.Chunk.Archetype.Components.Contains(ComponentID<T>.ID))
            return false;

        // Follow link
        entity = entity.GetComponentRef<T>().Target;
        return true;
    }
}


/// <summary>
/// Construct a path that follows a path entirely of relational components
/// </summary>
/// <typeparam name="T0"></typeparam>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
/// <typeparam name="T3"></typeparam>
/// <typeparam name="T4"></typeparam>
/// <typeparam name="T5"></typeparam>
/// <typeparam name="T6"></typeparam>
/// <typeparam name="T7"></typeparam>
/// <typeparam name="T8"></typeparam>
/// <typeparam name="T9"></typeparam>
[ExcludeFromCodeCoverage]
public readonly struct Path<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
    : IPath
    where T0 : IEntityRelationComponent
    where T1 : IEntityRelationComponent
    where T2 : IEntityRelationComponent
    where T3 : IEntityRelationComponent
    where T4 : IEntityRelationComponent
    where T5 : IEntityRelationComponent
    where T6 : IEntityRelationComponent
    where T7 : IEntityRelationComponent
    where T8 : IEntityRelationComponent
    where T9 : IEntityRelationComponent
{
    /// <inheritdoc />
    public Entity? TryFollow(Entity entity)
    {
        if (!TryFollow<T0>(ref entity))
            return null;
        if (!TryFollow<T1>(ref entity))
            return null;
        if (!TryFollow<T2>(ref entity))
            return null;
        if (!TryFollow<T3>(ref entity))
            return null;
        if (!TryFollow<T4>(ref entity))
            return null;
        if (!TryFollow<T5>(ref entity))
            return null;
        if (!TryFollow<T6>(ref entity))
            return null;
        if (!TryFollow<T7>(ref entity))
            return null;
        if (!TryFollow<T8>(ref entity))
            return null;
        if (!TryFollow<T9>(ref entity))
            return null;

        if (!entity.IsAlive())
            return default;

        return entity;
    }

    private static bool TryFollow<T>(ref Entity entity)
        where T : IEntityRelationComponent
    {
        // Try to get entity info for this entity
        var dummy = default(EntityInfo);
        ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

        // Can't follow a path through a dead entity!
        if (isDead)
            return false;

        // Check if the component is present
        if (!entityInfo.Chunk.Archetype.Components.Contains(ComponentID<T>.ID))
            return false;

        // Follow link
        entity = entity.GetComponentRef<T>().Target;
        return true;
    }
}


/// <summary>
/// Construct a path that follows a path entirely of relational components
/// </summary>
/// <typeparam name="T0"></typeparam>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
/// <typeparam name="T3"></typeparam>
/// <typeparam name="T4"></typeparam>
/// <typeparam name="T5"></typeparam>
/// <typeparam name="T6"></typeparam>
/// <typeparam name="T7"></typeparam>
/// <typeparam name="T8"></typeparam>
/// <typeparam name="T9"></typeparam>
/// <typeparam name="T10"></typeparam>
[ExcludeFromCodeCoverage]
public readonly struct Path<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
    : IPath
    where T0 : IEntityRelationComponent
    where T1 : IEntityRelationComponent
    where T2 : IEntityRelationComponent
    where T3 : IEntityRelationComponent
    where T4 : IEntityRelationComponent
    where T5 : IEntityRelationComponent
    where T6 : IEntityRelationComponent
    where T7 : IEntityRelationComponent
    where T8 : IEntityRelationComponent
    where T9 : IEntityRelationComponent
    where T10 : IEntityRelationComponent
{
    /// <inheritdoc />
    public Entity? TryFollow(Entity entity)
    {
        if (!TryFollow<T0>(ref entity))
            return null;
        if (!TryFollow<T1>(ref entity))
            return null;
        if (!TryFollow<T2>(ref entity))
            return null;
        if (!TryFollow<T3>(ref entity))
            return null;
        if (!TryFollow<T4>(ref entity))
            return null;
        if (!TryFollow<T5>(ref entity))
            return null;
        if (!TryFollow<T6>(ref entity))
            return null;
        if (!TryFollow<T7>(ref entity))
            return null;
        if (!TryFollow<T8>(ref entity))
            return null;
        if (!TryFollow<T9>(ref entity))
            return null;
        if (!TryFollow<T10>(ref entity))
            return null;

        if (!entity.IsAlive())
            return default;

        return entity;
    }

    private static bool TryFollow<T>(ref Entity entity)
        where T : IEntityRelationComponent
    {
        // Try to get entity info for this entity
        var dummy = default(EntityInfo);
        ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

        // Can't follow a path through a dead entity!
        if (isDead)
            return false;

        // Check if the component is present
        if (!entityInfo.Chunk.Archetype.Components.Contains(ComponentID<T>.ID))
            return false;

        // Follow link
        entity = entity.GetComponentRef<T>().Target;
        return true;
    }
}


/// <summary>
/// Construct a path that follows a path entirely of relational components
/// </summary>
/// <typeparam name="T0"></typeparam>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
/// <typeparam name="T3"></typeparam>
/// <typeparam name="T4"></typeparam>
/// <typeparam name="T5"></typeparam>
/// <typeparam name="T6"></typeparam>
/// <typeparam name="T7"></typeparam>
/// <typeparam name="T8"></typeparam>
/// <typeparam name="T9"></typeparam>
/// <typeparam name="T10"></typeparam>
/// <typeparam name="T11"></typeparam>
[ExcludeFromCodeCoverage]
public readonly struct Path<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
    : IPath
    where T0 : IEntityRelationComponent
    where T1 : IEntityRelationComponent
    where T2 : IEntityRelationComponent
    where T3 : IEntityRelationComponent
    where T4 : IEntityRelationComponent
    where T5 : IEntityRelationComponent
    where T6 : IEntityRelationComponent
    where T7 : IEntityRelationComponent
    where T8 : IEntityRelationComponent
    where T9 : IEntityRelationComponent
    where T10 : IEntityRelationComponent
    where T11 : IEntityRelationComponent
{
    /// <inheritdoc />
    public Entity? TryFollow(Entity entity)
    {
        if (!TryFollow<T0>(ref entity))
            return null;
        if (!TryFollow<T1>(ref entity))
            return null;
        if (!TryFollow<T2>(ref entity))
            return null;
        if (!TryFollow<T3>(ref entity))
            return null;
        if (!TryFollow<T4>(ref entity))
            return null;
        if (!TryFollow<T5>(ref entity))
            return null;
        if (!TryFollow<T6>(ref entity))
            return null;
        if (!TryFollow<T7>(ref entity))
            return null;
        if (!TryFollow<T8>(ref entity))
            return null;
        if (!TryFollow<T9>(ref entity))
            return null;
        if (!TryFollow<T10>(ref entity))
            return null;
        if (!TryFollow<T11>(ref entity))
            return null;

        if (!entity.IsAlive())
            return default;

        return entity;
    }

    private static bool TryFollow<T>(ref Entity entity)
        where T : IEntityRelationComponent
    {
        // Try to get entity info for this entity
        var dummy = default(EntityInfo);
        ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

        // Can't follow a path through a dead entity!
        if (isDead)
            return false;

        // Check if the component is present
        if (!entityInfo.Chunk.Archetype.Components.Contains(ComponentID<T>.ID))
            return false;

        // Follow link
        entity = entity.GetComponentRef<T>().Target;
        return true;
    }
}


/// <summary>
/// Construct a path that follows a path entirely of relational components
/// </summary>
/// <typeparam name="T0"></typeparam>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
/// <typeparam name="T3"></typeparam>
/// <typeparam name="T4"></typeparam>
/// <typeparam name="T5"></typeparam>
/// <typeparam name="T6"></typeparam>
/// <typeparam name="T7"></typeparam>
/// <typeparam name="T8"></typeparam>
/// <typeparam name="T9"></typeparam>
/// <typeparam name="T10"></typeparam>
/// <typeparam name="T11"></typeparam>
/// <typeparam name="T12"></typeparam>
[ExcludeFromCodeCoverage]
public readonly struct Path<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
    : IPath
    where T0 : IEntityRelationComponent
    where T1 : IEntityRelationComponent
    where T2 : IEntityRelationComponent
    where T3 : IEntityRelationComponent
    where T4 : IEntityRelationComponent
    where T5 : IEntityRelationComponent
    where T6 : IEntityRelationComponent
    where T7 : IEntityRelationComponent
    where T8 : IEntityRelationComponent
    where T9 : IEntityRelationComponent
    where T10 : IEntityRelationComponent
    where T11 : IEntityRelationComponent
    where T12 : IEntityRelationComponent
{
    /// <inheritdoc />
    public Entity? TryFollow(Entity entity)
    {
        if (!TryFollow<T0>(ref entity))
            return null;
        if (!TryFollow<T1>(ref entity))
            return null;
        if (!TryFollow<T2>(ref entity))
            return null;
        if (!TryFollow<T3>(ref entity))
            return null;
        if (!TryFollow<T4>(ref entity))
            return null;
        if (!TryFollow<T5>(ref entity))
            return null;
        if (!TryFollow<T6>(ref entity))
            return null;
        if (!TryFollow<T7>(ref entity))
            return null;
        if (!TryFollow<T8>(ref entity))
            return null;
        if (!TryFollow<T9>(ref entity))
            return null;
        if (!TryFollow<T10>(ref entity))
            return null;
        if (!TryFollow<T11>(ref entity))
            return null;
        if (!TryFollow<T12>(ref entity))
            return null;

        if (!entity.IsAlive())
            return default;

        return entity;
    }

    private static bool TryFollow<T>(ref Entity entity)
        where T : IEntityRelationComponent
    {
        // Try to get entity info for this entity
        var dummy = default(EntityInfo);
        ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

        // Can't follow a path through a dead entity!
        if (isDead)
            return false;

        // Check if the component is present
        if (!entityInfo.Chunk.Archetype.Components.Contains(ComponentID<T>.ID))
            return false;

        // Follow link
        entity = entity.GetComponentRef<T>().Target;
        return true;
    }
}


/// <summary>
/// Construct a path that follows a path entirely of relational components
/// </summary>
/// <typeparam name="T0"></typeparam>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
/// <typeparam name="T3"></typeparam>
/// <typeparam name="T4"></typeparam>
/// <typeparam name="T5"></typeparam>
/// <typeparam name="T6"></typeparam>
/// <typeparam name="T7"></typeparam>
/// <typeparam name="T8"></typeparam>
/// <typeparam name="T9"></typeparam>
/// <typeparam name="T10"></typeparam>
/// <typeparam name="T11"></typeparam>
/// <typeparam name="T12"></typeparam>
/// <typeparam name="T13"></typeparam>
[ExcludeFromCodeCoverage]
public readonly struct Path<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
    : IPath
    where T0 : IEntityRelationComponent
    where T1 : IEntityRelationComponent
    where T2 : IEntityRelationComponent
    where T3 : IEntityRelationComponent
    where T4 : IEntityRelationComponent
    where T5 : IEntityRelationComponent
    where T6 : IEntityRelationComponent
    where T7 : IEntityRelationComponent
    where T8 : IEntityRelationComponent
    where T9 : IEntityRelationComponent
    where T10 : IEntityRelationComponent
    where T11 : IEntityRelationComponent
    where T12 : IEntityRelationComponent
    where T13 : IEntityRelationComponent
{
    /// <inheritdoc />
    public Entity? TryFollow(Entity entity)
    {
        if (!TryFollow<T0>(ref entity))
            return null;
        if (!TryFollow<T1>(ref entity))
            return null;
        if (!TryFollow<T2>(ref entity))
            return null;
        if (!TryFollow<T3>(ref entity))
            return null;
        if (!TryFollow<T4>(ref entity))
            return null;
        if (!TryFollow<T5>(ref entity))
            return null;
        if (!TryFollow<T6>(ref entity))
            return null;
        if (!TryFollow<T7>(ref entity))
            return null;
        if (!TryFollow<T8>(ref entity))
            return null;
        if (!TryFollow<T9>(ref entity))
            return null;
        if (!TryFollow<T10>(ref entity))
            return null;
        if (!TryFollow<T11>(ref entity))
            return null;
        if (!TryFollow<T12>(ref entity))
            return null;
        if (!TryFollow<T13>(ref entity))
            return null;

        if (!entity.IsAlive())
            return default;

        return entity;
    }

    private static bool TryFollow<T>(ref Entity entity)
        where T : IEntityRelationComponent
    {
        // Try to get entity info for this entity
        var dummy = default(EntityInfo);
        ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

        // Can't follow a path through a dead entity!
        if (isDead)
            return false;

        // Check if the component is present
        if (!entityInfo.Chunk.Archetype.Components.Contains(ComponentID<T>.ID))
            return false;

        // Follow link
        entity = entity.GetComponentRef<T>().Target;
        return true;
    }
}


/// <summary>
/// Construct a path that follows a path entirely of relational components
/// </summary>
/// <typeparam name="T0"></typeparam>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
/// <typeparam name="T3"></typeparam>
/// <typeparam name="T4"></typeparam>
/// <typeparam name="T5"></typeparam>
/// <typeparam name="T6"></typeparam>
/// <typeparam name="T7"></typeparam>
/// <typeparam name="T8"></typeparam>
/// <typeparam name="T9"></typeparam>
/// <typeparam name="T10"></typeparam>
/// <typeparam name="T11"></typeparam>
/// <typeparam name="T12"></typeparam>
/// <typeparam name="T13"></typeparam>
/// <typeparam name="T14"></typeparam>
[ExcludeFromCodeCoverage]
public readonly struct Path<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
    : IPath
    where T0 : IEntityRelationComponent
    where T1 : IEntityRelationComponent
    where T2 : IEntityRelationComponent
    where T3 : IEntityRelationComponent
    where T4 : IEntityRelationComponent
    where T5 : IEntityRelationComponent
    where T6 : IEntityRelationComponent
    where T7 : IEntityRelationComponent
    where T8 : IEntityRelationComponent
    where T9 : IEntityRelationComponent
    where T10 : IEntityRelationComponent
    where T11 : IEntityRelationComponent
    where T12 : IEntityRelationComponent
    where T13 : IEntityRelationComponent
    where T14 : IEntityRelationComponent
{
    /// <inheritdoc />
    public Entity? TryFollow(Entity entity)
    {
        if (!TryFollow<T0>(ref entity))
            return null;
        if (!TryFollow<T1>(ref entity))
            return null;
        if (!TryFollow<T2>(ref entity))
            return null;
        if (!TryFollow<T3>(ref entity))
            return null;
        if (!TryFollow<T4>(ref entity))
            return null;
        if (!TryFollow<T5>(ref entity))
            return null;
        if (!TryFollow<T6>(ref entity))
            return null;
        if (!TryFollow<T7>(ref entity))
            return null;
        if (!TryFollow<T8>(ref entity))
            return null;
        if (!TryFollow<T9>(ref entity))
            return null;
        if (!TryFollow<T10>(ref entity))
            return null;
        if (!TryFollow<T11>(ref entity))
            return null;
        if (!TryFollow<T12>(ref entity))
            return null;
        if (!TryFollow<T13>(ref entity))
            return null;
        if (!TryFollow<T14>(ref entity))
            return null;

        if (!entity.IsAlive())
            return default;

        return entity;
    }

    private static bool TryFollow<T>(ref Entity entity)
        where T : IEntityRelationComponent
    {
        // Try to get entity info for this entity
        var dummy = default(EntityInfo);
        ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

        // Can't follow a path through a dead entity!
        if (isDead)
            return false;

        // Check if the component is present
        if (!entityInfo.Chunk.Archetype.Components.Contains(ComponentID<T>.ID))
            return false;

        // Follow link
        entity = entity.GetComponentRef<T>().Target;
        return true;
    }
}


/// <summary>
/// Construct a path that follows a path entirely of relational components
/// </summary>
/// <typeparam name="T0"></typeparam>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
/// <typeparam name="T3"></typeparam>
/// <typeparam name="T4"></typeparam>
/// <typeparam name="T5"></typeparam>
/// <typeparam name="T6"></typeparam>
/// <typeparam name="T7"></typeparam>
/// <typeparam name="T8"></typeparam>
/// <typeparam name="T9"></typeparam>
/// <typeparam name="T10"></typeparam>
/// <typeparam name="T11"></typeparam>
/// <typeparam name="T12"></typeparam>
/// <typeparam name="T13"></typeparam>
/// <typeparam name="T14"></typeparam>
/// <typeparam name="T15"></typeparam>
[ExcludeFromCodeCoverage]
public readonly struct Path<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
    : IPath
    where T0 : IEntityRelationComponent
    where T1 : IEntityRelationComponent
    where T2 : IEntityRelationComponent
    where T3 : IEntityRelationComponent
    where T4 : IEntityRelationComponent
    where T5 : IEntityRelationComponent
    where T6 : IEntityRelationComponent
    where T7 : IEntityRelationComponent
    where T8 : IEntityRelationComponent
    where T9 : IEntityRelationComponent
    where T10 : IEntityRelationComponent
    where T11 : IEntityRelationComponent
    where T12 : IEntityRelationComponent
    where T13 : IEntityRelationComponent
    where T14 : IEntityRelationComponent
    where T15 : IEntityRelationComponent
{
    /// <inheritdoc />
    public Entity? TryFollow(Entity entity)
    {
        if (!TryFollow<T0>(ref entity))
            return null;
        if (!TryFollow<T1>(ref entity))
            return null;
        if (!TryFollow<T2>(ref entity))
            return null;
        if (!TryFollow<T3>(ref entity))
            return null;
        if (!TryFollow<T4>(ref entity))
            return null;
        if (!TryFollow<T5>(ref entity))
            return null;
        if (!TryFollow<T6>(ref entity))
            return null;
        if (!TryFollow<T7>(ref entity))
            return null;
        if (!TryFollow<T8>(ref entity))
            return null;
        if (!TryFollow<T9>(ref entity))
            return null;
        if (!TryFollow<T10>(ref entity))
            return null;
        if (!TryFollow<T11>(ref entity))
            return null;
        if (!TryFollow<T12>(ref entity))
            return null;
        if (!TryFollow<T13>(ref entity))
            return null;
        if (!TryFollow<T14>(ref entity))
            return null;
        if (!TryFollow<T15>(ref entity))
            return null;

        if (!entity.IsAlive())
            return default;

        return entity;
    }

    private static bool TryFollow<T>(ref Entity entity)
        where T : IEntityRelationComponent
    {
        // Try to get entity info for this entity
        var dummy = default(EntityInfo);
        ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

        // Can't follow a path through a dead entity!
        if (isDead)
            return false;

        // Check if the component is present
        if (!entityInfo.Chunk.Archetype.Components.Contains(ComponentID<T>.ID))
            return false;

        // Follow link
        entity = entity.GetComponentRef<T>().Target;
        return true;
    }
}



