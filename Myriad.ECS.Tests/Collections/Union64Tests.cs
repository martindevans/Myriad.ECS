using Myriad.ECS.Collections;

namespace Myriad.ECS.Tests.Collections;

[TestClass]
public class Union64Tests
{
    [TestMethod]
    public void Equal()
    {
        var a = new Union64
        {
            I0 = 36920830,
            I1 = -4524324
        };

        var b = new Union64
        {
            Long = a.Long
        };

        Assert.AreEqual(a.I0, b.I0);
        Assert.AreEqual(a.I1, b.I1);

        Assert.AreEqual(a.U0, b.U0);
        Assert.AreEqual(a.U1, b.U1);

        Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
    }

    [TestMethod]
    public void NotEqual()
    {
        var a = new Union64
        {
            I0 = 36920830,
            I1 = -4524324
        };

        var b = new Union64
        {
            I0 = 1,
            I1 = 2
        };

        Assert.AreNotEqual(a.I0, b.I0);
        Assert.AreNotEqual(a.I1, b.I1);

        Assert.AreNotEqual(a.U0, b.U0);
        Assert.AreNotEqual(a.U1, b.U1);

        Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
    }
}