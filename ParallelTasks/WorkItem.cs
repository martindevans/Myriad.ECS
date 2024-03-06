using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace ParallelTasks;

internal class WorkItem
{
    // MartinG@DigitalRune: I replaced the SpinLocks in this class with normal locks. The 
    // SpinLocks could cause severe problems where threads are blocked up to several milliseconds. 
    // (This behavior was extremely hard to reproduce.)

    // In my applications I often use nested parallel for-loops and need to keep track of all 
    // replicable tasks, not just the most recent. Otherwise, threads can run out of work, 
    // although there are still replicable tasks left. I store the replicable tasks in a stack 
    // (the most recent task on top). - Not sure if this makes sense in all cases.
    private static readonly Stack<Task> replicables = new();
    private static readonly object replicablesLock = new();
    private static Task? topReplicable;

    [DisallowNull]
    internal static Task? Replicable
    {
        get
        {
            var taken = false;
            try
            {
                taken = Monitor.TryEnter(replicablesLock);
                if (taken)
                {
                    return topReplicable;
                }
                else
                {
                    return null;
                }
            }
            finally
            {
                if (taken)
                    Monitor.Exit(replicablesLock);
            }
        }
        set
        {
            lock (replicablesLock)
            {
                replicables.Push(value.Value);
                topReplicable = value.Value;
            }
        }
    }


    internal static void SetReplicableNull(Task? task)
    {
        if (!topReplicable.HasValue)
        {
            return;
        }

        if (!task.HasValue)
        {
            // SetReplicableNull(null) can be called to clear all replicables.
            lock (replicablesLock)
            {
                replicables.Clear();
                topReplicable = null;
            }
        }
        else
        {
            // When called for a specific task, the task is removed from the stack if it is the 
            // top item. (If it is not the top item or we don't get the lock ignore it.)       
            var taken = false;
            try
            {
                taken = Monitor.TryEnter(replicablesLock);
                if (taken)
                {
                    if (replicables.Count > 0)
                    {
                        var replicable = replicables.Peek();
                        if (replicable.ID == task.Value.ID && replicable.Item == task.Value.Item)
                            replicables.Pop();

                        if (replicables.Count > 0)
                            topReplicable = replicables.Peek();
                        else
                            topReplicable = null;
                    }
                }
            }
            finally
            {
                if (taken)
                    Monitor.Exit(replicablesLock);
            }
        }
    }

    private List<Exception>? exceptionBuffer;
    private readonly ManualResetEvent _resetEvent = new(true);
    private volatile int runCount;
    private volatile int executing;
    private volatile int waitCount;
    private readonly object _executionLock = new();

    private static readonly Pool<WorkItem> _idleWorkItems = Pool<WorkItem>.Instance;
    private static readonly ConcurrentDictionary<Thread, Stack<Task>> _runningTasks = new();


    public int RunCount => runCount;

    public ConcurrentDictionary<int, Exception[]> Exceptions { get; } = new();
    public IWork? Work { get; private set; }

    public static Task? CurrentTask
    {
        get
        {
            if (_runningTasks.TryGetValue(Thread.CurrentThread, out var tasks))
            {
                if (tasks.Count > 0)
                    return tasks.Peek();
            }
            return null;
        }
    }

    public Task PrepareStart(IWork work)
    {
        Work = work;
        _resetEvent.Reset();
        exceptionBuffer = null;

        var task = new Task(this);
        var currentTask = CurrentTask;
        if (currentTask.HasValue && currentTask.Value.Item == this)
            throw new InvalidOperationException("Task does not have an assigned WorkItem");

        return task;
    }

    public bool DoWork(int expectedID)
    {
        lock (_executionLock)
        {
            if (expectedID < runCount)
                return true;
            if (executing == Work.Options.MaximumThreads)
                return false;
            Interlocked.Increment(ref executing);
        }

        // associate the current task with this thread, so that Task.CurrentTask gives the correct result
        if (!_runningTasks.TryGetValue(Thread.CurrentThread, out var tasks))
        {
            tasks = new Stack<Task>();
            _runningTasks[Thread.CurrentThread] = tasks;
        }
        tasks.Push(new Task(this));

        // execute the task
        try { Work.DoWork(); }
        catch (Exception e)
        {
            if (exceptionBuffer == null)
            {
                var newExceptions = new List<Exception>();
                Interlocked.CompareExchange(ref exceptionBuffer, newExceptions, null);
            }

            lock (exceptionBuffer)
                exceptionBuffer.Add(e);
        }

        if (tasks != null)
            tasks.Pop();

        lock (_executionLock)
        {
            Interlocked.Decrement(ref executing);

            if (executing == 0)
            {
                if (exceptionBuffer != null)
                    Exceptions[runCount] = [..exceptionBuffer];

                Interlocked.Increment(ref runCount);

                // open the reset event, so tasks waiting on this once can continue
                _resetEvent.Set();

                // wait for waiting tasks to all exit
                while (waitCount > 0) ;

                Requeue();

                return true;
            }
            return false;
        }

    }

    private void Requeue()
    {
        // requeue the WorkItem for reuse, but only if the runCount hasnt reached the maximum value
        // dont requeue if an exception has been caught, to stop potential memory leaks.
        if (runCount < int.MaxValue && exceptionBuffer == null)
            _idleWorkItems.Return(this);
    }

    public void Wait(int id)
    {
        WaitOrExecute(id);

        if (Exceptions.TryGetValue(id, out var e))
            throw new TaskException(e);
    }

    private void WaitOrExecute(int id)
    {
        if (runCount != id)
            return;

        if (DoWork(id))
            return;

        try
        {
            Interlocked.Increment(ref waitCount);
            var i = 0;
            while (runCount == id)
            {
                if (i > 1000)
                    _resetEvent.WaitOne();
                else
                    Thread.Sleep(0);
                i++;
            }
        }
        finally
        {
            Interlocked.Decrement(ref waitCount);
        }
    }

    public static WorkItem Get()
    {
        return _idleWorkItems.Get();
    }
}