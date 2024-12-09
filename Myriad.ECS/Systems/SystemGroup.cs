namespace Myriad.ECS.Systems;

/// <summary>
/// Execute all systems in order
/// </summary>
/// <typeparam name="TData"></typeparam>
/// <param name="name"></param>
/// <param name="systems"></param>
public sealed class SystemGroup<TData>(string name, params ISystem<TData>[] systems)
    : BaseSystemGroup<TData>(name, systems)
{
    /// <inheritdoc />
    protected override void BeforeUpdateInternal(List<SystemGroupItem<TData>> systems, TData data)
    {
        foreach (var item in systems)
            item.BeforeUpdate(data);
    }

    /// <inheritdoc />
    protected override void UpdateInternal(List<SystemGroupItem<TData>> systems, TData data)
    {
        foreach (var item in systems)
            item.Update(data);
    }

    /// <inheritdoc />
    protected override void AfterUpdateInternal(List<SystemGroupItem<TData>> systems, TData data)
    {
        foreach (var item in systems)
            item.AfterUpdate(data);
    }
}

/// <summary>
/// Execute all systems in order, allows adding new systems to the group
/// </summary>
/// <typeparam name="TData"></typeparam>
/// <param name="name"></param>
/// <param name="systems"></param>
public sealed class DynamicSystemGroup<TData>(string name, params ISystem<TData>[] systems)
    : BaseSystemGroup<TData>(name, systems)
{
    /// <inheritdoc />
    protected override void BeforeUpdateInternal(List<SystemGroupItem<TData>> systems, TData data)
    {
        foreach (var item in systems)
            item.BeforeUpdate(data);
    }

    /// <inheritdoc />
    protected override void UpdateInternal(List<SystemGroupItem<TData>> systems, TData data)
    {
        foreach (var item in systems)
            item.Update(data);
    }

    /// <inheritdoc />
    protected override void AfterUpdateInternal(List<SystemGroupItem<TData>> systems, TData data)
    {
        foreach (var item in systems)
            item.AfterUpdate(data);
    }

    /// <summary>
    /// Add a new system to the end of this group
    /// </summary>
    /// <param name="system"></param>
    /// <returns></returns>
    public new SystemGroupItem<TData> Add(ISystem<TData> system)
    {
        return base.Add(system);
    }

    /// <summary>
    /// Remove a system from this group
    /// </summary>
    /// <param name="system"></param>
    /// <returns></returns>
    public new bool Remove(ISystem<TData> system)
    {
        return base.Remove(system);
    }
}