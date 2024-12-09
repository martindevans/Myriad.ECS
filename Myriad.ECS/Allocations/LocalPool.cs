namespace Myriad.ECS.Allocations;

/// <summary>
/// A non-thread safe pool, backed by the global thread safe pool.
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly struct LocalPool<T>
    : IDisposable
    where T : class, new()
{
    private readonly List<T> _items = Pool<List<T>>.Get();
    private readonly int _maxSize;

    /// <summary>
    /// Create a new <see cref="LocalPool{T}"/>
    /// </summary>
    public LocalPool()
        : this(16)
    {
    }

    /// <summary>
    /// Create a new <see cref="LocalPool{T}"/>
    /// </summary>
    public LocalPool(int maxSize)
    {
        _items = Pool<List<T>>.Get();
        _maxSize = maxSize;
    }

    /// <summary>
    /// Get al item from this pool, fetches from the global pool if none are available
    /// </summary>
    /// <returns></returns>
    public T Get()
    {
        if (_items.Count == 0)
            return Pool<T>.Get();

        var item = _items[^1];
        _items.RemoveAt(_items.Count - 1);
        return item;
    }

    /// <summary>
    /// Return an item to this pool
    /// </summary>
    /// <param name="item"></param>
    public void Return(T item)
    {
        if (_items.Count < _maxSize)
            _items.Add(item);
        else
            Pool<T>.Return(item);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        foreach (var item in _items)
            Pool<T>.Return(item);
        _items.Clear();

        Pool<List<T>>.Return(_items);
    }
}