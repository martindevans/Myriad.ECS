using Myriad.ECS.Command;
using Myriad.ECS.Worlds;

namespace Myriad.ECS.Systems;

/// <summary>
/// Executes a command buffer in Update.
/// Resolver is available to be accessed in AfterUpdate, and is disposed in EarlyUpdate.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="world"></param>
public class CommandBufferSystem<T>(World world)
    : ISystem<T>, ISystemBefore<T>
{
    private CommandBuffer.Resolver? _resolver;

    /// <summary>
    /// Get the resolver from the previous playback. This should only be accessed in
    /// AfterUpdate, and will throw an exception in other cases.
    /// </summary>
    public CommandBuffer.Resolver Resolver
    {
        get
        {
            if (_resolver == null)
                throw new InvalidOperationException("Resolver is null. Must only access resolver in AfterUpdate");
            return _resolver;
        }
    }

    /// <summary>
    /// The CommandBuffer which will be executed in the next Update tick.
    /// </summary>
    public CommandBuffer Buffer { get; } = new(world);

    public bool Enabled { get; set; }

    public void BeforeUpdate(T data)
    {
        _resolver?.Dispose();
        _resolver = null;
    }

    public void Update(T time)
    {
        _resolver = Buffer.Playback();
    }
}