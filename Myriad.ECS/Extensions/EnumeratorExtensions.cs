using System.Collections;
using static Myriad.ECS.Extensions.EnumeratorExtensions;

namespace Myriad.ECS.Extensions;

internal static class EnumeratorExtensions
{
    /// <summary>
    /// Skip N items in a enumerator
    /// </summary>
    /// <typeparam name="TEnumerator"></typeparam>
    /// <param name="enumerator"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static bool Skip<TEnumerator>(ref this TEnumerator enumerator, int count)
        where TEnumerator : struct, IEnumerator
    {
        while (count > 0)
        {
            count--;
            if (!enumerator.MoveNext())
                return false;
        }

        return true;
    }
}