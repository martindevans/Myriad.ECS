using Myriad.ECS.Collections;
using Myriad.ECS.IDs;

namespace Myriad.ECS.Extensions;

internal static class FrozenOrderedListSetExtensions
{
    public static BloomFilter32x512 ToBloomFilter(this FrozenOrderedListSet<ComponentID> set)
    {
        var filter = new BloomFilter32x512();
        foreach (var item in set)
            filter.Add(item);
        return filter;
    }
}