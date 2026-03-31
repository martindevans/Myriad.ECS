namespace Myriad.ECS.Systems;

/// <summary>
/// A system that does nothing every frame, owns some <see cref="IDisposable"/> obejcts and will dispose them when the systemis disposed
/// </summary>
public sealed class CleanupSystem<T>
    : ISystem<T>, IDisposable
{
    private readonly IDisposable?[] _disposables;

    /// <summary>
    /// Create a new <see cref="CleanupSystem{T}"/> that will dispose all given disposables when the system is disposed
    /// </summary>
    /// <param name="disposables"></param>
    public CleanupSystem(params IDisposable[] disposables)
    {
        _disposables = disposables;
    }

    /// <summary>
    /// Does nithing.
    /// </summary>
    /// <param name="data"></param>
    public void Update(T data)
    {
    }

    /// <inheritdoc />
    public void Dispose()
    {
        foreach (var disposable in _disposables)
            disposable?.Dispose();
        Array.Clear(_disposables);
    }
}