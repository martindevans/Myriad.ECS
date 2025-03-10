using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Myriad.ECS.IDs;
using Myriad.ECS.xxHash;

namespace Myriad.ECS.Collections;

/// <summary>
/// Probabalistic set of component IDs. Can be used to check if two sets intersect.<br />
///
/// False positives are possible (i.e. If Intersects returns true, then there <b>might</b> be an overlap).<br />
/// False negatives are <b>not</b> possible (i.e. If Intersects return false, then there <b>definitely</b> is no overlap).<br />
/// </summary>
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
        Span<int> value = [ id.Value ];
        var bytes = MemoryMarshal.Cast<int, byte>(value);
        
        // Set one random bit in each of the bitsets
        SetRandomBit(bytes, 136920569, ref _a);
        SetRandomBit(bytes, 803654167, ref _b);
        SetRandomBit(bytes, 786675075, ref _c);
        SetRandomBit(bytes, 562713536, ref _d);
        SetRandomBit(bytes, 703121798, ref _e);
        SetRandomBit(bytes, 133703782, ref _f);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void SetRandomBit(Span<byte> bytes, ulong seed, ref ulong output)
        {
            var hash = xxHash64.ComputeHash(bytes, seed) & 0b0011_1111;
            output |= 1UL << (int)hash;
        }
    }

    /// <summary>
    /// Check if this bloom filter <b>might</b> intersect with another.
    /// <br /><br />
    /// False positives are possible (i.e. If Intersects returns true, then there <b>might</b> be an overlap).<br />
    /// False negatives are <b>not</b> possible (i.e. If Intersects return false, then there <b>definitely</b> is no overlap).<br />
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public readonly bool MaybeIntersects(ref readonly ComponentBloomFilter other)
    {
        // The same items have been added to all 6 sets, with different hashes.
        // Therefore if _any_ of the sets do not intersect, then the overall
        // set does not intersect.

#if NETSTANDARD2_1
        var fail = (_a & other._a) == 0
                || (_b & other._b) == 0
                || (_c & other._c) == 0
                || (_d & other._d) == 0
                || (_e & other._e) == 0
                || (_f & other._f) == 0;
#else
        // Bitwise & each matching element in the two sets together
        var abcd = System.Runtime.Intrinsics.Vector256.Create(_a, _b, _c, _d)
                 & System.Runtime.Intrinsics.Vector256.Create(other._a, other._b, other._c, other._d);
        var ef = System.Runtime.Intrinsics.Vector128.Create(_e, _f)
               & System.Runtime.Intrinsics.Vector128.Create(other._e, other._f);

        // Check if any of the elements had any matching bits
        var abz = System.Runtime.Intrinsics.Vector256.EqualsAny(abcd, System.Runtime.Intrinsics.Vector256<ulong>.Zero);
        var efz = System.Runtime.Intrinsics.Vector128.EqualsAny(ef, System.Runtime.Intrinsics.Vector128<ulong>.Zero);

        var fail = abz | efz;
#endif

        return !fail;
    }

    public void Union(ref readonly ComponentBloomFilter other)
    {
        _a |= other._a;
        _b |= other._b;
        _c |= other._c;
        _d |= other._d;
        _e |= other._e;
        _f |= other._f;
    }
}