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

    private GameTime? _gameTime;
    private readonly Action<ISystemBefore> _beforeUpdate;
    private readonly Action<ISystem> _update;
    private readonly Action<ISystemAfter> _afterUpdate;

    public ParallelSystemGroup(string name, params ISystem[] systems)
    {
        Name = name;

        _beforeSystems = systems.OfType<ISystemBefore>().ToArray();
        _systems = systems;
        _afterSystems = systems.OfType<ISystemAfter>().ToArray();

        // Create delegates now, capturing "this". This avoids creating ne delegates every frame.
        _gameTime = null;
        _beforeUpdate = system =>
        {
            if (system.Enabled)
                system.BeforeUpdate(_gameTime!);
        };
        _update = system =>
        {
            if (system.Enabled)
                system.Update(_gameTime!);
        };
        _afterUpdate = system =>
        {
            if (system.Enabled)
                system.Update(_gameTime!);
        };
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
                if (_beforeSystems.Length > 0 && AnyEnabled(_afterSystems))
                {
                    _gameTime = time;
                    Parallel.ForEach(_beforeSystems, _beforeUpdate);
                    _gameTime = null;
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
                    _gameTime = time;
                    Parallel.ForEach(_systems, _update);
                    _gameTime = null;
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
                    _gameTime = time;
                    Parallel.ForEach(_afterSystems, _afterUpdate);
                    _gameTime = null;
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
        foreach (var system in _systems.OfType<IDisposable>())
            system.Dispose();
    }
}