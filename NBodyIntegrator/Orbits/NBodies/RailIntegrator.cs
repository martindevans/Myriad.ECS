using Myriad.ECS;
using Myriad.ECS.Queries;
using Myriad.ECS.Systems;
using Myriad.ECS.Worlds;
using NBodyIntegrator.Orbits.Kepler;
using NBodyIntegrator.Orbits.NBodies.Integrators;
using NBodyIntegrator.Units;
using Unity.Mathematics;

namespace NBodyIntegrator.Orbits.NBodies;

public sealed class RailIntegrator(World world)
    : ISystem
{
    private KeplerObject[] _keplerMasses = [];

    public bool Enabled { get; set; } = true;

    public void Init()
    {
        _keplerMasses = KeplerWorldPosition.FindKeplerBodies(world);
    }

    public void Update(GameTime time)
    {
        world.ExecuteParallel<Integrate, NBody, PagedRail<NBody.Position>, PagedRail<NBody.Velocity>, PagedRail<NBody.Timestamp>, EngineBurnSchedule, Mass>(
            new Integrate(_keplerMasses),
            batchSize: 16
        );
    }

    public void Dispose()
    {
    }

    private readonly struct Integrate(KeplerObject[] keplerMasses)
        : IQuery6<NBody, PagedRail<NBody.Position>, PagedRail<NBody.Velocity>, PagedRail<NBody.Timestamp>, EngineBurnSchedule, Mass>
    {
        public void Execute(
            Entity e,
            ref NBody nbody, ref PagedRail<NBody.Position> positions, ref PagedRail<NBody.Velocity> velocities, ref PagedRail<NBody.Timestamp> times,
            ref EngineBurnSchedule schedule, ref Mass mass)
        {
            if (positions.Count != velocities.Count)
                throw new InvalidOperationException("Position/Velocity count mismatch");
            if (positions.Count != times.Count)
                throw new InvalidOperationException("Position/Velocity count mismatch");

            // If we've hit the length target there's no point integrating anything
            if (times.Count > 2)
            {
                var a = times.First().Value;
                var b = times.Last().Value;
                var totalTimeLength = b - a;

                if (totalTimeLength > nbody.MaximumTimeLength.Value)
                    return;
            }

            // Choose an integrator
            var integrator = new RKF45(nbody.IntegratorPrecision.Epsilon(RKF45.DefaultEpsilon), RKF45.DefaultMinDt, RKF45.DefaultMaxDt);

            var q = new AccelerationQuery(keplerMasses, mass.Value, schedule);

            // Get the index of the final point in the rail and copy out data
            var pos = positions.Last().Value;
            var vel = velocities.Last().Value;
            var tim = times.Last().Value;
            var dt = nbody.DeltaTime;

            // Do lots of integration steps, however many is required for 1 second of time to pass (with a cap of max steps, just in case)
            var totalIntegratedTime = 0.0;
            for (var i = 0; i < 64 && totalIntegratedTime < nbody.RailTimestep.Value; i++)
            {
                integrator.Integrate(ref pos, ref vel, ref tim, ref dt, ref q);
                totalIntegratedTime += dt;
            }

            // Store results
            positions.Add(pos);
            velocities.Add(vel);
            times.Add(tim);
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
                // if the burn is i the future then there's no current active burn
                if (burn.Start.Value > time)
                    return double3.zero;

                // If the burn is in the past subtract off all of the mass consumed
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

            return double3.zero;
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
            var accel = double3.zero;
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