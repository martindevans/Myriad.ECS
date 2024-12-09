

using System.Diagnostics;
using System.Buffers;
using Myriad.ECS.Queries;
using Myriad.ECS.IDs;
using Myriad.ECS.Worlds.Archetypes;
using Myriad.ECS.Allocations;
using Myriad.ECS.Threading;
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
	/// A query which accepts 1 components
	/// </summary>
	public interface IQuery<T0>
		where T0 : IComponent
	{
		/// <summary>
		/// Execute the query for a single entity
		/// </summary>
		public void Execute(Entity e, ref T0 t0);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		
		public int Execute<TQ, T0>(
			QueryDescription? query = null
		)
			where T0 : IComponent
			where TQ : IQuery<T0>, new()
		{
			var q = new TQ();
			return Execute<TQ, T0>(ref q, query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		
		public int Execute<TQ, T0>(
			ref QueryDescription? query
		)
			where T0 : IComponent
			where TQ : IQuery<T0>, new()
		{
			var q = new TQ();
			return Execute<TQ, T0>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <param name="q">The instance to execute over every entity.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		
		public int Execute<TQ, T0>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : IComponent
			where TQ : IQuery<T0>
		{
			return Execute<TQ, T0>(ref q, query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <param name="q">The instance to execute over every entity.</param>
		/// <returns>The number of entities discovered by this query</returns>
		
		public int Execute<TQ, T0>(
			TQ q,
			ref QueryDescription? query
		)
			where T0 : IComponent
			where TQ : IQuery<T0>
		{
			return Execute<TQ, T0>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <param name="q">
		/// The instance to execute over every entity. Passed by ref, so changes to the query
		/// struct will be persistent. This can allow values from one entity to be accessed by
		/// the next entity, or after the entire Execute call is complete.
		/// </param>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		
		public int Execute<TQ, T0>(
			ref TQ q,
			QueryDescription? query = null
		)
			where T0 : IComponent
			where TQ : IQuery<T0>
		{
			return Execute<TQ, T0>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <param name="q">
		/// The instance to execute over every entity. Passed by ref, so changes to the query
		/// struct will be persistent. This can allow values from one entity to be accessed by
		/// the next entity, or after the entire Execute call is complete.
		/// </param>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		
		public int Execute<TQ, T0>(
			ref TQ q,
			ref QueryDescription? query
		)
			where T0 : IComponent
			where TQ : IQuery<T0>
		{
			query ??= GetCachedQuery<T0>();

			var archetypes = query.GetArchetypes();

			var c0 = ComponentID<T0>.ID;

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;

				count += archetype.EntityCount;

				using (var enumerator = archetype.GetChunkEnumerator())
				{
					while (enumerator.MoveNext())
					{
						var chunk = enumerator.Current;
						Debug.Assert(chunk != null);

						var entities = chunk.Entities.Span;

						var t0 = chunk.GetSpan<T0>(c0);
						Debug.Assert(t0.Length == entities.Length);

						unsafe
						{
							#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
							fixed (Entity* eptr = entities)
							fixed (T0* t0ptr = t0)
							#pragma warning restore CS8500
							{
								for (var i = 0; i < entities.Length; i++)
									q.Execute(eptr[i], ref t0ptr[i]);
							}
						}
					}
				}
			}

			return count;
		}

		/// <summary>
		/// Execute a query in parallel over entities, blocks until complete.
		/// </summary>
		/// <param name="q"></param>
		/// <param name="query"></param>
		/// <param name="batchSize"></param>
		/// <returns></returns>
		/// <exception cref="AggregateException"></exception>
		
		public int ExecuteParallel<TQ, T0>(
			TQ q,
			QueryDescription? query = null,
			int batchSize = 128
		)
			where T0 : IComponent
			where TQ : IQuery<T0>
		{
			query ??= GetCachedQuery<T0>();

			var archetypes = query.GetArchetypes();

			// Early exit if there is no work to do, avoiding the cost of setting up the worker pool
			if (!query.Any())
				return 0;

			batchSize = Math.Clamp(batchSize, 1, Archetype.CHUNK_SIZE);

			var c0 = ComponentID<T0>.ID;

			#region Parallel Work Loop Setup
			// Borrow a counter which will be used to keep track of all in-progress work
			using var workCounterRental = Pool<CountdownEventContainer>.Rent();
			var workCounter = workCounterRental.Value.Event;
			workCounter.Reset(1);

			// Create parallel workers
			var processors = ThreadPool.Threads;
			var workersArr = ArrayPool<ParallelQueryWorker<WorkItem1<TQ, T0>>?>.Shared.Rent(processors);
			var workers = workersArr.AsMemory(0, processors);
			for (var i = 0; i < workers.Length; i++)
			{
				workersArr[i] = Pool<ParallelQueryWorker<WorkItem1<TQ, T0>>>.Get();
				workersArr[i]!.Configure(workersArr, workCounter);
			}

			// Start the workers. Even though there is no work they cannot exit yet because
			// the counter was reset to 1 above.
			foreach (var item in workersArr)
			{
				if (item != null)
					ThreadPool.QueueUserWorkItem(item);
			}

			// Enqueue work to this index
			var workerEnqueueIdx = 0;
			#endregion

			// Enqueue all of the work into the parallel workers
			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;

				count += archetype.EntityCount;

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entityCount = chunk.EntityCount;
					var numBatches = (int)Math.Ceiling(entityCount / (float)batchSize);

					// Inrement work counter for all of the batches we're about to create
					workCounter.AddCount(numBatches);

					var t0 = chunk.GetComponentArray<T0>(c0);
					for (var b = 0; b < numBatches; b++)
					{
						var start = b * batchSize;
						var end = Math.Min(start + batchSize, entityCount);
						var batchCount = end - start;
						var eMem = chunk.Entities.Slice(start, batchCount);

						var item = new WorkItem1<TQ, T0>(
							eMem,
							t0.AsMemory(start, batchCount),
							q
						);

						#region Parallel Work Loop Add To Queue
						// Add work to a worker
						workersArr[workerEnqueueIdx]!.Enqueue(item);

						workerEnqueueIdx++;
						if (workerEnqueueIdx >= workers.Length)
							workerEnqueueIdx = 0;
						#endregion
					}
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
			ArrayPool<ParallelQueryWorker<WorkItem1<TQ, T0>>>.Shared.Return(workersArr!);

			// Throw collected exceptions
			if (exceptions is { Count: > 0 })
				throw new AggregateException(exceptions);
			#endregion

			return count;
		}

		
		private readonly struct WorkItem1<TQ, T0>
			: IWorkItem
			where T0 : IComponent
			where TQ : IQuery<T0>
		{
			private readonly TQ _q;

			private readonly ReadOnlyMemory<Entity> _entities;
			private readonly Memory<T0> _c0;

			public WorkItem1(
				ReadOnlyMemory<Entity> entities,
				Memory<T0> c0,
				TQ q
			)
			{
				_entities = entities;

				_c0 = c0;

				_q = q;
			}

			public void Execute()
			{
				var eSpan = _entities.Span;
				var c0 = _c0.Span;

				for (var i = 0; i < _entities.Length; i++)
				{
					_q.Execute(
						eSpan[i]
						, ref c0[i]
					);
				}
			}
		}
	}
}
namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A query which accepts 2 components
	/// </summary>
	public interface IQuery<T0, T1>
		where T0 : IComponent
        where T1 : IComponent
	{
		/// <summary>
		/// Execute the query for a single entity
		/// </summary>
		public void Execute(Entity e, ref T0 t0, ref T1 t1);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1>(
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
			where TQ : IQuery<T0, T1>, new()
		{
			var q = new TQ();
			return Execute<TQ, T0, T1>(ref q, query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1>(
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
			where TQ : IQuery<T0, T1>, new()
		{
			var q = new TQ();
			return Execute<TQ, T0, T1>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <param name="q">The instance to execute over every entity.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
			where TQ : IQuery<T0, T1>
		{
			return Execute<TQ, T0, T1>(ref q, query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <param name="q">The instance to execute over every entity.</param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1>(
			TQ q,
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
			where TQ : IQuery<T0, T1>
		{
			return Execute<TQ, T0, T1>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <param name="q">
		/// The instance to execute over every entity. Passed by ref, so changes to the query
		/// struct will be persistent. This can allow values from one entity to be accessed by
		/// the next entity, or after the entire Execute call is complete.
		/// </param>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1>(
			ref TQ q,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
			where TQ : IQuery<T0, T1>
		{
			return Execute<TQ, T0, T1>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <param name="q">
		/// The instance to execute over every entity. Passed by ref, so changes to the query
		/// struct will be persistent. This can allow values from one entity to be accessed by
		/// the next entity, or after the entire Execute call is complete.
		/// </param>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1>(
			ref TQ q,
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
			where TQ : IQuery<T0, T1>
		{
			query ??= GetCachedQuery<T0, T1>();

			var archetypes = query.GetArchetypes();

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;

				count += archetype.EntityCount;

				using (var enumerator = archetype.GetChunkEnumerator())
				{
					while (enumerator.MoveNext())
					{
						var chunk = enumerator.Current;
						Debug.Assert(chunk != null);

						var entities = chunk.Entities.Span;

						var t0 = chunk.GetSpan<T0>(c0);
						Debug.Assert(t0.Length == entities.Length);
						var t1 = chunk.GetSpan<T1>(c1);
						Debug.Assert(t1.Length == entities.Length);

						unsafe
						{
							#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
							fixed (Entity* eptr = entities)
							fixed (T0* t0ptr = t0)
							fixed (T1* t1ptr = t1)
							#pragma warning restore CS8500
							{
								for (var i = 0; i < entities.Length; i++)
									q.Execute(eptr[i], ref t0ptr[i], ref t1ptr[i]);
							}
						}
					}
				}
			}

			return count;
		}

		/// <summary>
		/// Execute a query in parallel over entities, blocks until complete.
		/// </summary>
		/// <param name="q"></param>
		/// <param name="query"></param>
		/// <param name="batchSize"></param>
		/// <returns></returns>
		/// <exception cref="AggregateException"></exception>
		[ExcludeFromCodeCoverage]
		public int ExecuteParallel<TQ, T0, T1>(
			TQ q,
			QueryDescription? query = null,
			int batchSize = 128
		)
			where T0 : IComponent
            where T1 : IComponent
			where TQ : IQuery<T0, T1>
		{
			query ??= GetCachedQuery<T0, T1>();

			var archetypes = query.GetArchetypes();

			// Early exit if there is no work to do, avoiding the cost of setting up the worker pool
			if (!query.Any())
				return 0;

			batchSize = Math.Clamp(batchSize, 1, Archetype.CHUNK_SIZE);

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;

			#region Parallel Work Loop Setup
			// Borrow a counter which will be used to keep track of all in-progress work
			using var workCounterRental = Pool<CountdownEventContainer>.Rent();
			var workCounter = workCounterRental.Value.Event;
			workCounter.Reset(1);

			// Create parallel workers
			var processors = ThreadPool.Threads;
			var workersArr = ArrayPool<ParallelQueryWorker<WorkItem2<TQ, T0, T1>>?>.Shared.Rent(processors);
			var workers = workersArr.AsMemory(0, processors);
			for (var i = 0; i < workers.Length; i++)
			{
				workersArr[i] = Pool<ParallelQueryWorker<WorkItem2<TQ, T0, T1>>>.Get();
				workersArr[i]!.Configure(workersArr, workCounter);
			}

			// Start the workers. Even though there is no work they cannot exit yet because
			// the counter was reset to 1 above.
			foreach (var item in workersArr)
			{
				if (item != null)
					ThreadPool.QueueUserWorkItem(item);
			}

			// Enqueue work to this index
			var workerEnqueueIdx = 0;
			#endregion

			// Enqueue all of the work into the parallel workers
			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;

				count += archetype.EntityCount;

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entityCount = chunk.EntityCount;
					var numBatches = (int)Math.Ceiling(entityCount / (float)batchSize);

					// Inrement work counter for all of the batches we're about to create
					workCounter.AddCount(numBatches);

					var t0 = chunk.GetComponentArray<T0>(c0);
					var t1 = chunk.GetComponentArray<T1>(c1);
					for (var b = 0; b < numBatches; b++)
					{
						var start = b * batchSize;
						var end = Math.Min(start + batchSize, entityCount);
						var batchCount = end - start;
						var eMem = chunk.Entities.Slice(start, batchCount);

						var item = new WorkItem2<TQ, T0, T1>(
							eMem,
							t0.AsMemory(start, batchCount),
							t1.AsMemory(start, batchCount),
							q
						);

						#region Parallel Work Loop Add To Queue
						// Add work to a worker
						workersArr[workerEnqueueIdx]!.Enqueue(item);

						workerEnqueueIdx++;
						if (workerEnqueueIdx >= workers.Length)
							workerEnqueueIdx = 0;
						#endregion
					}
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
			ArrayPool<ParallelQueryWorker<WorkItem2<TQ, T0, T1>>>.Shared.Return(workersArr!);

			// Throw collected exceptions
			if (exceptions is { Count: > 0 })
				throw new AggregateException(exceptions);
			#endregion

			return count;
		}

		[ExcludeFromCodeCoverage]
		private readonly struct WorkItem2<TQ, T0, T1>
			: IWorkItem
			where T0 : IComponent
            where T1 : IComponent
			where TQ : IQuery<T0, T1>
		{
			private readonly TQ _q;

			private readonly ReadOnlyMemory<Entity> _entities;
			private readonly Memory<T0> _c0;
			private readonly Memory<T1> _c1;

			public WorkItem2(
				ReadOnlyMemory<Entity> entities,
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

			public void Execute()
			{
				var eSpan = _entities.Span;
				var c0 = _c0.Span;
				var c1 = _c1.Span;

				for (var i = 0; i < _entities.Length; i++)
				{
					_q.Execute(
						eSpan[i]
						, ref c0[i]
						, ref c1[i]
					);
				}
			}
		}
	}
}
namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A query which accepts 3 components
	/// </summary>
	public interface IQuery<T0, T1, T2>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
	{
		/// <summary>
		/// Execute the query for a single entity
		/// </summary>
		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2>(
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
			where TQ : IQuery<T0, T1, T2>, new()
		{
			var q = new TQ();
			return Execute<TQ, T0, T1, T2>(ref q, query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2>(
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
			where TQ : IQuery<T0, T1, T2>, new()
		{
			var q = new TQ();
			return Execute<TQ, T0, T1, T2>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <param name="q">The instance to execute over every entity.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
			where TQ : IQuery<T0, T1, T2>
		{
			return Execute<TQ, T0, T1, T2>(ref q, query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <param name="q">The instance to execute over every entity.</param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2>(
			TQ q,
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
			where TQ : IQuery<T0, T1, T2>
		{
			return Execute<TQ, T0, T1, T2>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <param name="q">
		/// The instance to execute over every entity. Passed by ref, so changes to the query
		/// struct will be persistent. This can allow values from one entity to be accessed by
		/// the next entity, or after the entire Execute call is complete.
		/// </param>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2>(
			ref TQ q,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
			where TQ : IQuery<T0, T1, T2>
		{
			return Execute<TQ, T0, T1, T2>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <param name="q">
		/// The instance to execute over every entity. Passed by ref, so changes to the query
		/// struct will be persistent. This can allow values from one entity to be accessed by
		/// the next entity, or after the entire Execute call is complete.
		/// </param>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2>(
			ref TQ q,
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
			where TQ : IQuery<T0, T1, T2>
		{
			query ??= GetCachedQuery<T0, T1, T2>();

			var archetypes = query.GetArchetypes();

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;
			var c2 = ComponentID<T2>.ID;

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;

				count += archetype.EntityCount;

				using (var enumerator = archetype.GetChunkEnumerator())
				{
					while (enumerator.MoveNext())
					{
						var chunk = enumerator.Current;
						Debug.Assert(chunk != null);

						var entities = chunk.Entities.Span;

						var t0 = chunk.GetSpan<T0>(c0);
						Debug.Assert(t0.Length == entities.Length);
						var t1 = chunk.GetSpan<T1>(c1);
						Debug.Assert(t1.Length == entities.Length);
						var t2 = chunk.GetSpan<T2>(c2);
						Debug.Assert(t2.Length == entities.Length);

						unsafe
						{
							#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
							fixed (Entity* eptr = entities)
							fixed (T0* t0ptr = t0)
							fixed (T1* t1ptr = t1)
							fixed (T2* t2ptr = t2)
							#pragma warning restore CS8500
							{
								for (var i = 0; i < entities.Length; i++)
									q.Execute(eptr[i], ref t0ptr[i], ref t1ptr[i], ref t2ptr[i]);
							}
						}
					}
				}
			}

			return count;
		}

		/// <summary>
		/// Execute a query in parallel over entities, blocks until complete.
		/// </summary>
		/// <param name="q"></param>
		/// <param name="query"></param>
		/// <param name="batchSize"></param>
		/// <returns></returns>
		/// <exception cref="AggregateException"></exception>
		[ExcludeFromCodeCoverage]
		public int ExecuteParallel<TQ, T0, T1, T2>(
			TQ q,
			QueryDescription? query = null,
			int batchSize = 128
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
			where TQ : IQuery<T0, T1, T2>
		{
			query ??= GetCachedQuery<T0, T1, T2>();

			var archetypes = query.GetArchetypes();

			// Early exit if there is no work to do, avoiding the cost of setting up the worker pool
			if (!query.Any())
				return 0;

			batchSize = Math.Clamp(batchSize, 1, Archetype.CHUNK_SIZE);

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;
			var c2 = ComponentID<T2>.ID;

			#region Parallel Work Loop Setup
			// Borrow a counter which will be used to keep track of all in-progress work
			using var workCounterRental = Pool<CountdownEventContainer>.Rent();
			var workCounter = workCounterRental.Value.Event;
			workCounter.Reset(1);

			// Create parallel workers
			var processors = ThreadPool.Threads;
			var workersArr = ArrayPool<ParallelQueryWorker<WorkItem3<TQ, T0, T1, T2>>?>.Shared.Rent(processors);
			var workers = workersArr.AsMemory(0, processors);
			for (var i = 0; i < workers.Length; i++)
			{
				workersArr[i] = Pool<ParallelQueryWorker<WorkItem3<TQ, T0, T1, T2>>>.Get();
				workersArr[i]!.Configure(workersArr, workCounter);
			}

			// Start the workers. Even though there is no work they cannot exit yet because
			// the counter was reset to 1 above.
			foreach (var item in workersArr)
			{
				if (item != null)
					ThreadPool.QueueUserWorkItem(item);
			}

			// Enqueue work to this index
			var workerEnqueueIdx = 0;
			#endregion

			// Enqueue all of the work into the parallel workers
			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;

				count += archetype.EntityCount;

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entityCount = chunk.EntityCount;
					var numBatches = (int)Math.Ceiling(entityCount / (float)batchSize);

					// Inrement work counter for all of the batches we're about to create
					workCounter.AddCount(numBatches);

					var t0 = chunk.GetComponentArray<T0>(c0);
					var t1 = chunk.GetComponentArray<T1>(c1);
					var t2 = chunk.GetComponentArray<T2>(c2);
					for (var b = 0; b < numBatches; b++)
					{
						var start = b * batchSize;
						var end = Math.Min(start + batchSize, entityCount);
						var batchCount = end - start;
						var eMem = chunk.Entities.Slice(start, batchCount);

						var item = new WorkItem3<TQ, T0, T1, T2>(
							eMem,
							t0.AsMemory(start, batchCount),
							t1.AsMemory(start, batchCount),
							t2.AsMemory(start, batchCount),
							q
						);

						#region Parallel Work Loop Add To Queue
						// Add work to a worker
						workersArr[workerEnqueueIdx]!.Enqueue(item);

						workerEnqueueIdx++;
						if (workerEnqueueIdx >= workers.Length)
							workerEnqueueIdx = 0;
						#endregion
					}
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
			ArrayPool<ParallelQueryWorker<WorkItem3<TQ, T0, T1, T2>>>.Shared.Return(workersArr!);

			// Throw collected exceptions
			if (exceptions is { Count: > 0 })
				throw new AggregateException(exceptions);
			#endregion

			return count;
		}

		[ExcludeFromCodeCoverage]
		private readonly struct WorkItem3<TQ, T0, T1, T2>
			: IWorkItem
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
			where TQ : IQuery<T0, T1, T2>
		{
			private readonly TQ _q;

			private readonly ReadOnlyMemory<Entity> _entities;
			private readonly Memory<T0> _c0;
			private readonly Memory<T1> _c1;
			private readonly Memory<T2> _c2;

			public WorkItem3(
				ReadOnlyMemory<Entity> entities,
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

			public void Execute()
			{
				var eSpan = _entities.Span;
				var c0 = _c0.Span;
				var c1 = _c1.Span;
				var c2 = _c2.Span;

				for (var i = 0; i < _entities.Length; i++)
				{
					_q.Execute(
						eSpan[i]
						, ref c0[i]
						, ref c1[i]
						, ref c2[i]
					);
				}
			}
		}
	}
}
namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A query which accepts 4 components
	/// </summary>
	public interface IQuery<T0, T1, T2, T3>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
	{
		/// <summary>
		/// Execute the query for a single entity
		/// </summary>
		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3>(
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
			where TQ : IQuery<T0, T1, T2, T3>, new()
		{
			var q = new TQ();
			return Execute<TQ, T0, T1, T2, T3>(ref q, query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3>(
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
			where TQ : IQuery<T0, T1, T2, T3>, new()
		{
			var q = new TQ();
			return Execute<TQ, T0, T1, T2, T3>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <param name="q">The instance to execute over every entity.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
			where TQ : IQuery<T0, T1, T2, T3>
		{
			return Execute<TQ, T0, T1, T2, T3>(ref q, query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <param name="q">The instance to execute over every entity.</param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3>(
			TQ q,
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
			where TQ : IQuery<T0, T1, T2, T3>
		{
			return Execute<TQ, T0, T1, T2, T3>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <param name="q">
		/// The instance to execute over every entity. Passed by ref, so changes to the query
		/// struct will be persistent. This can allow values from one entity to be accessed by
		/// the next entity, or after the entire Execute call is complete.
		/// </param>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3>(
			ref TQ q,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
			where TQ : IQuery<T0, T1, T2, T3>
		{
			return Execute<TQ, T0, T1, T2, T3>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <param name="q">
		/// The instance to execute over every entity. Passed by ref, so changes to the query
		/// struct will be persistent. This can allow values from one entity to be accessed by
		/// the next entity, or after the entire Execute call is complete.
		/// </param>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3>(
			ref TQ q,
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
			where TQ : IQuery<T0, T1, T2, T3>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3>();

			var archetypes = query.GetArchetypes();

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;
			var c2 = ComponentID<T2>.ID;
			var c3 = ComponentID<T3>.ID;

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;

				count += archetype.EntityCount;

				using (var enumerator = archetype.GetChunkEnumerator())
				{
					while (enumerator.MoveNext())
					{
						var chunk = enumerator.Current;
						Debug.Assert(chunk != null);

						var entities = chunk.Entities.Span;

						var t0 = chunk.GetSpan<T0>(c0);
						Debug.Assert(t0.Length == entities.Length);
						var t1 = chunk.GetSpan<T1>(c1);
						Debug.Assert(t1.Length == entities.Length);
						var t2 = chunk.GetSpan<T2>(c2);
						Debug.Assert(t2.Length == entities.Length);
						var t3 = chunk.GetSpan<T3>(c3);
						Debug.Assert(t3.Length == entities.Length);

						unsafe
						{
							#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
							fixed (Entity* eptr = entities)
							fixed (T0* t0ptr = t0)
							fixed (T1* t1ptr = t1)
							fixed (T2* t2ptr = t2)
							fixed (T3* t3ptr = t3)
							#pragma warning restore CS8500
							{
								for (var i = 0; i < entities.Length; i++)
									q.Execute(eptr[i], ref t0ptr[i], ref t1ptr[i], ref t2ptr[i], ref t3ptr[i]);
							}
						}
					}
				}
			}

			return count;
		}

		/// <summary>
		/// Execute a query in parallel over entities, blocks until complete.
		/// </summary>
		/// <param name="q"></param>
		/// <param name="query"></param>
		/// <param name="batchSize"></param>
		/// <returns></returns>
		/// <exception cref="AggregateException"></exception>
		[ExcludeFromCodeCoverage]
		public int ExecuteParallel<TQ, T0, T1, T2, T3>(
			TQ q,
			QueryDescription? query = null,
			int batchSize = 128
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
			where TQ : IQuery<T0, T1, T2, T3>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3>();

			var archetypes = query.GetArchetypes();

			// Early exit if there is no work to do, avoiding the cost of setting up the worker pool
			if (!query.Any())
				return 0;

			batchSize = Math.Clamp(batchSize, 1, Archetype.CHUNK_SIZE);

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;
			var c2 = ComponentID<T2>.ID;
			var c3 = ComponentID<T3>.ID;

			#region Parallel Work Loop Setup
			// Borrow a counter which will be used to keep track of all in-progress work
			using var workCounterRental = Pool<CountdownEventContainer>.Rent();
			var workCounter = workCounterRental.Value.Event;
			workCounter.Reset(1);

			// Create parallel workers
			var processors = ThreadPool.Threads;
			var workersArr = ArrayPool<ParallelQueryWorker<WorkItem4<TQ, T0, T1, T2, T3>>?>.Shared.Rent(processors);
			var workers = workersArr.AsMemory(0, processors);
			for (var i = 0; i < workers.Length; i++)
			{
				workersArr[i] = Pool<ParallelQueryWorker<WorkItem4<TQ, T0, T1, T2, T3>>>.Get();
				workersArr[i]!.Configure(workersArr, workCounter);
			}

			// Start the workers. Even though there is no work they cannot exit yet because
			// the counter was reset to 1 above.
			foreach (var item in workersArr)
			{
				if (item != null)
					ThreadPool.QueueUserWorkItem(item);
			}

			// Enqueue work to this index
			var workerEnqueueIdx = 0;
			#endregion

			// Enqueue all of the work into the parallel workers
			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;

				count += archetype.EntityCount;

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entityCount = chunk.EntityCount;
					var numBatches = (int)Math.Ceiling(entityCount / (float)batchSize);

					// Inrement work counter for all of the batches we're about to create
					workCounter.AddCount(numBatches);

					var t0 = chunk.GetComponentArray<T0>(c0);
					var t1 = chunk.GetComponentArray<T1>(c1);
					var t2 = chunk.GetComponentArray<T2>(c2);
					var t3 = chunk.GetComponentArray<T3>(c3);
					for (var b = 0; b < numBatches; b++)
					{
						var start = b * batchSize;
						var end = Math.Min(start + batchSize, entityCount);
						var batchCount = end - start;
						var eMem = chunk.Entities.Slice(start, batchCount);

						var item = new WorkItem4<TQ, T0, T1, T2, T3>(
							eMem,
							t0.AsMemory(start, batchCount),
							t1.AsMemory(start, batchCount),
							t2.AsMemory(start, batchCount),
							t3.AsMemory(start, batchCount),
							q
						);

						#region Parallel Work Loop Add To Queue
						// Add work to a worker
						workersArr[workerEnqueueIdx]!.Enqueue(item);

						workerEnqueueIdx++;
						if (workerEnqueueIdx >= workers.Length)
							workerEnqueueIdx = 0;
						#endregion
					}
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
			ArrayPool<ParallelQueryWorker<WorkItem4<TQ, T0, T1, T2, T3>>>.Shared.Return(workersArr!);

			// Throw collected exceptions
			if (exceptions is { Count: > 0 })
				throw new AggregateException(exceptions);
			#endregion

			return count;
		}

		[ExcludeFromCodeCoverage]
		private readonly struct WorkItem4<TQ, T0, T1, T2, T3>
			: IWorkItem
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
			where TQ : IQuery<T0, T1, T2, T3>
		{
			private readonly TQ _q;

			private readonly ReadOnlyMemory<Entity> _entities;
			private readonly Memory<T0> _c0;
			private readonly Memory<T1> _c1;
			private readonly Memory<T2> _c2;
			private readonly Memory<T3> _c3;

			public WorkItem4(
				ReadOnlyMemory<Entity> entities,
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

			public void Execute()
			{
				var eSpan = _entities.Span;
				var c0 = _c0.Span;
				var c1 = _c1.Span;
				var c2 = _c2.Span;
				var c3 = _c3.Span;

				for (var i = 0; i < _entities.Length; i++)
				{
					_q.Execute(
						eSpan[i]
						, ref c0[i]
						, ref c1[i]
						, ref c2[i]
						, ref c3[i]
					);
				}
			}
		}
	}
}
namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A query which accepts 5 components
	/// </summary>
	public interface IQuery<T0, T1, T2, T3, T4>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
	{
		/// <summary>
		/// Execute the query for a single entity
		/// </summary>
		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4>(
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
			where TQ : IQuery<T0, T1, T2, T3, T4>, new()
		{
			var q = new TQ();
			return Execute<TQ, T0, T1, T2, T3, T4>(ref q, query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4>(
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
			where TQ : IQuery<T0, T1, T2, T3, T4>, new()
		{
			var q = new TQ();
			return Execute<TQ, T0, T1, T2, T3, T4>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <param name="q">The instance to execute over every entity.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4>(
			TQ q,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
			where TQ : IQuery<T0, T1, T2, T3, T4>
		{
			return Execute<TQ, T0, T1, T2, T3, T4>(ref q, query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <param name="q">The instance to execute over every entity.</param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4>(
			TQ q,
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
			where TQ : IQuery<T0, T1, T2, T3, T4>
		{
			return Execute<TQ, T0, T1, T2, T3, T4>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <param name="q">
		/// The instance to execute over every entity. Passed by ref, so changes to the query
		/// struct will be persistent. This can allow values from one entity to be accessed by
		/// the next entity, or after the entire Execute call is complete.
		/// </param>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4>(
			ref TQ q,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
			where TQ : IQuery<T0, T1, T2, T3, T4>
		{
			return Execute<TQ, T0, T1, T2, T3, T4>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <param name="q">
		/// The instance to execute over every entity. Passed by ref, so changes to the query
		/// struct will be persistent. This can allow values from one entity to be accessed by
		/// the next entity, or after the entire Execute call is complete.
		/// </param>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4>(
			ref TQ q,
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
			where TQ : IQuery<T0, T1, T2, T3, T4>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4>();

			var archetypes = query.GetArchetypes();

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;
			var c2 = ComponentID<T2>.ID;
			var c3 = ComponentID<T3>.ID;
			var c4 = ComponentID<T4>.ID;

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;

				count += archetype.EntityCount;

				using (var enumerator = archetype.GetChunkEnumerator())
				{
					while (enumerator.MoveNext())
					{
						var chunk = enumerator.Current;
						Debug.Assert(chunk != null);

						var entities = chunk.Entities.Span;

						var t0 = chunk.GetSpan<T0>(c0);
						Debug.Assert(t0.Length == entities.Length);
						var t1 = chunk.GetSpan<T1>(c1);
						Debug.Assert(t1.Length == entities.Length);
						var t2 = chunk.GetSpan<T2>(c2);
						Debug.Assert(t2.Length == entities.Length);
						var t3 = chunk.GetSpan<T3>(c3);
						Debug.Assert(t3.Length == entities.Length);
						var t4 = chunk.GetSpan<T4>(c4);
						Debug.Assert(t4.Length == entities.Length);

						unsafe
						{
							#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
							fixed (Entity* eptr = entities)
							fixed (T0* t0ptr = t0)
							fixed (T1* t1ptr = t1)
							fixed (T2* t2ptr = t2)
							fixed (T3* t3ptr = t3)
							fixed (T4* t4ptr = t4)
							#pragma warning restore CS8500
							{
								for (var i = 0; i < entities.Length; i++)
									q.Execute(eptr[i], ref t0ptr[i], ref t1ptr[i], ref t2ptr[i], ref t3ptr[i], ref t4ptr[i]);
							}
						}
					}
				}
			}

			return count;
		}

		/// <summary>
		/// Execute a query in parallel over entities, blocks until complete.
		/// </summary>
		/// <param name="q"></param>
		/// <param name="query"></param>
		/// <param name="batchSize"></param>
		/// <returns></returns>
		/// <exception cref="AggregateException"></exception>
		[ExcludeFromCodeCoverage]
		public int ExecuteParallel<TQ, T0, T1, T2, T3, T4>(
			TQ q,
			QueryDescription? query = null,
			int batchSize = 128
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
			where TQ : IQuery<T0, T1, T2, T3, T4>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4>();

			var archetypes = query.GetArchetypes();

			// Early exit if there is no work to do, avoiding the cost of setting up the worker pool
			if (!query.Any())
				return 0;

			batchSize = Math.Clamp(batchSize, 1, Archetype.CHUNK_SIZE);

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;
			var c2 = ComponentID<T2>.ID;
			var c3 = ComponentID<T3>.ID;
			var c4 = ComponentID<T4>.ID;

			#region Parallel Work Loop Setup
			// Borrow a counter which will be used to keep track of all in-progress work
			using var workCounterRental = Pool<CountdownEventContainer>.Rent();
			var workCounter = workCounterRental.Value.Event;
			workCounter.Reset(1);

			// Create parallel workers
			var processors = ThreadPool.Threads;
			var workersArr = ArrayPool<ParallelQueryWorker<WorkItem5<TQ, T0, T1, T2, T3, T4>>?>.Shared.Rent(processors);
			var workers = workersArr.AsMemory(0, processors);
			for (var i = 0; i < workers.Length; i++)
			{
				workersArr[i] = Pool<ParallelQueryWorker<WorkItem5<TQ, T0, T1, T2, T3, T4>>>.Get();
				workersArr[i]!.Configure(workersArr, workCounter);
			}

			// Start the workers. Even though there is no work they cannot exit yet because
			// the counter was reset to 1 above.
			foreach (var item in workersArr)
			{
				if (item != null)
					ThreadPool.QueueUserWorkItem(item);
			}

			// Enqueue work to this index
			var workerEnqueueIdx = 0;
			#endregion

			// Enqueue all of the work into the parallel workers
			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;

				count += archetype.EntityCount;

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entityCount = chunk.EntityCount;
					var numBatches = (int)Math.Ceiling(entityCount / (float)batchSize);

					// Inrement work counter for all of the batches we're about to create
					workCounter.AddCount(numBatches);

					var t0 = chunk.GetComponentArray<T0>(c0);
					var t1 = chunk.GetComponentArray<T1>(c1);
					var t2 = chunk.GetComponentArray<T2>(c2);
					var t3 = chunk.GetComponentArray<T3>(c3);
					var t4 = chunk.GetComponentArray<T4>(c4);
					for (var b = 0; b < numBatches; b++)
					{
						var start = b * batchSize;
						var end = Math.Min(start + batchSize, entityCount);
						var batchCount = end - start;
						var eMem = chunk.Entities.Slice(start, batchCount);

						var item = new WorkItem5<TQ, T0, T1, T2, T3, T4>(
							eMem,
							t0.AsMemory(start, batchCount),
							t1.AsMemory(start, batchCount),
							t2.AsMemory(start, batchCount),
							t3.AsMemory(start, batchCount),
							t4.AsMemory(start, batchCount),
							q
						);

						#region Parallel Work Loop Add To Queue
						// Add work to a worker
						workersArr[workerEnqueueIdx]!.Enqueue(item);

						workerEnqueueIdx++;
						if (workerEnqueueIdx >= workers.Length)
							workerEnqueueIdx = 0;
						#endregion
					}
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
			ArrayPool<ParallelQueryWorker<WorkItem5<TQ, T0, T1, T2, T3, T4>>>.Shared.Return(workersArr!);

			// Throw collected exceptions
			if (exceptions is { Count: > 0 })
				throw new AggregateException(exceptions);
			#endregion

			return count;
		}

		[ExcludeFromCodeCoverage]
		private readonly struct WorkItem5<TQ, T0, T1, T2, T3, T4>
			: IWorkItem
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
			where TQ : IQuery<T0, T1, T2, T3, T4>
		{
			private readonly TQ _q;

			private readonly ReadOnlyMemory<Entity> _entities;
			private readonly Memory<T0> _c0;
			private readonly Memory<T1> _c1;
			private readonly Memory<T2> _c2;
			private readonly Memory<T3> _c3;
			private readonly Memory<T4> _c4;

			public WorkItem5(
				ReadOnlyMemory<Entity> entities,
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

			public void Execute()
			{
				var eSpan = _entities.Span;
				var c0 = _c0.Span;
				var c1 = _c1.Span;
				var c2 = _c2.Span;
				var c3 = _c3.Span;
				var c4 = _c4.Span;

				for (var i = 0; i < _entities.Length; i++)
				{
					_q.Execute(
						eSpan[i]
						, ref c0[i]
						, ref c1[i]
						, ref c2[i]
						, ref c3[i]
						, ref c4[i]
					);
				}
			}
		}
	}
}
namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A query which accepts 6 components
	/// </summary>
	public interface IQuery<T0, T1, T2, T3, T4, T5>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
	{
		/// <summary>
		/// Execute the query for a single entity
		/// </summary>
		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5>(
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
			where TQ : IQuery<T0, T1, T2, T3, T4, T5>, new()
		{
			var q = new TQ();
			return Execute<TQ, T0, T1, T2, T3, T4, T5>(ref q, query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5>(
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
			where TQ : IQuery<T0, T1, T2, T3, T4, T5>, new()
		{
			var q = new TQ();
			return Execute<TQ, T0, T1, T2, T3, T4, T5>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <param name="q">The instance to execute over every entity.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5>
		{
			return Execute<TQ, T0, T1, T2, T3, T4, T5>(ref q, query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <param name="q">The instance to execute over every entity.</param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5>(
			TQ q,
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
			where TQ : IQuery<T0, T1, T2, T3, T4, T5>
		{
			return Execute<TQ, T0, T1, T2, T3, T4, T5>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <param name="q">
		/// The instance to execute over every entity. Passed by ref, so changes to the query
		/// struct will be persistent. This can allow values from one entity to be accessed by
		/// the next entity, or after the entire Execute call is complete.
		/// </param>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5>(
			ref TQ q,
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
			where TQ : IQuery<T0, T1, T2, T3, T4, T5>
		{
			return Execute<TQ, T0, T1, T2, T3, T4, T5>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <param name="q">
		/// The instance to execute over every entity. Passed by ref, so changes to the query
		/// struct will be persistent. This can allow values from one entity to be accessed by
		/// the next entity, or after the entire Execute call is complete.
		/// </param>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5>(
			ref TQ q,
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
			where TQ : IQuery<T0, T1, T2, T3, T4, T5>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5>();

			var archetypes = query.GetArchetypes();

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

				count += archetype.EntityCount;

				using (var enumerator = archetype.GetChunkEnumerator())
				{
					while (enumerator.MoveNext())
					{
						var chunk = enumerator.Current;
						Debug.Assert(chunk != null);

						var entities = chunk.Entities.Span;

						var t0 = chunk.GetSpan<T0>(c0);
						Debug.Assert(t0.Length == entities.Length);
						var t1 = chunk.GetSpan<T1>(c1);
						Debug.Assert(t1.Length == entities.Length);
						var t2 = chunk.GetSpan<T2>(c2);
						Debug.Assert(t2.Length == entities.Length);
						var t3 = chunk.GetSpan<T3>(c3);
						Debug.Assert(t3.Length == entities.Length);
						var t4 = chunk.GetSpan<T4>(c4);
						Debug.Assert(t4.Length == entities.Length);
						var t5 = chunk.GetSpan<T5>(c5);
						Debug.Assert(t5.Length == entities.Length);

						unsafe
						{
							#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
							fixed (Entity* eptr = entities)
							fixed (T0* t0ptr = t0)
							fixed (T1* t1ptr = t1)
							fixed (T2* t2ptr = t2)
							fixed (T3* t3ptr = t3)
							fixed (T4* t4ptr = t4)
							fixed (T5* t5ptr = t5)
							#pragma warning restore CS8500
							{
								for (var i = 0; i < entities.Length; i++)
									q.Execute(eptr[i], ref t0ptr[i], ref t1ptr[i], ref t2ptr[i], ref t3ptr[i], ref t4ptr[i], ref t5ptr[i]);
							}
						}
					}
				}
			}

			return count;
		}

		/// <summary>
		/// Execute a query in parallel over entities, blocks until complete.
		/// </summary>
		/// <param name="q"></param>
		/// <param name="query"></param>
		/// <param name="batchSize"></param>
		/// <returns></returns>
		/// <exception cref="AggregateException"></exception>
		[ExcludeFromCodeCoverage]
		public int ExecuteParallel<TQ, T0, T1, T2, T3, T4, T5>(
			TQ q,
			QueryDescription? query = null,
			int batchSize = 128
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
			where TQ : IQuery<T0, T1, T2, T3, T4, T5>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5>();

			var archetypes = query.GetArchetypes();

			// Early exit if there is no work to do, avoiding the cost of setting up the worker pool
			if (!query.Any())
				return 0;

			batchSize = Math.Clamp(batchSize, 1, Archetype.CHUNK_SIZE);

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;
			var c2 = ComponentID<T2>.ID;
			var c3 = ComponentID<T3>.ID;
			var c4 = ComponentID<T4>.ID;
			var c5 = ComponentID<T5>.ID;

			#region Parallel Work Loop Setup
			// Borrow a counter which will be used to keep track of all in-progress work
			using var workCounterRental = Pool<CountdownEventContainer>.Rent();
			var workCounter = workCounterRental.Value.Event;
			workCounter.Reset(1);

			// Create parallel workers
			var processors = ThreadPool.Threads;
			var workersArr = ArrayPool<ParallelQueryWorker<WorkItem6<TQ, T0, T1, T2, T3, T4, T5>>?>.Shared.Rent(processors);
			var workers = workersArr.AsMemory(0, processors);
			for (var i = 0; i < workers.Length; i++)
			{
				workersArr[i] = Pool<ParallelQueryWorker<WorkItem6<TQ, T0, T1, T2, T3, T4, T5>>>.Get();
				workersArr[i]!.Configure(workersArr, workCounter);
			}

			// Start the workers. Even though there is no work they cannot exit yet because
			// the counter was reset to 1 above.
			foreach (var item in workersArr)
			{
				if (item != null)
					ThreadPool.QueueUserWorkItem(item);
			}

			// Enqueue work to this index
			var workerEnqueueIdx = 0;
			#endregion

			// Enqueue all of the work into the parallel workers
			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;

				count += archetype.EntityCount;

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entityCount = chunk.EntityCount;
					var numBatches = (int)Math.Ceiling(entityCount / (float)batchSize);

					// Inrement work counter for all of the batches we're about to create
					workCounter.AddCount(numBatches);

					var t0 = chunk.GetComponentArray<T0>(c0);
					var t1 = chunk.GetComponentArray<T1>(c1);
					var t2 = chunk.GetComponentArray<T2>(c2);
					var t3 = chunk.GetComponentArray<T3>(c3);
					var t4 = chunk.GetComponentArray<T4>(c4);
					var t5 = chunk.GetComponentArray<T5>(c5);
					for (var b = 0; b < numBatches; b++)
					{
						var start = b * batchSize;
						var end = Math.Min(start + batchSize, entityCount);
						var batchCount = end - start;
						var eMem = chunk.Entities.Slice(start, batchCount);

						var item = new WorkItem6<TQ, T0, T1, T2, T3, T4, T5>(
							eMem,
							t0.AsMemory(start, batchCount),
							t1.AsMemory(start, batchCount),
							t2.AsMemory(start, batchCount),
							t3.AsMemory(start, batchCount),
							t4.AsMemory(start, batchCount),
							t5.AsMemory(start, batchCount),
							q
						);

						#region Parallel Work Loop Add To Queue
						// Add work to a worker
						workersArr[workerEnqueueIdx]!.Enqueue(item);

						workerEnqueueIdx++;
						if (workerEnqueueIdx >= workers.Length)
							workerEnqueueIdx = 0;
						#endregion
					}
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
			ArrayPool<ParallelQueryWorker<WorkItem6<TQ, T0, T1, T2, T3, T4, T5>>>.Shared.Return(workersArr!);

			// Throw collected exceptions
			if (exceptions is { Count: > 0 })
				throw new AggregateException(exceptions);
			#endregion

			return count;
		}

		[ExcludeFromCodeCoverage]
		private readonly struct WorkItem6<TQ, T0, T1, T2, T3, T4, T5>
			: IWorkItem
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
			where TQ : IQuery<T0, T1, T2, T3, T4, T5>
		{
			private readonly TQ _q;

			private readonly ReadOnlyMemory<Entity> _entities;
			private readonly Memory<T0> _c0;
			private readonly Memory<T1> _c1;
			private readonly Memory<T2> _c2;
			private readonly Memory<T3> _c3;
			private readonly Memory<T4> _c4;
			private readonly Memory<T5> _c5;

			public WorkItem6(
				ReadOnlyMemory<Entity> entities,
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

			public void Execute()
			{
				var eSpan = _entities.Span;
				var c0 = _c0.Span;
				var c1 = _c1.Span;
				var c2 = _c2.Span;
				var c3 = _c3.Span;
				var c4 = _c4.Span;
				var c5 = _c5.Span;

				for (var i = 0; i < _entities.Length; i++)
				{
					_q.Execute(
						eSpan[i]
						, ref c0[i]
						, ref c1[i]
						, ref c2[i]
						, ref c3[i]
						, ref c4[i]
						, ref c5[i]
					);
				}
			}
		}
	}
}
namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A query which accepts 7 components
	/// </summary>
	public interface IQuery<T0, T1, T2, T3, T4, T5, T6>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
	{
		/// <summary>
		/// Execute the query for a single entity
		/// </summary>
		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6>(
			QueryDescription? query = null
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6>, new()
		{
			var q = new TQ();
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6>(ref q, query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6>(
			ref QueryDescription? query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6>, new()
		{
			var q = new TQ();
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <param name="q">The instance to execute over every entity.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6>
		{
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6>(ref q, query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <param name="q">The instance to execute over every entity.</param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6>
		{
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <param name="q">
		/// The instance to execute over every entity. Passed by ref, so changes to the query
		/// struct will be persistent. This can allow values from one entity to be accessed by
		/// the next entity, or after the entire Execute call is complete.
		/// </param>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6>
		{
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <param name="q">
		/// The instance to execute over every entity. Passed by ref, so changes to the query
		/// struct will be persistent. This can allow values from one entity to be accessed by
		/// the next entity, or after the entire Execute call is complete.
		/// </param>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6>
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

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;

				count += archetype.EntityCount;

				using (var enumerator = archetype.GetChunkEnumerator())
				{
					while (enumerator.MoveNext())
					{
						var chunk = enumerator.Current;
						Debug.Assert(chunk != null);

						var entities = chunk.Entities.Span;

						var t0 = chunk.GetSpan<T0>(c0);
						Debug.Assert(t0.Length == entities.Length);
						var t1 = chunk.GetSpan<T1>(c1);
						Debug.Assert(t1.Length == entities.Length);
						var t2 = chunk.GetSpan<T2>(c2);
						Debug.Assert(t2.Length == entities.Length);
						var t3 = chunk.GetSpan<T3>(c3);
						Debug.Assert(t3.Length == entities.Length);
						var t4 = chunk.GetSpan<T4>(c4);
						Debug.Assert(t4.Length == entities.Length);
						var t5 = chunk.GetSpan<T5>(c5);
						Debug.Assert(t5.Length == entities.Length);
						var t6 = chunk.GetSpan<T6>(c6);
						Debug.Assert(t6.Length == entities.Length);

						unsafe
						{
							#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
							fixed (Entity* eptr = entities)
							fixed (T0* t0ptr = t0)
							fixed (T1* t1ptr = t1)
							fixed (T2* t2ptr = t2)
							fixed (T3* t3ptr = t3)
							fixed (T4* t4ptr = t4)
							fixed (T5* t5ptr = t5)
							fixed (T6* t6ptr = t6)
							#pragma warning restore CS8500
							{
								for (var i = 0; i < entities.Length; i++)
									q.Execute(eptr[i], ref t0ptr[i], ref t1ptr[i], ref t2ptr[i], ref t3ptr[i], ref t4ptr[i], ref t5ptr[i], ref t6ptr[i]);
							}
						}
					}
				}
			}

			return count;
		}

		/// <summary>
		/// Execute a query in parallel over entities, blocks until complete.
		/// </summary>
		/// <param name="q"></param>
		/// <param name="query"></param>
		/// <param name="batchSize"></param>
		/// <returns></returns>
		/// <exception cref="AggregateException"></exception>
		[ExcludeFromCodeCoverage]
		public int ExecuteParallel<TQ, T0, T1, T2, T3, T4, T5, T6>(
			TQ q,
			QueryDescription? query = null,
			int batchSize = 128
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6>();

			var archetypes = query.GetArchetypes();

			// Early exit if there is no work to do, avoiding the cost of setting up the worker pool
			if (!query.Any())
				return 0;

			batchSize = Math.Clamp(batchSize, 1, Archetype.CHUNK_SIZE);

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;
			var c2 = ComponentID<T2>.ID;
			var c3 = ComponentID<T3>.ID;
			var c4 = ComponentID<T4>.ID;
			var c5 = ComponentID<T5>.ID;
			var c6 = ComponentID<T6>.ID;

			#region Parallel Work Loop Setup
			// Borrow a counter which will be used to keep track of all in-progress work
			using var workCounterRental = Pool<CountdownEventContainer>.Rent();
			var workCounter = workCounterRental.Value.Event;
			workCounter.Reset(1);

			// Create parallel workers
			var processors = ThreadPool.Threads;
			var workersArr = ArrayPool<ParallelQueryWorker<WorkItem7<TQ, T0, T1, T2, T3, T4, T5, T6>>?>.Shared.Rent(processors);
			var workers = workersArr.AsMemory(0, processors);
			for (var i = 0; i < workers.Length; i++)
			{
				workersArr[i] = Pool<ParallelQueryWorker<WorkItem7<TQ, T0, T1, T2, T3, T4, T5, T6>>>.Get();
				workersArr[i]!.Configure(workersArr, workCounter);
			}

			// Start the workers. Even though there is no work they cannot exit yet because
			// the counter was reset to 1 above.
			foreach (var item in workersArr)
			{
				if (item != null)
					ThreadPool.QueueUserWorkItem(item);
			}

			// Enqueue work to this index
			var workerEnqueueIdx = 0;
			#endregion

			// Enqueue all of the work into the parallel workers
			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;

				count += archetype.EntityCount;

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entityCount = chunk.EntityCount;
					var numBatches = (int)Math.Ceiling(entityCount / (float)batchSize);

					// Inrement work counter for all of the batches we're about to create
					workCounter.AddCount(numBatches);

					var t0 = chunk.GetComponentArray<T0>(c0);
					var t1 = chunk.GetComponentArray<T1>(c1);
					var t2 = chunk.GetComponentArray<T2>(c2);
					var t3 = chunk.GetComponentArray<T3>(c3);
					var t4 = chunk.GetComponentArray<T4>(c4);
					var t5 = chunk.GetComponentArray<T5>(c5);
					var t6 = chunk.GetComponentArray<T6>(c6);
					for (var b = 0; b < numBatches; b++)
					{
						var start = b * batchSize;
						var end = Math.Min(start + batchSize, entityCount);
						var batchCount = end - start;
						var eMem = chunk.Entities.Slice(start, batchCount);

						var item = new WorkItem7<TQ, T0, T1, T2, T3, T4, T5, T6>(
							eMem,
							t0.AsMemory(start, batchCount),
							t1.AsMemory(start, batchCount),
							t2.AsMemory(start, batchCount),
							t3.AsMemory(start, batchCount),
							t4.AsMemory(start, batchCount),
							t5.AsMemory(start, batchCount),
							t6.AsMemory(start, batchCount),
							q
						);

						#region Parallel Work Loop Add To Queue
						// Add work to a worker
						workersArr[workerEnqueueIdx]!.Enqueue(item);

						workerEnqueueIdx++;
						if (workerEnqueueIdx >= workers.Length)
							workerEnqueueIdx = 0;
						#endregion
					}
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
			ArrayPool<ParallelQueryWorker<WorkItem7<TQ, T0, T1, T2, T3, T4, T5, T6>>>.Shared.Return(workersArr!);

			// Throw collected exceptions
			if (exceptions is { Count: > 0 })
				throw new AggregateException(exceptions);
			#endregion

			return count;
		}

		[ExcludeFromCodeCoverage]
		private readonly struct WorkItem7<TQ, T0, T1, T2, T3, T4, T5, T6>
			: IWorkItem
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6>
		{
			private readonly TQ _q;

			private readonly ReadOnlyMemory<Entity> _entities;
			private readonly Memory<T0> _c0;
			private readonly Memory<T1> _c1;
			private readonly Memory<T2> _c2;
			private readonly Memory<T3> _c3;
			private readonly Memory<T4> _c4;
			private readonly Memory<T5> _c5;
			private readonly Memory<T6> _c6;

			public WorkItem7(
				ReadOnlyMemory<Entity> entities,
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

			public void Execute()
			{
				var eSpan = _entities.Span;
				var c0 = _c0.Span;
				var c1 = _c1.Span;
				var c2 = _c2.Span;
				var c3 = _c3.Span;
				var c4 = _c4.Span;
				var c5 = _c5.Span;
				var c6 = _c6.Span;

				for (var i = 0; i < _entities.Length; i++)
				{
					_q.Execute(
						eSpan[i]
						, ref c0[i]
						, ref c1[i]
						, ref c2[i]
						, ref c3[i]
						, ref c4[i]
						, ref c5[i]
						, ref c6[i]
					);
				}
			}
		}
	}
}
namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A query which accepts 8 components
	/// </summary>
	public interface IQuery<T0, T1, T2, T3, T4, T5, T6, T7>
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
		/// Execute the query for a single entity
		/// </summary>
		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7>, new()
		{
			var q = new TQ();
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7>(ref q, query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7>, new()
		{
			var q = new TQ();
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		/// <param name="q">The instance to execute over every entity.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7>
		{
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7>(ref q, query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <param name="q">The instance to execute over every entity.</param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7>
		{
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		/// <param name="q">
		/// The instance to execute over every entity. Passed by ref, so changes to the query
		/// struct will be persistent. This can allow values from one entity to be accessed by
		/// the next entity, or after the entire Execute call is complete.
		/// </param>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7>
		{
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		/// <param name="q">
		/// The instance to execute over every entity. Passed by ref, so changes to the query
		/// struct will be persistent. This can allow values from one entity to be accessed by
		/// the next entity, or after the entire Execute call is complete.
		/// </param>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7>
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

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;

				count += archetype.EntityCount;

				using (var enumerator = archetype.GetChunkEnumerator())
				{
					while (enumerator.MoveNext())
					{
						var chunk = enumerator.Current;
						Debug.Assert(chunk != null);

						var entities = chunk.Entities.Span;

						var t0 = chunk.GetSpan<T0>(c0);
						Debug.Assert(t0.Length == entities.Length);
						var t1 = chunk.GetSpan<T1>(c1);
						Debug.Assert(t1.Length == entities.Length);
						var t2 = chunk.GetSpan<T2>(c2);
						Debug.Assert(t2.Length == entities.Length);
						var t3 = chunk.GetSpan<T3>(c3);
						Debug.Assert(t3.Length == entities.Length);
						var t4 = chunk.GetSpan<T4>(c4);
						Debug.Assert(t4.Length == entities.Length);
						var t5 = chunk.GetSpan<T5>(c5);
						Debug.Assert(t5.Length == entities.Length);
						var t6 = chunk.GetSpan<T6>(c6);
						Debug.Assert(t6.Length == entities.Length);
						var t7 = chunk.GetSpan<T7>(c7);
						Debug.Assert(t7.Length == entities.Length);

						unsafe
						{
							#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
							fixed (Entity* eptr = entities)
							fixed (T0* t0ptr = t0)
							fixed (T1* t1ptr = t1)
							fixed (T2* t2ptr = t2)
							fixed (T3* t3ptr = t3)
							fixed (T4* t4ptr = t4)
							fixed (T5* t5ptr = t5)
							fixed (T6* t6ptr = t6)
							fixed (T7* t7ptr = t7)
							#pragma warning restore CS8500
							{
								for (var i = 0; i < entities.Length; i++)
									q.Execute(eptr[i], ref t0ptr[i], ref t1ptr[i], ref t2ptr[i], ref t3ptr[i], ref t4ptr[i], ref t5ptr[i], ref t6ptr[i], ref t7ptr[i]);
							}
						}
					}
				}
			}

			return count;
		}

		/// <summary>
		/// Execute a query in parallel over entities, blocks until complete.
		/// </summary>
		/// <param name="q"></param>
		/// <param name="query"></param>
		/// <param name="batchSize"></param>
		/// <returns></returns>
		/// <exception cref="AggregateException"></exception>
		[ExcludeFromCodeCoverage]
		public int ExecuteParallel<TQ, T0, T1, T2, T3, T4, T5, T6, T7>(
			TQ q,
			QueryDescription? query = null,
			int batchSize = 128
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7>();

			var archetypes = query.GetArchetypes();

			// Early exit if there is no work to do, avoiding the cost of setting up the worker pool
			if (!query.Any())
				return 0;

			batchSize = Math.Clamp(batchSize, 1, Archetype.CHUNK_SIZE);

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;
			var c2 = ComponentID<T2>.ID;
			var c3 = ComponentID<T3>.ID;
			var c4 = ComponentID<T4>.ID;
			var c5 = ComponentID<T5>.ID;
			var c6 = ComponentID<T6>.ID;
			var c7 = ComponentID<T7>.ID;

			#region Parallel Work Loop Setup
			// Borrow a counter which will be used to keep track of all in-progress work
			using var workCounterRental = Pool<CountdownEventContainer>.Rent();
			var workCounter = workCounterRental.Value.Event;
			workCounter.Reset(1);

			// Create parallel workers
			var processors = ThreadPool.Threads;
			var workersArr = ArrayPool<ParallelQueryWorker<WorkItem8<TQ, T0, T1, T2, T3, T4, T5, T6, T7>>?>.Shared.Rent(processors);
			var workers = workersArr.AsMemory(0, processors);
			for (var i = 0; i < workers.Length; i++)
			{
				workersArr[i] = Pool<ParallelQueryWorker<WorkItem8<TQ, T0, T1, T2, T3, T4, T5, T6, T7>>>.Get();
				workersArr[i]!.Configure(workersArr, workCounter);
			}

			// Start the workers. Even though there is no work they cannot exit yet because
			// the counter was reset to 1 above.
			foreach (var item in workersArr)
			{
				if (item != null)
					ThreadPool.QueueUserWorkItem(item);
			}

			// Enqueue work to this index
			var workerEnqueueIdx = 0;
			#endregion

			// Enqueue all of the work into the parallel workers
			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;

				count += archetype.EntityCount;

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entityCount = chunk.EntityCount;
					var numBatches = (int)Math.Ceiling(entityCount / (float)batchSize);

					// Inrement work counter for all of the batches we're about to create
					workCounter.AddCount(numBatches);

					var t0 = chunk.GetComponentArray<T0>(c0);
					var t1 = chunk.GetComponentArray<T1>(c1);
					var t2 = chunk.GetComponentArray<T2>(c2);
					var t3 = chunk.GetComponentArray<T3>(c3);
					var t4 = chunk.GetComponentArray<T4>(c4);
					var t5 = chunk.GetComponentArray<T5>(c5);
					var t6 = chunk.GetComponentArray<T6>(c6);
					var t7 = chunk.GetComponentArray<T7>(c7);
					for (var b = 0; b < numBatches; b++)
					{
						var start = b * batchSize;
						var end = Math.Min(start + batchSize, entityCount);
						var batchCount = end - start;
						var eMem = chunk.Entities.Slice(start, batchCount);

						var item = new WorkItem8<TQ, T0, T1, T2, T3, T4, T5, T6, T7>(
							eMem,
							t0.AsMemory(start, batchCount),
							t1.AsMemory(start, batchCount),
							t2.AsMemory(start, batchCount),
							t3.AsMemory(start, batchCount),
							t4.AsMemory(start, batchCount),
							t5.AsMemory(start, batchCount),
							t6.AsMemory(start, batchCount),
							t7.AsMemory(start, batchCount),
							q
						);

						#region Parallel Work Loop Add To Queue
						// Add work to a worker
						workersArr[workerEnqueueIdx]!.Enqueue(item);

						workerEnqueueIdx++;
						if (workerEnqueueIdx >= workers.Length)
							workerEnqueueIdx = 0;
						#endregion
					}
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
			ArrayPool<ParallelQueryWorker<WorkItem8<TQ, T0, T1, T2, T3, T4, T5, T6, T7>>>.Shared.Return(workersArr!);

			// Throw collected exceptions
			if (exceptions is { Count: > 0 })
				throw new AggregateException(exceptions);
			#endregion

			return count;
		}

		[ExcludeFromCodeCoverage]
		private readonly struct WorkItem8<TQ, T0, T1, T2, T3, T4, T5, T6, T7>
			: IWorkItem
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7>
		{
			private readonly TQ _q;

			private readonly ReadOnlyMemory<Entity> _entities;
			private readonly Memory<T0> _c0;
			private readonly Memory<T1> _c1;
			private readonly Memory<T2> _c2;
			private readonly Memory<T3> _c3;
			private readonly Memory<T4> _c4;
			private readonly Memory<T5> _c5;
			private readonly Memory<T6> _c6;
			private readonly Memory<T7> _c7;

			public WorkItem8(
				ReadOnlyMemory<Entity> entities,
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

			public void Execute()
			{
				var eSpan = _entities.Span;
				var c0 = _c0.Span;
				var c1 = _c1.Span;
				var c2 = _c2.Span;
				var c3 = _c3.Span;
				var c4 = _c4.Span;
				var c5 = _c5.Span;
				var c6 = _c6.Span;
				var c7 = _c7.Span;

				for (var i = 0; i < _entities.Length; i++)
				{
					_q.Execute(
						eSpan[i]
						, ref c0[i]
						, ref c1[i]
						, ref c2[i]
						, ref c3[i]
						, ref c4[i]
						, ref c5[i]
						, ref c6[i]
						, ref c7[i]
					);
				}
			}
		}
	}
}
namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A query which accepts 9 components
	/// </summary>
	public interface IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8>
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
		/// Execute the query for a single entity
		/// </summary>
		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		/// <typeparam name="T8">Component 8 to include in query</typeparam>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8>, new()
		{
			var q = new TQ();
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8>(ref q, query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		/// <typeparam name="T8">Component 8 to include in query</typeparam>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8>, new()
		{
			var q = new TQ();
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		/// <typeparam name="T8">Component 8 to include in query</typeparam>
		/// <param name="q">The instance to execute over every entity.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8>
		{
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8>(ref q, query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		/// <typeparam name="T8">Component 8 to include in query</typeparam>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <param name="q">The instance to execute over every entity.</param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8>
		{
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		/// <typeparam name="T8">Component 8 to include in query</typeparam>
		/// <param name="q">
		/// The instance to execute over every entity. Passed by ref, so changes to the query
		/// struct will be persistent. This can allow values from one entity to be accessed by
		/// the next entity, or after the entire Execute call is complete.
		/// </param>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8>
		{
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
		/// <typeparam name="T0">Component 0 to include in query</typeparam>
		/// <typeparam name="T1">Component 1 to include in query</typeparam>
		/// <typeparam name="T2">Component 2 to include in query</typeparam>
		/// <typeparam name="T3">Component 3 to include in query</typeparam>
		/// <typeparam name="T4">Component 4 to include in query</typeparam>
		/// <typeparam name="T5">Component 5 to include in query</typeparam>
		/// <typeparam name="T6">Component 6 to include in query</typeparam>
		/// <typeparam name="T7">Component 7 to include in query</typeparam>
		/// <typeparam name="T8">Component 8 to include in query</typeparam>
		/// <param name="q">
		/// The instance to execute over every entity. Passed by ref, so changes to the query
		/// struct will be persistent. This can allow values from one entity to be accessed by
		/// the next entity, or after the entire Execute call is complete.
		/// </param>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8>
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

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;

				count += archetype.EntityCount;

				using (var enumerator = archetype.GetChunkEnumerator())
				{
					while (enumerator.MoveNext())
					{
						var chunk = enumerator.Current;
						Debug.Assert(chunk != null);

						var entities = chunk.Entities.Span;

						var t0 = chunk.GetSpan<T0>(c0);
						Debug.Assert(t0.Length == entities.Length);
						var t1 = chunk.GetSpan<T1>(c1);
						Debug.Assert(t1.Length == entities.Length);
						var t2 = chunk.GetSpan<T2>(c2);
						Debug.Assert(t2.Length == entities.Length);
						var t3 = chunk.GetSpan<T3>(c3);
						Debug.Assert(t3.Length == entities.Length);
						var t4 = chunk.GetSpan<T4>(c4);
						Debug.Assert(t4.Length == entities.Length);
						var t5 = chunk.GetSpan<T5>(c5);
						Debug.Assert(t5.Length == entities.Length);
						var t6 = chunk.GetSpan<T6>(c6);
						Debug.Assert(t6.Length == entities.Length);
						var t7 = chunk.GetSpan<T7>(c7);
						Debug.Assert(t7.Length == entities.Length);
						var t8 = chunk.GetSpan<T8>(c8);
						Debug.Assert(t8.Length == entities.Length);

						unsafe
						{
							#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
							fixed (Entity* eptr = entities)
							fixed (T0* t0ptr = t0)
							fixed (T1* t1ptr = t1)
							fixed (T2* t2ptr = t2)
							fixed (T3* t3ptr = t3)
							fixed (T4* t4ptr = t4)
							fixed (T5* t5ptr = t5)
							fixed (T6* t6ptr = t6)
							fixed (T7* t7ptr = t7)
							fixed (T8* t8ptr = t8)
							#pragma warning restore CS8500
							{
								for (var i = 0; i < entities.Length; i++)
									q.Execute(eptr[i], ref t0ptr[i], ref t1ptr[i], ref t2ptr[i], ref t3ptr[i], ref t4ptr[i], ref t5ptr[i], ref t6ptr[i], ref t7ptr[i], ref t8ptr[i]);
							}
						}
					}
				}
			}

			return count;
		}

		/// <summary>
		/// Execute a query in parallel over entities, blocks until complete.
		/// </summary>
		/// <param name="q"></param>
		/// <param name="query"></param>
		/// <param name="batchSize"></param>
		/// <returns></returns>
		/// <exception cref="AggregateException"></exception>
		[ExcludeFromCodeCoverage]
		public int ExecuteParallel<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
			TQ q,
			QueryDescription? query = null,
			int batchSize = 128
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8>();

			var archetypes = query.GetArchetypes();

			// Early exit if there is no work to do, avoiding the cost of setting up the worker pool
			if (!query.Any())
				return 0;

			batchSize = Math.Clamp(batchSize, 1, Archetype.CHUNK_SIZE);

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;
			var c2 = ComponentID<T2>.ID;
			var c3 = ComponentID<T3>.ID;
			var c4 = ComponentID<T4>.ID;
			var c5 = ComponentID<T5>.ID;
			var c6 = ComponentID<T6>.ID;
			var c7 = ComponentID<T7>.ID;
			var c8 = ComponentID<T8>.ID;

			#region Parallel Work Loop Setup
			// Borrow a counter which will be used to keep track of all in-progress work
			using var workCounterRental = Pool<CountdownEventContainer>.Rent();
			var workCounter = workCounterRental.Value.Event;
			workCounter.Reset(1);

			// Create parallel workers
			var processors = ThreadPool.Threads;
			var workersArr = ArrayPool<ParallelQueryWorker<WorkItem9<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8>>?>.Shared.Rent(processors);
			var workers = workersArr.AsMemory(0, processors);
			for (var i = 0; i < workers.Length; i++)
			{
				workersArr[i] = Pool<ParallelQueryWorker<WorkItem9<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8>>>.Get();
				workersArr[i]!.Configure(workersArr, workCounter);
			}

			// Start the workers. Even though there is no work they cannot exit yet because
			// the counter was reset to 1 above.
			foreach (var item in workersArr)
			{
				if (item != null)
					ThreadPool.QueueUserWorkItem(item);
			}

			// Enqueue work to this index
			var workerEnqueueIdx = 0;
			#endregion

			// Enqueue all of the work into the parallel workers
			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;

				count += archetype.EntityCount;

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entityCount = chunk.EntityCount;
					var numBatches = (int)Math.Ceiling(entityCount / (float)batchSize);

					// Inrement work counter for all of the batches we're about to create
					workCounter.AddCount(numBatches);

					var t0 = chunk.GetComponentArray<T0>(c0);
					var t1 = chunk.GetComponentArray<T1>(c1);
					var t2 = chunk.GetComponentArray<T2>(c2);
					var t3 = chunk.GetComponentArray<T3>(c3);
					var t4 = chunk.GetComponentArray<T4>(c4);
					var t5 = chunk.GetComponentArray<T5>(c5);
					var t6 = chunk.GetComponentArray<T6>(c6);
					var t7 = chunk.GetComponentArray<T7>(c7);
					var t8 = chunk.GetComponentArray<T8>(c8);
					for (var b = 0; b < numBatches; b++)
					{
						var start = b * batchSize;
						var end = Math.Min(start + batchSize, entityCount);
						var batchCount = end - start;
						var eMem = chunk.Entities.Slice(start, batchCount);

						var item = new WorkItem9<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
							eMem,
							t0.AsMemory(start, batchCount),
							t1.AsMemory(start, batchCount),
							t2.AsMemory(start, batchCount),
							t3.AsMemory(start, batchCount),
							t4.AsMemory(start, batchCount),
							t5.AsMemory(start, batchCount),
							t6.AsMemory(start, batchCount),
							t7.AsMemory(start, batchCount),
							t8.AsMemory(start, batchCount),
							q
						);

						#region Parallel Work Loop Add To Queue
						// Add work to a worker
						workersArr[workerEnqueueIdx]!.Enqueue(item);

						workerEnqueueIdx++;
						if (workerEnqueueIdx >= workers.Length)
							workerEnqueueIdx = 0;
						#endregion
					}
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
			ArrayPool<ParallelQueryWorker<WorkItem9<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8>>>.Shared.Return(workersArr!);

			// Throw collected exceptions
			if (exceptions is { Count: > 0 })
				throw new AggregateException(exceptions);
			#endregion

			return count;
		}

		[ExcludeFromCodeCoverage]
		private readonly struct WorkItem9<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8>
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8>
		{
			private readonly TQ _q;

			private readonly ReadOnlyMemory<Entity> _entities;
			private readonly Memory<T0> _c0;
			private readonly Memory<T1> _c1;
			private readonly Memory<T2> _c2;
			private readonly Memory<T3> _c3;
			private readonly Memory<T4> _c4;
			private readonly Memory<T5> _c5;
			private readonly Memory<T6> _c6;
			private readonly Memory<T7> _c7;
			private readonly Memory<T8> _c8;

			public WorkItem9(
				ReadOnlyMemory<Entity> entities,
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

			public void Execute()
			{
				var eSpan = _entities.Span;
				var c0 = _c0.Span;
				var c1 = _c1.Span;
				var c2 = _c2.Span;
				var c3 = _c3.Span;
				var c4 = _c4.Span;
				var c5 = _c5.Span;
				var c6 = _c6.Span;
				var c7 = _c7.Span;
				var c8 = _c8.Span;

				for (var i = 0; i < _entities.Length; i++)
				{
					_q.Execute(
						eSpan[i]
						, ref c0[i]
						, ref c1[i]
						, ref c2[i]
						, ref c3[i]
						, ref c4[i]
						, ref c5[i]
						, ref c6[i]
						, ref c7[i]
						, ref c8[i]
					);
				}
			}
		}
	}
}
namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A query which accepts 10 components
	/// </summary>
	public interface IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
		where T0 : IComponent
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
		/// Execute the query for a single entity
		/// </summary>
		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
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
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>, new()
		{
			var q = new TQ();
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(ref q, query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
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
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>, new()
		{
			var q = new TQ();
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
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
		/// <param name="q">The instance to execute over every entity.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
		{
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(ref q, query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
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
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <param name="q">The instance to execute over every entity.</param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
		{
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
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
		/// <param name="q">
		/// The instance to execute over every entity. Passed by ref, so changes to the query
		/// struct will be persistent. This can allow values from one entity to be accessed by
		/// the next entity, or after the entire Execute call is complete.
		/// </param>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
		{
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
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
		/// <param name="q">
		/// The instance to execute over every entity. Passed by ref, so changes to the query
		/// struct will be persistent. This can allow values from one entity to be accessed by
		/// the next entity, or after the entire Execute call is complete.
		/// </param>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
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

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;

				count += archetype.EntityCount;

				using (var enumerator = archetype.GetChunkEnumerator())
				{
					while (enumerator.MoveNext())
					{
						var chunk = enumerator.Current;
						Debug.Assert(chunk != null);

						var entities = chunk.Entities.Span;

						var t0 = chunk.GetSpan<T0>(c0);
						Debug.Assert(t0.Length == entities.Length);
						var t1 = chunk.GetSpan<T1>(c1);
						Debug.Assert(t1.Length == entities.Length);
						var t2 = chunk.GetSpan<T2>(c2);
						Debug.Assert(t2.Length == entities.Length);
						var t3 = chunk.GetSpan<T3>(c3);
						Debug.Assert(t3.Length == entities.Length);
						var t4 = chunk.GetSpan<T4>(c4);
						Debug.Assert(t4.Length == entities.Length);
						var t5 = chunk.GetSpan<T5>(c5);
						Debug.Assert(t5.Length == entities.Length);
						var t6 = chunk.GetSpan<T6>(c6);
						Debug.Assert(t6.Length == entities.Length);
						var t7 = chunk.GetSpan<T7>(c7);
						Debug.Assert(t7.Length == entities.Length);
						var t8 = chunk.GetSpan<T8>(c8);
						Debug.Assert(t8.Length == entities.Length);
						var t9 = chunk.GetSpan<T9>(c9);
						Debug.Assert(t9.Length == entities.Length);

						unsafe
						{
							#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
							fixed (Entity* eptr = entities)
							fixed (T0* t0ptr = t0)
							fixed (T1* t1ptr = t1)
							fixed (T2* t2ptr = t2)
							fixed (T3* t3ptr = t3)
							fixed (T4* t4ptr = t4)
							fixed (T5* t5ptr = t5)
							fixed (T6* t6ptr = t6)
							fixed (T7* t7ptr = t7)
							fixed (T8* t8ptr = t8)
							fixed (T9* t9ptr = t9)
							#pragma warning restore CS8500
							{
								for (var i = 0; i < entities.Length; i++)
									q.Execute(eptr[i], ref t0ptr[i], ref t1ptr[i], ref t2ptr[i], ref t3ptr[i], ref t4ptr[i], ref t5ptr[i], ref t6ptr[i], ref t7ptr[i], ref t8ptr[i], ref t9ptr[i]);
							}
						}
					}
				}
			}

			return count;
		}

		/// <summary>
		/// Execute a query in parallel over entities, blocks until complete.
		/// </summary>
		/// <param name="q"></param>
		/// <param name="query"></param>
		/// <param name="batchSize"></param>
		/// <returns></returns>
		/// <exception cref="AggregateException"></exception>
		[ExcludeFromCodeCoverage]
		public int ExecuteParallel<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
			TQ q,
			QueryDescription? query = null,
			int batchSize = 128
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>();

			var archetypes = query.GetArchetypes();

			// Early exit if there is no work to do, avoiding the cost of setting up the worker pool
			if (!query.Any())
				return 0;

			batchSize = Math.Clamp(batchSize, 1, Archetype.CHUNK_SIZE);

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

			#region Parallel Work Loop Setup
			// Borrow a counter which will be used to keep track of all in-progress work
			using var workCounterRental = Pool<CountdownEventContainer>.Rent();
			var workCounter = workCounterRental.Value.Event;
			workCounter.Reset(1);

			// Create parallel workers
			var processors = ThreadPool.Threads;
			var workersArr = ArrayPool<ParallelQueryWorker<WorkItem10<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>>?>.Shared.Rent(processors);
			var workers = workersArr.AsMemory(0, processors);
			for (var i = 0; i < workers.Length; i++)
			{
				workersArr[i] = Pool<ParallelQueryWorker<WorkItem10<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>>>.Get();
				workersArr[i]!.Configure(workersArr, workCounter);
			}

			// Start the workers. Even though there is no work they cannot exit yet because
			// the counter was reset to 1 above.
			foreach (var item in workersArr)
			{
				if (item != null)
					ThreadPool.QueueUserWorkItem(item);
			}

			// Enqueue work to this index
			var workerEnqueueIdx = 0;
			#endregion

			// Enqueue all of the work into the parallel workers
			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;

				count += archetype.EntityCount;

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entityCount = chunk.EntityCount;
					var numBatches = (int)Math.Ceiling(entityCount / (float)batchSize);

					// Inrement work counter for all of the batches we're about to create
					workCounter.AddCount(numBatches);

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
					for (var b = 0; b < numBatches; b++)
					{
						var start = b * batchSize;
						var end = Math.Min(start + batchSize, entityCount);
						var batchCount = end - start;
						var eMem = chunk.Entities.Slice(start, batchCount);

						var item = new WorkItem10<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
							eMem,
							t0.AsMemory(start, batchCount),
							t1.AsMemory(start, batchCount),
							t2.AsMemory(start, batchCount),
							t3.AsMemory(start, batchCount),
							t4.AsMemory(start, batchCount),
							t5.AsMemory(start, batchCount),
							t6.AsMemory(start, batchCount),
							t7.AsMemory(start, batchCount),
							t8.AsMemory(start, batchCount),
							t9.AsMemory(start, batchCount),
							q
						);

						#region Parallel Work Loop Add To Queue
						// Add work to a worker
						workersArr[workerEnqueueIdx]!.Enqueue(item);

						workerEnqueueIdx++;
						if (workerEnqueueIdx >= workers.Length)
							workerEnqueueIdx = 0;
						#endregion
					}
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
			ArrayPool<ParallelQueryWorker<WorkItem10<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>>>.Shared.Return(workersArr!);

			// Throw collected exceptions
			if (exceptions is { Count: > 0 })
				throw new AggregateException(exceptions);
			#endregion

			return count;
		}

		[ExcludeFromCodeCoverage]
		private readonly struct WorkItem10<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
		{
			private readonly TQ _q;

			private readonly ReadOnlyMemory<Entity> _entities;
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

			public WorkItem10(
				ReadOnlyMemory<Entity> entities,
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

			public void Execute()
			{
				var eSpan = _entities.Span;
				var c0 = _c0.Span;
				var c1 = _c1.Span;
				var c2 = _c2.Span;
				var c3 = _c3.Span;
				var c4 = _c4.Span;
				var c5 = _c5.Span;
				var c6 = _c6.Span;
				var c7 = _c7.Span;
				var c8 = _c8.Span;
				var c9 = _c9.Span;

				for (var i = 0; i < _entities.Length; i++)
				{
					_q.Execute(
						eSpan[i]
						, ref c0[i]
						, ref c1[i]
						, ref c2[i]
						, ref c3[i]
						, ref c4[i]
						, ref c5[i]
						, ref c6[i]
						, ref c7[i]
						, ref c8[i]
						, ref c9[i]
					);
				}
			}
		}
	}
}
namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A query which accepts 11 components
	/// </summary>
	public interface IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
		where T0 : IComponent
        where T1 : IComponent
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
		/// Execute the query for a single entity
		/// </summary>
		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
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
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, new()
		{
			var q = new TQ();
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(ref q, query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
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
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, new()
		{
			var q = new TQ();
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
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
		/// <param name="q">The instance to execute over every entity.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
		{
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(ref q, query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
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
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <param name="q">The instance to execute over every entity.</param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
		{
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
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
		/// <param name="q">
		/// The instance to execute over every entity. Passed by ref, so changes to the query
		/// struct will be persistent. This can allow values from one entity to be accessed by
		/// the next entity, or after the entire Execute call is complete.
		/// </param>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
		{
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
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
		/// <param name="q">
		/// The instance to execute over every entity. Passed by ref, so changes to the query
		/// struct will be persistent. This can allow values from one entity to be accessed by
		/// the next entity, or after the entire Execute call is complete.
		/// </param>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
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

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;

				count += archetype.EntityCount;

				using (var enumerator = archetype.GetChunkEnumerator())
				{
					while (enumerator.MoveNext())
					{
						var chunk = enumerator.Current;
						Debug.Assert(chunk != null);

						var entities = chunk.Entities.Span;

						var t0 = chunk.GetSpan<T0>(c0);
						Debug.Assert(t0.Length == entities.Length);
						var t1 = chunk.GetSpan<T1>(c1);
						Debug.Assert(t1.Length == entities.Length);
						var t2 = chunk.GetSpan<T2>(c2);
						Debug.Assert(t2.Length == entities.Length);
						var t3 = chunk.GetSpan<T3>(c3);
						Debug.Assert(t3.Length == entities.Length);
						var t4 = chunk.GetSpan<T4>(c4);
						Debug.Assert(t4.Length == entities.Length);
						var t5 = chunk.GetSpan<T5>(c5);
						Debug.Assert(t5.Length == entities.Length);
						var t6 = chunk.GetSpan<T6>(c6);
						Debug.Assert(t6.Length == entities.Length);
						var t7 = chunk.GetSpan<T7>(c7);
						Debug.Assert(t7.Length == entities.Length);
						var t8 = chunk.GetSpan<T8>(c8);
						Debug.Assert(t8.Length == entities.Length);
						var t9 = chunk.GetSpan<T9>(c9);
						Debug.Assert(t9.Length == entities.Length);
						var t10 = chunk.GetSpan<T10>(c10);
						Debug.Assert(t10.Length == entities.Length);

						unsafe
						{
							#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
							fixed (Entity* eptr = entities)
							fixed (T0* t0ptr = t0)
							fixed (T1* t1ptr = t1)
							fixed (T2* t2ptr = t2)
							fixed (T3* t3ptr = t3)
							fixed (T4* t4ptr = t4)
							fixed (T5* t5ptr = t5)
							fixed (T6* t6ptr = t6)
							fixed (T7* t7ptr = t7)
							fixed (T8* t8ptr = t8)
							fixed (T9* t9ptr = t9)
							fixed (T10* t10ptr = t10)
							#pragma warning restore CS8500
							{
								for (var i = 0; i < entities.Length; i++)
									q.Execute(eptr[i], ref t0ptr[i], ref t1ptr[i], ref t2ptr[i], ref t3ptr[i], ref t4ptr[i], ref t5ptr[i], ref t6ptr[i], ref t7ptr[i], ref t8ptr[i], ref t9ptr[i], ref t10ptr[i]);
							}
						}
					}
				}
			}

			return count;
		}

		/// <summary>
		/// Execute a query in parallel over entities, blocks until complete.
		/// </summary>
		/// <param name="q"></param>
		/// <param name="query"></param>
		/// <param name="batchSize"></param>
		/// <returns></returns>
		/// <exception cref="AggregateException"></exception>
		[ExcludeFromCodeCoverage]
		public int ExecuteParallel<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
			TQ q,
			QueryDescription? query = null,
			int batchSize = 128
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>();

			var archetypes = query.GetArchetypes();

			// Early exit if there is no work to do, avoiding the cost of setting up the worker pool
			if (!query.Any())
				return 0;

			batchSize = Math.Clamp(batchSize, 1, Archetype.CHUNK_SIZE);

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

			#region Parallel Work Loop Setup
			// Borrow a counter which will be used to keep track of all in-progress work
			using var workCounterRental = Pool<CountdownEventContainer>.Rent();
			var workCounter = workCounterRental.Value.Event;
			workCounter.Reset(1);

			// Create parallel workers
			var processors = ThreadPool.Threads;
			var workersArr = ArrayPool<ParallelQueryWorker<WorkItem11<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>?>.Shared.Rent(processors);
			var workers = workersArr.AsMemory(0, processors);
			for (var i = 0; i < workers.Length; i++)
			{
				workersArr[i] = Pool<ParallelQueryWorker<WorkItem11<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>>.Get();
				workersArr[i]!.Configure(workersArr, workCounter);
			}

			// Start the workers. Even though there is no work they cannot exit yet because
			// the counter was reset to 1 above.
			foreach (var item in workersArr)
			{
				if (item != null)
					ThreadPool.QueueUserWorkItem(item);
			}

			// Enqueue work to this index
			var workerEnqueueIdx = 0;
			#endregion

			// Enqueue all of the work into the parallel workers
			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;

				count += archetype.EntityCount;

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entityCount = chunk.EntityCount;
					var numBatches = (int)Math.Ceiling(entityCount / (float)batchSize);

					// Inrement work counter for all of the batches we're about to create
					workCounter.AddCount(numBatches);

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
					for (var b = 0; b < numBatches; b++)
					{
						var start = b * batchSize;
						var end = Math.Min(start + batchSize, entityCount);
						var batchCount = end - start;
						var eMem = chunk.Entities.Slice(start, batchCount);

						var item = new WorkItem11<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
							eMem,
							t0.AsMemory(start, batchCount),
							t1.AsMemory(start, batchCount),
							t2.AsMemory(start, batchCount),
							t3.AsMemory(start, batchCount),
							t4.AsMemory(start, batchCount),
							t5.AsMemory(start, batchCount),
							t6.AsMemory(start, batchCount),
							t7.AsMemory(start, batchCount),
							t8.AsMemory(start, batchCount),
							t9.AsMemory(start, batchCount),
							t10.AsMemory(start, batchCount),
							q
						);

						#region Parallel Work Loop Add To Queue
						// Add work to a worker
						workersArr[workerEnqueueIdx]!.Enqueue(item);

						workerEnqueueIdx++;
						if (workerEnqueueIdx >= workers.Length)
							workerEnqueueIdx = 0;
						#endregion
					}
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
			ArrayPool<ParallelQueryWorker<WorkItem11<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>>.Shared.Return(workersArr!);

			// Throw collected exceptions
			if (exceptions is { Count: > 0 })
				throw new AggregateException(exceptions);
			#endregion

			return count;
		}

		[ExcludeFromCodeCoverage]
		private readonly struct WorkItem11<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
		{
			private readonly TQ _q;

			private readonly ReadOnlyMemory<Entity> _entities;
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

			public WorkItem11(
				ReadOnlyMemory<Entity> entities,
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

			public void Execute()
			{
				var eSpan = _entities.Span;
				var c0 = _c0.Span;
				var c1 = _c1.Span;
				var c2 = _c2.Span;
				var c3 = _c3.Span;
				var c4 = _c4.Span;
				var c5 = _c5.Span;
				var c6 = _c6.Span;
				var c7 = _c7.Span;
				var c8 = _c8.Span;
				var c9 = _c9.Span;
				var c10 = _c10.Span;

				for (var i = 0; i < _entities.Length; i++)
				{
					_q.Execute(
						eSpan[i]
						, ref c0[i]
						, ref c1[i]
						, ref c2[i]
						, ref c3[i]
						, ref c4[i]
						, ref c5[i]
						, ref c6[i]
						, ref c7[i]
						, ref c8[i]
						, ref c9[i]
						, ref c10[i]
					);
				}
			}
		}
	}
}
namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A query which accepts 12 components
	/// </summary>
	public interface IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
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
		/// Execute the query for a single entity
		/// </summary>
		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
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
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, new()
		{
			var q = new TQ();
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(ref q, query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
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
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, new()
		{
			var q = new TQ();
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
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
		/// <param name="q">The instance to execute over every entity.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
		{
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(ref q, query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
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
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <param name="q">The instance to execute over every entity.</param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
		{
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
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
		/// <param name="q">
		/// The instance to execute over every entity. Passed by ref, so changes to the query
		/// struct will be persistent. This can allow values from one entity to be accessed by
		/// the next entity, or after the entire Execute call is complete.
		/// </param>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
		{
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
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
		/// <param name="q">
		/// The instance to execute over every entity. Passed by ref, so changes to the query
		/// struct will be persistent. This can allow values from one entity to be accessed by
		/// the next entity, or after the entire Execute call is complete.
		/// </param>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
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

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;

				count += archetype.EntityCount;

				using (var enumerator = archetype.GetChunkEnumerator())
				{
					while (enumerator.MoveNext())
					{
						var chunk = enumerator.Current;
						Debug.Assert(chunk != null);

						var entities = chunk.Entities.Span;

						var t0 = chunk.GetSpan<T0>(c0);
						Debug.Assert(t0.Length == entities.Length);
						var t1 = chunk.GetSpan<T1>(c1);
						Debug.Assert(t1.Length == entities.Length);
						var t2 = chunk.GetSpan<T2>(c2);
						Debug.Assert(t2.Length == entities.Length);
						var t3 = chunk.GetSpan<T3>(c3);
						Debug.Assert(t3.Length == entities.Length);
						var t4 = chunk.GetSpan<T4>(c4);
						Debug.Assert(t4.Length == entities.Length);
						var t5 = chunk.GetSpan<T5>(c5);
						Debug.Assert(t5.Length == entities.Length);
						var t6 = chunk.GetSpan<T6>(c6);
						Debug.Assert(t6.Length == entities.Length);
						var t7 = chunk.GetSpan<T7>(c7);
						Debug.Assert(t7.Length == entities.Length);
						var t8 = chunk.GetSpan<T8>(c8);
						Debug.Assert(t8.Length == entities.Length);
						var t9 = chunk.GetSpan<T9>(c9);
						Debug.Assert(t9.Length == entities.Length);
						var t10 = chunk.GetSpan<T10>(c10);
						Debug.Assert(t10.Length == entities.Length);
						var t11 = chunk.GetSpan<T11>(c11);
						Debug.Assert(t11.Length == entities.Length);

						unsafe
						{
							#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
							fixed (Entity* eptr = entities)
							fixed (T0* t0ptr = t0)
							fixed (T1* t1ptr = t1)
							fixed (T2* t2ptr = t2)
							fixed (T3* t3ptr = t3)
							fixed (T4* t4ptr = t4)
							fixed (T5* t5ptr = t5)
							fixed (T6* t6ptr = t6)
							fixed (T7* t7ptr = t7)
							fixed (T8* t8ptr = t8)
							fixed (T9* t9ptr = t9)
							fixed (T10* t10ptr = t10)
							fixed (T11* t11ptr = t11)
							#pragma warning restore CS8500
							{
								for (var i = 0; i < entities.Length; i++)
									q.Execute(eptr[i], ref t0ptr[i], ref t1ptr[i], ref t2ptr[i], ref t3ptr[i], ref t4ptr[i], ref t5ptr[i], ref t6ptr[i], ref t7ptr[i], ref t8ptr[i], ref t9ptr[i], ref t10ptr[i], ref t11ptr[i]);
							}
						}
					}
				}
			}

			return count;
		}

		/// <summary>
		/// Execute a query in parallel over entities, blocks until complete.
		/// </summary>
		/// <param name="q"></param>
		/// <param name="query"></param>
		/// <param name="batchSize"></param>
		/// <returns></returns>
		/// <exception cref="AggregateException"></exception>
		[ExcludeFromCodeCoverage]
		public int ExecuteParallel<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
			TQ q,
			QueryDescription? query = null,
			int batchSize = 128
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>();

			var archetypes = query.GetArchetypes();

			// Early exit if there is no work to do, avoiding the cost of setting up the worker pool
			if (!query.Any())
				return 0;

			batchSize = Math.Clamp(batchSize, 1, Archetype.CHUNK_SIZE);

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

			#region Parallel Work Loop Setup
			// Borrow a counter which will be used to keep track of all in-progress work
			using var workCounterRental = Pool<CountdownEventContainer>.Rent();
			var workCounter = workCounterRental.Value.Event;
			workCounter.Reset(1);

			// Create parallel workers
			var processors = ThreadPool.Threads;
			var workersArr = ArrayPool<ParallelQueryWorker<WorkItem12<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>?>.Shared.Rent(processors);
			var workers = workersArr.AsMemory(0, processors);
			for (var i = 0; i < workers.Length; i++)
			{
				workersArr[i] = Pool<ParallelQueryWorker<WorkItem12<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>>.Get();
				workersArr[i]!.Configure(workersArr, workCounter);
			}

			// Start the workers. Even though there is no work they cannot exit yet because
			// the counter was reset to 1 above.
			foreach (var item in workersArr)
			{
				if (item != null)
					ThreadPool.QueueUserWorkItem(item);
			}

			// Enqueue work to this index
			var workerEnqueueIdx = 0;
			#endregion

			// Enqueue all of the work into the parallel workers
			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;

				count += archetype.EntityCount;

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entityCount = chunk.EntityCount;
					var numBatches = (int)Math.Ceiling(entityCount / (float)batchSize);

					// Inrement work counter for all of the batches we're about to create
					workCounter.AddCount(numBatches);

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
					for (var b = 0; b < numBatches; b++)
					{
						var start = b * batchSize;
						var end = Math.Min(start + batchSize, entityCount);
						var batchCount = end - start;
						var eMem = chunk.Entities.Slice(start, batchCount);

						var item = new WorkItem12<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
							eMem,
							t0.AsMemory(start, batchCount),
							t1.AsMemory(start, batchCount),
							t2.AsMemory(start, batchCount),
							t3.AsMemory(start, batchCount),
							t4.AsMemory(start, batchCount),
							t5.AsMemory(start, batchCount),
							t6.AsMemory(start, batchCount),
							t7.AsMemory(start, batchCount),
							t8.AsMemory(start, batchCount),
							t9.AsMemory(start, batchCount),
							t10.AsMemory(start, batchCount),
							t11.AsMemory(start, batchCount),
							q
						);

						#region Parallel Work Loop Add To Queue
						// Add work to a worker
						workersArr[workerEnqueueIdx]!.Enqueue(item);

						workerEnqueueIdx++;
						if (workerEnqueueIdx >= workers.Length)
							workerEnqueueIdx = 0;
						#endregion
					}
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
			ArrayPool<ParallelQueryWorker<WorkItem12<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>>.Shared.Return(workersArr!);

			// Throw collected exceptions
			if (exceptions is { Count: > 0 })
				throw new AggregateException(exceptions);
			#endregion

			return count;
		}

		[ExcludeFromCodeCoverage]
		private readonly struct WorkItem12<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
		{
			private readonly TQ _q;

			private readonly ReadOnlyMemory<Entity> _entities;
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

			public WorkItem12(
				ReadOnlyMemory<Entity> entities,
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

			public void Execute()
			{
				var eSpan = _entities.Span;
				var c0 = _c0.Span;
				var c1 = _c1.Span;
				var c2 = _c2.Span;
				var c3 = _c3.Span;
				var c4 = _c4.Span;
				var c5 = _c5.Span;
				var c6 = _c6.Span;
				var c7 = _c7.Span;
				var c8 = _c8.Span;
				var c9 = _c9.Span;
				var c10 = _c10.Span;
				var c11 = _c11.Span;

				for (var i = 0; i < _entities.Length; i++)
				{
					_q.Execute(
						eSpan[i]
						, ref c0[i]
						, ref c1[i]
						, ref c2[i]
						, ref c3[i]
						, ref c4[i]
						, ref c5[i]
						, ref c6[i]
						, ref c7[i]
						, ref c8[i]
						, ref c9[i]
						, ref c10[i]
						, ref c11[i]
					);
				}
			}
		}
	}
}
namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A query which accepts 13 components
	/// </summary>
	public interface IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
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
		/// Execute the query for a single entity
		/// </summary>
		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
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
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, new()
		{
			var q = new TQ();
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(ref q, query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
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
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, new()
		{
			var q = new TQ();
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
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
		/// <param name="q">The instance to execute over every entity.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
		{
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(ref q, query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
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
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <param name="q">The instance to execute over every entity.</param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
		{
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
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
		/// <param name="q">
		/// The instance to execute over every entity. Passed by ref, so changes to the query
		/// struct will be persistent. This can allow values from one entity to be accessed by
		/// the next entity, or after the entire Execute call is complete.
		/// </param>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
		{
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
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
		/// <param name="q">
		/// The instance to execute over every entity. Passed by ref, so changes to the query
		/// struct will be persistent. This can allow values from one entity to be accessed by
		/// the next entity, or after the entire Execute call is complete.
		/// </param>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
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

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;

				count += archetype.EntityCount;

				using (var enumerator = archetype.GetChunkEnumerator())
				{
					while (enumerator.MoveNext())
					{
						var chunk = enumerator.Current;
						Debug.Assert(chunk != null);

						var entities = chunk.Entities.Span;

						var t0 = chunk.GetSpan<T0>(c0);
						Debug.Assert(t0.Length == entities.Length);
						var t1 = chunk.GetSpan<T1>(c1);
						Debug.Assert(t1.Length == entities.Length);
						var t2 = chunk.GetSpan<T2>(c2);
						Debug.Assert(t2.Length == entities.Length);
						var t3 = chunk.GetSpan<T3>(c3);
						Debug.Assert(t3.Length == entities.Length);
						var t4 = chunk.GetSpan<T4>(c4);
						Debug.Assert(t4.Length == entities.Length);
						var t5 = chunk.GetSpan<T5>(c5);
						Debug.Assert(t5.Length == entities.Length);
						var t6 = chunk.GetSpan<T6>(c6);
						Debug.Assert(t6.Length == entities.Length);
						var t7 = chunk.GetSpan<T7>(c7);
						Debug.Assert(t7.Length == entities.Length);
						var t8 = chunk.GetSpan<T8>(c8);
						Debug.Assert(t8.Length == entities.Length);
						var t9 = chunk.GetSpan<T9>(c9);
						Debug.Assert(t9.Length == entities.Length);
						var t10 = chunk.GetSpan<T10>(c10);
						Debug.Assert(t10.Length == entities.Length);
						var t11 = chunk.GetSpan<T11>(c11);
						Debug.Assert(t11.Length == entities.Length);
						var t12 = chunk.GetSpan<T12>(c12);
						Debug.Assert(t12.Length == entities.Length);

						unsafe
						{
							#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
							fixed (Entity* eptr = entities)
							fixed (T0* t0ptr = t0)
							fixed (T1* t1ptr = t1)
							fixed (T2* t2ptr = t2)
							fixed (T3* t3ptr = t3)
							fixed (T4* t4ptr = t4)
							fixed (T5* t5ptr = t5)
							fixed (T6* t6ptr = t6)
							fixed (T7* t7ptr = t7)
							fixed (T8* t8ptr = t8)
							fixed (T9* t9ptr = t9)
							fixed (T10* t10ptr = t10)
							fixed (T11* t11ptr = t11)
							fixed (T12* t12ptr = t12)
							#pragma warning restore CS8500
							{
								for (var i = 0; i < entities.Length; i++)
									q.Execute(eptr[i], ref t0ptr[i], ref t1ptr[i], ref t2ptr[i], ref t3ptr[i], ref t4ptr[i], ref t5ptr[i], ref t6ptr[i], ref t7ptr[i], ref t8ptr[i], ref t9ptr[i], ref t10ptr[i], ref t11ptr[i], ref t12ptr[i]);
							}
						}
					}
				}
			}

			return count;
		}

		/// <summary>
		/// Execute a query in parallel over entities, blocks until complete.
		/// </summary>
		/// <param name="q"></param>
		/// <param name="query"></param>
		/// <param name="batchSize"></param>
		/// <returns></returns>
		/// <exception cref="AggregateException"></exception>
		[ExcludeFromCodeCoverage]
		public int ExecuteParallel<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
			TQ q,
			QueryDescription? query = null,
			int batchSize = 128
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>();

			var archetypes = query.GetArchetypes();

			// Early exit if there is no work to do, avoiding the cost of setting up the worker pool
			if (!query.Any())
				return 0;

			batchSize = Math.Clamp(batchSize, 1, Archetype.CHUNK_SIZE);

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

			#region Parallel Work Loop Setup
			// Borrow a counter which will be used to keep track of all in-progress work
			using var workCounterRental = Pool<CountdownEventContainer>.Rent();
			var workCounter = workCounterRental.Value.Event;
			workCounter.Reset(1);

			// Create parallel workers
			var processors = ThreadPool.Threads;
			var workersArr = ArrayPool<ParallelQueryWorker<WorkItem13<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>?>.Shared.Rent(processors);
			var workers = workersArr.AsMemory(0, processors);
			for (var i = 0; i < workers.Length; i++)
			{
				workersArr[i] = Pool<ParallelQueryWorker<WorkItem13<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>>.Get();
				workersArr[i]!.Configure(workersArr, workCounter);
			}

			// Start the workers. Even though there is no work they cannot exit yet because
			// the counter was reset to 1 above.
			foreach (var item in workersArr)
			{
				if (item != null)
					ThreadPool.QueueUserWorkItem(item);
			}

			// Enqueue work to this index
			var workerEnqueueIdx = 0;
			#endregion

			// Enqueue all of the work into the parallel workers
			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;

				count += archetype.EntityCount;

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entityCount = chunk.EntityCount;
					var numBatches = (int)Math.Ceiling(entityCount / (float)batchSize);

					// Inrement work counter for all of the batches we're about to create
					workCounter.AddCount(numBatches);

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
					for (var b = 0; b < numBatches; b++)
					{
						var start = b * batchSize;
						var end = Math.Min(start + batchSize, entityCount);
						var batchCount = end - start;
						var eMem = chunk.Entities.Slice(start, batchCount);

						var item = new WorkItem13<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
							eMem,
							t0.AsMemory(start, batchCount),
							t1.AsMemory(start, batchCount),
							t2.AsMemory(start, batchCount),
							t3.AsMemory(start, batchCount),
							t4.AsMemory(start, batchCount),
							t5.AsMemory(start, batchCount),
							t6.AsMemory(start, batchCount),
							t7.AsMemory(start, batchCount),
							t8.AsMemory(start, batchCount),
							t9.AsMemory(start, batchCount),
							t10.AsMemory(start, batchCount),
							t11.AsMemory(start, batchCount),
							t12.AsMemory(start, batchCount),
							q
						);

						#region Parallel Work Loop Add To Queue
						// Add work to a worker
						workersArr[workerEnqueueIdx]!.Enqueue(item);

						workerEnqueueIdx++;
						if (workerEnqueueIdx >= workers.Length)
							workerEnqueueIdx = 0;
						#endregion
					}
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
			ArrayPool<ParallelQueryWorker<WorkItem13<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>>.Shared.Return(workersArr!);

			// Throw collected exceptions
			if (exceptions is { Count: > 0 })
				throw new AggregateException(exceptions);
			#endregion

			return count;
		}

		[ExcludeFromCodeCoverage]
		private readonly struct WorkItem13<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
		{
			private readonly TQ _q;

			private readonly ReadOnlyMemory<Entity> _entities;
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

			public WorkItem13(
				ReadOnlyMemory<Entity> entities,
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

			public void Execute()
			{
				var eSpan = _entities.Span;
				var c0 = _c0.Span;
				var c1 = _c1.Span;
				var c2 = _c2.Span;
				var c3 = _c3.Span;
				var c4 = _c4.Span;
				var c5 = _c5.Span;
				var c6 = _c6.Span;
				var c7 = _c7.Span;
				var c8 = _c8.Span;
				var c9 = _c9.Span;
				var c10 = _c10.Span;
				var c11 = _c11.Span;
				var c12 = _c12.Span;

				for (var i = 0; i < _entities.Length; i++)
				{
					_q.Execute(
						eSpan[i]
						, ref c0[i]
						, ref c1[i]
						, ref c2[i]
						, ref c3[i]
						, ref c4[i]
						, ref c5[i]
						, ref c6[i]
						, ref c7[i]
						, ref c8[i]
						, ref c9[i]
						, ref c10[i]
						, ref c11[i]
						, ref c12[i]
					);
				}
			}
		}
	}
}
namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A query which accepts 14 components
	/// </summary>
	public interface IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
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
		/// Execute the query for a single entity
		/// </summary>
		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12, ref T13 t13);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
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
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, new()
		{
			var q = new TQ();
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(ref q, query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
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
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, new()
		{
			var q = new TQ();
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
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
		/// <param name="q">The instance to execute over every entity.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
		{
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(ref q, query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
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
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <param name="q">The instance to execute over every entity.</param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
		{
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
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
		/// <param name="q">
		/// The instance to execute over every entity. Passed by ref, so changes to the query
		/// struct will be persistent. This can allow values from one entity to be accessed by
		/// the next entity, or after the entire Execute call is complete.
		/// </param>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
		{
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
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
		/// <param name="q">
		/// The instance to execute over every entity. Passed by ref, so changes to the query
		/// struct will be persistent. This can allow values from one entity to be accessed by
		/// the next entity, or after the entire Execute call is complete.
		/// </param>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
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

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;

				count += archetype.EntityCount;

				using (var enumerator = archetype.GetChunkEnumerator())
				{
					while (enumerator.MoveNext())
					{
						var chunk = enumerator.Current;
						Debug.Assert(chunk != null);

						var entities = chunk.Entities.Span;

						var t0 = chunk.GetSpan<T0>(c0);
						Debug.Assert(t0.Length == entities.Length);
						var t1 = chunk.GetSpan<T1>(c1);
						Debug.Assert(t1.Length == entities.Length);
						var t2 = chunk.GetSpan<T2>(c2);
						Debug.Assert(t2.Length == entities.Length);
						var t3 = chunk.GetSpan<T3>(c3);
						Debug.Assert(t3.Length == entities.Length);
						var t4 = chunk.GetSpan<T4>(c4);
						Debug.Assert(t4.Length == entities.Length);
						var t5 = chunk.GetSpan<T5>(c5);
						Debug.Assert(t5.Length == entities.Length);
						var t6 = chunk.GetSpan<T6>(c6);
						Debug.Assert(t6.Length == entities.Length);
						var t7 = chunk.GetSpan<T7>(c7);
						Debug.Assert(t7.Length == entities.Length);
						var t8 = chunk.GetSpan<T8>(c8);
						Debug.Assert(t8.Length == entities.Length);
						var t9 = chunk.GetSpan<T9>(c9);
						Debug.Assert(t9.Length == entities.Length);
						var t10 = chunk.GetSpan<T10>(c10);
						Debug.Assert(t10.Length == entities.Length);
						var t11 = chunk.GetSpan<T11>(c11);
						Debug.Assert(t11.Length == entities.Length);
						var t12 = chunk.GetSpan<T12>(c12);
						Debug.Assert(t12.Length == entities.Length);
						var t13 = chunk.GetSpan<T13>(c13);
						Debug.Assert(t13.Length == entities.Length);

						unsafe
						{
							#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
							fixed (Entity* eptr = entities)
							fixed (T0* t0ptr = t0)
							fixed (T1* t1ptr = t1)
							fixed (T2* t2ptr = t2)
							fixed (T3* t3ptr = t3)
							fixed (T4* t4ptr = t4)
							fixed (T5* t5ptr = t5)
							fixed (T6* t6ptr = t6)
							fixed (T7* t7ptr = t7)
							fixed (T8* t8ptr = t8)
							fixed (T9* t9ptr = t9)
							fixed (T10* t10ptr = t10)
							fixed (T11* t11ptr = t11)
							fixed (T12* t12ptr = t12)
							fixed (T13* t13ptr = t13)
							#pragma warning restore CS8500
							{
								for (var i = 0; i < entities.Length; i++)
									q.Execute(eptr[i], ref t0ptr[i], ref t1ptr[i], ref t2ptr[i], ref t3ptr[i], ref t4ptr[i], ref t5ptr[i], ref t6ptr[i], ref t7ptr[i], ref t8ptr[i], ref t9ptr[i], ref t10ptr[i], ref t11ptr[i], ref t12ptr[i], ref t13ptr[i]);
							}
						}
					}
				}
			}

			return count;
		}

		/// <summary>
		/// Execute a query in parallel over entities, blocks until complete.
		/// </summary>
		/// <param name="q"></param>
		/// <param name="query"></param>
		/// <param name="batchSize"></param>
		/// <returns></returns>
		/// <exception cref="AggregateException"></exception>
		[ExcludeFromCodeCoverage]
		public int ExecuteParallel<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
			TQ q,
			QueryDescription? query = null,
			int batchSize = 128
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>();

			var archetypes = query.GetArchetypes();

			// Early exit if there is no work to do, avoiding the cost of setting up the worker pool
			if (!query.Any())
				return 0;

			batchSize = Math.Clamp(batchSize, 1, Archetype.CHUNK_SIZE);

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

			#region Parallel Work Loop Setup
			// Borrow a counter which will be used to keep track of all in-progress work
			using var workCounterRental = Pool<CountdownEventContainer>.Rent();
			var workCounter = workCounterRental.Value.Event;
			workCounter.Reset(1);

			// Create parallel workers
			var processors = ThreadPool.Threads;
			var workersArr = ArrayPool<ParallelQueryWorker<WorkItem14<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>>?>.Shared.Rent(processors);
			var workers = workersArr.AsMemory(0, processors);
			for (var i = 0; i < workers.Length; i++)
			{
				workersArr[i] = Pool<ParallelQueryWorker<WorkItem14<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>>>.Get();
				workersArr[i]!.Configure(workersArr, workCounter);
			}

			// Start the workers. Even though there is no work they cannot exit yet because
			// the counter was reset to 1 above.
			foreach (var item in workersArr)
			{
				if (item != null)
					ThreadPool.QueueUserWorkItem(item);
			}

			// Enqueue work to this index
			var workerEnqueueIdx = 0;
			#endregion

			// Enqueue all of the work into the parallel workers
			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;

				count += archetype.EntityCount;

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entityCount = chunk.EntityCount;
					var numBatches = (int)Math.Ceiling(entityCount / (float)batchSize);

					// Inrement work counter for all of the batches we're about to create
					workCounter.AddCount(numBatches);

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
					for (var b = 0; b < numBatches; b++)
					{
						var start = b * batchSize;
						var end = Math.Min(start + batchSize, entityCount);
						var batchCount = end - start;
						var eMem = chunk.Entities.Slice(start, batchCount);

						var item = new WorkItem14<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
							eMem,
							t0.AsMemory(start, batchCount),
							t1.AsMemory(start, batchCount),
							t2.AsMemory(start, batchCount),
							t3.AsMemory(start, batchCount),
							t4.AsMemory(start, batchCount),
							t5.AsMemory(start, batchCount),
							t6.AsMemory(start, batchCount),
							t7.AsMemory(start, batchCount),
							t8.AsMemory(start, batchCount),
							t9.AsMemory(start, batchCount),
							t10.AsMemory(start, batchCount),
							t11.AsMemory(start, batchCount),
							t12.AsMemory(start, batchCount),
							t13.AsMemory(start, batchCount),
							q
						);

						#region Parallel Work Loop Add To Queue
						// Add work to a worker
						workersArr[workerEnqueueIdx]!.Enqueue(item);

						workerEnqueueIdx++;
						if (workerEnqueueIdx >= workers.Length)
							workerEnqueueIdx = 0;
						#endregion
					}
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
			ArrayPool<ParallelQueryWorker<WorkItem14<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>>>.Shared.Return(workersArr!);

			// Throw collected exceptions
			if (exceptions is { Count: > 0 })
				throw new AggregateException(exceptions);
			#endregion

			return count;
		}

		[ExcludeFromCodeCoverage]
		private readonly struct WorkItem14<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
		{
			private readonly TQ _q;

			private readonly ReadOnlyMemory<Entity> _entities;
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

			public WorkItem14(
				ReadOnlyMemory<Entity> entities,
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

			public void Execute()
			{
				var eSpan = _entities.Span;
				var c0 = _c0.Span;
				var c1 = _c1.Span;
				var c2 = _c2.Span;
				var c3 = _c3.Span;
				var c4 = _c4.Span;
				var c5 = _c5.Span;
				var c6 = _c6.Span;
				var c7 = _c7.Span;
				var c8 = _c8.Span;
				var c9 = _c9.Span;
				var c10 = _c10.Span;
				var c11 = _c11.Span;
				var c12 = _c12.Span;
				var c13 = _c13.Span;

				for (var i = 0; i < _entities.Length; i++)
				{
					_q.Execute(
						eSpan[i]
						, ref c0[i]
						, ref c1[i]
						, ref c2[i]
						, ref c3[i]
						, ref c4[i]
						, ref c5[i]
						, ref c6[i]
						, ref c7[i]
						, ref c8[i]
						, ref c9[i]
						, ref c10[i]
						, ref c11[i]
						, ref c12[i]
						, ref c13[i]
					);
				}
			}
		}
	}
}
namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A query which accepts 15 components
	/// </summary>
	public interface IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
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
		/// Execute the query for a single entity
		/// </summary>
		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12, ref T13 t13, ref T14 t14);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
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
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, new()
		{
			var q = new TQ();
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(ref q, query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
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
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, new()
		{
			var q = new TQ();
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
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
		/// <param name="q">The instance to execute over every entity.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
		{
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(ref q, query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
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
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <param name="q">The instance to execute over every entity.</param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
		{
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
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
		/// <param name="q">
		/// The instance to execute over every entity. Passed by ref, so changes to the query
		/// struct will be persistent. This can allow values from one entity to be accessed by
		/// the next entity, or after the entire Execute call is complete.
		/// </param>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
		{
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
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
		/// <param name="q">
		/// The instance to execute over every entity. Passed by ref, so changes to the query
		/// struct will be persistent. This can allow values from one entity to be accessed by
		/// the next entity, or after the entire Execute call is complete.
		/// </param>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
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

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;

				count += archetype.EntityCount;

				using (var enumerator = archetype.GetChunkEnumerator())
				{
					while (enumerator.MoveNext())
					{
						var chunk = enumerator.Current;
						Debug.Assert(chunk != null);

						var entities = chunk.Entities.Span;

						var t0 = chunk.GetSpan<T0>(c0);
						Debug.Assert(t0.Length == entities.Length);
						var t1 = chunk.GetSpan<T1>(c1);
						Debug.Assert(t1.Length == entities.Length);
						var t2 = chunk.GetSpan<T2>(c2);
						Debug.Assert(t2.Length == entities.Length);
						var t3 = chunk.GetSpan<T3>(c3);
						Debug.Assert(t3.Length == entities.Length);
						var t4 = chunk.GetSpan<T4>(c4);
						Debug.Assert(t4.Length == entities.Length);
						var t5 = chunk.GetSpan<T5>(c5);
						Debug.Assert(t5.Length == entities.Length);
						var t6 = chunk.GetSpan<T6>(c6);
						Debug.Assert(t6.Length == entities.Length);
						var t7 = chunk.GetSpan<T7>(c7);
						Debug.Assert(t7.Length == entities.Length);
						var t8 = chunk.GetSpan<T8>(c8);
						Debug.Assert(t8.Length == entities.Length);
						var t9 = chunk.GetSpan<T9>(c9);
						Debug.Assert(t9.Length == entities.Length);
						var t10 = chunk.GetSpan<T10>(c10);
						Debug.Assert(t10.Length == entities.Length);
						var t11 = chunk.GetSpan<T11>(c11);
						Debug.Assert(t11.Length == entities.Length);
						var t12 = chunk.GetSpan<T12>(c12);
						Debug.Assert(t12.Length == entities.Length);
						var t13 = chunk.GetSpan<T13>(c13);
						Debug.Assert(t13.Length == entities.Length);
						var t14 = chunk.GetSpan<T14>(c14);
						Debug.Assert(t14.Length == entities.Length);

						unsafe
						{
							#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
							fixed (Entity* eptr = entities)
							fixed (T0* t0ptr = t0)
							fixed (T1* t1ptr = t1)
							fixed (T2* t2ptr = t2)
							fixed (T3* t3ptr = t3)
							fixed (T4* t4ptr = t4)
							fixed (T5* t5ptr = t5)
							fixed (T6* t6ptr = t6)
							fixed (T7* t7ptr = t7)
							fixed (T8* t8ptr = t8)
							fixed (T9* t9ptr = t9)
							fixed (T10* t10ptr = t10)
							fixed (T11* t11ptr = t11)
							fixed (T12* t12ptr = t12)
							fixed (T13* t13ptr = t13)
							fixed (T14* t14ptr = t14)
							#pragma warning restore CS8500
							{
								for (var i = 0; i < entities.Length; i++)
									q.Execute(eptr[i], ref t0ptr[i], ref t1ptr[i], ref t2ptr[i], ref t3ptr[i], ref t4ptr[i], ref t5ptr[i], ref t6ptr[i], ref t7ptr[i], ref t8ptr[i], ref t9ptr[i], ref t10ptr[i], ref t11ptr[i], ref t12ptr[i], ref t13ptr[i], ref t14ptr[i]);
							}
						}
					}
				}
			}

			return count;
		}

		/// <summary>
		/// Execute a query in parallel over entities, blocks until complete.
		/// </summary>
		/// <param name="q"></param>
		/// <param name="query"></param>
		/// <param name="batchSize"></param>
		/// <returns></returns>
		/// <exception cref="AggregateException"></exception>
		[ExcludeFromCodeCoverage]
		public int ExecuteParallel<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
			TQ q,
			QueryDescription? query = null,
			int batchSize = 128
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>();

			var archetypes = query.GetArchetypes();

			// Early exit if there is no work to do, avoiding the cost of setting up the worker pool
			if (!query.Any())
				return 0;

			batchSize = Math.Clamp(batchSize, 1, Archetype.CHUNK_SIZE);

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

			#region Parallel Work Loop Setup
			// Borrow a counter which will be used to keep track of all in-progress work
			using var workCounterRental = Pool<CountdownEventContainer>.Rent();
			var workCounter = workCounterRental.Value.Event;
			workCounter.Reset(1);

			// Create parallel workers
			var processors = ThreadPool.Threads;
			var workersArr = ArrayPool<ParallelQueryWorker<WorkItem15<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>>?>.Shared.Rent(processors);
			var workers = workersArr.AsMemory(0, processors);
			for (var i = 0; i < workers.Length; i++)
			{
				workersArr[i] = Pool<ParallelQueryWorker<WorkItem15<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>>>.Get();
				workersArr[i]!.Configure(workersArr, workCounter);
			}

			// Start the workers. Even though there is no work they cannot exit yet because
			// the counter was reset to 1 above.
			foreach (var item in workersArr)
			{
				if (item != null)
					ThreadPool.QueueUserWorkItem(item);
			}

			// Enqueue work to this index
			var workerEnqueueIdx = 0;
			#endregion

			// Enqueue all of the work into the parallel workers
			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;

				count += archetype.EntityCount;

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entityCount = chunk.EntityCount;
					var numBatches = (int)Math.Ceiling(entityCount / (float)batchSize);

					// Inrement work counter for all of the batches we're about to create
					workCounter.AddCount(numBatches);

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
					for (var b = 0; b < numBatches; b++)
					{
						var start = b * batchSize;
						var end = Math.Min(start + batchSize, entityCount);
						var batchCount = end - start;
						var eMem = chunk.Entities.Slice(start, batchCount);

						var item = new WorkItem15<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
							eMem,
							t0.AsMemory(start, batchCount),
							t1.AsMemory(start, batchCount),
							t2.AsMemory(start, batchCount),
							t3.AsMemory(start, batchCount),
							t4.AsMemory(start, batchCount),
							t5.AsMemory(start, batchCount),
							t6.AsMemory(start, batchCount),
							t7.AsMemory(start, batchCount),
							t8.AsMemory(start, batchCount),
							t9.AsMemory(start, batchCount),
							t10.AsMemory(start, batchCount),
							t11.AsMemory(start, batchCount),
							t12.AsMemory(start, batchCount),
							t13.AsMemory(start, batchCount),
							t14.AsMemory(start, batchCount),
							q
						);

						#region Parallel Work Loop Add To Queue
						// Add work to a worker
						workersArr[workerEnqueueIdx]!.Enqueue(item);

						workerEnqueueIdx++;
						if (workerEnqueueIdx >= workers.Length)
							workerEnqueueIdx = 0;
						#endregion
					}
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
			ArrayPool<ParallelQueryWorker<WorkItem15<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>>>.Shared.Return(workersArr!);

			// Throw collected exceptions
			if (exceptions is { Count: > 0 })
				throw new AggregateException(exceptions);
			#endregion

			return count;
		}

		[ExcludeFromCodeCoverage]
		private readonly struct WorkItem15<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
		{
			private readonly TQ _q;

			private readonly ReadOnlyMemory<Entity> _entities;
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

			public WorkItem15(
				ReadOnlyMemory<Entity> entities,
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

			public void Execute()
			{
				var eSpan = _entities.Span;
				var c0 = _c0.Span;
				var c1 = _c1.Span;
				var c2 = _c2.Span;
				var c3 = _c3.Span;
				var c4 = _c4.Span;
				var c5 = _c5.Span;
				var c6 = _c6.Span;
				var c7 = _c7.Span;
				var c8 = _c8.Span;
				var c9 = _c9.Span;
				var c10 = _c10.Span;
				var c11 = _c11.Span;
				var c12 = _c12.Span;
				var c13 = _c13.Span;
				var c14 = _c14.Span;

				for (var i = 0; i < _entities.Length; i++)
				{
					_q.Execute(
						eSpan[i]
						, ref c0[i]
						, ref c1[i]
						, ref c2[i]
						, ref c3[i]
						, ref c4[i]
						, ref c5[i]
						, ref c6[i]
						, ref c7[i]
						, ref c8[i]
						, ref c9[i]
						, ref c10[i]
						, ref c11[i]
						, ref c12[i]
						, ref c13[i]
						, ref c14[i]
					);
				}
			}
		}
	}
}
namespace Myriad.ECS.Queries
{
	/// <summary>
	/// A query which accepts 16 components
	/// </summary>
	public interface IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
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
		/// Execute the query for a single entity
		/// </summary>
		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12, ref T13 t13, ref T14 t14, ref T15 t15);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
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
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, new()
		{
			var q = new TQ();
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(ref q, query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
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
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, new()
		{
			var q = new TQ();
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
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
		/// <param name="q">The instance to execute over every entity.</param>
		/// <param name="query"></param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
		{
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(ref q, query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity. A new TQ() instance is used.</typeparam>
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
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <param name="q">The instance to execute over every entity.</param>
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
		{
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
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
		/// <param name="q">
		/// The instance to execute over every entity. Passed by ref, so changes to the query
		/// struct will be persistent. This can allow values from one entity to be accessed by
		/// the next entity, or after the entire Execute call is complete.
		/// </param>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
		{
			return Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(ref q, ref query);
		}

		/// <summary>
		/// Execute a query, optionally filtering by a <see cref="QueryDescription"/>.
		/// </summary>
		/// <typeparam name="TQ">The type of the query to execute for every entity.</typeparam>
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
		/// <param name="q">
		/// The instance to execute over every entity. Passed by ref, so changes to the query
		/// struct will be persistent. This can allow values from one entity to be accessed by
		/// the next entity, or after the entire Execute call is complete.
		/// </param>
		
		/// <param name="query">
		/// Optional query to filter by. If non-null this <b>must</b> Include all of the component
		/// types specified in the type signature of this call!
		/// <br /><br />
		/// If null a default query will be used, selecting all entities which include the components
		/// in the type signature. This query object will be written to the query parameter by ref. It
		/// can be used next frame to slightly speed up query execution.
		/// </param>
		
		/// <returns>The number of entities discovered by this query</returns>
		[ExcludeFromCodeCoverage]
		public int Execute<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
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

			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;

				count += archetype.EntityCount;

				using (var enumerator = archetype.GetChunkEnumerator())
				{
					while (enumerator.MoveNext())
					{
						var chunk = enumerator.Current;
						Debug.Assert(chunk != null);

						var entities = chunk.Entities.Span;

						var t0 = chunk.GetSpan<T0>(c0);
						Debug.Assert(t0.Length == entities.Length);
						var t1 = chunk.GetSpan<T1>(c1);
						Debug.Assert(t1.Length == entities.Length);
						var t2 = chunk.GetSpan<T2>(c2);
						Debug.Assert(t2.Length == entities.Length);
						var t3 = chunk.GetSpan<T3>(c3);
						Debug.Assert(t3.Length == entities.Length);
						var t4 = chunk.GetSpan<T4>(c4);
						Debug.Assert(t4.Length == entities.Length);
						var t5 = chunk.GetSpan<T5>(c5);
						Debug.Assert(t5.Length == entities.Length);
						var t6 = chunk.GetSpan<T6>(c6);
						Debug.Assert(t6.Length == entities.Length);
						var t7 = chunk.GetSpan<T7>(c7);
						Debug.Assert(t7.Length == entities.Length);
						var t8 = chunk.GetSpan<T8>(c8);
						Debug.Assert(t8.Length == entities.Length);
						var t9 = chunk.GetSpan<T9>(c9);
						Debug.Assert(t9.Length == entities.Length);
						var t10 = chunk.GetSpan<T10>(c10);
						Debug.Assert(t10.Length == entities.Length);
						var t11 = chunk.GetSpan<T11>(c11);
						Debug.Assert(t11.Length == entities.Length);
						var t12 = chunk.GetSpan<T12>(c12);
						Debug.Assert(t12.Length == entities.Length);
						var t13 = chunk.GetSpan<T13>(c13);
						Debug.Assert(t13.Length == entities.Length);
						var t14 = chunk.GetSpan<T14>(c14);
						Debug.Assert(t14.Length == entities.Length);
						var t15 = chunk.GetSpan<T15>(c15);
						Debug.Assert(t15.Length == entities.Length);

						unsafe
						{
							#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
							fixed (Entity* eptr = entities)
							fixed (T0* t0ptr = t0)
							fixed (T1* t1ptr = t1)
							fixed (T2* t2ptr = t2)
							fixed (T3* t3ptr = t3)
							fixed (T4* t4ptr = t4)
							fixed (T5* t5ptr = t5)
							fixed (T6* t6ptr = t6)
							fixed (T7* t7ptr = t7)
							fixed (T8* t8ptr = t8)
							fixed (T9* t9ptr = t9)
							fixed (T10* t10ptr = t10)
							fixed (T11* t11ptr = t11)
							fixed (T12* t12ptr = t12)
							fixed (T13* t13ptr = t13)
							fixed (T14* t14ptr = t14)
							fixed (T15* t15ptr = t15)
							#pragma warning restore CS8500
							{
								for (var i = 0; i < entities.Length; i++)
									q.Execute(eptr[i], ref t0ptr[i], ref t1ptr[i], ref t2ptr[i], ref t3ptr[i], ref t4ptr[i], ref t5ptr[i], ref t6ptr[i], ref t7ptr[i], ref t8ptr[i], ref t9ptr[i], ref t10ptr[i], ref t11ptr[i], ref t12ptr[i], ref t13ptr[i], ref t14ptr[i], ref t15ptr[i]);
							}
						}
					}
				}
			}

			return count;
		}

		/// <summary>
		/// Execute a query in parallel over entities, blocks until complete.
		/// </summary>
		/// <param name="q"></param>
		/// <param name="query"></param>
		/// <param name="batchSize"></param>
		/// <returns></returns>
		/// <exception cref="AggregateException"></exception>
		[ExcludeFromCodeCoverage]
		public int ExecuteParallel<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
			TQ q,
			QueryDescription? query = null,
			int batchSize = 128
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>();

			var archetypes = query.GetArchetypes();

			// Early exit if there is no work to do, avoiding the cost of setting up the worker pool
			if (!query.Any())
				return 0;

			batchSize = Math.Clamp(batchSize, 1, Archetype.CHUNK_SIZE);

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

			#region Parallel Work Loop Setup
			// Borrow a counter which will be used to keep track of all in-progress work
			using var workCounterRental = Pool<CountdownEventContainer>.Rent();
			var workCounter = workCounterRental.Value.Event;
			workCounter.Reset(1);

			// Create parallel workers
			var processors = ThreadPool.Threads;
			var workersArr = ArrayPool<ParallelQueryWorker<WorkItem16<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>>?>.Shared.Rent(processors);
			var workers = workersArr.AsMemory(0, processors);
			for (var i = 0; i < workers.Length; i++)
			{
				workersArr[i] = Pool<ParallelQueryWorker<WorkItem16<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>>>.Get();
				workersArr[i]!.Configure(workersArr, workCounter);
			}

			// Start the workers. Even though there is no work they cannot exit yet because
			// the counter was reset to 1 above.
			foreach (var item in workersArr)
			{
				if (item != null)
					ThreadPool.QueueUserWorkItem(item);
			}

			// Enqueue work to this index
			var workerEnqueueIdx = 0;
			#endregion

			// Enqueue all of the work into the parallel workers
			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;

				count += archetype.EntityCount;

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entityCount = chunk.EntityCount;
					var numBatches = (int)Math.Ceiling(entityCount / (float)batchSize);

					// Inrement work counter for all of the batches we're about to create
					workCounter.AddCount(numBatches);

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
					for (var b = 0; b < numBatches; b++)
					{
						var start = b * batchSize;
						var end = Math.Min(start + batchSize, entityCount);
						var batchCount = end - start;
						var eMem = chunk.Entities.Slice(start, batchCount);

						var item = new WorkItem16<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
							eMem,
							t0.AsMemory(start, batchCount),
							t1.AsMemory(start, batchCount),
							t2.AsMemory(start, batchCount),
							t3.AsMemory(start, batchCount),
							t4.AsMemory(start, batchCount),
							t5.AsMemory(start, batchCount),
							t6.AsMemory(start, batchCount),
							t7.AsMemory(start, batchCount),
							t8.AsMemory(start, batchCount),
							t9.AsMemory(start, batchCount),
							t10.AsMemory(start, batchCount),
							t11.AsMemory(start, batchCount),
							t12.AsMemory(start, batchCount),
							t13.AsMemory(start, batchCount),
							t14.AsMemory(start, batchCount),
							t15.AsMemory(start, batchCount),
							q
						);

						#region Parallel Work Loop Add To Queue
						// Add work to a worker
						workersArr[workerEnqueueIdx]!.Enqueue(item);

						workerEnqueueIdx++;
						if (workerEnqueueIdx >= workers.Length)
							workerEnqueueIdx = 0;
						#endregion
					}
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
			ArrayPool<ParallelQueryWorker<WorkItem16<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>>>.Shared.Return(workersArr!);

			// Throw collected exceptions
			if (exceptions is { Count: > 0 })
				throw new AggregateException(exceptions);
			#endregion

			return count;
		}

		[ExcludeFromCodeCoverage]
		private readonly struct WorkItem16<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
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
			where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
		{
			private readonly TQ _q;

			private readonly ReadOnlyMemory<Entity> _entities;
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

			public WorkItem16(
				ReadOnlyMemory<Entity> entities,
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

			public void Execute()
			{
				var eSpan = _entities.Span;
				var c0 = _c0.Span;
				var c1 = _c1.Span;
				var c2 = _c2.Span;
				var c3 = _c3.Span;
				var c4 = _c4.Span;
				var c5 = _c5.Span;
				var c6 = _c6.Span;
				var c7 = _c7.Span;
				var c8 = _c8.Span;
				var c9 = _c9.Span;
				var c10 = _c10.Span;
				var c11 = _c11.Span;
				var c12 = _c12.Span;
				var c13 = _c13.Span;
				var c14 = _c14.Span;
				var c15 = _c15.Span;

				for (var i = 0; i < _entities.Length; i++)
				{
					_q.Execute(
						eSpan[i]
						, ref c0[i]
						, ref c1[i]
						, ref c2[i]
						, ref c3[i]
						, ref c4[i]
						, ref c5[i]
						, ref c6[i]
						, ref c7[i]
						, ref c8[i]
						, ref c9[i]
						, ref c10[i]
						, ref c11[i]
						, ref c12[i]
						, ref c13[i]
						, ref c14[i]
						, ref c15[i]
					);
				}
			}
		}
	}
}


