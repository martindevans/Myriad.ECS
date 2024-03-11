namespace Myriad.ECS.Systems;

public interface ISystem
{
    /// <summary>
    /// Indicates if this system is enabled. If false, all update calls will be skipped.
    /// </summary>
    bool Enabled { get; set; }

    public void Update(GameTime time);
}

public abstract class BaseSystem
    : ISystem
{
    public bool Enabled { get; set; } = true;

    public abstract void Update(GameTime time);
}

public interface ISystemInit
    : ISystem
{
    /// <summary>
    /// Called once when a system is first created
    /// </summary>
    public void Init();
}

public interface ISystemBefore
    : ISystem
{
    public void BeforeUpdate(GameTime time);
}

public interface ISystemAfter
    : ISystem
{
    public void AfterUpdate(GameTime time);
}