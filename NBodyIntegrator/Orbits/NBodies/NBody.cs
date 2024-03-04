using System.Globalization;
using System.Runtime.InteropServices;
using Myriad.ECS;
using NBodyIntegrator.Units;

namespace NBodyIntegrator.Orbits.NBodies;

public enum NBodyPrecision
{
    VeryLow = -2,
    Low = -1,
    Medium = 0,
    High = 1,
    VeryHigh = 2
}

public static class NBodyPrecisionExtensions
{
    public static double Epsilon(this NBodyPrecision precision, double baseEpsilon)
    {
        var factor = precision switch
        {
            NBodyPrecision.VeryLow => 4,
            NBodyPrecision.Low => 2,
            NBodyPrecision.Medium => 1,
            NBodyPrecision.High => 0.5,
            NBodyPrecision.VeryHigh => 0.25,
            _ => 1,
        };

        return baseEpsilon * factor;
    }
}

[StructLayout(LayoutKind.Sequential, Size = 64)]
public struct NBody
    : IComponent
{
    /// <summary>
    /// Timestep for this body
    /// </summary>
    public double DeltaTime;

    /// <summary>
    /// Maximum length this rail may be (measured as time from start to end)
    /// </summary>
    public double MaximumTimeLength;

    /// <summary>
    /// Amount of time to integrate between points on the rail
    /// </summary>
    public double RailTimestep;

    /// <summary>
    /// Set how precise the orbital integrator is.
    /// </summary>
    public NBodyPrecision IntegratorPrecision;

    /// <summary>
    /// Data is stored in three dynamic buffers. This same "RailPoint" circular buffer
    /// manages all three of them, so they remain perfectly in sync.
    /// </summary>
    public CircularBufferIndexer RailPoints;

    public struct Timestamp(double value)
    {
        public double Value = value;

        public static implicit operator double(Timestamp self) => self.Value;
        public static implicit operator Timestamp(double self) => new() { Value = self };

        public readonly override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
    }

    public struct Position(double x, double y, double z)
    {
        public Metre3 Value = new(x, y, z);

        public static implicit operator Metre3(Position self) => self.Value;
        public static implicit operator Position(Metre3 self) => new() { Value = self };

        public readonly override string ToString() => Value.ToString();
    }

    public struct Velocity(double x, double y, double z)
    {
        public Metre3 Value = new(x, y, z);

        public static implicit operator Metre3(Velocity self) => self.Value;
        public static implicit operator Velocity(Metre3 self) => new() { Value = self };

        public readonly override string ToString() => Value.ToString();
    }
}