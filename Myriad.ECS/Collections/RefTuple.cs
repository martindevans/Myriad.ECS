
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedParameter.Global
// ReSharper disable LoopCanBeConvertedToQuery
// ReSharper disable CheckNamespace
// ReSharper disable ArrangeAccessorOwnerBody

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Myriad.ECS.Collections;

#if NET6_0_OR_GREATER

public readonly ref struct RefT<T>
{
	private readonly ref T _ref;

	public ref T Ref 
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _ref;
	}

	internal RefT(ref T r)
	{
		_ref = ref r;
	}

	public static implicit operator T(RefT<T> reference)
    {
		return reference.Ref;
    }
}

#else

public readonly struct RefT<T>
{
	private readonly T[] _arr;
	private readonly int _index;

	public ref T Ref
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
	    get
	    {
			return ref _arr[_index];
	    }
	}

	internal RefT(T[] a, int i)
	{
		_arr = a;
		_index = i;
	}

	public static implicit operator T(RefT<T> reference)
    {
		return reference.Ref;
    }
}

#endif



public readonly ref struct RefTuple<T0>
{
	public readonly Entity Entity;

	private readonly RefT<T0> _item0;
	public ref T0 Item0
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item0.Ref;
	}

	internal RefTuple(Entity entity, RefT<T0> item0)
	{
		Entity = entity;
		_item0 = item0;
	}

	public void Deconstruct(out Entity entity, out RefT<T0> item0)
	{
		entity = Entity;
		item0 = _item0;
	}

}


public readonly ref struct RefTuple<T0, T1>
{
	public readonly Entity Entity;

	private readonly RefT<T0> _item0;
	public ref T0 Item0
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item0.Ref;
	}

	private readonly RefT<T1> _item1;
	public ref T1 Item1
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item1.Ref;
	}

	internal RefTuple(Entity entity, RefT<T0> item0, RefT<T1> item1)
	{
		Entity = entity;
		_item0 = item0;
		_item1 = item1;
	}

	public void Deconstruct(out Entity entity, out RefT<T0> item0, out RefT<T1> item1)
	{
		entity = Entity;
		item0 = _item0;
		item1 = _item1;
	}

	public void Deconstruct(out RefT<T0> item0, out RefT<T1> item1)
	{
		item0 = _item0;
		item1 = _item1;
	}
}

[ExcludeFromCodeCoverage]
public readonly ref struct RefTuple<T0, T1, T2>
{
	public readonly Entity Entity;

	private readonly RefT<T0> _item0;
	public ref T0 Item0
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item0.Ref;
	}

	private readonly RefT<T1> _item1;
	public ref T1 Item1
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item1.Ref;
	}

	private readonly RefT<T2> _item2;
	public ref T2 Item2
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item2.Ref;
	}

	internal RefTuple(Entity entity, RefT<T0> item0, RefT<T1> item1, RefT<T2> item2)
	{
		Entity = entity;
		_item0 = item0;
		_item1 = item1;
		_item2 = item2;
	}

	public void Deconstruct(out Entity entity, out RefT<T0> item0, out RefT<T1> item1, out RefT<T2> item2)
	{
		entity = Entity;
		item0 = _item0;
		item1 = _item1;
		item2 = _item2;
	}

	public void Deconstruct(out RefT<T0> item0, out RefT<T1> item1, out RefT<T2> item2)
	{
		item0 = _item0;
		item1 = _item1;
		item2 = _item2;
	}
}

[ExcludeFromCodeCoverage]
public readonly ref struct RefTuple<T0, T1, T2, T3>
{
	public readonly Entity Entity;

	private readonly RefT<T0> _item0;
	public ref T0 Item0
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item0.Ref;
	}

	private readonly RefT<T1> _item1;
	public ref T1 Item1
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item1.Ref;
	}

	private readonly RefT<T2> _item2;
	public ref T2 Item2
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item2.Ref;
	}

	private readonly RefT<T3> _item3;
	public ref T3 Item3
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item3.Ref;
	}

	internal RefTuple(Entity entity, RefT<T0> item0, RefT<T1> item1, RefT<T2> item2, RefT<T3> item3)
	{
		Entity = entity;
		_item0 = item0;
		_item1 = item1;
		_item2 = item2;
		_item3 = item3;
	}

	public void Deconstruct(out Entity entity, out RefT<T0> item0, out RefT<T1> item1, out RefT<T2> item2, out RefT<T3> item3)
	{
		entity = Entity;
		item0 = _item0;
		item1 = _item1;
		item2 = _item2;
		item3 = _item3;
	}

	public void Deconstruct(out RefT<T0> item0, out RefT<T1> item1, out RefT<T2> item2, out RefT<T3> item3)
	{
		item0 = _item0;
		item1 = _item1;
		item2 = _item2;
		item3 = _item3;
	}
}

[ExcludeFromCodeCoverage]
public readonly ref struct RefTuple<T0, T1, T2, T3, T4>
{
	public readonly Entity Entity;

	private readonly RefT<T0> _item0;
	public ref T0 Item0
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item0.Ref;
	}

	private readonly RefT<T1> _item1;
	public ref T1 Item1
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item1.Ref;
	}

	private readonly RefT<T2> _item2;
	public ref T2 Item2
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item2.Ref;
	}

	private readonly RefT<T3> _item3;
	public ref T3 Item3
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item3.Ref;
	}

	private readonly RefT<T4> _item4;
	public ref T4 Item4
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item4.Ref;
	}

	internal RefTuple(Entity entity, RefT<T0> item0, RefT<T1> item1, RefT<T2> item2, RefT<T3> item3, RefT<T4> item4)
	{
		Entity = entity;
		_item0 = item0;
		_item1 = item1;
		_item2 = item2;
		_item3 = item3;
		_item4 = item4;
	}

	public void Deconstruct(out Entity entity, out RefT<T0> item0, out RefT<T1> item1, out RefT<T2> item2, out RefT<T3> item3, out RefT<T4> item4)
	{
		entity = Entity;
		item0 = _item0;
		item1 = _item1;
		item2 = _item2;
		item3 = _item3;
		item4 = _item4;
	}

	public void Deconstruct(out RefT<T0> item0, out RefT<T1> item1, out RefT<T2> item2, out RefT<T3> item3, out RefT<T4> item4)
	{
		item0 = _item0;
		item1 = _item1;
		item2 = _item2;
		item3 = _item3;
		item4 = _item4;
	}
}

[ExcludeFromCodeCoverage]
public readonly ref struct RefTuple<T0, T1, T2, T3, T4, T5>
{
	public readonly Entity Entity;

	private readonly RefT<T0> _item0;
	public ref T0 Item0
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item0.Ref;
	}

	private readonly RefT<T1> _item1;
	public ref T1 Item1
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item1.Ref;
	}

	private readonly RefT<T2> _item2;
	public ref T2 Item2
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item2.Ref;
	}

	private readonly RefT<T3> _item3;
	public ref T3 Item3
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item3.Ref;
	}

	private readonly RefT<T4> _item4;
	public ref T4 Item4
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item4.Ref;
	}

	private readonly RefT<T5> _item5;
	public ref T5 Item5
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item5.Ref;
	}

	internal RefTuple(Entity entity, RefT<T0> item0, RefT<T1> item1, RefT<T2> item2, RefT<T3> item3, RefT<T4> item4, RefT<T5> item5)
	{
		Entity = entity;
		_item0 = item0;
		_item1 = item1;
		_item2 = item2;
		_item3 = item3;
		_item4 = item4;
		_item5 = item5;
	}

	public void Deconstruct(out Entity entity, out RefT<T0> item0, out RefT<T1> item1, out RefT<T2> item2, out RefT<T3> item3, out RefT<T4> item4, out RefT<T5> item5)
	{
		entity = Entity;
		item0 = _item0;
		item1 = _item1;
		item2 = _item2;
		item3 = _item3;
		item4 = _item4;
		item5 = _item5;
	}

	public void Deconstruct(out RefT<T0> item0, out RefT<T1> item1, out RefT<T2> item2, out RefT<T3> item3, out RefT<T4> item4, out RefT<T5> item5)
	{
		item0 = _item0;
		item1 = _item1;
		item2 = _item2;
		item3 = _item3;
		item4 = _item4;
		item5 = _item5;
	}
}

[ExcludeFromCodeCoverage]
public readonly ref struct RefTuple<T0, T1, T2, T3, T4, T5, T6>
{
	public readonly Entity Entity;

	private readonly RefT<T0> _item0;
	public ref T0 Item0
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item0.Ref;
	}

	private readonly RefT<T1> _item1;
	public ref T1 Item1
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item1.Ref;
	}

	private readonly RefT<T2> _item2;
	public ref T2 Item2
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item2.Ref;
	}

	private readonly RefT<T3> _item3;
	public ref T3 Item3
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item3.Ref;
	}

	private readonly RefT<T4> _item4;
	public ref T4 Item4
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item4.Ref;
	}

	private readonly RefT<T5> _item5;
	public ref T5 Item5
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item5.Ref;
	}

	private readonly RefT<T6> _item6;
	public ref T6 Item6
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item6.Ref;
	}

	internal RefTuple(Entity entity, RefT<T0> item0, RefT<T1> item1, RefT<T2> item2, RefT<T3> item3, RefT<T4> item4, RefT<T5> item5, RefT<T6> item6)
	{
		Entity = entity;
		_item0 = item0;
		_item1 = item1;
		_item2 = item2;
		_item3 = item3;
		_item4 = item4;
		_item5 = item5;
		_item6 = item6;
	}

	public void Deconstruct(out Entity entity, out RefT<T0> item0, out RefT<T1> item1, out RefT<T2> item2, out RefT<T3> item3, out RefT<T4> item4, out RefT<T5> item5, out RefT<T6> item6)
	{
		entity = Entity;
		item0 = _item0;
		item1 = _item1;
		item2 = _item2;
		item3 = _item3;
		item4 = _item4;
		item5 = _item5;
		item6 = _item6;
	}

	public void Deconstruct(out RefT<T0> item0, out RefT<T1> item1, out RefT<T2> item2, out RefT<T3> item3, out RefT<T4> item4, out RefT<T5> item5, out RefT<T6> item6)
	{
		item0 = _item0;
		item1 = _item1;
		item2 = _item2;
		item3 = _item3;
		item4 = _item4;
		item5 = _item5;
		item6 = _item6;
	}
}

[ExcludeFromCodeCoverage]
public readonly ref struct RefTuple<T0, T1, T2, T3, T4, T5, T6, T7>
{
	public readonly Entity Entity;

	private readonly RefT<T0> _item0;
	public ref T0 Item0
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item0.Ref;
	}

	private readonly RefT<T1> _item1;
	public ref T1 Item1
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item1.Ref;
	}

	private readonly RefT<T2> _item2;
	public ref T2 Item2
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item2.Ref;
	}

	private readonly RefT<T3> _item3;
	public ref T3 Item3
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item3.Ref;
	}

	private readonly RefT<T4> _item4;
	public ref T4 Item4
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item4.Ref;
	}

	private readonly RefT<T5> _item5;
	public ref T5 Item5
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item5.Ref;
	}

	private readonly RefT<T6> _item6;
	public ref T6 Item6
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item6.Ref;
	}

	private readonly RefT<T7> _item7;
	public ref T7 Item7
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item7.Ref;
	}

	internal RefTuple(Entity entity, RefT<T0> item0, RefT<T1> item1, RefT<T2> item2, RefT<T3> item3, RefT<T4> item4, RefT<T5> item5, RefT<T6> item6, RefT<T7> item7)
	{
		Entity = entity;
		_item0 = item0;
		_item1 = item1;
		_item2 = item2;
		_item3 = item3;
		_item4 = item4;
		_item5 = item5;
		_item6 = item6;
		_item7 = item7;
	}

	public void Deconstruct(out Entity entity, out RefT<T0> item0, out RefT<T1> item1, out RefT<T2> item2, out RefT<T3> item3, out RefT<T4> item4, out RefT<T5> item5, out RefT<T6> item6, out RefT<T7> item7)
	{
		entity = Entity;
		item0 = _item0;
		item1 = _item1;
		item2 = _item2;
		item3 = _item3;
		item4 = _item4;
		item5 = _item5;
		item6 = _item6;
		item7 = _item7;
	}

	public void Deconstruct(out RefT<T0> item0, out RefT<T1> item1, out RefT<T2> item2, out RefT<T3> item3, out RefT<T4> item4, out RefT<T5> item5, out RefT<T6> item6, out RefT<T7> item7)
	{
		item0 = _item0;
		item1 = _item1;
		item2 = _item2;
		item3 = _item3;
		item4 = _item4;
		item5 = _item5;
		item6 = _item6;
		item7 = _item7;
	}
}

[ExcludeFromCodeCoverage]
public readonly ref struct RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8>
{
	public readonly Entity Entity;

	private readonly RefT<T0> _item0;
	public ref T0 Item0
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item0.Ref;
	}

	private readonly RefT<T1> _item1;
	public ref T1 Item1
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item1.Ref;
	}

	private readonly RefT<T2> _item2;
	public ref T2 Item2
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item2.Ref;
	}

	private readonly RefT<T3> _item3;
	public ref T3 Item3
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item3.Ref;
	}

	private readonly RefT<T4> _item4;
	public ref T4 Item4
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item4.Ref;
	}

	private readonly RefT<T5> _item5;
	public ref T5 Item5
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item5.Ref;
	}

	private readonly RefT<T6> _item6;
	public ref T6 Item6
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item6.Ref;
	}

	private readonly RefT<T7> _item7;
	public ref T7 Item7
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item7.Ref;
	}

	private readonly RefT<T8> _item8;
	public ref T8 Item8
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item8.Ref;
	}

	internal RefTuple(Entity entity, RefT<T0> item0, RefT<T1> item1, RefT<T2> item2, RefT<T3> item3, RefT<T4> item4, RefT<T5> item5, RefT<T6> item6, RefT<T7> item7, RefT<T8> item8)
	{
		Entity = entity;
		_item0 = item0;
		_item1 = item1;
		_item2 = item2;
		_item3 = item3;
		_item4 = item4;
		_item5 = item5;
		_item6 = item6;
		_item7 = item7;
		_item8 = item8;
	}

	public void Deconstruct(out Entity entity, out RefT<T0> item0, out RefT<T1> item1, out RefT<T2> item2, out RefT<T3> item3, out RefT<T4> item4, out RefT<T5> item5, out RefT<T6> item6, out RefT<T7> item7, out RefT<T8> item8)
	{
		entity = Entity;
		item0 = _item0;
		item1 = _item1;
		item2 = _item2;
		item3 = _item3;
		item4 = _item4;
		item5 = _item5;
		item6 = _item6;
		item7 = _item7;
		item8 = _item8;
	}

	public void Deconstruct(out RefT<T0> item0, out RefT<T1> item1, out RefT<T2> item2, out RefT<T3> item3, out RefT<T4> item4, out RefT<T5> item5, out RefT<T6> item6, out RefT<T7> item7, out RefT<T8> item8)
	{
		item0 = _item0;
		item1 = _item1;
		item2 = _item2;
		item3 = _item3;
		item4 = _item4;
		item5 = _item5;
		item6 = _item6;
		item7 = _item7;
		item8 = _item8;
	}
}

[ExcludeFromCodeCoverage]
public readonly ref struct RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
{
	public readonly Entity Entity;

	private readonly RefT<T0> _item0;
	public ref T0 Item0
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item0.Ref;
	}

	private readonly RefT<T1> _item1;
	public ref T1 Item1
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item1.Ref;
	}

	private readonly RefT<T2> _item2;
	public ref T2 Item2
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item2.Ref;
	}

	private readonly RefT<T3> _item3;
	public ref T3 Item3
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item3.Ref;
	}

	private readonly RefT<T4> _item4;
	public ref T4 Item4
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item4.Ref;
	}

	private readonly RefT<T5> _item5;
	public ref T5 Item5
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item5.Ref;
	}

	private readonly RefT<T6> _item6;
	public ref T6 Item6
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item6.Ref;
	}

	private readonly RefT<T7> _item7;
	public ref T7 Item7
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item7.Ref;
	}

	private readonly RefT<T8> _item8;
	public ref T8 Item8
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item8.Ref;
	}

	private readonly RefT<T9> _item9;
	public ref T9 Item9
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item9.Ref;
	}

	internal RefTuple(Entity entity, RefT<T0> item0, RefT<T1> item1, RefT<T2> item2, RefT<T3> item3, RefT<T4> item4, RefT<T5> item5, RefT<T6> item6, RefT<T7> item7, RefT<T8> item8, RefT<T9> item9)
	{
		Entity = entity;
		_item0 = item0;
		_item1 = item1;
		_item2 = item2;
		_item3 = item3;
		_item4 = item4;
		_item5 = item5;
		_item6 = item6;
		_item7 = item7;
		_item8 = item8;
		_item9 = item9;
	}

	public void Deconstruct(out Entity entity, out RefT<T0> item0, out RefT<T1> item1, out RefT<T2> item2, out RefT<T3> item3, out RefT<T4> item4, out RefT<T5> item5, out RefT<T6> item6, out RefT<T7> item7, out RefT<T8> item8, out RefT<T9> item9)
	{
		entity = Entity;
		item0 = _item0;
		item1 = _item1;
		item2 = _item2;
		item3 = _item3;
		item4 = _item4;
		item5 = _item5;
		item6 = _item6;
		item7 = _item7;
		item8 = _item8;
		item9 = _item9;
	}

	public void Deconstruct(out RefT<T0> item0, out RefT<T1> item1, out RefT<T2> item2, out RefT<T3> item3, out RefT<T4> item4, out RefT<T5> item5, out RefT<T6> item6, out RefT<T7> item7, out RefT<T8> item8, out RefT<T9> item9)
	{
		item0 = _item0;
		item1 = _item1;
		item2 = _item2;
		item3 = _item3;
		item4 = _item4;
		item5 = _item5;
		item6 = _item6;
		item7 = _item7;
		item8 = _item8;
		item9 = _item9;
	}
}

[ExcludeFromCodeCoverage]
public readonly ref struct RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
{
	public readonly Entity Entity;

	private readonly RefT<T0> _item0;
	public ref T0 Item0
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item0.Ref;
	}

	private readonly RefT<T1> _item1;
	public ref T1 Item1
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item1.Ref;
	}

	private readonly RefT<T2> _item2;
	public ref T2 Item2
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item2.Ref;
	}

	private readonly RefT<T3> _item3;
	public ref T3 Item3
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item3.Ref;
	}

	private readonly RefT<T4> _item4;
	public ref T4 Item4
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item4.Ref;
	}

	private readonly RefT<T5> _item5;
	public ref T5 Item5
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item5.Ref;
	}

	private readonly RefT<T6> _item6;
	public ref T6 Item6
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item6.Ref;
	}

	private readonly RefT<T7> _item7;
	public ref T7 Item7
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item7.Ref;
	}

	private readonly RefT<T8> _item8;
	public ref T8 Item8
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item8.Ref;
	}

	private readonly RefT<T9> _item9;
	public ref T9 Item9
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item9.Ref;
	}

	private readonly RefT<T10> _item10;
	public ref T10 Item10
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item10.Ref;
	}

	internal RefTuple(Entity entity, RefT<T0> item0, RefT<T1> item1, RefT<T2> item2, RefT<T3> item3, RefT<T4> item4, RefT<T5> item5, RefT<T6> item6, RefT<T7> item7, RefT<T8> item8, RefT<T9> item9, RefT<T10> item10)
	{
		Entity = entity;
		_item0 = item0;
		_item1 = item1;
		_item2 = item2;
		_item3 = item3;
		_item4 = item4;
		_item5 = item5;
		_item6 = item6;
		_item7 = item7;
		_item8 = item8;
		_item9 = item9;
		_item10 = item10;
	}

	public void Deconstruct(out Entity entity, out RefT<T0> item0, out RefT<T1> item1, out RefT<T2> item2, out RefT<T3> item3, out RefT<T4> item4, out RefT<T5> item5, out RefT<T6> item6, out RefT<T7> item7, out RefT<T8> item8, out RefT<T9> item9, out RefT<T10> item10)
	{
		entity = Entity;
		item0 = _item0;
		item1 = _item1;
		item2 = _item2;
		item3 = _item3;
		item4 = _item4;
		item5 = _item5;
		item6 = _item6;
		item7 = _item7;
		item8 = _item8;
		item9 = _item9;
		item10 = _item10;
	}

	public void Deconstruct(out RefT<T0> item0, out RefT<T1> item1, out RefT<T2> item2, out RefT<T3> item3, out RefT<T4> item4, out RefT<T5> item5, out RefT<T6> item6, out RefT<T7> item7, out RefT<T8> item8, out RefT<T9> item9, out RefT<T10> item10)
	{
		item0 = _item0;
		item1 = _item1;
		item2 = _item2;
		item3 = _item3;
		item4 = _item4;
		item5 = _item5;
		item6 = _item6;
		item7 = _item7;
		item8 = _item8;
		item9 = _item9;
		item10 = _item10;
	}
}

[ExcludeFromCodeCoverage]
public readonly ref struct RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
{
	public readonly Entity Entity;

	private readonly RefT<T0> _item0;
	public ref T0 Item0
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item0.Ref;
	}

	private readonly RefT<T1> _item1;
	public ref T1 Item1
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item1.Ref;
	}

	private readonly RefT<T2> _item2;
	public ref T2 Item2
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item2.Ref;
	}

	private readonly RefT<T3> _item3;
	public ref T3 Item3
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item3.Ref;
	}

	private readonly RefT<T4> _item4;
	public ref T4 Item4
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item4.Ref;
	}

	private readonly RefT<T5> _item5;
	public ref T5 Item5
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item5.Ref;
	}

	private readonly RefT<T6> _item6;
	public ref T6 Item6
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item6.Ref;
	}

	private readonly RefT<T7> _item7;
	public ref T7 Item7
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item7.Ref;
	}

	private readonly RefT<T8> _item8;
	public ref T8 Item8
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item8.Ref;
	}

	private readonly RefT<T9> _item9;
	public ref T9 Item9
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item9.Ref;
	}

	private readonly RefT<T10> _item10;
	public ref T10 Item10
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item10.Ref;
	}

	private readonly RefT<T11> _item11;
	public ref T11 Item11
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item11.Ref;
	}

	internal RefTuple(Entity entity, RefT<T0> item0, RefT<T1> item1, RefT<T2> item2, RefT<T3> item3, RefT<T4> item4, RefT<T5> item5, RefT<T6> item6, RefT<T7> item7, RefT<T8> item8, RefT<T9> item9, RefT<T10> item10, RefT<T11> item11)
	{
		Entity = entity;
		_item0 = item0;
		_item1 = item1;
		_item2 = item2;
		_item3 = item3;
		_item4 = item4;
		_item5 = item5;
		_item6 = item6;
		_item7 = item7;
		_item8 = item8;
		_item9 = item9;
		_item10 = item10;
		_item11 = item11;
	}

	public void Deconstruct(out Entity entity, out RefT<T0> item0, out RefT<T1> item1, out RefT<T2> item2, out RefT<T3> item3, out RefT<T4> item4, out RefT<T5> item5, out RefT<T6> item6, out RefT<T7> item7, out RefT<T8> item8, out RefT<T9> item9, out RefT<T10> item10, out RefT<T11> item11)
	{
		entity = Entity;
		item0 = _item0;
		item1 = _item1;
		item2 = _item2;
		item3 = _item3;
		item4 = _item4;
		item5 = _item5;
		item6 = _item6;
		item7 = _item7;
		item8 = _item8;
		item9 = _item9;
		item10 = _item10;
		item11 = _item11;
	}

	public void Deconstruct(out RefT<T0> item0, out RefT<T1> item1, out RefT<T2> item2, out RefT<T3> item3, out RefT<T4> item4, out RefT<T5> item5, out RefT<T6> item6, out RefT<T7> item7, out RefT<T8> item8, out RefT<T9> item9, out RefT<T10> item10, out RefT<T11> item11)
	{
		item0 = _item0;
		item1 = _item1;
		item2 = _item2;
		item3 = _item3;
		item4 = _item4;
		item5 = _item5;
		item6 = _item6;
		item7 = _item7;
		item8 = _item8;
		item9 = _item9;
		item10 = _item10;
		item11 = _item11;
	}
}

[ExcludeFromCodeCoverage]
public readonly ref struct RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
{
	public readonly Entity Entity;

	private readonly RefT<T0> _item0;
	public ref T0 Item0
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item0.Ref;
	}

	private readonly RefT<T1> _item1;
	public ref T1 Item1
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item1.Ref;
	}

	private readonly RefT<T2> _item2;
	public ref T2 Item2
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item2.Ref;
	}

	private readonly RefT<T3> _item3;
	public ref T3 Item3
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item3.Ref;
	}

	private readonly RefT<T4> _item4;
	public ref T4 Item4
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item4.Ref;
	}

	private readonly RefT<T5> _item5;
	public ref T5 Item5
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item5.Ref;
	}

	private readonly RefT<T6> _item6;
	public ref T6 Item6
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item6.Ref;
	}

	private readonly RefT<T7> _item7;
	public ref T7 Item7
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item7.Ref;
	}

	private readonly RefT<T8> _item8;
	public ref T8 Item8
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item8.Ref;
	}

	private readonly RefT<T9> _item9;
	public ref T9 Item9
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item9.Ref;
	}

	private readonly RefT<T10> _item10;
	public ref T10 Item10
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item10.Ref;
	}

	private readonly RefT<T11> _item11;
	public ref T11 Item11
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item11.Ref;
	}

	private readonly RefT<T12> _item12;
	public ref T12 Item12
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item12.Ref;
	}

	internal RefTuple(Entity entity, RefT<T0> item0, RefT<T1> item1, RefT<T2> item2, RefT<T3> item3, RefT<T4> item4, RefT<T5> item5, RefT<T6> item6, RefT<T7> item7, RefT<T8> item8, RefT<T9> item9, RefT<T10> item10, RefT<T11> item11, RefT<T12> item12)
	{
		Entity = entity;
		_item0 = item0;
		_item1 = item1;
		_item2 = item2;
		_item3 = item3;
		_item4 = item4;
		_item5 = item5;
		_item6 = item6;
		_item7 = item7;
		_item8 = item8;
		_item9 = item9;
		_item10 = item10;
		_item11 = item11;
		_item12 = item12;
	}

	public void Deconstruct(out Entity entity, out RefT<T0> item0, out RefT<T1> item1, out RefT<T2> item2, out RefT<T3> item3, out RefT<T4> item4, out RefT<T5> item5, out RefT<T6> item6, out RefT<T7> item7, out RefT<T8> item8, out RefT<T9> item9, out RefT<T10> item10, out RefT<T11> item11, out RefT<T12> item12)
	{
		entity = Entity;
		item0 = _item0;
		item1 = _item1;
		item2 = _item2;
		item3 = _item3;
		item4 = _item4;
		item5 = _item5;
		item6 = _item6;
		item7 = _item7;
		item8 = _item8;
		item9 = _item9;
		item10 = _item10;
		item11 = _item11;
		item12 = _item12;
	}

	public void Deconstruct(out RefT<T0> item0, out RefT<T1> item1, out RefT<T2> item2, out RefT<T3> item3, out RefT<T4> item4, out RefT<T5> item5, out RefT<T6> item6, out RefT<T7> item7, out RefT<T8> item8, out RefT<T9> item9, out RefT<T10> item10, out RefT<T11> item11, out RefT<T12> item12)
	{
		item0 = _item0;
		item1 = _item1;
		item2 = _item2;
		item3 = _item3;
		item4 = _item4;
		item5 = _item5;
		item6 = _item6;
		item7 = _item7;
		item8 = _item8;
		item9 = _item9;
		item10 = _item10;
		item11 = _item11;
		item12 = _item12;
	}
}

[ExcludeFromCodeCoverage]
public readonly ref struct RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
{
	public readonly Entity Entity;

	private readonly RefT<T0> _item0;
	public ref T0 Item0
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item0.Ref;
	}

	private readonly RefT<T1> _item1;
	public ref T1 Item1
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item1.Ref;
	}

	private readonly RefT<T2> _item2;
	public ref T2 Item2
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item2.Ref;
	}

	private readonly RefT<T3> _item3;
	public ref T3 Item3
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item3.Ref;
	}

	private readonly RefT<T4> _item4;
	public ref T4 Item4
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item4.Ref;
	}

	private readonly RefT<T5> _item5;
	public ref T5 Item5
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item5.Ref;
	}

	private readonly RefT<T6> _item6;
	public ref T6 Item6
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item6.Ref;
	}

	private readonly RefT<T7> _item7;
	public ref T7 Item7
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item7.Ref;
	}

	private readonly RefT<T8> _item8;
	public ref T8 Item8
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item8.Ref;
	}

	private readonly RefT<T9> _item9;
	public ref T9 Item9
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item9.Ref;
	}

	private readonly RefT<T10> _item10;
	public ref T10 Item10
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item10.Ref;
	}

	private readonly RefT<T11> _item11;
	public ref T11 Item11
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item11.Ref;
	}

	private readonly RefT<T12> _item12;
	public ref T12 Item12
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item12.Ref;
	}

	private readonly RefT<T13> _item13;
	public ref T13 Item13
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item13.Ref;
	}

	internal RefTuple(Entity entity, RefT<T0> item0, RefT<T1> item1, RefT<T2> item2, RefT<T3> item3, RefT<T4> item4, RefT<T5> item5, RefT<T6> item6, RefT<T7> item7, RefT<T8> item8, RefT<T9> item9, RefT<T10> item10, RefT<T11> item11, RefT<T12> item12, RefT<T13> item13)
	{
		Entity = entity;
		_item0 = item0;
		_item1 = item1;
		_item2 = item2;
		_item3 = item3;
		_item4 = item4;
		_item5 = item5;
		_item6 = item6;
		_item7 = item7;
		_item8 = item8;
		_item9 = item9;
		_item10 = item10;
		_item11 = item11;
		_item12 = item12;
		_item13 = item13;
	}

	public void Deconstruct(out Entity entity, out RefT<T0> item0, out RefT<T1> item1, out RefT<T2> item2, out RefT<T3> item3, out RefT<T4> item4, out RefT<T5> item5, out RefT<T6> item6, out RefT<T7> item7, out RefT<T8> item8, out RefT<T9> item9, out RefT<T10> item10, out RefT<T11> item11, out RefT<T12> item12, out RefT<T13> item13)
	{
		entity = Entity;
		item0 = _item0;
		item1 = _item1;
		item2 = _item2;
		item3 = _item3;
		item4 = _item4;
		item5 = _item5;
		item6 = _item6;
		item7 = _item7;
		item8 = _item8;
		item9 = _item9;
		item10 = _item10;
		item11 = _item11;
		item12 = _item12;
		item13 = _item13;
	}

	public void Deconstruct(out RefT<T0> item0, out RefT<T1> item1, out RefT<T2> item2, out RefT<T3> item3, out RefT<T4> item4, out RefT<T5> item5, out RefT<T6> item6, out RefT<T7> item7, out RefT<T8> item8, out RefT<T9> item9, out RefT<T10> item10, out RefT<T11> item11, out RefT<T12> item12, out RefT<T13> item13)
	{
		item0 = _item0;
		item1 = _item1;
		item2 = _item2;
		item3 = _item3;
		item4 = _item4;
		item5 = _item5;
		item6 = _item6;
		item7 = _item7;
		item8 = _item8;
		item9 = _item9;
		item10 = _item10;
		item11 = _item11;
		item12 = _item12;
		item13 = _item13;
	}
}

[ExcludeFromCodeCoverage]
public readonly ref struct RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
{
	public readonly Entity Entity;

	private readonly RefT<T0> _item0;
	public ref T0 Item0
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item0.Ref;
	}

	private readonly RefT<T1> _item1;
	public ref T1 Item1
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item1.Ref;
	}

	private readonly RefT<T2> _item2;
	public ref T2 Item2
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item2.Ref;
	}

	private readonly RefT<T3> _item3;
	public ref T3 Item3
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item3.Ref;
	}

	private readonly RefT<T4> _item4;
	public ref T4 Item4
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item4.Ref;
	}

	private readonly RefT<T5> _item5;
	public ref T5 Item5
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item5.Ref;
	}

	private readonly RefT<T6> _item6;
	public ref T6 Item6
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item6.Ref;
	}

	private readonly RefT<T7> _item7;
	public ref T7 Item7
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item7.Ref;
	}

	private readonly RefT<T8> _item8;
	public ref T8 Item8
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item8.Ref;
	}

	private readonly RefT<T9> _item9;
	public ref T9 Item9
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item9.Ref;
	}

	private readonly RefT<T10> _item10;
	public ref T10 Item10
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item10.Ref;
	}

	private readonly RefT<T11> _item11;
	public ref T11 Item11
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item11.Ref;
	}

	private readonly RefT<T12> _item12;
	public ref T12 Item12
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item12.Ref;
	}

	private readonly RefT<T13> _item13;
	public ref T13 Item13
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item13.Ref;
	}

	private readonly RefT<T14> _item14;
	public ref T14 Item14
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item14.Ref;
	}

	internal RefTuple(Entity entity, RefT<T0> item0, RefT<T1> item1, RefT<T2> item2, RefT<T3> item3, RefT<T4> item4, RefT<T5> item5, RefT<T6> item6, RefT<T7> item7, RefT<T8> item8, RefT<T9> item9, RefT<T10> item10, RefT<T11> item11, RefT<T12> item12, RefT<T13> item13, RefT<T14> item14)
	{
		Entity = entity;
		_item0 = item0;
		_item1 = item1;
		_item2 = item2;
		_item3 = item3;
		_item4 = item4;
		_item5 = item5;
		_item6 = item6;
		_item7 = item7;
		_item8 = item8;
		_item9 = item9;
		_item10 = item10;
		_item11 = item11;
		_item12 = item12;
		_item13 = item13;
		_item14 = item14;
	}

	public void Deconstruct(out Entity entity, out RefT<T0> item0, out RefT<T1> item1, out RefT<T2> item2, out RefT<T3> item3, out RefT<T4> item4, out RefT<T5> item5, out RefT<T6> item6, out RefT<T7> item7, out RefT<T8> item8, out RefT<T9> item9, out RefT<T10> item10, out RefT<T11> item11, out RefT<T12> item12, out RefT<T13> item13, out RefT<T14> item14)
	{
		entity = Entity;
		item0 = _item0;
		item1 = _item1;
		item2 = _item2;
		item3 = _item3;
		item4 = _item4;
		item5 = _item5;
		item6 = _item6;
		item7 = _item7;
		item8 = _item8;
		item9 = _item9;
		item10 = _item10;
		item11 = _item11;
		item12 = _item12;
		item13 = _item13;
		item14 = _item14;
	}

	public void Deconstruct(out RefT<T0> item0, out RefT<T1> item1, out RefT<T2> item2, out RefT<T3> item3, out RefT<T4> item4, out RefT<T5> item5, out RefT<T6> item6, out RefT<T7> item7, out RefT<T8> item8, out RefT<T9> item9, out RefT<T10> item10, out RefT<T11> item11, out RefT<T12> item12, out RefT<T13> item13, out RefT<T14> item14)
	{
		item0 = _item0;
		item1 = _item1;
		item2 = _item2;
		item3 = _item3;
		item4 = _item4;
		item5 = _item5;
		item6 = _item6;
		item7 = _item7;
		item8 = _item8;
		item9 = _item9;
		item10 = _item10;
		item11 = _item11;
		item12 = _item12;
		item13 = _item13;
		item14 = _item14;
	}
}

[ExcludeFromCodeCoverage]
public readonly ref struct RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
{
	public readonly Entity Entity;

	private readonly RefT<T0> _item0;
	public ref T0 Item0
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item0.Ref;
	}

	private readonly RefT<T1> _item1;
	public ref T1 Item1
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item1.Ref;
	}

	private readonly RefT<T2> _item2;
	public ref T2 Item2
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item2.Ref;
	}

	private readonly RefT<T3> _item3;
	public ref T3 Item3
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item3.Ref;
	}

	private readonly RefT<T4> _item4;
	public ref T4 Item4
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item4.Ref;
	}

	private readonly RefT<T5> _item5;
	public ref T5 Item5
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item5.Ref;
	}

	private readonly RefT<T6> _item6;
	public ref T6 Item6
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item6.Ref;
	}

	private readonly RefT<T7> _item7;
	public ref T7 Item7
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item7.Ref;
	}

	private readonly RefT<T8> _item8;
	public ref T8 Item8
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item8.Ref;
	}

	private readonly RefT<T9> _item9;
	public ref T9 Item9
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item9.Ref;
	}

	private readonly RefT<T10> _item10;
	public ref T10 Item10
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item10.Ref;
	}

	private readonly RefT<T11> _item11;
	public ref T11 Item11
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item11.Ref;
	}

	private readonly RefT<T12> _item12;
	public ref T12 Item12
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item12.Ref;
	}

	private readonly RefT<T13> _item13;
	public ref T13 Item13
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item13.Ref;
	}

	private readonly RefT<T14> _item14;
	public ref T14 Item14
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item14.Ref;
	}

	private readonly RefT<T15> _item15;
	public ref T15 Item15
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _item15.Ref;
	}

	internal RefTuple(Entity entity, RefT<T0> item0, RefT<T1> item1, RefT<T2> item2, RefT<T3> item3, RefT<T4> item4, RefT<T5> item5, RefT<T6> item6, RefT<T7> item7, RefT<T8> item8, RefT<T9> item9, RefT<T10> item10, RefT<T11> item11, RefT<T12> item12, RefT<T13> item13, RefT<T14> item14, RefT<T15> item15)
	{
		Entity = entity;
		_item0 = item0;
		_item1 = item1;
		_item2 = item2;
		_item3 = item3;
		_item4 = item4;
		_item5 = item5;
		_item6 = item6;
		_item7 = item7;
		_item8 = item8;
		_item9 = item9;
		_item10 = item10;
		_item11 = item11;
		_item12 = item12;
		_item13 = item13;
		_item14 = item14;
		_item15 = item15;
	}

	public void Deconstruct(out Entity entity, out RefT<T0> item0, out RefT<T1> item1, out RefT<T2> item2, out RefT<T3> item3, out RefT<T4> item4, out RefT<T5> item5, out RefT<T6> item6, out RefT<T7> item7, out RefT<T8> item8, out RefT<T9> item9, out RefT<T10> item10, out RefT<T11> item11, out RefT<T12> item12, out RefT<T13> item13, out RefT<T14> item14, out RefT<T15> item15)
	{
		entity = Entity;
		item0 = _item0;
		item1 = _item1;
		item2 = _item2;
		item3 = _item3;
		item4 = _item4;
		item5 = _item5;
		item6 = _item6;
		item7 = _item7;
		item8 = _item8;
		item9 = _item9;
		item10 = _item10;
		item11 = _item11;
		item12 = _item12;
		item13 = _item13;
		item14 = _item14;
		item15 = _item15;
	}

	public void Deconstruct(out RefT<T0> item0, out RefT<T1> item1, out RefT<T2> item2, out RefT<T3> item3, out RefT<T4> item4, out RefT<T5> item5, out RefT<T6> item6, out RefT<T7> item7, out RefT<T8> item8, out RefT<T9> item9, out RefT<T10> item10, out RefT<T11> item11, out RefT<T12> item12, out RefT<T13> item13, out RefT<T14> item14, out RefT<T15> item15)
	{
		item0 = _item0;
		item1 = _item1;
		item2 = _item2;
		item3 = _item3;
		item4 = _item4;
		item5 = _item5;
		item6 = _item6;
		item7 = _item7;
		item8 = _item8;
		item9 = _item9;
		item10 = _item10;
		item11 = _item11;
		item12 = _item12;
		item13 = _item13;
		item14 = _item14;
		item15 = _item15;
	}
}


