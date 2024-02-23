using Myriad.ECS.Collections;

namespace Myriad.ECS.Tests.Collections;

[TestClass]
public class OrderedListSetTests
{
    [TestMethod]
    public void Create()
    {
        var set = new OrderedListSet<int>();

        Assert.AreEqual(0, set.Count);
    }

    [TestMethod]
    public void Create_NonEmpty()
    {
        var set = new OrderedListSet<int> { 1, 2, 3 };

        Assert.AreEqual(3, set.Count);
        Assert.IsTrue(set.Contains(1));
        Assert.IsTrue(set.Contains(2));
        Assert.IsTrue(set.Contains(3));
        Assert.IsFalse(set.Contains(4));
    }

    [TestMethod]
    public void UnionWith()
    {
        var set = new OrderedListSet<int> { 1, 2, 3 };

        set.UnionWith(new HashSet<int> { 1, 2, 3, 4 });

        Assert.AreEqual(4, set.Count);
        Assert.IsTrue(set.Contains(1));
        Assert.IsTrue(set.Contains(2));
        Assert.IsTrue(set.Contains(3));
        Assert.IsTrue(set.Contains(4));
    }

    [TestMethod]
    public void AddUnique()
    {
        var set = new OrderedListSet<int> { 1, 2, 3, 11, 5 };

        Assert.AreEqual(5, set.Count);
    }

    [TestMethod]
    public void AddDuplicates()
    {
        var set = new OrderedListSet<int> { 1, 1, 2, 3, 2, 2, 11, };

        Assert.AreEqual(4, set.Count);
    }

    [TestMethod]
    public void SetEquals_True()
    {
        var a = new OrderedListSet<int> { 1, 2, 3 };
        var b = new OrderedListSet<int> { 3, 2, 1 };

        Assert.IsTrue(a.SetEquals(b));
    }

    [TestMethod]
    public void SetEquals_False_SameCount()
    {
        var a = new OrderedListSet<int> { 1, 2, 3 };
        var b = new OrderedListSet<int> { 2, 1, 0 };

        Assert.IsFalse(a.SetEquals(b));
    }

    [TestMethod]
    public void SetEquals_False_Superset()
    {
        var a = new OrderedListSet<int> { 1, 2, 3 };
        var b = new OrderedListSet<int> { 1, 2, 3, 4 };

        Assert.IsFalse(a.SetEquals(b));
    }

    [TestMethod]
    public void SetEquals_False_Subset()
    {
        var a = new OrderedListSet<int> { 1, 2, 3 };
        var b = new OrderedListSet<int> { 1, 2 };

        Assert.IsFalse(a.SetEquals(b));
    }

    [TestMethod]
    public void SetEquals_Enumerable_True()
    {
        var a = new OrderedListSet<int> { 1, 2, 3 };
        var b = new HashSet<int> { 3, 2, 1 };

        Assert.IsTrue(a.SetEquals(b));
    }

    [TestMethod]
    public void SetEquals_Enumerable_False_SameCount()
    {
        var a = new OrderedListSet<int> { 1, 2, 3 };
        var b = new HashSet<int> { 2, 1, 0 };

        Assert.IsFalse(a.SetEquals(b));
    }

    [TestMethod]
    public void SetEquals_Enumerable_False_DifferentCount()
    {
        var a = new OrderedListSet<int> { 1, 2, 3 };
        var b = new HashSet<int> { 3, 2, 1, 0 };

        Assert.IsFalse(a.SetEquals(b));
    }

    [TestMethod]
    public void SetEquals_Enumerable2_False_DifferentCount()
    {
        var a = new OrderedListSet<int> { 1, 2, 3 };
        var b = Items();

        Assert.IsFalse(a.SetEquals(b));

        return;

        static IEnumerable<int> Items()
        {
            yield return 1;
            yield return 2;
        }
    }
}