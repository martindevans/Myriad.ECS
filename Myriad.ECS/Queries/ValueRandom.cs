using Myriad.ECS.xxHash;
using System.Runtime.InteropServices;

namespace Myriad.ECS.Queries;

internal struct ValueRandom
{
    private int _seed;

    public ValueRandom(int seed)
    {
        _seed = seed;
    }

    public int Next()
    {
        Span<int> seed = [ _seed ];

        // Hash the state, to generate 64 bits
        var byteSpan = MemoryMarshal.Cast<int, byte>(seed);
        var hash = xxHash64.ComputeHash(byteSpan, seed: 568456);

        // Take the low bits and the high bits as two 32 bit values
        var low = (uint)(hash & 0xFFFF_FFFF);
        var hi = (uint)((hash >> 32) & 0xFFFF_FFFF);

        // Next state is the high bits
        _seed = unchecked((int)hi);

        // Result is the low bits
        return unchecked((int)low);
    }
}