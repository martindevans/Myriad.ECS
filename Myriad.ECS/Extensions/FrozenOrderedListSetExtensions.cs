using Myriad.ECS.Collections;
using Myriad.ECS.IDs;

namespace Myriad.ECS.Extensions;

internal static class FrozenOrderedListSetExtensions
{
    public static ComponentBloomFilter ToBloomFilter(this FrozenOrderedListSet<ComponentID> set)
    {
        var filter = new ComponentBloomFilter();
        foreach (var item in set)
            filter.Add(item);
        return filter;
    }
}