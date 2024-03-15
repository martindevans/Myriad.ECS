using System.Diagnostics;

namespace Myriad.ECS.Systems;

public sealed class SystemGroup<TData>
    : ISystemGroup<TData>
{
    public string Name { get; }
    public bool Enabled { get; set; } = true;

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

        if (Enabled)
        {
            _timer.Start();
            {
                foreach (var system in _beforeSystems)
                    if (system.Enabled)
                        system.BeforeUpdate(data);
            }
            _timer.Stop();
        }
    }

    public void Update(TData data)
    {
        if (Enabled)
        {
            _timer.Start();
            {
                foreach (var system in _systems)
                    if (system.Enabled)
                        system.Update(data);
            }
            _timer.Stop();
        }
    }

    public void AfterUpdate(TData data)
    {
        if (Enabled)
        {
            _timer.Start();
            {
                foreach (var system in _afterSystems)
                    if (system.Enabled)
                        system.AfterUpdate(data);
            }
            _timer.Stop();
        }

        ExecutionTime = _timer.Elapsed;
    }

    public void Dispose()
    {
        foreach (var system in _systems.OfType<IDisposable>())
            system.Dispose();
    }
}