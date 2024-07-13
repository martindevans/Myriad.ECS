namespace Myriad.ECS.Threading;

/// <summary>
/// A threadpool provides a way to queue work on other threads
/// </summary>
public interface IThreadPool
{
    /// <summary>
    /// Get how many workers threads should be scheduled
    /// </summary>
    int Threads { get; }

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