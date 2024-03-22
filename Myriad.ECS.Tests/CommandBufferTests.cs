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

        using var resolver = buffer.Playback();
        var entity = resolver.Resolve(eb);

        Assert.IsTrue(entity.IsAlive(world));
        Assert.AreEqual(1, world.Archetypes.Count);
        Assert.AreEqual(0, world.Archetypes.Single().Components.Count);
    }

    [TestMethod]
    public void CreateManyEntities()
    {
        var world = new WorldBuilder().Build();
        var buffer = new CommandBuffer(world);

        // Create lots of entities
        var buffered = new List<CommandBuffer.BufferedEntity>();
        for (var i = 0; i < 50000; i++)
            buffered.Add(buffer.Create().Set(new ComponentInt32(i)));

        // Execute buffer
        using var resolver = buffer.Playback();

        // Resolve results
        var entities = new List<Entity>();
        foreach (var b in buffered)
            entities.Add(resolver.Resolve(b));

        for (var i = 0; i < entities.Count; i++)
        {
            var entity = entities[i];
            Assert.IsTrue(entity.IsAlive(world));
            Assert.AreEqual(1, world.Archetypes.Count);
            Assert.AreEqual(1, world.Archetypes.Single().Components.Count);
            Assert.AreEqual(i, world.GetComponentRef<ComponentInt32>(entity).Value);
        }
    }

    [TestMethod]
    public void ChurnManyEntities()
    {
        var world = new WorldBuilder().Build();
        var buffer = new CommandBuffer(world);

        var rng = new Random(46576);

        // keep track ofevery single entity ever created
        var alive = new List<Entity>();
        var dead = new List<Entity>();

        // Do lots of rounds of creation and destruction
        for (var i = 0; i < 20; i++)
        {
            // Create lots of entities
            var buffered = new List<CommandBuffer.BufferedEntity>();
            for (var j = 0; j < 10000; j++)
            {
                var b = buffer.Create().Set(new ComponentInt32(j));
                buffered.Add(b);

                for (int k = 0; k < 3; k++)
                {
                    switch (rng.Next(0, 6))
                    {
                        case 0: b.Set(new ComponentByte((byte)i), true); break;
                        case 1: b.Set(new ComponentInt16((short)i), true); break;
                        case 2: b.Set(new ComponentFloat(i), true); break;
                        case 3: b.Set(new ComponentInt32(i), true); break;
                        case 4: b.Set(new ComponentInt64(i), true); break;
                        default: break;
                    }
                }
            }

            // Destroy some random entities
            for (var j = 0; j < 1000; j++)
            {
                if (alive.Count == 0)
                    break;
                var idx = rng.Next(0, alive.Count);
                var ent = alive[idx];
                Assert.IsTrue(ent.IsAlive(world));
                buffer.Delete(ent);
                alive.RemoveAt(idx);
                dead.Add(ent);
            }

            // Execute
            var resolver = buffer.Playback();

            // Resolve results
            foreach (var b in buffered)
                alive.Add(resolver.Resolve(b));

            // Check all the entities
            for (var j = 0; j < alive.Count; j++)
            {
                var entity = alive[j];
                Assert.IsTrue(entity.IsAlive(world));
            }
            for (var j = 0; j < dead.Count; j++)
            {
                var entity = dead[j];
                Assert.IsTrue(!entity.IsAlive(world));
            }

            // Check archetypes
            Assert.AreEqual(alive.Count, world.Archetypes.Select(a => a.EntityCount).Sum());
        }
    }

    [TestMethod]
    public void CreateEntityLateResolveThrows()
    {
        var world = new WorldBuilder().Build();
        var buffer = new CommandBuffer(world);

        var eb = buffer.Create();

        var resolver = buffer.Playback();
        resolver.Dispose();

        Assert.ThrowsException<ObjectDisposedException>(() =>
        {
            resolver.Resolve(eb);
        });
    }

    [TestMethod]
    public void CreateEntityCannotResolveFromAnotherBuffer()
    {
        var world = new WorldBuilder().Build();
        var buffer1 = new CommandBuffer(world);
        var buffer2 = new CommandBuffer(world);

        // Create the entity
        var eb1 = buffer1.Create();
        buffer1.Playback();

        // Also run the other buffer to get another resolver
        var resolver2 = buffer2.Playback();

        // Resolve the entity ID using the wrong resolver
        Assert.ThrowsException<InvalidOperationException>(() =>
        {
            resolver2.Resolve(eb1);
        });
    }

    [TestMethod]
    public void CreateEntityCannotResolveFromPreviousPlayback()
    {
        var world = new WorldBuilder().Build();
        var buffer1 = new CommandBuffer(world);

        // Create the entity
        var eb1 = buffer1.Create();
        buffer1.Playback();

        // Re-use that buffer
        var resolver2 = buffer1.Playback();

        // Resolve the entity ID using the wrong resolver
        Assert.ThrowsException<InvalidOperationException>(() =>
        {
            resolver2.Resolve(eb1);
        });
    }

    [TestMethod]
    public void ModifyBufferedEntityAfterPlaybackThrows()
    {
        var world = new WorldBuilder().Build();
        var buffer1 = new CommandBuffer(world);

        // Create the entity
        var eb1 = buffer1.Create();
        buffer1.Playback();

        // Try to modify the buffered entity
        Assert.ThrowsException<InvalidOperationException>(() =>
        {
            eb1.Set(new ComponentFloat(8)); 
        });
    }

    [TestMethod]
    public void CreateEntityAndSet()
    {
        var world = new WorldBuilder().Build();
        var buffer = new CommandBuffer(world);

        var eb = buffer
            .Create()
            .Set(new ComponentFloat(17));

        var resolver = buffer.Playback();
        var entity = resolver.Resolve(eb);

        Assert.IsTrue(entity.IsAlive(world));
        Assert.AreEqual(1, world.Archetypes.Count);
        Assert.AreEqual(1, world.Archetypes.Single().Components.Count);
        Assert.IsTrue(world.Archetypes.Single().Components.Contains(ComponentID<ComponentFloat>.ID));
    }

    [TestMethod]
    public void SetTwiceOnNewEntityThrows()
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

        using var resolver = buffer.Playback();

        var entities = new[]
        {
            resolver.Resolve(buffered[0]),
            resolver.Resolve(buffered[1]),
            resolver.Resolve(buffered[2])
        };

        foreach (var entity in entities)
            Assert.IsTrue(entity.IsAlive(world));

        buffer.Delete(entities[1]);
        buffer.Playback();

        Assert.IsTrue(entities[0].IsAlive(world));
        Assert.IsFalse(entities[1].IsAlive(world));
        Assert.IsTrue(entities[2].IsAlive(world));

        Assert.AreEqual(1, world.GetComponentRef<ComponentFloat>(entities[0]).Value);
        Assert.AreEqual(3, world.GetComponentRef<ComponentFloat>(entities[2]).Value);
    }

    [TestMethod]
    public void DeleteEntities()
    {
        var world = new WorldBuilder().Build();
        var buffer = new CommandBuffer(world);

        var buffered = new[]
        {
            buffer.Create().Set(new ComponentFloat(1)),
            buffer.Create().Set(new ComponentFloat(2)),
            buffer.Create().Set(new ComponentFloat(3))
        };

        using var resolver = buffer.Playback();

        var entities = new[]
        {
            resolver.Resolve(buffered[0]),
            resolver.Resolve(buffered[1]),
            resolver.Resolve(buffered[2])
        };

        foreach (var entity in entities)
            Assert.IsTrue(entity.IsAlive(world));

        buffer.Delete([entities[0], entities[1]]);
        buffer.Playback();

        Assert.IsFalse(entities[0].IsAlive(world));
        Assert.IsFalse(entities[1].IsAlive(world));
        Assert.IsTrue(entities[2].IsAlive(world));

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
        using var resolver = buffer.Playback();
        var entity = resolver.Resolve(eb);

        // Remove a component
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
        using var resolver = buffer.Playback();
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
        using var resolver = buffer.Playback();
        var entity = resolver.Resolve(eb);

        // Overwrite one
        buffer.Set(entity, new ComponentInt16(789));
        buffer.Playback();

        // Check the value has changed
        Assert.IsTrue(world.HasComponent<ComponentFloat>(entity));
        Assert.IsTrue(world.HasComponent<ComponentInt16>(entity));
        Assert.AreEqual(789, world.GetComponentRef<ComponentInt16>(entity).Value);
    }

    [TestMethod]
    public void SetTwiceOnEntity()
    {
        var world = new WorldBuilder().Build();
        var buffer = new CommandBuffer(world);

        // Create an entity with 2 components
        var eb = buffer
                .Create()
                .Set(new ComponentFloat(123))
                .Set(new ComponentInt16(456));
        using var resolver = buffer.Playback();
        var entity = resolver.Resolve(eb);

        // Overwrite one, twice
        buffer.Set(entity, new ComponentInt16(789));
        buffer.Set(entity, new ComponentInt16(987));
        buffer.Playback();

        // Check the value has changed to the latest value
        Assert.IsTrue(world.HasComponent<ComponentFloat>(entity));
        Assert.IsTrue(world.HasComponent<ComponentInt16>(entity));
        Assert.AreEqual(987, world.GetComponentRef<ComponentInt16>(entity).Value);
    }

    [TestMethod]
    public void SetThenRemoveOnEntity()
    {
        var world = new WorldBuilder().Build();
        var buffer = new CommandBuffer(world);

        // Create an entity with 2 components
        var eb = buffer
                .Create()
                .Set(new ComponentFloat(123))
                .Set(new ComponentInt16(456));
        using var resolver = buffer.Playback();
        var entity = resolver.Resolve(eb);

        // Overwrite one
        buffer.Set(entity, new ComponentInt16(789));

        // Then remove it
        buffer.Remove<ComponentInt16>(entity);

        buffer.Playback();

        // Check the value is gone
        Assert.IsTrue(world.HasComponent<ComponentFloat>(entity));
        Assert.IsFalse(world.HasComponent<ComponentInt16>(entity));
    }

    [TestMethod]
    public void RemoveInvalidComponent()
    {
        var world = new WorldBuilder().Build();
        var buffer = new CommandBuffer(world);

        // Create an entity with 2 components
        var eb = buffer
                .Create()
                .Set(new ComponentFloat(123))
                .Set(new ComponentInt16(456));
        using var resolver = buffer.Playback();
        var entity = resolver.Resolve(eb);

        // Remove a component
        buffer.Remove<ComponentInt32>(entity);

        buffer.Playback();

        // Check entity is unchanged
        Assert.IsTrue(world.HasComponent<ComponentFloat>(entity));
        Assert.IsTrue(world.HasComponent<ComponentInt16>(entity));
        Assert.IsFalse(world.HasComponent<ComponentInt32>(entity));
    }

    [TestMethod]
    public void RemoveAndSetComponent()
    {
        var world = new WorldBuilder().Build();
        var buffer = new CommandBuffer(world);

        // Create an entity with 2 components
        var eb = buffer
                .Create()
                .Set(new ComponentFloat(123))
                .Set(new ComponentInt16(456));
        using var resolver = buffer.Playback();
        var entity = resolver.Resolve(eb);

        // Remove a component
        buffer.Remove<ComponentInt16>(entity);

        // Then set the same component!
        buffer.Set(entity, new ComponentInt16(789));

        buffer.Playback();

        // Check entity structure is unchanged
        Assert.IsTrue(world.HasComponent<ComponentFloat>(entity));
        Assert.IsTrue(world.HasComponent<ComponentInt16>(entity));
        Assert.IsFalse(world.HasComponent<ComponentInt32>(entity));

        // Check value is correct
        Assert.AreEqual(789, world.GetComponentRef<ComponentInt16>(entity).Value);
    }
}