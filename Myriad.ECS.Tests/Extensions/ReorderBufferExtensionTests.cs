using Myriad.ECS.Extensions;

namespace Myriad.ECS.Tests.Extensions;

[TestClass]
public class ReorderBufferExtensionTests
{
    [TestMethod]
    public void BasicShuffle()
    {
        var data = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        var indices = new TestDataIndex[] { 8, 7, 6, 5, 4, 3, 2, 1, 0 };

        new TestMover<int>(data).ApplyReorderInPlace<TestMover<int>, TestDataIndex>(indices);
    }

    [TestMethod]
    public void TestRandomShuffle()
    {
        // Fill data array with random data
        var data = new TestData[128];
        var rng = new Random(346325);
        for (var i = 0; i < data.Length; i++)
            data[i] = new TestData(rng.NextSingle());
        
        // Extract keys
        var shuffle = new TestSortable<float>[data.Length];
        for (var i = 0; i < data.Length; i++)
            shuffle[i] = new TestSortable<float>(i, data[i].Value);
        
        // Sort keys
        shuffle.AsSpan().Sort();
        
        // Apply reorder
        new TestMover<TestData>(data).ApplyReorderInPlace<TestMover<TestData>, TestSortable<float>>(shuffle);
        
        // Now ensure that the array of data has been shuffled into order
        var prev = data[0];
        for (var i = 1; i < data.Length; i++)
        {
            var item = data[i];
            Assert.IsGreaterThanOrEqualTo(prev.Value, item.Value);
            
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            Assert.AreEqual(-item.Value, item.OtherValue);
        }
    }

    private struct TestMover<TData>
        : ReorderBuffer.IDataMove
        where TData : struct
    {
        private readonly TData[] _data;
        private TData _temp;

        public TestMover(TData[] data)
        {
            _data = data;
            _temp = default;
        }
        
        public void Move(int indexFrom, int indexTo)
        {
            _data[indexTo] = _data[indexFrom];
        }

        public void SaveToTemporary(int indexFrom)
        {
            _temp = _data[indexFrom];
        }

        public void RestoreFromTemporary(int indexTo)
        {
            _data[indexTo] = _temp;
        }
    }

    private struct TestDataIndex
        : ReorderBuffer.IDataIndex
    {
        public TestDataIndex(int index)
        {
            OriginalIndex = index;
        }

        public int OriginalIndex { get; }
        
        public static implicit operator TestDataIndex(int value)
        {
            return new TestDataIndex(value);
        }
    }

    private struct TestData
    {
        public float Value;
        public float OtherValue;

        public TestData(float value)
        {
            Value = value;
            OtherValue = -value;
        }
    }

    private struct TestSortable<TKey>
        : IComparable<TestSortable<TKey>>
        , ReorderBuffer.IDataIndex
        where TKey : unmanaged, IComparable<TKey>
    {
        public int OriginalIndex { get; }
        public readonly TKey Key;

        public TestSortable(int index, TKey key)
        {
            OriginalIndex = index;
            Key = key;
        }

        public int CompareTo(TestSortable<TKey> other)
        {
            return Key.CompareTo(other.Key);
        }
    }
}