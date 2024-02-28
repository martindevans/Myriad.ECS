using System.Numerics;

namespace NBodyIntegrator;

public static class Vector3Extensions
{
    public static Vector3 Perpendicular(this Vector3 v)
    {
        return new Vector3(
            MathF.CopySign(v.Z, v.X),
            MathF.CopySign(v.Z, v.Y),
            -MathF.CopySign(v.X, v.Z) - MathF.CopySign(v.Y, v.Z)
        );
    }
}