using Myriad.ECS.Queries;
using Myriad.ECS.IDs;

//using Parallel = System.Threading.Tasks.Parallel;
//using Parallel = ParallelTasks.Parallel;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedParameter.Global
// ReSharper disable LoopCanBeConvertedToQuery
// ReSharper disable ConvertToPrimaryConstructor

namespace Myriad.ECS.Queries
{
	public interface IQueryMap1<out TR, T0>
		where T0 : IComponent
	{
		public TR Execute(Entity e, ref T0 t0);
	}

	public interface IQueryReduce1<TValue>
	{
		public TValue Reduce(TValue a, TValue b);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to one final output
		/// </summary>
		/// <typeparam name="TM">Type of mapper</typeparam>
		/// <typeparam name="TR">Type of reducer</typeparam>
		/// <typeparam name="TOutput">Type of value returned</typeparam>
		/// <typeparam name="T0">Component 0 to pass into mapper</typeparam>
		/// <param name="q">query, which produces a value per entity</param>
		/// <param name="r">Reducer, for combining values</param>
		/// <param name="initial">Initial value to start reducing from</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		public TOutput ExecuteMapReduce<TM, TR, TOutput, T0>(
			TM q,
			TR r,
			TOutput initial,
			QueryDescription? query = null
		)
			where T0 : IComponent
			where TM : IQueryMap1<TOutput, T0>
			where TR : IQueryReduce1<TOutput>
			where TOutput : unmanaged
		{
			query ??= GetCachedQuery<T0>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return initial;

			var c0 = ComponentID<T0>.ID;

			var accumulator = initial;

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

					var t0 = chunk.GetSpan<T0>(c0);

					for (var i = entities.Length - 1; i >= 0; i--)
					{
						accumulator = r.Reduce(accumulator, q.Execute(entities[i], ref t0[i]));
					}
				}
			}

			return accumulator;
		}

		/* public int ExecuteParallel<TQ, T0>(
			TQ q,
			QueryDescription? query = null,
			int batchSize = 16
		)
			where T0 : IComponent
			where TQ : IQueryMap1<T0>
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
		} */
	}
}
namespace Myriad.ECS.Queries
{
	public interface IQueryMap2<out TR, T0, T1>
		where T0 : IComponent
        where T1 : IComponent
	{
		public TR Execute(Entity e, ref T0 t0, ref T1 t1);
	}

	public interface IQueryReduce2<TValue>
	{
		public TValue Reduce(TValue a, TValue b);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to one final output
		/// </summary>
		/// <typeparam name="TM">Type of mapper</typeparam>
		/// <typeparam name="TR">Type of reducer</typeparam>
		/// <typeparam name="TOutput">Type of value returned</typeparam>
		/// <typeparam name="T0">Component 0 to pass into mapper</typeparam>
		/// <typeparam name="T1">Component 1 to pass into mapper</typeparam>
		/// <param name="q">query, which produces a value per entity</param>
		/// <param name="r">Reducer, for combining values</param>
		/// <param name="initial">Initial value to start reducing from</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		public TOutput ExecuteMapReduce<TM, TR, TOutput, T0, T1>(
			TM q,
			TR r,
			TOutput initial,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
			where TM : IQueryMap2<TOutput, T0, T1>
			where TR : IQueryReduce2<TOutput>
			where TOutput : unmanaged
		{
			query ??= GetCachedQuery<T0, T1>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return initial;

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;

			var accumulator = initial;

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

		/* public int ExecuteParallel<TQ, T0, T1>(
			TQ q,
			QueryDescription? query = null,
			int batchSize = 16
		)
			where T0 : IComponent
            where T1 : IComponent
			where TQ : IQueryMap2<T0, T1>
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
		} */
	}
}
namespace Myriad.ECS.Queries
{
	public interface IQueryMap3<out TR, T0, T1, T2>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
	{
		public TR Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2);
	}

	public interface IQueryReduce3<TValue>
	{
		public TValue Reduce(TValue a, TValue b);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to one final output
		/// </summary>
		/// <typeparam name="TM">Type of mapper</typeparam>
		/// <typeparam name="TR">Type of reducer</typeparam>
		/// <typeparam name="TOutput">Type of value returned</typeparam>
		/// <typeparam name="T0">Component 0 to pass into mapper</typeparam>
		/// <typeparam name="T1">Component 1 to pass into mapper</typeparam>
		/// <typeparam name="T2">Component 2 to pass into mapper</typeparam>
		/// <param name="q">query, which produces a value per entity</param>
		/// <param name="r">Reducer, for combining values</param>
		/// <param name="initial">Initial value to start reducing from</param>
		/// <param name="query">Query to select matched entities</param>
		/// <returns>A value calculated by reducing all intermediate values</returns>
		public TOutput ExecuteMapReduce<TM, TR, TOutput, T0, T1, T2>(
			TM q,
			TR r,
			TOutput initial,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
			where TM : IQueryMap3<TOutput, T0, T1, T2>
			where TR : IQueryReduce3<TOutput>
			where TOutput : unmanaged
		{
			query ??= GetCachedQuery<T0, T1, T2>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return initial;

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;
			var c2 = ComponentID<T2>.ID;

			var accumulator = initial;

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

		/* public int ExecuteParallel<TQ, T0, T1, T2>(
			TQ q,
			QueryDescription? query = null,
			int batchSize = 16
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
			where TQ : IQueryMap3<T0, T1, T2>
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
		} */
	}
}
namespace Myriad.ECS.Queries
{
	public interface IQueryMap4<out TR, T0, T1, T2, T3>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
	{
		public TR Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3);
	}

	public interface IQueryReduce4<TValue>
	{
		public TValue Reduce(TValue a, TValue b);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to one final output
		/// </summary>
		/// <typeparam name="TM">Type of mapper</typeparam>
		/// <typeparam name="TR">Type of reducer</typeparam>
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
		public TOutput ExecuteMapReduce<TM, TR, TOutput, T0, T1, T2, T3>(
			TM q,
			TR r,
			TOutput initial,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
			where TM : IQueryMap4<TOutput, T0, T1, T2, T3>
			where TR : IQueryReduce4<TOutput>
			where TOutput : unmanaged
		{
			query ??= GetCachedQuery<T0, T1, T2, T3>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return initial;

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;
			var c2 = ComponentID<T2>.ID;
			var c3 = ComponentID<T3>.ID;

			var accumulator = initial;

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

		/* public int ExecuteParallel<TQ, T0, T1, T2, T3>(
			TQ q,
			QueryDescription? query = null,
			int batchSize = 16
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
			where TQ : IQueryMap4<T0, T1, T2, T3>
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
		} */
	}
}
namespace Myriad.ECS.Queries
{
	public interface IQueryMap5<out TR, T0, T1, T2, T3, T4>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
	{
		public TR Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4);
	}

	public interface IQueryReduce5<TValue>
	{
		public TValue Reduce(TValue a, TValue b);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to one final output
		/// </summary>
		/// <typeparam name="TM">Type of mapper</typeparam>
		/// <typeparam name="TR">Type of reducer</typeparam>
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
		public TOutput ExecuteMapReduce<TM, TR, TOutput, T0, T1, T2, T3, T4>(
			TM q,
			TR r,
			TOutput initial,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
			where TM : IQueryMap5<TOutput, T0, T1, T2, T3, T4>
			where TR : IQueryReduce5<TOutput>
			where TOutput : unmanaged
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return initial;

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;
			var c2 = ComponentID<T2>.ID;
			var c3 = ComponentID<T3>.ID;
			var c4 = ComponentID<T4>.ID;

			var accumulator = initial;

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

		/* public int ExecuteParallel<TQ, T0, T1, T2, T3, T4>(
			TQ q,
			QueryDescription? query = null,
			int batchSize = 16
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
			where TQ : IQueryMap5<T0, T1, T2, T3, T4>
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
		} */
	}
}
namespace Myriad.ECS.Queries
{
	public interface IQueryMap6<out TR, T0, T1, T2, T3, T4, T5>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
	{
		public TR Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5);
	}

	public interface IQueryReduce6<TValue>
	{
		public TValue Reduce(TValue a, TValue b);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to one final output
		/// </summary>
		/// <typeparam name="TM">Type of mapper</typeparam>
		/// <typeparam name="TR">Type of reducer</typeparam>
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
		public TOutput ExecuteMapReduce<TM, TR, TOutput, T0, T1, T2, T3, T4, T5>(
			TM q,
			TR r,
			TOutput initial,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
			where TM : IQueryMap6<TOutput, T0, T1, T2, T3, T4, T5>
			where TR : IQueryReduce6<TOutput>
			where TOutput : unmanaged
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return initial;

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
				if (archetype.EntityCount == 0)
					continue;

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
					{
						accumulator = r.Reduce(accumulator, q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i]));
					}
				}
			}

			return accumulator;
		}

		/* public int ExecuteParallel<TQ, T0, T1, T2, T3, T4, T5>(
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
			where TQ : IQueryMap6<T0, T1, T2, T3, T4, T5>
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
		} */
	}
}
namespace Myriad.ECS.Queries
{
	public interface IQueryMap7<out TR, T0, T1, T2, T3, T4, T5, T6>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
	{
		public TR Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6);
	}

	public interface IQueryReduce7<TValue>
	{
		public TValue Reduce(TValue a, TValue b);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to one final output
		/// </summary>
		/// <typeparam name="TM">Type of mapper</typeparam>
		/// <typeparam name="TR">Type of reducer</typeparam>
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
		public TOutput ExecuteMapReduce<TM, TR, TOutput, T0, T1, T2, T3, T4, T5, T6>(
			TM q,
			TR r,
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
			where TM : IQueryMap7<TOutput, T0, T1, T2, T3, T4, T5, T6>
			where TR : IQueryReduce7<TOutput>
			where TOutput : unmanaged
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return initial;

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
				if (archetype.EntityCount == 0)
					continue;

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
					{
						accumulator = r.Reduce(accumulator, q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i]));
					}
				}
			}

			return accumulator;
		}

		/* public int ExecuteParallel<TQ, T0, T1, T2, T3, T4, T5, T6>(
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
			where TQ : IQueryMap7<T0, T1, T2, T3, T4, T5, T6>
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
		} */
	}
}
namespace Myriad.ECS.Queries
{
	public interface IQueryMap8<out TR, T0, T1, T2, T3, T4, T5, T6, T7>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
	{
		public TR Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7);
	}

	public interface IQueryReduce8<TValue>
	{
		public TValue Reduce(TValue a, TValue b);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to one final output
		/// </summary>
		/// <typeparam name="TM">Type of mapper</typeparam>
		/// <typeparam name="TR">Type of reducer</typeparam>
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
		public TOutput ExecuteMapReduce<TM, TR, TOutput, T0, T1, T2, T3, T4, T5, T6, T7>(
			TM q,
			TR r,
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
			where TM : IQueryMap8<TOutput, T0, T1, T2, T3, T4, T5, T6, T7>
			where TR : IQueryReduce8<TOutput>
			where TOutput : unmanaged
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return initial;

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
				if (archetype.EntityCount == 0)
					continue;

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
					{
						accumulator = r.Reduce(accumulator, q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i]));
					}
				}
			}

			return accumulator;
		}

		/* public int ExecuteParallel<TQ, T0, T1, T2, T3, T4, T5, T6, T7>(
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
			where TQ : IQueryMap8<T0, T1, T2, T3, T4, T5, T6, T7>
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
		} */
	}
}
namespace Myriad.ECS.Queries
{
	public interface IQueryMap9<out TR, T0, T1, T2, T3, T4, T5, T6, T7, T8>
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
		public TR Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8);
	}

	public interface IQueryReduce9<TValue>
	{
		public TValue Reduce(TValue a, TValue b);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to one final output
		/// </summary>
		/// <typeparam name="TM">Type of mapper</typeparam>
		/// <typeparam name="TR">Type of reducer</typeparam>
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
		public TOutput ExecuteMapReduce<TM, TR, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
			TM q,
			TR r,
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
			where TM : IQueryMap9<TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8>
			where TR : IQueryReduce9<TOutput>
			where TOutput : unmanaged
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return initial;

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
				if (archetype.EntityCount == 0)
					continue;

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
					{
						accumulator = r.Reduce(accumulator, q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i]));
					}
				}
			}

			return accumulator;
		}

		/* public int ExecuteParallel<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
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
			where TQ : IQueryMap9<T0, T1, T2, T3, T4, T5, T6, T7, T8>
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
		} */
	}
}
namespace Myriad.ECS.Queries
{
	public interface IQueryMap10<out TR, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
		where T0 : IComponent
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
		public TR Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9);
	}

	public interface IQueryReduce10<TValue>
	{
		public TValue Reduce(TValue a, TValue b);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to one final output
		/// </summary>
		/// <typeparam name="TM">Type of mapper</typeparam>
		/// <typeparam name="TR">Type of reducer</typeparam>
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
		public TOutput ExecuteMapReduce<TM, TR, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
			TM q,
			TR r,
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
			where TM : IQueryMap10<TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
			where TR : IQueryReduce10<TOutput>
			where TOutput : unmanaged
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return initial;

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
				if (archetype.EntityCount == 0)
					continue;

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
					var t9 = chunk.GetSpan<T9>(c9);

					for (var i = entities.Length - 1; i >= 0; i--)
					{
						accumulator = r.Reduce(accumulator, q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i]));
					}
				}
			}

			return accumulator;
		}

		/* public int ExecuteParallel<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
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
            where T9 : IComponent
			where TQ : IQueryMap10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
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
					var t9 = chunk.GetComponentArray<T9>(c9);

					Parallel.For(0, numBatches, b =>
                    {
						var start = b * batchSize;
						var end = Math.Min(start + batchSize, entityCount);

						for (var i = start; i < end; i++)
						{
							var entities = chunk.Entities;
							q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i]);
						}
					});
				}
			}

			return count;
		} */
	}
}
namespace Myriad.ECS.Queries
{
	public interface IQueryMap11<out TR, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
		where T0 : IComponent
        where T1 : IComponent
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
		public TR Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10);
	}

	public interface IQueryReduce11<TValue>
	{
		public TValue Reduce(TValue a, TValue b);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to one final output
		/// </summary>
		/// <typeparam name="TM">Type of mapper</typeparam>
		/// <typeparam name="TR">Type of reducer</typeparam>
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
		public TOutput ExecuteMapReduce<TM, TR, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
			TM q,
			TR r,
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
			where TM : IQueryMap11<TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
			where TR : IQueryReduce11<TOutput>
			where TOutput : unmanaged
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return initial;

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
				if (archetype.EntityCount == 0)
					continue;

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

		/* public int ExecuteParallel<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
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
            where T9 : IComponent
            where T10 : IComponent
			where TQ : IQueryMap11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
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
					var t9 = chunk.GetComponentArray<T9>(c9);
					var t10 = chunk.GetComponentArray<T10>(c10);

					Parallel.For(0, numBatches, b =>
                    {
						var start = b * batchSize;
						var end = Math.Min(start + batchSize, entityCount);

						for (var i = start; i < end; i++)
						{
							var entities = chunk.Entities;
							q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i]);
						}
					});
				}
			}

			return count;
		} */
	}
}
namespace Myriad.ECS.Queries
{
	public interface IQueryMap12<out TR, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
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
		public TR Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11);
	}

	public interface IQueryReduce12<TValue>
	{
		public TValue Reduce(TValue a, TValue b);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to one final output
		/// </summary>
		/// <typeparam name="TM">Type of mapper</typeparam>
		/// <typeparam name="TR">Type of reducer</typeparam>
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
		public TOutput ExecuteMapReduce<TM, TR, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
			TM q,
			TR r,
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
			where TM : IQueryMap12<TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
			where TR : IQueryReduce12<TOutput>
			where TOutput : unmanaged
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return initial;

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
				if (archetype.EntityCount == 0)
					continue;

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

		/* public int ExecuteParallel<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
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
            where T9 : IComponent
            where T10 : IComponent
            where T11 : IComponent
			where TQ : IQueryMap12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
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
					var t9 = chunk.GetComponentArray<T9>(c9);
					var t10 = chunk.GetComponentArray<T10>(c10);
					var t11 = chunk.GetComponentArray<T11>(c11);

					Parallel.For(0, numBatches, b =>
                    {
						var start = b * batchSize;
						var end = Math.Min(start + batchSize, entityCount);

						for (var i = start; i < end; i++)
						{
							var entities = chunk.Entities;
							q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i]);
						}
					});
				}
			}

			return count;
		} */
	}
}
namespace Myriad.ECS.Queries
{
	public interface IQueryMap13<out TR, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
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
		public TR Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12);
	}

	public interface IQueryReduce13<TValue>
	{
		public TValue Reduce(TValue a, TValue b);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to one final output
		/// </summary>
		/// <typeparam name="TM">Type of mapper</typeparam>
		/// <typeparam name="TR">Type of reducer</typeparam>
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
		public TOutput ExecuteMapReduce<TM, TR, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
			TM q,
			TR r,
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
			where TM : IQueryMap13<TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
			where TR : IQueryReduce13<TOutput>
			where TOutput : unmanaged
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return initial;

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
				if (archetype.EntityCount == 0)
					continue;

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

		/* public int ExecuteParallel<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
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
            where T9 : IComponent
            where T10 : IComponent
            where T11 : IComponent
            where T12 : IComponent
			where TQ : IQueryMap13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
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
					var t9 = chunk.GetComponentArray<T9>(c9);
					var t10 = chunk.GetComponentArray<T10>(c10);
					var t11 = chunk.GetComponentArray<T11>(c11);
					var t12 = chunk.GetComponentArray<T12>(c12);

					Parallel.For(0, numBatches, b =>
                    {
						var start = b * batchSize;
						var end = Math.Min(start + batchSize, entityCount);

						for (var i = start; i < end; i++)
						{
							var entities = chunk.Entities;
							q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i]);
						}
					});
				}
			}

			return count;
		} */
	}
}
namespace Myriad.ECS.Queries
{
	public interface IQueryMap14<out TR, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
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
		public TR Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12, ref T13 t13);
	}

	public interface IQueryReduce14<TValue>
	{
		public TValue Reduce(TValue a, TValue b);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to one final output
		/// </summary>
		/// <typeparam name="TM">Type of mapper</typeparam>
		/// <typeparam name="TR">Type of reducer</typeparam>
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
		public TOutput ExecuteMapReduce<TM, TR, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
			TM q,
			TR r,
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
			where TM : IQueryMap14<TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
			where TR : IQueryReduce14<TOutput>
			where TOutput : unmanaged
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return initial;

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
				if (archetype.EntityCount == 0)
					continue;

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

		/* public int ExecuteParallel<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
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
            where T9 : IComponent
            where T10 : IComponent
            where T11 : IComponent
            where T12 : IComponent
            where T13 : IComponent
			where TQ : IQueryMap14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
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
					var t9 = chunk.GetComponentArray<T9>(c9);
					var t10 = chunk.GetComponentArray<T10>(c10);
					var t11 = chunk.GetComponentArray<T11>(c11);
					var t12 = chunk.GetComponentArray<T12>(c12);
					var t13 = chunk.GetComponentArray<T13>(c13);

					Parallel.For(0, numBatches, b =>
                    {
						var start = b * batchSize;
						var end = Math.Min(start + batchSize, entityCount);

						for (var i = start; i < end; i++)
						{
							var entities = chunk.Entities;
							q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i], ref t13[i]);
						}
					});
				}
			}

			return count;
		} */
	}
}
namespace Myriad.ECS.Queries
{
	public interface IQueryMap15<out TR, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
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
		public TR Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12, ref T13 t13, ref T14 t14);
	}

	public interface IQueryReduce15<TValue>
	{
		public TValue Reduce(TValue a, TValue b);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to one final output
		/// </summary>
		/// <typeparam name="TM">Type of mapper</typeparam>
		/// <typeparam name="TR">Type of reducer</typeparam>
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
		public TOutput ExecuteMapReduce<TM, TR, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
			TM q,
			TR r,
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
			where TM : IQueryMap15<TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
			where TR : IQueryReduce15<TOutput>
			where TOutput : unmanaged
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return initial;

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
				if (archetype.EntityCount == 0)
					continue;

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

		/* public int ExecuteParallel<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
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
            where T9 : IComponent
            where T10 : IComponent
            where T11 : IComponent
            where T12 : IComponent
            where T13 : IComponent
            where T14 : IComponent
			where TQ : IQueryMap15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
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
					var t9 = chunk.GetComponentArray<T9>(c9);
					var t10 = chunk.GetComponentArray<T10>(c10);
					var t11 = chunk.GetComponentArray<T11>(c11);
					var t12 = chunk.GetComponentArray<T12>(c12);
					var t13 = chunk.GetComponentArray<T13>(c13);
					var t14 = chunk.GetComponentArray<T14>(c14);

					Parallel.For(0, numBatches, b =>
                    {
						var start = b * batchSize;
						var end = Math.Min(start + batchSize, entityCount);

						for (var i = start; i < end; i++)
						{
							var entities = chunk.Entities;
							q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i], ref t13[i], ref t14[i]);
						}
					});
				}
			}

			return count;
		} */
	}
}
namespace Myriad.ECS.Queries
{
	public interface IQueryMap16<out TR, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
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
		public TR Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12, ref T13 t13, ref T14 t14, ref T15 t15);
	}

	public interface IQueryReduce16<TValue>
	{
		public TValue Reduce(TValue a, TValue b);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, mapping every result to a value and then reducing those values to one final output
		/// </summary>
		/// <typeparam name="TM">Type of mapper</typeparam>
		/// <typeparam name="TR">Type of reducer</typeparam>
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
		public TOutput ExecuteMapReduce<TM, TR, TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
			TM q,
			TR r,
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
			where TM : IQueryMap16<TOutput, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
			where TR : IQueryReduce16<TOutput>
			where TOutput : unmanaged
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return initial;

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
				if (archetype.EntityCount == 0)
					continue;

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

		/* public int ExecuteParallel<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
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
            where T9 : IComponent
            where T10 : IComponent
            where T11 : IComponent
            where T12 : IComponent
            where T13 : IComponent
            where T14 : IComponent
            where T15 : IComponent
			where TQ : IQueryMap16<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
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
					var t9 = chunk.GetComponentArray<T9>(c9);
					var t10 = chunk.GetComponentArray<T10>(c10);
					var t11 = chunk.GetComponentArray<T11>(c11);
					var t12 = chunk.GetComponentArray<T12>(c12);
					var t13 = chunk.GetComponentArray<T13>(c13);
					var t14 = chunk.GetComponentArray<T14>(c14);
					var t15 = chunk.GetComponentArray<T15>(c15);

					Parallel.For(0, numBatches, b =>
                    {
						var start = b * batchSize;
						var end = Math.Min(start + batchSize, entityCount);

						for (var i = start; i < end; i++)
						{
							var entities = chunk.Entities;
							q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i], ref t13[i], ref t14[i], ref t15[i]);
						}
					});
				}
			}

			return count;
		} */
	}
}


