namespace Myriad.ECS.Systems;

/// <summary>
/// Base interface for all systems
/// </summary>
/// <typeparam name="TData"></typeparam>
public interface ISystem<in TData>
{
    /// <summary>
    /// Called one per tick
    /// </summary>
    /// <param name="data"></param>
    public void Update(TData data);
}

/// <summary>
/// Add one time setup to a system
/// </summary>
/// <typeparam name="TData"></typeparam>
public interface ISystemInit<in TData>
    : ISystem<TData>
{
    /// <summary>
    /// Called once when a system is first created
    /// </summary>
    public void Init();
}

/// <summary>
/// Updates this system just before the normal Update phase
/// </summary>
/// <typeparam name="TData"></typeparam>
public interface ISystemBefore<in TData>
    : ISystem<TData>
{
    /// <summary>
    /// Called one per tick, before the normal Update call
    /// </summary>
    /// <param name="data"></param>
    public void BeforeUpdate(TData data);
}

/// <summary>
/// Updates this system just after the normal Update phase
/// </summary>
/// <typeparam name="TData"></typeparam>
public interface ISystemAfter<in TData>
    : ISystem<TData>
{
    /// <summary>
    /// Called one per tick, after the normal Update call
    /// </summary>
    /// <param name="data"></param>
    public void AfterUpdate(TData data);
}