namespace Myriad.ECS.Components;

/// <summary>
/// Store an array reference in a component
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="Array"></param>
public record struct ComponentArray<T>(T[] Array)
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
    public readonly ref T this[int Index] => ref Array[Index];
}

/// <summary>
/// Store an list in a component
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="List"></param>
public record struct ComponentList<T>(List<T> List)
    : IComponent
{
    /// <summary>
    /// Get the length of the list
    /// </summary>
    public readonly int Count => List.Count;

    /// <summary>
    /// Index into the array
    /// </summary>
    /// <param name="Index"></param>
    /// <returns></returns>
    public readonly T this[int Index]
    { 
        get => List[Index];
        set => List[Index] = value;
    }
}