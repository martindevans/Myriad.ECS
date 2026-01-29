using System.Diagnostics;
using Myriad.ECS.Collections;
using System.Diagnostics.CodeAnalysis;
using Myriad.ECS.IDs;

namespace Myriad.ECS.Worlds.Chunks;

/* dotcover disable */

internal sealed partial class Chunk
{
    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    
    internal RefTuple<T0> GetRefTuple<T0>(int rowIndex,
        ComponentID c0
    )
        where T0 : IComponent
    {
        Debug.Assert(rowIndex < EntityCount);
        Debug.Assert(rowIndex >= 0);

        // Get the component(s)
        return new RefTuple<T0>(
            _entities[rowIndex],
            GetRefT<T0>(rowIndex, c0)
        );
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    
    internal RefTuple<T0, T1> GetRefTuple<T0, T1>(int rowIndex,
        ComponentID c0,
        ComponentID c1
    )
        where T0 : IComponent
        where T1 : IComponent
    {
        Debug.Assert(rowIndex < EntityCount);
        Debug.Assert(rowIndex >= 0);

        // Get the component(s)
        return new RefTuple<T0, T1>(
            _entities[rowIndex],
            GetRefT<T0>(rowIndex, c0),
            GetRefT<T1>(rowIndex, c1)
        );
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal RefTuple<T0, T1, T2> GetRefTuple<T0, T1, T2>(int rowIndex,
        ComponentID c0,
        ComponentID c1,
        ComponentID c2
    )
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
    {
        Debug.Assert(rowIndex < EntityCount);
        Debug.Assert(rowIndex >= 0);

        // Get the component(s)
        return new RefTuple<T0, T1, T2>(
            _entities[rowIndex],
            GetRefT<T0>(rowIndex, c0),
            GetRefT<T1>(rowIndex, c1),
            GetRefT<T2>(rowIndex, c2)
        );
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal RefTuple<T0, T1, T2, T3> GetRefTuple<T0, T1, T2, T3>(int rowIndex,
        ComponentID c0,
        ComponentID c1,
        ComponentID c2,
        ComponentID c3
    )
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
    {
        Debug.Assert(rowIndex < EntityCount);
        Debug.Assert(rowIndex >= 0);

        // Get the component(s)
        return new RefTuple<T0, T1, T2, T3>(
            _entities[rowIndex],
            GetRefT<T0>(rowIndex, c0),
            GetRefT<T1>(rowIndex, c1),
            GetRefT<T2>(rowIndex, c2),
            GetRefT<T3>(rowIndex, c3)
        );
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal RefTuple<T0, T1, T2, T3, T4> GetRefTuple<T0, T1, T2, T3, T4>(int rowIndex,
        ComponentID c0,
        ComponentID c1,
        ComponentID c2,
        ComponentID c3,
        ComponentID c4
    )
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
    {
        Debug.Assert(rowIndex < EntityCount);
        Debug.Assert(rowIndex >= 0);

        // Get the component(s)
        return new RefTuple<T0, T1, T2, T3, T4>(
            _entities[rowIndex],
            GetRefT<T0>(rowIndex, c0),
            GetRefT<T1>(rowIndex, c1),
            GetRefT<T2>(rowIndex, c2),
            GetRefT<T3>(rowIndex, c3),
            GetRefT<T4>(rowIndex, c4)
        );
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal RefTuple<T0, T1, T2, T3, T4, T5> GetRefTuple<T0, T1, T2, T3, T4, T5>(int rowIndex,
        ComponentID c0,
        ComponentID c1,
        ComponentID c2,
        ComponentID c3,
        ComponentID c4,
        ComponentID c5
    )
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
    {
        Debug.Assert(rowIndex < EntityCount);
        Debug.Assert(rowIndex >= 0);

        // Get the component(s)
        return new RefTuple<T0, T1, T2, T3, T4, T5>(
            _entities[rowIndex],
            GetRefT<T0>(rowIndex, c0),
            GetRefT<T1>(rowIndex, c1),
            GetRefT<T2>(rowIndex, c2),
            GetRefT<T3>(rowIndex, c3),
            GetRefT<T4>(rowIndex, c4),
            GetRefT<T5>(rowIndex, c5)
        );
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal RefTuple<T0, T1, T2, T3, T4, T5, T6> GetRefTuple<T0, T1, T2, T3, T4, T5, T6>(int rowIndex,
        ComponentID c0,
        ComponentID c1,
        ComponentID c2,
        ComponentID c3,
        ComponentID c4,
        ComponentID c5,
        ComponentID c6
    )
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
    {
        Debug.Assert(rowIndex < EntityCount);
        Debug.Assert(rowIndex >= 0);

        // Get the component(s)
        return new RefTuple<T0, T1, T2, T3, T4, T5, T6>(
            _entities[rowIndex],
            GetRefT<T0>(rowIndex, c0),
            GetRefT<T1>(rowIndex, c1),
            GetRefT<T2>(rowIndex, c2),
            GetRefT<T3>(rowIndex, c3),
            GetRefT<T4>(rowIndex, c4),
            GetRefT<T5>(rowIndex, c5),
            GetRefT<T6>(rowIndex, c6)
        );
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal RefTuple<T0, T1, T2, T3, T4, T5, T6, T7> GetRefTuple<T0, T1, T2, T3, T4, T5, T6, T7>(int rowIndex,
        ComponentID c0,
        ComponentID c1,
        ComponentID c2,
        ComponentID c3,
        ComponentID c4,
        ComponentID c5,
        ComponentID c6,
        ComponentID c7
    )
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
    {
        Debug.Assert(rowIndex < EntityCount);
        Debug.Assert(rowIndex >= 0);

        // Get the component(s)
        return new RefTuple<T0, T1, T2, T3, T4, T5, T6, T7>(
            _entities[rowIndex],
            GetRefT<T0>(rowIndex, c0),
            GetRefT<T1>(rowIndex, c1),
            GetRefT<T2>(rowIndex, c2),
            GetRefT<T3>(rowIndex, c3),
            GetRefT<T4>(rowIndex, c4),
            GetRefT<T5>(rowIndex, c5),
            GetRefT<T6>(rowIndex, c6),
            GetRefT<T7>(rowIndex, c7)
        );
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8> GetRefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8>(int rowIndex,
        ComponentID c0,
        ComponentID c1,
        ComponentID c2,
        ComponentID c3,
        ComponentID c4,
        ComponentID c5,
        ComponentID c6,
        ComponentID c7,
        ComponentID c8
    )
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
        Debug.Assert(rowIndex < EntityCount);
        Debug.Assert(rowIndex >= 0);

        // Get the component(s)
        return new RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8>(
            _entities[rowIndex],
            GetRefT<T0>(rowIndex, c0),
            GetRefT<T1>(rowIndex, c1),
            GetRefT<T2>(rowIndex, c2),
            GetRefT<T3>(rowIndex, c3),
            GetRefT<T4>(rowIndex, c4),
            GetRefT<T5>(rowIndex, c5),
            GetRefT<T6>(rowIndex, c6),
            GetRefT<T7>(rowIndex, c7),
            GetRefT<T8>(rowIndex, c8)
        );
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> GetRefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(int rowIndex,
        ComponentID c0,
        ComponentID c1,
        ComponentID c2,
        ComponentID c3,
        ComponentID c4,
        ComponentID c5,
        ComponentID c6,
        ComponentID c7,
        ComponentID c8,
        ComponentID c9
    )
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
        Debug.Assert(rowIndex < EntityCount);
        Debug.Assert(rowIndex >= 0);

        // Get the component(s)
        return new RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            _entities[rowIndex],
            GetRefT<T0>(rowIndex, c0),
            GetRefT<T1>(rowIndex, c1),
            GetRefT<T2>(rowIndex, c2),
            GetRefT<T3>(rowIndex, c3),
            GetRefT<T4>(rowIndex, c4),
            GetRefT<T5>(rowIndex, c5),
            GetRefT<T6>(rowIndex, c6),
            GetRefT<T7>(rowIndex, c7),
            GetRefT<T8>(rowIndex, c8),
            GetRefT<T9>(rowIndex, c9)
        );
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> GetRefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(int rowIndex,
        ComponentID c0,
        ComponentID c1,
        ComponentID c2,
        ComponentID c3,
        ComponentID c4,
        ComponentID c5,
        ComponentID c6,
        ComponentID c7,
        ComponentID c8,
        ComponentID c9,
        ComponentID c10
    )
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
        Debug.Assert(rowIndex < EntityCount);
        Debug.Assert(rowIndex >= 0);

        // Get the component(s)
        return new RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            _entities[rowIndex],
            GetRefT<T0>(rowIndex, c0),
            GetRefT<T1>(rowIndex, c1),
            GetRefT<T2>(rowIndex, c2),
            GetRefT<T3>(rowIndex, c3),
            GetRefT<T4>(rowIndex, c4),
            GetRefT<T5>(rowIndex, c5),
            GetRefT<T6>(rowIndex, c6),
            GetRefT<T7>(rowIndex, c7),
            GetRefT<T8>(rowIndex, c8),
            GetRefT<T9>(rowIndex, c9),
            GetRefT<T10>(rowIndex, c10)
        );
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> GetRefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(int rowIndex,
        ComponentID c0,
        ComponentID c1,
        ComponentID c2,
        ComponentID c3,
        ComponentID c4,
        ComponentID c5,
        ComponentID c6,
        ComponentID c7,
        ComponentID c8,
        ComponentID c9,
        ComponentID c10,
        ComponentID c11
    )
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
        Debug.Assert(rowIndex < EntityCount);
        Debug.Assert(rowIndex >= 0);

        // Get the component(s)
        return new RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            _entities[rowIndex],
            GetRefT<T0>(rowIndex, c0),
            GetRefT<T1>(rowIndex, c1),
            GetRefT<T2>(rowIndex, c2),
            GetRefT<T3>(rowIndex, c3),
            GetRefT<T4>(rowIndex, c4),
            GetRefT<T5>(rowIndex, c5),
            GetRefT<T6>(rowIndex, c6),
            GetRefT<T7>(rowIndex, c7),
            GetRefT<T8>(rowIndex, c8),
            GetRefT<T9>(rowIndex, c9),
            GetRefT<T10>(rowIndex, c10),
            GetRefT<T11>(rowIndex, c11)
        );
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> GetRefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(int rowIndex,
        ComponentID c0,
        ComponentID c1,
        ComponentID c2,
        ComponentID c3,
        ComponentID c4,
        ComponentID c5,
        ComponentID c6,
        ComponentID c7,
        ComponentID c8,
        ComponentID c9,
        ComponentID c10,
        ComponentID c11,
        ComponentID c12
    )
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
        Debug.Assert(rowIndex < EntityCount);
        Debug.Assert(rowIndex >= 0);

        // Get the component(s)
        return new RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            _entities[rowIndex],
            GetRefT<T0>(rowIndex, c0),
            GetRefT<T1>(rowIndex, c1),
            GetRefT<T2>(rowIndex, c2),
            GetRefT<T3>(rowIndex, c3),
            GetRefT<T4>(rowIndex, c4),
            GetRefT<T5>(rowIndex, c5),
            GetRefT<T6>(rowIndex, c6),
            GetRefT<T7>(rowIndex, c7),
            GetRefT<T8>(rowIndex, c8),
            GetRefT<T9>(rowIndex, c9),
            GetRefT<T10>(rowIndex, c10),
            GetRefT<T11>(rowIndex, c11),
            GetRefT<T12>(rowIndex, c12)
        );
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> GetRefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(int rowIndex,
        ComponentID c0,
        ComponentID c1,
        ComponentID c2,
        ComponentID c3,
        ComponentID c4,
        ComponentID c5,
        ComponentID c6,
        ComponentID c7,
        ComponentID c8,
        ComponentID c9,
        ComponentID c10,
        ComponentID c11,
        ComponentID c12,
        ComponentID c13
    )
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
        Debug.Assert(rowIndex < EntityCount);
        Debug.Assert(rowIndex >= 0);

        // Get the component(s)
        return new RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            _entities[rowIndex],
            GetRefT<T0>(rowIndex, c0),
            GetRefT<T1>(rowIndex, c1),
            GetRefT<T2>(rowIndex, c2),
            GetRefT<T3>(rowIndex, c3),
            GetRefT<T4>(rowIndex, c4),
            GetRefT<T5>(rowIndex, c5),
            GetRefT<T6>(rowIndex, c6),
            GetRefT<T7>(rowIndex, c7),
            GetRefT<T8>(rowIndex, c8),
            GetRefT<T9>(rowIndex, c9),
            GetRefT<T10>(rowIndex, c10),
            GetRefT<T11>(rowIndex, c11),
            GetRefT<T12>(rowIndex, c12),
            GetRefT<T13>(rowIndex, c13)
        );
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> GetRefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(int rowIndex,
        ComponentID c0,
        ComponentID c1,
        ComponentID c2,
        ComponentID c3,
        ComponentID c4,
        ComponentID c5,
        ComponentID c6,
        ComponentID c7,
        ComponentID c8,
        ComponentID c9,
        ComponentID c10,
        ComponentID c11,
        ComponentID c12,
        ComponentID c13,
        ComponentID c14
    )
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
        Debug.Assert(rowIndex < EntityCount);
        Debug.Assert(rowIndex >= 0);

        // Get the component(s)
        return new RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            _entities[rowIndex],
            GetRefT<T0>(rowIndex, c0),
            GetRefT<T1>(rowIndex, c1),
            GetRefT<T2>(rowIndex, c2),
            GetRefT<T3>(rowIndex, c3),
            GetRefT<T4>(rowIndex, c4),
            GetRefT<T5>(rowIndex, c5),
            GetRefT<T6>(rowIndex, c6),
            GetRefT<T7>(rowIndex, c7),
            GetRefT<T8>(rowIndex, c8),
            GetRefT<T9>(rowIndex, c9),
            GetRefT<T10>(rowIndex, c10),
            GetRefT<T11>(rowIndex, c11),
            GetRefT<T12>(rowIndex, c12),
            GetRefT<T13>(rowIndex, c13),
            GetRefT<T14>(rowIndex, c14)
        );
    }

    /// <summary>
    /// Get a tuple of several components
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> GetRefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(int rowIndex,
        ComponentID c0,
        ComponentID c1,
        ComponentID c2,
        ComponentID c3,
        ComponentID c4,
        ComponentID c5,
        ComponentID c6,
        ComponentID c7,
        ComponentID c8,
        ComponentID c9,
        ComponentID c10,
        ComponentID c11,
        ComponentID c12,
        ComponentID c13,
        ComponentID c14,
        ComponentID c15
    )
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
        Debug.Assert(rowIndex < EntityCount);
        Debug.Assert(rowIndex >= 0);

        // Get the component(s)
        return new RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            _entities[rowIndex],
            GetRefT<T0>(rowIndex, c0),
            GetRefT<T1>(rowIndex, c1),
            GetRefT<T2>(rowIndex, c2),
            GetRefT<T3>(rowIndex, c3),
            GetRefT<T4>(rowIndex, c4),
            GetRefT<T5>(rowIndex, c5),
            GetRefT<T6>(rowIndex, c6),
            GetRefT<T7>(rowIndex, c7),
            GetRefT<T8>(rowIndex, c8),
            GetRefT<T9>(rowIndex, c9),
            GetRefT<T10>(rowIndex, c10),
            GetRefT<T11>(rowIndex, c11),
            GetRefT<T12>(rowIndex, c12),
            GetRefT<T13>(rowIndex, c13),
            GetRefT<T14>(rowIndex, c14),
            GetRefT<T15>(rowIndex, c15)
        );
    }

}

