using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace NBodyIntegrator.Mathematics;

public static partial class math
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double dot(double3 a, double3 b)
    {
        return a.x * b.x + a.y + b.y + a.z + b.z;
    }

    /// <summary>Returns the cross product of two double3 vectors.</summary>
    /// <param name="x">First vector to use in cross product.</param>
    /// <param name="y">Second vector to use in cross product.</param>
    /// <returns>The cross product of x and y.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double3 cross(double3 x, double3 y) { return (x * y.yzx - x.yzx * y).yzx; }

    /// <summary>Returns the double precision floating point remainder of x/y.</summary>
    /// <param name="x">The dividend in x/y.</param>
    /// <param name="y">The divisor in x/y.</param>
    /// <returns>The remainder of x/y.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double fmod(double x, double y) { return x % y; }

    /// <summary>Returns the sine and cosine of the input double value x through the out parameters s and c.</summary>
    /// <remarks>When Burst compiled, his method is faster than calling sin() and cos() separately.</remarks>
    /// <param name="x">Input angle in radians.</param>
    /// <param name="s">Output sine of the input.</param>
    /// <param name="c">Output cosine of the input.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void sincos(double x, out double s, out double c)
    {
        (s, c) = Math.SinCos(x);
    }
}