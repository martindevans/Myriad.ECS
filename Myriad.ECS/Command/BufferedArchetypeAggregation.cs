using Myriad.ECS.IDs;

namespace Myriad.ECS.Command;

public sealed partial class CommandBuffer
{
    // Keep track of a fixed number of aggregation nodes. The root node (0) is the node for a new entity
    // with no components. Nodes store a list of "edges" leading to other nodes. Edges indicate
    // the addition of that component to the entity. Buffered entities keep track of their node ID. Every
    // buffered entity with the same node ID therefore has the same archetype. Except for node=-1, which
    // indicates unknown.

    private const int MaxAggregationEdges = 1024;

    /// <summary>
    /// Map from (current_archetype_key, added_component) => new_archetype_key
    /// </summary>
    private readonly Dictionary<(int, ComponentID), int> _archetypeEdges = new();

    /// <summary>
    /// Given an archetype key and an added component, determine the new archetype key
    /// </summary>
    /// <param name="currentKey"></param>
    /// <param name="added"></param>
    /// <returns></returns>
    private int GetArchetypeKey(int currentKey, ComponentID added)
    {
        if (!_archetypeEdges.TryGetValue((currentKey, added), out var value))
        {
            // Limit the number of edges to prevent explosive growth in some edge cases.
            if (_archetypeEdges.Count >= MaxAggregationEdges)
                return -1;

            value = _archetypeEdges.Count + 1;
            _archetypeEdges.Add((currentKey, added), value);
        }

        return value;
    }
}