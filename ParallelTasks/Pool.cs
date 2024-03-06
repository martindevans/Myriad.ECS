using System.Collections.Concurrent;

namespace ParallelTasks;

/// <summary>
/// A thread safe, non-blocking, object pool.
/// </summary>
/// <typeparam name="T">The type of item to store. Must be a class with a parameterless constructor.</typeparam>
internal class Pool<T>
    where T: class, new()
{
    public static Pool<T> Instance { get; } = new();

    private readonly ConcurrentBag<T> _bag = [];

    private Pool()
    {
    }

    /// <summary>
    /// Gets an instance from the pool.
    /// </summary>
    /// <returns>An instance of <typeparamref name="T"/>.</returns>
    public T Get()
    {
        if (_bag.TryTake(out var item))
            return item;
        return new T();
    }

    /// <summary>
    /// Returns an instance to the pool, so it is available for re-use.
    /// It is advised that the item is reset to a default state before being returned.
    /// </summary>
    /// <param name="instance">The instance to return to the pool.</param>
    public void Return(T instance)
    {
        _bag.Add(instance);
    }
}