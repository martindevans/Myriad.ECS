using Myriad.ECS.Collections;
using Myriad.ECS.IDs;

namespace Myriad.ECS.Tests.Collections;

[TestClass]
public class BloomFilter64X384Tests
{
    [TestMethod]
    public void EmptyNotIntersect()
    {
        var a = new BloomFilter32x384();
        var b = new BloomFilter32x384();

        Assert.IsFalse(a.MaybeIntersects(ref b));
    }

    [TestMethod]
    public void DisjointNotIntersect()
    {
        var a = new BloomFilter32x384();
        a.Add(ComponentID<Component0>.ID);
        a.Add(ComponentID<Component1>.ID);
        a.Add(ComponentID<Component2>.ID);
        a.Add(ComponentID<Component3>.ID);

        var b = new BloomFilter32x384();
        b.Add(ComponentID<Component8>.ID);
        b.Add(ComponentID<Component9>.ID);
        b.Add(ComponentID<Component10>.ID);
        b.Add(ComponentID<Component11>.ID);

        var i = a.MaybeIntersects(ref b);
        Assert.IsFalse(i);

        // Note that this test _can_ fail, since the bloom filter is probabalistic and errs on the side of caution.
    }

    [TestMethod]
    public void IntersectingIntersect()
    {
        var a = new BloomFilter32x384();
        a.Add(ComponentID<Component0>.ID);
        a.Add(ComponentID<Component1>.ID);
        a.Add(ComponentID<Component4>.ID);

        var b = new BloomFilter32x384();
        b.Add(ComponentID<Component2>.ID);
        b.Add(ComponentID<Component3>.ID);
        b.Add(ComponentID<Component4>.ID);

        Assert.IsTrue(a.MaybeIntersects(ref b));
    }

    [TestMethod]
    public void UnionIntersects()
    {
        var a = new BloomFilter32x384();
        a.Add(ComponentID<Component0>.ID);
        a.Add(ComponentID<Component1>.ID);
        a.Add(ComponentID<Component2>.ID);

        var b = new BloomFilter32x384();
        b.Add(ComponentID<Component3>.ID);
        b.Add(ComponentID<Component4>.ID);
        b.Add(ComponentID<Component5>.ID);

        var c = new BloomFilter32x384();
        c.Add(ComponentID<Component0>.ID);

        Assert.IsFalse(a.MaybeIntersects(ref b));
        Assert.IsFalse(b.MaybeIntersects(ref c));
        Assert.IsTrue(a.MaybeIntersects(ref c));

        var d = new BloomFilter32x384();
        d.Union(ref b);
        d.Union(ref c);

        Assert.IsTrue(a.MaybeIntersects(ref d));
    }
}