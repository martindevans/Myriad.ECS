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
    public void DisposeResolverTwiceThrows()
    {
        var world = new WorldBuilder().Build();
        var buffer = new CommandBuffer(world);

        var r = buffer.Playback();
        r.Dispose();

        Assert.ThrowsException<ObjectDisposedException>(() =>
        {
            r.Dispose();
        });
    }

    [TestMethod]
    public void CreateEntity()
    {
        var world = new WorldBuilder().Build();
        var buffer = new CommandBuffer(world);

        var eb = buffer.Create();

        using var resolver = buffer.Playback();
        var entity = eb.Resolve();

        Assert.IsTrue(entity.Exists());
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
            entities.Add(b.Resolve());

        for (var i = 0; i < entities.Count; i++)
        {
            var entity = entities[i];
            Assert.IsTrue(entity.Exists());
            Assert.AreEqual(1, world.Archetypes.Count);
            Assert.AreEqual(1, world.Archetypes.Single().Components.Count);
            Assert.AreEqual(i, entity.GetComponentRef<ComponentInt32>().Value);
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
                Assert.IsTrue(ent.Exists());
                buffer.Delete(ent);
                alive.RemoveAt(idx);
                dead.Add(ent);
            }

            // Execute
            using (buffer.Playback())
            {
                // Resolve results
                foreach (var b in buffered)
                    alive.Add(b.Resolve());
            }

            // Check all the entities
            for (var j = 0; j < alive.Count; j++)
            {
                var entity = alive[j];
                Assert.IsTrue(entity.Exists());
            }
            for (var j = 0; j < dead.Count; j++)
            {
                var entity = dead[j];
                Assert.IsTrue(!entity.Exists());
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
            eb.Resolve();
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
        buffer1.Playback().Dispose();

        // Also run the other buffer to get another resolver
        using var resolver2 = buffer2.Playback();

        // Resolve the entity ID using the wrong resolver
        Assert.ThrowsException<InvalidOperationException>(() =>
        {
            eb1.Resolve();
        });
    }

    [TestMethod]
    public void CreateEntityCannotResolveFromPreviousPlayback()
    {
        var world = new WorldBuilder().Build();
        var buffer1 = new CommandBuffer(world);

        // Create the entity
        var eb1 = buffer1.Create();
        buffer1.Playback().Dispose();

        // Re-use that buffer
        using var resolver2 = buffer1.Playback();

        // Resolve the entity ID using the wrong resolver
        Assert.ThrowsException<ObjectDisposedException>(() =>
        {
            eb1.Resolve();
        });
    }

    [TestMethod]
    public void ModifyBufferedEntityAfterPlaybackThrows()
    {
        var world = new WorldBuilder().Build();
        var buffer1 = new CommandBuffer(world);

        // Create the entity
        var eb1 = buffer1.Create();
        using var resolver = buffer1.Playback();

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

        using var resolver = buffer.Playback();
        var entity = eb.Resolve();

        Assert.IsTrue(entity.Exists());
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
            buffered[0].Resolve(),
            buffered[1].Resolve(),
            buffered[2].Resolve(),
        };

        foreach (var entity in entities)
            Assert.IsTrue(entity.Exists());

        buffer.Delete(entities[1]);
        buffer.Playback();

        Assert.IsTrue(entities[0].Exists());
        Assert.IsFalse(entities[1].Exists());
        Assert.IsTrue(entities[2].Exists());

        Assert.AreEqual(1, entities[0].GetComponentRef<ComponentFloat>().Value);
        Assert.AreEqual(3, entities[2].GetComponentRef<ComponentFloat>().Value);
    }

    [TestMethod]
    public void DeleteEntityTwice()
    {
        var world = new WorldBuilder().Build();
        var buffer = new CommandBuffer(world);

        var buffered = buffer.Create().Set(new ComponentFloat(1));
        using var resolver = buffer.Playback();
        var entity = buffered.Resolve();
        Assert.IsTrue(entity.Exists());

        buffer.Delete(entity);
        buffer.Delete(entity);
        buffer.Delete(entity);
        buffer.Delete(entity);
        buffer.Delete(entity);
        buffer.Playback();

        Assert.IsFalse(entity.Exists());
    }

    [TestMethod]
    public void DeleteDeadEntity()
    {
        var world = new WorldBuilder().Build();
        var buffer = new CommandBuffer(world);

        // Create an entity
        var buffered = buffer.Create().Set(new ComponentFloat(1));
        using var resolver = buffer.Playback();
        var entity = buffered.Resolve();
        Assert.IsTrue(entity.Exists());

        // Setup deletion for that entity
        buffer.Delete(entity);

        // Delete that entity before playing back the first buffer
        var buffer2 = new CommandBuffer(world);
        buffer2.Delete(entity);
        buffer2.Playback().Dispose();
        Assert.IsFalse(entity.Exists());

        // Now play the first buffer back
        buffer.Playback().Dispose();

        Assert.IsFalse(entity.Exists());
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
            buffered[0].Resolve(),
            buffered[1].Resolve(),
            buffered[2].Resolve(),
        };

        foreach (var entity in entities)
            Assert.IsTrue(entity.Exists());

        buffer.Delete([entities[0], entities[1]]);
        buffer.Playback();

        Assert.IsFalse(entities[0].Exists());
        Assert.IsFalse(entities[1].Exists());
        Assert.IsTrue(entities[2].Exists());

        Assert.AreEqual(3, entities[2].GetComponentRef<ComponentFloat>().Value);
    }

    [TestMethod]
    public void ModifyThenDelete()
    {
        var world = new WorldBuilder().Build();
        var buffer = new CommandBuffer(world);

        // Create entity
        var buffered = buffer.Create().Set(new ComponentFloat(1));
        using var resolver = buffer.Playback();
        var entity = buffered.Resolve();
        Assert.IsTrue(entity.Exists());

        // Modify it _and_ delete it
        buffer.Set(entity, new ComponentInt64(7));
        buffer.Remove<ComponentFloat>(entity);
        buffer.Delete(entity);

        buffer.Playback().Dispose();

        // Check it's dead
        Assert.IsFalse(entity.Exists());
        Assert.AreEqual(0, world.Count<ComponentFloat>());
        Assert.AreEqual(0, world.Count<ComponentInt16>());
        Assert.AreEqual(0, world.Count<ComponentInt32>());
        Assert.AreEqual(0, world.Count<ComponentInt64>());
        Assert.AreEqual(0, world.Count<ComponentFloat>());
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
        var entity = eb.Resolve();

        // Remove a component
        buffer.Remove<ComponentInt16>(entity);
        buffer.Playback();

        Assert.AreEqual(123, entity.GetComponentRef<ComponentFloat>().Value);
        Assert.IsFalse(entity.HasComponent<ComponentInt16>());
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
        var entity = eb.Resolve();

        // Add a third
        buffer.Set(entity, new ComponentInt32(789));
        buffer.Playback();

        // Check they are all present
        Assert.IsTrue(entity.HasComponent<ComponentFloat>());
        Assert.IsTrue(entity.HasComponent<ComponentInt16>());
        Assert.IsTrue(entity.HasComponent<ComponentInt32>());
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
        var entity = eb.Resolve();

        // Overwrite one
        buffer.Set(entity, new ComponentInt16(789));
        buffer.Playback();

        // Check the value has changed
        Assert.IsTrue(entity.HasComponent<ComponentFloat>());
        Assert.IsTrue(entity.HasComponent<ComponentInt16>());
        Assert.AreEqual(789, entity.GetComponentRef<ComponentInt16>().Value);
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
        var entity = eb.Resolve();

        // Overwrite one, twice
        buffer.Set(entity, new ComponentInt16(789));
        buffer.Set(entity, new ComponentInt16(987));
        buffer.Playback();

        // Check the value has changed to the latest value
        Assert.IsTrue(entity.HasComponent<ComponentFloat>());
        Assert.IsTrue(entity.HasComponent<ComponentInt16>());
        Assert.AreEqual(987, entity.GetComponentRef<ComponentInt16>().Value);
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
        var entity = eb.Resolve();

        // Overwrite one
        buffer.Set(entity, new ComponentInt16(789));

        // Then remove it
        buffer.Remove<ComponentInt16>(entity);

        buffer.Playback();

        // Check the value is gone
        Assert.IsTrue(entity.HasComponent<ComponentFloat>());
        Assert.IsFalse(entity.HasComponent<ComponentInt16>());
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
        var entity = eb.Resolve();

        // Remove a component
        buffer.Remove<ComponentInt32>(entity);

        buffer.Playback();

        // Check entity is unchanged
        Assert.IsTrue(entity.HasComponent<ComponentFloat>());
        Assert.IsTrue(entity.HasComponent<ComponentInt16>());
        Assert.IsFalse(entity.HasComponent<ComponentInt32>());
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
        var entity = eb.Resolve();

        // Remove a component
        buffer.Remove<ComponentInt16>(entity);

        // Then set the same component!
        buffer.Set(entity, new ComponentInt16(789));

        buffer.Playback();

        // Check entity structure is unchanged
        Assert.IsTrue(entity.HasComponent<ComponentFloat>());
        Assert.IsTrue(entity.HasComponent<ComponentInt16>());
        Assert.IsFalse(entity.HasComponent<ComponentInt32>());

        // Check value is correct
        Assert.AreEqual(789, entity.GetComponentRef<ComponentInt16>().Value);
    }

    [TestMethod]
    public void CreateManyArchetypes()
    {
        var world = new WorldBuilder().Build();
        var buffer = new CommandBuffer(world);

        // Create entities in lots of different archetypes. The idea is to
        // create so many the entity runs out of aggregation buffers.

        var buffered = new List<CommandBuffer.BufferedEntity>();
        var rng = new Random(17);
        for (var i = 0; i < 1024; i++)
        {
            var eb = buffer.Create();
            buffered.Add(eb);

            for (var j = 0; j < 4; j++)
            {
                switch (rng.Next(18))
                {
                    case 0: eb.Set(new Component0(), true); break;
                    case 1: eb.Set(new Component1(), true); break;
                    case 2: eb.Set(new Component2(), true); break;
                    case 3: eb.Set(new Component3(), true); break;
                    case 4: eb.Set(new Component4(), true); break;
                    case 5: eb.Set(new Component5(), true); break;
                    case 6: eb.Set(new Component6(), true); break;
                    case 7: eb.Set(new Component7(), true); break;
                    case 8: eb.Set(new Component8(), true); break;
                    case 9: eb.Set(new Component9(), true); break;
                    case 10: eb.Set(new Component10(), true); break;
                    case 11: eb.Set(new Component11(), true); break;
                    case 12: eb.Set(new Component12(), true); break;
                    case 13: eb.Set(new Component13(), true); break;
                    case 14: eb.Set(new Component14(), true); break;
                    case 15: eb.Set(new Component15(), true); break;
                    case 16: eb.Set(new Component16(), true); break;
                    case 17: eb.Set(new Component17(), true); break;
                }
            }
        }

        using var resolver = buffer.Playback();
        Assert.AreEqual(1024, resolver.Count);

        // Ensure this is identical to the loop above!
        rng = new Random(17);
        for (var i = 0; i < buffered.Count; i++)
        {
            var entity = buffered[i].Resolve();

            for (var j = 0; j < 4; j++)
            {
                switch (rng.Next(18))
                {
                    case 0: Assert.IsTrue(entity.HasComponent<Component0>()); break;
                    case 1: Assert.IsTrue(entity.HasComponent<Component1>()); break;
                    case 2: Assert.IsTrue(entity.HasComponent<Component2>()); break;
                    case 3: Assert.IsTrue(entity.HasComponent<Component3>()); break;
                    case 4: Assert.IsTrue(entity.HasComponent<Component4>()); break;
                    case 5: Assert.IsTrue(entity.HasComponent<Component5>()); break;
                    case 6: Assert.IsTrue(entity.HasComponent<Component6>()); break;
                    case 7: Assert.IsTrue(entity.HasComponent<Component7>()); break;
                    case 8: Assert.IsTrue(entity.HasComponent<Component8>()); break;
                    case 9: Assert.IsTrue(entity.HasComponent<Component9>()); break;
                    case 10: Assert.IsTrue(entity.HasComponent<Component10>()); break;
                    case 11: Assert.IsTrue(entity.HasComponent<Component11>()); break;
                    case 12: Assert.IsTrue(entity.HasComponent<Component12>()); break;
                    case 13: Assert.IsTrue(entity.HasComponent<Component13>()); break;
                    case 14: Assert.IsTrue(entity.HasComponent<Component14>()); break;
                    case 15: Assert.IsTrue(entity.HasComponent<Component15>()); break;
                    case 16: Assert.IsTrue(entity.HasComponent<Component16>()); break;
                    case 17: Assert.IsTrue(entity.HasComponent<Component17>()); break;
                }
            }
        }
    }
}