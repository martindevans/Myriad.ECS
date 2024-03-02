using System.Runtime.CompilerServices;
using NBodyIntegrator.Units;
using Unity.Mathematics;

namespace NBodyIntegrator.Integrator.NBodies.Integrators
{
    /// <summary>
    /// https://en.wikipedia.org/wiki/Runge%E2%80%93Kutta%E2%80%93Fehlberg_method
    /// </summary>
    public readonly struct RKF45
    {
        #region constants
        //A(K)
        private const double A1 = 0;
        private const double A2 = 2 / 9D;
        private const double A3 = 1 / 12D;
        private const double A4 = 69 / 128D;
        private const double A5 = -17 / 12D;
        private const double A6 = 65 / 432D;

        ////C(K)
        //private static double C1 = 1 / 9D;
        //private static double C2 = 0;
        //private static double C3 = 9 / 20D;
        //private static double C4 = 16 / 45D;
        //private static double C5 = 1 / 12D;

        //CH(K)
        private const double CH1 = 47 / 450D;
        private const double CH2 = 0;
        private const double CH3 = 12 / 25D;
        private const double CH4 = 32 / 225D;
        private const double CH5 = 1 / 30D;
        private const double CH6 = 6 / 25D;

        //CT(K)
        private const double CT1 = -1 / 150D;
        private const double CT2 = 0;
        private const double CT3 = 3 / 100D;
        private const double CT4 = -16 / 75D;
        private const double CT5 = -1 / 20D;
        private const double CT6 = 6 / 25D;

        //B(K,L)
        private const double B21 = 2 / 9D;
        private const double B31 = 1 / 12D;
        private const double B41 = 69 / 128D;
        private const double B51 = -17 / 12D;
        private const double B61 = 65 / 432D;
        private const double B32 = 1 / 4D;
        private const double B42 = -243 / 128D;
        private const double B52 = 27 / 4D;
        private const double B62 = -5 / 16D;
        private const double B43 = 135 / 64D;
        private const double B53 = -27 / 5D;
        private const double B63 = 13 / 16D;
        private const double B54 = 16 / 15D;
        private const double B64 = 4 / 27D;
        private const double B65 = 5 / 144D;
        #endregion

        public const double DefaultMinDt = 0.1;
        public const double DefaultMaxDt = 600;
        public const double DefaultEpsilon = 0.00125;

        private readonly double? _epsilon;
        private readonly double? _minDt;
        private readonly double? _maxDt;

        public RKF45(double epsilon, double minDt, double maxDt)
        {
            _epsilon = epsilon;
            _minDt = minDt;
            _maxDt = maxDt;
        }

        public void Integrate<TQuery>(ref Metre3 position, ref Metre3 velocity, ref double timestamp, ref double dt, ref TQuery accel)
            where TQuery : struct, IAccelerationQuery
        {
            var epsilon = _epsilon ?? DefaultEpsilon;
            var minDt = _minDt ?? DefaultMinDt;
            var maxDt = _maxDt ?? DefaultMaxDt;

            var initial = new State
            {
                x = position.Value,
                v = velocity.Value,
            };

            // Calculate factors
            var k1 = dt * F(timestamp, A1 * dt, initial, ref accel);
            var k2 = dt * F(timestamp, A2 * dt, initial + B21 * k1, ref accel);
            var k3 = dt * F(timestamp, A3 * dt, initial + B31 * k1 + B32 * k2, ref accel);
            var k4 = dt * F(timestamp, A4 * dt, initial + B41 * k1 + B42 * k2 + B43 * k3, ref accel);
            var k5 = dt * F(timestamp, A5 * dt, initial + B51 * k1 + B52 * k2 + B53 * k3 + B54 * k4, ref accel);
            var k6 = dt * F(timestamp, A6 * dt, initial + B61 * k1 + B62 * k2 + B63 * k3 + B64 * k4 + B65 * k5, ref accel);

            // Calculate final result
            var final = initial + CH1 * k1 + CH2 * k2 + CH3 * k3 + CH4 * k4 + CH5 * k5 + CH6 * k6;
            position = new Metre3(final.x);
            velocity = new Metre3(final.v);
            timestamp += dt;

            // Calculate truncation error
            var error = Error(k1, k2, k3, k4, k5, k6);

            // Update timestep
            var hnew = 0.9 * dt * Math.Pow(epsilon / error, 0.2);
            dt = Math.Clamp(hnew, minDt, maxDt);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double Error(in Derivative k1, in Derivative k2, in Derivative k3, in Derivative k4, in Derivative k5, in Derivative k6)
        {
            var err = CT1 * k1 + CT2 * k2 + CT3 * k3 + CT4 * k4 + CT5 * k5 + CT6 * k6;
            err.dx = err.dx.Abs();
            err.dv = err.dv.Abs();
            return Math.Sqrt(err.dx.LengthSquared() + err.dv.LengthSquared());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Derivative F<TQuery>(double t, double dt, State state, ref TQuery accel)
            where TQuery : struct, IAccelerationQuery
        {
            Derivative output;
            output.dx = state.v;
            output.dv = accel.Acceleration(new Metre3(state.x), t + dt).Value;
            return output;
        }

        private struct State
        {
            public double3 x;
            public double3 v;

            public static State operator +(State a, Derivative b)
            {
                return new State
                {
                    x = a.x + b.dx,
                    v = a.v + b.dv
                };
            }
        }

        private struct Derivative
        {
            public double3 dx;
            public double3 dv;

            public static Derivative operator *(in Derivative a, double b)
            {
                return b * a;
            }

            public static Derivative operator *(double a, in Derivative b)
            {
                return new Derivative
                {
                    dx = b.dx * a,
                    dv = b.dv * a
                };
            }

            public static Derivative operator +(in Derivative a, in Derivative b)
            {
                return new Derivative
                {
                    dx = a.dx + b.dx,
                    dv = a.dv + b.dv,
                };
            }
        }
    }
}
