using System.Diagnostics.CodeAnalysis;
using Myriad.ECS.IDs;
using Myriad.ECS.Queries;
using Myriad.ECS.Worlds;

// ReSharper disable UnusedType.Global
// ReSharper disable StaticMemberInGenericType

namespace Myriad.ECS.Paths;

public readonly partial struct Path
{
    /// <summary>
    /// Base class for path steps that take generic parameters
    /// </summary>
    /// <typeparam name="T0"></typeparam>
    [ExcludeFromCodeCoverage]
    public abstract class BaseGenericStep<T0>
        : IStep
        where T0 : IComponent
    {
#region Component IDs
        /// <summary>Component ID for T0</summary>
        protected static readonly ComponentID C0 = ComponentID<T0>.ID;

        /// <summary>All components IDs, in order</summary>
        protected static readonly ReadOnlyMemory<ComponentID> SortedComponentIDs;

        static BaseGenericStep()
        {
            var arr = new[] {
                C0,
            };
            Array.Sort(arr);

            SortedComponentIDs = arr;
        }
        #endregion

        /// <inheritdoc />
        public abstract bool TryFollow(ref Entity entity);
    }

    /// <summary>
    /// Construct a path step that checks if a predicate is true
    /// </summary>
    /// <typeparam name="P">Predicate</typeparam>
    /// <typeparam name="T0"></typeparam>
    
    public sealed class Predicate<P, T0>
        : BaseGenericStep<T0>
        where P : IQueryMap<bool, T0>
        where T0 : IComponent
    {
        private readonly P _predicate;

        /// <summary>
        /// Create a new <see cref="Predicate{P,T0}"/>
        /// </summary>
        /// <param name="predicate"></param>
        public Predicate(P predicate)
        {
            _predicate = predicate;
        }

        /// <inheritdoc />
        public override bool TryFollow(ref Entity entity)
        {
            // Try to get entity info for this entity
            var dummy = default(EntityInfo);
            ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

            // Can't follow a path through a dead entity!
            if (isDead)
                return false;

            // Check if all components are present in one go
            var components = entityInfo.Chunk.Archetype.Components;
            if (!components.IsSupersetOfSortedSpan(SortedComponentIDs.Span))
                return false;

            // Get component references
            var row = entityInfo.GetRow(entity.ID);
            ref var t0 = ref row.GetMutable<T0>(C0);

            // Execute predicate
            return _predicate.Execute(entity, ref t0);
        }
    }

    /// <summary>
    /// Check that the current entity has the given components, fail to follow the path if any are missing
    /// </summary>
    /// <typeparam name="T0"></typeparam>
    
    public sealed class HasComponents<T0>
        : BaseGenericStep<T0>
        where T0 : IComponent
    {
        /// <inheritdoc />
        public override bool TryFollow(ref Entity entity)
        {
            // Try to get entity info for this entity
            var dummy = default(EntityInfo);
            ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

            // Can't follow a path through a dead entity!
            if (isDead)
                return false;

            // Check if all components are present in one go
            var components = entityInfo.Chunk.Archetype.Components;
            if (!components.IsSupersetOfSortedSpan(SortedComponentIDs.Span))
                return false;

            return true;
        }
    }

    /// <summary>
    /// Try to follow a component, using a mapper to extract an entity from it
    /// </summary>
    /// <typeparam name="M"></typeparam>
    /// <typeparam name="T0"></typeparam>
    
    public sealed class Follow<M, T0>
        : BaseGenericStep<T0>
        where M : IQueryMap<Entity, T0>
        where T0 : IComponent
    {
        private readonly M _map;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        public Follow(M map)
        {
            _map = map;
        }

        /// <inheritdoc />
        public override bool TryFollow(ref Entity entity)
        {
            // Try to get entity info for this entity
            var dummy = default(EntityInfo);
            ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

            // Can't follow a path through a dead entity!
            if (isDead)
                return false;

            // Check if all components are present in one go
            var components = entityInfo.Chunk.Archetype.Components;
            if (!components.IsSupersetOfSortedSpan(SortedComponentIDs.Span))
                return false;

            // Get component references
            var row = entityInfo.GetRow(entity.ID);
            ref var t0 = ref row.GetMutable<T0>(C0);

            // Execute predicate
            entity = _map.Execute(entity, ref t0);
            return true;
        }
    }

    /// <summary>
    /// Base class for path steps that take generic parameters
    /// </summary>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    [ExcludeFromCodeCoverage]
    public abstract class BaseGenericStep<T0, T1>
        : IStep
        where T0 : IComponent
        where T1 : IComponent
    {
#region Component IDs
        /// <summary>Component ID for T0</summary>
        protected static readonly ComponentID C0 = ComponentID<T0>.ID;
        /// <summary>Component ID for T1</summary>
        protected static readonly ComponentID C1 = ComponentID<T1>.ID;

        /// <summary>All components IDs, in order</summary>
        protected static readonly ReadOnlyMemory<ComponentID> SortedComponentIDs;

        static BaseGenericStep()
        {
            var arr = new[] {
                C0,
                C1,
            };
            Array.Sort(arr);

            SortedComponentIDs = arr;
        }
        #endregion

        /// <inheritdoc />
        public abstract bool TryFollow(ref Entity entity);
    }

    /// <summary>
    /// Construct a path step that checks if a predicate is true
    /// </summary>
    /// <typeparam name="P">Predicate</typeparam>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    
    public sealed class Predicate<P, T0, T1>
        : BaseGenericStep<T0, T1>
        where P : IQueryMap<bool, T0, T1>
        where T0 : IComponent
        where T1 : IComponent
    {
        private readonly P _predicate;

        /// <summary>
        /// Create a new <see cref="Predicate{P,T0}"/>
        /// </summary>
        /// <param name="predicate"></param>
        public Predicate(P predicate)
        {
            _predicate = predicate;
        }

        /// <inheritdoc />
        public override bool TryFollow(ref Entity entity)
        {
            // Try to get entity info for this entity
            var dummy = default(EntityInfo);
            ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

            // Can't follow a path through a dead entity!
            if (isDead)
                return false;

            // Check if all components are present in one go
            var components = entityInfo.Chunk.Archetype.Components;
            if (!components.IsSupersetOfSortedSpan(SortedComponentIDs.Span))
                return false;

            // Get component references
            var row = entityInfo.GetRow(entity.ID);
            ref var t0 = ref row.GetMutable<T0>(C0);
            ref var t1 = ref row.GetMutable<T1>(C1);

            // Execute predicate
            return _predicate.Execute(entity, ref t0, ref t1);
        }
    }

    /// <summary>
    /// Check that the current entity has the given components, fail to follow the path if any are missing
    /// </summary>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    
    public sealed class HasComponents<T0, T1>
        : BaseGenericStep<T0, T1>
        where T0 : IComponent
        where T1 : IComponent
    {
        /// <inheritdoc />
        public override bool TryFollow(ref Entity entity)
        {
            // Try to get entity info for this entity
            var dummy = default(EntityInfo);
            ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

            // Can't follow a path through a dead entity!
            if (isDead)
                return false;

            // Check if all components are present in one go
            var components = entityInfo.Chunk.Archetype.Components;
            if (!components.IsSupersetOfSortedSpan(SortedComponentIDs.Span))
                return false;

            return true;
        }
    }

    /// <summary>
    /// Try to follow a component, using a mapper to extract an entity from it
    /// </summary>
    /// <typeparam name="M"></typeparam>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    
    public sealed class Follow<M, T0, T1>
        : BaseGenericStep<T0, T1>
        where M : IQueryMap<Entity, T0, T1>
        where T0 : IComponent
        where T1 : IComponent
    {
        private readonly M _map;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        public Follow(M map)
        {
            _map = map;
        }

        /// <inheritdoc />
        public override bool TryFollow(ref Entity entity)
        {
            // Try to get entity info for this entity
            var dummy = default(EntityInfo);
            ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

            // Can't follow a path through a dead entity!
            if (isDead)
                return false;

            // Check if all components are present in one go
            var components = entityInfo.Chunk.Archetype.Components;
            if (!components.IsSupersetOfSortedSpan(SortedComponentIDs.Span))
                return false;

            // Get component references
            var row = entityInfo.GetRow(entity.ID);
            ref var t0 = ref row.GetMutable<T0>(C0);
            ref var t1 = ref row.GetMutable<T1>(C1);

            // Execute predicate
            entity = _map.Execute(entity, ref t0, ref t1);
            return true;
        }
    }

    /// <summary>
    /// Base class for path steps that take generic parameters
    /// </summary>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    [ExcludeFromCodeCoverage]
    public abstract class BaseGenericStep<T0, T1, T2>
        : IStep
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
    {
#region Component IDs
        /// <summary>Component ID for T0</summary>
        protected static readonly ComponentID C0 = ComponentID<T0>.ID;
        /// <summary>Component ID for T1</summary>
        protected static readonly ComponentID C1 = ComponentID<T1>.ID;
        /// <summary>Component ID for T2</summary>
        protected static readonly ComponentID C2 = ComponentID<T2>.ID;

        /// <summary>All components IDs, in order</summary>
        protected static readonly ReadOnlyMemory<ComponentID> SortedComponentIDs;

        static BaseGenericStep()
        {
            var arr = new[] {
                C0,
                C1,
                C2,
            };
            Array.Sort(arr);

            SortedComponentIDs = arr;
        }
        #endregion

        /// <inheritdoc />
        public abstract bool TryFollow(ref Entity entity);
    }

    /// <summary>
    /// Construct a path step that checks if a predicate is true
    /// </summary>
    /// <typeparam name="P">Predicate</typeparam>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    [ExcludeFromCodeCoverage]
    public sealed class Predicate<P, T0, T1, T2>
        : BaseGenericStep<T0, T1, T2>
        where P : IQueryMap<bool, T0, T1, T2>
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
    {
        private readonly P _predicate;

        /// <summary>
        /// Create a new <see cref="Predicate{P,T0}"/>
        /// </summary>
        /// <param name="predicate"></param>
        public Predicate(P predicate)
        {
            _predicate = predicate;
        }

        /// <inheritdoc />
        public override bool TryFollow(ref Entity entity)
        {
            // Try to get entity info for this entity
            var dummy = default(EntityInfo);
            ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

            // Can't follow a path through a dead entity!
            if (isDead)
                return false;

            // Check if all components are present in one go
            var components = entityInfo.Chunk.Archetype.Components;
            if (!components.IsSupersetOfSortedSpan(SortedComponentIDs.Span))
                return false;

            // Get component references
            var row = entityInfo.GetRow(entity.ID);
            ref var t0 = ref row.GetMutable<T0>(C0);
            ref var t1 = ref row.GetMutable<T1>(C1);
            ref var t2 = ref row.GetMutable<T2>(C2);

            // Execute predicate
            return _predicate.Execute(entity, ref t0, ref t1, ref t2);
        }
    }

    /// <summary>
    /// Check that the current entity has the given components, fail to follow the path if any are missing
    /// </summary>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    [ExcludeFromCodeCoverage]
    public sealed class HasComponents<T0, T1, T2>
        : BaseGenericStep<T0, T1, T2>
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
    {
        /// <inheritdoc />
        public override bool TryFollow(ref Entity entity)
        {
            // Try to get entity info for this entity
            var dummy = default(EntityInfo);
            ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

            // Can't follow a path through a dead entity!
            if (isDead)
                return false;

            // Check if all components are present in one go
            var components = entityInfo.Chunk.Archetype.Components;
            if (!components.IsSupersetOfSortedSpan(SortedComponentIDs.Span))
                return false;

            return true;
        }
    }

    /// <summary>
    /// Try to follow a component, using a mapper to extract an entity from it
    /// </summary>
    /// <typeparam name="M"></typeparam>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    [ExcludeFromCodeCoverage]
    public sealed class Follow<M, T0, T1, T2>
        : BaseGenericStep<T0, T1, T2>
        where M : IQueryMap<Entity, T0, T1, T2>
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
    {
        private readonly M _map;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        public Follow(M map)
        {
            _map = map;
        }

        /// <inheritdoc />
        public override bool TryFollow(ref Entity entity)
        {
            // Try to get entity info for this entity
            var dummy = default(EntityInfo);
            ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

            // Can't follow a path through a dead entity!
            if (isDead)
                return false;

            // Check if all components are present in one go
            var components = entityInfo.Chunk.Archetype.Components;
            if (!components.IsSupersetOfSortedSpan(SortedComponentIDs.Span))
                return false;

            // Get component references
            var row = entityInfo.GetRow(entity.ID);
            ref var t0 = ref row.GetMutable<T0>(C0);
            ref var t1 = ref row.GetMutable<T1>(C1);
            ref var t2 = ref row.GetMutable<T2>(C2);

            // Execute predicate
            entity = _map.Execute(entity, ref t0, ref t1, ref t2);
            return true;
        }
    }

    /// <summary>
    /// Base class for path steps that take generic parameters
    /// </summary>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    [ExcludeFromCodeCoverage]
    public abstract class BaseGenericStep<T0, T1, T2, T3>
        : IStep
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
    {
#region Component IDs
        /// <summary>Component ID for T0</summary>
        protected static readonly ComponentID C0 = ComponentID<T0>.ID;
        /// <summary>Component ID for T1</summary>
        protected static readonly ComponentID C1 = ComponentID<T1>.ID;
        /// <summary>Component ID for T2</summary>
        protected static readonly ComponentID C2 = ComponentID<T2>.ID;
        /// <summary>Component ID for T3</summary>
        protected static readonly ComponentID C3 = ComponentID<T3>.ID;

        /// <summary>All components IDs, in order</summary>
        protected static readonly ReadOnlyMemory<ComponentID> SortedComponentIDs;

        static BaseGenericStep()
        {
            var arr = new[] {
                C0,
                C1,
                C2,
                C3,
            };
            Array.Sort(arr);

            SortedComponentIDs = arr;
        }
        #endregion

        /// <inheritdoc />
        public abstract bool TryFollow(ref Entity entity);
    }

    /// <summary>
    /// Construct a path step that checks if a predicate is true
    /// </summary>
    /// <typeparam name="P">Predicate</typeparam>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    [ExcludeFromCodeCoverage]
    public sealed class Predicate<P, T0, T1, T2, T3>
        : BaseGenericStep<T0, T1, T2, T3>
        where P : IQueryMap<bool, T0, T1, T2, T3>
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
    {
        private readonly P _predicate;

        /// <summary>
        /// Create a new <see cref="Predicate{P,T0}"/>
        /// </summary>
        /// <param name="predicate"></param>
        public Predicate(P predicate)
        {
            _predicate = predicate;
        }

        /// <inheritdoc />
        public override bool TryFollow(ref Entity entity)
        {
            // Try to get entity info for this entity
            var dummy = default(EntityInfo);
            ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

            // Can't follow a path through a dead entity!
            if (isDead)
                return false;

            // Check if all components are present in one go
            var components = entityInfo.Chunk.Archetype.Components;
            if (!components.IsSupersetOfSortedSpan(SortedComponentIDs.Span))
                return false;

            // Get component references
            var row = entityInfo.GetRow(entity.ID);
            ref var t0 = ref row.GetMutable<T0>(C0);
            ref var t1 = ref row.GetMutable<T1>(C1);
            ref var t2 = ref row.GetMutable<T2>(C2);
            ref var t3 = ref row.GetMutable<T3>(C3);

            // Execute predicate
            return _predicate.Execute(entity, ref t0, ref t1, ref t2, ref t3);
        }
    }

    /// <summary>
    /// Check that the current entity has the given components, fail to follow the path if any are missing
    /// </summary>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    [ExcludeFromCodeCoverage]
    public sealed class HasComponents<T0, T1, T2, T3>
        : BaseGenericStep<T0, T1, T2, T3>
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
    {
        /// <inheritdoc />
        public override bool TryFollow(ref Entity entity)
        {
            // Try to get entity info for this entity
            var dummy = default(EntityInfo);
            ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

            // Can't follow a path through a dead entity!
            if (isDead)
                return false;

            // Check if all components are present in one go
            var components = entityInfo.Chunk.Archetype.Components;
            if (!components.IsSupersetOfSortedSpan(SortedComponentIDs.Span))
                return false;

            return true;
        }
    }

    /// <summary>
    /// Try to follow a component, using a mapper to extract an entity from it
    /// </summary>
    /// <typeparam name="M"></typeparam>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    [ExcludeFromCodeCoverage]
    public sealed class Follow<M, T0, T1, T2, T3>
        : BaseGenericStep<T0, T1, T2, T3>
        where M : IQueryMap<Entity, T0, T1, T2, T3>
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
    {
        private readonly M _map;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        public Follow(M map)
        {
            _map = map;
        }

        /// <inheritdoc />
        public override bool TryFollow(ref Entity entity)
        {
            // Try to get entity info for this entity
            var dummy = default(EntityInfo);
            ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

            // Can't follow a path through a dead entity!
            if (isDead)
                return false;

            // Check if all components are present in one go
            var components = entityInfo.Chunk.Archetype.Components;
            if (!components.IsSupersetOfSortedSpan(SortedComponentIDs.Span))
                return false;

            // Get component references
            var row = entityInfo.GetRow(entity.ID);
            ref var t0 = ref row.GetMutable<T0>(C0);
            ref var t1 = ref row.GetMutable<T1>(C1);
            ref var t2 = ref row.GetMutable<T2>(C2);
            ref var t3 = ref row.GetMutable<T3>(C3);

            // Execute predicate
            entity = _map.Execute(entity, ref t0, ref t1, ref t2, ref t3);
            return true;
        }
    }

    /// <summary>
    /// Base class for path steps that take generic parameters
    /// </summary>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    [ExcludeFromCodeCoverage]
    public abstract class BaseGenericStep<T0, T1, T2, T3, T4>
        : IStep
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
    {
#region Component IDs
        /// <summary>Component ID for T0</summary>
        protected static readonly ComponentID C0 = ComponentID<T0>.ID;
        /// <summary>Component ID for T1</summary>
        protected static readonly ComponentID C1 = ComponentID<T1>.ID;
        /// <summary>Component ID for T2</summary>
        protected static readonly ComponentID C2 = ComponentID<T2>.ID;
        /// <summary>Component ID for T3</summary>
        protected static readonly ComponentID C3 = ComponentID<T3>.ID;
        /// <summary>Component ID for T4</summary>
        protected static readonly ComponentID C4 = ComponentID<T4>.ID;

        /// <summary>All components IDs, in order</summary>
        protected static readonly ReadOnlyMemory<ComponentID> SortedComponentIDs;

        static BaseGenericStep()
        {
            var arr = new[] {
                C0,
                C1,
                C2,
                C3,
                C4,
            };
            Array.Sort(arr);

            SortedComponentIDs = arr;
        }
        #endregion

        /// <inheritdoc />
        public abstract bool TryFollow(ref Entity entity);
    }

    /// <summary>
    /// Construct a path step that checks if a predicate is true
    /// </summary>
    /// <typeparam name="P">Predicate</typeparam>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    [ExcludeFromCodeCoverage]
    public sealed class Predicate<P, T0, T1, T2, T3, T4>
        : BaseGenericStep<T0, T1, T2, T3, T4>
        where P : IQueryMap<bool, T0, T1, T2, T3, T4>
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
    {
        private readonly P _predicate;

        /// <summary>
        /// Create a new <see cref="Predicate{P,T0}"/>
        /// </summary>
        /// <param name="predicate"></param>
        public Predicate(P predicate)
        {
            _predicate = predicate;
        }

        /// <inheritdoc />
        public override bool TryFollow(ref Entity entity)
        {
            // Try to get entity info for this entity
            var dummy = default(EntityInfo);
            ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

            // Can't follow a path through a dead entity!
            if (isDead)
                return false;

            // Check if all components are present in one go
            var components = entityInfo.Chunk.Archetype.Components;
            if (!components.IsSupersetOfSortedSpan(SortedComponentIDs.Span))
                return false;

            // Get component references
            var row = entityInfo.GetRow(entity.ID);
            ref var t0 = ref row.GetMutable<T0>(C0);
            ref var t1 = ref row.GetMutable<T1>(C1);
            ref var t2 = ref row.GetMutable<T2>(C2);
            ref var t3 = ref row.GetMutable<T3>(C3);
            ref var t4 = ref row.GetMutable<T4>(C4);

            // Execute predicate
            return _predicate.Execute(entity, ref t0, ref t1, ref t2, ref t3, ref t4);
        }
    }

    /// <summary>
    /// Check that the current entity has the given components, fail to follow the path if any are missing
    /// </summary>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    [ExcludeFromCodeCoverage]
    public sealed class HasComponents<T0, T1, T2, T3, T4>
        : BaseGenericStep<T0, T1, T2, T3, T4>
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
    {
        /// <inheritdoc />
        public override bool TryFollow(ref Entity entity)
        {
            // Try to get entity info for this entity
            var dummy = default(EntityInfo);
            ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

            // Can't follow a path through a dead entity!
            if (isDead)
                return false;

            // Check if all components are present in one go
            var components = entityInfo.Chunk.Archetype.Components;
            if (!components.IsSupersetOfSortedSpan(SortedComponentIDs.Span))
                return false;

            return true;
        }
    }

    /// <summary>
    /// Try to follow a component, using a mapper to extract an entity from it
    /// </summary>
    /// <typeparam name="M"></typeparam>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    [ExcludeFromCodeCoverage]
    public sealed class Follow<M, T0, T1, T2, T3, T4>
        : BaseGenericStep<T0, T1, T2, T3, T4>
        where M : IQueryMap<Entity, T0, T1, T2, T3, T4>
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
    {
        private readonly M _map;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        public Follow(M map)
        {
            _map = map;
        }

        /// <inheritdoc />
        public override bool TryFollow(ref Entity entity)
        {
            // Try to get entity info for this entity
            var dummy = default(EntityInfo);
            ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

            // Can't follow a path through a dead entity!
            if (isDead)
                return false;

            // Check if all components are present in one go
            var components = entityInfo.Chunk.Archetype.Components;
            if (!components.IsSupersetOfSortedSpan(SortedComponentIDs.Span))
                return false;

            // Get component references
            var row = entityInfo.GetRow(entity.ID);
            ref var t0 = ref row.GetMutable<T0>(C0);
            ref var t1 = ref row.GetMutable<T1>(C1);
            ref var t2 = ref row.GetMutable<T2>(C2);
            ref var t3 = ref row.GetMutable<T3>(C3);
            ref var t4 = ref row.GetMutable<T4>(C4);

            // Execute predicate
            entity = _map.Execute(entity, ref t0, ref t1, ref t2, ref t3, ref t4);
            return true;
        }
    }

    /// <summary>
    /// Base class for path steps that take generic parameters
    /// </summary>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    [ExcludeFromCodeCoverage]
    public abstract class BaseGenericStep<T0, T1, T2, T3, T4, T5>
        : IStep
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
    {
#region Component IDs
        /// <summary>Component ID for T0</summary>
        protected static readonly ComponentID C0 = ComponentID<T0>.ID;
        /// <summary>Component ID for T1</summary>
        protected static readonly ComponentID C1 = ComponentID<T1>.ID;
        /// <summary>Component ID for T2</summary>
        protected static readonly ComponentID C2 = ComponentID<T2>.ID;
        /// <summary>Component ID for T3</summary>
        protected static readonly ComponentID C3 = ComponentID<T3>.ID;
        /// <summary>Component ID for T4</summary>
        protected static readonly ComponentID C4 = ComponentID<T4>.ID;
        /// <summary>Component ID for T5</summary>
        protected static readonly ComponentID C5 = ComponentID<T5>.ID;

        /// <summary>All components IDs, in order</summary>
        protected static readonly ReadOnlyMemory<ComponentID> SortedComponentIDs;

        static BaseGenericStep()
        {
            var arr = new[] {
                C0,
                C1,
                C2,
                C3,
                C4,
                C5,
            };
            Array.Sort(arr);

            SortedComponentIDs = arr;
        }
        #endregion

        /// <inheritdoc />
        public abstract bool TryFollow(ref Entity entity);
    }

    /// <summary>
    /// Construct a path step that checks if a predicate is true
    /// </summary>
    /// <typeparam name="P">Predicate</typeparam>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    [ExcludeFromCodeCoverage]
    public sealed class Predicate<P, T0, T1, T2, T3, T4, T5>
        : BaseGenericStep<T0, T1, T2, T3, T4, T5>
        where P : IQueryMap<bool, T0, T1, T2, T3, T4, T5>
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
    {
        private readonly P _predicate;

        /// <summary>
        /// Create a new <see cref="Predicate{P,T0}"/>
        /// </summary>
        /// <param name="predicate"></param>
        public Predicate(P predicate)
        {
            _predicate = predicate;
        }

        /// <inheritdoc />
        public override bool TryFollow(ref Entity entity)
        {
            // Try to get entity info for this entity
            var dummy = default(EntityInfo);
            ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

            // Can't follow a path through a dead entity!
            if (isDead)
                return false;

            // Check if all components are present in one go
            var components = entityInfo.Chunk.Archetype.Components;
            if (!components.IsSupersetOfSortedSpan(SortedComponentIDs.Span))
                return false;

            // Get component references
            var row = entityInfo.GetRow(entity.ID);
            ref var t0 = ref row.GetMutable<T0>(C0);
            ref var t1 = ref row.GetMutable<T1>(C1);
            ref var t2 = ref row.GetMutable<T2>(C2);
            ref var t3 = ref row.GetMutable<T3>(C3);
            ref var t4 = ref row.GetMutable<T4>(C4);
            ref var t5 = ref row.GetMutable<T5>(C5);

            // Execute predicate
            return _predicate.Execute(entity, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5);
        }
    }

    /// <summary>
    /// Check that the current entity has the given components, fail to follow the path if any are missing
    /// </summary>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    [ExcludeFromCodeCoverage]
    public sealed class HasComponents<T0, T1, T2, T3, T4, T5>
        : BaseGenericStep<T0, T1, T2, T3, T4, T5>
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
    {
        /// <inheritdoc />
        public override bool TryFollow(ref Entity entity)
        {
            // Try to get entity info for this entity
            var dummy = default(EntityInfo);
            ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

            // Can't follow a path through a dead entity!
            if (isDead)
                return false;

            // Check if all components are present in one go
            var components = entityInfo.Chunk.Archetype.Components;
            if (!components.IsSupersetOfSortedSpan(SortedComponentIDs.Span))
                return false;

            return true;
        }
    }

    /// <summary>
    /// Try to follow a component, using a mapper to extract an entity from it
    /// </summary>
    /// <typeparam name="M"></typeparam>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    [ExcludeFromCodeCoverage]
    public sealed class Follow<M, T0, T1, T2, T3, T4, T5>
        : BaseGenericStep<T0, T1, T2, T3, T4, T5>
        where M : IQueryMap<Entity, T0, T1, T2, T3, T4, T5>
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
    {
        private readonly M _map;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        public Follow(M map)
        {
            _map = map;
        }

        /// <inheritdoc />
        public override bool TryFollow(ref Entity entity)
        {
            // Try to get entity info for this entity
            var dummy = default(EntityInfo);
            ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

            // Can't follow a path through a dead entity!
            if (isDead)
                return false;

            // Check if all components are present in one go
            var components = entityInfo.Chunk.Archetype.Components;
            if (!components.IsSupersetOfSortedSpan(SortedComponentIDs.Span))
                return false;

            // Get component references
            var row = entityInfo.GetRow(entity.ID);
            ref var t0 = ref row.GetMutable<T0>(C0);
            ref var t1 = ref row.GetMutable<T1>(C1);
            ref var t2 = ref row.GetMutable<T2>(C2);
            ref var t3 = ref row.GetMutable<T3>(C3);
            ref var t4 = ref row.GetMutable<T4>(C4);
            ref var t5 = ref row.GetMutable<T5>(C5);

            // Execute predicate
            entity = _map.Execute(entity, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5);
            return true;
        }
    }

    /// <summary>
    /// Base class for path steps that take generic parameters
    /// </summary>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    [ExcludeFromCodeCoverage]
    public abstract class BaseGenericStep<T0, T1, T2, T3, T4, T5, T6>
        : IStep
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
    {
#region Component IDs
        /// <summary>Component ID for T0</summary>
        protected static readonly ComponentID C0 = ComponentID<T0>.ID;
        /// <summary>Component ID for T1</summary>
        protected static readonly ComponentID C1 = ComponentID<T1>.ID;
        /// <summary>Component ID for T2</summary>
        protected static readonly ComponentID C2 = ComponentID<T2>.ID;
        /// <summary>Component ID for T3</summary>
        protected static readonly ComponentID C3 = ComponentID<T3>.ID;
        /// <summary>Component ID for T4</summary>
        protected static readonly ComponentID C4 = ComponentID<T4>.ID;
        /// <summary>Component ID for T5</summary>
        protected static readonly ComponentID C5 = ComponentID<T5>.ID;
        /// <summary>Component ID for T6</summary>
        protected static readonly ComponentID C6 = ComponentID<T6>.ID;

        /// <summary>All components IDs, in order</summary>
        protected static readonly ReadOnlyMemory<ComponentID> SortedComponentIDs;

        static BaseGenericStep()
        {
            var arr = new[] {
                C0,
                C1,
                C2,
                C3,
                C4,
                C5,
                C6,
            };
            Array.Sort(arr);

            SortedComponentIDs = arr;
        }
        #endregion

        /// <inheritdoc />
        public abstract bool TryFollow(ref Entity entity);
    }

    /// <summary>
    /// Construct a path step that checks if a predicate is true
    /// </summary>
    /// <typeparam name="P">Predicate</typeparam>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    [ExcludeFromCodeCoverage]
    public sealed class Predicate<P, T0, T1, T2, T3, T4, T5, T6>
        : BaseGenericStep<T0, T1, T2, T3, T4, T5, T6>
        where P : IQueryMap<bool, T0, T1, T2, T3, T4, T5, T6>
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
    {
        private readonly P _predicate;

        /// <summary>
        /// Create a new <see cref="Predicate{P,T0}"/>
        /// </summary>
        /// <param name="predicate"></param>
        public Predicate(P predicate)
        {
            _predicate = predicate;
        }

        /// <inheritdoc />
        public override bool TryFollow(ref Entity entity)
        {
            // Try to get entity info for this entity
            var dummy = default(EntityInfo);
            ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

            // Can't follow a path through a dead entity!
            if (isDead)
                return false;

            // Check if all components are present in one go
            var components = entityInfo.Chunk.Archetype.Components;
            if (!components.IsSupersetOfSortedSpan(SortedComponentIDs.Span))
                return false;

            // Get component references
            var row = entityInfo.GetRow(entity.ID);
            ref var t0 = ref row.GetMutable<T0>(C0);
            ref var t1 = ref row.GetMutable<T1>(C1);
            ref var t2 = ref row.GetMutable<T2>(C2);
            ref var t3 = ref row.GetMutable<T3>(C3);
            ref var t4 = ref row.GetMutable<T4>(C4);
            ref var t5 = ref row.GetMutable<T5>(C5);
            ref var t6 = ref row.GetMutable<T6>(C6);

            // Execute predicate
            return _predicate.Execute(entity, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6);
        }
    }

    /// <summary>
    /// Check that the current entity has the given components, fail to follow the path if any are missing
    /// </summary>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    [ExcludeFromCodeCoverage]
    public sealed class HasComponents<T0, T1, T2, T3, T4, T5, T6>
        : BaseGenericStep<T0, T1, T2, T3, T4, T5, T6>
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
    {
        /// <inheritdoc />
        public override bool TryFollow(ref Entity entity)
        {
            // Try to get entity info for this entity
            var dummy = default(EntityInfo);
            ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

            // Can't follow a path through a dead entity!
            if (isDead)
                return false;

            // Check if all components are present in one go
            var components = entityInfo.Chunk.Archetype.Components;
            if (!components.IsSupersetOfSortedSpan(SortedComponentIDs.Span))
                return false;

            return true;
        }
    }

    /// <summary>
    /// Try to follow a component, using a mapper to extract an entity from it
    /// </summary>
    /// <typeparam name="M"></typeparam>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    [ExcludeFromCodeCoverage]
    public sealed class Follow<M, T0, T1, T2, T3, T4, T5, T6>
        : BaseGenericStep<T0, T1, T2, T3, T4, T5, T6>
        where M : IQueryMap<Entity, T0, T1, T2, T3, T4, T5, T6>
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
    {
        private readonly M _map;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        public Follow(M map)
        {
            _map = map;
        }

        /// <inheritdoc />
        public override bool TryFollow(ref Entity entity)
        {
            // Try to get entity info for this entity
            var dummy = default(EntityInfo);
            ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

            // Can't follow a path through a dead entity!
            if (isDead)
                return false;

            // Check if all components are present in one go
            var components = entityInfo.Chunk.Archetype.Components;
            if (!components.IsSupersetOfSortedSpan(SortedComponentIDs.Span))
                return false;

            // Get component references
            var row = entityInfo.GetRow(entity.ID);
            ref var t0 = ref row.GetMutable<T0>(C0);
            ref var t1 = ref row.GetMutable<T1>(C1);
            ref var t2 = ref row.GetMutable<T2>(C2);
            ref var t3 = ref row.GetMutable<T3>(C3);
            ref var t4 = ref row.GetMutable<T4>(C4);
            ref var t5 = ref row.GetMutable<T5>(C5);
            ref var t6 = ref row.GetMutable<T6>(C6);

            // Execute predicate
            entity = _map.Execute(entity, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6);
            return true;
        }
    }

    /// <summary>
    /// Base class for path steps that take generic parameters
    /// </summary>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    [ExcludeFromCodeCoverage]
    public abstract class BaseGenericStep<T0, T1, T2, T3, T4, T5, T6, T7>
        : IStep
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
    {
#region Component IDs
        /// <summary>Component ID for T0</summary>
        protected static readonly ComponentID C0 = ComponentID<T0>.ID;
        /// <summary>Component ID for T1</summary>
        protected static readonly ComponentID C1 = ComponentID<T1>.ID;
        /// <summary>Component ID for T2</summary>
        protected static readonly ComponentID C2 = ComponentID<T2>.ID;
        /// <summary>Component ID for T3</summary>
        protected static readonly ComponentID C3 = ComponentID<T3>.ID;
        /// <summary>Component ID for T4</summary>
        protected static readonly ComponentID C4 = ComponentID<T4>.ID;
        /// <summary>Component ID for T5</summary>
        protected static readonly ComponentID C5 = ComponentID<T5>.ID;
        /// <summary>Component ID for T6</summary>
        protected static readonly ComponentID C6 = ComponentID<T6>.ID;
        /// <summary>Component ID for T7</summary>
        protected static readonly ComponentID C7 = ComponentID<T7>.ID;

        /// <summary>All components IDs, in order</summary>
        protected static readonly ReadOnlyMemory<ComponentID> SortedComponentIDs;

        static BaseGenericStep()
        {
            var arr = new[] {
                C0,
                C1,
                C2,
                C3,
                C4,
                C5,
                C6,
                C7,
            };
            Array.Sort(arr);

            SortedComponentIDs = arr;
        }
        #endregion

        /// <inheritdoc />
        public abstract bool TryFollow(ref Entity entity);
    }

    /// <summary>
    /// Construct a path step that checks if a predicate is true
    /// </summary>
    /// <typeparam name="P">Predicate</typeparam>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    [ExcludeFromCodeCoverage]
    public sealed class Predicate<P, T0, T1, T2, T3, T4, T5, T6, T7>
        : BaseGenericStep<T0, T1, T2, T3, T4, T5, T6, T7>
        where P : IQueryMap<bool, T0, T1, T2, T3, T4, T5, T6, T7>
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
    {
        private readonly P _predicate;

        /// <summary>
        /// Create a new <see cref="Predicate{P,T0}"/>
        /// </summary>
        /// <param name="predicate"></param>
        public Predicate(P predicate)
        {
            _predicate = predicate;
        }

        /// <inheritdoc />
        public override bool TryFollow(ref Entity entity)
        {
            // Try to get entity info for this entity
            var dummy = default(EntityInfo);
            ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

            // Can't follow a path through a dead entity!
            if (isDead)
                return false;

            // Check if all components are present in one go
            var components = entityInfo.Chunk.Archetype.Components;
            if (!components.IsSupersetOfSortedSpan(SortedComponentIDs.Span))
                return false;

            // Get component references
            var row = entityInfo.GetRow(entity.ID);
            ref var t0 = ref row.GetMutable<T0>(C0);
            ref var t1 = ref row.GetMutable<T1>(C1);
            ref var t2 = ref row.GetMutable<T2>(C2);
            ref var t3 = ref row.GetMutable<T3>(C3);
            ref var t4 = ref row.GetMutable<T4>(C4);
            ref var t5 = ref row.GetMutable<T5>(C5);
            ref var t6 = ref row.GetMutable<T6>(C6);
            ref var t7 = ref row.GetMutable<T7>(C7);

            // Execute predicate
            return _predicate.Execute(entity, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7);
        }
    }

    /// <summary>
    /// Check that the current entity has the given components, fail to follow the path if any are missing
    /// </summary>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    [ExcludeFromCodeCoverage]
    public sealed class HasComponents<T0, T1, T2, T3, T4, T5, T6, T7>
        : BaseGenericStep<T0, T1, T2, T3, T4, T5, T6, T7>
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
    {
        /// <inheritdoc />
        public override bool TryFollow(ref Entity entity)
        {
            // Try to get entity info for this entity
            var dummy = default(EntityInfo);
            ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

            // Can't follow a path through a dead entity!
            if (isDead)
                return false;

            // Check if all components are present in one go
            var components = entityInfo.Chunk.Archetype.Components;
            if (!components.IsSupersetOfSortedSpan(SortedComponentIDs.Span))
                return false;

            return true;
        }
    }

    /// <summary>
    /// Try to follow a component, using a mapper to extract an entity from it
    /// </summary>
    /// <typeparam name="M"></typeparam>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    [ExcludeFromCodeCoverage]
    public sealed class Follow<M, T0, T1, T2, T3, T4, T5, T6, T7>
        : BaseGenericStep<T0, T1, T2, T3, T4, T5, T6, T7>
        where M : IQueryMap<Entity, T0, T1, T2, T3, T4, T5, T6, T7>
        where T0 : IComponent
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
    {
        private readonly M _map;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        public Follow(M map)
        {
            _map = map;
        }

        /// <inheritdoc />
        public override bool TryFollow(ref Entity entity)
        {
            // Try to get entity info for this entity
            var dummy = default(EntityInfo);
            ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

            // Can't follow a path through a dead entity!
            if (isDead)
                return false;

            // Check if all components are present in one go
            var components = entityInfo.Chunk.Archetype.Components;
            if (!components.IsSupersetOfSortedSpan(SortedComponentIDs.Span))
                return false;

            // Get component references
            var row = entityInfo.GetRow(entity.ID);
            ref var t0 = ref row.GetMutable<T0>(C0);
            ref var t1 = ref row.GetMutable<T1>(C1);
            ref var t2 = ref row.GetMutable<T2>(C2);
            ref var t3 = ref row.GetMutable<T3>(C3);
            ref var t4 = ref row.GetMutable<T4>(C4);
            ref var t5 = ref row.GetMutable<T5>(C5);
            ref var t6 = ref row.GetMutable<T6>(C6);
            ref var t7 = ref row.GetMutable<T7>(C7);

            // Execute predicate
            entity = _map.Execute(entity, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7);
            return true;
        }
    }

    /// <summary>
    /// Base class for path steps that take generic parameters
    /// </summary>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    [ExcludeFromCodeCoverage]
    public abstract class BaseGenericStep<T0, T1, T2, T3, T4, T5, T6, T7, T8>
        : IStep
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
#region Component IDs
        /// <summary>Component ID for T0</summary>
        protected static readonly ComponentID C0 = ComponentID<T0>.ID;
        /// <summary>Component ID for T1</summary>
        protected static readonly ComponentID C1 = ComponentID<T1>.ID;
        /// <summary>Component ID for T2</summary>
        protected static readonly ComponentID C2 = ComponentID<T2>.ID;
        /// <summary>Component ID for T3</summary>
        protected static readonly ComponentID C3 = ComponentID<T3>.ID;
        /// <summary>Component ID for T4</summary>
        protected static readonly ComponentID C4 = ComponentID<T4>.ID;
        /// <summary>Component ID for T5</summary>
        protected static readonly ComponentID C5 = ComponentID<T5>.ID;
        /// <summary>Component ID for T6</summary>
        protected static readonly ComponentID C6 = ComponentID<T6>.ID;
        /// <summary>Component ID for T7</summary>
        protected static readonly ComponentID C7 = ComponentID<T7>.ID;
        /// <summary>Component ID for T8</summary>
        protected static readonly ComponentID C8 = ComponentID<T8>.ID;

        /// <summary>All components IDs, in order</summary>
        protected static readonly ReadOnlyMemory<ComponentID> SortedComponentIDs;

        static BaseGenericStep()
        {
            var arr = new[] {
                C0,
                C1,
                C2,
                C3,
                C4,
                C5,
                C6,
                C7,
                C8,
            };
            Array.Sort(arr);

            SortedComponentIDs = arr;
        }
        #endregion

        /// <inheritdoc />
        public abstract bool TryFollow(ref Entity entity);
    }

    /// <summary>
    /// Construct a path step that checks if a predicate is true
    /// </summary>
    /// <typeparam name="P">Predicate</typeparam>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    [ExcludeFromCodeCoverage]
    public sealed class Predicate<P, T0, T1, T2, T3, T4, T5, T6, T7, T8>
        : BaseGenericStep<T0, T1, T2, T3, T4, T5, T6, T7, T8>
        where P : IQueryMap<bool, T0, T1, T2, T3, T4, T5, T6, T7, T8>
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
        private readonly P _predicate;

        /// <summary>
        /// Create a new <see cref="Predicate{P,T0}"/>
        /// </summary>
        /// <param name="predicate"></param>
        public Predicate(P predicate)
        {
            _predicate = predicate;
        }

        /// <inheritdoc />
        public override bool TryFollow(ref Entity entity)
        {
            // Try to get entity info for this entity
            var dummy = default(EntityInfo);
            ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

            // Can't follow a path through a dead entity!
            if (isDead)
                return false;

            // Check if all components are present in one go
            var components = entityInfo.Chunk.Archetype.Components;
            if (!components.IsSupersetOfSortedSpan(SortedComponentIDs.Span))
                return false;

            // Get component references
            var row = entityInfo.GetRow(entity.ID);
            ref var t0 = ref row.GetMutable<T0>(C0);
            ref var t1 = ref row.GetMutable<T1>(C1);
            ref var t2 = ref row.GetMutable<T2>(C2);
            ref var t3 = ref row.GetMutable<T3>(C3);
            ref var t4 = ref row.GetMutable<T4>(C4);
            ref var t5 = ref row.GetMutable<T5>(C5);
            ref var t6 = ref row.GetMutable<T6>(C6);
            ref var t7 = ref row.GetMutable<T7>(C7);
            ref var t8 = ref row.GetMutable<T8>(C8);

            // Execute predicate
            return _predicate.Execute(entity, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7, ref t8);
        }
    }

    /// <summary>
    /// Check that the current entity has the given components, fail to follow the path if any are missing
    /// </summary>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    [ExcludeFromCodeCoverage]
    public sealed class HasComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8>
        : BaseGenericStep<T0, T1, T2, T3, T4, T5, T6, T7, T8>
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
        /// <inheritdoc />
        public override bool TryFollow(ref Entity entity)
        {
            // Try to get entity info for this entity
            var dummy = default(EntityInfo);
            ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

            // Can't follow a path through a dead entity!
            if (isDead)
                return false;

            // Check if all components are present in one go
            var components = entityInfo.Chunk.Archetype.Components;
            if (!components.IsSupersetOfSortedSpan(SortedComponentIDs.Span))
                return false;

            return true;
        }
    }

    /// <summary>
    /// Try to follow a component, using a mapper to extract an entity from it
    /// </summary>
    /// <typeparam name="M"></typeparam>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    [ExcludeFromCodeCoverage]
    public sealed class Follow<M, T0, T1, T2, T3, T4, T5, T6, T7, T8>
        : BaseGenericStep<T0, T1, T2, T3, T4, T5, T6, T7, T8>
        where M : IQueryMap<Entity, T0, T1, T2, T3, T4, T5, T6, T7, T8>
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
        private readonly M _map;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        public Follow(M map)
        {
            _map = map;
        }

        /// <inheritdoc />
        public override bool TryFollow(ref Entity entity)
        {
            // Try to get entity info for this entity
            var dummy = default(EntityInfo);
            ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

            // Can't follow a path through a dead entity!
            if (isDead)
                return false;

            // Check if all components are present in one go
            var components = entityInfo.Chunk.Archetype.Components;
            if (!components.IsSupersetOfSortedSpan(SortedComponentIDs.Span))
                return false;

            // Get component references
            var row = entityInfo.GetRow(entity.ID);
            ref var t0 = ref row.GetMutable<T0>(C0);
            ref var t1 = ref row.GetMutable<T1>(C1);
            ref var t2 = ref row.GetMutable<T2>(C2);
            ref var t3 = ref row.GetMutable<T3>(C3);
            ref var t4 = ref row.GetMutable<T4>(C4);
            ref var t5 = ref row.GetMutable<T5>(C5);
            ref var t6 = ref row.GetMutable<T6>(C6);
            ref var t7 = ref row.GetMutable<T7>(C7);
            ref var t8 = ref row.GetMutable<T8>(C8);

            // Execute predicate
            entity = _map.Execute(entity, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7, ref t8);
            return true;
        }
    }

    /// <summary>
    /// Base class for path steps that take generic parameters
    /// </summary>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    /// <typeparam name="T9"></typeparam>
    [ExcludeFromCodeCoverage]
    public abstract class BaseGenericStep<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
        : IStep
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
#region Component IDs
        /// <summary>Component ID for T0</summary>
        protected static readonly ComponentID C0 = ComponentID<T0>.ID;
        /// <summary>Component ID for T1</summary>
        protected static readonly ComponentID C1 = ComponentID<T1>.ID;
        /// <summary>Component ID for T2</summary>
        protected static readonly ComponentID C2 = ComponentID<T2>.ID;
        /// <summary>Component ID for T3</summary>
        protected static readonly ComponentID C3 = ComponentID<T3>.ID;
        /// <summary>Component ID for T4</summary>
        protected static readonly ComponentID C4 = ComponentID<T4>.ID;
        /// <summary>Component ID for T5</summary>
        protected static readonly ComponentID C5 = ComponentID<T5>.ID;
        /// <summary>Component ID for T6</summary>
        protected static readonly ComponentID C6 = ComponentID<T6>.ID;
        /// <summary>Component ID for T7</summary>
        protected static readonly ComponentID C7 = ComponentID<T7>.ID;
        /// <summary>Component ID for T8</summary>
        protected static readonly ComponentID C8 = ComponentID<T8>.ID;
        /// <summary>Component ID for T9</summary>
        protected static readonly ComponentID C9 = ComponentID<T9>.ID;

        /// <summary>All components IDs, in order</summary>
        protected static readonly ReadOnlyMemory<ComponentID> SortedComponentIDs;

        static BaseGenericStep()
        {
            var arr = new[] {
                C0,
                C1,
                C2,
                C3,
                C4,
                C5,
                C6,
                C7,
                C8,
                C9,
            };
            Array.Sort(arr);

            SortedComponentIDs = arr;
        }
        #endregion

        /// <inheritdoc />
        public abstract bool TryFollow(ref Entity entity);
    }

    /// <summary>
    /// Construct a path step that checks if a predicate is true
    /// </summary>
    /// <typeparam name="P">Predicate</typeparam>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    /// <typeparam name="T9"></typeparam>
    [ExcludeFromCodeCoverage]
    public sealed class Predicate<P, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
        : BaseGenericStep<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
        where P : IQueryMap<bool, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
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
        private readonly P _predicate;

        /// <summary>
        /// Create a new <see cref="Predicate{P,T0}"/>
        /// </summary>
        /// <param name="predicate"></param>
        public Predicate(P predicate)
        {
            _predicate = predicate;
        }

        /// <inheritdoc />
        public override bool TryFollow(ref Entity entity)
        {
            // Try to get entity info for this entity
            var dummy = default(EntityInfo);
            ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

            // Can't follow a path through a dead entity!
            if (isDead)
                return false;

            // Check if all components are present in one go
            var components = entityInfo.Chunk.Archetype.Components;
            if (!components.IsSupersetOfSortedSpan(SortedComponentIDs.Span))
                return false;

            // Get component references
            var row = entityInfo.GetRow(entity.ID);
            ref var t0 = ref row.GetMutable<T0>(C0);
            ref var t1 = ref row.GetMutable<T1>(C1);
            ref var t2 = ref row.GetMutable<T2>(C2);
            ref var t3 = ref row.GetMutable<T3>(C3);
            ref var t4 = ref row.GetMutable<T4>(C4);
            ref var t5 = ref row.GetMutable<T5>(C5);
            ref var t6 = ref row.GetMutable<T6>(C6);
            ref var t7 = ref row.GetMutable<T7>(C7);
            ref var t8 = ref row.GetMutable<T8>(C8);
            ref var t9 = ref row.GetMutable<T9>(C9);

            // Execute predicate
            return _predicate.Execute(entity, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7, ref t8, ref t9);
        }
    }

    /// <summary>
    /// Check that the current entity has the given components, fail to follow the path if any are missing
    /// </summary>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    /// <typeparam name="T9"></typeparam>
    [ExcludeFromCodeCoverage]
    public sealed class HasComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
        : BaseGenericStep<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
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
        /// <inheritdoc />
        public override bool TryFollow(ref Entity entity)
        {
            // Try to get entity info for this entity
            var dummy = default(EntityInfo);
            ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

            // Can't follow a path through a dead entity!
            if (isDead)
                return false;

            // Check if all components are present in one go
            var components = entityInfo.Chunk.Archetype.Components;
            if (!components.IsSupersetOfSortedSpan(SortedComponentIDs.Span))
                return false;

            return true;
        }
    }

    /// <summary>
    /// Try to follow a component, using a mapper to extract an entity from it
    /// </summary>
    /// <typeparam name="M"></typeparam>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    /// <typeparam name="T9"></typeparam>
    [ExcludeFromCodeCoverage]
    public sealed class Follow<M, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
        : BaseGenericStep<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
        where M : IQueryMap<Entity, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
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
        private readonly M _map;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        public Follow(M map)
        {
            _map = map;
        }

        /// <inheritdoc />
        public override bool TryFollow(ref Entity entity)
        {
            // Try to get entity info for this entity
            var dummy = default(EntityInfo);
            ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

            // Can't follow a path through a dead entity!
            if (isDead)
                return false;

            // Check if all components are present in one go
            var components = entityInfo.Chunk.Archetype.Components;
            if (!components.IsSupersetOfSortedSpan(SortedComponentIDs.Span))
                return false;

            // Get component references
            var row = entityInfo.GetRow(entity.ID);
            ref var t0 = ref row.GetMutable<T0>(C0);
            ref var t1 = ref row.GetMutable<T1>(C1);
            ref var t2 = ref row.GetMutable<T2>(C2);
            ref var t3 = ref row.GetMutable<T3>(C3);
            ref var t4 = ref row.GetMutable<T4>(C4);
            ref var t5 = ref row.GetMutable<T5>(C5);
            ref var t6 = ref row.GetMutable<T6>(C6);
            ref var t7 = ref row.GetMutable<T7>(C7);
            ref var t8 = ref row.GetMutable<T8>(C8);
            ref var t9 = ref row.GetMutable<T9>(C9);

            // Execute predicate
            entity = _map.Execute(entity, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7, ref t8, ref t9);
            return true;
        }
    }

    /// <summary>
    /// Base class for path steps that take generic parameters
    /// </summary>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    /// <typeparam name="T9"></typeparam>
    /// <typeparam name="T10"></typeparam>
    [ExcludeFromCodeCoverage]
    public abstract class BaseGenericStep<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
        : IStep
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
#region Component IDs
        /// <summary>Component ID for T0</summary>
        protected static readonly ComponentID C0 = ComponentID<T0>.ID;
        /// <summary>Component ID for T1</summary>
        protected static readonly ComponentID C1 = ComponentID<T1>.ID;
        /// <summary>Component ID for T2</summary>
        protected static readonly ComponentID C2 = ComponentID<T2>.ID;
        /// <summary>Component ID for T3</summary>
        protected static readonly ComponentID C3 = ComponentID<T3>.ID;
        /// <summary>Component ID for T4</summary>
        protected static readonly ComponentID C4 = ComponentID<T4>.ID;
        /// <summary>Component ID for T5</summary>
        protected static readonly ComponentID C5 = ComponentID<T5>.ID;
        /// <summary>Component ID for T6</summary>
        protected static readonly ComponentID C6 = ComponentID<T6>.ID;
        /// <summary>Component ID for T7</summary>
        protected static readonly ComponentID C7 = ComponentID<T7>.ID;
        /// <summary>Component ID for T8</summary>
        protected static readonly ComponentID C8 = ComponentID<T8>.ID;
        /// <summary>Component ID for T9</summary>
        protected static readonly ComponentID C9 = ComponentID<T9>.ID;
        /// <summary>Component ID for T10</summary>
        protected static readonly ComponentID C10 = ComponentID<T10>.ID;

        /// <summary>All components IDs, in order</summary>
        protected static readonly ReadOnlyMemory<ComponentID> SortedComponentIDs;

        static BaseGenericStep()
        {
            var arr = new[] {
                C0,
                C1,
                C2,
                C3,
                C4,
                C5,
                C6,
                C7,
                C8,
                C9,
                C10,
            };
            Array.Sort(arr);

            SortedComponentIDs = arr;
        }
        #endregion

        /// <inheritdoc />
        public abstract bool TryFollow(ref Entity entity);
    }

    /// <summary>
    /// Construct a path step that checks if a predicate is true
    /// </summary>
    /// <typeparam name="P">Predicate</typeparam>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    /// <typeparam name="T9"></typeparam>
    /// <typeparam name="T10"></typeparam>
    [ExcludeFromCodeCoverage]
    public sealed class Predicate<P, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
        : BaseGenericStep<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
        where P : IQueryMap<bool, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
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
        private readonly P _predicate;

        /// <summary>
        /// Create a new <see cref="Predicate{P,T0}"/>
        /// </summary>
        /// <param name="predicate"></param>
        public Predicate(P predicate)
        {
            _predicate = predicate;
        }

        /// <inheritdoc />
        public override bool TryFollow(ref Entity entity)
        {
            // Try to get entity info for this entity
            var dummy = default(EntityInfo);
            ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

            // Can't follow a path through a dead entity!
            if (isDead)
                return false;

            // Check if all components are present in one go
            var components = entityInfo.Chunk.Archetype.Components;
            if (!components.IsSupersetOfSortedSpan(SortedComponentIDs.Span))
                return false;

            // Get component references
            var row = entityInfo.GetRow(entity.ID);
            ref var t0 = ref row.GetMutable<T0>(C0);
            ref var t1 = ref row.GetMutable<T1>(C1);
            ref var t2 = ref row.GetMutable<T2>(C2);
            ref var t3 = ref row.GetMutable<T3>(C3);
            ref var t4 = ref row.GetMutable<T4>(C4);
            ref var t5 = ref row.GetMutable<T5>(C5);
            ref var t6 = ref row.GetMutable<T6>(C6);
            ref var t7 = ref row.GetMutable<T7>(C7);
            ref var t8 = ref row.GetMutable<T8>(C8);
            ref var t9 = ref row.GetMutable<T9>(C9);
            ref var t10 = ref row.GetMutable<T10>(C10);

            // Execute predicate
            return _predicate.Execute(entity, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7, ref t8, ref t9, ref t10);
        }
    }

    /// <summary>
    /// Check that the current entity has the given components, fail to follow the path if any are missing
    /// </summary>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    /// <typeparam name="T9"></typeparam>
    /// <typeparam name="T10"></typeparam>
    [ExcludeFromCodeCoverage]
    public sealed class HasComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
        : BaseGenericStep<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
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
        /// <inheritdoc />
        public override bool TryFollow(ref Entity entity)
        {
            // Try to get entity info for this entity
            var dummy = default(EntityInfo);
            ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

            // Can't follow a path through a dead entity!
            if (isDead)
                return false;

            // Check if all components are present in one go
            var components = entityInfo.Chunk.Archetype.Components;
            if (!components.IsSupersetOfSortedSpan(SortedComponentIDs.Span))
                return false;

            return true;
        }
    }

    /// <summary>
    /// Try to follow a component, using a mapper to extract an entity from it
    /// </summary>
    /// <typeparam name="M"></typeparam>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    /// <typeparam name="T9"></typeparam>
    /// <typeparam name="T10"></typeparam>
    [ExcludeFromCodeCoverage]
    public sealed class Follow<M, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
        : BaseGenericStep<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
        where M : IQueryMap<Entity, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
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
        private readonly M _map;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        public Follow(M map)
        {
            _map = map;
        }

        /// <inheritdoc />
        public override bool TryFollow(ref Entity entity)
        {
            // Try to get entity info for this entity
            var dummy = default(EntityInfo);
            ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

            // Can't follow a path through a dead entity!
            if (isDead)
                return false;

            // Check if all components are present in one go
            var components = entityInfo.Chunk.Archetype.Components;
            if (!components.IsSupersetOfSortedSpan(SortedComponentIDs.Span))
                return false;

            // Get component references
            var row = entityInfo.GetRow(entity.ID);
            ref var t0 = ref row.GetMutable<T0>(C0);
            ref var t1 = ref row.GetMutable<T1>(C1);
            ref var t2 = ref row.GetMutable<T2>(C2);
            ref var t3 = ref row.GetMutable<T3>(C3);
            ref var t4 = ref row.GetMutable<T4>(C4);
            ref var t5 = ref row.GetMutable<T5>(C5);
            ref var t6 = ref row.GetMutable<T6>(C6);
            ref var t7 = ref row.GetMutable<T7>(C7);
            ref var t8 = ref row.GetMutable<T8>(C8);
            ref var t9 = ref row.GetMutable<T9>(C9);
            ref var t10 = ref row.GetMutable<T10>(C10);

            // Execute predicate
            entity = _map.Execute(entity, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7, ref t8, ref t9, ref t10);
            return true;
        }
    }

    /// <summary>
    /// Base class for path steps that take generic parameters
    /// </summary>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    /// <typeparam name="T9"></typeparam>
    /// <typeparam name="T10"></typeparam>
    /// <typeparam name="T11"></typeparam>
    [ExcludeFromCodeCoverage]
    public abstract class BaseGenericStep<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
        : IStep
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
#region Component IDs
        /// <summary>Component ID for T0</summary>
        protected static readonly ComponentID C0 = ComponentID<T0>.ID;
        /// <summary>Component ID for T1</summary>
        protected static readonly ComponentID C1 = ComponentID<T1>.ID;
        /// <summary>Component ID for T2</summary>
        protected static readonly ComponentID C2 = ComponentID<T2>.ID;
        /// <summary>Component ID for T3</summary>
        protected static readonly ComponentID C3 = ComponentID<T3>.ID;
        /// <summary>Component ID for T4</summary>
        protected static readonly ComponentID C4 = ComponentID<T4>.ID;
        /// <summary>Component ID for T5</summary>
        protected static readonly ComponentID C5 = ComponentID<T5>.ID;
        /// <summary>Component ID for T6</summary>
        protected static readonly ComponentID C6 = ComponentID<T6>.ID;
        /// <summary>Component ID for T7</summary>
        protected static readonly ComponentID C7 = ComponentID<T7>.ID;
        /// <summary>Component ID for T8</summary>
        protected static readonly ComponentID C8 = ComponentID<T8>.ID;
        /// <summary>Component ID for T9</summary>
        protected static readonly ComponentID C9 = ComponentID<T9>.ID;
        /// <summary>Component ID for T10</summary>
        protected static readonly ComponentID C10 = ComponentID<T10>.ID;
        /// <summary>Component ID for T11</summary>
        protected static readonly ComponentID C11 = ComponentID<T11>.ID;

        /// <summary>All components IDs, in order</summary>
        protected static readonly ReadOnlyMemory<ComponentID> SortedComponentIDs;

        static BaseGenericStep()
        {
            var arr = new[] {
                C0,
                C1,
                C2,
                C3,
                C4,
                C5,
                C6,
                C7,
                C8,
                C9,
                C10,
                C11,
            };
            Array.Sort(arr);

            SortedComponentIDs = arr;
        }
        #endregion

        /// <inheritdoc />
        public abstract bool TryFollow(ref Entity entity);
    }

    /// <summary>
    /// Construct a path step that checks if a predicate is true
    /// </summary>
    /// <typeparam name="P">Predicate</typeparam>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    /// <typeparam name="T9"></typeparam>
    /// <typeparam name="T10"></typeparam>
    /// <typeparam name="T11"></typeparam>
    [ExcludeFromCodeCoverage]
    public sealed class Predicate<P, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
        : BaseGenericStep<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
        where P : IQueryMap<bool, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
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
        private readonly P _predicate;

        /// <summary>
        /// Create a new <see cref="Predicate{P,T0}"/>
        /// </summary>
        /// <param name="predicate"></param>
        public Predicate(P predicate)
        {
            _predicate = predicate;
        }

        /// <inheritdoc />
        public override bool TryFollow(ref Entity entity)
        {
            // Try to get entity info for this entity
            var dummy = default(EntityInfo);
            ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

            // Can't follow a path through a dead entity!
            if (isDead)
                return false;

            // Check if all components are present in one go
            var components = entityInfo.Chunk.Archetype.Components;
            if (!components.IsSupersetOfSortedSpan(SortedComponentIDs.Span))
                return false;

            // Get component references
            var row = entityInfo.GetRow(entity.ID);
            ref var t0 = ref row.GetMutable<T0>(C0);
            ref var t1 = ref row.GetMutable<T1>(C1);
            ref var t2 = ref row.GetMutable<T2>(C2);
            ref var t3 = ref row.GetMutable<T3>(C3);
            ref var t4 = ref row.GetMutable<T4>(C4);
            ref var t5 = ref row.GetMutable<T5>(C5);
            ref var t6 = ref row.GetMutable<T6>(C6);
            ref var t7 = ref row.GetMutable<T7>(C7);
            ref var t8 = ref row.GetMutable<T8>(C8);
            ref var t9 = ref row.GetMutable<T9>(C9);
            ref var t10 = ref row.GetMutable<T10>(C10);
            ref var t11 = ref row.GetMutable<T11>(C11);

            // Execute predicate
            return _predicate.Execute(entity, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7, ref t8, ref t9, ref t10, ref t11);
        }
    }

    /// <summary>
    /// Check that the current entity has the given components, fail to follow the path if any are missing
    /// </summary>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    /// <typeparam name="T9"></typeparam>
    /// <typeparam name="T10"></typeparam>
    /// <typeparam name="T11"></typeparam>
    [ExcludeFromCodeCoverage]
    public sealed class HasComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
        : BaseGenericStep<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
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
        /// <inheritdoc />
        public override bool TryFollow(ref Entity entity)
        {
            // Try to get entity info for this entity
            var dummy = default(EntityInfo);
            ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

            // Can't follow a path through a dead entity!
            if (isDead)
                return false;

            // Check if all components are present in one go
            var components = entityInfo.Chunk.Archetype.Components;
            if (!components.IsSupersetOfSortedSpan(SortedComponentIDs.Span))
                return false;

            return true;
        }
    }

    /// <summary>
    /// Try to follow a component, using a mapper to extract an entity from it
    /// </summary>
    /// <typeparam name="M"></typeparam>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    /// <typeparam name="T9"></typeparam>
    /// <typeparam name="T10"></typeparam>
    /// <typeparam name="T11"></typeparam>
    [ExcludeFromCodeCoverage]
    public sealed class Follow<M, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
        : BaseGenericStep<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
        where M : IQueryMap<Entity, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
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
        private readonly M _map;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        public Follow(M map)
        {
            _map = map;
        }

        /// <inheritdoc />
        public override bool TryFollow(ref Entity entity)
        {
            // Try to get entity info for this entity
            var dummy = default(EntityInfo);
            ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

            // Can't follow a path through a dead entity!
            if (isDead)
                return false;

            // Check if all components are present in one go
            var components = entityInfo.Chunk.Archetype.Components;
            if (!components.IsSupersetOfSortedSpan(SortedComponentIDs.Span))
                return false;

            // Get component references
            var row = entityInfo.GetRow(entity.ID);
            ref var t0 = ref row.GetMutable<T0>(C0);
            ref var t1 = ref row.GetMutable<T1>(C1);
            ref var t2 = ref row.GetMutable<T2>(C2);
            ref var t3 = ref row.GetMutable<T3>(C3);
            ref var t4 = ref row.GetMutable<T4>(C4);
            ref var t5 = ref row.GetMutable<T5>(C5);
            ref var t6 = ref row.GetMutable<T6>(C6);
            ref var t7 = ref row.GetMutable<T7>(C7);
            ref var t8 = ref row.GetMutable<T8>(C8);
            ref var t9 = ref row.GetMutable<T9>(C9);
            ref var t10 = ref row.GetMutable<T10>(C10);
            ref var t11 = ref row.GetMutable<T11>(C11);

            // Execute predicate
            entity = _map.Execute(entity, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7, ref t8, ref t9, ref t10, ref t11);
            return true;
        }
    }

    /// <summary>
    /// Base class for path steps that take generic parameters
    /// </summary>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    /// <typeparam name="T9"></typeparam>
    /// <typeparam name="T10"></typeparam>
    /// <typeparam name="T11"></typeparam>
    /// <typeparam name="T12"></typeparam>
    [ExcludeFromCodeCoverage]
    public abstract class BaseGenericStep<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
        : IStep
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
#region Component IDs
        /// <summary>Component ID for T0</summary>
        protected static readonly ComponentID C0 = ComponentID<T0>.ID;
        /// <summary>Component ID for T1</summary>
        protected static readonly ComponentID C1 = ComponentID<T1>.ID;
        /// <summary>Component ID for T2</summary>
        protected static readonly ComponentID C2 = ComponentID<T2>.ID;
        /// <summary>Component ID for T3</summary>
        protected static readonly ComponentID C3 = ComponentID<T3>.ID;
        /// <summary>Component ID for T4</summary>
        protected static readonly ComponentID C4 = ComponentID<T4>.ID;
        /// <summary>Component ID for T5</summary>
        protected static readonly ComponentID C5 = ComponentID<T5>.ID;
        /// <summary>Component ID for T6</summary>
        protected static readonly ComponentID C6 = ComponentID<T6>.ID;
        /// <summary>Component ID for T7</summary>
        protected static readonly ComponentID C7 = ComponentID<T7>.ID;
        /// <summary>Component ID for T8</summary>
        protected static readonly ComponentID C8 = ComponentID<T8>.ID;
        /// <summary>Component ID for T9</summary>
        protected static readonly ComponentID C9 = ComponentID<T9>.ID;
        /// <summary>Component ID for T10</summary>
        protected static readonly ComponentID C10 = ComponentID<T10>.ID;
        /// <summary>Component ID for T11</summary>
        protected static readonly ComponentID C11 = ComponentID<T11>.ID;
        /// <summary>Component ID for T12</summary>
        protected static readonly ComponentID C12 = ComponentID<T12>.ID;

        /// <summary>All components IDs, in order</summary>
        protected static readonly ReadOnlyMemory<ComponentID> SortedComponentIDs;

        static BaseGenericStep()
        {
            var arr = new[] {
                C0,
                C1,
                C2,
                C3,
                C4,
                C5,
                C6,
                C7,
                C8,
                C9,
                C10,
                C11,
                C12,
            };
            Array.Sort(arr);

            SortedComponentIDs = arr;
        }
        #endregion

        /// <inheritdoc />
        public abstract bool TryFollow(ref Entity entity);
    }

    /// <summary>
    /// Construct a path step that checks if a predicate is true
    /// </summary>
    /// <typeparam name="P">Predicate</typeparam>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    /// <typeparam name="T9"></typeparam>
    /// <typeparam name="T10"></typeparam>
    /// <typeparam name="T11"></typeparam>
    /// <typeparam name="T12"></typeparam>
    [ExcludeFromCodeCoverage]
    public sealed class Predicate<P, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
        : BaseGenericStep<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
        where P : IQueryMap<bool, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
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
        private readonly P _predicate;

        /// <summary>
        /// Create a new <see cref="Predicate{P,T0}"/>
        /// </summary>
        /// <param name="predicate"></param>
        public Predicate(P predicate)
        {
            _predicate = predicate;
        }

        /// <inheritdoc />
        public override bool TryFollow(ref Entity entity)
        {
            // Try to get entity info for this entity
            var dummy = default(EntityInfo);
            ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

            // Can't follow a path through a dead entity!
            if (isDead)
                return false;

            // Check if all components are present in one go
            var components = entityInfo.Chunk.Archetype.Components;
            if (!components.IsSupersetOfSortedSpan(SortedComponentIDs.Span))
                return false;

            // Get component references
            var row = entityInfo.GetRow(entity.ID);
            ref var t0 = ref row.GetMutable<T0>(C0);
            ref var t1 = ref row.GetMutable<T1>(C1);
            ref var t2 = ref row.GetMutable<T2>(C2);
            ref var t3 = ref row.GetMutable<T3>(C3);
            ref var t4 = ref row.GetMutable<T4>(C4);
            ref var t5 = ref row.GetMutable<T5>(C5);
            ref var t6 = ref row.GetMutable<T6>(C6);
            ref var t7 = ref row.GetMutable<T7>(C7);
            ref var t8 = ref row.GetMutable<T8>(C8);
            ref var t9 = ref row.GetMutable<T9>(C9);
            ref var t10 = ref row.GetMutable<T10>(C10);
            ref var t11 = ref row.GetMutable<T11>(C11);
            ref var t12 = ref row.GetMutable<T12>(C12);

            // Execute predicate
            return _predicate.Execute(entity, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7, ref t8, ref t9, ref t10, ref t11, ref t12);
        }
    }

    /// <summary>
    /// Check that the current entity has the given components, fail to follow the path if any are missing
    /// </summary>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    /// <typeparam name="T9"></typeparam>
    /// <typeparam name="T10"></typeparam>
    /// <typeparam name="T11"></typeparam>
    /// <typeparam name="T12"></typeparam>
    [ExcludeFromCodeCoverage]
    public sealed class HasComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
        : BaseGenericStep<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
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
        /// <inheritdoc />
        public override bool TryFollow(ref Entity entity)
        {
            // Try to get entity info for this entity
            var dummy = default(EntityInfo);
            ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

            // Can't follow a path through a dead entity!
            if (isDead)
                return false;

            // Check if all components are present in one go
            var components = entityInfo.Chunk.Archetype.Components;
            if (!components.IsSupersetOfSortedSpan(SortedComponentIDs.Span))
                return false;

            return true;
        }
    }

    /// <summary>
    /// Try to follow a component, using a mapper to extract an entity from it
    /// </summary>
    /// <typeparam name="M"></typeparam>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    /// <typeparam name="T9"></typeparam>
    /// <typeparam name="T10"></typeparam>
    /// <typeparam name="T11"></typeparam>
    /// <typeparam name="T12"></typeparam>
    [ExcludeFromCodeCoverage]
    public sealed class Follow<M, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
        : BaseGenericStep<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
        where M : IQueryMap<Entity, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
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
        private readonly M _map;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        public Follow(M map)
        {
            _map = map;
        }

        /// <inheritdoc />
        public override bool TryFollow(ref Entity entity)
        {
            // Try to get entity info for this entity
            var dummy = default(EntityInfo);
            ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

            // Can't follow a path through a dead entity!
            if (isDead)
                return false;

            // Check if all components are present in one go
            var components = entityInfo.Chunk.Archetype.Components;
            if (!components.IsSupersetOfSortedSpan(SortedComponentIDs.Span))
                return false;

            // Get component references
            var row = entityInfo.GetRow(entity.ID);
            ref var t0 = ref row.GetMutable<T0>(C0);
            ref var t1 = ref row.GetMutable<T1>(C1);
            ref var t2 = ref row.GetMutable<T2>(C2);
            ref var t3 = ref row.GetMutable<T3>(C3);
            ref var t4 = ref row.GetMutable<T4>(C4);
            ref var t5 = ref row.GetMutable<T5>(C5);
            ref var t6 = ref row.GetMutable<T6>(C6);
            ref var t7 = ref row.GetMutable<T7>(C7);
            ref var t8 = ref row.GetMutable<T8>(C8);
            ref var t9 = ref row.GetMutable<T9>(C9);
            ref var t10 = ref row.GetMutable<T10>(C10);
            ref var t11 = ref row.GetMutable<T11>(C11);
            ref var t12 = ref row.GetMutable<T12>(C12);

            // Execute predicate
            entity = _map.Execute(entity, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7, ref t8, ref t9, ref t10, ref t11, ref t12);
            return true;
        }
    }

    /// <summary>
    /// Base class for path steps that take generic parameters
    /// </summary>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    /// <typeparam name="T9"></typeparam>
    /// <typeparam name="T10"></typeparam>
    /// <typeparam name="T11"></typeparam>
    /// <typeparam name="T12"></typeparam>
    /// <typeparam name="T13"></typeparam>
    [ExcludeFromCodeCoverage]
    public abstract class BaseGenericStep<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        : IStep
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
#region Component IDs
        /// <summary>Component ID for T0</summary>
        protected static readonly ComponentID C0 = ComponentID<T0>.ID;
        /// <summary>Component ID for T1</summary>
        protected static readonly ComponentID C1 = ComponentID<T1>.ID;
        /// <summary>Component ID for T2</summary>
        protected static readonly ComponentID C2 = ComponentID<T2>.ID;
        /// <summary>Component ID for T3</summary>
        protected static readonly ComponentID C3 = ComponentID<T3>.ID;
        /// <summary>Component ID for T4</summary>
        protected static readonly ComponentID C4 = ComponentID<T4>.ID;
        /// <summary>Component ID for T5</summary>
        protected static readonly ComponentID C5 = ComponentID<T5>.ID;
        /// <summary>Component ID for T6</summary>
        protected static readonly ComponentID C6 = ComponentID<T6>.ID;
        /// <summary>Component ID for T7</summary>
        protected static readonly ComponentID C7 = ComponentID<T7>.ID;
        /// <summary>Component ID for T8</summary>
        protected static readonly ComponentID C8 = ComponentID<T8>.ID;
        /// <summary>Component ID for T9</summary>
        protected static readonly ComponentID C9 = ComponentID<T9>.ID;
        /// <summary>Component ID for T10</summary>
        protected static readonly ComponentID C10 = ComponentID<T10>.ID;
        /// <summary>Component ID for T11</summary>
        protected static readonly ComponentID C11 = ComponentID<T11>.ID;
        /// <summary>Component ID for T12</summary>
        protected static readonly ComponentID C12 = ComponentID<T12>.ID;
        /// <summary>Component ID for T13</summary>
        protected static readonly ComponentID C13 = ComponentID<T13>.ID;

        /// <summary>All components IDs, in order</summary>
        protected static readonly ReadOnlyMemory<ComponentID> SortedComponentIDs;

        static BaseGenericStep()
        {
            var arr = new[] {
                C0,
                C1,
                C2,
                C3,
                C4,
                C5,
                C6,
                C7,
                C8,
                C9,
                C10,
                C11,
                C12,
                C13,
            };
            Array.Sort(arr);

            SortedComponentIDs = arr;
        }
        #endregion

        /// <inheritdoc />
        public abstract bool TryFollow(ref Entity entity);
    }

    /// <summary>
    /// Construct a path step that checks if a predicate is true
    /// </summary>
    /// <typeparam name="P">Predicate</typeparam>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    /// <typeparam name="T9"></typeparam>
    /// <typeparam name="T10"></typeparam>
    /// <typeparam name="T11"></typeparam>
    /// <typeparam name="T12"></typeparam>
    /// <typeparam name="T13"></typeparam>
    [ExcludeFromCodeCoverage]
    public sealed class Predicate<P, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        : BaseGenericStep<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        where P : IQueryMap<bool, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
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
        private readonly P _predicate;

        /// <summary>
        /// Create a new <see cref="Predicate{P,T0}"/>
        /// </summary>
        /// <param name="predicate"></param>
        public Predicate(P predicate)
        {
            _predicate = predicate;
        }

        /// <inheritdoc />
        public override bool TryFollow(ref Entity entity)
        {
            // Try to get entity info for this entity
            var dummy = default(EntityInfo);
            ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

            // Can't follow a path through a dead entity!
            if (isDead)
                return false;

            // Check if all components are present in one go
            var components = entityInfo.Chunk.Archetype.Components;
            if (!components.IsSupersetOfSortedSpan(SortedComponentIDs.Span))
                return false;

            // Get component references
            var row = entityInfo.GetRow(entity.ID);
            ref var t0 = ref row.GetMutable<T0>(C0);
            ref var t1 = ref row.GetMutable<T1>(C1);
            ref var t2 = ref row.GetMutable<T2>(C2);
            ref var t3 = ref row.GetMutable<T3>(C3);
            ref var t4 = ref row.GetMutable<T4>(C4);
            ref var t5 = ref row.GetMutable<T5>(C5);
            ref var t6 = ref row.GetMutable<T6>(C6);
            ref var t7 = ref row.GetMutable<T7>(C7);
            ref var t8 = ref row.GetMutable<T8>(C8);
            ref var t9 = ref row.GetMutable<T9>(C9);
            ref var t10 = ref row.GetMutable<T10>(C10);
            ref var t11 = ref row.GetMutable<T11>(C11);
            ref var t12 = ref row.GetMutable<T12>(C12);
            ref var t13 = ref row.GetMutable<T13>(C13);

            // Execute predicate
            return _predicate.Execute(entity, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7, ref t8, ref t9, ref t10, ref t11, ref t12, ref t13);
        }
    }

    /// <summary>
    /// Check that the current entity has the given components, fail to follow the path if any are missing
    /// </summary>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    /// <typeparam name="T9"></typeparam>
    /// <typeparam name="T10"></typeparam>
    /// <typeparam name="T11"></typeparam>
    /// <typeparam name="T12"></typeparam>
    /// <typeparam name="T13"></typeparam>
    [ExcludeFromCodeCoverage]
    public sealed class HasComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        : BaseGenericStep<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
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
        /// <inheritdoc />
        public override bool TryFollow(ref Entity entity)
        {
            // Try to get entity info for this entity
            var dummy = default(EntityInfo);
            ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

            // Can't follow a path through a dead entity!
            if (isDead)
                return false;

            // Check if all components are present in one go
            var components = entityInfo.Chunk.Archetype.Components;
            if (!components.IsSupersetOfSortedSpan(SortedComponentIDs.Span))
                return false;

            return true;
        }
    }

    /// <summary>
    /// Try to follow a component, using a mapper to extract an entity from it
    /// </summary>
    /// <typeparam name="M"></typeparam>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    /// <typeparam name="T9"></typeparam>
    /// <typeparam name="T10"></typeparam>
    /// <typeparam name="T11"></typeparam>
    /// <typeparam name="T12"></typeparam>
    /// <typeparam name="T13"></typeparam>
    [ExcludeFromCodeCoverage]
    public sealed class Follow<M, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        : BaseGenericStep<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        where M : IQueryMap<Entity, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
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
        private readonly M _map;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        public Follow(M map)
        {
            _map = map;
        }

        /// <inheritdoc />
        public override bool TryFollow(ref Entity entity)
        {
            // Try to get entity info for this entity
            var dummy = default(EntityInfo);
            ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

            // Can't follow a path through a dead entity!
            if (isDead)
                return false;

            // Check if all components are present in one go
            var components = entityInfo.Chunk.Archetype.Components;
            if (!components.IsSupersetOfSortedSpan(SortedComponentIDs.Span))
                return false;

            // Get component references
            var row = entityInfo.GetRow(entity.ID);
            ref var t0 = ref row.GetMutable<T0>(C0);
            ref var t1 = ref row.GetMutable<T1>(C1);
            ref var t2 = ref row.GetMutable<T2>(C2);
            ref var t3 = ref row.GetMutable<T3>(C3);
            ref var t4 = ref row.GetMutable<T4>(C4);
            ref var t5 = ref row.GetMutable<T5>(C5);
            ref var t6 = ref row.GetMutable<T6>(C6);
            ref var t7 = ref row.GetMutable<T7>(C7);
            ref var t8 = ref row.GetMutable<T8>(C8);
            ref var t9 = ref row.GetMutable<T9>(C9);
            ref var t10 = ref row.GetMutable<T10>(C10);
            ref var t11 = ref row.GetMutable<T11>(C11);
            ref var t12 = ref row.GetMutable<T12>(C12);
            ref var t13 = ref row.GetMutable<T13>(C13);

            // Execute predicate
            entity = _map.Execute(entity, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7, ref t8, ref t9, ref t10, ref t11, ref t12, ref t13);
            return true;
        }
    }

    /// <summary>
    /// Base class for path steps that take generic parameters
    /// </summary>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    /// <typeparam name="T9"></typeparam>
    /// <typeparam name="T10"></typeparam>
    /// <typeparam name="T11"></typeparam>
    /// <typeparam name="T12"></typeparam>
    /// <typeparam name="T13"></typeparam>
    /// <typeparam name="T14"></typeparam>
    [ExcludeFromCodeCoverage]
    public abstract class BaseGenericStep<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        : IStep
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
#region Component IDs
        /// <summary>Component ID for T0</summary>
        protected static readonly ComponentID C0 = ComponentID<T0>.ID;
        /// <summary>Component ID for T1</summary>
        protected static readonly ComponentID C1 = ComponentID<T1>.ID;
        /// <summary>Component ID for T2</summary>
        protected static readonly ComponentID C2 = ComponentID<T2>.ID;
        /// <summary>Component ID for T3</summary>
        protected static readonly ComponentID C3 = ComponentID<T3>.ID;
        /// <summary>Component ID for T4</summary>
        protected static readonly ComponentID C4 = ComponentID<T4>.ID;
        /// <summary>Component ID for T5</summary>
        protected static readonly ComponentID C5 = ComponentID<T5>.ID;
        /// <summary>Component ID for T6</summary>
        protected static readonly ComponentID C6 = ComponentID<T6>.ID;
        /// <summary>Component ID for T7</summary>
        protected static readonly ComponentID C7 = ComponentID<T7>.ID;
        /// <summary>Component ID for T8</summary>
        protected static readonly ComponentID C8 = ComponentID<T8>.ID;
        /// <summary>Component ID for T9</summary>
        protected static readonly ComponentID C9 = ComponentID<T9>.ID;
        /// <summary>Component ID for T10</summary>
        protected static readonly ComponentID C10 = ComponentID<T10>.ID;
        /// <summary>Component ID for T11</summary>
        protected static readonly ComponentID C11 = ComponentID<T11>.ID;
        /// <summary>Component ID for T12</summary>
        protected static readonly ComponentID C12 = ComponentID<T12>.ID;
        /// <summary>Component ID for T13</summary>
        protected static readonly ComponentID C13 = ComponentID<T13>.ID;
        /// <summary>Component ID for T14</summary>
        protected static readonly ComponentID C14 = ComponentID<T14>.ID;

        /// <summary>All components IDs, in order</summary>
        protected static readonly ReadOnlyMemory<ComponentID> SortedComponentIDs;

        static BaseGenericStep()
        {
            var arr = new[] {
                C0,
                C1,
                C2,
                C3,
                C4,
                C5,
                C6,
                C7,
                C8,
                C9,
                C10,
                C11,
                C12,
                C13,
                C14,
            };
            Array.Sort(arr);

            SortedComponentIDs = arr;
        }
        #endregion

        /// <inheritdoc />
        public abstract bool TryFollow(ref Entity entity);
    }

    /// <summary>
    /// Construct a path step that checks if a predicate is true
    /// </summary>
    /// <typeparam name="P">Predicate</typeparam>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    /// <typeparam name="T9"></typeparam>
    /// <typeparam name="T10"></typeparam>
    /// <typeparam name="T11"></typeparam>
    /// <typeparam name="T12"></typeparam>
    /// <typeparam name="T13"></typeparam>
    /// <typeparam name="T14"></typeparam>
    [ExcludeFromCodeCoverage]
    public sealed class Predicate<P, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        : BaseGenericStep<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        where P : IQueryMap<bool, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
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
        private readonly P _predicate;

        /// <summary>
        /// Create a new <see cref="Predicate{P,T0}"/>
        /// </summary>
        /// <param name="predicate"></param>
        public Predicate(P predicate)
        {
            _predicate = predicate;
        }

        /// <inheritdoc />
        public override bool TryFollow(ref Entity entity)
        {
            // Try to get entity info for this entity
            var dummy = default(EntityInfo);
            ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

            // Can't follow a path through a dead entity!
            if (isDead)
                return false;

            // Check if all components are present in one go
            var components = entityInfo.Chunk.Archetype.Components;
            if (!components.IsSupersetOfSortedSpan(SortedComponentIDs.Span))
                return false;

            // Get component references
            var row = entityInfo.GetRow(entity.ID);
            ref var t0 = ref row.GetMutable<T0>(C0);
            ref var t1 = ref row.GetMutable<T1>(C1);
            ref var t2 = ref row.GetMutable<T2>(C2);
            ref var t3 = ref row.GetMutable<T3>(C3);
            ref var t4 = ref row.GetMutable<T4>(C4);
            ref var t5 = ref row.GetMutable<T5>(C5);
            ref var t6 = ref row.GetMutable<T6>(C6);
            ref var t7 = ref row.GetMutable<T7>(C7);
            ref var t8 = ref row.GetMutable<T8>(C8);
            ref var t9 = ref row.GetMutable<T9>(C9);
            ref var t10 = ref row.GetMutable<T10>(C10);
            ref var t11 = ref row.GetMutable<T11>(C11);
            ref var t12 = ref row.GetMutable<T12>(C12);
            ref var t13 = ref row.GetMutable<T13>(C13);
            ref var t14 = ref row.GetMutable<T14>(C14);

            // Execute predicate
            return _predicate.Execute(entity, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7, ref t8, ref t9, ref t10, ref t11, ref t12, ref t13, ref t14);
        }
    }

    /// <summary>
    /// Check that the current entity has the given components, fail to follow the path if any are missing
    /// </summary>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    /// <typeparam name="T9"></typeparam>
    /// <typeparam name="T10"></typeparam>
    /// <typeparam name="T11"></typeparam>
    /// <typeparam name="T12"></typeparam>
    /// <typeparam name="T13"></typeparam>
    /// <typeparam name="T14"></typeparam>
    [ExcludeFromCodeCoverage]
    public sealed class HasComponents<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        : BaseGenericStep<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
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
        /// <inheritdoc />
        public override bool TryFollow(ref Entity entity)
        {
            // Try to get entity info for this entity
            var dummy = default(EntityInfo);
            ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

            // Can't follow a path through a dead entity!
            if (isDead)
                return false;

            // Check if all components are present in one go
            var components = entityInfo.Chunk.Archetype.Components;
            if (!components.IsSupersetOfSortedSpan(SortedComponentIDs.Span))
                return false;

            return true;
        }
    }

    /// <summary>
    /// Try to follow a component, using a mapper to extract an entity from it
    /// </summary>
    /// <typeparam name="M"></typeparam>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    /// <typeparam name="T9"></typeparam>
    /// <typeparam name="T10"></typeparam>
    /// <typeparam name="T11"></typeparam>
    /// <typeparam name="T12"></typeparam>
    /// <typeparam name="T13"></typeparam>
    /// <typeparam name="T14"></typeparam>
    [ExcludeFromCodeCoverage]
    public sealed class Follow<M, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        : BaseGenericStep<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        where M : IQueryMap<Entity, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
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
        private readonly M _map;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        public Follow(M map)
        {
            _map = map;
        }

        /// <inheritdoc />
        public override bool TryFollow(ref Entity entity)
        {
            // Try to get entity info for this entity
            var dummy = default(EntityInfo);
            ref var entityInfo = ref entity.World.GetEntityInfo(entity.ID, ref dummy, out var isDead);

            // Can't follow a path through a dead entity!
            if (isDead)
                return false;

            // Check if all components are present in one go
            var components = entityInfo.Chunk.Archetype.Components;
            if (!components.IsSupersetOfSortedSpan(SortedComponentIDs.Span))
                return false;

            // Get component references
            var row = entityInfo.GetRow(entity.ID);
            ref var t0 = ref row.GetMutable<T0>(C0);
            ref var t1 = ref row.GetMutable<T1>(C1);
            ref var t2 = ref row.GetMutable<T2>(C2);
            ref var t3 = ref row.GetMutable<T3>(C3);
            ref var t4 = ref row.GetMutable<T4>(C4);
            ref var t5 = ref row.GetMutable<T5>(C5);
            ref var t6 = ref row.GetMutable<T6>(C6);
            ref var t7 = ref row.GetMutable<T7>(C7);
            ref var t8 = ref row.GetMutable<T8>(C8);
            ref var t9 = ref row.GetMutable<T9>(C9);
            ref var t10 = ref row.GetMutable<T10>(C10);
            ref var t11 = ref row.GetMutable<T11>(C11);
            ref var t12 = ref row.GetMutable<T12>(C12);
            ref var t13 = ref row.GetMutable<T13>(C13);
            ref var t14 = ref row.GetMutable<T14>(C14);

            // Execute predicate
            entity = _map.Execute(entity, ref t0, ref t1, ref t2, ref t3, ref t4, ref t5, ref t6, ref t7, ref t8, ref t9, ref t10, ref t11, ref t12, ref t13, ref t14);
            return true;
        }
    }

}

