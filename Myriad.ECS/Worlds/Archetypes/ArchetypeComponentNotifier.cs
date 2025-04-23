using Myriad.ECS.Collections;
using Myriad.ECS.Components;
using Myriad.ECS.IDs;
using System.Reflection;

namespace Myriad.ECS.Worlds.Archetypes;

internal class ArchetypePhantomComponentNotifier
{
    private readonly List<IPhantomNotifier> _notifiers = [];

    public ArchetypePhantomComponentNotifier(FrozenOrderedListSet<ComponentID> components)
    {
        // Get a disposer for each disposable component
        foreach (var component in components)
        {
            if (!component.IsPhantomNotifierComponent)
                continue;
            _notifiers.Add(PhantomNotifier.Get(component));
        }
    }

    public void Notify(EntityId entity, EntityInfo info)
    {
        foreach (var notifier in _notifiers)
            notifier.Notify(entity, info);
    }

    private static class PhantomNotifier
    {
        [ThreadStatic] private static Dictionary<ComponentID, IPhantomNotifier>? _disposerCache;

        private static IPhantomNotifier Get(Type type)
        {
            var t = typeof(PhantomNotifier<>);
            var tg = t.MakeGenericType(type);
            var p = tg.GetProperty("Instance", BindingFlags.Static | BindingFlags.Public)!;
            var v = p.GetValue(null)!;

            return (IPhantomNotifier)v;
        }

        public static IPhantomNotifier Get(ComponentID id)
        {
            _disposerCache ??= [];

            if (!_disposerCache.TryGetValue(id, out var value))
            {
                value = Get(id.Type);
                _disposerCache[id] = value;
            }

            return value;
        }
    }

    private static class PhantomNotifier<T>
        where T : IComponent
    {
        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once StaticMemberInGenericType
        public static IPhantomNotifier Instance { get; } = GetInstance();

        private static IPhantomNotifier GetInstance()
        {
            var id = ComponentID<T>.ID;
            if (!id.IsPhantomNotifierComponent)
                throw new ArgumentException("Cannot get notifier for component which does not implement IPhantomNotifierComponent");

            return (IPhantomNotifier)Activator.CreateInstance(typeof(NotifierImpl<>).MakeGenericType(typeof(T), typeof(T)))!;
        }

        private class NotifierImpl<U>
            : IPhantomNotifier
            where U : IPhantomNotifierComponent
        {
            public void Notify(EntityId entity, EntityInfo info)
            {
                info.Chunk.GetRef<U>(info.RowIndex).OnBecomePhantom(entity);
            }
        }

    }

    private interface IPhantomNotifier
    {
        void Notify(EntityId entity, EntityInfo info);
    }
}