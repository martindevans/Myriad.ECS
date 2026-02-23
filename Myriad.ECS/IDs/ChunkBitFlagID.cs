using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Myriad.ECS.IDs;

/// <summary>
/// Tag interface for chunk bit flag types
/// </summary>
public interface IChunkBitFlag;

/// <summary>
/// Unique numeric ID for a type which implements IChunkBitFlag
/// </summary>
[DebuggerDisplay("{Type} ({Value})")]
public readonly record struct ChunkBitFlagID
    : IComparable<ChunkBitFlagID>
{
    /// <summary>
    /// Get the raw value of this ID
    /// </summary>
    public int Value { get; }

    /// <summary>
    /// The <see cref="System.Type"/> of the flag this ID is for
    /// </summary>
    public Type Type => ChunkBitFlagRegistry.Get(this);

    internal ChunkBitFlagID(int value)
    {
        Value = value;
    }

    /// <inheritdoc />
    public int CompareTo(ChunkBitFlagID other)
    {
        return Value.CompareTo(other.Value);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"B{Value}";
    }

    /// <summary>
    /// Get the component ID for the given type
    /// </summary>
    /// <param name="type"></param>
    /// <exception cref="ArgumentException">Thrown if 'type' does not implement <see cref="IChunkBitFlag"/></exception>
    /// <returns></returns>
    public static ChunkBitFlagID Get(Type type)
    {
        return ChunkBitFlagRegistry.Get(type);
    }
}

/// <summary>
/// Retrieve the component ID for a type
/// </summary>
/// <typeparam name="TBitFlag"></typeparam>
public static class ChunkBitFlagID<TBitFlag>
    where TBitFlag : IChunkBitFlag
{
    /// <summary>
    /// The ID for <typeparamref name="TBitFlag" />
    /// </summary>
    public static readonly ChunkBitFlagID ID = ChunkBitFlagRegistry.Get<TBitFlag>();
}

/// <summary>
/// Store a lookup from chunk bit flag type to unique 32 bit ID.
/// </summary>
internal static class ChunkBitFlagRegistry
{
    private static readonly Locks.RWLock<State> _lock = new(new State());

    /// <summary>
    /// Get the ID for the given type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static ChunkBitFlagID Get<T>()
        where T : IChunkBitFlag
    {
        var type = typeof(T);

        using (var locker = _lock.EnterReadLock())
        {
            if (locker.Value.TryGet(type, out var value))
                return value;
        }

        using (var locker = _lock.EnterWriteLock())
        {
            return locker.Value.GetOrAdd(type);
        }
    }

    /// <summary>
    /// Get the ID for the given type
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static ChunkBitFlagID Get(Type type)
    {
        TypeCheck(type);

        using (var locker = _lock.EnterReadLock())
        {
            if (locker.Value.TryGet(type, out var value))
                return value;
        }

        using (var locker = _lock.EnterWriteLock())
        {
            return locker.Value.GetOrAdd(type);
        }
    }

    /// <summary>
    /// Get the type for a given ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static Type Get(ChunkBitFlagID id)
    {
        using var locker = _lock.EnterReadLock();

        if (!locker.Value.TryGet(id, out var type))
            throw new InvalidOperationException("Unknown component ID");

        return type;
    }

    private static void TypeCheck(Type type)
    {
        if (!typeof(IChunkBitFlag).IsAssignableFrom(type))
            throw new ArgumentException($"Type `{type.FullName}` is not assignable to `{nameof(IChunkBitFlag)}`)");
    }

    private class State
    {
        private readonly Dictionary<ChunkBitFlagID, Type> TypeLookup = [];
        private readonly Dictionary<Type, ChunkBitFlagID> IDLookup = [];

        // Init the first ID to be the one after the default ID. That
        // means that default is _not_ a valid ID.
        private int _nextId = 1;

        public ChunkBitFlagID GetOrAdd(Type type)
        {
            if (!IDLookup.TryGetValue(type, out var value))
            {
                var id = _nextId++;

                // Store it for future lookups
                value = new ChunkBitFlagID(id);
                IDLookup[type] = value;
                TypeLookup[value] = type;
            }

            return value;
        }

        public bool TryGet(Type type, out ChunkBitFlagID id)
        {
            return IDLookup.TryGetValue(type, out id);
        }

        public bool TryGet(ChunkBitFlagID id, [MaybeNullWhen(false)] out Type type)
        {
            return TypeLookup.TryGetValue(id, out type);
        }
    }
}