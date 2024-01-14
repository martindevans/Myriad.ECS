using Myriad.ECS.Worlds;
using Myriad.ParallelTasks;
using System.Runtime.CompilerServices;

namespace Myriad.ECS.Queries;

public interface IQuery
{
    public static abstract QueryBuilder QueryBuilder { get; }

	public Future Execute(QueryDescription query, World world);
}

public interface IQuery1<T0>
    : IQuery
    where T0 : IComponent
{
    void Execute(Entity e, ref T0 t0);

	Future IQuery.Execute(QueryDescription query, World world)
    {
        foreach (var archetypeMatch in query.GetArchetypes())
        {
            foreach (var chunk in archetypeMatch.Archetype)
            {
                var entities = chunk.Entities;

                var t0span = chunk.GetMutable<T0>();
                ref var t0 = ref t0span[0];

                for (var i = 0; i < entities.Length; i++)
                {
                    Execute(entities[i], ref t0);

                    Unsafe.Add(ref t0, 1);
                }
            }
        }

        return new Future();
    }
}

public interface IQuery2<T0, T1>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
{
    void Execute(Entity e, ref T0 t0, ref T1 t1);

	Future IQuery.Execute(QueryDescription query, World world)
    {
        foreach (var archetypeMatch in query.GetArchetypes())
        {
            foreach (var chunk in archetypeMatch.Archetype)
            {
                var entities = chunk.Entities;

                var t0span = chunk.GetMutable<T0>();
                ref var t0 = ref t0span[0];

                var t1span = chunk.GetMutable<T1>();
                ref var t1 = ref t1span[0];

                for (var i = 0; i < entities.Length; i++)
                {
                    Execute(entities[i], ref t0, ref t1);

                    Unsafe.Add(ref t0, 1);
                    Unsafe.Add(ref t1, 1);
                }
            }
        }

        return new Future();
    }
}

public interface IQuery3<T0, T1, T2>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
{
    void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2);

	Future IQuery.Execute(QueryDescription query, World world)
    {
        foreach (var archetypeMatch in query.GetArchetypes())
        {
            foreach (var chunk in archetypeMatch.Archetype)
            {
                var entities = chunk.Entities;

                var t0span = chunk.GetMutable<T0>();
                ref var t0 = ref t0span[0];

                var t1span = chunk.GetMutable<T1>();
                ref var t1 = ref t1span[0];

                var t2span = chunk.GetMutable<T2>();
                ref var t2 = ref t2span[0];

                for (var i = 0; i < entities.Length; i++)
                {
                    Execute(entities[i], ref t0, ref t1, ref t2);

                    Unsafe.Add(ref t0, 1);
                    Unsafe.Add(ref t1, 1);
                    Unsafe.Add(ref t2, 1);
                }
            }
        }

        return new Future();
    }
}

public interface IQuery4<T0, T1, T2, T3>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
{
    void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3);

	Future IQuery.Execute(QueryDescription query, World world)
    {
        foreach (var archetypeMatch in query.GetArchetypes())
        {
            foreach (var chunk in archetypeMatch.Archetype)
            {
                var entities = chunk.Entities;

                var t0span = chunk.GetMutable<T0>();
                ref var t0 = ref t0span[0];

                var t1span = chunk.GetMutable<T1>();
                ref var t1 = ref t1span[0];

                var t2span = chunk.GetMutable<T2>();
                ref var t2 = ref t2span[0];

                var t3span = chunk.GetMutable<T3>();
                ref var t3 = ref t3span[0];

                for (var i = 0; i < entities.Length; i++)
                {
                    Execute(entities[i], ref t0, ref t1, ref t2, ref t3);

                    Unsafe.Add(ref t0, 1);
                    Unsafe.Add(ref t1, 1);
                    Unsafe.Add(ref t2, 1);
                    Unsafe.Add(ref t3, 1);
                }
            }
        }

        return new Future();
    }
}

public interface IQuery5<T0, T1, T2, T3, T4>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
{
    void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4);

	Future IQuery.Execute(QueryDescription query, World world)
    {
        foreach (var archetypeMatch in query.GetArchetypes())
        {
            foreach (var chunk in archetypeMatch.Archetype)
            {
                var entities = chunk.Entities;

                var t0span = chunk.GetMutable<T0>();
                ref var t0 = ref t0span[0];

                var t1span = chunk.GetMutable<T1>();
                ref var t1 = ref t1span[0];

                var t2span = chunk.GetMutable<T2>();
                ref var t2 = ref t2span[0];

                var t3span = chunk.GetMutable<T3>();
                ref var t3 = ref t3span[0];

                var t4span = chunk.GetMutable<T4>();
                ref var t4 = ref t4span[0];

                for (var i = 0; i < entities.Length; i++)
                {
                    Execute(entities[i], ref t0, ref t1, ref t2, ref t3, ref t4);

                    Unsafe.Add(ref t0, 1);
                    Unsafe.Add(ref t1, 1);
                    Unsafe.Add(ref t2, 1);
                    Unsafe.Add(ref t3, 1);
                    Unsafe.Add(ref t4, 1);
                }
            }
        }

        return new Future();
    }
}

public interface IQuery6<T0, T1, T2, T3, T4, T5>
    : IQuery
    where T0 : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
    where T5 : IComponent
{
    void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5);

	Future IQuery.Execute(QueryDescription query, World world)
    {
        foreach (var archetypeMatch in query.GetArchetypes())
        {
            foreach (var chunk in archetypeMatch.Archetype)
            {
                var entities = chunk.Entities;

                var t0span = chunk.GetMutable<T0>();
                ref var t0 = ref t0span[0];

                var t1span = chunk.GetMutable<T1>();
                ref var t1 = ref t1span[0];

                var t2span = chunk.GetMutable<T2>();
                ref var t2 = ref t2span[0];

                var t3span = chunk.GetMutable<T3>();
                ref var t3 = ref t3span[0];

                var t4span = chunk.GetMutable<T4>();
                ref var t4 = ref t4span[0];

                var t5span = chunk.GetMutable<T5>();
                ref var t5 = ref t5span[0];

                for (var i = 0; i < entities.Length; i++)
                {
                    Execute(entities[i], ref t0, ref t1, ref t2, ref t3, ref t4, ref t5);

                    Unsafe.Add(ref t0, 1);
                    Unsafe.Add(ref t1, 1);
                    Unsafe.Add(ref t2, 1);
                    Unsafe.Add(ref t3, 1);
                    Unsafe.Add(ref t4, 1);
                    Unsafe.Add(ref t5, 1);
                }
            }
        }

        return new Future();
    }
}

public interface IQuery7<T0, T1, T2, T3, T4, T5, T6>
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

	Future IQuery.Execute(QueryDescription query, World world)
    {
        foreach (var archetypeMatch in query.GetArchetypes())
        {
            foreach (var chunk in archetypeMatch.Archetype)
            {
                var entities = chunk.Entities;

                var t0span = chunk.GetMutable<T0>();
                ref var t0 = ref t0span[0];

                var t1span = chunk.GetMutable<T1>();
                ref var t1 = ref t1span[0];

                var t2span = chunk.GetMutable<T2>();
                ref var t2 = ref t2span[0];

                var t3span = chunk.GetMutable<T3>();
                ref var t3 = ref t3span[0];

                var t4span = chunk.GetMutable<T4>();
                ref var t4 = ref t4span[0];

                var t5span = chunk.GetMutable<T5>();
                ref var t5 = ref t5span[0];

                var t6span = chunk.GetMutable<T6>();
                ref var t6 = ref t6span[0];

                for (var i = 0; i < entities.Length; i++)
                {
                    Execute(entities[i], ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6);

                    Unsafe.Add(ref t0, 1);
                    Unsafe.Add(ref t1, 1);
                    Unsafe.Add(ref t2, 1);
                    Unsafe.Add(ref t3, 1);
                    Unsafe.Add(ref t4, 1);
                    Unsafe.Add(ref t5, 1);
                    Unsafe.Add(ref t6, 1);
                }
            }
        }

        return new Future();
    }
}

public interface IQuery8<T0, T1, T2, T3, T4, T5, T6, T7>
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

	Future IQuery.Execute(QueryDescription query, World world)
    {
        foreach (var archetypeMatch in query.GetArchetypes())
        {
            foreach (var chunk in archetypeMatch.Archetype)
            {
                var entities = chunk.Entities;

                var t0span = chunk.GetMutable<T0>();
                ref var t0 = ref t0span[0];

                var t1span = chunk.GetMutable<T1>();
                ref var t1 = ref t1span[0];

                var t2span = chunk.GetMutable<T2>();
                ref var t2 = ref t2span[0];

                var t3span = chunk.GetMutable<T3>();
                ref var t3 = ref t3span[0];

                var t4span = chunk.GetMutable<T4>();
                ref var t4 = ref t4span[0];

                var t5span = chunk.GetMutable<T5>();
                ref var t5 = ref t5span[0];

                var t6span = chunk.GetMutable<T6>();
                ref var t6 = ref t6span[0];

                var t7span = chunk.GetMutable<T7>();
                ref var t7 = ref t7span[0];

                for (var i = 0; i < entities.Length; i++)
                {
                    Execute(entities[i], ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7);

                    Unsafe.Add(ref t0, 1);
                    Unsafe.Add(ref t1, 1);
                    Unsafe.Add(ref t2, 1);
                    Unsafe.Add(ref t3, 1);
                    Unsafe.Add(ref t4, 1);
                    Unsafe.Add(ref t5, 1);
                    Unsafe.Add(ref t6, 1);
                    Unsafe.Add(ref t7, 1);
                }
            }
        }

        return new Future();
    }
}



