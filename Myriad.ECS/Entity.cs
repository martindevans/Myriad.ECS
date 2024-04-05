using System.Diagnostics;
using Myriad.ECS.Collections;
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

        // Hash it
        unsafe
        {
            var span = new Span<byte>(&u.B0, 8);
            return unchecked((long)xxHash64.ComputeHash(span, 17));
        }
    }
}