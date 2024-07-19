using Myriad.ECS.IDs;
using Myriad.ECS.Worlds.Archetypes;

namespace Myriad.ECS.Tests;

[TestClass]
public class ArchetypeHashTests
{
    [TestMethod]
    public void ArchetypeHashEqual()
    {
        var hash1 = new ArchetypeHash()
           .Toggle(ComponentID<ComponentFloat>.ID);
        Console.WriteLine(hash1);

        var hash2 = new ArchetypeHash()
           .Toggle(ComponentID<ComponentFloat>.ID);
        Console.WriteLine(hash1);

        Assert.AreEqual(hash1, hash2);
    }

    [TestMethod]
    public void ArchetypeHashNotEqual()
    {
        var hash1 = new ArchetypeHash();
        hash1 = hash1.Toggle(ComponentID<ComponentInt16>.ID);
        Console.WriteLine(hash1);

        var hash2 = new ArchetypeHash();
        hash2 = hash2.Toggle(ComponentID<ComponentFloat>.ID);
        Console.WriteLine(hash2);

        Assert.AreNotEqual(hash1, hash2);
    }

    [TestMethod]
    public void ArchetypeHashRemoveComponents()
    {
        var hash1 = new ArchetypeHash()
           .Toggle(ComponentID<ComponentInt16>.ID)
            .Toggle(ComponentID<ComponentFloat>.ID);
        Console.WriteLine(hash1);

        // Create the same hash again, with one extra item
        var hash2 = new ArchetypeHash()
           .Toggle(ComponentID<ComponentInt16>.ID)
           .Toggle(ComponentID<ComponentFloat>.ID)
           .Toggle(ComponentID<ComponentInt32>.ID);
        Console.WriteLine(hash2);
        Assert.AreNotEqual(hash1, hash2);

        // Remove the extra item
        hash2 = hash2.Toggle(ComponentID<ComponentInt32>.ID);
        Assert.AreEqual(hash1, hash2);
    }
}