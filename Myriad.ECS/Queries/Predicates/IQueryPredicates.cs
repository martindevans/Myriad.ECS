namespace Myriad.ECS.Queries.Predicates;

public interface IQueryPredicate
{
	void ConfigureQueryBuilder(QueryBuilder q);
}

public interface IQueryPredicate<T0>
    : IQueryPredicate
{
	bool Execute(Entity e, ref readonly T0 t0);
}

public interface IQueryPredicate<T0, T1>
    : IQueryPredicate
{
	bool Execute(Entity e, ref readonly T0 t0, ref readonly T1 t1);
}

public interface IQueryPredicate<T0, T1, T2>
    : IQueryPredicate
{
	bool Execute(Entity e, ref readonly T0 t0, ref readonly T1 t1, ref readonly T2 t2);
}

public interface IQueryPredicate<T0, T1, T2, T3>
    : IQueryPredicate
{
	bool Execute(Entity e, ref readonly T0 t0, ref readonly T1 t1, ref readonly T2 t2, ref readonly T3 t3);
}

public interface IQueryPredicate<T0, T1, T2, T3, T4>
    : IQueryPredicate
{
	bool Execute(Entity e, ref readonly T0 t0, ref readonly T1 t1, ref readonly T2 t2, ref readonly T3 t3, ref readonly T4 t4);
}

public interface IQueryPredicate<T0, T1, T2, T3, T4, T5>
    : IQueryPredicate
{
	bool Execute(Entity e, ref readonly T0 t0, ref readonly T1 t1, ref readonly T2 t2, ref readonly T3 t3, ref readonly T4 t4, ref readonly T5 t5);
}

public interface IQueryPredicate<T0, T1, T2, T3, T4, T5, T6>
    : IQueryPredicate
{
	bool Execute(Entity e, ref readonly T0 t0, ref readonly T1 t1, ref readonly T2 t2, ref readonly T3 t3, ref readonly T4 t4, ref readonly T5 t5, ref readonly T6 t6);
}

public interface IQueryPredicate<T0, T1, T2, T3, T4, T5, T6, T7>
    : IQueryPredicate
{
	bool Execute(Entity e, ref readonly T0 t0, ref readonly T1 t1, ref readonly T2 t2, ref readonly T3 t3, ref readonly T4 t4, ref readonly T5 t5, ref readonly T6 t6, ref readonly T7 t7);
}

