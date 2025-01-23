using Myriad.ECS.Queries;
using Myriad.ECS.IDs;
using System.Diagnostics.CodeAnalysis;

//using Parallel = System.Threading.Tasks.Parallel;
//using Parallel = ParallelTasks.Parallel;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedParameter.Global
// ReSharper disable LoopCanBeConvertedToQuery
// ReSharper disable ConvertToPrimaryConstructor

namespace Myriad.ECS.Queries
{
    /// <summary>
	/// Reduce intermediate values
	/// </summary>
	public interface IQueryReduce<TValue>
	{
		/// <summary>
		/// Reduce two intermediate value to one
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public TValue Reduce(TValue a, TValue b);
	}
}

namespace Myriad.ECS.Queries
{
	/// <summary>
	/// Map from a set of components to a single value
	/// </summary>
	public interface IQueryMap<out TReducer, T0>
		where T0 : IComponent
	{
		/// <summary>
		/// Extract a value from the given entity and components
		/// </summary>
		public TReducer Execute(Entity e, ref T0 t0);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0>(
			TOutput initial,
			QueryDescription? query = null
		)
			where T0 : IComponent
			where TMapper : IQueryMap<TOutput, T0>, new()
			where TReducer : IQueryReduce<TOutput>, new()
		{
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0>(initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0>(
			TOutput initial,
			ref QueryDescription? query
		)
			where T0 : IComponent
			where TMapper : IQueryMap<TOutput, T0>, new()
			where TReducer : IQueryReduce<TOutput>, new()
		{
			var q = new TMapper();
			var r = new TReducer();
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0>(ref q, ref r, initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="q">query, which produces a value per entity</param>
		/// <param name="r">Reducer, for combining values</param>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0>(
			TMapper q,
			TReducer r,
			TOutput initial,
			QueryDescription? query = null
		)
			where T0 : IComponent
			where TMapper : IQueryMap<TOutput, T0>
			where TReducer : IQueryReduce<TOutput>
		{
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0>(ref q, ref r, initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <typeparam name="TMapper">Type of mapper</typeparam>
		/// <typeparam name="TReducer">Type of reducer</typeparam>
		/// <typeparam name="TOutput">Type of value returned</typeparam>
		/// <typeparam name="T0">Component 0 to pass into mapper</typeparam>
		/// <param name="q">query, which produces a value per entity</param>
		/// <param name="r">Reducer, for combining values</param>
		/// <param name="initial">Initial value to start reducing from</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0>(
			ref TMapper q,
			ref TReducer r,
			TOutput initial,
			ref QueryDescription? query
		)
			where T0 : IComponent
			where TMapper : IQueryMap<TOutput, T0>
			where TReducer : IQueryReduce<TOutput>
		{
			query ??= GetCachedQuery<T0>();

			var archetypes = query.GetArchetypes();

			var c0 = ComponentID<T0>.ID;

			var accumulator = initial;

			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];
					var entities = chunk.Entities.Span;

					var t0 = chunk.GetSpan<T0>(c0);

					for (var i = entities.Length - 1; i >= 0; i--)
					{
						accumulator = r.Reduce(accumulator, q.Execute(entities[i], ref t0[i]));
					}
				}
			}

			return accumulator;
		}
	}
}
namespace Myriad.ECS.Queries
{
	/// <summary>
	/// Map from a set of components to a single value
	/// </summary>
	public interface IQueryMap<out TReducer, T0, T1>
		where T0 : IComponent
        where T1 : IComponent
	{
		/// <summary>
		/// Extract a value from the given entity and components
		/// </summary>
		public TReducer Execute(Entity e, ref T0 t0, ref T1 t1);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1>(
			TOutput initial,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
			where TMapper : IQueryMap<TOutput, T0, T1>, new()
			where TReducer : IQueryReduce<TOutput>, new()
		{
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1>(initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1>(
			TOutput initial,
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
			where TMapper : IQueryMap<TOutput, T0, T1>, new()
			where TReducer : IQueryReduce<TOutput>, new()
		{
			var q = new TMapper();
			var r = new TReducer();
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1>(ref q, ref r, initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="q">query, which produces a value per entity</param>
		/// <param name="r">Reducer, for combining values</param>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1>(
			TMapper q,
			TReducer r,
			TOutput initial,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
			where TMapper : IQueryMap<TOutput, T0, T1>
			where TReducer : IQueryReduce<TOutput>
		{
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1>(ref q, ref r, initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <typeparam name="TMapper">Type of mapper</typeparam>
		/// <typeparam name="TReducer">Type of reducer</typeparam>
		/// <typeparam name="TOutput">Type of value returned</typeparam>
		/// <typeparam name="T0">Component 0 to pass into mapper</typeparam>
		/// <typeparam name="T1">Component 1 to pass into mapper</typeparam>
		/// <param name="q">query, which produces a value per entity</param>
		/// <param name="r">Reducer, for combining values</param>
		/// <param name="initial">Initial value to start reducing from</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1>(
			ref TMapper q,
			ref TReducer r,
			TOutput initial,
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
			where TMapper : IQueryMap<TOutput, T0, T1>
			where TReducer : IQueryReduce<TOutput>
		{
			query ??= GetCachedQuery<T0, T1>();

			var archetypes = query.GetArchetypes();

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;

			var accumulator = initial;

			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];
					var entities = chunk.Entities.Span;

					var t0 = chunk.GetSpan<T0>(c0);
					var t1 = chunk.GetSpan<T1>(c1);

					for (var i = entities.Length - 1; i >= 0; i--)
					{
						accumulator = r.Reduce(accumulator, q.Execute(entities[i], ref t0[i], ref t1[i]));
					}
				}
			}

			return accumulator;
		}
	}
}
namespace Myriad.ECS.Queries
{
	/// <summary>
	/// Map from a set of components to a single value
	/// </summary>
	public interface IQueryMap<out TReducer, T0, T1, T2>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
	{
		/// <summary>
		/// Extract a value from the given entity and components
		/// </summary>
		public TReducer Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2>(
			TOutput initial,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
			where TMapper : IQueryMap<TOutput, T0, T1, T2>, new()
			where TReducer : IQueryReduce<TOutput>, new()
		{
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2>(initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2>(
			TOutput initial,
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
			where TMapper : IQueryMap<TOutput, T0, T1, T2>, new()
			where TReducer : IQueryReduce<TOutput>, new()
		{
			var q = new TMapper();
			var r = new TReducer();
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2>(ref q, ref r, initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="q">query, which produces a value per entity</param>
		/// <param name="r">Reducer, for combining values</param>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2>(
			TMapper q,
			TReducer r,
			TOutput initial,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
			where TMapper : IQueryMap<TOutput, T0, T1, T2>
			where TReducer : IQueryReduce<TOutput>
		{
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2>(ref q, ref r, initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <typeparam name="TMapper">Type of mapper</typeparam>
		/// <typeparam name="TReducer">Type of reducer</typeparam>
		/// <typeparam name="TOutput">Type of value returned</typeparam>
		/// <typeparam name="T0">Component 0 to pass into mapper</typeparam>
		/// <typeparam name="T1">Component 1 to pass into mapper</typeparam>
		/// <typeparam name="T2">Component 2 to pass into mapper</typeparam>
		/// <param name="q">query, which produces a value per entity</param>
		/// <param name="r">Reducer, for combining values</param>
		/// <param name="initial">Initial value to start reducing from</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2>(
			ref TMapper q,
			ref TReducer r,
			TOutput initial,
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
			where TMapper : IQueryMap<TOutput, T0, T1, T2>
			where TReducer : IQueryReduce<TOutput>
		{
			query ??= GetCachedQuery<T0, T1, T2>();

			var archetypes = query.GetArchetypes();

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;
			var c2 = ComponentID<T2>.ID;

			var accumulator = initial;

			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];
					var entities = chunk.Entities.Span;

					var t0 = chunk.GetSpan<T0>(c0);
					var t1 = chunk.GetSpan<T1>(c1);
					var t2 = chunk.GetSpan<T2>(c2);

					for (var i = entities.Length - 1; i >= 0; i--)
					{
						accumulator = r.Reduce(accumulator, q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i]));
					}
				}
			}

			return accumulator;
		}
	}
}
namespace Myriad.ECS.Queries
{
	/// <summary>
	/// Map from a set of components to a single value
	/// </summary>
	public interface IQueryMap<out TReducer, T0, T1, T2, T3>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
	{
		/// <summary>
		/// Extract a value from the given entity and components
		/// </summary>
		public TReducer Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3>(
			TOutput initial,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3>, new()
			where TReducer : IQueryReduce<TOutput>, new()
		{
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3>(initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3>(
			TOutput initial,
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3>, new()
			where TReducer : IQueryReduce<TOutput>, new()
		{
			var q = new TMapper();
			var r = new TReducer();
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3>(ref q, ref r, initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="q">query, which produces a value per entity</param>
		/// <param name="r">Reducer, for combining values</param>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3>(
			TMapper q,
			TReducer r,
			TOutput initial,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3>
			where TReducer : IQueryReduce<TOutput>
		{
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3>(ref q, ref r, initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <typeparam name="TMapper">Type of mapper</typeparam>
		/// <typeparam name="TReducer">Type of reducer</typeparam>
		/// <typeparam name="TOutput">Type of value returned</typeparam>
		/// <typeparam name="T0">Component 0 to pass into mapper</typeparam>
		/// <typeparam name="T1">Component 1 to pass into mapper</typeparam>
		/// <typeparam name="T2">Component 2 to pass into mapper</typeparam>
		/// <typeparam name="T3">Component 3 to pass into mapper</typeparam>
		/// <param name="q">query, which produces a value per entity</param>
		/// <param name="r">Reducer, for combining values</param>
		/// <param name="initial">Initial value to start reducing from</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3>(
			ref TMapper q,
			ref TReducer r,
			TOutput initial,
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3>
			where TReducer : IQueryReduce<TOutput>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3>();

			var archetypes = query.GetArchetypes();

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;
			var c2 = ComponentID<T2>.ID;
			var c3 = ComponentID<T3>.ID;

			var accumulator = initial;

			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];
					var entities = chunk.Entities.Span;

					var t0 = chunk.GetSpan<T0>(c0);
					var t1 = chunk.GetSpan<T1>(c1);
					var t2 = chunk.GetSpan<T2>(c2);
					var t3 = chunk.GetSpan<T3>(c3);

					for (var i = entities.Length - 1; i >= 0; i--)
					{
						accumulator = r.Reduce(accumulator, q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i]));
					}
				}
			}

			return accumulator;
		}
	}
}
namespace Myriad.ECS.Queries
{
	/// <summary>
	/// Map from a set of components to a single value
	/// </summary>
	public interface IQueryMap<out TReducer, T0, T1, T2, T3, T4>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
	{
		/// <summary>
		/// Extract a value from the given entity and components
		/// </summary>
		public TReducer Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4>(
			TOutput initial,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4>, new()
			where TReducer : IQueryReduce<TOutput>, new()
		{
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4>(initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4>(
			TOutput initial,
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4>, new()
			where TReducer : IQueryReduce<TOutput>, new()
		{
			var q = new TMapper();
			var r = new TReducer();
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4>(ref q, ref r, initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="q">query, which produces a value per entity</param>
		/// <param name="r">Reducer, for combining values</param>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4>(
			TMapper q,
			TReducer r,
			TOutput initial,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4>
			where TReducer : IQueryReduce<TOutput>
		{
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4>(ref q, ref r, initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <typeparam name="TMapper">Type of mapper</typeparam>
		/// <typeparam name="TReducer">Type of reducer</typeparam>
		/// <typeparam name="TOutput">Type of value returned</typeparam>
		/// <typeparam name="T0">Component 0 to pass into mapper</typeparam>
		/// <typeparam name="T1">Component 1 to pass into mapper</typeparam>
		/// <typeparam name="T2">Component 2 to pass into mapper</typeparam>
		/// <typeparam name="T3">Component 3 to pass into mapper</typeparam>
		/// <typeparam name="T4">Component 4 to pass into mapper</typeparam>
		/// <param name="q">query, which produces a value per entity</param>
		/// <param name="r">Reducer, for combining values</param>
		/// <param name="initial">Initial value to start reducing from</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4>(
			ref TMapper q,
			ref TReducer r,
			TOutput initial,
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4>
			where TReducer : IQueryReduce<TOutput>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4>();

			var archetypes = query.GetArchetypes();

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;
			var c2 = ComponentID<T2>.ID;
			var c3 = ComponentID<T3>.ID;
			var c4 = ComponentID<T4>.ID;

			var accumulator = initial;

			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];
					var entities = chunk.Entities.Span;

					var t0 = chunk.GetSpan<T0>(c0);
					var t1 = chunk.GetSpan<T1>(c1);
					var t2 = chunk.GetSpan<T2>(c2);
					var t3 = chunk.GetSpan<T3>(c3);
					var t4 = chunk.GetSpan<T4>(c4);

					for (var i = entities.Length - 1; i >= 0; i--)
					{
						accumulator = r.Reduce(accumulator, q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i]));
					}
				}
			}

			return accumulator;
		}
	}
}
namespace Myriad.ECS.Queries
{
	/// <summary>
	/// Map from a set of components to a single value
	/// </summary>
	public interface IQueryMap<out TReducer, T0, T1, T2, T3, T4, T5>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
	{
		/// <summary>
		/// Extract a value from the given entity and components
		/// </summary>
		public TReducer Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5>(
			TOutput initial,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4, T5>, new()
			where TReducer : IQueryReduce<TOutput>, new()
		{
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5>(initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5>(
			TOutput initial,
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4, T5>, new()
			where TReducer : IQueryReduce<TOutput>, new()
		{
			var q = new TMapper();
			var r = new TReducer();
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5>(ref q, ref r, initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="q">query, which produces a value per entity</param>
		/// <param name="r">Reducer, for combining values</param>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5>(
			TMapper q,
			TReducer r,
			TOutput initial,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4, T5>
			where TReducer : IQueryReduce<TOutput>
		{
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5>(ref q, ref r, initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <typeparam name="TMapper">Type of mapper</typeparam>
		/// <typeparam name="TReducer">Type of reducer</typeparam>
		/// <typeparam name="TOutput">Type of value returned</typeparam>
		/// <typeparam name="T0">Component 0 to pass into mapper</typeparam>
		/// <typeparam name="T1">Component 1 to pass into mapper</typeparam>
		/// <typeparam name="T2">Component 2 to pass into mapper</typeparam>
		/// <typeparam name="T3">Component 3 to pass into mapper</typeparam>
		/// <typeparam name="T4">Component 4 to pass into mapper</typeparam>
		/// <typeparam name="T5">Component 5 to pass into mapper</typeparam>
		/// <param name="q">query, which produces a value per entity</param>
		/// <param name="r">Reducer, for combining values</param>
		/// <param name="initial">Initial value to start reducing from</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5>(
			ref TMapper q,
			ref TReducer r,
			TOutput initial,
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4, T5>
			where TReducer : IQueryReduce<TOutput>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5>();

			var archetypes = query.GetArchetypes();

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;
			var c2 = ComponentID<T2>.ID;
			var c3 = ComponentID<T3>.ID;
			var c4 = ComponentID<T4>.ID;
			var c5 = ComponentID<T5>.ID;

			var accumulator = initial;

			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];
					var entities = chunk.Entities.Span;

					var t0 = chunk.GetSpan<T0>(c0);
					var t1 = chunk.GetSpan<T1>(c1);
					var t2 = chunk.GetSpan<T2>(c2);
					var t3 = chunk.GetSpan<T3>(c3);
					var t4 = chunk.GetSpan<T4>(c4);
					var t5 = chunk.GetSpan<T5>(c5);

					for (var i = entities.Length - 1; i >= 0; i--)
					{
						accumulator = r.Reduce(accumulator, q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i]));
					}
				}
			}

			return accumulator;
		}
	}
}
namespace Myriad.ECS.Queries
{
	/// <summary>
	/// Map from a set of components to a single value
	/// </summary>
	public interface IQueryMap<out TReducer, T0, T1, T2, T3, T4, T5, T6>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
	{
		/// <summary>
		/// Extract a value from the given entity and components
		/// </summary>
		public TReducer Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6>(
			TOutput initial,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4, T5, T6>, new()
			where TReducer : IQueryReduce<TOutput>, new()
		{
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6>(initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6>(
			TOutput initial,
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4, T5, T6>, new()
			where TReducer : IQueryReduce<TOutput>, new()
		{
			var q = new TMapper();
			var r = new TReducer();
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6>(ref q, ref r, initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="q">query, which produces a value per entity</param>
		/// <param name="r">Reducer, for combining values</param>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6>(
			TMapper q,
			TReducer r,
			TOutput initial,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4, T5, T6>
			where TReducer : IQueryReduce<TOutput>
		{
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6>(ref q, ref r, initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <typeparam name="TMapper">Type of mapper</typeparam>
		/// <typeparam name="TReducer">Type of reducer</typeparam>
		/// <typeparam name="TOutput">Type of value returned</typeparam>
		/// <typeparam name="T0">Component 0 to pass into mapper</typeparam>
		/// <typeparam name="T1">Component 1 to pass into mapper</typeparam>
		/// <typeparam name="T2">Component 2 to pass into mapper</typeparam>
		/// <typeparam name="T3">Component 3 to pass into mapper</typeparam>
		/// <typeparam name="T4">Component 4 to pass into mapper</typeparam>
		/// <typeparam name="T5">Component 5 to pass into mapper</typeparam>
		/// <typeparam name="T6">Component 6 to pass into mapper</typeparam>
		/// <param name="q">query, which produces a value per entity</param>
		/// <param name="r">Reducer, for combining values</param>
		/// <param name="initial">Initial value to start reducing from</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6>(
			ref TMapper q,
			ref TReducer r,
			TOutput initial,
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4, T5, T6>
			where TReducer : IQueryReduce<TOutput>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6>();

			var archetypes = query.GetArchetypes();

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;
			var c2 = ComponentID<T2>.ID;
			var c3 = ComponentID<T3>.ID;
			var c4 = ComponentID<T4>.ID;
			var c5 = ComponentID<T5>.ID;
			var c6 = ComponentID<T6>.ID;

			var accumulator = initial;

			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];
					var entities = chunk.Entities.Span;

					var t0 = chunk.GetSpan<T0>(c0);
					var t1 = chunk.GetSpan<T1>(c1);
					var t2 = chunk.GetSpan<T2>(c2);
					var t3 = chunk.GetSpan<T3>(c3);
					var t4 = chunk.GetSpan<T4>(c4);
					var t5 = chunk.GetSpan<T5>(c5);
					var t6 = chunk.GetSpan<T6>(c6);

					for (var i = entities.Length - 1; i >= 0; i--)
					{
						accumulator = r.Reduce(accumulator, q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i]));
					}
				}
			}

			return accumulator;
		}
	}
}
namespace Myriad.ECS.Queries
{
	/// <summary>
	/// Map from a set of components to a single value
	/// </summary>
	public interface IQueryMap<out TReducer, T0, T1, T2, T3, T4, T5, T6, T7>
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
		/// Extract a value from the given entity and components
		/// </summary>
		public TReducer Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7>(
			TOutput initial,
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
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4, T5, T6, T7>, new()
			where TReducer : IQueryReduce<TOutput>, new()
		{
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7>(initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7>(
			TOutput initial,
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
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4, T5, T6, T7>, new()
			where TReducer : IQueryReduce<TOutput>, new()
		{
			var q = new TMapper();
			var r = new TReducer();
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7>(ref q, ref r, initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="q">query, which produces a value per entity</param>
		/// <param name="r">Reducer, for combining values</param>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7>(
			TMapper q,
			TReducer r,
			TOutput initial,
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
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4, T5, T6, T7>
			where TReducer : IQueryReduce<TOutput>
		{
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7>(ref q, ref r, initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <typeparam name="TMapper">Type of mapper</typeparam>
		/// <typeparam name="TReducer">Type of reducer</typeparam>
		/// <typeparam name="TOutput">Type of value returned</typeparam>
		/// <typeparam name="T0">Component 0 to pass into mapper</typeparam>
		/// <typeparam name="T1">Component 1 to pass into mapper</typeparam>
		/// <typeparam name="T2">Component 2 to pass into mapper</typeparam>
		/// <typeparam name="T3">Component 3 to pass into mapper</typeparam>
		/// <typeparam name="T4">Component 4 to pass into mapper</typeparam>
		/// <typeparam name="T5">Component 5 to pass into mapper</typeparam>
		/// <typeparam name="T6">Component 6 to pass into mapper</typeparam>
		/// <typeparam name="T7">Component 7 to pass into mapper</typeparam>
		/// <param name="q">query, which produces a value per entity</param>
		/// <param name="r">Reducer, for combining values</param>
		/// <param name="initial">Initial value to start reducing from</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7>(
			ref TMapper q,
			ref TReducer r,
			TOutput initial,
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
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4, T5, T6, T7>
			where TReducer : IQueryReduce<TOutput>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7>();

			var archetypes = query.GetArchetypes();

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;
			var c2 = ComponentID<T2>.ID;
			var c3 = ComponentID<T3>.ID;
			var c4 = ComponentID<T4>.ID;
			var c5 = ComponentID<T5>.ID;
			var c6 = ComponentID<T6>.ID;
			var c7 = ComponentID<T7>.ID;

			var accumulator = initial;

			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];
					var entities = chunk.Entities.Span;

					var t0 = chunk.GetSpan<T0>(c0);
					var t1 = chunk.GetSpan<T1>(c1);
					var t2 = chunk.GetSpan<T2>(c2);
					var t3 = chunk.GetSpan<T3>(c3);
					var t4 = chunk.GetSpan<T4>(c4);
					var t5 = chunk.GetSpan<T5>(c5);
					var t6 = chunk.GetSpan<T6>(c6);
					var t7 = chunk.GetSpan<T7>(c7);

					for (var i = entities.Length - 1; i >= 0; i--)
					{
						accumulator = r.Reduce(accumulator, q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i]));
					}
				}
			}

			return accumulator;
		}
	}
}
namespace Myriad.ECS.Queries
{
	/// <summary>
	/// Map from a set of components to a single value
	/// </summary>
	public interface IQueryMap<out TReducer, T0, T1, T2, T3, T4, T5, T6, T7, T8>
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
		/// Extract a value from the given entity and components
		/// </summary>
		public TReducer Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
			TOutput initial,
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
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8>, new()
			where TReducer : IQueryReduce<TOutput>, new()
		{
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8>(initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
			TOutput initial,
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
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8>, new()
			where TReducer : IQueryReduce<TOutput>, new()
		{
			var q = new TMapper();
			var r = new TReducer();
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8>(ref q, ref r, initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="q">query, which produces a value per entity</param>
		/// <param name="r">Reducer, for combining values</param>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
			TMapper q,
			TReducer r,
			TOutput initial,
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
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8>
			where TReducer : IQueryReduce<TOutput>
		{
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8>(ref q, ref r, initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <typeparam name="TMapper">Type of mapper</typeparam>
		/// <typeparam name="TReducer">Type of reducer</typeparam>
		/// <typeparam name="TOutput">Type of value returned</typeparam>
		/// <typeparam name="T0">Component 0 to pass into mapper</typeparam>
		/// <typeparam name="T1">Component 1 to pass into mapper</typeparam>
		/// <typeparam name="T2">Component 2 to pass into mapper</typeparam>
		/// <typeparam name="T3">Component 3 to pass into mapper</typeparam>
		/// <typeparam name="T4">Component 4 to pass into mapper</typeparam>
		/// <typeparam name="T5">Component 5 to pass into mapper</typeparam>
		/// <typeparam name="T6">Component 6 to pass into mapper</typeparam>
		/// <typeparam name="T7">Component 7 to pass into mapper</typeparam>
		/// <typeparam name="T8">Component 8 to pass into mapper</typeparam>
		/// <param name="q">query, which produces a value per entity</param>
		/// <param name="r">Reducer, for combining values</param>
		/// <param name="initial">Initial value to start reducing from</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
			ref TMapper q,
			ref TReducer r,
			TOutput initial,
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
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8>
			where TReducer : IQueryReduce<TOutput>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8>();

			var archetypes = query.GetArchetypes();

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;
			var c2 = ComponentID<T2>.ID;
			var c3 = ComponentID<T3>.ID;
			var c4 = ComponentID<T4>.ID;
			var c5 = ComponentID<T5>.ID;
			var c6 = ComponentID<T6>.ID;
			var c7 = ComponentID<T7>.ID;
			var c8 = ComponentID<T8>.ID;

			var accumulator = initial;

			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];
					var entities = chunk.Entities.Span;

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
					{
						accumulator = r.Reduce(accumulator, q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i]));
					}
				}
			}

			return accumulator;
		}
	}
}
namespace Myriad.ECS.Queries
{
	/// <summary>
	/// Map from a set of components to a single value
	/// </summary>
	public interface IQueryMap<out TReducer, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
		where T0 : IComponent
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
		/// Extract a value from the given entity and components
		/// </summary>
		public TReducer Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
			TOutput initial,
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
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>, new()
			where TReducer : IQueryReduce<TOutput>, new()
		{
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
			TOutput initial,
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
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>, new()
			where TReducer : IQueryReduce<TOutput>, new()
		{
			var q = new TMapper();
			var r = new TReducer();
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(ref q, ref r, initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="q">query, which produces a value per entity</param>
		/// <param name="r">Reducer, for combining values</param>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
			TMapper q,
			TReducer r,
			TOutput initial,
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
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
			where TReducer : IQueryReduce<TOutput>
		{
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(ref q, ref r, initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <typeparam name="TMapper">Type of mapper</typeparam>
		/// <typeparam name="TReducer">Type of reducer</typeparam>
		/// <typeparam name="TOutput">Type of value returned</typeparam>
		/// <typeparam name="T0">Component 0 to pass into mapper</typeparam>
		/// <typeparam name="T1">Component 1 to pass into mapper</typeparam>
		/// <typeparam name="T2">Component 2 to pass into mapper</typeparam>
		/// <typeparam name="T3">Component 3 to pass into mapper</typeparam>
		/// <typeparam name="T4">Component 4 to pass into mapper</typeparam>
		/// <typeparam name="T5">Component 5 to pass into mapper</typeparam>
		/// <typeparam name="T6">Component 6 to pass into mapper</typeparam>
		/// <typeparam name="T7">Component 7 to pass into mapper</typeparam>
		/// <typeparam name="T8">Component 8 to pass into mapper</typeparam>
		/// <typeparam name="T9">Component 9 to pass into mapper</typeparam>
		/// <param name="q">query, which produces a value per entity</param>
		/// <param name="r">Reducer, for combining values</param>
		/// <param name="initial">Initial value to start reducing from</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
			ref TMapper q,
			ref TReducer r,
			TOutput initial,
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
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
			where TReducer : IQueryReduce<TOutput>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>();

			var archetypes = query.GetArchetypes();

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;
			var c2 = ComponentID<T2>.ID;
			var c3 = ComponentID<T3>.ID;
			var c4 = ComponentID<T4>.ID;
			var c5 = ComponentID<T5>.ID;
			var c6 = ComponentID<T6>.ID;
			var c7 = ComponentID<T7>.ID;
			var c8 = ComponentID<T8>.ID;
			var c9 = ComponentID<T9>.ID;

			var accumulator = initial;

			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];
					var entities = chunk.Entities.Span;

					var t0 = chunk.GetSpan<T0>(c0);
					var t1 = chunk.GetSpan<T1>(c1);
					var t2 = chunk.GetSpan<T2>(c2);
					var t3 = chunk.GetSpan<T3>(c3);
					var t4 = chunk.GetSpan<T4>(c4);
					var t5 = chunk.GetSpan<T5>(c5);
					var t6 = chunk.GetSpan<T6>(c6);
					var t7 = chunk.GetSpan<T7>(c7);
					var t8 = chunk.GetSpan<T8>(c8);
					var t9 = chunk.GetSpan<T9>(c9);

					for (var i = entities.Length - 1; i >= 0; i--)
					{
						accumulator = r.Reduce(accumulator, q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i]));
					}
				}
			}

			return accumulator;
		}
	}
}
namespace Myriad.ECS.Queries
{
	/// <summary>
	/// Map from a set of components to a single value
	/// </summary>
	public interface IQueryMap<out TReducer, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
		where T0 : IComponent
        where T1 : IComponent
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
		/// Extract a value from the given entity and components
		/// </summary>
		public TReducer Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
			TOutput initial,
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
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, new()
			where TReducer : IQueryReduce<TOutput>, new()
		{
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
			TOutput initial,
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
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, new()
			where TReducer : IQueryReduce<TOutput>, new()
		{
			var q = new TMapper();
			var r = new TReducer();
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(ref q, ref r, initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="q">query, which produces a value per entity</param>
		/// <param name="r">Reducer, for combining values</param>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
			TMapper q,
			TReducer r,
			TOutput initial,
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
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
			where TReducer : IQueryReduce<TOutput>
		{
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(ref q, ref r, initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <typeparam name="TMapper">Type of mapper</typeparam>
		/// <typeparam name="TReducer">Type of reducer</typeparam>
		/// <typeparam name="TOutput">Type of value returned</typeparam>
		/// <typeparam name="T0">Component 0 to pass into mapper</typeparam>
		/// <typeparam name="T1">Component 1 to pass into mapper</typeparam>
		/// <typeparam name="T2">Component 2 to pass into mapper</typeparam>
		/// <typeparam name="T3">Component 3 to pass into mapper</typeparam>
		/// <typeparam name="T4">Component 4 to pass into mapper</typeparam>
		/// <typeparam name="T5">Component 5 to pass into mapper</typeparam>
		/// <typeparam name="T6">Component 6 to pass into mapper</typeparam>
		/// <typeparam name="T7">Component 7 to pass into mapper</typeparam>
		/// <typeparam name="T8">Component 8 to pass into mapper</typeparam>
		/// <typeparam name="T9">Component 9 to pass into mapper</typeparam>
		/// <typeparam name="T10">Component 10 to pass into mapper</typeparam>
		/// <param name="q">query, which produces a value per entity</param>
		/// <param name="r">Reducer, for combining values</param>
		/// <param name="initial">Initial value to start reducing from</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
			ref TMapper q,
			ref TReducer r,
			TOutput initial,
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
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
			where TReducer : IQueryReduce<TOutput>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>();

			var archetypes = query.GetArchetypes();

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;
			var c2 = ComponentID<T2>.ID;
			var c3 = ComponentID<T3>.ID;
			var c4 = ComponentID<T4>.ID;
			var c5 = ComponentID<T5>.ID;
			var c6 = ComponentID<T6>.ID;
			var c7 = ComponentID<T7>.ID;
			var c8 = ComponentID<T8>.ID;
			var c9 = ComponentID<T9>.ID;
			var c10 = ComponentID<T10>.ID;

			var accumulator = initial;

			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];
					var entities = chunk.Entities.Span;

					var t0 = chunk.GetSpan<T0>(c0);
					var t1 = chunk.GetSpan<T1>(c1);
					var t2 = chunk.GetSpan<T2>(c2);
					var t3 = chunk.GetSpan<T3>(c3);
					var t4 = chunk.GetSpan<T4>(c4);
					var t5 = chunk.GetSpan<T5>(c5);
					var t6 = chunk.GetSpan<T6>(c6);
					var t7 = chunk.GetSpan<T7>(c7);
					var t8 = chunk.GetSpan<T8>(c8);
					var t9 = chunk.GetSpan<T9>(c9);
					var t10 = chunk.GetSpan<T10>(c10);

					for (var i = entities.Length - 1; i >= 0; i--)
					{
						accumulator = r.Reduce(accumulator, q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i]));
					}
				}
			}

			return accumulator;
		}
	}
}
namespace Myriad.ECS.Queries
{
	/// <summary>
	/// Map from a set of components to a single value
	/// </summary>
	public interface IQueryMap<out TReducer, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
		where T0 : IComponent
        where T1 : IComponent
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
		/// Extract a value from the given entity and components
		/// </summary>
		public TReducer Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
			TOutput initial,
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
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, new()
			where TReducer : IQueryReduce<TOutput>, new()
		{
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
			TOutput initial,
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
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, new()
			where TReducer : IQueryReduce<TOutput>, new()
		{
			var q = new TMapper();
			var r = new TReducer();
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(ref q, ref r, initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="q">query, which produces a value per entity</param>
		/// <param name="r">Reducer, for combining values</param>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
			TMapper q,
			TReducer r,
			TOutput initial,
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
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
			where TReducer : IQueryReduce<TOutput>
		{
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(ref q, ref r, initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <typeparam name="TMapper">Type of mapper</typeparam>
		/// <typeparam name="TReducer">Type of reducer</typeparam>
		/// <typeparam name="TOutput">Type of value returned</typeparam>
		/// <typeparam name="T0">Component 0 to pass into mapper</typeparam>
		/// <typeparam name="T1">Component 1 to pass into mapper</typeparam>
		/// <typeparam name="T2">Component 2 to pass into mapper</typeparam>
		/// <typeparam name="T3">Component 3 to pass into mapper</typeparam>
		/// <typeparam name="T4">Component 4 to pass into mapper</typeparam>
		/// <typeparam name="T5">Component 5 to pass into mapper</typeparam>
		/// <typeparam name="T6">Component 6 to pass into mapper</typeparam>
		/// <typeparam name="T7">Component 7 to pass into mapper</typeparam>
		/// <typeparam name="T8">Component 8 to pass into mapper</typeparam>
		/// <typeparam name="T9">Component 9 to pass into mapper</typeparam>
		/// <typeparam name="T10">Component 10 to pass into mapper</typeparam>
		/// <typeparam name="T11">Component 11 to pass into mapper</typeparam>
		/// <param name="q">query, which produces a value per entity</param>
		/// <param name="r">Reducer, for combining values</param>
		/// <param name="initial">Initial value to start reducing from</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
			ref TMapper q,
			ref TReducer r,
			TOutput initial,
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
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
			where TReducer : IQueryReduce<TOutput>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>();

			var archetypes = query.GetArchetypes();

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;
			var c2 = ComponentID<T2>.ID;
			var c3 = ComponentID<T3>.ID;
			var c4 = ComponentID<T4>.ID;
			var c5 = ComponentID<T5>.ID;
			var c6 = ComponentID<T6>.ID;
			var c7 = ComponentID<T7>.ID;
			var c8 = ComponentID<T8>.ID;
			var c9 = ComponentID<T9>.ID;
			var c10 = ComponentID<T10>.ID;
			var c11 = ComponentID<T11>.ID;

			var accumulator = initial;

			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];
					var entities = chunk.Entities.Span;

					var t0 = chunk.GetSpan<T0>(c0);
					var t1 = chunk.GetSpan<T1>(c1);
					var t2 = chunk.GetSpan<T2>(c2);
					var t3 = chunk.GetSpan<T3>(c3);
					var t4 = chunk.GetSpan<T4>(c4);
					var t5 = chunk.GetSpan<T5>(c5);
					var t6 = chunk.GetSpan<T6>(c6);
					var t7 = chunk.GetSpan<T7>(c7);
					var t8 = chunk.GetSpan<T8>(c8);
					var t9 = chunk.GetSpan<T9>(c9);
					var t10 = chunk.GetSpan<T10>(c10);
					var t11 = chunk.GetSpan<T11>(c11);

					for (var i = entities.Length - 1; i >= 0; i--)
					{
						accumulator = r.Reduce(accumulator, q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i]));
					}
				}
			}

			return accumulator;
		}
	}
}
namespace Myriad.ECS.Queries
{
	/// <summary>
	/// Map from a set of components to a single value
	/// </summary>
	public interface IQueryMap<out TReducer, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
		where T0 : IComponent
        where T1 : IComponent
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
		/// Extract a value from the given entity and components
		/// </summary>
		public TReducer Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
			TOutput initial,
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
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, new()
			where TReducer : IQueryReduce<TOutput>, new()
		{
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
			TOutput initial,
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
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, new()
			where TReducer : IQueryReduce<TOutput>, new()
		{
			var q = new TMapper();
			var r = new TReducer();
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(ref q, ref r, initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="q">query, which produces a value per entity</param>
		/// <param name="r">Reducer, for combining values</param>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
			TMapper q,
			TReducer r,
			TOutput initial,
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
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
			where TReducer : IQueryReduce<TOutput>
		{
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(ref q, ref r, initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <typeparam name="TMapper">Type of mapper</typeparam>
		/// <typeparam name="TReducer">Type of reducer</typeparam>
		/// <typeparam name="TOutput">Type of value returned</typeparam>
		/// <typeparam name="T0">Component 0 to pass into mapper</typeparam>
		/// <typeparam name="T1">Component 1 to pass into mapper</typeparam>
		/// <typeparam name="T2">Component 2 to pass into mapper</typeparam>
		/// <typeparam name="T3">Component 3 to pass into mapper</typeparam>
		/// <typeparam name="T4">Component 4 to pass into mapper</typeparam>
		/// <typeparam name="T5">Component 5 to pass into mapper</typeparam>
		/// <typeparam name="T6">Component 6 to pass into mapper</typeparam>
		/// <typeparam name="T7">Component 7 to pass into mapper</typeparam>
		/// <typeparam name="T8">Component 8 to pass into mapper</typeparam>
		/// <typeparam name="T9">Component 9 to pass into mapper</typeparam>
		/// <typeparam name="T10">Component 10 to pass into mapper</typeparam>
		/// <typeparam name="T11">Component 11 to pass into mapper</typeparam>
		/// <typeparam name="T12">Component 12 to pass into mapper</typeparam>
		/// <param name="q">query, which produces a value per entity</param>
		/// <param name="r">Reducer, for combining values</param>
		/// <param name="initial">Initial value to start reducing from</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
			ref TMapper q,
			ref TReducer r,
			TOutput initial,
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
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
			where TReducer : IQueryReduce<TOutput>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>();

			var archetypes = query.GetArchetypes();

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;
			var c2 = ComponentID<T2>.ID;
			var c3 = ComponentID<T3>.ID;
			var c4 = ComponentID<T4>.ID;
			var c5 = ComponentID<T5>.ID;
			var c6 = ComponentID<T6>.ID;
			var c7 = ComponentID<T7>.ID;
			var c8 = ComponentID<T8>.ID;
			var c9 = ComponentID<T9>.ID;
			var c10 = ComponentID<T10>.ID;
			var c11 = ComponentID<T11>.ID;
			var c12 = ComponentID<T12>.ID;

			var accumulator = initial;

			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];
					var entities = chunk.Entities.Span;

					var t0 = chunk.GetSpan<T0>(c0);
					var t1 = chunk.GetSpan<T1>(c1);
					var t2 = chunk.GetSpan<T2>(c2);
					var t3 = chunk.GetSpan<T3>(c3);
					var t4 = chunk.GetSpan<T4>(c4);
					var t5 = chunk.GetSpan<T5>(c5);
					var t6 = chunk.GetSpan<T6>(c6);
					var t7 = chunk.GetSpan<T7>(c7);
					var t8 = chunk.GetSpan<T8>(c8);
					var t9 = chunk.GetSpan<T9>(c9);
					var t10 = chunk.GetSpan<T10>(c10);
					var t11 = chunk.GetSpan<T11>(c11);
					var t12 = chunk.GetSpan<T12>(c12);

					for (var i = entities.Length - 1; i >= 0; i--)
					{
						accumulator = r.Reduce(accumulator, q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i]));
					}
				}
			}

			return accumulator;
		}
	}
}
namespace Myriad.ECS.Queries
{
	/// <summary>
	/// Map from a set of components to a single value
	/// </summary>
	public interface IQueryMap<out TReducer, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
		where T0 : IComponent
        where T1 : IComponent
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
		/// Extract a value from the given entity and components
		/// </summary>
		public TReducer Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12, ref T13 t13);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
			TOutput initial,
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
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, new()
			where TReducer : IQueryReduce<TOutput>, new()
		{
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
			TOutput initial,
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
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, new()
			where TReducer : IQueryReduce<TOutput>, new()
		{
			var q = new TMapper();
			var r = new TReducer();
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(ref q, ref r, initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="q">query, which produces a value per entity</param>
		/// <param name="r">Reducer, for combining values</param>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
			TMapper q,
			TReducer r,
			TOutput initial,
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
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
			where TReducer : IQueryReduce<TOutput>
		{
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(ref q, ref r, initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <typeparam name="TMapper">Type of mapper</typeparam>
		/// <typeparam name="TReducer">Type of reducer</typeparam>
		/// <typeparam name="TOutput">Type of value returned</typeparam>
		/// <typeparam name="T0">Component 0 to pass into mapper</typeparam>
		/// <typeparam name="T1">Component 1 to pass into mapper</typeparam>
		/// <typeparam name="T2">Component 2 to pass into mapper</typeparam>
		/// <typeparam name="T3">Component 3 to pass into mapper</typeparam>
		/// <typeparam name="T4">Component 4 to pass into mapper</typeparam>
		/// <typeparam name="T5">Component 5 to pass into mapper</typeparam>
		/// <typeparam name="T6">Component 6 to pass into mapper</typeparam>
		/// <typeparam name="T7">Component 7 to pass into mapper</typeparam>
		/// <typeparam name="T8">Component 8 to pass into mapper</typeparam>
		/// <typeparam name="T9">Component 9 to pass into mapper</typeparam>
		/// <typeparam name="T10">Component 10 to pass into mapper</typeparam>
		/// <typeparam name="T11">Component 11 to pass into mapper</typeparam>
		/// <typeparam name="T12">Component 12 to pass into mapper</typeparam>
		/// <typeparam name="T13">Component 13 to pass into mapper</typeparam>
		/// <param name="q">query, which produces a value per entity</param>
		/// <param name="r">Reducer, for combining values</param>
		/// <param name="initial">Initial value to start reducing from</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
			ref TMapper q,
			ref TReducer r,
			TOutput initial,
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
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
			where TReducer : IQueryReduce<TOutput>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>();

			var archetypes = query.GetArchetypes();

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;
			var c2 = ComponentID<T2>.ID;
			var c3 = ComponentID<T3>.ID;
			var c4 = ComponentID<T4>.ID;
			var c5 = ComponentID<T5>.ID;
			var c6 = ComponentID<T6>.ID;
			var c7 = ComponentID<T7>.ID;
			var c8 = ComponentID<T8>.ID;
			var c9 = ComponentID<T9>.ID;
			var c10 = ComponentID<T10>.ID;
			var c11 = ComponentID<T11>.ID;
			var c12 = ComponentID<T12>.ID;
			var c13 = ComponentID<T13>.ID;

			var accumulator = initial;

			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];
					var entities = chunk.Entities.Span;

					var t0 = chunk.GetSpan<T0>(c0);
					var t1 = chunk.GetSpan<T1>(c1);
					var t2 = chunk.GetSpan<T2>(c2);
					var t3 = chunk.GetSpan<T3>(c3);
					var t4 = chunk.GetSpan<T4>(c4);
					var t5 = chunk.GetSpan<T5>(c5);
					var t6 = chunk.GetSpan<T6>(c6);
					var t7 = chunk.GetSpan<T7>(c7);
					var t8 = chunk.GetSpan<T8>(c8);
					var t9 = chunk.GetSpan<T9>(c9);
					var t10 = chunk.GetSpan<T10>(c10);
					var t11 = chunk.GetSpan<T11>(c11);
					var t12 = chunk.GetSpan<T12>(c12);
					var t13 = chunk.GetSpan<T13>(c13);

					for (var i = entities.Length - 1; i >= 0; i--)
					{
						accumulator = r.Reduce(accumulator, q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i], ref t13[i]));
					}
				}
			}

			return accumulator;
		}
	}
}
namespace Myriad.ECS.Queries
{
	/// <summary>
	/// Map from a set of components to a single value
	/// </summary>
	public interface IQueryMap<out TReducer, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
		where T0 : IComponent
        where T1 : IComponent
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
		/// Extract a value from the given entity and components
		/// </summary>
		public TReducer Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12, ref T13 t13, ref T14 t14);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
			TOutput initial,
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
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, new()
			where TReducer : IQueryReduce<TOutput>, new()
		{
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
			TOutput initial,
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
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, new()
			where TReducer : IQueryReduce<TOutput>, new()
		{
			var q = new TMapper();
			var r = new TReducer();
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(ref q, ref r, initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="q">query, which produces a value per entity</param>
		/// <param name="r">Reducer, for combining values</param>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
			TMapper q,
			TReducer r,
			TOutput initial,
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
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
			where TReducer : IQueryReduce<TOutput>
		{
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(ref q, ref r, initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <typeparam name="TMapper">Type of mapper</typeparam>
		/// <typeparam name="TReducer">Type of reducer</typeparam>
		/// <typeparam name="TOutput">Type of value returned</typeparam>
		/// <typeparam name="T0">Component 0 to pass into mapper</typeparam>
		/// <typeparam name="T1">Component 1 to pass into mapper</typeparam>
		/// <typeparam name="T2">Component 2 to pass into mapper</typeparam>
		/// <typeparam name="T3">Component 3 to pass into mapper</typeparam>
		/// <typeparam name="T4">Component 4 to pass into mapper</typeparam>
		/// <typeparam name="T5">Component 5 to pass into mapper</typeparam>
		/// <typeparam name="T6">Component 6 to pass into mapper</typeparam>
		/// <typeparam name="T7">Component 7 to pass into mapper</typeparam>
		/// <typeparam name="T8">Component 8 to pass into mapper</typeparam>
		/// <typeparam name="T9">Component 9 to pass into mapper</typeparam>
		/// <typeparam name="T10">Component 10 to pass into mapper</typeparam>
		/// <typeparam name="T11">Component 11 to pass into mapper</typeparam>
		/// <typeparam name="T12">Component 12 to pass into mapper</typeparam>
		/// <typeparam name="T13">Component 13 to pass into mapper</typeparam>
		/// <typeparam name="T14">Component 14 to pass into mapper</typeparam>
		/// <param name="q">query, which produces a value per entity</param>
		/// <param name="r">Reducer, for combining values</param>
		/// <param name="initial">Initial value to start reducing from</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
			ref TMapper q,
			ref TReducer r,
			TOutput initial,
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
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
			where TReducer : IQueryReduce<TOutput>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>();

			var archetypes = query.GetArchetypes();

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;
			var c2 = ComponentID<T2>.ID;
			var c3 = ComponentID<T3>.ID;
			var c4 = ComponentID<T4>.ID;
			var c5 = ComponentID<T5>.ID;
			var c6 = ComponentID<T6>.ID;
			var c7 = ComponentID<T7>.ID;
			var c8 = ComponentID<T8>.ID;
			var c9 = ComponentID<T9>.ID;
			var c10 = ComponentID<T10>.ID;
			var c11 = ComponentID<T11>.ID;
			var c12 = ComponentID<T12>.ID;
			var c13 = ComponentID<T13>.ID;
			var c14 = ComponentID<T14>.ID;

			var accumulator = initial;

			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];
					var entities = chunk.Entities.Span;

					var t0 = chunk.GetSpan<T0>(c0);
					var t1 = chunk.GetSpan<T1>(c1);
					var t2 = chunk.GetSpan<T2>(c2);
					var t3 = chunk.GetSpan<T3>(c3);
					var t4 = chunk.GetSpan<T4>(c4);
					var t5 = chunk.GetSpan<T5>(c5);
					var t6 = chunk.GetSpan<T6>(c6);
					var t7 = chunk.GetSpan<T7>(c7);
					var t8 = chunk.GetSpan<T8>(c8);
					var t9 = chunk.GetSpan<T9>(c9);
					var t10 = chunk.GetSpan<T10>(c10);
					var t11 = chunk.GetSpan<T11>(c11);
					var t12 = chunk.GetSpan<T12>(c12);
					var t13 = chunk.GetSpan<T13>(c13);
					var t14 = chunk.GetSpan<T14>(c14);

					for (var i = entities.Length - 1; i >= 0; i--)
					{
						accumulator = r.Reduce(accumulator, q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i], ref t13[i], ref t14[i]));
					}
				}
			}

			return accumulator;
		}
	}
}
namespace Myriad.ECS.Queries
{
	/// <summary>
	/// Map from a set of components to a single value
	/// </summary>
	public interface IQueryMap<out TReducer, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
		where T0 : IComponent
        where T1 : IComponent
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
		/// Extract a value from the given entity and components
		/// </summary>
		public TReducer Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12, ref T13 t13, ref T14 t14, ref T15 t15);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
			TOutput initial,
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
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, new()
			where TReducer : IQueryReduce<TOutput>, new()
		{
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
			TOutput initial,
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
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, new()
			where TReducer : IQueryReduce<TOutput>, new()
		{
			var q = new TMapper();
			var r = new TReducer();
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(ref q, ref r, initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <param name="q">query, which produces a value per entity</param>
		/// <param name="r">Reducer, for combining values</param>
		/// <param name="initial">Initial value to seed the reducer. If no entities are
		/// matched this is returned.</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
			TMapper q,
			TReducer r,
			TOutput initial,
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
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
			where TReducer : IQueryReduce<TOutput>
		{
			return ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(ref q, ref r, initial, ref query);
		}

		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to
		/// one final output value.
		/// </summary>
		/// <typeparam name="TMapper">Type of mapper</typeparam>
		/// <typeparam name="TReducer">Type of reducer</typeparam>
		/// <typeparam name="TOutput">Type of value returned</typeparam>
		/// <typeparam name="T0">Component 0 to pass into mapper</typeparam>
		/// <typeparam name="T1">Component 1 to pass into mapper</typeparam>
		/// <typeparam name="T2">Component 2 to pass into mapper</typeparam>
		/// <typeparam name="T3">Component 3 to pass into mapper</typeparam>
		/// <typeparam name="T4">Component 4 to pass into mapper</typeparam>
		/// <typeparam name="T5">Component 5 to pass into mapper</typeparam>
		/// <typeparam name="T6">Component 6 to pass into mapper</typeparam>
		/// <typeparam name="T7">Component 7 to pass into mapper</typeparam>
		/// <typeparam name="T8">Component 8 to pass into mapper</typeparam>
		/// <typeparam name="T9">Component 9 to pass into mapper</typeparam>
		/// <typeparam name="T10">Component 10 to pass into mapper</typeparam>
		/// <typeparam name="T11">Component 11 to pass into mapper</typeparam>
		/// <typeparam name="T12">Component 12 to pass into mapper</typeparam>
		/// <typeparam name="T13">Component 13 to pass into mapper</typeparam>
		/// <typeparam name="T14">Component 14 to pass into mapper</typeparam>
		/// <typeparam name="T15">Component 15 to pass into mapper</typeparam>
		/// <param name="q">query, which produces a value per entity</param>
		/// <param name="r">Reducer, for combining values</param>
		/// <param name="initial">Initial value to start reducing from</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		[ExcludeFromCodeCoverage]
		public TOutput ExecuteMapReduce<TMapper, TReducer, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
			ref TMapper q,
			ref TReducer r,
			TOutput initial,
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
			where TMapper : IQueryMap<TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
			where TReducer : IQueryReduce<TOutput>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>();

			var archetypes = query.GetArchetypes();

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;
			var c2 = ComponentID<T2>.ID;
			var c3 = ComponentID<T3>.ID;
			var c4 = ComponentID<T4>.ID;
			var c5 = ComponentID<T5>.ID;
			var c6 = ComponentID<T6>.ID;
			var c7 = ComponentID<T7>.ID;
			var c8 = ComponentID<T8>.ID;
			var c9 = ComponentID<T9>.ID;
			var c10 = ComponentID<T10>.ID;
			var c11 = ComponentID<T11>.ID;
			var c12 = ComponentID<T12>.ID;
			var c13 = ComponentID<T13>.ID;
			var c14 = ComponentID<T14>.ID;
			var c15 = ComponentID<T15>.ID;

			var accumulator = initial;

			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];
					var entities = chunk.Entities.Span;

					var t0 = chunk.GetSpan<T0>(c0);
					var t1 = chunk.GetSpan<T1>(c1);
					var t2 = chunk.GetSpan<T2>(c2);
					var t3 = chunk.GetSpan<T3>(c3);
					var t4 = chunk.GetSpan<T4>(c4);
					var t5 = chunk.GetSpan<T5>(c5);
					var t6 = chunk.GetSpan<T6>(c6);
					var t7 = chunk.GetSpan<T7>(c7);
					var t8 = chunk.GetSpan<T8>(c8);
					var t9 = chunk.GetSpan<T9>(c9);
					var t10 = chunk.GetSpan<T10>(c10);
					var t11 = chunk.GetSpan<T11>(c11);
					var t12 = chunk.GetSpan<T12>(c12);
					var t13 = chunk.GetSpan<T13>(c13);
					var t14 = chunk.GetSpan<T14>(c14);
					var t15 = chunk.GetSpan<T15>(c15);

					for (var i = entities.Length - 1; i >= 0; i--)
					{
						accumulator = r.Reduce(accumulator, q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i], ref t13[i], ref t14[i], ref t15[i]));
					}
				}
			}

			return accumulator;
		}
	}
}


