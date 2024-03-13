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
        /// <typeparam name="T0"></typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0>(
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
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over</param>
        /// <returns>The total number of entities processed</returns>
		public int ExecuteChunk<TQ, T0, T1>(
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
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over</param>
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
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over</param>
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
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over</param>
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
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over</param>
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
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over</param>
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
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over</param>
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
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <param name="q">The TQ instance which will be executed for each chunk</param>
        /// <param name="query">A query expressing which entities to execute this query over</param>
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



