# Myriad.ECS

[![Nuget](https://img.shields.io/nuget/v/Myriad.ECS?style=for-the-badge)](https://www.nuget.org/packages/Myriad.ECS/)

Myriad.ECS is a high performance Entity Component System (ECS) for C#.

## Benchmarks

[C# ECS Benchmarks](https://github.com/Doraku/Ecs.CSharp.Benchmark).

## Unity

Myriad.ECS supports netstandard2.0, and is compatible with Unity. There is a Unity integration package (providing editor integration) [here](https://github.com/martindevans/Myriad.ECS.Unity).

## Guide

Everything in Myriad happens in a `World`, created with a `WorldBuilder`.

### Components

Entities are just IDs, associated with a set of components. A component can be any type (managed or unmanaged) that implements `IComponent`.

```csharp
public record struct Position(Vector2 Value) : IComponent;
public record struct Velocity(Vector2 Value) : IComponent;
```

`IComponent` is simply a tag that ensures you cannot accidentally attach something to an entity that you didn't mean to. For example adding a `Vector2` to an `Entity` directly, instead of a `Position` or `Velocity` component.

### CommandBuffer

The only way to make structural changes to the world (creating or destroying entities, adding or removing components) is through a `CommandBuffer`. A `CommandBuffer` allows you to executes multiple commands, which are added to the buffer. The world is only modified when the buffered is executed.

```csharp
var buffer = new CommandBuffer(world);

// Create an entity. This returns a "buffered entity" object that can be used to resolve the real Entity when it is eventually created
var bufferedEntity = setup.Create()
     .Set(new Position(new Vector3(1, 2, 3)))
     .Set(new Velocity(new Vector3(0, 1, 0)))
     .Set(new Mass(1));

// Execute the buffer, receive a "resolver"
using var resolver = buffer.Playback();

// Resolve the buffered entity into a real Entity
var entity = bufferedEntity.Resolve(resolver);
```

### Querying

Myriad.ECS has several different querying systems. These have different trade-offs in usability and performance.

#### QueryDescription/QueryBuilder

Queries can be filtered based on the components an Entity has. This is done with a `QueryDescription`, created with a `QueryBuilder`. Filtering like this is very fast, so as much as possible should be encoded into query descriptions. There are 4 types filtering a query can do:
 - Include: Entities **must** include this component.
 - Exclude: Entities **must not** include this component.
 - At Least One: Entities must contain **one or more** of the listed components.
 - Exactly One: Entities must contain **exactly one** of the listed components.

#### IQuery

A basic `IQuery` is defined in a struct, and runs a bit of code for every entity matched by the `QueryDescription`. The generic parameters define are the type of query, followed by the components the query accesses.

```csharp
// Execute the query
_world.Execute<Integrate, Position, Velocity>(new Integrate(deltaTime), query);

// Define what the query does
private struct IntegrateChunk(float deltaTime)
    : IQuery<Position, Velocity>
{
    public readonly void Execute(Entity e, ref Position pos, ref Velocity vel)
    {
        pos.Value += vel.Value * deltaTime;
    }
}
```

#### IChunkQuery

A chunk query is fundamentally the same as a basic query, but it does not run the inner loop over entity chunks. Instead your code is given spans, where the same index in all spans is the same entity.

```csharp
// Execute the query
_world.ExecuteChunk<IntegrateChunk, Position, Velocity>(new IntegrateChunk(deltaTime), query);

// Define what the query does
private struct IntegrateChunk(float deltaTime)
    : IChunkQuery<Position, Velocity>
{
    public readonly void Execute(ChunkHandle chunk, ReadOnlySpan<Entity> e, Span<Position> pos, Span<Velocity> vel)
    {
        for (var i = 0; i < pos.Length; i++)
            pos[i].Value += vel[i].Value * deltaTime;
    }
}
```

#### Query Delegate

A delegate query is simpler way to express an `IQuery`. No struct is required, and the generic parameters can be inferred.

```csharp
// Simple delegate query
_world.Query(static (ref Position pos, ref Velocity vel) => {
    pos.Value += vel.Value;
});

// Delegate query with state (first arg to query is first arg to delegate)
_world.Query(deltaTime, static (float deltaTime, ref Position pos, ref Velocity vel) => {
    pos.Value += vel.Value * deltaTime;
});
```

#### Query Enumerable

An enumerable query is another way to express an `IQuery`. A tuple containing the entity and a reference to the requested components is provided.

```csharp
// Method signature
public QueryResultEnumerable2<T0, T1> Query<T0, T1, ...etc>(QueryDescription query);

// Method call
foreach (var (e, p, v) in world.Query<Position, Velocity>())
    p.Ref.Value += v.Ref.Value;
```

### Magic Components

Some components in Myriad.ECS provide special behaviour.

### IPhantomComponent

A phantom component is created by implementing `IPhantomComponent` instead of `IComponents`. When an entity with a phantom component is destroyed the entity is not destroyed - instead it becomes a "phantom". Phantom entities are automatically **excluded** from queries and must be explicitly included with `.Include<Phantom>` on the query description.

A phantom entity can be destroyed in two ways:
 - Delete it again.
 - Remove all phantom components.

Phantom components are useful for tracking per-entity state. For example if there is some event that needs to run when an entity is destroyed you can attach a component when the entity is created (`DoTheThing : IPhantomComponent`) and then query for `Include<DoTheThing, Phantom>()`. Once done you can should the `DoTheThing` component. Once all of the phantom components have been removed the entity will disappear.
### IPhantomNotifierComponent

An `IPhantomNotifierComponent` receives a callback when the entity it belongs to first becomes a phantom.

### IDisposableComponent

A disposable component is created by implementing `IDisposableComponent`. When the component is destroyed the `Dispose` method will be called. This is a safe way to move ownership of disposable resources into entities.

### IEntityRelationComponent

A relational component stores a link from one entity to another. This can be initialised directly in a CommandBuffer, creating links between entities that do not yet exist.

```csharp
var cmd = new CommandBuffer()

var e1 = cmd.Create();
var e2 = cmd.Create()
    .Set(new ExampleRelationComponent(), e1);
```

`e1` and `e2` have not yet been created - when the CommandBuffer is executed the `ExampleRelationComponent` will automatically be set to the ID of `e1`.

### Systems

Systems are a completely optional part of `Myriad.ECS`. The library can be used as an in memory database without using any systems.

#### `ISystem`

All systems must implement `ISystem<TData>`, with an `Update(TData)` method. The `TData` parameter specifies what type will be passed into the `Update` method, for example a `GameTime` object.

#### `ISystemInit`

Adds an `Init` method that is run exactly once, before any other calls.

#### `ISystemBefore`

Adds a `BeforeUpdate(TData)` which is called every tick, just before `Update`.

#### `ISystemAfter`

Adds an `AfterUpdate(TData)` which is called every tick, just after `Update`.

#### `SystemGroup`

A SystemGroup defines a named set of systems to run in order.

```csharp
var cmdPhysics = new CommandBufferSystem(world);
var cmdIo = new CommandBufferSystem(world);

var systems = new SystemGroup<GameTime>(
    "main",
    new SystemGroup<GameTime>(
        "physics",
        new Integrator(world),
        new SystemGroup<GameTime>(
            "collisions",
            new BroadPhaseCollisions(world, cmdPhysics),
            new NarrowPhaseCollisions(world, cmdPhysics),
        ),
        cmdPhysics
    ),
    new SystemGroup<GameTime>(
        "io",
        new ReadPlayerInputKeyboard(world, cmdIo),
        new ReadPlayerInputMouse(world, cmdIo),
        new ReadPlayerInputController(world, cmdIo),
        new ApplyHaptics(world),
        cmdIo
    )
);
systems.Init();
```

#### `CommandBufferSystem`

A `CommandBufferSystem` exposes a `CommandBuffer` and executes the buffer when the system runs. In the above example `CommandBufferSystem`s are created at the start and are passed into various systems, then they are scheduled to run at the end of their respective groups.

This allows multiple systems to share one single `CommandBuffer`, which is executed just once at the end of a group of systems instead of every system making ad-hoc changes.
