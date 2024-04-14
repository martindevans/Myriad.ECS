using System.Diagnostics;

namespace Myriad.ECS.Systems;

public sealed class SystemGroup<TData>
    : ISystemGroup<TData>
{
    public string Name { get; }

    public TimeSpan ExecutionTime { get; private set; }

    private readonly Stopwatch _timer = new();

    private readonly ISystemBefore<TData>[] _beforeSystems;
    private readonly ISystem<TData>[] _systems;
    private readonly ISystemAfter<TData>[] _afterSystems;

    public SystemGroup(string name, params ISystem<TData>[] systems)
    {
        Name = name;

        _beforeSystems = systems.OfType<ISystemBefore<TData>>().ToArray();
        _systems = systems;
        _afterSystems = systems.OfType<ISystemAfter<TData>>().ToArray();
    }

    public void Init()
    {
        foreach (var system in _systems.OfType<ISystemInit<TData>>())
            system.Init();
    }

    public void BeforeUpdate(TData data)
    {
        _timer.Reset();

        _timer.Start();
        {
            foreach (var system in _beforeSystems)
                system.BeforeUpdate(data);
        }
        _timer.Stop();
    }

    public void Update(TData data)
    {
        _timer.Start();
        {
            foreach (var system in _systems)
                system.Update(data);
        }
        _timer.Stop();
    }

    public void AfterUpdate(TData data)
    {
        _timer.Start();
        {
            foreach (var system in _afterSystems)
                system.AfterUpdate(data);
        }
        _timer.Stop();

        ExecutionTime = _timer.Elapsed;
    }

    public void Dispose()
    {
        foreach (var system in _systems.OfType<IDisposable>())
            system.Dispose();
    }

    public IEnumerable<ISystem<TData>> Systems => _systems;

    public IEnumerable<ISystem<TData>> RecursiveSystems
    {
        get
        {
            foreach (var system in _systems)
            {
                if (system is ISystemGroup<TData> group)
                {
                    foreach (var nested in group.RecursiveSystems)
                        yield return nested;
                }
                else
                {
                    yield return system;
                }
            }
        }
    }
}