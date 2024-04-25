using Myriad.ECS;
using Myriad.ECS.Queries;
using Myriad.ECS.Systems;
using Myriad.ECS.Worlds;
using NBodyIntegrator.Units;

namespace NBodyIntegrator.Orbits.Kepler;

/// <summary>
/// Set the `WorldPosition` based on the current game time and the `KeplerObject` data
/// </summary>
/// <param name="world"></param>
public sealed class KeplerWorldPosition(World world)
    : BaseSystem<GameTime>, ISystemInit<GameTime>
{
    private KeplerObject[] _keplerMasses = [];

    public void Init()
    {
        _keplerMasses = FindKeplerBodies(world);
    }

    public override void Update(GameTime time)
    {
        // calculate the position of every kepler body
        Span<Metre3> positions = stackalloc Metre3[_keplerMasses.Length];
        for (var i = 0; i < _keplerMasses.Length; i++)
        {
            var ko = _keplerMasses[i];

            if (ko.ParentIndex < 0)
            {
                positions[i] = ko.FixedPosition;
            }
            else
            {
                var localPos = ko.Kepler.PositionAtTime(time.Time);
                var wpos = positions[ko.ParentIndex] + localPos;
                positions[i] = wpos;
            }
        }

        // Write that data back to the ECS
        for (var i = 0; i < _keplerMasses.Length; i++)
            _keplerMasses[i].Self.GetComponentRef<WorldPosition>(world).Value = positions[i];
    }

    public static KeplerObject[] FindKeplerBodies(World world)
    {
        var keplerMasses = new List<KeplerObject>();

        // Find all fixed bodies and store their information
        var fixedQuery = new QueryBuilder()
                        .Include<FixedBody>().Include<WorldPosition>().Include<GravityMass>()
                        .Build(world);
        foreach (var (e, _, w, g) in world.Query<FixedBody, WorldPosition, GravityMass>(fixedQuery))
            keplerMasses.Add(new(e, -1, default, w.Ref.Value, g.Ref.Value));

        // Find all non-fixed bodies
        var dynamicQuery = new QueryBuilder()
                          .Include<GravityMass>().Include<KeplerOrbit>()
                          .Build(world);

        // Store a list of all "unmatched" items. That is dynamic kepler bodies which we have
        // not yet found the parent index for.
        var unmatched = new List<(Entity, GravityMass, KeplerOrbit)>();
        foreach (var (e, g, k) in world.Query<GravityMass, KeplerOrbit>(dynamicQuery))
            unmatched.Add((e, g.Ref, k.Ref));

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

        return [.. keplerMasses];
    }
}

public record struct KeplerObject(Entity Self, int ParentIndex, KeplerOrbit Kepler, Metre3 FixedPosition, double MassKg);