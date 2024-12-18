﻿using Myriad.ECS;
using Myriad.ECS.Queries;
using Myriad.ECS.Systems;
using Myriad.ECS.Worlds;
using NBodyIntegrator.Orbits.NBodies;
using NBodyIntegrator.Units;

namespace NBodyIntegrator.Live;

public class SetWorldPositionFromRail(World world)
    : ISystemDeclare<GameTime>
{
    public void Update(GameTime time)
    {
        world.Execute<UpdatePosition, PagedRail, WorldPosition>(
            new UpdatePosition(time.Time)
        );
    }

    public void Declare(ref SystemDeclaration declaration)
    {
        declaration.Read<PagedRail>();
        declaration.Write<WorldPosition>();
    }

    private readonly struct UpdatePosition(double time)
        : IChunkQuery<PagedRail, WorldPosition>, IQuery<PagedRail, WorldPosition>
    {
        public void Execute(ChunkHandle chunk, ReadOnlySpan<Entity> e, Span<PagedRail> r, Span<WorldPosition> w)
        {
            for (var i = 0; i < e.Length; i++)
                Execute(e[i], ref r[i], ref w[i]);
        }

        public void Execute(Entity e, ref PagedRail rail, ref WorldPosition position)
        {
            var maybeStates = rail.NearestStates(time);
            if (!maybeStates.HasValue)
                return;

            var (before, after) = maybeStates.Value;
            position.Value = CalculatePosition(before, after, time);
        }

        private static Metre3 CalculatePosition(RailPoint before, RailPoint after, double time)
        {
            var duration = after.Time - before.Time;
            if (duration <= double.Epsilon)
                return after.Position;

            var t = (time - before.Time) / duration;
            return Metre3.Lerp(before.Position, after.Position, t);
        }
    }
}