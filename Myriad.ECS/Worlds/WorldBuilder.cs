using Myriad.ECS.Allocations;
using Myriad.ECS.IDs;

namespace Myriad.ECS.Worlds;

public sealed partial class WorldBuilder
{
    private readonly List<IReadOnlySet<ComponentID>> _archetypes = [ ];

    public WorldBuilder()
    {

    }

    private bool AddArchetype(HashSet<ComponentID> ids)
    {
        foreach (var archetype in _archetypes)
        {
            if (archetype.SetEquals(ids))
            {
                Pool<HashSet<ComponentID>>.Return(ids);
                return false;
            }
        }

        _archetypes.Add(ids);
        return true;
    }

    public World Build()
    {
        var w = new World()
        {
            _archetypes = new List<Archetype>()
        };

        foreach (var archetype in _archetypes)
        {
            w._archetypes.Add(new Archetype
            {
                Components = archetype,
            });
        }

        return w;
    }
}