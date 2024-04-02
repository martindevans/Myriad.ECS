using Myriad.ECS.Command;
using Myriad.ECS.Queries;
using Myriad.ECS.Worlds;

namespace Myriad.ECS.Systems;

/// <summary>
/// Automatically attaches <see cref="TAssociated"/> component to any entity which has <see cref="TComponent"/>. Automatically removes
/// <see cref="TAssociated"/> component from any entity which has <see cref="TAssociated"/> and does not have <see cref="TComponent"/>.
/// </summary>
/// <typeparam name="TComponent"></typeparam>
/// <typeparam name="TAssociated"></typeparam>
public class SystemState<TComponent, TAssociated>(World world, TAssociated prototype)
    where TComponent : IComponent
    where TAssociated : IComponent
{
    private readonly QueryDescription _addQuery = new QueryBuilder().Include<TComponent>().Exclude<TAssociated>().Build(world);
    private readonly QueryDescription _removeQuery = new QueryBuilder().Exclude<TComponent>().Include<TAssociated>().Build(world);

    public void Update(CommandBuffer cmd)
    {
        world.Execute(new Attach(cmd, prototype), _addQuery);
        world.Execute(new Detach(cmd), _removeQuery);
    }

    private readonly struct Attach(CommandBuffer cmd, TAssociated prototype)
        : IQuery0
    {
        public void Execute(Entity e)
        {
            cmd.Set(e, prototype);
        }
    }

    private readonly struct Detach(CommandBuffer cmd)
        : IQuery0
    {
        public void Execute(Entity e)
        {
            cmd.Remove<TAssociated>(e);
        }
    }
}