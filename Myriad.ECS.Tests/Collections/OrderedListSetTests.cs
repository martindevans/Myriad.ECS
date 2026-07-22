using Myriad.ECS.Collections;
using Myriad.ECS.IDs;

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
        var set = new OrderedListSet<int>();
        set.Add(1);
        set.Add(2);
        set.Add(3);

        Assert.AreEqual(3, set.Count);
        Assert.IsTrue(set.Contains(1));
        Assert.IsTrue(set.Contains(2));
        Assert.IsTrue(set.Contains(3));
        Assert.IsFalse(set.Contains(4));
    }

    [TestMethod]
    public void UnionWith()
    {
        var set = new OrderedListSet<int>();
        set.Add(1);
        set.Add(2);
        set.Add(3);

        var ints = new OrderedListSet<int>();
        ints.Add(1);
        ints.Add(2);
        ints.Add(3);
        ints.Add(4);
        var frozen = FrozenOrderedListSet<int>.Create(ints);
        set.UnionWith(frozen);

        Assert.AreEqual(4, set.Count);
        Assert.IsTrue(set.Contains(1));
        Assert.IsTrue(set.Contains(2));
        Assert.IsTrue(set.Contains(3));
        Assert.IsTrue(set.Contains(4));
    }

    [TestMethod]
    public void AddUnique()
    {
        var set = new OrderedListSet<int>();
        set.Add(1);
        set.Add(2);
        set.Add(3);
        set.Add(11);
        set.Add(5);

        Assert.AreEqual(5, set.Count);
    }

    [TestMethod]
    public void AddDuplicates()
    {
        var set = new OrderedListSet<int>();
        set.Add(1);
        set.Add(1);
        set.Add(2);
        set.Add(3);
        set.Add(2);
        set.Add(2);
        set.Add(11);

        Assert.AreEqual(4, set.Count);
    }

    [TestMethod]
    public void AddRange_Duplicates_Dictionary()
    {
        var set = new OrderedListSet<int>();
        set.Add(1);
        set.Add(2);
        set.Add(3);

        set.AddRange(new Dictionary<int, bool>
        {
            { 1, true },
            { 4, true },
            { 5, true },
        }.Keys);

        Assert.AreEqual(5, set.Count);
    }

    [TestMethod]
    public void SetEquals_True()
    {
        var a = new OrderedListSet<int>();
        a.Add(1);
        a.Add(2);
        a.Add(3);
        var b = new OrderedListSet<int>();
        b.Add(3);
        b.Add(2);
        b.Add(1);

        Assert.IsTrue(a.SetEquals(b));
    }

    [TestMethod]
    public void SetEquals_True_Dictionary()
    {
        var a = new OrderedListSet<int>();
        a.Add(1);
        a.Add(2);
        a.Add(3);
        var b = new Dictionary<int, bool>();
        b.Add(3, true);
        b.Add(2, true);
        b.Add(1, true);

        Assert.IsTrue(a.SetEquals(b));
    }

    [TestMethod]
    public void SetEquals_False_SameCount()
    {
        var a = new OrderedListSet<int>();
        a.Add(1);
        a.Add(2);
        a.Add(3);
        var b = new OrderedListSet<int>();
        b.Add(2);
        b.Add(1);
        b.Add(0);

        Assert.IsFalse(a.Freeze().SetEquals(b.Freeze()));
    }

    [TestMethod]
    public void SetEquals_False_SameCount_Dictionary()
    {
        var a = new OrderedListSet<int>();
        a.Add(1);
        a.Add(2);
        a.Add(3);
        var b = new Dictionary<int, bool>();
        b.Add(2, true);
        b.Add(1, true);
        b.Add(0, true);

        Assert.IsFalse(a.Freeze().SetEquals(b));
    }

    [TestMethod]
    public void SetEquals_False_Dictionary()
    {
        var a = new OrderedListSet<int>();
        a.Add(1);
        a.Add(2);
        a.Add(3);
        var b = new Dictionary<int, bool>();
        b.Add(2, true);
        b.Add(1, true);

        Assert.IsFalse(a.Freeze().SetEquals(b));
    }

    [TestMethod]
    public void SetEquals_False_Superset()
    {
        var a = new OrderedListSet<int>();
        a.Add(1);
        a.Add(2);
        a.Add(3);
        var b = new OrderedListSet<int>();
        b.Add(1);
        b.Add(2);
        b.Add(3);
        b.Add(4);

        Assert.IsFalse(a.SetEquals(b));
    }

    [TestMethod]
    public void SetEquals_False_Subset()
    {
        var a = new OrderedListSet<int>();
        a.Add(1);
        a.Add(2);
        a.Add(3);
        var b = new OrderedListSet<int>();
        b.Add(1);
        b.Add(2);

        Assert.IsFalse(a.SetEquals(b));
    }

    [TestMethod]
    public void SetEquals_Enumerable_True()
    {
        var a = new OrderedListSet<int>();
        a.Add(1);
        a.Add(2);
        a.Add(3);
        var b = new HashSet<int>
        {
            3,
            2,
            1,
        };

        Assert.IsTrue(a.SetEquals(b));
    }

    [TestMethod]
    public void SetEquals_Enumerable_False_SameCount()
    {
        var a = new OrderedListSet<int>();
        a.Add(1);
        a.Add(2);
        a.Add(3);
        var b = new HashSet<int>
        {
            2,
            1,
            0,
        };

        Assert.IsFalse(a.SetEquals(b));
    }

    [TestMethod]
    public void SetEquals_Enumerable_False_DifferentCount()
    {
        var a = new OrderedListSet<int>();
        a.Add(1);
        a.Add(2);
        a.Add(3);
        var b = new HashSet<int>
        {
            3,
            2,
            1,
            0,
        };

        Assert.IsFalse(a.SetEquals(b));
    }

    [TestMethod]
    public void IsSuperset()
    {
        var a = new OrderedListSet<int>();
        a.Add(1);
        a.Add(2);
        a.Add(3);
        var b = new OrderedListSet<int>();
        b.Add(2);
        b.Add(3);

        Assert.IsTrue(a.Freeze().IsSupersetOf(b.Freeze()));
        Assert.IsFalse(b.Freeze().IsSupersetOf(a.Freeze()));
    }

    [TestMethod]
    public void Overlaps_True()
    {
        var a = new OrderedListSet<int>();
        a.Add(1);
        a.Add(2);
        a.Add(3);
        var b = new OrderedListSet<int>();
        b.Add(2);
        b.Add(3);

        Assert.IsTrue(a.Overlaps(b));
        Assert.IsTrue(b.Overlaps(a));
    }

    [TestMethod]
    public void Overlaps_False()
    {
        var a = new OrderedListSet<int>();
        a.Add(1);
        a.Add(2);
        a.Add(3);
        var b = new OrderedListSet<int>();
        b.Add(4);
        b.Add(5);

        Assert.IsFalse(a.Overlaps(b));
        Assert.IsFalse(b.Overlaps(a));
    }

    [TestMethod]
    public void Overlaps_False_Empty()
    {
        var a = new OrderedListSet<int>();
        a.Add(1);
        a.Add(2);
        a.Add(3);
        var b = new OrderedListSet<int>();

        Assert.IsFalse(a.Overlaps(b));
        Assert.IsFalse(b.Overlaps(a));
    }

    [TestMethod]
    public void Single_Throws()
    {
        var a = new OrderedListSet<int>();
        a.Add(1);
        a.Add(2);

        Assert.Throws<InvalidOperationException>(() => a.Single());
    }

    [TestMethod]
    public void Single()
    {
        var a = new OrderedListSet<int>();
        a.Add(1);

        Assert.AreEqual(1, a.Single());
    }

    [TestMethod]
    public void IntersectWith_FrozenOrderedListSet_PartialOverlap()
    {
        var a = new OrderedListSet<int>();
        a.Add(1);
        a.Add(2);
        a.Add(3);

        var b = new OrderedListSet<int>();
        b.Add(2);
        b.Add(3);
        b.Add(4);
        var bb = b.Freeze();
        
        a.IntersectWith(bb);
        
        Assert.AreEqual(2, a.Count);
        Assert.IsFalse(a.Contains(1));
        Assert.IsTrue(a.Contains(2));
        Assert.IsTrue(a.Contains(3));
    }

    [TestMethod]
    public void IntersectWith_FrozenOrderedListSet_PartialOverlap_NonContiguous()
    {
        var a = new OrderedListSet<int>();
        a.Add(1);
        a.Add(2);
        a.Add(3);
        a.Add(4);
        a.Add(5);

        var b = new OrderedListSet<int>();
        b.Add(2);
        b.Add(4);
        var bb = b.Freeze();

        a.IntersectWith(bb);

        Assert.AreEqual(2, a.Count);
        Assert.IsFalse(a.Contains(1));
        Assert.IsTrue(a.Contains(2));
        Assert.IsFalse(a.Contains(3));
        Assert.IsTrue(a.Contains(4));
        Assert.IsFalse(a.Contains(5));
    }

    [TestMethod]
    public void IntersectWith_FrozenOrderedListSet_NoOverlap()
    {
        var a = new OrderedListSet<int>();
        a.Add(1);
        a.Add(2);
        a.Add(3);
        a.Add(4);
        a.Add(5);

        var b = new OrderedListSet<int>();
        b.Add(6);
        var bb = b.Freeze();

        a.IntersectWith(bb);

        Assert.AreEqual(0, a.Count);
    }

    [TestMethod]
    public void IntersectWith_FrozenOrderedListSet_FullOverlap()
    {
        var a = new OrderedListSet<int>();
        a.Add(1);
        a.Add(2);
        a.Add(3);

        var b = new OrderedListSet<int>();
        b.Add(1);
        b.Add(2);
        b.Add(3);
        var bb = b.Freeze();

        a.IntersectWith(bb);

        Assert.AreEqual(3, a.Count);
    }

    [TestMethod]
    public void IntersectWith_FrozenOrderedListSet_Repro()
    {
        var a = new OrderedListSet<int>();
        a.Add(48);

        var b = new OrderedListSet<int>();
        b.Add(32);
        b.Add(48);
        var bb = b.Freeze();

        a.IntersectWith(bb);

        Assert.AreEqual(1, a.Count);
    }

    [TestMethod]
    public void IntersectWith_Fuzz()
    {
        for (var seed = 0; seed < 10_000; seed++)
            IntersectWith_Fuzz_Single(seed);
    }

    private static void IntersectWith_Fuzz_Single(int seed)
    {
        var rng = new Random(seed);

        var expectedA = new HashSet<int>();
        var expectedB = new HashSet<int>();

        var actualA = new OrderedListSet<int>();
        var actualB = new OrderedListSet<int>();

        for (var i = 0; i < rng.Next(50); i++)
        {
            var v = rng.Next(100);
            expectedA.Add(v);
            actualA.Add(v);
        }

        for (var i = 0; i < rng.Next(50); i++)
        {
            var v = rng.Next(100);
            expectedB.Add(v);
            actualB.Add(v);
        }

        expectedA.IntersectWith(expectedB);
        actualA.IntersectWith(actualB.Freeze());

        CollectionAssert.AreEqual(
            expectedA.OrderBy(x => x).ToArray(),
            actualA.ToArray(),
            $"""
             Failed with seed {seed}
             Input A: [{string.Join(", ", actualA)}]
             Input B: [{string.Join(", ", actualB)}]
             """
        );
    }
}