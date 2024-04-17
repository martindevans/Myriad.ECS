﻿using System.Diagnostics;
using Myriad.ECS.Collections;
using Myriad.ECS.IDs;
using Myriad.ECS.Worlds;
using Myriad.ECS.xxHash;

namespace Myriad.ECS;

[DebuggerDisplay("{ID}v{Version}")]
public readonly record struct Entity
    : IComparable<Entity>
{
    public readonly int ID;
    public readonly uint Version;

    internal Entity(int id, uint version)
    {
        ID = id;
        Version = version;
    }

    /// <summary>
    /// Check if this Entity still exists.
    /// </summary>
    /// <param name="world"></param>
    /// <returns></returns>
    public bool Exists(World world)
    {
        return ID != 0
            && world.GetVersion(ID) == Version;
    }

    /// <summary>
    /// Check if this Entity still exists and is not a phantom.
    /// </summary>
    /// <param name="world"></param>
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
    /// <param name="world"></param>
    /// <returns>true if this entity is a phantom. False is it does not exist or is not a phantom.</returns>
    public bool IsPhantom(World world)
    {
        return ID != 0
            && Exists(world)
            && world.GetArchetype(this).IsPhantom;
    }

    public int CompareTo(Entity other)
    {
        var idc = ID.CompareTo(other.ID);
        if (idc != 0)
            return idc;

        return Version.CompareTo(other.Version);
    }

    public long UniqueID()
    {
        // Set the entity ID and version into the hi and lo 32 bits
        var u = new Union64
        {
            I0 = ID,
            I1 = unchecked((int)Version)
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

    public FrozenOrderedListSet<ComponentID> GetComponents(World w)
    {
        if (!Exists(w))
            throw new InvalidOperationException("Cannot call GetComponents for entity that does not exist");

        var info = w.GetEntityInfo(this);
        return info.Chunk.Archetype.Components;
    }
}