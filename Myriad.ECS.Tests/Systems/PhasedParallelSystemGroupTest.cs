using System.Collections.Concurrent;
using Myriad.ECS.IDs;
using Myriad.ECS.Systems;

namespace Myriad.ECS.Tests.Systems;

[TestClass]
public class PhasedParallelSystemGroupTest
{
    [TestMethod]
    public void NoOverlap()
    {
        var group = new PhasedParallelSystemGroup<ConcurrentDictionary<int, SemaphoreSlim>>(
            "test",
            new SystemWriteComponent<Component0>(),
            new SystemWriteComponent<Component0>(),
            new SystemWriteComponent<Component1>(),
            new SystemWriteComponent<Component1>(),
            new SystemWriteComponent<Component2>(),
            new SystemWriteComponent<Component2>(),
            new SystemWriteComponent<Component3>(),
            new SystemWriteComponent<Component3>(),
            new SystemWriteComponent<Component4>()
        );

        var values = new ConcurrentDictionary<int, SemaphoreSlim>();
        for (var i = 0; i < 250; i++)
        {
            group.BeforeUpdate(values);
            group.Update(values);
            group.AfterUpdate(values);
        }

        Assert.AreEqual(2, group.Phases);
    }

    [TestMethod]
    public void MaxOverlap()
    {
        var group = new PhasedParallelSystemGroup<ConcurrentDictionary<int, SemaphoreSlim>>(
            "test",
            new SystemWriteComponent<Component0>(),
            new SystemWriteComponent<Component1>(),
            new SystemWriteComponent<Component2>(),
            new SystemWriteComponent<Component3>(),
            new SystemWriteComponent<Component4>()
        );

        var values = new ConcurrentDictionary<int, SemaphoreSlim>();
        for (var i = 0; i < 250; i++)
        {
            group.BeforeUpdate(values);
            group.Update(values);
            group.AfterUpdate(values);
        }

        Assert.AreEqual(1, group.Phases);
    }

    public abstract class BaseSystemCheckSemaphore<TComponent>
        : ISystemDeclare<ConcurrentDictionary<int, SemaphoreSlim>>
        , ISystemBefore<ConcurrentDictionary<int, SemaphoreSlim>>
        , ISystemAfter<ConcurrentDictionary<int, SemaphoreSlim>>
        where TComponent : IComponent
    {
        private readonly bool _check;
        private readonly bool _take;

        protected BaseSystemCheckSemaphore(bool check, bool take)
        {
            _check = check;
            _take = take;
        }

        public void Update(ConcurrentDictionary<int, SemaphoreSlim> data)
        {
            var v = data.GetOrAdd(ComponentID<TComponent>.ID.Value, new SemaphoreSlim(1));

            if (_check)
            {
                if (v.CurrentCount != 1)
                    Assert.Fail("Semaphore count is not one");
            }

            if (_take)
            {
                if (!v.Wait(0))
                    Assert.Fail("Failed to take lock");
            }

            Thread.Sleep(1);

            if (_take)
            {
                v.Release();
            }
        }

        public void BeforeUpdate(ConcurrentDictionary<int, SemaphoreSlim> data)
        {
        }

        public void AfterUpdate(ConcurrentDictionary<int, SemaphoreSlim> data)
        {
        }

        public abstract void Declare(ref SystemDeclaration declaration);
    }

    public class SystemWriteComponent<TComponent>
        : BaseSystemCheckSemaphore<TComponent>
        where TComponent : IComponent
    {
        public SystemWriteComponent()
            : base(false, true)
        {
        }

        public override void Declare(ref SystemDeclaration declaration)
        {
            declaration.Write<TComponent>();
        }
    }

    public class SystemReadComponent<TComponent>
        : BaseSystemCheckSemaphore<TComponent>
        where TComponent : IComponent
    {
        public SystemReadComponent()
            : base(true, false)
        {
        }

        public override void Declare(ref SystemDeclaration declaration)
        {
            declaration.Read<TComponent>();
        }
    }

    public class SystemEmpty<TData>
        : ISystemDeclare<TData>
    {
        public void Declare(ref SystemDeclaration declaration)
        {
        }

        public void Update(TData data)
        {
        }
    }
}