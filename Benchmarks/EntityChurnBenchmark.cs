using BenchmarkDotNet.Attributes;
using Benchmarks.Components;
using Myriad.ECS;
using Myriad.ECS.Command;
using Myriad.ECS.Worlds;

namespace Benchmarks;

[MemoryDiagnoser]
public class EntityChurnBenchmark
{
    private World _world = null!;
    private CommandBuffer _buffer = null!;
    private readonly Random _rng = new(46576);
    private readonly List<CommandBuffer.BufferedEntity> _buffered = [ ];

    [GlobalSetup]
    public void Setup()
    {
        _world = new WorldBuilder().Build();
        _buffer = new CommandBuffer(_world);
    }

    [Benchmark]
    public void Churn()
    {
        // keep track ofevery single entity ever created
        var alive = new List<Entity>();

        // Do lots of rounds of creation and destruction
        for (var i = 0; i < 500000; i++)
        {
            // Create lots of entities
            for (var j = 0; j < 100; j++)
                _buffered.Add(_buffer.Create().Set(new ComponentInt32(j)));

            // Destroy some random entities
            for (var j = 0; j < alive.Count; j++)
            {
                var ent = alive[j];
                _buffer.Delete(ent);
            }
            alive.Clear();

            // Execute
            using var resolver = _buffer.Playback();

            // Resolve results
            foreach (var b in _buffered)
                if (_rng.NextSingle() > 0.25f)
                    alive.Add(resolver.Resolve(b));
            _buffered.Clear();
        }
    }
}