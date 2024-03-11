using System.Numerics;
using Humanizer;
using Myriad.ECS.Command;
using Myriad.ECS.Systems;
using Myriad.ECS.Worlds;
using NBodyIntegrator;
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
for (var i = 0; i < 10; i++)
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

var systems = new SystemGroup(
    "main",
    new SystemGroup(
        "kepler",
        new KeplerWorldPosition(world)
    ),
    new SystemGroup(
        "nbody",
        new RailTrimmer(world),
        new RailIntegrator(world)
    )
);
systems.Init();

// Advance sim
const long ticks = 100_000;
var tickMin = TimeSpan.MaxValue;
var tickTotal = TimeSpan.Zero;
var tickMax = TimeSpan.MinValue;
var gt = new GameTime(1 / 60f);
var maxMem = 0L;

AnsiConsole
   .Progress()
   .Start(ctx =>
    {
        var task = ctx.AddTask("Running");
        task.MaxValue = ticks;

        using var csvWriter = File.CreateText("data_dump.csv");
        csvWriter.WriteLine("entity,type,timestamp,posx,posy,posz");

        using var binWriter = new BinaryWriter(File.Create("data_dump.bin"));

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

            // Partway through sim invalidate one of the rails
            if (i == 5000)
            {
                world.GetComponentRef<NBody>(nb).Invalidate(
                    1814400, // 3 weeks
                    world.GetComponentRef<PagedRail>(nb)
                );
            }

            task.Increment(1);
        }

        WriteCsv(csvWriter, world);
        WriteBinary(binWriter, world);
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
    var lastTime = r.Item.LastState().time;
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
    var rail = new PagedRail(position, velocity, 0);
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

        // 2.64MN is a raptor 3 engine
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

static void WriteCsv(TextWriter writer, World world)
{
    //double maxt = 0;

    //// Write out entire rail
    //foreach (var (e, r) in world.Query<PagedRail>())
    //{
    //    if (r.Item.ItemCount == 0)
    //        continue;

    //    maxt = Math.Max(maxt, t.Item.Last().Value);

    //    var positionSpans = p.Item.GetEnumerator();
    //    var timeSpans = t.Item.GetEnumerator();

    //    while (true)
    //    {
    //        if (!positionSpans.MoveNext() || !timeSpans.MoveNext())
    //            break;

    //        var positions = positionSpans.Current;
    //        var times = timeSpans.Current;
    //        if (positions.Length != times.Length)
    //            throw new InvalidOperationException("page length mismatch");

    //        for (var i = 0; i < positions.Length; i++)
    //        {
    //            var pos = positions[i].Value;
    //            writer.WriteLine($"{e.ID},\"n\",{times[i].Value:F2},{pos.Value.X:F3},{pos.Value.Y:F3},{pos.Value.Z:F3}");
    //            maxt = Math.Max(maxt, times[i].Value);
    //        }
    //    }
    //}

    //// Write out kepler positions at hourly intervals
    //foreach (var (e, k, _) in world.Query<KeplerOrbit, WorldPosition>())
    //{
    //    for (var i = 0; i < maxt; i += 3600)
    //    {
    //        var pos = k.Item.PositionAtTime(i);
    //        writer.WriteLine($"{e.ID},\"k\",{i:F2},{pos.Value.X:F3},{pos.Value.Y:F3},{pos.Value.Z:F3}");
    //    }
    //}
}

static void WriteBinary(BinaryWriter writer, World world)
{
    //// Write out entire rail
    //foreach (var (_, p, t) in world.Query<PagedRail<NBody.Position>, PagedRail<NBody.Timestamp>>())
    //{
    //    if (p.Item.ItemCount == 0)
    //        continue;

    //    var positionSpans = p.Item.GetEnumerator();
    //    var timeSpans = t.Item.GetEnumerator();

    //    while (true)
    //    {
    //        if (!positionSpans.MoveNext() || !timeSpans.MoveNext())
    //            break;

    //        var positions = positionSpans.Current;
    //        var times = timeSpans.Current;
    //        if (positions.Length != times.Length)
    //            throw new InvalidOperationException("page length mismatch");

    //        // Write out a "keyframe" at the start
    //        writer.Write(positions.Length);
    //        writer.Write(positions[0].Value.Value.X);
    //        writer.Write(positions[0].Value.Value.Y);
    //        writer.Write(positions[0].Value.Value.Z);
    //        writer.Write(times[0].Value);

    //        // Keep track of the sum from the keyframe to the latest frame
    //        var estimatePos = positions[0].Value.Value;
    //        var estimateTime = times[0].Value;

    //        for (var i = 1; i < positions.Length; i++)
    //        {
    //            var pos = positions[i].Value.Value;
    //            var time = times[i].Value;

    //            // Calculate delta from last estimated frame
    //            var deltaPos = (Vector3)(pos - estimatePos);
    //            var deltaTime = (Half)(time - estimateTime);

    //            // Write single precision position
    //            writer.Write(deltaPos.X);
    //            writer.Write(deltaPos.Y);
    //            writer.Write(deltaPos.Z);

    //            // Write half precision time
    //            writer.Write(deltaTime);

    //            // Update cumulative estimate
    //            estimatePos += (double3)deltaPos;
    //            estimateTime += (float)deltaTime;
    //        }
    //    }
    //}
}