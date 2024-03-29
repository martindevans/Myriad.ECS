using BenchmarkDotNet.Attributes;
using Benchmarks.Components;
using Myriad.ECS;
using Myriad.ECS.Command;
using Myriad.ECS.Worlds;

namespace Benchmarks;

[MemoryDiagnoser]
[ShortRunJob]
public class EntityModifyBenchmark
{
    private const int COUNT = 1_000_000;

    private World _world = null!;
    private readonly List<Entity> _entities = [];
    private CommandBuffer _ready = null!;

    [GlobalSetup]
    public void Setup()
    {
        _world = new WorldBuilder().Build();

        // Create initial entities
        var rng = new Random(1);
        var buffer = new CommandBuffer(_world);
        for (var i = 0; i < COUNT; i++)
            CreateEntity(buffer, rng);
        using var resolver = buffer.Playback();
        for (var i = 0; i < resolver.Count; i++)
            _entities.Add(resolver[i]);
    }

    private static void CreateEntity(CommandBuffer buffer, Random random)
    {
        var entity = buffer.Create();

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

    private static void ModifyEntity(CommandBuffer buffer, Random random, Entity entity)
    {
        for (var i = 0; i < 5; i++)
        {
            switch (random.Next(0, 5))
            {
                case 0: buffer.Set(entity, new ComponentByte((byte)i)); break;
                case 1: buffer.Set(entity, new ComponentInt16((short)i)); break;
                case 2: buffer.Set(entity, new ComponentFloat(i)); break;
                case 3: buffer.Set(entity, new ComponentInt32(i)); break;
                case 4: buffer.Set(entity, new ComponentInt64(i)); break;
            }
        }
    }

    [IterationSetup]
    public void IterationSetup()
    {
        // Setup modifications in a buffer
        var rng = new Random();
        var ready = new CommandBuffer(_world);
        foreach (var entity in _entities)
            ModifyEntity(ready, rng, entity);

        _ready = ready;
    }

    [Benchmark]
    public void Playback()
    {
        _ready.Playback().Dispose();
    }
}