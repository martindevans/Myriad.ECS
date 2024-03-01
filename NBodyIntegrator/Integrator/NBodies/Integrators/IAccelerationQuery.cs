using NBodyIntegrator.Units;

namespace NBodyIntegrator.Integrator.NBodies.Integrators;

public interface IAccelerationQuery
{
    public Metre3 Acceleration(Metre3 position, double time);
}