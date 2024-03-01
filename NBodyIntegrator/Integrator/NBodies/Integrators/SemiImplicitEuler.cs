using NBodyIntegrator.Units;

namespace NBodyIntegrator.Integrator.NBodies.Integrators
{
    public readonly struct SemiImplicitEuler<TQuery>
        where TQuery : struct, IAccelerationQuery
    {
        public void Integrate(ref Metre3 position, ref Metre3 velocity, ref double timestamp, ref double dt, ref TQuery accel)
        {
            timestamp += dt;

            var x0 = position;
            var v0 = velocity;

            var v1 = v0 + accel.Acceleration(position, timestamp) * dt;
            var x1 = x0 + v1 * dt;

            position = x1;
            velocity = v1;
        }
    }
}