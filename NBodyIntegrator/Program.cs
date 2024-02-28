using System.Numerics;
using Myriad.ECS.Command;
using Myriad.ECS.Systems;
using Myriad.ECS.Worlds;
using NBodyIntegrator;
using NBodyIntegrator.Integrator.NBodies;
using NBodyIntegrator.Units;
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
for (var i = 0; i < 4096; i++)
{
    var altitude = rng.NextDouble() * 35_786_000;
    var speed = Math.Sqrt((G * EARTH_MASS) / (EARTH_RADIUS + altitude));

    var outward = Vector3.Normalize(new Vector3(rng.NextSingle(), rng.NextSingle(), rng.NextSingle()) * 2 - Vector3.One);
    var position = new double3(outward) * (altitude + EARTH_RADIUS);
    var velocity = new double3(outward.Perpendicular()) * speed;

    // Initialize a circular buffer to hold orbit data
    const int size = 131072;
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

var systems = new List<ISystem>()
{
    new RailTrimmer(world)
};

// Advance sim
var gt = new GameTime(1);
for (var i = 0; i < 1000000; i++)
{
    foreach (var system in systems)
        system.Update(gt);
    gt.Tick();
}