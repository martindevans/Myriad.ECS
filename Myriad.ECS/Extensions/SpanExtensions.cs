namespace Myriad.ECS.Extensions;

internal static class SpanExtensions
{
#if !NET6_0_OR_GREATER
    internal static void Sort<TItem>(this Span<TItem> span)
        where TItem : IComparable<TItem>
    {
        for (var i = 1; i < span.Length; i++)
            Insert(span[..(i + 1)], item: span[i]);
    }

    private static void Insert<TItem>(Span<TItem> span, TItem item)
        where TItem : IComparable<TItem>
    {
        // shift until element is in place
        int i;
        for (i = span.Length - 1; i > 0 && item.CompareTo(span[i - 1]) <= 0; i--)
            span[i] = span[i - 1];

        span[i] = item;
    }
#endif
}