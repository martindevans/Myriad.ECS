namespace Myriad.ECS.Systems;

public interface ISystemGroup<in TData>
    : ISystemInit<TData>, ISystemBefore<TData>, ISystemAfter<TData>, IDisposable
{
    /// <summary>
    /// A unique identifier for this system group
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Total time spent executing update calls in the last frame
    /// </summary>
    TimeSpan ExecutionTime { get; }

    /// <summary>
    /// Get all systems in this group
    /// </summary>
    IEnumerable<ISystem<TData>> Systems { get; }
}