using Myriad.ECS.Command;
using Myriad.ECS.Queries;
using Myriad.ECS.Threading;
using Myriad.ECS.Worlds;

namespace Myriad.ECS.Tests;

[TestClass]
public class WorldTests
{
    [TestMethod]
    public void Overwrite()
    {
        var w = new WorldBuilder().Build();

        var cmd = new CommandBuffer(w);
        var ab = cmd.Create().Set(new ComponentInt32(1));
        var bb = cmd.Create().Set(new ComponentInt32(2)).Set(new ComponentFloat(10));
        var cb = cmd.Create().Set(new ComponentFloat(10));
        using var resolver = cmd.Playback();
        var a = ab.Resolve();
        var b = bb.Resolve();
        var c = cb.Resolve();

        Assert.AreEqual(2, w.Overwrite(new ComponentInt32(3)));

        Assert.AreEqual(3, a.GetComponentRef<ComponentInt32>().Value);
        Assert.AreEqual(3, a.GetComponentRefT<ComponentInt32>().Ref.Value);
        Assert.AreEqual(3, b.GetComponentRef<ComponentInt32>().Value);
        Assert.AreEqual(3, b.GetComponentRefT<ComponentInt32>().Ref.Value);

        Assert.AreEqual(10, b.GetComponentRef<ComponentFloat>().Value);
        Assert.AreEqual(10, c.GetComponentRef<ComponentFloat>().Value);
    }

    [TestMethod]
    public void Filtered()
    {
        var w = new WorldBuilder().Build();

        var cmd = new CommandBuffer(w);
        var ab = cmd.Create().Set(new ComponentInt32(1));
        var bb = cmd.Create().Set(new ComponentInt32(2)).Set(new ComponentFloat(10));
        var cb = cmd.Create().Set(new ComponentFloat(10));
        using var resolver = cmd.Playback();
        var a = ab.Resolve();
        var b = bb.Resolve();
        var c = cb.Resolve();

        var q = new QueryBuilder().Exclude<ComponentFloat>().Build(w);

        Assert.AreEqual(1, w.Overwrite(new ComponentInt32(3), q));

        Assert.AreEqual(3, a.GetComponentRef<ComponentInt32>().Value);
        Assert.AreEqual(2, b.GetComponentRef<ComponentInt32>().Value);

        Assert.AreEqual(10, b.GetComponentRef<ComponentFloat>().Value);
        Assert.AreEqual(10, c.GetComponentRef<ComponentFloat>().Value);
    }

    [TestMethod]
    public void Incidental()
    {
        var w = new WorldBuilder().Build();

        var cmd = new CommandBuffer(w);
        var ab = cmd.Create().Set(new ComponentInt32(1));
        var bb = cmd.Create().Set(new ComponentInt32(2)).Set(new ComponentFloat(10));
        var cb = cmd.Create().Set(new ComponentFloat(10));
        using var resolver = cmd.Playback();
        var a = ab.Resolve();
        var b = bb.Resolve();
        var c = cb.Resolve();

        var q = new QueryBuilder().Include<ComponentFloat>().Build(w);

        Assert.AreEqual(1, w.Overwrite(new ComponentInt32(3), q));

        Assert.AreEqual(1, a.GetComponentRef<ComponentInt32>().Value);
        Assert.AreEqual(3, b.GetComponentRef<ComponentInt32>().Value);

        Assert.AreEqual(10, b.GetComponentRef<ComponentFloat>().Value);
        Assert.AreEqual(10, c.GetComponentRef<ComponentFloat>().Value);
    }

    [TestMethod]
    public void Excluded()
    {
        var w = new WorldBuilder().Build();

        var cmd = new CommandBuffer(w);
        var ab = cmd.Create().Set(new ComponentInt32(1));
        var bb = cmd.Create().Set(new ComponentInt32(2)).Set(new ComponentFloat(10));
        var cb = cmd.Create().Set(new ComponentFloat(10));
        using var resolver = cmd.Playback();

        var q = new QueryBuilder().Exclude<ComponentInt32>().Build(w);

        Assert.AreEqual(0, w.Overwrite(new ComponentInt32(3), q));
    }
}