using Myriad.ECS.Worlds.Archetypes;

namespace Myriad.ECS.Queries;

/// <summary>
/// Keep track of how far through execution a query got, can be used to resume execution at <b>approximately</b> the same point.
/// Some entities may be repeated, and some may be skipped, but if run every frame eventually it will get to every entity.
/// </summary>
public class Cursor
{
    /// <summary>
    /// The last archetype that was processed
    /// </summary>
    internal Archetype? LastArchetype;

    /// <summary>
    /// How many chunks of the last archetype were processed last time this cursor was used
    /// </summary>
    internal int Chunks;

    /// <summary>
    /// Get or set the number of entities queries may process before early exiting
    /// </summary>
    public int EntityBudget
    {
        get;
        set;
    }

    /// <summary>
    /// Reset this cursor back to an empty state
    /// </summary>
    public void Reset()
    {
        LastArchetype = null;
        Chunks = 0;
    }
}