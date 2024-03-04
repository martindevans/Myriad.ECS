using Myriad.ECS;
using Myriad.ECS.Queries;
using Myriad.ECS.Systems;
using Myriad.ECS.Worlds;

namespace NBodyIntegrator.Orbits.NBodies;

public sealed class RailTrimmer(World world)
    : ISystem
{
    public bool Enabled { get; set; } = true;

    private readonly QueryDescription _trimQuery = new QueryBuilder()
        .Include<NBody>()
        .Include<EntityArray<NBody.Timestamp>>()
        .Build(world);

    public void Init()
    {
    }

    public void Update(GameTime time)
    {
        world.Execute<TrimRails, NBody, EntityArray<NBody.Timestamp>>(new TrimRails(time.Time), _trimQuery);
    }

    private readonly struct TrimRails(double CurrentTime)
        : IQuery2<NBody, EntityArray<NBody.Timestamp>>
    {
        public void Execute(
            Entity entity,
            ref NBody nbody,
            ref EntityArray<NBody.Timestamp> rawTimes
        )
        {
            // Remove useless data from start of rail
            TrimRail(ref nbody.RailPoints, ref rawTimes);
        }

        /// <summary>
        /// Trim down rail so that it has just one point before "CurrentTime"
        /// </summary>
        private void TrimRail(ref CircularBufferIndexer circular, ref EntityArray<NBody.Timestamp> time)
        {
            while (circular.Count > 2)
            {
                // Get the first two timestamps
                var ai = circular.IndexAt(0);
                if (!ai.HasValue)
                    return;
                var bi = circular.IndexAt(1);
                if (!bi.HasValue)
                    return;

                // we know neither are null now
                var a = time.Array[ai.Value].Value;
                var b = time.Array[bi.Value].Value;

                // If they are _both_ before the current time remove the first one
                if (a < CurrentTime && b < CurrentTime)
                {
                    if (!circular.TryRemove().HasValue)
                        return;
                }
                else
                    return;
            }
        }
    }

    public void Dispose()
    {
    }
}