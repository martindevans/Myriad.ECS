using System.Diagnostics;
using Myriad.ECS.IDs;
using Myriad.ECS.Worlds;
using Myriad.ECS.Worlds.Chunks;

namespace Myriad.ECS.Components;

/// <summary>
/// A relation component can be added to a command buffer, along with an associated buffered entity. When the
/// buffered entity is created it will be automatically resolved and added to this component.
/// </summary>
public interface IEntityRelationComponent : IComponent
{
    /// <summary>
    /// The target entity of this relationship
    /// </summary>
    public Entity Target { get; set; }
}

/// <summary>
/// Contains a reference to the entity it is attached to
/// </summary>
public struct SelfReference
    : IEntityRelationComponent
{
    /// <summary>
    /// The entity this component is attached to
    /// </summary>
    public Entity Target { get; set; }
}

/// <summary>
/// A reference to another Entity which caches information about the Entity, including whether it has a
/// specific component type. This can be used to avoid structural checks when access through the relation.
/// </summary>
/// <typeparam name="TComponent"></typeparam>
public struct TypedReference<TComponent>
    : IEntityRelationComponent
    where TComponent : IComponent
{
    private static readonly ComponentID ComponentID = ComponentID<TComponent>.ID;

    private Entity _entity;

    /// <inheritdoc />
    public Entity Target
    {
        get => _entity;
        set
        {
            _entity = value;
            _cache = null;
        }
    }

    private Cache? _cache;

    /// <summary>
    /// Check the entity state
    /// </summary>
    /// <param name="world">The entity world</param>
    /// <param name="hasComponent">Indicates if the entity has the component</param>
    /// <param name="exists">Indicates if the entity exists</param>
    /// <param name="isPhantom">Indicates if the entity is a phantom</param>
    /// <param name="defaultRef">Default reference to return when hasComponent is false</param>
    /// <returns></returns>
    public ref TComponent Check(World world, out bool hasComponent, out bool exists, out bool isPhantom, ref TComponent defaultRef)
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

        hasComponent = v.HasComponent;
        exists = v.RowIndex >= 0;
        isPhantom = v.IsPhantom;

        if (hasComponent)
            return ref v.Chunk!.GetRef<TComponent>(v.RowIndex);
        return ref defaultRef;
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

        public Cache(int rowIndex, Chunk? chunk, bool hasComponent, bool isPhantom)
        {
            RowIndex = rowIndex;
            Chunk = chunk;
            HasComponent = hasComponent;
            IsPhantom = isPhantom;
        }

        public bool CheckNotStale(EntityId entity)
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

            var hasComponent = archetype.Components.Contains(ComponentID);
            var isPhantom = archetype.IsPhantom;

            return new Cache(
                info.RowIndex, info.Chunk, hasComponent, isPhantom
            );
        }
    }
}