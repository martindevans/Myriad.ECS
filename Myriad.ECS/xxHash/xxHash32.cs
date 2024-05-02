// Taken from https://github.com/uranium62/xxHash/tree/6b20e7f7b32dfc29e5019d3d35f5b7270f1656f3 under MIT license
// //
// // MIT License
// // 
// // Copyright (c) 2018 Melnik Alexander
// // 
// // Permission is hereby granted, free of charge, to any person obtaining a copy
// // of this software and associated documentation files (the "Software"), to deal
// // in the Software without restriction, including without limitation the rights
// // to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// // copies of the Software, and to permit persons to whom the Software is
// // furnished to do so, subject to the following conditions:
// // 
// // The above copyright notice and this permission notice shall be included in all
// // copies or substantial portions of the Software.
// // 
// // THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// // IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// // FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// // AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// // LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// // OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// // SOFTWARE.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// ReSharper disable All
#pragma warning disable IDE1006 // Naming Styles

namespace Myriad.ECS.xxHash
{
    [ExcludeFromCodeCoverage]

    internal static partial class xxHash32
    {
        /// <summary>
        /// Compute xxHash for the data byte span 
        /// </summary>
        /// <param name="data">The source of data</param>
        /// <param name="seed">The seed number</param>
        /// <returns>hash</returns>
        public static unsafe uint ComputeHash(Span<byte> data, uint seed = 0)
        {
            Debug.Assert(data != null);

            fixed (byte* pData = &MemoryMarshal.GetReference(data))
            {
                return UnsafeComputeHash(pData, data.Length, seed);
            }
        }

        /// <summary>
        /// Compute xxHash for the data byte span 
        /// </summary>
        /// <param name="data">The source of data</param>
        /// <param name="seed">The seed number</param>
        /// <returns>hash</returns>
        public static unsafe uint ComputeHash(ReadOnlySpan<byte> data, uint seed = 0)
        {
            Debug.Assert(data != null);

            fixed (byte* pData = &MemoryMarshal.GetReference(data))
            {
                return UnsafeComputeHash(pData, data.Length, seed);
            }
        }

        private const uint XXH_PRIME32_1 = 2654435761U;
        private const uint XXH_PRIME32_2 = 2246822519U;
        private const uint XXH_PRIME32_3 = 3266489917U;
        private const uint XXH_PRIME32_4 = 668265263U;
        private const uint XXH_PRIME32_5 = 374761393U;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe uint UnsafeComputeHash(byte* input, int len, uint seed)
        {
            uint h32;

            if (len >= 16)
            {
                byte* end = input + len;
                byte* limit = end - 15;

                uint v1 = seed + XXH_PRIME32_1 + XXH_PRIME32_2;
                uint v2 = seed + XXH_PRIME32_2;
                uint v3 = seed + 0;
                uint v4 = seed - XXH_PRIME32_1;

                do
                {
                    var reg1 = *((uint*)(input + 0));
                    var reg2 = *((uint*)(input + 4));
                    var reg3 = *((uint*)(input + 8));
                    var reg4 = *((uint*)(input + 12));

                    // XXH32_round
                    v1 += reg1 * XXH_PRIME32_2;
                    v1 = (v1 << 13) | (v1 >> (32 - 13));
                    v1 *= XXH_PRIME32_1;

                    // XXH32_round
                    v2 += reg2 * XXH_PRIME32_2;
                    v2 = (v2 << 13) | (v2 >> (32 - 13));
                    v2 *= XXH_PRIME32_1;

                    // XXH32_round
                    v3 += reg3 * XXH_PRIME32_2;
                    v3 = (v3 << 13) | (v3 >> (32 - 13));
                    v3 *= XXH_PRIME32_1;

                    // XXH32_round
                    v4 += reg4 * XXH_PRIME32_2;
                    v4 = (v4 << 13) | (v4 >> (32 - 13));
                    v4 *= XXH_PRIME32_1;

                    input += 16;
                } while (input < limit);

                h32 = ((v1 << 1) | (v1 >> (32 - 1))) +
                      ((v2 << 7) | (v2 >> (32 - 7))) +
                      ((v3 << 12) | (v3 >> (32 - 12))) +
                      ((v4 << 18) | (v4 >> (32 - 18)));
            }
            else
            {
                h32 = seed + XXH_PRIME32_5;
            }

            h32 += (uint)len;

            // XXH32_finalize
            len &= 15;
            while (len >= 4)
            {
                h32 += *((uint*)input) * XXH_PRIME32_3;
                input += 4;
                h32 = ((h32 << 17) | (h32 >> (32 - 17))) * XXH_PRIME32_4;
                len -= 4;
            }

            while (len > 0)
            {
                h32 += *((byte*)input) * XXH_PRIME32_5;
                ++input;
                h32 = ((h32 << 11) | (h32 >> (32 - 11))) * XXH_PRIME32_1;
                --len;
            }

            // XXH32_avalanche
            h32 ^= h32 >> 15;
            h32 *= XXH_PRIME32_2;
            h32 ^= h32 >> 13;
            h32 *= XXH_PRIME32_3;
            h32 ^= h32 >> 16;

            return h32;
        }
    }
}