using System.Numerics;
using Unity.Mathematics;

namespace NBodyIntegrator.Mathematics;

public struct QuaternionDouble
{
    public double X;
    public double Y;
    public double Z;
    public double W;

    public QuaternionDouble(Quaternion q)
    {
        X = q.X;
        Y = q.Y;
        Z = q.Z;
        W = q.W;
    }

    public static double3 operator *(QuaternionDouble q, double3 v)
    {
        // https://gamedev.stackexchange.com/a/50545/1687

        var u = new double3(q.X, q.Y, q.Z);
        var s = q.W;

        return 2 * math.dot(u, v) * u
             + (s * s - math.dot(u, u)) * v
             + 2 * s * math.cross(u, v);
    }
}