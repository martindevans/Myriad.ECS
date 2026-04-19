namespace Myriad.ECS.Systems;

/// <summary>
/// A system that does nothing every frame, owns some <see cref="IDisposable"/> objects and will dispose them when the system is disposed
/// </summary>
public sealed class CleanupSystem<TData>
    : ISystem<TData>, IDisposable
{
    private readonly List<IDisposable> _disposables = [ ];

    /// <summary>
    /// Create a new <see cref="CleanupSystem{TData}"/> that will dispose all given disposables when the system is disposed
    /// </summary>
    /// <param name="disposables"></param>
    public CleanupSystem(params IDisposable[] disposables)
        : this(disposables.AsSpan())
    {
    }

    /// <summary>
    /// Create a new <see cref="CleanupSystem{TData}"/> that will dispose all given disposables when the system is disposed
    /// </summary>
    /// <param name="disposables"></param>
    public CleanupSystem(ReadOnlySpan<IDisposable> disposables)
    {
        _disposables.EnsureCapacity(disposables.Length);
        foreach (var disposable in disposables)
            _disposables.Add(disposable);
    }

    /// <summary>
    /// Does nothing.
    /// </summary>
    /// <param name="data"></param>
    public void Update(TData data)
    {
    }

    /// <inheritdoc />
    public void Dispose()
    {
        foreach (var disposable in _disposables)
            disposable?.Dispose();
        _disposables.Clear();
    }
}