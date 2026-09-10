using Myriad.ECS.Collections;
using Myriad.ECS.Worlds;
using Myriad.ECS.Worlds.Archetypes;

namespace Myriad.ECS.Command;

/// <summary>
/// Utility for blocking on archetypes. Stores which archetypes have already been blocked on
/// and skips blocking a second time.
/// </summary>
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
        
        _set.Clear();
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