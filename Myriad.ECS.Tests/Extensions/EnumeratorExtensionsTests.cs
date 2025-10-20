using Myriad.ECS.Extensions;

namespace Myriad.ECS.Tests.Extensions;

[TestClass]
public class EnumeratorExtensionsTests
{
    [TestMethod]
    public void SkipEnumerator()
    {
        var e = Enumerable.Range(0, 100).ToList().GetEnumerator();
        Assert.IsTrue(e.Skip(5));

        Assert.AreEqual(4, e.Current);
    }

    [TestMethod]
    public void SkipEnumerator_PastEnd()
    {
        var e = Enumerable.Range(0, 10).ToList().GetEnumerator();
        Assert.IsFalse(e.Skip(15));
    }
}