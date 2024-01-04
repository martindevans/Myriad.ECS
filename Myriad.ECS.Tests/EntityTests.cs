using Myriad.ECS.Worlds;

namespace Myriad.ECS.Tests;

[TestClass]
public class EntityTests
{
    [TestMethod]
    public void DefaultEntityIsNotAlive()
    {
        var w = new WorldBuilder().Build();
        Assert.IsFalse(default(Entity).IsAlive(w));
    }   
}