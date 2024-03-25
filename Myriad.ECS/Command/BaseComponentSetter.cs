using Myriad.ECS.Allocations;
using Myriad.ECS.IDs;
using Myriad.ECS.Worlds.Archetypes;

namespace Myriad.ECS.Command;

internal abstract class BaseComponentSetter
{
    public abstract ComponentID ID { get; }

    public abstract void ReturnToPool();

    public abstract void Write(Row row);
}

internal sealed class GenericComponentSetter<T>
    : BaseComponentSetter
    where T : IComponent
{
    public override ComponentID ID { get; } = ComponentID<T>.ID;

    private T? _value;

    public static GenericComponentSetter<T> Get(T value)
    {
        var pooled = Pool<GenericComponentSetter<T>>.Get();
        pooled._value = value;
        return pooled;
    }

    public override void ReturnToPool()
    {
        _value = default;
        Pool.Return(this);
    }

    public override void Write(Row row)
    {
        row.GetMutable<T>() = _value!;
    }
}