namespace ParallelTasks;

internal class ForLoopWork
    : IWork
{
    private Action<int>? _action;

    private int _length;
    private int _stride;
    private volatile int _index;

    public WorkOptions Options => new() { MaximumThreads = int.MaxValue };

    public void Prepare(Action<int> action, int startInclusive, int endExclusive, int stride)
    {
        _action = action;
        _index = startInclusive;
        _length = endExclusive;
        _stride = stride;
    }

    public void DoWork()
    {
        int start;
        while ((start = IncrementIndex()) < _length)
        {
            var end = Math.Min(start + _stride, _length);
            for (var i = start; i < end; i++)
            {
                _action!(i);
            }
        }
    }

    private int IncrementIndex()
    {
        return Interlocked.Add(ref _index, _stride) - _stride;
    }

    public static ForLoopWork Get()
    {
        return Pool<ForLoopWork>.Instance.Get();
    }

    public void Return()
    {
        Pool<ForLoopWork>.Instance.Return(this);
    }
}

internal class ForEachLoopWork<T>
    : IWork
{
    private Action<T>? _action;
    private IEnumerator<T>? _enumerator;

    private volatile bool _notDone;
    private readonly object _syncLock = new();

    public WorkOptions Options => new() { MaximumThreads = int.MaxValue };

    public void Prepare(Action<T> action, IEnumerator<T> enumerator)
    {
        _action = action;
        _enumerator = enumerator;
        _notDone = true;
    }

    public void DoWork()
    {
        var item = default(T);
        while (_notDone)
        {
            var haveValue = false;
            lock (_syncLock)
            {
                // ReSharper disable once AssignmentInConditionalExpression
                if (_notDone = _enumerator!.MoveNext())
                {
                    item = _enumerator.Current;
                    haveValue = true;
                }
            }

            if (haveValue)
                _action!(item!);
        }
    }

    public static ForEachLoopWork<T> Get()
    {
        return Pool<ForEachLoopWork<T>>.Instance.Get();
    }

    public void Return()
    {
        Pool<ForEachLoopWork<T>>.Instance.Return(this);
    }
}