namespace Myriad.ECS.Queries.Filters;

public interface IQueryFilter
{
	void ConfigureQueryBuilder(QueryBuilder q);
}

public interface IQueryFilter<T0>
    : IQueryFilter
{
	bool Filter(Entity e, in T0 t0);
}

public interface IQueryFilter<T0, T1>
    : IQueryFilter
{
	bool Filter(Entity e, in T0 t0, in T1 t1);
}

public interface IQueryFilter<T0, T1, T2>
    : IQueryFilter
{
	bool Filter(Entity e, in T0 t0, in T1 t1, in T2 t2);
}

public interface IQueryFilter<T0, T1, T2, T3>
    : IQueryFilter
{
	bool Filter(Entity e, in T0 t0, in T1 t1, in T2 t2, in T3 t3);
}

public interface IQueryFilter<T0, T1, T2, T3, T4>
    : IQueryFilter
{
	bool Filter(Entity e, in T0 t0, in T1 t1, in T2 t2, in T3 t3, in T4 t4);
}

public interface IQueryFilter<T0, T1, T2, T3, T4, T5>
    : IQueryFilter
{
	bool Filter(Entity e, in T0 t0, in T1 t1, in T2 t2, in T3 t3, in T4 t4, in T5 t5);
}

public interface IQueryFilter<T0, T1, T2, T3, T4, T5, T6>
    : IQueryFilter
{
	bool Filter(Entity e, in T0 t0, in T1 t1, in T2 t2, in T3 t3, in T4 t4, in T5 t5, in T6 t6);
}

public interface IQueryFilter<T0, T1, T2, T3, T4, T5, T6, T7>
    : IQueryFilter
{
	bool Filter(Entity e, in T0 t0, in T1 t1, in T2 t2, in T3 t3, in T4 t4, in T5 t5, in T6 t6, in T7 t7);
}

