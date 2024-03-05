using Myriad.ECS.Command;
using Myriad.ECS.Systems;
using Myriad.ECS.Worlds;
using NBodyIntegrator;
using NBodyIntegrator.Orbits.Kepler;
using NBodyIntegrator.Orbits.NBodies;
using NBodyIntegrator.Units;
using Spectre.Console;
using Unity.Mathematics;

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
        NBodyPrecision.Medium
    );
}

//// Elliptical
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
//CreateNBody(
//    setup,
//    new Metre3(384748000, 0, 0) + new Metre3(5000, 5000, 5000),
//    new Metre3(0, 0, -1000),
//    NBodyPrecision.Medium
//);
CreateNBody(
    setup,
    new Metre3(140218493, -5186504, -346837760) + new Metre3(0, 1737000, 0),
    new Metre3(2300, 0, 0),
    NBodyPrecision.Medium
);

using var resolver = setup.Playback();

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

        using var writer = File.CreateText("data_dump.csv");
        writer.WriteLine("entity,type,timestamp,posx,posy,posz");

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

            task.Increment(1);

            if (i % 4 == 1)
                WriteCsv(writer, gt.Time);
        }
    });

// General stats
Console.WriteLine($"# {ticks:N0} Ticks");
Console.WriteLine($" - Total: {tickTotal.TotalMicroseconds}us");
Console.WriteLine($" - Min:   {tickMin.TotalMicroseconds}us");
Console.WriteLine($" - Avg:   {tickTotal.TotalMicroseconds / ticks}us");
Console.WriteLine($" - Max:   {tickMax.TotalMicroseconds}us");
Console.WriteLine();
Console.WriteLine("# Memory");
Console.WriteLine($" - Max: {maxMem:N0}");

return;

static CommandBuffer.BufferedEntity CreateNBody(CommandBuffer buffer, Metre3 position, Metre3 velocity, NBodyPrecision precision)
{
    // Initialize a circular buffer to hold orbit data
    const int railSize = 3600;
    var bufferAccess = new CircularBufferIndexer(railSize);
    var idx = bufferAccess.TryAdd()!.Value;

    // Create entity
    var entity = buffer
        .Create()
        .Set(new NBody { DeltaTime = 1, MaximumTimeLength = 2419200, RailTimestep = 1, IntegratorPrecision = precision })
        .Set(new WorldPosition(position));

    // Create buffers to hold orbit data
    var pos = new PagedRail<NBody.Position>();
    pos.Add(new NBody.Position(position.Value.x, position.Value.y, position.Value.z));
    entity.Set(pos);

    var vel = new PagedRail<NBody.Velocity>();
    vel.Add(new NBody.Velocity(velocity.Value.x, velocity.Value.y, velocity.Value.z));
    entity.Set(vel);

    var time = new PagedRail<NBody.Timestamp>();
    time.Add(new NBody.Timestamp(0));
    entity.Set(time);

    return entity;
}

void WriteCsv(TextWriter writer, double gameTime)
{
    double maxt = 0;

    foreach (var (e, n, p, t) in world.Query<NBody, PagedRail<NBody.Position>, PagedRail<NBody.Timestamp>>())
    {
        if (p.Item.Count == 0)
            continue;

        var pos = p.Item.Last().Value;
        var time = t.Item.Last().Value;
        maxt = Math.Max(maxt, time);

        writer.WriteLine($"{e.ID},\"n\",{time:F2},{pos.Value.x:F3},{pos.Value.y:F3},{pos.Value.z:F3}");
    }

    foreach (var (e, k, w) in world.Query<KeplerOrbit, WorldPosition>())
    {
        var pos = k.Item.PositionAtTime(maxt);
        writer.WriteLine($"{e.ID},\"k\",{maxt:F2},{pos.Value.x:F3},{pos.Value.y:F3},{pos.Value.z:F3}");
    }
}