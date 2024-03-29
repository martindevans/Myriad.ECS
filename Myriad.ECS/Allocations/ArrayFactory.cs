using System.Reflection;

namespace Myriad.ECS.Allocations;

internal static class ArrayFactory
{
    [ThreadStatic] private static Dictionary<Type, Func<int, Array>>? _factories;

    /// <summary>
    /// Prepare this type so that arrays of it can be constructed later
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static void Prepare<T>()
    {
        _factories ??= [ ];

        if (!_factories.ContainsKey(typeof(T)))
            _factories.Add(typeof(T), Create<T>);
    }

    /// <summary>
    /// Prepare this type so that arrays of it can be constructed later
    /// </summary>
    public static void Prepare(Type type)
    {
        typeof(ArrayFactory).GetMethod("Prepare", BindingFlags.Public | BindingFlags.Static, null, CallingConventions.Any, [], null)!
                            .MakeGenericMethod(type)
                            .Invoke(null, null);
    }

    /// <summary>
    /// Create an array of the given type
    /// </summary>
    /// <param name="type"></param>
    /// <param name="capacity"></param>
    /// <returns></returns>
    public static Array Create(Type type, int capacity)
    {
        if (_factories != null && _factories.TryGetValue(type, out var factory))
            return factory(capacity);

        return Array.CreateInstance(type, capacity);
    }

    private static T[] Create<T>(int capacity)
    {
        return capacity == 0
             ? []
             : new T[capacity];
    }
}