using System.Collections.Concurrent;
using Myriad.ECS.IDs;
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
            new SystemCheckSemaphore<Component0>(),
            new SystemCheckSemaphore<Component0>(),
            new SystemCheckSemaphore<Component1>(),
            new SystemCheckSemaphore<Component1>(),
            new SystemCheckSemaphore<Component2>(),
            new SystemCheckSemaphore<Component2>(),
            new SystemCheckSemaphore<Component3>(),
            new SystemCheckSemaphore<Component3>(),
            new SystemCheckSemaphore<Component4>()
        );

        var values = new ConcurrentDictionary<int, SemaphoreSlim>();
        for (var i = 0; i < 250; i++)
            group.Update(values);

        Assert.AreEqual(2, group.MaxDependencyChain);
    }

    [TestMethod]
    public void MaxOverlap()
    {
        var group = new OrderedParallelSystemGroup<ConcurrentDictionary<int, SemaphoreSlim>>(
            "test",
            new SystemCheckSemaphore<Component0>(),
            new SystemCheckSemaphore<Component1>(),
            new SystemCheckSemaphore<Component2>(),
            new SystemCheckSemaphore<Component3>(),
            new SystemCheckSemaphore<Component4>()
        );

        var values = new ConcurrentDictionary<int, SemaphoreSlim>();
        for (var i = 0; i < 250; i++)
            group.Update(values);

        Assert.AreEqual(1, group.MaxDependencyChain);
    }

    private class SystemCheckSemaphore<TComponent>
        : ISystemDeclare<ConcurrentDictionary<int, SemaphoreSlim>> where TComponent : IComponent
    {
        public void Update(ConcurrentDictionary<int, SemaphoreSlim> data)
        {
            var v = data.GetOrAdd(ComponentID<TComponent>.ID.Value, new SemaphoreSlim(1));

            if (!v.Wait(0))
                Assert.Fail("Failed to take lock");

            Thread.Sleep(1);

            v.Release();
        }

        public void Declare(ref SystemDeclaration declaration)
        {
            declaration.Write<TComponent>();
        }
    }
}