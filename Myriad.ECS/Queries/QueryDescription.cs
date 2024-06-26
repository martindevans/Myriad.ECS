﻿using Myriad.ECS.Collections;
using Myriad.ECS.IDs;
using Myriad.ECS.Worlds;
using Myriad.ECS.Worlds.Archetypes;

namespace Myriad.ECS.Queries;

public sealed class QueryDescription(
    World _world,
    FrozenOrderedListSet<ComponentID> _include,
    FrozenOrderedListSet<ComponentID> _exclude,
    FrozenOrderedListSet<ComponentID> _atLeastOne,
    FrozenOrderedListSet<ComponentID> _exactlyOne
)
{
    // Cache of result from last time TryMatch was called
    private MatchResult? _result;
    private readonly ReaderWriterLockSlim _resultLock = new();

    private readonly OrderedListSet<ComponentID> _temporarySet = new();

    /// <summary>
    /// Create a query builder which describes this query
    /// </summary>
    /// <returns></returns>
    public QueryBuilder ToBuilder()
    {
        var builder = new QueryBuilder();

        foreach (var id in _include)
            builder.Include(id);
        foreach (var id in _exclude)
            builder.Exclude(id);
        foreach (var id in _atLeastOne)
            builder.AtLeastOneOf(id);
        foreach (var id in _exactlyOne)
            builder.ExactlyOneOf(id);

        return builder;
    }

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
            if (_result != null && !_result.Value.IsStale(_world))
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
                foreach (var item in _world.Archetypes)
                    if (TryMatch(item) is ArchetypeMatch m)
                        matches.Add(m);

                // Store result for next time
                _result = new MatchResult(_world.Archetypes.Count, new FrozenOrderedListSet<ArchetypeMatch>(matches));

                // Return matches
                return _result.Value.Archetypes;
            }

            // If the number of archetypes has changed since last time regenerate the cache
            if (_result.Value.IsStale(_world))
            {
                // Lazy copy of the match set, in case there are no matches
                var copy = (OrderedListSet<ArchetypeMatch>?)null;

                // Check every new archetype
                for (var i = _result.Value.ArchetypeWatermark; i < _world.Archetypes.Count; i++)
                {
                    var m = TryMatch(_world.Archetypes[i]);
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
                    _result = new MatchResult(_world.Archetypes.Count, _result.Value.Archetypes);
                }
                else
                {
                    // Create a new match result
                    _result = new MatchResult(_world.Archetypes.Count, new FrozenOrderedListSet<ArchetypeMatch>(copy));
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

        if (!components.IsSupersetOf(_include))
            return null;

        if (components.Overlaps(_exclude))
            return null;

        // Use the temp hashset to do this
        var set = _temporarySet;
        set.Clear();

        // Check if there are any "exactly one" items
        var exactlyOne = default(ComponentID?);
        if (_exactlyOne.Count > 0)
        {
            set.UnionWith(components);
            set.IntersectWith(_exactlyOne);
            if (set.Count != 1)
            {
                set.Clear();
                return null;
            }

            exactlyOne = set.Single();
            set.Clear();
        }

        // Check if there are any "at least one" items
        if (_atLeastOne.Count > 0)
        {
            set.UnionWith(components);
            set.IntersectWith(_atLeastOne);
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
    /// An archetype which matches this query
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
}