using System.Diagnostics;
using Myriad.ECS.Worlds;

namespace Myriad.ECS;

[DebuggerDisplay("{ID}v{Version}")]
public readonly record struct Entity
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
}