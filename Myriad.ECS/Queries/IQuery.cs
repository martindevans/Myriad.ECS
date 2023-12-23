namespace Myriad.ECS.Queries;

public interface IQuery;

public interface IQueryR<T0>
    : IQuery
{
	void Execute(Entity e, in T0 t0);
}

public interface IQueryRR<T0, T1>
    : IQuery
{
	void Execute(Entity e, in T0 t0, in T1 t1);
}

public interface IQueryRRR<T0, T1, T2>
    : IQuery
{
	void Execute(Entity e, in T0 t0, in T1 t1, in T2 t2);
}

public interface IQueryRRRR<T0, T1, T2, T3>
    : IQuery
{
	void Execute(Entity e, in T0 t0, in T1 t1, in T2 t2, in T3 t3);
}

public interface IQueryRRRRR<T0, T1, T2, T3, T4>
    : IQuery
{
	void Execute(Entity e, in T0 t0, in T1 t1, in T2 t2, in T3 t3, in T4 t4);
}

public interface IQueryRRRRRR<T0, T1, T2, T3, T4, T5>
    : IQuery
{
	void Execute(Entity e, in T0 t0, in T1 t1, in T2 t2, in T3 t3, in T4 t4, in T5 t5);
}

public interface IQueryRRRRRRR<T0, T1, T2, T3, T4, T5, T6>
    : IQuery
{
	void Execute(Entity e, in T0 t0, in T1 t1, in T2 t2, in T3 t3, in T4 t4, in T5 t5, in T6 t6);
}

public interface IQueryRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7>
    : IQuery
{
	void Execute(Entity e, in T0 t0, in T1 t1, in T2 t2, in T3 t3, in T4 t4, in T5 t5, in T6 t6, in T7 t7);
}

public interface IQueryW<T0>
    : IQuery
{
	void Execute(Entity e, ref T0 t0);
}

public interface IQueryWR<T0, T1>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, in T1 t1);
}

public interface IQueryWRR<T0, T1, T2>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, in T1 t1, in T2 t2);
}

public interface IQueryWRRR<T0, T1, T2, T3>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, in T1 t1, in T2 t2, in T3 t3);
}

public interface IQueryWRRRR<T0, T1, T2, T3, T4>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, in T1 t1, in T2 t2, in T3 t3, in T4 t4);
}

public interface IQueryWRRRRR<T0, T1, T2, T3, T4, T5>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, in T1 t1, in T2 t2, in T3 t3, in T4 t4, in T5 t5);
}

public interface IQueryWRRRRRR<T0, T1, T2, T3, T4, T5, T6>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, in T1 t1, in T2 t2, in T3 t3, in T4 t4, in T5 t5, in T6 t6);
}

public interface IQueryWRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, in T1 t1, in T2 t2, in T3 t3, in T4 t4, in T5 t5, in T6 t6, in T7 t7);
}

public interface IQueryWRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, in T1 t1, in T2 t2, in T3 t3, in T4 t4, in T5 t5, in T6 t6, in T7 t7, in T8 t8);
}

public interface IQueryWW<T0, T1>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1);
}

public interface IQueryWWR<T0, T1, T2>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, in T2 t2);
}

public interface IQueryWWRR<T0, T1, T2, T3>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, in T2 t2, in T3 t3);
}

public interface IQueryWWRRR<T0, T1, T2, T3, T4>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, in T2 t2, in T3 t3, in T4 t4);
}

public interface IQueryWWRRRR<T0, T1, T2, T3, T4, T5>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, in T2 t2, in T3 t3, in T4 t4, in T5 t5);
}

public interface IQueryWWRRRRR<T0, T1, T2, T3, T4, T5, T6>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, in T2 t2, in T3 t3, in T4 t4, in T5 t5, in T6 t6);
}

public interface IQueryWWRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, in T2 t2, in T3 t3, in T4 t4, in T5 t5, in T6 t6, in T7 t7);
}

public interface IQueryWWRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, in T2 t2, in T3 t3, in T4 t4, in T5 t5, in T6 t6, in T7 t7, in T8 t8);
}

public interface IQueryWWRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, in T2 t2, in T3 t3, in T4 t4, in T5 t5, in T6 t6, in T7 t7, in T8 t8, in T9 t9);
}

public interface IQueryWWW<T0, T1, T2>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2);
}

public interface IQueryWWWR<T0, T1, T2, T3>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, in T3 t3);
}

public interface IQueryWWWRR<T0, T1, T2, T3, T4>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, in T3 t3, in T4 t4);
}

public interface IQueryWWWRRR<T0, T1, T2, T3, T4, T5>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, in T3 t3, in T4 t4, in T5 t5);
}

public interface IQueryWWWRRRR<T0, T1, T2, T3, T4, T5, T6>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, in T3 t3, in T4 t4, in T5 t5, in T6 t6);
}

public interface IQueryWWWRRRRR<T0, T1, T2, T3, T4, T5, T6, T7>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, in T3 t3, in T4 t4, in T5 t5, in T6 t6, in T7 t7);
}

public interface IQueryWWWRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, in T3 t3, in T4 t4, in T5 t5, in T6 t6, in T7 t7, in T8 t8);
}

public interface IQueryWWWRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, in T3 t3, in T4 t4, in T5 t5, in T6 t6, in T7 t7, in T8 t8, in T9 t9);
}

public interface IQueryWWWRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, in T3 t3, in T4 t4, in T5 t5, in T6 t6, in T7 t7, in T8 t8, in T9 t9, in T10 t10);
}

public interface IQueryWWWW<T0, T1, T2, T3>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3);
}

public interface IQueryWWWWR<T0, T1, T2, T3, T4>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, in T4 t4);
}

public interface IQueryWWWWRR<T0, T1, T2, T3, T4, T5>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, in T4 t4, in T5 t5);
}

public interface IQueryWWWWRRR<T0, T1, T2, T3, T4, T5, T6>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, in T4 t4, in T5 t5, in T6 t6);
}

public interface IQueryWWWWRRRR<T0, T1, T2, T3, T4, T5, T6, T7>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, in T4 t4, in T5 t5, in T6 t6, in T7 t7);
}

public interface IQueryWWWWRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, in T4 t4, in T5 t5, in T6 t6, in T7 t7, in T8 t8);
}

public interface IQueryWWWWRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, in T4 t4, in T5 t5, in T6 t6, in T7 t7, in T8 t8, in T9 t9);
}

public interface IQueryWWWWRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, in T4 t4, in T5 t5, in T6 t6, in T7 t7, in T8 t8, in T9 t9, in T10 t10);
}

public interface IQueryWWWWRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, in T4 t4, in T5 t5, in T6 t6, in T7 t7, in T8 t8, in T9 t9, in T10 t10, in T11 t11);
}

public interface IQueryWWWWW<T0, T1, T2, T3, T4>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4);
}

public interface IQueryWWWWWR<T0, T1, T2, T3, T4, T5>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, in T5 t5);
}

public interface IQueryWWWWWRR<T0, T1, T2, T3, T4, T5, T6>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, in T5 t5, in T6 t6);
}

public interface IQueryWWWWWRRR<T0, T1, T2, T3, T4, T5, T6, T7>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, in T5 t5, in T6 t6, in T7 t7);
}

public interface IQueryWWWWWRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, in T5 t5, in T6 t6, in T7 t7, in T8 t8);
}

public interface IQueryWWWWWRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, in T5 t5, in T6 t6, in T7 t7, in T8 t8, in T9 t9);
}

public interface IQueryWWWWWRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, in T5 t5, in T6 t6, in T7 t7, in T8 t8, in T9 t9, in T10 t10);
}

public interface IQueryWWWWWRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, in T5 t5, in T6 t6, in T7 t7, in T8 t8, in T9 t9, in T10 t10, in T11 t11);
}

public interface IQueryWWWWWRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, in T5 t5, in T6 t6, in T7 t7, in T8 t8, in T9 t9, in T10 t10, in T11 t11, in T12 t12);
}

public interface IQueryWWWWWW<T0, T1, T2, T3, T4, T5>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5);
}

public interface IQueryWWWWWWR<T0, T1, T2, T3, T4, T5, T6>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, in T6 t6);
}

public interface IQueryWWWWWWRR<T0, T1, T2, T3, T4, T5, T6, T7>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, in T6 t6, in T7 t7);
}

public interface IQueryWWWWWWRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, in T6 t6, in T7 t7, in T8 t8);
}

public interface IQueryWWWWWWRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, in T6 t6, in T7 t7, in T8 t8, in T9 t9);
}

public interface IQueryWWWWWWRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, in T6 t6, in T7 t7, in T8 t8, in T9 t9, in T10 t10);
}

public interface IQueryWWWWWWRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, in T6 t6, in T7 t7, in T8 t8, in T9 t9, in T10 t10, in T11 t11);
}

public interface IQueryWWWWWWRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, in T6 t6, in T7 t7, in T8 t8, in T9 t9, in T10 t10, in T11 t11, in T12 t12);
}

public interface IQueryWWWWWWRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, in T6 t6, in T7 t7, in T8 t8, in T9 t9, in T10 t10, in T11 t11, in T12 t12, in T13 t13);
}

public interface IQueryWWWWWWW<T0, T1, T2, T3, T4, T5, T6>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6);
}

public interface IQueryWWWWWWWR<T0, T1, T2, T3, T4, T5, T6, T7>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, in T7 t7);
}

public interface IQueryWWWWWWWRR<T0, T1, T2, T3, T4, T5, T6, T7, T8>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, in T7 t7, in T8 t8);
}

public interface IQueryWWWWWWWRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, in T7 t7, in T8 t8, in T9 t9);
}

public interface IQueryWWWWWWWRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, in T7 t7, in T8 t8, in T9 t9, in T10 t10);
}

public interface IQueryWWWWWWWRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, in T7 t7, in T8 t8, in T9 t9, in T10 t10, in T11 t11);
}

public interface IQueryWWWWWWWRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, in T7 t7, in T8 t8, in T9 t9, in T10 t10, in T11 t11, in T12 t12);
}

public interface IQueryWWWWWWWRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, in T7 t7, in T8 t8, in T9 t9, in T10 t10, in T11 t11, in T12 t12, in T13 t13);
}

public interface IQueryWWWWWWWRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, in T7 t7, in T8 t8, in T9 t9, in T10 t10, in T11 t11, in T12 t12, in T13 t13, in T14 t14);
}

public interface IQueryWWWWWWWW<T0, T1, T2, T3, T4, T5, T6, T7>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7);
}

public interface IQueryWWWWWWWWR<T0, T1, T2, T3, T4, T5, T6, T7, T8>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, in T8 t8);
}

public interface IQueryWWWWWWWWRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, in T8 t8, in T9 t9);
}

public interface IQueryWWWWWWWWRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, in T8 t8, in T9 t9, in T10 t10);
}

public interface IQueryWWWWWWWWRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, in T8 t8, in T9 t9, in T10 t10, in T11 t11);
}

public interface IQueryWWWWWWWWRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, in T8 t8, in T9 t9, in T10 t10, in T11 t11, in T12 t12);
}

public interface IQueryWWWWWWWWRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, in T8 t8, in T9 t9, in T10 t10, in T11 t11, in T12 t12, in T13 t13);
}

public interface IQueryWWWWWWWWRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, in T8 t8, in T9 t9, in T10 t10, in T11 t11, in T12 t12, in T13 t13, in T14 t14);
}

public interface IQueryWWWWWWWWRRRRRRRR<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
    : IQuery
{
	void Execute(Entity e, ref T0 t0, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4, ref T5 t5, ref T6 t6, ref T7 t7, in T8 t8, in T9 t9, in T10 t10, in T11 t11, in T12 t12, in T13 t13, in T14 t14, in T15 t15);
}



