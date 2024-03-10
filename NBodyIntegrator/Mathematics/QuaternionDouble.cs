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

        var yy2 = rotation.Y * xyz2.Y;
        var yz2 = rotation.Y * xyz2.Z;
        var zz2 = rotation.Z * xyz2.Z;

        return new double3(
            value.X * (1.0f - yy2 - zz2) + value.Y * (x2.Y - w2.Z) + value.Z * (x2.Z + w2.Y),
            value.X * (x2.Y + w2.Z) + value.Y * (1.0f - x2.X - zz2) + value.Z * (yz2 - w2.X),
            value.X * (x2.Z - w2.Y) + value.Y * (yz2 + w2.X) + value.Z * (1.0f - x2.X - yy2)
        );
    }
}