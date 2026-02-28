using Myriad.ECS.IDs;
using Myriad.ECS.xxHash;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Myriad.ECS.Collections;

/// <summary>
/// Probabalistic set of 32 bit values (e.g. component IDs), with 512 total bits and 8 hash functions. Can be used to check if two sets intersect.<br />
///
/// False positives are possible (i.e. If Intersects returns true, then there <b>might</b> be an overlap).<br />
/// False negatives are <b>not</b> possible (i.e. If Intersects return false, then there <b>definitely</b> is no overlap).<br />
/// </summary>
[StructLayout(LayoutKind.Sequential, Pack = 0)]
internal struct BloomFilter32x512
{
    private ulong _a;
    private ulong _b;
    private ulong _c;
    private ulong _d;
    private ulong _e;
    private ulong _f;
    private ulong _g;
    private ulong _h;

    public void Add(ComponentID id)
    {
        Add(id.Value);
    }

    public void Add(int id)
    {
        Span<int> value = stackalloc int[] { id };
        var bytes = MemoryMarshal.Cast<int, byte>(value);

        // Set one random bit in each of the bitsets
        SetRandomBit(bytes, 136920569, ref _a);
        SetRandomBit(bytes, 803654167, ref _b);
        SetRandomBit(bytes, 786675075, ref _c);
        SetRandomBit(bytes, 562713536, ref _d);
        SetRandomBit(bytes, 703121798, ref _e);
        SetRandomBit(bytes, 133703782, ref _f);
        SetRandomBit(bytes, 978376609, ref _g);
        SetRandomBit(bytes, 542356235, ref _h);

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
    public readonly bool MaybeIntersects(ref readonly BloomFilter32x512 other)
    {
        // The same items have been added to all 6 sets, with different hashes.
        // Therefore if _any_ of the sets do not intersect, then the overall
        // set does not intersect.

#if NETSTANDARD2_1
        var mask =
            IsNonZero(_a & other._a) &
            IsNonZero(_b & other._b) &
            IsNonZero(_c & other._c) &
            IsNonZero(_d & other._d) &
            IsNonZero(_e & other._e) &
            IsNonZero(_f & other._f) &
            IsNonZero(_g & other._g) & 
            IsNonZero(_h & other._h);

        // If any were zero the final bit will be zero.
        var fail = mask != 1;

        // Returns 1 if x is non-zero
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static ulong IsNonZero(ulong x) => (x | (~x + 1)) >> 63;
#else
        // Get references to the start of the structs (byte 0)
        ref readonly var selfRef = ref Unsafe.As<BloomFilter32x512, ulong>(ref Unsafe.AsRef(in this));
        ref readonly var otherRef = ref Unsafe.As<BloomFilter32x512, ulong>(ref Unsafe.AsRef(in other));

        // Combine lower and upper 256 bits of the 2 filters
        var v1 = System.Runtime.Intrinsics.Vector256.LoadUnsafe(in selfRef)    & System.Runtime.Intrinsics.Vector256.LoadUnsafe(in otherRef);
        var v2 = System.Runtime.Intrinsics.Vector256.LoadUnsafe(in selfRef, 4) & System.Runtime.Intrinsics.Vector256.LoadUnsafe(in otherRef, 4);

        var fail = System.Runtime.Intrinsics.Vector256.EqualsAny(v1, System.Runtime.Intrinsics.Vector256<ulong>.Zero)
                 | System.Runtime.Intrinsics.Vector256.EqualsAny(v2, System.Runtime.Intrinsics.Vector256<ulong>.Zero);
#endif

        return !fail;
    }

    /// <summary>
    /// Modify this bloom filter to the union of this and other.
    /// </summary>
    /// <param name="other"></param>
    public void Union(ref readonly BloomFilter32x512 other)
    {
        _a |= other._a;
        _b |= other._b;
        _c |= other._c;
        _d |= other._d;
        _e |= other._e;
        _f |= other._f;
        _g |= other._g;
        _h |= other._h;
    }
}