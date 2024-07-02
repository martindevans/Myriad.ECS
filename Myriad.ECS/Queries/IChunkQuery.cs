using Myriad.ECS.Queries;
using Myriad.ECS.IDs;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedParameter.Global
// ReSharper disable LoopCanBeConvertedToQuery
#pragma warning disable CA1822 // Mark members as static

namespace Myriad.ECS.Queries
{
	public interface IChunkQuery1<T0>
		where T0 : IComponent
	{
		public void Execute(ReadOnlySpan<Entity> e, Span<T0> t0);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0>(
			QueryDescription? query = null
		)
			where T0 : IComponent
			where TQ : IChunkQuery1<T0>, new()
		{
			var q = new TQ();
			return ExecuteChunk<TQ, T0>(ref q, query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0>(
			ref QueryDescription? query
		)
			where T0 : IComponent
			where TQ : IChunkQuery1<T0>, new()
		{
			var q = new TQ();
			return ExecuteChunk<TQ, T0>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
		/// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : IComponent
			where TQ : IChunkQuery1<T0>
		{
			return ExecuteChunk<TQ, T0>(ref q, query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0>(
			TQ q,
			ref QueryDescription? query
		)
			where T0 : IComponent
			where TQ : IChunkQuery1<T0>
		{
			return ExecuteChunk<TQ, T0>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a default
		/// query object will be used (based on type parameters).</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0>(
			ref TQ q,
			QueryDescription? query = null
		)
			where T0 : IComponent
			where TQ : IChunkQuery1<T0>
		{
			return ExecuteChunk<TQ, T0>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0>(
			ref TQ q,
			ref QueryDescription? query
		)
			where T0 : IComponent
			where TQ : IChunkQuery1<T0>
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

				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

					count += entities.Length;

					var t0 = chunk.GetSpan<T0>(c0);

					q.Execute(entities, t0);
				}
			}

			return count;
		}

		public int ExecuteChunkParallel<TQ, T0>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : IComponent
			where TQ : IChunkQuery1<T0>
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

				Parallel.For(0, archetype.Chunks.Count, c =>
				{
					var chunk = archetype.Chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						return;

					Interlocked.Add(ref count, entities.Length);

					var t0 = chunk.GetSpan<T0>(c0);

					q.Execute(entities, t0);
				});
			}

			return count;
		}
	}
}

namespace Myriad.ECS.Queries
{
	public interface IChunkQuery2<T0, T1>
		where T0 : IComponent
        where T1 : IComponent
	{
		public void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1>(
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
			where TQ : IChunkQuery2<T0, T1>, new()
		{
			var q = new TQ();
			return ExecuteChunk<TQ, T0, T1>(ref q, query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1>(
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
			where TQ : IChunkQuery2<T0, T1>, new()
		{
			var q = new TQ();
			return ExecuteChunk<TQ, T0, T1>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
		/// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
			where TQ : IChunkQuery2<T0, T1>
		{
			return ExecuteChunk<TQ, T0, T1>(ref q, query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1>(
			TQ q,
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
			where TQ : IChunkQuery2<T0, T1>
		{
			return ExecuteChunk<TQ, T0, T1>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a default
		/// query object will be used (based on type parameters).</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1>(
			ref TQ q,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
			where TQ : IChunkQuery2<T0, T1>
		{
			return ExecuteChunk<TQ, T0, T1>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1>(
			ref TQ q,
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
			where TQ : IChunkQuery2<T0, T1>
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

				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

					count += entities.Length;

					var t0 = chunk.GetSpan<T0>(c0);
					var t1 = chunk.GetSpan<T1>(c1);

					q.Execute(entities, t0, t1);
				}
			}

			return count;
		}

		public int ExecuteChunkParallel<TQ, T0, T1>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
			where TQ : IChunkQuery2<T0, T1>
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

				Parallel.For(0, archetype.Chunks.Count, c =>
				{
					var chunk = archetype.Chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						return;

					Interlocked.Add(ref count, entities.Length);

					var t0 = chunk.GetSpan<T0>(c0);
					var t1 = chunk.GetSpan<T1>(c1);

					q.Execute(entities, t0, t1);
				});
			}

			return count;
		}
	}
}

namespace Myriad.ECS.Queries
{
	public interface IChunkQuery3<T0, T1, T2>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
	{
		public void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2>(
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
			where TQ : IChunkQuery3<T0, T1, T2>, new()
		{
			var q = new TQ();
			return ExecuteChunk<TQ, T0, T1, T2>(ref q, query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2>(
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
			where TQ : IChunkQuery3<T0, T1, T2>, new()
		{
			var q = new TQ();
			return ExecuteChunk<TQ, T0, T1, T2>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
		/// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
			where TQ : IChunkQuery3<T0, T1, T2>
		{
			return ExecuteChunk<TQ, T0, T1, T2>(ref q, query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2>(
			TQ q,
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
			where TQ : IChunkQuery3<T0, T1, T2>
		{
			return ExecuteChunk<TQ, T0, T1, T2>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a default
		/// query object will be used (based on type parameters).</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2>(
			ref TQ q,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
			where TQ : IChunkQuery3<T0, T1, T2>
		{
			return ExecuteChunk<TQ, T0, T1, T2>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2>(
			ref TQ q,
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
			where TQ : IChunkQuery3<T0, T1, T2>
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

				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

					count += entities.Length;

					var t0 = chunk.GetSpan<T0>(c0);
					var t1 = chunk.GetSpan<T1>(c1);
					var t2 = chunk.GetSpan<T2>(c2);

					q.Execute(entities, t0, t1, t2);
				}
			}

			return count;
		}

		public int ExecuteChunkParallel<TQ, T0, T1, T2>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
			where TQ : IChunkQuery3<T0, T1, T2>
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

				Parallel.For(0, archetype.Chunks.Count, c =>
				{
					var chunk = archetype.Chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						return;

					Interlocked.Add(ref count, entities.Length);

					var t0 = chunk.GetSpan<T0>(c0);
					var t1 = chunk.GetSpan<T1>(c1);
					var t2 = chunk.GetSpan<T2>(c2);

					q.Execute(entities, t0, t1, t2);
				});
			}

			return count;
		}
	}
}

namespace Myriad.ECS.Queries
{
	public interface IChunkQuery4<T0, T1, T2, T3>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
	{
		public void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3>(
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
			where TQ : IChunkQuery4<T0, T1, T2, T3>, new()
		{
			var q = new TQ();
			return ExecuteChunk<TQ, T0, T1, T2, T3>(ref q, query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3>(
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
			where TQ : IChunkQuery4<T0, T1, T2, T3>, new()
		{
			var q = new TQ();
			return ExecuteChunk<TQ, T0, T1, T2, T3>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
		/// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
			where TQ : IChunkQuery4<T0, T1, T2, T3>
		{
			return ExecuteChunk<TQ, T0, T1, T2, T3>(ref q, query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3>(
			TQ q,
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
			where TQ : IChunkQuery4<T0, T1, T2, T3>
		{
			return ExecuteChunk<TQ, T0, T1, T2, T3>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a default
		/// query object will be used (based on type parameters).</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3>(
			ref TQ q,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
			where TQ : IChunkQuery4<T0, T1, T2, T3>
		{
			return ExecuteChunk<TQ, T0, T1, T2, T3>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3>(
			ref TQ q,
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
			where TQ : IChunkQuery4<T0, T1, T2, T3>
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

				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

					count += entities.Length;

					var t0 = chunk.GetSpan<T0>(c0);
					var t1 = chunk.GetSpan<T1>(c1);
					var t2 = chunk.GetSpan<T2>(c2);
					var t3 = chunk.GetSpan<T3>(c3);

					q.Execute(entities, t0, t1, t2, t3);
				}
			}

			return count;
		}

		public int ExecuteChunkParallel<TQ, T0, T1, T2, T3>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
			where TQ : IChunkQuery4<T0, T1, T2, T3>
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

				Parallel.For(0, archetype.Chunks.Count, c =>
				{
					var chunk = archetype.Chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						return;

					Interlocked.Add(ref count, entities.Length);

					var t0 = chunk.GetSpan<T0>(c0);
					var t1 = chunk.GetSpan<T1>(c1);
					var t2 = chunk.GetSpan<T2>(c2);
					var t3 = chunk.GetSpan<T3>(c3);

					q.Execute(entities, t0, t1, t2, t3);
				});
			}

			return count;
		}
	}
}

namespace Myriad.ECS.Queries
{
	public interface IChunkQuery5<T0, T1, T2, T3, T4>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
	{
		public void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4>(
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
			where TQ : IChunkQuery5<T0, T1, T2, T3, T4>, new()
		{
			var q = new TQ();
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4>(ref q, query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4>(
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
			where TQ : IChunkQuery5<T0, T1, T2, T3, T4>, new()
		{
			var q = new TQ();
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
		/// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
			where TQ : IChunkQuery5<T0, T1, T2, T3, T4>
		{
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4>(ref q, query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4>(
			TQ q,
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
			where TQ : IChunkQuery5<T0, T1, T2, T3, T4>
		{
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a default
		/// query object will be used (based on type parameters).</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4>(
			ref TQ q,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
			where TQ : IChunkQuery5<T0, T1, T2, T3, T4>
		{
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4>(
			ref TQ q,
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
			where TQ : IChunkQuery5<T0, T1, T2, T3, T4>
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

				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

					count += entities.Length;

					var t0 = chunk.GetSpan<T0>(c0);
					var t1 = chunk.GetSpan<T1>(c1);
					var t2 = chunk.GetSpan<T2>(c2);
					var t3 = chunk.GetSpan<T3>(c3);
					var t4 = chunk.GetSpan<T4>(c4);

					q.Execute(entities, t0, t1, t2, t3, t4);
				}
			}

			return count;
		}

		public int ExecuteChunkParallel<TQ, T0, T1, T2, T3, T4>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
			where TQ : IChunkQuery5<T0, T1, T2, T3, T4>
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

				Parallel.For(0, archetype.Chunks.Count, c =>
				{
					var chunk = archetype.Chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						return;

					Interlocked.Add(ref count, entities.Length);

					var t0 = chunk.GetSpan<T0>(c0);
					var t1 = chunk.GetSpan<T1>(c1);
					var t2 = chunk.GetSpan<T2>(c2);
					var t3 = chunk.GetSpan<T3>(c3);
					var t4 = chunk.GetSpan<T4>(c4);

					q.Execute(entities, t0, t1, t2, t3, t4);
				});
			}

			return count;
		}
	}
}

namespace Myriad.ECS.Queries
{
	public interface IChunkQuery6<T0, T1, T2, T3, T4, T5>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
	{
		public void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5>(
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
			where TQ : IChunkQuery6<T0, T1, T2, T3, T4, T5>, new()
		{
			var q = new TQ();
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5>(ref q, query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5>(
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
			where TQ : IChunkQuery6<T0, T1, T2, T3, T4, T5>, new()
		{
			var q = new TQ();
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
		/// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
			where TQ : IChunkQuery6<T0, T1, T2, T3, T4, T5>
		{
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5>(ref q, query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5>(
			TQ q,
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
			where TQ : IChunkQuery6<T0, T1, T2, T3, T4, T5>
		{
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a default
		/// query object will be used (based on type parameters).</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5>(
			ref TQ q,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
			where TQ : IChunkQuery6<T0, T1, T2, T3, T4, T5>
		{
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5>(
			ref TQ q,
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
			where TQ : IChunkQuery6<T0, T1, T2, T3, T4, T5>
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

				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

					count += entities.Length;

					var t0 = chunk.GetSpan<T0>(c0);
					var t1 = chunk.GetSpan<T1>(c1);
					var t2 = chunk.GetSpan<T2>(c2);
					var t3 = chunk.GetSpan<T3>(c3);
					var t4 = chunk.GetSpan<T4>(c4);
					var t5 = chunk.GetSpan<T5>(c5);

					q.Execute(entities, t0, t1, t2, t3, t4, t5);
				}
			}

			return count;
		}

		public int ExecuteChunkParallel<TQ, T0, T1, T2, T3, T4, T5>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
			where TQ : IChunkQuery6<T0, T1, T2, T3, T4, T5>
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

				Parallel.For(0, archetype.Chunks.Count, c =>
				{
					var chunk = archetype.Chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						return;

					Interlocked.Add(ref count, entities.Length);

					var t0 = chunk.GetSpan<T0>(c0);
					var t1 = chunk.GetSpan<T1>(c1);
					var t2 = chunk.GetSpan<T2>(c2);
					var t3 = chunk.GetSpan<T3>(c3);
					var t4 = chunk.GetSpan<T4>(c4);
					var t5 = chunk.GetSpan<T5>(c5);

					q.Execute(entities, t0, t1, t2, t3, t4, t5);
				});
			}

			return count;
		}
	}
}

namespace Myriad.ECS.Queries
{
	public interface IChunkQuery7<T0, T1, T2, T3, T4, T5, T6>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
	{
		public void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6>(
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
			where TQ : IChunkQuery7<T0, T1, T2, T3, T4, T5, T6>, new()
		{
			var q = new TQ();
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6>(ref q, query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6>(
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
			where TQ : IChunkQuery7<T0, T1, T2, T3, T4, T5, T6>, new()
		{
			var q = new TQ();
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
		/// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6>(
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
			where TQ : IChunkQuery7<T0, T1, T2, T3, T4, T5, T6>
		{
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6>(ref q, query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6>(
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
			where TQ : IChunkQuery7<T0, T1, T2, T3, T4, T5, T6>
		{
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a default
		/// query object will be used (based on type parameters).</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6>(
			ref TQ q,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
			where TQ : IChunkQuery7<T0, T1, T2, T3, T4, T5, T6>
		{
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6>(
			ref TQ q,
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
			where TQ : IChunkQuery7<T0, T1, T2, T3, T4, T5, T6>
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

				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

					count += entities.Length;

					var t0 = chunk.GetSpan<T0>(c0);
					var t1 = chunk.GetSpan<T1>(c1);
					var t2 = chunk.GetSpan<T2>(c2);
					var t3 = chunk.GetSpan<T3>(c3);
					var t4 = chunk.GetSpan<T4>(c4);
					var t5 = chunk.GetSpan<T5>(c5);
					var t6 = chunk.GetSpan<T6>(c6);

					q.Execute(entities, t0, t1, t2, t3, t4, t5, t6);
				}
			}

			return count;
		}

		public int ExecuteChunkParallel<TQ, T0, T1, T2, T3, T4, T5, T6>(
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
			where TQ : IChunkQuery7<T0, T1, T2, T3, T4, T5, T6>
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

				Parallel.For(0, archetype.Chunks.Count, c =>
				{
					var chunk = archetype.Chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						return;

					Interlocked.Add(ref count, entities.Length);

					var t0 = chunk.GetSpan<T0>(c0);
					var t1 = chunk.GetSpan<T1>(c1);
					var t2 = chunk.GetSpan<T2>(c2);
					var t3 = chunk.GetSpan<T3>(c3);
					var t4 = chunk.GetSpan<T4>(c4);
					var t5 = chunk.GetSpan<T5>(c5);
					var t6 = chunk.GetSpan<T6>(c6);

					q.Execute(entities, t0, t1, t2, t3, t4, t5, t6);
				});
			}

			return count;
		}
	}
}

namespace Myriad.ECS.Queries
{
	public interface IChunkQuery8<T0, T1, T2, T3, T4, T5, T6, T7>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
	{
		public void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7>(
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
			where TQ : IChunkQuery8<T0, T1, T2, T3, T4, T5, T6, T7>, new()
		{
			var q = new TQ();
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7>(ref q, query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7>(
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
			where TQ : IChunkQuery8<T0, T1, T2, T3, T4, T5, T6, T7>, new()
		{
			var q = new TQ();
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
		/// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7>(
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
			where TQ : IChunkQuery8<T0, T1, T2, T3, T4, T5, T6, T7>
		{
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7>(ref q, query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7>(
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
			where TQ : IChunkQuery8<T0, T1, T2, T3, T4, T5, T6, T7>
		{
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a default
		/// query object will be used (based on type parameters).</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7>(
			ref TQ q,
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
			where TQ : IChunkQuery8<T0, T1, T2, T3, T4, T5, T6, T7>
		{
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7>(
			ref TQ q,
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
			where TQ : IChunkQuery8<T0, T1, T2, T3, T4, T5, T6, T7>
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

				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

					count += entities.Length;

					var t0 = chunk.GetSpan<T0>(c0);
					var t1 = chunk.GetSpan<T1>(c1);
					var t2 = chunk.GetSpan<T2>(c2);
					var t3 = chunk.GetSpan<T3>(c3);
					var t4 = chunk.GetSpan<T4>(c4);
					var t5 = chunk.GetSpan<T5>(c5);
					var t6 = chunk.GetSpan<T6>(c6);
					var t7 = chunk.GetSpan<T7>(c7);

					q.Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7);
				}
			}

			return count;
		}

		public int ExecuteChunkParallel<TQ, T0, T1, T2, T3, T4, T5, T6, T7>(
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
			where TQ : IChunkQuery8<T0, T1, T2, T3, T4, T5, T6, T7>
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

				Parallel.For(0, archetype.Chunks.Count, c =>
				{
					var chunk = archetype.Chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						return;

					Interlocked.Add(ref count, entities.Length);

					var t0 = chunk.GetSpan<T0>(c0);
					var t1 = chunk.GetSpan<T1>(c1);
					var t2 = chunk.GetSpan<T2>(c2);
					var t3 = chunk.GetSpan<T3>(c3);
					var t4 = chunk.GetSpan<T4>(c4);
					var t5 = chunk.GetSpan<T5>(c5);
					var t6 = chunk.GetSpan<T6>(c6);
					var t7 = chunk.GetSpan<T7>(c7);

					q.Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7);
				});
			}

			return count;
		}
	}
}

namespace Myriad.ECS.Queries
{
	public interface IChunkQuery9<T0, T1, T2, T3, T4, T5, T6, T7, T8>
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
		public void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
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
			where TQ : IChunkQuery9<T0, T1, T2, T3, T4, T5, T6, T7, T8>, new()
		{
			var q = new TQ();
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8>(ref q, query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
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
			where TQ : IChunkQuery9<T0, T1, T2, T3, T4, T5, T6, T7, T8>, new()
		{
			var q = new TQ();
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
		/// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
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
			where TQ : IChunkQuery9<T0, T1, T2, T3, T4, T5, T6, T7, T8>
		{
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8>(ref q, query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
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
			where TQ : IChunkQuery9<T0, T1, T2, T3, T4, T5, T6, T7, T8>
		{
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a default
		/// query object will be used (based on type parameters).</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
			ref TQ q,
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
			where TQ : IChunkQuery9<T0, T1, T2, T3, T4, T5, T6, T7, T8>
		{
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
			ref TQ q,
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
			where TQ : IChunkQuery9<T0, T1, T2, T3, T4, T5, T6, T7, T8>
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

				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

					count += entities.Length;

					var t0 = chunk.GetSpan<T0>(c0);
					var t1 = chunk.GetSpan<T1>(c1);
					var t2 = chunk.GetSpan<T2>(c2);
					var t3 = chunk.GetSpan<T3>(c3);
					var t4 = chunk.GetSpan<T4>(c4);
					var t5 = chunk.GetSpan<T5>(c5);
					var t6 = chunk.GetSpan<T6>(c6);
					var t7 = chunk.GetSpan<T7>(c7);
					var t8 = chunk.GetSpan<T8>(c8);

					q.Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8);
				}
			}

			return count;
		}

		public int ExecuteChunkParallel<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
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
			where TQ : IChunkQuery9<T0, T1, T2, T3, T4, T5, T6, T7, T8>
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

				Parallel.For(0, archetype.Chunks.Count, c =>
				{
					var chunk = archetype.Chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						return;

					Interlocked.Add(ref count, entities.Length);

					var t0 = chunk.GetSpan<T0>(c0);
					var t1 = chunk.GetSpan<T1>(c1);
					var t2 = chunk.GetSpan<T2>(c2);
					var t3 = chunk.GetSpan<T3>(c3);
					var t4 = chunk.GetSpan<T4>(c4);
					var t5 = chunk.GetSpan<T5>(c5);
					var t6 = chunk.GetSpan<T6>(c6);
					var t7 = chunk.GetSpan<T7>(c7);
					var t8 = chunk.GetSpan<T8>(c8);

					q.Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8);
				});
			}

			return count;
		}
	}
}

namespace Myriad.ECS.Queries
{
	public interface IChunkQuery10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
		where T0 : IComponent
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
		public void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <typeparam name="T9">Type of component 9 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
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
			where TQ : IChunkQuery10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>, new()
		{
			var q = new TQ();
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(ref q, query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <typeparam name="T9">Type of component 9 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
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
			where TQ : IChunkQuery10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>, new()
		{
			var q = new TQ();
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <typeparam name="T9">Type of component 9 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
		/// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
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
			where TQ : IChunkQuery10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
		{
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(ref q, query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <typeparam name="T9">Type of component 9 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
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
			where TQ : IChunkQuery10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
		{
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <typeparam name="T9">Type of component 9 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a default
		/// query object will be used (based on type parameters).</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
			ref TQ q,
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
			where TQ : IChunkQuery10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
		{
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <typeparam name="T9">Type of component 9 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
			ref TQ q,
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
			where TQ : IChunkQuery10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>();

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
		    var c9 = ComponentID<T9>.ID;

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

					count += entities.Length;

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

					q.Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9);
				}
			}

			return count;
		}

		public int ExecuteChunkParallel<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
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
			where TQ : IChunkQuery10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>();

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
		    var c9 = ComponentID<T9>.ID;

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
				var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				Parallel.For(0, archetype.Chunks.Count, c =>
				{
					var chunk = archetype.Chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						return;

					Interlocked.Add(ref count, entities.Length);

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

					q.Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9);
				});
			}

			return count;
		}
	}
}

namespace Myriad.ECS.Queries
{
	public interface IChunkQuery11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
		where T0 : IComponent
        where T1 : IComponent
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
		public void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <typeparam name="T9">Type of component 9 to retrieve</typeparam>
        /// <typeparam name="T10">Type of component 10 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
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
			where TQ : IChunkQuery11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, new()
		{
			var q = new TQ();
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(ref q, query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <typeparam name="T9">Type of component 9 to retrieve</typeparam>
        /// <typeparam name="T10">Type of component 10 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
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
			where TQ : IChunkQuery11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, new()
		{
			var q = new TQ();
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <typeparam name="T9">Type of component 9 to retrieve</typeparam>
        /// <typeparam name="T10">Type of component 10 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
		/// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
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
			where TQ : IChunkQuery11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
		{
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(ref q, query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <typeparam name="T9">Type of component 9 to retrieve</typeparam>
        /// <typeparam name="T10">Type of component 10 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
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
			where TQ : IChunkQuery11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
		{
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <typeparam name="T9">Type of component 9 to retrieve</typeparam>
        /// <typeparam name="T10">Type of component 10 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a default
		/// query object will be used (based on type parameters).</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
			ref TQ q,
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
			where TQ : IChunkQuery11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
		{
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <typeparam name="T9">Type of component 9 to retrieve</typeparam>
        /// <typeparam name="T10">Type of component 10 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
			ref TQ q,
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
			where TQ : IChunkQuery11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>();

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
		    var c9 = ComponentID<T9>.ID;
		    var c10 = ComponentID<T10>.ID;

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

					count += entities.Length;

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

					q.Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
				}
			}

			return count;
		}

		public int ExecuteChunkParallel<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
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
			where TQ : IChunkQuery11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>();

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
		    var c9 = ComponentID<T9>.ID;
		    var c10 = ComponentID<T10>.ID;

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
				var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				Parallel.For(0, archetype.Chunks.Count, c =>
				{
					var chunk = archetype.Chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						return;

					Interlocked.Add(ref count, entities.Length);

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

					q.Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
				});
			}

			return count;
		}
	}
}

namespace Myriad.ECS.Queries
{
	public interface IChunkQuery12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
		where T0 : IComponent
        where T1 : IComponent
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
		public void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10, Span<T11> t11);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <typeparam name="T9">Type of component 9 to retrieve</typeparam>
        /// <typeparam name="T10">Type of component 10 to retrieve</typeparam>
        /// <typeparam name="T11">Type of component 11 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
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
			where TQ : IChunkQuery12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, new()
		{
			var q = new TQ();
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(ref q, query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <typeparam name="T9">Type of component 9 to retrieve</typeparam>
        /// <typeparam name="T10">Type of component 10 to retrieve</typeparam>
        /// <typeparam name="T11">Type of component 11 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
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
			where TQ : IChunkQuery12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, new()
		{
			var q = new TQ();
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <typeparam name="T9">Type of component 9 to retrieve</typeparam>
        /// <typeparam name="T10">Type of component 10 to retrieve</typeparam>
        /// <typeparam name="T11">Type of component 11 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
		/// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
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
			where TQ : IChunkQuery12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
		{
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(ref q, query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <typeparam name="T9">Type of component 9 to retrieve</typeparam>
        /// <typeparam name="T10">Type of component 10 to retrieve</typeparam>
        /// <typeparam name="T11">Type of component 11 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
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
			where TQ : IChunkQuery12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
		{
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <typeparam name="T9">Type of component 9 to retrieve</typeparam>
        /// <typeparam name="T10">Type of component 10 to retrieve</typeparam>
        /// <typeparam name="T11">Type of component 11 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a default
		/// query object will be used (based on type parameters).</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
			ref TQ q,
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
			where TQ : IChunkQuery12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
		{
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <typeparam name="T9">Type of component 9 to retrieve</typeparam>
        /// <typeparam name="T10">Type of component 10 to retrieve</typeparam>
        /// <typeparam name="T11">Type of component 11 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
			ref TQ q,
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
			where TQ : IChunkQuery12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>();

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
		    var c9 = ComponentID<T9>.ID;
		    var c10 = ComponentID<T10>.ID;
		    var c11 = ComponentID<T11>.ID;

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

					count += entities.Length;

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

					q.Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
				}
			}

			return count;
		}

		public int ExecuteChunkParallel<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
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
			where TQ : IChunkQuery12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>();

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
		    var c9 = ComponentID<T9>.ID;
		    var c10 = ComponentID<T10>.ID;
		    var c11 = ComponentID<T11>.ID;

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
				var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				Parallel.For(0, archetype.Chunks.Count, c =>
				{
					var chunk = archetype.Chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						return;

					Interlocked.Add(ref count, entities.Length);

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

					q.Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
				});
			}

			return count;
		}
	}
}

namespace Myriad.ECS.Queries
{
	public interface IChunkQuery13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
		where T0 : IComponent
        where T1 : IComponent
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
		public void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10, Span<T11> t11, Span<T12> t12);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <typeparam name="T9">Type of component 9 to retrieve</typeparam>
        /// <typeparam name="T10">Type of component 10 to retrieve</typeparam>
        /// <typeparam name="T11">Type of component 11 to retrieve</typeparam>
        /// <typeparam name="T12">Type of component 12 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
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
			where TQ : IChunkQuery13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, new()
		{
			var q = new TQ();
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(ref q, query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <typeparam name="T9">Type of component 9 to retrieve</typeparam>
        /// <typeparam name="T10">Type of component 10 to retrieve</typeparam>
        /// <typeparam name="T11">Type of component 11 to retrieve</typeparam>
        /// <typeparam name="T12">Type of component 12 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
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
			where TQ : IChunkQuery13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, new()
		{
			var q = new TQ();
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <typeparam name="T9">Type of component 9 to retrieve</typeparam>
        /// <typeparam name="T10">Type of component 10 to retrieve</typeparam>
        /// <typeparam name="T11">Type of component 11 to retrieve</typeparam>
        /// <typeparam name="T12">Type of component 12 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
		/// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
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
			where TQ : IChunkQuery13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
		{
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(ref q, query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <typeparam name="T9">Type of component 9 to retrieve</typeparam>
        /// <typeparam name="T10">Type of component 10 to retrieve</typeparam>
        /// <typeparam name="T11">Type of component 11 to retrieve</typeparam>
        /// <typeparam name="T12">Type of component 12 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
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
			where TQ : IChunkQuery13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
		{
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <typeparam name="T9">Type of component 9 to retrieve</typeparam>
        /// <typeparam name="T10">Type of component 10 to retrieve</typeparam>
        /// <typeparam name="T11">Type of component 11 to retrieve</typeparam>
        /// <typeparam name="T12">Type of component 12 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a default
		/// query object will be used (based on type parameters).</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
			ref TQ q,
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
			where TQ : IChunkQuery13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
		{
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <typeparam name="T9">Type of component 9 to retrieve</typeparam>
        /// <typeparam name="T10">Type of component 10 to retrieve</typeparam>
        /// <typeparam name="T11">Type of component 11 to retrieve</typeparam>
        /// <typeparam name="T12">Type of component 12 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
			ref TQ q,
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
			where TQ : IChunkQuery13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>();

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
		    var c9 = ComponentID<T9>.ID;
		    var c10 = ComponentID<T10>.ID;
		    var c11 = ComponentID<T11>.ID;
		    var c12 = ComponentID<T12>.ID;

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

					count += entities.Length;

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

					q.Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
				}
			}

			return count;
		}

		public int ExecuteChunkParallel<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
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
			where TQ : IChunkQuery13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>();

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
		    var c9 = ComponentID<T9>.ID;
		    var c10 = ComponentID<T10>.ID;
		    var c11 = ComponentID<T11>.ID;
		    var c12 = ComponentID<T12>.ID;

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
				var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				Parallel.For(0, archetype.Chunks.Count, c =>
				{
					var chunk = archetype.Chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						return;

					Interlocked.Add(ref count, entities.Length);

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

					q.Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
				});
			}

			return count;
		}
	}
}

namespace Myriad.ECS.Queries
{
	public interface IChunkQuery14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
		where T0 : IComponent
        where T1 : IComponent
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
		public void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10, Span<T11> t11, Span<T12> t12, Span<T13> t13);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <typeparam name="T9">Type of component 9 to retrieve</typeparam>
        /// <typeparam name="T10">Type of component 10 to retrieve</typeparam>
        /// <typeparam name="T11">Type of component 11 to retrieve</typeparam>
        /// <typeparam name="T12">Type of component 12 to retrieve</typeparam>
        /// <typeparam name="T13">Type of component 13 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
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
			where TQ : IChunkQuery14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, new()
		{
			var q = new TQ();
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(ref q, query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <typeparam name="T9">Type of component 9 to retrieve</typeparam>
        /// <typeparam name="T10">Type of component 10 to retrieve</typeparam>
        /// <typeparam name="T11">Type of component 11 to retrieve</typeparam>
        /// <typeparam name="T12">Type of component 12 to retrieve</typeparam>
        /// <typeparam name="T13">Type of component 13 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
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
			where TQ : IChunkQuery14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, new()
		{
			var q = new TQ();
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <typeparam name="T9">Type of component 9 to retrieve</typeparam>
        /// <typeparam name="T10">Type of component 10 to retrieve</typeparam>
        /// <typeparam name="T11">Type of component 11 to retrieve</typeparam>
        /// <typeparam name="T12">Type of component 12 to retrieve</typeparam>
        /// <typeparam name="T13">Type of component 13 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
		/// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
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
			where TQ : IChunkQuery14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
		{
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(ref q, query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <typeparam name="T9">Type of component 9 to retrieve</typeparam>
        /// <typeparam name="T10">Type of component 10 to retrieve</typeparam>
        /// <typeparam name="T11">Type of component 11 to retrieve</typeparam>
        /// <typeparam name="T12">Type of component 12 to retrieve</typeparam>
        /// <typeparam name="T13">Type of component 13 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
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
			where TQ : IChunkQuery14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
		{
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <typeparam name="T9">Type of component 9 to retrieve</typeparam>
        /// <typeparam name="T10">Type of component 10 to retrieve</typeparam>
        /// <typeparam name="T11">Type of component 11 to retrieve</typeparam>
        /// <typeparam name="T12">Type of component 12 to retrieve</typeparam>
        /// <typeparam name="T13">Type of component 13 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a default
		/// query object will be used (based on type parameters).</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
			ref TQ q,
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
			where TQ : IChunkQuery14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
		{
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <typeparam name="T9">Type of component 9 to retrieve</typeparam>
        /// <typeparam name="T10">Type of component 10 to retrieve</typeparam>
        /// <typeparam name="T11">Type of component 11 to retrieve</typeparam>
        /// <typeparam name="T12">Type of component 12 to retrieve</typeparam>
        /// <typeparam name="T13">Type of component 13 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
			ref TQ q,
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
			where TQ : IChunkQuery14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>();

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
		    var c9 = ComponentID<T9>.ID;
		    var c10 = ComponentID<T10>.ID;
		    var c11 = ComponentID<T11>.ID;
		    var c12 = ComponentID<T12>.ID;
		    var c13 = ComponentID<T13>.ID;

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

					count += entities.Length;

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

					q.Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
				}
			}

			return count;
		}

		public int ExecuteChunkParallel<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
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
			where TQ : IChunkQuery14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>();

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
		    var c9 = ComponentID<T9>.ID;
		    var c10 = ComponentID<T10>.ID;
		    var c11 = ComponentID<T11>.ID;
		    var c12 = ComponentID<T12>.ID;
		    var c13 = ComponentID<T13>.ID;

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
				var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				Parallel.For(0, archetype.Chunks.Count, c =>
				{
					var chunk = archetype.Chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						return;

					Interlocked.Add(ref count, entities.Length);

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

					q.Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
				});
			}

			return count;
		}
	}
}

namespace Myriad.ECS.Queries
{
	public interface IChunkQuery15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
		where T0 : IComponent
        where T1 : IComponent
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
		public void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10, Span<T11> t11, Span<T12> t12, Span<T13> t13, Span<T14> t14);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <typeparam name="T9">Type of component 9 to retrieve</typeparam>
        /// <typeparam name="T10">Type of component 10 to retrieve</typeparam>
        /// <typeparam name="T11">Type of component 11 to retrieve</typeparam>
        /// <typeparam name="T12">Type of component 12 to retrieve</typeparam>
        /// <typeparam name="T13">Type of component 13 to retrieve</typeparam>
        /// <typeparam name="T14">Type of component 14 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
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
			where TQ : IChunkQuery15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, new()
		{
			var q = new TQ();
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(ref q, query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <typeparam name="T9">Type of component 9 to retrieve</typeparam>
        /// <typeparam name="T10">Type of component 10 to retrieve</typeparam>
        /// <typeparam name="T11">Type of component 11 to retrieve</typeparam>
        /// <typeparam name="T12">Type of component 12 to retrieve</typeparam>
        /// <typeparam name="T13">Type of component 13 to retrieve</typeparam>
        /// <typeparam name="T14">Type of component 14 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
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
			where TQ : IChunkQuery15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, new()
		{
			var q = new TQ();
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <typeparam name="T9">Type of component 9 to retrieve</typeparam>
        /// <typeparam name="T10">Type of component 10 to retrieve</typeparam>
        /// <typeparam name="T11">Type of component 11 to retrieve</typeparam>
        /// <typeparam name="T12">Type of component 12 to retrieve</typeparam>
        /// <typeparam name="T13">Type of component 13 to retrieve</typeparam>
        /// <typeparam name="T14">Type of component 14 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
		/// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
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
			where TQ : IChunkQuery15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
		{
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(ref q, query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <typeparam name="T9">Type of component 9 to retrieve</typeparam>
        /// <typeparam name="T10">Type of component 10 to retrieve</typeparam>
        /// <typeparam name="T11">Type of component 11 to retrieve</typeparam>
        /// <typeparam name="T12">Type of component 12 to retrieve</typeparam>
        /// <typeparam name="T13">Type of component 13 to retrieve</typeparam>
        /// <typeparam name="T14">Type of component 14 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
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
			where TQ : IChunkQuery15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
		{
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <typeparam name="T9">Type of component 9 to retrieve</typeparam>
        /// <typeparam name="T10">Type of component 10 to retrieve</typeparam>
        /// <typeparam name="T11">Type of component 11 to retrieve</typeparam>
        /// <typeparam name="T12">Type of component 12 to retrieve</typeparam>
        /// <typeparam name="T13">Type of component 13 to retrieve</typeparam>
        /// <typeparam name="T14">Type of component 14 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a default
		/// query object will be used (based on type parameters).</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
			ref TQ q,
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
			where TQ : IChunkQuery15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
		{
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <typeparam name="T9">Type of component 9 to retrieve</typeparam>
        /// <typeparam name="T10">Type of component 10 to retrieve</typeparam>
        /// <typeparam name="T11">Type of component 11 to retrieve</typeparam>
        /// <typeparam name="T12">Type of component 12 to retrieve</typeparam>
        /// <typeparam name="T13">Type of component 13 to retrieve</typeparam>
        /// <typeparam name="T14">Type of component 14 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
			ref TQ q,
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
			where TQ : IChunkQuery15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>();

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
		    var c9 = ComponentID<T9>.ID;
		    var c10 = ComponentID<T10>.ID;
		    var c11 = ComponentID<T11>.ID;
		    var c12 = ComponentID<T12>.ID;
		    var c13 = ComponentID<T13>.ID;
		    var c14 = ComponentID<T14>.ID;

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

					count += entities.Length;

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

					q.Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
				}
			}

			return count;
		}

		public int ExecuteChunkParallel<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
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
			where TQ : IChunkQuery15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>();

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
		    var c9 = ComponentID<T9>.ID;
		    var c10 = ComponentID<T10>.ID;
		    var c11 = ComponentID<T11>.ID;
		    var c12 = ComponentID<T12>.ID;
		    var c13 = ComponentID<T13>.ID;
		    var c14 = ComponentID<T14>.ID;

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
				var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				Parallel.For(0, archetype.Chunks.Count, c =>
				{
					var chunk = archetype.Chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						return;

					Interlocked.Add(ref count, entities.Length);

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

					q.Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
				});
			}

			return count;
		}
	}
}

namespace Myriad.ECS.Queries
{
	public interface IChunkQuery16<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
		where T0 : IComponent
        where T1 : IComponent
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
		public void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, Span<T9> t9, Span<T10> t10, Span<T11> t11, Span<T12> t12, Span<T13> t13, Span<T14> t14, Span<T15> t15);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <typeparam name="T9">Type of component 9 to retrieve</typeparam>
        /// <typeparam name="T10">Type of component 10 to retrieve</typeparam>
        /// <typeparam name="T11">Type of component 11 to retrieve</typeparam>
        /// <typeparam name="T12">Type of component 12 to retrieve</typeparam>
        /// <typeparam name="T13">Type of component 13 to retrieve</typeparam>
        /// <typeparam name="T14">Type of component 14 to retrieve</typeparam>
        /// <typeparam name="T15">Type of component 15 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
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
			where TQ : IChunkQuery16<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, new()
		{
			var q = new TQ();
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(ref q, query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <typeparam name="T9">Type of component 9 to retrieve</typeparam>
        /// <typeparam name="T10">Type of component 10 to retrieve</typeparam>
        /// <typeparam name="T11">Type of component 11 to retrieve</typeparam>
        /// <typeparam name="T12">Type of component 12 to retrieve</typeparam>
        /// <typeparam name="T13">Type of component 13 to retrieve</typeparam>
        /// <typeparam name="T14">Type of component 14 to retrieve</typeparam>
        /// <typeparam name="T15">Type of component 15 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
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
			where TQ : IChunkQuery16<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, new()
		{
			var q = new TQ();
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <typeparam name="T9">Type of component 9 to retrieve</typeparam>
        /// <typeparam name="T10">Type of component 10 to retrieve</typeparam>
        /// <typeparam name="T11">Type of component 11 to retrieve</typeparam>
        /// <typeparam name="T12">Type of component 12 to retrieve</typeparam>
        /// <typeparam name="T13">Type of component 13 to retrieve</typeparam>
        /// <typeparam name="T14">Type of component 14 to retrieve</typeparam>
        /// <typeparam name="T15">Type of component 15 to retrieve</typeparam>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
		/// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
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
			where TQ : IChunkQuery16<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
		{
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(ref q, query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <typeparam name="T9">Type of component 9 to retrieve</typeparam>
        /// <typeparam name="T10">Type of component 10 to retrieve</typeparam>
        /// <typeparam name="T11">Type of component 11 to retrieve</typeparam>
        /// <typeparam name="T12">Type of component 12 to retrieve</typeparam>
        /// <typeparam name="T13">Type of component 13 to retrieve</typeparam>
        /// <typeparam name="T14">Type of component 14 to retrieve</typeparam>
        /// <typeparam name="T15">Type of component 15 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
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
			where TQ : IChunkQuery16<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
		{
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <typeparam name="T9">Type of component 9 to retrieve</typeparam>
        /// <typeparam name="T10">Type of component 10 to retrieve</typeparam>
        /// <typeparam name="T11">Type of component 11 to retrieve</typeparam>
        /// <typeparam name="T12">Type of component 12 to retrieve</typeparam>
        /// <typeparam name="T13">Type of component 13 to retrieve</typeparam>
        /// <typeparam name="T14">Type of component 14 to retrieve</typeparam>
        /// <typeparam name="T15">Type of component 15 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a default
		/// query object will be used (based on type parameters).</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
			ref TQ q,
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
			where TQ : IChunkQuery16<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
		{
			return ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(ref q, ref query);
		}

		/// <summary>
        /// Execute a query which executes on entire chunks.
        /// </summary>
        /// <typeparam name="TQ">The type of the query</typeparam>
        /// <typeparam name="T0">Type of component 0 to retrieve</typeparam>
        /// <typeparam name="T1">Type of component 1 to retrieve</typeparam>
        /// <typeparam name="T2">Type of component 2 to retrieve</typeparam>
        /// <typeparam name="T3">Type of component 3 to retrieve</typeparam>
        /// <typeparam name="T4">Type of component 4 to retrieve</typeparam>
        /// <typeparam name="T5">Type of component 5 to retrieve</typeparam>
        /// <typeparam name="T6">Type of component 6 to retrieve</typeparam>
        /// <typeparam name="T7">Type of component 7 to retrieve</typeparam>
        /// <typeparam name="T8">Type of component 8 to retrieve</typeparam>
        /// <typeparam name="T9">Type of component 9 to retrieve</typeparam>
        /// <typeparam name="T10">Type of component 10 to retrieve</typeparam>
        /// <typeparam name="T11">Type of component 11 to retrieve</typeparam>
        /// <typeparam name="T12">Type of component 12 to retrieve</typeparam>
        /// <typeparam name="T13">Type of component 13 to retrieve</typeparam>
        /// <typeparam name="T14">Type of component 14 to retrieve</typeparam>
        /// <typeparam name="T15">Type of component 15 to retrieve</typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over. If null a suitable
		/// query object will automatically be created and written into this field.</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
			ref TQ q,
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
			where TQ : IChunkQuery16<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>();

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
		    var c9 = ComponentID<T9>.ID;
		    var c10 = ComponentID<T10>.ID;
		    var c11 = ComponentID<T11>.ID;
		    var c12 = ComponentID<T12>.ID;
		    var c13 = ComponentID<T13>.ID;
		    var c14 = ComponentID<T14>.ID;
		    var c15 = ComponentID<T15>.ID;

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				var chunks = archetype.Chunks;
				for (var c = chunks.Count - 1; c >= 0; c--)
				{
					var chunk = chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

					count += entities.Length;

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

					q.Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
				}
			}

			return count;
		}

		public int ExecuteChunkParallel<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
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
			where TQ : IChunkQuery16<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>();

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
		    var c9 = ComponentID<T9>.ID;
		    var c10 = ComponentID<T10>.ID;
		    var c11 = ComponentID<T11>.ID;
		    var c12 = ComponentID<T12>.ID;
		    var c13 = ComponentID<T13>.ID;
		    var c14 = ComponentID<T14>.ID;
		    var c15 = ComponentID<T15>.ID;

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
				var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				Parallel.For(0, archetype.Chunks.Count, c =>
				{
					var chunk = archetype.Chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						return;

					Interlocked.Add(ref count, entities.Length);

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

					q.Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
				});
			}

			return count;
		}
	}
}



