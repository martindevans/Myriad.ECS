﻿using Myriad.ECS.Collections;
using Myriad.ECS.IDs;
using Myriad.ECS.Worlds;
using System.Diagnostics;

namespace Myriad.ECS;

/// <summary>
/// The ID of an <see cref="Entity"/> (not carrying a reference to a <see cref="World"/>)
/// </summary>
[DebuggerDisplay("{ID}v{Version}")]
public readonly partial record struct EntityId
    : IComparable<EntityId>
{
    /// <summary>
    /// The <see cref="Entity"/> of an entity, may be re-used very quickly once an <see cref="Entity"/> is destroyed.
    /// </summary>
    public readonly int ID;

    /// <summary>
    /// The version number of this ID, may also be re-used but only after the full 32 bit counter has been overflowed for this specific ID.
    /// </summary>
    public readonly uint Version;

    internal EntityId(int id, uint version)
    {
        ID = id;
        Version = version;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(ID, Version);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{ID}v{Version}";
    }

    /// <summary>
    /// Create a new <see cref="Entity"/> struct that represents this Entity
    /// </summary>
    /// <param name="world"></param>
    /// <returns></returns>
    public Entity ToEntity(World world)
    {
        return new Entity(this, world);
    }

    /// <summary>
    /// Check if this Entity still exists.
    /// </summary>
    /// <returns></returns>
    public bool Exists(World world)
    {
        return ID != 0
            && world.GetVersion(ID) == Version;
    }

    /// <summary>
    /// Check if this Entity still exists and is not a phantom.
    /// </summary>
    /// <returns></returns>
    public bool IsAlive(World world)
    {
        return Exists(world)
            && !IsPhantom(world);
    }

    /// <summary>
    /// Check if this Entity is in a phantom state. i.e. automatically excluded from queries
    /// and automatically deleted when the last IPhantomComponent component is removed.
    /// </summary>
    /// <returns>true if this entity is a phantom. False is it does not exist or is not a phantom.</returns>
    public bool IsPhantom(World world)
    {
        return ID != 0
            && Exists(world)
            && world.GetArchetype(this).IsPhantom;
    }

    /// <inheritdoc />
    public int CompareTo(EntityId other)
    {
        var idc = ID.CompareTo(other.ID);
        if (idc != 0)
            return idc;

        return Version.CompareTo(other.Version);
    }

    /// <summary>
    /// Get a unique 64 bit ID for this entity
    /// </summary>
    /// <returns></returns>
    public long UniqueID()
    {
        // Set the entity ID and version into the hi and lo 32 bits
        var u = new Union64
        {
            I0 = ID,
            U1 = Version
        };

        // Swap around some bytes (this is effectively an injective hash)
        Swap(ref u.B0, ref u.B1);
        Swap(ref u.B2, ref u.B3);
        Swap(ref u.B4, ref u.B5);
        Swap(ref u.B6, ref u.B7);
        unchecked
        {
            u.I0 *= 1297519;
            u.I1 *= 722479;
        }
        Swap(ref u.B4, ref u.B1);
        Swap(ref u.B7, ref u.B3);
        Swap(ref u.B0, ref u.B2);
        Swap(ref u.B6, ref u.B5);

        return u.Long;

        static void Swap(ref byte a, ref byte b)
        {
            (a, b) = (b, a);
        }
    }

    /// <summary>
    /// Get the set of components which this entity currently has
    /// </summary>
    /// <returns></returns>
    public FrozenOrderedListSet<ComponentID> GetComponents(World world)
    {
        var info = world.GetEntityInfo(this);
        return info.Chunk.Archetype.Components;
    }

    /// <summary>
    /// Check if this entity has a component
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public bool HasComponent<T>(World world)
        where T : IComponent
    {
        // Try to get entity info ref, returns ref to dummy if not
        EntityInfo dummy = default;
        ref var info = ref world.GetEntityInfo(this, ref dummy, out var isNotExists);

        // If it doesn't exist it doesn't have the component!
        if (isNotExists)
            return false;

        // Check for component
        return info.Chunk.Archetype.Components.Contains(ComponentID<T>.ID);
    }

    /// <summary>
    /// Get a reference to a component of the given type. If the entity
    /// does not have this component an exception will be thrown.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public ref T GetComponentRef<T>(World world)
        where T : IComponent
    {
        return ref GetComponentRefT<T>(world).Ref;
    }

    /// <summary>
    /// Get a reference to a component of the given type. If the entity
    /// does not have this component an exception will be thrown.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public RefT<T> GetComponentRefT<T>(World world)
        where T : IComponent
    {
        ref var entityInfo = ref world.GetEntityInfo(this);
        return entityInfo.Chunk.GetRefT<T>(this, entityInfo.RowIndex);
    }

    /// <summary>
    /// Get a <b>boxed copy</b> of a component from this entity. Only use for debugging!
    /// </summary>
    /// <param name="world"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public object? GetBoxedComponent(World world, ComponentID id)
    {
        if (!Exists(world))
            return null;
        if (!GetComponents(world).Contains(id))
            return null;

        ref var entityInfo = ref world.GetEntityInfo(this);
        return entityInfo.Chunk.GetComponentArray(id).GetValue(entityInfo.RowIndex);
    }
}