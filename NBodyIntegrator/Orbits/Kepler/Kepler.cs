using System.Numerics;
using Myriad.ECS;
using NBodyIntegrator.Mathematics;
using NBodyIntegrator.Units;

namespace NBodyIntegrator.Orbits.Kepler;

public readonly struct KeplerOrbit
    : IComponent, IEquatable<KeplerOrbit>
{
    private const double TwoPi = Math.PI * 2;

    public readonly Entity Parent;
    public readonly KeplerElements Elements;

    private readonly double _meanMotion;
    private readonly double _sqrtOneMinusEcc;
    private readonly double _sqrtOnePlusEcc;
    private readonly double _oneMinusEccSqr;
    private readonly Quaternion _quaternion;

    public KeplerOrbit(Entity parent, KeplerElements elements)
    {
        Parent = parent;
        Elements = elements;

        _meanMotion = TwoPi / Elements.Period.Value;
        _sqrtOneMinusEcc = Math.Sqrt(1 - Elements.Eccentricity);
        _sqrtOnePlusEcc = Math.Sqrt(1 + Elements.Eccentricity);
        _oneMinusEccSqr = 1 - Math.Pow(Elements.Eccentricity, 2);

        _quaternion = Quaternion.CreateFromAxisAngle(new Vector3(0, 0, 1), (float)(Radians)Elements.LongitudeOfAscendingNode)
                    * Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0), (float)(Radians)Elements.Inclination)
                    * Quaternion.CreateFromAxisAngle(new Vector3(0, 0, 1), (float)(Radians)Elements.ArgumentOfPeriapsis);
    }

    public Metre3 PositionAtTime(double epoch)
    {
        // https://github.com/salel/Roche/blob/244fd71ed841576f1c6dad8198312524bc1ff291/src/entity.cpp#L48

        var m0 = Elements.MeanAnomalyEpoch.Value;
        var ecc = Elements.Eccentricity;

        // Mean Anomaly compute
        var meanMotion = _meanMotion;
        var meanAnomaly = (epoch * meanMotion + m0) % TwoPi;

        // Mean anomaly to Eccentric
        var En = MeanToEccentric(meanAnomaly, ecc);

        // Eccentric anomaly to True anomaly
        var (sinEn2, cosEn2) = Math.SinCos(En / 2);
        var trueAnomaly = 2 * Math.Atan2(_sqrtOnePlusEcc * sinEn2, _sqrtOneMinusEcc * cosEn2);

        // Calculate actual position
        return PositionAtTrueAnomaly(trueAnomaly);
    }

    public Metre3 PositionAtTrueAnomaly(double trueAnomaly)
    {
        var ecc = Elements.Eccentricity;
        var sma = Elements.SemiMajorAxis.Value;

        // Distance from parent body
        var (sta, cta) = Math.SinCos(trueAnomaly);
        var dist = sma * (_oneMinusEccSqr / (1 + ecc * cta));

        // Plane changes
        var posInPlane = new double3(
            -sta * dist,
            cta * dist,
            0.0
        );

        var pos = new QuaternionDouble(_quaternion) * posInPlane;

        return new Metre3(pos.xzy);
    }

    private static double MeanToEccentric(double mean, double ecc, int maxIters = 20)
    {
        // Starting value
        var En = ecc < 0.8 ? mean : Math.PI;

        // Newton to find eccentric anomaly (En)
        for (var i = 0; i < maxIters; ++i)
        {
            var (sinEn, cosEn) = Math.SinCos(En);

            var a = En;
            var b = En - (En - ecc * sinEn - mean) / (1 - ecc * cosEn);
            En = b;

            var delta = Math.Abs((b - a) / b);
            if (delta < 0.001)
                break;
        }

        return En;
    }

    #region equality
    public bool Equals(KeplerOrbit other)
    {
        return Parent.Equals(other.Parent) && Elements.Equals(other.Elements);
    }

    public override bool Equals(object? obj)
    {
        return obj is KeplerOrbit other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Parent, Elements);
    }

    public static bool operator ==(KeplerOrbit left, KeplerOrbit right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(KeplerOrbit left, KeplerOrbit right)
    {
        return !(left == right);
    }
    #endregion
}

public record struct GravityMass(double Value) : IComponent;

public struct FixedBody : IComponent;