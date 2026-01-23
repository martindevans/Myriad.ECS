using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Myriad.ECS.IDs;
using Myriad.ECS.Collections;
using Myriad.ECS.Worlds;
using Myriad.ECS.Worlds.Chunks;

namespace Myriad.ECS.Components;

/// <summary>
/// A reference to another Entity which caches information about the Entity, including whether it has a
/// specific component type. This can be used to avoid structural checks.
/// </summary>

public struct TypedEntityReference<T0>
    where T0 : IComponent
{
    private static readonly ComponentID ComponentID0 = ComponentID<T0>.ID;
    private static readonly ReadOnlyMemory<ComponentID> ComponentSet = SortedListOfComponents<T0>.Components;

    private Cache? _cache;
    private EntityId _entity;

    /// <summary>
    /// The target entity
    /// </summary>
    public EntityId Target
    {
        get => _entity;
        set
        {
            _entity = value;
            _cache = null;
        }
    }

    /// <summary>
    /// Check the entity state
    /// </summary>
    /// <param name="world">The entity world</param>
    /// <returns></returns>
    public Result TryGetComponentRef(World world)
    {
        // Check if cache is stale
        if (_cache?.CheckIsStale(_entity) ?? true)
        {
            // Clear cache
            _cache = null;

            // Get info for this entity
            EntityInfo _ = default;
            ref readonly var info = ref world.GetEntityInfo(_entity, ref _, out var doesNotExist);

            // Create new cache
            _cache = doesNotExist
                ? Cache.DoesNotExist()
                : Cache.Create(in info);
        }

        // Cache is definitely valid now. Extract results.
        var v = _cache.Value;

        // Early out if the entity doesn't even exist
        if (v.RowIndex < 0)
            return new Result(default, false, false, false);

        // Early out if the entity doesn't have the component
        if (!v.HasComponent)
            return new Result(default, true, v.IsPhantom, false);

        // Get the component(s)
        var tuple = v.Chunk!.GetRefTuple<T0>(
            v.RowIndex,
            ComponentID0
        );

        return new Result(
            tuple,
            true,
            v.IsPhantom,
            true
        );
    }

    /// <summary>
    /// Cache data about an entity, valid if the entity is still in the same place in the same chunk
    /// </summary>
    private readonly record struct Cache
    {
        public int RowIndex { get; }
        public Chunk? Chunk { get; }

        public bool HasComponent { get; }
        public bool IsPhantom { get; }

        private Cache(int rowIndex, Chunk? chunk, bool hasComponent, bool isPhantom)
        {
            RowIndex = rowIndex;
            Chunk = chunk;
            HasComponent = hasComponent;
            IsPhantom = isPhantom;
        }

        private bool CheckNotStale(EntityId entity)
        {
            // This is the indicator for the entity not existing
            if (RowIndex < 0)
                return false;
            
            // It's only allowed to be null when the entity does not exist
            Debug.Assert(Chunk != null);

            // Check if index is out of range
            if (Chunk.EntityCount <= RowIndex)
                return false;

            // Check if the entity is still where we last saw it
            return Chunk.Entities.Span[RowIndex] == entity;
        }

        public bool CheckIsStale(EntityId entity)
        {
            return !CheckNotStale(entity);
        }

        public static Cache DoesNotExist()
        {
            return new Cache(-1, null, false, false);
        }

        public static Cache Create(ref readonly EntityInfo info)
        {
            var archetype = info.Chunk.Archetype;

            var hasComponent = archetype.Components.IsSupersetOfSortedSpan(ComponentSet.Span);
            var isPhantom = archetype.IsPhantom;

            return new Cache(
                info.RowIndex, info.Chunk, hasComponent, isPhantom
            );
        }
    }

    /// <summary>
    /// Result of trying to get the component ref from a TypedEntityReference
    /// </summary>
    public ref struct Result
    {
        /// <summary>
        /// The ref tuple, only valid if <see cref="HasComponent"/> is true
        /// </summary>
        public RefTuple<T0> Components { get; }

        /// <summary>
        /// Indicates if the entity exists
        /// </summary>
        public bool Exists;

        /// <summary>
        /// Indicates if the entity is a phantom
        /// </summary>
        public bool IsPhantom;

        /// <summary>
        /// Indicates if the entity has the expected components
        /// </summary>
        public bool HasComponent;

        internal Result(RefTuple<T0> components, bool exists, bool isPhantom, bool hasComponent)
        {
            Components = components;

            Exists = exists;
            IsPhantom = isPhantom;

            HasComponent = hasComponent;
        }
    }
}

/// <summary>
/// A reference to another Entity which caches information about the Entity, including whether it has a
/// specific component type. This can be used to avoid structural checks.
/// </summary>
[ExcludeFromCodeCoverage]
public struct TypedEntityReference<T0, T1>
    where T0 : IComponent
        where T1 : IComponent
{
    private static readonly ComponentID ComponentID0 = ComponentID<T0>.ID;
    private static readonly ComponentID ComponentID1 = ComponentID<T1>.ID;
    private static readonly ReadOnlyMemory<ComponentID> ComponentSet = SortedListOfComponents<T0, T1>.Components;

    private Cache? _cache;
    private EntityId _entity;

    /// <summary>
    /// The target entity
    /// </summary>
    public EntityId Target
    {
        get => _entity;
        set
        {
            _entity = value;
            _cache = null;
        }
    }

    /// <summary>
    /// Check the entity state
    /// </summary>
    /// <param name="world">The entity world</param>
    /// <returns></returns>
    public Result TryGetComponentRef(World world)
    {
        // Check if cache is stale
        if (_cache?.CheckIsStale(_entity) ?? true)
        {
            // Clear cache
            _cache = null;

            // Get info for this entity
            EntityInfo _ = default;
            ref readonly var info = ref world.GetEntityInfo(_entity, ref _, out var doesNotExist);

            // Create new cache
            _cache = doesNotExist
                ? Cache.DoesNotExist()
                : Cache.Create(in info);
        }

        // Cache is definitely valid now. Extract results.
        var v = _cache.Value;

        // Early out if the entity doesn't even exist
        if (v.RowIndex < 0)
            return new Result(default, false, false, false);

        // Early out if the entity doesn't have the component
        if (!v.HasComponent)
            return new Result(default, true, v.IsPhantom, false);

        // Get the component(s)
        var tuple = v.Chunk!.GetRefTuple<T0, T1>(
            v.RowIndex,
            ComponentID0,
            ComponentID1
        );

        return new Result(
            tuple,
            true,
            v.IsPhantom,
            true
        );
    }

    /// <summary>
    /// Cache data about an entity, valid if the entity is still in the same place in the same chunk
    /// </summary>
    private readonly record struct Cache
    {
        public int RowIndex { get; }
        public Chunk? Chunk { get; }

        public bool HasComponent { get; }
        public bool IsPhantom { get; }

        private Cache(int rowIndex, Chunk? chunk, bool hasComponent, bool isPhantom)
        {
            RowIndex = rowIndex;
            Chunk = chunk;
            HasComponent = hasComponent;
            IsPhantom = isPhantom;
        }

        private bool CheckNotStale(EntityId entity)
        {
            // This is the indicator for the entity not existing
            if (RowIndex < 0)
                return false;
            
            // It's only allowed to be null when the entity does not exist
            Debug.Assert(Chunk != null);

            // Check if index is out of range
            if (Chunk.EntityCount <= RowIndex)
                return false;

            // Check if the entity is still where we last saw it
            return Chunk.Entities.Span[RowIndex] == entity;
        }

        public bool CheckIsStale(EntityId entity)
        {
            return !CheckNotStale(entity);
        }

        public static Cache DoesNotExist()
        {
            return new Cache(-1, null, false, false);
        }

        public static Cache Create(ref readonly EntityInfo info)
        {
            var archetype = info.Chunk.Archetype;

            var hasComponent = archetype.Components.IsSupersetOfSortedSpan(ComponentSet.Span);
            var isPhantom = archetype.IsPhantom;

            return new Cache(
                info.RowIndex, info.Chunk, hasComponent, isPhantom
            );
        }
    }

    /// <summary>
    /// Result of trying to get the component ref from a TypedEntityReference
    /// </summary>
    public ref struct Result
    {
        /// <summary>
        /// The ref tuple, only valid if <see cref="HasComponent"/> is true
        /// </summary>
        public RefTuple<T0, T1> Components { get; }

        /// <summary>
        /// Indicates if the entity exists
        /// </summary>
        public bool Exists;

        /// <summary>
        /// Indicates if the entity is a phantom
        /// </summary>
        public bool IsPhantom;

        /// <summary>
        /// Indicates if the entity has the expected components
        /// </summary>
        public bool HasComponent;

        internal Result(RefTuple<T0, T1> components, bool exists, bool isPhantom, bool hasComponent)
        {
            Components = components;

            Exists = exists;
            IsPhantom = isPhantom;

            HasComponent = hasComponent;
        }
    }
}

/// <summary>
/// A reference to another Entity which caches information about the Entity, including whether it has a
/// specific component type. This can be used to avoid structural checks.
/// </summary>
[ExcludeFromCodeCoverage]
public struct TypedEntityReference<T0, T1, T2>
    where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
{
    private static readonly ComponentID ComponentID0 = ComponentID<T0>.ID;
    private static readonly ComponentID ComponentID1 = ComponentID<T1>.ID;
    private static readonly ComponentID ComponentID2 = ComponentID<T2>.ID;
    private static readonly ReadOnlyMemory<ComponentID> ComponentSet = SortedListOfComponents<T0, T1, T2>.Components;

    private Cache? _cache;
    private EntityId _entity;

    /// <summary>
    /// The target entity
    /// </summary>
    public EntityId Target
    {
        get => _entity;
        set
        {
            _entity = value;
            _cache = null;
        }
    }

    /// <summary>
    /// Check the entity state
    /// </summary>
    /// <param name="world">The entity world</param>
    /// <returns></returns>
    public Result TryGetComponentRef(World world)
    {
        // Check if cache is stale
        if (_cache?.CheckIsStale(_entity) ?? true)
        {
            // Clear cache
            _cache = null;

            // Get info for this entity
            EntityInfo _ = default;
            ref readonly var info = ref world.GetEntityInfo(_entity, ref _, out var doesNotExist);

            // Create new cache
            _cache = doesNotExist
                ? Cache.DoesNotExist()
                : Cache.Create(in info);
        }

        // Cache is definitely valid now. Extract results.
        var v = _cache.Value;

        // Early out if the entity doesn't even exist
        if (v.RowIndex < 0)
            return new Result(default, false, false, false);

        // Early out if the entity doesn't have the component
        if (!v.HasComponent)
            return new Result(default, true, v.IsPhantom, false);

        // Get the component(s)
        var tuple = v.Chunk!.GetRefTuple<T0, T1, T2>(
            v.RowIndex,
            ComponentID0,
            ComponentID1,
            ComponentID2
        );

        return new Result(
            tuple,
            true,
            v.IsPhantom,
            true
        );
    }

    /// <summary>
    /// Cache data about an entity, valid if the entity is still in the same place in the same chunk
    /// </summary>
    private readonly record struct Cache
    {
        public int RowIndex { get; }
        public Chunk? Chunk { get; }

        public bool HasComponent { get; }
        public bool IsPhantom { get; }

        private Cache(int rowIndex, Chunk? chunk, bool hasComponent, bool isPhantom)
        {
            RowIndex = rowIndex;
            Chunk = chunk;
            HasComponent = hasComponent;
            IsPhantom = isPhantom;
        }

        private bool CheckNotStale(EntityId entity)
        {
            // This is the indicator for the entity not existing
            if (RowIndex < 0)
                return false;
            
            // It's only allowed to be null when the entity does not exist
            Debug.Assert(Chunk != null);

            // Check if index is out of range
            if (Chunk.EntityCount <= RowIndex)
                return false;

            // Check if the entity is still where we last saw it
            return Chunk.Entities.Span[RowIndex] == entity;
        }

        public bool CheckIsStale(EntityId entity)
        {
            return !CheckNotStale(entity);
        }

        public static Cache DoesNotExist()
        {
            return new Cache(-1, null, false, false);
        }

        public static Cache Create(ref readonly EntityInfo info)
        {
            var archetype = info.Chunk.Archetype;

            var hasComponent = archetype.Components.IsSupersetOfSortedSpan(ComponentSet.Span);
            var isPhantom = archetype.IsPhantom;

            return new Cache(
                info.RowIndex, info.Chunk, hasComponent, isPhantom
            );
        }
    }

    /// <summary>
    /// Result of trying to get the component ref from a TypedEntityReference
    /// </summary>
    public ref struct Result
    {
        /// <summary>
        /// The ref tuple, only valid if <see cref="HasComponent"/> is true
        /// </summary>
        public RefTuple<T0, T1, T2> Components { get; }

        /// <summary>
        /// Indicates if the entity exists
        /// </summary>
        public bool Exists;

        /// <summary>
        /// Indicates if the entity is a phantom
        /// </summary>
        public bool IsPhantom;

        /// <summary>
        /// Indicates if the entity has the expected components
        /// </summary>
        public bool HasComponent;

        internal Result(RefTuple<T0, T1, T2> components, bool exists, bool isPhantom, bool hasComponent)
        {
            Components = components;

            Exists = exists;
            IsPhantom = isPhantom;

            HasComponent = hasComponent;
        }
    }
}

/// <summary>
/// A reference to another Entity which caches information about the Entity, including whether it has a
/// specific component type. This can be used to avoid structural checks.
/// </summary>
[ExcludeFromCodeCoverage]
public struct TypedEntityReference<T0, T1, T2, T3>
    where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
{
    private static readonly ComponentID ComponentID0 = ComponentID<T0>.ID;
    private static readonly ComponentID ComponentID1 = ComponentID<T1>.ID;
    private static readonly ComponentID ComponentID2 = ComponentID<T2>.ID;
    private static readonly ComponentID ComponentID3 = ComponentID<T3>.ID;
    private static readonly ReadOnlyMemory<ComponentID> ComponentSet = SortedListOfComponents<T0, T1, T2, T3>.Components;

    private Cache? _cache;
    private EntityId _entity;

    /// <summary>
    /// The target entity
    /// </summary>
    public EntityId Target
    {
        get => _entity;
        set
        {
            _entity = value;
            _cache = null;
        }
    }

    /// <summary>
    /// Check the entity state
    /// </summary>
    /// <param name="world">The entity world</param>
    /// <returns></returns>
    public Result TryGetComponentRef(World world)
    {
        // Check if cache is stale
        if (_cache?.CheckIsStale(_entity) ?? true)
        {
            // Clear cache
            _cache = null;

            // Get info for this entity
            EntityInfo _ = default;
            ref readonly var info = ref world.GetEntityInfo(_entity, ref _, out var doesNotExist);

            // Create new cache
            _cache = doesNotExist
                ? Cache.DoesNotExist()
                : Cache.Create(in info);
        }

        // Cache is definitely valid now. Extract results.
        var v = _cache.Value;

        // Early out if the entity doesn't even exist
        if (v.RowIndex < 0)
            return new Result(default, false, false, false);

        // Early out if the entity doesn't have the component
        if (!v.HasComponent)
            return new Result(default, true, v.IsPhantom, false);

        // Get the component(s)
        var tuple = v.Chunk!.GetRefTuple<T0, T1, T2, T3>(
            v.RowIndex,
            ComponentID0,
            ComponentID1,
            ComponentID2,
            ComponentID3
        );

        return new Result(
            tuple,
            true,
            v.IsPhantom,
            true
        );
    }

    /// <summary>
    /// Cache data about an entity, valid if the entity is still in the same place in the same chunk
    /// </summary>
    private readonly record struct Cache
    {
        public int RowIndex { get; }
        public Chunk? Chunk { get; }

        public bool HasComponent { get; }
        public bool IsPhantom { get; }

        private Cache(int rowIndex, Chunk? chunk, bool hasComponent, bool isPhantom)
        {
            RowIndex = rowIndex;
            Chunk = chunk;
            HasComponent = hasComponent;
            IsPhantom = isPhantom;
        }

        private bool CheckNotStale(EntityId entity)
        {
            // This is the indicator for the entity not existing
            if (RowIndex < 0)
                return false;
            
            // It's only allowed to be null when the entity does not exist
            Debug.Assert(Chunk != null);

            // Check if index is out of range
            if (Chunk.EntityCount <= RowIndex)
                return false;

            // Check if the entity is still where we last saw it
            return Chunk.Entities.Span[RowIndex] == entity;
        }

        public bool CheckIsStale(EntityId entity)
        {
            return !CheckNotStale(entity);
        }

        public static Cache DoesNotExist()
        {
            return new Cache(-1, null, false, false);
        }

        public static Cache Create(ref readonly EntityInfo info)
        {
            var archetype = info.Chunk.Archetype;

            var hasComponent = archetype.Components.IsSupersetOfSortedSpan(ComponentSet.Span);
            var isPhantom = archetype.IsPhantom;

            return new Cache(
                info.RowIndex, info.Chunk, hasComponent, isPhantom
            );
        }
    }

    /// <summary>
    /// Result of trying to get the component ref from a TypedEntityReference
    /// </summary>
    public ref struct Result
    {
        /// <summary>
        /// The ref tuple, only valid if <see cref="HasComponent"/> is true
        /// </summary>
        public RefTuple<T0, T1, T2, T3> Components { get; }

        /// <summary>
        /// Indicates if the entity exists
        /// </summary>
        public bool Exists;

        /// <summary>
        /// Indicates if the entity is a phantom
        /// </summary>
        public bool IsPhantom;

        /// <summary>
        /// Indicates if the entity has the expected components
        /// </summary>
        public bool HasComponent;

        internal Result(RefTuple<T0, T1, T2, T3> components, bool exists, bool isPhantom, bool hasComponent)
        {
            Components = components;

            Exists = exists;
            IsPhantom = isPhantom;

            HasComponent = hasComponent;
        }
    }
}

/// <summary>
/// A reference to another Entity which caches information about the Entity, including whether it has a
/// specific component type. This can be used to avoid structural checks.
/// </summary>
[ExcludeFromCodeCoverage]
public struct TypedEntityReference<T0, T1, T2, T3, T4>
    where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
{
    private static readonly ComponentID ComponentID0 = ComponentID<T0>.ID;
    private static readonly ComponentID ComponentID1 = ComponentID<T1>.ID;
    private static readonly ComponentID ComponentID2 = ComponentID<T2>.ID;
    private static readonly ComponentID ComponentID3 = ComponentID<T3>.ID;
    private static readonly ComponentID ComponentID4 = ComponentID<T4>.ID;
    private static readonly ReadOnlyMemory<ComponentID> ComponentSet = SortedListOfComponents<T0, T1, T2, T3, T4>.Components;

    private Cache? _cache;
    private EntityId _entity;

    /// <summary>
    /// The target entity
    /// </summary>
    public EntityId Target
    {
        get => _entity;
        set
        {
            _entity = value;
            _cache = null;
        }
    }

    /// <summary>
    /// Check the entity state
    /// </summary>
    /// <param name="world">The entity world</param>
    /// <returns></returns>
    public Result TryGetComponentRef(World world)
    {
        // Check if cache is stale
        if (_cache?.CheckIsStale(_entity) ?? true)
        {
            // Clear cache
            _cache = null;

            // Get info for this entity
            EntityInfo _ = default;
            ref readonly var info = ref world.GetEntityInfo(_entity, ref _, out var doesNotExist);

            // Create new cache
            _cache = doesNotExist
                ? Cache.DoesNotExist()
                : Cache.Create(in info);
        }

        // Cache is definitely valid now. Extract results.
        var v = _cache.Value;

        // Early out if the entity doesn't even exist
        if (v.RowIndex < 0)
            return new Result(default, false, false, false);

        // Early out if the entity doesn't have the component
        if (!v.HasComponent)
            return new Result(default, true, v.IsPhantom, false);

        // Get the component(s)
        var tuple = v.Chunk!.GetRefTuple<T0, T1, T2, T3, T4>(
            v.RowIndex,
            ComponentID0,
            ComponentID1,
            ComponentID2,
            ComponentID3,
            ComponentID4
        );

        return new Result(
            tuple,
            true,
            v.IsPhantom,
            true
        );
    }

    /// <summary>
    /// Cache data about an entity, valid if the entity is still in the same place in the same chunk
    /// </summary>
    private readonly record struct Cache
    {
        public int RowIndex { get; }
        public Chunk? Chunk { get; }

        public bool HasComponent { get; }
        public bool IsPhantom { get; }

        private Cache(int rowIndex, Chunk? chunk, bool hasComponent, bool isPhantom)
        {
            RowIndex = rowIndex;
            Chunk = chunk;
            HasComponent = hasComponent;
            IsPhantom = isPhantom;
        }

        private bool CheckNotStale(EntityId entity)
        {
            // This is the indicator for the entity not existing
            if (RowIndex < 0)
                return false;
            
            // It's only allowed to be null when the entity does not exist
            Debug.Assert(Chunk != null);

            // Check if index is out of range
            if (Chunk.EntityCount <= RowIndex)
                return false;

            // Check if the entity is still where we last saw it
            return Chunk.Entities.Span[RowIndex] == entity;
        }

        public bool CheckIsStale(EntityId entity)
        {
            return !CheckNotStale(entity);
        }

        public static Cache DoesNotExist()
        {
            return new Cache(-1, null, false, false);
        }

        public static Cache Create(ref readonly EntityInfo info)
        {
            var archetype = info.Chunk.Archetype;

            var hasComponent = archetype.Components.IsSupersetOfSortedSpan(ComponentSet.Span);
            var isPhantom = archetype.IsPhantom;

            return new Cache(
                info.RowIndex, info.Chunk, hasComponent, isPhantom
            );
        }
    }

    /// <summary>
    /// Result of trying to get the component ref from a TypedEntityReference
    /// </summary>
    public ref struct Result
    {
        /// <summary>
        /// The ref tuple, only valid if <see cref="HasComponent"/> is true
        /// </summary>
        public RefTuple<T0, T1, T2, T3, T4> Components { get; }

        /// <summary>
        /// Indicates if the entity exists
        /// </summary>
        public bool Exists;

        /// <summary>
        /// Indicates if the entity is a phantom
        /// </summary>
        public bool IsPhantom;

        /// <summary>
        /// Indicates if the entity has the expected components
        /// </summary>
        public bool HasComponent;

        internal Result(RefTuple<T0, T1, T2, T3, T4> components, bool exists, bool isPhantom, bool hasComponent)
        {
            Components = components;

            Exists = exists;
            IsPhantom = isPhantom;

            HasComponent = hasComponent;
        }
    }
}

/// <summary>
/// A reference to another Entity which caches information about the Entity, including whether it has a
/// specific component type. This can be used to avoid structural checks.
/// </summary>
[ExcludeFromCodeCoverage]
public struct TypedEntityReference<T0, T1, T2, T3, T4, T5>
    where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
{
    private static readonly ComponentID ComponentID0 = ComponentID<T0>.ID;
    private static readonly ComponentID ComponentID1 = ComponentID<T1>.ID;
    private static readonly ComponentID ComponentID2 = ComponentID<T2>.ID;
    private static readonly ComponentID ComponentID3 = ComponentID<T3>.ID;
    private static readonly ComponentID ComponentID4 = ComponentID<T4>.ID;
    private static readonly ComponentID ComponentID5 = ComponentID<T5>.ID;
    private static readonly ReadOnlyMemory<ComponentID> ComponentSet = SortedListOfComponents<T0, T1, T2, T3, T4, T5>.Components;

    private Cache? _cache;
    private EntityId _entity;

    /// <summary>
    /// The target entity
    /// </summary>
    public EntityId Target
    {
        get => _entity;
        set
        {
            _entity = value;
            _cache = null;
        }
    }

    /// <summary>
    /// Check the entity state
    /// </summary>
    /// <param name="world">The entity world</param>
    /// <returns></returns>
    public Result TryGetComponentRef(World world)
    {
        // Check if cache is stale
        if (_cache?.CheckIsStale(_entity) ?? true)
        {
            // Clear cache
            _cache = null;

            // Get info for this entity
            EntityInfo _ = default;
            ref readonly var info = ref world.GetEntityInfo(_entity, ref _, out var doesNotExist);

            // Create new cache
            _cache = doesNotExist
                ? Cache.DoesNotExist()
                : Cache.Create(in info);
        }

        // Cache is definitely valid now. Extract results.
        var v = _cache.Value;

        // Early out if the entity doesn't even exist
        if (v.RowIndex < 0)
            return new Result(default, false, false, false);

        // Early out if the entity doesn't have the component
        if (!v.HasComponent)
            return new Result(default, true, v.IsPhantom, false);

        // Get the component(s)
        var tuple = v.Chunk!.GetRefTuple<T0, T1, T2, T3, T4, T5>(
            v.RowIndex,
            ComponentID0,
            ComponentID1,
            ComponentID2,
            ComponentID3,
            ComponentID4,
            ComponentID5
        );

        return new Result(
            tuple,
            true,
            v.IsPhantom,
            true
        );
    }

    /// <summary>
    /// Cache data about an entity, valid if the entity is still in the same place in the same chunk
    /// </summary>
    private readonly record struct Cache
    {
        public int RowIndex { get; }
        public Chunk? Chunk { get; }

        public bool HasComponent { get; }
        public bool IsPhantom { get; }

        private Cache(int rowIndex, Chunk? chunk, bool hasComponent, bool isPhantom)
        {
            RowIndex = rowIndex;
            Chunk = chunk;
            HasComponent = hasComponent;
            IsPhantom = isPhantom;
        }

        private bool CheckNotStale(EntityId entity)
        {
            // This is the indicator for the entity not existing
            if (RowIndex < 0)
                return false;
            
            // It's only allowed to be null when the entity does not exist
            Debug.Assert(Chunk != null);

            // Check if index is out of range
            if (Chunk.EntityCount <= RowIndex)
                return false;

            // Check if the entity is still where we last saw it
            return Chunk.Entities.Span[RowIndex] == entity;
        }

        public bool CheckIsStale(EntityId entity)
        {
            return !CheckNotStale(entity);
        }

        public static Cache DoesNotExist()
        {
            return new Cache(-1, null, false, false);
        }

        public static Cache Create(ref readonly EntityInfo info)
        {
            var archetype = info.Chunk.Archetype;

            var hasComponent = archetype.Components.IsSupersetOfSortedSpan(ComponentSet.Span);
            var isPhantom = archetype.IsPhantom;

            return new Cache(
                info.RowIndex, info.Chunk, hasComponent, isPhantom
            );
        }
    }

    /// <summary>
    /// Result of trying to get the component ref from a TypedEntityReference
    /// </summary>
    public ref struct Result
    {
        /// <summary>
        /// The ref tuple, only valid if <see cref="HasComponent"/> is true
        /// </summary>
        public RefTuple<T0, T1, T2, T3, T4, T5> Components { get; }

        /// <summary>
        /// Indicates if the entity exists
        /// </summary>
        public bool Exists;

        /// <summary>
        /// Indicates if the entity is a phantom
        /// </summary>
        public bool IsPhantom;

        /// <summary>
        /// Indicates if the entity has the expected components
        /// </summary>
        public bool HasComponent;

        internal Result(RefTuple<T0, T1, T2, T3, T4, T5> components, bool exists, bool isPhantom, bool hasComponent)
        {
            Components = components;

            Exists = exists;
            IsPhantom = isPhantom;

            HasComponent = hasComponent;
        }
    }
}

/// <summary>
/// A reference to another Entity which caches information about the Entity, including whether it has a
/// specific component type. This can be used to avoid structural checks.
/// </summary>
[ExcludeFromCodeCoverage]
public struct TypedEntityReference<T0, T1, T2, T3, T4, T5, T6>
    where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
{
    private static readonly ComponentID ComponentID0 = ComponentID<T0>.ID;
    private static readonly ComponentID ComponentID1 = ComponentID<T1>.ID;
    private static readonly ComponentID ComponentID2 = ComponentID<T2>.ID;
    private static readonly ComponentID ComponentID3 = ComponentID<T3>.ID;
    private static readonly ComponentID ComponentID4 = ComponentID<T4>.ID;
    private static readonly ComponentID ComponentID5 = ComponentID<T5>.ID;
    private static readonly ComponentID ComponentID6 = ComponentID<T6>.ID;
    private static readonly ReadOnlyMemory<ComponentID> ComponentSet = SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6>.Components;

    private Cache? _cache;
    private EntityId _entity;

    /// <summary>
    /// The target entity
    /// </summary>
    public EntityId Target
    {
        get => _entity;
        set
        {
            _entity = value;
            _cache = null;
        }
    }

    /// <summary>
    /// Check the entity state
    /// </summary>
    /// <param name="world">The entity world</param>
    /// <returns></returns>
    public Result TryGetComponentRef(World world)
    {
        // Check if cache is stale
        if (_cache?.CheckIsStale(_entity) ?? true)
        {
            // Clear cache
            _cache = null;

            // Get info for this entity
            EntityInfo _ = default;
            ref readonly var info = ref world.GetEntityInfo(_entity, ref _, out var doesNotExist);

            // Create new cache
            _cache = doesNotExist
                ? Cache.DoesNotExist()
                : Cache.Create(in info);
        }

        // Cache is definitely valid now. Extract results.
        var v = _cache.Value;

        // Early out if the entity doesn't even exist
        if (v.RowIndex < 0)
            return new Result(default, false, false, false);

        // Early out if the entity doesn't have the component
        if (!v.HasComponent)
            return new Result(default, true, v.IsPhantom, false);

        // Get the component(s)
        var tuple = v.Chunk!.GetRefTuple<T0, T1, T2, T3, T4, T5, T6>(
            v.RowIndex,
            ComponentID0,
            ComponentID1,
            ComponentID2,
            ComponentID3,
            ComponentID4,
            ComponentID5,
            ComponentID6
        );

        return new Result(
            tuple,
            true,
            v.IsPhantom,
            true
        );
    }

    /// <summary>
    /// Cache data about an entity, valid if the entity is still in the same place in the same chunk
    /// </summary>
    private readonly record struct Cache
    {
        public int RowIndex { get; }
        public Chunk? Chunk { get; }

        public bool HasComponent { get; }
        public bool IsPhantom { get; }

        private Cache(int rowIndex, Chunk? chunk, bool hasComponent, bool isPhantom)
        {
            RowIndex = rowIndex;
            Chunk = chunk;
            HasComponent = hasComponent;
            IsPhantom = isPhantom;
        }

        private bool CheckNotStale(EntityId entity)
        {
            // This is the indicator for the entity not existing
            if (RowIndex < 0)
                return false;
            
            // It's only allowed to be null when the entity does not exist
            Debug.Assert(Chunk != null);

            // Check if index is out of range
            if (Chunk.EntityCount <= RowIndex)
                return false;

            // Check if the entity is still where we last saw it
            return Chunk.Entities.Span[RowIndex] == entity;
        }

        public bool CheckIsStale(EntityId entity)
        {
            return !CheckNotStale(entity);
        }

        public static Cache DoesNotExist()
        {
            return new Cache(-1, null, false, false);
        }

        public static Cache Create(ref readonly EntityInfo info)
        {
            var archetype = info.Chunk.Archetype;

            var hasComponent = archetype.Components.IsSupersetOfSortedSpan(ComponentSet.Span);
            var isPhantom = archetype.IsPhantom;

            return new Cache(
                info.RowIndex, info.Chunk, hasComponent, isPhantom
            );
        }
    }

    /// <summary>
    /// Result of trying to get the component ref from a TypedEntityReference
    /// </summary>
    public ref struct Result
    {
        /// <summary>
        /// The ref tuple, only valid if <see cref="HasComponent"/> is true
        /// </summary>
        public RefTuple<T0, T1, T2, T3, T4, T5, T6> Components { get; }

        /// <summary>
        /// Indicates if the entity exists
        /// </summary>
        public bool Exists;

        /// <summary>
        /// Indicates if the entity is a phantom
        /// </summary>
        public bool IsPhantom;

        /// <summary>
        /// Indicates if the entity has the expected components
        /// </summary>
        public bool HasComponent;

        internal Result(RefTuple<T0, T1, T2, T3, T4, T5, T6> components, bool exists, bool isPhantom, bool hasComponent)
        {
            Components = components;

            Exists = exists;
            IsPhantom = isPhantom;

            HasComponent = hasComponent;
        }
    }
}

/// <summary>
/// A reference to another Entity which caches information about the Entity, including whether it has a
/// specific component type. This can be used to avoid structural checks.
/// </summary>
[ExcludeFromCodeCoverage]
public struct TypedEntityReference<T0, T1, T2, T3, T4, T5, T6, T7>
    where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
{
    private static readonly ComponentID ComponentID0 = ComponentID<T0>.ID;
    private static readonly ComponentID ComponentID1 = ComponentID<T1>.ID;
    private static readonly ComponentID ComponentID2 = ComponentID<T2>.ID;
    private static readonly ComponentID ComponentID3 = ComponentID<T3>.ID;
    private static readonly ComponentID ComponentID4 = ComponentID<T4>.ID;
    private static readonly ComponentID ComponentID5 = ComponentID<T5>.ID;
    private static readonly ComponentID ComponentID6 = ComponentID<T6>.ID;
    private static readonly ComponentID ComponentID7 = ComponentID<T7>.ID;
    private static readonly ReadOnlyMemory<ComponentID> ComponentSet = SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6, T7>.Components;

    private Cache? _cache;
    private EntityId _entity;

    /// <summary>
    /// The target entity
    /// </summary>
    public EntityId Target
    {
        get => _entity;
        set
        {
            _entity = value;
            _cache = null;
        }
    }

    /// <summary>
    /// Check the entity state
    /// </summary>
    /// <param name="world">The entity world</param>
    /// <returns></returns>
    public Result TryGetComponentRef(World world)
    {
        // Check if cache is stale
        if (_cache?.CheckIsStale(_entity) ?? true)
        {
            // Clear cache
            _cache = null;

            // Get info for this entity
            EntityInfo _ = default;
            ref readonly var info = ref world.GetEntityInfo(_entity, ref _, out var doesNotExist);

            // Create new cache
            _cache = doesNotExist
                ? Cache.DoesNotExist()
                : Cache.Create(in info);
        }

        // Cache is definitely valid now. Extract results.
        var v = _cache.Value;

        // Early out if the entity doesn't even exist
        if (v.RowIndex < 0)
            return new Result(default, false, false, false);

        // Early out if the entity doesn't have the component
        if (!v.HasComponent)
            return new Result(default, true, v.IsPhantom, false);

        // Get the component(s)
        var tuple = v.Chunk!.GetRefTuple<T0, T1, T2, T3, T4, T5, T6, T7>(
            v.RowIndex,
            ComponentID0,
            ComponentID1,
            ComponentID2,
            ComponentID3,
            ComponentID4,
            ComponentID5,
            ComponentID6,
            ComponentID7
        );

        return new Result(
            tuple,
            true,
            v.IsPhantom,
            true
        );
    }

    /// <summary>
    /// Cache data about an entity, valid if the entity is still in the same place in the same chunk
    /// </summary>
    private readonly record struct Cache
    {
        public int RowIndex { get; }
        public Chunk? Chunk { get; }

        public bool HasComponent { get; }
        public bool IsPhantom { get; }

        private Cache(int rowIndex, Chunk? chunk, bool hasComponent, bool isPhantom)
        {
            RowIndex = rowIndex;
            Chunk = chunk;
            HasComponent = hasComponent;
            IsPhantom = isPhantom;
        }

        private bool CheckNotStale(EntityId entity)
        {
            // This is the indicator for the entity not existing
            if (RowIndex < 0)
                return false;
            
            // It's only allowed to be null when the entity does not exist
            Debug.Assert(Chunk != null);

            // Check if index is out of range
            if (Chunk.EntityCount <= RowIndex)
                return false;

            // Check if the entity is still where we last saw it
            return Chunk.Entities.Span[RowIndex] == entity;
        }

        public bool CheckIsStale(EntityId entity)
        {
            return !CheckNotStale(entity);
        }

        public static Cache DoesNotExist()
        {
            return new Cache(-1, null, false, false);
        }

        public static Cache Create(ref readonly EntityInfo info)
        {
            var archetype = info.Chunk.Archetype;

            var hasComponent = archetype.Components.IsSupersetOfSortedSpan(ComponentSet.Span);
            var isPhantom = archetype.IsPhantom;

            return new Cache(
                info.RowIndex, info.Chunk, hasComponent, isPhantom
            );
        }
    }

    /// <summary>
    /// Result of trying to get the component ref from a TypedEntityReference
    /// </summary>
    public ref struct Result
    {
        /// <summary>
        /// The ref tuple, only valid if <see cref="HasComponent"/> is true
        /// </summary>
        public RefTuple<T0, T1, T2, T3, T4, T5, T6, T7> Components { get; }

        /// <summary>
        /// Indicates if the entity exists
        /// </summary>
        public bool Exists;

        /// <summary>
        /// Indicates if the entity is a phantom
        /// </summary>
        public bool IsPhantom;

        /// <summary>
        /// Indicates if the entity has the expected components
        /// </summary>
        public bool HasComponent;

        internal Result(RefTuple<T0, T1, T2, T3, T4, T5, T6, T7> components, bool exists, bool isPhantom, bool hasComponent)
        {
            Components = components;

            Exists = exists;
            IsPhantom = isPhantom;

            HasComponent = hasComponent;
        }
    }
}

/// <summary>
/// A reference to another Entity which caches information about the Entity, including whether it has a
/// specific component type. This can be used to avoid structural checks.
/// </summary>
[ExcludeFromCodeCoverage]
public struct TypedEntityReference<T0, T1, T2, T3, T4, T5, T6, T7, T8>
    where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
        where T8 : IComponent
{
    private static readonly ComponentID ComponentID0 = ComponentID<T0>.ID;
    private static readonly ComponentID ComponentID1 = ComponentID<T1>.ID;
    private static readonly ComponentID ComponentID2 = ComponentID<T2>.ID;
    private static readonly ComponentID ComponentID3 = ComponentID<T3>.ID;
    private static readonly ComponentID ComponentID4 = ComponentID<T4>.ID;
    private static readonly ComponentID ComponentID5 = ComponentID<T5>.ID;
    private static readonly ComponentID ComponentID6 = ComponentID<T6>.ID;
    private static readonly ComponentID ComponentID7 = ComponentID<T7>.ID;
    private static readonly ComponentID ComponentID8 = ComponentID<T8>.ID;
    private static readonly ReadOnlyMemory<ComponentID> ComponentSet = SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8>.Components;

    private Cache? _cache;
    private EntityId _entity;

    /// <summary>
    /// The target entity
    /// </summary>
    public EntityId Target
    {
        get => _entity;
        set
        {
            _entity = value;
            _cache = null;
        }
    }

    /// <summary>
    /// Check the entity state
    /// </summary>
    /// <param name="world">The entity world</param>
    /// <returns></returns>
    public Result TryGetComponentRef(World world)
    {
        // Check if cache is stale
        if (_cache?.CheckIsStale(_entity) ?? true)
        {
            // Clear cache
            _cache = null;

            // Get info for this entity
            EntityInfo _ = default;
            ref readonly var info = ref world.GetEntityInfo(_entity, ref _, out var doesNotExist);

            // Create new cache
            _cache = doesNotExist
                ? Cache.DoesNotExist()
                : Cache.Create(in info);
        }

        // Cache is definitely valid now. Extract results.
        var v = _cache.Value;

        // Early out if the entity doesn't even exist
        if (v.RowIndex < 0)
            return new Result(default, false, false, false);

        // Early out if the entity doesn't have the component
        if (!v.HasComponent)
            return new Result(default, true, v.IsPhantom, false);

        // Get the component(s)
        var tuple = v.Chunk!.GetRefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8>(
            v.RowIndex,
            ComponentID0,
            ComponentID1,
            ComponentID2,
            ComponentID3,
            ComponentID4,
            ComponentID5,
            ComponentID6,
            ComponentID7,
            ComponentID8
        );

        return new Result(
            tuple,
            true,
            v.IsPhantom,
            true
        );
    }

    /// <summary>
    /// Cache data about an entity, valid if the entity is still in the same place in the same chunk
    /// </summary>
    private readonly record struct Cache
    {
        public int RowIndex { get; }
        public Chunk? Chunk { get; }

        public bool HasComponent { get; }
        public bool IsPhantom { get; }

        private Cache(int rowIndex, Chunk? chunk, bool hasComponent, bool isPhantom)
        {
            RowIndex = rowIndex;
            Chunk = chunk;
            HasComponent = hasComponent;
            IsPhantom = isPhantom;
        }

        private bool CheckNotStale(EntityId entity)
        {
            // This is the indicator for the entity not existing
            if (RowIndex < 0)
                return false;
            
            // It's only allowed to be null when the entity does not exist
            Debug.Assert(Chunk != null);

            // Check if index is out of range
            if (Chunk.EntityCount <= RowIndex)
                return false;

            // Check if the entity is still where we last saw it
            return Chunk.Entities.Span[RowIndex] == entity;
        }

        public bool CheckIsStale(EntityId entity)
        {
            return !CheckNotStale(entity);
        }

        public static Cache DoesNotExist()
        {
            return new Cache(-1, null, false, false);
        }

        public static Cache Create(ref readonly EntityInfo info)
        {
            var archetype = info.Chunk.Archetype;

            var hasComponent = archetype.Components.IsSupersetOfSortedSpan(ComponentSet.Span);
            var isPhantom = archetype.IsPhantom;

            return new Cache(
                info.RowIndex, info.Chunk, hasComponent, isPhantom
            );
        }
    }

    /// <summary>
    /// Result of trying to get the component ref from a TypedEntityReference
    /// </summary>
    public ref struct Result
    {
        /// <summary>
        /// The ref tuple, only valid if <see cref="HasComponent"/> is true
        /// </summary>
        public RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8> Components { get; }

        /// <summary>
        /// Indicates if the entity exists
        /// </summary>
        public bool Exists;

        /// <summary>
        /// Indicates if the entity is a phantom
        /// </summary>
        public bool IsPhantom;

        /// <summary>
        /// Indicates if the entity has the expected components
        /// </summary>
        public bool HasComponent;

        internal Result(RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8> components, bool exists, bool isPhantom, bool hasComponent)
        {
            Components = components;

            Exists = exists;
            IsPhantom = isPhantom;

            HasComponent = hasComponent;
        }
    }
}

/// <summary>
/// A reference to another Entity which caches information about the Entity, including whether it has a
/// specific component type. This can be used to avoid structural checks.
/// </summary>
[ExcludeFromCodeCoverage]
public struct TypedEntityReference<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
    where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
        where T8 : IComponent
        where T9 : IComponent
{
    private static readonly ComponentID ComponentID0 = ComponentID<T0>.ID;
    private static readonly ComponentID ComponentID1 = ComponentID<T1>.ID;
    private static readonly ComponentID ComponentID2 = ComponentID<T2>.ID;
    private static readonly ComponentID ComponentID3 = ComponentID<T3>.ID;
    private static readonly ComponentID ComponentID4 = ComponentID<T4>.ID;
    private static readonly ComponentID ComponentID5 = ComponentID<T5>.ID;
    private static readonly ComponentID ComponentID6 = ComponentID<T6>.ID;
    private static readonly ComponentID ComponentID7 = ComponentID<T7>.ID;
    private static readonly ComponentID ComponentID8 = ComponentID<T8>.ID;
    private static readonly ComponentID ComponentID9 = ComponentID<T9>.ID;
    private static readonly ReadOnlyMemory<ComponentID> ComponentSet = SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>.Components;

    private Cache? _cache;
    private EntityId _entity;

    /// <summary>
    /// The target entity
    /// </summary>
    public EntityId Target
    {
        get => _entity;
        set
        {
            _entity = value;
            _cache = null;
        }
    }

    /// <summary>
    /// Check the entity state
    /// </summary>
    /// <param name="world">The entity world</param>
    /// <returns></returns>
    public Result TryGetComponentRef(World world)
    {
        // Check if cache is stale
        if (_cache?.CheckIsStale(_entity) ?? true)
        {
            // Clear cache
            _cache = null;

            // Get info for this entity
            EntityInfo _ = default;
            ref readonly var info = ref world.GetEntityInfo(_entity, ref _, out var doesNotExist);

            // Create new cache
            _cache = doesNotExist
                ? Cache.DoesNotExist()
                : Cache.Create(in info);
        }

        // Cache is definitely valid now. Extract results.
        var v = _cache.Value;

        // Early out if the entity doesn't even exist
        if (v.RowIndex < 0)
            return new Result(default, false, false, false);

        // Early out if the entity doesn't have the component
        if (!v.HasComponent)
            return new Result(default, true, v.IsPhantom, false);

        // Get the component(s)
        var tuple = v.Chunk!.GetRefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            v.RowIndex,
            ComponentID0,
            ComponentID1,
            ComponentID2,
            ComponentID3,
            ComponentID4,
            ComponentID5,
            ComponentID6,
            ComponentID7,
            ComponentID8,
            ComponentID9
        );

        return new Result(
            tuple,
            true,
            v.IsPhantom,
            true
        );
    }

    /// <summary>
    /// Cache data about an entity, valid if the entity is still in the same place in the same chunk
    /// </summary>
    private readonly record struct Cache
    {
        public int RowIndex { get; }
        public Chunk? Chunk { get; }

        public bool HasComponent { get; }
        public bool IsPhantom { get; }

        private Cache(int rowIndex, Chunk? chunk, bool hasComponent, bool isPhantom)
        {
            RowIndex = rowIndex;
            Chunk = chunk;
            HasComponent = hasComponent;
            IsPhantom = isPhantom;
        }

        private bool CheckNotStale(EntityId entity)
        {
            // This is the indicator for the entity not existing
            if (RowIndex < 0)
                return false;
            
            // It's only allowed to be null when the entity does not exist
            Debug.Assert(Chunk != null);

            // Check if index is out of range
            if (Chunk.EntityCount <= RowIndex)
                return false;

            // Check if the entity is still where we last saw it
            return Chunk.Entities.Span[RowIndex] == entity;
        }

        public bool CheckIsStale(EntityId entity)
        {
            return !CheckNotStale(entity);
        }

        public static Cache DoesNotExist()
        {
            return new Cache(-1, null, false, false);
        }

        public static Cache Create(ref readonly EntityInfo info)
        {
            var archetype = info.Chunk.Archetype;

            var hasComponent = archetype.Components.IsSupersetOfSortedSpan(ComponentSet.Span);
            var isPhantom = archetype.IsPhantom;

            return new Cache(
                info.RowIndex, info.Chunk, hasComponent, isPhantom
            );
        }
    }

    /// <summary>
    /// Result of trying to get the component ref from a TypedEntityReference
    /// </summary>
    public ref struct Result
    {
        /// <summary>
        /// The ref tuple, only valid if <see cref="HasComponent"/> is true
        /// </summary>
        public RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> Components { get; }

        /// <summary>
        /// Indicates if the entity exists
        /// </summary>
        public bool Exists;

        /// <summary>
        /// Indicates if the entity is a phantom
        /// </summary>
        public bool IsPhantom;

        /// <summary>
        /// Indicates if the entity has the expected components
        /// </summary>
        public bool HasComponent;

        internal Result(RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> components, bool exists, bool isPhantom, bool hasComponent)
        {
            Components = components;

            Exists = exists;
            IsPhantom = isPhantom;

            HasComponent = hasComponent;
        }
    }
}

/// <summary>
/// A reference to another Entity which caches information about the Entity, including whether it has a
/// specific component type. This can be used to avoid structural checks.
/// </summary>
[ExcludeFromCodeCoverage]
public struct TypedEntityReference<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
    where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
        where T8 : IComponent
        where T9 : IComponent
        where T10 : IComponent
{
    private static readonly ComponentID ComponentID0 = ComponentID<T0>.ID;
    private static readonly ComponentID ComponentID1 = ComponentID<T1>.ID;
    private static readonly ComponentID ComponentID2 = ComponentID<T2>.ID;
    private static readonly ComponentID ComponentID3 = ComponentID<T3>.ID;
    private static readonly ComponentID ComponentID4 = ComponentID<T4>.ID;
    private static readonly ComponentID ComponentID5 = ComponentID<T5>.ID;
    private static readonly ComponentID ComponentID6 = ComponentID<T6>.ID;
    private static readonly ComponentID ComponentID7 = ComponentID<T7>.ID;
    private static readonly ComponentID ComponentID8 = ComponentID<T8>.ID;
    private static readonly ComponentID ComponentID9 = ComponentID<T9>.ID;
    private static readonly ComponentID ComponentID10 = ComponentID<T10>.ID;
    private static readonly ReadOnlyMemory<ComponentID> ComponentSet = SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.Components;

    private Cache? _cache;
    private EntityId _entity;

    /// <summary>
    /// The target entity
    /// </summary>
    public EntityId Target
    {
        get => _entity;
        set
        {
            _entity = value;
            _cache = null;
        }
    }

    /// <summary>
    /// Check the entity state
    /// </summary>
    /// <param name="world">The entity world</param>
    /// <returns></returns>
    public Result TryGetComponentRef(World world)
    {
        // Check if cache is stale
        if (_cache?.CheckIsStale(_entity) ?? true)
        {
            // Clear cache
            _cache = null;

            // Get info for this entity
            EntityInfo _ = default;
            ref readonly var info = ref world.GetEntityInfo(_entity, ref _, out var doesNotExist);

            // Create new cache
            _cache = doesNotExist
                ? Cache.DoesNotExist()
                : Cache.Create(in info);
        }

        // Cache is definitely valid now. Extract results.
        var v = _cache.Value;

        // Early out if the entity doesn't even exist
        if (v.RowIndex < 0)
            return new Result(default, false, false, false);

        // Early out if the entity doesn't have the component
        if (!v.HasComponent)
            return new Result(default, true, v.IsPhantom, false);

        // Get the component(s)
        var tuple = v.Chunk!.GetRefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            v.RowIndex,
            ComponentID0,
            ComponentID1,
            ComponentID2,
            ComponentID3,
            ComponentID4,
            ComponentID5,
            ComponentID6,
            ComponentID7,
            ComponentID8,
            ComponentID9,
            ComponentID10
        );

        return new Result(
            tuple,
            true,
            v.IsPhantom,
            true
        );
    }

    /// <summary>
    /// Cache data about an entity, valid if the entity is still in the same place in the same chunk
    /// </summary>
    private readonly record struct Cache
    {
        public int RowIndex { get; }
        public Chunk? Chunk { get; }

        public bool HasComponent { get; }
        public bool IsPhantom { get; }

        private Cache(int rowIndex, Chunk? chunk, bool hasComponent, bool isPhantom)
        {
            RowIndex = rowIndex;
            Chunk = chunk;
            HasComponent = hasComponent;
            IsPhantom = isPhantom;
        }

        private bool CheckNotStale(EntityId entity)
        {
            // This is the indicator for the entity not existing
            if (RowIndex < 0)
                return false;
            
            // It's only allowed to be null when the entity does not exist
            Debug.Assert(Chunk != null);

            // Check if index is out of range
            if (Chunk.EntityCount <= RowIndex)
                return false;

            // Check if the entity is still where we last saw it
            return Chunk.Entities.Span[RowIndex] == entity;
        }

        public bool CheckIsStale(EntityId entity)
        {
            return !CheckNotStale(entity);
        }

        public static Cache DoesNotExist()
        {
            return new Cache(-1, null, false, false);
        }

        public static Cache Create(ref readonly EntityInfo info)
        {
            var archetype = info.Chunk.Archetype;

            var hasComponent = archetype.Components.IsSupersetOfSortedSpan(ComponentSet.Span);
            var isPhantom = archetype.IsPhantom;

            return new Cache(
                info.RowIndex, info.Chunk, hasComponent, isPhantom
            );
        }
    }

    /// <summary>
    /// Result of trying to get the component ref from a TypedEntityReference
    /// </summary>
    public ref struct Result
    {
        /// <summary>
        /// The ref tuple, only valid if <see cref="HasComponent"/> is true
        /// </summary>
        public RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Components { get; }

        /// <summary>
        /// Indicates if the entity exists
        /// </summary>
        public bool Exists;

        /// <summary>
        /// Indicates if the entity is a phantom
        /// </summary>
        public bool IsPhantom;

        /// <summary>
        /// Indicates if the entity has the expected components
        /// </summary>
        public bool HasComponent;

        internal Result(RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> components, bool exists, bool isPhantom, bool hasComponent)
        {
            Components = components;

            Exists = exists;
            IsPhantom = isPhantom;

            HasComponent = hasComponent;
        }
    }
}

/// <summary>
/// A reference to another Entity which caches information about the Entity, including whether it has a
/// specific component type. This can be used to avoid structural checks.
/// </summary>
[ExcludeFromCodeCoverage]
public struct TypedEntityReference<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
    where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
        where T8 : IComponent
        where T9 : IComponent
        where T10 : IComponent
        where T11 : IComponent
{
    private static readonly ComponentID ComponentID0 = ComponentID<T0>.ID;
    private static readonly ComponentID ComponentID1 = ComponentID<T1>.ID;
    private static readonly ComponentID ComponentID2 = ComponentID<T2>.ID;
    private static readonly ComponentID ComponentID3 = ComponentID<T3>.ID;
    private static readonly ComponentID ComponentID4 = ComponentID<T4>.ID;
    private static readonly ComponentID ComponentID5 = ComponentID<T5>.ID;
    private static readonly ComponentID ComponentID6 = ComponentID<T6>.ID;
    private static readonly ComponentID ComponentID7 = ComponentID<T7>.ID;
    private static readonly ComponentID ComponentID8 = ComponentID<T8>.ID;
    private static readonly ComponentID ComponentID9 = ComponentID<T9>.ID;
    private static readonly ComponentID ComponentID10 = ComponentID<T10>.ID;
    private static readonly ComponentID ComponentID11 = ComponentID<T11>.ID;
    private static readonly ReadOnlyMemory<ComponentID> ComponentSet = SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.Components;

    private Cache? _cache;
    private EntityId _entity;

    /// <summary>
    /// The target entity
    /// </summary>
    public EntityId Target
    {
        get => _entity;
        set
        {
            _entity = value;
            _cache = null;
        }
    }

    /// <summary>
    /// Check the entity state
    /// </summary>
    /// <param name="world">The entity world</param>
    /// <returns></returns>
    public Result TryGetComponentRef(World world)
    {
        // Check if cache is stale
        if (_cache?.CheckIsStale(_entity) ?? true)
        {
            // Clear cache
            _cache = null;

            // Get info for this entity
            EntityInfo _ = default;
            ref readonly var info = ref world.GetEntityInfo(_entity, ref _, out var doesNotExist);

            // Create new cache
            _cache = doesNotExist
                ? Cache.DoesNotExist()
                : Cache.Create(in info);
        }

        // Cache is definitely valid now. Extract results.
        var v = _cache.Value;

        // Early out if the entity doesn't even exist
        if (v.RowIndex < 0)
            return new Result(default, false, false, false);

        // Early out if the entity doesn't have the component
        if (!v.HasComponent)
            return new Result(default, true, v.IsPhantom, false);

        // Get the component(s)
        var tuple = v.Chunk!.GetRefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            v.RowIndex,
            ComponentID0,
            ComponentID1,
            ComponentID2,
            ComponentID3,
            ComponentID4,
            ComponentID5,
            ComponentID6,
            ComponentID7,
            ComponentID8,
            ComponentID9,
            ComponentID10,
            ComponentID11
        );

        return new Result(
            tuple,
            true,
            v.IsPhantom,
            true
        );
    }

    /// <summary>
    /// Cache data about an entity, valid if the entity is still in the same place in the same chunk
    /// </summary>
    private readonly record struct Cache
    {
        public int RowIndex { get; }
        public Chunk? Chunk { get; }

        public bool HasComponent { get; }
        public bool IsPhantom { get; }

        private Cache(int rowIndex, Chunk? chunk, bool hasComponent, bool isPhantom)
        {
            RowIndex = rowIndex;
            Chunk = chunk;
            HasComponent = hasComponent;
            IsPhantom = isPhantom;
        }

        private bool CheckNotStale(EntityId entity)
        {
            // This is the indicator for the entity not existing
            if (RowIndex < 0)
                return false;
            
            // It's only allowed to be null when the entity does not exist
            Debug.Assert(Chunk != null);

            // Check if index is out of range
            if (Chunk.EntityCount <= RowIndex)
                return false;

            // Check if the entity is still where we last saw it
            return Chunk.Entities.Span[RowIndex] == entity;
        }

        public bool CheckIsStale(EntityId entity)
        {
            return !CheckNotStale(entity);
        }

        public static Cache DoesNotExist()
        {
            return new Cache(-1, null, false, false);
        }

        public static Cache Create(ref readonly EntityInfo info)
        {
            var archetype = info.Chunk.Archetype;

            var hasComponent = archetype.Components.IsSupersetOfSortedSpan(ComponentSet.Span);
            var isPhantom = archetype.IsPhantom;

            return new Cache(
                info.RowIndex, info.Chunk, hasComponent, isPhantom
            );
        }
    }

    /// <summary>
    /// Result of trying to get the component ref from a TypedEntityReference
    /// </summary>
    public ref struct Result
    {
        /// <summary>
        /// The ref tuple, only valid if <see cref="HasComponent"/> is true
        /// </summary>
        public RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Components { get; }

        /// <summary>
        /// Indicates if the entity exists
        /// </summary>
        public bool Exists;

        /// <summary>
        /// Indicates if the entity is a phantom
        /// </summary>
        public bool IsPhantom;

        /// <summary>
        /// Indicates if the entity has the expected components
        /// </summary>
        public bool HasComponent;

        internal Result(RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> components, bool exists, bool isPhantom, bool hasComponent)
        {
            Components = components;

            Exists = exists;
            IsPhantom = isPhantom;

            HasComponent = hasComponent;
        }
    }
}

/// <summary>
/// A reference to another Entity which caches information about the Entity, including whether it has a
/// specific component type. This can be used to avoid structural checks.
/// </summary>
[ExcludeFromCodeCoverage]
public struct TypedEntityReference<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
    where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
        where T8 : IComponent
        where T9 : IComponent
        where T10 : IComponent
        where T11 : IComponent
        where T12 : IComponent
{
    private static readonly ComponentID ComponentID0 = ComponentID<T0>.ID;
    private static readonly ComponentID ComponentID1 = ComponentID<T1>.ID;
    private static readonly ComponentID ComponentID2 = ComponentID<T2>.ID;
    private static readonly ComponentID ComponentID3 = ComponentID<T3>.ID;
    private static readonly ComponentID ComponentID4 = ComponentID<T4>.ID;
    private static readonly ComponentID ComponentID5 = ComponentID<T5>.ID;
    private static readonly ComponentID ComponentID6 = ComponentID<T6>.ID;
    private static readonly ComponentID ComponentID7 = ComponentID<T7>.ID;
    private static readonly ComponentID ComponentID8 = ComponentID<T8>.ID;
    private static readonly ComponentID ComponentID9 = ComponentID<T9>.ID;
    private static readonly ComponentID ComponentID10 = ComponentID<T10>.ID;
    private static readonly ComponentID ComponentID11 = ComponentID<T11>.ID;
    private static readonly ComponentID ComponentID12 = ComponentID<T12>.ID;
    private static readonly ReadOnlyMemory<ComponentID> ComponentSet = SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.Components;

    private Cache? _cache;
    private EntityId _entity;

    /// <summary>
    /// The target entity
    /// </summary>
    public EntityId Target
    {
        get => _entity;
        set
        {
            _entity = value;
            _cache = null;
        }
    }

    /// <summary>
    /// Check the entity state
    /// </summary>
    /// <param name="world">The entity world</param>
    /// <returns></returns>
    public Result TryGetComponentRef(World world)
    {
        // Check if cache is stale
        if (_cache?.CheckIsStale(_entity) ?? true)
        {
            // Clear cache
            _cache = null;

            // Get info for this entity
            EntityInfo _ = default;
            ref readonly var info = ref world.GetEntityInfo(_entity, ref _, out var doesNotExist);

            // Create new cache
            _cache = doesNotExist
                ? Cache.DoesNotExist()
                : Cache.Create(in info);
        }

        // Cache is definitely valid now. Extract results.
        var v = _cache.Value;

        // Early out if the entity doesn't even exist
        if (v.RowIndex < 0)
            return new Result(default, false, false, false);

        // Early out if the entity doesn't have the component
        if (!v.HasComponent)
            return new Result(default, true, v.IsPhantom, false);

        // Get the component(s)
        var tuple = v.Chunk!.GetRefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            v.RowIndex,
            ComponentID0,
            ComponentID1,
            ComponentID2,
            ComponentID3,
            ComponentID4,
            ComponentID5,
            ComponentID6,
            ComponentID7,
            ComponentID8,
            ComponentID9,
            ComponentID10,
            ComponentID11,
            ComponentID12
        );

        return new Result(
            tuple,
            true,
            v.IsPhantom,
            true
        );
    }

    /// <summary>
    /// Cache data about an entity, valid if the entity is still in the same place in the same chunk
    /// </summary>
    private readonly record struct Cache
    {
        public int RowIndex { get; }
        public Chunk? Chunk { get; }

        public bool HasComponent { get; }
        public bool IsPhantom { get; }

        private Cache(int rowIndex, Chunk? chunk, bool hasComponent, bool isPhantom)
        {
            RowIndex = rowIndex;
            Chunk = chunk;
            HasComponent = hasComponent;
            IsPhantom = isPhantom;
        }

        private bool CheckNotStale(EntityId entity)
        {
            // This is the indicator for the entity not existing
            if (RowIndex < 0)
                return false;
            
            // It's only allowed to be null when the entity does not exist
            Debug.Assert(Chunk != null);

            // Check if index is out of range
            if (Chunk.EntityCount <= RowIndex)
                return false;

            // Check if the entity is still where we last saw it
            return Chunk.Entities.Span[RowIndex] == entity;
        }

        public bool CheckIsStale(EntityId entity)
        {
            return !CheckNotStale(entity);
        }

        public static Cache DoesNotExist()
        {
            return new Cache(-1, null, false, false);
        }

        public static Cache Create(ref readonly EntityInfo info)
        {
            var archetype = info.Chunk.Archetype;

            var hasComponent = archetype.Components.IsSupersetOfSortedSpan(ComponentSet.Span);
            var isPhantom = archetype.IsPhantom;

            return new Cache(
                info.RowIndex, info.Chunk, hasComponent, isPhantom
            );
        }
    }

    /// <summary>
    /// Result of trying to get the component ref from a TypedEntityReference
    /// </summary>
    public ref struct Result
    {
        /// <summary>
        /// The ref tuple, only valid if <see cref="HasComponent"/> is true
        /// </summary>
        public RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Components { get; }

        /// <summary>
        /// Indicates if the entity exists
        /// </summary>
        public bool Exists;

        /// <summary>
        /// Indicates if the entity is a phantom
        /// </summary>
        public bool IsPhantom;

        /// <summary>
        /// Indicates if the entity has the expected components
        /// </summary>
        public bool HasComponent;

        internal Result(RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> components, bool exists, bool isPhantom, bool hasComponent)
        {
            Components = components;

            Exists = exists;
            IsPhantom = isPhantom;

            HasComponent = hasComponent;
        }
    }
}

/// <summary>
/// A reference to another Entity which caches information about the Entity, including whether it has a
/// specific component type. This can be used to avoid structural checks.
/// </summary>
[ExcludeFromCodeCoverage]
public struct TypedEntityReference<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
    where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
        where T8 : IComponent
        where T9 : IComponent
        where T10 : IComponent
        where T11 : IComponent
        where T12 : IComponent
        where T13 : IComponent
{
    private static readonly ComponentID ComponentID0 = ComponentID<T0>.ID;
    private static readonly ComponentID ComponentID1 = ComponentID<T1>.ID;
    private static readonly ComponentID ComponentID2 = ComponentID<T2>.ID;
    private static readonly ComponentID ComponentID3 = ComponentID<T3>.ID;
    private static readonly ComponentID ComponentID4 = ComponentID<T4>.ID;
    private static readonly ComponentID ComponentID5 = ComponentID<T5>.ID;
    private static readonly ComponentID ComponentID6 = ComponentID<T6>.ID;
    private static readonly ComponentID ComponentID7 = ComponentID<T7>.ID;
    private static readonly ComponentID ComponentID8 = ComponentID<T8>.ID;
    private static readonly ComponentID ComponentID9 = ComponentID<T9>.ID;
    private static readonly ComponentID ComponentID10 = ComponentID<T10>.ID;
    private static readonly ComponentID ComponentID11 = ComponentID<T11>.ID;
    private static readonly ComponentID ComponentID12 = ComponentID<T12>.ID;
    private static readonly ComponentID ComponentID13 = ComponentID<T13>.ID;
    private static readonly ReadOnlyMemory<ComponentID> ComponentSet = SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.Components;

    private Cache? _cache;
    private EntityId _entity;

    /// <summary>
    /// The target entity
    /// </summary>
    public EntityId Target
    {
        get => _entity;
        set
        {
            _entity = value;
            _cache = null;
        }
    }

    /// <summary>
    /// Check the entity state
    /// </summary>
    /// <param name="world">The entity world</param>
    /// <returns></returns>
    public Result TryGetComponentRef(World world)
    {
        // Check if cache is stale
        if (_cache?.CheckIsStale(_entity) ?? true)
        {
            // Clear cache
            _cache = null;

            // Get info for this entity
            EntityInfo _ = default;
            ref readonly var info = ref world.GetEntityInfo(_entity, ref _, out var doesNotExist);

            // Create new cache
            _cache = doesNotExist
                ? Cache.DoesNotExist()
                : Cache.Create(in info);
        }

        // Cache is definitely valid now. Extract results.
        var v = _cache.Value;

        // Early out if the entity doesn't even exist
        if (v.RowIndex < 0)
            return new Result(default, false, false, false);

        // Early out if the entity doesn't have the component
        if (!v.HasComponent)
            return new Result(default, true, v.IsPhantom, false);

        // Get the component(s)
        var tuple = v.Chunk!.GetRefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            v.RowIndex,
            ComponentID0,
            ComponentID1,
            ComponentID2,
            ComponentID3,
            ComponentID4,
            ComponentID5,
            ComponentID6,
            ComponentID7,
            ComponentID8,
            ComponentID9,
            ComponentID10,
            ComponentID11,
            ComponentID12,
            ComponentID13
        );

        return new Result(
            tuple,
            true,
            v.IsPhantom,
            true
        );
    }

    /// <summary>
    /// Cache data about an entity, valid if the entity is still in the same place in the same chunk
    /// </summary>
    private readonly record struct Cache
    {
        public int RowIndex { get; }
        public Chunk? Chunk { get; }

        public bool HasComponent { get; }
        public bool IsPhantom { get; }

        private Cache(int rowIndex, Chunk? chunk, bool hasComponent, bool isPhantom)
        {
            RowIndex = rowIndex;
            Chunk = chunk;
            HasComponent = hasComponent;
            IsPhantom = isPhantom;
        }

        private bool CheckNotStale(EntityId entity)
        {
            // This is the indicator for the entity not existing
            if (RowIndex < 0)
                return false;
            
            // It's only allowed to be null when the entity does not exist
            Debug.Assert(Chunk != null);

            // Check if index is out of range
            if (Chunk.EntityCount <= RowIndex)
                return false;

            // Check if the entity is still where we last saw it
            return Chunk.Entities.Span[RowIndex] == entity;
        }

        public bool CheckIsStale(EntityId entity)
        {
            return !CheckNotStale(entity);
        }

        public static Cache DoesNotExist()
        {
            return new Cache(-1, null, false, false);
        }

        public static Cache Create(ref readonly EntityInfo info)
        {
            var archetype = info.Chunk.Archetype;

            var hasComponent = archetype.Components.IsSupersetOfSortedSpan(ComponentSet.Span);
            var isPhantom = archetype.IsPhantom;

            return new Cache(
                info.RowIndex, info.Chunk, hasComponent, isPhantom
            );
        }
    }

    /// <summary>
    /// Result of trying to get the component ref from a TypedEntityReference
    /// </summary>
    public ref struct Result
    {
        /// <summary>
        /// The ref tuple, only valid if <see cref="HasComponent"/> is true
        /// </summary>
        public RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> Components { get; }

        /// <summary>
        /// Indicates if the entity exists
        /// </summary>
        public bool Exists;

        /// <summary>
        /// Indicates if the entity is a phantom
        /// </summary>
        public bool IsPhantom;

        /// <summary>
        /// Indicates if the entity has the expected components
        /// </summary>
        public bool HasComponent;

        internal Result(RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> components, bool exists, bool isPhantom, bool hasComponent)
        {
            Components = components;

            Exists = exists;
            IsPhantom = isPhantom;

            HasComponent = hasComponent;
        }
    }
}

/// <summary>
/// A reference to another Entity which caches information about the Entity, including whether it has a
/// specific component type. This can be used to avoid structural checks.
/// </summary>
[ExcludeFromCodeCoverage]
public struct TypedEntityReference<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
    where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
        where T8 : IComponent
        where T9 : IComponent
        where T10 : IComponent
        where T11 : IComponent
        where T12 : IComponent
        where T13 : IComponent
        where T14 : IComponent
{
    private static readonly ComponentID ComponentID0 = ComponentID<T0>.ID;
    private static readonly ComponentID ComponentID1 = ComponentID<T1>.ID;
    private static readonly ComponentID ComponentID2 = ComponentID<T2>.ID;
    private static readonly ComponentID ComponentID3 = ComponentID<T3>.ID;
    private static readonly ComponentID ComponentID4 = ComponentID<T4>.ID;
    private static readonly ComponentID ComponentID5 = ComponentID<T5>.ID;
    private static readonly ComponentID ComponentID6 = ComponentID<T6>.ID;
    private static readonly ComponentID ComponentID7 = ComponentID<T7>.ID;
    private static readonly ComponentID ComponentID8 = ComponentID<T8>.ID;
    private static readonly ComponentID ComponentID9 = ComponentID<T9>.ID;
    private static readonly ComponentID ComponentID10 = ComponentID<T10>.ID;
    private static readonly ComponentID ComponentID11 = ComponentID<T11>.ID;
    private static readonly ComponentID ComponentID12 = ComponentID<T12>.ID;
    private static readonly ComponentID ComponentID13 = ComponentID<T13>.ID;
    private static readonly ComponentID ComponentID14 = ComponentID<T14>.ID;
    private static readonly ReadOnlyMemory<ComponentID> ComponentSet = SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.Components;

    private Cache? _cache;
    private EntityId _entity;

    /// <summary>
    /// The target entity
    /// </summary>
    public EntityId Target
    {
        get => _entity;
        set
        {
            _entity = value;
            _cache = null;
        }
    }

    /// <summary>
    /// Check the entity state
    /// </summary>
    /// <param name="world">The entity world</param>
    /// <returns></returns>
    public Result TryGetComponentRef(World world)
    {
        // Check if cache is stale
        if (_cache?.CheckIsStale(_entity) ?? true)
        {
            // Clear cache
            _cache = null;

            // Get info for this entity
            EntityInfo _ = default;
            ref readonly var info = ref world.GetEntityInfo(_entity, ref _, out var doesNotExist);

            // Create new cache
            _cache = doesNotExist
                ? Cache.DoesNotExist()
                : Cache.Create(in info);
        }

        // Cache is definitely valid now. Extract results.
        var v = _cache.Value;

        // Early out if the entity doesn't even exist
        if (v.RowIndex < 0)
            return new Result(default, false, false, false);

        // Early out if the entity doesn't have the component
        if (!v.HasComponent)
            return new Result(default, true, v.IsPhantom, false);

        // Get the component(s)
        var tuple = v.Chunk!.GetRefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            v.RowIndex,
            ComponentID0,
            ComponentID1,
            ComponentID2,
            ComponentID3,
            ComponentID4,
            ComponentID5,
            ComponentID6,
            ComponentID7,
            ComponentID8,
            ComponentID9,
            ComponentID10,
            ComponentID11,
            ComponentID12,
            ComponentID13,
            ComponentID14
        );

        return new Result(
            tuple,
            true,
            v.IsPhantom,
            true
        );
    }

    /// <summary>
    /// Cache data about an entity, valid if the entity is still in the same place in the same chunk
    /// </summary>
    private readonly record struct Cache
    {
        public int RowIndex { get; }
        public Chunk? Chunk { get; }

        public bool HasComponent { get; }
        public bool IsPhantom { get; }

        private Cache(int rowIndex, Chunk? chunk, bool hasComponent, bool isPhantom)
        {
            RowIndex = rowIndex;
            Chunk = chunk;
            HasComponent = hasComponent;
            IsPhantom = isPhantom;
        }

        private bool CheckNotStale(EntityId entity)
        {
            // This is the indicator for the entity not existing
            if (RowIndex < 0)
                return false;
            
            // It's only allowed to be null when the entity does not exist
            Debug.Assert(Chunk != null);

            // Check if index is out of range
            if (Chunk.EntityCount <= RowIndex)
                return false;

            // Check if the entity is still where we last saw it
            return Chunk.Entities.Span[RowIndex] == entity;
        }

        public bool CheckIsStale(EntityId entity)
        {
            return !CheckNotStale(entity);
        }

        public static Cache DoesNotExist()
        {
            return new Cache(-1, null, false, false);
        }

        public static Cache Create(ref readonly EntityInfo info)
        {
            var archetype = info.Chunk.Archetype;

            var hasComponent = archetype.Components.IsSupersetOfSortedSpan(ComponentSet.Span);
            var isPhantom = archetype.IsPhantom;

            return new Cache(
                info.RowIndex, info.Chunk, hasComponent, isPhantom
            );
        }
    }

    /// <summary>
    /// Result of trying to get the component ref from a TypedEntityReference
    /// </summary>
    public ref struct Result
    {
        /// <summary>
        /// The ref tuple, only valid if <see cref="HasComponent"/> is true
        /// </summary>
        public RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> Components { get; }

        /// <summary>
        /// Indicates if the entity exists
        /// </summary>
        public bool Exists;

        /// <summary>
        /// Indicates if the entity is a phantom
        /// </summary>
        public bool IsPhantom;

        /// <summary>
        /// Indicates if the entity has the expected components
        /// </summary>
        public bool HasComponent;

        internal Result(RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> components, bool exists, bool isPhantom, bool hasComponent)
        {
            Components = components;

            Exists = exists;
            IsPhantom = isPhantom;

            HasComponent = hasComponent;
        }
    }
}

/// <summary>
/// A reference to another Entity which caches information about the Entity, including whether it has a
/// specific component type. This can be used to avoid structural checks.
/// </summary>
[ExcludeFromCodeCoverage]
public struct TypedEntityReference<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
    where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
        where T8 : IComponent
        where T9 : IComponent
        where T10 : IComponent
        where T11 : IComponent
        where T12 : IComponent
        where T13 : IComponent
        where T14 : IComponent
        where T15 : IComponent
{
    private static readonly ComponentID ComponentID0 = ComponentID<T0>.ID;
    private static readonly ComponentID ComponentID1 = ComponentID<T1>.ID;
    private static readonly ComponentID ComponentID2 = ComponentID<T2>.ID;
    private static readonly ComponentID ComponentID3 = ComponentID<T3>.ID;
    private static readonly ComponentID ComponentID4 = ComponentID<T4>.ID;
    private static readonly ComponentID ComponentID5 = ComponentID<T5>.ID;
    private static readonly ComponentID ComponentID6 = ComponentID<T6>.ID;
    private static readonly ComponentID ComponentID7 = ComponentID<T7>.ID;
    private static readonly ComponentID ComponentID8 = ComponentID<T8>.ID;
    private static readonly ComponentID ComponentID9 = ComponentID<T9>.ID;
    private static readonly ComponentID ComponentID10 = ComponentID<T10>.ID;
    private static readonly ComponentID ComponentID11 = ComponentID<T11>.ID;
    private static readonly ComponentID ComponentID12 = ComponentID<T12>.ID;
    private static readonly ComponentID ComponentID13 = ComponentID<T13>.ID;
    private static readonly ComponentID ComponentID14 = ComponentID<T14>.ID;
    private static readonly ComponentID ComponentID15 = ComponentID<T15>.ID;
    private static readonly ReadOnlyMemory<ComponentID> ComponentSet = SortedListOfComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.Components;

    private Cache? _cache;
    private EntityId _entity;

    /// <summary>
    /// The target entity
    /// </summary>
    public EntityId Target
    {
        get => _entity;
        set
        {
            _entity = value;
            _cache = null;
        }
    }

    /// <summary>
    /// Check the entity state
    /// </summary>
    /// <param name="world">The entity world</param>
    /// <returns></returns>
    public Result TryGetComponentRef(World world)
    {
        // Check if cache is stale
        if (_cache?.CheckIsStale(_entity) ?? true)
        {
            // Clear cache
            _cache = null;

            // Get info for this entity
            EntityInfo _ = default;
            ref readonly var info = ref world.GetEntityInfo(_entity, ref _, out var doesNotExist);

            // Create new cache
            _cache = doesNotExist
                ? Cache.DoesNotExist()
                : Cache.Create(in info);
        }

        // Cache is definitely valid now. Extract results.
        var v = _cache.Value;

        // Early out if the entity doesn't even exist
        if (v.RowIndex < 0)
            return new Result(default, false, false, false);

        // Early out if the entity doesn't have the component
        if (!v.HasComponent)
            return new Result(default, true, v.IsPhantom, false);

        // Get the component(s)
        var tuple = v.Chunk!.GetRefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            v.RowIndex,
            ComponentID0,
            ComponentID1,
            ComponentID2,
            ComponentID3,
            ComponentID4,
            ComponentID5,
            ComponentID6,
            ComponentID7,
            ComponentID8,
            ComponentID9,
            ComponentID10,
            ComponentID11,
            ComponentID12,
            ComponentID13,
            ComponentID14,
            ComponentID15
        );

        return new Result(
            tuple,
            true,
            v.IsPhantom,
            true
        );
    }

    /// <summary>
    /// Cache data about an entity, valid if the entity is still in the same place in the same chunk
    /// </summary>
    private readonly record struct Cache
    {
        public int RowIndex { get; }
        public Chunk? Chunk { get; }

        public bool HasComponent { get; }
        public bool IsPhantom { get; }

        private Cache(int rowIndex, Chunk? chunk, bool hasComponent, bool isPhantom)
        {
            RowIndex = rowIndex;
            Chunk = chunk;
            HasComponent = hasComponent;
            IsPhantom = isPhantom;
        }

        private bool CheckNotStale(EntityId entity)
        {
            // This is the indicator for the entity not existing
            if (RowIndex < 0)
                return false;
            
            // It's only allowed to be null when the entity does not exist
            Debug.Assert(Chunk != null);

            // Check if index is out of range
            if (Chunk.EntityCount <= RowIndex)
                return false;

            // Check if the entity is still where we last saw it
            return Chunk.Entities.Span[RowIndex] == entity;
        }

        public bool CheckIsStale(EntityId entity)
        {
            return !CheckNotStale(entity);
        }

        public static Cache DoesNotExist()
        {
            return new Cache(-1, null, false, false);
        }

        public static Cache Create(ref readonly EntityInfo info)
        {
            var archetype = info.Chunk.Archetype;

            var hasComponent = archetype.Components.IsSupersetOfSortedSpan(ComponentSet.Span);
            var isPhantom = archetype.IsPhantom;

            return new Cache(
                info.RowIndex, info.Chunk, hasComponent, isPhantom
            );
        }
    }

    /// <summary>
    /// Result of trying to get the component ref from a TypedEntityReference
    /// </summary>
    public ref struct Result
    {
        /// <summary>
        /// The ref tuple, only valid if <see cref="HasComponent"/> is true
        /// </summary>
        public RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> Components { get; }

        /// <summary>
        /// Indicates if the entity exists
        /// </summary>
        public bool Exists;

        /// <summary>
        /// Indicates if the entity is a phantom
        /// </summary>
        public bool IsPhantom;

        /// <summary>
        /// Indicates if the entity has the expected components
        /// </summary>
        public bool HasComponent;

        internal Result(RefTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> components, bool exists, bool isPhantom, bool hasComponent)
        {
            Components = components;

            Exists = exists;
            IsPhantom = isPhantom;

            HasComponent = hasComponent;
        }
    }
}


