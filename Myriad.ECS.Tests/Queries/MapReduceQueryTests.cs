﻿using Myriad.ECS.Command;
using Myriad.ECS.Queries;
using Myriad.ECS.Worlds;

namespace Myriad.ECS.Tests.Queries
{
    [TestClass]
    public class MapReduceQueryTests
    {
        [TestMethod]
        public void MapReduceNone()
        {
            var world = new WorldBuilder().Build();

            var result = world.ExecuteMapReduce<MapGetInteger, Reduce.I32.Mul, int, ComponentInt32>(new MapGetInteger(), new Reduce.I32.Mul(), 2);

            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void MapReduceOne()
        {
            var world = new WorldBuilder().Build();

            var cmd = new CommandBuffer(world);
            cmd.Create().Set(new ComponentInt32(7));
            cmd.Playback().Dispose();

            var result = world.ExecuteMapReduce<MapGetInteger, Reduce.I32.Mul, int, ComponentInt32>(new MapGetInteger(), new Reduce.I32.Mul(), 2);

            Assert.AreEqual(14, result);
        }

        [TestMethod]
        public void MapReduceMany()
        {
            var world = new WorldBuilder().Build();

            var cmd = new CommandBuffer(world);
            var sum = 0;
            for (var i = 0; i < 9999; i++)
            {
                sum += i;
                cmd.Create().Set(new ComponentInt32(i));
            }
            cmd.Playback().Dispose();

            var result = world.ExecuteMapReduce<MapGetInteger, Reduce.I32.Add, int, ComponentInt32>(0);

            Assert.AreEqual(sum, result);
        }

        private struct MapGetInteger
            : IQueryMap<int, ComponentInt32>
        {
            public readonly int Execute(Entity e, ref ComponentInt32 t0)
            {
                return t0.Value;
            }
        }
    }
}
