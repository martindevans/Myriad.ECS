namespace Myriad.ECS.Threading;

/// <summary>
/// Use the dotnet <see cref="ThreadPool"/>
/// </summary>
public class DefaultThreadPool
    : IThreadPool
{
    /// <inheritdoc />
    public int Threads { get; } = Math.Max(4, Math.Min(64, Environment.ProcessorCount) - 3);

    /// <inheritdoc />
    public void QueueUserWorkItem(IThreadPoolWork callback)
    {
#if NET8_0_OR_GREATER
        ThreadPool.UnsafeQueueUserWorkItem(callback, false);
#else
        ThreadPool.UnsafeQueueUserWorkItem(_ => callback.Execute(), null);
#endif
    }
}