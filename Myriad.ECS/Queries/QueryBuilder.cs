using Myriad.ECS.Registry;
using System.Runtime.CompilerServices;
using Myriad.ECS.IDs;
using Myriad.ECS.Worlds;
using Myriad.ECS.Collections;

namespace Myriad.ECS.Queries;

/// <summary>
/// Build a new <see cref="QueryDescription"/> object
/// </summary>
public sealed partial class QueryBuilder
{
    private readonly IDSet<ComponentRegistry, IComponent, ComponentID> _include;
    private readonly IDSet<ComponentRegistry, IComponent, ComponentID> _exclude;
    private readonly IDSet<ComponentRegistry, IComponent, ComponentID> _atLeastOne;
    private readonly IDSet<ComponentRegistry, IComponent, ComponentID> _exactlyOne;

    public QueryBuilder()
    {
        _include = new(ContainsComponent, 0);
        _exclude = new(ContainsComponent, 1);
        _atLeastOne = new(ContainsComponent, 2);
        _exactlyOne = new(ContainsComponent, 3);
    }

    /// <summary>
    /// Build a <see cref="QueryDescription"/> from the current state of this builder
    /// </summary>
    /// <param name="world"></param>
    /// <returns></returns>
    public QueryDescription Build(World world)
    {
        return new QueryDescription(
            world,
            _include.ToFrozenSet(),
            _exclude.ToFrozenSet(),
            _atLeastOne.ToFrozenSet(),
            _exactlyOne.ToFrozenSet()
        );
    }

    private void ContainsComponent(ComponentID id, int index, string caller)
    {
        if (index != _include.Index && _include.Contains(id))
            throw new InvalidOperationException($"Cannot '{caller}' - component is already included");
        if (index != _exclude.Index && _exclude.Contains(id))
            throw new InvalidOperationException($"Cannot '{caller}' - component is already excluded");
        if (index != _atLeastOne.Index && _atLeastOne.Contains(id))
            throw new InvalidOperationException($"Cannot '{caller}' - component is already included (at least one)");
        if (index != _exactlyOne.Index && _exactlyOne.Contains(id))
            throw new InvalidOperationException($"Cannot '{caller}' - component is already included (exactly one)");
    }

    #region include
    /// <summary>
    /// The given component must exist for an entity to be matched by this query
    /// </summary>
    /// <typeparam name="T">The component type</typeparam>
    /// <returns>this builder</returns>
    public QueryBuilder Include<T>()
        where T : IComponent
    {
        _include.Add<T>();
        return this;
    }

    /// <summary>
    /// The given component must exist for an entity to be matched by this query
    /// </summary>
    /// <param name="type">The component type</param>
    /// <returns>this builder</returns>
    public QueryBuilder Include(Type type)
    {
        _include.Add(type);
        return this;
    }

    internal QueryBuilder Include(ComponentID id)
    {
        _include.Add(id);
        return this;
    }

    /// <summary>
    /// Check if the given component type has been marked as "Include"
    /// </summary>
    /// <param name="type">The component type</param>
    /// <returns>true, if the component is included, otherwise false</returns>
    public bool IsIncluded(Type type)
    {
        return _include.Contains(type);
    }

    /// <summary>
    /// Check if the given component type has been marked as "Include"
    /// </summary>
    /// <typeparam name="T">The component type</typeparam>
    /// <returns>true, if the component is included, otherwise false</returns>
    public bool IsIncluded<T>()
        where T : IComponent
    {
        return _include.Contains<T>();
    }
    #endregion

    #region exclude
    public QueryBuilder Exclude<T>()
        where T : IComponent
    {
        _exclude.Add<T>();
        return this;
    }

    public QueryBuilder Exclude(Type type)
    {
        _exclude.Add(type);
        return this;
    }

    internal QueryBuilder Exclude(ComponentID id)
    {
        _exclude.Add(id);
        return this;
    }

    public bool IsExcluded(Type type)
    {
        return _exclude.Contains(type);
    }

    public bool IsExcluded<T>()
        where T : IComponent
    {
        return _exclude.Contains<T>();
    }
    #endregion

    #region at least one
    public QueryBuilder AtLeastOneOf<T>()
        where T : IComponent
    {
        _atLeastOne.Add<T>();
        return this;
    }

    public QueryBuilder AtLeastOneOf(Type type)
    {
        _atLeastOne.Add(type);
        return this;
    }

    internal QueryBuilder AtLeastOneOf(ComponentID id)
    {
        _atLeastOne.Add(id);
        return this;
    }

    public bool IsAtLeastOneOf(Type type)
    {
        return _atLeastOne.Contains(type);
    }

    public bool IsAtLeastOneOf<T>()
        where T : IComponent
    {
        return _atLeastOne.Contains<T>();
    }
    #endregion

    #region exactly one
    public QueryBuilder ExactlyOneOf<T>()
        where T : IComponent
    {
        _exactlyOne.Add<T>();
        return this;
    }

    public QueryBuilder ExactlyOneOf(Type type)
    {
        _exactlyOne.Add(type);
        return this;
    }

    internal QueryBuilder ExactlyOneOf(ComponentID id)
    {
        _exactlyOne.Add(id);
        return this;
    }

    public bool IsExactlyOneOf(Type type)
    {
        return _exactlyOne.Contains(type);
    }

    public bool IsExactlyOneOf<T>()
        where T : IComponent
    {
        return _exactlyOne.Contains<T>();
    }
    #endregion

    private class IDSet<TR, TB, TID>(Action<TID, int, string> check, int Index)
        where TR : BaseRegistry<TB, TID>
        where TID : struct, IIDNumber<TID>
    {
        public int Index { get; } = Index;

        private readonly HashSet<TID> _items = [];

        private FrozenOrderedListSet<TID>? _frozenCache;

        public FrozenOrderedListSet<TID> ToFrozenSet()
        {
            if (_frozenCache != null)
                return _frozenCache;

            _frozenCache = new FrozenOrderedListSet<TID>(_items);
            return _frozenCache;
        }

        public bool Add(TID id, [CallerMemberName] string caller = "")
        {
            check(id, Index, caller);
            if (_items.Add(id))
            {
                _frozenCache = null;
                return true;
            }

            return false;
        }

        public bool Add(Type type, [CallerMemberName] string caller = "")
        {
            return Add(BaseRegistry<TB, TID>.Get(type), caller);
        }

        public bool Add<T>([CallerMemberName] string caller = "")
            where T : TB
        {
            return Add(BaseRegistry<TB, TID>.Get<T>(), caller);
        }

        public bool Contains(TID id)
        {
            return _items.Contains(id);
        }

        public bool Contains(Type type)
        {
            return Contains(BaseRegistry<TB, TID>.Get(type));
        }

        public bool Contains<T>()
            where T : TB
        {
            return Contains(BaseRegistry<TB, TID>.Get<T>());
        }
    }
}