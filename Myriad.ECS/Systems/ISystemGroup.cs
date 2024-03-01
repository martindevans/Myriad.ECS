namespace Myriad.ECS.Systems;

public interface ISystemGroup
    : ISystem, ISystemBefore, ISystemAfter
{
    /// <summary>
    /// A unique identifier for this system group
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Total time spent executing update calls in the last frame
    /// </summary>
    TimeSpan ExecutionTime { get; }
}