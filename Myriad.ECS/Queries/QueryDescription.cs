using Myriad.ECS.Collections;
using Myriad.ECS.Components;
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
    private readonly OrderedListSet<ComponentID> _temporarySet = [];

    private readonly ComponentBloomFilter _includeBloom;
    private readonly ComponentBloomFilter _excludeBloom;

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
    internal QueryDescription(World world, FrozenOrderedListSet<ComponentID> include, FrozenOrderedListSet<ComponentID> exclude, FrozenOrderedListSet<ComponentID> atLeastOne, FrozenOrderedListSet<ComponentID> exactlyOne)
    {
        World = world;

        Include = include;
        Exclude = exclude;
        AtLeastOneOf = atLeastOne;
        ExactlyOneOf = exactlyOne;

        _includeBloom = include.ToBloomFilter();
        _excludeBloom = exclude.ToBloomFilter();
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

        // Phantom gets auto excluded when the QueryDescription is built. Undo that now by not
        // explicitly excluding it.
        foreach (var id in Exclude)
            if (id != ComponentID<Phantom>.ID)
                builder.Exclude(id);

        foreach (var id in AtLeastOneOf)
            builder.AtLeastOneOf(id);
        foreach (var id in ExactlyOneOf)
            builder.ExactlyOneOf(id);

        return builder;
    }

    #region is in query
    /// <summary>
    /// Check if this query requires the given component
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public bool IsIncluded<T>()
        where T : IComponent
    {
        return IsIncluded(ComponentID<T>.ID);
    }

    /// <summary>
    /// Check if this query requires the given component
    /// </summary>
    /// <returns></returns>
    public bool IsIncluded(Type type)
    {
        return IsIncluded(ComponentID.Get(type));
    }

    /// <summary>
    /// Check if this query requires the given component
    /// </summary>
    /// <returns></returns>
    public bool IsIncluded(ComponentID id)
    {
        return Include.Contains(id);
    }


    /// <summary>
    /// Check if this query excludes entities with the given component
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public bool IsExcluded<T>()
        where T : IComponent
    {
        return IsExcluded(ComponentID<T>.ID);
    }

    /// <summary>
    /// Check if this query excludes entities with the given component
    /// </summary>
    /// <returns></returns>
    public bool IsExcluded(Type type)
    {
        return IsExcluded(ComponentID.Get(type));
    }

    /// <summary>
    /// Check if this query excludes entities with the given component
    /// </summary>
    /// <returns></returns>
    public bool IsExcluded(ComponentID id)
    {
        return Exclude.Contains(id);
    }


    /// <summary>
    /// Check if the given component is one of the components which at least one of must be on the entity
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public bool IsAtLeastOneOf<T>()
        where T : IComponent
    {
        return IsAtLeastOneOf(ComponentID<T>.ID);
    }

    /// <summary>
    /// Check if the given component is one of the components which at least one of must be on the entity
    /// </summary>
    /// <returns></returns>
    public bool IsAtLeastOneOf(Type type)
    {
        return IsAtLeastOneOf(ComponentID.Get(type));
    }

    /// <summary>
    /// Check if the given component is one of the components which at least one of must be on the entity
    /// </summary>
    /// <returns></returns>
    public bool IsAtLeastOneOf(ComponentID id)
    {
        return AtLeastOneOf.Contains(id);
    }


    /// <summary>
    /// Check if the given component is one of the components which exactly one of must be on the entity
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public bool IsExactlyOneOf<T>()
        where T : IComponent
    {
        return IsExactlyOneOf(ComponentID<T>.ID);
    }

    /// <summary>
    /// Check if the given component is one of the components which exactly one of must be on the entity
    /// </summary>
    /// <returns></returns>
    public bool IsExactlyOneOf(Type type)
    {
        return IsExactlyOneOf(ComponentID.Get(type));
    }

    /// <summary>
    /// Check if the given component is one of the components which exactly one of must be on the entity
    /// </summary>
    /// <returns></returns>
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
                _result = new MatchResult(World.Archetypes.Count, FrozenOrderedListSet<ArchetypeMatch>.Create(matches));

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
                    _result = new MatchResult(World.Archetypes.Count, FrozenOrderedListSet<ArchetypeMatch>.Create(copy));
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
        // Quick bloom filter test if the included components intersects with the archetype.
        // If this returns false there is definitely no overlap at all and we can early exit.
        if (Include.Count > 0 && !archetype.ComponentsBloomFilter.MaybeIntersects(in _includeBloom))
            return null;

        // Do the full set check for included components
        if (!archetype.Components.IsSupersetOf(Include))
            return null;

        // If this is false it means there is definitely _not_ an intersection, which means we can skip
        // the inner check.
        if (Exclude.Count > 0 && _excludeBloom.MaybeIntersects(in archetype.ComponentsBloomFilter))
        {
            if (archetype.Components.Overlaps(Exclude))
                return null;
        }

        // Use the temp hashset to do this
        var set = _temporarySet;
        set.Clear();

        // Check if there are any "exactly one" items
        var exactlyOne = default(ComponentID?);
        if (ExactlyOneOf.Count > 0)
        {
            set.Clear();
            set.UnionWith(archetype.Components);
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
            set.UnionWith(archetype.Components);
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
        /// <inheritdoc />
        public int CompareTo(ArchetypeMatch other)
        {
            return Archetype.Hash.CompareTo(other.Archetype.Hash);
        }
    }
    #endregion

    #region LINQish
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
    /// Count how many chunks hold entities which match this query
    /// </summary>
    /// <returns></returns>
    public int CountChunks()
    {
        var chunks = 0;
        foreach (var archetype in GetArchetypes())
            chunks += archetype.Archetype.Chunks.Count;
        return chunks;
    }

    /// <summary>
    /// Check if the count of entities matching this query is greater than a given value
    /// </summary>
    /// <returns></returns>
    public bool IsCountGreaterThan(int threshold)
    {
        var count = 0;
        foreach (var archetype in GetArchetypes())
        {
            count += archetype.Archetype.EntityCount;

            if (count > threshold)
                return true;
        }

        return false;
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
    /// Check if this query description matches the given entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public bool Contains(Entity entity)
    {
        var info = entity.World.GetEntityInfo(entity.ID);
        var archetype = new ArchetypeMatch(info.Chunk.Archetype, null, null);
        return GetArchetypes().Contains(archetype);
    }

    /// <summary>
    /// Get the first entity which this query matches (or null)
    /// </summary>
    /// <returns></returns>
    public Entity? FirstOrDefault()
    {
        foreach (var archetype in GetArchetypes())
        {
            if (archetype.Archetype.EntityCount == 0)
                continue;
            
            for (var i = 0; i < archetype.Archetype.Chunks.Count; i++)
            {
                var chunk = archetype.Archetype.Chunks[i];
                if (chunk.EntityCount > 0)
                    return chunk.Entities.Span[0];
            }
        }

        return default;
    }

    /// <summary>
    /// Get the first entity which this query matches (or throw if there are none)
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">Thrown if there are no matches</exception>
    public Entity First()
    {
        return FirstOrDefault()
            ?? throw new InvalidOperationException("QueryDescription.First() found no matching entities");
    }

    /// <summary>
    /// Get the single entity which this query matches (or null if there are none).
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">Thrown if there are more than one matches</exception>
    public Entity? SingleOrDefault()
    {
        Entity? result = default;

        foreach (var archetype in GetArchetypes())
        {
            if (archetype.Archetype.EntityCount == 0)
                continue;
            
            for (var i = 0; i < archetype.Archetype.Chunks.Count; i++)
            {
                var chunk = archetype.Archetype.Chunks[i];
                if (chunk.EntityCount == 0)
                    continue;

                if (chunk.EntityCount > 1 || result.HasValue)
                    throw new InvalidOperationException("QueryDescription.SingleOrDefault() found more than one matching entity");

                result = chunk.Entities.Span[0];
            }
            
        }

        return result;
    }

    /// <summary>
    /// Get a single entity which this query matches (throws if there is not exactly one match)
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">If none or multiple entities were found.</exception>
    public Entity Single()
    {
        return SingleOrDefault()
            ?? throw new InvalidOperationException("QueryDescription.Single() found no matching entities");
    }

    /// <summary>
    /// Get a random entity matched by this query (or null if there are none).
    /// </summary>
    /// <param name="random"></param>
    /// <returns></returns>
    public Entity? RandomOrDefault(Random random)
    {
        // Get total entity count
        var count = Count();
        if (count == 0)
            return default;

        // Choose the index of the entity
        var choice = random.Next(0, count);

        // Find that entity
        foreach (var archetype in GetArchetypes())
        {
            // Check if it's within this archetype, if not move to the next archetype
            if (choice - archetype.Archetype.EntityCount >= 0)
            {
                choice -= archetype.Archetype.EntityCount;
            }
            else
            {
                // Step through chunks
                var chunks = archetype.Archetype.Chunks;
                for (var i = 0; i < chunks.Count; i++)
                {
                    var chunk = chunks[i];

                    // Check if it's within this chunk, if not move to next chunk
                    if (choice - chunk.EntityCount >= 0)
                    {
                        choice -= chunk.EntityCount;
                    }
                    else
                    {
                        return chunk.Entities.Span[choice];
                    }
                }
            }
        }

        // This shouldn't happen. We picked an index from the count of entities, and then searched
        // for that entity. This only happens if we didn't find an entity with that index.
        return default;
    }

    /// <summary>
    /// Get a random entity matched by this query (throws if there are none)
    /// </summary>
    /// <param name="random"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">If none were found.</exception>
    public Entity Random(Random random)
    {
        return RandomOrDefault(random)
            ?? throw new InvalidOperationException("QueryDescription.Random() found no matching entities");
    }
    #endregion

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
        foreach (var match in GetArchetypes())
        {
            if (!match.Archetype.Components.Contains(id))
                continue;

            count += match.Archetype.EntityCount;

            // Wait for multithreaded access to this archetype
            match.Archetype.Block();

            using var chunks = match.Archetype.GetChunkEnumerator();
            while (chunks.MoveNext())
            {
                var chunk = chunks.Current;
                var arr = chunk!.GetSpan<T>(id);
                arr.Fill(item);
            }
        }

        return count;
    }
    #endregion
}
