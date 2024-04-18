using System.Diagnostics;

namespace Myriad.ECS.Systems;

public interface ISystemGroup<TData>
    : ISystemInit<TData>, ISystemBefore<TData>, ISystemAfter<TData>, IDisposable
{
    /// <summary>
    /// A unique identifier for this system group
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Total time spent executing all update calls in the last frame
    /// </summary>
    TimeSpan TotalExecutionTime { get; }

    /// <summary>
    /// Get all systems in this group
    /// </summary>
    IEnumerable<SystemGroupItem<TData>> Systems { get; }

    /// <summary>
    /// Get all systems in this group, expanding out all system groups into contained systems
    /// </summary>
    IEnumerable<SystemGroupItem<TData>> RecursiveSystems { get; }

    /// <summary>
    /// Get a system of the given type
    /// </summary>
    /// <typeparam name="TSys"></typeparam>
    /// <returns></returns>
    TSys? TryGet<TSys>() where TSys : ISystem<TData>
    {
        return RecursiveSystems.Select(a => a.System).OfType<TSys>().FirstOrDefault();
    }
}

public class SystemGroupItem<TData>
{
    public bool Enabled { get; set; } = true;

    private ISystemBefore<TData>? SystemBefore { get; }
    private ISystemAfter<TData>? SystemAfter { get; }
    public ISystem<TData> System { get; }

    public Type Type { get; }

    private readonly Stopwatch _timer = new();
    public TimeSpan BeforeUpdateTime { get; private set; }
    public TimeSpan UpdateTime { get; private set; }
    public TimeSpan AfterUpdateTime { get; private set; }

    public SystemGroupItem(ISystem<TData> system)
    {
        SystemBefore = system as ISystemBefore<TData>;
        SystemAfter = system as ISystemAfter<TData>;
        System = system;

        Type = system.GetType();
    }

    internal void BeforeUpdate(TData data)
    {
        _timer.Restart();
        if (Enabled)
            SystemBefore?.BeforeUpdate(data);
        BeforeUpdateTime = _timer.Elapsed;
    }

    internal void Update(TData data)
    {
        _timer.Restart();
        if (Enabled)
            System.Update(data);
        UpdateTime = _timer.Elapsed;
    }

    internal void AfterUpdate(TData data)
    {
        _timer.Restart();
        if (Enabled)
            SystemAfter?.AfterUpdate(data);
        AfterUpdateTime = _timer.Elapsed;
    }
}

public abstract class BaseSystemGroup<TData>
    : ISystemGroup<TData>
{
    public string Name { get; }

    public TimeSpan TotalExecutionTime { get; private set; }

    private readonly Stopwatch _timer = new();

    private readonly SystemGroupItem<TData>[] _beforeSystems;
    private readonly SystemGroupItem<TData>[] _systems;
    private readonly SystemGroupItem<TData>[] _afterSystems;

    protected BaseSystemGroup(string name, params ISystem<TData>[] systems)
    {
        Name = name;

        var sys = systems.Select(a => new SystemGroupItem<TData>(a)).ToArray();

        _beforeSystems = sys.Where(a => a.System is ISystemBefore<TData>).ToArray();
        _systems = sys;
        _afterSystems = sys.Where(a => a.System is ISystemAfter<TData>).ToArray();
    }

    public void Init()
    {
        foreach (var system in _systems)
            (system.System as ISystemInit<TData>)?.Init();
    }

    public void Dispose()
    {
        foreach (var system in _systems.Select(a => a.System as IDisposable))
            system?.Dispose();

        GC.SuppressFinalize(this);
    }

    public void BeforeUpdate(TData data)
    {
        _timer.Reset();

        _timer.Start();
        BeforeUpdateInternal(_beforeSystems, data);
        _timer.Stop();
    }

    protected abstract void BeforeUpdateInternal(SystemGroupItem<TData>[] systems, TData data);

    public void Update(TData data)
    {
        _timer.Start();
        UpdateInternal(_systems, data);
        _timer.Stop();
    }

    protected abstract void UpdateInternal(SystemGroupItem<TData>[] systems, TData data);

    public void AfterUpdate(TData data)
    {
        _timer.Start();
        AfterUpdateInternal(_afterSystems, data);
        _timer.Stop();

        TotalExecutionTime = _timer.Elapsed;
    }

    protected abstract void AfterUpdateInternal(SystemGroupItem<TData>[] systems, TData data);

    public IEnumerable<SystemGroupItem<TData>> Systems => _systems;

    public IEnumerable<SystemGroupItem<TData>> RecursiveSystems
    {
        get
        {
            foreach (var system in _systems)
            {
                if (system.System is ISystemGroup<TData> group)
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