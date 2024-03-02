using System.Collections.Concurrent;

namespace ParallelTasks;

internal class Worker
{
    private readonly Thread _thread;
    private readonly ConcurrentBag<Task> _tasks = [ ];
    private readonly WorkStealingScheduler _scheduler;

    public AutoResetEvent Gate { get; } = new AutoResetEvent(false);

    private static readonly ConcurrentDictionary<Thread, Worker> _workers = new();

    public static Worker? CurrentWorker
    {
        get
        {
            var currentThread = Thread.CurrentThread;
            return _workers.GetValueOrDefault(currentThread);
        }
    }

    public Worker(WorkStealingScheduler scheduler, int index)
    {
        _thread = new Thread(Work)
        {
            Name = "ParallelTasks Worker " + index,
            IsBackground = true,
        };

        _scheduler = scheduler;

        _workers[_thread] = this;
    }

    public void Start()
    {
        _thread.Start();
    }

    public void AddWork(Task task)
    {
        _tasks.Add(task);
    }

    private void Work()
    {
        while (true)
        {
            if (_tasks.TryTake(out var task))
                task.DoWork();
            else
                FindWork();
        }

        // ReSharper disable once FunctionNeverReturns
    }

    private void FindWork()
    {
        Task task;
        var foundWork = false;
        do
        {
            if (_scheduler.TryGetTask(out task))
                break;

            var replicable = WorkItem.Replicable;
            if (replicable.HasValue)
            {
                replicable.Value.DoWork();
                WorkItem.SetReplicableNull(replicable);

                // MartinG@DigitalRune: Continue checking local queue and replicables. 
                // No need to steal work yet.
                continue;
            }

            for (var i = 0; i < _scheduler.Workers.Count; i++)
            {
                var worker = _scheduler.Workers[i];
                if (worker == this)
                    continue;

                if (worker._tasks.TryTake(out task))
                {
                    foundWork = true;
                    break;
                }
            }

            if (!foundWork)
            {
                // Wait until a new task gets scheduled.
                Gate.WaitOne();
            }
        } while (!foundWork);

        _tasks.Add(task);
    }
}