using Myriad.ECS.Collections;
using Myriad.ECS.Worlds;
using Myriad.ECS.Worlds.Archetypes;

namespace Myriad.ECS.Command;

internal struct Blocker
{
    private readonly World _world;
    private readonly OrderedListSet<long> _set;
    private bool _blockedAll;

    public Blocker(World world, OrderedListSet<long> set)
    {
        _world = world;
        _set = set;
        _blockedAll = false;
    }

    public void Block()
    {
        if (!_blockedAll)
            _world.Block();
        _blockedAll = true;
    }

    public void Block(Archetype archetype)
    {
        if (_blockedAll)
            return;

        if (_set.Add(archetype.ArchetypeId))
            archetype.Block();
    }
}