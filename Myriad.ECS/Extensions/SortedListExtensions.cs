namespace Myriad.ECS.Extensions;

internal static class SortedListExtensions
{
    /// <summary>
    /// Get an enumerable struct wrapper around a SortedList
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static SortedListEnumerable<TKey, TValue> Enumerable<TKey, TValue>(this SortedList<TKey, TValue> list)
        where TKey : notnull
    {
        return new SortedListEnumerable<TKey, TValue>(list);
    }

    private static KeyValueEnumerator<TKey, TValue> Enumerate<TKey, TValue>(this SortedList<TKey, TValue> list)
        where TKey : notnull
    {
        return new KeyValueEnumerator<TKey, TValue>(list);
    }

    internal readonly struct SortedListEnumerable<TKey, TValue>(SortedList<TKey, TValue> list)
        where TKey : notnull
    {
        public KeyValueEnumerator<TKey, TValue> GetEnumerator()
        {
            return list.Enumerate();
        }
    }

    internal struct KeyValueEnumerator<TKey, TValue>(SortedList<TKey, TValue> list)
        where TKey : notnull
    {
        private int _index = -1;

        public bool MoveNext()
        {
            _index++;
            return _index < list.Count;
        }

        public readonly (TKey, TValue) Current => (list.GetKeyAtIndex(_index), list.GetValueAtIndex(_index));
    }
}