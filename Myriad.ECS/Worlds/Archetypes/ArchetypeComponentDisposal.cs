using Myriad.ECS.Collections;
using Myriad.ECS.Command;
using Myriad.ECS.Components;
using Myriad.ECS.IDs;
using Myriad.ECS.Worlds.Chunks;

namespace Myriad.ECS.Worlds.Archetypes;

internal class ArchetypeComponentDisposal
{
    private readonly List<IDisposer> _disposers = [ ];

    public ArchetypeComponentDisposal(FrozenOrderedListSet<ComponentID> components)
    {
        // Get a disposer for each disposable component
        foreach (var component in components)
        {
            if (!component.IsDisposableComponent)
                continue;
            _disposers.Add(Disposer.Get(component));
        }
    }

    public void DisposeEntity(ref LazyCommandBuffer buffer, EntityInfo info)
    {
        DisposeEntity(ref buffer, info.Chunk, info.RowIndex);
    }

    public void DisposeEntity(ref LazyCommandBuffer buffer, Chunk chunk, int rowIndex)
    {
        foreach (var disposer in _disposers)
            disposer.Dispose(chunk, rowIndex, ref buffer);
    }

    /// <summary>
    /// Dispose components which are not in the destination archetype
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="info"></param>
    /// <param name="to"></param>
    public void DisposeRemoved(ref LazyCommandBuffer buffer, EntityInfo info, FrozenOrderedListSet<ComponentID> to)
    {
        foreach (var disposer in _disposers)
            if (!to.Contains(disposer.Component))
                disposer.Dispose(info.Chunk, info.RowIndex, ref buffer);
    }
}