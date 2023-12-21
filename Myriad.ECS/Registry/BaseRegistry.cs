using Myriad.ParallelTasks.Locks;
using System.Reflection;
using Myriad.ECS.IDs;

namespace Myriad.ECS.Registry;

/// <summary>
/// Store a lookup from component type to unique 32 bit ID.
/// </summary>
public abstract class BaseRegistry<TBase, TID>
    where TID : struct, IIDNumber<TID>
{
    private static readonly RWLock<State> _lock = new(new State());

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
                            where type.IsAssignableTo(typeof(TBase))
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

    private static void TypeCheck(Type type)
    {
        if (!type.IsAssignableTo(typeof(TBase)))
            throw new ArgumentException($"Type `{type.FullName}` is not assignable to `{typeof(TBase).Name}`)");
    }

    private class State
    {
        private readonly Dictionary<Type, TID> IDLookup = [];
        private TID _nextId = TID.First();

        public TID GetOrAdd(Type type)
        {
            if (!IDLookup.TryGetValue(type, out var id))
            {
                id = _nextId;
                _nextId = TID.Next(_nextId);

                IDLookup[type] = id;
            }

            return id;
        }

        public bool TryGet(Type type, out TID id)
        {
            return IDLookup.TryGetValue(type, out id);
        }
    }
}