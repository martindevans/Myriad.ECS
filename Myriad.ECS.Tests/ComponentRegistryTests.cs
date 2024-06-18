using System.Reflection;
using Myriad.ECS.IDs;

namespace Myriad.ECS.Tests;

[TestClass]
public class ComponentRegistryTests
{
    [TestMethod]
    public void CannotAssignNonComponent()
    {
        Assert.ThrowsException<ArgumentException>(() =>
        {
            ComponentRegistry.Get(typeof(int));
        });
    }

    [TestMethod]
    public void AssignsDistinctIds()
    {
        var ids = new[]
        {
            ComponentRegistry.Get<ComponentInt32>(),
            ComponentRegistry.Get<ComponentInt64>(),
            ComponentRegistry.Get(typeof(ComponentInt16)),
            ComponentRegistry.Get(typeof(ComponentFloat)),
        };

        Assert.AreEqual(4, ids.Distinct().Count());

        Assert.AreEqual(typeof(ComponentInt16), ComponentID<ComponentInt16>.ID.Type);
    }

    [TestMethod]
    public void DoesNotReassign()
    {
        var id = ComponentRegistry.Get<ComponentInt32>();
        var id2 = ComponentRegistry.Get<ComponentInt32>();

        Assert.AreEqual(id, id2);
    }

    [TestMethod]
    public void ThrowsForUnknownId()
    {
        Assert.ThrowsException<InvalidOperationException>(() =>
        {
            ComponentRegistry.Get(default(ComponentID));
        });
    }

    [TestMethod]
    public void PhantomEntityHasPhantomFlag()
    {
        Assert.IsTrue(ComponentRegistry.Get(typeof(TestPhantom0)).IsPhantomComponent);
        Assert.IsTrue(ComponentRegistry.Get(typeof(TestPhantom1)).IsPhantomComponent);
        Assert.IsTrue(ComponentRegistry.Get(typeof(TestPhantom2)).IsPhantomComponent);
        Assert.IsFalse(ComponentRegistry.Get(typeof(Component1)).IsPhantomComponent);
    }
}