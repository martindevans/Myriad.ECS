using System.Reflection;
using Myriad.ECS.IDs;
using Myriad.ECS.Registry;

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
        ComponentRegistry.Register(Assembly.GetExecutingAssembly());

        var id = ComponentRegistry.Get<ComponentInt32>();
        ComponentRegistry.Register(typeof(ComponentInt32));
        var id2 = ComponentRegistry.Get<ComponentInt32>();

        Assert.AreEqual(id, id2);
    }

    [TestMethod]
    public void ThrowsForUnknownId()
    {
        ComponentRegistry.Register(Assembly.GetExecutingAssembly());

        Assert.ThrowsException<InvalidOperationException>(() =>
        {
            ComponentRegistry.Get(default(ComponentID));
        });
    }

    [TestMethod]
    public void PhantomEntityHasPhantomFlag()
    {
        ComponentRegistry.Register(Assembly.GetExecutingAssembly());

        Assert.IsTrue(ComponentRegistry.Get(typeof(TestPhantom0)).IsPhantomComponent);
        Assert.IsTrue(ComponentRegistry.Get(typeof(TestPhantom1)).IsPhantomComponent);
        Assert.IsTrue(ComponentRegistry.Get(typeof(TestPhantom2)).IsPhantomComponent);
        Assert.IsFalse(ComponentRegistry.Get(typeof(Component1)).IsPhantomComponent);
    }
}