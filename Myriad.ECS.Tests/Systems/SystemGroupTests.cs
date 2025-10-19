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
        : ISystemBefore<object>, ISystemAfter<object>
    {
        public int Updated;
        public int Before;
        public int After;

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

        Assert.IsNull(systems.TryGet<UpdateCountSystem>());

        group.Add(a);
        group.Add(b);

        Assert.AreEqual(a, systems.TryGet<UpdateCountSystem>());
        Assert.AreEqual(b, systems.TryGet<SystemEmpty<object>>());

        Assert.IsTrue(group.Remove(a));

        Assert.IsNull(systems.TryGet<UpdateCountSystem>());

        Assert.IsFalse(group.Remove(a));
    }
}