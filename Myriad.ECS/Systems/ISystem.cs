namespace Myriad.ECS.Systems;

public interface ISystem
    : IDisposable
{
    /// <summary>
    /// Indicates if this system is enabled. If false, all update calls will be skipped.
    /// </summary>
    bool Enabled { get; set; }

    /// <summary>
    /// Called once when a system is first created, before the first calls to <see cref="Update"/>
    /// </summary>
    public void Init();

    public void Update(GameTime time);
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