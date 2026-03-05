using Myriad.ECS.Command;
using Myriad.ECS.Worlds;

namespace Myriad.ECS.Systems;

/// <summary>
/// Callback when a commandbuffer system has been executed
/// </summary>
public interface ICommandBufferSystemSubscriber
{
    /// <summary>
    /// Called immediately after a commandbuffer has been executed
    /// </summary>
    /// <param name="resolver"></param>
    void OnCommandbufferPlayback(CommandBuffer.Resolver resolver);
}

/// <summary>
/// Base system for all command buffer systems. Contains a <see cref="CommandBuffer"/> which will be executed at some point.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class BaseCommandBufferSystem<T>
    : ISystem<T>
{
    private readonly List<ICommandBufferSystemSubscriber> _subscribers = [];

    /// <summary>
    /// Get the resolver from the previous playback.
    /// </summary>
    public CommandBuffer.Resolver Resolver { get; private set; }

    /// <summary>
    /// The CommandBuffer which will be executed.
    /// </summary>
    public CommandBuffer Buffer { get; }

    /// <summary>
    /// The world this system is bound to
    /// </summary>
    public World World { get; }

    /// <summary>
    /// Create a new <see cref="CommandBufferSystem{TData}"/>
    /// </summary>
    /// <param name="world"></param>
    protected BaseCommandBufferSystem(World world)
    {
        World = world;
        Buffer = new CommandBuffer(World);

        // Playback the buffer immediately to get a empty resolver
        Resolver = Buffer.Playback();
    }

    /// <summary>
    /// Play this buffer back (invalidating the previous resolver)
    /// </summary>
    protected void Playback()
    {
        Resolver.Dispose();
        Resolver = Buffer.Playback();

        foreach (var subscriber in _subscribers)
            subscriber.OnCommandbufferPlayback(Resolver);
    }

    /// <summary>
    /// Add a new subscriber to receive a callback when the buffer is executed
    /// </summary>
    /// <param name="subscriber"></param>
    public void Subscribe(ICommandBufferSystemSubscriber subscriber)
    {
        _subscribers.Add(subscriber);
    }

    /// <summary>
    /// Remove a subscriber
    /// </summary>
    /// <param name="subscriber"></param>
    public bool Unsubscribe(ICommandBufferSystemSubscriber subscriber)
    {
        return _subscribers.Remove(subscriber);
    }

    /// <inheritdoc />
    public virtual void Update(T data)
    {
    }
}

/// <summary>
/// Executes a command buffer in Update.
/// </summary>
/// <typeparam name="T"></typeparam>
public class CommandBufferSystem<T>
    : BaseCommandBufferSystem<T>
{
    /// <summary>
    /// Create a new <see cref="CommandBufferSystem{T}"/>
    /// </summary>
    /// <param name="world"></param>
    public CommandBufferSystem(World world)
        : base(world)
    {
    }

    /// <inheritdoc />
    public override void Update(T time)
    {
        Playback();
    }
}

/// <summary>
/// Executes a command buffer in BeforeUpdate.
/// </summary>
/// <typeparam name="T"></typeparam>
public class EarlyCommandBufferSystem<T>
    : BaseCommandBufferSystem<T>, ISystemBefore<T>
{
    /// <summary>
    /// Create a new <see cref="EarlyCommandBufferSystem{T}"/>
    /// </summary>
    /// <param name="world"></param>
    public EarlyCommandBufferSystem(World world)
        : base(world)
    {
    }

    /// <inheritdoc />
    public void BeforeUpdate(T data)
    {
        Playback();
    }
}

/// <summary>
/// Executes a command buffer in AfterUpdate.
/// </summary>
/// <typeparam name="T"></typeparam>
public class LateCommandBufferSystem<T>
    : BaseCommandBufferSystem<T>, ISystemAfter<T>
{
    /// <summary>
    /// Create a new <see cref="LateCommandBufferSystem{T}"/>
    /// </summary>
    /// <param name="world"></param>
    public LateCommandBufferSystem(World world)
        : base(world)
    {
    }

    /// <inheritdoc />
    public void AfterUpdate(T data)
    {
        Playback();
    }
}