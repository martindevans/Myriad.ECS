using System.Diagnostics;
using Myriad.ECS.Worlds;

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
}