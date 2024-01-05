using Myriad.ECS.Command;
using Myriad.ECS.IDs;
using Myriad.ECS.Worlds;

namespace Myriad.ECS.Tests;

[TestClass]
public class CommandBufferTests
{
    [TestMethod]
    public void CreateCommandBuffer()
    {
        var world = new WorldBuilder().Build();
        var buffer = new CommandBuffer(world);
        Assert.IsNotNull(buffer);
    }

    [TestMethod]
    public void CreateEntity()
    {
        var world = new WorldBuilder().Build();
        var buffer = new CommandBuffer(world);

        var eb = buffer.Create();

        using var resolver = buffer.Playback().Block();
        var entity = resolver.Resolve(eb);

        Assert.IsTrue(entity.IsAlive(world));
        Assert.AreEqual(1, world.Archetypes.Count);
        Assert.AreEqual(0, world.Archetypes.Single().Components.Count);
    }

    [TestMethod]
    public void CreateEntityAndSet()
    {
        var world = new WorldBuilder().Build();
        var buffer = new CommandBuffer(world);

        var eb = buffer
            .Create()
            .Set(new ComponentFloat(17));

        var resolver = buffer.Playback().Block();
        var entity = resolver.Resolve(eb);

        Assert.IsTrue(entity.IsAlive(world));
        Assert.AreEqual(1, world.Archetypes.Count);
        Assert.AreEqual(1, world.Archetypes.Single().Components.Count);
        Assert.IsTrue(world.Archetypes.Single().Components.Contains(ComponentID<ComponentFloat>.ID));
    }

    [TestMethod]
    public void SetTwiceOnNewEntity()
    {
        var world = new WorldBuilder().Build();
        var buffer = new CommandBuffer(world);

        var eb = buffer.Create();

        eb.Set(new ComponentFloat(1));

        Assert.ThrowsException<InvalidOperationException>(() =>
        {
            eb.Set(new ComponentFloat(2));
        });
    }

    [TestMethod]
    public void SetTwiceOnNewEntityWithOverwrite()
    {
        var world = new WorldBuilder().Build();
        var buffer = new CommandBuffer(world);

        var eb = buffer.Create();

        eb.Set(new ComponentFloat(1));
        eb.Set(new ComponentFloat(2), true);
    }

    [TestMethod]
    public void DeleteEntity()
    {
        var world = new WorldBuilder().Build();
        var buffer = new CommandBuffer(world);

        var buffered = new[]
        {
            buffer.Create().Set(new ComponentFloat(1)),
            buffer.Create().Set(new ComponentFloat(2)),
            buffer.Create().Set(new ComponentFloat(3))
        };

        using var resolver = buffer.Playback().Block();

        var entities = new[]
        {
            resolver.Resolve(buffered[0]),
            resolver.Resolve(buffered[1]),
            resolver.Resolve(buffered[2])
        };

        foreach (var entity in entities)
            Assert.IsTrue(entity.IsAlive(world));

        buffer.Delete(entities[1]);
        buffer.Playback().Block();

        Assert.IsTrue(entities[0].IsAlive(world));
        Assert.IsFalse(entities[1].IsAlive(world));
        Assert.IsTrue(entities[2].IsAlive(world));

        Assert.AreEqual(1, world.GetComponentRef<ComponentFloat>(entities[0]).Value);
        Assert.AreEqual(3, world.GetComponentRef<ComponentFloat>(entities[2]).Value);
    }

    [TestMethod]
    public void RemoveFromEntity()
    {
        var world = new WorldBuilder().Build();
        var buffer = new CommandBuffer(world);

        // Create an entity with 2 components
        var eb = buffer
                .Create()
                .Set(new ComponentFloat(123))
                .Set(new ComponentInt16(456));
        using var resolver = buffer.Playback().Block();
        var entity = resolver.Resolve(eb);

        // Remove a components
        buffer.Remove<ComponentInt16>(entity);
        buffer.Playback();

        Assert.AreEqual(123, world.GetComponentRef<ComponentFloat>(entity).Value);
        Assert.IsFalse(world.HasComponent<ComponentInt16>(entity));
    }

    [TestMethod]
    public void AddToEntity()
    {
        var world = new WorldBuilder().Build();
        var buffer = new CommandBuffer(world);

        // Create an entity with 2 components
        var eb = buffer
                .Create()
                .Set(new ComponentFloat(123))
                .Set(new ComponentInt16(456));
        using var resolver = buffer.Playback().Block();
        var entity = resolver.Resolve(eb);

        // Add a third
        buffer.Set(entity, new ComponentInt32(789));
        buffer.Playback();

        // Check they are all present
        Assert.IsTrue(world.HasComponent<ComponentFloat>(entity));
        Assert.IsTrue(world.HasComponent<ComponentInt16>(entity));
        Assert.IsTrue(world.HasComponent<ComponentInt32>(entity));
    }

    [TestMethod]
    public void SetOnEntity()
    {
        var world = new WorldBuilder().Build();
        var buffer = new CommandBuffer(world);

        // Create an entity with 2 components
        var eb = buffer
                .Create()
                .Set(new ComponentFloat(123))
                .Set(new ComponentInt16(456));
        using var resolver = buffer.Playback().Block();
        var entity = resolver.Resolve(eb);

        // Overwrite one
        buffer.Set(entity, new ComponentInt16(789));
        buffer.Playback();

        // Check the value has changed
        Assert.IsTrue(world.HasComponent<ComponentFloat>(entity));
        Assert.IsTrue(world.HasComponent<ComponentInt16>(entity));
        Assert.AreEqual(789, world.GetComponentRef<ComponentInt16>(entity).Value);
    }
}