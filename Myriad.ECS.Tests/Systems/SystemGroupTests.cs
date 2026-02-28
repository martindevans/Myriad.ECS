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
    public void DisableSystemDisableChildren()
    {
        var a = new DisableSystem();
        var b = new DisableSystem();
        var c = new DisableSystem();
        var d = new DisableSystem();

        var ab = new SystemGroup<object>("ab", a, b);
        var cd = new SystemGroup<object>("cd", c, d);

        var systems = new SystemGroup<object>("abcd", ab, cd);

        ab.Enabled = false;
        Assert.AreEqual(1, a.Disabled);
        Assert.AreEqual(1, b.Disabled);
        Assert.AreEqual(0, c.Disabled);
        Assert.AreEqual(0, d.Disabled);

        systems.Enabled = false;
        Assert.AreEqual(2, a.Disabled);
        Assert.AreEqual(2, b.Disabled);
        Assert.AreEqual(1, c.Disabled);
        Assert.AreEqual(1, d.Disabled);
    }

    [TestMethod]
    public void DisableSystemItem()
    {
        var a = new DisableSystem();
        var b = new DisableSystem();

        var ab = new SystemGroup<object>("ab", a, b);

        ab.Systems[0].Enabled = false;
        ab.Systems[0].Enabled = false;
        Assert.AreEqual(1, a.Disabled);
        Assert.AreEqual(0, b.Disabled);
    }

    private class DisableSystem
        : ISystemDisable<object>
    {
        public int Updated;
        public int Disabled;

        public void Update(object data)
        {
            Updated++;
        }

        public void OnDisableSystem()
        {
            Disabled++;
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
        Assert.AreEqual(2, ((ISystemGroup<object>)systems).Systems.Count());
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