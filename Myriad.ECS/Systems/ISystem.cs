namespace Myriad.ECS.Systems;

public interface ISystem<in TData>
{
    public void Update(TData data);
}

public abstract class BaseSystem<TData>
    : ISystem<TData>
{
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