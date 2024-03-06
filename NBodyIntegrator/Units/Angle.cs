using Unity.Mathematics;

namespace NBodyIntegrator.Units;

[Serializable]
public struct Radians(double value)
    : IEquatable<Radians>
{
    public double Value = value;

    public static explicit operator Degrees(Radians r)
    {
        const double ConvertToDegrees = 360 / Math.Tau;

        return new Degrees(r.Value * ConvertToDegrees);
    }

    public static explicit operator double(Radians r)
    {
        return r.Value;
    }

    public readonly override string ToString()
    {
        return $"{Value}rad";
    }

    #region equality
    public readonly bool Equals(Radians other)
    {
        return Value.Equals(other.Value);
    }

    public readonly override bool Equals(object? obj)
    {
        return obj is Radians other && Equals(other);
    }

    public readonly override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==(Radians left, Radians right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Radians left, Radians right)
    {
        return !(left == right);
    }
    #endregion
}

[Serializable]
public struct Degrees(double value)
    : IEquatable<Degrees>
{
    public double Value = value;

    public static explicit operator Radians(Degrees d)
    {
        const double ConvertToRadians = Math.Tau / 360;

        return new Radians(d.Value * ConvertToRadians);
    }

    public static explicit operator double(Degrees d)
    {
        return d.Value;
    }

    public readonly override string ToString()
    {
        return $"{Value}°";
    }

    #region equality
    public readonly bool Equals(Degrees other)
    {
        return Value.Equals(other.Value);
    }

    public readonly override bool Equals(object? obj)
    {
        return obj is Degrees other && Equals(other);
    }

    public readonly override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==(Degrees left, Degrees right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Degrees left, Degrees right)
    {
        return !(left == right);
    }
    #endregion
}