﻿using System.Diagnostics;
using Myriad.ECS.Collections;
using Myriad.ECS.IDs;
using Myriad.ECS.Worlds;

namespace Myriad.ECS;

[DebuggerDisplay("{ID}v{Version}")]
public readonly partial record struct Entity
    : IComparable<Entity>
{
    public readonly int ID;
    public readonly uint Version;
    public readonly World World;

    internal Entity(int id, uint version, World world)
    {
        ID = id;
        Version = version;
        World = world;
    }

    /// <summary>
    /// Check if this Entity still exists.
    /// </summary>
    /// <returns></returns>
    public bool Exists()
    {
        return ID != 0
            && World.GetVersion(ID) == Version;
    }

    /// <summary>
    /// Check if this Entity still exists and is not a phantom.
    /// </summary>
    /// <returns></returns>
    public bool IsAlive()
    {
        return Exists()
            && !IsPhantom();
    }

    /// <summary>
    /// Check if this Entity is in a phantom state. i.e. automatically excluded from queries
    /// and automatically deleted when the last IPhantomComponent component is removed.
    /// </summary>
    /// <returns>true if this entity is a phantom. False is it does not exist or is not a phantom.</returns>
    public bool IsPhantom()
    {
        return ID != 0
            && Exists()
            && World.GetArchetype(this).IsPhantom;
    }

    /// <inheritdoc />
    public int CompareTo(Entity other)
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
    public FrozenOrderedListSet<ComponentID> GetComponents()
    {
        var info = World.GetEntityInfo(this);
        return info.Chunk.Archetype.Components;
    }

    /// <summary>
    /// Check if this entity has a component
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public bool HasComponent<T>()
        where T : IComponent
    {
        return GetComponents().Contains(ComponentID<T>.ID);
    }

    /// <summary>
    /// Get a reference to a component of the given type. If the entity
    /// does not have this component an exception will be thrown.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public ref T GetComponentRef<T>()
        where T : IComponent
    {
        ref var entityInfo = ref World.GetEntityInfo(this);
        return ref entityInfo.Chunk.GetRef<T>(this, entityInfo.RowIndex);
    }

    /// <summary>
    /// Get a <b>boxed copy</b> of a component from this entity. Only use for debugging!
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public object? GetBoxedComponent(ComponentID id)
    {
        if (!Exists())
            return null;
        if (!GetComponents().Contains(id))
            return null;

        ref var entityInfo = ref World.GetEntityInfo(this);
        return entityInfo.Chunk.GetComponentArray(id).GetValue(entityInfo.RowIndex);
    }
}