using Myriad.ECS.Command;
using Myriad.ECS.Worlds;

namespace Myriad.ECS.Systems;

/// <summary>
/// Executes a command buffer in Update.
/// </summary>
/// <typeparam name="T"></typeparam>
public class CommandBufferSystem<T>
    : ISystem<T>
{
    /// <summary>
    /// Get the resolver from the previous playback.
    /// </summary>
    public CommandBuffer.Resolver Resolver { get; private set; }

    /// <summary>
    /// The CommandBuffer which will be executed in the next Update tick.
    /// </summary>
    public CommandBuffer Buffer { get; }

    /// <summary>
    /// The world this system is bound to
    /// </summary>
    public World World { get; }

    public CommandBufferSystem(World world)
    {
        World = world;
        Buffer = new CommandBuffer(World);

        // Playback the buffer immediately to get a empty resolver
        Resolver = Buffer.Playback();
    }

    public void Update(T time)
    {
        Resolver.Dispose();
        Resolver = Buffer.Playback();
    }
}

/// <summary>
/// Executes a command buffer in BeforeUpdate.
/// </summary>
/// <typeparam name="T"></typeparam>
public class EarlyCommandBufferSystem<T>
    : ISystem<T>, ISystemBefore<T>
{
    /// <summary>
    /// Get the resolver from the previous playback.
    /// </summary>
    public CommandBuffer.Resolver Resolver { get; private set; }

    /// <summary>
    /// The CommandBuffer which will be executed in the next Update tick.
    /// </summary>
    public CommandBuffer Buffer { get; }

    /// <summary>
    /// The world this system is bound to
    /// </summary>
    public World World { get; }

    public EarlyCommandBufferSystem(World world)
    {
        World = world;
        Buffer = new CommandBuffer(World);

        // Playback the buffer immediately to get a empty resolver
        Resolver = Buffer.Playback();
    }

    public void BeforeUpdate(T data)
    {
        Resolver.Dispose();
        Resolver = Buffer.Playback();
    }

    public void Update(T time)
    {
    }
}

/// <summary>
/// Executes a command buffer in AfterUpdate.
/// </summary>
/// <typeparam name="T"></typeparam>
public class LateCommandBufferSystem<T>
    : ISystem<T>, ISystemAfter<T>
{
    /// <summary>
    /// Get the resolver from the previous playback.
    /// </summary>
    public CommandBuffer.Resolver Resolver { get; private set; }

    /// <summary>
    /// The CommandBuffer which will be executed in the next Update tick.
    /// </summary>
    public CommandBuffer Buffer { get; }

    /// <summary>
    /// The world this system is bound to
    /// </summary>
    public World World { get; }

    public LateCommandBufferSystem(World world)
    {
        World = world;
        Buffer = new CommandBuffer(World);

        // Playback the buffer immediately to get a empty resolver
        Resolver = Buffer.Playback();
    }

    public void Update(T time)
    {
    }

    public void AfterUpdate(T data)
    {
        Resolver.Dispose();
        Resolver = Buffer.Playback();
    }
}