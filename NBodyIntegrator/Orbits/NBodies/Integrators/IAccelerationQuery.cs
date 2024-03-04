using NBodyIntegrator.Units;

namespace NBodyIntegrator.Orbits.NBodies.Integrators;

public interface IAccelerationQuery
{
    public Metre3 Acceleration(Metre3 position, double time);
}