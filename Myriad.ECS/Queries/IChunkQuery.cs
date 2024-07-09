﻿using Myriad.ECS.Queries;
using Myriad.ECS.IDs;
using Myriad.ECS.Threading;

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
                count += archetype.EntityCount;

				Parallel.For(0, archetype.Chunks.Count, c =>
				{
					var chunk = archetype.Chunks[c];

					var entities = chunk.Entities;
					if (entities.Length == 0)
						return;

					var t0 = chunk.GetSpan<T0>(c0);

					q.Execute(entities, t0);
				});
			}

			return count;
		}

		private readonly struct ChunkWorkItem1<TQ, T0>
			: IWorkItem
			where T0 : IComponent
			where TQ : IChunkQuery1<T0>
		{
			private readonly TQ _q;

			private readonly Memory<Entity> _entities;
			private readonly Memory<T0> _c0;

			public ChunkWorkItem1(
				Memory<Entity> entities,
				Memory<T0> c0,
				TQ q
			)
			{
				_entities = entities;

				_c0 = c0;

				_q = q;
			}

			public readonly void Execute()
			{
				_q.Execute(
					_entities.Span
					, _c0.Span
				);
			}
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

		private readonly struct ChunkWorkItem2<TQ, T0, T1>
			: IWorkItem
			where T0 : IComponent
            where T1 : IComponent
			where TQ : IChunkQuery2<T0, T1>
		{
			private readonly TQ _q;

			private readonly Memory<Entity> _entities;
			private readonly Memory<T0> _c0;
			private readonly Memory<T1> _c1;

			public ChunkWorkItem2(
				Memory<Entity> entities,
				Memory<T0> c0,
				Memory<T1> c1,
				TQ q
			)
			{
				_entities = entities;

				_c0 = c0;
				_c1 = c1;

				_q = q;
			}

			public readonly void Execute()
			{
				_q.Execute(
					_entities.Span
					, _c0.Span
					, _c1.Span
				);
			}
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

		private readonly struct ChunkWorkItem3<TQ, T0, T1, T2>
			: IWorkItem
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
			where TQ : IChunkQuery3<T0, T1, T2>
		{
			private readonly TQ _q;

			private readonly Memory<Entity> _entities;
			private readonly Memory<T0> _c0;
			private readonly Memory<T1> _c1;
			private readonly Memory<T2> _c2;

			public ChunkWorkItem3(
				Memory<Entity> entities,
				Memory<T0> c0,
				Memory<T1> c1,
				Memory<T2> c2,
				TQ q
			)
			{
				_entities = entities;

				_c0 = c0;
				_c1 = c1;
				_c2 = c2;

				_q = q;
			}

			public readonly void Execute()
			{
				_q.Execute(
					_entities.Span
					, _c0.Span
					, _c1.Span
					, _c2.Span
				);
			}
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

		private readonly struct ChunkWorkItem4<TQ, T0, T1, T2, T3>
			: IWorkItem
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
			where TQ : IChunkQuery4<T0, T1, T2, T3>
		{
			private readonly TQ _q;

			private readonly Memory<Entity> _entities;
			private readonly Memory<T0> _c0;
			private readonly Memory<T1> _c1;
			private readonly Memory<T2> _c2;
			private readonly Memory<T3> _c3;

			public ChunkWorkItem4(
				Memory<Entity> entities,
				Memory<T0> c0,
				Memory<T1> c1,
				Memory<T2> c2,
				Memory<T3> c3,
				TQ q
			)
			{
				_entities = entities;

				_c0 = c0;
				_c1 = c1;
				_c2 = c2;
				_c3 = c3;

				_q = q;
			}

			public readonly void Execute()
			{
				_q.Execute(
					_entities.Span
					, _c0.Span
					, _c1.Span
					, _c2.Span
					, _c3.Span
				);
			}
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

		private readonly struct ChunkWorkItem5<TQ, T0, T1, T2, T3, T4>
			: IWorkItem
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
			where TQ : IChunkQuery5<T0, T1, T2, T3, T4>
		{
			private readonly TQ _q;

			private readonly Memory<Entity> _entities;
			private readonly Memory<T0> _c0;
			private readonly Memory<T1> _c1;
			private readonly Memory<T2> _c2;
			private readonly Memory<T3> _c3;
			private readonly Memory<T4> _c4;

			public ChunkWorkItem5(
				Memory<Entity> entities,
				Memory<T0> c0,
				Memory<T1> c1,
				Memory<T2> c2,
				Memory<T3> c3,
				Memory<T4> c4,
				TQ q
			)
			{
				_entities = entities;

				_c0 = c0;
				_c1 = c1;
				_c2 = c2;
				_c3 = c3;
				_c4 = c4;

				_q = q;
			}

			public readonly void Execute()
			{
				_q.Execute(
					_entities.Span
					, _c0.Span
					, _c1.Span
					, _c2.Span
					, _c3.Span
					, _c4.Span
				);
			}
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

		private readonly struct ChunkWorkItem6<TQ, T0, T1, T2, T3, T4, T5>
			: IWorkItem
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
			where TQ : IChunkQuery6<T0, T1, T2, T3, T4, T5>
		{
			private readonly TQ _q;

			private readonly Memory<Entity> _entities;
			private readonly Memory<T0> _c0;
			private readonly Memory<T1> _c1;
			private readonly Memory<T2> _c2;
			private readonly Memory<T3> _c3;
			private readonly Memory<T4> _c4;
			private readonly Memory<T5> _c5;

			public ChunkWorkItem6(
				Memory<Entity> entities,
				Memory<T0> c0,
				Memory<T1> c1,
				Memory<T2> c2,
				Memory<T3> c3,
				Memory<T4> c4,
				Memory<T5> c5,
				TQ q
			)
			{
				_entities = entities;

				_c0 = c0;
				_c1 = c1;
				_c2 = c2;
				_c3 = c3;
				_c4 = c4;
				_c5 = c5;

				_q = q;
			}

			public readonly void Execute()
			{
				_q.Execute(
					_entities.Span
					, _c0.Span
					, _c1.Span
					, _c2.Span
					, _c3.Span
					, _c4.Span
					, _c5.Span
				);
			}
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

		private readonly struct ChunkWorkItem7<TQ, T0, T1, T2, T3, T4, T5, T6>
			: IWorkItem
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
			where TQ : IChunkQuery7<T0, T1, T2, T3, T4, T5, T6>
		{
			private readonly TQ _q;

			private readonly Memory<Entity> _entities;
			private readonly Memory<T0> _c0;
			private readonly Memory<T1> _c1;
			private readonly Memory<T2> _c2;
			private readonly Memory<T3> _c3;
			private readonly Memory<T4> _c4;
			private readonly Memory<T5> _c5;
			private readonly Memory<T6> _c6;

			public ChunkWorkItem7(
				Memory<Entity> entities,
				Memory<T0> c0,
				Memory<T1> c1,
				Memory<T2> c2,
				Memory<T3> c3,
				Memory<T4> c4,
				Memory<T5> c5,
				Memory<T6> c6,
				TQ q
			)
			{
				_entities = entities;

				_c0 = c0;
				_c1 = c1;
				_c2 = c2;
				_c3 = c3;
				_c4 = c4;
				_c5 = c5;
				_c6 = c6;

				_q = q;
			}

			public readonly void Execute()
			{
				_q.Execute(
					_entities.Span
					, _c0.Span
					, _c1.Span
					, _c2.Span
					, _c3.Span
					, _c4.Span
					, _c5.Span
					, _c6.Span
				);
			}
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

		private readonly struct ChunkWorkItem8<TQ, T0, T1, T2, T3, T4, T5, T6, T7>
			: IWorkItem
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
			private readonly TQ _q;

			private readonly Memory<Entity> _entities;
			private readonly Memory<T0> _c0;
			private readonly Memory<T1> _c1;
			private readonly Memory<T2> _c2;
			private readonly Memory<T3> _c3;
			private readonly Memory<T4> _c4;
			private readonly Memory<T5> _c5;
			private readonly Memory<T6> _c6;
			private readonly Memory<T7> _c7;

			public ChunkWorkItem8(
				Memory<Entity> entities,
				Memory<T0> c0,
				Memory<T1> c1,
				Memory<T2> c2,
				Memory<T3> c3,
				Memory<T4> c4,
				Memory<T5> c5,
				Memory<T6> c6,
				Memory<T7> c7,
				TQ q
			)
			{
				_entities = entities;

				_c0 = c0;
				_c1 = c1;
				_c2 = c2;
				_c3 = c3;
				_c4 = c4;
				_c5 = c5;
				_c6 = c6;
				_c7 = c7;

				_q = q;
			}

			public readonly void Execute()
			{
				_q.Execute(
					_entities.Span
					, _c0.Span
					, _c1.Span
					, _c2.Span
					, _c3.Span
					, _c4.Span
					, _c5.Span
					, _c6.Span
					, _c7.Span
				);
			}
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

		private readonly struct ChunkWorkItem9<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8>
			: IWorkItem
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
			private readonly TQ _q;

			private readonly Memory<Entity> _entities;
			private readonly Memory<T0> _c0;
			private readonly Memory<T1> _c1;
			private readonly Memory<T2> _c2;
			private readonly Memory<T3> _c3;
			private readonly Memory<T4> _c4;
			private readonly Memory<T5> _c5;
			private readonly Memory<T6> _c6;
			private readonly Memory<T7> _c7;
			private readonly Memory<T8> _c8;

			public ChunkWorkItem9(
				Memory<Entity> entities,
				Memory<T0> c0,
				Memory<T1> c1,
				Memory<T2> c2,
				Memory<T3> c3,
				Memory<T4> c4,
				Memory<T5> c5,
				Memory<T6> c6,
				Memory<T7> c7,
				Memory<T8> c8,
				TQ q
			)
			{
				_entities = entities;

				_c0 = c0;
				_c1 = c1;
				_c2 = c2;
				_c3 = c3;
				_c4 = c4;
				_c5 = c5;
				_c6 = c6;
				_c7 = c7;
				_c8 = c8;

				_q = q;
			}

			public readonly void Execute()
			{
				_q.Execute(
					_entities.Span
					, _c0.Span
					, _c1.Span
					, _c2.Span
					, _c3.Span
					, _c4.Span
					, _c5.Span
					, _c6.Span
					, _c7.Span
					, _c8.Span
				);
			}
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

		private readonly struct ChunkWorkItem10<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
			: IWorkItem
			where T0 : IComponent
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
			private readonly TQ _q;

			private readonly Memory<Entity> _entities;
			private readonly Memory<T0> _c0;
			private readonly Memory<T1> _c1;
			private readonly Memory<T2> _c2;
			private readonly Memory<T3> _c3;
			private readonly Memory<T4> _c4;
			private readonly Memory<T5> _c5;
			private readonly Memory<T6> _c6;
			private readonly Memory<T7> _c7;
			private readonly Memory<T8> _c8;
			private readonly Memory<T9> _c9;

			public ChunkWorkItem10(
				Memory<Entity> entities,
				Memory<T0> c0,
				Memory<T1> c1,
				Memory<T2> c2,
				Memory<T3> c3,
				Memory<T4> c4,
				Memory<T5> c5,
				Memory<T6> c6,
				Memory<T7> c7,
				Memory<T8> c8,
				Memory<T9> c9,
				TQ q
			)
			{
				_entities = entities;

				_c0 = c0;
				_c1 = c1;
				_c2 = c2;
				_c3 = c3;
				_c4 = c4;
				_c5 = c5;
				_c6 = c6;
				_c7 = c7;
				_c8 = c8;
				_c9 = c9;

				_q = q;
			}

			public readonly void Execute()
			{
				_q.Execute(
					_entities.Span
					, _c0.Span
					, _c1.Span
					, _c2.Span
					, _c3.Span
					, _c4.Span
					, _c5.Span
					, _c6.Span
					, _c7.Span
					, _c8.Span
					, _c9.Span
				);
			}
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

		private readonly struct ChunkWorkItem11<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
			: IWorkItem
			where T0 : IComponent
            where T1 : IComponent
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
			private readonly TQ _q;

			private readonly Memory<Entity> _entities;
			private readonly Memory<T0> _c0;
			private readonly Memory<T1> _c1;
			private readonly Memory<T2> _c2;
			private readonly Memory<T3> _c3;
			private readonly Memory<T4> _c4;
			private readonly Memory<T5> _c5;
			private readonly Memory<T6> _c6;
			private readonly Memory<T7> _c7;
			private readonly Memory<T8> _c8;
			private readonly Memory<T9> _c9;
			private readonly Memory<T10> _c10;

			public ChunkWorkItem11(
				Memory<Entity> entities,
				Memory<T0> c0,
				Memory<T1> c1,
				Memory<T2> c2,
				Memory<T3> c3,
				Memory<T4> c4,
				Memory<T5> c5,
				Memory<T6> c6,
				Memory<T7> c7,
				Memory<T8> c8,
				Memory<T9> c9,
				Memory<T10> c10,
				TQ q
			)
			{
				_entities = entities;

				_c0 = c0;
				_c1 = c1;
				_c2 = c2;
				_c3 = c3;
				_c4 = c4;
				_c5 = c5;
				_c6 = c6;
				_c7 = c7;
				_c8 = c8;
				_c9 = c9;
				_c10 = c10;

				_q = q;
			}

			public readonly void Execute()
			{
				_q.Execute(
					_entities.Span
					, _c0.Span
					, _c1.Span
					, _c2.Span
					, _c3.Span
					, _c4.Span
					, _c5.Span
					, _c6.Span
					, _c7.Span
					, _c8.Span
					, _c9.Span
					, _c10.Span
				);
			}
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

		private readonly struct ChunkWorkItem12<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
			: IWorkItem
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
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
			private readonly TQ _q;

			private readonly Memory<Entity> _entities;
			private readonly Memory<T0> _c0;
			private readonly Memory<T1> _c1;
			private readonly Memory<T2> _c2;
			private readonly Memory<T3> _c3;
			private readonly Memory<T4> _c4;
			private readonly Memory<T5> _c5;
			private readonly Memory<T6> _c6;
			private readonly Memory<T7> _c7;
			private readonly Memory<T8> _c8;
			private readonly Memory<T9> _c9;
			private readonly Memory<T10> _c10;
			private readonly Memory<T11> _c11;

			public ChunkWorkItem12(
				Memory<Entity> entities,
				Memory<T0> c0,
				Memory<T1> c1,
				Memory<T2> c2,
				Memory<T3> c3,
				Memory<T4> c4,
				Memory<T5> c5,
				Memory<T6> c6,
				Memory<T7> c7,
				Memory<T8> c8,
				Memory<T9> c9,
				Memory<T10> c10,
				Memory<T11> c11,
				TQ q
			)
			{
				_entities = entities;

				_c0 = c0;
				_c1 = c1;
				_c2 = c2;
				_c3 = c3;
				_c4 = c4;
				_c5 = c5;
				_c6 = c6;
				_c7 = c7;
				_c8 = c8;
				_c9 = c9;
				_c10 = c10;
				_c11 = c11;

				_q = q;
			}

			public readonly void Execute()
			{
				_q.Execute(
					_entities.Span
					, _c0.Span
					, _c1.Span
					, _c2.Span
					, _c3.Span
					, _c4.Span
					, _c5.Span
					, _c6.Span
					, _c7.Span
					, _c8.Span
					, _c9.Span
					, _c10.Span
					, _c11.Span
				);
			}
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

		private readonly struct ChunkWorkItem13<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
			: IWorkItem
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
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
			private readonly TQ _q;

			private readonly Memory<Entity> _entities;
			private readonly Memory<T0> _c0;
			private readonly Memory<T1> _c1;
			private readonly Memory<T2> _c2;
			private readonly Memory<T3> _c3;
			private readonly Memory<T4> _c4;
			private readonly Memory<T5> _c5;
			private readonly Memory<T6> _c6;
			private readonly Memory<T7> _c7;
			private readonly Memory<T8> _c8;
			private readonly Memory<T9> _c9;
			private readonly Memory<T10> _c10;
			private readonly Memory<T11> _c11;
			private readonly Memory<T12> _c12;

			public ChunkWorkItem13(
				Memory<Entity> entities,
				Memory<T0> c0,
				Memory<T1> c1,
				Memory<T2> c2,
				Memory<T3> c3,
				Memory<T4> c4,
				Memory<T5> c5,
				Memory<T6> c6,
				Memory<T7> c7,
				Memory<T8> c8,
				Memory<T9> c9,
				Memory<T10> c10,
				Memory<T11> c11,
				Memory<T12> c12,
				TQ q
			)
			{
				_entities = entities;

				_c0 = c0;
				_c1 = c1;
				_c2 = c2;
				_c3 = c3;
				_c4 = c4;
				_c5 = c5;
				_c6 = c6;
				_c7 = c7;
				_c8 = c8;
				_c9 = c9;
				_c10 = c10;
				_c11 = c11;
				_c12 = c12;

				_q = q;
			}

			public readonly void Execute()
			{
				_q.Execute(
					_entities.Span
					, _c0.Span
					, _c1.Span
					, _c2.Span
					, _c3.Span
					, _c4.Span
					, _c5.Span
					, _c6.Span
					, _c7.Span
					, _c8.Span
					, _c9.Span
					, _c10.Span
					, _c11.Span
					, _c12.Span
				);
			}
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

		private readonly struct ChunkWorkItem14<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
			: IWorkItem
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
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
			private readonly TQ _q;

			private readonly Memory<Entity> _entities;
			private readonly Memory<T0> _c0;
			private readonly Memory<T1> _c1;
			private readonly Memory<T2> _c2;
			private readonly Memory<T3> _c3;
			private readonly Memory<T4> _c4;
			private readonly Memory<T5> _c5;
			private readonly Memory<T6> _c6;
			private readonly Memory<T7> _c7;
			private readonly Memory<T8> _c8;
			private readonly Memory<T9> _c9;
			private readonly Memory<T10> _c10;
			private readonly Memory<T11> _c11;
			private readonly Memory<T12> _c12;
			private readonly Memory<T13> _c13;

			public ChunkWorkItem14(
				Memory<Entity> entities,
				Memory<T0> c0,
				Memory<T1> c1,
				Memory<T2> c2,
				Memory<T3> c3,
				Memory<T4> c4,
				Memory<T5> c5,
				Memory<T6> c6,
				Memory<T7> c7,
				Memory<T8> c8,
				Memory<T9> c9,
				Memory<T10> c10,
				Memory<T11> c11,
				Memory<T12> c12,
				Memory<T13> c13,
				TQ q
			)
			{
				_entities = entities;

				_c0 = c0;
				_c1 = c1;
				_c2 = c2;
				_c3 = c3;
				_c4 = c4;
				_c5 = c5;
				_c6 = c6;
				_c7 = c7;
				_c8 = c8;
				_c9 = c9;
				_c10 = c10;
				_c11 = c11;
				_c12 = c12;
				_c13 = c13;

				_q = q;
			}

			public readonly void Execute()
			{
				_q.Execute(
					_entities.Span
					, _c0.Span
					, _c1.Span
					, _c2.Span
					, _c3.Span
					, _c4.Span
					, _c5.Span
					, _c6.Span
					, _c7.Span
					, _c8.Span
					, _c9.Span
					, _c10.Span
					, _c11.Span
					, _c12.Span
					, _c13.Span
				);
			}
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

		private readonly struct ChunkWorkItem15<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
			: IWorkItem
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
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
			private readonly TQ _q;

			private readonly Memory<Entity> _entities;
			private readonly Memory<T0> _c0;
			private readonly Memory<T1> _c1;
			private readonly Memory<T2> _c2;
			private readonly Memory<T3> _c3;
			private readonly Memory<T4> _c4;
			private readonly Memory<T5> _c5;
			private readonly Memory<T6> _c6;
			private readonly Memory<T7> _c7;
			private readonly Memory<T8> _c8;
			private readonly Memory<T9> _c9;
			private readonly Memory<T10> _c10;
			private readonly Memory<T11> _c11;
			private readonly Memory<T12> _c12;
			private readonly Memory<T13> _c13;
			private readonly Memory<T14> _c14;

			public ChunkWorkItem15(
				Memory<Entity> entities,
				Memory<T0> c0,
				Memory<T1> c1,
				Memory<T2> c2,
				Memory<T3> c3,
				Memory<T4> c4,
				Memory<T5> c5,
				Memory<T6> c6,
				Memory<T7> c7,
				Memory<T8> c8,
				Memory<T9> c9,
				Memory<T10> c10,
				Memory<T11> c11,
				Memory<T12> c12,
				Memory<T13> c13,
				Memory<T14> c14,
				TQ q
			)
			{
				_entities = entities;

				_c0 = c0;
				_c1 = c1;
				_c2 = c2;
				_c3 = c3;
				_c4 = c4;
				_c5 = c5;
				_c6 = c6;
				_c7 = c7;
				_c8 = c8;
				_c9 = c9;
				_c10 = c10;
				_c11 = c11;
				_c12 = c12;
				_c13 = c13;
				_c14 = c14;

				_q = q;
			}

			public readonly void Execute()
			{
				_q.Execute(
					_entities.Span
					, _c0.Span
					, _c1.Span
					, _c2.Span
					, _c3.Span
					, _c4.Span
					, _c5.Span
					, _c6.Span
					, _c7.Span
					, _c8.Span
					, _c9.Span
					, _c10.Span
					, _c11.Span
					, _c12.Span
					, _c13.Span
					, _c14.Span
				);
			}
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

		private readonly struct ChunkWorkItem16<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
			: IWorkItem
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
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
			private readonly TQ _q;

			private readonly Memory<Entity> _entities;
			private readonly Memory<T0> _c0;
			private readonly Memory<T1> _c1;
			private readonly Memory<T2> _c2;
			private readonly Memory<T3> _c3;
			private readonly Memory<T4> _c4;
			private readonly Memory<T5> _c5;
			private readonly Memory<T6> _c6;
			private readonly Memory<T7> _c7;
			private readonly Memory<T8> _c8;
			private readonly Memory<T9> _c9;
			private readonly Memory<T10> _c10;
			private readonly Memory<T11> _c11;
			private readonly Memory<T12> _c12;
			private readonly Memory<T13> _c13;
			private readonly Memory<T14> _c14;
			private readonly Memory<T15> _c15;

			public ChunkWorkItem16(
				Memory<Entity> entities,
				Memory<T0> c0,
				Memory<T1> c1,
				Memory<T2> c2,
				Memory<T3> c3,
				Memory<T4> c4,
				Memory<T5> c5,
				Memory<T6> c6,
				Memory<T7> c7,
				Memory<T8> c8,
				Memory<T9> c9,
				Memory<T10> c10,
				Memory<T11> c11,
				Memory<T12> c12,
				Memory<T13> c13,
				Memory<T14> c14,
				Memory<T15> c15,
				TQ q
			)
			{
				_entities = entities;

				_c0 = c0;
				_c1 = c1;
				_c2 = c2;
				_c3 = c3;
				_c4 = c4;
				_c5 = c5;
				_c6 = c6;
				_c7 = c7;
				_c8 = c8;
				_c9 = c9;
				_c10 = c10;
				_c11 = c11;
				_c12 = c12;
				_c13 = c13;
				_c14 = c14;
				_c15 = c15;

				_q = q;
			}

			public readonly void Execute()
			{
				_q.Execute(
					_entities.Span
					, _c0.Span
					, _c1.Span
					, _c2.Span
					, _c3.Span
					, _c4.Span
					, _c5.Span
					, _c6.Span
					, _c7.Span
					, _c8.Span
					, _c9.Span
					, _c10.Span
					, _c11.Span
					, _c12.Span
					, _c13.Span
					, _c14.Span
					, _c15.Span
				);
			}
		}
	}
}



