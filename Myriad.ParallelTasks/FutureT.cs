namespace Myriad.ParallelTasks;

public struct Future<T>
{
    private readonly T _result;

    public Future(T result)
    {
        _result = result;
    }

    public T Block()
    {
        return _result;
    }
}