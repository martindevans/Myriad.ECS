using Myriad.ECS.Worlds;
using Myriad.ECS.Execution;
using Myriad.ParallelTasks;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedParameter.Global
// ReSharper disable LoopCanBeConvertedToQuery

namespace Myriad.ECS.Queries;

public interface IQuery
{
	public static abstract QueryBuilder QueryBuilder { get; }

	public Future Schedule(QueryDescription query, World world, ExecutionSchedule schedule);
}

public interface IQueryR<T0>
    : IQuery
    where T0 : IComponent
{
	void Execute(Entity e, in T0 t0);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithReadAccess<T0>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], in t0[i]);
                }
            }
        }

        return context.Build();
    }
}

public interface IQueryRR<T0, T1>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
{
	void Execute(Entity e, in T0 t0, in T1 t1);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithReadAccess<T0>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T1>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], in t0[i], in t1[i]);
                }
            }
        }

        return context.Build();
    }
}

public interface IQueryRRR<T0, T1, T2>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
{
	void Execute(Entity e, in T0 t0, in T1 t1, in T2 t2);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithReadAccess<T0>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T1>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T2>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], in t0[i], in t1[i], in t2[i]);
                }
            }
        }

        return context.Build();
    }
}

public interface IQueryRRRR<T0, T1, T2, T3>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
{
	void Execute(Entity e, in T0 t0, in T1 t1, in T2 t2, in T3 t3);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithReadAccess<T0>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T1>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T2>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T3>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], in t0[i], in t1[i], in t2[i], in t3[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, in T0 t0, in T1 t1, in T2 t2, in T3 t3, in T4 t4);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithReadAccess<T0>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T1>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T2>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T3>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T4>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], in t0[i], in t1[i], in t2[i], in t3[i], in t4[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, in T0 t0, in T1 t1, in T2 t2, in T3 t3, in T4 t4, in T5 t5);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithReadAccess<T0>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T1>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T2>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T3>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T4>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T5>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], in t0[i], in t1[i], in t2[i], in t3[i], in t4[i], in t5[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, in T0 t0, in T1 t1, in T2 t2, in T3 t3, in T4 t4, in T5 t5, in T6 t6);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithReadAccess<T0>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T1>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T2>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T3>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T4>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T5>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T6>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], in t0[i], in t1[i], in t2[i], in t3[i], in t4[i], in t5[i], in t6[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, in T0 t0, in T1 t1, in T2 t2, in T3 t3, in T4 t4, in T5 t5, in T6 t6, in T7 t7);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithReadAccess<T0>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T1>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T2>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T3>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T4>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T5>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T6>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T7>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], in t0[i], in t1[i], in t2[i], in t3[i], in t4[i], in t5[i], in t6[i], in t7[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, in T0 t0, in T1 t1, in T2 t2, in T3 t3, in T4 t4, in T5 t5, in T6 t6, in T7 t7, in T8 t8);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithReadAccess<T0>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T1>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T2>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T3>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T4>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T5>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T6>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T7>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T8>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], in t0[i], in t1[i], in t2[i], in t3[i], in t4[i], in t5[i], in t6[i], in t7[i], in t8[i]);
                }
            }
        }

        return context.Build();
    }
}

public interface IQueryW<T0>
    : IQuery
    where T0 : IComponent
{
	void Execute(Entity e, ref T0 t0);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i]);
                }
            }
        }

        return context.Build();
    }
}

public interface IQueryWR<T0, T1>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
{
	void Execute(Entity e, ref T0 t0, in T1 t1);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T1>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], in t1[i]);
                }
            }
        }

        return context.Build();
    }
}

public interface IQueryWRR<T0, T1, T2>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
{
	void Execute(Entity e, ref T0 t0, in T1 t1, in T2 t2);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T1>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T2>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], in t1[i], in t2[i]);
                }
            }
        }

        return context.Build();
    }
}

public interface IQueryWRRR<T0, T1, T2, T3>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
{
	void Execute(Entity e, ref T0 t0, in T1 t1, in T2 t2, in T3 t3);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T1>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T2>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T3>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], in t1[i], in t2[i], in t3[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, in T1 t1, in T2 t2, in T3 t3, in T4 t4);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T1>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T2>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T3>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T4>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], in t1[i], in t2[i], in t3[i], in t4[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, in T1 t1, in T2 t2, in T3 t3, in T4 t4, in T5 t5);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T1>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T2>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T3>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T4>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T5>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], in t1[i], in t2[i], in t3[i], in t4[i], in t5[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, in T1 t1, in T2 t2, in T3 t3, in T4 t4, in T5 t5, in T6 t6);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T1>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T2>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T3>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T4>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T5>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T6>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], in t1[i], in t2[i], in t3[i], in t4[i], in t5[i], in t6[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, in T1 t1, in T2 t2, in T3 t3, in T4 t4, in T5 t5, in T6 t6, in T7 t7);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T1>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T2>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T3>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T4>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T5>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T6>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T7>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], in t1[i], in t2[i], in t3[i], in t4[i], in t5[i], in t6[i], in t7[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, in T1 t1, in T2 t2, in T3 t3, in T4 t4, in T5 t5, in T6 t6, in T7 t7, in T8 t8);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T1>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T2>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T3>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T4>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T5>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T6>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T7>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T8>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], in t1[i], in t2[i], in t3[i], in t4[i], in t5[i], in t6[i], in t7[i], in t8[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, in T1 t1, in T2 t2, in T3 t3, in T4 t4, in T5 t5, in T6 t6, in T7 t7, in T8 t8, in T9 t9);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T1>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T2>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T3>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T4>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T5>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T6>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T7>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T8>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T9>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

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

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], in t1[i], in t2[i], in t3[i], in t4[i], in t5[i], in t6[i], in t7[i], in t8[i], in t9[i]);
                }
            }
        }

        return context.Build();
    }
}

public interface IQueryWW<T0, T1>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
{
	void Execute(Entity e, ref T0 t0, ref T1 t1);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i]);
                }
            }
        }

        return context.Build();
    }
}

public interface IQueryWWR<T0, T1, T2>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, in T2 t2);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T2>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], in t2[i]);
                }
            }
        }

        return context.Build();
    }
}

public interface IQueryWWRR<T0, T1, T2, T3>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, in T2 t2, in T3 t3);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T2>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T3>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], in t2[i], in t3[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, in T2 t2, in T3 t3, in T4 t4);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T2>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T3>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T4>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], in t2[i], in t3[i], in t4[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, in T2 t2, in T3 t3, in T4 t4, in T5 t5);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T2>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T3>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T4>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T5>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], in t2[i], in t3[i], in t4[i], in t5[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, in T2 t2, in T3 t3, in T4 t4, in T5 t5, in T6 t6);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T2>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T3>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T4>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T5>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T6>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], in t2[i], in t3[i], in t4[i], in t5[i], in t6[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, in T2 t2, in T3 t3, in T4 t4, in T5 t5, in T6 t6, in T7 t7);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T2>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T3>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T4>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T5>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T6>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T7>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], in t2[i], in t3[i], in t4[i], in t5[i], in t6[i], in t7[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, in T2 t2, in T3 t3, in T4 t4, in T5 t5, in T6 t6, in T7 t7, in T8 t8);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T2>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T3>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T4>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T5>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T6>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T7>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T8>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], in t2[i], in t3[i], in t4[i], in t5[i], in t6[i], in t7[i], in t8[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, in T2 t2, in T3 t3, in T4 t4, in T5 t5, in T6 t6, in T7 t7, in T8 t8, in T9 t9);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T2>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T3>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T4>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T5>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T6>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T7>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T8>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T9>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

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

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], in t2[i], in t3[i], in t4[i], in t5[i], in t6[i], in t7[i], in t8[i], in t9[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, in T2 t2, in T3 t3, in T4 t4, in T5 t5, in T6 t6, in T7 t7, in T8 t8, in T9 t9, in T10 t10);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T2>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T3>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T4>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T5>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T6>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T7>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T8>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T9>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T10>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

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

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], in t2[i], in t3[i], in t4[i], in t5[i], in t6[i], in t7[i], in t8[i], in t9[i], in t10[i]);
                }
            }
        }

        return context.Build();
    }
}

public interface IQueryWWW<T0, T1, T2>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i]);
                }
            }
        }

        return context.Build();
    }
}

public interface IQueryWWWR<T0, T1, T2, T3>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, in T3 t3);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T3>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], in t3[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, in T3 t3, in T4 t4);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T3>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T4>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], in t3[i], in t4[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, in T3 t3, in T4 t4, in T5 t5);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T3>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T4>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T5>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], in t3[i], in t4[i], in t5[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, in T3 t3, in T4 t4, in T5 t5, in T6 t6);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T3>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T4>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T5>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T6>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], in t3[i], in t4[i], in t5[i], in t6[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, in T3 t3, in T4 t4, in T5 t5, in T6 t6, in T7 t7);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T3>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T4>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T5>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T6>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T7>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], in t3[i], in t4[i], in t5[i], in t6[i], in t7[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, in T3 t3, in T4 t4, in T5 t5, in T6 t6, in T7 t7, in T8 t8);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T3>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T4>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T5>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T6>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T7>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T8>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], in t3[i], in t4[i], in t5[i], in t6[i], in t7[i], in t8[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, in T3 t3, in T4 t4, in T5 t5, in T6 t6, in T7 t7, in T8 t8, in T9 t9);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T3>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T4>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T5>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T6>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T7>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T8>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T9>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

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

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], in t3[i], in t4[i], in t5[i], in t6[i], in t7[i], in t8[i], in t9[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, in T3 t3, in T4 t4, in T5 t5, in T6 t6, in T7 t7, in T8 t8, in T9 t9, in T10 t10);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T3>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T4>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T5>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T6>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T7>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T8>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T9>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T10>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

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

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], in t3[i], in t4[i], in t5[i], in t6[i], in t7[i], in t8[i], in t9[i], in t10[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, in T3 t3, in T4 t4, in T5 t5, in T6 t6, in T7 t7, in T8 t8, in T9 t9, in T10 t10, in T11 t11);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T3>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T4>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T5>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T6>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T7>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T8>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T9>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T10>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T11>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

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

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], in t3[i], in t4[i], in t5[i], in t6[i], in t7[i], in t8[i], in t9[i], in t10[i], in t11[i]);
                }
            }
        }

        return context.Build();
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

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, in T4 t4);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T4>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], in t4[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, in T4 t4, in T5 t5);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T4>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T5>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], in t4[i], in t5[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, in T4 t4, in T5 t5, in T6 t6);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T4>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T5>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T6>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], in t4[i], in t5[i], in t6[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, in T4 t4, in T5 t5, in T6 t6, in T7 t7);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T4>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T5>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T6>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T7>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], in t4[i], in t5[i], in t6[i], in t7[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, in T4 t4, in T5 t5, in T6 t6, in T7 t7, in T8 t8);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T4>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T5>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T6>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T7>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T8>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], in t4[i], in t5[i], in t6[i], in t7[i], in t8[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, in T4 t4, in T5 t5, in T6 t6, in T7 t7, in T8 t8, in T9 t9);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T4>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T5>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T6>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T7>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T8>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T9>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

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

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], in t4[i], in t5[i], in t6[i], in t7[i], in t8[i], in t9[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, in T4 t4, in T5 t5, in T6 t6, in T7 t7, in T8 t8, in T9 t9, in T10 t10);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T4>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T5>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T6>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T7>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T8>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T9>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T10>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

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

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], in t4[i], in t5[i], in t6[i], in t7[i], in t8[i], in t9[i], in t10[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, in T4 t4, in T5 t5, in T6 t6, in T7 t7, in T8 t8, in T9 t9, in T10 t10, in T11 t11);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T4>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T5>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T6>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T7>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T8>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T9>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T10>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T11>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

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

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], in t4[i], in t5[i], in t6[i], in t7[i], in t8[i], in t9[i], in t10[i], in t11[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, in T4 t4, in T5 t5, in T6 t6, in T7 t7, in T8 t8, in T9 t9, in T10 t10, in T11 t11, in T12 t12);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T4>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T5>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T6>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T7>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T8>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T9>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T10>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T11>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T12>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

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

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], in t4[i], in t5[i], in t6[i], in t7[i], in t8[i], in t9[i], in t10[i], in t11[i], in t12[i]);
                }
            }
        }

        return context.Build();
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

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, in T5 t5);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T5>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], in t5[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, in T5 t5, in T6 t6);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T5>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T6>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], in t5[i], in t6[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, in T5 t5, in T6 t6, in T7 t7);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T5>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T6>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T7>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], in t5[i], in t6[i], in t7[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, in T5 t5, in T6 t6, in T7 t7, in T8 t8);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T5>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T6>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T7>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T8>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], in t5[i], in t6[i], in t7[i], in t8[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, in T5 t5, in T6 t6, in T7 t7, in T8 t8, in T9 t9);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T5>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T6>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T7>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T8>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T9>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

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

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], in t5[i], in t6[i], in t7[i], in t8[i], in t9[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, in T5 t5, in T6 t6, in T7 t7, in T8 t8, in T9 t9, in T10 t10);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T5>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T6>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T7>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T8>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T9>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T10>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

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

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], in t5[i], in t6[i], in t7[i], in t8[i], in t9[i], in t10[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, in T5 t5, in T6 t6, in T7 t7, in T8 t8, in T9 t9, in T10 t10, in T11 t11);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T5>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T6>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T7>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T8>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T9>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T10>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T11>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

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

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], in t5[i], in t6[i], in t7[i], in t8[i], in t9[i], in t10[i], in t11[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, in T5 t5, in T6 t6, in T7 t7, in T8 t8, in T9 t9, in T10 t10, in T11 t11, in T12 t12);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T5>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T6>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T7>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T8>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T9>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T10>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T11>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T12>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

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

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], in t5[i], in t6[i], in t7[i], in t8[i], in t9[i], in t10[i], in t11[i], in t12[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, in T5 t5, in T6 t6, in T7 t7, in T8 t8, in T9 t9, in T10 t10, in T11 t11, in T12 t12, in T13 t13);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T5>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T6>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T7>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T8>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T9>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T10>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T11>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T12>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T13>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

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

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], in t5[i], in t6[i], in t7[i], in t8[i], in t9[i], in t10[i], in t11[i], in t12[i], in t13[i]);
                }
            }
        }

        return context.Build();
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

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T5>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, in T6 t6);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T5>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T6>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], in t6[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, in T6 t6, in T7 t7);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T5>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T6>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T7>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], in t6[i], in t7[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, in T6 t6, in T7 t7, in T8 t8);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T5>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T6>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T7>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T8>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], in t6[i], in t7[i], in t8[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, in T6 t6, in T7 t7, in T8 t8, in T9 t9);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T5>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T6>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T7>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T8>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T9>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

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

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], in t6[i], in t7[i], in t8[i], in t9[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, in T6 t6, in T7 t7, in T8 t8, in T9 t9, in T10 t10);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T5>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T6>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T7>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T8>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T9>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T10>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

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

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], in t6[i], in t7[i], in t8[i], in t9[i], in t10[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, in T6 t6, in T7 t7, in T8 t8, in T9 t9, in T10 t10, in T11 t11);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T5>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T6>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T7>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T8>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T9>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T10>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T11>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

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

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], in t6[i], in t7[i], in t8[i], in t9[i], in t10[i], in t11[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, in T6 t6, in T7 t7, in T8 t8, in T9 t9, in T10 t10, in T11 t11, in T12 t12);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T5>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T6>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T7>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T8>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T9>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T10>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T11>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T12>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

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

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], in t6[i], in t7[i], in t8[i], in t9[i], in t10[i], in t11[i], in t12[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, in T6 t6, in T7 t7, in T8 t8, in T9 t9, in T10 t10, in T11 t11, in T12 t12, in T13 t13);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T5>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T6>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T7>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T8>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T9>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T10>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T11>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T12>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T13>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

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

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], in t6[i], in t7[i], in t8[i], in t9[i], in t10[i], in t11[i], in t12[i], in t13[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, in T6 t6, in T7 t7, in T8 t8, in T9 t9, in T10 t10, in T11 t11, in T12 t12, in T13 t13, in T14 t14);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T5>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T6>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T7>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T8>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T9>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T10>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T11>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T12>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T13>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T14>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

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

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], in t6[i], in t7[i], in t8[i], in t9[i], in t10[i], in t11[i], in t12[i], in t13[i], in t14[i]);
                }
            }
        }

        return context.Build();
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

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T5>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T6>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, in T7 t7);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T5>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T6>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T7>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], in t7[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, in T7 t7, in T8 t8);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T5>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T6>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T7>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T8>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], in t7[i], in t8[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, in T7 t7, in T8 t8, in T9 t9);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T5>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T6>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T7>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T8>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T9>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

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

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], in t7[i], in t8[i], in t9[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, in T7 t7, in T8 t8, in T9 t9, in T10 t10);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T5>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T6>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T7>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T8>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T9>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T10>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

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

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], in t7[i], in t8[i], in t9[i], in t10[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, in T7 t7, in T8 t8, in T9 t9, in T10 t10, in T11 t11);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T5>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T6>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T7>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T8>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T9>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T10>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T11>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

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

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], in t7[i], in t8[i], in t9[i], in t10[i], in t11[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, in T7 t7, in T8 t8, in T9 t9, in T10 t10, in T11 t11, in T12 t12);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T5>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T6>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T7>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T8>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T9>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T10>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T11>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T12>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

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

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], in t7[i], in t8[i], in t9[i], in t10[i], in t11[i], in t12[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, in T7 t7, in T8 t8, in T9 t9, in T10 t10, in T11 t11, in T12 t12, in T13 t13);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T5>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T6>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T7>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T8>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T9>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T10>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T11>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T12>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T13>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

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

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], in t7[i], in t8[i], in t9[i], in t10[i], in t11[i], in t12[i], in t13[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, in T7 t7, in T8 t8, in T9 t9, in T10 t10, in T11 t11, in T12 t12, in T13 t13, in T14 t14);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T5>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T6>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T7>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T8>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T9>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T10>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T11>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T12>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T13>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T14>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

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

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], in t7[i], in t8[i], in t9[i], in t10[i], in t11[i], in t12[i], in t13[i], in t14[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, in T7 t7, in T8 t8, in T9 t9, in T10 t10, in T11 t11, in T12 t12, in T13 t13, in T14 t14, in T15 t15);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T5>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T6>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T7>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T8>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T9>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T10>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T11>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T12>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T13>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T14>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T15>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

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

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], in t7[i], in t8[i], in t9[i], in t10[i], in t11[i], in t12[i], in t13[i], in t14[i], in t15[i]);
                }
            }
        }

        return context.Build();
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

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T5>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T6>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T7>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, in T8 t8);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T5>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T6>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T7>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T8>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], in t8[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, in T8 t8, in T9 t9);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T5>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T6>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T7>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T8>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T9>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

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

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], in t8[i], in t9[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, in T8 t8, in T9 t9, in T10 t10);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T5>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T6>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T7>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T8>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T9>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T10>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

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

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], in t8[i], in t9[i], in t10[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, in T8 t8, in T9 t9, in T10 t10, in T11 t11);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T5>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T6>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T7>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T8>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T9>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T10>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T11>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

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

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], in t8[i], in t9[i], in t10[i], in t11[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, in T8 t8, in T9 t9, in T10 t10, in T11 t11, in T12 t12);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T5>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T6>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T7>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T8>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T9>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T10>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T11>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T12>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

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

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], in t8[i], in t9[i], in t10[i], in t11[i], in t12[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, in T8 t8, in T9 t9, in T10 t10, in T11 t11, in T12 t12, in T13 t13);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T5>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T6>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T7>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T8>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T9>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T10>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T11>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T12>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T13>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

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

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], in t8[i], in t9[i], in t10[i], in t11[i], in t12[i], in t13[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, in T8 t8, in T9 t9, in T10 t10, in T11 t11, in T12 t12, in T13 t13, in T14 t14);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T5>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T6>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T7>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T8>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T9>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T10>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T11>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T12>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T13>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T14>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

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

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], in t8[i], in t9[i], in t10[i], in t11[i], in t12[i], in t13[i], in t14[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, in T8 t8, in T9 t9, in T10 t10, in T11 t11, in T12 t12, in T13 t13, in T14 t14, in T15 t15);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T5>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T6>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T7>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T8>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T9>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T10>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T11>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T12>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T13>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T14>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T15>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

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

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], in t8[i], in t9[i], in t10[i], in t11[i], in t12[i], in t13[i], in t14[i], in t15[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, in T8 t8, in T9 t9, in T10 t10, in T11 t11, in T12 t12, in T13 t13, in T14 t14, in T15 t15, in T16 t16);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T5>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T6>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T7>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T8>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T9>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T10>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T11>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T12>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T13>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T14>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T15>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T16>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

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

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], in t8[i], in t9[i], in t10[i], in t11[i], in t12[i], in t13[i], in t14[i], in t15[i], in t16[i]);
                }
            }
        }

        return context.Build();
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

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T5>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T6>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T7>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T8>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

                var t0 = chunk.GetSpan<T0>();
                var t1 = chunk.GetSpan<T1>();
                var t2 = chunk.GetSpan<T2>();
                var t3 = chunk.GetSpan<T3>();
                var t4 = chunk.GetSpan<T4>();
                var t5 = chunk.GetSpan<T5>();
                var t6 = chunk.GetSpan<T6>();
                var t7 = chunk.GetSpan<T7>();
                var t8 = chunk.GetSpan<T8>();

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, in T9 t9);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T5>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T6>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T7>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T8>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T9>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

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

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], in t9[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, in T9 t9, in T10 t10);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T5>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T6>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T7>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T8>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T9>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T10>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

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

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], in t9[i], in t10[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, in T9 t9, in T10 t10, in T11 t11);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T5>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T6>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T7>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T8>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T9>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T10>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T11>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

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

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], in t9[i], in t10[i], in t11[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, in T9 t9, in T10 t10, in T11 t11, in T12 t12);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T5>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T6>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T7>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T8>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T9>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T10>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T11>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T12>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

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

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], in t9[i], in t10[i], in t11[i], in t12[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, in T9 t9, in T10 t10, in T11 t11, in T12 t12, in T13 t13);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T5>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T6>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T7>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T8>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T9>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T10>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T11>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T12>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T13>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

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

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], in t9[i], in t10[i], in t11[i], in t12[i], in t13[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, in T9 t9, in T10 t10, in T11 t11, in T12 t12, in T13 t13, in T14 t14);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T5>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T6>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T7>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T8>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T9>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T10>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T11>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T12>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T13>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T14>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

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

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], in t9[i], in t10[i], in t11[i], in t12[i], in t13[i], in t14[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, in T9 t9, in T10 t10, in T11 t11, in T12 t12, in T13 t13, in T14 t14, in T15 t15);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T5>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T6>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T7>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T8>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T9>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T10>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T11>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T12>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T13>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T14>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T15>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

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

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], in t9[i], in t10[i], in t11[i], in t12[i], in t13[i], in t14[i], in t15[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, in T9 t9, in T10 t10, in T11 t11, in T12 t12, in T13 t13, in T14 t14, in T15 t15, in T16 t16);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T5>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T6>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T7>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T8>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T9>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T10>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T11>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T12>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T13>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T14>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T15>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T16>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

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

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], in t9[i], in t10[i], in t11[i], in t12[i], in t13[i], in t14[i], in t15[i], in t16[i]);
                }
            }
        }

        return context.Build();
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
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, ref T8 t8, in T9 t9, in T10 t10, in T11 t11, in T12 t12, in T13 t13, in T14 t14, in T15 t15, in T16 t16, in T17 t17);

    Future IQuery.Schedule(QueryDescription query, World world, ExecutionSchedule schedule)
    {
	    // Build an execution schedule for this work, declaring what resources it reads and writes
        var context = schedule.CreateJob();

		var archetypes = query.GetArchetypes();
		for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

            context = context.WithWriteAccess<T0>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T1>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T2>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T3>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T4>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T5>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T6>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T7>(archetypeMatch.Archetype);
            context = context.WithWriteAccess<T8>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T9>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T10>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T11>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T12>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T13>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T14>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T15>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T16>(archetypeMatch.Archetype);
            context = context.WithReadAccess<T17>(archetypeMatch.Archetype);
        }

        for (var a = 0; a < archetypes.Count; a++)
        {
			var archetypeMatch = archetypes[a];

            if (archetypeMatch.Archetype.EntityCount == 0)
                continue;

			var chunks = archetypeMatch.Archetype.Chunks;
            for (var c = chunks.Count - 1; c >= 0; c--)
            {
				var chunk = chunks[c];
                var entities = chunk.Entities;

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

				for (var i = entities.Length - 1; i >= 0; i--)
                {
                    Execute(entities[i], ref t0[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i], in t9[i], in t10[i], in t11[i], in t12[i], in t13[i], in t14[i], in t15[i], in t16[i], in t17[i]);
                }
            }
        }

        return context.Build();
    }
}



