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
    public class DisposableComponentSystem<T, TC>(World world)
        : ISystemAfter<T>
        where TC : IDisposableComponent
    {
        public bool Enabled { get; set; }

        private readonly CommandBuffer _cmd = new(world);

        public void Update(T time)
        {
        }

        public void AfterUpdate(T data)
        {
            world.Execute<DisposalQuery, Phantom, TC>(new DisposalQuery(_cmd));
            _cmd.Playback().Dispose();
        }

        private readonly struct DisposalQuery(CommandBuffer cmd)
            : IQuery2<Phantom, TC>
        {
            public void Execute(Entity e, ref Phantom _, ref TC disposable)
            {
                disposable.Dispose();
                cmd.Remove<TC>(e);
            }
        }
    }
}
