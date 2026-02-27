using Myriad.ECS.Command;
using Myriad.ECS.Components;
using Myriad.ECS.Queries;
using Myriad.ECS.Worlds;

namespace Myriad.ECS.Systems;

/// <summary>
/// Automatically attaches <typeparamref name="TAssociated"/> component to any entity which has <typeparamref name="TComponent"/>. Automatically removes
/// <typeparamref name="TAssociated"/> component from any entity which has <typeparamref name="TAssociated"/> and does not have <typeparamref name="TComponent"/>.
/// </summary>
/// <typeparam name="TComponent"></typeparam>
/// <typeparam name="TAssociated"></typeparam>
public abstract class SystemState<TComponent, TAssociated>(World world)
    where TComponent : IComponent
    where TAssociated : IComponent
{
    private readonly QueryDescription _addQuery = new QueryBuilder().Include<TComponent>().Exclude<TAssociated>().Build(world);
    private readonly QueryDescription _removeQuery = new QueryBuilder().Exclude<TComponent>().Include<TAssociated>().Build(world);
    private readonly QueryDescription _removeQueryPhantom = new QueryBuilder().Include<TAssociated>().Include<Phantom>().Build(world);

    /// <summary>
    /// Indicates if <typeparamref name="TAssociated"/> is an <see cref="IPhantomComponent"/>
    /// </summary>
    public bool AssociatedIsPhantom { get; } = typeof(IPhantomComponent).IsAssignableFrom(typeof(TAssociated));

    /// <summary>
    /// Apply changes
    /// </summary>
    /// <param name="cmd"></param>
    /// <returns>The number of updated entities</returns>
    public int Update(CommandBuffer cmd)
    {
        return world.Execute(new Attach(this, cmd), _addQuery)
             + world.Execute<Detach, TAssociated>(new Detach(this, cmd), AssociatedIsPhantom ? _removeQueryPhantom : _removeQuery);
    }

    /// <summary>
    /// Create an instance of <typeparamref name="TAssociated" /> for the given entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    protected abstract TAssociated Create(Entity entity);

    /// <summary>
    /// Called when an Entity is found that has <typeparamref name="TComponent"/> but does not have <typeparamref name="TAssociated"/>.
    ///
    /// Call base.OnAttach to attach default <typeparamref name="TAssociated"/>, or attach it yourself and do not call base.OnAttach.
    /// </summary>
    /// <param name="e"></param>
    /// <param name="c"></param>
    protected virtual void OnAttach(Entity e, CommandBuffer c)
    {
        c.Set(e, Create(e));
    }

    /// <summary>
    /// Called when an Entity is found that has <typeparamref name="TAssociated"/> but does not have <typeparamref name="TComponent"/>.
    /// 
    /// Call base.OnDetach to detach <typeparamref name="TAssociated"/>.
    /// </summary>
    /// <param name="e"></param>
    /// <param name="c"></param>
    /// <param name="associated"></param>
    protected virtual void OnDetach(Entity e, CommandBuffer c, ref TAssociated associated)
    {
        c.Remove<TAssociated>(e);
    }

    private readonly struct Attach(SystemState<TComponent, TAssociated> state, CommandBuffer cmd)
        : IQuery
    {
        public void Execute(Entity e)
        {
            state.OnAttach(e, cmd);
        }
    }

    private readonly struct Detach(SystemState<TComponent, TAssociated> state, CommandBuffer cmd)
        : IQuery<TAssociated>
    {
        public void Execute(Entity e, ref TAssociated a)
        {
            state.OnDetach(e, cmd, ref a);
        }
    }
}

/// <summary>
/// A <see cref="SystemState{TComponent,TAssociated}"/> which creates the associated component with a factory function
/// </summary>
/// <typeparam name="TComponent"></typeparam>
/// <typeparam name="TAssociated"></typeparam>
/// <param name="world"></param>
/// <param name="factory"></param>
public class FactorySystemState<TComponent, TAssociated>(World world, Func<Entity, TAssociated> factory)
    : SystemState<TComponent, TAssociated>(world)
    where TComponent : IComponent
    where TAssociated : IComponent
{
    /// <inheritdoc />
    protected override TAssociated Create(Entity entity)
    {
        return factory(entity);
    }
}