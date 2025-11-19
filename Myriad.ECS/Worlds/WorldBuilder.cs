using Myriad.ECS.Allocations;
using Myriad.ECS.Collections;
using Myriad.ECS.IDs;
using Myriad.ECS.Locks;
using Myriad.ECS.Threading;

namespace Myriad.ECS.Worlds;

/// <summary>
/// A builder to create a new <see cref="World"/>
/// </summary>
public sealed partial class WorldBuilder
{
    private readonly List<OrderedListSet<ComponentID>> _archetypes = [ ];
    private IThreadPool? _pool;
    private IWorldArchetypeSafetyManager? _lockManager;

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
            if (!set.Add(ComponentID.Get(type)))
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
            throw new InvalidOperationException($"Cannot call '{nameof(WithThreadPool)}' twice");
        _pool = pool;

        return this;
    }

    //todo: make this public when it's ready
    /// <summary>
    /// Define the <see cref="IWorldArchetypeSafetyManager"/> used by this world.
    /// </summary>
    /// <param name="manager"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    internal WorldBuilder WithSafetySystem(IWorldArchetypeSafetyManager manager)
    {
        if (_lockManager != null)
            throw new InvalidOperationException($"Cannot call '{nameof(WithSafetySystem)}' twice");
        _lockManager = manager;

        return this;
    }

    /// <summary>
    /// Create a new <see cref="World"/> using the configuration in this <see cref="WorldBuilder"/>.
    /// </summary>
    /// <returns></returns>
    public World Build()
    {
        var w = new World(
            _pool ?? new DefaultThreadPool(),
            _lockManager ?? new DefaultWorldArchetypeSafetyManager()
        );

        foreach (var components in _archetypes)
            w.GetOrCreateArchetype(components);

        return w;
    }
}