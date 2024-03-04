using Myriad.ECS;
using Myriad.ECS.Queries;
using Myriad.ECS.Systems;
using Myriad.ECS.Worlds;

namespace NBodyIntegrator.Orbits.NBodies;

public sealed class RailTrimmer(World world)
    : ISystem
{
    private const int MAX_ITERS = 64;

    public bool Enabled { get; set; } = true;

    public void Init()
    {
    }

    public void Update(GameTime time)
    {
        world.Execute<TrimRails, NBody, PagedRail<NBody.Position>, PagedRail<NBody.Velocity>, PagedRail<NBody.Timestamp>>(new TrimRails(time.Time));
    }

    private readonly struct TrimRails(double CurrentTime)
        : IQuery4<NBody, PagedRail<NBody.Position>, PagedRail<NBody.Velocity>, PagedRail<NBody.Timestamp>>
    {
        public void Execute(
            Entity entity,
            ref NBody nbody,
            ref PagedRail<NBody.Position> positions,
            ref PagedRail<NBody.Velocity> velocities,
            ref PagedRail<NBody.Timestamp> times
        )
        {
            if (positions.Count != velocities.Count)
                throw new InvalidOperationException("Position/Velocity count mismatch");
            if (positions.Count != times.Count)
                throw new InvalidOperationException("Position/Velocity count mismatch");

            // Keep removing frames while:
            // - Iteration limit
            // - More than 2
            // - First two are both before now
            for (var i = 0; i < MAX_ITERS && times.Count > 2; i++)
            {
                var a = times.First();
                var b = times.Second();

                if (a.Value >= CurrentTime || b.Value >= CurrentTime)
                    return;

                positions.RemoveFirst();
                velocities.RemoveFirst();
                times.RemoveFirst();
            }
        }
    }

    public void Dispose()
    {
    }
}