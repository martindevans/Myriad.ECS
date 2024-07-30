using System.Runtime.InteropServices;
using Myriad.ECS.IDs;
using Myriad.ECS.xxHash;

namespace Myriad.ECS.Collections;

[StructLayout(LayoutKind.Sequential, Pack = 0)]
internal struct ComponentBloomFilter
{
    private ulong _a;
    private ulong _b;
    private ulong _c;
    private ulong _d;
    private ulong _e;
    private ulong _f;

    public void Add(ComponentID id)
    {
        Span<int> value = stackalloc int[] { id.Value };
        var bytes = MemoryMarshal.Cast<int, byte>(value);
        
        // Set one random bit in each of the bitsets
        SetRandomBit(bytes, 136920569, ref _a);
        SetRandomBit(bytes, 803654167, ref _b);
        SetRandomBit(bytes, 786675075, ref _c);
        SetRandomBit(bytes, 562713536, ref _d);
        SetRandomBit(bytes, 703121798, ref _e);
        SetRandomBit(bytes, 133703782, ref _f);

        static void SetRandomBit(Span<byte> bytes, ulong seed, ref ulong output)
        {
            var hash = xxHash64.ComputeHash(bytes, seed) & 0b0011_1111;
            output |= 1UL << (int)hash;
        }
    }

    public readonly bool Intersects(ref readonly ComponentBloomFilter other)
    {
        // The same items have been added to all 6 sets, with different hashes.
        // Therefore if _any_ of the sets do not intersect, then the overall
        // set does not intersect.
        var fail = (_a & other._a) == 0
                || (_b & other._b) == 0
                || (_c & other._c) == 0
                || (_d & other._d) == 0
                || (_e & other._e) == 0
                || (_f & other._f) == 0;

        return !fail;
    }

    internal void Union(ref readonly ComponentBloomFilter other)
    {
        _a |= other._a;
        _b |= other._b;
        _c |= other._c;
        _d |= other._d;
        _e |= other._e;
        _f |= other._f;
    }
}