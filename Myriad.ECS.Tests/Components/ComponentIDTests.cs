using Myriad.ECS.Command;
using Myriad.ECS.Components;
using Myriad.ECS.IDs;
using Myriad.ECS.Worlds;

namespace Myriad.ECS.Tests.Components;

[TestClass]
public class ComponentIDTests
{
    [TestMethod]
    public void ToStringContainsPhantom()
    {
        var p = ComponentID<TestPhantom0>.ID;
        var np = ComponentID<Component0>.ID;

        Assert.IsTrue(p.IsPhantomComponent);
        Assert.IsTrue(p.ToString().Contains("phantom"));

        Assert.IsFalse(np.IsPhantomComponent);
        Assert.IsFalse(np.ToString().Contains("phantom"));
    }

    [TestMethod]
    public void ToStringContainsDispose()
    {
        var d = ComponentID<TestDisposable>.ID;
        var nd = ComponentID<Component0>.ID;

        Assert.IsTrue(d.IsDisposableComponent);
        Assert.IsTrue(d.ToString().Contains("dispos"));

        Assert.IsFalse(nd.IsPhantomComponent);
        Assert.IsFalse(nd.ToString().Contains("dispos"));
    }
}