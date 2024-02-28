using NBodyIntegrator.Units;

namespace NBodyIntegrator;

public struct KeplerElements
    : IEquatable<KeplerElements>
{
    public double Eccentricity { get; private set; }
    public Metre SemiMajorAxis { get; private set; }
    public Degrees Inclination { get; private set; }
    public Degrees LongitudeOfAscendingNode { get; private set; }
    public Degrees ArgumentOfPeriapsis { get; private set; }
    public Seconds Period { get; private set; }
    public Degrees MeanAnomalyEpoch { get; private set; }

    public KeplerElements(double eccentricity, Metre sma, Degrees inclination, Degrees longitudeAscending, Degrees argPeriapsis, Seconds period, Degrees meanAnomalyEpoch)
    {
        Eccentricity = eccentricity;
        SemiMajorAxis = sma;
        Inclination = inclination;
        LongitudeOfAscendingNode = longitudeAscending;
        ArgumentOfPeriapsis = argPeriapsis;
        Period = period;
        MeanAnomalyEpoch = meanAnomalyEpoch;
    }

    public readonly override string ToString()
    {
        return $"ecc:{Eccentricity} sma:{SemiMajorAxis} inc:{Inclination} lan:{LongitudeOfAscendingNode} argp:{ArgumentOfPeriapsis} period:{Period} mae:{MeanAnomalyEpoch}";
    }

    #region equality
    public readonly bool Equals(KeplerElements other)
    {
        return Eccentricity.Equals(other.Eccentricity)
            && SemiMajorAxis.Equals(other.SemiMajorAxis)
            && Inclination.Equals(other.Inclination)
            && LongitudeOfAscendingNode.Equals(other.LongitudeOfAscendingNode)
            && ArgumentOfPeriapsis.Equals(other.ArgumentOfPeriapsis)
            && Period.Equals(other.Period)
            && MeanAnomalyEpoch.Equals(other.MeanAnomalyEpoch);
    }

    public readonly override bool Equals(object? obj)
    {
        return obj is KeplerElements other && Equals(other);
    }

    public readonly override int GetHashCode()
    {
        return HashCode.Combine(Eccentricity, SemiMajorAxis, Inclination, LongitudeOfAscendingNode, ArgumentOfPeriapsis, Period, MeanAnomalyEpoch);
    }
    #endregion
}