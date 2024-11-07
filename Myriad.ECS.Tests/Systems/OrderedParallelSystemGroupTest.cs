using System.Collections.Concurrent;
using Myriad.ECS.Systems;

namespace Myriad.ECS.Tests.Systems;

[TestClass]
public class OrderedParallelSystemGroupTest
{
    [TestMethod]
    public void NoOverlap()
    {
        var group = new OrderedParallelSystemGroup<ConcurrentDictionary<int, SemaphoreSlim>>(
            "test",
            new PhasedParallelSystemGroupTest.SystemWriteComponent<Component0>(),
            new PhasedParallelSystemGroupTest.SystemWriteComponent<Component0>(),
            new PhasedParallelSystemGroupTest.SystemReadComponent<Component0>(),
            new PhasedParallelSystemGroupTest.SystemWriteComponent<Component1>(),
            new PhasedParallelSystemGroupTest.SystemWriteComponent<Component1>(),
            new PhasedParallelSystemGroupTest.SystemReadComponent<Component1>(),
            new PhasedParallelSystemGroupTest.SystemWriteComponent<Component2>(),
            new PhasedParallelSystemGroupTest.SystemWriteComponent<Component2>(),
            new PhasedParallelSystemGroupTest.SystemWriteComponent<Component3>(),
            new PhasedParallelSystemGroupTest.SystemWriteComponent<Component3>(),
            new PhasedParallelSystemGroupTest.SystemWriteComponent<Component4>(),
        );

        var values = new ConcurrentDictionary<int, SemaphoreSlim>();
        for (var i = 0; i < 250; i++)
        {
            group.BeforeUpdate(values);
            group.Update(values);
            group.AfterUpdate(values);
        }

        Assert.AreEqual(3, group.MaxDependencyChain);
    }

    [TestMethod]
    public void MaxOverlap()
    {
        var group = new OrderedParallelSystemGroup<ConcurrentDictionary<int, SemaphoreSlim>>(
            "test",
            new PhasedParallelSystemGroupTest.SystemWriteComponent<Component0>(),
            new PhasedParallelSystemGroupTest.SystemWriteComponent<Component1>(),
            new PhasedParallelSystemGroupTest.SystemWriteComponent<Component2>(),
            new PhasedParallelSystemGroupTest.SystemWriteComponent<Component3>(),
            new PhasedParallelSystemGroupTest.SystemWriteComponent<Component4>()
        );

        var values = new ConcurrentDictionary<int, SemaphoreSlim>();
        for (var i = 0; i < 250; i++)
        {
            group.BeforeUpdate(values);
            group.Update(values);
            group.AfterUpdate(values);
        }

        Assert.AreEqual(1, group.MaxDependencyChain);
    }

    [TestMethod]
    public void NoDeps()
    {
        var group = new OrderedParallelSystemGroup<int>(
            "test",
            new PhasedParallelSystemGroupTest.SystemEmpty<int>(),
            new PhasedParallelSystemGroupTest.SystemEmpty<int>(),
            new PhasedParallelSystemGroupTest.SystemEmpty<int>(),
            new PhasedParallelSystemGroupTest.SystemEmpty<int>()
        );

        group.Init();
        group.BeforeUpdate(0);
        group.Update(0);
        group.AfterUpdate(0);
        group.Dispose();
        Assert.AreEqual(1, group.MaxDependencyChain);
    }
}