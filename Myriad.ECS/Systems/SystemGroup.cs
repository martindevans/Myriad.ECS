using System.Diagnostics;

namespace Myriad.ECS.Systems;

public sealed class SystemGroup
    : ISystemGroup
{
    public string Name { get; }
    public bool Enabled { get; set; } = true;

    public TimeSpan ExecutionTime { get; private set; }
    private readonly Stopwatch _timer = new();

    private readonly ISystemBefore[] _beforeSystems;
    private readonly ISystem[] _systems;
    private readonly ISystemAfter[] _afterSystems;

    public SystemGroup(string name, params ISystem[] systems)
    {
        Name = name;

        _beforeSystems = systems.OfType<ISystemBefore>().ToArray();
        _systems = systems;
        _afterSystems = systems.OfType<ISystemAfter>().ToArray();
    }

    public void Init()
    {
        foreach (var system in _systems.OfType<ISystemInit>())
            system.Init();
    }

    public void BeforeUpdate(GameTime time)
    {
        _timer.Reset();

        if (Enabled)
        {
            _timer.Start();
            {
                foreach (var system in _beforeSystems)
                    if (system.Enabled)
                        system.BeforeUpdate(time);
            }
            _timer.Stop();
        }
    }

    public void Update(GameTime time)
    {
        if (Enabled)
        {
            _timer.Start();
            {
                foreach (var system in _systems)
                    if (system.Enabled)
                        system.Update(time);
            }
            _timer.Stop();
        }
    }

    public void AfterUpdate(GameTime time)
    {
        if (Enabled)
        {
            _timer.Start();
            {
                foreach (var system in _afterSystems)
                    if (system.Enabled)
                        system.AfterUpdate(time);
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