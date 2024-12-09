using System.Diagnostics;
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

    /// <summary>
    /// Get an item from this pool, creates a new one if there are none in the pool
    /// </summary>
    /// <returns></returns>
    public static T Get()
    {
        Init();
        Debug.Assert(_items != null);

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

    /// <summary>
    /// Get an item from this pool, creates a new one if there are none in the pool
    /// </summary>
    /// <returns>A <see cref="Rental"/> contains the borrowed object and will return it when disposed</returns>
    public static Rental Rent()
    {
        return new Rental(Get());
    }

    /// <summary>
    /// Return an item to the pool
    /// </summary>
    /// <param name="item"></param>
    public static void Return(T item)
    {
        Init();
        Debug.Assert(_items != null);

        if (_pressure > _maxSize)
        {
            _maxSize *= 2;
            _pressure = 0;
        }

        if (_items.Count < _maxSize)
            _items.Add(item);
    }

    /// <summary>
    /// Contains an object borrowed from a pool, returns it when disposed
    /// </summary>
    /// <param name="value"></param>
    public readonly struct Rental(T value)
        : IDisposable
    {
        /// <summary>
        /// The borrowed object
        /// </summary>
        public T Value { get; } = value;

        /// <inheritdoc />
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
    /// <summary>
    /// Return an item to the pool
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="item"></param>
    public static void Return<T>(T item)
        where T : class, new()
    {
        Pool<T>.Return(item);
    }
}