using Myriad.ECS.Command;
using Myriad.ECS.Components;
using Myriad.ECS.Queries;
using Myriad.ECS.Worlds;

namespace Myriad.ECS.Systems;

/// <summary>
/// Automatically attaches <see cref="TAssociated"/> component to any entity which has <see cref="TComponent"/>. Automatically removes
/// <see cref="TAssociated"/> component from any entity which has <see cref="TAssociated"/> and does not have <see cref="TComponent"/>.
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

    private readonly bool _associatedIsPhantom = typeof(IPhantomComponent).IsAssignableFrom(typeof(TAssociated));

    public void Update(CommandBuffer cmd)
    {
        world.Execute(new Attach(this, cmd), _addQuery);
        world.Execute<Detach, TAssociated>(new Detach(this, cmd), _removeQuery);

        if (_associatedIsPhantom)
            world.Execute<DetachPhantom, TAssociated>(new DetachPhantom(this, cmd), _removeQueryPhantom);
    }

    protected abstract TAssociated Create(Entity entity);

    /// <summary>
    /// Called when an Entity is found that has <see cref="TComponent"/> but does not have <see cref="TAssociated"/>.
    ///
    /// Call base.OnAttach to attach default <see cref="TAssociated"/>, or attach it yourself and do not call base.OnAttach.
    /// </summary>
    /// <param name="e"></param>
    /// <param name="c"></param>
    protected virtual void OnAttach(Entity e, CommandBuffer c)
    {
        c.Set(e, Create(e));
    }

    /// <summary>
    /// Called when an Entity is found that has <see cref="TAssociated"/> but does not have <see cref="TComponent"/>.
    /// 
    /// Call base.OnDetach to detach <see cref="TAssociated"/>.
    /// </summary>
    /// <param name="e"></param>
    /// <param name="c"></param>
    /// <param name="associated"></param>
    protected virtual void OnDetach(Entity e, CommandBuffer c, ref TAssociated associated)
    {
        c.Remove<TAssociated>(e);
    }

    /// <summary>
    /// Called when an Entity is found that has <see cref="TAssociated"/> and is a Phantom entity. This will only happen if <see cref="TAssociated"/> is
    /// an <see cref="IPhantomComponent"/>.
    /// 
    /// call base.OnDetachPhantom to datach <see cref="TAssociated"/>
    /// </summary>
    /// <param name="e"></param>
    /// <param name="c"></param>
    /// <param name="associated"></param>
    protected virtual void OnDetachPhantom(Entity e, CommandBuffer c, TAssociated associated)
    {
        c.Remove<TAssociated>(e);
    }

    private readonly struct Attach(SystemState<TComponent, TAssociated> state, CommandBuffer cmd)
        : IQuery0
    {
        public void Execute(Entity e)
        {
            state.OnAttach(e, cmd);
        }
    }

    private readonly struct Detach(SystemState<TComponent, TAssociated> state, CommandBuffer cmd)
        : IQuery1<TAssociated>
    {
        public void Execute(Entity e, ref TAssociated a)
        {
            state.OnDetach(e, cmd, ref a);
        }
    }

    private readonly struct DetachPhantom(SystemState<TComponent, TAssociated> state, CommandBuffer cmd)
        : IQuery1<TAssociated>
    {
        public void Execute(Entity e, ref TAssociated a)
        {
            state.OnDetachPhantom(e, cmd, a);
        }
    }
}