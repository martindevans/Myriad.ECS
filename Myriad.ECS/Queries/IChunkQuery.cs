using Myriad.ECS.Worlds;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedParameter.Global
// ReSharper disable LoopCanBeConvertedToQuery

namespace Myriad.ECS.Queries;

public interface IChunkQueryR<T0>
    : IQuery
    where T0 : IComponent
{
	void Execute(ReadOnlySpan<Entity> e, ReadOnlySpan<T0> t0);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();

                Execute(entities, t0);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();

                Execute(entities, t0);
            });
        }

		return count;
    }
}

public interface IChunkQueryRR<T0, T1>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
{
	void Execute(ReadOnlySpan<Entity> e, ReadOnlySpan<T0> t0, ReadOnlySpan<T1> t1);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();

                Execute(entities, t0, t1);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();

                Execute(entities, t0, t1);
            });
        }

		return count;
    }
}

public interface IChunkQueryRRR<T0, T1, T2>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
{
	void Execute(ReadOnlySpan<Entity> e, ReadOnlySpan<T0> t0, ReadOnlySpan<T1> t1, ReadOnlySpan<T2> t2);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();

                Execute(entities, t0, t1, t2);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();

                Execute(entities, t0, t1, t2);
            });
        }

		return count;
    }
}

public interface IChunkQueryRRRR<T0, T1, T2, T3>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
{
	void Execute(ReadOnlySpan<Entity> e, ReadOnlySpan<T0> t0, ReadOnlySpan<T1> t1, ReadOnlySpan<T2> t2, ReadOnlySpan<T3> t3);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();

                Execute(entities, t0, t1, t2, t3);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();

                Execute(entities, t0, t1, t2, t3);
            });
        }

		return count;
    }
}

public interface IChunkQueryRRRRR<T0, T1, T2, T3, T4>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
{
	void Execute(ReadOnlySpan<Entity> e, ReadOnlySpan<T0> t0, ReadOnlySpan<T1> t1, ReadOnlySpan<T2> t2, ReadOnlySpan<T3> t3, ReadOnlySpan<T4> t4);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();

                Execute(entities, t0, t1, t2, t3, t4);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();

                Execute(entities, t0, t1, t2, t3, t4);
            });
        }

		return count;
    }
}

public interface IChunkQueryRRRRRR<T0, T1, T2, T3, T4, T5>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
    where T5 : IComponent
{
	void Execute(ReadOnlySpan<Entity> e, ReadOnlySpan<T0> t0, ReadOnlySpan<T1> t1, ReadOnlySpan<T2> t2, ReadOnlySpan<T3> t3, ReadOnlySpan<T4> t4, ReadOnlySpan<T5> t5);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();

                Execute(entities, t0, t1, t2, t3, t4, t5);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();

                Execute(entities, t0, t1, t2, t3, t4, t5);
            });
        }

		return count;
    }
}

public interface IChunkQueryRRRRRRR<T0, T1, T2, T3, T4, T5, T6>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
    where T5 : IComponent
    where T6 : IComponent
{
	void Execute(ReadOnlySpan<Entity> e, ReadOnlySpan<T0> t0, ReadOnlySpan<T1> t1, ReadOnlySpan<T2> t2, ReadOnlySpan<T3> t3, ReadOnlySpan<T4> t4, ReadOnlySpan<T5> t5, ReadOnlySpan<T6> t6);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6);
            });
        }

		return count;
    }
}

public interface IChunkQueryRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7>
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
	void Execute(ReadOnlySpan<Entity> e, ReadOnlySpan<T0> t0, ReadOnlySpan<T1> t1, ReadOnlySpan<T2> t2, ReadOnlySpan<T3> t3, ReadOnlySpan<T4> t4, ReadOnlySpan<T5> t5, ReadOnlySpan<T6> t6, ReadOnlySpan<T7> t7);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7);
            });
        }

		return count;
    }
}

public interface IChunkQueryRRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8>
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
	void Execute(ReadOnlySpan<Entity> e, ReadOnlySpan<T0> t0, ReadOnlySpan<T1> t1, ReadOnlySpan<T2> t2, ReadOnlySpan<T3> t3, ReadOnlySpan<T4> t4, ReadOnlySpan<T5> t5, ReadOnlySpan<T6> t6, ReadOnlySpan<T7> t7, ReadOnlySpan<T8> t8);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8);
            });
        }

		return count;
    }
}

public interface IChunkQueryW<T0>
    : IQuery
    where T0 : IComponent
{
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();

                Execute(entities, t0);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();

                Execute(entities, t0);
            });
        }

		return count;
    }
}

public interface IChunkQueryWR<T0, T1>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
{
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, ReadOnlySpan<T1> t1);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();

                Execute(entities, t0, t1);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();

                Execute(entities, t0, t1);
            });
        }

		return count;
    }
}

public interface IChunkQueryWRR<T0, T1, T2>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
{
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, ReadOnlySpan<T1> t1, ReadOnlySpan<T2> t2);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();

                Execute(entities, t0, t1, t2);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();

                Execute(entities, t0, t1, t2);
            });
        }

		return count;
    }
}

public interface IChunkQueryWRRR<T0, T1, T2, T3>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
{
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, ReadOnlySpan<T1> t1, ReadOnlySpan<T2> t2, ReadOnlySpan<T3> t3);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();

                Execute(entities, t0, t1, t2, t3);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();

                Execute(entities, t0, t1, t2, t3);
            });
        }

		return count;
    }
}

public interface IChunkQueryWRRRR<T0, T1, T2, T3, T4>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
{
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, ReadOnlySpan<T1> t1, ReadOnlySpan<T2> t2, ReadOnlySpan<T3> t3, ReadOnlySpan<T4> t4);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();

                Execute(entities, t0, t1, t2, t3, t4);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();

                Execute(entities, t0, t1, t2, t3, t4);
            });
        }

		return count;
    }
}

public interface IChunkQueryWRRRRR<T0, T1, T2, T3, T4, T5>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
    where T5 : IComponent
{
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, ReadOnlySpan<T1> t1, ReadOnlySpan<T2> t2, ReadOnlySpan<T3> t3, ReadOnlySpan<T4> t4, ReadOnlySpan<T5> t5);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();

                Execute(entities, t0, t1, t2, t3, t4, t5);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();

                Execute(entities, t0, t1, t2, t3, t4, t5);
            });
        }

		return count;
    }
}

public interface IChunkQueryWRRRRRR<T0, T1, T2, T3, T4, T5, T6>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
    where T5 : IComponent
    where T6 : IComponent
{
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, ReadOnlySpan<T1> t1, ReadOnlySpan<T2> t2, ReadOnlySpan<T3> t3, ReadOnlySpan<T4> t4, ReadOnlySpan<T5> t5, ReadOnlySpan<T6> t6);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6);
            });
        }

		return count;
    }
}

public interface IChunkQueryWRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, ReadOnlySpan<T1> t1, ReadOnlySpan<T2> t2, ReadOnlySpan<T3> t3, ReadOnlySpan<T4> t4, ReadOnlySpan<T5> t5, ReadOnlySpan<T6> t6, ReadOnlySpan<T7> t7);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7);
            });
        }

		return count;
    }
}

public interface IChunkQueryWRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, ReadOnlySpan<T1> t1, ReadOnlySpan<T2> t2, ReadOnlySpan<T3> t3, ReadOnlySpan<T4> t4, ReadOnlySpan<T5> t5, ReadOnlySpan<T6> t6, ReadOnlySpan<T7> t7, ReadOnlySpan<T8> t8);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8);
            });
        }

		return count;
    }
}

public interface IChunkQueryWRRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, ReadOnlySpan<T1> t1, ReadOnlySpan<T2> t2, ReadOnlySpan<T3> t3, ReadOnlySpan<T4> t4, ReadOnlySpan<T5> t5, ReadOnlySpan<T6> t6, ReadOnlySpan<T7> t7, ReadOnlySpan<T8> t8, ReadOnlySpan<T9> t9);

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

                count += entities.Length;

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9);
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

                Interlocked.Add(ref count, entities.Length);

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9);
            });
        }

		return count;
    }
}

public interface IChunkQueryWW<T0, T1>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
{
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();

                Execute(entities, t0, t1);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();

                Execute(entities, t0, t1);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWR<T0, T1, T2>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
{
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, ReadOnlySpan<T2> t2);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();

                Execute(entities, t0, t1, t2);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();

                Execute(entities, t0, t1, t2);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWRR<T0, T1, T2, T3>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
{
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, ReadOnlySpan<T2> t2, ReadOnlySpan<T3> t3);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();

                Execute(entities, t0, t1, t2, t3);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();

                Execute(entities, t0, t1, t2, t3);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWRRR<T0, T1, T2, T3, T4>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
{
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, ReadOnlySpan<T2> t2, ReadOnlySpan<T3> t3, ReadOnlySpan<T4> t4);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();

                Execute(entities, t0, t1, t2, t3, t4);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();

                Execute(entities, t0, t1, t2, t3, t4);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWRRRR<T0, T1, T2, T3, T4, T5>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
    where T5 : IComponent
{
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, ReadOnlySpan<T2> t2, ReadOnlySpan<T3> t3, ReadOnlySpan<T4> t4, ReadOnlySpan<T5> t5);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();

                Execute(entities, t0, t1, t2, t3, t4, t5);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();

                Execute(entities, t0, t1, t2, t3, t4, t5);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWRRRRR<T0, T1, T2, T3, T4, T5, T6>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
    where T5 : IComponent
    where T6 : IComponent
{
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, ReadOnlySpan<T2> t2, ReadOnlySpan<T3> t3, ReadOnlySpan<T4> t4, ReadOnlySpan<T5> t5, ReadOnlySpan<T6> t6);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, ReadOnlySpan<T2> t2, ReadOnlySpan<T3> t3, ReadOnlySpan<T4> t4, ReadOnlySpan<T5> t5, ReadOnlySpan<T6> t6, ReadOnlySpan<T7> t7);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, ReadOnlySpan<T2> t2, ReadOnlySpan<T3> t3, ReadOnlySpan<T4> t4, ReadOnlySpan<T5> t5, ReadOnlySpan<T6> t6, ReadOnlySpan<T7> t7, ReadOnlySpan<T8> t8);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, ReadOnlySpan<T2> t2, ReadOnlySpan<T3> t3, ReadOnlySpan<T4> t4, ReadOnlySpan<T5> t5, ReadOnlySpan<T6> t6, ReadOnlySpan<T7> t7, ReadOnlySpan<T8> t8, ReadOnlySpan<T9> t9);

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

                count += entities.Length;

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9);
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

                Interlocked.Add(ref count, entities.Length);

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWRRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, ReadOnlySpan<T2> t2, ReadOnlySpan<T3> t3, ReadOnlySpan<T4> t4, ReadOnlySpan<T5> t5, ReadOnlySpan<T6> t6, ReadOnlySpan<T7> t7, ReadOnlySpan<T8> t8, ReadOnlySpan<T9> t9, ReadOnlySpan<T10> t10);

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

                count += entities.Length;

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
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

                Interlocked.Add(ref count, entities.Length);

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWW<T0, T1, T2>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
{
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();

                Execute(entities, t0, t1, t2);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();

                Execute(entities, t0, t1, t2);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWR<T0, T1, T2, T3>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
{
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, ReadOnlySpan<T3> t3);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();

                Execute(entities, t0, t1, t2, t3);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();

                Execute(entities, t0, t1, t2, t3);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWRR<T0, T1, T2, T3, T4>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
{
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, ReadOnlySpan<T3> t3, ReadOnlySpan<T4> t4);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();

                Execute(entities, t0, t1, t2, t3, t4);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();

                Execute(entities, t0, t1, t2, t3, t4);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWRRR<T0, T1, T2, T3, T4, T5>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
    where T5 : IComponent
{
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, ReadOnlySpan<T3> t3, ReadOnlySpan<T4> t4, ReadOnlySpan<T5> t5);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();

                Execute(entities, t0, t1, t2, t3, t4, t5);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();

                Execute(entities, t0, t1, t2, t3, t4, t5);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWRRRR<T0, T1, T2, T3, T4, T5, T6>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
    where T5 : IComponent
    where T6 : IComponent
{
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, ReadOnlySpan<T3> t3, ReadOnlySpan<T4> t4, ReadOnlySpan<T5> t5, ReadOnlySpan<T6> t6);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWRRRRR<T0, T1, T2, T3, T4, T5, T6, T7>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, ReadOnlySpan<T3> t3, ReadOnlySpan<T4> t4, ReadOnlySpan<T5> t5, ReadOnlySpan<T6> t6, ReadOnlySpan<T7> t7);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, ReadOnlySpan<T3> t3, ReadOnlySpan<T4> t4, ReadOnlySpan<T5> t5, ReadOnlySpan<T6> t6, ReadOnlySpan<T7> t7, ReadOnlySpan<T8> t8);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, ReadOnlySpan<T3> t3, ReadOnlySpan<T4> t4, ReadOnlySpan<T5> t5, ReadOnlySpan<T6> t6, ReadOnlySpan<T7> t7, ReadOnlySpan<T8> t8, ReadOnlySpan<T9> t9);

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

                count += entities.Length;

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9);
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

                Interlocked.Add(ref count, entities.Length);

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, ReadOnlySpan<T3> t3, ReadOnlySpan<T4> t4, ReadOnlySpan<T5> t5, ReadOnlySpan<T6> t6, ReadOnlySpan<T7> t7, ReadOnlySpan<T8> t8, ReadOnlySpan<T9> t9, ReadOnlySpan<T10> t10);

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

                count += entities.Length;

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
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

                Interlocked.Add(ref count, entities.Length);

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWRRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, ReadOnlySpan<T3> t3, ReadOnlySpan<T4> t4, ReadOnlySpan<T5> t5, ReadOnlySpan<T6> t6, ReadOnlySpan<T7> t7, ReadOnlySpan<T8> t8, ReadOnlySpan<T9> t9, ReadOnlySpan<T10> t10, ReadOnlySpan<T11> t11);

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

                count += entities.Length;

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
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

                Interlocked.Add(ref count, entities.Length);

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWW<T0, T1, T2, T3>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
{
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();

                Execute(entities, t0, t1, t2, t3);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();

                Execute(entities, t0, t1, t2, t3);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWR<T0, T1, T2, T3, T4>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
{
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, ReadOnlySpan<T4> t4);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();

                Execute(entities, t0, t1, t2, t3, t4);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();

                Execute(entities, t0, t1, t2, t3, t4);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWRR<T0, T1, T2, T3, T4, T5>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
    where T5 : IComponent
{
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, ReadOnlySpan<T4> t4, ReadOnlySpan<T5> t5);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();

                Execute(entities, t0, t1, t2, t3, t4, t5);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();

                Execute(entities, t0, t1, t2, t3, t4, t5);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWRRR<T0, T1, T2, T3, T4, T5, T6>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
    where T5 : IComponent
    where T6 : IComponent
{
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, ReadOnlySpan<T4> t4, ReadOnlySpan<T5> t5, ReadOnlySpan<T6> t6);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWRRRR<T0, T1, T2, T3, T4, T5, T6, T7>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, ReadOnlySpan<T4> t4, ReadOnlySpan<T5> t5, ReadOnlySpan<T6> t6, ReadOnlySpan<T7> t7);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, ReadOnlySpan<T4> t4, ReadOnlySpan<T5> t5, ReadOnlySpan<T6> t6, ReadOnlySpan<T7> t7, ReadOnlySpan<T8> t8);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, ReadOnlySpan<T4> t4, ReadOnlySpan<T5> t5, ReadOnlySpan<T6> t6, ReadOnlySpan<T7> t7, ReadOnlySpan<T8> t8, ReadOnlySpan<T9> t9);

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

                count += entities.Length;

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9);
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

                Interlocked.Add(ref count, entities.Length);

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, ReadOnlySpan<T4> t4, ReadOnlySpan<T5> t5, ReadOnlySpan<T6> t6, ReadOnlySpan<T7> t7, ReadOnlySpan<T8> t8, ReadOnlySpan<T9> t9, ReadOnlySpan<T10> t10);

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

                count += entities.Length;

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
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

                Interlocked.Add(ref count, entities.Length);

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, ReadOnlySpan<T4> t4, ReadOnlySpan<T5> t5, ReadOnlySpan<T6> t6, ReadOnlySpan<T7> t7, ReadOnlySpan<T8> t8, ReadOnlySpan<T9> t9, ReadOnlySpan<T10> t10, ReadOnlySpan<T11> t11);

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

                count += entities.Length;

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
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

                Interlocked.Add(ref count, entities.Length);

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWRRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, ReadOnlySpan<T4> t4, ReadOnlySpan<T5> t5, ReadOnlySpan<T6> t6, ReadOnlySpan<T7> t7, ReadOnlySpan<T8> t8, ReadOnlySpan<T9> t9, ReadOnlySpan<T10> t10, ReadOnlySpan<T11> t11, ReadOnlySpan<T12> t12);

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

                count += entities.Length;

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
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

                Interlocked.Add(ref count, entities.Length);

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWW<T0, T1, T2, T3, T4>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
{
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();

                Execute(entities, t0, t1, t2, t3, t4);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();

                Execute(entities, t0, t1, t2, t3, t4);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWR<T0, T1, T2, T3, T4, T5>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
    where T5 : IComponent
{
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, ReadOnlySpan<T5> t5);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();

                Execute(entities, t0, t1, t2, t3, t4, t5);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();

                Execute(entities, t0, t1, t2, t3, t4, t5);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWRR<T0, T1, T2, T3, T4, T5, T6>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
    where T5 : IComponent
    where T6 : IComponent
{
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, ReadOnlySpan<T5> t5, ReadOnlySpan<T6> t6);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWRRR<T0, T1, T2, T3, T4, T5, T6, T7>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, ReadOnlySpan<T5> t5, ReadOnlySpan<T6> t6, ReadOnlySpan<T7> t7);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, ReadOnlySpan<T5> t5, ReadOnlySpan<T6> t6, ReadOnlySpan<T7> t7, ReadOnlySpan<T8> t8);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, ReadOnlySpan<T5> t5, ReadOnlySpan<T6> t6, ReadOnlySpan<T7> t7, ReadOnlySpan<T8> t8, ReadOnlySpan<T9> t9);

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

                count += entities.Length;

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9);
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

                Interlocked.Add(ref count, entities.Length);

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, ReadOnlySpan<T5> t5, ReadOnlySpan<T6> t6, ReadOnlySpan<T7> t7, ReadOnlySpan<T8> t8, ReadOnlySpan<T9> t9, ReadOnlySpan<T10> t10);

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

                count += entities.Length;

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
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

                Interlocked.Add(ref count, entities.Length);

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, ReadOnlySpan<T5> t5, ReadOnlySpan<T6> t6, ReadOnlySpan<T7> t7, ReadOnlySpan<T8> t8, ReadOnlySpan<T9> t9, ReadOnlySpan<T10> t10, ReadOnlySpan<T11> t11);

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

                count += entities.Length;

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
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

                Interlocked.Add(ref count, entities.Length);

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, ReadOnlySpan<T5> t5, ReadOnlySpan<T6> t6, ReadOnlySpan<T7> t7, ReadOnlySpan<T8> t8, ReadOnlySpan<T9> t9, ReadOnlySpan<T10> t10, ReadOnlySpan<T11> t11, ReadOnlySpan<T12> t12);

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

                count += entities.Length;

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
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

                Interlocked.Add(ref count, entities.Length);

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWRRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, ReadOnlySpan<T5> t5, ReadOnlySpan<T6> t6, ReadOnlySpan<T7> t7, ReadOnlySpan<T8> t8, ReadOnlySpan<T9> t9, ReadOnlySpan<T10> t10, ReadOnlySpan<T11> t11, ReadOnlySpan<T12> t12, ReadOnlySpan<T13> t13);

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

                count += entities.Length;

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
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

                Interlocked.Add(ref count, entities.Length);

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWW<T0, T1, T2, T3, T4, T5>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
    where T5 : IComponent
{
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();

                Execute(entities, t0, t1, t2, t3, t4, t5);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();

                Execute(entities, t0, t1, t2, t3, t4, t5);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWWR<T0, T1, T2, T3, T4, T5, T6>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
    where T5 : IComponent
    where T6 : IComponent
{
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, ReadOnlySpan<T6> t6);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWWRR<T0, T1, T2, T3, T4, T5, T6, T7>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, ReadOnlySpan<T6> t6, ReadOnlySpan<T7> t7);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWWRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, ReadOnlySpan<T6> t6, ReadOnlySpan<T7> t7, ReadOnlySpan<T8> t8);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWWRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, ReadOnlySpan<T6> t6, ReadOnlySpan<T7> t7, ReadOnlySpan<T8> t8, ReadOnlySpan<T9> t9);

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

                count += entities.Length;

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9);
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

                Interlocked.Add(ref count, entities.Length);

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWWRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, ReadOnlySpan<T6> t6, ReadOnlySpan<T7> t7, ReadOnlySpan<T8> t8, ReadOnlySpan<T9> t9, ReadOnlySpan<T10> t10);

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

                count += entities.Length;

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
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

                Interlocked.Add(ref count, entities.Length);

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWWRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, ReadOnlySpan<T6> t6, ReadOnlySpan<T7> t7, ReadOnlySpan<T8> t8, ReadOnlySpan<T9> t9, ReadOnlySpan<T10> t10, ReadOnlySpan<T11> t11);

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

                count += entities.Length;

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
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

                Interlocked.Add(ref count, entities.Length);

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWWRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, ReadOnlySpan<T6> t6, ReadOnlySpan<T7> t7, ReadOnlySpan<T8> t8, ReadOnlySpan<T9> t9, ReadOnlySpan<T10> t10, ReadOnlySpan<T11> t11, ReadOnlySpan<T12> t12);

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

                count += entities.Length;

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
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

                Interlocked.Add(ref count, entities.Length);

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWWRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, ReadOnlySpan<T6> t6, ReadOnlySpan<T7> t7, ReadOnlySpan<T8> t8, ReadOnlySpan<T9> t9, ReadOnlySpan<T10> t10, ReadOnlySpan<T11> t11, ReadOnlySpan<T12> t12, ReadOnlySpan<T13> t13);

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

                count += entities.Length;

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
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

                Interlocked.Add(ref count, entities.Length);

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWWRRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, ReadOnlySpan<T6> t6, ReadOnlySpan<T7> t7, ReadOnlySpan<T8> t8, ReadOnlySpan<T9> t9, ReadOnlySpan<T10> t10, ReadOnlySpan<T11> t11, ReadOnlySpan<T12> t12, ReadOnlySpan<T13> t13, ReadOnlySpan<T14> t14);

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

                count += entities.Length;

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
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

                Interlocked.Add(ref count, entities.Length);

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWWW<T0, T1, T2, T3, T4, T5, T6>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
    where T5 : IComponent
    where T6 : IComponent
{
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWWWR<T0, T1, T2, T3, T4, T5, T6, T7>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, ReadOnlySpan<T7> t7);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWWWRR<T0, T1, T2, T3, T4, T5, T6, T7, T8>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, ReadOnlySpan<T7> t7, ReadOnlySpan<T8> t8);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWWWRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, ReadOnlySpan<T7> t7, ReadOnlySpan<T8> t8, ReadOnlySpan<T9> t9);

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

                count += entities.Length;

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9);
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

                Interlocked.Add(ref count, entities.Length);

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWWWRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, ReadOnlySpan<T7> t7, ReadOnlySpan<T8> t8, ReadOnlySpan<T9> t9, ReadOnlySpan<T10> t10);

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

                count += entities.Length;

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
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

                Interlocked.Add(ref count, entities.Length);

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWWWRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, ReadOnlySpan<T7> t7, ReadOnlySpan<T8> t8, ReadOnlySpan<T9> t9, ReadOnlySpan<T10> t10, ReadOnlySpan<T11> t11);

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

                count += entities.Length;

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
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

                Interlocked.Add(ref count, entities.Length);

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWWWRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, ReadOnlySpan<T7> t7, ReadOnlySpan<T8> t8, ReadOnlySpan<T9> t9, ReadOnlySpan<T10> t10, ReadOnlySpan<T11> t11, ReadOnlySpan<T12> t12);

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

                count += entities.Length;

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
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

                Interlocked.Add(ref count, entities.Length);

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWWWRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, ReadOnlySpan<T7> t7, ReadOnlySpan<T8> t8, ReadOnlySpan<T9> t9, ReadOnlySpan<T10> t10, ReadOnlySpan<T11> t11, ReadOnlySpan<T12> t12, ReadOnlySpan<T13> t13);

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

                count += entities.Length;

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
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

                Interlocked.Add(ref count, entities.Length);

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWWWRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, ReadOnlySpan<T7> t7, ReadOnlySpan<T8> t8, ReadOnlySpan<T9> t9, ReadOnlySpan<T10> t10, ReadOnlySpan<T11> t11, ReadOnlySpan<T12> t12, ReadOnlySpan<T13> t13, ReadOnlySpan<T14> t14);

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

                count += entities.Length;

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
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

                Interlocked.Add(ref count, entities.Length);

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWWWRRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, ReadOnlySpan<T7> t7, ReadOnlySpan<T8> t8, ReadOnlySpan<T9> t9, ReadOnlySpan<T10> t10, ReadOnlySpan<T11> t11, ReadOnlySpan<T12> t12, ReadOnlySpan<T13> t13, ReadOnlySpan<T14> t14, ReadOnlySpan<T15> t15);

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

                count += entities.Length;

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
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

                Interlocked.Add(ref count, entities.Length);

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWWWW<T0, T1, T2, T3, T4, T5, T6, T7>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWWWWR<T0, T1, T2, T3, T4, T5, T6, T7, T8>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, ReadOnlySpan<T8> t8);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWWWWRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, ReadOnlySpan<T8> t8, ReadOnlySpan<T9> t9);

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

                count += entities.Length;

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9);
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

                Interlocked.Add(ref count, entities.Length);

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWWWWRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, ReadOnlySpan<T8> t8, ReadOnlySpan<T9> t9, ReadOnlySpan<T10> t10);

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

                count += entities.Length;

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
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

                Interlocked.Add(ref count, entities.Length);

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWWWWRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, ReadOnlySpan<T8> t8, ReadOnlySpan<T9> t9, ReadOnlySpan<T10> t10, ReadOnlySpan<T11> t11);

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

                count += entities.Length;

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
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

                Interlocked.Add(ref count, entities.Length);

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWWWWRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, ReadOnlySpan<T8> t8, ReadOnlySpan<T9> t9, ReadOnlySpan<T10> t10, ReadOnlySpan<T11> t11, ReadOnlySpan<T12> t12);

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

                count += entities.Length;

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
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

                Interlocked.Add(ref count, entities.Length);

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWWWWRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, ReadOnlySpan<T8> t8, ReadOnlySpan<T9> t9, ReadOnlySpan<T10> t10, ReadOnlySpan<T11> t11, ReadOnlySpan<T12> t12, ReadOnlySpan<T13> t13);

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

                count += entities.Length;

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
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

                Interlocked.Add(ref count, entities.Length);

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWWWWRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, ReadOnlySpan<T8> t8, ReadOnlySpan<T9> t9, ReadOnlySpan<T10> t10, ReadOnlySpan<T11> t11, ReadOnlySpan<T12> t12, ReadOnlySpan<T13> t13, ReadOnlySpan<T14> t14);

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

                count += entities.Length;

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
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

                Interlocked.Add(ref count, entities.Length);

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWWWWRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, ReadOnlySpan<T8> t8, ReadOnlySpan<T9> t9, ReadOnlySpan<T10> t10, ReadOnlySpan<T11> t11, ReadOnlySpan<T12> t12, ReadOnlySpan<T13> t13, ReadOnlySpan<T14> t14, ReadOnlySpan<T15> t15);

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

                count += entities.Length;

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
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

                Interlocked.Add(ref count, entities.Length);

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWWWWRRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, ReadOnlySpan<T8> t8, ReadOnlySpan<T9> t9, ReadOnlySpan<T10> t10, ReadOnlySpan<T11> t11, ReadOnlySpan<T12> t12, ReadOnlySpan<T13> t13, ReadOnlySpan<T14> t14, ReadOnlySpan<T15> t15, ReadOnlySpan<T16> t16);

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

                count += entities.Length;

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
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

                Interlocked.Add(ref count, entities.Length);

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWWWWW<T0, T1, T2, T3, T4, T5, T6, T7, T8>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8);

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

                count += entities.Length;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8);
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

                Interlocked.Add(ref count, entities.Length);

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWWWWWR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, ReadOnlySpan<T9> t9);

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

                count += entities.Length;

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9);
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

                Interlocked.Add(ref count, entities.Length);

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWWWWWRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, ReadOnlySpan<T9> t9, ReadOnlySpan<T10> t10);

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

                count += entities.Length;

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
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

                Interlocked.Add(ref count, entities.Length);

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWWWWWRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, ReadOnlySpan<T9> t9, ReadOnlySpan<T10> t10, ReadOnlySpan<T11> t11);

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

                count += entities.Length;

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
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

                Interlocked.Add(ref count, entities.Length);

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWWWWWRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, ReadOnlySpan<T9> t9, ReadOnlySpan<T10> t10, ReadOnlySpan<T11> t11, ReadOnlySpan<T12> t12);

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

                count += entities.Length;

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
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

                Interlocked.Add(ref count, entities.Length);

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWWWWWRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, ReadOnlySpan<T9> t9, ReadOnlySpan<T10> t10, ReadOnlySpan<T11> t11, ReadOnlySpan<T12> t12, ReadOnlySpan<T13> t13);

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

                count += entities.Length;

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
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

                Interlocked.Add(ref count, entities.Length);

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWWWWWRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, ReadOnlySpan<T9> t9, ReadOnlySpan<T10> t10, ReadOnlySpan<T11> t11, ReadOnlySpan<T12> t12, ReadOnlySpan<T13> t13, ReadOnlySpan<T14> t14);

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

                count += entities.Length;

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
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

                Interlocked.Add(ref count, entities.Length);

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWWWWWRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, ReadOnlySpan<T9> t9, ReadOnlySpan<T10> t10, ReadOnlySpan<T11> t11, ReadOnlySpan<T12> t12, ReadOnlySpan<T13> t13, ReadOnlySpan<T14> t14, ReadOnlySpan<T15> t15);

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

                count += entities.Length;

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
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

                Interlocked.Add(ref count, entities.Length);

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWWWWWRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, ReadOnlySpan<T9> t9, ReadOnlySpan<T10> t10, ReadOnlySpan<T11> t11, ReadOnlySpan<T12> t12, ReadOnlySpan<T13> t13, ReadOnlySpan<T14> t14, ReadOnlySpan<T15> t15, ReadOnlySpan<T16> t16);

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

                count += entities.Length;

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
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

                Interlocked.Add(ref count, entities.Length);

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
            });
        }

		return count;
    }
}

public interface IChunkQueryWWWWWWWWWRRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>
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
	void Execute(ReadOnlySpan<Entity> e, Span<T0> t0, Span<T1> t1, Span<T2> t2, Span<T3> t3, Span<T4> t4, Span<T5> t5, Span<T6> t6, Span<T7> t7, Span<T8> t8, ReadOnlySpan<T9> t9, ReadOnlySpan<T10> t10, ReadOnlySpan<T11> t11, ReadOnlySpan<T12> t12, ReadOnlySpan<T13> t13, ReadOnlySpan<T14> t14, ReadOnlySpan<T15> t15, ReadOnlySpan<T16> t16, ReadOnlySpan<T17> t17);

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

                count += entities.Length;

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17);
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

                Interlocked.Add(ref count, entities.Length);

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

                Execute(entities, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17);
            });
        }

		return count;
    }
}



