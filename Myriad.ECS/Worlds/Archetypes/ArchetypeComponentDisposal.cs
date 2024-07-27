using Myriad.ECS.Collections;
using Myriad.ECS.Components;
using Myriad.ECS.IDs;
using Myriad.ECS.Worlds.Chunks;

namespace Myriad.ECS.Worlds.Archetypes;

internal class ArchetypeComponentDisposal
{
    [ThreadStatic] private static Dictionary<ComponentID, IDisposer>? _disposerCache;

    private readonly List<IDisposer> _disposers = [ ];

    public ArchetypeComponentDisposal(FrozenOrderedListSet<ComponentID> components)
    {
        // Get a disposer for each disposable component
        foreach (var component in components)
        {
            if (!component.IsDisposableComponent)
                continue;
            _disposers.Add(GetDisposer(component));
        }
    }

    private static IDisposer GetDisposer(ComponentID component)
    {
        // Thread static must be initialised here, not in the field initialiser
        _disposerCache ??= [];

        if (!_disposerCache.TryGetValue(component, out var disposer))
        {
            disposer = (IDisposer)typeof(Disposer<>).MakeGenericType(component.Type).GetConstructor([])!.Invoke(null);
            _disposerCache.Add(component, disposer);
        }

        return disposer;
    }

    public void DisposeEntity(EntityInfo info)
    {
        DisposeEntity(info.Chunk, info.RowIndex);
    }

    public void DisposeEntity(Chunk chunk, int rowIndex)
    {
        foreach (var disposer in _disposers)
            disposer.Dispose(chunk, rowIndex);
    }

    public void DisposeRemoved(EntityInfo info, FrozenOrderedListSet<ComponentID> to)
    {
        foreach (var disposer in _disposers)
            if (!to.Contains(disposer.Component))
                disposer.Dispose(info.Chunk, info.RowIndex);
    }

    private class Disposer<TComponent>
        : IDisposer
        where TComponent : IDisposableComponent
    {
        public ComponentID Component { get; } = ComponentID<TComponent>.ID;

        public void Dispose(Chunk chunk, int rowIndex)
        {
            chunk.GetRef<TComponent>(rowIndex, Component).Dispose();
        }
    }

    private interface IDisposer
    {
        ComponentID Component { get; }

        void Dispose(Chunk chunk, int rowIndex);
    }
}