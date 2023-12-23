namespace Myriad.ECS.Queries.Attributes;

internal interface IAllAttribute
{
    IReadOnlyList<Type> Types { get; }
}

[AttributeUsage(AttributeTargets.Struct, AllowMultiple = true)]
public sealed class All<T0>
    : Attribute, IAllAttribute
{
    IReadOnlyList<Type> IAllAttribute.Types => new[] { typeof(T0) };
}

[AttributeUsage(AttributeTargets.Struct, AllowMultiple = true)]
public sealed class All<T0, T1>
    : Attribute, IAllAttribute
{
    IReadOnlyList<Type> IAllAttribute.Types => new[] { typeof(T0), typeof(T1) };
}

[AttributeUsage(AttributeTargets.Struct, AllowMultiple = true)]
public sealed class All<T0, T1, T2>
    : Attribute, IAllAttribute
{
    IReadOnlyList<Type> IAllAttribute.Types => new[] { typeof(T0), typeof(T1), typeof(T2) };
}

[AttributeUsage(AttributeTargets.Struct, AllowMultiple = true)]
public sealed class All<T0, T1, T2, T3>
    : Attribute, IAllAttribute
{
    IReadOnlyList<Type> IAllAttribute.Types => new[] { typeof(T0), typeof(T1), typeof(T2), typeof(T3) };
}

[AttributeUsage(AttributeTargets.Struct, AllowMultiple = true)]
public sealed class All<T0, T1, T2, T3, T4>
    : Attribute, IAllAttribute
{
    IReadOnlyList<Type> IAllAttribute.Types => new[] { typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4) };
}

[AttributeUsage(AttributeTargets.Struct, AllowMultiple = true)]
public sealed class All<T0, T1, T2, T3, T4, T5>
    : Attribute, IAllAttribute
{
    IReadOnlyList<Type> IAllAttribute.Types => new[] { typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5) };
}

[AttributeUsage(AttributeTargets.Struct, AllowMultiple = true)]
public sealed class All<T0, T1, T2, T3, T4, T5, T6>
    : Attribute, IAllAttribute
{
    IReadOnlyList<Type> IAllAttribute.Types => new[] { typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6) };
}

[AttributeUsage(AttributeTargets.Struct, AllowMultiple = true)]
public sealed class All<T0, T1, T2, T3, T4, T5, T6, T7>
    : Attribute, IAllAttribute
{
    IReadOnlyList<Type> IAllAttribute.Types => new[] { typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7) };
}

[AttributeUsage(AttributeTargets.Struct, AllowMultiple = true)]
public sealed class All<T0, T1, T2, T3, T4, T5, T6, T7, T8>
    : Attribute, IAllAttribute
{
    IReadOnlyList<Type> IAllAttribute.Types => new[] { typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8) };
}

