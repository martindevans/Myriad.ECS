using System.Collections;

namespace Myriad.ECS.Allocations;

/// <summary>
/// Thread safe global pool.
/// </summary>
/// <typeparam name="T"></typeparam>
public static class Pool<T>
    where T : class, new()
{
    private const int MAX = 64;
    [ThreadStatic] private static List<T>? _items;

    public static T Get()
    {
        // Initialize item. This can't be done in the field initializer for a threadstatic field!
        _items ??= [];

        if (_items.Count == 0)
            return new();

        var item = _items[^1];
        _items.RemoveAt(_items.Count - 1);
        return item;
    }

    public static Rental Rent()
    {
        return new Rental(Get());
    }

    public static void Return(T item)
    {
        // Initialize item. This can't be done in the field initializer for a threadstatic field!
        _items ??= [];

        if (_items.Count < MAX)
            _items.Add(item);
    }

    public readonly struct Rental(T value)
        : IDisposable
    {
        public T Value { get; } = value;

        public void Dispose()
        {
            Pool<T>.Return(Value);
        }
    }
}

/// <summary>
/// Thread safe global pool.
/// </summary>
public static class Pool
{
    public static void Return<T>(T item)
        where T : class, new()
    {
        Pool<T>.Return(item);
    }
}