using Myriad.ECS.Allocations;

namespace Myriad.ECS.Tests.Allocations
{
    [TestClass]
    public class ArrayFactoryTests
    {
        [TestMethod]
        public void Create()
        {
            var arr = ArrayFactory.Create(typeof(int), 13);

            Assert.IsNotNull(arr);
            Assert.AreEqual(13, arr.Length);
        }
    }
}
