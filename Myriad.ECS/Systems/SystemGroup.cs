namespace Myriad.ECS.Systems;

public sealed class SystemGroup<TData>(string name, params ISystem<TData>[] systems)
    : BaseSystemGroup<TData>(name, systems)
{
    protected override void BeforeUpdateInternal(SystemGroupItem<TData>[] systems, TData data)
    {
        foreach (var item in systems)
            item.BeforeUpdate(data);
    }

    protected override void UpdateInternal(SystemGroupItem<TData>[] systems, TData data)
    {
        foreach (var item in systems)
            item.Update(data);
    }

    protected override void AfterUpdateInternal(SystemGroupItem<TData>[] systems, TData data)
    {
        foreach (var item in systems)
            item.AfterUpdate(data);
    }
}