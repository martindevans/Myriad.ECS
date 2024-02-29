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
[ShortRunJob]
public class QueryBenchmark
{
    [Params(100_000, 1_000_000, 10_000_000), UsedImplicitly]
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

        Console.WriteLine("Setup Complete");
    }

    private static void AddEntity(CommandBuffer buffer, Random random)
    {
        var entity = buffer.Create();

        entity.Set(new Position(new Vector2(random.NextSingle(), random.NextSingle())));
        entity.Set(new Velocity(new Vector2(random.NextSingle(), random.NextSingle())));

        for (var i = 0; i < 10; i++)
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
        _world.Execute<QueryAction, Position, Velocity>(new QueryAction(), _query);
    }

    [Benchmark]
    public void ChunkQuery()
    {
        _world.ExecuteChunk<ChunkQueryAction, Position, Velocity>(new ChunkQueryAction(), _query);
    }

    [Benchmark]
    public void ParallelQuery()
    {
        _world.ExecuteParallel<QueryAction, Position, Velocity>(new QueryAction(), _query);
    }

    [Benchmark]
    public void ParallelChunkQuery()
    {
        _world.ExecuteChunkParallel<ChunkQueryAction, Position, Velocity>(new ChunkQueryAction(), _query);
    }

    [Benchmark]
    public void QueryEnumerable()
    {
        foreach (var item in _world.Query<Position, Velocity>(_query))
            item.Item0.Value += item.Item1.Value;
    }

    private struct QueryAction
        : IQuery2<Position, Velocity>
    {
        public readonly void Execute(Entity e, ref Position pos, ref Velocity vel)
        {
            pos.Value += vel.Value;
        }
    }

    private struct ChunkQueryAction
        : IChunkQuery2<Position, Velocity>
    {
        public readonly void Execute(ReadOnlySpan<Entity> e, Span<Position> pos, Span<Velocity> vel)
        {
            for (var i = 0; i < pos.Length; i++)
            {
                pos[i].Value += vel[i].Value;
            }
        }
    }
}