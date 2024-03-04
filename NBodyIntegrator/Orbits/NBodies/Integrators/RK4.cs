using System.Runtime.CompilerServices;
using NBodyIntegrator.Units;
using Unity.Mathematics;

namespace NBodyIntegrator.Orbits.NBodies.Integrators
{
    /// <summary>
    /// https://gafferongames.com/post/integration_basics/
    /// </summary>
    /// <typeparam name="TQuery"></typeparam>
    public struct RK4<TQuery>
        where TQuery : struct, IAccelerationQuery
    {
        public void Integrate(ref Metre3 position, ref Metre3 velocity, ref double timestamp, double dt, ref TQuery accel)
        {
            var initial = new State
            {
                x = position.Value,
                v = velocity.Value,
            };

            var a = Evaluate(initial, timestamp, dt * 0.0, default, ref accel);
            var b = Evaluate(initial, timestamp, dt * 0.5, a, ref accel);
            var c = Evaluate(initial, timestamp, dt * 0.5, b, ref accel);
            var d = Evaluate(initial, timestamp, dt * 1.0, c, ref accel);

            var dxdt = 1.0 / 6.0 * (a.dx + 2.0f * (b.dx + c.dx) + d.dx);
            var dvdt = 1.0 / 6.0 * (a.dv + 2.0f * (b.dv + c.dv) + d.dv);

            position.Value += dxdt * dt;
            velocity.Value += dvdt * dt;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Derivative Evaluate(State initial, double t, double dt, in Derivative d, ref TQuery accel)
        {
            State state;
            state.x = initial.x + d.dx * dt;
            state.v = initial.v + d.dv * dt;

            Derivative output;
            output.dx = state.v;
            output.dv = accel.Acceleration(new Metre3(state.x), t + dt).Value;
            return output;
        }

        private struct State
        {
            public double3 x;
            public double3 v;
        }

        private struct Derivative
        {
            public double3 dx;
            public double3 dv;
        }
    }
}
