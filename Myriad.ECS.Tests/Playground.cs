namespace Myriad.ECS.Tests;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestMethod1()
    {
        TryGet(out var foo);
        //foo.Bar = 1;
    }

    public bool TryGet(out Foo output)
    {
        output = default;
        return false;
    }

    public ref struct Foo
    {
        public ref int Bar;
    }
}