using Myriad.ECS.Queries;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedParameter.Global
// ReSharper disable LoopCanBeConvertedToQuery
// ReSharper disable ConvertToPrimaryConstructor

namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A delegate called for entities in a query. Receives just the components requested.
	/// </summary>
	public delegate void QueryDelegateChunk<T0>(
		Span<T0> t0
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity and the components requested.
	/// </summary>
	public delegate void QueryDelegateChunkHandle<T0>(
		ChunkHandle chunk,
		Span<T0> t0
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateChunkData<in TData, T0>(
		TData data,
		Span<T0> t0
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity, a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateChunkHandleData<in TData, T0>(
		TData data,
		ChunkHandle chunk,
		Span<T0> t0
	);

	
	internal readonly struct QueryDelegateChunkWrapper1E<T0>
		: IChunkQuery<T0>
		where T0 : IComponent
	{
		private readonly QueryDelegateChunkHandle<T0> _delegate;

		public QueryDelegateChunkWrapper1E(QueryDelegateChunkHandle<T0> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0)
		{
			_delegate(chunk, t0);
		}
	}

	
	internal readonly struct QueryDelegateChunkWrapper1<T0>
		: IChunkQuery<T0>
		where T0 : IComponent
	{
		private readonly QueryDelegateChunk<T0> _delegate;

		public QueryDelegateChunkWrapper1(QueryDelegateChunk<T0> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0)
		{
			_delegate(t0);
		}
	}

	
	internal readonly struct QueryDelegateChunkWrapper1ED<TData, T0>
		: IChunkQuery<T0>
		where T0 : IComponent
	{
		private readonly QueryDelegateChunkHandleData<TData, T0> _delegate;
		private readonly TData _data;

		public QueryDelegateChunkWrapper1ED(TData data, QueryDelegateChunkHandleData<TData, T0> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0)
		{
			_delegate(_data, chunk, t0);
		}
	}

	
	internal readonly struct QueryDelegateChunkWrapper1D<TData, T0>
		: IChunkQuery<T0>
		where T0 : IComponent
	{
		private readonly QueryDelegateChunkData<TData, T0> _delegate;
		private readonly TData _data;

		public QueryDelegateChunkWrapper1D(TData data, QueryDelegateChunkData<TData, T0> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0)
		{
			_delegate(_data, t0);
		}
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0>(QueryDelegateChunkHandle<T0> @delegate, ref QueryDescription? query)
			where T0 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper1E<T0>, T0>(
				new QueryDelegateChunkWrapper1E<T0>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0>(QueryDelegateChunkHandle<T0> @delegate, QueryDescription? query = null)
			where T0 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper1E<T0>, T0>(
				new QueryDelegateChunkWrapper1E<T0>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<T0>(QueryDelegateChunkHandle<T0> @delegate, QueryDescription? query = null)
			where T0 : IComponent
		{
			return ExecuteChunkParallel<QueryDelegateChunkWrapper1E<T0>, T0>(
				new QueryDelegateChunkWrapper1E<T0>(@delegate),
				query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0>(QueryDelegateChunk<T0> @delegate, ref QueryDescription? query)
			where T0 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper1<T0>, T0>(
				new QueryDelegateChunkWrapper1<T0>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0>(QueryDelegateChunk<T0> @delegate, QueryDescription? query = null)
			where T0 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper1<T0>, T0>(
				new QueryDelegateChunkWrapper1<T0>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<T0>(QueryDelegateChunk<T0> @delegate, QueryDescription? query = null)
			where T0 : IComponent
		{
			return ExecuteChunkParallel<QueryDelegateChunkWrapper1<T0>, T0>(
				new QueryDelegateChunkWrapper1<T0>(@delegate),
				query
			);
		}

		// vv-- TData variants --vv //

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0>(TData data, QueryDelegateChunkHandleData<TData, T0> @delegate, ref QueryDescription? query)
			where T0 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper1ED<TData, T0>, T0>(
				new QueryDelegateChunkWrapper1ED<TData, T0>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0>(TData data, QueryDelegateChunkHandleData<TData, T0> @delegate, QueryDescription? query = null)
			where T0 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper1ED<TData, T0>, T0>(
				new QueryDelegateChunkWrapper1ED<TData, T0>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<TData, T0>(TData data, QueryDelegateChunkHandleData<TData, T0> @delegate, QueryDescription? query = null)
			where T0 : IComponent
		{
			return ExecuteChunkParallel<QueryDelegateChunkWrapper1ED<TData, T0>, T0>(
				new QueryDelegateChunkWrapper1ED<TData, T0>(data, @delegate),
				query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0>(TData data, QueryDelegateChunkData<TData, T0> @delegate, ref QueryDescription? query)
			where T0 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper1D<TData, T0>, T0>(
				new QueryDelegateChunkWrapper1D<TData, T0>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0>(TData data, QueryDelegateChunkData<TData, T0> @delegate, QueryDescription? query = null)
			where T0 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper1D<TData, T0>, T0>(
				new QueryDelegateChunkWrapper1D<TData, T0>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<TData, T0>(TData data, QueryDelegateChunkData<TData, T0> @delegate, QueryDescription? query = null)
			where T0 : IComponent
		{
			return ExecuteChunkParallel<QueryDelegateChunkWrapper1D<TData, T0>, T0>(
				new QueryDelegateChunkWrapper1D<TData, T0>(data, @delegate),
				query
			);
		}
	}
}

namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A delegate called for entities in a query. Receives just the components requested.
	/// </summary>
	public delegate void QueryDelegateChunk<T0, T1>(
		Span<T0> t0, Span<T1> t1
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity and the components requested.
	/// </summary>
	public delegate void QueryDelegateChunkHandle<T0, T1>(
		ChunkHandle chunk,
		Span<T0> t0, Span<T1> t1
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateChunkData<in TData, T0, T1>(
		TData data,
		Span<T0> t0, Span<T1> t1
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity, a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateChunkHandleData<in TData, T0, T1>(
		TData data,
		ChunkHandle chunk,
		Span<T0> t0, Span<T1> t1
	);

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper2E<T0, T1>
		: IChunkQuery<T0, T1>
		where T0 : IComponent
            where T1 : IComponent
	{
		private readonly QueryDelegateChunkHandle<T0, T1> _delegate;

		public QueryDelegateChunkWrapper2E(QueryDelegateChunkHandle<T0, T1> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1)
		{
			_delegate(chunk, t0, t1);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper2<T0, T1>
		: IChunkQuery<T0, T1>
		where T0 : IComponent
            where T1 : IComponent
	{
		private readonly QueryDelegateChunk<T0, T1> _delegate;

		public QueryDelegateChunkWrapper2(QueryDelegateChunk<T0, T1> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1)
		{
			_delegate(t0, t1);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper2ED<TData, T0, T1>
		: IChunkQuery<T0, T1>
		where T0 : IComponent
            where T1 : IComponent
	{
		private readonly QueryDelegateChunkHandleData<TData, T0, T1> _delegate;
		private readonly TData _data;

		public QueryDelegateChunkWrapper2ED(TData data, QueryDelegateChunkHandleData<TData, T0, T1> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1)
		{
			_delegate(_data, chunk, t0, t1);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper2D<TData, T0, T1>
		: IChunkQuery<T0, T1>
		where T0 : IComponent
            where T1 : IComponent
	{
		private readonly QueryDelegateChunkData<TData, T0, T1> _delegate;
		private readonly TData _data;

		public QueryDelegateChunkWrapper2D(TData data, QueryDelegateChunkData<TData, T0, T1> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1)
		{
			_delegate(_data, t0, t1);
		}
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1>(QueryDelegateChunkHandle<T0, T1> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper2E<T0, T1>, T0, T1>(
				new QueryDelegateChunkWrapper2E<T0, T1>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1>(QueryDelegateChunkHandle<T0, T1> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper2E<T0, T1>, T0, T1>(
				new QueryDelegateChunkWrapper2E<T0, T1>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<T0, T1>(QueryDelegateChunkHandle<T0, T1> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
		{
			return ExecuteChunkParallel<QueryDelegateChunkWrapper2E<T0, T1>, T0, T1>(
				new QueryDelegateChunkWrapper2E<T0, T1>(@delegate),
				query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1>(QueryDelegateChunk<T0, T1> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper2<T0, T1>, T0, T1>(
				new QueryDelegateChunkWrapper2<T0, T1>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1>(QueryDelegateChunk<T0, T1> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper2<T0, T1>, T0, T1>(
				new QueryDelegateChunkWrapper2<T0, T1>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<T0, T1>(QueryDelegateChunk<T0, T1> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
		{
			return ExecuteChunkParallel<QueryDelegateChunkWrapper2<T0, T1>, T0, T1>(
				new QueryDelegateChunkWrapper2<T0, T1>(@delegate),
				query
			);
		}

		// vv-- TData variants --vv //

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1>(TData data, QueryDelegateChunkHandleData<TData, T0, T1> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper2ED<TData, T0, T1>, T0, T1>(
				new QueryDelegateChunkWrapper2ED<TData, T0, T1>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1>(TData data, QueryDelegateChunkHandleData<TData, T0, T1> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper2ED<TData, T0, T1>, T0, T1>(
				new QueryDelegateChunkWrapper2ED<TData, T0, T1>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<TData, T0, T1>(TData data, QueryDelegateChunkHandleData<TData, T0, T1> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
		{
			return ExecuteChunkParallel<QueryDelegateChunkWrapper2ED<TData, T0, T1>, T0, T1>(
				new QueryDelegateChunkWrapper2ED<TData, T0, T1>(data, @delegate),
				query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1>(TData data, QueryDelegateChunkData<TData, T0, T1> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper2D<TData, T0, T1>, T0, T1>(
				new QueryDelegateChunkWrapper2D<TData, T0, T1>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1>(TData data, QueryDelegateChunkData<TData, T0, T1> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper2D<TData, T0, T1>, T0, T1>(
				new QueryDelegateChunkWrapper2D<TData, T0, T1>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<TData, T0, T1>(TData data, QueryDelegateChunkData<TData, T0, T1> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
		{
			return ExecuteChunkParallel<QueryDelegateChunkWrapper2D<TData, T0, T1>, T0, T1>(
				new QueryDelegateChunkWrapper2D<TData, T0, T1>(data, @delegate),
				query
			);
		}
	}
}

namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A delegate called for entities in a query. Receives just the components requested.
	/// </summary>
	public delegate void QueryDelegateChunk<T0, T1, T2>(
		Span<T0> t0, Span<T1> t1, Span<T2> t2
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity and the components requested.
	/// </summary>
	public delegate void QueryDelegateChunkHandle<T0, T1, T2>(
		ChunkHandle chunk,
		Span<T0> t0, Span<T1> t1, Span<T2> t2
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateChunkData<in TData, T0, T1, T2>(
		TData data,
		Span<T0> t0, Span<T1> t1, Span<T2> t2
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity, a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateChunkHandleData<in TData, T0, T1, T2>(
		TData data,
		ChunkHandle chunk,
		Span<T0> t0, Span<T1> t1, Span<T2> t2
	);

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper3E<T0, T1, T2>
		: IChunkQuery<T0, T1, T2>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
	{
		private readonly QueryDelegateChunkHandle<T0, T1, T2> _delegate;

		public QueryDelegateChunkWrapper3E(QueryDelegateChunkHandle<T0, T1, T2> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2)
		{
			_delegate(chunk, t0, t1, t2);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper3<T0, T1, T2>
		: IChunkQuery<T0, T1, T2>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
	{
		private readonly QueryDelegateChunk<T0, T1, T2> _delegate;

		public QueryDelegateChunkWrapper3(QueryDelegateChunk<T0, T1, T2> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2)
		{
			_delegate(t0, t1, t2);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper3ED<TData, T0, T1, T2>
		: IChunkQuery<T0, T1, T2>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
	{
		private readonly QueryDelegateChunkHandleData<TData, T0, T1, T2> _delegate;
		private readonly TData _data;

		public QueryDelegateChunkWrapper3ED(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2)
		{
			_delegate(_data, chunk, t0, t1, t2);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper3D<TData, T0, T1, T2>
		: IChunkQuery<T0, T1, T2>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
	{
		private readonly QueryDelegateChunkData<TData, T0, T1, T2> _delegate;
		private readonly TData _data;

		public QueryDelegateChunkWrapper3D(TData data, QueryDelegateChunkData<TData, T0, T1, T2> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2)
		{
			_delegate(_data, t0, t1, t2);
		}
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2>(QueryDelegateChunkHandle<T0, T1, T2> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper3E<T0, T1, T2>, T0, T1, T2>(
				new QueryDelegateChunkWrapper3E<T0, T1, T2>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2>(QueryDelegateChunkHandle<T0, T1, T2> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper3E<T0, T1, T2>, T0, T1, T2>(
				new QueryDelegateChunkWrapper3E<T0, T1, T2>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<T0, T1, T2>(QueryDelegateChunkHandle<T0, T1, T2> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
		{
			return ExecuteChunkParallel<QueryDelegateChunkWrapper3E<T0, T1, T2>, T0, T1, T2>(
				new QueryDelegateChunkWrapper3E<T0, T1, T2>(@delegate),
				query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2>(QueryDelegateChunk<T0, T1, T2> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper3<T0, T1, T2>, T0, T1, T2>(
				new QueryDelegateChunkWrapper3<T0, T1, T2>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2>(QueryDelegateChunk<T0, T1, T2> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper3<T0, T1, T2>, T0, T1, T2>(
				new QueryDelegateChunkWrapper3<T0, T1, T2>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<T0, T1, T2>(QueryDelegateChunk<T0, T1, T2> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
		{
			return ExecuteChunkParallel<QueryDelegateChunkWrapper3<T0, T1, T2>, T0, T1, T2>(
				new QueryDelegateChunkWrapper3<T0, T1, T2>(@delegate),
				query
			);
		}

		// vv-- TData variants --vv //

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2>(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper3ED<TData, T0, T1, T2>, T0, T1, T2>(
				new QueryDelegateChunkWrapper3ED<TData, T0, T1, T2>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2>(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper3ED<TData, T0, T1, T2>, T0, T1, T2>(
				new QueryDelegateChunkWrapper3ED<TData, T0, T1, T2>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<TData, T0, T1, T2>(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
		{
			return ExecuteChunkParallel<QueryDelegateChunkWrapper3ED<TData, T0, T1, T2>, T0, T1, T2>(
				new QueryDelegateChunkWrapper3ED<TData, T0, T1, T2>(data, @delegate),
				query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2>(TData data, QueryDelegateChunkData<TData, T0, T1, T2> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper3D<TData, T0, T1, T2>, T0, T1, T2>(
				new QueryDelegateChunkWrapper3D<TData, T0, T1, T2>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2>(TData data, QueryDelegateChunkData<TData, T0, T1, T2> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper3D<TData, T0, T1, T2>, T0, T1, T2>(
				new QueryDelegateChunkWrapper3D<TData, T0, T1, T2>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<TData, T0, T1, T2>(TData data, QueryDelegateChunkData<TData, T0, T1, T2> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
		{
			return ExecuteChunkParallel<QueryDelegateChunkWrapper3D<TData, T0, T1, T2>, T0, T1, T2>(
				new QueryDelegateChunkWrapper3D<TData, T0, T1, T2>(data, @delegate),
				query
			);
		}
	}
}

namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A delegate called for entities in a query. Receives just the components requested.
	/// </summary>
	public delegate void QueryDelegateChunk<T0, T1, T2, T3>(
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity and the components requested.
	/// </summary>
	public delegate void QueryDelegateChunkHandle<T0, T1, T2, T3>(
		ChunkHandle chunk,
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateChunkData<in TData, T0, T1, T2, T3>(
		TData data,
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity, a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateChunkHandleData<in TData, T0, T1, T2, T3>(
		TData data,
		ChunkHandle chunk,
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3
	);

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper4E<T0, T1, T2, T3>
		: IChunkQuery<T0, T1, T2, T3>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
	{
		private readonly QueryDelegateChunkHandle<T0, T1, T2, T3> _delegate;

		public QueryDelegateChunkWrapper4E(QueryDelegateChunkHandle<T0, T1, T2, T3> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3)
		{
			_delegate(chunk, t0, t1, t2, t3);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper4<T0, T1, T2, T3>
		: IChunkQuery<T0, T1, T2, T3>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
	{
		private readonly QueryDelegateChunk<T0, T1, T2, T3> _delegate;

		public QueryDelegateChunkWrapper4(QueryDelegateChunk<T0, T1, T2, T3> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3)
		{
			_delegate(t0, t1, t2, t3);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper4ED<TData, T0, T1, T2, T3>
		: IChunkQuery<T0, T1, T2, T3>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
	{
		private readonly QueryDelegateChunkHandleData<TData, T0, T1, T2, T3> _delegate;
		private readonly TData _data;

		public QueryDelegateChunkWrapper4ED(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3)
		{
			_delegate(_data, chunk, t0, t1, t2, t3);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper4D<TData, T0, T1, T2, T3>
		: IChunkQuery<T0, T1, T2, T3>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
	{
		private readonly QueryDelegateChunkData<TData, T0, T1, T2, T3> _delegate;
		private readonly TData _data;

		public QueryDelegateChunkWrapper4D(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3)
		{
			_delegate(_data, t0, t1, t2, t3);
		}
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3>(QueryDelegateChunkHandle<T0, T1, T2, T3> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper4E<T0, T1, T2, T3>, T0, T1, T2, T3>(
				new QueryDelegateChunkWrapper4E<T0, T1, T2, T3>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3>(QueryDelegateChunkHandle<T0, T1, T2, T3> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper4E<T0, T1, T2, T3>, T0, T1, T2, T3>(
				new QueryDelegateChunkWrapper4E<T0, T1, T2, T3>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<T0, T1, T2, T3>(QueryDelegateChunkHandle<T0, T1, T2, T3> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
		{
			return ExecuteChunkParallel<QueryDelegateChunkWrapper4E<T0, T1, T2, T3>, T0, T1, T2, T3>(
				new QueryDelegateChunkWrapper4E<T0, T1, T2, T3>(@delegate),
				query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3>(QueryDelegateChunk<T0, T1, T2, T3> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper4<T0, T1, T2, T3>, T0, T1, T2, T3>(
				new QueryDelegateChunkWrapper4<T0, T1, T2, T3>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3>(QueryDelegateChunk<T0, T1, T2, T3> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper4<T0, T1, T2, T3>, T0, T1, T2, T3>(
				new QueryDelegateChunkWrapper4<T0, T1, T2, T3>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<T0, T1, T2, T3>(QueryDelegateChunk<T0, T1, T2, T3> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
		{
			return ExecuteChunkParallel<QueryDelegateChunkWrapper4<T0, T1, T2, T3>, T0, T1, T2, T3>(
				new QueryDelegateChunkWrapper4<T0, T1, T2, T3>(@delegate),
				query
			);
		}

		// vv-- TData variants --vv //

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3>(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper4ED<TData, T0, T1, T2, T3>, T0, T1, T2, T3>(
				new QueryDelegateChunkWrapper4ED<TData, T0, T1, T2, T3>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3>(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper4ED<TData, T0, T1, T2, T3>, T0, T1, T2, T3>(
				new QueryDelegateChunkWrapper4ED<TData, T0, T1, T2, T3>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<TData, T0, T1, T2, T3>(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
		{
			return ExecuteChunkParallel<QueryDelegateChunkWrapper4ED<TData, T0, T1, T2, T3>, T0, T1, T2, T3>(
				new QueryDelegateChunkWrapper4ED<TData, T0, T1, T2, T3>(data, @delegate),
				query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3>(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper4D<TData, T0, T1, T2, T3>, T0, T1, T2, T3>(
				new QueryDelegateChunkWrapper4D<TData, T0, T1, T2, T3>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3>(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper4D<TData, T0, T1, T2, T3>, T0, T1, T2, T3>(
				new QueryDelegateChunkWrapper4D<TData, T0, T1, T2, T3>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<TData, T0, T1, T2, T3>(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
		{
			return ExecuteChunkParallel<QueryDelegateChunkWrapper4D<TData, T0, T1, T2, T3>, T0, T1, T2, T3>(
				new QueryDelegateChunkWrapper4D<TData, T0, T1, T2, T3>(data, @delegate),
				query
			);
		}
	}
}

namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A delegate called for entities in a query. Receives just the components requested.
	/// </summary>
	public delegate void QueryDelegateChunk<T0, T1, T2, T3, T4>(
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity and the components requested.
	/// </summary>
	public delegate void QueryDelegateChunkHandle<T0, T1, T2, T3, T4>(
		ChunkHandle chunk,
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateChunkData<in TData, T0, T1, T2, T3, T4>(
		TData data,
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity, a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateChunkHandleData<in TData, T0, T1, T2, T3, T4>(
		TData data,
		ChunkHandle chunk,
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4
	);

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper5E<T0, T1, T2, T3, T4>
		: IChunkQuery<T0, T1, T2, T3, T4>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
	{
		private readonly QueryDelegateChunkHandle<T0, T1, T2, T3, T4> _delegate;

		public QueryDelegateChunkWrapper5E(QueryDelegateChunkHandle<T0, T1, T2, T3, T4> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4)
		{
			_delegate(chunk, t0, t1, t2, t3, t4);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper5<T0, T1, T2, T3, T4>
		: IChunkQuery<T0, T1, T2, T3, T4>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
	{
		private readonly QueryDelegateChunk<T0, T1, T2, T3, T4> _delegate;

		public QueryDelegateChunkWrapper5(QueryDelegateChunk<T0, T1, T2, T3, T4> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4)
		{
			_delegate(t0, t1, t2, t3, t4);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper5ED<TData, T0, T1, T2, T3, T4>
		: IChunkQuery<T0, T1, T2, T3, T4>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
	{
		private readonly QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4> _delegate;
		private readonly TData _data;

		public QueryDelegateChunkWrapper5ED(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4)
		{
			_delegate(_data, chunk, t0, t1, t2, t3, t4);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper5D<TData, T0, T1, T2, T3, T4>
		: IChunkQuery<T0, T1, T2, T3, T4>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
	{
		private readonly QueryDelegateChunkData<TData, T0, T1, T2, T3, T4> _delegate;
		private readonly TData _data;

		public QueryDelegateChunkWrapper5D(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3, T4> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4)
		{
			_delegate(_data, t0, t1, t2, t3, t4);
		}
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3, T4>(QueryDelegateChunkHandle<T0, T1, T2, T3, T4> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper5E<T0, T1, T2, T3, T4>, T0, T1, T2, T3, T4>(
				new QueryDelegateChunkWrapper5E<T0, T1, T2, T3, T4>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3, T4>(QueryDelegateChunkHandle<T0, T1, T2, T3, T4> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper5E<T0, T1, T2, T3, T4>, T0, T1, T2, T3, T4>(
				new QueryDelegateChunkWrapper5E<T0, T1, T2, T3, T4>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<T0, T1, T2, T3, T4>(QueryDelegateChunkHandle<T0, T1, T2, T3, T4> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
		{
			return ExecuteChunkParallel<QueryDelegateChunkWrapper5E<T0, T1, T2, T3, T4>, T0, T1, T2, T3, T4>(
				new QueryDelegateChunkWrapper5E<T0, T1, T2, T3, T4>(@delegate),
				query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3, T4>(QueryDelegateChunk<T0, T1, T2, T3, T4> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper5<T0, T1, T2, T3, T4>, T0, T1, T2, T3, T4>(
				new QueryDelegateChunkWrapper5<T0, T1, T2, T3, T4>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3, T4>(QueryDelegateChunk<T0, T1, T2, T3, T4> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper5<T0, T1, T2, T3, T4>, T0, T1, T2, T3, T4>(
				new QueryDelegateChunkWrapper5<T0, T1, T2, T3, T4>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<T0, T1, T2, T3, T4>(QueryDelegateChunk<T0, T1, T2, T3, T4> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
		{
			return ExecuteChunkParallel<QueryDelegateChunkWrapper5<T0, T1, T2, T3, T4>, T0, T1, T2, T3, T4>(
				new QueryDelegateChunkWrapper5<T0, T1, T2, T3, T4>(@delegate),
				query
			);
		}

		// vv-- TData variants --vv //

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3, T4>(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper5ED<TData, T0, T1, T2, T3, T4>, T0, T1, T2, T3, T4>(
				new QueryDelegateChunkWrapper5ED<TData, T0, T1, T2, T3, T4>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3, T4>(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper5ED<TData, T0, T1, T2, T3, T4>, T0, T1, T2, T3, T4>(
				new QueryDelegateChunkWrapper5ED<TData, T0, T1, T2, T3, T4>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<TData, T0, T1, T2, T3, T4>(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
		{
			return ExecuteChunkParallel<QueryDelegateChunkWrapper5ED<TData, T0, T1, T2, T3, T4>, T0, T1, T2, T3, T4>(
				new QueryDelegateChunkWrapper5ED<TData, T0, T1, T2, T3, T4>(data, @delegate),
				query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3, T4>(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3, T4> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper5D<TData, T0, T1, T2, T3, T4>, T0, T1, T2, T3, T4>(
				new QueryDelegateChunkWrapper5D<TData, T0, T1, T2, T3, T4>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3, T4>(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3, T4> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper5D<TData, T0, T1, T2, T3, T4>, T0, T1, T2, T3, T4>(
				new QueryDelegateChunkWrapper5D<TData, T0, T1, T2, T3, T4>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<TData, T0, T1, T2, T3, T4>(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3, T4> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
		{
			return ExecuteChunkParallel<QueryDelegateChunkWrapper5D<TData, T0, T1, T2, T3, T4>, T0, T1, T2, T3, T4>(
				new QueryDelegateChunkWrapper5D<TData, T0, T1, T2, T3, T4>(data, @delegate),
				query
			);
		}
	}
}

namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A delegate called for entities in a query. Receives just the components requested.
	/// </summary>
	public delegate void QueryDelegateChunk<T0, T1, T2, T3, T4, T5>(
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity and the components requested.
	/// </summary>
	public delegate void QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5>(
		ChunkHandle chunk,
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateChunkData<in TData, T0, T1, T2, T3, T4, T5>(
		TData data,
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity, a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateChunkHandleData<in TData, T0, T1, T2, T3, T4, T5>(
		TData data,
		ChunkHandle chunk,
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5
	);

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper6E<T0, T1, T2, T3, T4, T5>
		: IChunkQuery<T0, T1, T2, T3, T4, T5>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
	{
		private readonly QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5> _delegate;

		public QueryDelegateChunkWrapper6E(QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5)
		{
			_delegate(chunk, t0, t1, t2, t3, t4, t5);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper6<T0, T1, T2, T3, T4, T5>
		: IChunkQuery<T0, T1, T2, T3, T4, T5>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
	{
		private readonly QueryDelegateChunk<T0, T1, T2, T3, T4, T5> _delegate;

		public QueryDelegateChunkWrapper6(QueryDelegateChunk<T0, T1, T2, T3, T4, T5> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5)
		{
			_delegate(t0, t1, t2, t3, t4, t5);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper6ED<TData, T0, T1, T2, T3, T4, T5>
		: IChunkQuery<T0, T1, T2, T3, T4, T5>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
	{
		private readonly QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5> _delegate;
		private readonly TData _data;

		public QueryDelegateChunkWrapper6ED(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5)
		{
			_delegate(_data, chunk, t0, t1, t2, t3, t4, t5);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper6D<TData, T0, T1, T2, T3, T4, T5>
		: IChunkQuery<T0, T1, T2, T3, T4, T5>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
	{
		private readonly QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5> _delegate;
		private readonly TData _data;

		public QueryDelegateChunkWrapper6D(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5)
		{
			_delegate(_data, t0, t1, t2, t3, t4, t5);
		}
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3, T4, T5>(QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper6E<T0, T1, T2, T3, T4, T5>, T0, T1, T2, T3, T4, T5>(
				new QueryDelegateChunkWrapper6E<T0, T1, T2, T3, T4, T5>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3, T4, T5>(QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper6E<T0, T1, T2, T3, T4, T5>, T0, T1, T2, T3, T4, T5>(
				new QueryDelegateChunkWrapper6E<T0, T1, T2, T3, T4, T5>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<T0, T1, T2, T3, T4, T5>(QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
		{
			return ExecuteChunkParallel<QueryDelegateChunkWrapper6E<T0, T1, T2, T3, T4, T5>, T0, T1, T2, T3, T4, T5>(
				new QueryDelegateChunkWrapper6E<T0, T1, T2, T3, T4, T5>(@delegate),
				query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3, T4, T5>(QueryDelegateChunk<T0, T1, T2, T3, T4, T5> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper6<T0, T1, T2, T3, T4, T5>, T0, T1, T2, T3, T4, T5>(
				new QueryDelegateChunkWrapper6<T0, T1, T2, T3, T4, T5>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3, T4, T5>(QueryDelegateChunk<T0, T1, T2, T3, T4, T5> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper6<T0, T1, T2, T3, T4, T5>, T0, T1, T2, T3, T4, T5>(
				new QueryDelegateChunkWrapper6<T0, T1, T2, T3, T4, T5>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<T0, T1, T2, T3, T4, T5>(QueryDelegateChunk<T0, T1, T2, T3, T4, T5> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
		{
			return ExecuteChunkParallel<QueryDelegateChunkWrapper6<T0, T1, T2, T3, T4, T5>, T0, T1, T2, T3, T4, T5>(
				new QueryDelegateChunkWrapper6<T0, T1, T2, T3, T4, T5>(@delegate),
				query
			);
		}

		// vv-- TData variants --vv //

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3, T4, T5>(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper6ED<TData, T0, T1, T2, T3, T4, T5>, T0, T1, T2, T3, T4, T5>(
				new QueryDelegateChunkWrapper6ED<TData, T0, T1, T2, T3, T4, T5>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3, T4, T5>(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper6ED<TData, T0, T1, T2, T3, T4, T5>, T0, T1, T2, T3, T4, T5>(
				new QueryDelegateChunkWrapper6ED<TData, T0, T1, T2, T3, T4, T5>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<TData, T0, T1, T2, T3, T4, T5>(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
		{
			return ExecuteChunkParallel<QueryDelegateChunkWrapper6ED<TData, T0, T1, T2, T3, T4, T5>, T0, T1, T2, T3, T4, T5>(
				new QueryDelegateChunkWrapper6ED<TData, T0, T1, T2, T3, T4, T5>(data, @delegate),
				query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3, T4, T5>(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper6D<TData, T0, T1, T2, T3, T4, T5>, T0, T1, T2, T3, T4, T5>(
				new QueryDelegateChunkWrapper6D<TData, T0, T1, T2, T3, T4, T5>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3, T4, T5>(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper6D<TData, T0, T1, T2, T3, T4, T5>, T0, T1, T2, T3, T4, T5>(
				new QueryDelegateChunkWrapper6D<TData, T0, T1, T2, T3, T4, T5>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<TData, T0, T1, T2, T3, T4, T5>(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
		{
			return ExecuteChunkParallel<QueryDelegateChunkWrapper6D<TData, T0, T1, T2, T3, T4, T5>, T0, T1, T2, T3, T4, T5>(
				new QueryDelegateChunkWrapper6D<TData, T0, T1, T2, T3, T4, T5>(data, @delegate),
				query
			);
		}
	}
}

namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A delegate called for entities in a query. Receives just the components requested.
	/// </summary>
	public delegate void QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6>(
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity and the components requested.
	/// </summary>
	public delegate void QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6>(
		ChunkHandle chunk,
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateChunkData<in TData, T0, T1, T2, T3, T4, T5, T6>(
		TData data,
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity, a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateChunkHandleData<in TData, T0, T1, T2, T3, T4, T5, T6>(
		TData data,
		ChunkHandle chunk,
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6
	);

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper7E<T0, T1, T2, T3, T4, T5, T6>
		: IChunkQuery<T0, T1, T2, T3, T4, T5, T6>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
	{
		private readonly QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6> _delegate;

		public QueryDelegateChunkWrapper7E(QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6)
		{
			_delegate(chunk, t0, t1, t2, t3, t4, t5, t6);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper7<T0, T1, T2, T3, T4, T5, T6>
		: IChunkQuery<T0, T1, T2, T3, T4, T5, T6>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
	{
		private readonly QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6> _delegate;

		public QueryDelegateChunkWrapper7(QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6)
		{
			_delegate(t0, t1, t2, t3, t4, t5, t6);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper7ED<TData, T0, T1, T2, T3, T4, T5, T6>
		: IChunkQuery<T0, T1, T2, T3, T4, T5, T6>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
	{
		private readonly QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5, T6> _delegate;
		private readonly TData _data;

		public QueryDelegateChunkWrapper7ED(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5, T6> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6)
		{
			_delegate(_data, chunk, t0, t1, t2, t3, t4, t5, t6);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper7D<TData, T0, T1, T2, T3, T4, T5, T6>
		: IChunkQuery<T0, T1, T2, T3, T4, T5, T6>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
	{
		private readonly QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5, T6> _delegate;
		private readonly TData _data;

		public QueryDelegateChunkWrapper7D(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5, T6> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6)
		{
			_delegate(_data, t0, t1, t2, t3, t4, t5, t6);
		}
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3, T4, T5, T6>(QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper7E<T0, T1, T2, T3, T4, T5, T6>, T0, T1, T2, T3, T4, T5, T6>(
				new QueryDelegateChunkWrapper7E<T0, T1, T2, T3, T4, T5, T6>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3, T4, T5, T6>(QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper7E<T0, T1, T2, T3, T4, T5, T6>, T0, T1, T2, T3, T4, T5, T6>(
				new QueryDelegateChunkWrapper7E<T0, T1, T2, T3, T4, T5, T6>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<T0, T1, T2, T3, T4, T5, T6>(QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
		{
			return ExecuteChunkParallel<QueryDelegateChunkWrapper7E<T0, T1, T2, T3, T4, T5, T6>, T0, T1, T2, T3, T4, T5, T6>(
				new QueryDelegateChunkWrapper7E<T0, T1, T2, T3, T4, T5, T6>(@delegate),
				query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3, T4, T5, T6>(QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper7<T0, T1, T2, T3, T4, T5, T6>, T0, T1, T2, T3, T4, T5, T6>(
				new QueryDelegateChunkWrapper7<T0, T1, T2, T3, T4, T5, T6>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3, T4, T5, T6>(QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper7<T0, T1, T2, T3, T4, T5, T6>, T0, T1, T2, T3, T4, T5, T6>(
				new QueryDelegateChunkWrapper7<T0, T1, T2, T3, T4, T5, T6>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<T0, T1, T2, T3, T4, T5, T6>(QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
		{
			return ExecuteChunkParallel<QueryDelegateChunkWrapper7<T0, T1, T2, T3, T4, T5, T6>, T0, T1, T2, T3, T4, T5, T6>(
				new QueryDelegateChunkWrapper7<T0, T1, T2, T3, T4, T5, T6>(@delegate),
				query
			);
		}

		// vv-- TData variants --vv //

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3, T4, T5, T6>(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5, T6> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper7ED<TData, T0, T1, T2, T3, T4, T5, T6>, T0, T1, T2, T3, T4, T5, T6>(
				new QueryDelegateChunkWrapper7ED<TData, T0, T1, T2, T3, T4, T5, T6>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3, T4, T5, T6>(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5, T6> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper7ED<TData, T0, T1, T2, T3, T4, T5, T6>, T0, T1, T2, T3, T4, T5, T6>(
				new QueryDelegateChunkWrapper7ED<TData, T0, T1, T2, T3, T4, T5, T6>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<TData, T0, T1, T2, T3, T4, T5, T6>(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5, T6> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
		{
			return ExecuteChunkParallel<QueryDelegateChunkWrapper7ED<TData, T0, T1, T2, T3, T4, T5, T6>, T0, T1, T2, T3, T4, T5, T6>(
				new QueryDelegateChunkWrapper7ED<TData, T0, T1, T2, T3, T4, T5, T6>(data, @delegate),
				query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3, T4, T5, T6>(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5, T6> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper7D<TData, T0, T1, T2, T3, T4, T5, T6>, T0, T1, T2, T3, T4, T5, T6>(
				new QueryDelegateChunkWrapper7D<TData, T0, T1, T2, T3, T4, T5, T6>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3, T4, T5, T6>(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5, T6> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper7D<TData, T0, T1, T2, T3, T4, T5, T6>, T0, T1, T2, T3, T4, T5, T6>(
				new QueryDelegateChunkWrapper7D<TData, T0, T1, T2, T3, T4, T5, T6>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<TData, T0, T1, T2, T3, T4, T5, T6>(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5, T6> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
		{
			return ExecuteChunkParallel<QueryDelegateChunkWrapper7D<TData, T0, T1, T2, T3, T4, T5, T6>, T0, T1, T2, T3, T4, T5, T6>(
				new QueryDelegateChunkWrapper7D<TData, T0, T1, T2, T3, T4, T5, T6>(data, @delegate),
				query
			);
		}
	}
}

namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A delegate called for entities in a query. Receives just the components requested.
	/// </summary>
	public delegate void QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7>(
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity and the components requested.
	/// </summary>
	public delegate void QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7>(
		ChunkHandle chunk,
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateChunkData<in TData, T0, T1, T2, T3, T4, T5, T6, T7>(
		TData data,
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity, a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateChunkHandleData<in TData, T0, T1, T2, T3, T4, T5, T6, T7>(
		TData data,
		ChunkHandle chunk,
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7
	);

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper8E<T0, T1, T2, T3, T4, T5, T6, T7>
		: IChunkQuery<T0, T1, T2, T3, T4, T5, T6, T7>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
	{
		private readonly QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7> _delegate;

		public QueryDelegateChunkWrapper8E(QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7)
		{
			_delegate(chunk, t0, t1, t2, t3, t4, t5, t6, t7);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper8<T0, T1, T2, T3, T4, T5, T6, T7>
		: IChunkQuery<T0, T1, T2, T3, T4, T5, T6, T7>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
	{
		private readonly QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7> _delegate;

		public QueryDelegateChunkWrapper8(QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7)
		{
			_delegate(t0, t1, t2, t3, t4, t5, t6, t7);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper8ED<TData, T0, T1, T2, T3, T4, T5, T6, T7>
		: IChunkQuery<T0, T1, T2, T3, T4, T5, T6, T7>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
	{
		private readonly QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5, T6, T7> _delegate;
		private readonly TData _data;

		public QueryDelegateChunkWrapper8ED(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5, T6, T7> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7)
		{
			_delegate(_data, chunk, t0, t1, t2, t3, t4, t5, t6, t7);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper8D<TData, T0, T1, T2, T3, T4, T5, T6, T7>
		: IChunkQuery<T0, T1, T2, T3, T4, T5, T6, T7>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
	{
		private readonly QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5, T6, T7> _delegate;
		private readonly TData _data;

		public QueryDelegateChunkWrapper8D(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5, T6, T7> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7)
		{
			_delegate(_data, t0, t1, t2, t3, t4, t5, t6, t7);
		}
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3, T4, T5, T6, T7>(QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper8E<T0, T1, T2, T3, T4, T5, T6, T7>, T0, T1, T2, T3, T4, T5, T6, T7>(
				new QueryDelegateChunkWrapper8E<T0, T1, T2, T3, T4, T5, T6, T7>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3, T4, T5, T6, T7>(QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper8E<T0, T1, T2, T3, T4, T5, T6, T7>, T0, T1, T2, T3, T4, T5, T6, T7>(
				new QueryDelegateChunkWrapper8E<T0, T1, T2, T3, T4, T5, T6, T7>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<T0, T1, T2, T3, T4, T5, T6, T7>(QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
		{
			return ExecuteChunkParallel<QueryDelegateChunkWrapper8E<T0, T1, T2, T3, T4, T5, T6, T7>, T0, T1, T2, T3, T4, T5, T6, T7>(
				new QueryDelegateChunkWrapper8E<T0, T1, T2, T3, T4, T5, T6, T7>(@delegate),
				query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3, T4, T5, T6, T7>(QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper8<T0, T1, T2, T3, T4, T5, T6, T7>, T0, T1, T2, T3, T4, T5, T6, T7>(
				new QueryDelegateChunkWrapper8<T0, T1, T2, T3, T4, T5, T6, T7>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3, T4, T5, T6, T7>(QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper8<T0, T1, T2, T3, T4, T5, T6, T7>, T0, T1, T2, T3, T4, T5, T6, T7>(
				new QueryDelegateChunkWrapper8<T0, T1, T2, T3, T4, T5, T6, T7>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<T0, T1, T2, T3, T4, T5, T6, T7>(QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
		{
			return ExecuteChunkParallel<QueryDelegateChunkWrapper8<T0, T1, T2, T3, T4, T5, T6, T7>, T0, T1, T2, T3, T4, T5, T6, T7>(
				new QueryDelegateChunkWrapper8<T0, T1, T2, T3, T4, T5, T6, T7>(@delegate),
				query
			);
		}

		// vv-- TData variants --vv //

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3, T4, T5, T6, T7>(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5, T6, T7> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper8ED<TData, T0, T1, T2, T3, T4, T5, T6, T7>, T0, T1, T2, T3, T4, T5, T6, T7>(
				new QueryDelegateChunkWrapper8ED<TData, T0, T1, T2, T3, T4, T5, T6, T7>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3, T4, T5, T6, T7>(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5, T6, T7> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper8ED<TData, T0, T1, T2, T3, T4, T5, T6, T7>, T0, T1, T2, T3, T4, T5, T6, T7>(
				new QueryDelegateChunkWrapper8ED<TData, T0, T1, T2, T3, T4, T5, T6, T7>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<TData, T0, T1, T2, T3, T4, T5, T6, T7>(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5, T6, T7> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
		{
			return ExecuteChunkParallel<QueryDelegateChunkWrapper8ED<TData, T0, T1, T2, T3, T4, T5, T6, T7>, T0, T1, T2, T3, T4, T5, T6, T7>(
				new QueryDelegateChunkWrapper8ED<TData, T0, T1, T2, T3, T4, T5, T6, T7>(data, @delegate),
				query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3, T4, T5, T6, T7>(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5, T6, T7> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper8D<TData, T0, T1, T2, T3, T4, T5, T6, T7>, T0, T1, T2, T3, T4, T5, T6, T7>(
				new QueryDelegateChunkWrapper8D<TData, T0, T1, T2, T3, T4, T5, T6, T7>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3, T4, T5, T6, T7>(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5, T6, T7> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
		{
			return ExecuteChunk<QueryDelegateChunkWrapper8D<TData, T0, T1, T2, T3, T4, T5, T6, T7>, T0, T1, T2, T3, T4, T5, T6, T7>(
				new QueryDelegateChunkWrapper8D<TData, T0, T1, T2, T3, T4, T5, T6, T7>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<TData, T0, T1, T2, T3, T4, T5, T6, T7>(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5, T6, T7> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
		{
			return ExecuteChunkParallel<QueryDelegateChunkWrapper8D<TData, T0, T1, T2, T3, T4, T5, T6, T7>, T0, T1, T2, T3, T4, T5, T6, T7>(
				new QueryDelegateChunkWrapper8D<TData, T0, T1, T2, T3, T4, T5, T6, T7>(data, @delegate),
				query
			);
		}
	}
}

namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A delegate called for entities in a query. Receives just the components requested.
	/// </summary>
	public delegate void QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7, T8>(
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity and the components requested.
	/// </summary>
	public delegate void QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7, T8>(
		ChunkHandle chunk,
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateChunkData<in TData, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
		TData data,
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity, a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateChunkHandleData<in TData, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
		TData data,
		ChunkHandle chunk,
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8
	);

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper9E<T0, T1, T2, T3, T4, T5, T6, T7, T8>
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
	{
		private readonly QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7, T8> _delegate;

		public QueryDelegateChunkWrapper9E(QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7, T8> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8)
		{
			_delegate(chunk, t0, t1, t2, t3, t4, t5, t6, t7, t8);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper9<T0, T1, T2, T3, T4, T5, T6, T7, T8>
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
	{
		private readonly QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7, T8> _delegate;

		public QueryDelegateChunkWrapper9(QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7, T8> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8)
		{
			_delegate(t0, t1, t2, t3, t4, t5, t6, t7, t8);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper9ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8>
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
	{
		private readonly QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8> _delegate;
		private readonly TData _data;

		public QueryDelegateChunkWrapper9ED(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8)
		{
			_delegate(_data, chunk, t0, t1, t2, t3, t4, t5, t6, t7, t8);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper9D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8>
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
	{
		private readonly QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8> _delegate;
		private readonly TData _data;

		public QueryDelegateChunkWrapper9D(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8)
		{
			_delegate(_data, t0, t1, t2, t3, t4, t5, t6, t7, t8);
		}
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3, T4, T5, T6, T7, T8>(QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7, T8> @delegate, ref QueryDescription? query)
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
			return ExecuteChunk<QueryDelegateChunkWrapper9E<T0, T1, T2, T3, T4, T5, T6, T7, T8>, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
				new QueryDelegateChunkWrapper9E<T0, T1, T2, T3, T4, T5, T6, T7, T8>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3, T4, T5, T6, T7, T8>(QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7, T8> @delegate, QueryDescription? query = null)
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
			return ExecuteChunk<QueryDelegateChunkWrapper9E<T0, T1, T2, T3, T4, T5, T6, T7, T8>, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
				new QueryDelegateChunkWrapper9E<T0, T1, T2, T3, T4, T5, T6, T7, T8>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<T0, T1, T2, T3, T4, T5, T6, T7, T8>(QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7, T8> @delegate, QueryDescription? query = null)
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
			return ExecuteChunkParallel<QueryDelegateChunkWrapper9E<T0, T1, T2, T3, T4, T5, T6, T7, T8>, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
				new QueryDelegateChunkWrapper9E<T0, T1, T2, T3, T4, T5, T6, T7, T8>(@delegate),
				query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3, T4, T5, T6, T7, T8>(QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7, T8> @delegate, ref QueryDescription? query)
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
			return ExecuteChunk<QueryDelegateChunkWrapper9<T0, T1, T2, T3, T4, T5, T6, T7, T8>, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
				new QueryDelegateChunkWrapper9<T0, T1, T2, T3, T4, T5, T6, T7, T8>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3, T4, T5, T6, T7, T8>(QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7, T8> @delegate, QueryDescription? query = null)
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
			return ExecuteChunk<QueryDelegateChunkWrapper9<T0, T1, T2, T3, T4, T5, T6, T7, T8>, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
				new QueryDelegateChunkWrapper9<T0, T1, T2, T3, T4, T5, T6, T7, T8>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<T0, T1, T2, T3, T4, T5, T6, T7, T8>(QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7, T8> @delegate, QueryDescription? query = null)
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
			return ExecuteChunkParallel<QueryDelegateChunkWrapper9<T0, T1, T2, T3, T4, T5, T6, T7, T8>, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
				new QueryDelegateChunkWrapper9<T0, T1, T2, T3, T4, T5, T6, T7, T8>(@delegate),
				query
			);
		}

		// vv-- TData variants --vv //

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8>(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8> @delegate, ref QueryDescription? query)
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
			return ExecuteChunk<QueryDelegateChunkWrapper9ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8>, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
				new QueryDelegateChunkWrapper9ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8>(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8> @delegate, QueryDescription? query = null)
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
			return ExecuteChunk<QueryDelegateChunkWrapper9ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8>, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
				new QueryDelegateChunkWrapper9ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8>(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8> @delegate, QueryDescription? query = null)
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
			return ExecuteChunkParallel<QueryDelegateChunkWrapper9ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8>, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
				new QueryDelegateChunkWrapper9ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8>(data, @delegate),
				query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8>(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8> @delegate, ref QueryDescription? query)
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
			return ExecuteChunk<QueryDelegateChunkWrapper9D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8>, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
				new QueryDelegateChunkWrapper9D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8>(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8> @delegate, QueryDescription? query = null)
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
			return ExecuteChunk<QueryDelegateChunkWrapper9D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8>, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
				new QueryDelegateChunkWrapper9D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8>(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8> @delegate, QueryDescription? query = null)
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
			return ExecuteChunkParallel<QueryDelegateChunkWrapper9D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8>, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
				new QueryDelegateChunkWrapper9D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8>(data, @delegate),
				query
			);
		}
	}
}

namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A delegate called for entities in a query. Receives just the components requested.
	/// </summary>
	public delegate void QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity and the components requested.
	/// </summary>
	public delegate void QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
		ChunkHandle chunk,
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateChunkData<in TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
		TData data,
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity, a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateChunkHandleData<in TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
		TData data,
		ChunkHandle chunk,
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9
	);

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper10E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
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
	{
		private readonly QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> _delegate;

		public QueryDelegateChunkWrapper10E(QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9)
		{
			_delegate(chunk, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
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
	{
		private readonly QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> _delegate;

		public QueryDelegateChunkWrapper10(QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9)
		{
			_delegate(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper10ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
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
	{
		private readonly QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> _delegate;
		private readonly TData _data;

		public QueryDelegateChunkWrapper10ED(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9)
		{
			_delegate(_data, chunk, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper10D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
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
	{
		private readonly QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> _delegate;
		private readonly TData _data;

		public QueryDelegateChunkWrapper10D(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9)
		{
			_delegate(_data, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9);
		}
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> @delegate, ref QueryDescription? query)
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
			return ExecuteChunk<QueryDelegateChunkWrapper10E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
				new QueryDelegateChunkWrapper10E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> @delegate, QueryDescription? query = null)
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
			return ExecuteChunk<QueryDelegateChunkWrapper10E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
				new QueryDelegateChunkWrapper10E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> @delegate, QueryDescription? query = null)
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
			return ExecuteChunkParallel<QueryDelegateChunkWrapper10E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
				new QueryDelegateChunkWrapper10E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(@delegate),
				query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> @delegate, ref QueryDescription? query)
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
			return ExecuteChunk<QueryDelegateChunkWrapper10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
				new QueryDelegateChunkWrapper10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> @delegate, QueryDescription? query = null)
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
			return ExecuteChunk<QueryDelegateChunkWrapper10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
				new QueryDelegateChunkWrapper10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> @delegate, QueryDescription? query = null)
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
			return ExecuteChunkParallel<QueryDelegateChunkWrapper10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
				new QueryDelegateChunkWrapper10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(@delegate),
				query
			);
		}

		// vv-- TData variants --vv //

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> @delegate, ref QueryDescription? query)
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
			return ExecuteChunk<QueryDelegateChunkWrapper10ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
				new QueryDelegateChunkWrapper10ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> @delegate, QueryDescription? query = null)
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
			return ExecuteChunk<QueryDelegateChunkWrapper10ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
				new QueryDelegateChunkWrapper10ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> @delegate, QueryDescription? query = null)
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
			return ExecuteChunkParallel<QueryDelegateChunkWrapper10ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
				new QueryDelegateChunkWrapper10ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(data, @delegate),
				query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> @delegate, ref QueryDescription? query)
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
			return ExecuteChunk<QueryDelegateChunkWrapper10D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
				new QueryDelegateChunkWrapper10D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> @delegate, QueryDescription? query = null)
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
			return ExecuteChunk<QueryDelegateChunkWrapper10D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
				new QueryDelegateChunkWrapper10D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> @delegate, QueryDescription? query = null)
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
			return ExecuteChunkParallel<QueryDelegateChunkWrapper10D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
				new QueryDelegateChunkWrapper10D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(data, @delegate),
				query
			);
		}
	}
}

namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A delegate called for entities in a query. Receives just the components requested.
	/// </summary>
	public delegate void QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity and the components requested.
	/// </summary>
	public delegate void QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
		ChunkHandle chunk,
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateChunkData<in TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
		TData data,
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity, a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateChunkHandleData<in TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
		TData data,
		ChunkHandle chunk,
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10
	);

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper11E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
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
	{
		private readonly QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> _delegate;

		public QueryDelegateChunkWrapper11E(QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10)
		{
			_delegate(chunk, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
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
	{
		private readonly QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> _delegate;

		public QueryDelegateChunkWrapper11(QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10)
		{
			_delegate(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper11ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
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
	{
		private readonly QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> _delegate;
		private readonly TData _data;

		public QueryDelegateChunkWrapper11ED(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10)
		{
			_delegate(_data, chunk, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper11D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
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
	{
		private readonly QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> _delegate;
		private readonly TData _data;

		public QueryDelegateChunkWrapper11D(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10)
		{
			_delegate(_data, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
		}
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> @delegate, ref QueryDescription? query)
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
			return ExecuteChunk<QueryDelegateChunkWrapper11E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
				new QueryDelegateChunkWrapper11E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> @delegate, QueryDescription? query = null)
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
			return ExecuteChunk<QueryDelegateChunkWrapper11E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
				new QueryDelegateChunkWrapper11E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> @delegate, QueryDescription? query = null)
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
			return ExecuteChunkParallel<QueryDelegateChunkWrapper11E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
				new QueryDelegateChunkWrapper11E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(@delegate),
				query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> @delegate, ref QueryDescription? query)
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
			return ExecuteChunk<QueryDelegateChunkWrapper11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
				new QueryDelegateChunkWrapper11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> @delegate, QueryDescription? query = null)
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
			return ExecuteChunk<QueryDelegateChunkWrapper11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
				new QueryDelegateChunkWrapper11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> @delegate, QueryDescription? query = null)
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
			return ExecuteChunkParallel<QueryDelegateChunkWrapper11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
				new QueryDelegateChunkWrapper11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(@delegate),
				query
			);
		}

		// vv-- TData variants --vv //

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> @delegate, ref QueryDescription? query)
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
			return ExecuteChunk<QueryDelegateChunkWrapper11ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
				new QueryDelegateChunkWrapper11ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> @delegate, QueryDescription? query = null)
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
			return ExecuteChunk<QueryDelegateChunkWrapper11ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
				new QueryDelegateChunkWrapper11ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> @delegate, QueryDescription? query = null)
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
			return ExecuteChunkParallel<QueryDelegateChunkWrapper11ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
				new QueryDelegateChunkWrapper11ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(data, @delegate),
				query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> @delegate, ref QueryDescription? query)
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
			return ExecuteChunk<QueryDelegateChunkWrapper11D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
				new QueryDelegateChunkWrapper11D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> @delegate, QueryDescription? query = null)
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
			return ExecuteChunk<QueryDelegateChunkWrapper11D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
				new QueryDelegateChunkWrapper11D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> @delegate, QueryDescription? query = null)
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
			return ExecuteChunkParallel<QueryDelegateChunkWrapper11D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
				new QueryDelegateChunkWrapper11D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(data, @delegate),
				query
			);
		}
	}
}

namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A delegate called for entities in a query. Receives just the components requested.
	/// </summary>
	public delegate void QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10, Span<T11> t11
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity and the components requested.
	/// </summary>
	public delegate void QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
		ChunkHandle chunk,
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10, Span<T11> t11
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateChunkData<in TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
		TData data,
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10, Span<T11> t11
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity, a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateChunkHandleData<in TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
		TData data,
		ChunkHandle chunk,
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10, Span<T11> t11
	);

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper12E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
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
	{
		private readonly QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> _delegate;

		public QueryDelegateChunkWrapper12E(QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10, Span<T11> t11)
		{
			_delegate(chunk, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
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
	{
		private readonly QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> _delegate;

		public QueryDelegateChunkWrapper12(QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10, Span<T11> t11)
		{
			_delegate(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper12ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
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
	{
		private readonly QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> _delegate;
		private readonly TData _data;

		public QueryDelegateChunkWrapper12ED(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10, Span<T11> t11)
		{
			_delegate(_data, chunk, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper12D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
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
	{
		private readonly QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> _delegate;
		private readonly TData _data;

		public QueryDelegateChunkWrapper12D(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10, Span<T11> t11)
		{
			_delegate(_data, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
		}
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> @delegate, ref QueryDescription? query)
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
			return ExecuteChunk<QueryDelegateChunkWrapper12E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
				new QueryDelegateChunkWrapper12E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> @delegate, QueryDescription? query = null)
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
			return ExecuteChunk<QueryDelegateChunkWrapper12E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
				new QueryDelegateChunkWrapper12E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> @delegate, QueryDescription? query = null)
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
			return ExecuteChunkParallel<QueryDelegateChunkWrapper12E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
				new QueryDelegateChunkWrapper12E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(@delegate),
				query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> @delegate, ref QueryDescription? query)
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
			return ExecuteChunk<QueryDelegateChunkWrapper12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
				new QueryDelegateChunkWrapper12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> @delegate, QueryDescription? query = null)
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
			return ExecuteChunk<QueryDelegateChunkWrapper12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
				new QueryDelegateChunkWrapper12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> @delegate, QueryDescription? query = null)
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
			return ExecuteChunkParallel<QueryDelegateChunkWrapper12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
				new QueryDelegateChunkWrapper12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(@delegate),
				query
			);
		}

		// vv-- TData variants --vv //

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> @delegate, ref QueryDescription? query)
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
			return ExecuteChunk<QueryDelegateChunkWrapper12ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
				new QueryDelegateChunkWrapper12ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> @delegate, QueryDescription? query = null)
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
			return ExecuteChunk<QueryDelegateChunkWrapper12ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
				new QueryDelegateChunkWrapper12ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> @delegate, QueryDescription? query = null)
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
			return ExecuteChunkParallel<QueryDelegateChunkWrapper12ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
				new QueryDelegateChunkWrapper12ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(data, @delegate),
				query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> @delegate, ref QueryDescription? query)
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
			return ExecuteChunk<QueryDelegateChunkWrapper12D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
				new QueryDelegateChunkWrapper12D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> @delegate, QueryDescription? query = null)
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
			return ExecuteChunk<QueryDelegateChunkWrapper12D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
				new QueryDelegateChunkWrapper12D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> @delegate, QueryDescription? query = null)
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
			return ExecuteChunkParallel<QueryDelegateChunkWrapper12D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
				new QueryDelegateChunkWrapper12D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(data, @delegate),
				query
			);
		}
	}
}

namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A delegate called for entities in a query. Receives just the components requested.
	/// </summary>
	public delegate void QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10, Span<T11> t11, Span<T12> t12
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity and the components requested.
	/// </summary>
	public delegate void QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
		ChunkHandle chunk,
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10, Span<T11> t11, Span<T12> t12
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateChunkData<in TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
		TData data,
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10, Span<T11> t11, Span<T12> t12
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity, a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateChunkHandleData<in TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
		TData data,
		ChunkHandle chunk,
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10, Span<T11> t11, Span<T12> t12
	);

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper13E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
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
	{
		private readonly QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> _delegate;

		public QueryDelegateChunkWrapper13E(QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10, Span<T11> t11, Span<T12> t12)
		{
			_delegate(chunk, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
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
	{
		private readonly QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> _delegate;

		public QueryDelegateChunkWrapper13(QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10, Span<T11> t11, Span<T12> t12)
		{
			_delegate(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper13ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
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
	{
		private readonly QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> _delegate;
		private readonly TData _data;

		public QueryDelegateChunkWrapper13ED(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10, Span<T11> t11, Span<T12> t12)
		{
			_delegate(_data, chunk, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper13D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
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
	{
		private readonly QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> _delegate;
		private readonly TData _data;

		public QueryDelegateChunkWrapper13D(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10, Span<T11> t11, Span<T12> t12)
		{
			_delegate(_data, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
		}
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> @delegate, ref QueryDescription? query)
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
			return ExecuteChunk<QueryDelegateChunkWrapper13E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
				new QueryDelegateChunkWrapper13E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> @delegate, QueryDescription? query = null)
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
			return ExecuteChunk<QueryDelegateChunkWrapper13E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
				new QueryDelegateChunkWrapper13E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> @delegate, QueryDescription? query = null)
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
			return ExecuteChunkParallel<QueryDelegateChunkWrapper13E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
				new QueryDelegateChunkWrapper13E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(@delegate),
				query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> @delegate, ref QueryDescription? query)
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
			return ExecuteChunk<QueryDelegateChunkWrapper13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
				new QueryDelegateChunkWrapper13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> @delegate, QueryDescription? query = null)
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
			return ExecuteChunk<QueryDelegateChunkWrapper13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
				new QueryDelegateChunkWrapper13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> @delegate, QueryDescription? query = null)
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
			return ExecuteChunkParallel<QueryDelegateChunkWrapper13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
				new QueryDelegateChunkWrapper13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(@delegate),
				query
			);
		}

		// vv-- TData variants --vv //

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> @delegate, ref QueryDescription? query)
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
			return ExecuteChunk<QueryDelegateChunkWrapper13ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
				new QueryDelegateChunkWrapper13ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> @delegate, QueryDescription? query = null)
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
			return ExecuteChunk<QueryDelegateChunkWrapper13ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
				new QueryDelegateChunkWrapper13ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> @delegate, QueryDescription? query = null)
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
			return ExecuteChunkParallel<QueryDelegateChunkWrapper13ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
				new QueryDelegateChunkWrapper13ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(data, @delegate),
				query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> @delegate, ref QueryDescription? query)
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
			return ExecuteChunk<QueryDelegateChunkWrapper13D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
				new QueryDelegateChunkWrapper13D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> @delegate, QueryDescription? query = null)
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
			return ExecuteChunk<QueryDelegateChunkWrapper13D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
				new QueryDelegateChunkWrapper13D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> @delegate, QueryDescription? query = null)
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
			return ExecuteChunkParallel<QueryDelegateChunkWrapper13D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
				new QueryDelegateChunkWrapper13D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(data, @delegate),
				query
			);
		}
	}
}

namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A delegate called for entities in a query. Receives just the components requested.
	/// </summary>
	public delegate void QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10, Span<T11> t11, Span<T12> t12, Span<T13> t13
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity and the components requested.
	/// </summary>
	public delegate void QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
		ChunkHandle chunk,
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10, Span<T11> t11, Span<T12> t12, Span<T13> t13
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateChunkData<in TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
		TData data,
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10, Span<T11> t11, Span<T12> t12, Span<T13> t13
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity, a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateChunkHandleData<in TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
		TData data,
		ChunkHandle chunk,
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10, Span<T11> t11, Span<T12> t12, Span<T13> t13
	);

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper14E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
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
	{
		private readonly QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> _delegate;

		public QueryDelegateChunkWrapper14E(QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10, Span<T11> t11, Span<T12> t12, Span<T13> t13)
		{
			_delegate(chunk, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
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
	{
		private readonly QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> _delegate;

		public QueryDelegateChunkWrapper14(QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10, Span<T11> t11, Span<T12> t12, Span<T13> t13)
		{
			_delegate(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper14ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
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
	{
		private readonly QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> _delegate;
		private readonly TData _data;

		public QueryDelegateChunkWrapper14ED(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10, Span<T11> t11, Span<T12> t12, Span<T13> t13)
		{
			_delegate(_data, chunk, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper14D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
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
	{
		private readonly QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> _delegate;
		private readonly TData _data;

		public QueryDelegateChunkWrapper14D(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10, Span<T11> t11, Span<T12> t12, Span<T13> t13)
		{
			_delegate(_data, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
		}
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> @delegate, ref QueryDescription? query)
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
			return ExecuteChunk<QueryDelegateChunkWrapper14E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
				new QueryDelegateChunkWrapper14E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> @delegate, QueryDescription? query = null)
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
			return ExecuteChunk<QueryDelegateChunkWrapper14E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
				new QueryDelegateChunkWrapper14E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> @delegate, QueryDescription? query = null)
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
			return ExecuteChunkParallel<QueryDelegateChunkWrapper14E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
				new QueryDelegateChunkWrapper14E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(@delegate),
				query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> @delegate, ref QueryDescription? query)
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
			return ExecuteChunk<QueryDelegateChunkWrapper14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
				new QueryDelegateChunkWrapper14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> @delegate, QueryDescription? query = null)
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
			return ExecuteChunk<QueryDelegateChunkWrapper14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
				new QueryDelegateChunkWrapper14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> @delegate, QueryDescription? query = null)
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
			return ExecuteChunkParallel<QueryDelegateChunkWrapper14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
				new QueryDelegateChunkWrapper14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(@delegate),
				query
			);
		}

		// vv-- TData variants --vv //

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> @delegate, ref QueryDescription? query)
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
			return ExecuteChunk<QueryDelegateChunkWrapper14ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
				new QueryDelegateChunkWrapper14ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> @delegate, QueryDescription? query = null)
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
			return ExecuteChunk<QueryDelegateChunkWrapper14ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
				new QueryDelegateChunkWrapper14ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> @delegate, QueryDescription? query = null)
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
			return ExecuteChunkParallel<QueryDelegateChunkWrapper14ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
				new QueryDelegateChunkWrapper14ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(data, @delegate),
				query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> @delegate, ref QueryDescription? query)
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
			return ExecuteChunk<QueryDelegateChunkWrapper14D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
				new QueryDelegateChunkWrapper14D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> @delegate, QueryDescription? query = null)
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
			return ExecuteChunk<QueryDelegateChunkWrapper14D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
				new QueryDelegateChunkWrapper14D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> @delegate, QueryDescription? query = null)
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
			return ExecuteChunkParallel<QueryDelegateChunkWrapper14D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
				new QueryDelegateChunkWrapper14D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(data, @delegate),
				query
			);
		}
	}
}

namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A delegate called for entities in a query. Receives just the components requested.
	/// </summary>
	public delegate void QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10, Span<T11> t11, Span<T12> t12, Span<T13> t13, Span<T14> t14
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity and the components requested.
	/// </summary>
	public delegate void QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
		ChunkHandle chunk,
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10, Span<T11> t11, Span<T12> t12, Span<T13> t13, Span<T14> t14
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateChunkData<in TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
		TData data,
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10, Span<T11> t11, Span<T12> t12, Span<T13> t13, Span<T14> t14
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity, a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateChunkHandleData<in TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
		TData data,
		ChunkHandle chunk,
		Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10, Span<T11> t11, Span<T12> t12, Span<T13> t13, Span<T14> t14
	);

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper15E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
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
	{
		private readonly QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> _delegate;

		public QueryDelegateChunkWrapper15E(QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10, Span<T11> t11, Span<T12> t12, Span<T13> t13, Span<T14> t14)
		{
			_delegate(chunk, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
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
	{
		private readonly QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> _delegate;

		public QueryDelegateChunkWrapper15(QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10, Span<T11> t11, Span<T12> t12, Span<T13> t13, Span<T14> t14)
		{
			_delegate(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper15ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
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
	{
		private readonly QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> _delegate;
		private readonly TData _data;

		public QueryDelegateChunkWrapper15ED(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10, Span<T11> t11, Span<T12> t12, Span<T13> t13, Span<T14> t14)
		{
			_delegate(_data, chunk, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateChunkWrapper15D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
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
	{
		private readonly QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> _delegate;
		private readonly TData _data;

		public QueryDelegateChunkWrapper15D(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(ChunkHandle chunk, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10, Span<T11> t11, Span<T12> t12, Span<T13> t13, Span<T14> t14)
		{
			_delegate(_data, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
		}
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> @delegate, ref QueryDescription? query)
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
			return ExecuteChunk<QueryDelegateChunkWrapper15E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
				new QueryDelegateChunkWrapper15E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> @delegate, QueryDescription? query = null)
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
			return ExecuteChunk<QueryDelegateChunkWrapper15E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
				new QueryDelegateChunkWrapper15E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(QueryDelegateChunkHandle<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> @delegate, QueryDescription? query = null)
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
			return ExecuteChunkParallel<QueryDelegateChunkWrapper15E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
				new QueryDelegateChunkWrapper15E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(@delegate),
				query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> @delegate, ref QueryDescription? query)
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
			return ExecuteChunk<QueryDelegateChunkWrapper15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
				new QueryDelegateChunkWrapper15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> @delegate, QueryDescription? query = null)
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
			return ExecuteChunk<QueryDelegateChunkWrapper15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
				new QueryDelegateChunkWrapper15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(QueryDelegateChunk<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> @delegate, QueryDescription? query = null)
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
			return ExecuteChunkParallel<QueryDelegateChunkWrapper15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
				new QueryDelegateChunkWrapper15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(@delegate),
				query
			);
		}

		// vv-- TData variants --vv //

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> @delegate, ref QueryDescription? query)
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
			return ExecuteChunk<QueryDelegateChunkWrapper15ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
				new QueryDelegateChunkWrapper15ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> @delegate, QueryDescription? query = null)
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
			return ExecuteChunk<QueryDelegateChunkWrapper15ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
				new QueryDelegateChunkWrapper15ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(TData data, QueryDelegateChunkHandleData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> @delegate, QueryDescription? query = null)
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
			return ExecuteChunkParallel<QueryDelegateChunkWrapper15ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
				new QueryDelegateChunkWrapper15ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(data, @delegate),
				query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> @delegate, ref QueryDescription? query)
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
			return ExecuteChunk<QueryDelegateChunkWrapper15D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
				new QueryDelegateChunkWrapper15D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> @delegate, QueryDescription? query = null)
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
			return ExecuteChunk<QueryDelegateChunkWrapper15D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
				new QueryDelegateChunkWrapper15D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int QueryParallel<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(TData data, QueryDelegateChunkData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> @delegate, QueryDescription? query = null)
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
			return ExecuteChunkParallel<QueryDelegateChunkWrapper15D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
				new QueryDelegateChunkWrapper15D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(data, @delegate),
				query
			);
		}
	}
}



