﻿using System.Reflection;
using Myriad.ECS.IDs;
using System.Diagnostics.CodeAnalysis;
using Myriad.ECS.Allocations;

namespace Myriad.ECS.Registry;

/// <summary>
/// Store a lookup from component type to unique 32 bit ID.
/// </summary>
public abstract class BaseRegistry<TBase, TID>
    where TID : struct, IIDNumber<TID>
{
    private static readonly Locks.RWLock<State> _lock = new(new State());

    /// <summary>
    /// Register all TBase in the given assembly
    /// </summary>
    /// <param name="assembly"></param>
    public static void Register(Assembly assembly)
    {
        Register([assembly]);
    }

    /// <summary>
    /// Register all TBase in all given assemblies
    /// </summary>
    /// <param name="assemblies"></param>
    public static void Register(params Assembly[] assemblies)
    {
        using (var locker = _lock.EnterWriteLock())
        {
            foreach (var assembly in assemblies)
            {
                var types = from type in assembly.GetTypes()
                            where typeof(TBase).IsAssignableFrom(type)
                            select type;

                foreach (var type in types)
                    locker.Value.GetOrAdd(type);
            }
        }
    }

    /// <summary>
    /// Register a specific component
    /// </summary>
    /// <param name="type"></param>
    public static void Register(Type type)
    {
        TypeCheck(type);

        using (var locker = _lock.EnterWriteLock())
        {
            locker.Value.GetOrAdd(type);
        }
    }

    /// <summary>
    /// Get the ID for the given type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static TID Get<T>()
        where T : TBase
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
    public static TID Get(Type type)
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
    public static Type Get(TID id)
    {
        using (var locker = _lock.EnterReadLock())
        {
            if (!locker.Value.TryGet(id, out var type))
                throw new InvalidOperationException("Unknown component ID");
            return type;
        }
    }

    private static void TypeCheck(Type type)
    {
        if (!typeof(TBase).IsAssignableFrom(type))
            throw new ArgumentException($"Type `{type.FullName}` is not assignable to `{typeof(TBase).Name}`)");
    }

    private class State
    {
        private readonly Dictionary<TID, Type> TypeLookup = [];
        private readonly Dictionary<Type, TID> IDLookup = [];

        // Init the first ID to be the one after the default ID. That
        // means that default is _not_ a valid ID.
        private TID _nextId = default(TID).Next();

        public TID GetOrAdd(Type type)
        {
            if (!IDLookup.TryGetValue(type, out var id))
            {
                id = _nextId;
                _nextId = _nextId.Next();

                IDLookup[type] = id;
                TypeLookup[id] = type;

                ArrayFactory.Prepare(type);
            }

            return id;
        }

        public bool TryGet(Type type, out TID id)
        {
            return IDLookup.TryGetValue(type, out id);
        }

        public bool TryGet(TID id, [MaybeNullWhen(false)] out Type type)
        {
            return TypeLookup.TryGetValue(id, out type);
        }
    }
}