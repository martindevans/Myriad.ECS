using Myriad.ECS.Allocations;
using Myriad.ECS.Systems;
using Myriad.ECS.Worlds;

namespace NBodyIntegrator.Orbits.NBodies;

public sealed class RailTrimmer(World world)
    : BaseSystem<GameTime>, ISystemDeclare<GameTime>
{
    private const int MAX_ITERS = 64;

    public void Declare(ref SystemDeclaration declaration)
    {
        declaration.Write<PagedRail>();
    }

    public override void Update(GameTime time)
    {
        foreach (var (_, rail) in world.Query<PagedRail>())
            for (var i = 0; i < MAX_ITERS; i++)
                if (!TryTrimStart(ref rail.Ref, time.Time))
                    break;
    }

    /// <summary>
    /// Remove the entire first page if it can be removed.
    /// </summary>
    /// <param name="rail"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    private static bool TryTrimStart(ref PagedRail rail, double time)
    {
        if (rail.Pages.Count < 2)
            return false;

        // Get the first and second page
        var p0span = rail.Pages[0].GetSpanTimes();
        var p1span = rail.Pages[1].GetSpanTimes();

        // If the end of the first page is still in the future we can't remove the first page
        if (p0span[^1] > time)
            return false;

        // We want at least one point of the next page to be in the past too
        if (p1span[0] > time)
            return false;

        // Remove and recycle first page
        var p0 = rail.Pages[0];
        rail.Pages.RemoveAt(0);
        Pool.Return(p0);
        return true;
    }
}