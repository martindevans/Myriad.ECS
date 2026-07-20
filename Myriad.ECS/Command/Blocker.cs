using Myriad.ECS.Worlds;

namespace Myriad.ECS.Command;

internal struct Blocker
{
    private readonly World _world;
    private bool _blockedAll;

    public Blocker(World world)
    {
        _world = world;
    }

    public void Block()
    {
        if (!_blockedAll)
            _world.Block();
        _blockedAll = true;
    }
}