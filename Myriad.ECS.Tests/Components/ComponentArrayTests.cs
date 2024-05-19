using Myriad.ECS.Command;
using Myriad.ECS.Components;
using Myriad.ECS.Worlds;

namespace Myriad.ECS.Tests.Components;

[TestClass]
public class ComponentArrayTests
{
    [TestMethod]
    public void GetAndMutateArray()
    {
        var w = new WorldBuilder().Build();
        var b = new CommandBuffer(w);

        var e = b.Create()
                 .Set(ComponentArray<int>.Create(7, 0));
        using var resolver = b.Playback();
        var entity = resolver.Resolve(e);

        var arr = entity.GetComponentRef<ComponentArray<int>>(w);
        Assert.AreEqual(7, arr.Length);
        arr[0] = 12;

        var arr2 = entity.GetComponentRef<ComponentArray<int>>(w);

        Assert.AreEqual(12, arr2[0]);
        Assert.AreSame(arr.Array, arr2.Array);
    }

    [TestMethod]
    public void GetAndMutateList()
    {
        var w = new WorldBuilder().Build();
        var b = new CommandBuffer(w);

        var e = b.Create()
                 .Set(ComponentList<int>.Create(7, 0));
        using var resolver = b.Playback();
        var entity = resolver.Resolve(e);

        var list = entity.GetComponentRef<ComponentList<int>>(w);
        Assert.AreEqual(7, list.Count);
        list[0] = 12;

        var list2 = entity.GetComponentRef<ComponentList<int>>(w);

        Assert.AreEqual(12, list2[0]);
        Assert.AreSame(list.List, list2.List);
    }
}