using System.Numerics;
using Humanizer;
using Myriad.ECS;
using Myriad.ECS.Command;
using Myriad.ECS.Systems;
using Myriad.ECS.Worlds;
using NBodyIntegrator;
using NBodyIntegrator.Live;
using NBodyIntegrator.Mathematics;
using NBodyIntegrator.Orbits.Kepler;
using NBodyIntegrator.Orbits.NBodies;
using NBodyIntegrator.Units;
using Spectre.Console;

const double EARTH_RADIUS = 6_371_000; // m
const double EARTH_MASS = 5.972E24; // kg
const double G = 6.6743E-11;

var world = new WorldBuilder().Build();
var setup = new CommandBuffer(world);

// Create Earth at the origin
var earthBuffered = setup.Create()
     .Set(new FixedBody())
     .Set(new WorldPosition(new Metre3(0, 0, 0)))
     .Set(new GravityMass(EARTH_MASS));
using var resolver1 = setup.Playback();
var earth = earthBuffered.Resolve();

// Create moon on kepler rails
setup.Create()
     .Set(new GravityMass(7.34767E22))
     .Set(new WorldPosition())
     .Set(new KeplerOrbit
      (
          parent: earth,
          elements: new KeplerElements(
              eccentricity: 4.151912181761209E-02,
              sma: (Metre)new KiloMetre(380626.48),
              inclination: new Degrees(5.107151746896368E+00),
              longitudeAscending: new Degrees(1.543164577863016E+02),
              argPeriapsis: new Degrees(2.713783924348262E+02),
              period: new Seconds(2.365771210134184E+06),
              meanAnomalyEpoch: new Degrees(2.502170811806626E+02)
          )
      ));


// Create satellites at random altitudes, with the correct velocity for a circular orbit at that altitude
const int count = 1000;
CommandBuffer.BufferedEntity baselineBuffered = default;
for (var i = 0; i < count; i++)
{
    var precision = 0.001 + (i / (double)count) * 10;

    var altitude = 0;//((float)i / count) * 35_786_000;
    var speed = Math.Sqrt((G * EARTH_MASS) / (EARTH_RADIUS + altitude));

    var position = new double3(altitude + EARTH_RADIUS, 0, 0);
    var velocity = new double3(0, 0, -speed);

    var b = CreateNBody(
        setup,
        new Metre3(position),
        new Metre3(velocity),
        precision,
        null
    );

    if (i == 0)
        baselineBuffered = b;
}

// Elliptical
//CreateNBody(
//    setup,
//    (Metre3)new MegaMetre3(14, -8, 370.3164),
//    new Metre3(0, -77, 0),
//    NBodyPrecision.Medium
//);
//CreateNBody(
//    setup,
//    (Metre3)new MegaMetre3(370.3164, -8, 14),
//    new Metre3(0, -77, 0),
//    NBodyPrecision.Medium
//);

//// lunar orbiter
//var bnb = CreateNBody(
//    setup,
//    new Metre3(384748000, 0, 0) + new Metre3(5000, 5000, 5000),
//    new Metre3(0, 0, -1000),
//    NBodyPrecision.Medium,
//    null
//);

//// slightly randomised lunar orbiters
//var rng = new Random();
//for (var i = 0; i < 22; i++)
//{
//    CreateNBody(
//        setup,
//        new Metre3(15422465, -13228745, 373592928) + new Metre3(1737000, 0, 0),
//        new Metre3(0, 2000, 0),
//        NBodyPrecision.Medium,
//        rng
//    );
//}

using var resolver = setup.Playback();
var baseline = baselineBuffered.Resolve();
//var nb = bnb.Resolve(resolver);

var systems = new SystemGroup<GameTime>(
    "main",
    new SystemGroup<GameTime>(
        "nbody",
        new RailTrimmer(world),
        new RailIntegrator(world)
    ),
    new PhasedParallelSystemGroup<GameTime>(
        "live-state",
        new KeplerWorldPosition(world),
        new SetWorldPositionFromRail(world),
        new EngineBurnUpdater(world)
    )
);
systems.Init();

// Advance sim
const long ticks = 100_000;
var tickMin = TimeSpan.MaxValue;
var tickTotal = TimeSpan.Zero;
var tickMax = TimeSpan.MinValue;
var gt = new GameTime();
var maxMem = 0L;

AnsiConsole
   .Progress()
   .Start(ctx =>
    {
        var task = ctx.AddTask("Running");
        task.MaxValue = ticks;

        for (var i = 0; i < ticks; i++)
        {
            systems.BeforeUpdate(gt);
            systems.Update(gt);
            systems.AfterUpdate(gt);
            gt.Tick(1 / 10f);

            if (systems.TotalExecutionTime < tickMin)
                tickMin = systems.TotalExecutionTime;
            tickTotal += systems.TotalExecutionTime;
            if (systems.TotalExecutionTime > tickMax)
                tickMax = systems.TotalExecutionTime;

            if (i % 1000 == 7)
                maxMem = Math.Max(maxMem, GC.GetTotalMemory(false));

            //// Partway through sim invalidate one of the rails
            //if (i == 5000)
            //{
            //    world.GetComponentRef<NBody>(nb).Invalidate(
            //        1814400, // 3 weeks
            //        world.GetComponentRef<PagedRail>(nb)
            //    );
            //}

            task.Increment(1);
        }
    });

// General stats
Console.WriteLine($"# {ticks:N0} Ticks");
Console.WriteLine($" - Total:   {tickTotal.TotalMilliseconds}ms");
Console.WriteLine($" - Min:     {tickMin.TotalMicroseconds}us");
Console.WriteLine($" - Avg:     {tickTotal.TotalMicroseconds / ticks}us");
Console.WriteLine($" - Max:     {tickMax.TotalMicroseconds}us");
Console.WriteLine();
Console.WriteLine("# Memory");
Console.WriteLine($" - Max Bytes: {maxMem.Bytes().Humanize()}");

// Orbit stats
var maxt = double.MinValue;
var mint = double.MaxValue;
var counter = 0;
foreach (var (_, r) in world.Query<PagedRail>())
{
    var lastTime = r.Ref.LastState().Time;
    maxt = Math.Max(maxt, lastTime);
    mint = Math.Min(mint, lastTime);
    counter++;
}

Console.WriteLine();
Console.WriteLine("# Orbits");
Console.WriteLine($" - Total Bodies: {counter}");
Console.WriteLine($" - Longest:      {maxt.Seconds().Humanize(3)}");
Console.WriteLine($" - Shortest:     {mint.Seconds().Humanize(3)}");

WriteEnergyChart(world, baseline);

return;

static void WriteEnergyChart(World world, Entity baseline)
{
    // Find the shortest rail
    var endTime = double.MaxValue;
    world.Query((ref PagedRail rail) =>
    {
        endTime = double.Min(endTime, rail.LastState().Time);
    });
    endTime -= 100;

    // Find energy of min epsilon (baseline)
    var (baselineEnergy, baselineSpeed, baselineAlt) = EnergySpeedAlt(ref baseline.GetComponentRef<PagedRail>(), endTime);

    using (var writer = new StreamWriter(File.Create("energy.csv")))
    {
        writer.WriteLine("epsilon,energy_delta,energy_delta_abs,speed_delta,alt_delta");

        foreach (var (_, n, r) in world.Query<NBody, PagedRail>())
        {
            var (e, s, a) = EnergySpeedAlt(ref r.Ref, endTime);

            e -= baselineEnergy;
            s -= baselineSpeed;
            a -= baselineAlt;

            writer.WriteLine($"{n.Ref.IntegratorPrecision}, {e}, {Math.Abs(e)}, {s}, {a}");
        }
    }
}

static (double, double, double) EnergySpeedAlt(ref PagedRail rail, double time)
{
    var (before, after) = rail.NearestStates(time)!.Value;

    var duration = after.Time - before.Time;
    if (duration <= double.Epsilon)
        throw new NotImplementedException();

    var t = (time - before.Time) / duration;

    var alt = Metre3.Lerp(before.Position, after.Position, t).Value.Length();
    var spd = Metre3.Lerp(before.Velocity, after.Velocity, t).Value.Length();

    var ke = 0.5 * 1 * spd * spd;
    var pe = 1 * G * alt;
    return (ke + pe, spd, alt);
}

static CommandBuffer.BufferedEntity CreateNBody(CommandBuffer buffer, Metre3 position, Metre3 velocity, double precision, Random? rng)
{
    // Create entity
    var entity = buffer
        .Create()
        .Set(new NBody { DeltaTime = 0, MaximumTimeLength = new Seconds(2419200), IntegratorPrecision = precision })
        .Set(new WorldPosition(position));

    // Create buffers to hold orbit data
    var rail = new PagedRail
    {
        Epoch = 1,
        Pages = []
    };

    var page = new OrbitRailPage();
    page.Init(1, 0);
    page.GetSpanPositions()[0] = position;
    page.GetSpanVelocities()[0] = velocity;
    page.GetSpanTimes()[0] = 0;
    rail.Pages.Add(page);

    entity.Set(rail);

    // Starship mass
    entity.Set(new Mass(1_300_000));

    var schedule = new List<EngineBurn>();
    entity.Set(new EngineBurnSchedule(schedule));

    if (rng != null)
    {
        // schedule burn 1 week into flight
        var start = 604800;

        // Burn for 2 minutes
        var end = start + 120;

        // 2.64MN is a raptor 3 engines
        var force = 2_640_000;

        // 650kg/s fuel consumption
        var fuelConsumption = 650;

        // Any random dir
        var dir = Vector3.Normalize(new Vector3(rng.NextSingle() * 2 - 1, rng.NextSingle() * 2 - 1, rng.NextSingle() * 2 - 1));

        // schedule it!
        schedule.Add(new EngineBurn(
            new Seconds(start),
            new Seconds(end),
            force,
            fuelConsumption,
            new double3(dir)
        ));
    }

    return entity;
}