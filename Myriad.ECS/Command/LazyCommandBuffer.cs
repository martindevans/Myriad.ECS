using Myriad.ECS.Worlds;
using System.Diagnostics.CodeAnalysis;

namespace Myriad.ECS.Command;

/// <summary>
/// Provides a CommandBuffer, which is lazily created the first time it is accessed
/// </summary>
public struct LazyCommandBuffer
{
    private CommandBuffer? _buffer;

    public World World { get; private set; }

    public LazyCommandBuffer(World world)
    {
        World = world;
        _buffer = null;
    }

    /// <summary>
    /// Get the buffer (constructing one if it does not yet exist)
    /// </summary>
    public CommandBuffer CommandBuffer
    {
        get
        {
            _buffer ??= World.GetPooledCommandBuffer();
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