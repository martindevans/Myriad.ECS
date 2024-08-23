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
        // keep track of every single entity currently alive
        var alive = new List<Entity>();

        // Do lots of rounds of creation and destruction
        for (var i = 0; i < 500000; i++)
        {
            // Create some entities
            for (var j = 0; j < 100; j++)
                _buffered.Add(_buffer.Create().Set(new ComponentInt32(j)));

            // Destroy all previously created entities
            _buffer.Delete(alive);
            alive.Clear();

            // Execute
            using var resolver = _buffer.Playback();

            // Resolve results
            foreach (var b in _buffered)
                alive.Add(b.Resolve());
            _buffered.Clear();
        }
    }
}