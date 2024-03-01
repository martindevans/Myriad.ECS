using NBodyIntegrator.Units;

namespace NBodyIntegrator.Integrator.NBodies.Integrators
{
    /// <summary>
    /// https://en.wikipedia.org/wiki/Leapfrog_integration#4th_order_Yoshida_integrator
    /// </summary>
    public readonly struct Leapfrog<TQuery>
        where TQuery : struct, IAccelerationQuery
    {
        private const double C1 = +0.6756035959798;
        private const double C2 = (D1 + D2) / 2;
        private const double C3 = C2;
        private const double C4 = C1;

        private const double D1 = +1.3512071919596576;
        private const double D2 = -1.702414383919315;
        private const double D3 = D1;

        public void Integrate(ref Metre3 position, ref Metre3 velocity, ref double timestamp, ref double dt, ref TQuery accel)
        {
            timestamp += dt;

            var x0 = position;
            var v0 = velocity;

            var x1 = x0 + C1 * v0 * dt;
            var v1 = v0 + D1 * accel.Acceleration(x1, timestamp) * dt;

            var x2 = x1 + C2 * v1 * dt;
            var v2 = v1 + D2 * accel.Acceleration(x2, timestamp) * dt;

            var x3 = x2 + C3 * v2 * dt;
            var v3 = v2 + D3 * accel.Acceleration(x3, timestamp) * dt;

            position = x3 + C4 * v3 * dt;
            velocity = v3;
        }
    }
}
