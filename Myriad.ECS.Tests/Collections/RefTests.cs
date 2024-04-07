using Myriad.ECS.Collections;

namespace Myriad.ECS.Tests.Collections;

[TestClass]
public class RefTests
{
    [TestMethod]
    public void RefItem()
    {
        var i = 1;
        var r = new RefT<int> { Ref = ref i };

        r.Ref++;

        Assert.AreEqual(i, r.Ref);
        Assert.AreEqual(2, i);
    }

    [TestMethod]
    public void RefItemCast()
    {
        var i = 1;
        var r = new RefT<int> { Ref = ref i };
        r.Ref++;

        var j = (int)r;

        Assert.AreEqual(i, j);
        Assert.AreEqual(2, j);
    }
}