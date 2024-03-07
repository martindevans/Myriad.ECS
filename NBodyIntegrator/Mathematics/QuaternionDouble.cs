using System.Numerics;

namespace NBodyIntegrator.Mathematics;

public readonly struct QuaternionDouble
{
    public readonly double X;
    public readonly double Y;
    public readonly double Z;
    public readonly double W;

    public QuaternionDouble(Quaternion q)
    {
        X = q.X;
        Y = q.Y;
        Z = q.Z;
        W = q.W;
    }

    public static double3 operator *(QuaternionDouble rotation, double3 value)
    {
        var xyz = new double3(rotation.X, rotation.Y, rotation.Z);
        var xyz2 = xyz + xyz;

        var w2 = rotation.W * xyz2;
        var x2 = rotation.X * xyz2;

        var yy2 = rotation.Y * xyz2.y;
        var yz2 = rotation.Y * xyz2.z;
        var zz2 = rotation.Z * xyz2.z;

        return new double3(
            value.x * (1.0f - yy2 - zz2) + value.y * (x2.y - w2.z) + value.z * (x2.z + w2.y),
            value.x * (x2.y + w2.z) + value.y * (1.0f - x2.x - zz2) + value.z * (yz2 - w2.x),
            value.x * (x2.z - w2.y) + value.y * (yz2 + w2.x) + value.z * (1.0f - x2.x - yy2)
        );
    }
}