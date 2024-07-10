

using Myriad.ECS.Queries;
using Myriad.ECS.IDs;
using Myriad.ECS.Threading;
using Myriad.ECS.Allocations;
using System.Buffers;

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

			#region Parallel Work Loop Setup
			// Borrow a counter which will be used to keep track of all in-progress work
			using var workCounterRental = Pool<CountdownEventContainer>.Rent();
			var workCounter = workCounterRental.Value.Event;
			workCounter.Reset(1);

			// Create parallel workers
			var processors = Math.Max(4, Math.Min(64, Environment.ProcessorCount) - 3);
			var workersArr = ArrayPool<ParallelQueryWorker<ChunkWorkItem1<TQ, T0>>?>.Shared.Rent(processors);
			var workers = workersArr.AsMemory(0, processors);
			for (var i = 0; i < workers.Length; i++)
			{
				workersArr[i] = Pool<ParallelQueryWorker<ChunkWorkItem1<TQ, T0>>>.Get();
				workersArr[i]!.Configure(workersArr, workCounter);
			}

			// Start the workers. Even though there is no work they cannot exit yet because
			// the counter was reset to 1 above.
			foreach (var item in workersArr)
			{
				if (item != null)
				{
#if NET8_0_OR_GREATER
					ThreadPool.UnsafeQueueUserWorkItem(item, true);
#else
					ThreadPool.UnsafeQueueUserWorkItem(item.Execute, true);
#endif
				}
			}

			// Enqueue work to this index
			var workerEnqueueIdx = 0;
			#endregion

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
				var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				count += archetype.EntityCount;

				// Inrement work counter for all of the chunk we're about to enqueue
				workCounter.AddCount(archetype.Chunks.Count);

				using var enumerator = archetype.GetChunkEnumerator();
				while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current!;
					var item = new ChunkWorkItem1<TQ, T0>(chunk, q);

						#region Parallel Work Loop Add To Queue
						// Add work to a worker
						workersArr[workerEnqueueIdx]!.Enqueue(item);

						workerEnqueueIdx++;
						if (workerEnqueueIdx >= workers.Length)
							workerEnqueueIdx = 0;
						#endregion
				}
			}

			#region Parallel Work Loop Teardown
			// Collection of exceptions thrown in any work
			List<Exception>? exceptions = null;

			// Clear the 1 that was added at the start (when the counter was reset)
			workCounter.Signal();

			// Try to steal some work to do on this thread
			while (!workCounter.IsSet)
			{
				for (var i = 0; i < workers.Length && !workCounter.IsSet; i++)
				{
					if (workersArr[i]!.Steal(out var work))
					{
						workCounter.Signal();
						try
						{
							work.Execute();
						}
						catch (Exception ex)
						{
							exceptions ??= [ ];
							exceptions.Add(ex);
							break;
						}
					}
				}
			}

			// Wait for work to finish
			workCounter.Wait();

			// Recycle workers
			for (var i = 0; i < workers.Length; i++)
			{
				var worker = workersArr[i]!;
				worker.FinishEvent.Wait();
				worker.Clear(ref exceptions);
				Pool.Return(worker);
			}
			Array.Clear(workersArr, 0, workersArr.Length);
			ArrayPool<ParallelQueryWorker<ChunkWorkItem1<TQ, T0>>>.Shared.Return(workersArr!);

			// Throw collected exceptions
			if (exceptions is { Count: > 0 })
				throw new AggregateException(exceptions);
			#endregion

			return count;
		}

		private readonly struct ChunkWorkItem1<TQ, T0>
			: IWorkItem
			where T0 : IComponent
			where TQ : IChunkQuery1<T0>
		{
			private readonly TQ _q;
			private readonly Chunks.Chunk _chunk;

			public ChunkWorkItem1(Chunks.Chunk chunk, TQ q)
			{
				_chunk = chunk;
				_q = q;
			}

			public void Execute()
			{
				_q.Execute(
					_chunk.Entities
					, _chunk.GetSpan<T0>()
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

			#region Parallel Work Loop Setup
			// Borrow a counter which will be used to keep track of all in-progress work
			using var workCounterRental = Pool<CountdownEventContainer>.Rent();
			var workCounter = workCounterRental.Value.Event;
			workCounter.Reset(1);

			// Create parallel workers
			var processors = Math.Max(4, Math.Min(64, Environment.ProcessorCount) - 3);
			var workersArr = ArrayPool<ParallelQueryWorker<ChunkWorkItem2<TQ, T0, T1>>?>.Shared.Rent(processors);
			var workers = workersArr.AsMemory(0, processors);
			for (var i = 0; i < workers.Length; i++)
			{
				workersArr[i] = Pool<ParallelQueryWorker<ChunkWorkItem2<TQ, T0, T1>>>.Get();
				workersArr[i]!.Configure(workersArr, workCounter);
			}

			// Start the workers. Even though there is no work they cannot exit yet because
			// the counter was reset to 1 above.
			foreach (var item in workersArr)
			{
				if (item != null)
				{
#if NET8_0_OR_GREATER
					ThreadPool.UnsafeQueueUserWorkItem(item, true);
#else
					ThreadPool.UnsafeQueueUserWorkItem(item.Execute, true);
#endif
				}
			}

			// Enqueue work to this index
			var workerEnqueueIdx = 0;
			#endregion

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
				var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				count += archetype.EntityCount;

				// Inrement work counter for all of the chunk we're about to enqueue
				workCounter.AddCount(archetype.Chunks.Count);

				using var enumerator = archetype.GetChunkEnumerator();
				while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current!;
					var item = new ChunkWorkItem2<TQ, T0, T1>(chunk, q);

						#region Parallel Work Loop Add To Queue
						// Add work to a worker
						workersArr[workerEnqueueIdx]!.Enqueue(item);

						workerEnqueueIdx++;
						if (workerEnqueueIdx >= workers.Length)
							workerEnqueueIdx = 0;
						#endregion
				}
			}

			#region Parallel Work Loop Teardown
			// Collection of exceptions thrown in any work
			List<Exception>? exceptions = null;

			// Clear the 1 that was added at the start (when the counter was reset)
			workCounter.Signal();

			// Try to steal some work to do on this thread
			while (!workCounter.IsSet)
			{
				for (var i = 0; i < workers.Length && !workCounter.IsSet; i++)
				{
					if (workersArr[i]!.Steal(out var work))
					{
						workCounter.Signal();
						try
						{
							work.Execute();
						}
						catch (Exception ex)
						{
							exceptions ??= [ ];
							exceptions.Add(ex);
							break;
						}
					}
				}
			}

			// Wait for work to finish
			workCounter.Wait();

			// Recycle workers
			for (var i = 0; i < workers.Length; i++)
			{
				var worker = workersArr[i]!;
				worker.FinishEvent.Wait();
				worker.Clear(ref exceptions);
				Pool.Return(worker);
			}
			Array.Clear(workersArr, 0, workersArr.Length);
			ArrayPool<ParallelQueryWorker<ChunkWorkItem2<TQ, T0, T1>>>.Shared.Return(workersArr!);

			// Throw collected exceptions
			if (exceptions is { Count: > 0 })
				throw new AggregateException(exceptions);
			#endregion

			return count;
		}

		private readonly struct ChunkWorkItem2<TQ, T0, T1>
			: IWorkItem
			where T0 : IComponent
            where T1 : IComponent
			where TQ : IChunkQuery2<T0, T1>
		{
			private readonly TQ _q;
			private readonly Chunks.Chunk _chunk;

			public ChunkWorkItem2(Chunks.Chunk chunk, TQ q)
			{
				_chunk = chunk;
				_q = q;
			}

			public void Execute()
			{
				_q.Execute(
					_chunk.Entities
					, _chunk.GetSpan<T0>()
					, _chunk.GetSpan<T1>()
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

			#region Parallel Work Loop Setup
			// Borrow a counter which will be used to keep track of all in-progress work
			using var workCounterRental = Pool<CountdownEventContainer>.Rent();
			var workCounter = workCounterRental.Value.Event;
			workCounter.Reset(1);

			// Create parallel workers
			var processors = Math.Max(4, Math.Min(64, Environment.ProcessorCount) - 3);
			var workersArr = ArrayPool<ParallelQueryWorker<ChunkWorkItem3<TQ, T0, T1, T2>>?>.Shared.Rent(processors);
			var workers = workersArr.AsMemory(0, processors);
			for (var i = 0; i < workers.Length; i++)
			{
				workersArr[i] = Pool<ParallelQueryWorker<ChunkWorkItem3<TQ, T0, T1, T2>>>.Get();
				workersArr[i]!.Configure(workersArr, workCounter);
			}

			// Start the workers. Even though there is no work they cannot exit yet because
			// the counter was reset to 1 above.
			foreach (var item in workersArr)
			{
				if (item != null)
				{
#if NET8_0_OR_GREATER
					ThreadPool.UnsafeQueueUserWorkItem(item, true);
#else
					ThreadPool.UnsafeQueueUserWorkItem(item.Execute, true);
#endif
				}
			}

			// Enqueue work to this index
			var workerEnqueueIdx = 0;
			#endregion

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
				var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				count += archetype.EntityCount;

				// Inrement work counter for all of the chunk we're about to enqueue
				workCounter.AddCount(archetype.Chunks.Count);

				using var enumerator = archetype.GetChunkEnumerator();
				while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current!;
					var item = new ChunkWorkItem3<TQ, T0, T1, T2>(chunk, q);

						#region Parallel Work Loop Add To Queue
						// Add work to a worker
						workersArr[workerEnqueueIdx]!.Enqueue(item);

						workerEnqueueIdx++;
						if (workerEnqueueIdx >= workers.Length)
							workerEnqueueIdx = 0;
						#endregion
				}
			}

			#region Parallel Work Loop Teardown
			// Collection of exceptions thrown in any work
			List<Exception>? exceptions = null;

			// Clear the 1 that was added at the start (when the counter was reset)
			workCounter.Signal();

			// Try to steal some work to do on this thread
			while (!workCounter.IsSet)
			{
				for (var i = 0; i < workers.Length && !workCounter.IsSet; i++)
				{
					if (workersArr[i]!.Steal(out var work))
					{
						workCounter.Signal();
						try
						{
							work.Execute();
						}
						catch (Exception ex)
						{
							exceptions ??= [ ];
							exceptions.Add(ex);
							break;
						}
					}
				}
			}

			// Wait for work to finish
			workCounter.Wait();

			// Recycle workers
			for (var i = 0; i < workers.Length; i++)
			{
				var worker = workersArr[i]!;
				worker.FinishEvent.Wait();
				worker.Clear(ref exceptions);
				Pool.Return(worker);
			}
			Array.Clear(workersArr, 0, workersArr.Length);
			ArrayPool<ParallelQueryWorker<ChunkWorkItem3<TQ, T0, T1, T2>>>.Shared.Return(workersArr!);

			// Throw collected exceptions
			if (exceptions is { Count: > 0 })
				throw new AggregateException(exceptions);
			#endregion

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
			private readonly Chunks.Chunk _chunk;

			public ChunkWorkItem3(Chunks.Chunk chunk, TQ q)
			{
				_chunk = chunk;
				_q = q;
			}

			public void Execute()
			{
				_q.Execute(
					_chunk.Entities
					, _chunk.GetSpan<T0>()
					, _chunk.GetSpan<T1>()
					, _chunk.GetSpan<T2>()
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

			#region Parallel Work Loop Setup
			// Borrow a counter which will be used to keep track of all in-progress work
			using var workCounterRental = Pool<CountdownEventContainer>.Rent();
			var workCounter = workCounterRental.Value.Event;
			workCounter.Reset(1);

			// Create parallel workers
			var processors = Math.Max(4, Math.Min(64, Environment.ProcessorCount) - 3);
			var workersArr = ArrayPool<ParallelQueryWorker<ChunkWorkItem4<TQ, T0, T1, T2, T3>>?>.Shared.Rent(processors);
			var workers = workersArr.AsMemory(0, processors);
			for (var i = 0; i < workers.Length; i++)
			{
				workersArr[i] = Pool<ParallelQueryWorker<ChunkWorkItem4<TQ, T0, T1, T2, T3>>>.Get();
				workersArr[i]!.Configure(workersArr, workCounter);
			}

			// Start the workers. Even though there is no work they cannot exit yet because
			// the counter was reset to 1 above.
			foreach (var item in workersArr)
			{
				if (item != null)
				{
#if NET8_0_OR_GREATER
					ThreadPool.UnsafeQueueUserWorkItem(item, true);
#else
					ThreadPool.UnsafeQueueUserWorkItem(item.Execute, true);
#endif
				}
			}

			// Enqueue work to this index
			var workerEnqueueIdx = 0;
			#endregion

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
				var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				count += archetype.EntityCount;

				// Inrement work counter for all of the chunk we're about to enqueue
				workCounter.AddCount(archetype.Chunks.Count);

				using var enumerator = archetype.GetChunkEnumerator();
				while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current!;
					var item = new ChunkWorkItem4<TQ, T0, T1, T2, T3>(chunk, q);

						#region Parallel Work Loop Add To Queue
						// Add work to a worker
						workersArr[workerEnqueueIdx]!.Enqueue(item);

						workerEnqueueIdx++;
						if (workerEnqueueIdx >= workers.Length)
							workerEnqueueIdx = 0;
						#endregion
				}
			}

			#region Parallel Work Loop Teardown
			// Collection of exceptions thrown in any work
			List<Exception>? exceptions = null;

			// Clear the 1 that was added at the start (when the counter was reset)
			workCounter.Signal();

			// Try to steal some work to do on this thread
			while (!workCounter.IsSet)
			{
				for (var i = 0; i < workers.Length && !workCounter.IsSet; i++)
				{
					if (workersArr[i]!.Steal(out var work))
					{
						workCounter.Signal();
						try
						{
							work.Execute();
						}
						catch (Exception ex)
						{
							exceptions ??= [ ];
							exceptions.Add(ex);
							break;
						}
					}
				}
			}

			// Wait for work to finish
			workCounter.Wait();

			// Recycle workers
			for (var i = 0; i < workers.Length; i++)
			{
				var worker = workersArr[i]!;
				worker.FinishEvent.Wait();
				worker.Clear(ref exceptions);
				Pool.Return(worker);
			}
			Array.Clear(workersArr, 0, workersArr.Length);
			ArrayPool<ParallelQueryWorker<ChunkWorkItem4<TQ, T0, T1, T2, T3>>>.Shared.Return(workersArr!);

			// Throw collected exceptions
			if (exceptions is { Count: > 0 })
				throw new AggregateException(exceptions);
			#endregion

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
			private readonly Chunks.Chunk _chunk;

			public ChunkWorkItem4(Chunks.Chunk chunk, TQ q)
			{
				_chunk = chunk;
				_q = q;
			}

			public void Execute()
			{
				_q.Execute(
					_chunk.Entities
					, _chunk.GetSpan<T0>()
					, _chunk.GetSpan<T1>()
					, _chunk.GetSpan<T2>()
					, _chunk.GetSpan<T3>()
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

			#region Parallel Work Loop Setup
			// Borrow a counter which will be used to keep track of all in-progress work
			using var workCounterRental = Pool<CountdownEventContainer>.Rent();
			var workCounter = workCounterRental.Value.Event;
			workCounter.Reset(1);

			// Create parallel workers
			var processors = Math.Max(4, Math.Min(64, Environment.ProcessorCount) - 3);
			var workersArr = ArrayPool<ParallelQueryWorker<ChunkWorkItem5<TQ, T0, T1, T2, T3, T4>>?>.Shared.Rent(processors);
			var workers = workersArr.AsMemory(0, processors);
			for (var i = 0; i < workers.Length; i++)
			{
				workersArr[i] = Pool<ParallelQueryWorker<ChunkWorkItem5<TQ, T0, T1, T2, T3, T4>>>.Get();
				workersArr[i]!.Configure(workersArr, workCounter);
			}

			// Start the workers. Even though there is no work they cannot exit yet because
			// the counter was reset to 1 above.
			foreach (var item in workersArr)
			{
				if (item != null)
				{
#if NET8_0_OR_GREATER
					ThreadPool.UnsafeQueueUserWorkItem(item, true);
#else
					ThreadPool.UnsafeQueueUserWorkItem(item.Execute, true);
#endif
				}
			}

			// Enqueue work to this index
			var workerEnqueueIdx = 0;
			#endregion

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
				var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				count += archetype.EntityCount;

				// Inrement work counter for all of the chunk we're about to enqueue
				workCounter.AddCount(archetype.Chunks.Count);

				using var enumerator = archetype.GetChunkEnumerator();
				while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current!;
					var item = new ChunkWorkItem5<TQ, T0, T1, T2, T3, T4>(chunk, q);

						#region Parallel Work Loop Add To Queue
						// Add work to a worker
						workersArr[workerEnqueueIdx]!.Enqueue(item);

						workerEnqueueIdx++;
						if (workerEnqueueIdx >= workers.Length)
							workerEnqueueIdx = 0;
						#endregion
				}
			}

			#region Parallel Work Loop Teardown
			// Collection of exceptions thrown in any work
			List<Exception>? exceptions = null;

			// Clear the 1 that was added at the start (when the counter was reset)
			workCounter.Signal();

			// Try to steal some work to do on this thread
			while (!workCounter.IsSet)
			{
				for (var i = 0; i < workers.Length && !workCounter.IsSet; i++)
				{
					if (workersArr[i]!.Steal(out var work))
					{
						workCounter.Signal();
						try
						{
							work.Execute();
						}
						catch (Exception ex)
						{
							exceptions ??= [ ];
							exceptions.Add(ex);
							break;
						}
					}
				}
			}

			// Wait for work to finish
			workCounter.Wait();

			// Recycle workers
			for (var i = 0; i < workers.Length; i++)
			{
				var worker = workersArr[i]!;
				worker.FinishEvent.Wait();
				worker.Clear(ref exceptions);
				Pool.Return(worker);
			}
			Array.Clear(workersArr, 0, workersArr.Length);
			ArrayPool<ParallelQueryWorker<ChunkWorkItem5<TQ, T0, T1, T2, T3, T4>>>.Shared.Return(workersArr!);

			// Throw collected exceptions
			if (exceptions is { Count: > 0 })
				throw new AggregateException(exceptions);
			#endregion

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
			private readonly Chunks.Chunk _chunk;

			public ChunkWorkItem5(Chunks.Chunk chunk, TQ q)
			{
				_chunk = chunk;
				_q = q;
			}

			public void Execute()
			{
				_q.Execute(
					_chunk.Entities
					, _chunk.GetSpan<T0>()
					, _chunk.GetSpan<T1>()
					, _chunk.GetSpan<T2>()
					, _chunk.GetSpan<T3>()
					, _chunk.GetSpan<T4>()
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

			#region Parallel Work Loop Setup
			// Borrow a counter which will be used to keep track of all in-progress work
			using var workCounterRental = Pool<CountdownEventContainer>.Rent();
			var workCounter = workCounterRental.Value.Event;
			workCounter.Reset(1);

			// Create parallel workers
			var processors = Math.Max(4, Math.Min(64, Environment.ProcessorCount) - 3);
			var workersArr = ArrayPool<ParallelQueryWorker<ChunkWorkItem6<TQ, T0, T1, T2, T3, T4, T5>>?>.Shared.Rent(processors);
			var workers = workersArr.AsMemory(0, processors);
			for (var i = 0; i < workers.Length; i++)
			{
				workersArr[i] = Pool<ParallelQueryWorker<ChunkWorkItem6<TQ, T0, T1, T2, T3, T4, T5>>>.Get();
				workersArr[i]!.Configure(workersArr, workCounter);
			}

			// Start the workers. Even though there is no work they cannot exit yet because
			// the counter was reset to 1 above.
			foreach (var item in workersArr)
			{
				if (item != null)
				{
#if NET8_0_OR_GREATER
					ThreadPool.UnsafeQueueUserWorkItem(item, true);
#else
					ThreadPool.UnsafeQueueUserWorkItem(item.Execute, true);
#endif
				}
			}

			// Enqueue work to this index
			var workerEnqueueIdx = 0;
			#endregion

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
				var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				count += archetype.EntityCount;

				// Inrement work counter for all of the chunk we're about to enqueue
				workCounter.AddCount(archetype.Chunks.Count);

				using var enumerator = archetype.GetChunkEnumerator();
				while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current!;
					var item = new ChunkWorkItem6<TQ, T0, T1, T2, T3, T4, T5>(chunk, q);

						#region Parallel Work Loop Add To Queue
						// Add work to a worker
						workersArr[workerEnqueueIdx]!.Enqueue(item);

						workerEnqueueIdx++;
						if (workerEnqueueIdx >= workers.Length)
							workerEnqueueIdx = 0;
						#endregion
				}
			}

			#region Parallel Work Loop Teardown
			// Collection of exceptions thrown in any work
			List<Exception>? exceptions = null;

			// Clear the 1 that was added at the start (when the counter was reset)
			workCounter.Signal();

			// Try to steal some work to do on this thread
			while (!workCounter.IsSet)
			{
				for (var i = 0; i < workers.Length && !workCounter.IsSet; i++)
				{
					if (workersArr[i]!.Steal(out var work))
					{
						workCounter.Signal();
						try
						{
							work.Execute();
						}
						catch (Exception ex)
						{
							exceptions ??= [ ];
							exceptions.Add(ex);
							break;
						}
					}
				}
			}

			// Wait for work to finish
			workCounter.Wait();

			// Recycle workers
			for (var i = 0; i < workers.Length; i++)
			{
				var worker = workersArr[i]!;
				worker.FinishEvent.Wait();
				worker.Clear(ref exceptions);
				Pool.Return(worker);
			}
			Array.Clear(workersArr, 0, workersArr.Length);
			ArrayPool<ParallelQueryWorker<ChunkWorkItem6<TQ, T0, T1, T2, T3, T4, T5>>>.Shared.Return(workersArr!);

			// Throw collected exceptions
			if (exceptions is { Count: > 0 })
				throw new AggregateException(exceptions);
			#endregion

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
			private readonly Chunks.Chunk _chunk;

			public ChunkWorkItem6(Chunks.Chunk chunk, TQ q)
			{
				_chunk = chunk;
				_q = q;
			}

			public void Execute()
			{
				_q.Execute(
					_chunk.Entities
					, _chunk.GetSpan<T0>()
					, _chunk.GetSpan<T1>()
					, _chunk.GetSpan<T2>()
					, _chunk.GetSpan<T3>()
					, _chunk.GetSpan<T4>()
					, _chunk.GetSpan<T5>()
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

			#region Parallel Work Loop Setup
			// Borrow a counter which will be used to keep track of all in-progress work
			using var workCounterRental = Pool<CountdownEventContainer>.Rent();
			var workCounter = workCounterRental.Value.Event;
			workCounter.Reset(1);

			// Create parallel workers
			var processors = Math.Max(4, Math.Min(64, Environment.ProcessorCount) - 3);
			var workersArr = ArrayPool<ParallelQueryWorker<ChunkWorkItem7<TQ, T0, T1, T2, T3, T4, T5, T6>>?>.Shared.Rent(processors);
			var workers = workersArr.AsMemory(0, processors);
			for (var i = 0; i < workers.Length; i++)
			{
				workersArr[i] = Pool<ParallelQueryWorker<ChunkWorkItem7<TQ, T0, T1, T2, T3, T4, T5, T6>>>.Get();
				workersArr[i]!.Configure(workersArr, workCounter);
			}

			// Start the workers. Even though there is no work they cannot exit yet because
			// the counter was reset to 1 above.
			foreach (var item in workersArr)
			{
				if (item != null)
				{
#if NET8_0_OR_GREATER
					ThreadPool.UnsafeQueueUserWorkItem(item, true);
#else
					ThreadPool.UnsafeQueueUserWorkItem(item.Execute, true);
#endif
				}
			}

			// Enqueue work to this index
			var workerEnqueueIdx = 0;
			#endregion

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
				var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				count += archetype.EntityCount;

				// Inrement work counter for all of the chunk we're about to enqueue
				workCounter.AddCount(archetype.Chunks.Count);

				using var enumerator = archetype.GetChunkEnumerator();
				while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current!;
					var item = new ChunkWorkItem7<TQ, T0, T1, T2, T3, T4, T5, T6>(chunk, q);

						#region Parallel Work Loop Add To Queue
						// Add work to a worker
						workersArr[workerEnqueueIdx]!.Enqueue(item);

						workerEnqueueIdx++;
						if (workerEnqueueIdx >= workers.Length)
							workerEnqueueIdx = 0;
						#endregion
				}
			}

			#region Parallel Work Loop Teardown
			// Collection of exceptions thrown in any work
			List<Exception>? exceptions = null;

			// Clear the 1 that was added at the start (when the counter was reset)
			workCounter.Signal();

			// Try to steal some work to do on this thread
			while (!workCounter.IsSet)
			{
				for (var i = 0; i < workers.Length && !workCounter.IsSet; i++)
				{
					if (workersArr[i]!.Steal(out var work))
					{
						workCounter.Signal();
						try
						{
							work.Execute();
						}
						catch (Exception ex)
						{
							exceptions ??= [ ];
							exceptions.Add(ex);
							break;
						}
					}
				}
			}

			// Wait for work to finish
			workCounter.Wait();

			// Recycle workers
			for (var i = 0; i < workers.Length; i++)
			{
				var worker = workersArr[i]!;
				worker.FinishEvent.Wait();
				worker.Clear(ref exceptions);
				Pool.Return(worker);
			}
			Array.Clear(workersArr, 0, workersArr.Length);
			ArrayPool<ParallelQueryWorker<ChunkWorkItem7<TQ, T0, T1, T2, T3, T4, T5, T6>>>.Shared.Return(workersArr!);

			// Throw collected exceptions
			if (exceptions is { Count: > 0 })
				throw new AggregateException(exceptions);
			#endregion

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
			private readonly Chunks.Chunk _chunk;

			public ChunkWorkItem7(Chunks.Chunk chunk, TQ q)
			{
				_chunk = chunk;
				_q = q;
			}

			public void Execute()
			{
				_q.Execute(
					_chunk.Entities
					, _chunk.GetSpan<T0>()
					, _chunk.GetSpan<T1>()
					, _chunk.GetSpan<T2>()
					, _chunk.GetSpan<T3>()
					, _chunk.GetSpan<T4>()
					, _chunk.GetSpan<T5>()
					, _chunk.GetSpan<T6>()
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

			#region Parallel Work Loop Setup
			// Borrow a counter which will be used to keep track of all in-progress work
			using var workCounterRental = Pool<CountdownEventContainer>.Rent();
			var workCounter = workCounterRental.Value.Event;
			workCounter.Reset(1);

			// Create parallel workers
			var processors = Math.Max(4, Math.Min(64, Environment.ProcessorCount) - 3);
			var workersArr = ArrayPool<ParallelQueryWorker<ChunkWorkItem8<TQ, T0, T1, T2, T3, T4, T5, T6, T7>>?>.Shared.Rent(processors);
			var workers = workersArr.AsMemory(0, processors);
			for (var i = 0; i < workers.Length; i++)
			{
				workersArr[i] = Pool<ParallelQueryWorker<ChunkWorkItem8<TQ, T0, T1, T2, T3, T4, T5, T6, T7>>>.Get();
				workersArr[i]!.Configure(workersArr, workCounter);
			}

			// Start the workers. Even though there is no work they cannot exit yet because
			// the counter was reset to 1 above.
			foreach (var item in workersArr)
			{
				if (item != null)
				{
#if NET8_0_OR_GREATER
					ThreadPool.UnsafeQueueUserWorkItem(item, true);
#else
					ThreadPool.UnsafeQueueUserWorkItem(item.Execute, true);
#endif
				}
			}

			// Enqueue work to this index
			var workerEnqueueIdx = 0;
			#endregion

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
				var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				count += archetype.EntityCount;

				// Inrement work counter for all of the chunk we're about to enqueue
				workCounter.AddCount(archetype.Chunks.Count);

				using var enumerator = archetype.GetChunkEnumerator();
				while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current!;
					var item = new ChunkWorkItem8<TQ, T0, T1, T2, T3, T4, T5, T6, T7>(chunk, q);

						#region Parallel Work Loop Add To Queue
						// Add work to a worker
						workersArr[workerEnqueueIdx]!.Enqueue(item);

						workerEnqueueIdx++;
						if (workerEnqueueIdx >= workers.Length)
							workerEnqueueIdx = 0;
						#endregion
				}
			}

			#region Parallel Work Loop Teardown
			// Collection of exceptions thrown in any work
			List<Exception>? exceptions = null;

			// Clear the 1 that was added at the start (when the counter was reset)
			workCounter.Signal();

			// Try to steal some work to do on this thread
			while (!workCounter.IsSet)
			{
				for (var i = 0; i < workers.Length && !workCounter.IsSet; i++)
				{
					if (workersArr[i]!.Steal(out var work))
					{
						workCounter.Signal();
						try
						{
							work.Execute();
						}
						catch (Exception ex)
						{
							exceptions ??= [ ];
							exceptions.Add(ex);
							break;
						}
					}
				}
			}

			// Wait for work to finish
			workCounter.Wait();

			// Recycle workers
			for (var i = 0; i < workers.Length; i++)
			{
				var worker = workersArr[i]!;
				worker.FinishEvent.Wait();
				worker.Clear(ref exceptions);
				Pool.Return(worker);
			}
			Array.Clear(workersArr, 0, workersArr.Length);
			ArrayPool<ParallelQueryWorker<ChunkWorkItem8<TQ, T0, T1, T2, T3, T4, T5, T6, T7>>>.Shared.Return(workersArr!);

			// Throw collected exceptions
			if (exceptions is { Count: > 0 })
				throw new AggregateException(exceptions);
			#endregion

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
			private readonly Chunks.Chunk _chunk;

			public ChunkWorkItem8(Chunks.Chunk chunk, TQ q)
			{
				_chunk = chunk;
				_q = q;
			}

			public void Execute()
			{
				_q.Execute(
					_chunk.Entities
					, _chunk.GetSpan<T0>()
					, _chunk.GetSpan<T1>()
					, _chunk.GetSpan<T2>()
					, _chunk.GetSpan<T3>()
					, _chunk.GetSpan<T4>()
					, _chunk.GetSpan<T5>()
					, _chunk.GetSpan<T6>()
					, _chunk.GetSpan<T7>()
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

			#region Parallel Work Loop Setup
			// Borrow a counter which will be used to keep track of all in-progress work
			using var workCounterRental = Pool<CountdownEventContainer>.Rent();
			var workCounter = workCounterRental.Value.Event;
			workCounter.Reset(1);

			// Create parallel workers
			var processors = Math.Max(4, Math.Min(64, Environment.ProcessorCount) - 3);
			var workersArr = ArrayPool<ParallelQueryWorker<ChunkWorkItem9<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8>>?>.Shared.Rent(processors);
			var workers = workersArr.AsMemory(0, processors);
			for (var i = 0; i < workers.Length; i++)
			{
				workersArr[i] = Pool<ParallelQueryWorker<ChunkWorkItem9<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8>>>.Get();
				workersArr[i]!.Configure(workersArr, workCounter);
			}

			// Start the workers. Even though there is no work they cannot exit yet because
			// the counter was reset to 1 above.
			foreach (var item in workersArr)
			{
				if (item != null)
				{
#if NET8_0_OR_GREATER
					ThreadPool.UnsafeQueueUserWorkItem(item, true);
#else
					ThreadPool.UnsafeQueueUserWorkItem(item.Execute, true);
#endif
				}
			}

			// Enqueue work to this index
			var workerEnqueueIdx = 0;
			#endregion

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
				var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				count += archetype.EntityCount;

				// Inrement work counter for all of the chunk we're about to enqueue
				workCounter.AddCount(archetype.Chunks.Count);

				using var enumerator = archetype.GetChunkEnumerator();
				while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current!;
					var item = new ChunkWorkItem9<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8>(chunk, q);

						#region Parallel Work Loop Add To Queue
						// Add work to a worker
						workersArr[workerEnqueueIdx]!.Enqueue(item);

						workerEnqueueIdx++;
						if (workerEnqueueIdx >= workers.Length)
							workerEnqueueIdx = 0;
						#endregion
				}
			}

			#region Parallel Work Loop Teardown
			// Collection of exceptions thrown in any work
			List<Exception>? exceptions = null;

			// Clear the 1 that was added at the start (when the counter was reset)
			workCounter.Signal();

			// Try to steal some work to do on this thread
			while (!workCounter.IsSet)
			{
				for (var i = 0; i < workers.Length && !workCounter.IsSet; i++)
				{
					if (workersArr[i]!.Steal(out var work))
					{
						workCounter.Signal();
						try
						{
							work.Execute();
						}
						catch (Exception ex)
						{
							exceptions ??= [ ];
							exceptions.Add(ex);
							break;
						}
					}
				}
			}

			// Wait for work to finish
			workCounter.Wait();

			// Recycle workers
			for (var i = 0; i < workers.Length; i++)
			{
				var worker = workersArr[i]!;
				worker.FinishEvent.Wait();
				worker.Clear(ref exceptions);
				Pool.Return(worker);
			}
			Array.Clear(workersArr, 0, workersArr.Length);
			ArrayPool<ParallelQueryWorker<ChunkWorkItem9<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8>>>.Shared.Return(workersArr!);

			// Throw collected exceptions
			if (exceptions is { Count: > 0 })
				throw new AggregateException(exceptions);
			#endregion

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
			private readonly Chunks.Chunk _chunk;

			public ChunkWorkItem9(Chunks.Chunk chunk, TQ q)
			{
				_chunk = chunk;
				_q = q;
			}

			public void Execute()
			{
				_q.Execute(
					_chunk.Entities
					, _chunk.GetSpan<T0>()
					, _chunk.GetSpan<T1>()
					, _chunk.GetSpan<T2>()
					, _chunk.GetSpan<T3>()
					, _chunk.GetSpan<T4>()
					, _chunk.GetSpan<T5>()
					, _chunk.GetSpan<T6>()
					, _chunk.GetSpan<T7>()
					, _chunk.GetSpan<T8>()
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

			#region Parallel Work Loop Setup
			// Borrow a counter which will be used to keep track of all in-progress work
			using var workCounterRental = Pool<CountdownEventContainer>.Rent();
			var workCounter = workCounterRental.Value.Event;
			workCounter.Reset(1);

			// Create parallel workers
			var processors = Math.Max(4, Math.Min(64, Environment.ProcessorCount) - 3);
			var workersArr = ArrayPool<ParallelQueryWorker<ChunkWorkItem10<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>>?>.Shared.Rent(processors);
			var workers = workersArr.AsMemory(0, processors);
			for (var i = 0; i < workers.Length; i++)
			{
				workersArr[i] = Pool<ParallelQueryWorker<ChunkWorkItem10<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>>>.Get();
				workersArr[i]!.Configure(workersArr, workCounter);
			}

			// Start the workers. Even though there is no work they cannot exit yet because
			// the counter was reset to 1 above.
			foreach (var item in workersArr)
			{
				if (item != null)
				{
#if NET8_0_OR_GREATER
					ThreadPool.UnsafeQueueUserWorkItem(item, true);
#else
					ThreadPool.UnsafeQueueUserWorkItem(item.Execute, true);
#endif
				}
			}

			// Enqueue work to this index
			var workerEnqueueIdx = 0;
			#endregion

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
				var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				count += archetype.EntityCount;

				// Inrement work counter for all of the chunk we're about to enqueue
				workCounter.AddCount(archetype.Chunks.Count);

				using var enumerator = archetype.GetChunkEnumerator();
				while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current!;
					var item = new ChunkWorkItem10<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(chunk, q);

						#region Parallel Work Loop Add To Queue
						// Add work to a worker
						workersArr[workerEnqueueIdx]!.Enqueue(item);

						workerEnqueueIdx++;
						if (workerEnqueueIdx >= workers.Length)
							workerEnqueueIdx = 0;
						#endregion
				}
			}

			#region Parallel Work Loop Teardown
			// Collection of exceptions thrown in any work
			List<Exception>? exceptions = null;

			// Clear the 1 that was added at the start (when the counter was reset)
			workCounter.Signal();

			// Try to steal some work to do on this thread
			while (!workCounter.IsSet)
			{
				for (var i = 0; i < workers.Length && !workCounter.IsSet; i++)
				{
					if (workersArr[i]!.Steal(out var work))
					{
						workCounter.Signal();
						try
						{
							work.Execute();
						}
						catch (Exception ex)
						{
							exceptions ??= [ ];
							exceptions.Add(ex);
							break;
						}
					}
				}
			}

			// Wait for work to finish
			workCounter.Wait();

			// Recycle workers
			for (var i = 0; i < workers.Length; i++)
			{
				var worker = workersArr[i]!;
				worker.FinishEvent.Wait();
				worker.Clear(ref exceptions);
				Pool.Return(worker);
			}
			Array.Clear(workersArr, 0, workersArr.Length);
			ArrayPool<ParallelQueryWorker<ChunkWorkItem10<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>>>.Shared.Return(workersArr!);

			// Throw collected exceptions
			if (exceptions is { Count: > 0 })
				throw new AggregateException(exceptions);
			#endregion

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
			private readonly Chunks.Chunk _chunk;

			public ChunkWorkItem10(Chunks.Chunk chunk, TQ q)
			{
				_chunk = chunk;
				_q = q;
			}

			public void Execute()
			{
				_q.Execute(
					_chunk.Entities
					, _chunk.GetSpan<T0>()
					, _chunk.GetSpan<T1>()
					, _chunk.GetSpan<T2>()
					, _chunk.GetSpan<T3>()
					, _chunk.GetSpan<T4>()
					, _chunk.GetSpan<T5>()
					, _chunk.GetSpan<T6>()
					, _chunk.GetSpan<T7>()
					, _chunk.GetSpan<T8>()
					, _chunk.GetSpan<T9>()
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

			#region Parallel Work Loop Setup
			// Borrow a counter which will be used to keep track of all in-progress work
			using var workCounterRental = Pool<CountdownEventContainer>.Rent();
			var workCounter = workCounterRental.Value.Event;
			workCounter.Reset(1);

			// Create parallel workers
			var processors = Math.Max(4, Math.Min(64, Environment.ProcessorCount) - 3);
			var workersArr = ArrayPool<ParallelQueryWorker<ChunkWorkItem11<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>?>.Shared.Rent(processors);
			var workers = workersArr.AsMemory(0, processors);
			for (var i = 0; i < workers.Length; i++)
			{
				workersArr[i] = Pool<ParallelQueryWorker<ChunkWorkItem11<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>>.Get();
				workersArr[i]!.Configure(workersArr, workCounter);
			}

			// Start the workers. Even though there is no work they cannot exit yet because
			// the counter was reset to 1 above.
			foreach (var item in workersArr)
			{
				if (item != null)
				{
#if NET8_0_OR_GREATER
					ThreadPool.UnsafeQueueUserWorkItem(item, true);
#else
					ThreadPool.UnsafeQueueUserWorkItem(item.Execute, true);
#endif
				}
			}

			// Enqueue work to this index
			var workerEnqueueIdx = 0;
			#endregion

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
				var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				count += archetype.EntityCount;

				// Inrement work counter for all of the chunk we're about to enqueue
				workCounter.AddCount(archetype.Chunks.Count);

				using var enumerator = archetype.GetChunkEnumerator();
				while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current!;
					var item = new ChunkWorkItem11<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(chunk, q);

						#region Parallel Work Loop Add To Queue
						// Add work to a worker
						workersArr[workerEnqueueIdx]!.Enqueue(item);

						workerEnqueueIdx++;
						if (workerEnqueueIdx >= workers.Length)
							workerEnqueueIdx = 0;
						#endregion
				}
			}

			#region Parallel Work Loop Teardown
			// Collection of exceptions thrown in any work
			List<Exception>? exceptions = null;

			// Clear the 1 that was added at the start (when the counter was reset)
			workCounter.Signal();

			// Try to steal some work to do on this thread
			while (!workCounter.IsSet)
			{
				for (var i = 0; i < workers.Length && !workCounter.IsSet; i++)
				{
					if (workersArr[i]!.Steal(out var work))
					{
						workCounter.Signal();
						try
						{
							work.Execute();
						}
						catch (Exception ex)
						{
							exceptions ??= [ ];
							exceptions.Add(ex);
							break;
						}
					}
				}
			}

			// Wait for work to finish
			workCounter.Wait();

			// Recycle workers
			for (var i = 0; i < workers.Length; i++)
			{
				var worker = workersArr[i]!;
				worker.FinishEvent.Wait();
				worker.Clear(ref exceptions);
				Pool.Return(worker);
			}
			Array.Clear(workersArr, 0, workersArr.Length);
			ArrayPool<ParallelQueryWorker<ChunkWorkItem11<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>>.Shared.Return(workersArr!);

			// Throw collected exceptions
			if (exceptions is { Count: > 0 })
				throw new AggregateException(exceptions);
			#endregion

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
			private readonly Chunks.Chunk _chunk;

			public ChunkWorkItem11(Chunks.Chunk chunk, TQ q)
			{
				_chunk = chunk;
				_q = q;
			}

			public void Execute()
			{
				_q.Execute(
					_chunk.Entities
					, _chunk.GetSpan<T0>()
					, _chunk.GetSpan<T1>()
					, _chunk.GetSpan<T2>()
					, _chunk.GetSpan<T3>()
					, _chunk.GetSpan<T4>()
					, _chunk.GetSpan<T5>()
					, _chunk.GetSpan<T6>()
					, _chunk.GetSpan<T7>()
					, _chunk.GetSpan<T8>()
					, _chunk.GetSpan<T9>()
					, _chunk.GetSpan<T10>()
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

			#region Parallel Work Loop Setup
			// Borrow a counter which will be used to keep track of all in-progress work
			using var workCounterRental = Pool<CountdownEventContainer>.Rent();
			var workCounter = workCounterRental.Value.Event;
			workCounter.Reset(1);

			// Create parallel workers
			var processors = Math.Max(4, Math.Min(64, Environment.ProcessorCount) - 3);
			var workersArr = ArrayPool<ParallelQueryWorker<ChunkWorkItem12<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>?>.Shared.Rent(processors);
			var workers = workersArr.AsMemory(0, processors);
			for (var i = 0; i < workers.Length; i++)
			{
				workersArr[i] = Pool<ParallelQueryWorker<ChunkWorkItem12<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>>.Get();
				workersArr[i]!.Configure(workersArr, workCounter);
			}

			// Start the workers. Even though there is no work they cannot exit yet because
			// the counter was reset to 1 above.
			foreach (var item in workersArr)
			{
				if (item != null)
				{
#if NET8_0_OR_GREATER
					ThreadPool.UnsafeQueueUserWorkItem(item, true);
#else
					ThreadPool.UnsafeQueueUserWorkItem(item.Execute, true);
#endif
				}
			}

			// Enqueue work to this index
			var workerEnqueueIdx = 0;
			#endregion

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
				var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				count += archetype.EntityCount;

				// Inrement work counter for all of the chunk we're about to enqueue
				workCounter.AddCount(archetype.Chunks.Count);

				using var enumerator = archetype.GetChunkEnumerator();
				while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current!;
					var item = new ChunkWorkItem12<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(chunk, q);

						#region Parallel Work Loop Add To Queue
						// Add work to a worker
						workersArr[workerEnqueueIdx]!.Enqueue(item);

						workerEnqueueIdx++;
						if (workerEnqueueIdx >= workers.Length)
							workerEnqueueIdx = 0;
						#endregion
				}
			}

			#region Parallel Work Loop Teardown
			// Collection of exceptions thrown in any work
			List<Exception>? exceptions = null;

			// Clear the 1 that was added at the start (when the counter was reset)
			workCounter.Signal();

			// Try to steal some work to do on this thread
			while (!workCounter.IsSet)
			{
				for (var i = 0; i < workers.Length && !workCounter.IsSet; i++)
				{
					if (workersArr[i]!.Steal(out var work))
					{
						workCounter.Signal();
						try
						{
							work.Execute();
						}
						catch (Exception ex)
						{
							exceptions ??= [ ];
							exceptions.Add(ex);
							break;
						}
					}
				}
			}

			// Wait for work to finish
			workCounter.Wait();

			// Recycle workers
			for (var i = 0; i < workers.Length; i++)
			{
				var worker = workersArr[i]!;
				worker.FinishEvent.Wait();
				worker.Clear(ref exceptions);
				Pool.Return(worker);
			}
			Array.Clear(workersArr, 0, workersArr.Length);
			ArrayPool<ParallelQueryWorker<ChunkWorkItem12<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>>.Shared.Return(workersArr!);

			// Throw collected exceptions
			if (exceptions is { Count: > 0 })
				throw new AggregateException(exceptions);
			#endregion

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
			private readonly Chunks.Chunk _chunk;

			public ChunkWorkItem12(Chunks.Chunk chunk, TQ q)
			{
				_chunk = chunk;
				_q = q;
			}

			public void Execute()
			{
				_q.Execute(
					_chunk.Entities
					, _chunk.GetSpan<T0>()
					, _chunk.GetSpan<T1>()
					, _chunk.GetSpan<T2>()
					, _chunk.GetSpan<T3>()
					, _chunk.GetSpan<T4>()
					, _chunk.GetSpan<T5>()
					, _chunk.GetSpan<T6>()
					, _chunk.GetSpan<T7>()
					, _chunk.GetSpan<T8>()
					, _chunk.GetSpan<T9>()
					, _chunk.GetSpan<T10>()
					, _chunk.GetSpan<T11>()
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

			#region Parallel Work Loop Setup
			// Borrow a counter which will be used to keep track of all in-progress work
			using var workCounterRental = Pool<CountdownEventContainer>.Rent();
			var workCounter = workCounterRental.Value.Event;
			workCounter.Reset(1);

			// Create parallel workers
			var processors = Math.Max(4, Math.Min(64, Environment.ProcessorCount) - 3);
			var workersArr = ArrayPool<ParallelQueryWorker<ChunkWorkItem13<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>?>.Shared.Rent(processors);
			var workers = workersArr.AsMemory(0, processors);
			for (var i = 0; i < workers.Length; i++)
			{
				workersArr[i] = Pool<ParallelQueryWorker<ChunkWorkItem13<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>>.Get();
				workersArr[i]!.Configure(workersArr, workCounter);
			}

			// Start the workers. Even though there is no work they cannot exit yet because
			// the counter was reset to 1 above.
			foreach (var item in workersArr)
			{
				if (item != null)
				{
#if NET8_0_OR_GREATER
					ThreadPool.UnsafeQueueUserWorkItem(item, true);
#else
					ThreadPool.UnsafeQueueUserWorkItem(item.Execute, true);
#endif
				}
			}

			// Enqueue work to this index
			var workerEnqueueIdx = 0;
			#endregion

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
				var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				count += archetype.EntityCount;

				// Inrement work counter for all of the chunk we're about to enqueue
				workCounter.AddCount(archetype.Chunks.Count);

				using var enumerator = archetype.GetChunkEnumerator();
				while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current!;
					var item = new ChunkWorkItem13<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(chunk, q);

						#region Parallel Work Loop Add To Queue
						// Add work to a worker
						workersArr[workerEnqueueIdx]!.Enqueue(item);

						workerEnqueueIdx++;
						if (workerEnqueueIdx >= workers.Length)
							workerEnqueueIdx = 0;
						#endregion
				}
			}

			#region Parallel Work Loop Teardown
			// Collection of exceptions thrown in any work
			List<Exception>? exceptions = null;

			// Clear the 1 that was added at the start (when the counter was reset)
			workCounter.Signal();

			// Try to steal some work to do on this thread
			while (!workCounter.IsSet)
			{
				for (var i = 0; i < workers.Length && !workCounter.IsSet; i++)
				{
					if (workersArr[i]!.Steal(out var work))
					{
						workCounter.Signal();
						try
						{
							work.Execute();
						}
						catch (Exception ex)
						{
							exceptions ??= [ ];
							exceptions.Add(ex);
							break;
						}
					}
				}
			}

			// Wait for work to finish
			workCounter.Wait();

			// Recycle workers
			for (var i = 0; i < workers.Length; i++)
			{
				var worker = workersArr[i]!;
				worker.FinishEvent.Wait();
				worker.Clear(ref exceptions);
				Pool.Return(worker);
			}
			Array.Clear(workersArr, 0, workersArr.Length);
			ArrayPool<ParallelQueryWorker<ChunkWorkItem13<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>>.Shared.Return(workersArr!);

			// Throw collected exceptions
			if (exceptions is { Count: > 0 })
				throw new AggregateException(exceptions);
			#endregion

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
			private readonly Chunks.Chunk _chunk;

			public ChunkWorkItem13(Chunks.Chunk chunk, TQ q)
			{
				_chunk = chunk;
				_q = q;
			}

			public void Execute()
			{
				_q.Execute(
					_chunk.Entities
					, _chunk.GetSpan<T0>()
					, _chunk.GetSpan<T1>()
					, _chunk.GetSpan<T2>()
					, _chunk.GetSpan<T3>()
					, _chunk.GetSpan<T4>()
					, _chunk.GetSpan<T5>()
					, _chunk.GetSpan<T6>()
					, _chunk.GetSpan<T7>()
					, _chunk.GetSpan<T8>()
					, _chunk.GetSpan<T9>()
					, _chunk.GetSpan<T10>()
					, _chunk.GetSpan<T11>()
					, _chunk.GetSpan<T12>()
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

			#region Parallel Work Loop Setup
			// Borrow a counter which will be used to keep track of all in-progress work
			using var workCounterRental = Pool<CountdownEventContainer>.Rent();
			var workCounter = workCounterRental.Value.Event;
			workCounter.Reset(1);

			// Create parallel workers
			var processors = Math.Max(4, Math.Min(64, Environment.ProcessorCount) - 3);
			var workersArr = ArrayPool<ParallelQueryWorker<ChunkWorkItem14<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>>?>.Shared.Rent(processors);
			var workers = workersArr.AsMemory(0, processors);
			for (var i = 0; i < workers.Length; i++)
			{
				workersArr[i] = Pool<ParallelQueryWorker<ChunkWorkItem14<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>>>.Get();
				workersArr[i]!.Configure(workersArr, workCounter);
			}

			// Start the workers. Even though there is no work they cannot exit yet because
			// the counter was reset to 1 above.
			foreach (var item in workersArr)
			{
				if (item != null)
				{
#if NET8_0_OR_GREATER
					ThreadPool.UnsafeQueueUserWorkItem(item, true);
#else
					ThreadPool.UnsafeQueueUserWorkItem(item.Execute, true);
#endif
				}
			}

			// Enqueue work to this index
			var workerEnqueueIdx = 0;
			#endregion

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
				var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				count += archetype.EntityCount;

				// Inrement work counter for all of the chunk we're about to enqueue
				workCounter.AddCount(archetype.Chunks.Count);

				using var enumerator = archetype.GetChunkEnumerator();
				while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current!;
					var item = new ChunkWorkItem14<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(chunk, q);

						#region Parallel Work Loop Add To Queue
						// Add work to a worker
						workersArr[workerEnqueueIdx]!.Enqueue(item);

						workerEnqueueIdx++;
						if (workerEnqueueIdx >= workers.Length)
							workerEnqueueIdx = 0;
						#endregion
				}
			}

			#region Parallel Work Loop Teardown
			// Collection of exceptions thrown in any work
			List<Exception>? exceptions = null;

			// Clear the 1 that was added at the start (when the counter was reset)
			workCounter.Signal();

			// Try to steal some work to do on this thread
			while (!workCounter.IsSet)
			{
				for (var i = 0; i < workers.Length && !workCounter.IsSet; i++)
				{
					if (workersArr[i]!.Steal(out var work))
					{
						workCounter.Signal();
						try
						{
							work.Execute();
						}
						catch (Exception ex)
						{
							exceptions ??= [ ];
							exceptions.Add(ex);
							break;
						}
					}
				}
			}

			// Wait for work to finish
			workCounter.Wait();

			// Recycle workers
			for (var i = 0; i < workers.Length; i++)
			{
				var worker = workersArr[i]!;
				worker.FinishEvent.Wait();
				worker.Clear(ref exceptions);
				Pool.Return(worker);
			}
			Array.Clear(workersArr, 0, workersArr.Length);
			ArrayPool<ParallelQueryWorker<ChunkWorkItem14<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>>>.Shared.Return(workersArr!);

			// Throw collected exceptions
			if (exceptions is { Count: > 0 })
				throw new AggregateException(exceptions);
			#endregion

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
			private readonly Chunks.Chunk _chunk;

			public ChunkWorkItem14(Chunks.Chunk chunk, TQ q)
			{
				_chunk = chunk;
				_q = q;
			}

			public void Execute()
			{
				_q.Execute(
					_chunk.Entities
					, _chunk.GetSpan<T0>()
					, _chunk.GetSpan<T1>()
					, _chunk.GetSpan<T2>()
					, _chunk.GetSpan<T3>()
					, _chunk.GetSpan<T4>()
					, _chunk.GetSpan<T5>()
					, _chunk.GetSpan<T6>()
					, _chunk.GetSpan<T7>()
					, _chunk.GetSpan<T8>()
					, _chunk.GetSpan<T9>()
					, _chunk.GetSpan<T10>()
					, _chunk.GetSpan<T11>()
					, _chunk.GetSpan<T12>()
					, _chunk.GetSpan<T13>()
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

			#region Parallel Work Loop Setup
			// Borrow a counter which will be used to keep track of all in-progress work
			using var workCounterRental = Pool<CountdownEventContainer>.Rent();
			var workCounter = workCounterRental.Value.Event;
			workCounter.Reset(1);

			// Create parallel workers
			var processors = Math.Max(4, Math.Min(64, Environment.ProcessorCount) - 3);
			var workersArr = ArrayPool<ParallelQueryWorker<ChunkWorkItem15<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>>?>.Shared.Rent(processors);
			var workers = workersArr.AsMemory(0, processors);
			for (var i = 0; i < workers.Length; i++)
			{
				workersArr[i] = Pool<ParallelQueryWorker<ChunkWorkItem15<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>>>.Get();
				workersArr[i]!.Configure(workersArr, workCounter);
			}

			// Start the workers. Even though there is no work they cannot exit yet because
			// the counter was reset to 1 above.
			foreach (var item in workersArr)
			{
				if (item != null)
				{
#if NET8_0_OR_GREATER
					ThreadPool.UnsafeQueueUserWorkItem(item, true);
#else
					ThreadPool.UnsafeQueueUserWorkItem(item.Execute, true);
#endif
				}
			}

			// Enqueue work to this index
			var workerEnqueueIdx = 0;
			#endregion

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
				var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				count += archetype.EntityCount;

				// Inrement work counter for all of the chunk we're about to enqueue
				workCounter.AddCount(archetype.Chunks.Count);

				using var enumerator = archetype.GetChunkEnumerator();
				while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current!;
					var item = new ChunkWorkItem15<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(chunk, q);

						#region Parallel Work Loop Add To Queue
						// Add work to a worker
						workersArr[workerEnqueueIdx]!.Enqueue(item);

						workerEnqueueIdx++;
						if (workerEnqueueIdx >= workers.Length)
							workerEnqueueIdx = 0;
						#endregion
				}
			}

			#region Parallel Work Loop Teardown
			// Collection of exceptions thrown in any work
			List<Exception>? exceptions = null;

			// Clear the 1 that was added at the start (when the counter was reset)
			workCounter.Signal();

			// Try to steal some work to do on this thread
			while (!workCounter.IsSet)
			{
				for (var i = 0; i < workers.Length && !workCounter.IsSet; i++)
				{
					if (workersArr[i]!.Steal(out var work))
					{
						workCounter.Signal();
						try
						{
							work.Execute();
						}
						catch (Exception ex)
						{
							exceptions ??= [ ];
							exceptions.Add(ex);
							break;
						}
					}
				}
			}

			// Wait for work to finish
			workCounter.Wait();

			// Recycle workers
			for (var i = 0; i < workers.Length; i++)
			{
				var worker = workersArr[i]!;
				worker.FinishEvent.Wait();
				worker.Clear(ref exceptions);
				Pool.Return(worker);
			}
			Array.Clear(workersArr, 0, workersArr.Length);
			ArrayPool<ParallelQueryWorker<ChunkWorkItem15<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>>>.Shared.Return(workersArr!);

			// Throw collected exceptions
			if (exceptions is { Count: > 0 })
				throw new AggregateException(exceptions);
			#endregion

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
			private readonly Chunks.Chunk _chunk;

			public ChunkWorkItem15(Chunks.Chunk chunk, TQ q)
			{
				_chunk = chunk;
				_q = q;
			}

			public void Execute()
			{
				_q.Execute(
					_chunk.Entities
					, _chunk.GetSpan<T0>()
					, _chunk.GetSpan<T1>()
					, _chunk.GetSpan<T2>()
					, _chunk.GetSpan<T3>()
					, _chunk.GetSpan<T4>()
					, _chunk.GetSpan<T5>()
					, _chunk.GetSpan<T6>()
					, _chunk.GetSpan<T7>()
					, _chunk.GetSpan<T8>()
					, _chunk.GetSpan<T9>()
					, _chunk.GetSpan<T10>()
					, _chunk.GetSpan<T11>()
					, _chunk.GetSpan<T12>()
					, _chunk.GetSpan<T13>()
					, _chunk.GetSpan<T14>()
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

			#region Parallel Work Loop Setup
			// Borrow a counter which will be used to keep track of all in-progress work
			using var workCounterRental = Pool<CountdownEventContainer>.Rent();
			var workCounter = workCounterRental.Value.Event;
			workCounter.Reset(1);

			// Create parallel workers
			var processors = Math.Max(4, Math.Min(64, Environment.ProcessorCount) - 3);
			var workersArr = ArrayPool<ParallelQueryWorker<ChunkWorkItem16<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>>?>.Shared.Rent(processors);
			var workers = workersArr.AsMemory(0, processors);
			for (var i = 0; i < workers.Length; i++)
			{
				workersArr[i] = Pool<ParallelQueryWorker<ChunkWorkItem16<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>>>.Get();
				workersArr[i]!.Configure(workersArr, workCounter);
			}

			// Start the workers. Even though there is no work they cannot exit yet because
			// the counter was reset to 1 above.
			foreach (var item in workersArr)
			{
				if (item != null)
				{
#if NET8_0_OR_GREATER
					ThreadPool.UnsafeQueueUserWorkItem(item, true);
#else
					ThreadPool.UnsafeQueueUserWorkItem(item.Execute, true);
#endif
				}
			}

			// Enqueue work to this index
			var workerEnqueueIdx = 0;
			#endregion

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
				var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				count += archetype.EntityCount;

				// Inrement work counter for all of the chunk we're about to enqueue
				workCounter.AddCount(archetype.Chunks.Count);

				using var enumerator = archetype.GetChunkEnumerator();
				while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current!;
					var item = new ChunkWorkItem16<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(chunk, q);

						#region Parallel Work Loop Add To Queue
						// Add work to a worker
						workersArr[workerEnqueueIdx]!.Enqueue(item);

						workerEnqueueIdx++;
						if (workerEnqueueIdx >= workers.Length)
							workerEnqueueIdx = 0;
						#endregion
				}
			}

			#region Parallel Work Loop Teardown
			// Collection of exceptions thrown in any work
			List<Exception>? exceptions = null;

			// Clear the 1 that was added at the start (when the counter was reset)
			workCounter.Signal();

			// Try to steal some work to do on this thread
			while (!workCounter.IsSet)
			{
				for (var i = 0; i < workers.Length && !workCounter.IsSet; i++)
				{
					if (workersArr[i]!.Steal(out var work))
					{
						workCounter.Signal();
						try
						{
							work.Execute();
						}
						catch (Exception ex)
						{
							exceptions ??= [ ];
							exceptions.Add(ex);
							break;
						}
					}
				}
			}

			// Wait for work to finish
			workCounter.Wait();

			// Recycle workers
			for (var i = 0; i < workers.Length; i++)
			{
				var worker = workersArr[i]!;
				worker.FinishEvent.Wait();
				worker.Clear(ref exceptions);
				Pool.Return(worker);
			}
			Array.Clear(workersArr, 0, workersArr.Length);
			ArrayPool<ParallelQueryWorker<ChunkWorkItem16<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>>>.Shared.Return(workersArr!);

			// Throw collected exceptions
			if (exceptions is { Count: > 0 })
				throw new AggregateException(exceptions);
			#endregion

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
			private readonly Chunks.Chunk _chunk;

			public ChunkWorkItem16(Chunks.Chunk chunk, TQ q)
			{
				_chunk = chunk;
				_q = q;
			}

			public void Execute()
			{
				_q.Execute(
					_chunk.Entities
					, _chunk.GetSpan<T0>()
					, _chunk.GetSpan<T1>()
					, _chunk.GetSpan<T2>()
					, _chunk.GetSpan<T3>()
					, _chunk.GetSpan<T4>()
					, _chunk.GetSpan<T5>()
					, _chunk.GetSpan<T6>()
					, _chunk.GetSpan<T7>()
					, _chunk.GetSpan<T8>()
					, _chunk.GetSpan<T9>()
					, _chunk.GetSpan<T10>()
					, _chunk.GetSpan<T11>()
					, _chunk.GetSpan<T12>()
					, _chunk.GetSpan<T13>()
					, _chunk.GetSpan<T14>()
					, _chunk.GetSpan<T15>()
				);
			}
		}
	}
}



