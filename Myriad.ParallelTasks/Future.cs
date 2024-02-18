namespace Myriad.ParallelTasks;

public struct Future
{
    public void Block()
    {
        throw new NotImplementedException();
    }

    public static Future Combine(Future a, Future b)
    {
        throw new NotImplementedException();
    }

    public static Future Combine<T>(Future<T> a, Future b)
    {
        throw new NotImplementedException();
    }

    public static Future Combine<T>(Future a, Future<T> b)
    {
        throw new NotImplementedException();
    }
}

public readonly struct Future<T>
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

    public static Future<T> Combine(Future<T> a, Future b)
    {
        throw new NotImplementedException();
    }

    public static Future<T> Combine(Future a, Future<T> b)
    {
        throw new NotImplementedException();
    }
}