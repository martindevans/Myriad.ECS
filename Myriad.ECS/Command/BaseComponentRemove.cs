using Myriad.ECS.IDs;

namespace Myriad.ECS.Command;

internal abstract class BaseComponentRemove
{
    public abstract ComponentID ID { get; }
}

internal sealed class GenericComponentRemove<T>
    : BaseComponentRemove
    where T : IComponent
{
    public static GenericComponentRemove<T> Instance { get; } = new GenericComponentRemove<T>();

    public override ComponentID ID { get; } = ComponentID<T>.ID;
}