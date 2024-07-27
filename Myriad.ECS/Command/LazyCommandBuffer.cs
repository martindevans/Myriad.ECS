using Myriad.ECS.Worlds;
using System;

namespace Myriad.ECS.Command;

public struct LazyCommandBuffer
{
    private readonly World _world;
    private CommandBuffer? _buffer;

    public LazyCommandBuffer(World world)
    {
        _world = world;
        _buffer = null;
    }

    public CommandBuffer CommandBuffer
    {
        get
        {
            if (_buffer == null)
                _buffer = _world.GetPooledCommandBuffer();
            return _buffer;
        }
    }

    internal CommandBuffer? Get()
    {
        return _buffer;
    }
}