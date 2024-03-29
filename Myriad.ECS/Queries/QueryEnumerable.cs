﻿
#if NET6_0_OR_GREATER

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

        public readonly Entity Current
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

        public QueryResultEnumerable0 Query(QueryDescription query)
            
        {
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

        public readonly RefTuple1<T0> Current
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
        public int Count<T0>(QueryDescription? query = null)
            where T0 : IComponent
        {
            // Find query that matches these types
            query ??= GetCachedQuery<T0>();

            var count = 0;
            foreach (var item in query.GetArchetypes())
                count += item.Archetype.EntityCount;

            return count;
        }

        public QueryResultEnumerable1<T0> Query<T0>(QueryDescription query)
            where T0 : IComponent
        {
            return new QueryResultEnumerable1<T0>(
                query
            );
        }

        public QueryResultEnumerable1<T0> Query<T0>()
            where T0 : IComponent
        {
            // Find query that matches these types
            var query = GetCachedQuery<T0>();

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

        public readonly RefTuple2<T0, T1> Current
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
        public int Count<T0, T1>(QueryDescription? query = null)
            where T0 : IComponent
            where T1 : IComponent
        {
            // Find query that matches these types
            query ??= GetCachedQuery<T0, T1>();

            var count = 0;
            foreach (var item in query.GetArchetypes())
                count += item.Archetype.EntityCount;

            return count;
        }

        public QueryResultEnumerable2<T0, T1> Query<T0, T1>(QueryDescription query)
            where T0 : IComponent
            where T1 : IComponent
        {
            return new QueryResultEnumerable2<T0, T1>(
                query
            );
        }

        public QueryResultEnumerable2<T0, T1> Query<T0, T1>()
            where T0 : IComponent
            where T1 : IComponent
        {
            // Find query that matches these types
            var query = GetCachedQuery<T0, T1>();

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

        public readonly RefTuple3<T0, T1, T2> Current
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
        public int Count<T0, T1, T2>(QueryDescription? query = null)
            where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
        {
            // Find query that matches these types
            query ??= GetCachedQuery<T0, T1, T2>();

            var count = 0;
            foreach (var item in query.GetArchetypes())
                count += item.Archetype.EntityCount;

            return count;
        }

        public QueryResultEnumerable3<T0, T1, T2> Query<T0, T1, T2>(QueryDescription query)
            where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
        {
            return new QueryResultEnumerable3<T0, T1, T2>(
                query
            );
        }

        public QueryResultEnumerable3<T0, T1, T2> Query<T0, T1, T2>()
            where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
        {
            // Find query that matches these types
            var query = GetCachedQuery<T0, T1, T2>();

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

        public readonly RefTuple4<T0, T1, T2, T3> Current
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
        public int Count<T0, T1, T2, T3>(QueryDescription? query = null)
            where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
        {
            // Find query that matches these types
            query ??= GetCachedQuery<T0, T1, T2, T3>();

            var count = 0;
            foreach (var item in query.GetArchetypes())
                count += item.Archetype.EntityCount;

            return count;
        }

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

        public QueryResultEnumerable4<T0, T1, T2, T3> Query<T0, T1, T2, T3>()
            where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
        {
            // Find query that matches these types
            var query = GetCachedQuery<T0, T1, T2, T3>();

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

        public readonly RefTuple5<T0, T1, T2, T3, T4> Current
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
        public int Count<T0, T1, T2, T3, T4>(QueryDescription? query = null)
            where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
        {
            // Find query that matches these types
            query ??= GetCachedQuery<T0, T1, T2, T3, T4>();

            var count = 0;
            foreach (var item in query.GetArchetypes())
                count += item.Archetype.EntityCount;

            return count;
        }

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

        public QueryResultEnumerable5<T0, T1, T2, T3, T4> Query<T0, T1, T2, T3, T4>()
            where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
        {
            // Find query that matches these types
            var query = GetCachedQuery<T0, T1, T2, T3, T4>();

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

        public readonly RefTuple6<T0, T1, T2, T3, T4, T5> Current
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
        public int Count<T0, T1, T2, T3, T4, T5>(QueryDescription? query = null)
            where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
        {
            // Find query that matches these types
            query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5>();

            var count = 0;
            foreach (var item in query.GetArchetypes())
                count += item.Archetype.EntityCount;

            return count;
        }

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

        public QueryResultEnumerable6<T0, T1, T2, T3, T4, T5> Query<T0, T1, T2, T3, T4, T5>()
            where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
        {
            // Find query that matches these types
            var query = GetCachedQuery<T0, T1, T2, T3, T4, T5>();

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

        public readonly RefTuple7<T0, T1, T2, T3, T4, T5, T6> Current
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
        public int Count<T0, T1, T2, T3, T4, T5, T6>(QueryDescription? query = null)
            where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
        {
            // Find query that matches these types
            query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6>();

            var count = 0;
            foreach (var item in query.GetArchetypes())
                count += item.Archetype.EntityCount;

            return count;
        }

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
            var query = GetCachedQuery<T0, T1, T2, T3, T4, T5, T6>();

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

        public readonly RefTuple8<T0, T1, T2, T3, T4, T5, T6, T7> Current
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
        public int Count<T0, T1, T2, T3, T4, T5, T6, T7>(QueryDescription? query = null)
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
            query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7>();

            var count = 0;
            foreach (var item in query.GetArchetypes())
                count += item.Archetype.EntityCount;

            return count;
        }

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
            var query = GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7>();

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

        public readonly RefTuple9<T0, T1, T2, T3, T4, T5, T6, T7, T8> Current
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
        public int Count<T0, T1, T2, T3, T4, T5, T6, T7, T8>(QueryDescription? query = null)
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
            query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8>();

            var count = 0;
            foreach (var item in query.GetArchetypes())
                count += item.Archetype.EntityCount;

            return count;
        }

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
            var query = GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8>();

            return new QueryResultEnumerable9<T0, T1, T2, T3, T4, T5, T6, T7, T8>(
                query
            );
        }

    }
}


namespace Myriad.ECS.Queries
{
    public readonly struct QueryResultEnumerable10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
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
        private readonly QueryDescription _query;

        internal QueryResultEnumerable10(QueryDescription query)
        {
            _query = query;
        }

        public QueryResultEnumerator10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> GetEnumerator()
        {
            return new QueryResultEnumerator10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
                _query.GetArchetypes()
            );
        }
    }

    public ref struct QueryResultEnumerator10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
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
        private static readonly ComponentID C0 = ComponentID<T0>.ID;
        private static readonly ComponentID C1 = ComponentID<T1>.ID;
        private static readonly ComponentID C2 = ComponentID<T2>.ID;
        private static readonly ComponentID C3 = ComponentID<T3>.ID;
        private static readonly ComponentID C4 = ComponentID<T4>.ID;
        private static readonly ComponentID C5 = ComponentID<T5>.ID;
        private static readonly ComponentID C6 = ComponentID<T6>.ID;
        private static readonly ComponentID C7 = ComponentID<T7>.ID;
        private static readonly ComponentID C8 = ComponentID<T8>.ID;
        private static readonly ComponentID C9 = ComponentID<T9>.ID;

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
        private Span<T9> Span9 = default;

        internal QueryResultEnumerator10(FrozenOrderedListSet<QueryDescription.ArchetypeMatch> archetypes)
        {
            _archetypesEnumerator = archetypes.GetEnumerator();
        }

        public readonly RefTuple10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> Current
        {
            get
            {
                return new RefTuple10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
                    SpanEntities[_entityIndex],
                    ref Span0[_entityIndex],
                    ref Span1[_entityIndex],
                    ref Span2[_entityIndex],
                    ref Span3[_entityIndex],
                    ref Span4[_entityIndex],
                    ref Span5[_entityIndex],
                    ref Span6[_entityIndex],
                    ref Span7[_entityIndex],
                    ref Span8[_entityIndex],
                    ref Span9[_entityIndex]
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
            Span9 = chunk.GetSpan<T9>(C9);
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
        public int Count<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(QueryDescription? query = null)
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
            // Find query that matches these types
            query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>();

            var count = 0;
            foreach (var item in query.GetArchetypes())
                count += item.Archetype.EntityCount;

            return count;
        }

        public QueryResultEnumerable10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(QueryDescription query)
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
            return new QueryResultEnumerable10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
                query
            );
        }

        public QueryResultEnumerable10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>()
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
            // Find query that matches these types
            var query = GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>();

            return new QueryResultEnumerable10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
                query
            );
        }

    }
}


namespace Myriad.ECS.Queries
{
    public readonly struct QueryResultEnumerable11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
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
        private readonly QueryDescription _query;

        internal QueryResultEnumerable11(QueryDescription query)
        {
            _query = query;
        }

        public QueryResultEnumerator11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> GetEnumerator()
        {
            return new QueryResultEnumerator11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
                _query.GetArchetypes()
            );
        }
    }

    public ref struct QueryResultEnumerator11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
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
        private static readonly ComponentID C0 = ComponentID<T0>.ID;
        private static readonly ComponentID C1 = ComponentID<T1>.ID;
        private static readonly ComponentID C2 = ComponentID<T2>.ID;
        private static readonly ComponentID C3 = ComponentID<T3>.ID;
        private static readonly ComponentID C4 = ComponentID<T4>.ID;
        private static readonly ComponentID C5 = ComponentID<T5>.ID;
        private static readonly ComponentID C6 = ComponentID<T6>.ID;
        private static readonly ComponentID C7 = ComponentID<T7>.ID;
        private static readonly ComponentID C8 = ComponentID<T8>.ID;
        private static readonly ComponentID C9 = ComponentID<T9>.ID;
        private static readonly ComponentID C10 = ComponentID<T10>.ID;

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
        private Span<T9> Span9 = default;
        private Span<T10> Span10 = default;

        internal QueryResultEnumerator11(FrozenOrderedListSet<QueryDescription.ArchetypeMatch> archetypes)
        {
            _archetypesEnumerator = archetypes.GetEnumerator();
        }

        public readonly RefTuple11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Current
        {
            get
            {
                return new RefTuple11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
                    SpanEntities[_entityIndex],
                    ref Span0[_entityIndex],
                    ref Span1[_entityIndex],
                    ref Span2[_entityIndex],
                    ref Span3[_entityIndex],
                    ref Span4[_entityIndex],
                    ref Span5[_entityIndex],
                    ref Span6[_entityIndex],
                    ref Span7[_entityIndex],
                    ref Span8[_entityIndex],
                    ref Span9[_entityIndex],
                    ref Span10[_entityIndex]
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
            Span9 = chunk.GetSpan<T9>(C9);
            Span10 = chunk.GetSpan<T10>(C10);
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
        public int Count<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(QueryDescription? query = null)
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
            // Find query that matches these types
            query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>();

            var count = 0;
            foreach (var item in query.GetArchetypes())
                count += item.Archetype.EntityCount;

            return count;
        }

        public QueryResultEnumerable11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(QueryDescription query)
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
            return new QueryResultEnumerable11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
                query
            );
        }

        public QueryResultEnumerable11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>()
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
            // Find query that matches these types
            var query = GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>();

            return new QueryResultEnumerable11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
                query
            );
        }

    }
}


namespace Myriad.ECS.Queries
{
    public readonly struct QueryResultEnumerable12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
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
        private readonly QueryDescription _query;

        internal QueryResultEnumerable12(QueryDescription query)
        {
            _query = query;
        }

        public QueryResultEnumerator12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> GetEnumerator()
        {
            return new QueryResultEnumerator12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
                _query.GetArchetypes()
            );
        }
    }

    public ref struct QueryResultEnumerator12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
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
        private static readonly ComponentID C0 = ComponentID<T0>.ID;
        private static readonly ComponentID C1 = ComponentID<T1>.ID;
        private static readonly ComponentID C2 = ComponentID<T2>.ID;
        private static readonly ComponentID C3 = ComponentID<T3>.ID;
        private static readonly ComponentID C4 = ComponentID<T4>.ID;
        private static readonly ComponentID C5 = ComponentID<T5>.ID;
        private static readonly ComponentID C6 = ComponentID<T6>.ID;
        private static readonly ComponentID C7 = ComponentID<T7>.ID;
        private static readonly ComponentID C8 = ComponentID<T8>.ID;
        private static readonly ComponentID C9 = ComponentID<T9>.ID;
        private static readonly ComponentID C10 = ComponentID<T10>.ID;
        private static readonly ComponentID C11 = ComponentID<T11>.ID;

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
        private Span<T9> Span9 = default;
        private Span<T10> Span10 = default;
        private Span<T11> Span11 = default;

        internal QueryResultEnumerator12(FrozenOrderedListSet<QueryDescription.ArchetypeMatch> archetypes)
        {
            _archetypesEnumerator = archetypes.GetEnumerator();
        }

        public readonly RefTuple12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Current
        {
            get
            {
                return new RefTuple12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
                    SpanEntities[_entityIndex],
                    ref Span0[_entityIndex],
                    ref Span1[_entityIndex],
                    ref Span2[_entityIndex],
                    ref Span3[_entityIndex],
                    ref Span4[_entityIndex],
                    ref Span5[_entityIndex],
                    ref Span6[_entityIndex],
                    ref Span7[_entityIndex],
                    ref Span8[_entityIndex],
                    ref Span9[_entityIndex],
                    ref Span10[_entityIndex],
                    ref Span11[_entityIndex]
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
            Span9 = chunk.GetSpan<T9>(C9);
            Span10 = chunk.GetSpan<T10>(C10);
            Span11 = chunk.GetSpan<T11>(C11);
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
        public int Count<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(QueryDescription? query = null)
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
            // Find query that matches these types
            query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>();

            var count = 0;
            foreach (var item in query.GetArchetypes())
                count += item.Archetype.EntityCount;

            return count;
        }

        public QueryResultEnumerable12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(QueryDescription query)
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
            return new QueryResultEnumerable12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
                query
            );
        }

        public QueryResultEnumerable12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>()
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
            // Find query that matches these types
            var query = GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>();

            return new QueryResultEnumerable12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
                query
            );
        }

    }
}


namespace Myriad.ECS.Queries
{
    public readonly struct QueryResultEnumerable13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
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
        private readonly QueryDescription _query;

        internal QueryResultEnumerable13(QueryDescription query)
        {
            _query = query;
        }

        public QueryResultEnumerator13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> GetEnumerator()
        {
            return new QueryResultEnumerator13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
                _query.GetArchetypes()
            );
        }
    }

    public ref struct QueryResultEnumerator13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
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
        private static readonly ComponentID C0 = ComponentID<T0>.ID;
        private static readonly ComponentID C1 = ComponentID<T1>.ID;
        private static readonly ComponentID C2 = ComponentID<T2>.ID;
        private static readonly ComponentID C3 = ComponentID<T3>.ID;
        private static readonly ComponentID C4 = ComponentID<T4>.ID;
        private static readonly ComponentID C5 = ComponentID<T5>.ID;
        private static readonly ComponentID C6 = ComponentID<T6>.ID;
        private static readonly ComponentID C7 = ComponentID<T7>.ID;
        private static readonly ComponentID C8 = ComponentID<T8>.ID;
        private static readonly ComponentID C9 = ComponentID<T9>.ID;
        private static readonly ComponentID C10 = ComponentID<T10>.ID;
        private static readonly ComponentID C11 = ComponentID<T11>.ID;
        private static readonly ComponentID C12 = ComponentID<T12>.ID;

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
        private Span<T9> Span9 = default;
        private Span<T10> Span10 = default;
        private Span<T11> Span11 = default;
        private Span<T12> Span12 = default;

        internal QueryResultEnumerator13(FrozenOrderedListSet<QueryDescription.ArchetypeMatch> archetypes)
        {
            _archetypesEnumerator = archetypes.GetEnumerator();
        }

        public readonly RefTuple13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Current
        {
            get
            {
                return new RefTuple13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
                    SpanEntities[_entityIndex],
                    ref Span0[_entityIndex],
                    ref Span1[_entityIndex],
                    ref Span2[_entityIndex],
                    ref Span3[_entityIndex],
                    ref Span4[_entityIndex],
                    ref Span5[_entityIndex],
                    ref Span6[_entityIndex],
                    ref Span7[_entityIndex],
                    ref Span8[_entityIndex],
                    ref Span9[_entityIndex],
                    ref Span10[_entityIndex],
                    ref Span11[_entityIndex],
                    ref Span12[_entityIndex]
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
            Span9 = chunk.GetSpan<T9>(C9);
            Span10 = chunk.GetSpan<T10>(C10);
            Span11 = chunk.GetSpan<T11>(C11);
            Span12 = chunk.GetSpan<T12>(C12);
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
        public int Count<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(QueryDescription? query = null)
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
            // Find query that matches these types
            query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>();

            var count = 0;
            foreach (var item in query.GetArchetypes())
                count += item.Archetype.EntityCount;

            return count;
        }

        public QueryResultEnumerable13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(QueryDescription query)
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
            return new QueryResultEnumerable13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
                query
            );
        }

        public QueryResultEnumerable13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>()
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
            // Find query that matches these types
            var query = GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>();

            return new QueryResultEnumerable13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
                query
            );
        }

    }
}


namespace Myriad.ECS.Queries
{
    public readonly struct QueryResultEnumerable14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
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
        private readonly QueryDescription _query;

        internal QueryResultEnumerable14(QueryDescription query)
        {
            _query = query;
        }

        public QueryResultEnumerator14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> GetEnumerator()
        {
            return new QueryResultEnumerator14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
                _query.GetArchetypes()
            );
        }
    }

    public ref struct QueryResultEnumerator14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
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
        private static readonly ComponentID C0 = ComponentID<T0>.ID;
        private static readonly ComponentID C1 = ComponentID<T1>.ID;
        private static readonly ComponentID C2 = ComponentID<T2>.ID;
        private static readonly ComponentID C3 = ComponentID<T3>.ID;
        private static readonly ComponentID C4 = ComponentID<T4>.ID;
        private static readonly ComponentID C5 = ComponentID<T5>.ID;
        private static readonly ComponentID C6 = ComponentID<T6>.ID;
        private static readonly ComponentID C7 = ComponentID<T7>.ID;
        private static readonly ComponentID C8 = ComponentID<T8>.ID;
        private static readonly ComponentID C9 = ComponentID<T9>.ID;
        private static readonly ComponentID C10 = ComponentID<T10>.ID;
        private static readonly ComponentID C11 = ComponentID<T11>.ID;
        private static readonly ComponentID C12 = ComponentID<T12>.ID;
        private static readonly ComponentID C13 = ComponentID<T13>.ID;

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
        private Span<T9> Span9 = default;
        private Span<T10> Span10 = default;
        private Span<T11> Span11 = default;
        private Span<T12> Span12 = default;
        private Span<T13> Span13 = default;

        internal QueryResultEnumerator14(FrozenOrderedListSet<QueryDescription.ArchetypeMatch> archetypes)
        {
            _archetypesEnumerator = archetypes.GetEnumerator();
        }

        public readonly RefTuple14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> Current
        {
            get
            {
                return new RefTuple14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
                    SpanEntities[_entityIndex],
                    ref Span0[_entityIndex],
                    ref Span1[_entityIndex],
                    ref Span2[_entityIndex],
                    ref Span3[_entityIndex],
                    ref Span4[_entityIndex],
                    ref Span5[_entityIndex],
                    ref Span6[_entityIndex],
                    ref Span7[_entityIndex],
                    ref Span8[_entityIndex],
                    ref Span9[_entityIndex],
                    ref Span10[_entityIndex],
                    ref Span11[_entityIndex],
                    ref Span12[_entityIndex],
                    ref Span13[_entityIndex]
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
            Span9 = chunk.GetSpan<T9>(C9);
            Span10 = chunk.GetSpan<T10>(C10);
            Span11 = chunk.GetSpan<T11>(C11);
            Span12 = chunk.GetSpan<T12>(C12);
            Span13 = chunk.GetSpan<T13>(C13);
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
        public int Count<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(QueryDescription? query = null)
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
            // Find query that matches these types
            query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>();

            var count = 0;
            foreach (var item in query.GetArchetypes())
                count += item.Archetype.EntityCount;

            return count;
        }

        public QueryResultEnumerable14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(QueryDescription query)
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
            return new QueryResultEnumerable14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
                query
            );
        }

        public QueryResultEnumerable14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>()
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
            // Find query that matches these types
            var query = GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>();

            return new QueryResultEnumerable14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
                query
            );
        }

    }
}


namespace Myriad.ECS.Queries
{
    public readonly struct QueryResultEnumerable15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
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
        private readonly QueryDescription _query;

        internal QueryResultEnumerable15(QueryDescription query)
        {
            _query = query;
        }

        public QueryResultEnumerator15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> GetEnumerator()
        {
            return new QueryResultEnumerator15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
                _query.GetArchetypes()
            );
        }
    }

    public ref struct QueryResultEnumerator15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
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
        private static readonly ComponentID C0 = ComponentID<T0>.ID;
        private static readonly ComponentID C1 = ComponentID<T1>.ID;
        private static readonly ComponentID C2 = ComponentID<T2>.ID;
        private static readonly ComponentID C3 = ComponentID<T3>.ID;
        private static readonly ComponentID C4 = ComponentID<T4>.ID;
        private static readonly ComponentID C5 = ComponentID<T5>.ID;
        private static readonly ComponentID C6 = ComponentID<T6>.ID;
        private static readonly ComponentID C7 = ComponentID<T7>.ID;
        private static readonly ComponentID C8 = ComponentID<T8>.ID;
        private static readonly ComponentID C9 = ComponentID<T9>.ID;
        private static readonly ComponentID C10 = ComponentID<T10>.ID;
        private static readonly ComponentID C11 = ComponentID<T11>.ID;
        private static readonly ComponentID C12 = ComponentID<T12>.ID;
        private static readonly ComponentID C13 = ComponentID<T13>.ID;
        private static readonly ComponentID C14 = ComponentID<T14>.ID;

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
        private Span<T9> Span9 = default;
        private Span<T10> Span10 = default;
        private Span<T11> Span11 = default;
        private Span<T12> Span12 = default;
        private Span<T13> Span13 = default;
        private Span<T14> Span14 = default;

        internal QueryResultEnumerator15(FrozenOrderedListSet<QueryDescription.ArchetypeMatch> archetypes)
        {
            _archetypesEnumerator = archetypes.GetEnumerator();
        }

        public readonly RefTuple15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> Current
        {
            get
            {
                return new RefTuple15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
                    SpanEntities[_entityIndex],
                    ref Span0[_entityIndex],
                    ref Span1[_entityIndex],
                    ref Span2[_entityIndex],
                    ref Span3[_entityIndex],
                    ref Span4[_entityIndex],
                    ref Span5[_entityIndex],
                    ref Span6[_entityIndex],
                    ref Span7[_entityIndex],
                    ref Span8[_entityIndex],
                    ref Span9[_entityIndex],
                    ref Span10[_entityIndex],
                    ref Span11[_entityIndex],
                    ref Span12[_entityIndex],
                    ref Span13[_entityIndex],
                    ref Span14[_entityIndex]
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
            Span9 = chunk.GetSpan<T9>(C9);
            Span10 = chunk.GetSpan<T10>(C10);
            Span11 = chunk.GetSpan<T11>(C11);
            Span12 = chunk.GetSpan<T12>(C12);
            Span13 = chunk.GetSpan<T13>(C13);
            Span14 = chunk.GetSpan<T14>(C14);
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
        public int Count<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(QueryDescription? query = null)
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
            // Find query that matches these types
            query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>();

            var count = 0;
            foreach (var item in query.GetArchetypes())
                count += item.Archetype.EntityCount;

            return count;
        }

        public QueryResultEnumerable15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(QueryDescription query)
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
            return new QueryResultEnumerable15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
                query
            );
        }

        public QueryResultEnumerable15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>()
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
            // Find query that matches these types
            var query = GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>();

            return new QueryResultEnumerable15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
                query
            );
        }

    }
}


namespace Myriad.ECS.Queries
{
    public readonly struct QueryResultEnumerable16<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
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
        private readonly QueryDescription _query;

        internal QueryResultEnumerable16(QueryDescription query)
        {
            _query = query;
        }

        public QueryResultEnumerator16<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> GetEnumerator()
        {
            return new QueryResultEnumerator16<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
                _query.GetArchetypes()
            );
        }
    }

    public ref struct QueryResultEnumerator16<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
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
        private static readonly ComponentID C0 = ComponentID<T0>.ID;
        private static readonly ComponentID C1 = ComponentID<T1>.ID;
        private static readonly ComponentID C2 = ComponentID<T2>.ID;
        private static readonly ComponentID C3 = ComponentID<T3>.ID;
        private static readonly ComponentID C4 = ComponentID<T4>.ID;
        private static readonly ComponentID C5 = ComponentID<T5>.ID;
        private static readonly ComponentID C6 = ComponentID<T6>.ID;
        private static readonly ComponentID C7 = ComponentID<T7>.ID;
        private static readonly ComponentID C8 = ComponentID<T8>.ID;
        private static readonly ComponentID C9 = ComponentID<T9>.ID;
        private static readonly ComponentID C10 = ComponentID<T10>.ID;
        private static readonly ComponentID C11 = ComponentID<T11>.ID;
        private static readonly ComponentID C12 = ComponentID<T12>.ID;
        private static readonly ComponentID C13 = ComponentID<T13>.ID;
        private static readonly ComponentID C14 = ComponentID<T14>.ID;
        private static readonly ComponentID C15 = ComponentID<T15>.ID;

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
        private Span<T9> Span9 = default;
        private Span<T10> Span10 = default;
        private Span<T11> Span11 = default;
        private Span<T12> Span12 = default;
        private Span<T13> Span13 = default;
        private Span<T14> Span14 = default;
        private Span<T15> Span15 = default;

        internal QueryResultEnumerator16(FrozenOrderedListSet<QueryDescription.ArchetypeMatch> archetypes)
        {
            _archetypesEnumerator = archetypes.GetEnumerator();
        }

        public readonly RefTuple16<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> Current
        {
            get
            {
                return new RefTuple16<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
                    SpanEntities[_entityIndex],
                    ref Span0[_entityIndex],
                    ref Span1[_entityIndex],
                    ref Span2[_entityIndex],
                    ref Span3[_entityIndex],
                    ref Span4[_entityIndex],
                    ref Span5[_entityIndex],
                    ref Span6[_entityIndex],
                    ref Span7[_entityIndex],
                    ref Span8[_entityIndex],
                    ref Span9[_entityIndex],
                    ref Span10[_entityIndex],
                    ref Span11[_entityIndex],
                    ref Span12[_entityIndex],
                    ref Span13[_entityIndex],
                    ref Span14[_entityIndex],
                    ref Span15[_entityIndex]
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
            Span9 = chunk.GetSpan<T9>(C9);
            Span10 = chunk.GetSpan<T10>(C10);
            Span11 = chunk.GetSpan<T11>(C11);
            Span12 = chunk.GetSpan<T12>(C12);
            Span13 = chunk.GetSpan<T13>(C13);
            Span14 = chunk.GetSpan<T14>(C14);
            Span15 = chunk.GetSpan<T15>(C15);
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
        public int Count<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(QueryDescription? query = null)
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
            // Find query that matches these types
            query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>();

            var count = 0;
            foreach (var item in query.GetArchetypes())
                count += item.Archetype.EntityCount;

            return count;
        }

        public QueryResultEnumerable16<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(QueryDescription query)
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
            return new QueryResultEnumerable16<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
                query
            );
        }

        public QueryResultEnumerable16<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>()
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
            // Find query that matches these types
            var query = GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>();

            return new QueryResultEnumerable16<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
                query
            );
        }

    }
}


#endif

