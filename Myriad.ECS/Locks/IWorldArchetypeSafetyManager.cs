using Myriad.ECS.Worlds.Archetypes;

namespace Myriad.ECS.Locks;

/// <summary>
/// Manages safe multithreaded access of archetypes
/// </summary>
public interface IWorldArchetypeSafetyManager
{
    /// <summary>
    /// Wait for multithreaded work which is accessing this archetype to finish
    /// </summary>
    /// <param name="archetype"></param>
    void Block(Archetype archetype);
}

/// <summary>
/// Implements <see cref="IWorldArchetypeSafetyManager"/> but does not actually do anything!
/// </summary>
public class DefaultWorldArchetypeSafetyManager
    : IWorldArchetypeSafetyManager
{
    /// <inheritdoc />
    public void Block(Archetype archetype)
    {
    }
}