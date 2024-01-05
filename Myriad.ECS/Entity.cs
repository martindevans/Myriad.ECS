using Myriad.ECS.Worlds;

namespace Myriad.ECS;

public readonly record struct Entity
{
    internal readonly int ID;
    internal readonly uint Version;

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

    public override string ToString()
    {
        return $"Entity({ID}v{Version})";
    }
}