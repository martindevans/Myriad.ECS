namespace Myriad.ECS.Extensions;

internal static class ListExtensions
{
#if !NET6_0_OR_GREATER
    internal static void EnsureCapacity<T>(this List<T> list, int capacity)
    {
        list.Capacity = Math.Max(list.Capacity, capacity);
    }
#endif
}