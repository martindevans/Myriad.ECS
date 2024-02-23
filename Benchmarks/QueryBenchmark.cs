using System.Numerics;
using BenchmarkDotNet.Attributes;
using Benchmarks.Components;
using JetBrains.Annotations;
using Myriad.ECS;
using Myriad.ECS.Command;
using Myriad.ECS.Queries;
using Myriad.ECS.Worlds;

namespace Benchmarks;

[MemoryDiagnoser]
public class QueryBenchmark
{
    [Params(10_000, 100_000, 1_000_000), UsedImplicitly]
    public int EntityCount = 1_000_000;

    private World _world = null!;
    private QueryDescription _query = null!;

    [GlobalSetup]
    public void Setup()
    {
        _world = new WorldBuilder().Build();

        var buffer = new CommandBuffer(_world);
        var rng = new Random(2);
        for (var i = 0; i < EntityCount; i++)
            AddEntity(buffer, rng);
        using var resolver = buffer.Playback();

        _query = new QueryBuilder()
            .Include<Position>()
            .Include<Velocity>()
            .Build(_world);
    }

    private static void AddEntity(CommandBuffer buffer, Random random)
    {
        var entity = buffer.Create();

        entity.Set(new Position(new Vector2(random.NextSingle(), random.NextSingle())));
        entity.Set(new Velocity(new Vector2(random.NextSingle(), random.NextSingle())));

        for (var i = 0; i < 5; i++)
        {
            switch (random.Next(0, 5))
            {
                case 0: entity.Set(new ComponentByte((byte)i), true); break;
                case 1: entity.Set(new ComponentInt16((short)i), true); break;
                case 2: entity.Set(new ComponentFloat(i), true); break;
                case 3: entity.Set(new ComponentInt32(i), true); break;
                case 4: entity.Set(new ComponentInt64(i), true); break;
            }
        }
    }

    [Benchmark]
    public void Query()
    {
        _world.Execute(_query, new QueryAction());
    }

    [Benchmark]
    public void ChunkQuery()
    {
        _world.Execute(_query, new ChunkQueryAction());
    }

    private struct QueryAction
        : IQueryWR<Position, Velocity>
    {
        public readonly void Execute(Entity e, ref Position pos, ref readonly Velocity vel)
        {
            pos.Value += vel.Value;
        }
    }

    private struct ChunkQueryAction
        : IChunkQueryWR<Position, Velocity>
    {
        public readonly void Execute(ReadOnlySpan<Entity> e, Span<Position> pos, ReadOnlySpan<Velocity> vel)
        {
            for (var i = 0; i < pos.Length; i++)
            {
                pos[i].Value += vel[i].Value;
            }
        }
    }
}