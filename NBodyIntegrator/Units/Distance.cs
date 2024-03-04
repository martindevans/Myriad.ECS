using Unity.Mathematics;

namespace NBodyIntegrator.Units;

public struct Metre(double value)
    : IEquatable<Metre>
{
    public double Value = value;

    public static explicit operator MegaMetre(Metre metres)
    {
        return new MegaMetre(metres.Value / 1000000);
    }

    public static explicit operator AstronomicalUnit(Metre metres)
    {
        return new AstronomicalUnit(metres.Value / 1000149597870700);
    }

    public readonly override string ToString()
    {
        return $"{Value}m";
    }

    #region equality
    public readonly bool Equals(Metre other)
    {
        return Value.Equals(other.Value);
    }

    public override bool Equals(object? obj)
    {
        return obj is Metre other && Equals(other);
    }

    public readonly override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==(Metre left, Metre right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Metre left, Metre right)
    {
        return !(left == right);
    }
    #endregion
}

public struct KiloMetre(double value)
    : IEquatable<KiloMetre>
{
    public double Value = value;

    public static explicit operator Metre(KiloMetre km)
    {
        return new Metre(km.Value * 1000);
    }

    public static explicit operator MegaMetre(KiloMetre km)
    {
        return new MegaMetre(km.Value / 1000);
    }

    public static explicit operator AstronomicalUnit(KiloMetre km)
    {
        return new AstronomicalUnit(km.Value / 149597870.7);
    }

    public readonly override string ToString()
    {
        return $"{Value:N0}km";
    }

    #region equality
    public readonly bool Equals(KiloMetre other)
    {
        return Value.Equals(other.Value);
    }

    public override bool Equals(object? obj)
    {
        return obj is KiloMetre other && Equals(other);
    }

    public override readonly int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==(KiloMetre left, KiloMetre right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(KiloMetre left, KiloMetre right)
    {
        return !(left == right);
    }
    #endregion
}

public struct MegaMetre(double value)
    : IEquatable<MegaMetre>
{
    public double Value = value;

    public static explicit operator Metre(MegaMetre mm)
    {
        return new Metre(mm.Value * 1000000);
    }

    public static explicit operator KiloMetre(MegaMetre mm)
    {
        return new KiloMetre(mm.Value * 1000);
    }

    public static explicit operator AstronomicalUnit(MegaMetre mm)
    {
        return new AstronomicalUnit(mm.Value / 1000149597.870700);
    }

    #region equality
    public readonly bool Equals(MegaMetre other)
    {
        return Value.Equals(other.Value);
    }

    public override bool Equals(object? obj)
    {
        return obj is MegaMetre other && Equals(other);
    }

    public readonly override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==(MegaMetre left, MegaMetre right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(MegaMetre left, MegaMetre right)
    {
        return !(left == right);
    }
    #endregion
}

public struct AstronomicalUnit(double value)
    : IEquatable<AstronomicalUnit>
{
    public double Value = value;

    public static explicit operator Metre(AstronomicalUnit au)
    {
        return new Metre(au.Value * 149597870700);
    }

    public static explicit operator KiloMetre(AstronomicalUnit au)
    {
        return new KiloMetre(au.Value * 149597870.7);
    }

    public readonly override string ToString()
    {
        return $"{Value:N0}au";
    }

    #region equality
    public readonly bool Equals(AstronomicalUnit other)
    {
        return Value.Equals(other.Value);
    }

    public readonly override bool Equals(object? obj)
    {
        return obj is AstronomicalUnit other && Equals(other);
    }

    public readonly override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==(AstronomicalUnit left, AstronomicalUnit right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(AstronomicalUnit left, AstronomicalUnit right)
    {
        return !(left == right);
    }
    #endregion
}

/// <summary>
/// Distance unit in the scene. 1 unit is 1000 kilometres
/// </summary>
public struct SceneUnit(double value)
    : IEquatable<SceneUnit>
{
    public const double MetersPerUnit = 1000000;
    public const double KilometersPerUnit = MetersPerUnit / 1000;

    public double Value = value;

    public static explicit operator SceneUnit(KiloMetre km)
    {
        return new SceneUnit(km.Value / 1000);
    }

    public static explicit operator KiloMetre(SceneUnit su)
    {
        return new KiloMetre(su.Value * 1000);
    }

    public readonly override string ToString()
    {
        return $"{Value:N0}units";
    }

    #region equality
    public readonly bool Equals(SceneUnit other)
    {
        return Value.Equals(other.Value);
    }

    public readonly override bool Equals(object? obj)
    {
        return obj is SceneUnit other && Equals(other);
    }

    public readonly override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==(SceneUnit left, SceneUnit right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(SceneUnit left, SceneUnit right)
    {
        return !(left == right);
    }
    #endregion
}


public struct Metre3
    : IEquatable<Metre3>
{
    public double3 Value;

    public Metre3(double x, double y, double z)
    {
        Value = new double3(x, y, z);
    }

    public Metre3(double3 value)
    {
        Value = value;
    }

    public readonly override string ToString()
    {
        return $"{Value}m";
    }

    public static Metre3 operator +(Metre3 a, Metre3 b) => new(a.Value + b.Value);
    public static Metre3 operator -(Metre3 a, Metre3 b) => new(a.Value - b.Value);
    public static Metre3 operator *(Metre3 a, double b) => new(a.Value * b);
    public static Metre3 operator *(double a, Metre3 b) => new(a * b.Value);
    public static Metre3 operator /(Metre3 a, double b) => new(a.Value / b);

    public static explicit operator MegaMetre3(Metre3 metres)
    {
        return new MegaMetre3(metres.Value / 1000000);
    }

    public static explicit operator AstronomicalUnit3(Metre3 metres)
    {
        return new AstronomicalUnit3(metres.Value / 1000149597870700);
    }

    #region equality
    public bool Equals(Metre3 other)
    {
        return Value.Equals(other.Value);
    }

    public override bool Equals(object? obj)
    {
        return obj is Metre3 other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==(Metre3 left, Metre3 right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Metre3 left, Metre3 right)
    {
        return !(left == right);
    }
    #endregion
}

public struct KiloMetre3
    : IEquatable<KiloMetre3>
{
    public double3 Value;

    public KiloMetre3(double x, double y, double z)
    {
        Value = new double3(x, y, z);
    }

    public KiloMetre3(double3 value)
    {
        Value = value;
    }

    public readonly override string ToString()
    {
        return $"{Value}km";
    }

    public static Metre3 operator +(KiloMetre3 a, KiloMetre3 b) => new(a.Value + b.Value);
    public static Metre3 operator -(KiloMetre3 a, KiloMetre3 b) => new(a.Value - b.Value);
    public static Metre3 operator *(KiloMetre3 a, double b) => new(a.Value * b);
    public static Metre3 operator *(double a, KiloMetre3 b) => new(a * b.Value);
    public static Metre3 operator /(KiloMetre3 a, double b) => new(a.Value / b);

    public static explicit operator KiloMetre3(Metre3 metres)
    {
        return new KiloMetre3(metres.Value / 1000);
    }

    public static explicit operator Metre3(KiloMetre3 metres)
    {
        return new Metre3(metres.Value * 1000);
    }

    #region equality
    public bool Equals(KiloMetre3 other)
    {
        return Value.Equals(other.Value);
    }

    public override bool Equals(object? obj)
    {
        return obj is KiloMetre3 other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==(KiloMetre3 left, KiloMetre3 right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(KiloMetre3 left, KiloMetre3 right)
    {
        return !(left == right);
    }
    #endregion
}

public struct MegaMetre3
    : IEquatable<MegaMetre3>
{
    public double3 Value;

    public MegaMetre3(double x, double y, double z)
    {
        Value = new double3(x, y, z);
    }

    public MegaMetre3(double3 value)
    {
        Value = value;
    }

    public readonly override string ToString()
    {
        return $"{Value}Mm";
    }

    public static MegaMetre3 operator +(MegaMetre3 a, MegaMetre3 b) => new(a.Value + b.Value);
    public static MegaMetre3 operator -(MegaMetre3 a, MegaMetre3 b) => new(a.Value - b.Value);
    public static MegaMetre3 operator *(MegaMetre3 a, double b) => new(a.Value * b);
    public static MegaMetre3 operator /(MegaMetre3 a, double b) => new(a.Value / b);

    public static explicit operator MegaMetre3(double3 mm)
    {
        return new(mm);
    }

    public static explicit operator Metre3(MegaMetre3 mm)
    {
        return new Metre3(mm.Value * 1000000);
    }

    public static explicit operator AstronomicalUnit3(MegaMetre3 mm)
    {
        return new AstronomicalUnit3(mm.Value / 1000149597.870700);
    }

    #region equality
    public bool Equals(MegaMetre3 other)
    {
        return Value.Equals(other.Value);
    }

    public override bool Equals(object? obj)
    {
        return obj is MegaMetre3 other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==(MegaMetre3 left, MegaMetre3 right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(MegaMetre3 left, MegaMetre3 right)
    {
        return !(left == right);
    }
    #endregion
}

public struct AstronomicalUnit3
    : IEquatable<AstronomicalUnit3>
{
    public double3 Value;

    public AstronomicalUnit3(double x, double y, double z)
    {
        Value = new double3(x, y, z);
    }

    public AstronomicalUnit3(double3 value)
    {
        Value = value;
    }

    public readonly override string ToString()
    {
        return $"{Value}au";
    }

    public static explicit operator Metre3(AstronomicalUnit3 au)
    {
        return new Metre3(au.Value * 1000149597870700);
    }

    public static explicit operator AstronomicalUnit3(MegaMetre mm)
    {
        return new AstronomicalUnit3(mm.Value * 1000149597.870700);
    }

    #region equality
    public bool Equals(AstronomicalUnit3 other)
    {
        return Value.Equals(other.Value);
    }

    public override bool Equals(object? obj)
    {
        return obj is AstronomicalUnit3 other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==(AstronomicalUnit3 left, AstronomicalUnit3 right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(AstronomicalUnit3 left, AstronomicalUnit3 right)
    {
        return !(left == right);
    }
    #endregion
}

public struct SceneUnit3
    : IEquatable<SceneUnit3>
{
    public const double MetersPerUnit = SceneUnit.MetersPerUnit;
    public const double KilometersPerUnit = SceneUnit.KilometersPerUnit;

    public double3 Value;

    public SceneUnit3(double x, double y, double z)
    {
        Value = new double3(x, y, z);
    }

    public SceneUnit3(double3 value)
    {
        Value = value;
    }

    public readonly override string ToString()
    {
        return $"{Value}scene_units";
    }

    public static explicit operator SceneUnit3(Metre3 metres)
    {
        return new SceneUnit3(metres.Value / MetersPerUnit);
    }

    public static explicit operator SceneUnit3(KiloMetre3 metres)
    {
        return new SceneUnit3(metres.Value / KilometersPerUnit);
    }

    public static explicit operator KiloMetre3(SceneUnit3 su)
    {
        return new KiloMetre3(su.Value * KilometersPerUnit);
    }

    public static SceneUnit3 operator +(SceneUnit3 a, SceneUnit3 b) => new(a.Value + b.Value);
    public static SceneUnit3 operator -(SceneUnit3 a, SceneUnit3 b) => new(a.Value - b.Value);
    public static SceneUnit3 operator *(SceneUnit3 a, double b) => new(a.Value * b);
    public static SceneUnit3 operator *(double a, SceneUnit3 b) => new(a * b.Value);
    public static SceneUnit3 operator /(SceneUnit3 a, double b) => new(a.Value / b);

    #region equality
    public bool Equals(SceneUnit3 other)
    {
        return Value.Equals(other.Value);
    }

    public override bool Equals(object? obj)
    {
        return obj is SceneUnit3 other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==(SceneUnit3 left, SceneUnit3 right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(SceneUnit3 left, SceneUnit3 right)
    {
        return !(left == right);
    }
    #endregion
}