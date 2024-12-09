using Myriad.ECS.Queries;
using System.Diagnostics.CodeAnalysis;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedParameter.Global
// ReSharper disable LoopCanBeConvertedToQuery
// ReSharper disable ConvertToPrimaryConstructor

namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A delegate called for entities in a query. Receives just the components requested.
	/// </summary>
	public delegate void QueryDelegate<T0>(
		ref T0 t0
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity and the components requested.
	/// </summary>
	public delegate void QueryDelegateEntity<T0>(
		Entity entity,
		ref T0 t0
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateData<in TData, T0>(
		TData data,
		ref T0 t0
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity, a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateEntityData<in TData, T0>(
		TData data,
		Entity entity,
		ref T0 t0
	);

	
	internal readonly struct QueryDelegateWrapper1E<T0>
		: IQuery<T0>
		where T0 : IComponent
	{
		private readonly QueryDelegateEntity<T0> _delegate;

		public QueryDelegateWrapper1E(QueryDelegateEntity<T0> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0)
		{
			_delegate(e, ref t0);
		}
	}

	
	internal readonly struct QueryDelegateWrapper1<T0>
		: IQuery<T0>
		where T0 : IComponent
	{
		private readonly QueryDelegate<T0> _delegate;

		public QueryDelegateWrapper1(QueryDelegate<T0> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0)
		{
			_delegate(ref t0);
		}
	}

	
	internal readonly struct QueryDelegateWrapper1ED<TData, T0>
		: IQuery<T0>
		where T0 : IComponent
	{
		private readonly QueryDelegateEntityData<TData, T0> _delegate;
		private readonly TData _data;

		public QueryDelegateWrapper1ED(TData data, QueryDelegateEntityData<TData, T0> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0)
		{
			_delegate(_data, e, ref t0);
		}
	}

	
	internal readonly struct QueryDelegateWrapper1D<TData, T0>
		: IQuery<T0>
		where T0 : IComponent
	{
		private readonly QueryDelegateData<TData, T0> _delegate;
		private readonly TData _data;

		public QueryDelegateWrapper1D(TData data, QueryDelegateData<TData, T0> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0)
		{
			_delegate(_data, ref t0);
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
		
		public void Query<T0>(QueryDelegateEntity<T0> @delegate, ref QueryDescription? query)
			where T0 : IComponent
		{
			Execute<QueryDelegateWrapper1E<T0>, T0>(
				new QueryDelegateWrapper1E<T0>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		
		public void Query<T0>(QueryDelegateEntity<T0> @delegate, QueryDescription? query = null)
			where T0 : IComponent
		{
			Execute<QueryDelegateWrapper1E<T0>, T0>(
				new QueryDelegateWrapper1E<T0>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		
		public void QueryParallel<T0>(QueryDelegateEntity<T0> @delegate, QueryDescription? query = null)
			where T0 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper1E<T0>, T0>(
				new QueryDelegateWrapper1E<T0>(@delegate),
				query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		
		public void Query<T0>(QueryDelegate<T0> @delegate, ref QueryDescription? query)
			where T0 : IComponent
		{
			Execute<QueryDelegateWrapper1<T0>, T0>(
				new QueryDelegateWrapper1<T0>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		
		public void Query<T0>(QueryDelegate<T0> @delegate, QueryDescription? query = null)
			where T0 : IComponent
		{
			Execute<QueryDelegateWrapper1<T0>, T0>(
				new QueryDelegateWrapper1<T0>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		
		public void QueryParallel<T0>(QueryDelegate<T0> @delegate, QueryDescription? query = null)
			where T0 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper1<T0>, T0>(
				new QueryDelegateWrapper1<T0>(@delegate),
				query
			);
		}

		// --- //

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		
		public void Query<TData, T0>(TData data, QueryDelegateEntityData<TData, T0> @delegate, ref QueryDescription? query)
			where T0 : IComponent
		{
			Execute<QueryDelegateWrapper1ED<TData, T0>, T0>(
				new QueryDelegateWrapper1ED<TData, T0>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		
		public void Query<TData, T0>(TData data, QueryDelegateEntityData<TData, T0> @delegate, QueryDescription? query = null)
			where T0 : IComponent
		{
			Execute<QueryDelegateWrapper1ED<TData, T0>, T0>(
				new QueryDelegateWrapper1ED<TData, T0>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		
		public void QueryParallel<TData, T0>(TData data, QueryDelegateEntityData<TData, T0> @delegate, QueryDescription? query = null)
			where T0 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper1ED<TData, T0>, T0>(
				new QueryDelegateWrapper1ED<TData, T0>(data, @delegate),
				query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		
		public void Query<TData, T0>(TData data, QueryDelegateData<TData, T0> @delegate, ref QueryDescription? query)
			where T0 : IComponent
		{
			Execute<QueryDelegateWrapper1D<TData, T0>, T0>(
				new QueryDelegateWrapper1D<TData, T0>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		
		public void Query<TData, T0>(TData data, QueryDelegateData<TData, T0> @delegate, QueryDescription? query = null)
			where T0 : IComponent
		{
			Execute<QueryDelegateWrapper1D<TData, T0>, T0>(
				new QueryDelegateWrapper1D<TData, T0>(data, @delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		
		public void QueryParallel<TData, T0>(TData data, QueryDelegateData<TData, T0> @delegate, QueryDescription? query = null)
			where T0 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper1D<TData, T0>, T0>(
				new QueryDelegateWrapper1D<TData, T0>(data, @delegate),
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
	public delegate void QueryDelegate<T0, T1>(
		ref T0 t0, ref T1 t1
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity and the components requested.
	/// </summary>
	public delegate void QueryDelegateEntity<T0, T1>(
		Entity entity,
		ref T0 t0, ref T1 t1
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateData<in TData, T0, T1>(
		TData data,
		ref T0 t0, ref T1 t1
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity, a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateEntityData<in TData, T0, T1>(
		TData data,
		Entity entity,
		ref T0 t0, ref T1 t1
	);

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper2E<T0, T1>
		: IQuery<T0, T1>
		where T0 : IComponent
            where T1 : IComponent
	{
		private readonly QueryDelegateEntity<T0, T1> _delegate;

		public QueryDelegateWrapper2E(QueryDelegateEntity<T0, T1> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1)
		{
			_delegate(e, ref t0, ref t1);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper2<T0, T1>
		: IQuery<T0, T1>
		where T0 : IComponent
            where T1 : IComponent
	{
		private readonly QueryDelegate<T0, T1> _delegate;

		public QueryDelegateWrapper2(QueryDelegate<T0, T1> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1)
		{
			_delegate(ref t0, ref t1);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper2ED<TData, T0, T1>
		: IQuery<T0, T1>
		where T0 : IComponent
            where T1 : IComponent
	{
		private readonly QueryDelegateEntityData<TData, T0, T1> _delegate;
		private readonly TData _data;

		public QueryDelegateWrapper2ED(TData data, QueryDelegateEntityData<TData, T0, T1> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1)
		{
			_delegate(_data, e, ref t0, ref t1);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper2D<TData, T0, T1>
		: IQuery<T0, T1>
		where T0 : IComponent
            where T1 : IComponent
	{
		private readonly QueryDelegateData<TData, T0, T1> _delegate;
		private readonly TData _data;

		public QueryDelegateWrapper2D(TData data, QueryDelegateData<TData, T0, T1> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1)
		{
			_delegate(_data, ref t0, ref t1);
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
		public void Query<T0, T1>(QueryDelegateEntity<T0, T1> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
		{
			Execute<QueryDelegateWrapper2E<T0, T1>, T0, T1>(
				new QueryDelegateWrapper2E<T0, T1>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void Query<T0, T1>(QueryDelegateEntity<T0, T1> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
		{
			Execute<QueryDelegateWrapper2E<T0, T1>, T0, T1>(
				new QueryDelegateWrapper2E<T0, T1>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void QueryParallel<T0, T1>(QueryDelegateEntity<T0, T1> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper2E<T0, T1>, T0, T1>(
				new QueryDelegateWrapper2E<T0, T1>(@delegate),
				query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void Query<T0, T1>(QueryDelegate<T0, T1> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
		{
			Execute<QueryDelegateWrapper2<T0, T1>, T0, T1>(
				new QueryDelegateWrapper2<T0, T1>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void Query<T0, T1>(QueryDelegate<T0, T1> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
		{
			Execute<QueryDelegateWrapper2<T0, T1>, T0, T1>(
				new QueryDelegateWrapper2<T0, T1>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void QueryParallel<T0, T1>(QueryDelegate<T0, T1> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper2<T0, T1>, T0, T1>(
				new QueryDelegateWrapper2<T0, T1>(@delegate),
				query
			);
		}

		// --- //

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		public void Query<TData, T0, T1>(TData data, QueryDelegateEntityData<TData, T0, T1> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
		{
			Execute<QueryDelegateWrapper2ED<TData, T0, T1>, T0, T1>(
				new QueryDelegateWrapper2ED<TData, T0, T1>(data, @delegate),
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
		public void Query<TData, T0, T1>(TData data, QueryDelegateEntityData<TData, T0, T1> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
		{
			Execute<QueryDelegateWrapper2ED<TData, T0, T1>, T0, T1>(
				new QueryDelegateWrapper2ED<TData, T0, T1>(data, @delegate),
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
		public void QueryParallel<TData, T0, T1>(TData data, QueryDelegateEntityData<TData, T0, T1> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper2ED<TData, T0, T1>, T0, T1>(
				new QueryDelegateWrapper2ED<TData, T0, T1>(data, @delegate),
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
		public void Query<TData, T0, T1>(TData data, QueryDelegateData<TData, T0, T1> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
		{
			Execute<QueryDelegateWrapper2D<TData, T0, T1>, T0, T1>(
				new QueryDelegateWrapper2D<TData, T0, T1>(data, @delegate),
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
		public void Query<TData, T0, T1>(TData data, QueryDelegateData<TData, T0, T1> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
		{
			Execute<QueryDelegateWrapper2D<TData, T0, T1>, T0, T1>(
				new QueryDelegateWrapper2D<TData, T0, T1>(data, @delegate),
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
		public void QueryParallel<TData, T0, T1>(TData data, QueryDelegateData<TData, T0, T1> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper2D<TData, T0, T1>, T0, T1>(
				new QueryDelegateWrapper2D<TData, T0, T1>(data, @delegate),
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
	public delegate void QueryDelegate<T0, T1, T2>(
		ref T0 t0, ref T1 t1, ref T2 t2
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity and the components requested.
	/// </summary>
	public delegate void QueryDelegateEntity<T0, T1, T2>(
		Entity entity,
		ref T0 t0, ref T1 t1, ref T2 t2
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateData<in TData, T0, T1, T2>(
		TData data,
		ref T0 t0, ref T1 t1, ref T2 t2
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity, a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateEntityData<in TData, T0, T1, T2>(
		TData data,
		Entity entity,
		ref T0 t0, ref T1 t1, ref T2 t2
	);

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper3E<T0, T1, T2>
		: IQuery<T0, T1, T2>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
	{
		private readonly QueryDelegateEntity<T0, T1, T2> _delegate;

		public QueryDelegateWrapper3E(QueryDelegateEntity<T0, T1, T2> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2)
		{
			_delegate(e, ref t0, ref t1, ref t2);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper3<T0, T1, T2>
		: IQuery<T0, T1, T2>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
	{
		private readonly QueryDelegate<T0, T1, T2> _delegate;

		public QueryDelegateWrapper3(QueryDelegate<T0, T1, T2> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2)
		{
			_delegate(ref t0, ref t1, ref t2);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper3ED<TData, T0, T1, T2>
		: IQuery<T0, T1, T2>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
	{
		private readonly QueryDelegateEntityData<TData, T0, T1, T2> _delegate;
		private readonly TData _data;

		public QueryDelegateWrapper3ED(TData data, QueryDelegateEntityData<TData, T0, T1, T2> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2)
		{
			_delegate(_data, e, ref t0, ref t1, ref t2);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper3D<TData, T0, T1, T2>
		: IQuery<T0, T1, T2>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
	{
		private readonly QueryDelegateData<TData, T0, T1, T2> _delegate;
		private readonly TData _data;

		public QueryDelegateWrapper3D(TData data, QueryDelegateData<TData, T0, T1, T2> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2)
		{
			_delegate(_data, ref t0, ref t1, ref t2);
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
		public void Query<T0, T1, T2>(QueryDelegateEntity<T0, T1, T2> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
		{
			Execute<QueryDelegateWrapper3E<T0, T1, T2>, T0, T1, T2>(
				new QueryDelegateWrapper3E<T0, T1, T2>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void Query<T0, T1, T2>(QueryDelegateEntity<T0, T1, T2> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
		{
			Execute<QueryDelegateWrapper3E<T0, T1, T2>, T0, T1, T2>(
				new QueryDelegateWrapper3E<T0, T1, T2>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void QueryParallel<T0, T1, T2>(QueryDelegateEntity<T0, T1, T2> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper3E<T0, T1, T2>, T0, T1, T2>(
				new QueryDelegateWrapper3E<T0, T1, T2>(@delegate),
				query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void Query<T0, T1, T2>(QueryDelegate<T0, T1, T2> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
		{
			Execute<QueryDelegateWrapper3<T0, T1, T2>, T0, T1, T2>(
				new QueryDelegateWrapper3<T0, T1, T2>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void Query<T0, T1, T2>(QueryDelegate<T0, T1, T2> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
		{
			Execute<QueryDelegateWrapper3<T0, T1, T2>, T0, T1, T2>(
				new QueryDelegateWrapper3<T0, T1, T2>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void QueryParallel<T0, T1, T2>(QueryDelegate<T0, T1, T2> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper3<T0, T1, T2>, T0, T1, T2>(
				new QueryDelegateWrapper3<T0, T1, T2>(@delegate),
				query
			);
		}

		// --- //

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		public void Query<TData, T0, T1, T2>(TData data, QueryDelegateEntityData<TData, T0, T1, T2> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
		{
			Execute<QueryDelegateWrapper3ED<TData, T0, T1, T2>, T0, T1, T2>(
				new QueryDelegateWrapper3ED<TData, T0, T1, T2>(data, @delegate),
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
		public void Query<TData, T0, T1, T2>(TData data, QueryDelegateEntityData<TData, T0, T1, T2> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
		{
			Execute<QueryDelegateWrapper3ED<TData, T0, T1, T2>, T0, T1, T2>(
				new QueryDelegateWrapper3ED<TData, T0, T1, T2>(data, @delegate),
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
		public void QueryParallel<TData, T0, T1, T2>(TData data, QueryDelegateEntityData<TData, T0, T1, T2> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper3ED<TData, T0, T1, T2>, T0, T1, T2>(
				new QueryDelegateWrapper3ED<TData, T0, T1, T2>(data, @delegate),
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
		public void Query<TData, T0, T1, T2>(TData data, QueryDelegateData<TData, T0, T1, T2> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
		{
			Execute<QueryDelegateWrapper3D<TData, T0, T1, T2>, T0, T1, T2>(
				new QueryDelegateWrapper3D<TData, T0, T1, T2>(data, @delegate),
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
		public void Query<TData, T0, T1, T2>(TData data, QueryDelegateData<TData, T0, T1, T2> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
		{
			Execute<QueryDelegateWrapper3D<TData, T0, T1, T2>, T0, T1, T2>(
				new QueryDelegateWrapper3D<TData, T0, T1, T2>(data, @delegate),
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
		public void QueryParallel<TData, T0, T1, T2>(TData data, QueryDelegateData<TData, T0, T1, T2> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper3D<TData, T0, T1, T2>, T0, T1, T2>(
				new QueryDelegateWrapper3D<TData, T0, T1, T2>(data, @delegate),
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
	public delegate void QueryDelegate<T0, T1, T2, T3>(
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity and the components requested.
	/// </summary>
	public delegate void QueryDelegateEntity<T0, T1, T2, T3>(
		Entity entity,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateData<in TData, T0, T1, T2, T3>(
		TData data,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity, a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateEntityData<in TData, T0, T1, T2, T3>(
		TData data,
		Entity entity,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3
	);

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper4E<T0, T1, T2, T3>
		: IQuery<T0, T1, T2, T3>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
	{
		private readonly QueryDelegateEntity<T0, T1, T2, T3> _delegate;

		public QueryDelegateWrapper4E(QueryDelegateEntity<T0, T1, T2, T3> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3)
		{
			_delegate(e, ref t0, ref t1, ref t2, ref t3);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper4<T0, T1, T2, T3>
		: IQuery<T0, T1, T2, T3>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
	{
		private readonly QueryDelegate<T0, T1, T2, T3> _delegate;

		public QueryDelegateWrapper4(QueryDelegate<T0, T1, T2, T3> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3)
		{
			_delegate(ref t0, ref t1, ref t2, ref t3);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper4ED<TData, T0, T1, T2, T3>
		: IQuery<T0, T1, T2, T3>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
	{
		private readonly QueryDelegateEntityData<TData, T0, T1, T2, T3> _delegate;
		private readonly TData _data;

		public QueryDelegateWrapper4ED(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3)
		{
			_delegate(_data, e, ref t0, ref t1, ref t2, ref t3);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper4D<TData, T0, T1, T2, T3>
		: IQuery<T0, T1, T2, T3>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
	{
		private readonly QueryDelegateData<TData, T0, T1, T2, T3> _delegate;
		private readonly TData _data;

		public QueryDelegateWrapper4D(TData data, QueryDelegateData<TData, T0, T1, T2, T3> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3)
		{
			_delegate(_data, ref t0, ref t1, ref t2, ref t3);
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
		public void Query<T0, T1, T2, T3>(QueryDelegateEntity<T0, T1, T2, T3> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
		{
			Execute<QueryDelegateWrapper4E<T0, T1, T2, T3>, T0, T1, T2, T3>(
				new QueryDelegateWrapper4E<T0, T1, T2, T3>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void Query<T0, T1, T2, T3>(QueryDelegateEntity<T0, T1, T2, T3> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
		{
			Execute<QueryDelegateWrapper4E<T0, T1, T2, T3>, T0, T1, T2, T3>(
				new QueryDelegateWrapper4E<T0, T1, T2, T3>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void QueryParallel<T0, T1, T2, T3>(QueryDelegateEntity<T0, T1, T2, T3> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper4E<T0, T1, T2, T3>, T0, T1, T2, T3>(
				new QueryDelegateWrapper4E<T0, T1, T2, T3>(@delegate),
				query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void Query<T0, T1, T2, T3>(QueryDelegate<T0, T1, T2, T3> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
		{
			Execute<QueryDelegateWrapper4<T0, T1, T2, T3>, T0, T1, T2, T3>(
				new QueryDelegateWrapper4<T0, T1, T2, T3>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void Query<T0, T1, T2, T3>(QueryDelegate<T0, T1, T2, T3> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
		{
			Execute<QueryDelegateWrapper4<T0, T1, T2, T3>, T0, T1, T2, T3>(
				new QueryDelegateWrapper4<T0, T1, T2, T3>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void QueryParallel<T0, T1, T2, T3>(QueryDelegate<T0, T1, T2, T3> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper4<T0, T1, T2, T3>, T0, T1, T2, T3>(
				new QueryDelegateWrapper4<T0, T1, T2, T3>(@delegate),
				query
			);
		}

		// --- //

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		public void Query<TData, T0, T1, T2, T3>(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
		{
			Execute<QueryDelegateWrapper4ED<TData, T0, T1, T2, T3>, T0, T1, T2, T3>(
				new QueryDelegateWrapper4ED<TData, T0, T1, T2, T3>(data, @delegate),
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
		public void Query<TData, T0, T1, T2, T3>(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
		{
			Execute<QueryDelegateWrapper4ED<TData, T0, T1, T2, T3>, T0, T1, T2, T3>(
				new QueryDelegateWrapper4ED<TData, T0, T1, T2, T3>(data, @delegate),
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
		public void QueryParallel<TData, T0, T1, T2, T3>(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper4ED<TData, T0, T1, T2, T3>, T0, T1, T2, T3>(
				new QueryDelegateWrapper4ED<TData, T0, T1, T2, T3>(data, @delegate),
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
		public void Query<TData, T0, T1, T2, T3>(TData data, QueryDelegateData<TData, T0, T1, T2, T3> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
		{
			Execute<QueryDelegateWrapper4D<TData, T0, T1, T2, T3>, T0, T1, T2, T3>(
				new QueryDelegateWrapper4D<TData, T0, T1, T2, T3>(data, @delegate),
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
		public void Query<TData, T0, T1, T2, T3>(TData data, QueryDelegateData<TData, T0, T1, T2, T3> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
		{
			Execute<QueryDelegateWrapper4D<TData, T0, T1, T2, T3>, T0, T1, T2, T3>(
				new QueryDelegateWrapper4D<TData, T0, T1, T2, T3>(data, @delegate),
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
		public void QueryParallel<TData, T0, T1, T2, T3>(TData data, QueryDelegateData<TData, T0, T1, T2, T3> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper4D<TData, T0, T1, T2, T3>, T0, T1, T2, T3>(
				new QueryDelegateWrapper4D<TData, T0, T1, T2, T3>(data, @delegate),
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
	public delegate void QueryDelegate<T0, T1, T2, T3, T4>(
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity and the components requested.
	/// </summary>
	public delegate void QueryDelegateEntity<T0, T1, T2, T3, T4>(
		Entity entity,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateData<in TData, T0, T1, T2, T3, T4>(
		TData data,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity, a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateEntityData<in TData, T0, T1, T2, T3, T4>(
		TData data,
		Entity entity,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4
	);

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper5E<T0, T1, T2, T3, T4>
		: IQuery<T0, T1, T2, T3, T4>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
	{
		private readonly QueryDelegateEntity<T0, T1, T2, T3, T4> _delegate;

		public QueryDelegateWrapper5E(QueryDelegateEntity<T0, T1, T2, T3, T4> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4)
		{
			_delegate(e, ref t0, ref t1, ref t2, ref t3, ref t4);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper5<T0, T1, T2, T3, T4>
		: IQuery<T0, T1, T2, T3, T4>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
	{
		private readonly QueryDelegate<T0, T1, T2, T3, T4> _delegate;

		public QueryDelegateWrapper5(QueryDelegate<T0, T1, T2, T3, T4> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4)
		{
			_delegate(ref t0, ref t1, ref t2, ref t3, ref t4);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper5ED<TData, T0, T1, T2, T3, T4>
		: IQuery<T0, T1, T2, T3, T4>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
	{
		private readonly QueryDelegateEntityData<TData, T0, T1, T2, T3, T4> _delegate;
		private readonly TData _data;

		public QueryDelegateWrapper5ED(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4)
		{
			_delegate(_data, e, ref t0, ref t1, ref t2, ref t3, ref t4);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper5D<TData, T0, T1, T2, T3, T4>
		: IQuery<T0, T1, T2, T3, T4>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
	{
		private readonly QueryDelegateData<TData, T0, T1, T2, T3, T4> _delegate;
		private readonly TData _data;

		public QueryDelegateWrapper5D(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4)
		{
			_delegate(_data, ref t0, ref t1, ref t2, ref t3, ref t4);
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
		public void Query<T0, T1, T2, T3, T4>(QueryDelegateEntity<T0, T1, T2, T3, T4> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
		{
			Execute<QueryDelegateWrapper5E<T0, T1, T2, T3, T4>, T0, T1, T2, T3, T4>(
				new QueryDelegateWrapper5E<T0, T1, T2, T3, T4>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void Query<T0, T1, T2, T3, T4>(QueryDelegateEntity<T0, T1, T2, T3, T4> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
		{
			Execute<QueryDelegateWrapper5E<T0, T1, T2, T3, T4>, T0, T1, T2, T3, T4>(
				new QueryDelegateWrapper5E<T0, T1, T2, T3, T4>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void QueryParallel<T0, T1, T2, T3, T4>(QueryDelegateEntity<T0, T1, T2, T3, T4> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper5E<T0, T1, T2, T3, T4>, T0, T1, T2, T3, T4>(
				new QueryDelegateWrapper5E<T0, T1, T2, T3, T4>(@delegate),
				query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void Query<T0, T1, T2, T3, T4>(QueryDelegate<T0, T1, T2, T3, T4> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
		{
			Execute<QueryDelegateWrapper5<T0, T1, T2, T3, T4>, T0, T1, T2, T3, T4>(
				new QueryDelegateWrapper5<T0, T1, T2, T3, T4>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void Query<T0, T1, T2, T3, T4>(QueryDelegate<T0, T1, T2, T3, T4> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
		{
			Execute<QueryDelegateWrapper5<T0, T1, T2, T3, T4>, T0, T1, T2, T3, T4>(
				new QueryDelegateWrapper5<T0, T1, T2, T3, T4>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void QueryParallel<T0, T1, T2, T3, T4>(QueryDelegate<T0, T1, T2, T3, T4> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper5<T0, T1, T2, T3, T4>, T0, T1, T2, T3, T4>(
				new QueryDelegateWrapper5<T0, T1, T2, T3, T4>(@delegate),
				query
			);
		}

		// --- //

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		public void Query<TData, T0, T1, T2, T3, T4>(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
		{
			Execute<QueryDelegateWrapper5ED<TData, T0, T1, T2, T3, T4>, T0, T1, T2, T3, T4>(
				new QueryDelegateWrapper5ED<TData, T0, T1, T2, T3, T4>(data, @delegate),
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
		public void Query<TData, T0, T1, T2, T3, T4>(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
		{
			Execute<QueryDelegateWrapper5ED<TData, T0, T1, T2, T3, T4>, T0, T1, T2, T3, T4>(
				new QueryDelegateWrapper5ED<TData, T0, T1, T2, T3, T4>(data, @delegate),
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
		public void QueryParallel<TData, T0, T1, T2, T3, T4>(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper5ED<TData, T0, T1, T2, T3, T4>, T0, T1, T2, T3, T4>(
				new QueryDelegateWrapper5ED<TData, T0, T1, T2, T3, T4>(data, @delegate),
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
		public void Query<TData, T0, T1, T2, T3, T4>(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
		{
			Execute<QueryDelegateWrapper5D<TData, T0, T1, T2, T3, T4>, T0, T1, T2, T3, T4>(
				new QueryDelegateWrapper5D<TData, T0, T1, T2, T3, T4>(data, @delegate),
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
		public void Query<TData, T0, T1, T2, T3, T4>(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
		{
			Execute<QueryDelegateWrapper5D<TData, T0, T1, T2, T3, T4>, T0, T1, T2, T3, T4>(
				new QueryDelegateWrapper5D<TData, T0, T1, T2, T3, T4>(data, @delegate),
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
		public void QueryParallel<TData, T0, T1, T2, T3, T4>(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper5D<TData, T0, T1, T2, T3, T4>, T0, T1, T2, T3, T4>(
				new QueryDelegateWrapper5D<TData, T0, T1, T2, T3, T4>(data, @delegate),
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
	public delegate void QueryDelegate<T0, T1, T2, T3, T4, T5>(
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity and the components requested.
	/// </summary>
	public delegate void QueryDelegateEntity<T0, T1, T2, T3, T4, T5>(
		Entity entity,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateData<in TData, T0, T1, T2, T3, T4, T5>(
		TData data,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity, a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateEntityData<in TData, T0, T1, T2, T3, T4, T5>(
		TData data,
		Entity entity,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5
	);

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper6E<T0, T1, T2, T3, T4, T5>
		: IQuery<T0, T1, T2, T3, T4, T5>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
	{
		private readonly QueryDelegateEntity<T0, T1, T2, T3, T4, T5> _delegate;

		public QueryDelegateWrapper6E(QueryDelegateEntity<T0, T1, T2, T3, T4, T5> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5)
		{
			_delegate(e, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper6<T0, T1, T2, T3, T4, T5>
		: IQuery<T0, T1, T2, T3, T4, T5>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
	{
		private readonly QueryDelegate<T0, T1, T2, T3, T4, T5> _delegate;

		public QueryDelegateWrapper6(QueryDelegate<T0, T1, T2, T3, T4, T5> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5)
		{
			_delegate(ref t0, ref t1, ref t2, ref t3, ref t4, ref t5);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper6ED<TData, T0, T1, T2, T3, T4, T5>
		: IQuery<T0, T1, T2, T3, T4, T5>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
	{
		private readonly QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5> _delegate;
		private readonly TData _data;

		public QueryDelegateWrapper6ED(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5)
		{
			_delegate(_data, e, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper6D<TData, T0, T1, T2, T3, T4, T5>
		: IQuery<T0, T1, T2, T3, T4, T5>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
	{
		private readonly QueryDelegateData<TData, T0, T1, T2, T3, T4, T5> _delegate;
		private readonly TData _data;

		public QueryDelegateWrapper6D(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4, T5> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5)
		{
			_delegate(_data, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5);
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
		public void Query<T0, T1, T2, T3, T4, T5>(QueryDelegateEntity<T0, T1, T2, T3, T4, T5> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
		{
			Execute<QueryDelegateWrapper6E<T0, T1, T2, T3, T4, T5>, T0, T1, T2, T3, T4, T5>(
				new QueryDelegateWrapper6E<T0, T1, T2, T3, T4, T5>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void Query<T0, T1, T2, T3, T4, T5>(QueryDelegateEntity<T0, T1, T2, T3, T4, T5> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
		{
			Execute<QueryDelegateWrapper6E<T0, T1, T2, T3, T4, T5>, T0, T1, T2, T3, T4, T5>(
				new QueryDelegateWrapper6E<T0, T1, T2, T3, T4, T5>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void QueryParallel<T0, T1, T2, T3, T4, T5>(QueryDelegateEntity<T0, T1, T2, T3, T4, T5> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper6E<T0, T1, T2, T3, T4, T5>, T0, T1, T2, T3, T4, T5>(
				new QueryDelegateWrapper6E<T0, T1, T2, T3, T4, T5>(@delegate),
				query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void Query<T0, T1, T2, T3, T4, T5>(QueryDelegate<T0, T1, T2, T3, T4, T5> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
		{
			Execute<QueryDelegateWrapper6<T0, T1, T2, T3, T4, T5>, T0, T1, T2, T3, T4, T5>(
				new QueryDelegateWrapper6<T0, T1, T2, T3, T4, T5>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void Query<T0, T1, T2, T3, T4, T5>(QueryDelegate<T0, T1, T2, T3, T4, T5> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
		{
			Execute<QueryDelegateWrapper6<T0, T1, T2, T3, T4, T5>, T0, T1, T2, T3, T4, T5>(
				new QueryDelegateWrapper6<T0, T1, T2, T3, T4, T5>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void QueryParallel<T0, T1, T2, T3, T4, T5>(QueryDelegate<T0, T1, T2, T3, T4, T5> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper6<T0, T1, T2, T3, T4, T5>, T0, T1, T2, T3, T4, T5>(
				new QueryDelegateWrapper6<T0, T1, T2, T3, T4, T5>(@delegate),
				query
			);
		}

		// --- //

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		public void Query<TData, T0, T1, T2, T3, T4, T5>(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
		{
			Execute<QueryDelegateWrapper6ED<TData, T0, T1, T2, T3, T4, T5>, T0, T1, T2, T3, T4, T5>(
				new QueryDelegateWrapper6ED<TData, T0, T1, T2, T3, T4, T5>(data, @delegate),
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
		public void Query<TData, T0, T1, T2, T3, T4, T5>(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
		{
			Execute<QueryDelegateWrapper6ED<TData, T0, T1, T2, T3, T4, T5>, T0, T1, T2, T3, T4, T5>(
				new QueryDelegateWrapper6ED<TData, T0, T1, T2, T3, T4, T5>(data, @delegate),
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
		public void QueryParallel<TData, T0, T1, T2, T3, T4, T5>(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper6ED<TData, T0, T1, T2, T3, T4, T5>, T0, T1, T2, T3, T4, T5>(
				new QueryDelegateWrapper6ED<TData, T0, T1, T2, T3, T4, T5>(data, @delegate),
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
		public void Query<TData, T0, T1, T2, T3, T4, T5>(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4, T5> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
		{
			Execute<QueryDelegateWrapper6D<TData, T0, T1, T2, T3, T4, T5>, T0, T1, T2, T3, T4, T5>(
				new QueryDelegateWrapper6D<TData, T0, T1, T2, T3, T4, T5>(data, @delegate),
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
		public void Query<TData, T0, T1, T2, T3, T4, T5>(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4, T5> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
		{
			Execute<QueryDelegateWrapper6D<TData, T0, T1, T2, T3, T4, T5>, T0, T1, T2, T3, T4, T5>(
				new QueryDelegateWrapper6D<TData, T0, T1, T2, T3, T4, T5>(data, @delegate),
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
		public void QueryParallel<TData, T0, T1, T2, T3, T4, T5>(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4, T5> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper6D<TData, T0, T1, T2, T3, T4, T5>, T0, T1, T2, T3, T4, T5>(
				new QueryDelegateWrapper6D<TData, T0, T1, T2, T3, T4, T5>(data, @delegate),
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
	public delegate void QueryDelegate<T0, T1, T2, T3, T4, T5, T6>(
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity and the components requested.
	/// </summary>
	public delegate void QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6>(
		Entity entity,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateData<in TData, T0, T1, T2, T3, T4, T5, T6>(
		TData data,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity, a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateEntityData<in TData, T0, T1, T2, T3, T4, T5, T6>(
		TData data,
		Entity entity,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6
	);

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper7E<T0, T1, T2, T3, T4, T5, T6>
		: IQuery<T0, T1, T2, T3, T4, T5, T6>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
	{
		private readonly QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6> _delegate;

		public QueryDelegateWrapper7E(QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6)
		{
			_delegate(e, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper7<T0, T1, T2, T3, T4, T5, T6>
		: IQuery<T0, T1, T2, T3, T4, T5, T6>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
	{
		private readonly QueryDelegate<T0, T1, T2, T3, T4, T5, T6> _delegate;

		public QueryDelegateWrapper7(QueryDelegate<T0, T1, T2, T3, T4, T5, T6> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6)
		{
			_delegate(ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper7ED<TData, T0, T1, T2, T3, T4, T5, T6>
		: IQuery<T0, T1, T2, T3, T4, T5, T6>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
	{
		private readonly QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6> _delegate;
		private readonly TData _data;

		public QueryDelegateWrapper7ED(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6)
		{
			_delegate(_data, e, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper7D<TData, T0, T1, T2, T3, T4, T5, T6>
		: IQuery<T0, T1, T2, T3, T4, T5, T6>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
	{
		private readonly QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6> _delegate;
		private readonly TData _data;

		public QueryDelegateWrapper7D(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6)
		{
			_delegate(_data, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6);
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
		public void Query<T0, T1, T2, T3, T4, T5, T6>(QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
		{
			Execute<QueryDelegateWrapper7E<T0, T1, T2, T3, T4, T5, T6>, T0, T1, T2, T3, T4, T5, T6>(
				new QueryDelegateWrapper7E<T0, T1, T2, T3, T4, T5, T6>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void Query<T0, T1, T2, T3, T4, T5, T6>(QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
		{
			Execute<QueryDelegateWrapper7E<T0, T1, T2, T3, T4, T5, T6>, T0, T1, T2, T3, T4, T5, T6>(
				new QueryDelegateWrapper7E<T0, T1, T2, T3, T4, T5, T6>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void QueryParallel<T0, T1, T2, T3, T4, T5, T6>(QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper7E<T0, T1, T2, T3, T4, T5, T6>, T0, T1, T2, T3, T4, T5, T6>(
				new QueryDelegateWrapper7E<T0, T1, T2, T3, T4, T5, T6>(@delegate),
				query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void Query<T0, T1, T2, T3, T4, T5, T6>(QueryDelegate<T0, T1, T2, T3, T4, T5, T6> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
		{
			Execute<QueryDelegateWrapper7<T0, T1, T2, T3, T4, T5, T6>, T0, T1, T2, T3, T4, T5, T6>(
				new QueryDelegateWrapper7<T0, T1, T2, T3, T4, T5, T6>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void Query<T0, T1, T2, T3, T4, T5, T6>(QueryDelegate<T0, T1, T2, T3, T4, T5, T6> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
		{
			Execute<QueryDelegateWrapper7<T0, T1, T2, T3, T4, T5, T6>, T0, T1, T2, T3, T4, T5, T6>(
				new QueryDelegateWrapper7<T0, T1, T2, T3, T4, T5, T6>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void QueryParallel<T0, T1, T2, T3, T4, T5, T6>(QueryDelegate<T0, T1, T2, T3, T4, T5, T6> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper7<T0, T1, T2, T3, T4, T5, T6>, T0, T1, T2, T3, T4, T5, T6>(
				new QueryDelegateWrapper7<T0, T1, T2, T3, T4, T5, T6>(@delegate),
				query
			);
		}

		// --- //

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		public void Query<TData, T0, T1, T2, T3, T4, T5, T6>(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
		{
			Execute<QueryDelegateWrapper7ED<TData, T0, T1, T2, T3, T4, T5, T6>, T0, T1, T2, T3, T4, T5, T6>(
				new QueryDelegateWrapper7ED<TData, T0, T1, T2, T3, T4, T5, T6>(data, @delegate),
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
		public void Query<TData, T0, T1, T2, T3, T4, T5, T6>(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
		{
			Execute<QueryDelegateWrapper7ED<TData, T0, T1, T2, T3, T4, T5, T6>, T0, T1, T2, T3, T4, T5, T6>(
				new QueryDelegateWrapper7ED<TData, T0, T1, T2, T3, T4, T5, T6>(data, @delegate),
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
		public void QueryParallel<TData, T0, T1, T2, T3, T4, T5, T6>(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper7ED<TData, T0, T1, T2, T3, T4, T5, T6>, T0, T1, T2, T3, T4, T5, T6>(
				new QueryDelegateWrapper7ED<TData, T0, T1, T2, T3, T4, T5, T6>(data, @delegate),
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
		public void Query<TData, T0, T1, T2, T3, T4, T5, T6>(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
		{
			Execute<QueryDelegateWrapper7D<TData, T0, T1, T2, T3, T4, T5, T6>, T0, T1, T2, T3, T4, T5, T6>(
				new QueryDelegateWrapper7D<TData, T0, T1, T2, T3, T4, T5, T6>(data, @delegate),
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
		public void Query<TData, T0, T1, T2, T3, T4, T5, T6>(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
		{
			Execute<QueryDelegateWrapper7D<TData, T0, T1, T2, T3, T4, T5, T6>, T0, T1, T2, T3, T4, T5, T6>(
				new QueryDelegateWrapper7D<TData, T0, T1, T2, T3, T4, T5, T6>(data, @delegate),
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
		public void QueryParallel<TData, T0, T1, T2, T3, T4, T5, T6>(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper7D<TData, T0, T1, T2, T3, T4, T5, T6>, T0, T1, T2, T3, T4, T5, T6>(
				new QueryDelegateWrapper7D<TData, T0, T1, T2, T3, T4, T5, T6>(data, @delegate),
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
	public delegate void QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7>(
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity and the components requested.
	/// </summary>
	public delegate void QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7>(
		Entity entity,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateData<in TData, T0, T1, T2, T3, T4, T5, T6, T7>(
		TData data,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity, a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateEntityData<in TData, T0, T1, T2, T3, T4, T5, T6, T7>(
		TData data,
		Entity entity,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7
	);

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper8E<T0, T1, T2, T3, T4, T5, T6, T7>
		: IQuery<T0, T1, T2, T3, T4, T5, T6, T7>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
	{
		private readonly QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7> _delegate;

		public QueryDelegateWrapper8E(QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7)
		{
			_delegate(e, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper8<T0, T1, T2, T3, T4, T5, T6, T7>
		: IQuery<T0, T1, T2, T3, T4, T5, T6, T7>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
	{
		private readonly QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7> _delegate;

		public QueryDelegateWrapper8(QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7)
		{
			_delegate(ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper8ED<TData, T0, T1, T2, T3, T4, T5, T6, T7>
		: IQuery<T0, T1, T2, T3, T4, T5, T6, T7>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
	{
		private readonly QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6, T7> _delegate;
		private readonly TData _data;

		public QueryDelegateWrapper8ED(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6, T7> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7)
		{
			_delegate(_data, e, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper8D<TData, T0, T1, T2, T3, T4, T5, T6, T7>
		: IQuery<T0, T1, T2, T3, T4, T5, T6, T7>
		where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
	{
		private readonly QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6, T7> _delegate;
		private readonly TData _data;

		public QueryDelegateWrapper8D(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6, T7> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7)
		{
			_delegate(_data, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7);
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
		public void Query<T0, T1, T2, T3, T4, T5, T6, T7>(QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
		{
			Execute<QueryDelegateWrapper8E<T0, T1, T2, T3, T4, T5, T6, T7>, T0, T1, T2, T3, T4, T5, T6, T7>(
				new QueryDelegateWrapper8E<T0, T1, T2, T3, T4, T5, T6, T7>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void Query<T0, T1, T2, T3, T4, T5, T6, T7>(QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
		{
			Execute<QueryDelegateWrapper8E<T0, T1, T2, T3, T4, T5, T6, T7>, T0, T1, T2, T3, T4, T5, T6, T7>(
				new QueryDelegateWrapper8E<T0, T1, T2, T3, T4, T5, T6, T7>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void QueryParallel<T0, T1, T2, T3, T4, T5, T6, T7>(QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper8E<T0, T1, T2, T3, T4, T5, T6, T7>, T0, T1, T2, T3, T4, T5, T6, T7>(
				new QueryDelegateWrapper8E<T0, T1, T2, T3, T4, T5, T6, T7>(@delegate),
				query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void Query<T0, T1, T2, T3, T4, T5, T6, T7>(QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
		{
			Execute<QueryDelegateWrapper8<T0, T1, T2, T3, T4, T5, T6, T7>, T0, T1, T2, T3, T4, T5, T6, T7>(
				new QueryDelegateWrapper8<T0, T1, T2, T3, T4, T5, T6, T7>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void Query<T0, T1, T2, T3, T4, T5, T6, T7>(QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
		{
			Execute<QueryDelegateWrapper8<T0, T1, T2, T3, T4, T5, T6, T7>, T0, T1, T2, T3, T4, T5, T6, T7>(
				new QueryDelegateWrapper8<T0, T1, T2, T3, T4, T5, T6, T7>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void QueryParallel<T0, T1, T2, T3, T4, T5, T6, T7>(QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper8<T0, T1, T2, T3, T4, T5, T6, T7>, T0, T1, T2, T3, T4, T5, T6, T7>(
				new QueryDelegateWrapper8<T0, T1, T2, T3, T4, T5, T6, T7>(@delegate),
				query
			);
		}

		// --- //

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		public void Query<TData, T0, T1, T2, T3, T4, T5, T6, T7>(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6, T7> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
		{
			Execute<QueryDelegateWrapper8ED<TData, T0, T1, T2, T3, T4, T5, T6, T7>, T0, T1, T2, T3, T4, T5, T6, T7>(
				new QueryDelegateWrapper8ED<TData, T0, T1, T2, T3, T4, T5, T6, T7>(data, @delegate),
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
		public void Query<TData, T0, T1, T2, T3, T4, T5, T6, T7>(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6, T7> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
		{
			Execute<QueryDelegateWrapper8ED<TData, T0, T1, T2, T3, T4, T5, T6, T7>, T0, T1, T2, T3, T4, T5, T6, T7>(
				new QueryDelegateWrapper8ED<TData, T0, T1, T2, T3, T4, T5, T6, T7>(data, @delegate),
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
		public void QueryParallel<TData, T0, T1, T2, T3, T4, T5, T6, T7>(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6, T7> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper8ED<TData, T0, T1, T2, T3, T4, T5, T6, T7>, T0, T1, T2, T3, T4, T5, T6, T7>(
				new QueryDelegateWrapper8ED<TData, T0, T1, T2, T3, T4, T5, T6, T7>(data, @delegate),
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
		public void Query<TData, T0, T1, T2, T3, T4, T5, T6, T7>(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6, T7> @delegate, ref QueryDescription? query)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
		{
			Execute<QueryDelegateWrapper8D<TData, T0, T1, T2, T3, T4, T5, T6, T7>, T0, T1, T2, T3, T4, T5, T6, T7>(
				new QueryDelegateWrapper8D<TData, T0, T1, T2, T3, T4, T5, T6, T7>(data, @delegate),
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
		public void Query<TData, T0, T1, T2, T3, T4, T5, T6, T7>(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6, T7> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
		{
			Execute<QueryDelegateWrapper8D<TData, T0, T1, T2, T3, T4, T5, T6, T7>, T0, T1, T2, T3, T4, T5, T6, T7>(
				new QueryDelegateWrapper8D<TData, T0, T1, T2, T3, T4, T5, T6, T7>(data, @delegate),
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
		public void QueryParallel<TData, T0, T1, T2, T3, T4, T5, T6, T7>(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6, T7> @delegate, QueryDescription? query = null)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper8D<TData, T0, T1, T2, T3, T4, T5, T6, T7>, T0, T1, T2, T3, T4, T5, T6, T7>(
				new QueryDelegateWrapper8D<TData, T0, T1, T2, T3, T4, T5, T6, T7>(data, @delegate),
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
	public delegate void QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7, T8>(
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity and the components requested.
	/// </summary>
	public delegate void QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8>(
		Entity entity,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateData<in TData, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
		TData data,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity, a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateEntityData<in TData, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
		TData data,
		Entity entity,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8
	);

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper9E<T0, T1, T2, T3, T4, T5, T6, T7, T8>
		: IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8>
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
		private readonly QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8> _delegate;

		public QueryDelegateWrapper9E(QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8)
		{
			_delegate(e, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7, ref t8);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper9<T0, T1, T2, T3, T4, T5, T6, T7, T8>
		: IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8>
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
		private readonly QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7, T8> _delegate;

		public QueryDelegateWrapper9(QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7, T8> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8)
		{
			_delegate(ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7, ref t8);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper9ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8>
		: IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8>
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
		private readonly QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8> _delegate;
		private readonly TData _data;

		public QueryDelegateWrapper9ED(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8)
		{
			_delegate(_data, e, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7, ref t8);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper9D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8>
		: IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8>
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
		private readonly QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8> _delegate;
		private readonly TData _data;

		public QueryDelegateWrapper9D(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8)
		{
			_delegate(_data, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7, ref t8);
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
		public void Query<T0, T1, T2, T3, T4, T5, T6, T7, T8>(QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8> @delegate, ref QueryDescription? query)
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
			Execute<QueryDelegateWrapper9E<T0, T1, T2, T3, T4, T5, T6, T7, T8>, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
				new QueryDelegateWrapper9E<T0, T1, T2, T3, T4, T5, T6, T7, T8>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void Query<T0, T1, T2, T3, T4, T5, T6, T7, T8>(QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8> @delegate, QueryDescription? query = null)
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
			Execute<QueryDelegateWrapper9E<T0, T1, T2, T3, T4, T5, T6, T7, T8>, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
				new QueryDelegateWrapper9E<T0, T1, T2, T3, T4, T5, T6, T7, T8>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void QueryParallel<T0, T1, T2, T3, T4, T5, T6, T7, T8>(QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8> @delegate, QueryDescription? query = null)
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
			ExecuteParallel<QueryDelegateWrapper9E<T0, T1, T2, T3, T4, T5, T6, T7, T8>, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
				new QueryDelegateWrapper9E<T0, T1, T2, T3, T4, T5, T6, T7, T8>(@delegate),
				query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void Query<T0, T1, T2, T3, T4, T5, T6, T7, T8>(QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7, T8> @delegate, ref QueryDescription? query)
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
			Execute<QueryDelegateWrapper9<T0, T1, T2, T3, T4, T5, T6, T7, T8>, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
				new QueryDelegateWrapper9<T0, T1, T2, T3, T4, T5, T6, T7, T8>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void Query<T0, T1, T2, T3, T4, T5, T6, T7, T8>(QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7, T8> @delegate, QueryDescription? query = null)
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
			Execute<QueryDelegateWrapper9<T0, T1, T2, T3, T4, T5, T6, T7, T8>, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
				new QueryDelegateWrapper9<T0, T1, T2, T3, T4, T5, T6, T7, T8>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void QueryParallel<T0, T1, T2, T3, T4, T5, T6, T7, T8>(QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7, T8> @delegate, QueryDescription? query = null)
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
			ExecuteParallel<QueryDelegateWrapper9<T0, T1, T2, T3, T4, T5, T6, T7, T8>, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
				new QueryDelegateWrapper9<T0, T1, T2, T3, T4, T5, T6, T7, T8>(@delegate),
				query
			);
		}

		// --- //

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		public void Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8>(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8> @delegate, ref QueryDescription? query)
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
			Execute<QueryDelegateWrapper9ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8>, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
				new QueryDelegateWrapper9ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8>(data, @delegate),
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
		public void Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8>(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8> @delegate, QueryDescription? query = null)
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
			Execute<QueryDelegateWrapper9ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8>, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
				new QueryDelegateWrapper9ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8>(data, @delegate),
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
		public void QueryParallel<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8>(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8> @delegate, QueryDescription? query = null)
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
			ExecuteParallel<QueryDelegateWrapper9ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8>, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
				new QueryDelegateWrapper9ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8>(data, @delegate),
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
		public void Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8>(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8> @delegate, ref QueryDescription? query)
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
			Execute<QueryDelegateWrapper9D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8>, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
				new QueryDelegateWrapper9D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8>(data, @delegate),
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
		public void Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8>(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8> @delegate, QueryDescription? query = null)
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
			Execute<QueryDelegateWrapper9D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8>, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
				new QueryDelegateWrapper9D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8>(data, @delegate),
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
		public void QueryParallel<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8>(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8> @delegate, QueryDescription? query = null)
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
			ExecuteParallel<QueryDelegateWrapper9D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8>, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
				new QueryDelegateWrapper9D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8>(data, @delegate),
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
	public delegate void QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity and the components requested.
	/// </summary>
	public delegate void QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
		Entity entity,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateData<in TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
		TData data,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity, a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateEntityData<in TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
		TData data,
		Entity entity,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9
	);

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper10E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
		: IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
		where T0 : IComponent
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
		private readonly QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> _delegate;

		public QueryDelegateWrapper10E(QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9)
		{
			_delegate(e, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7, ref t8, ref t9);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
		: IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
		where T0 : IComponent
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
		private readonly QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> _delegate;

		public QueryDelegateWrapper10(QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9)
		{
			_delegate(ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7, ref t8, ref t9);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper10ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
		: IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
		where T0 : IComponent
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
		private readonly QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> _delegate;
		private readonly TData _data;

		public QueryDelegateWrapper10ED(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9)
		{
			_delegate(_data, e, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7, ref t8, ref t9);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper10D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
		: IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
		where T0 : IComponent
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
		private readonly QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> _delegate;
		private readonly TData _data;

		public QueryDelegateWrapper10D(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9)
		{
			_delegate(_data, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7, ref t8, ref t9);
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
		public void Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> @delegate, ref QueryDescription? query)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper10E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
				new QueryDelegateWrapper10E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper10E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
				new QueryDelegateWrapper10E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void QueryParallel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			ExecuteParallel<QueryDelegateWrapper10E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
				new QueryDelegateWrapper10E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(@delegate),
				query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> @delegate, ref QueryDescription? query)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
				new QueryDelegateWrapper10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
				new QueryDelegateWrapper10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void QueryParallel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			ExecuteParallel<QueryDelegateWrapper10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
				new QueryDelegateWrapper10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(@delegate),
				query
			);
		}

		// --- //

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		public void Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> @delegate, ref QueryDescription? query)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper10ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
				new QueryDelegateWrapper10ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(data, @delegate),
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
		public void Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper10ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
				new QueryDelegateWrapper10ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(data, @delegate),
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
		public void QueryParallel<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			ExecuteParallel<QueryDelegateWrapper10ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
				new QueryDelegateWrapper10ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(data, @delegate),
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
		public void Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> @delegate, ref QueryDescription? query)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper10D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
				new QueryDelegateWrapper10D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(data, @delegate),
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
		public void Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper10D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
				new QueryDelegateWrapper10D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(data, @delegate),
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
		public void QueryParallel<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			ExecuteParallel<QueryDelegateWrapper10D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
				new QueryDelegateWrapper10D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(data, @delegate),
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
	public delegate void QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity and the components requested.
	/// </summary>
	public delegate void QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
		Entity entity,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateData<in TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
		TData data,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity, a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateEntityData<in TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
		TData data,
		Entity entity,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10
	);

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper11E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
		: IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
		where T0 : IComponent
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
		private readonly QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> _delegate;

		public QueryDelegateWrapper11E(QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10)
		{
			_delegate(e, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7, ref t8, ref t9, ref t10);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
		: IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
		where T0 : IComponent
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
		private readonly QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> _delegate;

		public QueryDelegateWrapper11(QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10)
		{
			_delegate(ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7, ref t8, ref t9, ref t10);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper11ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
		: IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
		where T0 : IComponent
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
		private readonly QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> _delegate;
		private readonly TData _data;

		public QueryDelegateWrapper11ED(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10)
		{
			_delegate(_data, e, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7, ref t8, ref t9, ref t10);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper11D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
		: IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
		where T0 : IComponent
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
		private readonly QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> _delegate;
		private readonly TData _data;

		public QueryDelegateWrapper11D(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10)
		{
			_delegate(_data, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7, ref t8, ref t9, ref t10);
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
		public void Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> @delegate, ref QueryDescription? query)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper11E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
				new QueryDelegateWrapper11E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper11E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
				new QueryDelegateWrapper11E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void QueryParallel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			ExecuteParallel<QueryDelegateWrapper11E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
				new QueryDelegateWrapper11E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(@delegate),
				query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> @delegate, ref QueryDescription? query)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
				new QueryDelegateWrapper11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
				new QueryDelegateWrapper11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void QueryParallel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			ExecuteParallel<QueryDelegateWrapper11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
				new QueryDelegateWrapper11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(@delegate),
				query
			);
		}

		// --- //

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		public void Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> @delegate, ref QueryDescription? query)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper11ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
				new QueryDelegateWrapper11ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(data, @delegate),
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
		public void Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper11ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
				new QueryDelegateWrapper11ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(data, @delegate),
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
		public void QueryParallel<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			ExecuteParallel<QueryDelegateWrapper11ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
				new QueryDelegateWrapper11ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(data, @delegate),
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
		public void Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> @delegate, ref QueryDescription? query)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper11D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
				new QueryDelegateWrapper11D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(data, @delegate),
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
		public void Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper11D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
				new QueryDelegateWrapper11D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(data, @delegate),
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
		public void QueryParallel<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			ExecuteParallel<QueryDelegateWrapper11D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
				new QueryDelegateWrapper11D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(data, @delegate),
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
	public delegate void QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity and the components requested.
	/// </summary>
	public delegate void QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
		Entity entity,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateData<in TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
		TData data,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity, a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateEntityData<in TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
		TData data,
		Entity entity,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11
	);

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper12E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
		: IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
		where T0 : IComponent
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
		private readonly QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> _delegate;

		public QueryDelegateWrapper12E(QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11)
		{
			_delegate(e, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7, ref t8, ref t9, ref t10, ref t11);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
		: IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
		where T0 : IComponent
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
		private readonly QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> _delegate;

		public QueryDelegateWrapper12(QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11)
		{
			_delegate(ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7, ref t8, ref t9, ref t10, ref t11);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper12ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
		: IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
		where T0 : IComponent
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
		private readonly QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> _delegate;
		private readonly TData _data;

		public QueryDelegateWrapper12ED(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11)
		{
			_delegate(_data, e, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7, ref t8, ref t9, ref t10, ref t11);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper12D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
		: IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
		where T0 : IComponent
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
		private readonly QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> _delegate;
		private readonly TData _data;

		public QueryDelegateWrapper12D(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11)
		{
			_delegate(_data, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7, ref t8, ref t9, ref t10, ref t11);
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
		public void Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> @delegate, ref QueryDescription? query)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper12E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
				new QueryDelegateWrapper12E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper12E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
				new QueryDelegateWrapper12E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void QueryParallel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			ExecuteParallel<QueryDelegateWrapper12E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
				new QueryDelegateWrapper12E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(@delegate),
				query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> @delegate, ref QueryDescription? query)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
				new QueryDelegateWrapper12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
				new QueryDelegateWrapper12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void QueryParallel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			ExecuteParallel<QueryDelegateWrapper12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
				new QueryDelegateWrapper12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(@delegate),
				query
			);
		}

		// --- //

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		public void Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> @delegate, ref QueryDescription? query)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper12ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
				new QueryDelegateWrapper12ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(data, @delegate),
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
		public void Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper12ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
				new QueryDelegateWrapper12ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(data, @delegate),
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
		public void QueryParallel<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			ExecuteParallel<QueryDelegateWrapper12ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
				new QueryDelegateWrapper12ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(data, @delegate),
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
		public void Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> @delegate, ref QueryDescription? query)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper12D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
				new QueryDelegateWrapper12D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(data, @delegate),
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
		public void Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper12D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
				new QueryDelegateWrapper12D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(data, @delegate),
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
		public void QueryParallel<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			ExecuteParallel<QueryDelegateWrapper12D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
				new QueryDelegateWrapper12D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(data, @delegate),
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
	public delegate void QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity and the components requested.
	/// </summary>
	public delegate void QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
		Entity entity,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateData<in TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
		TData data,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity, a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateEntityData<in TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
		TData data,
		Entity entity,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12
	);

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper13E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
		: IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
		where T0 : IComponent
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
		private readonly QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> _delegate;

		public QueryDelegateWrapper13E(QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12)
		{
			_delegate(e, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7, ref t8, ref t9, ref t10, ref t11, ref t12);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
		: IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
		where T0 : IComponent
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
		private readonly QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> _delegate;

		public QueryDelegateWrapper13(QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12)
		{
			_delegate(ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7, ref t8, ref t9, ref t10, ref t11, ref t12);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper13ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
		: IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
		where T0 : IComponent
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
		private readonly QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> _delegate;
		private readonly TData _data;

		public QueryDelegateWrapper13ED(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12)
		{
			_delegate(_data, e, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7, ref t8, ref t9, ref t10, ref t11, ref t12);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper13D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
		: IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
		where T0 : IComponent
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
		private readonly QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> _delegate;
		private readonly TData _data;

		public QueryDelegateWrapper13D(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12)
		{
			_delegate(_data, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7, ref t8, ref t9, ref t10, ref t11, ref t12);
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
		public void Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> @delegate, ref QueryDescription? query)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper13E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
				new QueryDelegateWrapper13E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper13E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
				new QueryDelegateWrapper13E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void QueryParallel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			ExecuteParallel<QueryDelegateWrapper13E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
				new QueryDelegateWrapper13E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(@delegate),
				query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> @delegate, ref QueryDescription? query)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
				new QueryDelegateWrapper13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
				new QueryDelegateWrapper13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void QueryParallel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			ExecuteParallel<QueryDelegateWrapper13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
				new QueryDelegateWrapper13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(@delegate),
				query
			);
		}

		// --- //

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		public void Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> @delegate, ref QueryDescription? query)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper13ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
				new QueryDelegateWrapper13ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(data, @delegate),
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
		public void Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper13ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
				new QueryDelegateWrapper13ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(data, @delegate),
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
		public void QueryParallel<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			ExecuteParallel<QueryDelegateWrapper13ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
				new QueryDelegateWrapper13ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(data, @delegate),
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
		public void Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> @delegate, ref QueryDescription? query)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper13D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
				new QueryDelegateWrapper13D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(data, @delegate),
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
		public void Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper13D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
				new QueryDelegateWrapper13D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(data, @delegate),
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
		public void QueryParallel<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			ExecuteParallel<QueryDelegateWrapper13D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
				new QueryDelegateWrapper13D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(data, @delegate),
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
	public delegate void QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12, ref T13 t13
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity and the components requested.
	/// </summary>
	public delegate void QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
		Entity entity,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12, ref T13 t13
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateData<in TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
		TData data,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12, ref T13 t13
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity, a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateEntityData<in TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
		TData data,
		Entity entity,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12, ref T13 t13
	);

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper14E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
		: IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
		where T0 : IComponent
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
		private readonly QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> _delegate;

		public QueryDelegateWrapper14E(QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12, ref T13 t13)
		{
			_delegate(e, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7, ref t8, ref t9, ref t10, ref t11, ref t12, ref t13);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
		: IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
		where T0 : IComponent
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
		private readonly QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> _delegate;

		public QueryDelegateWrapper14(QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12, ref T13 t13)
		{
			_delegate(ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7, ref t8, ref t9, ref t10, ref t11, ref t12, ref t13);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper14ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
		: IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
		where T0 : IComponent
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
		private readonly QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> _delegate;
		private readonly TData _data;

		public QueryDelegateWrapper14ED(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12, ref T13 t13)
		{
			_delegate(_data, e, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7, ref t8, ref t9, ref t10, ref t11, ref t12, ref t13);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper14D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
		: IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
		where T0 : IComponent
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
		private readonly QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> _delegate;
		private readonly TData _data;

		public QueryDelegateWrapper14D(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12, ref T13 t13)
		{
			_delegate(_data, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7, ref t8, ref t9, ref t10, ref t11, ref t12, ref t13);
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
		public void Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> @delegate, ref QueryDescription? query)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper14E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
				new QueryDelegateWrapper14E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper14E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
				new QueryDelegateWrapper14E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void QueryParallel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			ExecuteParallel<QueryDelegateWrapper14E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
				new QueryDelegateWrapper14E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(@delegate),
				query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> @delegate, ref QueryDescription? query)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
				new QueryDelegateWrapper14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
				new QueryDelegateWrapper14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void QueryParallel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			ExecuteParallel<QueryDelegateWrapper14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
				new QueryDelegateWrapper14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(@delegate),
				query
			);
		}

		// --- //

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		public void Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> @delegate, ref QueryDescription? query)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper14ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
				new QueryDelegateWrapper14ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(data, @delegate),
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
		public void Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper14ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
				new QueryDelegateWrapper14ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(data, @delegate),
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
		public void QueryParallel<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			ExecuteParallel<QueryDelegateWrapper14ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
				new QueryDelegateWrapper14ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(data, @delegate),
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
		public void Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> @delegate, ref QueryDescription? query)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper14D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
				new QueryDelegateWrapper14D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(data, @delegate),
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
		public void Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper14D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
				new QueryDelegateWrapper14D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(data, @delegate),
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
		public void QueryParallel<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			ExecuteParallel<QueryDelegateWrapper14D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
				new QueryDelegateWrapper14D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(data, @delegate),
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
	public delegate void QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12, ref T13 t13, ref T14 t14
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity and the components requested.
	/// </summary>
	public delegate void QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
		Entity entity,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12, ref T13 t13, ref T14 t14
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateData<in TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
		TData data,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12, ref T13 t13, ref T14 t14
	);

	/// <summary>
	/// A delegate called for entities in a query. Receives the entity, a data object passed into the query and the components requested.
	/// </summary>
	public delegate void QueryDelegateEntityData<in TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
		TData data,
		Entity entity,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12, ref T13 t13, ref T14 t14
	);

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper15E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
		: IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
		where T0 : IComponent
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
		private readonly QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> _delegate;

		public QueryDelegateWrapper15E(QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12, ref T13 t13, ref T14 t14)
		{
			_delegate(e, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7, ref t8, ref t9, ref t10, ref t11, ref t12, ref t13, ref t14);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
		: IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
		where T0 : IComponent
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
		private readonly QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> _delegate;

		public QueryDelegateWrapper15(QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> @delegate)
        {
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12, ref T13 t13, ref T14 t14)
		{
			_delegate(ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7, ref t8, ref t9, ref t10, ref t11, ref t12, ref t13, ref t14);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper15ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
		: IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
		where T0 : IComponent
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
		private readonly QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> _delegate;
		private readonly TData _data;

		public QueryDelegateWrapper15ED(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12, ref T13 t13, ref T14 t14)
		{
			_delegate(_data, e, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7, ref t8, ref t9, ref t10, ref t11, ref t12, ref t13, ref t14);
		}
	}

	[ExcludeFromCodeCoverage]
	internal readonly struct QueryDelegateWrapper15D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
		: IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
		where T0 : IComponent
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
		private readonly QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> _delegate;
		private readonly TData _data;

		public QueryDelegateWrapper15D(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> @delegate)
        {
			_data = data;
            _delegate = @delegate;
        }

		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12, ref T13 t13, ref T14 t14)
		{
			_delegate(_data, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7, ref t8, ref t9, ref t10, ref t11, ref t12, ref t13, ref t14);
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
		public void Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> @delegate, ref QueryDescription? query)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper15E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
				new QueryDelegateWrapper15E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper15E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
				new QueryDelegateWrapper15E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void QueryParallel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			ExecuteParallel<QueryDelegateWrapper15E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
				new QueryDelegateWrapper15E<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(@delegate),
				query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> @delegate, ref QueryDescription? query)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
				new QueryDelegateWrapper15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void Query<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
				new QueryDelegateWrapper15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(@delegate),
				ref query
			);
		}

		/// <summary>
		/// Execute a delegate for every entity in a query, in parallel.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[ExcludeFromCodeCoverage]
		public void QueryParallel<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			ExecuteParallel<QueryDelegateWrapper15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
				new QueryDelegateWrapper15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(@delegate),
				query
			);
		}

		// --- //

		/// <summary>
		/// Execute a delegate for every entity in a query. Passing an object through into every call.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		/// <param name="data">The object passed into every call</param>
		[ExcludeFromCodeCoverage]
		public void Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> @delegate, ref QueryDescription? query)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper15ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
				new QueryDelegateWrapper15ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(data, @delegate),
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
		public void Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper15ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
				new QueryDelegateWrapper15ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(data, @delegate),
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
		public void QueryParallel<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			ExecuteParallel<QueryDelegateWrapper15ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
				new QueryDelegateWrapper15ED<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(data, @delegate),
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
		public void Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> @delegate, ref QueryDescription? query)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper15D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
				new QueryDelegateWrapper15D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(data, @delegate),
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
		public void Query<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			Execute<QueryDelegateWrapper15D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
				new QueryDelegateWrapper15D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(data, @delegate),
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
		public void QueryParallel<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> @delegate, QueryDescription? query = null)
			where T0 : IComponent
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
			ExecuteParallel<QueryDelegateWrapper15D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
				new QueryDelegateWrapper15D<TData, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(data, @delegate),
				query
			);
		}
	}
}



