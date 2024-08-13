using System.Diagnostics;

namespace Myriad.ECS.Systems;

/// <summary>
/// A group of systems that will be executed together. A group is itself a system, so groups can be nested.
/// </summary>
/// <typeparam name="TData"></typeparam>
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

    /// <summary>
    /// Type of <see cref="System"/>
    /// </summary>
    public Type Type { get; }

    private readonly Stopwatch _timer = new();

    /// <summary>
    /// Indicates if this system has a BeforeUpdate phase
    /// </summary>
    public bool HasBeforeUpdate => SystemBefore != null;

    /// <summary>
    /// Time elapsed in last BeforeUpdate phase
    /// </summary>
    public TimeSpan BeforeUpdateTime { get; private set; }

    /// <summary>
    /// Time elapsed in last Update phase
    /// </summary>
    public TimeSpan UpdateTime { get; private set; }

    /// <summary>
    /// Indicates if this system has a AfterUpdate phase
    /// </summary>
    public bool HasAfterUpdate => SystemBefore != null;

    /// <summary>
    /// Time elapsed in last AfterUpdate phase
    /// </summary>
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

    private readonly List<SystemGroupItem<TData>> _beforeSystems;
    private readonly List<SystemGroupItem<TData>> _systems;
    private readonly List<SystemGroupItem<TData>> _afterSystems;

    protected BaseSystemGroup(string name, params ISystem<TData>[] systems)
    {
        Name = name;

        var sys = systems.Select(a => new SystemGroupItem<TData>(a)).ToArray();

        _beforeSystems = sys.Where(a => a.System is ISystemBefore<TData>).ToList();
        _systems = sys.ToList();
        _afterSystems = sys.Where(a => a.System is ISystemAfter<TData>).ToList();
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

    protected SystemGroupItem<TData> Add(ISystem<TData> system)
    {
        var item = new SystemGroupItem<TData>(system);

        _systems.Add(item);

        if (system is ISystemBefore<TData>)
            _beforeSystems.Add(item);
        if (system is ISystemAfter<TData>)
            _afterSystems.Add(item);

        return item;
    }

    protected bool Remove(ISystem<TData> system)
    {
        for (var i = 0; i < _systems.Count; i++)
        {
            if (_systems[i].System == system)
            {
                _beforeSystems.Remove(_systems[i]);
                _afterSystems.Remove(_systems[i]);
                _systems.RemoveAt(i);
                return true;
            }
        }

        return false;
    }

    public void BeforeUpdate(TData data)
    {
        _timer.Reset();

        _timer.Start();
        BeforeUpdateInternal(_beforeSystems, data);
        _timer.Stop();
    }

    protected abstract void BeforeUpdateInternal(List<SystemGroupItem<TData>> systems, TData data);

    public void Update(TData data)
    {
        _timer.Start();
        UpdateInternal(_systems, data);
        _timer.Stop();
    }

    protected abstract void UpdateInternal(List<SystemGroupItem<TData>> systems, TData data);

    public void AfterUpdate(TData data)
    {
        _timer.Start();
        AfterUpdateInternal(_afterSystems, data);
        _timer.Stop();

        TotalExecutionTime = _timer.Elapsed;
    }

    protected abstract void AfterUpdateInternal(List<SystemGroupItem<TData>> systems, TData data);

    public IReadOnlyList<SystemGroupItem<TData>> Systems => _systems;

    IEnumerable<SystemGroupItem<TData>> ISystemGroup<TData>.Systems => _systems;

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