namespace Myriad.ECS.Systems;

public interface ISystem<in TData>
{
    /// <summary>
    /// Indicates if this system is enabled. If false, all update calls will be skipped.
    /// </summary>
    bool Enabled { get; set; }

    public void Update(TData time);
}

public abstract class BaseSystem<TData>
    : ISystem<TData>
{
    public bool Enabled { get; set; } = true;

    public abstract void Update(TData data);
}

public interface ISystemInit<in TData>
    : ISystem<TData>
{
    /// <summary>
    /// Called once when a system is first created
    /// </summary>
    public void Init();
}

public interface ISystemBefore<in TData>
    : ISystem<TData>
{
    public void BeforeUpdate(TData data);
}

public interface ISystemAfter<in TData>
    : ISystem<TData>
{
    public void AfterUpdate(TData data);
}