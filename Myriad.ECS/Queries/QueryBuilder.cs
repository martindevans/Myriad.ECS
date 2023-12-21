using System.Collections.Frozen;
using Myriad.ECS.Queries.Filters;
using Myriad.ECS.Registry;
using System.Runtime.CompilerServices;
using Myriad.ECS.IDs;

namespace Myriad.ECS.Queries;

public sealed class QueryBuilder
{
    private readonly IDSet<ComponentRegistry, IComponent, ComponentID> _include;
    private readonly IDSet<ComponentRegistry, IComponent, ComponentID> _exclude;
    private readonly IDSet<ComponentRegistry, IComponent, ComponentID> _atLeastOne;
    private readonly IDSet<ComponentRegistry, IComponent, ComponentID> _exactlyOne;

    private readonly IDSet<FilterRegistry, IQueryFilter, FilterID> _filters;

    public QueryBuilder()
    {
        _include = new(ContainsComponent, 0);
        _exclude = new(ContainsComponent, 1);
        _atLeastOne = new(ContainsComponent, 2);
        _exactlyOne = new(ContainsComponent, 3);

        _filters = new IDSet<FilterRegistry, IQueryFilter, FilterID>(ContainsFilter, 4);
    }

    public QueryDescription Build(World world)
    {
        return new QueryDescription(
            world,
            _include.Items.ToFrozenSet(),
            _exclude.Items.ToFrozenSet(),
            _atLeastOne.Items.ToFrozenSet(),
            _exactlyOne.Items.ToFrozenSet(),
            _filters.Items.ToFrozenSet()
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

    private void ContainsFilter(FilterID id, int index, string caller)
    {
        if (index != _filters.Index && _filters.Contains(id))
            throw new InvalidOperationException($"Cannot '{caller}' - filter is already included");
    }

    #region include
    public QueryBuilder Include<T>()
        where T : IComponent
    {
        _include.Add<T>();
        return this;
    }

    public QueryBuilder Include(Type type)
    {
        _include.Add(type);
        return this;
    }

    public bool IsIncluded(Type type)
    {
        return _include.Contains(type);
    }

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

    #region filters
    public QueryBuilder Filter<T>()
        where T : struct, IQueryFilter
    {
        if (_filters.Add<T>())
            default(T).ConfigureQueryBuilder(this);

        return this;
    }

    public bool IsFilter(Type type)
    {
        return _filters.Contains(type);
    }

    public bool IsFilter<T>()
        where T : IQueryFilter
    {
        return _filters.Contains<T>();
    }
    #endregion

    private class IDSet<TR, TB, TID>
        where TR : BaseRegistry<TB, TID>
        where TID : struct, IIDNumber<TID>
    {
        public int Index { get; }

        private readonly Action<TID, int, string> _check;
        private readonly HashSet<TID> _items = [];

        public IReadOnlySet<TID> Items => _items;

        public IDSet(Action<TID, int, string> check, int Index)
        {
            this.Index = Index;
            _check = check;
        }

        private bool Add(TID id, string caller)
        {
            _check(id, Index, caller);
            return _items.Add(id);
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