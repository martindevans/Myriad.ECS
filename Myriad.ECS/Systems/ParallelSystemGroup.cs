namespace Myriad.ECS.Systems;

/// <summary>
/// Executes all systems in the group in parallel, with no safety assurances of any kind.
/// </summary>
/// <typeparam name="TData"></typeparam>
public sealed class ParallelSystemGroup<TData>
    : BaseSystemGroup<TData>
{
    private TData? _dataClosure;
    private readonly Action<SystemGroupItem<TData>> _beforeUpdate;
    private readonly Action<SystemGroupItem<TData>> _update;
    private readonly Action<SystemGroupItem<TData>> _afterUpdate;

    /// <inheritdoc />
    public ParallelSystemGroup(string name, params ISystem<TData>[] systems)
        : base(name, systems)
    {
        // Create delegates now, capturing "this._dataClosure". This avoids creating new delegates every frame.
        _dataClosure = default;
        _beforeUpdate = system =>
        {
            system.BeforeUpdate(_dataClosure!);
        };
        _update = system =>
        {
            system.Update(_dataClosure!);
        };
        _afterUpdate = system =>
        {
            system.Update(_dataClosure!);
        };
    }

    /// <inheritdoc />
    protected override void BeforeUpdateInternal(List<SystemGroupItem<TData>> systems, TData data)
    {
        if (systems.Count > 0)
        {
            _dataClosure = data;
            Parallel.ForEach(systems, _beforeUpdate);
            _dataClosure = default;
        }
    }

    /// <inheritdoc />
    protected override void UpdateInternal(List<SystemGroupItem<TData>> systems, TData data)
    {
        if (systems.Count > 0)
        {
            _dataClosure = data;
            Parallel.ForEach(systems, _update);
            _dataClosure = default;
        }
    }

    /// <inheritdoc />
    protected override void AfterUpdateInternal(List<SystemGroupItem<TData>> systems, TData data)
    {
        if (systems.Count > 0)
        {
            _dataClosure = data;
            Parallel.ForEach(systems, _afterUpdate);
            _dataClosure = default;
        }
    }
}