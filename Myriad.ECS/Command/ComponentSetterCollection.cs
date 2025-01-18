using System.Diagnostics;
using Myriad.ECS.Allocations;
using Myriad.ECS.Components;
using Myriad.ECS.IDs;
using Myriad.ECS.Worlds.Archetypes;

namespace Myriad.ECS.Command;

/// <summary>
/// Collection of components of all different types, keyed by ComponentID, involves no boxing
/// </summary>
internal class ComponentSetterCollection
{
    private readonly Dictionary<ComponentID, IComponentList> _components = [ ];

    public void Clear()
    {
        foreach (var (_, value) in _components)
            value.Recycle();
        _components.Clear();
    }

    public void ClearAndDispose(ref LazyCommandBuffer buffer)
    {
        foreach (var (cid, components) in _components)
        {
            if (!cid.IsDisposableComponent)
                continue;
            components.DisposeAll(ref buffer);
        }

        Clear();
    }

    public SetterId Add<T>(T value)
        where T : IComponent
    {
        var id = ComponentID<T>.ID;

        if (!_components.TryGetValue(id, out var list))
        {
            list = Pool<GenericComponentList<T>>.Get();
            list.Clear();
            _components.Add(id, list);
        }

        var idx = ((GenericComponentList<T>)list).Add(value);
        return new SetterId(id, idx);
    }

    public void Overwrite<T>(SetterId index, T value) where T : IComponent
    {
        var id = ComponentID<T>.ID;
        ((GenericComponentList<T>)_components[id]).Overwrite(index, value);
    }

    public void Discard<T>(T value) where T : IComponent
    {
        var id = ComponentID<T>.ID;
        ((GenericComponentList<T>)_components[id]).Discard(value);
    }

    public void Write(SetterId id, Row row)
    {
        var list = _components[id.ID];
        list.Write(id.Index, row);
    }

    /// <summary>
    /// Dispose all disposable components specified by the given sorted list
    /// </summary>
    /// <param name="sets"></param>
    /// <param name="buffer"></param>
    public void Dispose(Dictionary<ComponentID, SetterId>? sets, ref LazyCommandBuffer buffer)
    {
        if (sets != null)
        {
            foreach (var (cid, sid) in sets)
            {
                if (!cid.IsDisposableComponent)
                    continue;

                var list = _components[sid.ID];
                list.Dispose(sid.Index, ref buffer);
            }
        }
    }

    /// <summary>
    /// Dispose all disposable components that were not written to an entity
    /// </summary>
    /// <param name="lazy"></param>
    public void DisposeAllOverwritten(ref LazyCommandBuffer lazy)
    {
        foreach (var componentList in _components)
            componentList.Value.DisposeAllOverwritten(ref lazy);
    }

    public readonly struct SetterId
    {
        /// <summary>
        /// Component ID of the component being overwritten
        /// </summary>
        internal readonly ComponentID ID;

        /// <summary>
        /// Index of the setter in the setters list
        /// </summary>
        internal readonly int Index;

        internal SetterId(ComponentID id, int idx)
        {
            ID = id;
            Index = idx;
        }
    }

    #region component list
    private interface IComponentList
    {
        public void Clear();

        void Recycle();

        void Write(int index, Row dest);

        void Dispose(int index, ref LazyCommandBuffer buffer);

        void DisposeAllOverwritten(ref LazyCommandBuffer lazy);

        void DisposeAll(ref LazyCommandBuffer lazy);
    }

    [DebuggerDisplay("Count = {_values.Count}")]
    private class GenericComponentList<T>
        : IComponentList
        where T : IComponent
    {
        private static readonly IDisposer _disposer = Disposer<T>.Instance;

        private readonly List<T> _values = [ ];
        private readonly List<T> _overwrittenDisposableValues = [];

        public void Clear()
        {
            _values.Clear();
            _overwrittenDisposableValues.Clear();
        }

        public int Add(T value)
        {
            _values.Add(value);
            return _values.Count - 1;
        }

        public void Overwrite(SetterId index, T value)
        {
            if (index.ID.IsDisposableComponent)
                _overwrittenDisposableValues.Add(_values[index.Index]);

            _values[index.Index] = value;
        }

        public void Discard(T value)
        {
            if (_disposer.Component.IsDisposableComponent)
                _overwrittenDisposableValues.Add(value);
        }

        public void Recycle()
        {
            Pool.Return(this);
        }

        public void Write(int index, Row dest)
        {
            dest.GetMutable<T>() = _values[index];
        }

        public void Dispose(int index, ref LazyCommandBuffer buffer)
        {
            _disposer.Dispose(_values, index, ref buffer);
        }

        public void DisposeAllOverwritten(ref LazyCommandBuffer lazy)
        {
            _disposer.DisposeAll(_overwrittenDisposableValues, ref lazy);
            _overwrittenDisposableValues.Clear();
        }

        public void DisposeAll(ref LazyCommandBuffer lazy)
        {
            _disposer.DisposeAll(_values, ref lazy);
            _disposer.DisposeAll(_overwrittenDisposableValues, ref lazy);
        }
    }
    #endregion
}