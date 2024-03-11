using Myriad.ECS;
using Myriad.ECS.Queries;
using Myriad.ECS.Systems;
using Myriad.ECS.Worlds;
using NBodyIntegrator.Mathematics;
using NBodyIntegrator.Orbits.Kepler;
using NBodyIntegrator.Orbits.NBodies.Integrators;
using NBodyIntegrator.Units;

namespace NBodyIntegrator.Orbits.NBodies;

public sealed class RailIntegrator(World world)
    : BaseSystem, ISystemInit
{
    private const int MaxWork = 8;

    private KeplerObject[] _keplerMasses = [];

    public void Init()
    {
        _keplerMasses = KeplerWorldPosition.FindKeplerBodies(world);
    }

    public override void Update(GameTime time)
    {
        world.Execute<Integrate, NBody, PagedRail, EngineBurnSchedule, Mass>(
            new Integrate(_keplerMasses)
        );
    }

    private readonly struct Integrate(KeplerObject[] keplerMasses)
        : IQuery4<NBody, PagedRail, EngineBurnSchedule, Mass>
    {
        public void Execute(
            Entity e,
            ref NBody nbody, ref PagedRail rail, 
            ref EngineBurnSchedule schedule, ref Mass mass)
        {
            // Check if we've hit the target time length of the rail
            if (rail.Duration.Value >= nbody.MaximumTimeLength.Value)
                return;

            // Choose an integrator
            var integrator = new RKF45(nbody.IntegratorPrecision.Epsilon(RKF45.DefaultEpsilon));

            // Get the index of the final point in the rail and copy out data
            var (pos, vel, tim) = rail.LastState();
            var dt = nbody.DeltaTime;

            // Get somewhere to store results
            var output = rail.AddPage();

            // Now integrate enough steps to fill the new page
            var query = new AccelerationQuery(keplerMasses, mass.Value, schedule);
            for (var i = 0; i < output.Timestamps.Length; i++)
            {
                // Keep track of how much total time has been integrated, only store points
                // in the output span once it exceeds 1 second (or a work limit is reached).
                var timeAccumulator = 0.0;
                for (var s = 0; s < MaxWork && timeAccumulator < 1; s++)
                {
                    integrator.Integrate(ref pos, ref vel, ref tim, ref dt, ref query);
                    timeAccumulator += dt;
                }
                
                // Store a single data point
                output.Positions[i] = pos;
                output.Velocities[i] = vel;
                output.Timestamps[i] = tim;
            }

            // Store the final timestep
            nbody.DeltaTime = dt;
        }
    }

    private readonly struct AccelerationQuery(Memory<KeplerObject> keplerMasses, double startMass, EngineBurnSchedule schedule)
        : IAccelerationQuery
    {
        private const double G = 6.67e-11;

        public Metre3 Acceleration(Metre3 position, double time)
        {
            // Calculate acceleration due to gravity
            var gravity = CalculateMassAcceleration(time, position);

            // Calculate acceleration due to engines
            var engines = CalculateEngineAcceleration(time);

            return new Metre3(gravity + engines);
        }

        private double3 CalculateEngineAcceleration(double time)
        {
            var mass = startMass;
            foreach (var burn in schedule.Burns)
            {
                // if the burn is in the future then there's no current active burn
                if (burn.Start.Value > time)
                    return double3.Zero;

                // If the burn is in the past subtract off all of the mass consumed by that burn
                if (burn.End.Value < time)
                {
                    mass -= burn.MassPerSecond * (burn.End.Value - burn.Start.Value);
                }
                else
                {
                    // Burn is currently active
                    mass -= (time - burn.Start.Value) * burn.MassPerSecond;

                    return burn.Direction * burn.Force / mass;
                }
            }

            return double3.Zero;
        }

        private double3 CalculateMassAcceleration(double timestamp, Metre3 position)
        {
            // Allocate a span to store positions and masses in
            Span<(Metre3, double)> masses = stackalloc (Metre3, double)[keplerMasses.Length];

            // Calculate position for every mass
            var keplerMassesSpan = keplerMasses.Span;
            for (var i = 0; i < keplerMassesSpan.Length; i++)
            {
                var ko = keplerMassesSpan[i];

                if (ko.ParentIndex < 0)
                {
                    masses[i] = (ko.FixedPosition, ko.MassKg);
                }
                else
                {
                    var localPos = ko.Kepler.PositionAtTime(timestamp);
                    var wpos = masses[ko.ParentIndex].Item1 + localPos;
                    masses[i] = (wpos, ko.MassKg);
                }
            }

            // Calculate acceleration.
            // Add a small constant value to distance calculation. This solves the
            // problem of infinite force at zero distance and introduces only a very small error.
            const double close = 1;
            var accel = double3.Zero;
            for (var i = 0; i < masses.Length; i++)
            {
                var (p, m) = masses[i];

                var v = p - position;

                var lengthSqr = v.Value.LengthSquared() + close;
                var length = Math.Sqrt(lengthSqr);

                var f = G * m / lengthSqr;
                accel += v.Value / length * f;
            }
            return accel;
        }
    }
}