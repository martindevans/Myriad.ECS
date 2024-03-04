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
    private readonly QueryDescription _nbodyQuery = new QueryBuilder()
        .Include<NBody>()
        .Include<EntityArray<NBody.Position>>()
        .Include<EntityArray<NBody.Velocity>>()
        .Include<EntityArray<NBody.Timestamp>>()
        .Build(world);

    private KeplerObject[] _keplerMasses = [];

    public bool Enabled { get; set; } = true;

    public void Init()
    {
        _keplerMasses = KeplerWorldPosition.FindKeplerBodies(world);
    }

    public void Update(GameTime time)
    {
        world.ExecuteParallel<Integrate, NBody, EntityArray<NBody.Position>, EntityArray<NBody.Velocity>, EntityArray<NBody.Timestamp>>(
            new Integrate(_keplerMasses),
            _nbodyQuery,
            batchSize: 16
        );
    }

    public void Dispose()
    {
    }

    private readonly struct Integrate(KeplerObject[] keplerMasses)
        : IQuery4<NBody, EntityArray<NBody.Position>, EntityArray<NBody.Velocity>, EntityArray<NBody.Timestamp>>
    {
        public void Execute(Entity e, ref NBody nbody, ref EntityArray<NBody.Position> positions, ref EntityArray<NBody.Velocity> velocities, ref EntityArray<NBody.Timestamp> times)
        {
            // Get the index of the first and last points in the rail
            var firstIndex = nbody.RailPoints.IndexAt(0)!.Value;
            var lastIndex = nbody.RailPoints.IndexAt(nbody.RailPoints.Count - 1)!.Value;

            // If we've hit the length target there's no point integrating anything
            if (firstIndex != lastIndex)
            {
                var a = times.Array[firstIndex];
                var b = times.Array[lastIndex];
                var totalTimeLength = b.Value - a.Value;

                if (totalTimeLength > nbody.MaximumTimeLength)
                    return;
            }

            // If the rail is full it needs to be grown
            if (nbody.RailPoints.IsFull())
            {
                // Create a new circular indexer twice the size
                var cbold = nbody.RailPoints;
                nbody.RailPoints = new CircularBufferIndexer(cbold.Count * 2);

                // Get copies to the old arrays
                var pold = positions.Array;
                var vold = velocities.Array;
                var told = times.Array;

                // Create new arrays
                positions = new EntityArray<NBody.Position>(new NBody.Position[nbody.RailPoints.Capacity]);
                velocities = new EntityArray<NBody.Velocity>(new NBody.Velocity[nbody.RailPoints.Capacity]);
                times = new EntityArray<NBody.Timestamp>(new NBody.Timestamp[nbody.RailPoints.Capacity]);

                // Copy datapoints
                for (var i = 0; i < cbold.Count; i++)
                {
                    var iold = cbold.IndexAt(i)!.Value;
                    var inew = nbody.RailPoints.TryAdd()!.Value;

                    positions.Array[inew] = pold[iold];
                    velocities.Array[inew] = vold[iold];
                    times.Array[inew] = told[iold];
                }

                // Reculate first and last index
                firstIndex = nbody.RailPoints.IndexAt(0)!.Value;
                lastIndex = nbody.RailPoints.IndexAt(nbody.RailPoints.Count - 1)!.Value;
            }

            // Choose an integrator
            //Euler<AccelerationQuery> _integrator = new();
            //SemiImplicitEuler<AccelerationQuery> _integrator = new();
            //Leapfrog<AccelerationQuery> _integrator = new();
            //RK4<AccelerationQuery> _integrator = new();
            var integrator = new RKF45(nbody.IntegratorPrecision.Epsilon(RKF45.DefaultEpsilon), RKF45.DefaultMinDt, RKF45.DefaultMaxDt);

            var q = new AccelerationQuery(keplerMasses);

            // Get the index of the final point in the rail and copy out data
            var pos = positions.Array[lastIndex].Value;
            var vel = velocities.Array[lastIndex].Value;
            var tim = times.Array[lastIndex].Value;
            var dt = nbody.DeltaTime;

            // Do lots of integration steps, however many is required for 1 second of time to pass (with a cap of max steps, just in case)
            var totalIntegratedTime = 0.0;
            for (var i = 0; i < 64 && totalIntegratedTime < nbody.RailTimestep; i++)
            {
                integrator.Integrate(ref pos, ref vel, ref tim, ref dt, ref q);
                totalIntegratedTime += dt;
            }

            // Store results
            var index = nbody.RailPoints.TryAdd()!.Value;
            positions.Array[index] = pos;
            velocities.Array[index] = vel;
            times.Array[index] = tim;
            nbody.DeltaTime = dt;
        }
    }

    private readonly struct AccelerationQuery(Memory<KeplerObject> keplerMasses)
        : IAccelerationQuery
    {
        private const double G = 6.67e-11;

        public Metre3 Acceleration(Metre3 position, double time)
        {
            // Allocate some temporary space to store the position and mass of every body
            Span<(Metre3, double)> masses = stackalloc (Metre3, double)[keplerMasses.Length];
            SetTime(time, masses);

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
            return new Metre3(accel);
        }

        private void SetTime(double timestamp, Span<(Metre3, double)> masses)
        {
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
        }
    }
}