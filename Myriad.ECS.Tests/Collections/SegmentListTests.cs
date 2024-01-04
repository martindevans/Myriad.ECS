using Myriad.ECS.Collections;

namespace Myriad.ECS.Tests.Collections;

[TestClass]
public class SegmentListTests
{
    [TestMethod]
    public void Create()
    {
        var list = new SegmentedList<int>(128);

        Assert.AreEqual(128, list.SegmentCapacity);
    }

    [TestMethod]
    public void IndexSingleSegment()
    {
        var list = new SegmentedList<int>(16);

        // Write index to each slot
        for (var i = 0; i < list.SegmentCapacity; i++)
            list.GetMutable(i) = i;

        // Read index from each slot
        for (var i = 0; i < list.SegmentCapacity; i++)
            Assert.AreEqual(i, list.GetImmutable(i));
    }

    [TestMethod]
    public void IndexSingleSegmentOutOfRange()
    {
        var list = new SegmentedList<int>(16);

        Assert.ThrowsException<IndexOutOfRangeException>(() => { list.GetMutable(-1); });
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => { list.GetMutable(16); });
    }

    [TestMethod]
    public void Grow()
    {
        var list = new SegmentedList<int>(16);

        Assert.AreEqual(16, list.SegmentCapacity);
        Assert.AreEqual(16, list.TotalCapacity);
        Assert.AreEqual(0, list.GetImmutable(15));

        list.Grow();
        Assert.AreEqual(16, list.SegmentCapacity);
        Assert.AreEqual(32, list.TotalCapacity);
        Assert.AreEqual(0, list.GetImmutable(31));

        list.Grow();
        Assert.AreEqual(16, list.SegmentCapacity);
        Assert.AreEqual(48, list.TotalCapacity);
        Assert.AreEqual(0, list.GetImmutable(47));
    }
}