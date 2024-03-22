namespace Myriad.ECS.Extensions;

internal static class ListExtensions
{
    internal static void EnsureCapacity<T>(this List<T> list, int capacity)
    {
        if (list.Capacity > capacity)
            return;

        list.Capacity = capacity;
    }
}