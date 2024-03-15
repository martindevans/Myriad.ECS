using Myriad.ECS.Queries;
using Myriad.ECS.IDs;

using Parallel = System.Threading.Tasks.Parallel;
//using Parallel = ParallelTasks.Parallel;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedParameter.Global
// ReSharper disable LoopCanBeConvertedToQuery
// ReSharper disable ConvertToPrimaryConstructor


namespace Myriad.ECS.Queries
{
	public delegate void QueryDelegate<T0>(
		ref T0 t0
	);

	public delegate void QueryDelegateEntity<T0>(
		Entity entity,
		ref T0 t0
	);

	public delegate void QueryDelegateData<in TData, T0>(
		TData data,
		ref T0 t0
	);

	public delegate void QueryDelegateEntityData<in TData, T0>(
		TData data,
		Entity entity,
		ref T0 t0
	);

	internal readonly struct QueryDelegateWrapper1E<T0>
		: IQuery1<T0>
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
		: IQuery1<T0>
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
		: IQuery1<T0>
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
		: IQuery1<T0>
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
		public void Query<T0>(QueryDelegateEntity<T0> @delegate)
			where T0 : IComponent
		{
			Execute<QueryDelegateWrapper1E<T0>, T0>(
				new QueryDelegateWrapper1E<T0>(@delegate)
			);
		}

		public void QueryParallel<T0>(QueryDelegateEntity<T0> @delegate)
			where T0 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper1E<T0>, T0>(
				new QueryDelegateWrapper1E<T0>(@delegate)
			);
		}

		public void Query<T0>(QueryDelegate<T0> @delegate)
			where T0 : IComponent
		{
			Execute<QueryDelegateWrapper1<T0>, T0>(
				new QueryDelegateWrapper1<T0>(@delegate)
			);
		}

		public void QueryParallel<T0>(QueryDelegate<T0> @delegate)
			where T0 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper1<T0>, T0>(
				new QueryDelegateWrapper1<T0>(@delegate)
			);
		}

		// --- //

		public void Query<TData, T0>(TData data, QueryDelegateEntityData<TData, T0> @delegate)
			where T0 : IComponent
		{
			Execute<QueryDelegateWrapper1ED<TData, T0>, T0>(
				new QueryDelegateWrapper1ED<TData, T0>(data, @delegate)
			);
		}

		public void QueryParallel<TData, T0>(TData data, QueryDelegateEntityData<TData, T0> @delegate)
			where T0 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper1ED<TData, T0>, T0>(
				new QueryDelegateWrapper1ED<TData, T0>(data, @delegate)
			);
		}

		public void Query<TData, T0>(TData data, QueryDelegateData<TData, T0> @delegate)
			where T0 : IComponent
		{
			Execute<QueryDelegateWrapper1D<TData, T0>, T0>(
				new QueryDelegateWrapper1D<TData, T0>(data, @delegate)
			);
		}

		public void QueryParallel<TData, T0>(TData data, QueryDelegateData<TData, T0> @delegate)
			where T0 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper1D<TData, T0>, T0>(
				new QueryDelegateWrapper1D<TData, T0>(data, @delegate)
			);
		}
	}
}

namespace Myriad.ECS.Queries
{
	public delegate void QueryDelegate<T0, T1>(
		ref T0 t0, ref T1 t1
	);

	public delegate void QueryDelegateEntity<T0, T1>(
		Entity entity,
		ref T0 t0, ref T1 t1
	);

	public delegate void QueryDelegateData<in TData, T0, T1>(
		TData data,
		ref T0 t0, ref T1 t1
	);

	public delegate void QueryDelegateEntityData<in TData, T0, T1>(
		TData data,
		Entity entity,
		ref T0 t0, ref T1 t1
	);

	internal readonly struct QueryDelegateWrapper2E<T0, T1>
		: IQuery2<T0, T1>
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

	internal readonly struct QueryDelegateWrapper2<T0, T1>
		: IQuery2<T0, T1>
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

	internal readonly struct QueryDelegateWrapper2ED<TData, T0, T1>
		: IQuery2<T0, T1>
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

	internal readonly struct QueryDelegateWrapper2D<TData, T0, T1>
		: IQuery2<T0, T1>
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
		public void Query<T0, T1>(QueryDelegateEntity<T0, T1> @delegate)
			where T0 : IComponent
            where T1 : IComponent
		{
			Execute<QueryDelegateWrapper2E<T0, T1>, T0, T1>(
				new QueryDelegateWrapper2E<T0, T1>(@delegate)
			);
		}

		public void QueryParallel<T0, T1>(QueryDelegateEntity<T0, T1> @delegate)
			where T0 : IComponent
            where T1 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper2E<T0, T1>, T0, T1>(
				new QueryDelegateWrapper2E<T0, T1>(@delegate)
			);
		}

		public void Query<T0, T1>(QueryDelegate<T0, T1> @delegate)
			where T0 : IComponent
            where T1 : IComponent
		{
			Execute<QueryDelegateWrapper2<T0, T1>, T0, T1>(
				new QueryDelegateWrapper2<T0, T1>(@delegate)
			);
		}

		public void QueryParallel<T0, T1>(QueryDelegate<T0, T1> @delegate)
			where T0 : IComponent
            where T1 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper2<T0, T1>, T0, T1>(
				new QueryDelegateWrapper2<T0, T1>(@delegate)
			);
		}

		// --- //

		public void Query<TData, T0, T1>(TData data, QueryDelegateEntityData<TData, T0, T1> @delegate)
			where T0 : IComponent
            where T1 : IComponent
		{
			Execute<QueryDelegateWrapper2ED<TData, T0, T1>, T0, T1>(
				new QueryDelegateWrapper2ED<TData, T0, T1>(data, @delegate)
			);
		}

		public void QueryParallel<TData, T0, T1>(TData data, QueryDelegateEntityData<TData, T0, T1> @delegate)
			where T0 : IComponent
            where T1 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper2ED<TData, T0, T1>, T0, T1>(
				new QueryDelegateWrapper2ED<TData, T0, T1>(data, @delegate)
			);
		}

		public void Query<TData, T0, T1>(TData data, QueryDelegateData<TData, T0, T1> @delegate)
			where T0 : IComponent
            where T1 : IComponent
		{
			Execute<QueryDelegateWrapper2D<TData, T0, T1>, T0, T1>(
				new QueryDelegateWrapper2D<TData, T0, T1>(data, @delegate)
			);
		}

		public void QueryParallel<TData, T0, T1>(TData data, QueryDelegateData<TData, T0, T1> @delegate)
			where T0 : IComponent
            where T1 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper2D<TData, T0, T1>, T0, T1>(
				new QueryDelegateWrapper2D<TData, T0, T1>(data, @delegate)
			);
		}
	}
}

namespace Myriad.ECS.Queries
{
	public delegate void QueryDelegate<T0, T1, T2>(
		ref T0 t0, ref T1 t1, ref T2 t2
	);

	public delegate void QueryDelegateEntity<T0, T1, T2>(
		Entity entity,
		ref T0 t0, ref T1 t1, ref T2 t2
	);

	public delegate void QueryDelegateData<in TData, T0, T1, T2>(
		TData data,
		ref T0 t0, ref T1 t1, ref T2 t2
	);

	public delegate void QueryDelegateEntityData<in TData, T0, T1, T2>(
		TData data,
		Entity entity,
		ref T0 t0, ref T1 t1, ref T2 t2
	);

	internal readonly struct QueryDelegateWrapper3E<T0, T1, T2>
		: IQuery3<T0, T1, T2>
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

	internal readonly struct QueryDelegateWrapper3<T0, T1, T2>
		: IQuery3<T0, T1, T2>
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

	internal readonly struct QueryDelegateWrapper3ED<TData, T0, T1, T2>
		: IQuery3<T0, T1, T2>
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

	internal readonly struct QueryDelegateWrapper3D<TData, T0, T1, T2>
		: IQuery3<T0, T1, T2>
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
		public void Query<T0, T1, T2>(QueryDelegateEntity<T0, T1, T2> @delegate)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
		{
			Execute<QueryDelegateWrapper3E<T0, T1, T2>, T0, T1, T2>(
				new QueryDelegateWrapper3E<T0, T1, T2>(@delegate)
			);
		}

		public void QueryParallel<T0, T1, T2>(QueryDelegateEntity<T0, T1, T2> @delegate)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper3E<T0, T1, T2>, T0, T1, T2>(
				new QueryDelegateWrapper3E<T0, T1, T2>(@delegate)
			);
		}

		public void Query<T0, T1, T2>(QueryDelegate<T0, T1, T2> @delegate)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
		{
			Execute<QueryDelegateWrapper3<T0, T1, T2>, T0, T1, T2>(
				new QueryDelegateWrapper3<T0, T1, T2>(@delegate)
			);
		}

		public void QueryParallel<T0, T1, T2>(QueryDelegate<T0, T1, T2> @delegate)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper3<T0, T1, T2>, T0, T1, T2>(
				new QueryDelegateWrapper3<T0, T1, T2>(@delegate)
			);
		}

		// --- //

		public void Query<TData, T0, T1, T2>(TData data, QueryDelegateEntityData<TData, T0, T1, T2> @delegate)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
		{
			Execute<QueryDelegateWrapper3ED<TData, T0, T1, T2>, T0, T1, T2>(
				new QueryDelegateWrapper3ED<TData, T0, T1, T2>(data, @delegate)
			);
		}

		public void QueryParallel<TData, T0, T1, T2>(TData data, QueryDelegateEntityData<TData, T0, T1, T2> @delegate)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper3ED<TData, T0, T1, T2>, T0, T1, T2>(
				new QueryDelegateWrapper3ED<TData, T0, T1, T2>(data, @delegate)
			);
		}

		public void Query<TData, T0, T1, T2>(TData data, QueryDelegateData<TData, T0, T1, T2> @delegate)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
		{
			Execute<QueryDelegateWrapper3D<TData, T0, T1, T2>, T0, T1, T2>(
				new QueryDelegateWrapper3D<TData, T0, T1, T2>(data, @delegate)
			);
		}

		public void QueryParallel<TData, T0, T1, T2>(TData data, QueryDelegateData<TData, T0, T1, T2> @delegate)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper3D<TData, T0, T1, T2>, T0, T1, T2>(
				new QueryDelegateWrapper3D<TData, T0, T1, T2>(data, @delegate)
			);
		}
	}
}

namespace Myriad.ECS.Queries
{
	public delegate void QueryDelegate<T0, T1, T2, T3>(
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3
	);

	public delegate void QueryDelegateEntity<T0, T1, T2, T3>(
		Entity entity,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3
	);

	public delegate void QueryDelegateData<in TData, T0, T1, T2, T3>(
		TData data,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3
	);

	public delegate void QueryDelegateEntityData<in TData, T0, T1, T2, T3>(
		TData data,
		Entity entity,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3
	);

	internal readonly struct QueryDelegateWrapper4E<T0, T1, T2, T3>
		: IQuery4<T0, T1, T2, T3>
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

	internal readonly struct QueryDelegateWrapper4<T0, T1, T2, T3>
		: IQuery4<T0, T1, T2, T3>
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

	internal readonly struct QueryDelegateWrapper4ED<TData, T0, T1, T2, T3>
		: IQuery4<T0, T1, T2, T3>
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

	internal readonly struct QueryDelegateWrapper4D<TData, T0, T1, T2, T3>
		: IQuery4<T0, T1, T2, T3>
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
		public void Query<T0, T1, T2, T3>(QueryDelegateEntity<T0, T1, T2, T3> @delegate)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
		{
			Execute<QueryDelegateWrapper4E<T0, T1, T2, T3>, T0, T1, T2, T3>(
				new QueryDelegateWrapper4E<T0, T1, T2, T3>(@delegate)
			);
		}

		public void QueryParallel<T0, T1, T2, T3>(QueryDelegateEntity<T0, T1, T2, T3> @delegate)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper4E<T0, T1, T2, T3>, T0, T1, T2, T3>(
				new QueryDelegateWrapper4E<T0, T1, T2, T3>(@delegate)
			);
		}

		public void Query<T0, T1, T2, T3>(QueryDelegate<T0, T1, T2, T3> @delegate)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
		{
			Execute<QueryDelegateWrapper4<T0, T1, T2, T3>, T0, T1, T2, T3>(
				new QueryDelegateWrapper4<T0, T1, T2, T3>(@delegate)
			);
		}

		public void QueryParallel<T0, T1, T2, T3>(QueryDelegate<T0, T1, T2, T3> @delegate)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper4<T0, T1, T2, T3>, T0, T1, T2, T3>(
				new QueryDelegateWrapper4<T0, T1, T2, T3>(@delegate)
			);
		}

		// --- //

		public void Query<TData, T0, T1, T2, T3>(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3> @delegate)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
		{
			Execute<QueryDelegateWrapper4ED<TData, T0, T1, T2, T3>, T0, T1, T2, T3>(
				new QueryDelegateWrapper4ED<TData, T0, T1, T2, T3>(data, @delegate)
			);
		}

		public void QueryParallel<TData, T0, T1, T2, T3>(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3> @delegate)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper4ED<TData, T0, T1, T2, T3>, T0, T1, T2, T3>(
				new QueryDelegateWrapper4ED<TData, T0, T1, T2, T3>(data, @delegate)
			);
		}

		public void Query<TData, T0, T1, T2, T3>(TData data, QueryDelegateData<TData, T0, T1, T2, T3> @delegate)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
		{
			Execute<QueryDelegateWrapper4D<TData, T0, T1, T2, T3>, T0, T1, T2, T3>(
				new QueryDelegateWrapper4D<TData, T0, T1, T2, T3>(data, @delegate)
			);
		}

		public void QueryParallel<TData, T0, T1, T2, T3>(TData data, QueryDelegateData<TData, T0, T1, T2, T3> @delegate)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper4D<TData, T0, T1, T2, T3>, T0, T1, T2, T3>(
				new QueryDelegateWrapper4D<TData, T0, T1, T2, T3>(data, @delegate)
			);
		}
	}
}

namespace Myriad.ECS.Queries
{
	public delegate void QueryDelegate<T0, T1, T2, T3, T4>(
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4
	);

	public delegate void QueryDelegateEntity<T0, T1, T2, T3, T4>(
		Entity entity,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4
	);

	public delegate void QueryDelegateData<in TData, T0, T1, T2, T3, T4>(
		TData data,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4
	);

	public delegate void QueryDelegateEntityData<in TData, T0, T1, T2, T3, T4>(
		TData data,
		Entity entity,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4
	);

	internal readonly struct QueryDelegateWrapper5E<T0, T1, T2, T3, T4>
		: IQuery5<T0, T1, T2, T3, T4>
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

	internal readonly struct QueryDelegateWrapper5<T0, T1, T2, T3, T4>
		: IQuery5<T0, T1, T2, T3, T4>
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

	internal readonly struct QueryDelegateWrapper5ED<TData, T0, T1, T2, T3, T4>
		: IQuery5<T0, T1, T2, T3, T4>
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

	internal readonly struct QueryDelegateWrapper5D<TData, T0, T1, T2, T3, T4>
		: IQuery5<T0, T1, T2, T3, T4>
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
		public void Query<T0, T1, T2, T3, T4>(QueryDelegateEntity<T0, T1, T2, T3, T4> @delegate)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
		{
			Execute<QueryDelegateWrapper5E<T0, T1, T2, T3, T4>, T0, T1, T2, T3, T4>(
				new QueryDelegateWrapper5E<T0, T1, T2, T3, T4>(@delegate)
			);
		}

		public void QueryParallel<T0, T1, T2, T3, T4>(QueryDelegateEntity<T0, T1, T2, T3, T4> @delegate)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper5E<T0, T1, T2, T3, T4>, T0, T1, T2, T3, T4>(
				new QueryDelegateWrapper5E<T0, T1, T2, T3, T4>(@delegate)
			);
		}

		public void Query<T0, T1, T2, T3, T4>(QueryDelegate<T0, T1, T2, T3, T4> @delegate)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
		{
			Execute<QueryDelegateWrapper5<T0, T1, T2, T3, T4>, T0, T1, T2, T3, T4>(
				new QueryDelegateWrapper5<T0, T1, T2, T3, T4>(@delegate)
			);
		}

		public void QueryParallel<T0, T1, T2, T3, T4>(QueryDelegate<T0, T1, T2, T3, T4> @delegate)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper5<T0, T1, T2, T3, T4>, T0, T1, T2, T3, T4>(
				new QueryDelegateWrapper5<T0, T1, T2, T3, T4>(@delegate)
			);
		}

		// --- //

		public void Query<TData, T0, T1, T2, T3, T4>(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4> @delegate)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
		{
			Execute<QueryDelegateWrapper5ED<TData, T0, T1, T2, T3, T4>, T0, T1, T2, T3, T4>(
				new QueryDelegateWrapper5ED<TData, T0, T1, T2, T3, T4>(data, @delegate)
			);
		}

		public void QueryParallel<TData, T0, T1, T2, T3, T4>(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4> @delegate)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper5ED<TData, T0, T1, T2, T3, T4>, T0, T1, T2, T3, T4>(
				new QueryDelegateWrapper5ED<TData, T0, T1, T2, T3, T4>(data, @delegate)
			);
		}

		public void Query<TData, T0, T1, T2, T3, T4>(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4> @delegate)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
		{
			Execute<QueryDelegateWrapper5D<TData, T0, T1, T2, T3, T4>, T0, T1, T2, T3, T4>(
				new QueryDelegateWrapper5D<TData, T0, T1, T2, T3, T4>(data, @delegate)
			);
		}

		public void QueryParallel<TData, T0, T1, T2, T3, T4>(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4> @delegate)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper5D<TData, T0, T1, T2, T3, T4>, T0, T1, T2, T3, T4>(
				new QueryDelegateWrapper5D<TData, T0, T1, T2, T3, T4>(data, @delegate)
			);
		}
	}
}

namespace Myriad.ECS.Queries
{
	public delegate void QueryDelegate<T0, T1, T2, T3, T4, T5>(
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5
	);

	public delegate void QueryDelegateEntity<T0, T1, T2, T3, T4, T5>(
		Entity entity,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5
	);

	public delegate void QueryDelegateData<in TData, T0, T1, T2, T3, T4, T5>(
		TData data,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5
	);

	public delegate void QueryDelegateEntityData<in TData, T0, T1, T2, T3, T4, T5>(
		TData data,
		Entity entity,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5
	);

	internal readonly struct QueryDelegateWrapper6E<T0, T1, T2, T3, T4, T5>
		: IQuery6<T0, T1, T2, T3, T4, T5>
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

	internal readonly struct QueryDelegateWrapper6<T0, T1, T2, T3, T4, T5>
		: IQuery6<T0, T1, T2, T3, T4, T5>
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

	internal readonly struct QueryDelegateWrapper6ED<TData, T0, T1, T2, T3, T4, T5>
		: IQuery6<T0, T1, T2, T3, T4, T5>
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

	internal readonly struct QueryDelegateWrapper6D<TData, T0, T1, T2, T3, T4, T5>
		: IQuery6<T0, T1, T2, T3, T4, T5>
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
		public void Query<T0, T1, T2, T3, T4, T5>(QueryDelegateEntity<T0, T1, T2, T3, T4, T5> @delegate)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
		{
			Execute<QueryDelegateWrapper6E<T0, T1, T2, T3, T4, T5>, T0, T1, T2, T3, T4, T5>(
				new QueryDelegateWrapper6E<T0, T1, T2, T3, T4, T5>(@delegate)
			);
		}

		public void QueryParallel<T0, T1, T2, T3, T4, T5>(QueryDelegateEntity<T0, T1, T2, T3, T4, T5> @delegate)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper6E<T0, T1, T2, T3, T4, T5>, T0, T1, T2, T3, T4, T5>(
				new QueryDelegateWrapper6E<T0, T1, T2, T3, T4, T5>(@delegate)
			);
		}

		public void Query<T0, T1, T2, T3, T4, T5>(QueryDelegate<T0, T1, T2, T3, T4, T5> @delegate)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
		{
			Execute<QueryDelegateWrapper6<T0, T1, T2, T3, T4, T5>, T0, T1, T2, T3, T4, T5>(
				new QueryDelegateWrapper6<T0, T1, T2, T3, T4, T5>(@delegate)
			);
		}

		public void QueryParallel<T0, T1, T2, T3, T4, T5>(QueryDelegate<T0, T1, T2, T3, T4, T5> @delegate)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper6<T0, T1, T2, T3, T4, T5>, T0, T1, T2, T3, T4, T5>(
				new QueryDelegateWrapper6<T0, T1, T2, T3, T4, T5>(@delegate)
			);
		}

		// --- //

		public void Query<TData, T0, T1, T2, T3, T4, T5>(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5> @delegate)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
		{
			Execute<QueryDelegateWrapper6ED<TData, T0, T1, T2, T3, T4, T5>, T0, T1, T2, T3, T4, T5>(
				new QueryDelegateWrapper6ED<TData, T0, T1, T2, T3, T4, T5>(data, @delegate)
			);
		}

		public void QueryParallel<TData, T0, T1, T2, T3, T4, T5>(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5> @delegate)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper6ED<TData, T0, T1, T2, T3, T4, T5>, T0, T1, T2, T3, T4, T5>(
				new QueryDelegateWrapper6ED<TData, T0, T1, T2, T3, T4, T5>(data, @delegate)
			);
		}

		public void Query<TData, T0, T1, T2, T3, T4, T5>(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4, T5> @delegate)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
		{
			Execute<QueryDelegateWrapper6D<TData, T0, T1, T2, T3, T4, T5>, T0, T1, T2, T3, T4, T5>(
				new QueryDelegateWrapper6D<TData, T0, T1, T2, T3, T4, T5>(data, @delegate)
			);
		}

		public void QueryParallel<TData, T0, T1, T2, T3, T4, T5>(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4, T5> @delegate)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper6D<TData, T0, T1, T2, T3, T4, T5>, T0, T1, T2, T3, T4, T5>(
				new QueryDelegateWrapper6D<TData, T0, T1, T2, T3, T4, T5>(data, @delegate)
			);
		}
	}
}

namespace Myriad.ECS.Queries
{
	public delegate void QueryDelegate<T0, T1, T2, T3, T4, T5, T6>(
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6
	);

	public delegate void QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6>(
		Entity entity,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6
	);

	public delegate void QueryDelegateData<in TData, T0, T1, T2, T3, T4, T5, T6>(
		TData data,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6
	);

	public delegate void QueryDelegateEntityData<in TData, T0, T1, T2, T3, T4, T5, T6>(
		TData data,
		Entity entity,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6
	);

	internal readonly struct QueryDelegateWrapper7E<T0, T1, T2, T3, T4, T5, T6>
		: IQuery7<T0, T1, T2, T3, T4, T5, T6>
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

	internal readonly struct QueryDelegateWrapper7<T0, T1, T2, T3, T4, T5, T6>
		: IQuery7<T0, T1, T2, T3, T4, T5, T6>
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

	internal readonly struct QueryDelegateWrapper7ED<TData, T0, T1, T2, T3, T4, T5, T6>
		: IQuery7<T0, T1, T2, T3, T4, T5, T6>
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

	internal readonly struct QueryDelegateWrapper7D<TData, T0, T1, T2, T3, T4, T5, T6>
		: IQuery7<T0, T1, T2, T3, T4, T5, T6>
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
		public void Query<T0, T1, T2, T3, T4, T5, T6>(QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6> @delegate)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
		{
			Execute<QueryDelegateWrapper7E<T0, T1, T2, T3, T4, T5, T6>, T0, T1, T2, T3, T4, T5, T6>(
				new QueryDelegateWrapper7E<T0, T1, T2, T3, T4, T5, T6>(@delegate)
			);
		}

		public void QueryParallel<T0, T1, T2, T3, T4, T5, T6>(QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6> @delegate)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper7E<T0, T1, T2, T3, T4, T5, T6>, T0, T1, T2, T3, T4, T5, T6>(
				new QueryDelegateWrapper7E<T0, T1, T2, T3, T4, T5, T6>(@delegate)
			);
		}

		public void Query<T0, T1, T2, T3, T4, T5, T6>(QueryDelegate<T0, T1, T2, T3, T4, T5, T6> @delegate)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
		{
			Execute<QueryDelegateWrapper7<T0, T1, T2, T3, T4, T5, T6>, T0, T1, T2, T3, T4, T5, T6>(
				new QueryDelegateWrapper7<T0, T1, T2, T3, T4, T5, T6>(@delegate)
			);
		}

		public void QueryParallel<T0, T1, T2, T3, T4, T5, T6>(QueryDelegate<T0, T1, T2, T3, T4, T5, T6> @delegate)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper7<T0, T1, T2, T3, T4, T5, T6>, T0, T1, T2, T3, T4, T5, T6>(
				new QueryDelegateWrapper7<T0, T1, T2, T3, T4, T5, T6>(@delegate)
			);
		}

		// --- //

		public void Query<TData, T0, T1, T2, T3, T4, T5, T6>(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6> @delegate)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
		{
			Execute<QueryDelegateWrapper7ED<TData, T0, T1, T2, T3, T4, T5, T6>, T0, T1, T2, T3, T4, T5, T6>(
				new QueryDelegateWrapper7ED<TData, T0, T1, T2, T3, T4, T5, T6>(data, @delegate)
			);
		}

		public void QueryParallel<TData, T0, T1, T2, T3, T4, T5, T6>(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6> @delegate)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper7ED<TData, T0, T1, T2, T3, T4, T5, T6>, T0, T1, T2, T3, T4, T5, T6>(
				new QueryDelegateWrapper7ED<TData, T0, T1, T2, T3, T4, T5, T6>(data, @delegate)
			);
		}

		public void Query<TData, T0, T1, T2, T3, T4, T5, T6>(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6> @delegate)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
		{
			Execute<QueryDelegateWrapper7D<TData, T0, T1, T2, T3, T4, T5, T6>, T0, T1, T2, T3, T4, T5, T6>(
				new QueryDelegateWrapper7D<TData, T0, T1, T2, T3, T4, T5, T6>(data, @delegate)
			);
		}

		public void QueryParallel<TData, T0, T1, T2, T3, T4, T5, T6>(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6> @delegate)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
		{
			ExecuteParallel<QueryDelegateWrapper7D<TData, T0, T1, T2, T3, T4, T5, T6>, T0, T1, T2, T3, T4, T5, T6>(
				new QueryDelegateWrapper7D<TData, T0, T1, T2, T3, T4, T5, T6>(data, @delegate)
			);
		}
	}
}

namespace Myriad.ECS.Queries
{
	public delegate void QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7>(
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7
	);

	public delegate void QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7>(
		Entity entity,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7
	);

	public delegate void QueryDelegateData<in TData, T0, T1, T2, T3, T4, T5, T6, T7>(
		TData data,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7
	);

	public delegate void QueryDelegateEntityData<in TData, T0, T1, T2, T3, T4, T5, T6, T7>(
		TData data,
		Entity entity,
		ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7
	);

	internal readonly struct QueryDelegateWrapper8E<T0, T1, T2, T3, T4, T5, T6, T7>
		: IQuery8<T0, T1, T2, T3, T4, T5, T6, T7>
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

	internal readonly struct QueryDelegateWrapper8<T0, T1, T2, T3, T4, T5, T6, T7>
		: IQuery8<T0, T1, T2, T3, T4, T5, T6, T7>
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

	internal readonly struct QueryDelegateWrapper8ED<TData, T0, T1, T2, T3, T4, T5, T6, T7>
		: IQuery8<T0, T1, T2, T3, T4, T5, T6, T7>
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

	internal readonly struct QueryDelegateWrapper8D<TData, T0, T1, T2, T3, T4, T5, T6, T7>
		: IQuery8<T0, T1, T2, T3, T4, T5, T6, T7>
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
		public void Query<T0, T1, T2, T3, T4, T5, T6, T7>(QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7> @delegate)
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
				new QueryDelegateWrapper8E<T0, T1, T2, T3, T4, T5, T6, T7>(@delegate)
			);
		}

		public void QueryParallel<T0, T1, T2, T3, T4, T5, T6, T7>(QueryDelegateEntity<T0, T1, T2, T3, T4, T5, T6, T7> @delegate)
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
				new QueryDelegateWrapper8E<T0, T1, T2, T3, T4, T5, T6, T7>(@delegate)
			);
		}

		public void Query<T0, T1, T2, T3, T4, T5, T6, T7>(QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7> @delegate)
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
				new QueryDelegateWrapper8<T0, T1, T2, T3, T4, T5, T6, T7>(@delegate)
			);
		}

		public void QueryParallel<T0, T1, T2, T3, T4, T5, T6, T7>(QueryDelegate<T0, T1, T2, T3, T4, T5, T6, T7> @delegate)
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
				new QueryDelegateWrapper8<T0, T1, T2, T3, T4, T5, T6, T7>(@delegate)
			);
		}

		// --- //

		public void Query<TData, T0, T1, T2, T3, T4, T5, T6, T7>(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6, T7> @delegate)
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
				new QueryDelegateWrapper8ED<TData, T0, T1, T2, T3, T4, T5, T6, T7>(data, @delegate)
			);
		}

		public void QueryParallel<TData, T0, T1, T2, T3, T4, T5, T6, T7>(TData data, QueryDelegateEntityData<TData, T0, T1, T2, T3, T4, T5, T6, T7> @delegate)
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
				new QueryDelegateWrapper8ED<TData, T0, T1, T2, T3, T4, T5, T6, T7>(data, @delegate)
			);
		}

		public void Query<TData, T0, T1, T2, T3, T4, T5, T6, T7>(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6, T7> @delegate)
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
				new QueryDelegateWrapper8D<TData, T0, T1, T2, T3, T4, T5, T6, T7>(data, @delegate)
			);
		}

		public void QueryParallel<TData, T0, T1, T2, T3, T4, T5, T6, T7>(TData data, QueryDelegateData<TData, T0, T1, T2, T3, T4, T5, T6, T7> @delegate)
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
				new QueryDelegateWrapper8D<TData, T0, T1, T2, T3, T4, T5, T6, T7>(data, @delegate)
			);
		}
	}
}


namespace Myriad.ECS.Queries
{
	public interface IQuery1<T0>
		where T0 : IComponent
	{
		public void Execute(Entity e, ref T0 t0);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		public int Execute<TQ, T0>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : IComponent
			where TQ : IQuery1<T0>
		{
			query ??= GetCachedQuery<T0>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return 0;

			var c0 = ComponentID<T0>.ID;

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				count += archetype.EntityCount;

				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

					var t0 = chunk.GetSpan<T0>(c0);

					for (var i = entities.Length - 1; i >= 0; i--)
						q.Execute(entities[i], ref t0[i]);
				}
			}

			return count;
		}

		public int ExecuteParallel<TQ, T0>(
			TQ q,
			QueryDescription? query = null,
			int batchSize = 16
		)
			where T0 : IComponent
			where TQ : IQuery1<T0>
		{
			query ??= GetCachedQuery<T0>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return 0;

			var c0 = ComponentID<T0>.ID;
	
			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				count += archetype.EntityCount;

				for (var c = 0; c < archetype.Chunks.Count; c++)
                {
                    var chunk = archetype.Chunks[c];

					var entityCount = chunk.EntityCount;
					if (entityCount == 0)
						continue;

					var numBatches = (int)Math.Ceiling(entityCount / (double)batchSize);

					var t0 = chunk.GetComponentArray<T0>(c0);

					Parallel.For(0, numBatches, b =>
                    {
						var start = b * batchSize;
						var end = Math.Min(start + batchSize, entityCount);

						for (var i = start; i < end; i++)
						{
							var entities = chunk.Entities;
							q.Execute(entities[i], ref t0[i]);
						}
					});
				}
			}

			return count;
		}
	}
}
namespace Myriad.ECS.Queries
{
	public interface IQuery2<T0, T1>
		where T0 : IComponent
        where T1 : IComponent
	{
		public void Execute(Entity e, ref T0 t0, ref T1 t1);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		public int Execute<TQ, T0, T1>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
			where TQ : IQuery2<T0, T1>
		{
			query ??= GetCachedQuery<T0, T1>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return 0;

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				count += archetype.EntityCount;

				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

					var t0 = chunk.GetSpan<T0>(c0);
					var t1 = chunk.GetSpan<T1>(c1);

					for (var i = entities.Length - 1; i >= 0; i--)
						q.Execute(entities[i], ref t0[i], ref t1[i]);
				}
			}

			return count;
		}

		public int ExecuteParallel<TQ, T0, T1>(
			TQ q,
			QueryDescription? query = null,
			int batchSize = 16
		)
			where T0 : IComponent
            where T1 : IComponent
			where TQ : IQuery2<T0, T1>
		{
			query ??= GetCachedQuery<T0, T1>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return 0;

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;
	
			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				count += archetype.EntityCount;

				for (var c = 0; c < archetype.Chunks.Count; c++)
                {
                    var chunk = archetype.Chunks[c];

					var entityCount = chunk.EntityCount;
					if (entityCount == 0)
						continue;

					var numBatches = (int)Math.Ceiling(entityCount / (double)batchSize);

					var t0 = chunk.GetComponentArray<T0>(c0);
					var t1 = chunk.GetComponentArray<T1>(c1);

					Parallel.For(0, numBatches, b =>
                    {
						var start = b * batchSize;
						var end = Math.Min(start + batchSize, entityCount);

						for (var i = start; i < end; i++)
						{
							var entities = chunk.Entities;
							q.Execute(entities[i], ref t0[i], ref t1[i]);
						}
					});
				}
			}

			return count;
		}
	}
}
namespace Myriad.ECS.Queries
{
	public interface IQuery3<T0, T1, T2>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
	{
		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		public int Execute<TQ, T0, T1, T2>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
			where TQ : IQuery3<T0, T1, T2>
		{
			query ??= GetCachedQuery<T0, T1, T2>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return 0;

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;
			var c2 = ComponentID<T2>.ID;

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				count += archetype.EntityCount;

				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

					var t0 = chunk.GetSpan<T0>(c0);
					var t1 = chunk.GetSpan<T1>(c1);
					var t2 = chunk.GetSpan<T2>(c2);

					for (var i = entities.Length - 1; i >= 0; i--)
						q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i]);
				}
			}

			return count;
		}

		public int ExecuteParallel<TQ, T0, T1, T2>(
			TQ q,
			QueryDescription? query = null,
			int batchSize = 16
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
			where TQ : IQuery3<T0, T1, T2>
		{
			query ??= GetCachedQuery<T0, T1, T2>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return 0;

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;
			var c2 = ComponentID<T2>.ID;
	
			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				count += archetype.EntityCount;

				for (var c = 0; c < archetype.Chunks.Count; c++)
                {
                    var chunk = archetype.Chunks[c];

					var entityCount = chunk.EntityCount;
					if (entityCount == 0)
						continue;

					var numBatches = (int)Math.Ceiling(entityCount / (double)batchSize);

					var t0 = chunk.GetComponentArray<T0>(c0);
					var t1 = chunk.GetComponentArray<T1>(c1);
					var t2 = chunk.GetComponentArray<T2>(c2);

					Parallel.For(0, numBatches, b =>
                    {
						var start = b * batchSize;
						var end = Math.Min(start + batchSize, entityCount);

						for (var i = start; i < end; i++)
						{
							var entities = chunk.Entities;
							q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i]);
						}
					});
				}
			}

			return count;
		}
	}
}
namespace Myriad.ECS.Queries
{
	public interface IQuery4<T0, T1, T2, T3>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
	{
		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		public int Execute<TQ, T0, T1, T2, T3>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
			where TQ : IQuery4<T0, T1, T2, T3>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return 0;

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;
			var c2 = ComponentID<T2>.ID;
			var c3 = ComponentID<T3>.ID;

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				count += archetype.EntityCount;

				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

					var t0 = chunk.GetSpan<T0>(c0);
					var t1 = chunk.GetSpan<T1>(c1);
					var t2 = chunk.GetSpan<T2>(c2);
					var t3 = chunk.GetSpan<T3>(c3);

					for (var i = entities.Length - 1; i >= 0; i--)
						q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i]);
				}
			}

			return count;
		}

		public int ExecuteParallel<TQ, T0, T1, T2, T3>(
			TQ q,
			QueryDescription? query = null,
			int batchSize = 16
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
			where TQ : IQuery4<T0, T1, T2, T3>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return 0;

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;
			var c2 = ComponentID<T2>.ID;
			var c3 = ComponentID<T3>.ID;
	
			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				count += archetype.EntityCount;

				for (var c = 0; c < archetype.Chunks.Count; c++)
                {
                    var chunk = archetype.Chunks[c];

					var entityCount = chunk.EntityCount;
					if (entityCount == 0)
						continue;

					var numBatches = (int)Math.Ceiling(entityCount / (double)batchSize);

					var t0 = chunk.GetComponentArray<T0>(c0);
					var t1 = chunk.GetComponentArray<T1>(c1);
					var t2 = chunk.GetComponentArray<T2>(c2);
					var t3 = chunk.GetComponentArray<T3>(c3);

					Parallel.For(0, numBatches, b =>
                    {
						var start = b * batchSize;
						var end = Math.Min(start + batchSize, entityCount);

						for (var i = start; i < end; i++)
						{
							var entities = chunk.Entities;
							q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i]);
						}
					});
				}
			}

			return count;
		}
	}
}
namespace Myriad.ECS.Queries
{
	public interface IQuery5<T0, T1, T2, T3, T4>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
	{
		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		public int Execute<TQ, T0, T1, T2, T3, T4>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
			where TQ : IQuery5<T0, T1, T2, T3, T4>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return 0;

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;
			var c2 = ComponentID<T2>.ID;
			var c3 = ComponentID<T3>.ID;
			var c4 = ComponentID<T4>.ID;

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				count += archetype.EntityCount;

				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

					var t0 = chunk.GetSpan<T0>(c0);
					var t1 = chunk.GetSpan<T1>(c1);
					var t2 = chunk.GetSpan<T2>(c2);
					var t3 = chunk.GetSpan<T3>(c3);
					var t4 = chunk.GetSpan<T4>(c4);

					for (var i = entities.Length - 1; i >= 0; i--)
						q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i]);
				}
			}

			return count;
		}

		public int ExecuteParallel<TQ, T0, T1, T2, T3, T4>(
			TQ q,
			QueryDescription? query = null,
			int batchSize = 16
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
			where TQ : IQuery5<T0, T1, T2, T3, T4>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return 0;

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;
			var c2 = ComponentID<T2>.ID;
			var c3 = ComponentID<T3>.ID;
			var c4 = ComponentID<T4>.ID;
	
			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				count += archetype.EntityCount;

				for (var c = 0; c < archetype.Chunks.Count; c++)
                {
                    var chunk = archetype.Chunks[c];

					var entityCount = chunk.EntityCount;
					if (entityCount == 0)
						continue;

					var numBatches = (int)Math.Ceiling(entityCount / (double)batchSize);

					var t0 = chunk.GetComponentArray<T0>(c0);
					var t1 = chunk.GetComponentArray<T1>(c1);
					var t2 = chunk.GetComponentArray<T2>(c2);
					var t3 = chunk.GetComponentArray<T3>(c3);
					var t4 = chunk.GetComponentArray<T4>(c4);

					Parallel.For(0, numBatches, b =>
                    {
						var start = b * batchSize;
						var end = Math.Min(start + batchSize, entityCount);

						for (var i = start; i < end; i++)
						{
							var entities = chunk.Entities;
							q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i]);
						}
					});
				}
			}

			return count;
		}
	}
}
namespace Myriad.ECS.Queries
{
	public interface IQuery6<T0, T1, T2, T3, T4, T5>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
	{
		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		public int Execute<TQ, T0, T1, T2, T3, T4, T5>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
			where TQ : IQuery6<T0, T1, T2, T3, T4, T5>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return 0;

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;
			var c2 = ComponentID<T2>.ID;
			var c3 = ComponentID<T3>.ID;
			var c4 = ComponentID<T4>.ID;
			var c5 = ComponentID<T5>.ID;

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				count += archetype.EntityCount;

				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

					var t0 = chunk.GetSpan<T0>(c0);
					var t1 = chunk.GetSpan<T1>(c1);
					var t2 = chunk.GetSpan<T2>(c2);
					var t3 = chunk.GetSpan<T3>(c3);
					var t4 = chunk.GetSpan<T4>(c4);
					var t5 = chunk.GetSpan<T5>(c5);

					for (var i = entities.Length - 1; i >= 0; i--)
						q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i]);
				}
			}

			return count;
		}

		public int ExecuteParallel<TQ, T0, T1, T2, T3, T4, T5>(
			TQ q,
			QueryDescription? query = null,
			int batchSize = 16
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
			where TQ : IQuery6<T0, T1, T2, T3, T4, T5>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return 0;

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;
			var c2 = ComponentID<T2>.ID;
			var c3 = ComponentID<T3>.ID;
			var c4 = ComponentID<T4>.ID;
			var c5 = ComponentID<T5>.ID;
	
			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				count += archetype.EntityCount;

				for (var c = 0; c < archetype.Chunks.Count; c++)
                {
                    var chunk = archetype.Chunks[c];

					var entityCount = chunk.EntityCount;
					if (entityCount == 0)
						continue;

					var numBatches = (int)Math.Ceiling(entityCount / (double)batchSize);

					var t0 = chunk.GetComponentArray<T0>(c0);
					var t1 = chunk.GetComponentArray<T1>(c1);
					var t2 = chunk.GetComponentArray<T2>(c2);
					var t3 = chunk.GetComponentArray<T3>(c3);
					var t4 = chunk.GetComponentArray<T4>(c4);
					var t5 = chunk.GetComponentArray<T5>(c5);

					Parallel.For(0, numBatches, b =>
                    {
						var start = b * batchSize;
						var end = Math.Min(start + batchSize, entityCount);

						for (var i = start; i < end; i++)
						{
							var entities = chunk.Entities;
							q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i]);
						}
					});
				}
			}

			return count;
		}
	}
}
namespace Myriad.ECS.Queries
{
	public interface IQuery7<T0, T1, T2, T3, T4, T5, T6>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
	{
		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6>(
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
			where TQ : IQuery7<T0, T1, T2, T3, T4, T5, T6>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return 0;

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;
			var c2 = ComponentID<T2>.ID;
			var c3 = ComponentID<T3>.ID;
			var c4 = ComponentID<T4>.ID;
			var c5 = ComponentID<T5>.ID;
			var c6 = ComponentID<T6>.ID;

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				count += archetype.EntityCount;

				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

					var t0 = chunk.GetSpan<T0>(c0);
					var t1 = chunk.GetSpan<T1>(c1);
					var t2 = chunk.GetSpan<T2>(c2);
					var t3 = chunk.GetSpan<T3>(c3);
					var t4 = chunk.GetSpan<T4>(c4);
					var t5 = chunk.GetSpan<T5>(c5);
					var t6 = chunk.GetSpan<T6>(c6);

					for (var i = entities.Length - 1; i >= 0; i--)
						q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i]);
				}
			}

			return count;
		}

		public int ExecuteParallel<TQ, T0, T1, T2, T3, T4, T5, T6>(
			TQ q,
			QueryDescription? query = null,
			int batchSize = 16
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
			where TQ : IQuery7<T0, T1, T2, T3, T4, T5, T6>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return 0;

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;
			var c2 = ComponentID<T2>.ID;
			var c3 = ComponentID<T3>.ID;
			var c4 = ComponentID<T4>.ID;
			var c5 = ComponentID<T5>.ID;
			var c6 = ComponentID<T6>.ID;
	
			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				count += archetype.EntityCount;

				for (var c = 0; c < archetype.Chunks.Count; c++)
                {
                    var chunk = archetype.Chunks[c];

					var entityCount = chunk.EntityCount;
					if (entityCount == 0)
						continue;

					var numBatches = (int)Math.Ceiling(entityCount / (double)batchSize);

					var t0 = chunk.GetComponentArray<T0>(c0);
					var t1 = chunk.GetComponentArray<T1>(c1);
					var t2 = chunk.GetComponentArray<T2>(c2);
					var t3 = chunk.GetComponentArray<T3>(c3);
					var t4 = chunk.GetComponentArray<T4>(c4);
					var t5 = chunk.GetComponentArray<T5>(c5);
					var t6 = chunk.GetComponentArray<T6>(c6);

					Parallel.For(0, numBatches, b =>
                    {
						var start = b * batchSize;
						var end = Math.Min(start + batchSize, entityCount);

						for (var i = start; i < end; i++)
						{
							var entities = chunk.Entities;
							q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i]);
						}
					});
				}
			}

			return count;
		}
	}
}
namespace Myriad.ECS.Queries
{
	public interface IQuery8<T0, T1, T2, T3, T4, T5, T6, T7>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
	{
		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7>(
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
			where TQ : IQuery8<T0, T1, T2, T3, T4, T5, T6, T7>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return 0;

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;
			var c2 = ComponentID<T2>.ID;
			var c3 = ComponentID<T3>.ID;
			var c4 = ComponentID<T4>.ID;
			var c5 = ComponentID<T5>.ID;
			var c6 = ComponentID<T6>.ID;
			var c7 = ComponentID<T7>.ID;

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				count += archetype.EntityCount;

				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

					var t0 = chunk.GetSpan<T0>(c0);
					var t1 = chunk.GetSpan<T1>(c1);
					var t2 = chunk.GetSpan<T2>(c2);
					var t3 = chunk.GetSpan<T3>(c3);
					var t4 = chunk.GetSpan<T4>(c4);
					var t5 = chunk.GetSpan<T5>(c5);
					var t6 = chunk.GetSpan<T6>(c6);
					var t7 = chunk.GetSpan<T7>(c7);

					for (var i = entities.Length - 1; i >= 0; i--)
						q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i]);
				}
			}

			return count;
		}

		public int ExecuteParallel<TQ, T0, T1, T2, T3, T4, T5, T6, T7>(
			TQ q,
			QueryDescription? query = null,
			int batchSize = 16
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
			where TQ : IQuery8<T0, T1, T2, T3, T4, T5, T6, T7>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return 0;

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;
			var c2 = ComponentID<T2>.ID;
			var c3 = ComponentID<T3>.ID;
			var c4 = ComponentID<T4>.ID;
			var c5 = ComponentID<T5>.ID;
			var c6 = ComponentID<T6>.ID;
			var c7 = ComponentID<T7>.ID;
	
			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				count += archetype.EntityCount;

				for (var c = 0; c < archetype.Chunks.Count; c++)
                {
                    var chunk = archetype.Chunks[c];

					var entityCount = chunk.EntityCount;
					if (entityCount == 0)
						continue;

					var numBatches = (int)Math.Ceiling(entityCount / (double)batchSize);

					var t0 = chunk.GetComponentArray<T0>(c0);
					var t1 = chunk.GetComponentArray<T1>(c1);
					var t2 = chunk.GetComponentArray<T2>(c2);
					var t3 = chunk.GetComponentArray<T3>(c3);
					var t4 = chunk.GetComponentArray<T4>(c4);
					var t5 = chunk.GetComponentArray<T5>(c5);
					var t6 = chunk.GetComponentArray<T6>(c6);
					var t7 = chunk.GetComponentArray<T7>(c7);

					Parallel.For(0, numBatches, b =>
                    {
						var start = b * batchSize;
						var end = Math.Min(start + batchSize, entityCount);

						for (var i = start; i < end; i++)
						{
							var entities = chunk.Entities;
							q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i]);
						}
					});
				}
			}

			return count;
		}
	}
}
namespace Myriad.ECS.Queries
{
	public interface IQuery9<T0, T1, T2, T3, T4, T5, T6, T7, T8>
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
		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
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
			where TQ : IQuery9<T0, T1, T2, T3, T4, T5, T6, T7, T8>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return 0;

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;
			var c2 = ComponentID<T2>.ID;
			var c3 = ComponentID<T3>.ID;
			var c4 = ComponentID<T4>.ID;
			var c5 = ComponentID<T5>.ID;
			var c6 = ComponentID<T6>.ID;
			var c7 = ComponentID<T7>.ID;
			var c8 = ComponentID<T8>.ID;

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				count += archetype.EntityCount;

				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

					var t0 = chunk.GetSpan<T0>(c0);
					var t1 = chunk.GetSpan<T1>(c1);
					var t2 = chunk.GetSpan<T2>(c2);
					var t3 = chunk.GetSpan<T3>(c3);
					var t4 = chunk.GetSpan<T4>(c4);
					var t5 = chunk.GetSpan<T5>(c5);
					var t6 = chunk.GetSpan<T6>(c6);
					var t7 = chunk.GetSpan<T7>(c7);
					var t8 = chunk.GetSpan<T8>(c8);

					for (var i = entities.Length - 1; i >= 0; i--)
						q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i]);
				}
			}

			return count;
		}

		public int ExecuteParallel<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
			TQ q,
			QueryDescription? query = null,
			int batchSize = 16
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
			where TQ : IQuery9<T0, T1, T2, T3, T4, T5, T6, T7, T8>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return 0;

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;
			var c2 = ComponentID<T2>.ID;
			var c3 = ComponentID<T3>.ID;
			var c4 = ComponentID<T4>.ID;
			var c5 = ComponentID<T5>.ID;
			var c6 = ComponentID<T6>.ID;
			var c7 = ComponentID<T7>.ID;
			var c8 = ComponentID<T8>.ID;
	
			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				count += archetype.EntityCount;

				for (var c = 0; c < archetype.Chunks.Count; c++)
                {
                    var chunk = archetype.Chunks[c];

					var entityCount = chunk.EntityCount;
					if (entityCount == 0)
						continue;

					var numBatches = (int)Math.Ceiling(entityCount / (double)batchSize);

					var t0 = chunk.GetComponentArray<T0>(c0);
					var t1 = chunk.GetComponentArray<T1>(c1);
					var t2 = chunk.GetComponentArray<T2>(c2);
					var t3 = chunk.GetComponentArray<T3>(c3);
					var t4 = chunk.GetComponentArray<T4>(c4);
					var t5 = chunk.GetComponentArray<T5>(c5);
					var t6 = chunk.GetComponentArray<T6>(c6);
					var t7 = chunk.GetComponentArray<T7>(c7);
					var t8 = chunk.GetComponentArray<T8>(c8);

					Parallel.For(0, numBatches, b =>
                    {
						var start = b * batchSize;
						var end = Math.Min(start + batchSize, entityCount);

						for (var i = start; i < end; i++)
						{
							var entities = chunk.Entities;
							q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i]);
						}
					});
				}
			}

			return count;
		}
	}
}


