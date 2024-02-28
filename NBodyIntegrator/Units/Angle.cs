using Unity.Mathematics;

namespace NBodyIntegrator.Units;

[Serializable]
public struct Radians
    : IEquatable<Radians>
{
    public double Value;

    public Radians(double value)
    {
        Value = value;
    }

    public static explicit operator Degrees(Radians d)
    {
        const double ConvertToDegrees = 360 / Math.Tau;

        return new Degrees(d.Value * ConvertToDegrees);
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
public struct Degrees
    : IEquatable<Degrees>
{
    public double Value;

    public Degrees(double value)
    {
        Value = value;
    }

    public static explicit operator Radians(Degrees d)
    {
        const double ConvertToRadians = Math.Tau / 360;

        return new Radians(d.Value * ConvertToRadians);
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