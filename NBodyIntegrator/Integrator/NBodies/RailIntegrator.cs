using Myriad.ECS;
using Myriad.ECS.Queries;
using Myriad.ECS.Systems;
using Myriad.ECS.Worlds;
using NBodyIntegrator.Integrator.NBodies.Integrators;
using NBodyIntegrator.Units;
using Unity.Mathematics;

namespace NBodyIntegrator.Integrator.NBodies;

public sealed class RailIntegrator(World world)
    : ISystem
{
    private readonly QueryDescription _nbodyQuery = new QueryBuilder()
        .Include<NBody>()
        .Include<EntityArray<NBody.Position>>()
        .Include<EntityArray<NBody.Velocity>>()
        .Include<EntityArray<NBody.Timestamp>>()
        .Build(world);

    private KeplerObject[] _keplerMasses = Array.Empty<KeplerObject>();

    public bool Enabled { get; set; } = true;

    public void Init()
    {
        var keplerMasses = new List<KeplerObject>();

        // Find all fixed bodies and store their information
        var fixedQuery = new QueryBuilder()
            .Include<FixedBody>().Include<WorldPosition>().Include<GravityMass>()
            .Build(world);
        foreach (var (e, _, w, g) in world.Query<FixedBody, WorldPosition, GravityMass>(fixedQuery))
            keplerMasses.Add(new(e, -1, default, w.Item.Value, g.Item.Value));

        // Find all non-fixed bodies
        var dynamicQuery = new QueryBuilder()
            .Include<GravityMass>().Include<KeplerOrbit>()
            .Build(world);

        // Store a list of all "unmatched" items. That is dynamic kepler bodies which we have
        // not yet found the parent index for.
        var unmatched = new List<(Entity, GravityMass, KeplerOrbit)>();
        foreach (var (e, g, k) in world.Query<GravityMass, KeplerOrbit>(dynamicQuery))
            unmatched.Add((e, g.Item, k.Item));

        // Now match all those unmatched items
        while (unmatched.Count > 0)
        {
            var ok = false;
            for (var i = unmatched.Count - 1; i >= 0; i--)
            {
                var item = unmatched[i];

                var idx = keplerMasses.FindIndex(a => a.Self == item.Item3.Parent);
                if (idx < 0)
                    continue;

                ok = true;
                keplerMasses.Add(new KeplerObject(item.Item1, idx, item.Item3, default, item.Item2.Value));
                unmatched.RemoveAt(i);
            }

            if (!ok)
                throw new InvalidOperationException("Failed to match Kepler mass with parent body");
        }

        _keplerMasses = keplerMasses.ToArray();
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

    private record struct KeplerObject(Entity Self, int ParentIndex, KeplerOrbit Kepler, Metre3 FixedPosition, double MassKg);

    private readonly struct Integrate
        : IQuery4<NBody, EntityArray<NBody.Position>, EntityArray<NBody.Velocity>, EntityArray<NBody.Timestamp>>
    {
        private readonly KeplerObject[] _keplerMasses;

        //private readonly Euler<AccelerationQuery> _integrator = new();
        //private readonly SemiImplicitEuler<AccelerationQuery> _integrator = new();
        //private readonly Leapfrog<AccelerationQuery> _integrator = new();
        //private readonly RK4<AccelerationQuery> _integrator = new();
        private readonly RKF45 _integrator = new();

        public Integrate(KeplerObject[] keplerMasses)
        {
            _keplerMasses = keplerMasses;
        }

        public void Execute(Entity e, ref NBody nbody, ref EntityArray<NBody.Position> positions, ref EntityArray<NBody.Velocity> velocities, ref EntityArray<NBody.Timestamp> times)
        {
            if (nbody.RailPoints.IsFull(positions.Length))
                return;

            var q = new AccelerationQuery(_keplerMasses);

            // Get the index of the final point in the rail and copy out data
            var last = nbody.RailPoints.IndexAt(positions.Length, nbody.RailPoints.Count - 1)!.Value;

            var pos = positions.Array[last].Value;
            var vel = velocities.Array[last].Value;
            var tim = times.Array[last].Value;
            var dt = nbody.DeltaTime;

            // Do lots of integration steps, however many is required for 1 second of time to pass (with a cap of max steps, just in case)
            var totalIntegratedTime = 0.0;
            for (var i = 0; i < 64 && totalIntegratedTime < 1; i++)
            {
                _integrator.Integrate(ref pos, ref vel, ref tim, ref dt, ref q);
                totalIntegratedTime += dt;
            }

            // Store results
            var index = nbody.RailPoints.TryAdd(positions.Length)!.Value;
            positions.Array[index] = pos;
            velocities.Array[index] = vel;
            times.Array[index] = tim;
            nbody.DeltaTime = dt;
        }
    }

    private readonly struct AccelerationQuery
        : IAccelerationQuery
    {
        private const double G = 6.67e-11;

        private readonly Memory<KeplerObject> _keplerMasses;

        public AccelerationQuery(Memory<KeplerObject> keplerMasses)
        {
            _keplerMasses = keplerMasses;
        }

        public Metre3 Acceleration(Metre3 position, double time)
        {
            // Allocate some temporary space to store the position and mass of every body
            Span<(Metre3, double)> masses = stackalloc (Metre3, double)[_keplerMasses.Length];
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
            var keplerMasses = _keplerMasses.Span;

            for (var i = 0; i < keplerMasses.Length; i++)
            {
                var ko = keplerMasses[i];

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