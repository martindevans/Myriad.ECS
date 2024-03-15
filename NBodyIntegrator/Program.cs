using System.Numerics;
using Humanizer;
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
var earth = earthBuffered.Resolve(resolver1);

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
const int count = 2;
for (var i = 0; i < count; i++)
{
    var altitude = ((float)i / count) * 35_786_000;
    var speed = Math.Sqrt((G * EARTH_MASS) / (EARTH_RADIUS + altitude));

    var position = new double3(altitude + EARTH_RADIUS, 0, 0);
    var velocity = new double3(0, 0, -speed);

    CreateNBody(
        setup,
        new Metre3(position),
        new Metre3(velocity),
        NBodyPrecision.Medium,
        null
    );
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

// lunar orbiter
var bnb = CreateNBody(
    setup,
    new Metre3(384748000, 0, 0) + new Metre3(5000, 5000, 5000),
    new Metre3(0, 0, -1000),
    NBodyPrecision.Medium,
    null
);

// slightly randomised lunar orbiters
var rng = new Random();
for (var i = 0; i < 22; i++)
{
    CreateNBody(
        setup,
        new Metre3(15422465, -13228745, 373592928) + new Metre3(1737000, 0, 0),
        new Metre3(0, 2000, 0),
        NBodyPrecision.Medium,
        rng
    );
}

using var resolver = setup.Playback();
var nb = bnb.Resolve(resolver);

var systems = new SystemGroup<GameTime>(
    "main",
    new SystemGroup<GameTime>(
        "nbody",
        new RailTrimmer(world),
        new RailIntegrator(world)
    ),
    new SystemGroup<GameTime>(
        "live-state",
        new KeplerWorldPosition(world),
        new SetWorldPositionFromRail(world),
        new EngineBurnUpdater(world)
    )
);
systems.Init();

// Advance sim
const long ticks = 10_000_000;
var tickMin = TimeSpan.MaxValue;
var tickTotal = TimeSpan.Zero;
var tickMax = TimeSpan.MinValue;
var gt = new GameTime(1 / 10f);
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
            gt.Tick();

            if (systems.ExecutionTime < tickMin)
                tickMin = systems.ExecutionTime;
            tickTotal += systems.ExecutionTime;
            if (systems.ExecutionTime > tickMax)
                tickMax = systems.ExecutionTime;

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

using var binWriter = new BinaryWriter(File.Create("data_dump.bin"));
WriteBinary(binWriter, world);
binWriter.Flush();

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
    var lastTime = r.Item.LastState().Time;
    maxt = Math.Max(maxt, lastTime);
    mint = Math.Min(mint, lastTime);
    counter++;
}

Console.WriteLine();
Console.WriteLine("# Orbits");
Console.WriteLine($" - Total Bodies: {counter}");
Console.WriteLine($" - Longest:      {maxt.Seconds().Humanize(3)}");
Console.WriteLine($" - Shortest:     {mint.Seconds().Humanize(3)}");

return;

static CommandBuffer.BufferedEntity CreateNBody(CommandBuffer buffer, Metre3 position, Metre3 velocity, NBodyPrecision precision, Random? rng)
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

static void WriteBinary(BinaryWriter writer, World world)
{
    var packetCount = 0;

    // Write out entire rail
    foreach (var (e, r) in world.Query<PagedRail>())
    {
        foreach (var page in r.Item.Pages)
        {
            packetCount++;

            var positions = page.GetSpanPositions();
            var times = page.GetSpanTimes();

            // packet header:
            writer.Write((ushort)0); // protocol version
            writer.Write((ushort)0); // packet type

            // Entity ID
            writer.Write(e.ID);
            writer.Write(e.Version);

            // Write out a "keyframe" at the start
            writer.Write(checked((ushort)positions.Length));
            writer.Write(positions[0].Value.X);
            writer.Write(positions[0].Value.Y);
            writer.Write(positions[0].Value.Z);
            writer.Write(times[0]);

            // Keep track of the sum from the keyframe to the latest frame
            var estimatePos = positions[0].Value;
            var estimateTime = times[0];

            // Write out all the individual datapoints as a delta from the last predicted point
            for (var i = 1; i < positions.Length; i++)
            {
                var pos = positions[i].Value;
                var time = times[i];

                // Calculate delta from last estimated frame
                var deltaPos = (Vector3)(pos - estimatePos);
                var deltaTime = (float)(time - estimateTime);

                // Write single precision position
                writer.Write(deltaPos.X);
                writer.Write(deltaPos.Y);
                writer.Write(deltaPos.Z);

                // Write single precision delta time
                writer.Write(deltaTime);

                // Update cumulative estimate
                estimatePos += (double3)deltaPos;
                estimateTime += (float)deltaTime;
            }
        }
    }

    AnsiConsole.WriteLine($"Written {packetCount} packets");
    AnsiConsole.WriteLine();
}