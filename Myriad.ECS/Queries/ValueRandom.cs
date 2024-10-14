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

        // Calculate hash of components, for fast rejection
        var byteSpan = MemoryMarshal.Cast<int, byte>(seed);
        var hash = xxHash64.ComputeHash(byteSpan, seed: 568456);

        var low = (uint)(hash & 0xFFFF_FFFF);
        var hi = (uint)((hash >> 32) & 0xFFFF_FFFF);

        _seed = unchecked((int)hi);
        return unchecked((int)low);
    }
}