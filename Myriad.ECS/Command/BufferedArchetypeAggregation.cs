using Myriad.ECS.IDs;

namespace Myriad.ECS.Command;

public sealed partial class CommandBuffer
{
    // Keep track of a fixed number of aggregation nodes. The root node (0) is the node for a new entity
    // with no components. Nodes store a list of "edges" leading to other nodes. Edges indicate
    // the addition of that component to the entity. Buffered entities keep track of their node ID. Every
    // buffered entity with the same node ID therefore has the same archetype. Except for node=-1, which
    // indicates unknown.
    private int _aggregateNodesCount;
    private readonly BufferedAggregateNode[] _bufferedAggregateNodes = new BufferedAggregateNode[128];

    /// <summary>
    /// Buffered entities store a node ID, all buffered entities with the same node ID have exactly the same set of components. Initially
    /// new buffered entities start in node 0 (no components). Each aggregate node stores 16 edges, leadiong out to other nodes. Each node
    /// represents a component being added to a buffered entity.
    /// </summary>
    private struct BufferedAggregateNode
    {
        private const byte MaxEdges = 16;

        private unsafe fixed int _componentIdBuffer[MaxEdges];
        private unsafe fixed short _nodeIdBuffer[MaxEdges];
        private byte _edgeCount;

        public int GetNodeIndex(ComponentID component, BufferedAggregateNode[] nodesArr, ref int nodesCount)
        {
            unsafe
            {
                fixed (int* componentIdBufferPtr = _componentIdBuffer)
                fixed (short* nodeIdBufferPtr = _nodeIdBuffer)
                {
                    var componentIds = new Span<int>(componentIdBufferPtr, MaxEdges);
                    var nodeIds = new Span<short>(nodeIdBufferPtr, MaxEdges);

                    // Find the index of the edge for this component
                    var idx = componentIds[.._edgeCount].IndexOf(component.Value);
                    if (idx >= 0)
                        return nodeIds[idx];

                    // Not found...

                    // If the buffers are full return node -1. This is the "no particular group" node.
                    if (_edgeCount == MaxEdges)
                        return -1;

                    // If the node array itself is full return -1. This is the "no particular group" node.
                    if (nodesCount == nodesArr.Length)
                        return -1;

                    // Create a new node
                    nodesArr[nodesCount] = new BufferedAggregateNode();
                    var newNodeId = (short)nodesCount++;

                    // Create an edge pointing to a new node
                    componentIds[_edgeCount] = component.Value;
                    nodeIds[_edgeCount] = newNodeId;
                    _edgeCount++;


                    return newNodeId;
                }
            }
        }
    }
}