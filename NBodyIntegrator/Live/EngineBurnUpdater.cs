using Myriad.ECS;
using Myriad.ECS.Queries;
using Myriad.ECS.Systems;
using Myriad.ECS.Worlds;
using NBodyIntegrator.Orbits.NBodies;

namespace NBodyIntegrator.Live;

public class EngineBurnUpdater(World world)
    : BaseSystem<GameTime>, ISystemDeclare<GameTime>
{
    public override void Update(GameTime time)
    {
        world.Execute<UpdateMassFromBurn, EngineBurnSchedule, Mass>(
            new UpdateMassFromBurn(time.Time, time.DeltaTime)
        );
    }

    public void Declare(ref SystemDeclaration declaration)
    {
        declaration.Write<EngineBurnSchedule>();
        declaration.Write<Mass>();
    }

    private readonly struct UpdateMassFromBurn : IQuery<EngineBurnSchedule, Mass>
    {
        private readonly double _time;
        private readonly double _deltaTime;

        public UpdateMassFromBurn(double time, double deltaTime)
        {
            _time = time;
            _deltaTime = deltaTime;
        }

        public void Execute(Entity e, ref EngineBurnSchedule schedule, ref Mass mass)
        {
            // Remove all burns which are entirely before now
            while (schedule.Burns.Count > 0 && schedule.Burns[0].End.Value < _time)
                schedule.Burns.RemoveAt(0);

            // Early out of there are not burns
            if (schedule.Burns.Count == 0)
                return;

            // Apply 1 timestep of the next engine burn if necessary
            var burn0 = schedule.Burns[0];
            if (burn0.Start < _time && burn0.End >= _time)
            {
                var prevt = _time - _deltaTime;
                var startBefore = prevt < burn0.Start;
                var endAfter = _time > burn0.End;

                // It's possible this timestep did not start/end _exactly_ when the burn did. Compensate for
                // this by checking the previous timestep and offsetting it.
                var dt = (startBefore, endAfter) switch
                {
                    (true, true) => burn0.Duration.Value,
                    (true, false) => _time - burn0.Start.Value,
                    (false, true) => burn0.End.Value - prevt,
                    _ => _deltaTime
                };

                mass.Value -= burn0.MassPerSecond * dt;
            }
        }
    }
}