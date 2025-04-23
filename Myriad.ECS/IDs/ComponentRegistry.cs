using System.Diagnostics.CodeAnalysis;
using Myriad.ECS.Allocations;
using Myriad.ECS.Components;

namespace Myriad.ECS.IDs;

/// <summary>
/// Store a lookup from component type to unique 32 bit ID.
/// </summary>
internal static class ComponentRegistry
{
    private static readonly Locks.RWLock<State> _lock = new(new State());

    /// <summary>
    /// Get the ID for the given type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static ComponentID Get<T>()
        where T : IComponent
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
    public static ComponentID Get(Type type)
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
    public static Type Get(ComponentID id)
    {
        using var locker = _lock.EnterReadLock();

        if (!locker.Value.TryGet(id, out var type))
            throw new InvalidOperationException("Unknown component ID");

        return type;
    }

    private static void TypeCheck(Type type)
    {
        if (!typeof(IComponent).IsAssignableFrom(type))
            throw new ArgumentException($"Type `{type.FullName}` is not assignable to `{nameof(IComponent)}`)");
    }

    private class State
    {
        private readonly Dictionary<ComponentID, Type> TypeLookup = [];
        private readonly Dictionary<Type, ComponentID> IDLookup = [];

        // Init the first ID to be the one after the default ID. That
        // means that default is _not_ a valid ID.
        private int _nextId = 1;

        public ComponentID GetOrAdd(Type type)
        {
            if (!IDLookup.TryGetValue(type, out var value))
            {
                var id = _nextId++;

                // Shift over the ID to make space for the special bits
                id <<= ComponentID.SpecialBitsCount;

                // Set the bit indicating that this component implements IPhantomComponent
                if (typeof(IPhantomComponent).IsAssignableFrom(type))
                    id |= ComponentID.IsPhantomComponentMask;

                // Set the bit indicating that this component implements IEntityRelationComponent
                if (typeof(IEntityRelationComponent).IsAssignableFrom(type))
                    id |= ComponentID.IsRelationComponentMask;

                // Set the bit indicating that this component implements IDisposableComponent
                if (typeof(IDisposableComponent).IsAssignableFrom(type))
                    id |= ComponentID.IsDisposableComponentMask;

                // Set the bit indicating that this component implements IPhantomNotifierComponent
                if (typeof(IPhantomNotifierComponent).IsAssignableFrom(type))
                    id |= ComponentID.IsPhantomNotifierComponentMask;

                // Store it for future lookups
                value = new ComponentID(id);
                IDLookup[type] = value;
                TypeLookup[value] = type;

                // Since we've discovered this component we're likely to need
                // arrays made for it later. Prepare the array factory for that.
                ArrayFactory.Prepare(type);
            }

            return value;
        }

        public bool TryGet(Type type, out ComponentID id)
        {
            return IDLookup.TryGetValue(type, out id);
        }

        public bool TryGet(ComponentID id, [MaybeNullWhen(false)] out Type type)
        {
            return TypeLookup.TryGetValue(id, out type);
        }
    }
}