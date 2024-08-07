﻿using Myriad.ECS.Collections;
using Myriad.ECS.IDs;

namespace Myriad.ECS.Tests.Collections;

[TestClass]
public class ComponentBloomFilterTests
{
    [TestMethod]
    public void EmptyNotIntersect()
    {
        var a = new ComponentBloomFilter();
        var b = new ComponentBloomFilter();

        Assert.IsFalse(a.Intersects(ref b));
    }

    [TestMethod]
    public void DisjointNotIntersect()
    {
        var a = new ComponentBloomFilter();
        a.Add(ComponentID<Component0>.ID);
        a.Add(ComponentID<Component1>.ID);
        a.Add(ComponentID<Component2>.ID);
        a.Add(ComponentID<Component3>.ID);
        a.Add(ComponentID<Component4>.ID);
        a.Add(ComponentID<Component5>.ID);
        a.Add(ComponentID<Component6>.ID);
        a.Add(ComponentID<Component7>.ID);

        var b = new ComponentBloomFilter();
        b.Add(ComponentID<Component8>.ID);
        b.Add(ComponentID<Component9>.ID);
        b.Add(ComponentID<Component10>.ID);
        b.Add(ComponentID<Component11>.ID);
        b.Add(ComponentID<Component12>.ID);
        b.Add(ComponentID<Component13>.ID);
        b.Add(ComponentID<Component14>.ID);
        b.Add(ComponentID<Component15>.ID);

        var i = a.Intersects(ref b);
        Assert.IsFalse(i);
    }

    [TestMethod]
    public void IntersectingIntersect()
    {
        var a = new ComponentBloomFilter();
        a.Add(ComponentID<Component0>.ID);
        a.Add(ComponentID<Component1>.ID);
        a.Add(ComponentID<Component4>.ID);

        var b = new ComponentBloomFilter();
        b.Add(ComponentID<Component2>.ID);
        b.Add(ComponentID<Component3>.ID);
        b.Add(ComponentID<Component4>.ID);

        Assert.IsTrue(a.Intersects(ref b));
    }

    [TestMethod]
    public void UnionIntersects()
    {
        var a = new ComponentBloomFilter();
        a.Add(ComponentID<Component0>.ID);
        a.Add(ComponentID<Component1>.ID);
        a.Add(ComponentID<Component2>.ID);

        var b = new ComponentBloomFilter();
        b.Add(ComponentID<Component3>.ID);
        b.Add(ComponentID<Component4>.ID);
        b.Add(ComponentID<Component5>.ID);

        var c = new ComponentBloomFilter();
        c.Add(ComponentID<Component0>.ID);

        Assert.IsFalse(a.Intersects(ref b));
        Assert.IsFalse(b.Intersects(ref c));
        Assert.IsTrue(a.Intersects(ref c));

        var d = new ComponentBloomFilter();
        d.Union(ref b);
        d.Union(ref c);

        Assert.IsTrue(a.Intersects(ref d));
    }
}