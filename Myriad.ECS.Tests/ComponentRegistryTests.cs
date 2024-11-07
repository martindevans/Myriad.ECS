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
            ComponentID.Get(typeof(int));
        });
    }

    [TestMethod]
    public void AssignsDistinctIds()
    {
        var ids = new[]
        {
            ComponentID<ComponentInt32>.ID,
            ComponentID<ComponentInt64>.ID,
            ComponentID.Get(typeof(ComponentInt16)),
            ComponentID.Get(typeof(ComponentFloat)),
        };

        Assert.AreEqual(4, ids.Distinct().Count());

        Assert.AreEqual(typeof(ComponentInt16), ComponentID<ComponentInt16>.ID.Type);
    }

    [TestMethod]
    public void DoesNotReassign()
    {
        var id = ComponentID<ComponentInt32>.ID;
        var id2 = ComponentID<ComponentInt32>.ID;

        Assert.AreEqual(id, id2);
    }

    [TestMethod]
    public void ThrowsForUnknownId()
    {
        Assert.ThrowsException<InvalidOperationException>(() =>
        {
            var t = default(ComponentID).Type;
        });
    }

    [TestMethod]
    public void PhantomEntityHasPhantomFlag()
    {
        Assert.IsTrue(ComponentID.Get(typeof(TestPhantom0)).IsPhantomComponent);
        Assert.IsTrue(ComponentID.Get(typeof(TestPhantom1)).IsPhantomComponent);
        Assert.IsTrue(ComponentID.Get(typeof(TestPhantom2)).IsPhantomComponent);
        Assert.IsFalse(ComponentID.Get(typeof(Component1)).IsPhantomComponent);
    }
}