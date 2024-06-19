using System.Runtime.InteropServices;
using Myriad.ECS.IDs;
using Myriad.ECS.xxHash;

namespace Myriad.ECS.Collections;

internal struct ComponentBloomFilter
{
    private ulong _a;
    private ulong _b;
    private ulong _c;
    private ulong _d;

    public void Add(ComponentID id)
    {
        Span<int> value = stackalloc int[] { id.Value };
        var bytes = MemoryMarshal.Cast<int, byte>(value);

        var hashA = xxHash64.ComputeHash(bytes, 13) & 0b111111;
        var hashB = xxHash64.ComputeHash(bytes, 17) & 0b111111;
        var hashC = xxHash64.ComputeHash(bytes, 23) & 0b111111;
        var hashD = xxHash64.ComputeHash(bytes, 29) & 0b111111;

        _a |= 1UL << (int)hashA;
        _b |= 1UL << (int)hashB;
        _c |= 1UL << (int)hashC;
        _d |= 1UL << (int)hashD;
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

        return true;
    }

    internal void Union(ref readonly ComponentBloomFilter other)
    {
        _a |= other._a;
        _b |= other._b;
        _c |= other._c;
        _d |= other._d;
    }
}