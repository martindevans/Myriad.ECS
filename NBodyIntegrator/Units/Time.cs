namespace NBodyIntegrator.Units;

public struct Hours
    : IEquatable<Hours>
{
    public double Value;

    public Hours(double value)
    {
        Value = value;
    }

    public readonly override string ToString()
    {
        return $"{Value}hr";
    }

    public static explicit operator Seconds(Hours hr)
    {
        return new Seconds(hr.Value * 3600);
    }

    #region equality
    public readonly bool Equals(Hours other)
    {
        return Value.Equals(other.Value);
    }

    public readonly override bool Equals(object? obj)
    {
        return obj is Hours other && Equals(other);
    }

    public readonly override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==(Hours left, Hours right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Hours left, Hours right)
    {
        return !(left == right);
    }
    #endregion
}

public struct Seconds
    : IEquatable<Seconds>, IComparable<Seconds>, IComparable<double>
{
    public double Value;

    public Seconds(double value)
    {
        Value = value;
    }

    public readonly override string ToString()
    {
        return $"{Value}s";
    }

    #region equality
    public readonly bool Equals(Seconds other)
    {
        return Value.Equals(other.Value);
    }

    public readonly override bool Equals(object? obj)
    {
        return obj is Seconds other && Equals(other);
    }

    public readonly override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==(Seconds left, Seconds right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Seconds left, Seconds right)
    {
        return !(left == right);
    }
    #endregion

    public readonly int CompareTo(Seconds other)
    {
        return Value.CompareTo(other.Value);
    }

    public readonly int CompareTo(double other)
    {
        return Value.CompareTo(other);
    }

    public static bool operator <(Seconds a, double b)
    {
        return a.Value < b;
    }

    public static bool operator <=(Seconds a, double b)
    {
        return a.Value <= b;
    }

    public static bool operator >(Seconds a, double b)
    {
        return a.Value > b;
    }

    public static bool operator >=(Seconds a, double b)
    {
        return a.Value >= b;
    }

    public static bool operator <(double a, Seconds b)
    {
        return a < b.Value;
    }

    public static bool operator <=(double a, Seconds b)
    {
        return a <= b.Value;
    }

    public static bool operator >(double a, Seconds b)
    {
        return a > b.Value;
    }

    public static bool operator >=(double a, Seconds b)
    {
        return a >= b.Value;
    }
}