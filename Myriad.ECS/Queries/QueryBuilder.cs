using System.Runtime.CompilerServices;
using Myriad.ECS.IDs;
using Myriad.ECS.Worlds;
using Myriad.ECS.Collections;
using Myriad.ECS.Components;

namespace Myriad.ECS.Queries;

/// <summary>
/// Build a new <see cref="QueryDescription"/> object
/// </summary>
public sealed partial class QueryBuilder
{
    internal static readonly FrozenOrderedListSet<ComponentID> SetWithJustPhantom = FrozenOrderedListSet<ComponentID>.Create(
        new[] { ComponentID<Phantom>.ID }
    );

    private readonly ComponentSet _include;
    /// <summary>
    /// An Entity must include all of these components to be matched by this query
    /// </summary>
    public IReadOnlyList<ComponentID> Included => _include.Items;

    private readonly ComponentSet _exclude;
    /// <summary>
    /// Entities with these components will not be matched by this query
    /// </summary>
    public IReadOnlyList<ComponentID> Excluded => _exclude.Items;

    private readonly ComponentSet _atLeastOne;
    /// <summary>
    /// At least one of all these components must be on an Entity for it to be matched by this query
    /// </summary>
    public IReadOnlyList<ComponentID> AtLeastOnes => _atLeastOne.Items;

    private readonly ComponentSet _exactlyOne;
    /// <summary>
    /// Exactly one of all these components must be on an Entity for it to be matched by this query
    /// </summary>
    public IReadOnlyList<ComponentID> ExactlyOnes => _exactlyOne.Items;

    /// <summary>
    /// Create a new <see cref="QueryBuilder"/>
    /// </summary>
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
        // In the special case when it's only an include query try to get an item from the cache
        if (_exclude.Items.Count == 0 && _atLeastOne.Items.Count == 0 && _exactlyOne.Items.Count == 0)
        {
            var cached = world.TryGetCachedQuery(_include.Items);
            if (cached != null)
                return cached;
        }

        // Automatically exclude all Phantom entities, unless specifically requested.
        if (!_include.Contains(ComponentID<Phantom>.ID))
            Exclude<Phantom>();

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

    /// <summary>
    /// The given component must exist for an entity to be matched by this query
    /// </summary>
    /// <param name="id">The component type</param>
    /// <returns>this builder</returns>
    public QueryBuilder Include(ComponentID id)
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

    /// <summary>
    /// Check if the given component type has been marked as "Include"
    /// </summary>
    /// <param name="id">The component id</param>
    /// <returns>true, if the component is included, otherwise false</returns>
    public bool IsIncluded(ComponentID id)
    {
        return _include.Contains(id);
    }
    #endregion

    #region exclude
    /// <summary>
    /// The given component must not exist for an entity to be matched by this query
    /// </summary>
    /// <typeparam name="T">The component type</typeparam>
    /// <returns>this builder</returns>
    public QueryBuilder Exclude<T>()
        where T : IComponent
    {
        _exclude.Add<T>();
        return this;
    }

    /// <summary>
    /// The given component must not exist for an entity to be matched by this query
    /// </summary>
    /// <param name="type">The component type</param>
    /// <returns>this builder</returns>
    public QueryBuilder Exclude(Type type)
    {
        _exclude.Add(type);
        return this;
    }

    /// <summary>
    /// The given component must not exist for an entity to be matched by this query
    /// </summary>
    /// <param name="id">The component type</param>
    /// <returns>this builder</returns>
    public QueryBuilder Exclude(ComponentID id)
    {
        _exclude.Add(id);
        return this;
    }

    /// <summary>
    /// Check if the given component is excluded
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public bool IsExcluded(Type type)
    {
        return _exclude.Contains(type);
    }

    /// <summary>
    /// Check if the given component is excluded
    /// </summary>
    /// <returns></returns>
    public bool IsExcluded<T>()
        where T : IComponent
    {
        return _exclude.Contains<T>();
    }

    /// <summary>
    /// Check if the given component is excluded
    /// </summary>
    /// <returns></returns>
    public bool IsExcluded(ComponentID id)
    {
        return _exclude.Contains(id);
    }
    #endregion

    #region at least one
    /// <summary>
    /// At least one of all components specified as AtLeastOneOf must exist for an entity to be matched by this query
    /// </summary>
    /// <typeparam name="T">The component type</typeparam>
    /// <returns>this builder</returns>
    public QueryBuilder AtLeastOneOf<T>()
        where T : IComponent
    {
        _atLeastOne.Add<T>();
        return this;
    }

    /// <summary>
    /// At least one of all components specified as AtLeastOneOf must exist for an entity to be matched by this query
    /// </summary>
    /// <param name="type">The component type</param>
    /// <returns>this builder</returns>
    public QueryBuilder AtLeastOneOf(Type type)
    {
        _atLeastOne.Add(type);
        return this;
    }

    /// <summary>
    /// At least one of all components specified as AtLeastOneOf must exist for an entity to be matched by this query
    /// </summary>
    /// <param name="id">The component type</param>
    /// <returns>this builder</returns>
    public QueryBuilder AtLeastOneOf(ComponentID id)
    {
        _atLeastOne.Add(id);
        return this;
    }

    /// <summary>
    /// Check if the given component is one of the components which entities must have at least one of
    /// </summary>
    /// <returns></returns>
    public bool IsAtLeastOneOf(Type type)
    {
        return _atLeastOne.Contains(type);
    }

    /// <summary>
    /// Check if the given component is one of the components which entities must have at least one of
    /// </summary>
    /// <returns></returns>
    public bool IsAtLeastOneOf<T>()
        where T : IComponent
    {
        return _atLeastOne.Contains<T>();
    }

    /// <summary>
    /// Check if the given component is one of the components which entities must have at least one of
    /// </summary>
    /// <returns></returns>
    public bool IsAtLeastOneOf(ComponentID id)
    {
        return _atLeastOne.Contains(id);
    }
    #endregion

    #region exactly one
    /// <summary>
    /// Exactly one of all components specified as ExactlyOneOf must exist for an entity to be matched by this query
    /// </summary>
    /// <typeparam name="T">The component type</typeparam>
    /// <returns>this builder</returns>
    public QueryBuilder ExactlyOneOf<T>()
        where T : IComponent
    {
        _exactlyOne.Add<T>();
        return this;
    }

    /// <summary>
    /// Exactly one of all components specified as ExactlyOneOf must exist for an entity to be matched by this query
    /// </summary>
    /// <param name="type">The component type</param>
    /// <returns>this builder</returns>
    public QueryBuilder ExactlyOneOf(Type type)
    {
        _exactlyOne.Add(type);
        return this;
    }

    /// <summary>
    /// Exactly one of all components specified as ExactlyOneOf must exist for an entity to be matched by this query
    /// </summary>
    /// <param name="id">The component type</param>
    /// <returns>this builder</returns>
    public QueryBuilder ExactlyOneOf(ComponentID id)
    {
        _exactlyOne.Add(id);
        return this;
    }

    /// <summary>
    /// Check if the given component is one of the components which entities must have exactly one of
    /// </summary>
    /// <returns></returns>
    public bool IsExactlyOneOf(Type type)
    {
        return _exactlyOne.Contains(type);
    }

    /// <summary>
    /// Check if the given component is one of the components which entities must have exactly one of
    /// </summary>
    /// <returns></returns>
    public bool IsExactlyOneOf<T>()
        where T : IComponent
    {
        return _exactlyOne.Contains<T>();
    }

    /// <summary>
    /// Check if the given component is one of the components which entities must have exactly one of
    /// </summary>
    /// <returns></returns>
    public bool IsExactlyOneOf(ComponentID id)
    {
        return _exactlyOne.Contains(id);
    }
    #endregion

    private class ComponentSet(Action<ComponentID, int, string> check, int Index)
    {
        public int Index { get; } = Index;

        public readonly OrderedListSet<ComponentID> Items = new();

        private FrozenOrderedListSet<ComponentID>? _frozenCache;

        public FrozenOrderedListSet<ComponentID> ToFrozenSet()
        {
            if (_frozenCache != null)
                return _frozenCache;

            if (Items.Count == 0)
                return FrozenOrderedListSet<ComponentID>.Empty;

            if (Items.Count == 1 && Items.Contains(ComponentID<Phantom>.ID))
                return SetWithJustPhantom;

            _frozenCache = FrozenOrderedListSet<ComponentID>.Create(Items);
            return _frozenCache;
        }

        public bool Add(ComponentID id, [CallerMemberName] string caller = "")
        {
            check(id, Index, caller);
            if (Items.Add(id))
            {
                _frozenCache = null;
                return true;
            }

            return false;
        }

        public bool Add(Type type, [CallerMemberName] string caller = "")
        {
            return Add(ComponentID.Get(type), caller);
        }

        public bool Add<T>([CallerMemberName] string caller = "")
            where T : IComponent
        {
            return Add(ComponentID<T>.ID, caller);
        }

        public bool Contains(ComponentID id)
        {
            return Items.Contains(id);
        }

        public bool Contains(Type type)
        {
            return Contains(ComponentID.Get(type));
        }

        public bool Contains<T>()
            where T : IComponent
        {
            return Contains(ComponentID<T>.ID);
        }
    }
}