using System.Numerics;
using Myriad.ECS.Command;
using Myriad.ECS.Systems;
using Myriad.ECS.Worlds;
using NBodyIntegrator;
using NBodyIntegrator.Integrator.NBodies;
using NBodyIntegrator.Units;
using Spectre.Console;
using Unity.Mathematics;

const double EARTH_RADIUS = 6_371_000; // m
const double EARTH_MASS = 5.972E24; // kg
const double G = 6.6743E-11;

var world = new WorldBuilder().Build();

var setup = new CommandBuffer(world);

// Create Earth at the origin
setup.Create()
     .Set(new FixedBody())
     .Set(new WorldPosition(new Metre3(0, 0, 0)))
     .Set(new GravityMass(EARTH_MASS));

// Create satellites at random altitudes, with the correct velocity for a circular orbit at that altitude
var rng = new Random(214);
for (var i = 0; i < 1024; i++)
{
    var altitude = rng.NextDouble() * 35_786_000;
    var speed = Math.Sqrt((G * EARTH_MASS) / (EARTH_RADIUS + altitude));

    var outward = Vector3.Normalize(new Vector3(rng.NextSingle(), rng.NextSingle(), rng.NextSingle()) * 2 - Vector3.One);
    var position = new double3(outward) * (altitude + EARTH_RADIUS);
    var velocity = new double3(outward.Perpendicular()) * speed;

    // Initialize a circular buffer to hold orbit data
    const int size = 86_400;
    var bufferAccess = new DynamicCircularBuffer();
    var idx = bufferAccess.TryAdd(size)!.Value;

    // Create entity
    var entity = setup
        .Create()
        .Set(new NBody { DeltaTime = 1, RailPoints = bufferAccess, IntegratorPrecision = NBodyPrecision.Medium })
        .Set(new WorldPosition(new Metre3(position)));

    // Create buffers to hold orbit data
    var pos = new EntityArray<NBody.Position>(new NBody.Position[size]);
    pos.Array[idx] = new NBody.Position(position.x, position.y, position.z);
    entity.Set(pos);

    var vel = new EntityArray<NBody.Velocity>(new NBody.Velocity[size]);
    vel.Array[idx] = new NBody.Velocity(velocity.x, velocity.y, velocity.z);
    entity.Set(vel);

    var tim = new EntityArray<NBody.Timestamp>(new NBody.Timestamp[size]);
    tim.Array[idx] = new NBody.Timestamp(0);
    entity.Set(tim);
}

using var resolver = setup.Playback();

var systems = new SystemGroup(
    "main",
    new RailTrimmer(world),
    new RailIntegrator(world)
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
        }
   });

Console.WriteLine($"# {ticks:N0} Ticks");
Console.WriteLine($" - Total: {tickTotal.TotalMicroseconds}us");
Console.WriteLine($" - Min:   {tickMin.TotalMicroseconds}us");
Console.WriteLine($" - Avg:   {tickTotal.TotalMicroseconds / ticks}us");
Console.WriteLine($" - Max:   {tickMax.TotalMicroseconds}us");
Console.WriteLine();
Console.WriteLine("# Memory");
Console.WriteLine($" - Max: {maxMem:N0}");