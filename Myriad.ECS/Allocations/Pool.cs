using System.Diagnostics.CodeAnalysis;

// ReSharper disable StaticMemberInGenericType

namespace Myriad.ECS.Allocations;

/// <summary>
/// Thread safe global pool.
/// </summary>
/// <typeparam name="T"></typeparam>
public static class Pool<T>
    where T : class, new()
{
    [ThreadStatic] private static int _maxSize;
    [ThreadStatic] private static int _pressure;
    [ThreadStatic] private static List<T>? _items;

    [MemberNotNull(nameof(_items))]
    private static void Init()
    {
        // Initialize item. This can't be done in the field initializer for a threadstatic field!
        if (_items == null)
        {
            _items = [];
            _maxSize = 1024;
        }
    }

    public static T Get()
    {
        Init();

        if (_items.Count == 0)
        {
            // Every allocation significantly increases pressure
            _pressure += 8;
            return new();
        }

        // Every hit of the pool slightly reduces pressure
        if (_pressure > 0)
            _pressure--;

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
        Init();

        if (_pressure > _maxSize)
        {
            _maxSize *= 2;
            _pressure = 0;
        }

        if (_items.Count < _maxSize)
            _items.Add(item);
    }

    public readonly struct Rental(T value)
        : IDisposable
    {
        public T Value { get; } = value;

        public void Dispose()
        {
            Return(Value);
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