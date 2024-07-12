using Myriad.ECS.Collections;
using Myriad.ECS.IDs;
using Myriad.ECS.Threading;

namespace Myriad.ECS.Worlds;

public sealed partial class WorldBuilder
{
    private readonly List<OrderedListSet<ComponentID>> _archetypes = [ ];
    private IThreadPool? _pool;

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

    /// <summary>
    /// Define the threadpool system used by this world
    /// </summary>
    /// <param name="pool"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public WorldBuilder WithThreadPool(IThreadPool pool)
    {
        if (_pool != null)
            throw new InvalidOperationException("Cannot call 'WithThreadPool' twice");
        _pool = pool;

        return this;
    }

    public World Build()
    {
        var w = new World(
            _pool ?? new DefaultThreadPool()
        );

        foreach (var components in _archetypes)
            w.GetOrCreateArchetype(components);

        return w;
    }
}