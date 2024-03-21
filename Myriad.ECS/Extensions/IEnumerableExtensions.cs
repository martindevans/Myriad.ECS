using System.Collections;

namespace Myriad.ECS.Extensions;

public static class IEnumerableExtensions
{
#if !NET6_0_OR_GREATER
    internal static bool TryGetNonEnumeratedCount<TSource>(this IEnumerable<TSource> source, out int count)
    {
        if (source is ICollection<TSource> collectionoft)
        {
            count = collectionoft.Count;
            return true;
        }

        if (source is ICollection collection)
        {
            count = collection.Count;
            return true;
        }

        count = 0;
        return false;
    }
#endif
}