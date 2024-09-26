using Myriad.ECS.Collections;
using Myriad.ECS.Worlds;

namespace Myriad.ECS;

/* dotcover disable */

public readonly partial record struct Entity
{
    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
    public RefTuple<T0, T1> GetComponentRef<T0, T1>(World world)
        where T0 : IComponent
        where T1 : IComponent
    {
        ref var entityInfo = ref world.GetEntityInfo(this);

        return new RefTuple<T0, T1>(
            this,
#if NET6_0_OR_GREATER
            new RefT<T0>(ref entityInfo.Chunk.GetRef<T0>(entityInfo.RowIndex)),
#else
            new RefT<T0>(entityInfo.Chunk.GetComponentArray<T0>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T1>(ref entityInfo.Chunk.GetRef<T1>(entityInfo.RowIndex))
#else
            new RefT<T1>(entityInfo.Chunk.GetComponentArray<T1>(), entityInfo.RowIndex)
#endif

        );
    }

    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
    public RefTuple<T0, T1, T2> GetComponentRef<T0, T1, T2>(World world)
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
    {
        ref var entityInfo = ref world.GetEntityInfo(this);

        return new RefTuple<T0, T1, T2>(
            this,
#if NET6_0_OR_GREATER
            new RefT<T0>(ref entityInfo.Chunk.GetRef<T0>(entityInfo.RowIndex)),
#else
            new RefT<T0>(entityInfo.Chunk.GetComponentArray<T0>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T1>(ref entityInfo.Chunk.GetRef<T1>(entityInfo.RowIndex)),
#else
            new RefT<T1>(entityInfo.Chunk.GetComponentArray<T1>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T2>(ref entityInfo.Chunk.GetRef<T2>(entityInfo.RowIndex))
#else
            new RefT<T2>(entityInfo.Chunk.GetComponentArray<T2>(), entityInfo.RowIndex)
#endif

        );
    }

    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
    public RefTuple<T0, T1, T2, T3> GetComponentRef<T0, T1, T2, T3>(World world)
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
    {
        ref var entityInfo = ref world.GetEntityInfo(this);

        return new RefTuple<T0, T1, T2, T3>(
            this,
#if NET6_0_OR_GREATER
            new RefT<T0>(ref entityInfo.Chunk.GetRef<T0>(entityInfo.RowIndex)),
#else
            new RefT<T0>(entityInfo.Chunk.GetComponentArray<T0>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T1>(ref entityInfo.Chunk.GetRef<T1>(entityInfo.RowIndex)),
#else
            new RefT<T1>(entityInfo.Chunk.GetComponentArray<T1>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T2>(ref entityInfo.Chunk.GetRef<T2>(entityInfo.RowIndex)),
#else
            new RefT<T2>(entityInfo.Chunk.GetComponentArray<T2>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T3>(ref entityInfo.Chunk.GetRef<T3>(entityInfo.RowIndex))
#else
            new RefT<T3>(entityInfo.Chunk.GetComponentArray<T3>(), entityInfo.RowIndex)
#endif

        );
    }

    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
    public RefTuple<T0, T1, T2, T3, T4> GetComponentRef<T0, T1, T2, T3, T4>(World world)
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
    {
        ref var entityInfo = ref world.GetEntityInfo(this);

        return new RefTuple<T0, T1, T2, T3, T4>(
            this,
#if NET6_0_OR_GREATER
            new RefT<T0>(ref entityInfo.Chunk.GetRef<T0>(entityInfo.RowIndex)),
#else
            new RefT<T0>(entityInfo.Chunk.GetComponentArray<T0>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T1>(ref entityInfo.Chunk.GetRef<T1>(entityInfo.RowIndex)),
#else
            new RefT<T1>(entityInfo.Chunk.GetComponentArray<T1>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T2>(ref entityInfo.Chunk.GetRef<T2>(entityInfo.RowIndex)),
#else
            new RefT<T2>(entityInfo.Chunk.GetComponentArray<T2>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T3>(ref entityInfo.Chunk.GetRef<T3>(entityInfo.RowIndex)),
#else
            new RefT<T3>(entityInfo.Chunk.GetComponentArray<T3>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T4>(ref entityInfo.Chunk.GetRef<T4>(entityInfo.RowIndex))
#else
            new RefT<T4>(entityInfo.Chunk.GetComponentArray<T4>(), entityInfo.RowIndex)
#endif

        );
    }

    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
    public RefTuple<T0, T1, T2, T3, T4, T5> GetComponentRef<T0, T1, T2, T3, T4, T5>(World world)
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
    {
        ref var entityInfo = ref world.GetEntityInfo(this);

        return new RefTuple<T0, T1, T2, T3, T4, T5>(
            this,
#if NET6_0_OR_GREATER
            new RefT<T0>(ref entityInfo.Chunk.GetRef<T0>(entityInfo.RowIndex)),
#else
            new RefT<T0>(entityInfo.Chunk.GetComponentArray<T0>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T1>(ref entityInfo.Chunk.GetRef<T1>(entityInfo.RowIndex)),
#else
            new RefT<T1>(entityInfo.Chunk.GetComponentArray<T1>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T2>(ref entityInfo.Chunk.GetRef<T2>(entityInfo.RowIndex)),
#else
            new RefT<T2>(entityInfo.Chunk.GetComponentArray<T2>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T3>(ref entityInfo.Chunk.GetRef<T3>(entityInfo.RowIndex)),
#else
            new RefT<T3>(entityInfo.Chunk.GetComponentArray<T3>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T4>(ref entityInfo.Chunk.GetRef<T4>(entityInfo.RowIndex)),
#else
            new RefT<T4>(entityInfo.Chunk.GetComponentArray<T4>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T5>(ref entityInfo.Chunk.GetRef<T5>(entityInfo.RowIndex))
#else
            new RefT<T5>(entityInfo.Chunk.GetComponentArray<T5>(), entityInfo.RowIndex)
#endif

        );
    }

    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
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

        return new RefTuple<T0, T1, T2, T3, T4, T5, T6>(
            this,
#if NET6_0_OR_GREATER
            new RefT<T0>(ref entityInfo.Chunk.GetRef<T0>(entityInfo.RowIndex)),
#else
            new RefT<T0>(entityInfo.Chunk.GetComponentArray<T0>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T1>(ref entityInfo.Chunk.GetRef<T1>(entityInfo.RowIndex)),
#else
            new RefT<T1>(entityInfo.Chunk.GetComponentArray<T1>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T2>(ref entityInfo.Chunk.GetRef<T2>(entityInfo.RowIndex)),
#else
            new RefT<T2>(entityInfo.Chunk.GetComponentArray<T2>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T3>(ref entityInfo.Chunk.GetRef<T3>(entityInfo.RowIndex)),
#else
            new RefT<T3>(entityInfo.Chunk.GetComponentArray<T3>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T4>(ref entityInfo.Chunk.GetRef<T4>(entityInfo.RowIndex)),
#else
            new RefT<T4>(entityInfo.Chunk.GetComponentArray<T4>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T5>(ref entityInfo.Chunk.GetRef<T5>(entityInfo.RowIndex)),
#else
            new RefT<T5>(entityInfo.Chunk.GetComponentArray<T5>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T6>(ref entityInfo.Chunk.GetRef<T6>(entityInfo.RowIndex))
#else
            new RefT<T6>(entityInfo.Chunk.GetComponentArray<T6>(), entityInfo.RowIndex)
#endif

        );
    }

    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
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

        return new RefTuple<T0, T1, T2, T3, T4, T5, T6, T7>(
            this,
#if NET6_0_OR_GREATER
            new RefT<T0>(ref entityInfo.Chunk.GetRef<T0>(entityInfo.RowIndex)),
#else
            new RefT<T0>(entityInfo.Chunk.GetComponentArray<T0>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T1>(ref entityInfo.Chunk.GetRef<T1>(entityInfo.RowIndex)),
#else
            new RefT<T1>(entityInfo.Chunk.GetComponentArray<T1>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T2>(ref entityInfo.Chunk.GetRef<T2>(entityInfo.RowIndex)),
#else
            new RefT<T2>(entityInfo.Chunk.GetComponentArray<T2>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T3>(ref entityInfo.Chunk.GetRef<T3>(entityInfo.RowIndex)),
#else
            new RefT<T3>(entityInfo.Chunk.GetComponentArray<T3>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T4>(ref entityInfo.Chunk.GetRef<T4>(entityInfo.RowIndex)),
#else
            new RefT<T4>(entityInfo.Chunk.GetComponentArray<T4>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T5>(ref entityInfo.Chunk.GetRef<T5>(entityInfo.RowIndex)),
#else
            new RefT<T5>(entityInfo.Chunk.GetComponentArray<T5>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T6>(ref entityInfo.Chunk.GetRef<T6>(entityInfo.RowIndex)),
#else
            new RefT<T6>(entityInfo.Chunk.GetComponentArray<T6>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T7>(ref entityInfo.Chunk.GetRef<T7>(entityInfo.RowIndex))
#else
            new RefT<T7>(entityInfo.Chunk.GetComponentArray<T7>(), entityInfo.RowIndex)
#endif

        );
    }

    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
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

        return new RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8>(
            this,
#if NET6_0_OR_GREATER
            new RefT<T0>(ref entityInfo.Chunk.GetRef<T0>(entityInfo.RowIndex)),
#else
            new RefT<T0>(entityInfo.Chunk.GetComponentArray<T0>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T1>(ref entityInfo.Chunk.GetRef<T1>(entityInfo.RowIndex)),
#else
            new RefT<T1>(entityInfo.Chunk.GetComponentArray<T1>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T2>(ref entityInfo.Chunk.GetRef<T2>(entityInfo.RowIndex)),
#else
            new RefT<T2>(entityInfo.Chunk.GetComponentArray<T2>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T3>(ref entityInfo.Chunk.GetRef<T3>(entityInfo.RowIndex)),
#else
            new RefT<T3>(entityInfo.Chunk.GetComponentArray<T3>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T4>(ref entityInfo.Chunk.GetRef<T4>(entityInfo.RowIndex)),
#else
            new RefT<T4>(entityInfo.Chunk.GetComponentArray<T4>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T5>(ref entityInfo.Chunk.GetRef<T5>(entityInfo.RowIndex)),
#else
            new RefT<T5>(entityInfo.Chunk.GetComponentArray<T5>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T6>(ref entityInfo.Chunk.GetRef<T6>(entityInfo.RowIndex)),
#else
            new RefT<T6>(entityInfo.Chunk.GetComponentArray<T6>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T7>(ref entityInfo.Chunk.GetRef<T7>(entityInfo.RowIndex)),
#else
            new RefT<T7>(entityInfo.Chunk.GetComponentArray<T7>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T8>(ref entityInfo.Chunk.GetRef<T8>(entityInfo.RowIndex))
#else
            new RefT<T8>(entityInfo.Chunk.GetComponentArray<T8>(), entityInfo.RowIndex)
#endif

        );
    }

    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
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

        return new RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            this,
#if NET6_0_OR_GREATER
            new RefT<T0>(ref entityInfo.Chunk.GetRef<T0>(entityInfo.RowIndex)),
#else
            new RefT<T0>(entityInfo.Chunk.GetComponentArray<T0>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T1>(ref entityInfo.Chunk.GetRef<T1>(entityInfo.RowIndex)),
#else
            new RefT<T1>(entityInfo.Chunk.GetComponentArray<T1>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T2>(ref entityInfo.Chunk.GetRef<T2>(entityInfo.RowIndex)),
#else
            new RefT<T2>(entityInfo.Chunk.GetComponentArray<T2>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T3>(ref entityInfo.Chunk.GetRef<T3>(entityInfo.RowIndex)),
#else
            new RefT<T3>(entityInfo.Chunk.GetComponentArray<T3>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T4>(ref entityInfo.Chunk.GetRef<T4>(entityInfo.RowIndex)),
#else
            new RefT<T4>(entityInfo.Chunk.GetComponentArray<T4>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T5>(ref entityInfo.Chunk.GetRef<T5>(entityInfo.RowIndex)),
#else
            new RefT<T5>(entityInfo.Chunk.GetComponentArray<T5>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T6>(ref entityInfo.Chunk.GetRef<T6>(entityInfo.RowIndex)),
#else
            new RefT<T6>(entityInfo.Chunk.GetComponentArray<T6>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T7>(ref entityInfo.Chunk.GetRef<T7>(entityInfo.RowIndex)),
#else
            new RefT<T7>(entityInfo.Chunk.GetComponentArray<T7>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T8>(ref entityInfo.Chunk.GetRef<T8>(entityInfo.RowIndex)),
#else
            new RefT<T8>(entityInfo.Chunk.GetComponentArray<T8>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T9>(ref entityInfo.Chunk.GetRef<T9>(entityInfo.RowIndex))
#else
            new RefT<T9>(entityInfo.Chunk.GetComponentArray<T9>(), entityInfo.RowIndex)
#endif

        );
    }

    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
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

        return new RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            this,
#if NET6_0_OR_GREATER
            new RefT<T0>(ref entityInfo.Chunk.GetRef<T0>(entityInfo.RowIndex)),
#else
            new RefT<T0>(entityInfo.Chunk.GetComponentArray<T0>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T1>(ref entityInfo.Chunk.GetRef<T1>(entityInfo.RowIndex)),
#else
            new RefT<T1>(entityInfo.Chunk.GetComponentArray<T1>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T2>(ref entityInfo.Chunk.GetRef<T2>(entityInfo.RowIndex)),
#else
            new RefT<T2>(entityInfo.Chunk.GetComponentArray<T2>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T3>(ref entityInfo.Chunk.GetRef<T3>(entityInfo.RowIndex)),
#else
            new RefT<T3>(entityInfo.Chunk.GetComponentArray<T3>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T4>(ref entityInfo.Chunk.GetRef<T4>(entityInfo.RowIndex)),
#else
            new RefT<T4>(entityInfo.Chunk.GetComponentArray<T4>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T5>(ref entityInfo.Chunk.GetRef<T5>(entityInfo.RowIndex)),
#else
            new RefT<T5>(entityInfo.Chunk.GetComponentArray<T5>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T6>(ref entityInfo.Chunk.GetRef<T6>(entityInfo.RowIndex)),
#else
            new RefT<T6>(entityInfo.Chunk.GetComponentArray<T6>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T7>(ref entityInfo.Chunk.GetRef<T7>(entityInfo.RowIndex)),
#else
            new RefT<T7>(entityInfo.Chunk.GetComponentArray<T7>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T8>(ref entityInfo.Chunk.GetRef<T8>(entityInfo.RowIndex)),
#else
            new RefT<T8>(entityInfo.Chunk.GetComponentArray<T8>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T9>(ref entityInfo.Chunk.GetRef<T9>(entityInfo.RowIndex)),
#else
            new RefT<T9>(entityInfo.Chunk.GetComponentArray<T9>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T10>(ref entityInfo.Chunk.GetRef<T10>(entityInfo.RowIndex))
#else
            new RefT<T10>(entityInfo.Chunk.GetComponentArray<T10>(), entityInfo.RowIndex)
#endif

        );
    }

    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
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

        return new RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            this,
#if NET6_0_OR_GREATER
            new RefT<T0>(ref entityInfo.Chunk.GetRef<T0>(entityInfo.RowIndex)),
#else
            new RefT<T0>(entityInfo.Chunk.GetComponentArray<T0>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T1>(ref entityInfo.Chunk.GetRef<T1>(entityInfo.RowIndex)),
#else
            new RefT<T1>(entityInfo.Chunk.GetComponentArray<T1>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T2>(ref entityInfo.Chunk.GetRef<T2>(entityInfo.RowIndex)),
#else
            new RefT<T2>(entityInfo.Chunk.GetComponentArray<T2>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T3>(ref entityInfo.Chunk.GetRef<T3>(entityInfo.RowIndex)),
#else
            new RefT<T3>(entityInfo.Chunk.GetComponentArray<T3>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T4>(ref entityInfo.Chunk.GetRef<T4>(entityInfo.RowIndex)),
#else
            new RefT<T4>(entityInfo.Chunk.GetComponentArray<T4>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T5>(ref entityInfo.Chunk.GetRef<T5>(entityInfo.RowIndex)),
#else
            new RefT<T5>(entityInfo.Chunk.GetComponentArray<T5>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T6>(ref entityInfo.Chunk.GetRef<T6>(entityInfo.RowIndex)),
#else
            new RefT<T6>(entityInfo.Chunk.GetComponentArray<T6>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T7>(ref entityInfo.Chunk.GetRef<T7>(entityInfo.RowIndex)),
#else
            new RefT<T7>(entityInfo.Chunk.GetComponentArray<T7>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T8>(ref entityInfo.Chunk.GetRef<T8>(entityInfo.RowIndex)),
#else
            new RefT<T8>(entityInfo.Chunk.GetComponentArray<T8>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T9>(ref entityInfo.Chunk.GetRef<T9>(entityInfo.RowIndex)),
#else
            new RefT<T9>(entityInfo.Chunk.GetComponentArray<T9>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T10>(ref entityInfo.Chunk.GetRef<T10>(entityInfo.RowIndex)),
#else
            new RefT<T10>(entityInfo.Chunk.GetComponentArray<T10>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T11>(ref entityInfo.Chunk.GetRef<T11>(entityInfo.RowIndex))
#else
            new RefT<T11>(entityInfo.Chunk.GetComponentArray<T11>(), entityInfo.RowIndex)
#endif

        );
    }

    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
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

        return new RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            this,
#if NET6_0_OR_GREATER
            new RefT<T0>(ref entityInfo.Chunk.GetRef<T0>(entityInfo.RowIndex)),
#else
            new RefT<T0>(entityInfo.Chunk.GetComponentArray<T0>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T1>(ref entityInfo.Chunk.GetRef<T1>(entityInfo.RowIndex)),
#else
            new RefT<T1>(entityInfo.Chunk.GetComponentArray<T1>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T2>(ref entityInfo.Chunk.GetRef<T2>(entityInfo.RowIndex)),
#else
            new RefT<T2>(entityInfo.Chunk.GetComponentArray<T2>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T3>(ref entityInfo.Chunk.GetRef<T3>(entityInfo.RowIndex)),
#else
            new RefT<T3>(entityInfo.Chunk.GetComponentArray<T3>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T4>(ref entityInfo.Chunk.GetRef<T4>(entityInfo.RowIndex)),
#else
            new RefT<T4>(entityInfo.Chunk.GetComponentArray<T4>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T5>(ref entityInfo.Chunk.GetRef<T5>(entityInfo.RowIndex)),
#else
            new RefT<T5>(entityInfo.Chunk.GetComponentArray<T5>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T6>(ref entityInfo.Chunk.GetRef<T6>(entityInfo.RowIndex)),
#else
            new RefT<T6>(entityInfo.Chunk.GetComponentArray<T6>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T7>(ref entityInfo.Chunk.GetRef<T7>(entityInfo.RowIndex)),
#else
            new RefT<T7>(entityInfo.Chunk.GetComponentArray<T7>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T8>(ref entityInfo.Chunk.GetRef<T8>(entityInfo.RowIndex)),
#else
            new RefT<T8>(entityInfo.Chunk.GetComponentArray<T8>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T9>(ref entityInfo.Chunk.GetRef<T9>(entityInfo.RowIndex)),
#else
            new RefT<T9>(entityInfo.Chunk.GetComponentArray<T9>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T10>(ref entityInfo.Chunk.GetRef<T10>(entityInfo.RowIndex)),
#else
            new RefT<T10>(entityInfo.Chunk.GetComponentArray<T10>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T11>(ref entityInfo.Chunk.GetRef<T11>(entityInfo.RowIndex)),
#else
            new RefT<T11>(entityInfo.Chunk.GetComponentArray<T11>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T12>(ref entityInfo.Chunk.GetRef<T12>(entityInfo.RowIndex))
#else
            new RefT<T12>(entityInfo.Chunk.GetComponentArray<T12>(), entityInfo.RowIndex)
#endif

        );
    }

    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
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

        return new RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this,
#if NET6_0_OR_GREATER
            new RefT<T0>(ref entityInfo.Chunk.GetRef<T0>(entityInfo.RowIndex)),
#else
            new RefT<T0>(entityInfo.Chunk.GetComponentArray<T0>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T1>(ref entityInfo.Chunk.GetRef<T1>(entityInfo.RowIndex)),
#else
            new RefT<T1>(entityInfo.Chunk.GetComponentArray<T1>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T2>(ref entityInfo.Chunk.GetRef<T2>(entityInfo.RowIndex)),
#else
            new RefT<T2>(entityInfo.Chunk.GetComponentArray<T2>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T3>(ref entityInfo.Chunk.GetRef<T3>(entityInfo.RowIndex)),
#else
            new RefT<T3>(entityInfo.Chunk.GetComponentArray<T3>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T4>(ref entityInfo.Chunk.GetRef<T4>(entityInfo.RowIndex)),
#else
            new RefT<T4>(entityInfo.Chunk.GetComponentArray<T4>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T5>(ref entityInfo.Chunk.GetRef<T5>(entityInfo.RowIndex)),
#else
            new RefT<T5>(entityInfo.Chunk.GetComponentArray<T5>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T6>(ref entityInfo.Chunk.GetRef<T6>(entityInfo.RowIndex)),
#else
            new RefT<T6>(entityInfo.Chunk.GetComponentArray<T6>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T7>(ref entityInfo.Chunk.GetRef<T7>(entityInfo.RowIndex)),
#else
            new RefT<T7>(entityInfo.Chunk.GetComponentArray<T7>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T8>(ref entityInfo.Chunk.GetRef<T8>(entityInfo.RowIndex)),
#else
            new RefT<T8>(entityInfo.Chunk.GetComponentArray<T8>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T9>(ref entityInfo.Chunk.GetRef<T9>(entityInfo.RowIndex)),
#else
            new RefT<T9>(entityInfo.Chunk.GetComponentArray<T9>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T10>(ref entityInfo.Chunk.GetRef<T10>(entityInfo.RowIndex)),
#else
            new RefT<T10>(entityInfo.Chunk.GetComponentArray<T10>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T11>(ref entityInfo.Chunk.GetRef<T11>(entityInfo.RowIndex)),
#else
            new RefT<T11>(entityInfo.Chunk.GetComponentArray<T11>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T12>(ref entityInfo.Chunk.GetRef<T12>(entityInfo.RowIndex)),
#else
            new RefT<T12>(entityInfo.Chunk.GetComponentArray<T12>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T13>(ref entityInfo.Chunk.GetRef<T13>(entityInfo.RowIndex))
#else
            new RefT<T13>(entityInfo.Chunk.GetComponentArray<T13>(), entityInfo.RowIndex)
#endif

        );
    }

    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
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

        return new RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this,
#if NET6_0_OR_GREATER
            new RefT<T0>(ref entityInfo.Chunk.GetRef<T0>(entityInfo.RowIndex)),
#else
            new RefT<T0>(entityInfo.Chunk.GetComponentArray<T0>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T1>(ref entityInfo.Chunk.GetRef<T1>(entityInfo.RowIndex)),
#else
            new RefT<T1>(entityInfo.Chunk.GetComponentArray<T1>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T2>(ref entityInfo.Chunk.GetRef<T2>(entityInfo.RowIndex)),
#else
            new RefT<T2>(entityInfo.Chunk.GetComponentArray<T2>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T3>(ref entityInfo.Chunk.GetRef<T3>(entityInfo.RowIndex)),
#else
            new RefT<T3>(entityInfo.Chunk.GetComponentArray<T3>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T4>(ref entityInfo.Chunk.GetRef<T4>(entityInfo.RowIndex)),
#else
            new RefT<T4>(entityInfo.Chunk.GetComponentArray<T4>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T5>(ref entityInfo.Chunk.GetRef<T5>(entityInfo.RowIndex)),
#else
            new RefT<T5>(entityInfo.Chunk.GetComponentArray<T5>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T6>(ref entityInfo.Chunk.GetRef<T6>(entityInfo.RowIndex)),
#else
            new RefT<T6>(entityInfo.Chunk.GetComponentArray<T6>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T7>(ref entityInfo.Chunk.GetRef<T7>(entityInfo.RowIndex)),
#else
            new RefT<T7>(entityInfo.Chunk.GetComponentArray<T7>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T8>(ref entityInfo.Chunk.GetRef<T8>(entityInfo.RowIndex)),
#else
            new RefT<T8>(entityInfo.Chunk.GetComponentArray<T8>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T9>(ref entityInfo.Chunk.GetRef<T9>(entityInfo.RowIndex)),
#else
            new RefT<T9>(entityInfo.Chunk.GetComponentArray<T9>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T10>(ref entityInfo.Chunk.GetRef<T10>(entityInfo.RowIndex)),
#else
            new RefT<T10>(entityInfo.Chunk.GetComponentArray<T10>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T11>(ref entityInfo.Chunk.GetRef<T11>(entityInfo.RowIndex)),
#else
            new RefT<T11>(entityInfo.Chunk.GetComponentArray<T11>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T12>(ref entityInfo.Chunk.GetRef<T12>(entityInfo.RowIndex)),
#else
            new RefT<T12>(entityInfo.Chunk.GetComponentArray<T12>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T13>(ref entityInfo.Chunk.GetRef<T13>(entityInfo.RowIndex)),
#else
            new RefT<T13>(entityInfo.Chunk.GetComponentArray<T13>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T14>(ref entityInfo.Chunk.GetRef<T14>(entityInfo.RowIndex))
#else
            new RefT<T14>(entityInfo.Chunk.GetComponentArray<T14>(), entityInfo.RowIndex)
#endif

        );
    }

    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
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

        return new RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this,
#if NET6_0_OR_GREATER
            new RefT<T0>(ref entityInfo.Chunk.GetRef<T0>(entityInfo.RowIndex)),
#else
            new RefT<T0>(entityInfo.Chunk.GetComponentArray<T0>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T1>(ref entityInfo.Chunk.GetRef<T1>(entityInfo.RowIndex)),
#else
            new RefT<T1>(entityInfo.Chunk.GetComponentArray<T1>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T2>(ref entityInfo.Chunk.GetRef<T2>(entityInfo.RowIndex)),
#else
            new RefT<T2>(entityInfo.Chunk.GetComponentArray<T2>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T3>(ref entityInfo.Chunk.GetRef<T3>(entityInfo.RowIndex)),
#else
            new RefT<T3>(entityInfo.Chunk.GetComponentArray<T3>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T4>(ref entityInfo.Chunk.GetRef<T4>(entityInfo.RowIndex)),
#else
            new RefT<T4>(entityInfo.Chunk.GetComponentArray<T4>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T5>(ref entityInfo.Chunk.GetRef<T5>(entityInfo.RowIndex)),
#else
            new RefT<T5>(entityInfo.Chunk.GetComponentArray<T5>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T6>(ref entityInfo.Chunk.GetRef<T6>(entityInfo.RowIndex)),
#else
            new RefT<T6>(entityInfo.Chunk.GetComponentArray<T6>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T7>(ref entityInfo.Chunk.GetRef<T7>(entityInfo.RowIndex)),
#else
            new RefT<T7>(entityInfo.Chunk.GetComponentArray<T7>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T8>(ref entityInfo.Chunk.GetRef<T8>(entityInfo.RowIndex)),
#else
            new RefT<T8>(entityInfo.Chunk.GetComponentArray<T8>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T9>(ref entityInfo.Chunk.GetRef<T9>(entityInfo.RowIndex)),
#else
            new RefT<T9>(entityInfo.Chunk.GetComponentArray<T9>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T10>(ref entityInfo.Chunk.GetRef<T10>(entityInfo.RowIndex)),
#else
            new RefT<T10>(entityInfo.Chunk.GetComponentArray<T10>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T11>(ref entityInfo.Chunk.GetRef<T11>(entityInfo.RowIndex)),
#else
            new RefT<T11>(entityInfo.Chunk.GetComponentArray<T11>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T12>(ref entityInfo.Chunk.GetRef<T12>(entityInfo.RowIndex)),
#else
            new RefT<T12>(entityInfo.Chunk.GetComponentArray<T12>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T13>(ref entityInfo.Chunk.GetRef<T13>(entityInfo.RowIndex)),
#else
            new RefT<T13>(entityInfo.Chunk.GetComponentArray<T13>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T14>(ref entityInfo.Chunk.GetRef<T14>(entityInfo.RowIndex)),
#else
            new RefT<T14>(entityInfo.Chunk.GetComponentArray<T14>(), entityInfo.RowIndex),
#endif

#if NET6_0_OR_GREATER
            new RefT<T15>(ref entityInfo.Chunk.GetRef<T15>(entityInfo.RowIndex))
#else
            new RefT<T15>(entityInfo.Chunk.GetComponentArray<T15>(), entityInfo.RowIndex)
#endif

        );
    }

}

