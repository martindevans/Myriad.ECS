using Myriad.ECS.Collections;
using Myriad.ECS.IDs;

namespace Myriad.ECS.Worlds;

public sealed partial class WorldBuilder
{
    private readonly List<OrderedListSet<ComponentID>> _archetypes = [ ];

    private bool AddArchetype(HashSet<ComponentID> ids)
    {
        if (_archetypes.Any(a => a.SetEquals(ids)))
            return false;

        _archetypes.Add(new OrderedListSet<ComponentID>(ids));
        return true;
    }

    /// <summary>
    /// Declare a specific archetype that should be created ahead of time in this world. This
    /// can prevent expensive structural changes in the world later.
    /// </summary>
    public WorldBuilder WithArchetype(params Type[] types)
    {
        var set = new HashSet<ComponentID>(types.Length);

        foreach (var type in types)
            if (!set.Add(ComponentRegistry.Get(type)))
                throw new ArgumentException($"Duplicate component type: {type.Name}");

        AddArchetype(set);

        return this;
    }

    public World Build()
    {
        var w = new World();

        foreach (var components in _archetypes)
            w.GetOrCreateArchetype(components);

        return w;
    }
}