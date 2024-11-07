using Myriad.ECS.Collections;
using Myriad.ECS.IDs;
using Myriad.ECS.Worlds;
using Myriad.ECS.Worlds.Archetypes;

namespace Myriad.ECS.Queries;

/// <summary>
/// Describes a query for entities, bound to a world.
/// </summary>
public sealed class QueryDescription
{
    // Cache of result from last time TryMatch was called
    private MatchResult? _result;
    private readonly ReaderWriterLockSlim _resultLock = new();
    private readonly OrderedListSet<ComponentID> _temporarySet = new();

    /// <summary>
    /// The World that this query is for
    /// </summary>
    public World World { get; }

    /// <summary>
    /// The components which must be present on an entity for it to match this query
    /// </summary>
    public FrozenOrderedListSet<ComponentID> Include { get; }

    /// <summary>
    /// The components which must not be present on an entity for it to match this query
    /// </summary>
    public FrozenOrderedListSet<ComponentID> Exclude { get; }

    /// <summary>
    /// At least one of these components must be present on an entity for it to match this query
    /// </summary>
    public FrozenOrderedListSet<ComponentID> AtLeastOneOf { get; }

    /// <summary>
    /// Exactly one of these components must be present on an entity for it to match this query
    /// </summary>
    public FrozenOrderedListSet<ComponentID> ExactlyOneOf { get; }

    /// <summary>
    /// Describes a query for entities, bound to a world.
    /// </summary>
    public QueryDescription(World world, FrozenOrderedListSet<ComponentID> include, FrozenOrderedListSet<ComponentID> exclude, FrozenOrderedListSet<ComponentID> atLeastOne, FrozenOrderedListSet<ComponentID> exactlyOne)
    {
        World = world;
        Include = include;
        Exclude = exclude;
        AtLeastOneOf = atLeastOne;
        ExactlyOneOf = exactlyOne;
    }

    /// <summary>
    /// Create a query builder which describes this query
    /// </summary>
    /// <returns></returns>
    public QueryBuilder ToBuilder()
    {
        var builder = new QueryBuilder();

        foreach (var id in Include)
            builder.Include(id);
        foreach (var id in Exclude)
            builder.Exclude(id);
        foreach (var id in AtLeastOneOf)
            builder.AtLeastOneOf(id);
        foreach (var id in ExactlyOneOf)
            builder.ExactlyOneOf(id);

        return builder;
    }

    #region is in query
    public bool IsIncluded<T>()
        where T : IComponent
    {
        return IsIncluded(ComponentID<T>.ID);
    }

    public bool IsIncluded(Type type)
    {
        return IsIncluded(ComponentID.Get(type));
    }

    public bool IsIncluded(ComponentID id)
    {
        return Include.Contains(id);
    }


    public bool IsExcluded<T>()
        where T : IComponent
    {
        return IsExcluded(ComponentID<T>.ID);
    }

    public bool IsExcluded(Type type)
    {
        return IsExcluded(ComponentID.Get(type));
    }

    public bool IsExcluded(ComponentID id)
    {
        return Exclude.Contains(id);
    }


    public bool IsAtLeastOneOf<T>()
        where T : IComponent
    {
        return IsAtLeastOneOf(ComponentID<T>.ID);
    }

    public bool IsAtLeastOneOf(Type type)
    {
        return IsAtLeastOneOf(ComponentID.Get(type));
    }

    public bool IsAtLeastOneOf(ComponentID id)
    {
        return AtLeastOneOf.Contains(id);
    }


    public bool IsExactlyOneOf<T>()
        where T : IComponent
    {
        return IsExactlyOneOf(ComponentID<T>.ID);
    }

    public bool IsExactlyOneOf(Type type)
    {
        return IsExactlyOneOf(ComponentID.Get(type));
    }

    public bool IsExactlyOneOf(ComponentID id)
    {
        return ExactlyOneOf.Contains(id);
    }
    #endregion

    #region match
    /// <summary>
    /// Get all archetypes which match this query
    /// </summary>
    /// <returns></returns>
    public FrozenOrderedListSet<ArchetypeMatch> GetArchetypes()
    {
        // Quick check if we already have a non-stale result
        _resultLock.EnterReadLock();
        try
        {
            if (_result != null && !_result.Value.IsStale(World))
                return _result.Value.Archetypes;
        }
        finally
        {
            _resultLock.ExitReadLock();
        }

        // We don't have a valid cached result, calculate it now
        _resultLock.EnterWriteLock();
        try
        {
            // If this query has never been evaluated before do it now
            if (_result == null)
            {
                // Check every archetype
                var matches = new List<ArchetypeMatch>();
                foreach (var item in World.Archetypes)
                    if (TryMatch(item) is ArchetypeMatch m)
                        matches.Add(m);

                // Store result for next time
                _result = new MatchResult(World.Archetypes.Count, new FrozenOrderedListSet<ArchetypeMatch>(matches));

                // Return matches
                return _result.Value.Archetypes;
            }

            // If the number of archetypes has changed since last time regenerate the cache
            if (_result.Value.IsStale(World))
            {
                // Lazy copy of the match set, in case there are no matches
                var copy = default(OrderedListSet<ArchetypeMatch>?);

                // Check every new archetype
                for (var i = _result.Value.ArchetypeWatermark; i < World.Archetypes.Count; i++)
                {
                    var m = TryMatch(World.Archetypes[i]);
                    if (m == null)
                        continue;

                    // Lazy copy the set now that we know we need it
                    copy ??= new OrderedListSet<ArchetypeMatch>(_result.Value.Archetypes);

                    // Add the match
                    copy.Add(m.Value);
                }

                if (copy == null)
                {
                    // Copy is null, that means nothing new was found, just use the old result with the new watermark
                    _result = new MatchResult(World.Archetypes.Count, _result.Value.Archetypes);
                }
                else
                {
                    // Create a new match result
                    _result = new MatchResult(World.Archetypes.Count, new FrozenOrderedListSet<ArchetypeMatch>(copy));
                }
            }

            return _result.Value.Archetypes;
        }
        finally
        {
            _resultLock.ExitWriteLock();
        }
    }

    private ArchetypeMatch? TryMatch(Archetype archetype)
    {
        var components = archetype.Components;

        if (!components.IsSupersetOf(Include))
            return null;

        if (components.Overlaps(Exclude))
            return null;

        // Use the temp hashset to do this
        var set = _temporarySet;
        set.Clear();

        // Check if there are any "exactly one" items
        var exactlyOne = default(ComponentID?);
        if (ExactlyOneOf.Count > 0)
        {
            set.Clear();
            set.UnionWith(components);
            set.IntersectWith(ExactlyOneOf);
            if (set.Count != 1)
            {
                set.Clear();
                return null;
            }

            exactlyOne = set.Single();
            set.Clear();
        }

        // Check if there are any "at least one" items
        if (AtLeastOneOf.Count > 0)
        {
            set.Clear();
            set.UnionWith(components);
            set.IntersectWith(AtLeastOneOf);
            if (set.Count == 0)
            {
                set.Clear();
                return null;
            }
        }
        else
        {
            set.Clear();
            set = null;
        }

        var atLeastOne = set?.Freeze();

        return new ArchetypeMatch(archetype, atLeastOne, exactlyOne);
    }

    private readonly struct MatchResult(int watermark, FrozenOrderedListSet<ArchetypeMatch> archetypes)
    {
        /// <summary>
        /// The archetypes matching this query
        /// </summary>
        public FrozenOrderedListSet<ArchetypeMatch> Archetypes { get; } = archetypes;

        /// <summary>
        /// The number of archetypes in the world when this cache was created
        /// </summary>
        public int ArchetypeWatermark { get; } = watermark;

        public bool IsStale(World world)
        {
            return ArchetypeWatermark < world.ArchetypesCount;
        }
    }

    /// <summary>
    /// An archetype which matches a query
    /// </summary>
    /// <param name="Archetype">The archetype</param>
    /// <param name="AtLeastOne">All of the "at least one" components present (if there are any in this query)</param>
    /// <param name="ExactlyOne">The "exactly one" component present (if there is one in this query)</param>
    public readonly record struct ArchetypeMatch(Archetype Archetype, FrozenOrderedListSet<ComponentID>? AtLeastOne, ComponentID? ExactlyOne)
        : IComparable<ArchetypeMatch>
    {
        public int CompareTo(ArchetypeMatch other)
        {
            return Archetype.Hash.CompareTo(other.Archetype.Hash);
        }
    }
    #endregion

    /// <summary>
    /// Count how many entities match this query
    /// </summary>
    /// <returns></returns>
    public int Count()
    {
        var count = 0;
        foreach (var archetype in GetArchetypes())
            count += archetype.Archetype.EntityCount;
        return count;
    }

    /// <summary>
    /// Check if this query matches any entities
    /// </summary>
    /// <returns></returns>
    public bool Any()
    {
        foreach (var archetype in GetArchetypes())
            if (archetype.Archetype.EntityCount > 0)
                return true;
        return false;
    }

    /// <summary>
    /// Get the first entity which this query matches (or null)
    /// </summary>
    /// <returns></returns>
    public Entity? FirstOrDefault()
    {
        foreach (var archetype in GetArchetypes())
        {
            if (archetype.Archetype.EntityCount > 0)
            {
                for (var i = 0; i < archetype.Archetype.Chunks.Count; i++)
                {
                    var chunk = archetype.Archetype.Chunks[i];
                    if (chunk.EntityCount > 0)
                        return chunk.Entities.Span[0];
                }
            }
        }

        return default;
    }

    #region bulk write
    /// <summary>
    /// Overwrite a component for every entity which matches this query
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="item"></param>
    /// <returns>The number of entities written to</returns>
    public int Overwrite<T>(T item)
        where T : IComponent
    {
        var id = ComponentID<T>.ID;

        // Can't do any work if this item is specifically not in this query
        if (IsExcluded(id))
            return 0;

        var count = 0;
        foreach (var archetype in GetArchetypes())
        {
            if (!archetype.Archetype.Components.Contains(id))
                continue;

            count += archetype.Archetype.EntityCount;

            using var chunks = archetype.Archetype.GetChunkEnumerator();
            while (chunks.MoveNext())
            {
                var chunk = chunks.Current;
                var arr = chunk.GetSpan<T>(id);
                arr.Fill(item);
            }
        }

        return count;
    }
    #endregion
}