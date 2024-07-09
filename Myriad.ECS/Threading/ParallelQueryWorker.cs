using System.Collections.Concurrent;

namespace Myriad.ECS.Threading;

internal class ParallelQueryWorker<TWork>
#if NET8_0_OR_GREATER
    : IThreadPoolWorkItem
#endif
    where TWork : struct, IWorkItem
{
    private ParallelQueryWorker<TWork>?[]? _siblings;
    private CountdownEvent? _counter;

    private ConcurrentQueue<TWork> _work { get; } = new();

    public ManualResetEventSlim FinishEvent { get; } = new ManualResetEventSlim(false);

    public void Configure(ParallelQueryWorker<TWork>?[] siblings, CountdownEvent counter)
    {
        _siblings = siblings;
        _counter = counter;
        FinishEvent.Reset();
    }

    public void Clear()
    {
        _counter = null;
        _siblings = null;
        _work.Clear();
        FinishEvent.Reset();
    }

    public void Execute(object? state)
    {
        Execute();
    }

    public void Execute()
    {
        try
        {
            var counter = _counter;
            var siblings = _siblings;
            if (counter == null || siblings == null)
                throw new InvalidOperationException("Cannot execute work - worker not configured");

            while (!counter.IsSet || _work.Count > 0)
            {
                // Process the entire local queue
                while (_work.TryDequeue(out var work))
                    DoWorkItem(counter, ref work);

                // Try to steal work off siblings
                foreach (var sibling in siblings)
                {
                    if (sibling == null || !sibling.Steal(out var work))
                        continue;
                    DoWorkItem(counter, ref work);
                }
            }
        }
        finally
        {
            FinishEvent.Set();
        }
    }

    private void DoWorkItem(CountdownEvent counter, ref TWork work)
    {
        counter.Signal();
        work.Execute();
    }

    public void Enqueue(TWork work)
    {
        _work.Enqueue(work);
    }

    public bool Steal(out TWork result)
    {
        return _work.TryDequeue(out result);
    }
}

internal interface IWorkItem
{
    void Execute();
}