﻿using Myriad.ECS.Threading;
using Myriad.ECS.Worlds;

namespace Myriad.ECS.Tests;

[TestClass]
public class WorldBuilderTests
{
    [TestMethod]
    public void CannotSetThreadpoolTwice()
    {
        Assert.ThrowsException<InvalidOperationException>(() =>
        {
            new WorldBuilder().WithThreadPool(new DefaultThreadPool()).WithThreadPool(new DefaultThreadPool());
        });
    }

    [TestMethod]
    public void AddArchetypeThrowsWithDuplicateTypesGeneric()
    {
        Assert.ThrowsException<ArgumentException>(() =>
        {
            new WorldBuilder().WithArchetype<ComponentInt32, ComponentInt32>();
        });
    }

    [TestMethod]
    public void AddArchetypeThrowsWithDuplicateTypes()
    {
        Assert.ThrowsException<ArgumentException>(() =>
        {
            new WorldBuilder().WithArchetype(typeof(ComponentInt32), typeof(ComponentInt32));
        });
    }
}