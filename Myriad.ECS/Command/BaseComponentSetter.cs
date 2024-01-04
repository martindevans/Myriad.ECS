using Myriad.ECS.Allocations;
using Myriad.ECS.IDs;
using Myriad.ECS.Worlds.Archetypes;

namespace Myriad.ECS.Command;

internal abstract class BaseComponentSetter
{
    internal abstract ComponentID ID { get; }

    public abstract void ReturnToPool();

    public abstract void Write(Row row);
}

internal sealed class GenericComponentSetter<T>
    : BaseComponentSetter
    where T : IComponent
{
    internal override ComponentID ID { get; } = ComponentID<T>.ID;

    public T? Value { get; private set; }

    public static GenericComponentSetter<T> Get(T value)
    {
        var pooled = Pool<GenericComponentSetter<T>>.Get();
        pooled.Value = value;
        return pooled;
    }

    public override void ReturnToPool()
    {
        Value = default;
        Pool<GenericComponentSetter<T>>.Return(this);
    }

    public override void Write(Row row)
    {
        row.GetMutable<T>() = Value!;
    }

    public override bool Equals(object? obj)
    {
        if (obj is GenericComponentSetter<T>)
            return true;
        return false;
    }

    public override int GetHashCode()
    {
        return ID.GetHashCode();
    }
}