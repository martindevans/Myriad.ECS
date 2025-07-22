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
    /// Enable or disable this system. Disables systems will not receive any update calls (including early and late)
    /// </summary>
    bool Enabled { get; set; }

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

/// <summary>
/// A single item in a <see cref="SystemGroup{TData}"/>
/// </summary>
public sealed class SystemGroupItem<TData>
{
    /// <summary>
    /// Indicates if update calls will be made to this system.
    /// </summary>
    public bool Enabled { get; set; } = true;

    private ISystemBefore<TData>? SystemBefore { get; }
    private ISystemAfter<TData>? SystemAfter { get; }

    /// <summary>
    /// Get the system that this item represents
    /// </summary>
    public ISystem<TData> System { get; }

    /// <summary>
    /// Type of <see cref="System"/>
    /// </summary>
    public Type Type { get; }

    private readonly Stopwatch _timer = new();

    /// <summary>
    /// Indicates if this system has a BeforeUpdate phase
    /// </summary>
    public bool HasBeforeUpdate { get; }

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
    public bool HasAfterUpdate { get; }

    /// <summary>
    /// Time elapsed in last AfterUpdate phase
    /// </summary>
    public TimeSpan AfterUpdateTime { get; private set; }

    /// <summary>
    /// Create a new <see cref="SystemGroupItem{TData}"/> wrapping the given system
    /// </summary>
    /// <param name="system"></param>
    public SystemGroupItem(ISystem<TData> system)
    {
        SystemBefore = system as ISystemBefore<TData>;
        SystemAfter = system as ISystemAfter<TData>;
        System = system;

        HasBeforeUpdate = SystemBefore != null;
        HasAfterUpdate = SystemAfter != null;

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

/// <summary>
/// Base class for a group of systems executed together
/// </summary>
/// <typeparam name="TData"></typeparam>
public abstract class BaseSystemGroup<TData>
    : ISystemGroup<TData>
{
    /// <summary>
    /// The name of this group
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Total time consumed in this system in the previous update call
    /// </summary>
    public TimeSpan TotalExecutionTime { get; private set; }

    private readonly Stopwatch _timer = new();

    private readonly List<SystemGroupItem<TData>> _beforeSystems;
    private readonly List<SystemGroupItem<TData>> _systems;
    private readonly List<SystemGroupItem<TData>> _afterSystems;

    /// <inheritdoc />
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Create a new system group
    /// </summary>
    /// <param name="name"></param>
    /// <param name="systems"></param>
    protected BaseSystemGroup(string name, params ISystem<TData>[] systems)
    {
        Name = name;

        var sys = systems.Select(a => new SystemGroupItem<TData>(a)).ToArray();

        _beforeSystems = sys.Where(a => a.System is ISystemBefore<TData>).ToList();
        _systems = [.. sys];
        _afterSystems = sys.Where(a => a.System is ISystemAfter<TData>).ToList();
    }

    /// <summary>
    /// Call Init on all systems in this group which implement <see cref="ISystemInit{TData}"/>
    /// </summary>
    public void Init()
    {
        foreach (var system in _systems)
            (system.System as ISystemInit<TData>)?.Init();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        foreach (var system in _systems.Select(a => a.System as IDisposable))
            system?.Dispose();

        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Add a new system to the end of this group
    /// </summary>
    /// <param name="system"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Remove a system from this group
    /// </summary>
    /// <param name="system"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Call BeforeUpdate on all systems in this group
    /// </summary>
    /// <param name="data"></param>
    public void BeforeUpdate(TData data)
    {
        _timer.Reset();

        if (!Enabled)
            return;

        _timer.Start();
        BeforeUpdateInternal(_beforeSystems, data);
        _timer.Stop();
    }

    /// <summary>
    /// Called by BeforeUpdate, should implement the actual call to the systems
    /// </summary>
    /// <param name="systems"></param>
    /// <param name="data"></param>
    protected abstract void BeforeUpdateInternal(List<SystemGroupItem<TData>> systems, TData data);

    /// <summary>
    /// Call Update on all systems in this group
    /// </summary>
    /// <param name="data"></param>
    public void Update(TData data)
    {
        if (!Enabled)
            return;

        _timer.Start();
        UpdateInternal(_systems, data);
        _timer.Stop();
    }

    /// <summary>
    /// Called by Update, should implement the actual call to the systems
    /// </summary>
    /// <param name="systems"></param>
    /// <param name="data"></param>
    protected abstract void UpdateInternal(List<SystemGroupItem<TData>> systems, TData data);

    /// <summary>
    /// Call AfterUpdate on all systems in this group
    /// </summary>
    /// <param name="data"></param>
    public void AfterUpdate(TData data)
    {
        if (!Enabled)
            return;

        _timer.Start();
        AfterUpdateInternal(_afterSystems, data);
        _timer.Stop();

        TotalExecutionTime = _timer.Elapsed;
    }

    /// <summary>
    /// Called by AfterUpdate, should implement the actual call to the systems
    /// </summary>
    /// <param name="systems"></param>
    /// <param name="data"></param>
    protected abstract void AfterUpdateInternal(List<SystemGroupItem<TData>> systems, TData data);

    /// <summary>
    /// All the systems in this group
    /// </summary>
    public IReadOnlyList<SystemGroupItem<TData>> Systems => _systems;

    IEnumerable<SystemGroupItem<TData>> ISystemGroup<TData>.Systems => _systems;

    /// <summary>
    /// All the systems in this group, if any of the systems are a group they are expanded to their inner
    /// systems recursively.
    /// </summary>
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