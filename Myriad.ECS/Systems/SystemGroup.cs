namespace Myriad.ECS.Systems;

public sealed class SystemGroup<TData>(string name, params ISystem<TData>[] systems)
    : BaseSystemGroup<TData>(name, systems)
{
    protected override void BeforeUpdateInternal(List<SystemGroupItem<TData>> systems, TData data)
    {
        foreach (var item in systems)
            item.BeforeUpdate(data);
    }

    protected override void UpdateInternal(List<SystemGroupItem<TData>> systems, TData data)
    {
        foreach (var item in systems)
            item.Update(data);
    }

    protected override void AfterUpdateInternal(List<SystemGroupItem<TData>> systems, TData data)
    {
        foreach (var item in systems)
            item.AfterUpdate(data);
    }
}

public sealed class DynamicSystemGroup<TData>(string name, params ISystem<TData>[] systems)
    : BaseSystemGroup<TData>(name, systems)
{
    protected override void BeforeUpdateInternal(List<SystemGroupItem<TData>> systems, TData data)
    {
        foreach (var item in systems)
            item.BeforeUpdate(data);
    }

    protected override void UpdateInternal(List<SystemGroupItem<TData>> systems, TData data)
    {
        foreach (var item in systems)
            item.Update(data);
    }

    protected override void AfterUpdateInternal(List<SystemGroupItem<TData>> systems, TData data)
    {
        foreach (var item in systems)
            item.AfterUpdate(data);
    }

    public new SystemGroupItem<TData> Add(ISystem<TData> system)
    {
        return base.Add(system);
    }
}