namespace Myriad.ECS.Components;

/// <summary>
/// Store an array reference in a component
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="Array"></param>
public record struct EntityArray<T>(T[] Array)
    : IComponent
{
    /// <summary>
    /// Get the length of the array
    /// </summary>
    public readonly int Length => Array.Length;

    /// <summary>
    /// Index into the array
    /// </summary>
    /// <param name="Index"></param>
    /// <returns></returns>
    public ref T this[int Index] => ref Array[Index];
}