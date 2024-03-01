using NBodyIntegrator.Units;

namespace NBodyIntegrator.Integrator.NBodies.Integrators
{
    /// <summary>
    /// Simple euler integrator. Extremely bad quality, do not use.
    /// </summary>
    /// <typeparam name="TQuery"></typeparam>
    public readonly struct Euler<TQuery>
        where TQuery : struct, IAccelerationQuery
    {
        public void Integrate(ref Metre3 position, ref Metre3 velocity, ref double timestamp, ref double dt, ref TQuery accel)
        {
            timestamp += dt;

            var x0 = position;
            var v0 = velocity;

            var x1 = x0 + v0 * dt;
            var v1 = v0 + accel.Acceleration(position, timestamp) * dt;

            position = x1;
            velocity = v1;
        }
    }
}