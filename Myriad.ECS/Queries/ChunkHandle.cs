using Myriad.ECS.IDs;
using Myriad.ECS.Worlds.Archetypes;
using Myriad.ECS.Worlds.Chunks;

namespace Myriad.ECS.Queries;

/// <summary>
/// Temporary handle to a specific chunk
/// </summary>
public readonly ref partial struct ChunkHandle
{
    private readonly Chunk _chunk;

    /// <summary>
    /// The archetype this chunk belongs to
    /// </summary>
    public Archetype Archetype => _chunk.Archetype;

    /// <summary>
    /// Get the total number of entities in this chunk
    /// </summary>
    public readonly int EntityCount;

    /// <summary>
    /// Get the entities in this chunk
    /// </summary>
    public ReadOnlyMemory<Entity> Entities => _chunk.Entities;

    internal ChunkHandle(Chunk chunk)
    {
        _chunk = chunk;

        EntityCount = _chunk.EntityCount;
    }

    /// <summary>
    /// Test if this chunk contains a specific component
    /// </summary>
    /// <typeparam name="T">Component type</typeparam>
    /// <returns></returns>
    public bool HasComponent<T>()
        where T : IComponent
    {
        return HasComponent(ComponentID<T>.ID);
    }

    /// <summary>
    /// Test if this chunk contains a specific component
    /// </summary>
    /// <param name="id">Component type</param>
    /// <returns></returns>
    private bool HasComponent(ComponentID id)
    {
        return Archetype.Components.Contains(id);
    }

    /// <summary>
    /// Try to get the span of the given component type in this chunk
    /// </summary>
    /// <returns></returns>
    public Span<T> GetComponentSpan<T>()
        where T : IComponent
    {
        return _chunk.GetSpan<T>();
    }

    /// <summary>
    /// Set the value of the given chunk flag
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    public void SetBit<T>(bool value)
        where T : IChunkBitFlag
    {
        _chunk.SetFlag<T>(value);
    }

    /// <summary>
    /// Get the value of the given chunk flag
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public bool GetBit<T>()
        where T : IChunkBitFlag
    {
        return _chunk.GetFlag<T>();
    }

    /// <summary>
    /// Unlock unsafe access to the chunk
    /// </summary>
    /// <returns></returns>
    public Unsafe Danger()
    {
        return new Unsafe(_chunk);
    }

    /// <summary>
    /// Unsafe things to do with a chunk handle
    /// </summary>
    public readonly struct Unsafe
    {
        private readonly Chunk _chunk;

        internal Unsafe(Chunk chunk)
        {
            _chunk = chunk;
        }

        /// <summary>
        /// Get the raw array storing component data. Accessing beyond the entity count will be junk data.
        /// </summary>
        /// <returns></returns>
        public T[] GetComponentArray<T>()
            where T : IComponent
        {
            return _chunk.GetComponentArray<T>();
        }

        /// <summary>
        /// Get the raw array storing entity data. Accessing beyond the entity count will be junk data.
        /// </summary>
        /// <returns></returns>
        public Entity[] GetEntityArray()
        {
            return _chunk.GetEntityArray();
        }
    }
}