using Myriad.ECS.Queries;
using Myriad.ECS.IDs;
using Myriad.ECS.Collections;
using Myriad.ECS.Worlds.Chunks;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedParameter.Global
// ReSharper disable LoopCanBeConvertedToQuery
// ReSharper disable CheckNamespace
// ReSharper disable ArrangeAccessorOwnerBody
#pragma warning disable CA1822 // Mark members as static

namespace Myriad.ECS.Queries
{
    public readonly struct QueryResultEnumerable0
        
    {
        private readonly QueryDescription _query;

        internal QueryResultEnumerable0(QueryDescription query)
        {
            _query = query;
        }

        public QueryResultEnumerator0 GetEnumerator()
        {
            return new QueryResultEnumerator0(
                _query.GetArchetypes()
            );
        }
    }

    public ref struct QueryResultEnumerator0
        
    {

        private List<QueryDescription.ArchetypeMatch>.Enumerator _archetypesEnumerator;
        private List<Chunk>.Enumerator _chunksEnumerator;
        private int _entityIndex = -1;
        private bool _initialized = false;

        private ReadOnlySpan<Entity> SpanEntities = default;

        internal QueryResultEnumerator0(FrozenOrderedListSet<QueryDescription.ArchetypeMatch> archetypes)
        {
            _archetypesEnumerator = archetypes.GetEnumerator();
        }

        public Entity Current
        {
            get
            {
                return SpanEntities[_entityIndex];
            
            }
        }

        private void GetChunkSpans()
        {
            var chunk = _chunksEnumerator.Current;

            SpanEntities = chunk.Entities;
        }

        private bool NextArchetype()
        {
            while (true)
            {
                // If there are no archetypes exit with false
                if (!_archetypesEnumerator.MoveNext())
                    return false;

                // Try to move to the next (first) chunk of this archetype. Might fail if there
                // are no chunks in this archetype.
                _chunksEnumerator = _archetypesEnumerator.Current.Archetype.GetChunkEnumerator();
                if (NextChunk())
                    break;
            }

            return true;
        }

        private bool NextChunk()
        {
            if (!_chunksEnumerator.MoveNext())
                return false;

            GetChunkSpans();
            _entityIndex = 0;
            return true;
        }

        public bool MoveNext()
        {
            _entityIndex++;
            if (_entityIndex < SpanEntities.Length)
                return true;

            if (!_initialized)
            {
                _initialized = true;

                if (!NextArchetype())
                    return false;
                return true;
            }

            if (!NextChunk())
                if (!NextArchetype())
                    return false;

            return true;
        }

        // ReSharper disable once UnusedMember.Global (Justification: used by enumerator)
        public void Dispose()
        {
            _archetypesEnumerator.Dispose();
            _chunksEnumerator.Dispose();
        }
    }
}

namespace Myriad.ECS.Worlds
{
    public partial class World
    {
        private readonly List<(ComponentID[], QueryDescription)> _queryCache0 = [ ];

        [System.Diagnostics.Contracts.Pure]
        [JetBrains.Annotations.Pure]
        public QueryResultEnumerable0 Query(QueryDescription query)
            
        {
            return new QueryResultEnumerable0(
                query
            );
        }

        [System.Diagnostics.Contracts.Pure]
        [JetBrains.Annotations.Pure]
        public QueryResultEnumerable0 Query()
            
        {
            // Find query that matches these types
            var query = default(QueryDescription);
            foreach (var item in _queryCache0)
            {

                query = item.Item2;
                break;
            }

            // Didn't find one, create it now and store it in the cache
            if (query == null)
            {
                query = new QueryBuilder()
                    .Build(this);

                _queryCache0.Add(([
                ], query));
            }

            return new QueryResultEnumerable0(
                query
            );
        }
    }
}

namespace Myriad.ECS.Queries
{
    public readonly struct QueryResultEnumerable1<T0>
        where T0 : IComponent
    {
        private readonly QueryDescription _query;

        internal QueryResultEnumerable1(QueryDescription query)
        {
            _query = query;
        }

        public QueryResultEnumerator1<T0> GetEnumerator()
        {
            return new QueryResultEnumerator1<T0>(
                _query.GetArchetypes()
            );
        }
    }

    public ref struct QueryResultEnumerator1<T0>
        where T0 : IComponent
    {
        private static readonly ComponentID C0 = ComponentID<T0>.ID;

        private List<QueryDescription.ArchetypeMatch>.Enumerator _archetypesEnumerator;
        private List<Chunk>.Enumerator _chunksEnumerator;
        private int _entityIndex = -1;
        private bool _initialized = false;

        private ReadOnlySpan<Entity> SpanEntities = default;
        private Span<T0> Span0 = default;

        internal QueryResultEnumerator1(FrozenOrderedListSet<QueryDescription.ArchetypeMatch> archetypes)
        {
            _archetypesEnumerator = archetypes.GetEnumerator();
        }

        public RefTuple1<T0> Current
        {
            get
            {
                return new RefTuple1<T0>(
                    SpanEntities[_entityIndex],
                    ref Span0[_entityIndex]
                );
            
            }
        }

        private void GetChunkSpans()
        {
            var chunk = _chunksEnumerator.Current;

            SpanEntities = chunk.Entities;
            Span0 = chunk.GetSpan<T0>(C0);
        }

        private bool NextArchetype()
        {
            while (true)
            {
                // If there are no archetypes exit with false
                if (!_archetypesEnumerator.MoveNext())
                    return false;

                // Try to move to the next (first) chunk of this archetype. Might fail if there
                // are no chunks in this archetype.
                _chunksEnumerator = _archetypesEnumerator.Current.Archetype.GetChunkEnumerator();
                if (NextChunk())
                    break;
            }

            return true;
        }

        private bool NextChunk()
        {
            if (!_chunksEnumerator.MoveNext())
                return false;

            GetChunkSpans();
            _entityIndex = 0;
            return true;
        }

        public bool MoveNext()
        {
            _entityIndex++;
            if (_entityIndex < SpanEntities.Length)
                return true;

            if (!_initialized)
            {
                _initialized = true;

                if (!NextArchetype())
                    return false;
                return true;
            }

            if (!NextChunk())
                if (!NextArchetype())
                    return false;

            return true;
        }

        // ReSharper disable once UnusedMember.Global (Justification: used by enumerator)
        public void Dispose()
        {
            _archetypesEnumerator.Dispose();
            _chunksEnumerator.Dispose();
        }
    }
}

namespace Myriad.ECS.Worlds
{
    public partial class World
    {
        private readonly List<(ComponentID[], QueryDescription)> _queryCache1 = [ ];

        [System.Diagnostics.Contracts.Pure]
        [JetBrains.Annotations.Pure]
        public QueryResultEnumerable1<T0> Query<T0>(QueryDescription query)
            where T0 : IComponent
        {
            return new QueryResultEnumerable1<T0>(
                query
            );
        }

        [System.Diagnostics.Contracts.Pure]
        [JetBrains.Annotations.Pure]
        public QueryResultEnumerable1<T0> Query<T0>()
            where T0 : IComponent
        {
            // Find query that matches these types
            var query = default(QueryDescription);
            foreach (var item in _queryCache1)
            {
                if (!item.Item1.Contains(ComponentID<T0>.ID))
                    continue;

                query = item.Item2;
                break;
            }

            // Didn't find one, create it now and store it in the cache
            if (query == null)
            {
                query = new QueryBuilder()
                    .Include<T0>()
                    .Build(this);

                _queryCache1.Add(([
                    ComponentID<T0>.ID,
                ], query));
            }

            return new QueryResultEnumerable1<T0>(
                query
            );
        }
    }
}

namespace Myriad.ECS.Queries
{
    public readonly struct QueryResultEnumerable2<T0, T1>
        where T0 : IComponent
        where T1 : IComponent
    {
        private readonly QueryDescription _query;

        internal QueryResultEnumerable2(QueryDescription query)
        {
            _query = query;
        }

        public QueryResultEnumerator2<T0, T1> GetEnumerator()
        {
            return new QueryResultEnumerator2<T0, T1>(
                _query.GetArchetypes()
            );
        }
    }

    public ref struct QueryResultEnumerator2<T0, T1>
        where T0 : IComponent
        where T1 : IComponent
    {
        private static readonly ComponentID C0 = ComponentID<T0>.ID;
        private static readonly ComponentID C1 = ComponentID<T1>.ID;

        private List<QueryDescription.ArchetypeMatch>.Enumerator _archetypesEnumerator;
        private List<Chunk>.Enumerator _chunksEnumerator;
        private int _entityIndex = -1;
        private bool _initialized = false;

        private ReadOnlySpan<Entity> SpanEntities = default;
        private Span<T0> Span0 = default;
        private Span<T1> Span1 = default;

        internal QueryResultEnumerator2(FrozenOrderedListSet<QueryDescription.ArchetypeMatch> archetypes)
        {
            _archetypesEnumerator = archetypes.GetEnumerator();
        }

        public RefTuple2<T0, T1> Current
        {
            get
            {
                return new RefTuple2<T0, T1>(
                    SpanEntities[_entityIndex],
                    ref Span0[_entityIndex],
                    ref Span1[_entityIndex]
                );
            
            }
        }

        private void GetChunkSpans()
        {
            var chunk = _chunksEnumerator.Current;

            SpanEntities = chunk.Entities;
            Span0 = chunk.GetSpan<T0>(C0);
            Span1 = chunk.GetSpan<T1>(C1);
        }

        private bool NextArchetype()
        {
            while (true)
            {
                // If there are no archetypes exit with false
                if (!_archetypesEnumerator.MoveNext())
                    return false;

                // Try to move to the next (first) chunk of this archetype. Might fail if there
                // are no chunks in this archetype.
                _chunksEnumerator = _archetypesEnumerator.Current.Archetype.GetChunkEnumerator();
                if (NextChunk())
                    break;
            }

            return true;
        }

        private bool NextChunk()
        {
            if (!_chunksEnumerator.MoveNext())
                return false;

            GetChunkSpans();
            _entityIndex = 0;
            return true;
        }

        public bool MoveNext()
        {
            _entityIndex++;
            if (_entityIndex < SpanEntities.Length)
                return true;

            if (!_initialized)
            {
                _initialized = true;

                if (!NextArchetype())
                    return false;
                return true;
            }

            if (!NextChunk())
                if (!NextArchetype())
                    return false;

            return true;
        }

        // ReSharper disable once UnusedMember.Global (Justification: used by enumerator)
        public void Dispose()
        {
            _archetypesEnumerator.Dispose();
            _chunksEnumerator.Dispose();
        }
    }
}

namespace Myriad.ECS.Worlds
{
    public partial class World
    {
        private readonly List<(ComponentID[], QueryDescription)> _queryCache2 = [ ];

        [System.Diagnostics.Contracts.Pure]
        [JetBrains.Annotations.Pure]
        public QueryResultEnumerable2<T0, T1> Query<T0, T1>(QueryDescription query)
            where T0 : IComponent
            where T1 : IComponent
        {
            return new QueryResultEnumerable2<T0, T1>(
                query
            );
        }

        [System.Diagnostics.Contracts.Pure]
        [JetBrains.Annotations.Pure]
        public QueryResultEnumerable2<T0, T1> Query<T0, T1>()
            where T0 : IComponent
            where T1 : IComponent
        {
            // Find query that matches these types
            var query = default(QueryDescription);
            foreach (var item in _queryCache2)
            {
                if (!item.Item1.Contains(ComponentID<T0>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T1>.ID))
                    continue;

                query = item.Item2;
                break;
            }

            // Didn't find one, create it now and store it in the cache
            if (query == null)
            {
                query = new QueryBuilder()
                    .Include<T0>()
                    .Include<T1>()
                    .Build(this);

                _queryCache2.Add(([
                    ComponentID<T0>.ID,
                    ComponentID<T1>.ID,
                ], query));
            }

            return new QueryResultEnumerable2<T0, T1>(
                query
            );
        }
    }
}

namespace Myriad.ECS.Queries
{
    public readonly struct QueryResultEnumerable3<T0, T1, T2>
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
    {
        private readonly QueryDescription _query;

        internal QueryResultEnumerable3(QueryDescription query)
        {
            _query = query;
        }

        public QueryResultEnumerator3<T0, T1, T2> GetEnumerator()
        {
            return new QueryResultEnumerator3<T0, T1, T2>(
                _query.GetArchetypes()
            );
        }
    }

    public ref struct QueryResultEnumerator3<T0, T1, T2>
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
    {
        private static readonly ComponentID C0 = ComponentID<T0>.ID;
        private static readonly ComponentID C1 = ComponentID<T1>.ID;
        private static readonly ComponentID C2 = ComponentID<T2>.ID;

        private List<QueryDescription.ArchetypeMatch>.Enumerator _archetypesEnumerator;
        private List<Chunk>.Enumerator _chunksEnumerator;
        private int _entityIndex = -1;
        private bool _initialized = false;

        private ReadOnlySpan<Entity> SpanEntities = default;
        private Span<T0> Span0 = default;
        private Span<T1> Span1 = default;
        private Span<T2> Span2 = default;

        internal QueryResultEnumerator3(FrozenOrderedListSet<QueryDescription.ArchetypeMatch> archetypes)
        {
            _archetypesEnumerator = archetypes.GetEnumerator();
        }

        public RefTuple3<T0, T1, T2> Current
        {
            get
            {
                return new RefTuple3<T0, T1, T2>(
                    SpanEntities[_entityIndex],
                    ref Span0[_entityIndex],
                    ref Span1[_entityIndex],
                    ref Span2[_entityIndex]
                );
            
            }
        }

        private void GetChunkSpans()
        {
            var chunk = _chunksEnumerator.Current;

            SpanEntities = chunk.Entities;
            Span0 = chunk.GetSpan<T0>(C0);
            Span1 = chunk.GetSpan<T1>(C1);
            Span2 = chunk.GetSpan<T2>(C2);
        }

        private bool NextArchetype()
        {
            while (true)
            {
                // If there are no archetypes exit with false
                if (!_archetypesEnumerator.MoveNext())
                    return false;

                // Try to move to the next (first) chunk of this archetype. Might fail if there
                // are no chunks in this archetype.
                _chunksEnumerator = _archetypesEnumerator.Current.Archetype.GetChunkEnumerator();
                if (NextChunk())
                    break;
            }

            return true;
        }

        private bool NextChunk()
        {
            if (!_chunksEnumerator.MoveNext())
                return false;

            GetChunkSpans();
            _entityIndex = 0;
            return true;
        }

        public bool MoveNext()
        {
            _entityIndex++;
            if (_entityIndex < SpanEntities.Length)
                return true;

            if (!_initialized)
            {
                _initialized = true;

                if (!NextArchetype())
                    return false;
                return true;
            }

            if (!NextChunk())
                if (!NextArchetype())
                    return false;

            return true;
        }

        // ReSharper disable once UnusedMember.Global (Justification: used by enumerator)
        public void Dispose()
        {
            _archetypesEnumerator.Dispose();
            _chunksEnumerator.Dispose();
        }
    }
}

namespace Myriad.ECS.Worlds
{
    public partial class World
    {
        private readonly List<(ComponentID[], QueryDescription)> _queryCache3 = [ ];

        [System.Diagnostics.Contracts.Pure]
        [JetBrains.Annotations.Pure]
        public QueryResultEnumerable3<T0, T1, T2> Query<T0, T1, T2>(QueryDescription query)
            where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
        {
            return new QueryResultEnumerable3<T0, T1, T2>(
                query
            );
        }

        [System.Diagnostics.Contracts.Pure]
        [JetBrains.Annotations.Pure]
        public QueryResultEnumerable3<T0, T1, T2> Query<T0, T1, T2>()
            where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
        {
            // Find query that matches these types
            var query = default(QueryDescription);
            foreach (var item in _queryCache3)
            {
                if (!item.Item1.Contains(ComponentID<T0>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T1>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T2>.ID))
                    continue;

                query = item.Item2;
                break;
            }

            // Didn't find one, create it now and store it in the cache
            if (query == null)
            {
                query = new QueryBuilder()
                    .Include<T0>()
                    .Include<T1>()
                    .Include<T2>()
                    .Build(this);

                _queryCache3.Add(([
                    ComponentID<T0>.ID,
                    ComponentID<T1>.ID,
                    ComponentID<T2>.ID,
                ], query));
            }

            return new QueryResultEnumerable3<T0, T1, T2>(
                query
            );
        }
    }
}

namespace Myriad.ECS.Queries
{
    public readonly struct QueryResultEnumerable4<T0, T1, T2, T3>
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
    {
        private readonly QueryDescription _query;

        internal QueryResultEnumerable4(QueryDescription query)
        {
            _query = query;
        }

        public QueryResultEnumerator4<T0, T1, T2, T3> GetEnumerator()
        {
            return new QueryResultEnumerator4<T0, T1, T2, T3>(
                _query.GetArchetypes()
            );
        }
    }

    public ref struct QueryResultEnumerator4<T0, T1, T2, T3>
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
    {
        private static readonly ComponentID C0 = ComponentID<T0>.ID;
        private static readonly ComponentID C1 = ComponentID<T1>.ID;
        private static readonly ComponentID C2 = ComponentID<T2>.ID;
        private static readonly ComponentID C3 = ComponentID<T3>.ID;

        private List<QueryDescription.ArchetypeMatch>.Enumerator _archetypesEnumerator;
        private List<Chunk>.Enumerator _chunksEnumerator;
        private int _entityIndex = -1;
        private bool _initialized = false;

        private ReadOnlySpan<Entity> SpanEntities = default;
        private Span<T0> Span0 = default;
        private Span<T1> Span1 = default;
        private Span<T2> Span2 = default;
        private Span<T3> Span3 = default;

        internal QueryResultEnumerator4(FrozenOrderedListSet<QueryDescription.ArchetypeMatch> archetypes)
        {
            _archetypesEnumerator = archetypes.GetEnumerator();
        }

        public RefTuple4<T0, T1, T2, T3> Current
        {
            get
            {
                return new RefTuple4<T0, T1, T2, T3>(
                    SpanEntities[_entityIndex],
                    ref Span0[_entityIndex],
                    ref Span1[_entityIndex],
                    ref Span2[_entityIndex],
                    ref Span3[_entityIndex]
                );
            
            }
        }

        private void GetChunkSpans()
        {
            var chunk = _chunksEnumerator.Current;

            SpanEntities = chunk.Entities;
            Span0 = chunk.GetSpan<T0>(C0);
            Span1 = chunk.GetSpan<T1>(C1);
            Span2 = chunk.GetSpan<T2>(C2);
            Span3 = chunk.GetSpan<T3>(C3);
        }

        private bool NextArchetype()
        {
            while (true)
            {
                // If there are no archetypes exit with false
                if (!_archetypesEnumerator.MoveNext())
                    return false;

                // Try to move to the next (first) chunk of this archetype. Might fail if there
                // are no chunks in this archetype.
                _chunksEnumerator = _archetypesEnumerator.Current.Archetype.GetChunkEnumerator();
                if (NextChunk())
                    break;
            }

            return true;
        }

        private bool NextChunk()
        {
            if (!_chunksEnumerator.MoveNext())
                return false;

            GetChunkSpans();
            _entityIndex = 0;
            return true;
        }

        public bool MoveNext()
        {
            _entityIndex++;
            if (_entityIndex < SpanEntities.Length)
                return true;

            if (!_initialized)
            {
                _initialized = true;

                if (!NextArchetype())
                    return false;
                return true;
            }

            if (!NextChunk())
                if (!NextArchetype())
                    return false;

            return true;
        }

        // ReSharper disable once UnusedMember.Global (Justification: used by enumerator)
        public void Dispose()
        {
            _archetypesEnumerator.Dispose();
            _chunksEnumerator.Dispose();
        }
    }
}

namespace Myriad.ECS.Worlds
{
    public partial class World
    {
        private readonly List<(ComponentID[], QueryDescription)> _queryCache4 = [ ];

        [System.Diagnostics.Contracts.Pure]
        [JetBrains.Annotations.Pure]
        public QueryResultEnumerable4<T0, T1, T2, T3> Query<T0, T1, T2, T3>(QueryDescription query)
            where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
        {
            return new QueryResultEnumerable4<T0, T1, T2, T3>(
                query
            );
        }

        [System.Diagnostics.Contracts.Pure]
        [JetBrains.Annotations.Pure]
        public QueryResultEnumerable4<T0, T1, T2, T3> Query<T0, T1, T2, T3>()
            where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
        {
            // Find query that matches these types
            var query = default(QueryDescription);
            foreach (var item in _queryCache4)
            {
                if (!item.Item1.Contains(ComponentID<T0>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T1>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T2>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T3>.ID))
                    continue;

                query = item.Item2;
                break;
            }

            // Didn't find one, create it now and store it in the cache
            if (query == null)
            {
                query = new QueryBuilder()
                    .Include<T0>()
                    .Include<T1>()
                    .Include<T2>()
                    .Include<T3>()
                    .Build(this);

                _queryCache4.Add(([
                    ComponentID<T0>.ID,
                    ComponentID<T1>.ID,
                    ComponentID<T2>.ID,
                    ComponentID<T3>.ID,
                ], query));
            }

            return new QueryResultEnumerable4<T0, T1, T2, T3>(
                query
            );
        }
    }
}

namespace Myriad.ECS.Queries
{
    public readonly struct QueryResultEnumerable5<T0, T1, T2, T3, T4>
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
    {
        private readonly QueryDescription _query;

        internal QueryResultEnumerable5(QueryDescription query)
        {
            _query = query;
        }

        public QueryResultEnumerator5<T0, T1, T2, T3, T4> GetEnumerator()
        {
            return new QueryResultEnumerator5<T0, T1, T2, T3, T4>(
                _query.GetArchetypes()
            );
        }
    }

    public ref struct QueryResultEnumerator5<T0, T1, T2, T3, T4>
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
    {
        private static readonly ComponentID C0 = ComponentID<T0>.ID;
        private static readonly ComponentID C1 = ComponentID<T1>.ID;
        private static readonly ComponentID C2 = ComponentID<T2>.ID;
        private static readonly ComponentID C3 = ComponentID<T3>.ID;
        private static readonly ComponentID C4 = ComponentID<T4>.ID;

        private List<QueryDescription.ArchetypeMatch>.Enumerator _archetypesEnumerator;
        private List<Chunk>.Enumerator _chunksEnumerator;
        private int _entityIndex = -1;
        private bool _initialized = false;

        private ReadOnlySpan<Entity> SpanEntities = default;
        private Span<T0> Span0 = default;
        private Span<T1> Span1 = default;
        private Span<T2> Span2 = default;
        private Span<T3> Span3 = default;
        private Span<T4> Span4 = default;

        internal QueryResultEnumerator5(FrozenOrderedListSet<QueryDescription.ArchetypeMatch> archetypes)
        {
            _archetypesEnumerator = archetypes.GetEnumerator();
        }

        public RefTuple5<T0, T1, T2, T3, T4> Current
        {
            get
            {
                return new RefTuple5<T0, T1, T2, T3, T4>(
                    SpanEntities[_entityIndex],
                    ref Span0[_entityIndex],
                    ref Span1[_entityIndex],
                    ref Span2[_entityIndex],
                    ref Span3[_entityIndex],
                    ref Span4[_entityIndex]
                );
            
            }
        }

        private void GetChunkSpans()
        {
            var chunk = _chunksEnumerator.Current;

            SpanEntities = chunk.Entities;
            Span0 = chunk.GetSpan<T0>(C0);
            Span1 = chunk.GetSpan<T1>(C1);
            Span2 = chunk.GetSpan<T2>(C2);
            Span3 = chunk.GetSpan<T3>(C3);
            Span4 = chunk.GetSpan<T4>(C4);
        }

        private bool NextArchetype()
        {
            while (true)
            {
                // If there are no archetypes exit with false
                if (!_archetypesEnumerator.MoveNext())
                    return false;

                // Try to move to the next (first) chunk of this archetype. Might fail if there
                // are no chunks in this archetype.
                _chunksEnumerator = _archetypesEnumerator.Current.Archetype.GetChunkEnumerator();
                if (NextChunk())
                    break;
            }

            return true;
        }

        private bool NextChunk()
        {
            if (!_chunksEnumerator.MoveNext())
                return false;

            GetChunkSpans();
            _entityIndex = 0;
            return true;
        }

        public bool MoveNext()
        {
            _entityIndex++;
            if (_entityIndex < SpanEntities.Length)
                return true;

            if (!_initialized)
            {
                _initialized = true;

                if (!NextArchetype())
                    return false;
                return true;
            }

            if (!NextChunk())
                if (!NextArchetype())
                    return false;

            return true;
        }

        // ReSharper disable once UnusedMember.Global (Justification: used by enumerator)
        public void Dispose()
        {
            _archetypesEnumerator.Dispose();
            _chunksEnumerator.Dispose();
        }
    }
}

namespace Myriad.ECS.Worlds
{
    public partial class World
    {
        private readonly List<(ComponentID[], QueryDescription)> _queryCache5 = [ ];

        [System.Diagnostics.Contracts.Pure]
        [JetBrains.Annotations.Pure]
        public QueryResultEnumerable5<T0, T1, T2, T3, T4> Query<T0, T1, T2, T3, T4>(QueryDescription query)
            where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
        {
            return new QueryResultEnumerable5<T0, T1, T2, T3, T4>(
                query
            );
        }

        [System.Diagnostics.Contracts.Pure]
        [JetBrains.Annotations.Pure]
        public QueryResultEnumerable5<T0, T1, T2, T3, T4> Query<T0, T1, T2, T3, T4>()
            where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
        {
            // Find query that matches these types
            var query = default(QueryDescription);
            foreach (var item in _queryCache5)
            {
                if (!item.Item1.Contains(ComponentID<T0>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T1>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T2>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T3>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T4>.ID))
                    continue;

                query = item.Item2;
                break;
            }

            // Didn't find one, create it now and store it in the cache
            if (query == null)
            {
                query = new QueryBuilder()
                    .Include<T0>()
                    .Include<T1>()
                    .Include<T2>()
                    .Include<T3>()
                    .Include<T4>()
                    .Build(this);

                _queryCache5.Add(([
                    ComponentID<T0>.ID,
                    ComponentID<T1>.ID,
                    ComponentID<T2>.ID,
                    ComponentID<T3>.ID,
                    ComponentID<T4>.ID,
                ], query));
            }

            return new QueryResultEnumerable5<T0, T1, T2, T3, T4>(
                query
            );
        }
    }
}

namespace Myriad.ECS.Queries
{
    public readonly struct QueryResultEnumerable6<T0, T1, T2, T3, T4, T5>
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
    {
        private readonly QueryDescription _query;

        internal QueryResultEnumerable6(QueryDescription query)
        {
            _query = query;
        }

        public QueryResultEnumerator6<T0, T1, T2, T3, T4, T5> GetEnumerator()
        {
            return new QueryResultEnumerator6<T0, T1, T2, T3, T4, T5>(
                _query.GetArchetypes()
            );
        }
    }

    public ref struct QueryResultEnumerator6<T0, T1, T2, T3, T4, T5>
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
    {
        private static readonly ComponentID C0 = ComponentID<T0>.ID;
        private static readonly ComponentID C1 = ComponentID<T1>.ID;
        private static readonly ComponentID C2 = ComponentID<T2>.ID;
        private static readonly ComponentID C3 = ComponentID<T3>.ID;
        private static readonly ComponentID C4 = ComponentID<T4>.ID;
        private static readonly ComponentID C5 = ComponentID<T5>.ID;

        private List<QueryDescription.ArchetypeMatch>.Enumerator _archetypesEnumerator;
        private List<Chunk>.Enumerator _chunksEnumerator;
        private int _entityIndex = -1;
        private bool _initialized = false;

        private ReadOnlySpan<Entity> SpanEntities = default;
        private Span<T0> Span0 = default;
        private Span<T1> Span1 = default;
        private Span<T2> Span2 = default;
        private Span<T3> Span3 = default;
        private Span<T4> Span4 = default;
        private Span<T5> Span5 = default;

        internal QueryResultEnumerator6(FrozenOrderedListSet<QueryDescription.ArchetypeMatch> archetypes)
        {
            _archetypesEnumerator = archetypes.GetEnumerator();
        }

        public RefTuple6<T0, T1, T2, T3, T4, T5> Current
        {
            get
            {
                return new RefTuple6<T0, T1, T2, T3, T4, T5>(
                    SpanEntities[_entityIndex],
                    ref Span0[_entityIndex],
                    ref Span1[_entityIndex],
                    ref Span2[_entityIndex],
                    ref Span3[_entityIndex],
                    ref Span4[_entityIndex],
                    ref Span5[_entityIndex]
                );
            
            }
        }

        private void GetChunkSpans()
        {
            var chunk = _chunksEnumerator.Current;

            SpanEntities = chunk.Entities;
            Span0 = chunk.GetSpan<T0>(C0);
            Span1 = chunk.GetSpan<T1>(C1);
            Span2 = chunk.GetSpan<T2>(C2);
            Span3 = chunk.GetSpan<T3>(C3);
            Span4 = chunk.GetSpan<T4>(C4);
            Span5 = chunk.GetSpan<T5>(C5);
        }

        private bool NextArchetype()
        {
            while (true)
            {
                // If there are no archetypes exit with false
                if (!_archetypesEnumerator.MoveNext())
                    return false;

                // Try to move to the next (first) chunk of this archetype. Might fail if there
                // are no chunks in this archetype.
                _chunksEnumerator = _archetypesEnumerator.Current.Archetype.GetChunkEnumerator();
                if (NextChunk())
                    break;
            }

            return true;
        }

        private bool NextChunk()
        {
            if (!_chunksEnumerator.MoveNext())
                return false;

            GetChunkSpans();
            _entityIndex = 0;
            return true;
        }

        public bool MoveNext()
        {
            _entityIndex++;
            if (_entityIndex < SpanEntities.Length)
                return true;

            if (!_initialized)
            {
                _initialized = true;

                if (!NextArchetype())
                    return false;
                return true;
            }

            if (!NextChunk())
                if (!NextArchetype())
                    return false;

            return true;
        }

        // ReSharper disable once UnusedMember.Global (Justification: used by enumerator)
        public void Dispose()
        {
            _archetypesEnumerator.Dispose();
            _chunksEnumerator.Dispose();
        }
    }
}

namespace Myriad.ECS.Worlds
{
    public partial class World
    {
        private readonly List<(ComponentID[], QueryDescription)> _queryCache6 = [ ];

        [System.Diagnostics.Contracts.Pure]
        [JetBrains.Annotations.Pure]
        public QueryResultEnumerable6<T0, T1, T2, T3, T4, T5> Query<T0, T1, T2, T3, T4, T5>(QueryDescription query)
            where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
        {
            return new QueryResultEnumerable6<T0, T1, T2, T3, T4, T5>(
                query
            );
        }

        [System.Diagnostics.Contracts.Pure]
        [JetBrains.Annotations.Pure]
        public QueryResultEnumerable6<T0, T1, T2, T3, T4, T5> Query<T0, T1, T2, T3, T4, T5>()
            where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
        {
            // Find query that matches these types
            var query = default(QueryDescription);
            foreach (var item in _queryCache6)
            {
                if (!item.Item1.Contains(ComponentID<T0>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T1>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T2>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T3>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T4>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T5>.ID))
                    continue;

                query = item.Item2;
                break;
            }

            // Didn't find one, create it now and store it in the cache
            if (query == null)
            {
                query = new QueryBuilder()
                    .Include<T0>()
                    .Include<T1>()
                    .Include<T2>()
                    .Include<T3>()
                    .Include<T4>()
                    .Include<T5>()
                    .Build(this);

                _queryCache6.Add(([
                    ComponentID<T0>.ID,
                    ComponentID<T1>.ID,
                    ComponentID<T2>.ID,
                    ComponentID<T3>.ID,
                    ComponentID<T4>.ID,
                    ComponentID<T5>.ID,
                ], query));
            }

            return new QueryResultEnumerable6<T0, T1, T2, T3, T4, T5>(
                query
            );
        }
    }
}

namespace Myriad.ECS.Queries
{
    public readonly struct QueryResultEnumerable7<T0, T1, T2, T3, T4, T5, T6>
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
    {
        private readonly QueryDescription _query;

        internal QueryResultEnumerable7(QueryDescription query)
        {
            _query = query;
        }

        public QueryResultEnumerator7<T0, T1, T2, T3, T4, T5, T6> GetEnumerator()
        {
            return new QueryResultEnumerator7<T0, T1, T2, T3, T4, T5, T6>(
                _query.GetArchetypes()
            );
        }
    }

    public ref struct QueryResultEnumerator7<T0, T1, T2, T3, T4, T5, T6>
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
    {
        private static readonly ComponentID C0 = ComponentID<T0>.ID;
        private static readonly ComponentID C1 = ComponentID<T1>.ID;
        private static readonly ComponentID C2 = ComponentID<T2>.ID;
        private static readonly ComponentID C3 = ComponentID<T3>.ID;
        private static readonly ComponentID C4 = ComponentID<T4>.ID;
        private static readonly ComponentID C5 = ComponentID<T5>.ID;
        private static readonly ComponentID C6 = ComponentID<T6>.ID;

        private List<QueryDescription.ArchetypeMatch>.Enumerator _archetypesEnumerator;
        private List<Chunk>.Enumerator _chunksEnumerator;
        private int _entityIndex = -1;
        private bool _initialized = false;

        private ReadOnlySpan<Entity> SpanEntities = default;
        private Span<T0> Span0 = default;
        private Span<T1> Span1 = default;
        private Span<T2> Span2 = default;
        private Span<T3> Span3 = default;
        private Span<T4> Span4 = default;
        private Span<T5> Span5 = default;
        private Span<T6> Span6 = default;

        internal QueryResultEnumerator7(FrozenOrderedListSet<QueryDescription.ArchetypeMatch> archetypes)
        {
            _archetypesEnumerator = archetypes.GetEnumerator();
        }

        public RefTuple7<T0, T1, T2, T3, T4, T5, T6> Current
        {
            get
            {
                return new RefTuple7<T0, T1, T2, T3, T4, T5, T6>(
                    SpanEntities[_entityIndex],
                    ref Span0[_entityIndex],
                    ref Span1[_entityIndex],
                    ref Span2[_entityIndex],
                    ref Span3[_entityIndex],
                    ref Span4[_entityIndex],
                    ref Span5[_entityIndex],
                    ref Span6[_entityIndex]
                );
            
            }
        }

        private void GetChunkSpans()
        {
            var chunk = _chunksEnumerator.Current;

            SpanEntities = chunk.Entities;
            Span0 = chunk.GetSpan<T0>(C0);
            Span1 = chunk.GetSpan<T1>(C1);
            Span2 = chunk.GetSpan<T2>(C2);
            Span3 = chunk.GetSpan<T3>(C3);
            Span4 = chunk.GetSpan<T4>(C4);
            Span5 = chunk.GetSpan<T5>(C5);
            Span6 = chunk.GetSpan<T6>(C6);
        }

        private bool NextArchetype()
        {
            while (true)
            {
                // If there are no archetypes exit with false
                if (!_archetypesEnumerator.MoveNext())
                    return false;

                // Try to move to the next (first) chunk of this archetype. Might fail if there
                // are no chunks in this archetype.
                _chunksEnumerator = _archetypesEnumerator.Current.Archetype.GetChunkEnumerator();
                if (NextChunk())
                    break;
            }

            return true;
        }

        private bool NextChunk()
        {
            if (!_chunksEnumerator.MoveNext())
                return false;

            GetChunkSpans();
            _entityIndex = 0;
            return true;
        }

        public bool MoveNext()
        {
            _entityIndex++;
            if (_entityIndex < SpanEntities.Length)
                return true;

            if (!_initialized)
            {
                _initialized = true;

                if (!NextArchetype())
                    return false;
                return true;
            }

            if (!NextChunk())
                if (!NextArchetype())
                    return false;

            return true;
        }

        // ReSharper disable once UnusedMember.Global (Justification: used by enumerator)
        public void Dispose()
        {
            _archetypesEnumerator.Dispose();
            _chunksEnumerator.Dispose();
        }
    }
}

namespace Myriad.ECS.Worlds
{
    public partial class World
    {
        private readonly List<(ComponentID[], QueryDescription)> _queryCache7 = [ ];

        [System.Diagnostics.Contracts.Pure]
        [JetBrains.Annotations.Pure]
        public QueryResultEnumerable7<T0, T1, T2, T3, T4, T5, T6> Query<T0, T1, T2, T3, T4, T5, T6>(QueryDescription query)
            where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
        {
            return new QueryResultEnumerable7<T0, T1, T2, T3, T4, T5, T6>(
                query
            );
        }

        [System.Diagnostics.Contracts.Pure]
        [JetBrains.Annotations.Pure]
        public QueryResultEnumerable7<T0, T1, T2, T3, T4, T5, T6> Query<T0, T1, T2, T3, T4, T5, T6>()
            where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
        {
            // Find query that matches these types
            var query = default(QueryDescription);
            foreach (var item in _queryCache7)
            {
                if (!item.Item1.Contains(ComponentID<T0>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T1>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T2>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T3>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T4>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T5>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T6>.ID))
                    continue;

                query = item.Item2;
                break;
            }

            // Didn't find one, create it now and store it in the cache
            if (query == null)
            {
                query = new QueryBuilder()
                    .Include<T0>()
                    .Include<T1>()
                    .Include<T2>()
                    .Include<T3>()
                    .Include<T4>()
                    .Include<T5>()
                    .Include<T6>()
                    .Build(this);

                _queryCache7.Add(([
                    ComponentID<T0>.ID,
                    ComponentID<T1>.ID,
                    ComponentID<T2>.ID,
                    ComponentID<T3>.ID,
                    ComponentID<T4>.ID,
                    ComponentID<T5>.ID,
                    ComponentID<T6>.ID,
                ], query));
            }

            return new QueryResultEnumerable7<T0, T1, T2, T3, T4, T5, T6>(
                query
            );
        }
    }
}

namespace Myriad.ECS.Queries
{
    public readonly struct QueryResultEnumerable8<T0, T1, T2, T3, T4, T5, T6, T7>
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
    {
        private readonly QueryDescription _query;

        internal QueryResultEnumerable8(QueryDescription query)
        {
            _query = query;
        }

        public QueryResultEnumerator8<T0, T1, T2, T3, T4, T5, T6, T7> GetEnumerator()
        {
            return new QueryResultEnumerator8<T0, T1, T2, T3, T4, T5, T6, T7>(
                _query.GetArchetypes()
            );
        }
    }

    public ref struct QueryResultEnumerator8<T0, T1, T2, T3, T4, T5, T6, T7>
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
    {
        private static readonly ComponentID C0 = ComponentID<T0>.ID;
        private static readonly ComponentID C1 = ComponentID<T1>.ID;
        private static readonly ComponentID C2 = ComponentID<T2>.ID;
        private static readonly ComponentID C3 = ComponentID<T3>.ID;
        private static readonly ComponentID C4 = ComponentID<T4>.ID;
        private static readonly ComponentID C5 = ComponentID<T5>.ID;
        private static readonly ComponentID C6 = ComponentID<T6>.ID;
        private static readonly ComponentID C7 = ComponentID<T7>.ID;

        private List<QueryDescription.ArchetypeMatch>.Enumerator _archetypesEnumerator;
        private List<Chunk>.Enumerator _chunksEnumerator;
        private int _entityIndex = -1;
        private bool _initialized = false;

        private ReadOnlySpan<Entity> SpanEntities = default;
        private Span<T0> Span0 = default;
        private Span<T1> Span1 = default;
        private Span<T2> Span2 = default;
        private Span<T3> Span3 = default;
        private Span<T4> Span4 = default;
        private Span<T5> Span5 = default;
        private Span<T6> Span6 = default;
        private Span<T7> Span7 = default;

        internal QueryResultEnumerator8(FrozenOrderedListSet<QueryDescription.ArchetypeMatch> archetypes)
        {
            _archetypesEnumerator = archetypes.GetEnumerator();
        }

        public RefTuple8<T0, T1, T2, T3, T4, T5, T6, T7> Current
        {
            get
            {
                return new RefTuple8<T0, T1, T2, T3, T4, T5, T6, T7>(
                    SpanEntities[_entityIndex],
                    ref Span0[_entityIndex],
                    ref Span1[_entityIndex],
                    ref Span2[_entityIndex],
                    ref Span3[_entityIndex],
                    ref Span4[_entityIndex],
                    ref Span5[_entityIndex],
                    ref Span6[_entityIndex],
                    ref Span7[_entityIndex]
                );
            
            }
        }

        private void GetChunkSpans()
        {
            var chunk = _chunksEnumerator.Current;

            SpanEntities = chunk.Entities;
            Span0 = chunk.GetSpan<T0>(C0);
            Span1 = chunk.GetSpan<T1>(C1);
            Span2 = chunk.GetSpan<T2>(C2);
            Span3 = chunk.GetSpan<T3>(C3);
            Span4 = chunk.GetSpan<T4>(C4);
            Span5 = chunk.GetSpan<T5>(C5);
            Span6 = chunk.GetSpan<T6>(C6);
            Span7 = chunk.GetSpan<T7>(C7);
        }

        private bool NextArchetype()
        {
            while (true)
            {
                // If there are no archetypes exit with false
                if (!_archetypesEnumerator.MoveNext())
                    return false;

                // Try to move to the next (first) chunk of this archetype. Might fail if there
                // are no chunks in this archetype.
                _chunksEnumerator = _archetypesEnumerator.Current.Archetype.GetChunkEnumerator();
                if (NextChunk())
                    break;
            }

            return true;
        }

        private bool NextChunk()
        {
            if (!_chunksEnumerator.MoveNext())
                return false;

            GetChunkSpans();
            _entityIndex = 0;
            return true;
        }

        public bool MoveNext()
        {
            _entityIndex++;
            if (_entityIndex < SpanEntities.Length)
                return true;

            if (!_initialized)
            {
                _initialized = true;

                if (!NextArchetype())
                    return false;
                return true;
            }

            if (!NextChunk())
                if (!NextArchetype())
                    return false;

            return true;
        }

        // ReSharper disable once UnusedMember.Global (Justification: used by enumerator)
        public void Dispose()
        {
            _archetypesEnumerator.Dispose();
            _chunksEnumerator.Dispose();
        }
    }
}

namespace Myriad.ECS.Worlds
{
    public partial class World
    {
        private readonly List<(ComponentID[], QueryDescription)> _queryCache8 = [ ];

        [System.Diagnostics.Contracts.Pure]
        [JetBrains.Annotations.Pure]
        public QueryResultEnumerable8<T0, T1, T2, T3, T4, T5, T6, T7> Query<T0, T1, T2, T3, T4, T5, T6, T7>(QueryDescription query)
            where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
        {
            return new QueryResultEnumerable8<T0, T1, T2, T3, T4, T5, T6, T7>(
                query
            );
        }

        [System.Diagnostics.Contracts.Pure]
        [JetBrains.Annotations.Pure]
        public QueryResultEnumerable8<T0, T1, T2, T3, T4, T5, T6, T7> Query<T0, T1, T2, T3, T4, T5, T6, T7>()
            where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
        {
            // Find query that matches these types
            var query = default(QueryDescription);
            foreach (var item in _queryCache8)
            {
                if (!item.Item1.Contains(ComponentID<T0>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T1>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T2>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T3>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T4>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T5>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T6>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T7>.ID))
                    continue;

                query = item.Item2;
                break;
            }

            // Didn't find one, create it now and store it in the cache
            if (query == null)
            {
                query = new QueryBuilder()
                    .Include<T0>()
                    .Include<T1>()
                    .Include<T2>()
                    .Include<T3>()
                    .Include<T4>()
                    .Include<T5>()
                    .Include<T6>()
                    .Include<T7>()
                    .Build(this);

                _queryCache8.Add(([
                    ComponentID<T0>.ID,
                    ComponentID<T1>.ID,
                    ComponentID<T2>.ID,
                    ComponentID<T3>.ID,
                    ComponentID<T4>.ID,
                    ComponentID<T5>.ID,
                    ComponentID<T6>.ID,
                    ComponentID<T7>.ID,
                ], query));
            }

            return new QueryResultEnumerable8<T0, T1, T2, T3, T4, T5, T6, T7>(
                query
            );
        }
    }
}

namespace Myriad.ECS.Queries
{
    public readonly struct QueryResultEnumerable9<T0, T1, T2, T3, T4, T5, T6, T7, T8>
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
        private readonly QueryDescription _query;

        internal QueryResultEnumerable9(QueryDescription query)
        {
            _query = query;
        }

        public QueryResultEnumerator9<T0, T1, T2, T3, T4, T5, T6, T7, T8> GetEnumerator()
        {
            return new QueryResultEnumerator9<T0, T1, T2, T3, T4, T5, T6, T7, T8>(
                _query.GetArchetypes()
            );
        }
    }

    public ref struct QueryResultEnumerator9<T0, T1, T2, T3, T4, T5, T6, T7, T8>
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
        private static readonly ComponentID C0 = ComponentID<T0>.ID;
        private static readonly ComponentID C1 = ComponentID<T1>.ID;
        private static readonly ComponentID C2 = ComponentID<T2>.ID;
        private static readonly ComponentID C3 = ComponentID<T3>.ID;
        private static readonly ComponentID C4 = ComponentID<T4>.ID;
        private static readonly ComponentID C5 = ComponentID<T5>.ID;
        private static readonly ComponentID C6 = ComponentID<T6>.ID;
        private static readonly ComponentID C7 = ComponentID<T7>.ID;
        private static readonly ComponentID C8 = ComponentID<T8>.ID;

        private List<QueryDescription.ArchetypeMatch>.Enumerator _archetypesEnumerator;
        private List<Chunk>.Enumerator _chunksEnumerator;
        private int _entityIndex = -1;
        private bool _initialized = false;

        private ReadOnlySpan<Entity> SpanEntities = default;
        private Span<T0> Span0 = default;
        private Span<T1> Span1 = default;
        private Span<T2> Span2 = default;
        private Span<T3> Span3 = default;
        private Span<T4> Span4 = default;
        private Span<T5> Span5 = default;
        private Span<T6> Span6 = default;
        private Span<T7> Span7 = default;
        private Span<T8> Span8 = default;

        internal QueryResultEnumerator9(FrozenOrderedListSet<QueryDescription.ArchetypeMatch> archetypes)
        {
            _archetypesEnumerator = archetypes.GetEnumerator();
        }

        public RefTuple9<T0, T1, T2, T3, T4, T5, T6, T7, T8> Current
        {
            get
            {
                return new RefTuple9<T0, T1, T2, T3, T4, T5, T6, T7, T8>(
                    SpanEntities[_entityIndex],
                    ref Span0[_entityIndex],
                    ref Span1[_entityIndex],
                    ref Span2[_entityIndex],
                    ref Span3[_entityIndex],
                    ref Span4[_entityIndex],
                    ref Span5[_entityIndex],
                    ref Span6[_entityIndex],
                    ref Span7[_entityIndex],
                    ref Span8[_entityIndex]
                );
            
            }
        }

        private void GetChunkSpans()
        {
            var chunk = _chunksEnumerator.Current;

            SpanEntities = chunk.Entities;
            Span0 = chunk.GetSpan<T0>(C0);
            Span1 = chunk.GetSpan<T1>(C1);
            Span2 = chunk.GetSpan<T2>(C2);
            Span3 = chunk.GetSpan<T3>(C3);
            Span4 = chunk.GetSpan<T4>(C4);
            Span5 = chunk.GetSpan<T5>(C5);
            Span6 = chunk.GetSpan<T6>(C6);
            Span7 = chunk.GetSpan<T7>(C7);
            Span8 = chunk.GetSpan<T8>(C8);
        }

        private bool NextArchetype()
        {
            while (true)
            {
                // If there are no archetypes exit with false
                if (!_archetypesEnumerator.MoveNext())
                    return false;

                // Try to move to the next (first) chunk of this archetype. Might fail if there
                // are no chunks in this archetype.
                _chunksEnumerator = _archetypesEnumerator.Current.Archetype.GetChunkEnumerator();
                if (NextChunk())
                    break;
            }

            return true;
        }

        private bool NextChunk()
        {
            if (!_chunksEnumerator.MoveNext())
                return false;

            GetChunkSpans();
            _entityIndex = 0;
            return true;
        }

        public bool MoveNext()
        {
            _entityIndex++;
            if (_entityIndex < SpanEntities.Length)
                return true;

            if (!_initialized)
            {
                _initialized = true;

                if (!NextArchetype())
                    return false;
                return true;
            }

            if (!NextChunk())
                if (!NextArchetype())
                    return false;

            return true;
        }

        // ReSharper disable once UnusedMember.Global (Justification: used by enumerator)
        public void Dispose()
        {
            _archetypesEnumerator.Dispose();
            _chunksEnumerator.Dispose();
        }
    }
}

namespace Myriad.ECS.Worlds
{
    public partial class World
    {
        private readonly List<(ComponentID[], QueryDescription)> _queryCache9 = [ ];

        [System.Diagnostics.Contracts.Pure]
        [JetBrains.Annotations.Pure]
        public QueryResultEnumerable9<T0, T1, T2, T3, T4, T5, T6, T7, T8> Query<T0, T1, T2, T3, T4, T5, T6, T7, T8>(QueryDescription query)
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
            return new QueryResultEnumerable9<T0, T1, T2, T3, T4, T5, T6, T7, T8>(
                query
            );
        }

        [System.Diagnostics.Contracts.Pure]
        [JetBrains.Annotations.Pure]
        public QueryResultEnumerable9<T0, T1, T2, T3, T4, T5, T6, T7, T8> Query<T0, T1, T2, T3, T4, T5, T6, T7, T8>()
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
            // Find query that matches these types
            var query = default(QueryDescription);
            foreach (var item in _queryCache9)
            {
                if (!item.Item1.Contains(ComponentID<T0>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T1>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T2>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T3>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T4>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T5>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T6>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T7>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T8>.ID))
                    continue;

                query = item.Item2;
                break;
            }

            // Didn't find one, create it now and store it in the cache
            if (query == null)
            {
                query = new QueryBuilder()
                    .Include<T0>()
                    .Include<T1>()
                    .Include<T2>()
                    .Include<T3>()
                    .Include<T4>()
                    .Include<T5>()
                    .Include<T6>()
                    .Include<T7>()
                    .Include<T8>()
                    .Build(this);

                _queryCache9.Add(([
                    ComponentID<T0>.ID,
                    ComponentID<T1>.ID,
                    ComponentID<T2>.ID,
                    ComponentID<T3>.ID,
                    ComponentID<T4>.ID,
                    ComponentID<T5>.ID,
                    ComponentID<T6>.ID,
                    ComponentID<T7>.ID,
                    ComponentID<T8>.ID,
                ], query));
            }

            return new QueryResultEnumerable9<T0, T1, T2, T3, T4, T5, T6, T7, T8>(
                query
            );
        }
    }
}



