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
		private readonly struct ListCollectorEntityIdOnly
			: IQueryCollector
		{
			private readonly List<EntityId> _list;

            public ListCollectorEntityIdOnly(List<EntityId> list)
            {
                _list = list;
            }

            public void Add(Entity e)
            {
                _list.Add(e.ID);
            }

            public void EnsureCapacity(int capacity)
            {
                _list.EnsureCapacity(capacity);
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
			List<EntityId> output,
			QueryDescription query
		)
		{
			return Collect(new ListCollectorEntityIdOnly(output), query);
		}
	}
}


namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		
		private readonly struct ListCollectorEntityIdOnly<T0>
			: IQueryCollector<T0>
			where T0 : IComponent
		{
			private readonly List<EntityId> _list;

            public ListCollectorEntityIdOnly(List<EntityId> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0)
            {
                _list.Add(e.ID);
            }

            public void EnsureCapacity(int capacity)
            {
                _list.EnsureCapacity(capacity);
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
			List<EntityId> output,
			QueryDescription? query = null
		)
			where T0 : IComponent
		{
			return Collect<
				ListCollectorEntityIdOnly<T0>,
				T0
			>(
				new ListCollectorEntityIdOnly<T0>(output),
				query
			);
		}

		
		private readonly struct ListCollectorWithIdAndComponents<T0>
			: IQueryCollector<T0>
			where T0 : IComponent
		{
			private readonly List<(EntityId, T0)> _list;

            public ListCollectorWithIdAndComponents(List<(EntityId, T0)> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0)
            {
                _list.Add((e.ID, t0));
            }

            public void EnsureCapacity(int capacity)
            {
                _list.EnsureCapacity(capacity);
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
			List<(EntityId, T0)> output,
			QueryDescription? query = null
		)
			where T0 : IComponent
		{
			return Collect<
				ListCollectorWithIdAndComponents<T0>,
				T0
			>(
				new ListCollectorWithIdAndComponents<T0>(output),
				query
			);
		}
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorEntityIdOnly<T0, T1>
			: IQueryCollector<T0, T1>
			where T0 : IComponent
            where T1 : IComponent
		{
			private readonly List<EntityId> _list;

            public ListCollectorEntityIdOnly(List<EntityId> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1)
            {
                _list.Add(e.ID);
            }

            public void EnsureCapacity(int capacity)
            {
                _list.EnsureCapacity(capacity);
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
			List<EntityId> output,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
		{
			return Collect<
				ListCollectorEntityIdOnly<T0, T1>,
				T0, T1
			>(
				new ListCollectorEntityIdOnly<T0, T1>(output),
				query
			);
		}

		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorWithIdAndComponents<T0, T1>
			: IQueryCollector<T0, T1>
			where T0 : IComponent
            where T1 : IComponent
		{
			private readonly List<(EntityId, T0, T1)> _list;

            public ListCollectorWithIdAndComponents(List<(EntityId, T0, T1)> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1)
            {
                _list.Add((e.ID, t0, t1));
            }

            public void EnsureCapacity(int capacity)
            {
                _list.EnsureCapacity(capacity);
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
			List<(EntityId, T0, T1)> output,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
		{
			return Collect<
				ListCollectorWithIdAndComponents<T0, T1>,
				T0, T1
			>(
				new ListCollectorWithIdAndComponents<T0, T1>(output),
				query
			);
		}
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorEntityIdOnly<T0, T1, T2>
			: IQueryCollector<T0, T1, T2>
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
		{
			private readonly List<EntityId> _list;

            public ListCollectorEntityIdOnly(List<EntityId> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2)
            {
                _list.Add(e.ID);
            }

            public void EnsureCapacity(int capacity)
            {
                _list.EnsureCapacity(capacity);
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
			List<EntityId> output,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
		{
			return Collect<
				ListCollectorEntityIdOnly<T0, T1, T2>,
				T0, T1, T2
			>(
				new ListCollectorEntityIdOnly<T0, T1, T2>(output),
				query
			);
		}

		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorWithIdAndComponents<T0, T1, T2>
			: IQueryCollector<T0, T1, T2>
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
		{
			private readonly List<(EntityId, T0, T1, T2)> _list;

            public ListCollectorWithIdAndComponents(List<(EntityId, T0, T1, T2)> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2)
            {
                _list.Add((e.ID, t0, t1, t2));
            }

            public void EnsureCapacity(int capacity)
            {
                _list.EnsureCapacity(capacity);
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
			List<(EntityId, T0, T1, T2)> output,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
		{
			return Collect<
				ListCollectorWithIdAndComponents<T0, T1, T2>,
				T0, T1, T2
			>(
				new ListCollectorWithIdAndComponents<T0, T1, T2>(output),
				query
			);
		}
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorEntityIdOnly<T0, T1, T2, T3>
			: IQueryCollector<T0, T1, T2, T3>
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
		{
			private readonly List<EntityId> _list;

            public ListCollectorEntityIdOnly(List<EntityId> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3)
            {
                _list.Add(e.ID);
            }

            public void EnsureCapacity(int capacity)
            {
                _list.EnsureCapacity(capacity);
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
			List<EntityId> output,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
		{
			return Collect<
				ListCollectorEntityIdOnly<T0, T1, T2, T3>,
				T0, T1, T2, T3
			>(
				new ListCollectorEntityIdOnly<T0, T1, T2, T3>(output),
				query
			);
		}

		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorWithIdAndComponents<T0, T1, T2, T3>
			: IQueryCollector<T0, T1, T2, T3>
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
		{
			private readonly List<(EntityId, T0, T1, T2, T3)> _list;

            public ListCollectorWithIdAndComponents(List<(EntityId, T0, T1, T2, T3)> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3)
            {
                _list.Add((e.ID, t0, t1, t2, t3));
            }

            public void EnsureCapacity(int capacity)
            {
                _list.EnsureCapacity(capacity);
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
			List<(EntityId, T0, T1, T2, T3)> output,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
		{
			return Collect<
				ListCollectorWithIdAndComponents<T0, T1, T2, T3>,
				T0, T1, T2, T3
			>(
				new ListCollectorWithIdAndComponents<T0, T1, T2, T3>(output),
				query
			);
		}
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorEntityIdOnly<T0, T1, T2, T3, T4>
			: IQueryCollector<T0, T1, T2, T3, T4>
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
		{
			private readonly List<EntityId> _list;

            public ListCollectorEntityIdOnly(List<EntityId> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4)
            {
                _list.Add(e.ID);
            }

            public void EnsureCapacity(int capacity)
            {
                _list.EnsureCapacity(capacity);
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
			List<EntityId> output,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
		{
			return Collect<
				ListCollectorEntityIdOnly<T0, T1, T2, T3, T4>,
				T0, T1, T2, T3, T4
			>(
				new ListCollectorEntityIdOnly<T0, T1, T2, T3, T4>(output),
				query
			);
		}

		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorWithIdAndComponents<T0, T1, T2, T3, T4>
			: IQueryCollector<T0, T1, T2, T3, T4>
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
		{
			private readonly List<(EntityId, T0, T1, T2, T3, T4)> _list;

            public ListCollectorWithIdAndComponents(List<(EntityId, T0, T1, T2, T3, T4)> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4)
            {
                _list.Add((e.ID, t0, t1, t2, t3, t4));
            }

            public void EnsureCapacity(int capacity)
            {
                _list.EnsureCapacity(capacity);
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
			List<(EntityId, T0, T1, T2, T3, T4)> output,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
		{
			return Collect<
				ListCollectorWithIdAndComponents<T0, T1, T2, T3, T4>,
				T0, T1, T2, T3, T4
			>(
				new ListCollectorWithIdAndComponents<T0, T1, T2, T3, T4>(output),
				query
			);
		}
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorEntityIdOnly<T0, T1, T2, T3, T4, T5>
			: IQueryCollector<T0, T1, T2, T3, T4, T5>
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
		{
			private readonly List<EntityId> _list;

            public ListCollectorEntityIdOnly(List<EntityId> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5)
            {
                _list.Add(e.ID);
            }

            public void EnsureCapacity(int capacity)
            {
                _list.EnsureCapacity(capacity);
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
			List<EntityId> output,
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
				ListCollectorEntityIdOnly<T0, T1, T2, T3, T4, T5>,
				T0, T1, T2, T3, T4, T5
			>(
				new ListCollectorEntityIdOnly<T0, T1, T2, T3, T4, T5>(output),
				query
			);
		}

		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorWithIdAndComponents<T0, T1, T2, T3, T4, T5>
			: IQueryCollector<T0, T1, T2, T3, T4, T5>
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
		{
			private readonly List<(EntityId, T0, T1, T2, T3, T4, T5)> _list;

            public ListCollectorWithIdAndComponents(List<(EntityId, T0, T1, T2, T3, T4, T5)> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5)
            {
                _list.Add((e.ID, t0, t1, t2, t3, t4, t5));
            }

            public void EnsureCapacity(int capacity)
            {
                _list.EnsureCapacity(capacity);
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
			List<(EntityId, T0, T1, T2, T3, T4, T5)> output,
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
				ListCollectorWithIdAndComponents<T0, T1, T2, T3, T4, T5>,
				T0, T1, T2, T3, T4, T5
			>(
				new ListCollectorWithIdAndComponents<T0, T1, T2, T3, T4, T5>(output),
				query
			);
		}
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorEntityIdOnly<T0, T1, T2, T3, T4, T5, T6>
			: IQueryCollector<T0, T1, T2, T3, T4, T5, T6>
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
		{
			private readonly List<EntityId> _list;

            public ListCollectorEntityIdOnly(List<EntityId> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6)
            {
                _list.Add(e.ID);
            }

            public void EnsureCapacity(int capacity)
            {
                _list.EnsureCapacity(capacity);
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
			List<EntityId> output,
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
				ListCollectorEntityIdOnly<T0, T1, T2, T3, T4, T5, T6>,
				T0, T1, T2, T3, T4, T5, T6
			>(
				new ListCollectorEntityIdOnly<T0, T1, T2, T3, T4, T5, T6>(output),
				query
			);
		}

		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorWithIdAndComponents<T0, T1, T2, T3, T4, T5, T6>
			: IQueryCollector<T0, T1, T2, T3, T4, T5, T6>
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
		{
			private readonly List<(EntityId, T0, T1, T2, T3, T4, T5, T6)> _list;

            public ListCollectorWithIdAndComponents(List<(EntityId, T0, T1, T2, T3, T4, T5, T6)> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6)
            {
                _list.Add((e.ID, t0, t1, t2, t3, t4, t5, t6));
            }

            public void EnsureCapacity(int capacity)
            {
                _list.EnsureCapacity(capacity);
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
			List<(EntityId, T0, T1, T2, T3, T4, T5, T6)> output,
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
				ListCollectorWithIdAndComponents<T0, T1, T2, T3, T4, T5, T6>,
				T0, T1, T2, T3, T4, T5, T6
			>(
				new ListCollectorWithIdAndComponents<T0, T1, T2, T3, T4, T5, T6>(output),
				query
			);
		}
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorEntityIdOnly<T0, T1, T2, T3, T4, T5, T6, T7>
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
			private readonly List<EntityId> _list;

            public ListCollectorEntityIdOnly(List<EntityId> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7)
            {
                _list.Add(e.ID);
            }

            public void EnsureCapacity(int capacity)
            {
                _list.EnsureCapacity(capacity);
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
			List<EntityId> output,
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
				ListCollectorEntityIdOnly<T0, T1, T2, T3, T4, T5, T6, T7>,
				T0, T1, T2, T3, T4, T5, T6, T7
			>(
				new ListCollectorEntityIdOnly<T0, T1, T2, T3, T4, T5, T6, T7>(output),
				query
			);
		}

		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorWithIdAndComponents<T0, T1, T2, T3, T4, T5, T6, T7>
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
			private readonly List<(EntityId, T0, T1, T2, T3, T4, T5, T6, T7)> _list;

            public ListCollectorWithIdAndComponents(List<(EntityId, T0, T1, T2, T3, T4, T5, T6, T7)> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7)
            {
                _list.Add((e.ID, t0, t1, t2, t3, t4, t5, t6, t7));
            }

            public void EnsureCapacity(int capacity)
            {
                _list.EnsureCapacity(capacity);
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
			List<(EntityId, T0, T1, T2, T3, T4, T5, T6, T7)> output,
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
				ListCollectorWithIdAndComponents<T0, T1, T2, T3, T4, T5, T6, T7>,
				T0, T1, T2, T3, T4, T5, T6, T7
			>(
				new ListCollectorWithIdAndComponents<T0, T1, T2, T3, T4, T5, T6, T7>(output),
				query
			);
		}
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorEntityIdOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8>
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
			private readonly List<EntityId> _list;

            public ListCollectorEntityIdOnly(List<EntityId> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8)
            {
                _list.Add(e.ID);
            }

            public void EnsureCapacity(int capacity)
            {
                _list.EnsureCapacity(capacity);
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
			List<EntityId> output,
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
				ListCollectorEntityIdOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8
			>(
				new ListCollectorEntityIdOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8>(output),
				query
			);
		}

		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorWithIdAndComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8>
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
			private readonly List<(EntityId, T0, T1, T2, T3, T4, T5, T6, T7, T8)> _list;

            public ListCollectorWithIdAndComponents(List<(EntityId, T0, T1, T2, T3, T4, T5, T6, T7, T8)> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8)
            {
                _list.Add((e.ID, t0, t1, t2, t3, t4, t5, t6, t7, t8));
            }

            public void EnsureCapacity(int capacity)
            {
                _list.EnsureCapacity(capacity);
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
			List<(EntityId, T0, T1, T2, T3, T4, T5, T6, T7, T8)> output,
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
				ListCollectorWithIdAndComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8
			>(
				new ListCollectorWithIdAndComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8>(output),
				query
			);
		}
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorEntityIdOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
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
			private readonly List<EntityId> _list;

            public ListCollectorEntityIdOnly(List<EntityId> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9)
            {
                _list.Add(e.ID);
            }

            public void EnsureCapacity(int capacity)
            {
                _list.EnsureCapacity(capacity);
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
			List<EntityId> output,
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
				ListCollectorEntityIdOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8, T9
			>(
				new ListCollectorEntityIdOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(output),
				query
			);
		}

		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorWithIdAndComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
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
			private readonly List<(EntityId, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9)> _list;

            public ListCollectorWithIdAndComponents(List<(EntityId, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9)> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9)
            {
                _list.Add((e.ID, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9));
            }

            public void EnsureCapacity(int capacity)
            {
                _list.EnsureCapacity(capacity);
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
			List<(EntityId, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9)> output,
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
				ListCollectorWithIdAndComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8, T9
			>(
				new ListCollectorWithIdAndComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(output),
				query
			);
		}
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorEntityIdOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
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
			private readonly List<EntityId> _list;

            public ListCollectorEntityIdOnly(List<EntityId> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10)
            {
                _list.Add(e.ID);
            }

            public void EnsureCapacity(int capacity)
            {
                _list.EnsureCapacity(capacity);
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
			List<EntityId> output,
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
				ListCollectorEntityIdOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10
			>(
				new ListCollectorEntityIdOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(output),
				query
			);
		}

		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorWithIdAndComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
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
			private readonly List<(EntityId, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> _list;

            public ListCollectorWithIdAndComponents(List<(EntityId, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10)
            {
                _list.Add((e.ID, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10));
            }

            public void EnsureCapacity(int capacity)
            {
                _list.EnsureCapacity(capacity);
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
			List<(EntityId, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> output,
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
				ListCollectorWithIdAndComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10
			>(
				new ListCollectorWithIdAndComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(output),
				query
			);
		}
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorEntityIdOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
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
			private readonly List<EntityId> _list;

            public ListCollectorEntityIdOnly(List<EntityId> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11)
            {
                _list.Add(e.ID);
            }

            public void EnsureCapacity(int capacity)
            {
                _list.EnsureCapacity(capacity);
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
			List<EntityId> output,
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
				ListCollectorEntityIdOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11
			>(
				new ListCollectorEntityIdOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(output),
				query
			);
		}

		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorWithIdAndComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
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
			private readonly List<(EntityId, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> _list;

            public ListCollectorWithIdAndComponents(List<(EntityId, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11)
            {
                _list.Add((e.ID, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11));
            }

            public void EnsureCapacity(int capacity)
            {
                _list.EnsureCapacity(capacity);
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
			List<(EntityId, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> output,
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
				ListCollectorWithIdAndComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11
			>(
				new ListCollectorWithIdAndComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(output),
				query
			);
		}
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorEntityIdOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
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
			private readonly List<EntityId> _list;

            public ListCollectorEntityIdOnly(List<EntityId> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12)
            {
                _list.Add(e.ID);
            }

            public void EnsureCapacity(int capacity)
            {
                _list.EnsureCapacity(capacity);
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
			List<EntityId> output,
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
				ListCollectorEntityIdOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12
			>(
				new ListCollectorEntityIdOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(output),
				query
			);
		}

		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorWithIdAndComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
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
			private readonly List<(EntityId, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> _list;

            public ListCollectorWithIdAndComponents(List<(EntityId, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12)
            {
                _list.Add((e.ID, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12));
            }

            public void EnsureCapacity(int capacity)
            {
                _list.EnsureCapacity(capacity);
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
			List<(EntityId, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> output,
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
				ListCollectorWithIdAndComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12
			>(
				new ListCollectorWithIdAndComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(output),
				query
			);
		}
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorEntityIdOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
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
			private readonly List<EntityId> _list;

            public ListCollectorEntityIdOnly(List<EntityId> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12, ref T13 t13)
            {
                _list.Add(e.ID);
            }

            public void EnsureCapacity(int capacity)
            {
                _list.EnsureCapacity(capacity);
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
			List<EntityId> output,
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
				ListCollectorEntityIdOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13
			>(
				new ListCollectorEntityIdOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(output),
				query
			);
		}

		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorWithIdAndComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
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
			private readonly List<(EntityId, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)> _list;

            public ListCollectorWithIdAndComponents(List<(EntityId, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12, ref T13 t13)
            {
                _list.Add((e.ID, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13));
            }

            public void EnsureCapacity(int capacity)
            {
                _list.EnsureCapacity(capacity);
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
			List<(EntityId, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)> output,
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
				ListCollectorWithIdAndComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13
			>(
				new ListCollectorWithIdAndComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(output),
				query
			);
		}
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorEntityIdOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
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
			private readonly List<EntityId> _list;

            public ListCollectorEntityIdOnly(List<EntityId> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12, ref T13 t13, ref T14 t14)
            {
                _list.Add(e.ID);
            }

            public void EnsureCapacity(int capacity)
            {
                _list.EnsureCapacity(capacity);
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
			List<EntityId> output,
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
				ListCollectorEntityIdOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14
			>(
				new ListCollectorEntityIdOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(output),
				query
			);
		}

		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorWithIdAndComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
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
			private readonly List<(EntityId, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)> _list;

            public ListCollectorWithIdAndComponents(List<(EntityId, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12, ref T13 t13, ref T14 t14)
            {
                _list.Add((e.ID, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14));
            }

            public void EnsureCapacity(int capacity)
            {
                _list.EnsureCapacity(capacity);
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
			List<(EntityId, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)> output,
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
				ListCollectorWithIdAndComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14
			>(
				new ListCollectorWithIdAndComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(output),
				query
			);
		}
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorEntityIdOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
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
			private readonly List<EntityId> _list;

            public ListCollectorEntityIdOnly(List<EntityId> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12, ref T13 t13, ref T14 t14, ref T15 t15)
            {
                _list.Add(e.ID);
            }

            public void EnsureCapacity(int capacity)
            {
                _list.EnsureCapacity(capacity);
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
			List<EntityId> output,
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
				ListCollectorEntityIdOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15
			>(
				new ListCollectorEntityIdOnly<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(output),
				query
			);
		}

		[ExcludeFromCodeCoverage]
		private readonly struct ListCollectorWithIdAndComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
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
			private readonly List<(EntityId, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)> _list;

            public ListCollectorWithIdAndComponents(List<(EntityId, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)> list)
            {
                _list = list;
            }

            public void Add(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12, ref T13 t13, ref T14 t14, ref T15 t15)
            {
                _list.Add((e.ID, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15));
            }

            public void EnsureCapacity(int capacity)
            {
                _list.EnsureCapacity(capacity);
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
			List<(EntityId, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)> output,
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
				ListCollectorWithIdAndComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>,
				T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15
			>(
				new ListCollectorWithIdAndComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(output),
				query
			);
		}
	}
}


