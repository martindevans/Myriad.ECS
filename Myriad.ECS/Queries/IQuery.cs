using System.Diagnostics;
using Myriad.ECS.Queries;
using Myriad.ECS.IDs;
using Myriad.ECS.Worlds.Archetypes;

using Parallel = System.Threading.Tasks.Parallel;
//using Parallel = ParallelTasks.Parallel;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedParameter.Global
// ReSharper disable LoopCanBeConvertedToQuery
// ReSharper disable ConvertToPrimaryConstructor

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

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

					var t0 = chunk.GetSpan<T0>(c0);
					Debug.Assert(t0.Length == entities.Length);

					unsafe
					{
						#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
						fixed (Entity* eptr = &entities[0])
						fixed (T0* t0ptr = &t0[0])
						#pragma warning restore CS8500
						{
							for (var i = 0; i < entities.Length; i++)
								q.Execute(eptr[i], ref t0ptr[i]);
						}
					}
				}
			}

			return count;
		}

		public int ExecuteParallel<TQ, T0>(
			TQ q,
			QueryDescription? query = null,
			int batchSize = 64
		)
			where T0 : IComponent
			where TQ : IQuery1<T0>
		{
			query ??= GetCachedQuery<T0>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return 0;

			batchSize = Math.Clamp(batchSize, 1, Archetype.CHUNK_SIZE);

			var c0 = ComponentID<T0>.ID;
	
			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				count += archetype.EntityCount;

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entityCount = chunk.EntityCount;
					if (entityCount == 0)
						continue;

					var numBatches = (int)Math.Ceiling(entityCount / (float)batchSize);

					var t0 = chunk.GetComponentArray<T0>(c0);

					Parallel.For(0, numBatches, b =>
                    {
						var start = b * batchSize;
						var end = Math.Min(start + batchSize, entityCount);

						var entities = chunk.Entities;
						for (var i = start; i < end; i++)
							q.Execute(entities[i], ref t0[i]);
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

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

					var t0 = chunk.GetSpan<T0>(c0);
					Debug.Assert(t0.Length == entities.Length);
					var t1 = chunk.GetSpan<T1>(c1);
					Debug.Assert(t1.Length == entities.Length);

					unsafe
					{
						#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
						fixed (Entity* eptr = &entities[0])
						fixed (T0* t0ptr = &t0[0])
						fixed (T1* t1ptr = &t1[0])
						#pragma warning restore CS8500
						{
							for (var i = 0; i < entities.Length; i++)
								q.Execute(eptr[i], ref t0ptr[i], ref t1ptr[i]);
						}
					}
				}
			}

			return count;
		}

		public int ExecuteParallel<TQ, T0, T1>(
			TQ q,
			QueryDescription? query = null,
			int batchSize = 64
		)
			where T0 : IComponent
            where T1 : IComponent
			where TQ : IQuery2<T0, T1>
		{
			query ??= GetCachedQuery<T0, T1>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
				return 0;

			batchSize = Math.Clamp(batchSize, 1, Archetype.CHUNK_SIZE);

			var c0 = ComponentID<T0>.ID;
			var c1 = ComponentID<T1>.ID;
	
			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				count += archetype.EntityCount;

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entityCount = chunk.EntityCount;
					if (entityCount == 0)
						continue;

					var numBatches = (int)Math.Ceiling(entityCount / (float)batchSize);

					var t0 = chunk.GetComponentArray<T0>(c0);
					var t1 = chunk.GetComponentArray<T1>(c1);

					Parallel.For(0, numBatches, b =>
                    {
						var start = b * batchSize;
						var end = Math.Min(start + batchSize, entityCount);

						var entities = chunk.Entities;
						for (var i = start; i < end; i++)
							q.Execute(entities[i], ref t0[i], ref t1[i]);
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

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

					var t0 = chunk.GetSpan<T0>(c0);
					Debug.Assert(t0.Length == entities.Length);
					var t1 = chunk.GetSpan<T1>(c1);
					Debug.Assert(t1.Length == entities.Length);
					var t2 = chunk.GetSpan<T2>(c2);
					Debug.Assert(t2.Length == entities.Length);

					unsafe
					{
						#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
						fixed (Entity* eptr = &entities[0])
						fixed (T0* t0ptr = &t0[0])
						fixed (T1* t1ptr = &t1[0])
						fixed (T2* t2ptr = &t2[0])
						#pragma warning restore CS8500
						{
							for (var i = 0; i < entities.Length; i++)
								q.Execute(eptr[i], ref t0ptr[i], ref t1ptr[i], ref t2ptr[i]);
						}
					}
				}
			}

			return count;
		}

		public int ExecuteParallel<TQ, T0, T1, T2>(
			TQ q,
			QueryDescription? query = null,
			int batchSize = 64
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

			batchSize = Math.Clamp(batchSize, 1, Archetype.CHUNK_SIZE);

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

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entityCount = chunk.EntityCount;
					if (entityCount == 0)
						continue;

					var numBatches = (int)Math.Ceiling(entityCount / (float)batchSize);

					var t0 = chunk.GetComponentArray<T0>(c0);
					var t1 = chunk.GetComponentArray<T1>(c1);
					var t2 = chunk.GetComponentArray<T2>(c2);

					Parallel.For(0, numBatches, b =>
                    {
						var start = b * batchSize;
						var end = Math.Min(start + batchSize, entityCount);

						var entities = chunk.Entities;
						for (var i = start; i < end; i++)
							q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i]);
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

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

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
						fixed (Entity* eptr = &entities[0])
						fixed (T0* t0ptr = &t0[0])
						fixed (T1* t1ptr = &t1[0])
						fixed (T2* t2ptr = &t2[0])
						fixed (T3* t3ptr = &t3[0])
						#pragma warning restore CS8500
						{
							for (var i = 0; i < entities.Length; i++)
								q.Execute(eptr[i], ref t0ptr[i], ref t1ptr[i], ref t2ptr[i], ref t3ptr[i]);
						}
					}
				}
			}

			return count;
		}

		public int ExecuteParallel<TQ, T0, T1, T2, T3>(
			TQ q,
			QueryDescription? query = null,
			int batchSize = 64
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

			batchSize = Math.Clamp(batchSize, 1, Archetype.CHUNK_SIZE);

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

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entityCount = chunk.EntityCount;
					if (entityCount == 0)
						continue;

					var numBatches = (int)Math.Ceiling(entityCount / (float)batchSize);

					var t0 = chunk.GetComponentArray<T0>(c0);
					var t1 = chunk.GetComponentArray<T1>(c1);
					var t2 = chunk.GetComponentArray<T2>(c2);
					var t3 = chunk.GetComponentArray<T3>(c3);

					Parallel.For(0, numBatches, b =>
                    {
						var start = b * batchSize;
						var end = Math.Min(start + batchSize, entityCount);

						var entities = chunk.Entities;
						for (var i = start; i < end; i++)
							q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i]);
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

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

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
						fixed (Entity* eptr = &entities[0])
						fixed (T0* t0ptr = &t0[0])
						fixed (T1* t1ptr = &t1[0])
						fixed (T2* t2ptr = &t2[0])
						fixed (T3* t3ptr = &t3[0])
						fixed (T4* t4ptr = &t4[0])
						#pragma warning restore CS8500
						{
							for (var i = 0; i < entities.Length; i++)
								q.Execute(eptr[i], ref t0ptr[i], ref t1ptr[i], ref t2ptr[i], ref t3ptr[i], ref t4ptr[i]);
						}
					}
				}
			}

			return count;
		}

		public int ExecuteParallel<TQ, T0, T1, T2, T3, T4>(
			TQ q,
			QueryDescription? query = null,
			int batchSize = 64
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

			batchSize = Math.Clamp(batchSize, 1, Archetype.CHUNK_SIZE);

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

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entityCount = chunk.EntityCount;
					if (entityCount == 0)
						continue;

					var numBatches = (int)Math.Ceiling(entityCount / (float)batchSize);

					var t0 = chunk.GetComponentArray<T0>(c0);
					var t1 = chunk.GetComponentArray<T1>(c1);
					var t2 = chunk.GetComponentArray<T2>(c2);
					var t3 = chunk.GetComponentArray<T3>(c3);
					var t4 = chunk.GetComponentArray<T4>(c4);

					Parallel.For(0, numBatches, b =>
                    {
						var start = b * batchSize;
						var end = Math.Min(start + batchSize, entityCount);

						var entities = chunk.Entities;
						for (var i = start; i < end; i++)
							q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i]);
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

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

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
						fixed (Entity* eptr = &entities[0])
						fixed (T0* t0ptr = &t0[0])
						fixed (T1* t1ptr = &t1[0])
						fixed (T2* t2ptr = &t2[0])
						fixed (T3* t3ptr = &t3[0])
						fixed (T4* t4ptr = &t4[0])
						fixed (T5* t5ptr = &t5[0])
						#pragma warning restore CS8500
						{
							for (var i = 0; i < entities.Length; i++)
								q.Execute(eptr[i], ref t0ptr[i], ref t1ptr[i], ref t2ptr[i], ref t3ptr[i], ref t4ptr[i], ref t5ptr[i]);
						}
					}
				}
			}

			return count;
		}

		public int ExecuteParallel<TQ, T0, T1, T2, T3, T4, T5>(
			TQ q,
			QueryDescription? query = null,
			int batchSize = 64
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

			batchSize = Math.Clamp(batchSize, 1, Archetype.CHUNK_SIZE);

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

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entityCount = chunk.EntityCount;
					if (entityCount == 0)
						continue;

					var numBatches = (int)Math.Ceiling(entityCount / (float)batchSize);

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

						var entities = chunk.Entities;
						for (var i = start; i < end; i++)
							q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i]);
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

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

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
						fixed (Entity* eptr = &entities[0])
						fixed (T0* t0ptr = &t0[0])
						fixed (T1* t1ptr = &t1[0])
						fixed (T2* t2ptr = &t2[0])
						fixed (T3* t3ptr = &t3[0])
						fixed (T4* t4ptr = &t4[0])
						fixed (T5* t5ptr = &t5[0])
						fixed (T6* t6ptr = &t6[0])
						#pragma warning restore CS8500
						{
							for (var i = 0; i < entities.Length; i++)
								q.Execute(eptr[i], ref t0ptr[i], ref t1ptr[i], ref t2ptr[i], ref t3ptr[i], ref t4ptr[i], ref t5ptr[i], ref t6ptr[i]);
						}
					}
				}
			}

			return count;
		}

		public int ExecuteParallel<TQ, T0, T1, T2, T3, T4, T5, T6>(
			TQ q,
			QueryDescription? query = null,
			int batchSize = 64
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

			batchSize = Math.Clamp(batchSize, 1, Archetype.CHUNK_SIZE);

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

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entityCount = chunk.EntityCount;
					if (entityCount == 0)
						continue;

					var numBatches = (int)Math.Ceiling(entityCount / (float)batchSize);

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

						var entities = chunk.Entities;
						for (var i = start; i < end; i++)
							q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i]);
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

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

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
						fixed (Entity* eptr = &entities[0])
						fixed (T0* t0ptr = &t0[0])
						fixed (T1* t1ptr = &t1[0])
						fixed (T2* t2ptr = &t2[0])
						fixed (T3* t3ptr = &t3[0])
						fixed (T4* t4ptr = &t4[0])
						fixed (T5* t5ptr = &t5[0])
						fixed (T6* t6ptr = &t6[0])
						fixed (T7* t7ptr = &t7[0])
						#pragma warning restore CS8500
						{
							for (var i = 0; i < entities.Length; i++)
								q.Execute(eptr[i], ref t0ptr[i], ref t1ptr[i], ref t2ptr[i], ref t3ptr[i], ref t4ptr[i], ref t5ptr[i], ref t6ptr[i], ref t7ptr[i]);
						}
					}
				}
			}

			return count;
		}

		public int ExecuteParallel<TQ, T0, T1, T2, T3, T4, T5, T6, T7>(
			TQ q,
			QueryDescription? query = null,
			int batchSize = 64
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

			batchSize = Math.Clamp(batchSize, 1, Archetype.CHUNK_SIZE);

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

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entityCount = chunk.EntityCount;
					if (entityCount == 0)
						continue;

					var numBatches = (int)Math.Ceiling(entityCount / (float)batchSize);

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

						var entities = chunk.Entities;
						for (var i = start; i < end; i++)
							q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i]);
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

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

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
						fixed (Entity* eptr = &entities[0])
						fixed (T0* t0ptr = &t0[0])
						fixed (T1* t1ptr = &t1[0])
						fixed (T2* t2ptr = &t2[0])
						fixed (T3* t3ptr = &t3[0])
						fixed (T4* t4ptr = &t4[0])
						fixed (T5* t5ptr = &t5[0])
						fixed (T6* t6ptr = &t6[0])
						fixed (T7* t7ptr = &t7[0])
						fixed (T8* t8ptr = &t8[0])
						#pragma warning restore CS8500
						{
							for (var i = 0; i < entities.Length; i++)
								q.Execute(eptr[i], ref t0ptr[i], ref t1ptr[i], ref t2ptr[i], ref t3ptr[i], ref t4ptr[i], ref t5ptr[i], ref t6ptr[i], ref t7ptr[i], ref t8ptr[i]);
						}
					}
				}
			}

			return count;
		}

		public int ExecuteParallel<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8>(
			TQ q,
			QueryDescription? query = null,
			int batchSize = 64
		)
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
	
			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				count += archetype.EntityCount;

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entityCount = chunk.EntityCount;
					if (entityCount == 0)
						continue;

					var numBatches = (int)Math.Ceiling(entityCount / (float)batchSize);

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

						var entities = chunk.Entities;
						for (var i = start; i < end; i++)
							q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i]);
					});
				}
			}

			return count;
		}
	}
}
namespace Myriad.ECS.Queries
{
	public interface IQuery10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
		where T0 : IComponent
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
		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
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
			where TQ : IQuery10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
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

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

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
						fixed (Entity* eptr = &entities[0])
						fixed (T0* t0ptr = &t0[0])
						fixed (T1* t1ptr = &t1[0])
						fixed (T2* t2ptr = &t2[0])
						fixed (T3* t3ptr = &t3[0])
						fixed (T4* t4ptr = &t4[0])
						fixed (T5* t5ptr = &t5[0])
						fixed (T6* t6ptr = &t6[0])
						fixed (T7* t7ptr = &t7[0])
						fixed (T8* t8ptr = &t8[0])
						fixed (T9* t9ptr = &t9[0])
						#pragma warning restore CS8500
						{
							for (var i = 0; i < entities.Length; i++)
								q.Execute(eptr[i], ref t0ptr[i], ref t1ptr[i], ref t2ptr[i], ref t3ptr[i], ref t4ptr[i], ref t5ptr[i], ref t6ptr[i], ref t7ptr[i], ref t8ptr[i], ref t9ptr[i]);
						}
					}
				}
			}

			return count;
		}

		public int ExecuteParallel<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
			TQ q,
			QueryDescription? query = null,
			int batchSize = 64
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
            where T8 : IComponent
            where T9 : IComponent
			where TQ : IQuery10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
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
	
			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				count += archetype.EntityCount;

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entityCount = chunk.EntityCount;
					if (entityCount == 0)
						continue;

					var numBatches = (int)Math.Ceiling(entityCount / (float)batchSize);

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

						var entities = chunk.Entities;
						for (var i = start; i < end; i++)
							q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i]);
					});
				}
			}

			return count;
		}
	}
}
namespace Myriad.ECS.Queries
{
	public interface IQuery11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
		where T0 : IComponent
        where T1 : IComponent
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
		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
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
			where TQ : IQuery11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
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

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

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
						fixed (Entity* eptr = &entities[0])
						fixed (T0* t0ptr = &t0[0])
						fixed (T1* t1ptr = &t1[0])
						fixed (T2* t2ptr = &t2[0])
						fixed (T3* t3ptr = &t3[0])
						fixed (T4* t4ptr = &t4[0])
						fixed (T5* t5ptr = &t5[0])
						fixed (T6* t6ptr = &t6[0])
						fixed (T7* t7ptr = &t7[0])
						fixed (T8* t8ptr = &t8[0])
						fixed (T9* t9ptr = &t9[0])
						fixed (T10* t10ptr = &t10[0])
						#pragma warning restore CS8500
						{
							for (var i = 0; i < entities.Length; i++)
								q.Execute(eptr[i], ref t0ptr[i], ref t1ptr[i], ref t2ptr[i], ref t3ptr[i], ref t4ptr[i], ref t5ptr[i], ref t6ptr[i], ref t7ptr[i], ref t8ptr[i], ref t9ptr[i], ref t10ptr[i]);
						}
					}
				}
			}

			return count;
		}

		public int ExecuteParallel<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
			TQ q,
			QueryDescription? query = null,
			int batchSize = 64
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
            where T8 : IComponent
            where T9 : IComponent
            where T10 : IComponent
			where TQ : IQuery11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
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
	
			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				count += archetype.EntityCount;

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entityCount = chunk.EntityCount;
					if (entityCount == 0)
						continue;

					var numBatches = (int)Math.Ceiling(entityCount / (float)batchSize);

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

						var entities = chunk.Entities;
						for (var i = start; i < end; i++)
							q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i]);
					});
				}
			}

			return count;
		}
	}
}
namespace Myriad.ECS.Queries
{
	public interface IQuery12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
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
		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
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
			where TQ : IQuery12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
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

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

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
						fixed (Entity* eptr = &entities[0])
						fixed (T0* t0ptr = &t0[0])
						fixed (T1* t1ptr = &t1[0])
						fixed (T2* t2ptr = &t2[0])
						fixed (T3* t3ptr = &t3[0])
						fixed (T4* t4ptr = &t4[0])
						fixed (T5* t5ptr = &t5[0])
						fixed (T6* t6ptr = &t6[0])
						fixed (T7* t7ptr = &t7[0])
						fixed (T8* t8ptr = &t8[0])
						fixed (T9* t9ptr = &t9[0])
						fixed (T10* t10ptr = &t10[0])
						fixed (T11* t11ptr = &t11[0])
						#pragma warning restore CS8500
						{
							for (var i = 0; i < entities.Length; i++)
								q.Execute(eptr[i], ref t0ptr[i], ref t1ptr[i], ref t2ptr[i], ref t3ptr[i], ref t4ptr[i], ref t5ptr[i], ref t6ptr[i], ref t7ptr[i], ref t8ptr[i], ref t9ptr[i], ref t10ptr[i], ref t11ptr[i]);
						}
					}
				}
			}

			return count;
		}

		public int ExecuteParallel<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
			TQ q,
			QueryDescription? query = null,
			int batchSize = 64
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
            where T8 : IComponent
            where T9 : IComponent
            where T10 : IComponent
            where T11 : IComponent
			where TQ : IQuery12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
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
	
			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				count += archetype.EntityCount;

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entityCount = chunk.EntityCount;
					if (entityCount == 0)
						continue;

					var numBatches = (int)Math.Ceiling(entityCount / (float)batchSize);

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

						var entities = chunk.Entities;
						for (var i = start; i < end; i++)
							q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i]);
					});
				}
			}

			return count;
		}
	}
}
namespace Myriad.ECS.Queries
{
	public interface IQuery13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
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
		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
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
			where TQ : IQuery13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
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

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

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
						fixed (Entity* eptr = &entities[0])
						fixed (T0* t0ptr = &t0[0])
						fixed (T1* t1ptr = &t1[0])
						fixed (T2* t2ptr = &t2[0])
						fixed (T3* t3ptr = &t3[0])
						fixed (T4* t4ptr = &t4[0])
						fixed (T5* t5ptr = &t5[0])
						fixed (T6* t6ptr = &t6[0])
						fixed (T7* t7ptr = &t7[0])
						fixed (T8* t8ptr = &t8[0])
						fixed (T9* t9ptr = &t9[0])
						fixed (T10* t10ptr = &t10[0])
						fixed (T11* t11ptr = &t11[0])
						fixed (T12* t12ptr = &t12[0])
						#pragma warning restore CS8500
						{
							for (var i = 0; i < entities.Length; i++)
								q.Execute(eptr[i], ref t0ptr[i], ref t1ptr[i], ref t2ptr[i], ref t3ptr[i], ref t4ptr[i], ref t5ptr[i], ref t6ptr[i], ref t7ptr[i], ref t8ptr[i], ref t9ptr[i], ref t10ptr[i], ref t11ptr[i], ref t12ptr[i]);
						}
					}
				}
			}

			return count;
		}

		public int ExecuteParallel<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
			TQ q,
			QueryDescription? query = null,
			int batchSize = 64
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
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
			where TQ : IQuery13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
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
	
			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				count += archetype.EntityCount;

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entityCount = chunk.EntityCount;
					if (entityCount == 0)
						continue;

					var numBatches = (int)Math.Ceiling(entityCount / (float)batchSize);

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

						var entities = chunk.Entities;
						for (var i = start; i < end; i++)
							q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i]);
					});
				}
			}

			return count;
		}
	}
}
namespace Myriad.ECS.Queries
{
	public interface IQuery14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
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
		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12, ref T13 t13);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
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
			where TQ : IQuery14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
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

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

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
						fixed (Entity* eptr = &entities[0])
						fixed (T0* t0ptr = &t0[0])
						fixed (T1* t1ptr = &t1[0])
						fixed (T2* t2ptr = &t2[0])
						fixed (T3* t3ptr = &t3[0])
						fixed (T4* t4ptr = &t4[0])
						fixed (T5* t5ptr = &t5[0])
						fixed (T6* t6ptr = &t6[0])
						fixed (T7* t7ptr = &t7[0])
						fixed (T8* t8ptr = &t8[0])
						fixed (T9* t9ptr = &t9[0])
						fixed (T10* t10ptr = &t10[0])
						fixed (T11* t11ptr = &t11[0])
						fixed (T12* t12ptr = &t12[0])
						fixed (T13* t13ptr = &t13[0])
						#pragma warning restore CS8500
						{
							for (var i = 0; i < entities.Length; i++)
								q.Execute(eptr[i], ref t0ptr[i], ref t1ptr[i], ref t2ptr[i], ref t3ptr[i], ref t4ptr[i], ref t5ptr[i], ref t6ptr[i], ref t7ptr[i], ref t8ptr[i], ref t9ptr[i], ref t10ptr[i], ref t11ptr[i], ref t12ptr[i], ref t13ptr[i]);
						}
					}
				}
			}

			return count;
		}

		public int ExecuteParallel<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
			TQ q,
			QueryDescription? query = null,
			int batchSize = 64
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
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
			where TQ : IQuery14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
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
	
			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				count += archetype.EntityCount;

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entityCount = chunk.EntityCount;
					if (entityCount == 0)
						continue;

					var numBatches = (int)Math.Ceiling(entityCount / (float)batchSize);

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

						var entities = chunk.Entities;
						for (var i = start; i < end; i++)
							q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i], ref t13[i]);
					});
				}
			}

			return count;
		}
	}
}
namespace Myriad.ECS.Queries
{
	public interface IQuery15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
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
		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12, ref T13 t13, ref T14 t14);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
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
			where TQ : IQuery15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
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

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

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
						fixed (Entity* eptr = &entities[0])
						fixed (T0* t0ptr = &t0[0])
						fixed (T1* t1ptr = &t1[0])
						fixed (T2* t2ptr = &t2[0])
						fixed (T3* t3ptr = &t3[0])
						fixed (T4* t4ptr = &t4[0])
						fixed (T5* t5ptr = &t5[0])
						fixed (T6* t6ptr = &t6[0])
						fixed (T7* t7ptr = &t7[0])
						fixed (T8* t8ptr = &t8[0])
						fixed (T9* t9ptr = &t9[0])
						fixed (T10* t10ptr = &t10[0])
						fixed (T11* t11ptr = &t11[0])
						fixed (T12* t12ptr = &t12[0])
						fixed (T13* t13ptr = &t13[0])
						fixed (T14* t14ptr = &t14[0])
						#pragma warning restore CS8500
						{
							for (var i = 0; i < entities.Length; i++)
								q.Execute(eptr[i], ref t0ptr[i], ref t1ptr[i], ref t2ptr[i], ref t3ptr[i], ref t4ptr[i], ref t5ptr[i], ref t6ptr[i], ref t7ptr[i], ref t8ptr[i], ref t9ptr[i], ref t10ptr[i], ref t11ptr[i], ref t12ptr[i], ref t13ptr[i], ref t14ptr[i]);
						}
					}
				}
			}

			return count;
		}

		public int ExecuteParallel<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
			TQ q,
			QueryDescription? query = null,
			int batchSize = 64
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
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
			where TQ : IQuery15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
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
	
			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				count += archetype.EntityCount;

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entityCount = chunk.EntityCount;
					if (entityCount == 0)
						continue;

					var numBatches = (int)Math.Ceiling(entityCount / (float)batchSize);

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

						var entities = chunk.Entities;
						for (var i = start; i < end; i++)
							q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i], ref t13[i], ref t14[i]);
					});
				}
			}

			return count;
		}
	}
}
namespace Myriad.ECS.Queries
{
	public interface IQuery16<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
		where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
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
		public void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref T9 t9, ref T10 t10, ref T11 t11, ref T12 t12, ref T13 t13, ref T14 t14, ref T15 t15);
	}
}

namespace Myriad.ECS.Worlds
{
	public partial class World
	{
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
			where TQ : IQuery16<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
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

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entities = chunk.Entities;
					if (entities.Length == 0)
						continue;

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
						fixed (Entity* eptr = &entities[0])
						fixed (T0* t0ptr = &t0[0])
						fixed (T1* t1ptr = &t1[0])
						fixed (T2* t2ptr = &t2[0])
						fixed (T3* t3ptr = &t3[0])
						fixed (T4* t4ptr = &t4[0])
						fixed (T5* t5ptr = &t5[0])
						fixed (T6* t6ptr = &t6[0])
						fixed (T7* t7ptr = &t7[0])
						fixed (T8* t8ptr = &t8[0])
						fixed (T9* t9ptr = &t9[0])
						fixed (T10* t10ptr = &t10[0])
						fixed (T11* t11ptr = &t11[0])
						fixed (T12* t12ptr = &t12[0])
						fixed (T13* t13ptr = &t13[0])
						fixed (T14* t14ptr = &t14[0])
						fixed (T15* t15ptr = &t15[0])
						#pragma warning restore CS8500
						{
							for (var i = 0; i < entities.Length; i++)
								q.Execute(eptr[i], ref t0ptr[i], ref t1ptr[i], ref t2ptr[i], ref t3ptr[i], ref t4ptr[i], ref t5ptr[i], ref t6ptr[i], ref t7ptr[i], ref t8ptr[i], ref t9ptr[i], ref t10ptr[i], ref t11ptr[i], ref t12ptr[i], ref t13ptr[i], ref t14ptr[i], ref t15ptr[i]);
						}
					}
				}
			}

			return count;
		}

		public int ExecuteParallel<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
			TQ q,
			QueryDescription? query = null,
			int batchSize = 64
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
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
			where TQ : IQuery16<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
		{
			query ??= GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>();

			var archetypes = query.GetArchetypes();
			if (archetypes.Count == 0)
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
	
			var count = 0;
			foreach (var archetypeMatch in archetypes)
			{
			    var archetype = archetypeMatch.Archetype;
				if (archetype.EntityCount == 0)
					continue;

				count += archetype.EntityCount;

				using var enumerator = archetype.GetChunkEnumerator();
                while (enumerator.MoveNext())
				{
					var chunk = enumerator.Current;
					Debug.Assert(chunk != null);

					var entityCount = chunk.EntityCount;
					if (entityCount == 0)
						continue;

					var numBatches = (int)Math.Ceiling(entityCount / (float)batchSize);

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

						var entities = chunk.Entities;
						for (var i = start; i < end; i++)
							q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i], ref t13[i], ref t14[i], ref t15[i]);
					});
				}
			}

			return count;
		}
	}
}


