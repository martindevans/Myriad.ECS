using System.Collections;

namespace Myriad.ECS.Extensions;

internal static class SortedListExtensions
{
    public static SortedListEnumerable<TKey, TValue> Enumerable<TKey, TValue>(this SortedList<TKey, TValue> list)
        where TKey : notnull
    {
        return new SortedListEnumerable<TKey, TValue>(list);
    }

    public static KeyValueEnumerator<TKey, TValue> Enumerate<TKey, TValue>(this SortedList<TKey, TValue> list)
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
        : IEnumerator<(TKey, TValue)>
        where TKey : notnull
    {
        private int _index = -1;

        public bool MoveNext()
        {
            _index++;
            return _index < list.Count;
        }

        public void Reset()
        {
            _index = 0;
        }

        public readonly (TKey, TValue) Current => (list.GetKeyAtIndex(_index), list.GetValueAtIndex(_index));

        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }
    }
}