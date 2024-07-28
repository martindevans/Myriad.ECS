using Myriad.ECS.Worlds;
using System.Diagnostics.CodeAnalysis;

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

    /// <summary>
    /// Get the buffer, constructing one if it does not yet exist
    /// </summary>
    public CommandBuffer CommandBuffer
    {
        get
        {
            if (_buffer == null)
                _buffer = _world.GetPooledCommandBuffer();
            return _buffer;
        }
    }

    /// <summary>
    /// Get the buffer, or null if it does not yet exist
    /// </summary>
    /// <param name="buffer"></param>
    /// <returns></returns>
    public readonly bool TryGetBuffer([NotNullWhen(true)] out CommandBuffer? buffer)
    {
        buffer = _buffer;
        return buffer != null;
    }
}