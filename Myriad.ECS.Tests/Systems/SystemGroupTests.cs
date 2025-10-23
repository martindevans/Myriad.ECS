using Myriad.ECS.Systems;
using static Myriad.ECS.Tests.Systems.PhasedParallelSystemGroupTest;

namespace Myriad.ECS.Tests.Systems;

[TestClass]
public class SystemGroupTests
{
    [TestMethod]
    public void DisabledSystemNotUpdated()
    {
        var counter = new UpdateCountSystem();
        var systems = new SystemGroup<object>(
            "name",
            counter
        );

        systems.BeforeUpdate(null!);
        systems.Update(null!);
        systems.AfterUpdate(null!);

        Assert.AreEqual(1, counter.Before);
        Assert.AreEqual(1, counter.Updated);
        Assert.AreEqual(1, counter.After);

        systems.Enabled = false;

        systems.BeforeUpdate(null!);
        systems.Update(null!);
        systems.AfterUpdate(null!);

        Assert.AreEqual(1, counter.Before);
        Assert.AreEqual(1, counter.Updated);
        Assert.AreEqual(1, counter.After);

        systems.Enabled = true;

        systems.BeforeUpdate(null!);
        systems.Update(null!);
        systems.AfterUpdate(null!);

        Assert.AreEqual(2, counter.Before);
        Assert.AreEqual(2, counter.Updated);
        Assert.AreEqual(2, counter.After);
    }

    private class UpdateCountSystem
        : ISystemBefore<object>, ISystemAfter<object>, IDisposable
    {
        public int Updated;
        public int Before;
        public int After;
        public int Disposed;

        public void Update(object data)
        {
            Updated++;
        }

        public void BeforeUpdate(object data)
        {
            Before++;
        }

        public void AfterUpdate(object data)
        {
            After++;
        }

        public void Dispose()
        {
            Disposed++;
        }
    }

    [TestMethod]
    public void SystemsProperty()
    {
        var a = new UpdateCountSystem();
        var b = new UpdateCountSystem();
        var c = new UpdateCountSystem();

        var systems = new SystemGroup<object>(
            "name",
            new SystemGroup<object>("inner", a, b),
            c
        );

        // Assert is contains the inner group and system "c"
        Assert.AreEqual(2, systems.Systems.Count);
        Assert.IsTrue(systems.Systems.All(x => x.System != a));
        Assert.IsTrue(systems.Systems.All(x => x.System != b));
        Assert.IsTrue(systems.Systems.Any(x => x.System == c));
    }

    [TestMethod]
    public void RecursiveSystemsProperty()
    {
        var a = new UpdateCountSystem();
        var b = new UpdateCountSystem();
        var c = new UpdateCountSystem();

        var systems = new SystemGroup<object>(
            "name",
            new SystemGroup<object>("inner", a, b),
            c
        );

        var recursive = systems.RecursiveSystems.ToArray();

        Assert.AreEqual(3, recursive.Length);
        Assert.IsTrue(recursive.Any(x => x.System == a));
        Assert.IsTrue(recursive.Any(x => x.System == b));
        Assert.IsTrue(recursive.Any(x => x.System == c));
    }

    [TestMethod]
    public void AddRemove()
    {
        var a = new UpdateCountSystem();
        var b = new SystemEmpty<object>();

        var group = new DynamicSystemGroup<object>("name");
        ISystemGroup<object> systems = group;

        // System hasn't been added yet, updates should not affect system
        group.BeforeUpdate(new());
        group.Update(new());
        group.AfterUpdate(new());
        Assert.AreEqual(0, a.Before);
        Assert.AreEqual(0, a.Updated);
        Assert.AreEqual(0, a.After);

        // Searching for system that doesn't exist should be null
        Assert.IsNull(systems.TryGet<UpdateCountSystem>());

        group.Add(a);
        group.Add(b);

        // System has been added yet, updates should count
        group.BeforeUpdate(new());
        group.Update(new());
        group.AfterUpdate(new());
        Assert.AreEqual(1, a.Before);
        Assert.AreEqual(1, a.Updated);
        Assert.AreEqual(1, a.After);

        // Check systems are in system list
        Assert.AreEqual(a, systems.TryGet<UpdateCountSystem>());
        Assert.AreEqual(b, systems.TryGet<SystemEmpty<object>>());

        // It's in the group, so removing should be true
        Assert.IsTrue(group.Remove(a));

        // It's no longer present, updates should not affect system
        group.BeforeUpdate(new());
        group.Update(new());
        group.AfterUpdate(new());
        Assert.AreEqual(1, a.Before);
        Assert.AreEqual(1, a.Updated);
        Assert.AreEqual(1, a.After);

        Assert.IsNull(systems.TryGet<UpdateCountSystem>());

        // Can't remove twice
        Assert.IsFalse(group.Remove(a));
    }

    [TestMethod]
    public void DisposeGroup()
    {
        var a = new UpdateCountSystem();
        var b = new UpdateCountSystem();
        var c = new UpdateCountSystem();

        var systems = new DynamicSystemGroup<object>(
            "name",
            new DynamicSystemGroup<object>("inner", a, b),
            c
        );

        Assert.AreEqual(0, a.Disposed);
        Assert.AreEqual(0, b.Disposed);
        Assert.AreEqual(0, c.Disposed);

        systems.Dispose();

        Assert.AreEqual(1, a.Disposed);
        Assert.AreEqual(1, b.Disposed);
        Assert.AreEqual(1, c.Disposed);
    }
}