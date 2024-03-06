namespace ParallelTasks;

/// <summary>
/// A task struct which can return a result.
/// </summary>
/// <typeparam name="T">The type of result this future calculates.</typeparam>
public readonly struct Future<T>
{
    private readonly Task _task;
    private readonly FutureWork<T>? _work;
    private readonly int _id;

    /// <summary>
    /// Gets a value which indicates if this future has completed.
    /// </summary>
    public bool IsComplete => _task.IsComplete;

    /// <summary>
    /// Gets an array containing any exceptions thrown by this future.
    /// </summary>
    public Exception[] Exceptions => _task.Exceptions ?? [ ];

    internal Future(Task task, FutureWork<T> work)
    {
        _task = task;
        _work = work;
        _id = work.ID;
    }

    /// <summary>
    /// Gets the result. Blocks the calling thread until the future has completed execution.
    /// This can only be called once!
    /// </summary>
    /// <returns></returns>
    public T GetResult()
    {
        if (_work == null || _work.ID != _id)
            throw new InvalidOperationException("The result of a future can only be retrieved once.");

        _task.Wait();
        var result = _work.Result!;
        _work.ReturnToPool();

        return result;
    }
}

internal class FutureWork<T>
    : IWork
{
    public int ID { get; private set; }
    public WorkOptions Options { get; set; }

    public Func<T>? Function { get; set; }
    public T? Result { get; private set; }

    public void DoWork()
    {
        Result = Function!();            
    }

    public static FutureWork<T> GetInstance()
    {
        return Pool<FutureWork<T>>.Instance.Get();
    }

    public void ReturnToPool()
    {
        // Always increment ID, to invalidate any references to this work item
        ID++;

        // Only add it to the pool if it's not near overflowing
        if (ID < int.MaxValue - 10)
        {
            Function = null;
            Result = default;

            Pool<FutureWork<T>>.Instance.Return(this);
        }
    }
}