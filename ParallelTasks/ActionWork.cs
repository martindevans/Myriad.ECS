namespace ParallelTasks;

internal class ActionWork
    : IWork
{
    private static readonly Pool<ActionWork> instances = new();

    public Action? Action { get; set; }
    public WorkOptions Options { get; set; }

    public void DoWork()
    {
        Action?.Invoke();
        Action = null;

        instances.Return(this);
    }

    internal static ActionWork GetInstance()
    {
        return instances.Get();
    }
}