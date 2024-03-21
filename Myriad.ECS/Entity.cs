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

    public bool IsAlive(World world)
    {
        return ID != 0
            && world.GetVersion(ID) == Version;
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