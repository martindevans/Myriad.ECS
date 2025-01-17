using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Myriad.ECS.Command;
using Myriad.ECS.IDs;
using Myriad.ECS.Worlds.Chunks;

namespace Myriad.ECS.Components;

/// <summary>
/// Automatically has Dispose() called when this component is destroyed. Either because the Entity
/// was destroyed or because the component was removed from the Entity.
/// </summary>
public interface IDisposableComponent
    : IComponent
{
    /// <summary>
    /// Dispose this component
    /// </summary>
    /// <param name="buffer">May be used to enqueue more work as a result of this disposal</param>
    public void Dispose(ref LazyCommandBuffer buffer);
}

/// <summary>
/// A basic container for disposable objects which will automatically be disposed when the entity is destroyed
/// </summary>
public struct GenericDisposable<TDisposable>
    : IDisposableComponent
    where TDisposable : IDisposable
{
    /// <summary>
    /// The object that will be disposed
    /// </summary>
    public TDisposable? IDisposable;

    /// <summary>
    /// Create a new <see cref="GenericDisposable{TDisposable}"/> component
    /// </summary>
    /// <param name="disposable"></param>
    public GenericDisposable(TDisposable disposable)
    {
        IDisposable = disposable;
    }

    /// <inheritdoc />
    public void Dispose(ref LazyCommandBuffer buffer)
    {
        IDisposable?.Dispose();
        IDisposable = default;
    }
}

internal static class Disposer
{
    [ThreadStatic] private static Dictionary<ComponentID, IDisposer>? _disposerCache;

    public static IDisposer Get(Type type)
    {
        var t = typeof(Disposer<>);
        var tg = t.MakeGenericType(type);
        var p = tg.GetProperty("Instance", BindingFlags.Static | BindingFlags.Public)!;
        var v = p.GetValue(null)!;

        return (IDisposer)v;
    }

    public static IDisposer Get(ComponentID id)
    {
        _disposerCache ??= [ ];

        if (!_disposerCache.TryGetValue(id, out var value))
        {
            value = Get(id.Type);
            _disposerCache[id] = value;
        }

        return value;
    }
}

/// <summary>
/// Provides a generic way to dispose _any_ component. Does nothing for non-disposable components.
/// </summary>
/// <typeparam name="T"></typeparam>
internal static class Disposer<T>
    where T : IComponent
{
    public static IDisposer Instance { get; } = GetInstance();

    private static IDisposer GetInstance()
    {
        var id = ComponentID<T>.ID;

        if (!id.IsDisposableComponent)
            return new EmptyImpl();

        return (IDisposer)Activator.CreateInstance(typeof(DisposerImpl<>).MakeGenericType(typeof(T), typeof(T)))!;
    }

    private class DisposerImpl<U>
        : IDisposer
        where U : IDisposableComponent
    {
        public ComponentID Component { get; } = ComponentID<U>.ID;

        //public void Dispose(Array array, int index, ref LazyCommandBuffer buffer)
        //{
        //    var arr = (U[])array;
        //    arr[index].Dispose(ref buffer);
        //}

        public void Dispose(IList list, int index, ref LazyCommandBuffer buffer)
        {
            var arr = (List<U>)list;
            arr[index].Dispose(ref buffer);
        }

        public void Dispose(Chunk chunk, int rowIndex, ref LazyCommandBuffer buffer)
        {
            chunk.GetRef<U>(rowIndex).Dispose(ref buffer);
        }

        public void DisposeAll(IList list, ref LazyCommandBuffer buffer)
        {
            var arr = (List<U>)list;
            foreach (var component in arr)
                component.Dispose(ref buffer);
        }
    }

    [ExcludeFromCodeCoverage]
    private class EmptyImpl
        : IDisposer
    {
        public ComponentID Component { get; } = ComponentID<T>.ID;

        public void Dispose(IList list, int index, ref LazyCommandBuffer buffer)
        {
        }

        public void Dispose(Chunk chunk, int rowIndex, ref LazyCommandBuffer buffer)
        {
        }

        public void DisposeAll(IList list, ref LazyCommandBuffer buffer)
        {
        }
    }
}

internal interface IDisposer
{
    public ComponentID Component { get; }

    void Dispose(IList list, int index, ref LazyCommandBuffer buffer);

    void Dispose(Chunk chunk, int rowIndex, ref LazyCommandBuffer buffer);

    void DisposeAll(IList list, ref LazyCommandBuffer buffer);
}