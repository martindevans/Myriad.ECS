using System.Diagnostics;

namespace Myriad.ECS.Systems;

public sealed class ParallelSystemGroup
    : ISystemGroup
{
    public string Name { get; }
    public bool Enabled { get; set; } = true;

    public TimeSpan ExecutionTime { get; private set; }
    private readonly Stopwatch _timer = new();

    private readonly ISystemBefore[] _beforeSystems;
    private readonly ISystem[] _systems;
    private readonly ISystemAfter[] _afterSystems;

    public ParallelSystemGroup(string name, params ISystem[] systems)
    {
        Name = name;

        _beforeSystems = systems.OfType<ISystemBefore>().ToArray();
        _systems = systems;
        _afterSystems = systems.OfType<ISystemAfter>().ToArray();
    }

    public void Init()
    {
        foreach (var system in _systems)
            system.Init();
    }

    public void BeforeUpdate(GameTime time)
    {
        _timer.Reset();

        if (Enabled)
        {
            _timer.Start();
            {
                if (_beforeSystems.Length > 0 && AnyEnabled(_afterSystems))
                {
                    Parallel.ForEach(_beforeSystems, system =>
                    {
                        if (system.Enabled)
                            system.BeforeUpdate(time);
                    });
                }
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
                if (AnyEnabled(_afterSystems))
                {
                    Parallel.ForEach(_systems, system =>
                    {
                        if (system.Enabled)
                            system.Update(time);
                    });
                }
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
                if (_afterSystems.Length > 0 && AnyEnabled(_afterSystems))
                {
                    Parallel.ForEach(_afterSystems, system =>
                    {
                        if (system.Enabled) system.AfterUpdate(time);
                    });
                }
            }
            _timer.Stop();
        }

        ExecutionTime = _timer.Elapsed;
    }

    private static bool AnyEnabled<T>(T[] systems)
        where T : ISystem
    {
        foreach (var system in systems)
            if (system.Enabled)
                return true;
        return false;
    }

    public void Dispose()
    {
        foreach (var system in _systems)
            system.Dispose();
    }
}