
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedParameter.Global
// ReSharper disable LoopCanBeConvertedToQuery
// ReSharper disable CheckNamespace
// ReSharper disable ArrangeAccessorOwnerBody

namespace Myriad.ECS.Queries;

public ref struct Ref<T>
{
	public ref T Item;
}

public readonly ref struct RefTuple1<T0>
{
	public readonly Entity Entity;

	public readonly ref T0 Item0;

	internal RefTuple1(Entity entity, ref T0 item0)
	{
		Entity = entity;
		Item0 = ref item0;
	}

	public void Deconstruct(out Entity entity, out Ref<T0> item0)
	{
		entity = Entity;
		item0 = new Ref<T0> { Item = ref Item0 };
	}
}

public readonly ref struct RefTuple2<T0, T1>
{
	public readonly Entity Entity;

	public readonly ref T0 Item0;
	public readonly ref T1 Item1;

	internal RefTuple2(Entity entity, ref T0 item0, ref T1 item1)
	{
		Entity = entity;
		Item0 = ref item0;
		Item1 = ref item1;
	}

	public void Deconstruct(out Entity entity, out Ref<T0> item0, out Ref<T1> item1)
	{
		entity = Entity;
		item0 = new Ref<T0> { Item = ref Item0 };
		item1 = new Ref<T1> { Item = ref Item1 };
	}
}

public readonly ref struct RefTuple3<T0, T1, T2>
{
	public readonly Entity Entity;

	public readonly ref T0 Item0;
	public readonly ref T1 Item1;
	public readonly ref T2 Item2;

	internal RefTuple3(Entity entity, ref T0 item0, ref T1 item1, ref T2 item2)
	{
		Entity = entity;
		Item0 = ref item0;
		Item1 = ref item1;
		Item2 = ref item2;
	}

	public void Deconstruct(out Entity entity, out Ref<T0> item0, out Ref<T1> item1, out Ref<T2> item2)
	{
		entity = Entity;
		item0 = new Ref<T0> { Item = ref Item0 };
		item1 = new Ref<T1> { Item = ref Item1 };
		item2 = new Ref<T2> { Item = ref Item2 };
	}
}

public readonly ref struct RefTuple4<T0, T1, T2, T3>
{
	public readonly Entity Entity;

	public readonly ref T0 Item0;
	public readonly ref T1 Item1;
	public readonly ref T2 Item2;
	public readonly ref T3 Item3;

	internal RefTuple4(Entity entity, ref T0 item0, ref T1 item1, ref T2 item2, ref T3 item3)
	{
		Entity = entity;
		Item0 = ref item0;
		Item1 = ref item1;
		Item2 = ref item2;
		Item3 = ref item3;
	}

	public void Deconstruct(out Entity entity, out Ref<T0> item0, out Ref<T1> item1, out Ref<T2> item2, out Ref<T3> item3)
	{
		entity = Entity;
		item0 = new Ref<T0> { Item = ref Item0 };
		item1 = new Ref<T1> { Item = ref Item1 };
		item2 = new Ref<T2> { Item = ref Item2 };
		item3 = new Ref<T3> { Item = ref Item3 };
	}
}

public readonly ref struct RefTuple5<T0, T1, T2, T3, T4>
{
	public readonly Entity Entity;

	public readonly ref T0 Item0;
	public readonly ref T1 Item1;
	public readonly ref T2 Item2;
	public readonly ref T3 Item3;
	public readonly ref T4 Item4;

	internal RefTuple5(Entity entity, ref T0 item0, ref T1 item1, ref T2 item2, ref T3 item3, ref T4 item4)
	{
		Entity = entity;
		Item0 = ref item0;
		Item1 = ref item1;
		Item2 = ref item2;
		Item3 = ref item3;
		Item4 = ref item4;
	}

	public void Deconstruct(out Entity entity, out Ref<T0> item0, out Ref<T1> item1, out Ref<T2> item2, out Ref<T3> item3, out Ref<T4> item4)
	{
		entity = Entity;
		item0 = new Ref<T0> { Item = ref Item0 };
		item1 = new Ref<T1> { Item = ref Item1 };
		item2 = new Ref<T2> { Item = ref Item2 };
		item3 = new Ref<T3> { Item = ref Item3 };
		item4 = new Ref<T4> { Item = ref Item4 };
	}
}

public readonly ref struct RefTuple6<T0, T1, T2, T3, T4, T5>
{
	public readonly Entity Entity;

	public readonly ref T0 Item0;
	public readonly ref T1 Item1;
	public readonly ref T2 Item2;
	public readonly ref T3 Item3;
	public readonly ref T4 Item4;
	public readonly ref T5 Item5;

	internal RefTuple6(Entity entity, ref T0 item0, ref T1 item1, ref T2 item2, ref T3 item3, ref T4 item4, ref T5 item5)
	{
		Entity = entity;
		Item0 = ref item0;
		Item1 = ref item1;
		Item2 = ref item2;
		Item3 = ref item3;
		Item4 = ref item4;
		Item5 = ref item5;
	}

	public void Deconstruct(out Entity entity, out Ref<T0> item0, out Ref<T1> item1, out Ref<T2> item2, out Ref<T3> item3, out Ref<T4> item4, out Ref<T5> item5)
	{
		entity = Entity;
		item0 = new Ref<T0> { Item = ref Item0 };
		item1 = new Ref<T1> { Item = ref Item1 };
		item2 = new Ref<T2> { Item = ref Item2 };
		item3 = new Ref<T3> { Item = ref Item3 };
		item4 = new Ref<T4> { Item = ref Item4 };
		item5 = new Ref<T5> { Item = ref Item5 };
	}
}

public readonly ref struct RefTuple7<T0, T1, T2, T3, T4, T5, T6>
{
	public readonly Entity Entity;

	public readonly ref T0 Item0;
	public readonly ref T1 Item1;
	public readonly ref T2 Item2;
	public readonly ref T3 Item3;
	public readonly ref T4 Item4;
	public readonly ref T5 Item5;
	public readonly ref T6 Item6;

	internal RefTuple7(Entity entity, ref T0 item0, ref T1 item1, ref T2 item2, ref T3 item3, ref T4 item4, ref T5 item5, ref T6 item6)
	{
		Entity = entity;
		Item0 = ref item0;
		Item1 = ref item1;
		Item2 = ref item2;
		Item3 = ref item3;
		Item4 = ref item4;
		Item5 = ref item5;
		Item6 = ref item6;
	}

	public void Deconstruct(out Entity entity, out Ref<T0> item0, out Ref<T1> item1, out Ref<T2> item2, out Ref<T3> item3, out Ref<T4> item4, out Ref<T5> item5, out Ref<T6> item6)
	{
		entity = Entity;
		item0 = new Ref<T0> { Item = ref Item0 };
		item1 = new Ref<T1> { Item = ref Item1 };
		item2 = new Ref<T2> { Item = ref Item2 };
		item3 = new Ref<T3> { Item = ref Item3 };
		item4 = new Ref<T4> { Item = ref Item4 };
		item5 = new Ref<T5> { Item = ref Item5 };
		item6 = new Ref<T6> { Item = ref Item6 };
	}
}

public readonly ref struct RefTuple8<T0, T1, T2, T3, T4, T5, T6, T7>
{
	public readonly Entity Entity;

	public readonly ref T0 Item0;
	public readonly ref T1 Item1;
	public readonly ref T2 Item2;
	public readonly ref T3 Item3;
	public readonly ref T4 Item4;
	public readonly ref T5 Item5;
	public readonly ref T6 Item6;
	public readonly ref T7 Item7;

	internal RefTuple8(Entity entity, ref T0 item0, ref T1 item1, ref T2 item2, ref T3 item3, ref T4 item4, ref T5 item5, ref T6 item6, ref T7 item7)
	{
		Entity = entity;
		Item0 = ref item0;
		Item1 = ref item1;
		Item2 = ref item2;
		Item3 = ref item3;
		Item4 = ref item4;
		Item5 = ref item5;
		Item6 = ref item6;
		Item7 = ref item7;
	}

	public void Deconstruct(out Entity entity, out Ref<T0> item0, out Ref<T1> item1, out Ref<T2> item2, out Ref<T3> item3, out Ref<T4> item4, out Ref<T5> item5, out Ref<T6> item6, out Ref<T7> item7)
	{
		entity = Entity;
		item0 = new Ref<T0> { Item = ref Item0 };
		item1 = new Ref<T1> { Item = ref Item1 };
		item2 = new Ref<T2> { Item = ref Item2 };
		item3 = new Ref<T3> { Item = ref Item3 };
		item4 = new Ref<T4> { Item = ref Item4 };
		item5 = new Ref<T5> { Item = ref Item5 };
		item6 = new Ref<T6> { Item = ref Item6 };
		item7 = new Ref<T7> { Item = ref Item7 };
	}
}

public readonly ref struct RefTuple9<T0, T1, T2, T3, T4, T5, T6, T7, T8>
{
	public readonly Entity Entity;

	public readonly ref T0 Item0;
	public readonly ref T1 Item1;
	public readonly ref T2 Item2;
	public readonly ref T3 Item3;
	public readonly ref T4 Item4;
	public readonly ref T5 Item5;
	public readonly ref T6 Item6;
	public readonly ref T7 Item7;
	public readonly ref T8 Item8;

	internal RefTuple9(Entity entity, ref T0 item0, ref T1 item1, ref T2 item2, ref T3 item3, ref T4 item4, ref T5 item5, ref T6 item6, ref T7 item7, ref T8 item8)
	{
		Entity = entity;
		Item0 = ref item0;
		Item1 = ref item1;
		Item2 = ref item2;
		Item3 = ref item3;
		Item4 = ref item4;
		Item5 = ref item5;
		Item6 = ref item6;
		Item7 = ref item7;
		Item8 = ref item8;
	}

	public void Deconstruct(out Entity entity, out Ref<T0> item0, out Ref<T1> item1, out Ref<T2> item2, out Ref<T3> item3, out Ref<T4> item4, out Ref<T5> item5, out Ref<T6> item6, out Ref<T7> item7, out Ref<T8> item8)
	{
		entity = Entity;
		item0 = new Ref<T0> { Item = ref Item0 };
		item1 = new Ref<T1> { Item = ref Item1 };
		item2 = new Ref<T2> { Item = ref Item2 };
		item3 = new Ref<T3> { Item = ref Item3 };
		item4 = new Ref<T4> { Item = ref Item4 };
		item5 = new Ref<T5> { Item = ref Item5 };
		item6 = new Ref<T6> { Item = ref Item6 };
		item7 = new Ref<T7> { Item = ref Item7 };
		item8 = new Ref<T8> { Item = ref Item8 };
	}
}

public readonly ref struct RefTuple10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
{
	public readonly Entity Entity;

	public readonly ref T0 Item0;
	public readonly ref T1 Item1;
	public readonly ref T2 Item2;
	public readonly ref T3 Item3;
	public readonly ref T4 Item4;
	public readonly ref T5 Item5;
	public readonly ref T6 Item6;
	public readonly ref T7 Item7;
	public readonly ref T8 Item8;
	public readonly ref T9 Item9;

	internal RefTuple10(Entity entity, ref T0 item0, ref T1 item1, ref T2 item2, ref T3 item3, ref T4 item4, ref T5 item5, ref T6 item6, ref T7 item7, ref T8 item8, ref T9 item9)
	{
		Entity = entity;
		Item0 = ref item0;
		Item1 = ref item1;
		Item2 = ref item2;
		Item3 = ref item3;
		Item4 = ref item4;
		Item5 = ref item5;
		Item6 = ref item6;
		Item7 = ref item7;
		Item8 = ref item8;
		Item9 = ref item9;
	}

	public void Deconstruct(out Entity entity, out Ref<T0> item0, out Ref<T1> item1, out Ref<T2> item2, out Ref<T3> item3, out Ref<T4> item4, out Ref<T5> item5, out Ref<T6> item6, out Ref<T7> item7, out Ref<T8> item8, out Ref<T9> item9)
	{
		entity = Entity;
		item0 = new Ref<T0> { Item = ref Item0 };
		item1 = new Ref<T1> { Item = ref Item1 };
		item2 = new Ref<T2> { Item = ref Item2 };
		item3 = new Ref<T3> { Item = ref Item3 };
		item4 = new Ref<T4> { Item = ref Item4 };
		item5 = new Ref<T5> { Item = ref Item5 };
		item6 = new Ref<T6> { Item = ref Item6 };
		item7 = new Ref<T7> { Item = ref Item7 };
		item8 = new Ref<T8> { Item = ref Item8 };
		item9 = new Ref<T9> { Item = ref Item9 };
	}
}

public readonly ref struct RefTuple11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
{
	public readonly Entity Entity;

	public readonly ref T0 Item0;
	public readonly ref T1 Item1;
	public readonly ref T2 Item2;
	public readonly ref T3 Item3;
	public readonly ref T4 Item4;
	public readonly ref T5 Item5;
	public readonly ref T6 Item6;
	public readonly ref T7 Item7;
	public readonly ref T8 Item8;
	public readonly ref T9 Item9;
	public readonly ref T10 Item10;

	internal RefTuple11(Entity entity, ref T0 item0, ref T1 item1, ref T2 item2, ref T3 item3, ref T4 item4, ref T5 item5, ref T6 item6, ref T7 item7, ref T8 item8, ref T9 item9, ref T10 item10)
	{
		Entity = entity;
		Item0 = ref item0;
		Item1 = ref item1;
		Item2 = ref item2;
		Item3 = ref item3;
		Item4 = ref item4;
		Item5 = ref item5;
		Item6 = ref item6;
		Item7 = ref item7;
		Item8 = ref item8;
		Item9 = ref item9;
		Item10 = ref item10;
	}

	public void Deconstruct(out Entity entity, out Ref<T0> item0, out Ref<T1> item1, out Ref<T2> item2, out Ref<T3> item3, out Ref<T4> item4, out Ref<T5> item5, out Ref<T6> item6, out Ref<T7> item7, out Ref<T8> item8, out Ref<T9> item9, out Ref<T10> item10)
	{
		entity = Entity;
		item0 = new Ref<T0> { Item = ref Item0 };
		item1 = new Ref<T1> { Item = ref Item1 };
		item2 = new Ref<T2> { Item = ref Item2 };
		item3 = new Ref<T3> { Item = ref Item3 };
		item4 = new Ref<T4> { Item = ref Item4 };
		item5 = new Ref<T5> { Item = ref Item5 };
		item6 = new Ref<T6> { Item = ref Item6 };
		item7 = new Ref<T7> { Item = ref Item7 };
		item8 = new Ref<T8> { Item = ref Item8 };
		item9 = new Ref<T9> { Item = ref Item9 };
		item10 = new Ref<T10> { Item = ref Item10 };
	}
}

public readonly ref struct RefTuple12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
{
	public readonly Entity Entity;

	public readonly ref T0 Item0;
	public readonly ref T1 Item1;
	public readonly ref T2 Item2;
	public readonly ref T3 Item3;
	public readonly ref T4 Item4;
	public readonly ref T5 Item5;
	public readonly ref T6 Item6;
	public readonly ref T7 Item7;
	public readonly ref T8 Item8;
	public readonly ref T9 Item9;
	public readonly ref T10 Item10;
	public readonly ref T11 Item11;

	internal RefTuple12(Entity entity, ref T0 item0, ref T1 item1, ref T2 item2, ref T3 item3, ref T4 item4, ref T5 item5, ref T6 item6, ref T7 item7, ref T8 item8, ref T9 item9, ref T10 item10, ref T11 item11)
	{
		Entity = entity;
		Item0 = ref item0;
		Item1 = ref item1;
		Item2 = ref item2;
		Item3 = ref item3;
		Item4 = ref item4;
		Item5 = ref item5;
		Item6 = ref item6;
		Item7 = ref item7;
		Item8 = ref item8;
		Item9 = ref item9;
		Item10 = ref item10;
		Item11 = ref item11;
	}

	public void Deconstruct(out Entity entity, out Ref<T0> item0, out Ref<T1> item1, out Ref<T2> item2, out Ref<T3> item3, out Ref<T4> item4, out Ref<T5> item5, out Ref<T6> item6, out Ref<T7> item7, out Ref<T8> item8, out Ref<T9> item9, out Ref<T10> item10, out Ref<T11> item11)
	{
		entity = Entity;
		item0 = new Ref<T0> { Item = ref Item0 };
		item1 = new Ref<T1> { Item = ref Item1 };
		item2 = new Ref<T2> { Item = ref Item2 };
		item3 = new Ref<T3> { Item = ref Item3 };
		item4 = new Ref<T4> { Item = ref Item4 };
		item5 = new Ref<T5> { Item = ref Item5 };
		item6 = new Ref<T6> { Item = ref Item6 };
		item7 = new Ref<T7> { Item = ref Item7 };
		item8 = new Ref<T8> { Item = ref Item8 };
		item9 = new Ref<T9> { Item = ref Item9 };
		item10 = new Ref<T10> { Item = ref Item10 };
		item11 = new Ref<T11> { Item = ref Item11 };
	}
}

public readonly ref struct RefTuple13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
{
	public readonly Entity Entity;

	public readonly ref T0 Item0;
	public readonly ref T1 Item1;
	public readonly ref T2 Item2;
	public readonly ref T3 Item3;
	public readonly ref T4 Item4;
	public readonly ref T5 Item5;
	public readonly ref T6 Item6;
	public readonly ref T7 Item7;
	public readonly ref T8 Item8;
	public readonly ref T9 Item9;
	public readonly ref T10 Item10;
	public readonly ref T11 Item11;
	public readonly ref T12 Item12;

	internal RefTuple13(Entity entity, ref T0 item0, ref T1 item1, ref T2 item2, ref T3 item3, ref T4 item4, ref T5 item5, ref T6 item6, ref T7 item7, ref T8 item8, ref T9 item9, ref T10 item10, ref T11 item11, ref T12 item12)
	{
		Entity = entity;
		Item0 = ref item0;
		Item1 = ref item1;
		Item2 = ref item2;
		Item3 = ref item3;
		Item4 = ref item4;
		Item5 = ref item5;
		Item6 = ref item6;
		Item7 = ref item7;
		Item8 = ref item8;
		Item9 = ref item9;
		Item10 = ref item10;
		Item11 = ref item11;
		Item12 = ref item12;
	}

	public void Deconstruct(out Entity entity, out Ref<T0> item0, out Ref<T1> item1, out Ref<T2> item2, out Ref<T3> item3, out Ref<T4> item4, out Ref<T5> item5, out Ref<T6> item6, out Ref<T7> item7, out Ref<T8> item8, out Ref<T9> item9, out Ref<T10> item10, out Ref<T11> item11, out Ref<T12> item12)
	{
		entity = Entity;
		item0 = new Ref<T0> { Item = ref Item0 };
		item1 = new Ref<T1> { Item = ref Item1 };
		item2 = new Ref<T2> { Item = ref Item2 };
		item3 = new Ref<T3> { Item = ref Item3 };
		item4 = new Ref<T4> { Item = ref Item4 };
		item5 = new Ref<T5> { Item = ref Item5 };
		item6 = new Ref<T6> { Item = ref Item6 };
		item7 = new Ref<T7> { Item = ref Item7 };
		item8 = new Ref<T8> { Item = ref Item8 };
		item9 = new Ref<T9> { Item = ref Item9 };
		item10 = new Ref<T10> { Item = ref Item10 };
		item11 = new Ref<T11> { Item = ref Item11 };
		item12 = new Ref<T12> { Item = ref Item12 };
	}
}

public readonly ref struct RefTuple14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
{
	public readonly Entity Entity;

	public readonly ref T0 Item0;
	public readonly ref T1 Item1;
	public readonly ref T2 Item2;
	public readonly ref T3 Item3;
	public readonly ref T4 Item4;
	public readonly ref T5 Item5;
	public readonly ref T6 Item6;
	public readonly ref T7 Item7;
	public readonly ref T8 Item8;
	public readonly ref T9 Item9;
	public readonly ref T10 Item10;
	public readonly ref T11 Item11;
	public readonly ref T12 Item12;
	public readonly ref T13 Item13;

	internal RefTuple14(Entity entity, ref T0 item0, ref T1 item1, ref T2 item2, ref T3 item3, ref T4 item4, ref T5 item5, ref T6 item6, ref T7 item7, ref T8 item8, ref T9 item9, ref T10 item10, ref T11 item11, ref T12 item12, ref T13 item13)
	{
		Entity = entity;
		Item0 = ref item0;
		Item1 = ref item1;
		Item2 = ref item2;
		Item3 = ref item3;
		Item4 = ref item4;
		Item5 = ref item5;
		Item6 = ref item6;
		Item7 = ref item7;
		Item8 = ref item8;
		Item9 = ref item9;
		Item10 = ref item10;
		Item11 = ref item11;
		Item12 = ref item12;
		Item13 = ref item13;
	}

	public void Deconstruct(out Entity entity, out Ref<T0> item0, out Ref<T1> item1, out Ref<T2> item2, out Ref<T3> item3, out Ref<T4> item4, out Ref<T5> item5, out Ref<T6> item6, out Ref<T7> item7, out Ref<T8> item8, out Ref<T9> item9, out Ref<T10> item10, out Ref<T11> item11, out Ref<T12> item12, out Ref<T13> item13)
	{
		entity = Entity;
		item0 = new Ref<T0> { Item = ref Item0 };
		item1 = new Ref<T1> { Item = ref Item1 };
		item2 = new Ref<T2> { Item = ref Item2 };
		item3 = new Ref<T3> { Item = ref Item3 };
		item4 = new Ref<T4> { Item = ref Item4 };
		item5 = new Ref<T5> { Item = ref Item5 };
		item6 = new Ref<T6> { Item = ref Item6 };
		item7 = new Ref<T7> { Item = ref Item7 };
		item8 = new Ref<T8> { Item = ref Item8 };
		item9 = new Ref<T9> { Item = ref Item9 };
		item10 = new Ref<T10> { Item = ref Item10 };
		item11 = new Ref<T11> { Item = ref Item11 };
		item12 = new Ref<T12> { Item = ref Item12 };
		item13 = new Ref<T13> { Item = ref Item13 };
	}
}

public readonly ref struct RefTuple15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
{
	public readonly Entity Entity;

	public readonly ref T0 Item0;
	public readonly ref T1 Item1;
	public readonly ref T2 Item2;
	public readonly ref T3 Item3;
	public readonly ref T4 Item4;
	public readonly ref T5 Item5;
	public readonly ref T6 Item6;
	public readonly ref T7 Item7;
	public readonly ref T8 Item8;
	public readonly ref T9 Item9;
	public readonly ref T10 Item10;
	public readonly ref T11 Item11;
	public readonly ref T12 Item12;
	public readonly ref T13 Item13;
	public readonly ref T14 Item14;

	internal RefTuple15(Entity entity, ref T0 item0, ref T1 item1, ref T2 item2, ref T3 item3, ref T4 item4, ref T5 item5, ref T6 item6, ref T7 item7, ref T8 item8, ref T9 item9, ref T10 item10, ref T11 item11, ref T12 item12, ref T13 item13, ref T14 item14)
	{
		Entity = entity;
		Item0 = ref item0;
		Item1 = ref item1;
		Item2 = ref item2;
		Item3 = ref item3;
		Item4 = ref item4;
		Item5 = ref item5;
		Item6 = ref item6;
		Item7 = ref item7;
		Item8 = ref item8;
		Item9 = ref item9;
		Item10 = ref item10;
		Item11 = ref item11;
		Item12 = ref item12;
		Item13 = ref item13;
		Item14 = ref item14;
	}

	public void Deconstruct(out Entity entity, out Ref<T0> item0, out Ref<T1> item1, out Ref<T2> item2, out Ref<T3> item3, out Ref<T4> item4, out Ref<T5> item5, out Ref<T6> item6, out Ref<T7> item7, out Ref<T8> item8, out Ref<T9> item9, out Ref<T10> item10, out Ref<T11> item11, out Ref<T12> item12, out Ref<T13> item13, out Ref<T14> item14)
	{
		entity = Entity;
		item0 = new Ref<T0> { Item = ref Item0 };
		item1 = new Ref<T1> { Item = ref Item1 };
		item2 = new Ref<T2> { Item = ref Item2 };
		item3 = new Ref<T3> { Item = ref Item3 };
		item4 = new Ref<T4> { Item = ref Item4 };
		item5 = new Ref<T5> { Item = ref Item5 };
		item6 = new Ref<T6> { Item = ref Item6 };
		item7 = new Ref<T7> { Item = ref Item7 };
		item8 = new Ref<T8> { Item = ref Item8 };
		item9 = new Ref<T9> { Item = ref Item9 };
		item10 = new Ref<T10> { Item = ref Item10 };
		item11 = new Ref<T11> { Item = ref Item11 };
		item12 = new Ref<T12> { Item = ref Item12 };
		item13 = new Ref<T13> { Item = ref Item13 };
		item14 = new Ref<T14> { Item = ref Item14 };
	}
}

public readonly ref struct RefTuple16<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
{
	public readonly Entity Entity;

	public readonly ref T0 Item0;
	public readonly ref T1 Item1;
	public readonly ref T2 Item2;
	public readonly ref T3 Item3;
	public readonly ref T4 Item4;
	public readonly ref T5 Item5;
	public readonly ref T6 Item6;
	public readonly ref T7 Item7;
	public readonly ref T8 Item8;
	public readonly ref T9 Item9;
	public readonly ref T10 Item10;
	public readonly ref T11 Item11;
	public readonly ref T12 Item12;
	public readonly ref T13 Item13;
	public readonly ref T14 Item14;
	public readonly ref T15 Item15;

	internal RefTuple16(Entity entity, ref T0 item0, ref T1 item1, ref T2 item2, ref T3 item3, ref T4 item4, ref T5 item5, ref T6 item6, ref T7 item7, ref T8 item8, ref T9 item9, ref T10 item10, ref T11 item11, ref T12 item12, ref T13 item13, ref T14 item14, ref T15 item15)
	{
		Entity = entity;
		Item0 = ref item0;
		Item1 = ref item1;
		Item2 = ref item2;
		Item3 = ref item3;
		Item4 = ref item4;
		Item5 = ref item5;
		Item6 = ref item6;
		Item7 = ref item7;
		Item8 = ref item8;
		Item9 = ref item9;
		Item10 = ref item10;
		Item11 = ref item11;
		Item12 = ref item12;
		Item13 = ref item13;
		Item14 = ref item14;
		Item15 = ref item15;
	}

	public void Deconstruct(out Entity entity, out Ref<T0> item0, out Ref<T1> item1, out Ref<T2> item2, out Ref<T3> item3, out Ref<T4> item4, out Ref<T5> item5, out Ref<T6> item6, out Ref<T7> item7, out Ref<T8> item8, out Ref<T9> item9, out Ref<T10> item10, out Ref<T11> item11, out Ref<T12> item12, out Ref<T13> item13, out Ref<T14> item14, out Ref<T15> item15)
	{
		entity = Entity;
		item0 = new Ref<T0> { Item = ref Item0 };
		item1 = new Ref<T1> { Item = ref Item1 };
		item2 = new Ref<T2> { Item = ref Item2 };
		item3 = new Ref<T3> { Item = ref Item3 };
		item4 = new Ref<T4> { Item = ref Item4 };
		item5 = new Ref<T5> { Item = ref Item5 };
		item6 = new Ref<T6> { Item = ref Item6 };
		item7 = new Ref<T7> { Item = ref Item7 };
		item8 = new Ref<T8> { Item = ref Item8 };
		item9 = new Ref<T9> { Item = ref Item9 };
		item10 = new Ref<T10> { Item = ref Item10 };
		item11 = new Ref<T11> { Item = ref Item11 };
		item12 = new Ref<T12> { Item = ref Item12 };
		item13 = new Ref<T13> { Item = ref Item13 };
		item14 = new Ref<T14> { Item = ref Item14 };
		item15 = new Ref<T15> { Item = ref Item15 };
	}
}


