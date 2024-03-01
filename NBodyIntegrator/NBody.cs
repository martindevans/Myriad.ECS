using System.Globalization;
using Myriad.ECS;
using NBodyIntegrator.Units;

namespace NBodyIntegrator;

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
    private static double Epsilon(this NBodyPrecision precision, double baseEpsilon)
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

public struct NBody
    : IComponent
{
    /// <summary>
    /// Timestep for this body
    /// </summary>
    public double DeltaTime;

    /// <summary>
    /// Set how precise the orbital integrator is.
    /// </summary>
    public NBodyPrecision IntegratorPrecision;

    /// <summary>
    /// Data is stored in three dynamic buffers. This same "RailPoint" circular buffer
    /// manages all three of them, so they remain perfectly in sync.
    /// </summary>
    public DynamicCircularBuffer RailPoints;

    public struct Timestamp
    {
        public double Value;

        public Timestamp(double value)
        {
            Value = value;
        }

        public static implicit operator double(Timestamp self) => self.Value;
        public static implicit operator Timestamp(double self) => new() { Value = self };

        public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
    }

    public struct Position
    {
        public Metre3 Value;

        public Position(double x, double y, double z)
        {
            Value = new Metre3(x, y, z);
        }

        public static implicit operator Metre3(Position self) => self.Value;
        public static implicit operator Position(Metre3 self) => new() { Value = self };

        public override string ToString() => Value.ToString();
    }

    public struct Velocity
    {
        public Metre3 Value;

        public Velocity(double x, double y, double z)
        {
            Value = new Metre3(x, y, z);
        }

        public static implicit operator Metre3(Velocity self) => self.Value;
        public static implicit operator Velocity(Metre3 self) => new() { Value = self };

        public override string ToString() => Value.ToString();
    }
}