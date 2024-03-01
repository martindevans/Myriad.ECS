namespace ParallelTasks;

/// <summary>
/// A struct containing options about how an IWork instance can be executed.
/// </summary>
/// <param name="MaximumThreads">The maximum number of threads which can concurrently execute this work.</param>
public readonly record struct WorkOptions(int MaximumThreads = 1);