﻿using System.Globalization;
using System.Numerics;
using Myriad.ECS;
using NBodyIntegrator.Mathematics;
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

/// <summary>
/// Core component for NBody integrated items
/// </summary>
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
    public Seconds MaximumTimeLength;

    /// <summary>
    /// Amount of time to integrate between points on the rail
    /// </summary>
    public Seconds RailTimestep;

    /// <summary>
    /// Set how precise the orbital integrator is.
    /// </summary>
    public NBodyPrecision IntegratorPrecision;

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

public record struct Mass(double Value) : IComponent;

/// <summary>
/// List of upcoming burns, in time order
/// </summary>
/// <param name="Burns"></param>
public record struct EngineBurnSchedule(List<EngineBurn> Burns) : IComponent;

/// <summary>
/// An engine burn, applying force for some amount of time
/// </summary>
/// <param name="Start">Start timestamp</param>
/// <param name="End">End timestamp</param>
/// <param name="Force">Amount of force applies</param>
/// <param name="MassPerSecond">Amount of mass consumed per second</param>
public readonly record struct EngineBurn(Seconds Start, Seconds End, double Force, double MassPerSecond, double3 Direction)
    : IComparable<EngineBurn>
{
    public int CompareTo(EngineBurn other)
    {
        return Start.Value.CompareTo(other.Start.Value);
    }
}
