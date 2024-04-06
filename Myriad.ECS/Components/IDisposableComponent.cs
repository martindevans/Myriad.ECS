using Myriad.ECS.Command;
using Myriad.ECS.Queries;
using Myriad.ECS.Systems;
using Myriad.ECS.Worlds;

namespace Myriad.ECS.Components
{
    /// <summary>
    /// Run the <see cref="DisposableComponentSystem{T,TC}"/> to automatically call <see cref="IDisposable.Dispose"/> on this component
    /// </summary>
    public interface IDisposableComponent
        : IPhantomComponent, IDisposable;

    /// <summary>
    /// Calls Dispose on disposable component and then removes it from the entity after the entity is dead
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TC"></typeparam>
    /// <param name="world"></param>
    public sealed class DisposableComponentSystem<T, TC>(World world, CommandBuffer? buffer = null)
        : BaseSystem<T>, IDisposable
        where TC : IDisposableComponent
    {
        private readonly CommandBuffer _cmd = buffer ?? new(world);
        private readonly bool _autorun = buffer == null;

        private readonly QueryDescription _query = new QueryBuilder().Include<Phantom>().Include<TC>().Build(world);

        public override void Update(T t)
        {
            world.Execute<DisposalQuery, TC>(new DisposalQuery(_cmd), _query);

            if (_autorun)
                _cmd.Playback().Dispose();
        }

        public void Dispose()
        {
            var cmd = new CommandBuffer(world);

            // Execute once with the query, disposing on phantoms
            world.Execute<DisposalQuery, TC>(new DisposalQuery(cmd), _query);

            // Execute again without the query, disposing even on live entities
            world.Execute<DisposalQuery, TC>(new DisposalQuery(cmd));

            cmd.Playback().Dispose();
        }

        private readonly struct DisposalQuery(CommandBuffer cmd)
            : IQuery1<TC>
        {
            public void Execute(Entity e, ref TC disposable)
            {
                disposable.Dispose();
                cmd.Remove<TC>(e);
            }
        }
    }
}
