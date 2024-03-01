namespace ParallelTasks;

/// <summary>
/// A "work stealing" work scheduler class.
/// </summary>
internal class WorkStealingScheduler
{
    internal List<Worker> Workers { get; }

    private int _tasksCount;
    private readonly Queue<Task> _tasks;

    /// <summary>
    /// Creates a new instance of the <see cref="WorkStealingScheduler"/> class.
    /// </summary>
    public WorkStealingScheduler()
        : this(Environment.ProcessorCount)
    {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="WorkStealingScheduler"/> class.
    /// </summary>
    /// <param name="numThreads">The number of threads to create.</param>
    private WorkStealingScheduler(int numThreads)
    {
        _tasks = new Queue<Task>();
        _tasksCount = 0;

        Workers = new List<Worker>(numThreads);
        for (var i = 0; i < numThreads; i++)
            Workers.Add(new Worker(this, i));

        for (var i = 0; i < numThreads; i++)
        {
            Workers[i].Start();
        }
    }

    internal bool TryGetTask(out Task task)
    {
        if (_tasksCount == 0)
        {
            task = default;
            return false;
        }

        lock (_tasks)
        {
            if (_tasks.Count > 0)
            {
                task = _tasks.Dequeue();
                _tasksCount--;
                return true;
            }

            task = default;
            return false;
        }
    }

    /// <summary>
    /// Schedules a task for execution.
    /// </summary>
    /// <param name="task">The task to schedule.</param>
    public void Schedule(Task task)
    {
        var threads = task.Item.Work.Options.MaximumThreads;
        var worker = Worker.CurrentWorker;
        if (worker != null)
        {
            worker.AddWork(task);
        }
        else
        {
            lock (_tasks)
            {
                _tasks.Enqueue(task);
                _tasksCount++;
            }
        }

        if (threads > 1)
            WorkItem.Replicable = task;

        for (var i = 0; i < Workers.Count; i++)
        {
            Workers[i].Gate.Set();
        }
    }
}