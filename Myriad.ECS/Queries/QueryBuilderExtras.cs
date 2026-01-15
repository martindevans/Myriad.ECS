using System.Diagnostics.CodeAnalysis;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedParameter.Global
// ReSharper disable LoopCanBeConvertedToQuery
// ReSharper disable ConvertToPrimaryConstructor

namespace Myriad.ECS.Queries;

public sealed partial class QueryBuilder
{

	/// <summary>
	/// Include only entities which have all of these components
	/// </summary>
	/// <returns>The query builder</returns>
	[ExcludeFromCodeCoverage]
	public QueryBuilder Include<T0, T1>()
		where T0 : IComponent
		where T1 : IComponent
	{
		Include<T0>();
		Include<T1>();

		return this;
	}

	/// <summary>
	/// Include only entities which have all of these components
	/// </summary>
	/// <returns>The query builder</returns>
	// ReSharper disable once UnusedTypeParameter (Justification: Used for checking the query against the type signature)
	[ExcludeFromCodeCoverage]
	public QueryBuilder IncludeQuery<TQ, T0, T1>()
		where TQ : IQuery<T0, T1>
		where T0 : IComponent
		where T1 : IComponent
	{
		Include<T0>();
		Include<T1>();

		return this;
	}

	/// <summary>
	/// Include only entities which have all of these components
	/// </summary>
	/// <returns>The query builder</returns>
	[ExcludeFromCodeCoverage]
	public QueryBuilder Include<T0, T1, T2>()
		where T0 : IComponent
		where T1 : IComponent
		where T2 : IComponent
	{
		Include<T0>();
		Include<T1>();
		Include<T2>();

		return this;
	}

	/// <summary>
	/// Include only entities which have all of these components
	/// </summary>
	/// <returns>The query builder</returns>
	// ReSharper disable once UnusedTypeParameter (Justification: Used for checking the query against the type signature)
	[ExcludeFromCodeCoverage]
	public QueryBuilder IncludeQuery<TQ, T0, T1, T2>()
		where TQ : IQuery<T0, T1, T2>
		where T0 : IComponent
		where T1 : IComponent
		where T2 : IComponent
	{
		Include<T0>();
		Include<T1>();
		Include<T2>();

		return this;
	}

	/// <summary>
	/// Include only entities which have all of these components
	/// </summary>
	/// <returns>The query builder</returns>
	[ExcludeFromCodeCoverage]
	public QueryBuilder Include<T0, T1, T2, T3>()
		where T0 : IComponent
		where T1 : IComponent
		where T2 : IComponent
		where T3 : IComponent
	{
		Include<T0>();
		Include<T1>();
		Include<T2>();
		Include<T3>();

		return this;
	}

	/// <summary>
	/// Include only entities which have all of these components
	/// </summary>
	/// <returns>The query builder</returns>
	// ReSharper disable once UnusedTypeParameter (Justification: Used for checking the query against the type signature)
	[ExcludeFromCodeCoverage]
	public QueryBuilder IncludeQuery<TQ, T0, T1, T2, T3>()
		where TQ : IQuery<T0, T1, T2, T3>
		where T0 : IComponent
		where T1 : IComponent
		where T2 : IComponent
		where T3 : IComponent
	{
		Include<T0>();
		Include<T1>();
		Include<T2>();
		Include<T3>();

		return this;
	}

	/// <summary>
	/// Include only entities which have all of these components
	/// </summary>
	/// <returns>The query builder</returns>
	[ExcludeFromCodeCoverage]
	public QueryBuilder Include<T0, T1, T2, T3, T4>()
		where T0 : IComponent
		where T1 : IComponent
		where T2 : IComponent
		where T3 : IComponent
		where T4 : IComponent
	{
		Include<T0>();
		Include<T1>();
		Include<T2>();
		Include<T3>();
		Include<T4>();

		return this;
	}

	/// <summary>
	/// Include only entities which have all of these components
	/// </summary>
	/// <returns>The query builder</returns>
	// ReSharper disable once UnusedTypeParameter (Justification: Used for checking the query against the type signature)
	[ExcludeFromCodeCoverage]
	public QueryBuilder IncludeQuery<TQ, T0, T1, T2, T3, T4>()
		where TQ : IQuery<T0, T1, T2, T3, T4>
		where T0 : IComponent
		where T1 : IComponent
		where T2 : IComponent
		where T3 : IComponent
		where T4 : IComponent
	{
		Include<T0>();
		Include<T1>();
		Include<T2>();
		Include<T3>();
		Include<T4>();

		return this;
	}

	/// <summary>
	/// Include only entities which have all of these components
	/// </summary>
	/// <returns>The query builder</returns>
	[ExcludeFromCodeCoverage]
	public QueryBuilder Include<T0, T1, T2, T3, T4, T5>()
		where T0 : IComponent
		where T1 : IComponent
		where T2 : IComponent
		where T3 : IComponent
		where T4 : IComponent
		where T5 : IComponent
	{
		Include<T0>();
		Include<T1>();
		Include<T2>();
		Include<T3>();
		Include<T4>();
		Include<T5>();

		return this;
	}

	/// <summary>
	/// Include only entities which have all of these components
	/// </summary>
	/// <returns>The query builder</returns>
	// ReSharper disable once UnusedTypeParameter (Justification: Used for checking the query against the type signature)
	[ExcludeFromCodeCoverage]
	public QueryBuilder IncludeQuery<TQ, T0, T1, T2, T3, T4, T5>()
		where TQ : IQuery<T0, T1, T2, T3, T4, T5>
		where T0 : IComponent
		where T1 : IComponent
		where T2 : IComponent
		where T3 : IComponent
		where T4 : IComponent
		where T5 : IComponent
	{
		Include<T0>();
		Include<T1>();
		Include<T2>();
		Include<T3>();
		Include<T4>();
		Include<T5>();

		return this;
	}

	/// <summary>
	/// Include only entities which have all of these components
	/// </summary>
	/// <returns>The query builder</returns>
	[ExcludeFromCodeCoverage]
	public QueryBuilder Include<T0, T1, T2, T3, T4, T5, T6>()
		where T0 : IComponent
		where T1 : IComponent
		where T2 : IComponent
		where T3 : IComponent
		where T4 : IComponent
		where T5 : IComponent
		where T6 : IComponent
	{
		Include<T0>();
		Include<T1>();
		Include<T2>();
		Include<T3>();
		Include<T4>();
		Include<T5>();
		Include<T6>();

		return this;
	}

	/// <summary>
	/// Include only entities which have all of these components
	/// </summary>
	/// <returns>The query builder</returns>
	// ReSharper disable once UnusedTypeParameter (Justification: Used for checking the query against the type signature)
	[ExcludeFromCodeCoverage]
	public QueryBuilder IncludeQuery<TQ, T0, T1, T2, T3, T4, T5, T6>()
		where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6>
		where T0 : IComponent
		where T1 : IComponent
		where T2 : IComponent
		where T3 : IComponent
		where T4 : IComponent
		where T5 : IComponent
		where T6 : IComponent
	{
		Include<T0>();
		Include<T1>();
		Include<T2>();
		Include<T3>();
		Include<T4>();
		Include<T5>();
		Include<T6>();

		return this;
	}

	/// <summary>
	/// Include only entities which have all of these components
	/// </summary>
	/// <returns>The query builder</returns>
	[ExcludeFromCodeCoverage]
	public QueryBuilder Include<T0, T1, T2, T3, T4, T5, T6, T7>()
		where T0 : IComponent
		where T1 : IComponent
		where T2 : IComponent
		where T3 : IComponent
		where T4 : IComponent
		where T5 : IComponent
		where T6 : IComponent
		where T7 : IComponent
	{
		Include<T0>();
		Include<T1>();
		Include<T2>();
		Include<T3>();
		Include<T4>();
		Include<T5>();
		Include<T6>();
		Include<T7>();

		return this;
	}

	/// <summary>
	/// Include only entities which have all of these components
	/// </summary>
	/// <returns>The query builder</returns>
	// ReSharper disable once UnusedTypeParameter (Justification: Used for checking the query against the type signature)
	[ExcludeFromCodeCoverage]
	public QueryBuilder IncludeQuery<TQ, T0, T1, T2, T3, T4, T5, T6, T7>()
		where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7>
		where T0 : IComponent
		where T1 : IComponent
		where T2 : IComponent
		where T3 : IComponent
		where T4 : IComponent
		where T5 : IComponent
		where T6 : IComponent
		where T7 : IComponent
	{
		Include<T0>();
		Include<T1>();
		Include<T2>();
		Include<T3>();
		Include<T4>();
		Include<T5>();
		Include<T6>();
		Include<T7>();

		return this;
	}

	/// <summary>
	/// Include only entities which have all of these components
	/// </summary>
	/// <returns>The query builder</returns>
	[ExcludeFromCodeCoverage]
	public QueryBuilder Include<T0, T1, T2, T3, T4, T5, T6, T7, T8>()
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
		Include<T0>();
		Include<T1>();
		Include<T2>();
		Include<T3>();
		Include<T4>();
		Include<T5>();
		Include<T6>();
		Include<T7>();
		Include<T8>();

		return this;
	}

	/// <summary>
	/// Include only entities which have all of these components
	/// </summary>
	/// <returns>The query builder</returns>
	// ReSharper disable once UnusedTypeParameter (Justification: Used for checking the query against the type signature)
	[ExcludeFromCodeCoverage]
	public QueryBuilder IncludeQuery<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8>()
		where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8>
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
		Include<T0>();
		Include<T1>();
		Include<T2>();
		Include<T3>();
		Include<T4>();
		Include<T5>();
		Include<T6>();
		Include<T7>();
		Include<T8>();

		return this;
	}

	/// <summary>
	/// Include only entities which have all of these components
	/// </summary>
	/// <returns>The query builder</returns>
	[ExcludeFromCodeCoverage]
	public QueryBuilder Include<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>()
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
		Include<T0>();
		Include<T1>();
		Include<T2>();
		Include<T3>();
		Include<T4>();
		Include<T5>();
		Include<T6>();
		Include<T7>();
		Include<T8>();
		Include<T9>();

		return this;
	}

	/// <summary>
	/// Include only entities which have all of these components
	/// </summary>
	/// <returns>The query builder</returns>
	// ReSharper disable once UnusedTypeParameter (Justification: Used for checking the query against the type signature)
	[ExcludeFromCodeCoverage]
	public QueryBuilder IncludeQuery<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>()
		where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
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
		Include<T0>();
		Include<T1>();
		Include<T2>();
		Include<T3>();
		Include<T4>();
		Include<T5>();
		Include<T6>();
		Include<T7>();
		Include<T8>();
		Include<T9>();

		return this;
	}

	/// <summary>
	/// Include only entities which have all of these components
	/// </summary>
	/// <returns>The query builder</returns>
	[ExcludeFromCodeCoverage]
	public QueryBuilder Include<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>()
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
		Include<T0>();
		Include<T1>();
		Include<T2>();
		Include<T3>();
		Include<T4>();
		Include<T5>();
		Include<T6>();
		Include<T7>();
		Include<T8>();
		Include<T9>();
		Include<T10>();

		return this;
	}

	/// <summary>
	/// Include only entities which have all of these components
	/// </summary>
	/// <returns>The query builder</returns>
	// ReSharper disable once UnusedTypeParameter (Justification: Used for checking the query against the type signature)
	[ExcludeFromCodeCoverage]
	public QueryBuilder IncludeQuery<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>()
		where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
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
		Include<T0>();
		Include<T1>();
		Include<T2>();
		Include<T3>();
		Include<T4>();
		Include<T5>();
		Include<T6>();
		Include<T7>();
		Include<T8>();
		Include<T9>();
		Include<T10>();

		return this;
	}

	/// <summary>
	/// Include only entities which have all of these components
	/// </summary>
	/// <returns>The query builder</returns>
	[ExcludeFromCodeCoverage]
	public QueryBuilder Include<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>()
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
		Include<T0>();
		Include<T1>();
		Include<T2>();
		Include<T3>();
		Include<T4>();
		Include<T5>();
		Include<T6>();
		Include<T7>();
		Include<T8>();
		Include<T9>();
		Include<T10>();
		Include<T11>();

		return this;
	}

	/// <summary>
	/// Include only entities which have all of these components
	/// </summary>
	/// <returns>The query builder</returns>
	// ReSharper disable once UnusedTypeParameter (Justification: Used for checking the query against the type signature)
	[ExcludeFromCodeCoverage]
	public QueryBuilder IncludeQuery<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>()
		where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
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
		Include<T0>();
		Include<T1>();
		Include<T2>();
		Include<T3>();
		Include<T4>();
		Include<T5>();
		Include<T6>();
		Include<T7>();
		Include<T8>();
		Include<T9>();
		Include<T10>();
		Include<T11>();

		return this;
	}

	/// <summary>
	/// Include only entities which have all of these components
	/// </summary>
	/// <returns>The query builder</returns>
	[ExcludeFromCodeCoverage]
	public QueryBuilder Include<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>()
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
		Include<T0>();
		Include<T1>();
		Include<T2>();
		Include<T3>();
		Include<T4>();
		Include<T5>();
		Include<T6>();
		Include<T7>();
		Include<T8>();
		Include<T9>();
		Include<T10>();
		Include<T11>();
		Include<T12>();

		return this;
	}

	/// <summary>
	/// Include only entities which have all of these components
	/// </summary>
	/// <returns>The query builder</returns>
	// ReSharper disable once UnusedTypeParameter (Justification: Used for checking the query against the type signature)
	[ExcludeFromCodeCoverage]
	public QueryBuilder IncludeQuery<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>()
		where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
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
		Include<T0>();
		Include<T1>();
		Include<T2>();
		Include<T3>();
		Include<T4>();
		Include<T5>();
		Include<T6>();
		Include<T7>();
		Include<T8>();
		Include<T9>();
		Include<T10>();
		Include<T11>();
		Include<T12>();

		return this;
	}

	/// <summary>
	/// Include only entities which have all of these components
	/// </summary>
	/// <returns>The query builder</returns>
	[ExcludeFromCodeCoverage]
	public QueryBuilder Include<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>()
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
		Include<T0>();
		Include<T1>();
		Include<T2>();
		Include<T3>();
		Include<T4>();
		Include<T5>();
		Include<T6>();
		Include<T7>();
		Include<T8>();
		Include<T9>();
		Include<T10>();
		Include<T11>();
		Include<T12>();
		Include<T13>();

		return this;
	}

	/// <summary>
	/// Include only entities which have all of these components
	/// </summary>
	/// <returns>The query builder</returns>
	// ReSharper disable once UnusedTypeParameter (Justification: Used for checking the query against the type signature)
	[ExcludeFromCodeCoverage]
	public QueryBuilder IncludeQuery<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>()
		where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
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
		Include<T0>();
		Include<T1>();
		Include<T2>();
		Include<T3>();
		Include<T4>();
		Include<T5>();
		Include<T6>();
		Include<T7>();
		Include<T8>();
		Include<T9>();
		Include<T10>();
		Include<T11>();
		Include<T12>();
		Include<T13>();

		return this;
	}

	/// <summary>
	/// Include only entities which have all of these components
	/// </summary>
	/// <returns>The query builder</returns>
	[ExcludeFromCodeCoverage]
	public QueryBuilder Include<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>()
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
		Include<T0>();
		Include<T1>();
		Include<T2>();
		Include<T3>();
		Include<T4>();
		Include<T5>();
		Include<T6>();
		Include<T7>();
		Include<T8>();
		Include<T9>();
		Include<T10>();
		Include<T11>();
		Include<T12>();
		Include<T13>();
		Include<T14>();

		return this;
	}

	/// <summary>
	/// Include only entities which have all of these components
	/// </summary>
	/// <returns>The query builder</returns>
	// ReSharper disable once UnusedTypeParameter (Justification: Used for checking the query against the type signature)
	[ExcludeFromCodeCoverage]
	public QueryBuilder IncludeQuery<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>()
		where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
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
		Include<T0>();
		Include<T1>();
		Include<T2>();
		Include<T3>();
		Include<T4>();
		Include<T5>();
		Include<T6>();
		Include<T7>();
		Include<T8>();
		Include<T9>();
		Include<T10>();
		Include<T11>();
		Include<T12>();
		Include<T13>();
		Include<T14>();

		return this;
	}

	/// <summary>
	/// Include only entities which have all of these components
	/// </summary>
	/// <returns>The query builder</returns>
	[ExcludeFromCodeCoverage]
	public QueryBuilder Include<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>()
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
		Include<T0>();
		Include<T1>();
		Include<T2>();
		Include<T3>();
		Include<T4>();
		Include<T5>();
		Include<T6>();
		Include<T7>();
		Include<T8>();
		Include<T9>();
		Include<T10>();
		Include<T11>();
		Include<T12>();
		Include<T13>();
		Include<T14>();
		Include<T15>();

		return this;
	}

	/// <summary>
	/// Include only entities which have all of these components
	/// </summary>
	/// <returns>The query builder</returns>
	// ReSharper disable once UnusedTypeParameter (Justification: Used for checking the query against the type signature)
	[ExcludeFromCodeCoverage]
	public QueryBuilder IncludeQuery<TQ, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>()
		where TQ : IQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
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
		Include<T0>();
		Include<T1>();
		Include<T2>();
		Include<T3>();
		Include<T4>();
		Include<T5>();
		Include<T6>();
		Include<T7>();
		Include<T8>();
		Include<T9>();
		Include<T10>();
		Include<T11>();
		Include<T12>();
		Include<T13>();
		Include<T14>();
		Include<T15>();

		return this;
	}


	/// <summary>
	/// Exclude entities which have any of these components
	/// </summary>
	/// <returns>The query builder</returns>
	[ExcludeFromCodeCoverage]
	public QueryBuilder Exclude<T0, T1>()
		where T0 : IComponent
		where T1 : IComponent
	{
		Exclude<T0>();
		Exclude<T1>();

		return this;
	}

	/// <summary>
	/// Exclude entities which have any of these components
	/// </summary>
	/// <returns>The query builder</returns>
	[ExcludeFromCodeCoverage]
	public QueryBuilder Exclude<T0, T1, T2>()
		where T0 : IComponent
		where T1 : IComponent
		where T2 : IComponent
	{
		Exclude<T0>();
		Exclude<T1>();
		Exclude<T2>();

		return this;
	}

	/// <summary>
	/// Exclude entities which have any of these components
	/// </summary>
	/// <returns>The query builder</returns>
	[ExcludeFromCodeCoverage]
	public QueryBuilder Exclude<T0, T1, T2, T3>()
		where T0 : IComponent
		where T1 : IComponent
		where T2 : IComponent
		where T3 : IComponent
	{
		Exclude<T0>();
		Exclude<T1>();
		Exclude<T2>();
		Exclude<T3>();

		return this;
	}

	/// <summary>
	/// Exclude entities which have any of these components
	/// </summary>
	/// <returns>The query builder</returns>
	[ExcludeFromCodeCoverage]
	public QueryBuilder Exclude<T0, T1, T2, T3, T4>()
		where T0 : IComponent
		where T1 : IComponent
		where T2 : IComponent
		where T3 : IComponent
		where T4 : IComponent
	{
		Exclude<T0>();
		Exclude<T1>();
		Exclude<T2>();
		Exclude<T3>();
		Exclude<T4>();

		return this;
	}

	/// <summary>
	/// Exclude entities which have any of these components
	/// </summary>
	/// <returns>The query builder</returns>
	[ExcludeFromCodeCoverage]
	public QueryBuilder Exclude<T0, T1, T2, T3, T4, T5>()
		where T0 : IComponent
		where T1 : IComponent
		where T2 : IComponent
		where T3 : IComponent
		where T4 : IComponent
		where T5 : IComponent
	{
		Exclude<T0>();
		Exclude<T1>();
		Exclude<T2>();
		Exclude<T3>();
		Exclude<T4>();
		Exclude<T5>();

		return this;
	}

	/// <summary>
	/// Exclude entities which have any of these components
	/// </summary>
	/// <returns>The query builder</returns>
	[ExcludeFromCodeCoverage]
	public QueryBuilder Exclude<T0, T1, T2, T3, T4, T5, T6>()
		where T0 : IComponent
		where T1 : IComponent
		where T2 : IComponent
		where T3 : IComponent
		where T4 : IComponent
		where T5 : IComponent
		where T6 : IComponent
	{
		Exclude<T0>();
		Exclude<T1>();
		Exclude<T2>();
		Exclude<T3>();
		Exclude<T4>();
		Exclude<T5>();
		Exclude<T6>();

		return this;
	}

	/// <summary>
	/// Exclude entities which have any of these components
	/// </summary>
	/// <returns>The query builder</returns>
	[ExcludeFromCodeCoverage]
	public QueryBuilder Exclude<T0, T1, T2, T3, T4, T5, T6, T7>()
		where T0 : IComponent
		where T1 : IComponent
		where T2 : IComponent
		where T3 : IComponent
		where T4 : IComponent
		where T5 : IComponent
		where T6 : IComponent
		where T7 : IComponent
	{
		Exclude<T0>();
		Exclude<T1>();
		Exclude<T2>();
		Exclude<T3>();
		Exclude<T4>();
		Exclude<T5>();
		Exclude<T6>();
		Exclude<T7>();

		return this;
	}

	/// <summary>
	/// Exclude entities which have any of these components
	/// </summary>
	/// <returns>The query builder</returns>
	[ExcludeFromCodeCoverage]
	public QueryBuilder Exclude<T0, T1, T2, T3, T4, T5, T6, T7, T8>()
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
		Exclude<T0>();
		Exclude<T1>();
		Exclude<T2>();
		Exclude<T3>();
		Exclude<T4>();
		Exclude<T5>();
		Exclude<T6>();
		Exclude<T7>();
		Exclude<T8>();

		return this;
	}

	/// <summary>
	/// Exclude entities which have any of these components
	/// </summary>
	/// <returns>The query builder</returns>
	[ExcludeFromCodeCoverage]
	public QueryBuilder Exclude<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>()
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
		Exclude<T0>();
		Exclude<T1>();
		Exclude<T2>();
		Exclude<T3>();
		Exclude<T4>();
		Exclude<T5>();
		Exclude<T6>();
		Exclude<T7>();
		Exclude<T8>();
		Exclude<T9>();

		return this;
	}

	/// <summary>
	/// Exclude entities which have any of these components
	/// </summary>
	/// <returns>The query builder</returns>
	[ExcludeFromCodeCoverage]
	public QueryBuilder Exclude<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>()
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
		Exclude<T0>();
		Exclude<T1>();
		Exclude<T2>();
		Exclude<T3>();
		Exclude<T4>();
		Exclude<T5>();
		Exclude<T6>();
		Exclude<T7>();
		Exclude<T8>();
		Exclude<T9>();
		Exclude<T10>();

		return this;
	}

	/// <summary>
	/// Exclude entities which have any of these components
	/// </summary>
	/// <returns>The query builder</returns>
	[ExcludeFromCodeCoverage]
	public QueryBuilder Exclude<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>()
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
		Exclude<T0>();
		Exclude<T1>();
		Exclude<T2>();
		Exclude<T3>();
		Exclude<T4>();
		Exclude<T5>();
		Exclude<T6>();
		Exclude<T7>();
		Exclude<T8>();
		Exclude<T9>();
		Exclude<T10>();
		Exclude<T11>();

		return this;
	}

	/// <summary>
	/// Exclude entities which have any of these components
	/// </summary>
	/// <returns>The query builder</returns>
	[ExcludeFromCodeCoverage]
	public QueryBuilder Exclude<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>()
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
		Exclude<T0>();
		Exclude<T1>();
		Exclude<T2>();
		Exclude<T3>();
		Exclude<T4>();
		Exclude<T5>();
		Exclude<T6>();
		Exclude<T7>();
		Exclude<T8>();
		Exclude<T9>();
		Exclude<T10>();
		Exclude<T11>();
		Exclude<T12>();

		return this;
	}

	/// <summary>
	/// Exclude entities which have any of these components
	/// </summary>
	/// <returns>The query builder</returns>
	[ExcludeFromCodeCoverage]
	public QueryBuilder Exclude<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>()
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
		Exclude<T0>();
		Exclude<T1>();
		Exclude<T2>();
		Exclude<T3>();
		Exclude<T4>();
		Exclude<T5>();
		Exclude<T6>();
		Exclude<T7>();
		Exclude<T8>();
		Exclude<T9>();
		Exclude<T10>();
		Exclude<T11>();
		Exclude<T12>();
		Exclude<T13>();

		return this;
	}

	/// <summary>
	/// Exclude entities which have any of these components
	/// </summary>
	/// <returns>The query builder</returns>
	[ExcludeFromCodeCoverage]
	public QueryBuilder Exclude<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>()
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
		Exclude<T0>();
		Exclude<T1>();
		Exclude<T2>();
		Exclude<T3>();
		Exclude<T4>();
		Exclude<T5>();
		Exclude<T6>();
		Exclude<T7>();
		Exclude<T8>();
		Exclude<T9>();
		Exclude<T10>();
		Exclude<T11>();
		Exclude<T12>();
		Exclude<T13>();
		Exclude<T14>();

		return this;
	}

	/// <summary>
	/// Exclude entities which have any of these components
	/// </summary>
	/// <returns>The query builder</returns>
	[ExcludeFromCodeCoverage]
	public QueryBuilder Exclude<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>()
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
		Exclude<T0>();
		Exclude<T1>();
		Exclude<T2>();
		Exclude<T3>();
		Exclude<T4>();
		Exclude<T5>();
		Exclude<T6>();
		Exclude<T7>();
		Exclude<T8>();
		Exclude<T9>();
		Exclude<T10>();
		Exclude<T11>();
		Exclude<T12>();
		Exclude<T13>();
		Exclude<T14>();
		Exclude<T15>();

		return this;
	}


}