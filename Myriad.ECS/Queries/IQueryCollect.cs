using Myriad.ECS.Queries;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedParameter.Global
// ReSharper disable LoopCanBeConvertedToQuery
// ReSharper disable ConvertToPrimaryConstructor

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// A query which collects entities
		/// </summary>
		public interface IQueryCollector
		{
			/// <summary>
			/// Execute the query for a single entity
			/// </summary>
			public void Add(Entity e);

			/// <summary>
			/// Expand the collection
			/// </summary>
			public void AddCapacity(int capacity);
		}
		
		private readonly struct CollectorAdapter<TQ>
			: IChunkQuery
			where TQ : IQueryCollector
		{
			private readonly TQ _collector;

            public CollectorAdapter(TQ collector)
            {
                _collector = collector;
            }

            public void Execute(ChunkHandle chunk, ReadOnlySpan<Entity> e)
            {
                _collector.AddCapacity(e.Length);

                for (var i = e.Length - 1; i >= 0; i--)
                    _collector.Add(e[i]);
            }
		}

		private readonly struct ListCollectorEntityOnly
			: IQueryCollector
		{
			private readonly List<Entity> _list;

            public ListCollectorEntityOnly(List<Entity> list)
            {
                _list = list;
            }

            public void Add(Entity e)
            {
                _list.Add(e);
            }

            public void AddCapacity(int capacity)
            {
                _list.Capacity += capacity;
            }
		}

		/// <summary>
		/// Collect all entities which match the query into a list
		/// </summary>
		/// <param name="output">The list to add entities to.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect(
			List<Entity> output,
			QueryDescription query
		)
		{
			return Collect(new ListCollectorEntityOnly(output), query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
		/// <param name="q">The instance to execute over every entity.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<TQ>(
			TQ q,
			QueryDescription query
		)
			where TQ : IQueryCollector
		{
			return ExecuteChunk(new CollectorAdapter<TQ>(q), query);
		}
	}
}

namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A query which collects entities
	/// </summary>
	public interface IQueryCollector<T0>
		where T0 : IComponent
	{
		/// <summary>
		/// Execute the query for a single entity
		/// </summary>
		public void Add(Entity e, ref T0 t0);

		/// <summary>
		/// Expand the collection
		/// </summary>
		public void AddCapacity(int capacity);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		
		private readonly struct CollectorAdapter<TQ, T0>
			: IChunkQuery<T0>
			where T0 : IComponent
			where TQ : IQueryCollector<T0>
		{
			private readonly TQ _collector;

            public CollectorAdapter(TQ collector)
            {
                _collector = collector;
            }

            public void Execute(ChunkHandle chunk, ReadOnlySpan<Entity> e, Span<T0> t0)
            {
                _collector.AddCapacity(e.Length);

                for (var i = e.Length - 1; i >= 0; i--)
                    _collector.Add(e[i], ref t0[i]);
            }
		}

		
		private readonly struct ListCollectorEntityOnly<T0>
			: IQueryCollector<T0>
			where T0 : IComponent
		{
			private readonly List<Entity> _list;

            public ListCollectorEntityOnly(List<Entity> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0)
            {
                _list.Add(e);
            }

            public void AddCapacity(int capacity)
            {
                _list.Capacity += capacity;
            }
		}

		/// <summary>
		/// Collect all entities which match the query into a list
		/// </summary>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <param name="output">The list to add entities to.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<T0>(
			List<Entity> output,
			QueryDescription? query = null
		)
			where T0 : IComponent
		{
			return Collect<
				ListCollectorEntityOnly<T0>,
				T0
			>(
				new ListCollectorEntityOnly<T0>(output)
			);
		}

		
		private readonly struct ListCollectorWithComponents<T0>
			: IQueryCollector<T0>
			where T0 : IComponent
		{
			private readonly List<(Entity, T0)> _list;

            public ListCollectorWithComponents(List<(Entity, T0)> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0)
            {
                _list.Add((e, t0));
            }

            public void AddCapacity(int capacity)
            {
                _list.Capacity += capacity;
            }
		}

		/// <summary>
		/// Collect all entities which match the query into a list
		/// </summary>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <param name="output">The list to add entities to.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<T0>(
			List<(Entity, T0)> output,
			QueryDescription? query = null
		)
			where T0 : IComponent
		{
			return Collect<
				ListCollectorWithComponents<T0>,
				T0
			>(
				new ListCollectorWithComponents<T0>(output)
			);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <param name="q">The instance to execute over every entity.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<TQ, T0>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : IComponent
			where TQ : IQueryCollector<T0>
		{
			var ca = new CollectorAdapter<TQ, T0>(q);
			return ExecuteChunk
			<
				CollectorAdapter<TQ, T0>,
				T0
			>(
				ca,
				query
			);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <param name="q">The instance to execute over every entity.</param>
		/// <returns>The number of entities discovered by this query</returns>
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<TQ, T0>(
			TQ q,
			ref QueryDescription? query
		)
			where T0 : IComponent
			where TQ : IQueryCollector<T0>
		{
			return ExecuteChunk
			<
				CollectorAdapter<TQ, T0>,
				T0
			>(
				new CollectorAdapter<TQ, T0>(q),
				ref query
			);
		}
	}
}
namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A query which collects entities
	/// </summary>
	public interface IQueryCollector<T0, T1>
		where T0 : IComponent
        where T1 : IComponent
	{
		/// <summary>
		/// Execute the query for a single entity
		/// </summary>
		public void Add(Entity e, ref T0 t0, ref T1 t1);

		/// <summary>
		/// Expand the collection
		/// </summary>
		public void AddCapacity(int capacity);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		[ExcludeFromCodeCoverage]
		private readonly struct CollectorAdapter<TQ, T0, T1>
			: IChunkQuery<T0, T1>
			where T0 : IComponent
            where T1 : IComponent
			where TQ : IQueryCollector<T0, T1>
		{
			private readonly TQ _collector;

            public CollectorAdapter(TQ collector)
            {
                _collector = collector;
            }

            public void Execute(ChunkHandle chunk, ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1)
            {
                _collector.AddCapacity(e.Length);

                for (var i = e.Length - 1; i >= 0; i--)
                    _collector.Add(e[i], ref t0[i], ref t1[i]);
            }
		}

		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorEntityOnly<T0, T1>
			: IQueryCollector<T0, T1>
			where T0 : IComponent
            where T1 : IComponent
		{
			private readonly List<Entity> _list;

            public ListCollectorEntityOnly(List<Entity> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1)
            {
                _list.Add(e);
            }

            public void AddCapacity(int capacity)
            {
                _list.Capacity += capacity;
            }
		}

		/// <summary>
		/// Collect all entities which match the query into a list
		/// </summary>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <param name="output">The list to add entities to.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<T0, T1>(
			List<Entity> output,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
		{
			return Collect<
				ListCollectorEntityOnly<T0, T1>,
				T0, T1
			>(
				new ListCollectorEntityOnly<T0, T1>(output)
			);
		}

		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorWithComponents<T0, T1>
			: IQueryCollector<T0, T1>
			where T0 : IComponent
            where T1 : IComponent
		{
			private readonly List<(Entity, T0, T1)> _list;

            public ListCollectorWithComponents(List<(Entity, T0, T1)> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1)
            {
                _list.Add((e, t0, t1));
            }

            public void AddCapacity(int capacity)
            {
                _list.Capacity += capacity;
            }
		}

		/// <summary>
		/// Collect all entities which match the query into a list
		/// </summary>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <param name="output">The list to add entities to.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<T0, T1>(
			List<(Entity, T0, T1)> output,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
		{
			return Collect<
				ListCollectorWithComponents<T0, T1>,
				T0, T1
			>(
				new ListCollectorWithComponents<T0, T1>(output)
			);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <param name="q">The instance to execute over every entity.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<TQ, T0, T1>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
			where TQ : IQueryCollector<T0, T1>
		{
			var ca = new CollectorAdapter<TQ, T0, T1>(q);
			return ExecuteChunk
			<
				CollectorAdapter<TQ, T0, T1>,
				T0, T1
			>(
				ca,
				query
			);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <param name="q">The instance to execute over every entity.</param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<TQ, T0, T1>(
			TQ q,
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
			where TQ : IQueryCollector<T0, T1>
		{
			return ExecuteChunk
			<
				CollectorAdapter<TQ, T0, T1>,
				T0, T1
			>(
				new CollectorAdapter<TQ, T0, T1>(q),
				ref query
			);
		}
	}
}
namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A query which collects entities
	/// </summary>
	public interface IQueryCollector<T0, T1, T2>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
	{
		/// <summary>
		/// Execute the query for a single entity
		/// </summary>
		public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2);

		/// <summary>
		/// Expand the collection
		/// </summary>
		public void AddCapacity(int capacity);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		[ExcludeFromCodeCoverage]
		private readonly struct CollectorAdapter<TQ, T0, T1, T2>
			: IChunkQuery<T0, T1, T2>
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
			where TQ : IQueryCollector<T0, T1, T2>
		{
			private readonly TQ _collector;

            public CollectorAdapter(TQ collector)
            {
                _collector = collector;
            }

            public void Execute(ChunkHandle chunk, ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2)
            {
                _collector.AddCapacity(e.Length);

                for (var i = e.Length - 1; i >= 0; i--)
                    _collector.Add(e[i], ref t0[i], ref t1[i], ref t2[i]);
            }
		}

		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorEntityOnly<T0, T1, T2>
			: IQueryCollector<T0, T1, T2>
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
		{
			private readonly List<Entity> _list;

            public ListCollectorEntityOnly(List<Entity> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2)
            {
                _list.Add(e);
            }

            public void AddCapacity(int capacity)
            {
                _list.Capacity += capacity;
            }
		}

		/// <summary>
		/// Collect all entities which match the query into a list
		/// </summary>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <param name="output">The list to add entities to.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<T0, T1, T2>(
			List<Entity> output,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
		{
			return Collect<
				ListCollectorEntityOnly<T0, T1, T2>,
				T0, T1, T2
			>(
				new ListCollectorEntityOnly<T0, T1, T2>(output)
			);
		}

		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorWithComponents<T0, T1, T2>
			: IQueryCollector<T0, T1, T2>
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
		{
			private readonly List<(Entity, T0, T1, T2)> _list;

            public ListCollectorWithComponents(List<(Entity, T0, T1, T2)> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2)
            {
                _list.Add((e, t0, t1, t2));
            }

            public void AddCapacity(int capacity)
            {
                _list.Capacity += capacity;
            }
		}

		/// <summary>
		/// Collect all entities which match the query into a list
		/// </summary>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <param name="output">The list to add entities to.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<T0, T1, T2>(
			List<(Entity, T0, T1, T2)> output,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
		{
			return Collect<
				ListCollectorWithComponents<T0, T1, T2>,
				T0, T1, T2
			>(
				new ListCollectorWithComponents<T0, T1, T2>(output)
			);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <param name="q">The instance to execute over every entity.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<TQ, T0, T1, T2>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
			where TQ : IQueryCollector<T0, T1, T2>
		{
			var ca = new CollectorAdapter<TQ, T0, T1, T2>(q);
			return ExecuteChunk
			<
				CollectorAdapter<TQ, T0, T1, T2>,
				T0, T1, T2
			>(
				ca,
				query
			);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <param name="q">The instance to execute over every entity.</param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<TQ, T0, T1, T2>(
			TQ q,
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
			where TQ : IQueryCollector<T0, T1, T2>
		{
			return ExecuteChunk
			<
				CollectorAdapter<TQ, T0, T1, T2>,
				T0, T1, T2
			>(
				new CollectorAdapter<TQ, T0, T1, T2>(q),
				ref query
			);
		}
	}
}
namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A query which collects entities
	/// </summary>
	public interface IQueryCollector<T0, T1, T2, T3>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
	{
		/// <summary>
		/// Execute the query for a single entity
		/// </summary>
		public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3);

		/// <summary>
		/// Expand the collection
		/// </summary>
		public void AddCapacity(int capacity);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		[ExcludeFromCodeCoverage]
		private readonly struct CollectorAdapter<TQ, T0, T1, T2, T3>
			: IChunkQuery<T0, T1, T2, T3>
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
			where TQ : IQueryCollector<T0, T1, T2, T3>
		{
			private readonly TQ _collector;

            public CollectorAdapter(TQ collector)
            {
                _collector = collector;
            }

            public void Execute(ChunkHandle chunk, ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3)
            {
                _collector.AddCapacity(e.Length);

                for (var i = e.Length - 1; i >= 0; i--)
                    _collector.Add(e[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i]);
            }
		}

		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorEntityOnly<T0, T1, T2, T3>
			: IQueryCollector<T0, T1, T2, T3>
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
		{
			private readonly List<Entity> _list;

            public ListCollectorEntityOnly(List<Entity> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3)
            {
                _list.Add(e);
            }

            public void AddCapacity(int capacity)
            {
                _list.Capacity += capacity;
            }
		}

		/// <summary>
		/// Collect all entities which match the query into a list
		/// </summary>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <param name="output">The list to add entities to.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<T0, T1, T2, T3>(
			List<Entity> output,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
		{
			return Collect<
				ListCollectorEntityOnly<T0, T1, T2, T3>,
				T0, T1, T2, T3
			>(
				new ListCollectorEntityOnly<T0, T1, T2, T3>(output)
			);
		}

		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorWithComponents<T0, T1, T2, T3>
			: IQueryCollector<T0, T1, T2, T3>
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
		{
			private readonly List<(Entity, T0, T1, T2, T3)> _list;

            public ListCollectorWithComponents(List<(Entity, T0, T1, T2, T3)> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3)
            {
                _list.Add((e, t0, t1, t2, t3));
            }

            public void AddCapacity(int capacity)
            {
                _list.Capacity += capacity;
            }
		}

		/// <summary>
		/// Collect all entities which match the query into a list
		/// </summary>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <param name="output">The list to add entities to.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<T0, T1, T2, T3>(
			List<(Entity, T0, T1, T2, T3)> output,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
		{
			return Collect<
				ListCollectorWithComponents<T0, T1, T2, T3>,
				T0, T1, T2, T3
			>(
				new ListCollectorWithComponents<T0, T1, T2, T3>(output)
			);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <param name="q">The instance to execute over every entity.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<TQ, T0, T1, T2, T3>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
			where TQ : IQueryCollector<T0, T1, T2, T3>
		{
			var ca = new CollectorAdapter<TQ, T0, T1, T2, T3>(q);
			return ExecuteChunk
			<
				CollectorAdapter<TQ, T0, T1, T2, T3>,
				T0, T1, T2, T3
			>(
				ca,
				query
			);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <param name="q">The instance to execute over every entity.</param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<TQ, T0, T1, T2, T3>(
			TQ q,
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
			where TQ : IQueryCollector<T0, T1, T2, T3>
		{
			return ExecuteChunk
			<
				CollectorAdapter<TQ, T0, T1, T2, T3>,
				T0, T1, T2, T3
			>(
				new CollectorAdapter<TQ, T0, T1, T2, T3>(q),
				ref query
			);
		}
	}
}
namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A query which collects entities
	/// </summary>
	public interface IQueryCollector<T0, T1, T2, T3, T4>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
	{
		/// <summary>
		/// Execute the query for a single entity
		/// </summary>
		public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4);

		/// <summary>
		/// Expand the collection
		/// </summary>
		public void AddCapacity(int capacity);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		[ExcludeFromCodeCoverage]
		private readonly struct CollectorAdapter<TQ, T0, T1, T2, T3, T4>
			: IChunkQuery<T0, T1, T2, T3, T4>
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
			where TQ : IQueryCollector<T0, T1, T2, T3, T4>
		{
			private readonly TQ _collector;

            public CollectorAdapter(TQ collector)
            {
                _collector = collector;
            }

            public void Execute(ChunkHandle chunk, ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4)
            {
                _collector.AddCapacity(e.Length);

                for (var i = e.Length - 1; i >= 0; i--)
                    _collector.Add(e[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i]);
            }
		}

		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorEntityOnly<T0, T1, T2, T3, T4>
			: IQueryCollector<T0, T1, T2, T3, T4>
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
		{
			private readonly List<Entity> _list;

            public ListCollectorEntityOnly(List<Entity> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4)
            {
                _list.Add(e);
            }

            public void AddCapacity(int capacity)
            {
                _list.Capacity += capacity;
            }
		}

		/// <summary>
		/// Collect all entities which match the query into a list
		/// </summary>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <param name="output">The list to add entities to.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<T0, T1, T2, T3, T4>(
			List<Entity> output,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
		{
			return Collect<
				ListCollectorEntityOnly<T0, T1, T2, T3, T4>,
				T0, T1, T2, T3, T4
			>(
				new ListCollectorEntityOnly<T0, T1, T2, T3, T4>(output)
			);
		}

		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorWithComponents<T0, T1, T2, T3, T4>
			: IQueryCollector<T0, T1, T2, T3, T4>
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
		{
			private readonly List<(Entity, T0, T1, T2, T3, T4)> _list;

            public ListCollectorWithComponents(List<(Entity, T0, T1, T2, T3, T4)> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4)
            {
                _list.Add((e, t0, t1, t2, t3, t4));
            }

            public void AddCapacity(int capacity)
            {
                _list.Capacity += capacity;
            }
		}

		/// <summary>
		/// Collect all entities which match the query into a list
		/// </summary>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <param name="output">The list to add entities to.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<T0, T1, T2, T3, T4>(
			List<(Entity, T0, T1, T2, T3, T4)> output,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
		{
			return Collect<
				ListCollectorWithComponents<T0, T1, T2, T3, T4>,
				T0, T1, T2, T3, T4
			>(
				new ListCollectorWithComponents<T0, T1, T2, T3, T4>(output)
			);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <param name="q">The instance to execute over every entity.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<TQ, T0, T1, T2, T3, T4>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
			where TQ : IQueryCollector<T0, T1, T2, T3, T4>
		{
			var ca = new CollectorAdapter<TQ, T0, T1, T2, T3, T4>(q);
			return ExecuteChunk
			<
				CollectorAdapter<TQ, T0, T1, T2, T3, T4>,
				T0, T1, T2, T3, T4
			>(
				ca,
				query
			);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <param name="q">The instance to execute over every entity.</param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<TQ, T0, T1, T2, T3, T4>(
			TQ q,
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
			where TQ : IQueryCollector<T0, T1, T2, T3, T4>
		{
			return ExecuteChunk
			<
				CollectorAdapter<TQ, T0, T1, T2, T3, T4>,
				T0, T1, T2, T3, T4
			>(
				new CollectorAdapter<TQ, T0, T1, T2, T3, T4>(q),
				ref query
			);
		}
	}
}
namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A query which collects entities
	/// </summary>
	public interface IQueryCollector<T0, T1, T2, T3, T4, T5>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
	{
		/// <summary>
		/// Execute the query for a single entity
		/// </summary>
		public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5);

		/// <summary>
		/// Expand the collection
		/// </summary>
		public void AddCapacity(int capacity);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		[ExcludeFromCodeCoverage]
		private readonly struct CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5>
			: IChunkQuery<T0, T1, T2, T3, T4, T5>
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
			where TQ : IQueryCollector<T0, T1, T2, T3, T4, T5>
		{
			private readonly TQ _collector;

            public CollectorAdapter(TQ collector)
            {
                _collector = collector;
            }

            public void Execute(ChunkHandle chunk, ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5)
            {
                _collector.AddCapacity(e.Length);

                for (var i = e.Length - 1; i >= 0; i--)
                    _collector.Add(e[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i]);
            }
		}

		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorEntityOnly<T0, T1, T2, T3, T4, T5>
			: IQueryCollector<T0, T1, T2, T3, T4, T5>
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
		{
			private readonly List<Entity> _list;

            public ListCollectorEntityOnly(List<Entity> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5)
            {
                _list.Add(e);
            }

            public void AddCapacity(int capacity)
            {
                _list.Capacity += capacity;
            }
		}

		/// <summary>
		/// Collect all entities which match the query into a list
		/// </summary>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <param name="output">The list to add entities to.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<T0, T1, T2, T3, T4, T5>(
			List<Entity> output,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
		{
			return Collect<
				ListCollectorEntityOnly<T0, T1, T2, T3, T4, T5>,
				T0, T1, T2, T3, T4, T5
			>(
				new ListCollectorEntityOnly<T0, T1, T2, T3, T4, T5>(output)
			);
		}

		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorWithComponents<T0, T1, T2, T3, T4, T5>
			: IQueryCollector<T0, T1, T2, T3, T4, T5>
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
		{
			private readonly List<(Entity, T0, T1, T2, T3, T4, T5)> _list;

            public ListCollectorWithComponents(List<(Entity, T0, T1, T2, T3, T4, T5)> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5)
            {
                _list.Add((e, t0, t1, t2, t3, t4, t5));
            }

            public void AddCapacity(int capacity)
            {
                _list.Capacity += capacity;
            }
		}

		/// <summary>
		/// Collect all entities which match the query into a list
		/// </summary>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <param name="output">The list to add entities to.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<T0, T1, T2, T3, T4, T5>(
			List<(Entity, T0, T1, T2, T3, T4, T5)> output,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
		{
			return Collect<
				ListCollectorWithComponents<T0, T1, T2, T3, T4, T5>,
				T0, T1, T2, T3, T4, T5
			>(
				new ListCollectorWithComponents<T0, T1, T2, T3, T4, T5>(output)
			);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <param name="q">The instance to execute over every entity.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<TQ, T0, T1, T2, T3, T4, T5>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
			where TQ : IQueryCollector<T0, T1, T2, T3, T4, T5>
		{
			var ca = new CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5>(q);
			return ExecuteChunk
			<
				CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5>,
				T0, T1, T2, T3, T4, T5
			>(
				ca,
				query
			);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <param name="q">The instance to execute over every entity.</param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<TQ, T0, T1, T2, T3, T4, T5>(
			TQ q,
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
			where TQ : IQueryCollector<T0, T1, T2, T3, T4, T5>
		{
			return ExecuteChunk
			<
				CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5>,
				T0, T1, T2, T3, T4, T5
			>(
				new CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5>(q),
				ref query
			);
		}
	}
}
namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A query which collects entities
	/// </summary>
	public interface IQueryCollector<T0, T1, T2, T3, T4, T5, T6>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
	{
		/// <summary>
		/// Execute the query for a single entity
		/// </summary>
		public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6);

		/// <summary>
		/// Expand the collection
		/// </summary>
		public void AddCapacity(int capacity);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		[ExcludeFromCodeCoverage]
		private readonly struct CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6>
			: IChunkQuery<T0, T1, T2, T3, T4, T5, T6>
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
			where TQ : IQueryCollector<T0, T1, T2, T3, T4, T5, T6>
		{
			private readonly TQ _collector;

            public CollectorAdapter(TQ collector)
            {
                _collector = collector;
            }

            public void Execute(ChunkHandle chunk, ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6)
            {
                _collector.AddCapacity(e.Length);

                for (var i = e.Length - 1; i >= 0; i--)
                    _collector.Add(e[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i]);
            }
		}

		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorEntityOnly<T0, T1, T2, T3, T4, T5, T6>
			: IQueryCollector<T0, T1, T2, T3, T4, T5, T6>
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
		{
			private readonly List<Entity> _list;

            public ListCollectorEntityOnly(List<Entity> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6)
            {
                _list.Add(e);
            }

            public void AddCapacity(int capacity)
            {
                _list.Capacity += capacity;
            }
		}

		/// <summary>
		/// Collect all entities which match the query into a list
		/// </summary>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <param name="output">The list to add entities to.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<T0, T1, T2, T3, T4, T5, T6>(
			List<Entity> output,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
		{
			return Collect<
				ListCollectorEntityOnly<T0, T1, T2, T3, T4, T5, T6>,
				T0, T1, T2, T3, T4, T5, T6
			>(
				new ListCollectorEntityOnly<T0, T1, T2, T3, T4, T5, T6>(output)
			);
		}

		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorWithComponents<T0, T1, T2, T3, T4, T5, T6>
			: IQueryCollector<T0, T1, T2, T3, T4, T5, T6>
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
		{
			private readonly List<(Entity, T0, T1, T2, T3, T4, T5, T6)> _list;

            public ListCollectorWithComponents(List<(Entity, T0, T1, T2, T3, T4, T5, T6)> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6)
            {
                _list.Add((e, t0, t1, t2, t3, t4, t5, t6));
            }

            public void AddCapacity(int capacity)
            {
                _list.Capacity += capacity;
            }
		}

		/// <summary>
		/// Collect all entities which match the query into a list
		/// </summary>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <param name="output">The list to add entities to.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<T0, T1, T2, T3, T4, T5, T6>(
			List<(Entity, T0, T1, T2, T3, T4, T5, T6)> output,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
		{
			return Collect<
				ListCollectorWithComponents<T0, T1, T2, T3, T4, T5, T6>,
				T0, T1, T2, T3, T4, T5, T6
			>(
				new ListCollectorWithComponents<T0, T1, T2, T3, T4, T5, T6>(output)
			);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <param name="q">The instance to execute over every entity.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<TQ, T0, T1, T2, T3, T4, T5, T6>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
			where TQ : IQueryCollector<T0, T1, T2, T3, T4, T5, T6>
		{
			var ca = new CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6>(q);
			return ExecuteChunk
			<
				CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6>,
				T0, T1, T2, T3, T4, T5, T6
			>(
				ca,
				query
			);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <param name="q">The instance to execute over every entity.</param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<TQ, T0, T1, T2, T3, T4, T5, T6>(
			TQ q,
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
			where TQ : IQueryCollector<T0, T1, T2, T3, T4, T5, T6>
		{
			return ExecuteChunk
			<
				CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6>,
				T0, T1, T2, T3, T4, T5, T6
			>(
				new CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6>(q),
				ref query
			);
		}
	}
}
namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A query which collects entities
	/// </summary>
	public interface IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
	{
		/// <summary>
		/// Execute the query for a single entity
		/// </summary>
		public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7);

		/// <summary>
		/// Expand the collection
		/// </summary>
		public void AddCapacity(int capacity);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		[ExcludeFromCodeCoverage]
		private readonly struct CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6, T7>
			: IChunkQuery<T0, T1, T2, T3, T4, T5, T6, T7>
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
			where TQ : IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7>
		{
			private readonly TQ _collector;

            public CollectorAdapter(TQ collector)
            {
                _collector = collector;
            }

            public void Execute(ChunkHandle chunk, ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7)
            {
                _collector.AddCapacity(e.Length);

                for (var i = e.Length - 1; i >= 0; i--)
                    _collector.Add(e[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i]);
            }
		}

		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorEntityOnly<T0, T1, T2, T3, T4, T5, T6, T7>
			: IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7>
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
		{
			private readonly List<Entity> _list;

            public ListCollectorEntityOnly(List<Entity> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7)
            {
                _list.Add(e);
            }

            public void AddCapacity(int capacity)
            {
                _list.Capacity += capacity;
            }
		}

		/// <summary>
		/// Collect all entities which match the query into a list
		/// </summary>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		/// <param name="output">The list to add entities to.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<T0, T1, T2, T3, T4, T5, T6, T7>(
			List<Entity> output,
			QueryDescription? query = null
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
			return Collect<
				ListCollectorEntityOnly<T0, T1, T2, T3, T4, T5, T6, T7>,
				T0, T1, T2, T3, T4, T5, T6, T7
			>(
				new ListCollectorEntityOnly<T0, T1, T2, T3, T4, T5, T6, T7>(output)
			);
		}

		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorWithComponents<T0, T1, T2, T3, T4, T5, T6, T7>
			: IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7>
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
		{
			private readonly List<(Entity, T0, T1, T2, T3, T4, T5, T6, T7)> _list;

            public ListCollectorWithComponents(List<(Entity, T0, T1, T2, T3, T4, T5, T6, T7)> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7)
            {
                _list.Add((e, t0, t1, t2, t3, t4, t5, t6, t7));
            }

            public void AddCapacity(int capacity)
            {
                _list.Capacity += capacity;
            }
		}

		/// <summary>
		/// Collect all entities which match the query into a list
		/// </summary>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		/// <param name="output">The list to add entities to.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<T0, T1, T2, T3, T4, T5, T6, T7>(
			List<(Entity, T0, T1, T2, T3, T4, T5, T6, T7)> output,
			QueryDescription? query = null
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
			return Collect<
				ListCollectorWithComponents<T0, T1, T2, T3, T4, T5, T6, T7>,
				T0, T1, T2, T3, T4, T5, T6, T7
			>(
				new ListCollectorWithComponents<T0, T1, T2, T3, T4, T5, T6, T7>(output)
			);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		/// <param name="q">The instance to execute over every entity.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<TQ, T0, T1, T2, T3, T4, T5, T6, T7>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
			where TQ : IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7>
		{
			var ca = new CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6, T7>(q);
			return ExecuteChunk
			<
				CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6, T7>,
				T0, T1, T2, T3, T4, T5, T6, T7
			>(
				ca,
				query
			);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <param name="q">The instance to execute over every entity.</param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<TQ, T0, T1, T2, T3, T4, T5, T6, T7>(
			TQ q,
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
			where TQ : IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7>
		{
			return ExecuteChunk
			<
				CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6, T7>,
				T0, T1, T2, T3, T4, T5, T6, T7
			>(
				new CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6, T7>(q),
				ref query
			);
		}
	}
}
namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A query which collects entities
	/// </summary>
	public interface IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8>
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
		/// <summary>
		/// Execute the query for a single entity
		/// </summary>
		public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8);

		/// <summary>
		/// Expand the collection
		/// </summary>
		public void AddCapacity(int capacity);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		[ExcludeFromCodeCoverage]
		private readonly struct CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8>
			: IChunkQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8>
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
            where T8 : IComponent
			where TQ : IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8>
		{
			private readonly TQ _collector;

            public CollectorAdapter(TQ collector)
            {
                _collector = collector;
            }

            public void Execute(ChunkHandle chunk, ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8)
            {
                _collector.AddCapacity(e.Length);

                for (var i = e.Length - 1; i >= 0; i--)
                    _collector.Add(e[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i]);
            }
		}

		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorEntityOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8>
			: IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8>
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
			private readonly List<Entity> _list;

            public ListCollectorEntityOnly(List<Entity> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8)
            {
                _list.Add(e);
            }

            public void AddCapacity(int capacity)
            {
                _list.Capacity += capacity;
            }
		}

		/// <summary>
		/// Collect all entities which match the query into a list
		/// </summary>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		/// <typeparam name="T8">Component 8 to include in query</typeparam>
		/// <param name="output">The list to add entities to.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<T0, T1, T2, T3, T4, T5, T6, T7, T8>(
			List<Entity> output,
			QueryDescription? query = null
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
			return Collect<
				ListCollectorEntityOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8
			>(
				new ListCollectorEntityOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8>(output)
			);
		}

		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorWithComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8>
			: IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8>
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
			private readonly List<(Entity, T0, T1, T2, T3, T4, T5, T6, T7, T8)> _list;

            public ListCollectorWithComponents(List<(Entity, T0, T1, T2, T3, T4, T5, T6, T7, T8)> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8)
            {
                _list.Add((e, t0, t1, t2, t3, t4, t5, t6, t7, t8));
            }

            public void AddCapacity(int capacity)
            {
                _list.Capacity += capacity;
            }
		}

		/// <summary>
		/// Collect all entities which match the query into a list
		/// </summary>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		/// <typeparam name="T8">Component 8 to include in query</typeparam>
		/// <param name="output">The list to add entities to.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<T0, T1, T2, T3, T4, T5, T6, T7, T8>(
			List<(Entity, T0, T1, T2, T3, T4, T5, T6, T7, T8)> output,
			QueryDescription? query = null
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
			return Collect<
				ListCollectorWithComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8
			>(
				new ListCollectorWithComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8>(output)
			);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		/// <typeparam name="T8">Component 8 to include in query</typeparam>
		/// <param name="q">The instance to execute over every entity.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
			TQ q,
			QueryDescription? query = null
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
			where TQ : IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8>
		{
			var ca = new CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8>(q);
			return ExecuteChunk
			<
				CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8
			>(
				ca,
				query
			);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		/// <typeparam name="T8">Component 8 to include in query</typeparam>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <param name="q">The instance to execute over every entity.</param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
			TQ q,
			ref QueryDescription? query
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
			where TQ : IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8>
		{
			return ExecuteChunk
			<
				CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8
			>(
				new CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8>(q),
				ref query
			);
		}
	}
}
namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A query which collects entities
	/// </summary>
	public interface IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
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
		/// <summary>
		/// Execute the query for a single entity
		/// </summary>
		public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9);

		/// <summary>
		/// Expand the collection
		/// </summary>
		public void AddCapacity(int capacity);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		[ExcludeFromCodeCoverage]
		private readonly struct CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
			: IChunkQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
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
			where TQ : IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
		{
			private readonly TQ _collector;

            public CollectorAdapter(TQ collector)
            {
                _collector = collector;
            }

            public void Execute(ChunkHandle chunk, ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9)
            {
                _collector.AddCapacity(e.Length);

                for (var i = e.Length - 1; i >= 0; i--)
                    _collector.Add(e[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i]);
            }
		}

		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorEntityOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
			: IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
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
			private readonly List<Entity> _list;

            public ListCollectorEntityOnly(List<Entity> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9)
            {
                _list.Add(e);
            }

            public void AddCapacity(int capacity)
            {
                _list.Capacity += capacity;
            }
		}

		/// <summary>
		/// Collect all entities which match the query into a list
		/// </summary>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		/// <typeparam name="T8">Component 8 to include in query</typeparam>
		/// <typeparam name="T9">Component 9 to include in query</typeparam>
		/// <param name="output">The list to add entities to.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
			List<Entity> output,
			QueryDescription? query = null
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
			return Collect<
				ListCollectorEntityOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8, T9
			>(
				new ListCollectorEntityOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(output)
			);
		}

		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorWithComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
			: IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
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
			private readonly List<(Entity, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9)> _list;

            public ListCollectorWithComponents(List<(Entity, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9)> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9)
            {
                _list.Add((e, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9));
            }

            public void AddCapacity(int capacity)
            {
                _list.Capacity += capacity;
            }
		}

		/// <summary>
		/// Collect all entities which match the query into a list
		/// </summary>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		/// <typeparam name="T8">Component 8 to include in query</typeparam>
		/// <typeparam name="T9">Component 9 to include in query</typeparam>
		/// <param name="output">The list to add entities to.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
			List<(Entity, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9)> output,
			QueryDescription? query = null
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
			return Collect<
				ListCollectorWithComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8, T9
			>(
				new ListCollectorWithComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(output)
			);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		/// <typeparam name="T8">Component 8 to include in query</typeparam>
		/// <typeparam name="T9">Component 9 to include in query</typeparam>
		/// <param name="q">The instance to execute over every entity.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
			TQ q,
			QueryDescription? query = null
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
			where TQ : IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
		{
			var ca = new CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(q);
			return ExecuteChunk
			<
				CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8, T9
			>(
				ca,
				query
			);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		/// <typeparam name="T8">Component 8 to include in query</typeparam>
		/// <typeparam name="T9">Component 9 to include in query</typeparam>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <param name="q">The instance to execute over every entity.</param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
			TQ q,
			ref QueryDescription? query
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
			where TQ : IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
		{
			return ExecuteChunk
			<
				CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8, T9
			>(
				new CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(q),
				ref query
			);
		}
	}
}
namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A query which collects entities
	/// </summary>
	public interface IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
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
		/// <summary>
		/// Execute the query for a single entity
		/// </summary>
		public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10);

		/// <summary>
		/// Expand the collection
		/// </summary>
		public void AddCapacity(int capacity);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		[ExcludeFromCodeCoverage]
		private readonly struct CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
			: IChunkQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
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
			where TQ : IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
		{
			private readonly TQ _collector;

            public CollectorAdapter(TQ collector)
            {
                _collector = collector;
            }

            public void Execute(ChunkHandle chunk, ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10)
            {
                _collector.AddCapacity(e.Length);

                for (var i = e.Length - 1; i >= 0; i--)
                    _collector.Add(e[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i]);
            }
		}

		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorEntityOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
			: IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
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
			private readonly List<Entity> _list;

            public ListCollectorEntityOnly(List<Entity> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10)
            {
                _list.Add(e);
            }

            public void AddCapacity(int capacity)
            {
                _list.Capacity += capacity;
            }
		}

		/// <summary>
		/// Collect all entities which match the query into a list
		/// </summary>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		/// <typeparam name="T8">Component 8 to include in query</typeparam>
		/// <typeparam name="T9">Component 9 to include in query</typeparam>
		/// <typeparam name="T10">Component 10 to include in query</typeparam>
		/// <param name="output">The list to add entities to.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
			List<Entity> output,
			QueryDescription? query = null
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
			return Collect<
				ListCollectorEntityOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10
			>(
				new ListCollectorEntityOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(output)
			);
		}

		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorWithComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
			: IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
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
			private readonly List<(Entity, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> _list;

            public ListCollectorWithComponents(List<(Entity, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10)
            {
                _list.Add((e, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10));
            }

            public void AddCapacity(int capacity)
            {
                _list.Capacity += capacity;
            }
		}

		/// <summary>
		/// Collect all entities which match the query into a list
		/// </summary>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		/// <typeparam name="T8">Component 8 to include in query</typeparam>
		/// <typeparam name="T9">Component 9 to include in query</typeparam>
		/// <typeparam name="T10">Component 10 to include in query</typeparam>
		/// <param name="output">The list to add entities to.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
			List<(Entity, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> output,
			QueryDescription? query = null
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
			return Collect<
				ListCollectorWithComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10
			>(
				new ListCollectorWithComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(output)
			);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		/// <typeparam name="T8">Component 8 to include in query</typeparam>
		/// <typeparam name="T9">Component 9 to include in query</typeparam>
		/// <typeparam name="T10">Component 10 to include in query</typeparam>
		/// <param name="q">The instance to execute over every entity.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
			TQ q,
			QueryDescription? query = null
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
			where TQ : IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
		{
			var ca = new CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(q);
			return ExecuteChunk
			<
				CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10
			>(
				ca,
				query
			);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		/// <typeparam name="T8">Component 8 to include in query</typeparam>
		/// <typeparam name="T9">Component 9 to include in query</typeparam>
		/// <typeparam name="T10">Component 10 to include in query</typeparam>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <param name="q">The instance to execute over every entity.</param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
			TQ q,
			ref QueryDescription? query
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
			where TQ : IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
		{
			return ExecuteChunk
			<
				CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10
			>(
				new CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(q),
				ref query
			);
		}
	}
}
namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A query which collects entities
	/// </summary>
	public interface IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
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
		/// <summary>
		/// Execute the query for a single entity
		/// </summary>
		public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11);

		/// <summary>
		/// Expand the collection
		/// </summary>
		public void AddCapacity(int capacity);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		[ExcludeFromCodeCoverage]
		private readonly struct CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
			: IChunkQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
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
			where TQ : IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
		{
			private readonly TQ _collector;

            public CollectorAdapter(TQ collector)
            {
                _collector = collector;
            }

            public void Execute(ChunkHandle chunk, ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10, Span<T11> t11)
            {
                _collector.AddCapacity(e.Length);

                for (var i = e.Length - 1; i >= 0; i--)
                    _collector.Add(e[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i]);
            }
		}

		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorEntityOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
			: IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
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
			private readonly List<Entity> _list;

            public ListCollectorEntityOnly(List<Entity> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11)
            {
                _list.Add(e);
            }

            public void AddCapacity(int capacity)
            {
                _list.Capacity += capacity;
            }
		}

		/// <summary>
		/// Collect all entities which match the query into a list
		/// </summary>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		/// <typeparam name="T8">Component 8 to include in query</typeparam>
		/// <typeparam name="T9">Component 9 to include in query</typeparam>
		/// <typeparam name="T10">Component 10 to include in query</typeparam>
		/// <typeparam name="T11">Component 11 to include in query</typeparam>
		/// <param name="output">The list to add entities to.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
			List<Entity> output,
			QueryDescription? query = null
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
			return Collect<
				ListCollectorEntityOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11
			>(
				new ListCollectorEntityOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(output)
			);
		}

		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorWithComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
			: IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
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
			private readonly List<(Entity, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> _list;

            public ListCollectorWithComponents(List<(Entity, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11)
            {
                _list.Add((e, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11));
            }

            public void AddCapacity(int capacity)
            {
                _list.Capacity += capacity;
            }
		}

		/// <summary>
		/// Collect all entities which match the query into a list
		/// </summary>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		/// <typeparam name="T8">Component 8 to include in query</typeparam>
		/// <typeparam name="T9">Component 9 to include in query</typeparam>
		/// <typeparam name="T10">Component 10 to include in query</typeparam>
		/// <typeparam name="T11">Component 11 to include in query</typeparam>
		/// <param name="output">The list to add entities to.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
			List<(Entity, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> output,
			QueryDescription? query = null
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
			return Collect<
				ListCollectorWithComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11
			>(
				new ListCollectorWithComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(output)
			);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		/// <typeparam name="T8">Component 8 to include in query</typeparam>
		/// <typeparam name="T9">Component 9 to include in query</typeparam>
		/// <typeparam name="T10">Component 10 to include in query</typeparam>
		/// <typeparam name="T11">Component 11 to include in query</typeparam>
		/// <param name="q">The instance to execute over every entity.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
			TQ q,
			QueryDescription? query = null
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
			where TQ : IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
		{
			var ca = new CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(q);
			return ExecuteChunk
			<
				CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11
			>(
				ca,
				query
			);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		/// <typeparam name="T8">Component 8 to include in query</typeparam>
		/// <typeparam name="T9">Component 9 to include in query</typeparam>
		/// <typeparam name="T10">Component 10 to include in query</typeparam>
		/// <typeparam name="T11">Component 11 to include in query</typeparam>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <param name="q">The instance to execute over every entity.</param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
			TQ q,
			ref QueryDescription? query
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
			where TQ : IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
		{
			return ExecuteChunk
			<
				CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11
			>(
				new CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(q),
				ref query
			);
		}
	}
}
namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A query which collects entities
	/// </summary>
	public interface IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
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
		/// <summary>
		/// Execute the query for a single entity
		/// </summary>
		public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12);

		/// <summary>
		/// Expand the collection
		/// </summary>
		public void AddCapacity(int capacity);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		[ExcludeFromCodeCoverage]
		private readonly struct CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
			: IChunkQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
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
			where TQ : IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
		{
			private readonly TQ _collector;

            public CollectorAdapter(TQ collector)
            {
                _collector = collector;
            }

            public void Execute(ChunkHandle chunk, ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10, Span<T11> t11, Span<T12> t12)
            {
                _collector.AddCapacity(e.Length);

                for (var i = e.Length - 1; i >= 0; i--)
                    _collector.Add(e[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i]);
            }
		}

		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorEntityOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
			: IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
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
			private readonly List<Entity> _list;

            public ListCollectorEntityOnly(List<Entity> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12)
            {
                _list.Add(e);
            }

            public void AddCapacity(int capacity)
            {
                _list.Capacity += capacity;
            }
		}

		/// <summary>
		/// Collect all entities which match the query into a list
		/// </summary>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		/// <typeparam name="T8">Component 8 to include in query</typeparam>
		/// <typeparam name="T9">Component 9 to include in query</typeparam>
		/// <typeparam name="T10">Component 10 to include in query</typeparam>
		/// <typeparam name="T11">Component 11 to include in query</typeparam>
		/// <typeparam name="T12">Component 12 to include in query</typeparam>
		/// <param name="output">The list to add entities to.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
			List<Entity> output,
			QueryDescription? query = null
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
			return Collect<
				ListCollectorEntityOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12
			>(
				new ListCollectorEntityOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(output)
			);
		}

		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorWithComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
			: IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
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
			private readonly List<(Entity, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> _list;

            public ListCollectorWithComponents(List<(Entity, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12)
            {
                _list.Add((e, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12));
            }

            public void AddCapacity(int capacity)
            {
                _list.Capacity += capacity;
            }
		}

		/// <summary>
		/// Collect all entities which match the query into a list
		/// </summary>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		/// <typeparam name="T8">Component 8 to include in query</typeparam>
		/// <typeparam name="T9">Component 9 to include in query</typeparam>
		/// <typeparam name="T10">Component 10 to include in query</typeparam>
		/// <typeparam name="T11">Component 11 to include in query</typeparam>
		/// <typeparam name="T12">Component 12 to include in query</typeparam>
		/// <param name="output">The list to add entities to.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
			List<(Entity, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> output,
			QueryDescription? query = null
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
			return Collect<
				ListCollectorWithComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12
			>(
				new ListCollectorWithComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(output)
			);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		/// <typeparam name="T8">Component 8 to include in query</typeparam>
		/// <typeparam name="T9">Component 9 to include in query</typeparam>
		/// <typeparam name="T10">Component 10 to include in query</typeparam>
		/// <typeparam name="T11">Component 11 to include in query</typeparam>
		/// <typeparam name="T12">Component 12 to include in query</typeparam>
		/// <param name="q">The instance to execute over every entity.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
			TQ q,
			QueryDescription? query = null
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
			where TQ : IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
		{
			var ca = new CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(q);
			return ExecuteChunk
			<
				CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12
			>(
				ca,
				query
			);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		/// <typeparam name="T8">Component 8 to include in query</typeparam>
		/// <typeparam name="T9">Component 9 to include in query</typeparam>
		/// <typeparam name="T10">Component 10 to include in query</typeparam>
		/// <typeparam name="T11">Component 11 to include in query</typeparam>
		/// <typeparam name="T12">Component 12 to include in query</typeparam>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <param name="q">The instance to execute over every entity.</param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
			TQ q,
			ref QueryDescription? query
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
			where TQ : IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
		{
			return ExecuteChunk
			<
				CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12
			>(
				new CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(q),
				ref query
			);
		}
	}
}
namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A query which collects entities
	/// </summary>
	public interface IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
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
		/// <summary>
		/// Execute the query for a single entity
		/// </summary>
		public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12, ref T13 t13);

		/// <summary>
		/// Expand the collection
		/// </summary>
		public void AddCapacity(int capacity);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		[ExcludeFromCodeCoverage]
		private readonly struct CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
			: IChunkQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
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
			where TQ : IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
		{
			private readonly TQ _collector;

            public CollectorAdapter(TQ collector)
            {
                _collector = collector;
            }

            public void Execute(ChunkHandle chunk, ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10, Span<T11> t11, Span<T12> t12, Span<T13> t13)
            {
                _collector.AddCapacity(e.Length);

                for (var i = e.Length - 1; i >= 0; i--)
                    _collector.Add(e[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i], ref t13[i]);
            }
		}

		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorEntityOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
			: IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
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
			private readonly List<Entity> _list;

            public ListCollectorEntityOnly(List<Entity> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12, ref T13 t13)
            {
                _list.Add(e);
            }

            public void AddCapacity(int capacity)
            {
                _list.Capacity += capacity;
            }
		}

		/// <summary>
		/// Collect all entities which match the query into a list
		/// </summary>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		/// <typeparam name="T8">Component 8 to include in query</typeparam>
		/// <typeparam name="T9">Component 9 to include in query</typeparam>
		/// <typeparam name="T10">Component 10 to include in query</typeparam>
		/// <typeparam name="T11">Component 11 to include in query</typeparam>
		/// <typeparam name="T12">Component 12 to include in query</typeparam>
		/// <typeparam name="T13">Component 13 to include in query</typeparam>
		/// <param name="output">The list to add entities to.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
			List<Entity> output,
			QueryDescription? query = null
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
			return Collect<
				ListCollectorEntityOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13
			>(
				new ListCollectorEntityOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(output)
			);
		}

		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorWithComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
			: IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
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
			private readonly List<(Entity, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)> _list;

            public ListCollectorWithComponents(List<(Entity, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12, ref T13 t13)
            {
                _list.Add((e, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13));
            }

            public void AddCapacity(int capacity)
            {
                _list.Capacity += capacity;
            }
		}

		/// <summary>
		/// Collect all entities which match the query into a list
		/// </summary>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		/// <typeparam name="T8">Component 8 to include in query</typeparam>
		/// <typeparam name="T9">Component 9 to include in query</typeparam>
		/// <typeparam name="T10">Component 10 to include in query</typeparam>
		/// <typeparam name="T11">Component 11 to include in query</typeparam>
		/// <typeparam name="T12">Component 12 to include in query</typeparam>
		/// <typeparam name="T13">Component 13 to include in query</typeparam>
		/// <param name="output">The list to add entities to.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
			List<(Entity, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)> output,
			QueryDescription? query = null
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
			return Collect<
				ListCollectorWithComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13
			>(
				new ListCollectorWithComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(output)
			);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		/// <typeparam name="T8">Component 8 to include in query</typeparam>
		/// <typeparam name="T9">Component 9 to include in query</typeparam>
		/// <typeparam name="T10">Component 10 to include in query</typeparam>
		/// <typeparam name="T11">Component 11 to include in query</typeparam>
		/// <typeparam name="T12">Component 12 to include in query</typeparam>
		/// <typeparam name="T13">Component 13 to include in query</typeparam>
		/// <param name="q">The instance to execute over every entity.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
			TQ q,
			QueryDescription? query = null
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
			where TQ : IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
		{
			var ca = new CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(q);
			return ExecuteChunk
			<
				CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13
			>(
				ca,
				query
			);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		/// <typeparam name="T8">Component 8 to include in query</typeparam>
		/// <typeparam name="T9">Component 9 to include in query</typeparam>
		/// <typeparam name="T10">Component 10 to include in query</typeparam>
		/// <typeparam name="T11">Component 11 to include in query</typeparam>
		/// <typeparam name="T12">Component 12 to include in query</typeparam>
		/// <typeparam name="T13">Component 13 to include in query</typeparam>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <param name="q">The instance to execute over every entity.</param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
			TQ q,
			ref QueryDescription? query
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
			where TQ : IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
		{
			return ExecuteChunk
			<
				CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13
			>(
				new CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(q),
				ref query
			);
		}
	}
}
namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A query which collects entities
	/// </summary>
	public interface IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
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
		/// <summary>
		/// Execute the query for a single entity
		/// </summary>
		public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12, ref T13 t13, ref T14 t14);

		/// <summary>
		/// Expand the collection
		/// </summary>
		public void AddCapacity(int capacity);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		[ExcludeFromCodeCoverage]
		private readonly struct CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
			: IChunkQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
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
			where TQ : IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
		{
			private readonly TQ _collector;

            public CollectorAdapter(TQ collector)
            {
                _collector = collector;
            }

            public void Execute(ChunkHandle chunk, ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10, Span<T11> t11, Span<T12> t12, Span<T13> t13, Span<T14> t14)
            {
                _collector.AddCapacity(e.Length);

                for (var i = e.Length - 1; i >= 0; i--)
                    _collector.Add(e[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i], ref t13[i], ref t14[i]);
            }
		}

		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorEntityOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
			: IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
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
			private readonly List<Entity> _list;

            public ListCollectorEntityOnly(List<Entity> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12, ref T13 t13, ref T14 t14)
            {
                _list.Add(e);
            }

            public void AddCapacity(int capacity)
            {
                _list.Capacity += capacity;
            }
		}

		/// <summary>
		/// Collect all entities which match the query into a list
		/// </summary>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		/// <typeparam name="T8">Component 8 to include in query</typeparam>
		/// <typeparam name="T9">Component 9 to include in query</typeparam>
		/// <typeparam name="T10">Component 10 to include in query</typeparam>
		/// <typeparam name="T11">Component 11 to include in query</typeparam>
		/// <typeparam name="T12">Component 12 to include in query</typeparam>
		/// <typeparam name="T13">Component 13 to include in query</typeparam>
		/// <typeparam name="T14">Component 14 to include in query</typeparam>
		/// <param name="output">The list to add entities to.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
			List<Entity> output,
			QueryDescription? query = null
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
			return Collect<
				ListCollectorEntityOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14
			>(
				new ListCollectorEntityOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(output)
			);
		}

		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorWithComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
			: IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
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
			private readonly List<(Entity, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)> _list;

            public ListCollectorWithComponents(List<(Entity, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12, ref T13 t13, ref T14 t14)
            {
                _list.Add((e, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14));
            }

            public void AddCapacity(int capacity)
            {
                _list.Capacity += capacity;
            }
		}

		/// <summary>
		/// Collect all entities which match the query into a list
		/// </summary>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		/// <typeparam name="T8">Component 8 to include in query</typeparam>
		/// <typeparam name="T9">Component 9 to include in query</typeparam>
		/// <typeparam name="T10">Component 10 to include in query</typeparam>
		/// <typeparam name="T11">Component 11 to include in query</typeparam>
		/// <typeparam name="T12">Component 12 to include in query</typeparam>
		/// <typeparam name="T13">Component 13 to include in query</typeparam>
		/// <typeparam name="T14">Component 14 to include in query</typeparam>
		/// <param name="output">The list to add entities to.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
			List<(Entity, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)> output,
			QueryDescription? query = null
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
			return Collect<
				ListCollectorWithComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14
			>(
				new ListCollectorWithComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(output)
			);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		/// <typeparam name="T8">Component 8 to include in query</typeparam>
		/// <typeparam name="T9">Component 9 to include in query</typeparam>
		/// <typeparam name="T10">Component 10 to include in query</typeparam>
		/// <typeparam name="T11">Component 11 to include in query</typeparam>
		/// <typeparam name="T12">Component 12 to include in query</typeparam>
		/// <typeparam name="T13">Component 13 to include in query</typeparam>
		/// <typeparam name="T14">Component 14 to include in query</typeparam>
		/// <param name="q">The instance to execute over every entity.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
			TQ q,
			QueryDescription? query = null
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
			where TQ : IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
		{
			var ca = new CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(q);
			return ExecuteChunk
			<
				CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14
			>(
				ca,
				query
			);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		/// <typeparam name="T8">Component 8 to include in query</typeparam>
		/// <typeparam name="T9">Component 9 to include in query</typeparam>
		/// <typeparam name="T10">Component 10 to include in query</typeparam>
		/// <typeparam name="T11">Component 11 to include in query</typeparam>
		/// <typeparam name="T12">Component 12 to include in query</typeparam>
		/// <typeparam name="T13">Component 13 to include in query</typeparam>
		/// <typeparam name="T14">Component 14 to include in query</typeparam>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <param name="q">The instance to execute over every entity.</param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
			TQ q,
			ref QueryDescription? query
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
			where TQ : IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
		{
			return ExecuteChunk
			<
				CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14
			>(
				new CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(q),
				ref query
			);
		}
	}
}
namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A query which collects entities
	/// </summary>
	public interface IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
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
		/// <summary>
		/// Execute the query for a single entity
		/// </summary>
		public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12, ref T13 t13, ref T14 t14, ref T15 t15);

		/// <summary>
		/// Expand the collection
		/// </summary>
		public void AddCapacity(int capacity);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		[ExcludeFromCodeCoverage]
		private readonly struct CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
			: IChunkQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
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
			where TQ : IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
		{
			private readonly TQ _collector;

            public CollectorAdapter(TQ collector)
            {
                _collector = collector;
            }

            public void Execute(ChunkHandle chunk, ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10, Span<T11> t11, Span<T12> t12, Span<T13> t13, Span<T14> t14, Span<T15> t15)
            {
                _collector.AddCapacity(e.Length);

                for (var i = e.Length - 1; i >= 0; i--)
                    _collector.Add(e[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i], ref t13[i], ref t14[i], ref t15[i]);
            }
		}

		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorEntityOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
			: IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
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
			private readonly List<Entity> _list;

            public ListCollectorEntityOnly(List<Entity> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12, ref T13 t13, ref T14 t14, ref T15 t15)
            {
                _list.Add(e);
            }

            public void AddCapacity(int capacity)
            {
                _list.Capacity += capacity;
            }
		}

		/// <summary>
		/// Collect all entities which match the query into a list
		/// </summary>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		/// <typeparam name="T8">Component 8 to include in query</typeparam>
		/// <typeparam name="T9">Component 9 to include in query</typeparam>
		/// <typeparam name="T10">Component 10 to include in query</typeparam>
		/// <typeparam name="T11">Component 11 to include in query</typeparam>
		/// <typeparam name="T12">Component 12 to include in query</typeparam>
		/// <typeparam name="T13">Component 13 to include in query</typeparam>
		/// <typeparam name="T14">Component 14 to include in query</typeparam>
		/// <typeparam name="T15">Component 15 to include in query</typeparam>
		/// <param name="output">The list to add entities to.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
			List<Entity> output,
			QueryDescription? query = null
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
			return Collect<
				ListCollectorEntityOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15
			>(
				new ListCollectorEntityOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(output)
			);
		}

		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorWithComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
			: IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
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
			private readonly List<(Entity, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)> _list;

            public ListCollectorWithComponents(List<(Entity, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12, ref T13 t13, ref T14 t14, ref T15 t15)
            {
                _list.Add((e, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15));
            }

            public void AddCapacity(int capacity)
            {
                _list.Capacity += capacity;
            }
		}

		/// <summary>
		/// Collect all entities which match the query into a list
		/// </summary>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		/// <typeparam name="T8">Component 8 to include in query</typeparam>
		/// <typeparam name="T9">Component 9 to include in query</typeparam>
		/// <typeparam name="T10">Component 10 to include in query</typeparam>
		/// <typeparam name="T11">Component 11 to include in query</typeparam>
		/// <typeparam name="T12">Component 12 to include in query</typeparam>
		/// <typeparam name="T13">Component 13 to include in query</typeparam>
		/// <typeparam name="T14">Component 14 to include in query</typeparam>
		/// <typeparam name="T15">Component 15 to include in query</typeparam>
		/// <param name="output">The list to add entities to.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
			List<(Entity, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)> output,
			QueryDescription? query = null
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
			return Collect<
				ListCollectorWithComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15
			>(
				new ListCollectorWithComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(output)
			);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		/// <typeparam name="T8">Component 8 to include in query</typeparam>
		/// <typeparam name="T9">Component 9 to include in query</typeparam>
		/// <typeparam name="T10">Component 10 to include in query</typeparam>
		/// <typeparam name="T11">Component 11 to include in query</typeparam>
		/// <typeparam name="T12">Component 12 to include in query</typeparam>
		/// <typeparam name="T13">Component 13 to include in query</typeparam>
		/// <typeparam name="T14">Component 14 to include in query</typeparam>
		/// <typeparam name="T15">Component 15 to include in query</typeparam>
		/// <param name="q">The instance to execute over every entity.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
			TQ q,
			QueryDescription? query = null
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
			where TQ : IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
		{
			var ca = new CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(q);
			return ExecuteChunk
			<
				CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15
			>(
				ca,
				query
			);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		/// <typeparam name="T8">Component 8 to include in query</typeparam>
		/// <typeparam name="T9">Component 9 to include in query</typeparam>
		/// <typeparam name="T10">Component 10 to include in query</typeparam>
		/// <typeparam name="T11">Component 11 to include in query</typeparam>
		/// <typeparam name="T12">Component 12 to include in query</typeparam>
		/// <typeparam name="T13">Component 13 to include in query</typeparam>
		/// <typeparam name="T14">Component 14 to include in query</typeparam>
		/// <typeparam name="T15">Component 15 to include in query</typeparam>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <param name="q">The instance to execute over every entity.</param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Collect<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
			TQ q,
			ref QueryDescription? query
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
			where TQ : IQueryCollector<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
		{
			return ExecuteChunk
			<
				CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15
			>(
				new CollectorAdapter<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(q),
				ref query
			);
		}
	}
}


