using System.Diagnostics;

namespace Myriad.ECS.Systems;

public sealed class ParallelSystemGroup<TData>
    : ISystemGroup<TData>
{
    public string Name { get; }
    public bool Enabled { get; set; } = true;

    public TimeSpan ExecutionTime { get; private set; }
    private readonly Stopwatch _timer = new();

    private readonly ISystemBefore<TData>[] _beforeSystems;
    private readonly ISystem<TData>[] _systems;
    private readonly ISystemAfter<TData>[] _afterSystems;

    private TData? _dataClosure;
    private readonly Action<ISystemBefore<TData>> _beforeUpdate;
    private readonly Action<ISystem<TData>> _update;
    private readonly Action<ISystemAfter<TData>> _afterUpdate;

    public ParallelSystemGroup(string name, params ISystem<TData>[] systems)
    {
        Name = name;

        _beforeSystems = systems.OfType<ISystemBefore<TData>>().ToArray();
        _systems = systems;
        _afterSystems = systems.OfType<ISystemAfter<TData>>().ToArray();

        // Create delegates now, capturing "this". This avoids creating ne delegates every frame.
        _dataClosure = default;
        _beforeUpdate = system =>
        {
            system.BeforeUpdate(_dataClosure!);
        };
        _update = system =>
        {
            system.Update(_dataClosure!);
        };
        _afterUpdate = system =>
        {
            system.Update(_dataClosure!);
        };
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
                if (_beforeSystems.Length > 0)
                {
                    _dataClosure = data;
                    Parallel.ForEach(_beforeSystems, _beforeUpdate);
                    _dataClosure = default;
                }
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
                _dataClosure = data;
                Parallel.ForEach(_systems, _update);
                _dataClosure = default;
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
                if (_afterSystems.Length > 0)
                {
                    _dataClosure = data;
                    Parallel.ForEach(_afterSystems, _afterUpdate);
                    _dataClosure = default;
                }
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

    public IEnumerable<ISystem<TData>> Systems
    {
        get
        {
            foreach (var system in _systems)
            {
                if (system is ISystemGroup<TData> group)
                {
                    foreach (var nested in group.Systems)
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