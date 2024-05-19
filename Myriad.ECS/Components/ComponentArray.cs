namespace Myriad.ECS.Components;

/// <summary>
/// Store an array reference in a component
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly record struct ComponentArray<T>
    : IComponent
{
    /// <summary>
    /// The array
    /// </summary>
    public T[] Array { get; }

    /// <summary>
    /// Get the length of the array
    /// </summary>
    public int Length => Array.Length;

    /// <summary>
    /// Index into the array
    /// </summary>
    /// <param name="Index"></param>
    /// <returns></returns>
    public ref T this[int Index] => ref Array[Index];

    public ComponentArray(T[] array)
    {
        Array = array;
    }

    public static ComponentArray<T> Create(int length, T template)
    {
        var arr = new ComponentArray<T>(new T[length]);
        System.Array.Fill(arr.Array, template);
        return arr;
    }
}

/// <summary>
/// Store a list in a component
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly record struct ComponentList<T>
    : IComponent
{
    /// <summary>
    /// The array
    /// </summary>
    public List<T> List { get; }

    /// <summary>
    /// Get the length of the list
    /// </summary>
    public int Count => List.Count;

    /// <summary>
    /// Index into the array
    /// </summary>
    /// <param name="Index"></param>
    /// <returns></returns>
    public T this[int Index]
    { 
        get => List[Index];
        set => List[Index] = value;
    }

    public ComponentList(List<T> list)
    {
        List = list;
    }

    public static ComponentList<T> Create(int length, T template)
    {
        var l = new List<T>(length);
        for (var i = 0; i < length; i++)
            l.Add(template);

        return new ComponentList<T>(l);
    }
}