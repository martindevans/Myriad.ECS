using Myriad.ECS.Allocations;

namespace Myriad.ECS.Tests.Collections;

[TestClass]
public class LocalPoolTests
{
    [TestMethod]
    public void CreateAndGet()
    {
        using (var p = new LocalPool<object>())
        {
            var obj1 = p.Get();
            Assert.IsNotNull(obj1);

            var obj2 = p.Get();
            Assert.IsNotNull(obj2);

            Assert.AreNotSame(obj1, obj2);
        };
    }

    [TestMethod]
    public void Return()
    {
        using (var p = new LocalPool<object>(16))
        {
            var obj1 = p.Get();
            Assert.IsNotNull(obj1);

            p.Return(obj1);

            var obj2 = p.Get();
            Assert.IsNotNull(obj2);

            Assert.AreSame(obj1, obj2);
        };
    }

    [TestMethod]
    public void ReturnMany()
    {
        using (var p = new LocalPool<object>(5))
        {
            for (var i = 0; i < 100; i++)
                p.Return(new object());
        }
    }
}