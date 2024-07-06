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
        // If any of them do _not_ overlap, then the overall set can't overlap
        if ((_a & other._a) == 0)
            return false;
        if ((_b & other._b) == 0)
            return false;
        if ((_c & other._c) == 0)
            return false;
        if ((_d & other._d) == 0)
            return false;
        if ((_e & other._e) == 0)
            return false;
        if ((_f & other._f) == 0)
            return false;

        return true;
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