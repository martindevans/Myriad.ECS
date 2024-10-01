using System.Diagnostics;
using Myriad.ECS.Collections;
using Myriad.ECS.IDs;
using Myriad.ECS.Worlds;

namespace Myriad.ECS;

[DebuggerDisplay("{ID}")]
public readonly partial record struct Entity
    : IComparable<Entity>
{
    public readonly World World;
    public readonly EntityId ID;

    internal Entity(EntityId id, World world)
    {
        ID = id;
        World = world;
    }

    /// <summary>
    /// Check if this Entity still exists.
    /// </summary>
    /// <returns></returns>
    public bool Exists() => ID.Exists(World);

    /// <summary>
    /// Check if this Entity still exists and is not a phantom.
    /// </summary>
    /// <returns></returns>
    public bool IsAlive() => ID.IsAlive(World);

    /// <summary>
    /// Check if this Entity is in a phantom state. i.e. automatically excluded from queries
    /// and automatically deleted when the last IPhantomComponent component is removed.
    /// </summary>
    /// <returns>true if this entity is a phantom. False is it does not exist or is not a phantom.</returns>
    public bool IsPhantom() => ID.IsPhantom(World);

    /// <inheritdoc />
    public int CompareTo(Entity other)
    {
        return ID.CompareTo(other.ID);
    }

    /// <summary>
    /// Get a unique 64 bit ID for this entity
    /// </summary>
    /// <returns></returns>
    public long UniqueID() => ID.UniqueID();

    /// <summary>
    /// Get the set of components which this entity currently has
    /// </summary>
    /// <returns></returns>
    public FrozenOrderedListSet<ComponentID> GetComponents() => ID.GetComponents(World);

    /// <summary>
    /// Check if this entity has a component
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public bool HasComponent<T>() where T : IComponent => ID.HasComponent<T>(World);

    /// <summary>
    /// Get a reference to a component of the given type. If the entity
    /// does not have this component an exception will be thrown.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public ref T GetComponentRef<T>() where T : IComponent => ref ID.GetComponentRef<T>(World);

    /// <summary>
    /// Get a <b>boxed copy</b> of a component from this entity. Only use for debugging!
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public object? GetBoxedComponent(ComponentID id) => ID.GetBoxedComponent(World, id);
}