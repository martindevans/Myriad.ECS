using Myriad.ECS.Worlds;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedParameter.Global
// ReSharper disable LoopCanBeConvertedToQuery

namespace Myriad.ECS.Queries;

public interface IQuery
{
	public int Execute(QueryDescription query, World world);

	public int ExecuteParallel(QueryDescription query, World world);
}

public interface IQueryR<T0>
    : IQuery
    where T0 : IComponent
{
	void Execute(Entity e, ref readonly T0 t0);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i]);
            });
        }

		return count;
    }
}

public interface IQueryRR<T0, T1>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
{
	void Execute(Entity e, ref readonly T0 t0, ref readonly T1 t1);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i]);
            });
        }

		return count;
    }
}

public interface IQueryRRR<T0, T1, T2>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
{
	void Execute(Entity e, ref readonly T0 t0, ref readonly T1 t1, ref readonly T2 t2);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i]);
            });
        }

		return count;
    }
}

public interface IQueryRRRR<T0, T1, T2, T3>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
{
	void Execute(Entity e, ref readonly T0 t0, ref readonly T1 t1, ref readonly T2 t2, ref readonly T3 t3);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i]);
            });
        }

		return count;
    }
}

public interface IQueryRRRRR<T0, T1, T2, T3, T4>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
{
	void Execute(Entity e, ref readonly T0 t0, ref readonly T1 t1, ref readonly T2 t2, ref readonly T3 t3, ref readonly T4 t4);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i]);
            });
        }

		return count;
    }
}

public interface IQueryRRRRRR<T0, T1, T2, T3, T4, T5>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
    where T5 : IComponent
{
	void Execute(Entity e, ref readonly T0 t0, ref readonly T1 t1, ref readonly T2 t2, ref readonly T3 t3, ref readonly T4 t4, ref readonly T5 t5);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i]);
            });
        }

		return count;
    }
}

public interface IQueryRRRRRRR<T0, T1, T2, T3, T4, T5, T6>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
    where T5 : IComponent
    where T6 : IComponent
{
	void Execute(Entity e, ref readonly T0 t0, ref readonly T1 t1, ref readonly T2 t2, ref readonly T3 t3, ref readonly T4 t4, ref readonly T5 t5, ref readonly T6 t6);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i]);
            });
        }

		return count;
    }
}

public interface IQueryRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
    where T5 : IComponent
    where T6 : IComponent
    where T7 : IComponent
{
	void Execute(Entity e, ref readonly T0 t0, ref readonly T1 t1, ref readonly T2 t2, ref readonly T3 t3, ref readonly T4 t4, ref readonly T5 t5, ref readonly T6 t6, ref readonly T7 t7);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i]);
            });
        }

		return count;
    }
}

public interface IQueryRRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8>
    : IQuery
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
	void Execute(Entity e, ref readonly T0 t0, ref readonly T1 t1, ref readonly T2 t2, ref readonly T3 t3, ref readonly T4 t4, ref readonly T5 t5, ref readonly T6 t6, ref readonly T7 t7, ref readonly T8 t8);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i]);
            });
        }

		return count;
    }
}

public interface IQueryW<T0>
    : IQuery
    where T0 : IComponent
{
	void Execute(Entity e, ref T0 t0);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i]);
            });
        }

		return count;
    }
}

public interface IQueryWR<T0, T1>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
{
	void Execute(Entity e, ref T0 t0, ref readonly T1 t1);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i]);
            });
        }

		return count;
    }
}

public interface IQueryWRR<T0, T1, T2>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
{
	void Execute(Entity e, ref T0 t0, ref readonly T1 t1, ref readonly T2 t2);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i]);
            });
        }

		return count;
    }
}

public interface IQueryWRRR<T0, T1, T2, T3>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
{
	void Execute(Entity e, ref T0 t0, ref readonly T1 t1, ref readonly T2 t2, ref readonly T3 t3);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i]);
            });
        }

		return count;
    }
}

public interface IQueryWRRRR<T0, T1, T2, T3, T4>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
{
	void Execute(Entity e, ref T0 t0, ref readonly T1 t1, ref readonly T2 t2, ref readonly T3 t3, ref readonly T4 t4);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i]);
            });
        }

		return count;
    }
}

public interface IQueryWRRRRR<T0, T1, T2, T3, T4, T5>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
    where T5 : IComponent
{
	void Execute(Entity e, ref T0 t0, ref readonly T1 t1, ref readonly T2 t2, ref readonly T3 t3, ref readonly T4 t4, ref readonly T5 t5);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i]);
            });
        }

		return count;
    }
}

public interface IQueryWRRRRRR<T0, T1, T2, T3, T4, T5, T6>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
    where T5 : IComponent
    where T6 : IComponent
{
	void Execute(Entity e, ref T0 t0, ref readonly T1 t1, ref readonly T2 t2, ref readonly T3 t3, ref readonly T4 t4, ref readonly T5 t5, ref readonly T6 t6);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i]);
            });
        }

		return count;
    }
}

public interface IQueryWRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
    where T5 : IComponent
    where T6 : IComponent
    where T7 : IComponent
{
	void Execute(Entity e, ref T0 t0, ref readonly T1 t1, ref readonly T2 t2, ref readonly T3 t3, ref readonly T4 t4, ref readonly T5 t5, ref readonly T6 t6, ref readonly T7 t7);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i]);
            });
        }

		return count;
    }
}

public interface IQueryWRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8>
    : IQuery
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
	void Execute(Entity e, ref T0 t0, ref readonly T1 t1, ref readonly T2 t2, ref readonly T3 t3, ref readonly T4 t4, ref readonly T5 t5, ref readonly T6 t6, ref readonly T7 t7, ref readonly T8 t8);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i]);
            });
        }

		return count;
    }
}

public interface IQueryWRRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
    : IQuery
    where T0 : IComponent
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
	void Execute(Entity e, ref T0 t0, ref readonly T1 t1, ref readonly T2 t2, ref readonly T3 t3, ref readonly T4 t4, ref readonly T5 t5, ref readonly T6 t6, ref readonly T7 t7, ref readonly T8 t8, ref readonly T9 t9);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i]);
            });
        }

		return count;
    }
}

public interface IQueryWW<T0, T1>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
{
	void Execute(Entity e, ref T0 t0, ref T1 t1);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWR<T0, T1, T2>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref readonly T2 t2);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWRR<T0, T1, T2, T3>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref readonly T2 t2, ref readonly T3 t3);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWRRR<T0, T1, T2, T3, T4>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref readonly T2 t2, ref readonly T3 t3, ref readonly T4 t4);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWRRRR<T0, T1, T2, T3, T4, T5>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
    where T5 : IComponent
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref readonly T2 t2, ref readonly T3 t3, ref readonly T4 t4, ref readonly T5 t5);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWRRRRR<T0, T1, T2, T3, T4, T5, T6>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
    where T5 : IComponent
    where T6 : IComponent
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref readonly T2 t2, ref readonly T3 t3, ref readonly T4 t4, ref readonly T5 t5, ref readonly T6 t6);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
    where T5 : IComponent
    where T6 : IComponent
    where T7 : IComponent
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref readonly T2 t2, ref readonly T3 t3, ref readonly T4 t4, ref readonly T5 t5, ref readonly T6 t6, ref readonly T7 t7);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8>
    : IQuery
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref readonly T2 t2, ref readonly T3 t3, ref readonly T4 t4, ref readonly T5 t5, ref readonly T6 t6, ref readonly T7 t7, ref readonly T8 t8);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
    : IQuery
    where T0 : IComponent
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref readonly T2 t2, ref readonly T3 t3, ref readonly T4 t4, ref readonly T5 t5, ref readonly T6 t6, ref readonly T7 t7, ref readonly T8 t8, ref readonly T9 t9);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWRRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref readonly T2 t2, ref readonly T3 t3, ref readonly T4 t4, ref readonly T5 t5, ref readonly T6 t6, ref readonly T7 t7, ref readonly T8 t8, ref readonly T9 t9, ref readonly T10 t10);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWW<T0, T1, T2>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWR<T0, T1, T2, T3>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref readonly T3 t3);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWRR<T0, T1, T2, T3, T4>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref readonly T3 t3, ref readonly T4 t4);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWRRR<T0, T1, T2, T3, T4, T5>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
    where T5 : IComponent
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref readonly T3 t3, ref readonly T4 t4, ref readonly T5 t5);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWRRRR<T0, T1, T2, T3, T4, T5, T6>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
    where T5 : IComponent
    where T6 : IComponent
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref readonly T3 t3, ref readonly T4 t4, ref readonly T5 t5, ref readonly T6 t6);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWRRRRR<T0, T1, T2, T3, T4, T5, T6, T7>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
    where T5 : IComponent
    where T6 : IComponent
    where T7 : IComponent
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref readonly T3 t3, ref readonly T4 t4, ref readonly T5 t5, ref readonly T6 t6, ref readonly T7 t7);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8>
    : IQuery
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref readonly T3 t3, ref readonly T4 t4, ref readonly T5 t5, ref readonly T6 t6, ref readonly T7 t7, ref readonly T8 t8);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
    : IQuery
    where T0 : IComponent
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref readonly T3 t3, ref readonly T4 t4, ref readonly T5 t5, ref readonly T6 t6, ref readonly T7 t7, ref readonly T8 t8, ref readonly T9 t9);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref readonly T3 t3, ref readonly T4 t4, ref readonly T5 t5, ref readonly T6 t6, ref readonly T7 t7, ref readonly T8 t8, ref readonly T9 t9, ref readonly T10 t10);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWRRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref readonly T3 t3, ref readonly T4 t4, ref readonly T5 t5, ref readonly T6 t6, ref readonly T7 t7, ref readonly T8 t8, ref readonly T9 t9, ref readonly T10 t10, ref readonly T11 t11);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWW<T0, T1, T2, T3>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWR<T0, T1, T2, T3, T4>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref readonly T4 t4);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWRR<T0, T1, T2, T3, T4, T5>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
    where T5 : IComponent
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref readonly T4 t4, ref readonly T5 t5);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWRRR<T0, T1, T2, T3, T4, T5, T6>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
    where T5 : IComponent
    where T6 : IComponent
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref readonly T4 t4, ref readonly T5 t5, ref readonly T6 t6);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWRRRR<T0, T1, T2, T3, T4, T5, T6, T7>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
    where T5 : IComponent
    where T6 : IComponent
    where T7 : IComponent
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref readonly T4 t4, ref readonly T5 t5, ref readonly T6 t6, ref readonly T7 t7);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8>
    : IQuery
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref readonly T4 t4, ref readonly T5 t5, ref readonly T6 t6, ref readonly T7 t7, ref readonly T8 t8);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
    : IQuery
    where T0 : IComponent
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref readonly T4 t4, ref readonly T5 t5, ref readonly T6 t6, ref readonly T7 t7, ref readonly T8 t8, ref readonly T9 t9);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref readonly T4 t4, ref readonly T5 t5, ref readonly T6 t6, ref readonly T7 t7, ref readonly T8 t8, ref readonly T9 t9, ref readonly T10 t10);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref readonly T4 t4, ref readonly T5 t5, ref readonly T6 t6, ref readonly T7 t7, ref readonly T8 t8, ref readonly T9 t9, ref readonly T10 t10, ref readonly T11 t11);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWRRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref readonly T4 t4, ref readonly T5 t5, ref readonly T6 t6, ref readonly T7 t7, ref readonly T8 t8, ref readonly T9 t9, ref readonly T10 t10, ref readonly T11 t11, ref readonly T12 t12);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();
                var t12 = chunk.GetSpan<T12>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();
                var t12 = chunk.GetSpan<T12>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWW<T0, T1, T2, T3, T4>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWR<T0, T1, T2, T3, T4, T5>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
    where T5 : IComponent
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref readonly T5 t5);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWRR<T0, T1, T2, T3, T4, T5, T6>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
    where T5 : IComponent
    where T6 : IComponent
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref readonly T5 t5, ref readonly T6 t6);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWRRR<T0, T1, T2, T3, T4, T5, T6, T7>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
    where T5 : IComponent
    where T6 : IComponent
    where T7 : IComponent
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref readonly T5 t5, ref readonly T6 t6, ref readonly T7 t7);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8>
    : IQuery
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref readonly T5 t5, ref readonly T6 t6, ref readonly T7 t7, ref readonly T8 t8);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
    : IQuery
    where T0 : IComponent
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref readonly T5 t5, ref readonly T6 t6, ref readonly T7 t7, ref readonly T8 t8, ref readonly T9 t9);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref readonly T5 t5, ref readonly T6 t6, ref readonly T7 t7, ref readonly T8 t8, ref readonly T9 t9, ref readonly T10 t10);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref readonly T5 t5, ref readonly T6 t6, ref readonly T7 t7, ref readonly T8 t8, ref readonly T9 t9, ref readonly T10 t10, ref readonly T11 t11);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref readonly T5 t5, ref readonly T6 t6, ref readonly T7 t7, ref readonly T8 t8, ref readonly T9 t9, ref readonly T10 t10, ref readonly T11 t11, ref readonly T12 t12);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();
                var t12 = chunk.GetSpan<T12>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();
                var t12 = chunk.GetSpan<T12>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWRRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref readonly T5 t5, ref readonly T6 t6, ref readonly T7 t7, ref readonly T8 t8, ref readonly T9 t9, ref readonly T10 t10, ref readonly T11 t11, ref readonly T12 t12, ref readonly T13 t13);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();
                var t12 = chunk.GetSpan<T12>();
                var t13 = chunk.GetSpan<T13>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i], ref t13[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();
                var t12 = chunk.GetSpan<T12>();
                var t13 = chunk.GetSpan<T13>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i], ref t13[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWW<T0, T1, T2, T3, T4, T5>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
    where T5 : IComponent
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWWR<T0, T1, T2, T3, T4, T5, T6>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
    where T5 : IComponent
    where T6 : IComponent
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref readonly T6 t6);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWWRR<T0, T1, T2, T3, T4, T5, T6, T7>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
    where T5 : IComponent
    where T6 : IComponent
    where T7 : IComponent
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref readonly T6 t6, ref readonly T7 t7);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWWRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8>
    : IQuery
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref readonly T6 t6, ref readonly T7 t7, ref readonly T8 t8);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWWRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
    : IQuery
    where T0 : IComponent
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref readonly T6 t6, ref readonly T7 t7, ref readonly T8 t8, ref readonly T9 t9);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWWRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref readonly T6 t6, ref readonly T7 t7, ref readonly T8 t8, ref readonly T9 t9, ref readonly T10 t10);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWWRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref readonly T6 t6, ref readonly T7 t7, ref readonly T8 t8, ref readonly T9 t9, ref readonly T10 t10, ref readonly T11 t11);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWWRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref readonly T6 t6, ref readonly T7 t7, ref readonly T8 t8, ref readonly T9 t9, ref readonly T10 t10, ref readonly T11 t11, ref readonly T12 t12);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();
                var t12 = chunk.GetSpan<T12>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();
                var t12 = chunk.GetSpan<T12>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWWRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref readonly T6 t6, ref readonly T7 t7, ref readonly T8 t8, ref readonly T9 t9, ref readonly T10 t10, ref readonly T11 t11, ref readonly T12 t12, ref readonly T13 t13);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();
                var t12 = chunk.GetSpan<T12>();
                var t13 = chunk.GetSpan<T13>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i], ref t13[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();
                var t12 = chunk.GetSpan<T12>();
                var t13 = chunk.GetSpan<T13>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i], ref t13[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWWRRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref readonly T6 t6, ref readonly T7 t7, ref readonly T8 t8, ref readonly T9 t9, ref readonly T10 t10, ref readonly T11 t11, ref readonly T12 t12, ref readonly T13 t13, ref readonly T14 t14);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();
                var t12 = chunk.GetSpan<T12>();
                var t13 = chunk.GetSpan<T13>();
                var t14 = chunk.GetSpan<T14>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i], ref t13[i], ref t14[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();
                var t12 = chunk.GetSpan<T12>();
                var t13 = chunk.GetSpan<T13>();
                var t14 = chunk.GetSpan<T14>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i], ref t13[i], ref t14[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWWW<T0, T1, T2, T3, T4, T5, T6>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
    where T5 : IComponent
    where T6 : IComponent
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWWWR<T0, T1, T2, T3, T4, T5, T6, T7>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
    where T5 : IComponent
    where T6 : IComponent
    where T7 : IComponent
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref readonly T7 t7);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWWWRR<T0, T1, T2, T3, T4, T5, T6, T7, T8>
    : IQuery
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref readonly T7 t7, ref readonly T8 t8);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWWWRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
    : IQuery
    where T0 : IComponent
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref readonly T7 t7, ref readonly T8 t8, ref readonly T9 t9);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWWWRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref readonly T7 t7, ref readonly T8 t8, ref readonly T9 t9, ref readonly T10 t10);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWWWRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref readonly T7 t7, ref readonly T8 t8, ref readonly T9 t9, ref readonly T10 t10, ref readonly T11 t11);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWWWRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref readonly T7 t7, ref readonly T8 t8, ref readonly T9 t9, ref readonly T10 t10, ref readonly T11 t11, ref readonly T12 t12);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();
                var t12 = chunk.GetSpan<T12>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();
                var t12 = chunk.GetSpan<T12>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWWWRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref readonly T7 t7, ref readonly T8 t8, ref readonly T9 t9, ref readonly T10 t10, ref readonly T11 t11, ref readonly T12 t12, ref readonly T13 t13);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();
                var t12 = chunk.GetSpan<T12>();
                var t13 = chunk.GetSpan<T13>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i], ref t13[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();
                var t12 = chunk.GetSpan<T12>();
                var t13 = chunk.GetSpan<T13>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i], ref t13[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWWWRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref readonly T7 t7, ref readonly T8 t8, ref readonly T9 t9, ref readonly T10 t10, ref readonly T11 t11, ref readonly T12 t12, ref readonly T13 t13, ref readonly T14 t14);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();
                var t12 = chunk.GetSpan<T12>();
                var t13 = chunk.GetSpan<T13>();
                var t14 = chunk.GetSpan<T14>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i], ref t13[i], ref t14[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();
                var t12 = chunk.GetSpan<T12>();
                var t13 = chunk.GetSpan<T13>();
                var t14 = chunk.GetSpan<T14>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i], ref t13[i], ref t14[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWWWRRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref readonly T7 t7, ref readonly T8 t8, ref readonly T9 t9, ref readonly T10 t10, ref readonly T11 t11, ref readonly T12 t12, ref readonly T13 t13, ref readonly T14 t14, ref readonly T15 t15);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();
                var t12 = chunk.GetSpan<T12>();
                var t13 = chunk.GetSpan<T13>();
                var t14 = chunk.GetSpan<T14>();
                var t15 = chunk.GetSpan<T15>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i], ref t13[i], ref t14[i], ref t15[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();
                var t12 = chunk.GetSpan<T12>();
                var t13 = chunk.GetSpan<T13>();
                var t14 = chunk.GetSpan<T14>();
                var t15 = chunk.GetSpan<T15>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i], ref t13[i], ref t14[i], ref t15[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWWWW<T0, T1, T2, T3, T4, T5, T6, T7>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
    where T5 : IComponent
    where T6 : IComponent
    where T7 : IComponent
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWWWWR<T0, T1, T2, T3, T4, T5, T6, T7, T8>
    : IQuery
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref readonly T8 t8);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWWWWRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
    : IQuery
    where T0 : IComponent
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref readonly T8 t8, ref readonly T9 t9);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWWWWRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref readonly T8 t8, ref readonly T9 t9, ref readonly T10 t10);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWWWWRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref readonly T8 t8, ref readonly T9 t9, ref readonly T10 t10, ref readonly T11 t11);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWWWWRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref readonly T8 t8, ref readonly T9 t9, ref readonly T10 t10, ref readonly T11 t11, ref readonly T12 t12);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();
                var t12 = chunk.GetSpan<T12>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();
                var t12 = chunk.GetSpan<T12>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWWWWRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref readonly T8 t8, ref readonly T9 t9, ref readonly T10 t10, ref readonly T11 t11, ref readonly T12 t12, ref readonly T13 t13);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();
                var t12 = chunk.GetSpan<T12>();
                var t13 = chunk.GetSpan<T13>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i], ref t13[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();
                var t12 = chunk.GetSpan<T12>();
                var t13 = chunk.GetSpan<T13>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i], ref t13[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWWWWRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref readonly T8 t8, ref readonly T9 t9, ref readonly T10 t10, ref readonly T11 t11, ref readonly T12 t12, ref readonly T13 t13, ref readonly T14 t14);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();
                var t12 = chunk.GetSpan<T12>();
                var t13 = chunk.GetSpan<T13>();
                var t14 = chunk.GetSpan<T14>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i], ref t13[i], ref t14[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();
                var t12 = chunk.GetSpan<T12>();
                var t13 = chunk.GetSpan<T13>();
                var t14 = chunk.GetSpan<T14>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i], ref t13[i], ref t14[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWWWWRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref readonly T8 t8, ref readonly T9 t9, ref readonly T10 t10, ref readonly T11 t11, ref readonly T12 t12, ref readonly T13 t13, ref readonly T14 t14, ref readonly T15 t15);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();
                var t12 = chunk.GetSpan<T12>();
                var t13 = chunk.GetSpan<T13>();
                var t14 = chunk.GetSpan<T14>();
                var t15 = chunk.GetSpan<T15>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i], ref t13[i], ref t14[i], ref t15[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();
                var t12 = chunk.GetSpan<T12>();
                var t13 = chunk.GetSpan<T13>();
                var t14 = chunk.GetSpan<T14>();
                var t15 = chunk.GetSpan<T15>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i], ref t13[i], ref t14[i], ref t15[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWWWWRRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
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
    where T16 : IComponent
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref readonly T8 t8, ref readonly T9 t9, ref readonly T10 t10, ref readonly T11 t11, ref readonly T12 t12, ref readonly T13 t13, ref readonly T14 t14, ref readonly T15 t15, ref readonly T16 t16);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();
                var t12 = chunk.GetSpan<T12>();
                var t13 = chunk.GetSpan<T13>();
                var t14 = chunk.GetSpan<T14>();
                var t15 = chunk.GetSpan<T15>();
                var t16 = chunk.GetSpan<T16>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i], ref t13[i], ref t14[i], ref t15[i], ref t16[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();
                var t12 = chunk.GetSpan<T12>();
                var t13 = chunk.GetSpan<T13>();
                var t14 = chunk.GetSpan<T14>();
                var t15 = chunk.GetSpan<T15>();
                var t16 = chunk.GetSpan<T16>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i], ref t13[i], ref t14[i], ref t15[i], ref t16[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWWWWW<T0, T1, T2, T3, T4, T5, T6, T7, T8>
    : IQuery
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWWWWWR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
    : IQuery
    where T0 : IComponent
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref readonly T9 t9);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWWWWWRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref readonly T9 t9, ref readonly T10 t10);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWWWWWRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref readonly T9 t9, ref readonly T10 t10, ref readonly T11 t11);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWWWWWRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref readonly T9 t9, ref readonly T10 t10, ref readonly T11 t11, ref readonly T12 t12);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();
                var t12 = chunk.GetSpan<T12>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();
                var t12 = chunk.GetSpan<T12>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWWWWWRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref readonly T9 t9, ref readonly T10 t10, ref readonly T11 t11, ref readonly T12 t12, ref readonly T13 t13);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();
                var t12 = chunk.GetSpan<T12>();
                var t13 = chunk.GetSpan<T13>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i], ref t13[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();
                var t12 = chunk.GetSpan<T12>();
                var t13 = chunk.GetSpan<T13>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i], ref t13[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWWWWWRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref readonly T9 t9, ref readonly T10 t10, ref readonly T11 t11, ref readonly T12 t12, ref readonly T13 t13, ref readonly T14 t14);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();
                var t12 = chunk.GetSpan<T12>();
                var t13 = chunk.GetSpan<T13>();
                var t14 = chunk.GetSpan<T14>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i], ref t13[i], ref t14[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();
                var t12 = chunk.GetSpan<T12>();
                var t13 = chunk.GetSpan<T13>();
                var t14 = chunk.GetSpan<T14>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i], ref t13[i], ref t14[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWWWWWRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref readonly T9 t9, ref readonly T10 t10, ref readonly T11 t11, ref readonly T12 t12, ref readonly T13 t13, ref readonly T14 t14, ref readonly T15 t15);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();
                var t12 = chunk.GetSpan<T12>();
                var t13 = chunk.GetSpan<T13>();
                var t14 = chunk.GetSpan<T14>();
                var t15 = chunk.GetSpan<T15>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i], ref t13[i], ref t14[i], ref t15[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();
                var t12 = chunk.GetSpan<T12>();
                var t13 = chunk.GetSpan<T13>();
                var t14 = chunk.GetSpan<T14>();
                var t15 = chunk.GetSpan<T15>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i], ref t13[i], ref t14[i], ref t15[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWWWWWRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
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
    where T16 : IComponent
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref readonly T9 t9, ref readonly T10 t10, ref readonly T11 t11, ref readonly T12 t12, ref readonly T13 t13, ref readonly T14 t14, ref readonly T15 t15, ref readonly T16 t16);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();
                var t12 = chunk.GetSpan<T12>();
                var t13 = chunk.GetSpan<T13>();
                var t14 = chunk.GetSpan<T14>();
                var t15 = chunk.GetSpan<T15>();
                var t16 = chunk.GetSpan<T16>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i], ref t13[i], ref t14[i], ref t15[i], ref t16[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();
                var t12 = chunk.GetSpan<T12>();
                var t13 = chunk.GetSpan<T13>();
                var t14 = chunk.GetSpan<T14>();
                var t15 = chunk.GetSpan<T15>();
                var t16 = chunk.GetSpan<T16>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i], ref t13[i], ref t14[i], ref t15[i], ref t16[i]);
            });
        }

		return count;
    }
}

public interface IQueryWWWWWWWWWRRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
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
    where T16 : IComponent
    where T17 : IComponent
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, ref readonly T9 t9, ref readonly T10 t10, ref readonly T11 t11, ref readonly T12 t12, ref readonly T13 t13, ref readonly T14 t14, ref readonly T15 t15, ref readonly T16 t16, ref readonly T17 t17);

    int IQuery.Execute(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			var chunks = archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    continue;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();
                var t12 = chunk.GetSpan<T12>();
                var t13 = chunk.GetSpan<T13>();
                var t14 = chunk.GetSpan<T14>();
                var t15 = chunk.GetSpan<T15>();
                var t16 = chunk.GetSpan<T16>();
                var t17 = chunk.GetSpan<T17>();

                count += entities.Length;

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i], ref t13[i], ref t14[i], ref t15[i], ref t16[i], ref t17[i]);
            }
        }

		return count;
    }

	int IQuery.ExecuteParallel(QueryDescription query, World world)
    {
		var archetypes = query.GetArchetypes();

		var count = 0;
        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetype = archetypes[a].Archetype;
            if (archetype.EntityCount == 0)
                continue;

			Parallel.For(0, archetype.Chunks.Count, c =>
            {
				var chunk = archetype.Chunks[c];

                var entities = chunk.Entities;
				if (entities.Length == 0)
				    return;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();
                var t9 = chunk.GetSpan<T9>();
                var t10 = chunk.GetSpan<T10>();
                var t11 = chunk.GetSpan<T11>();
                var t12 = chunk.GetSpan<T12>();
                var t13 = chunk.GetSpan<T13>();
                var t14 = chunk.GetSpan<T14>();
                var t15 = chunk.GetSpan<T15>();
                var t16 = chunk.GetSpan<T16>();
                var t17 = chunk.GetSpan<T17>();

                Interlocked.Add(ref count, entities.Length);

				for (var i = entities.Length - 1; i >= 0; i--)
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], ref t9[i], ref t10[i], ref t11[i], ref t12[i], ref t13[i], ref t14[i], ref t15[i], ref t16[i], ref t17[i]);
            });
        }

		return count;
    }
}



