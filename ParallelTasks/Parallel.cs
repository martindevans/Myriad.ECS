namespace ParallelTasks;

/// <summary>
/// A static class containing factory methods for creating tasks.
/// </summary>
public static class Parallel
{
    /// <summary>
    /// Gets the work scheduler.
    /// </summary>
    private static readonly WorkStealingScheduler _scheduler = new();

    /// <summary>
    /// Creates and starts a task to execute the given work.
    /// </summary>
    /// <param name="work">The work to execute in parallel.</param>
    /// <returns>A task which represents one execution of the work.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="work"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Invalid number of maximum threads set in <see cref="IWork.Options"/>.
    /// </exception>
    public static Task Start(IWork work)
    {
        if (work == null)
            throw new ArgumentNullException(nameof(work));

        if (work.Options.MaximumThreads < 1)
            throw new ArgumentException("work.Options.MaximumThreads cannot be less than one.");

        var workItem = WorkItem.Get();
        var task = workItem.PrepareStart(work);
        _scheduler.Schedule(task);
        return task;
    }

    /// <summary>
    /// Creates and starts a task to execute the given work.
    /// </summary>
    /// <param name="action">The work to execute in parallel.</param>
    /// <returns>A task which represents one execution of the work.</returns>
    public static Task Start(Action action)
    {
        return Start(action, new WorkOptions { MaximumThreads = 1 });
    }

    /// <summary>
    /// Creates and starts a task to execute the given work.
    /// </summary>
    /// <param name="action">The work to execute in parallel.</param>
    /// <param name="options">The work options to use with this action.</param>
    /// <returns>A task which represents one execution of the work.</returns>
    public static Task Start(Action action, WorkOptions options)
    {
        if (options.MaximumThreads < 1)
            throw new ArgumentOutOfRangeException(nameof(options), "options.MaximumThreads cannot be less than 1.");
        var work = ActionWork.GetInstance();
        work.Action = action;
        work.Options = options;
        return Start(work);
    }

    /// <summary>
    /// Creates an starts a task which executes the given function and stores the result for later retrieval.
    /// </summary>
    /// <typeparam name="T">The type of result the function returns.</typeparam>
    /// <param name="function">The function to execute in parallel.</param>
    /// <returns>A future which represults one execution of the function.</returns>
    public static Future<T> Start<T>(Func<T> function)
    {
        return Start(function, new WorkOptions());
    }

    /// <summary>
    /// Creates an starts a task which executes the given function and stores the result for later retrieval.
    /// </summary>
    /// <typeparam name="T">The type of result the function returns.</typeparam>
    /// <param name="function">The function to execute in parallel.</param>
    /// <param name="options">The work options to use with this action.</param>
    /// <returns>A future which represents one execution of the function.</returns>
    public static Future<T> Start<T>(Func<T> function, WorkOptions options)
    {
        if (options.MaximumThreads < 1)
            throw new ArgumentOutOfRangeException(nameof(options), "options.MaximumThreads cannot be less than 1.");
        var work = FutureWork<T>.GetInstance();
        work.Function = function;
        work.Options = options;
        var task = Start(work);
        return new Future<T>(task, work);
    }

    /// <summary>
    /// Executes the given work items potentially in parallel with each other.
    /// This method will block until all work is completed.
    /// </summary>
    /// <param name="a">Work to execute.</param>
    /// <param name="b">Work to execute.</param>
    public static void Do(IWork a, IWork b)
    {
        var task = Start(b);
        a.DoWork();
        task.Wait();
    }

    /// <summary>
    /// Executes the given work items potentially in parallel with each other.
    /// This method will block until all work is completed.
    /// </summary>
    /// <param name="work">The work to execute.</param>
    public static void Do(params IWork[] work)
    {
        var tasks = Pool<List<Task>>.Instance.Get();

        for (var i = 0; i < work.Length; i++)
            tasks.Add(Start(work[i]));

        for (var i = 0; i < tasks.Count; i++)
            tasks[i].Wait();

        tasks.Clear();
        Pool<List<Task>>.Instance.Return(tasks);
    }

    /// <summary>
    /// Executes the given work items potentially in parallel with each other.
    /// This method will block until all work is completed.
    /// </summary>
    /// <param name="action1">The work to execute.</param>
    /// <param name="action2">The work to execute.</param>
    public static void Do(Action action1, Action action2)
    {
        var work = ActionWork.GetInstance();
        work.Action = action2;
        work.Options = new WorkOptions();
        var task = Start(work);
        action1();
        task.Wait();
    }

    /// <summary>
    /// Executes the given work items potentially in parallel with each other.
    /// This method will block until all work is completed.
    /// </summary>
    /// <param name="actions">The work to execute.</param>
    public static void Do(params Action[] actions)
    {
        var tasks = Pool<List<Task>>.Instance.Get();

        for (var i = 0; i < actions.Length; i++)
        {
            var work = ActionWork.GetInstance();
            work.Action = actions[i];
            work.Options = new WorkOptions();
            tasks.Add(Start(work));
        }

        for (var i = 0; i < actions.Length; i++)
        {
            tasks[i].Wait();
        }

        tasks.Clear();
        Pool<List<Task>>.Instance.Return(tasks);
    }

    /// <summary>
    /// Executes a for loop, where each iteration can potentially occur in parallel with others.
    /// </summary>
    /// <param name="startInclusive">The index (inclusive) at which to start iterating.</param>
    /// <param name="endExclusive">The index (exclusive) at which to end iterating.</param>
    /// <param name="body">The method to execute at each iteration. The current index is supplied as the parameter.</param>
    /// <param name="stride">The number of iterations that each processor takes at a time.</param>
    public static void For(int startInclusive, int endExclusive, Action<int> body, int stride = 8)
    {
        var work = ForLoopWork.Get();
        work.Prepare(body, startInclusive, endExclusive, stride);
        var task = Start(work);
        task.Wait();
        work.Return();
    }

    /// <summary>
    /// Executes a foreach loop, where each iteration can potentially occur in parallel with others.
    /// </summary>
    /// <typeparam name="T">The type of item to iterate over.</typeparam>
    /// <param name="collection">The enumerable data source.</param>
    /// <param name="action">The method to execute at each iteration. The item to process is supplied as the parameter.</param>
    public static void ForEach<T>(IEnumerable<T> collection, Action<T> action)
    {
        var work = ForEachLoopWork<T>.Get();
        work.Prepare(action, collection.GetEnumerator());
        var task = Start(work);
        task.Wait();
        work.Return();
    }
}