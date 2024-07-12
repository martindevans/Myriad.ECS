namespace Myriad.ECS.Threading;

/// <summary>
/// A threadpool provides a way to queue work on other threads
/// </summary>
public interface IThreadPool
{
    /// <summary>
    /// Queue a delegate to be called on another thread
    /// </summary>
    /// <param name="callback"></param>
    void QueueUserWorkItem(IThreadPoolWork callback);
}

public interface IThreadPoolWork
#if NET8_0_OR_GREATER
    : IThreadPoolWorkItem
#endif
{
#if !NET8_0_OR_GREATER
    void Execute();
#endif
}

/// <summary>
/// Use the dotnet <see cref="ThreadPool"/>
/// </summary>
public class DefaultThreadPool
    : IThreadPool
{
    public void QueueUserWorkItem(IThreadPoolWork callback)
    {
#if NET8_0_OR_GREATER
        ThreadPool.UnsafeQueueUserWorkItem(callback, false);
#else
        ThreadPool.UnsafeQueueUserWorkItem(_ => callback.Execute(), null);
#endif
    }
}