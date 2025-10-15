using Myriad.ECS.Collections;

namespace Myriad.ECS.Tests.Collections;

[TestClass]
public class FrozenOrderedListSetTests
{
    [TestMethod]
    public void Create_FromHashSet()
    {
        var set = FrozenOrderedListSet<int>.Create(new HashSet<int> { 1, 2, 3 });

        Assert.AreEqual(3, set.Count);
        Assert.IsTrue(set.Contains(1));
        Assert.IsTrue(set.Contains(2));
        Assert.IsTrue(set.Contains(3));
        Assert.IsFalse(set.Contains(4));
    }

    [TestMethod]
    public void Create_FromHashSet_Empty()
    {
        var set = FrozenOrderedListSet<int>.Create(new HashSet<int>());
        Assert.AreEqual(0, set.Count);
    }

    [TestMethod]
    public void Create_FromList()
    {
        var set = FrozenOrderedListSet<int>.Create(new List<int> { 3, 1, 2 });

        Assert.AreEqual(3, set.Count);
        Assert.IsTrue(set.Contains(1));
        Assert.IsTrue(set.Contains(2));
        Assert.IsTrue(set.Contains(3));
        Assert.IsFalse(set.Contains(4));
    }

    [TestMethod]
    public void Create_FromList_Empty()
    {
        var set = FrozenOrderedListSet<int>.Create(new List<int>());
        Assert.AreEqual(0, set.Count);
    }
}