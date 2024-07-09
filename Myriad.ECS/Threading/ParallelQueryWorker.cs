using System.Collections.Concurrent;
using Myriad.ECS.Queries;

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

            var rng = new ValueRandom(Array.IndexOf(siblings, this));

            while (!counter.IsSet)
            {
                // Process the entire local queue
                while (_work.TryDequeue(out var work))
                    DoWorkItem(counter, ref work);

                // Do a few rounds of trying to steal work off siblings.
                // Break out of the loop if there is any local work to do, or if the counter
                // is set (indicating there is no more work available anywhere).
                for (var i = 0; i < siblings.Length && _work.IsEmpty && !counter.IsSet; i++)
                {
                    // Choose a random sibling. This prevents all workers starting from the first
                    // worker every time, which would cause unnecessary contention and bias the system
                    // to drain those queues first.
                    var idx = Math.Abs(rng.Next()) % siblings.Length;
                    var sibling = siblings[idx];

                    // Steal work
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