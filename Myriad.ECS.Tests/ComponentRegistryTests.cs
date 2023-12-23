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
}