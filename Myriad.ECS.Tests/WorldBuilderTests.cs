using Myriad.ECS.Locks;
using Myriad.ECS.Worlds;

namespace Myriad.ECS.Tests;

[TestClass]
public class WorldBuilderTests
{
    [TestMethod]
    public void CannotSetSafetyTwice()
    {
        Assert.Throws<InvalidOperationException>(() =>
        {
            new WorldBuilder().WithSafetySystem(new DefaultWorldArchetypeSafetyManager()).WithSafetySystem(new DefaultWorldArchetypeSafetyManager());
        });
    }

    [TestMethod]
    public void AddArchetypeThrowsWithDuplicateTypesGeneric()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            new WorldBuilder().WithArchetype<ComponentInt32, ComponentInt32>();
        });
    }

    [TestMethod]
    public void AddArchetypeThrowsWithDuplicateTypes()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            new WorldBuilder().WithArchetype(typeof(ComponentInt32), typeof(ComponentInt32));
        });
    }
}