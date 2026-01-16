using Myriad.ECS.Collections;
using Myriad.ECS.IDs;
using Myriad.ECS.Worlds;
using System.Diagnostics;

namespace Myriad.ECS;

/// <summary>
/// An <see cref="Entity"/> is an ID in the <see cref="World"/> which has a set of components associated with it.
/// </summary>
[DebuggerDisplay("{ID}")]
public readonly partial record struct Entity
    : IComparable<Entity>
{
    /// <summary>
    /// The <see cref="World"/> this <see cref="Entity"/> is in
    /// </summary>
    public readonly World World;

    /// <summary>
    /// The raw ID of this <see cref="Entity"/>
    /// </summary>
    public readonly EntityId ID;

    /// <summary>
    /// Get the set of components which this entity currently has
    /// </summary>
    /// <returns></returns>
    public FrozenOrderedListSet<ComponentID> ComponentTypes => World == null ? FrozenOrderedListSet<ComponentID>.Empty : ID.GetComponents(World);

    /// <summary>
    /// Get a boxed array of all components. <b>DO NOT</b> use this for anything other than debugging!
    /// </summary>
    public object[] BoxedComponents => ComponentTypes.LINQ().Select(GetBoxedComponent).ToArray()!;

    internal Entity(EntityId id, World world)
    {
        ID = id;
        World = world;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return ID.GetHashCode();
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return ID.ToString();
    }

    /// <summary>
    /// Check if this Entity still exists.
    /// </summary>
    /// <returns></returns>
    public bool Exists() => World != null && ID.Exists(World);

    /// <summary>
    /// Check if this Entity still exists and is not a phantom.
    /// </summary>
    /// <returns></returns>
    public bool IsAlive() => World != null && ID.IsAlive(World);

    /// <summary>
    /// Check if this Entity is in a phantom state. i.e. automatically excluded from queries
    /// and automatically deleted when the last IPhantomComponent component is removed.
    /// </summary>
    /// <returns>true if this entity is a phantom. False is it does not exist or is not a phantom.</returns>
    public bool IsPhantom() => World != null && ID.IsPhantom(World);

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
    /// Check if this entity has a component
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public bool HasComponent<T>() where T : IComponent => World != null && ID.HasComponent<T>(World);

    /// <summary>
    /// Get a reference to a component of the given type. If the entity
    /// does not have this component an exception will be thrown.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public ref T GetComponentRef<T>() where T : IComponent => ref ID.GetComponentRef<T>(World);

    /// <summary>
    /// Get a reference to a component of the given type. If the entity
    /// does not have this component an exception will be thrown.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public RefT<T> GetComponentRefT<T>() where T : IComponent => ID.GetComponentRefT<T>(World);

    /// <summary>
    /// Try to get a tuple of one component, returns false if the entity does not exist or if the component is missing
    /// </summary>
    public bool TryGetComponentRef<T>(out RefTuple<T> output) where T : IComponent => ID.TryGetComponentRef(World, out output);

    /// <summary>
    /// Get a <b>boxed copy</b> of a component from this entity. Only use for debugging!
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public object? GetBoxedComponent(ComponentID id) => ID.GetBoxedComponent(World, id);

    /// <summary>
    /// Cast this entity to an <see cref="EntityId"/>. Exactly the same as simply accessing the <see cref="ID"/> property.
    /// </summary>
    /// <param name="e"></param>
    public static implicit operator EntityId(Entity e) => e.ID;
}