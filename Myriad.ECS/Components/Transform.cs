using Myriad.ECS.Queries;
using Myriad.ECS.Systems;
using Myriad.ECS.Worlds;

namespace Myriad.ECS.Components;

/// <summary>
/// Represents a transform
/// </summary>
public interface ITransform<TSelf>
    where TSelf : ITransform<TSelf>
{
    /// <summary>
    /// Given a child transform, compose it with this transform
    /// </summary>
    /// <param name="child"></param>
    /// <returns></returns>
    TSelf Compose(TSelf child);
}

/// <summary>
/// Indicates which entity this entity is transformed relative to
/// </summary>
public struct TransformParent
    : IEntityRelationComponent
{
    /// <summary>
    /// Parent entity transform
    /// </summary>
    public Entity Target { get; set; }
}

/// <summary>
/// Transform of this entity, relative to parent
/// </summary>
public interface ILocalTransform<TTransform>
    : IComponent
    where TTransform : ITransform<TTransform>
{
    /// <summary>
    /// Get or set the local transform
    /// </summary>
    TTransform Transform { get; set; }
}

/// <summary>
/// Transform of this entity, in world space.
/// </summary>
public interface IWorldTransform<TTransform>
    : IComponent
    where TTransform : ITransform<TTransform>
{
    /// <summary>
    /// Get or set the local transform
    /// </summary>
    TTransform Transform { get; set; }

    /// <summary>
    /// Indicates to the <see cref="BaseUpdateTransformHierarchySystem{TData,TTransform,TLocalTransform,TWorldTransform}"/> when this world transform was last updated
    /// </summary>
    int Phase { get; set; }
}

/// <summary>
/// Automatically updates hierarchy of entities, deriving world transform from local transform and parent.
/// </summary>
/// <typeparam name="TData"></typeparam>
/// <typeparam name="TLocalTransform"></typeparam>
/// <typeparam name="TWorldTransform"></typeparam>
/// <typeparam name="TTransform"></typeparam>
public class BaseUpdateTransformHierarchySystem<TData, TTransform, TLocalTransform, TWorldTransform>
    : ISystem<TData>
    where TLocalTransform : ILocalTransform<TTransform>
    where TWorldTransform : IWorldTransform<TTransform>
    where TTransform : ITransform<TTransform>
{
    private readonly World _world;
    private readonly QueryDescription _queryRoot;
    private readonly QueryDescription _query;

    private int _phase;

    /// <summary>
    /// Create new <see cref="BaseUpdateTransformHierarchySystem{TData,TTransform,TLocalTransform,TWorldTransform}"/>
    /// </summary>
    /// <param name="world"></param>
    public BaseUpdateTransformHierarchySystem(World world)
    {
        _world = world;
        _queryRoot = new QueryBuilder().Include<TLocalTransform, TWorldTransform>().Exclude<TransformParent>().Build(world);
        _query     = new QueryBuilder().Include<TLocalTransform, TWorldTransform>().Include<TransformParent>().Build(world);
        _phase = DateTime.UtcNow.Millisecond;
    }

    /// <inheritdoc />
    public void Update(TData data)
    {
        // Move to the next phase
        unchecked { _phase++; }

        // Update roots, just copying across transform from local to world
        _world.Execute<RootUpdate, TLocalTransform, TWorldTransform>(new RootUpdate(_phase), _queryRoot);

        // Recursively update children (walking up tree from entities to parent)
        _world.Execute<RecursiveUpdate, TLocalTransform, TWorldTransform, TransformParent>(new RecursiveUpdate(this, _phase), _query);
    }

    /// <summary>
    /// This will be called when a loop is detected. As soon as a loop is detected the last entity in the loop (arbitrary order) is treated
    /// as the root.
    /// </summary>
    /// <param name="entity">One of the entities in the loop</param>
    protected virtual void LoopDetected(Entity entity)
    {
    }

    private static void Update(int phaseDone, ref TLocalTransform local, ref TWorldTransform world)
    {
        if (world.Phase == phaseDone)
            return;

        world.Transform = local.Transform;
        world.Phase = phaseDone;
    }

    private void Update(int phaseDone, int phaseWip, Entity entity, ref TLocalTransform local, ref TWorldTransform world, ref TransformParent parent)
    {
        // Early exit if this has already been updated in this phase
        if (world.Phase == phaseDone)
            return;

        // Detect loops and early exit
        if (world.Phase == phaseWip)
        {
            world.Phase = phaseDone;
            LoopDetected(entity);
            return;
        }

        // Mark this object with the WIP flag, to detect and break loops later
        world.Phase = phaseWip;

        // Check status of parent entity
        var parentHasWorld = parent.Target.IsAlive() && parent.Target.HasComponent<TWorldTransform>();

        // If the parent doesn't exist or doesn't have a transform, treat this entity as a root
        if (!parentHasWorld)
        {
            Update(phaseDone, ref local, ref world);
        }
        else
        {
            ref var pWorldTrans = ref parent.Target.GetComponentRef<TWorldTransform>();
            if (parent.Target.HasComponent<TLocalTransform>())
            {
                ref var pLocalTrans = ref parent.Target.GetComponentRef<TLocalTransform>();

                // Update parent before using its transform
                if (parent.Target.HasComponent<TransformParent>())
                    Update(phaseDone, phaseWip, parent.Target, ref pLocalTrans, ref pWorldTrans, ref parent.Target.GetComponentRef<TransformParent>());
                else
                    Update(phaseDone, ref pLocalTrans, ref pWorldTrans);
            }
            else
            {
                // Parent has no local transform. Just mark it as done and transform relative to it.
                pWorldTrans.Phase = phaseDone;
            }

            // Transform relative to parent
            world.Transform = pWorldTrans.Transform.Compose(local.Transform);
        }

        // Mark this entity as done
        world.Phase = phaseDone;
    }

    private readonly struct RootUpdate
        : IQuery<TLocalTransform, TWorldTransform>
    {
        private readonly int _phase;

        public RootUpdate(int phase)
        {
            _phase = phase;
        }

        public void Execute(Entity e, ref TLocalTransform local, ref TWorldTransform world)
        {
            Update(_phase, ref local, ref world);
        }
    }

    private readonly struct RecursiveUpdate
        : IQuery<TLocalTransform, TWorldTransform, TransformParent>
    {
        private readonly BaseUpdateTransformHierarchySystem<TData, TTransform, TLocalTransform, TWorldTransform> _system;
        private readonly int _phaseDone;
        private readonly int _phaseWip;

        public RecursiveUpdate(BaseUpdateTransformHierarchySystem<TData, TTransform, TLocalTransform, TWorldTransform> system, int phase)
        {
            _system = system;
            _phaseDone = phase;

            // Create another number that's different from the phase number. Doesn't really matter what, as long as it's
            // always different. This is used to mark entities which are currently being updated, to detect loops.
            _phaseWip = phase ^ 0b1010101010101010101010101010101;
        }

        public void Execute(Entity e, ref TLocalTransform local, ref TWorldTransform world, ref TransformParent parent)
        {
            _system.Update(_phaseDone, _phaseWip, e, ref local, ref world, ref parent);
        }
    }
}