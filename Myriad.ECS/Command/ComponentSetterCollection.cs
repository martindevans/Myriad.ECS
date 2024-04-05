using System.Diagnostics;
using Myriad.ECS.Allocations;
using Myriad.ECS.Extensions;
using Myriad.ECS.IDs;
using Myriad.ECS.Worlds.Archetypes;

namespace Myriad.ECS.Command;

/// <summary>
/// Collection of components of all different types, keyed by ComponentID, involves no boxing
/// </summary>
internal class ComponentSetterCollection
{
    private readonly SortedList<ComponentID, IComponentList> _components = [ ];

    public void Clear()
    {
        foreach (var (_, value) in _components.Enumerable())
            value.Recycle();
        _components.Clear();
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

    public void Write(SetterId id, Row row)
    {
        var list = _components[id.ID];
        list.Write(id.Index, row);
    }

    public readonly struct SetterId
    {
        internal readonly ComponentID ID;
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
    }

    [DebuggerDisplay("Count = {_values.Count}")]
    private class GenericComponentList<T>
        : IComponentList
        where T : IComponent
    {
        private readonly List<T> _values = [ ];

        public void Clear()
        {
            _values.Clear();
        }

        public int Add(T value)
        {
            _values.Add(value);
            return _values.Count - 1;
        }

        public void Overwrite(SetterId index, T value)
        {
            _values[index.Index] = value;
        }

        public void Recycle()
        {
            Pool.Return(this);
        }

        public void Write(int index, Row dest)
        {
            dest.GetMutable<T>() = _values[index];
        }
    }
    #endregion
}