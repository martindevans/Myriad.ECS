using Myriad.ECS.Queries;
using Myriad.ECS.IDs;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedParameter.Global
// ReSharper disable LoopCanBeConvertedToQuery
#pragma warning disable CA1822 // Mark members as static

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
			QueryDescription query
		)
			where T0 : IComponent
			where TQ : IQuery1<T0>
		{
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
			QueryDescription query
		)
			where T0 : IComponent
			where TQ : IQuery1<T0>
		{
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
					if (chunk.EntityCount == 0)
						continue;

					var t0 = chunk.GetComponentArray<T0>(c0);

                    Parallel.For(0, chunk.EntityCount, i =>
                    {
						var entities = chunk.Entities;
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
			QueryDescription query
		)
			where T0 : IComponent
            where T1 : IComponent
			where TQ : IQuery2<T0, T1>
		{
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
			QueryDescription query
		)
			where T0 : IComponent
            where T1 : IComponent
			where TQ : IQuery2<T0, T1>
		{
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
					if (chunk.EntityCount == 0)
						continue;

					var t0 = chunk.GetComponentArray<T0>(c0);
					var t1 = chunk.GetComponentArray<T1>(c1);

                    Parallel.For(0, chunk.EntityCount, i =>
                    {
						var entities = chunk.Entities;
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
			QueryDescription query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
			where TQ : IQuery3<T0, T1, T2>
		{
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
			QueryDescription query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
			where TQ : IQuery3<T0, T1, T2>
		{
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
					if (chunk.EntityCount == 0)
						continue;

					var t0 = chunk.GetComponentArray<T0>(c0);
					var t1 = chunk.GetComponentArray<T1>(c1);
					var t2 = chunk.GetComponentArray<T2>(c2);

                    Parallel.For(0, chunk.EntityCount, i =>
                    {
						var entities = chunk.Entities;
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
			QueryDescription query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
			where TQ : IQuery4<T0, T1, T2, T3>
		{
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
			QueryDescription query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
			where TQ : IQuery4<T0, T1, T2, T3>
		{
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
					if (chunk.EntityCount == 0)
						continue;

					var t0 = chunk.GetComponentArray<T0>(c0);
					var t1 = chunk.GetComponentArray<T1>(c1);
					var t2 = chunk.GetComponentArray<T2>(c2);
					var t3 = chunk.GetComponentArray<T3>(c3);

                    Parallel.For(0, chunk.EntityCount, i =>
                    {
						var entities = chunk.Entities;
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
			QueryDescription query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
			where TQ : IQuery5<T0, T1, T2, T3, T4>
		{
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
			QueryDescription query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
			where TQ : IQuery5<T0, T1, T2, T3, T4>
		{
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
					if (chunk.EntityCount == 0)
						continue;

					var t0 = chunk.GetComponentArray<T0>(c0);
					var t1 = chunk.GetComponentArray<T1>(c1);
					var t2 = chunk.GetComponentArray<T2>(c2);
					var t3 = chunk.GetComponentArray<T3>(c3);
					var t4 = chunk.GetComponentArray<T4>(c4);

                    Parallel.For(0, chunk.EntityCount, i =>
                    {
						var entities = chunk.Entities;
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
			QueryDescription query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
			where TQ : IQuery6<T0, T1, T2, T3, T4, T5>
		{
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
			QueryDescription query
		)
			where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
			where TQ : IQuery6<T0, T1, T2, T3, T4, T5>
		{
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
					if (chunk.EntityCount == 0)
						continue;

					var t0 = chunk.GetComponentArray<T0>(c0);
					var t1 = chunk.GetComponentArray<T1>(c1);
					var t2 = chunk.GetComponentArray<T2>(c2);
					var t3 = chunk.GetComponentArray<T3>(c3);
					var t4 = chunk.GetComponentArray<T4>(c4);
					var t5 = chunk.GetComponentArray<T5>(c5);

                    Parallel.For(0, chunk.EntityCount, i =>
                    {
						var entities = chunk.Entities;
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
			QueryDescription query
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
			QueryDescription query
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
					if (chunk.EntityCount == 0)
						continue;

					var t0 = chunk.GetComponentArray<T0>(c0);
					var t1 = chunk.GetComponentArray<T1>(c1);
					var t2 = chunk.GetComponentArray<T2>(c2);
					var t3 = chunk.GetComponentArray<T3>(c3);
					var t4 = chunk.GetComponentArray<T4>(c4);
					var t5 = chunk.GetComponentArray<T5>(c5);
					var t6 = chunk.GetComponentArray<T6>(c6);

                    Parallel.For(0, chunk.EntityCount, i =>
                    {
						var entities = chunk.Entities;
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
			QueryDescription query
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
			QueryDescription query
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
					if (chunk.EntityCount == 0)
						continue;

					var t0 = chunk.GetComponentArray<T0>(c0);
					var t1 = chunk.GetComponentArray<T1>(c1);
					var t2 = chunk.GetComponentArray<T2>(c2);
					var t3 = chunk.GetComponentArray<T3>(c3);
					var t4 = chunk.GetComponentArray<T4>(c4);
					var t5 = chunk.GetComponentArray<T5>(c5);
					var t6 = chunk.GetComponentArray<T6>(c6);
					var t7 = chunk.GetComponentArray<T7>(c7);

                    Parallel.For(0, chunk.EntityCount, i =>
                    {
						var entities = chunk.Entities;
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
			QueryDescription query
		)
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
			QueryDescription query
		)
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
					if (chunk.EntityCount == 0)
						continue;

					var t0 = chunk.GetComponentArray<T0>(c0);
					var t1 = chunk.GetComponentArray<T1>(c1);
					var t2 = chunk.GetComponentArray<T2>(c2);
					var t3 = chunk.GetComponentArray<T3>(c3);
					var t4 = chunk.GetComponentArray<T4>(c4);
					var t5 = chunk.GetComponentArray<T5>(c5);
					var t6 = chunk.GetComponentArray<T6>(c6);
					var t7 = chunk.GetComponentArray<T7>(c7);
					var t8 = chunk.GetComponentArray<T8>(c8);

                    Parallel.For(0, chunk.EntityCount, i =>
                    {
						var entities = chunk.Entities;
						q.Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i]);
					});
				}
			}

			return count;
		}
	}
}


