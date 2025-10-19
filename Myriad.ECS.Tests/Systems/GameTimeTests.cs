using Myriad.ECS.Systems;

namespace Myriad.ECS.Tests.Systems;

[TestClass]
public class GameTimeTests
{
    [TestMethod]
    public void Tick()
    {
        var gt = new GameTime();

        gt.Tick(0.1);
        Assert.AreEqual(0.1, gt.DeltaTime);
        Assert.AreEqual(0.1, gt.Time);
        Assert.AreEqual(1ul, gt.Frame);

        gt.Tick(0.2);
        Assert.AreEqual(0.2, gt.DeltaTime, 1E-10);
        Assert.AreEqual(0.3, gt.Time, 1E-10);
        Assert.AreEqual(2ul, gt.Frame);
    }

    [TestMethod]
    public void SetFrameCount()
    {
        var gt = new GameTime();

        gt.Tick(1.1);
        gt.Frame = 4;

        Assert.AreEqual(1.1, gt.DeltaTime);
        Assert.AreEqual(1.1, gt.Time);
        Assert.AreEqual(4ul, gt.Frame);
    }

    [TestMethod]
    public void SetTime()
    {
        var gt = new GameTime();

        gt.Tick(1.1);
        gt.Time = 12;

        Assert.AreEqual(1.1, gt.DeltaTime);
        Assert.AreEqual(12, gt.Time);
        Assert.AreEqual(1ul, gt.Frame);
    }

    [TestMethod]
    public void SetDeltaTime()
    {
        var gt = new GameTime();

        gt.Tick(1.1);
        gt.DeltaTime = 12;

        Assert.AreEqual(12, gt.DeltaTime);
        Assert.AreEqual(1.1, gt.Time);
        Assert.AreEqual(1ul, gt.Frame);
    }
}