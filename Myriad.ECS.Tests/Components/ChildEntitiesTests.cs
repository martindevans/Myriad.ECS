using Myriad.ECS.Command;
using Myriad.ECS.Components;
using Myriad.ECS.Worlds;

namespace Myriad.ECS.Tests.Components;

[TestClass]
public class ChildEntitiesTests
{
    [TestMethod]
    public void DestroysChildren()
    {
        var w = new WorldBuilder().Build();
        var cmd = new CommandBuffer(w);

        // Create parent
        var parentBuf = cmd.Create().Set(new ChildEntities([]));
        using var r1 = cmd.Playback();
        var parent = parentBuf.Resolve();

        // Create children
        cmd.Create();
        cmd.Create();
        cmd.Create();
        cmd.Create();

        // Mark them as children of parent
        using (var resolver = cmd.Playback())
        {
            ref var childEntities = ref parent.GetComponentRef<ChildEntities>();
            for (var i = 0; i < resolver.Count; i++)
                childEntities.Children.Add(resolver[i]);
        }

        // Delete parent
        cmd.Delete(parent);
        cmd.Playback().Dispose();

        Assert.AreEqual(0, w.Count());
    }
}