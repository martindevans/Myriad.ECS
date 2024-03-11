using Myriad.ECS.Systems;
using Myriad.ECS.Worlds;

namespace NBodyIntegrator.Orbits.NBodies;

public sealed class RailTrimmer(World world)
    : BaseSystem
{
    private const int MAX_ITERS = 64;

    public override void Update(GameTime time)
    {
        var now = time.Time;

        foreach (var (_, rail) in world.Query<PagedRail>())
            for (var i = 0; i < MAX_ITERS; i++)
                if (!rail.Item.TryTrimStart(now))
                    break;
    }
}