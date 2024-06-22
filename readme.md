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

### Phantom Components

Myriad supports "Phantom Components", these are defined by `IPhantomComponent` instead of `IComponent`. When an `Entity` with any phantom components is destroyed the entity is not actually destroyed, instead it becomes a "phantom". Phantom entities are automatically **excluded** from queries and must be explicitly included with `.Include<Phantom>`.

A phantom entity can be destroyed in two ways:
 - Delete it again.
 - Remove all phantom components.

Phantom components are useful for tracking per-entity state. For example if there is some event that needs to run when an entity is destroyed you can attach a component when the entity is created (`DoTheThing : IPhantomComponent`) and then query for `Include<DoTheThing, Phantom>()`. When you have done whatever is needed you should remove the `DoTheThing` component. Once all of the phantoms have been handled and removed, the entity will be automatically destroyed.

One common case for this is resource disposal, for this you can use `IDisposableComponent` and `DisposableComponentSystem`. Run a `DisposableComponentSystem` every frame for every type of disposable component and it will ensure resources are correctly disposed (even when the world is destroyed).

### Querying

Myriad.ECS has several different querying systems. These have different trade-offs in usability and performance.

#### QueryDescription/QueryBuilder

Queries can be filtered based on the components an Entity has. This is done with a `QueryDescription`, created with a `QueryBuilder`. Filtering like this is very fast, so as much as possible should be encoded into query descriptions. There are 4 types filtering a query can do:
 - Include: Entities **must** include this component.
 - Exclude: Entities **must not** include this component.
 - At Least One: Entities must contain one or more of the listed components.
 - Exactly One: Entities must contain exactly one of the listed components.

#### ChunkQuery

A "Chunk Query" runs a bit of code for every chunk of entities. The method call requires generic parameters, one for the query itself and one for every type of component required in the callback. The specified components are _not_ checked against the query, supplying components which are not matched by the query will trigger an exception. If no query is supplied, a default one will be used which includes all requested components.

```csharp
// Method signature
public int ExecuteChunk<TQ, T0, T1, ...etc>(TQ q, QueryDescription? query = null)

// Method call
_world.ExecuteChunk<IntegrateChunk, Position, Velocity>(new IntegrateChunk(), query);

// Query action definition
private struct IntegrateChunk
    : IChunkQuery2<Position, Velocity>
{
    public readonly void Execute(ReadOnlySpan<Entity> e, Span<Position> pos, Span<Velocity> vel)
    {
        for (var i = 0; i < pos.Length; i++)
            pos[i].Value += vel[i].Value;
    }
}
```

#### Query

A "Query" is the same as a chunk query, except that the inner loop over individual entities is handled for you.

```csharp
// Method signature
public int Execute<TQ, T0, T1, ...etc>(TQ q, QueryDescription? query = null)

// Method call
_world.Execute<Integrate, Position, Velocity>(new Integrate(), query);

// Query action definition
private struct IntegrateChunk
    : IQuery2<Position, Velocity>
{
    public readonly void Execute(Entity e, ref Position pos, ref Velocity vel)
    {
        pos.Value += vel.Value;
    }
}
```

#### Query Delegate

A delegate query does not require creating an entire struct to wrap your code.

```csharp
// Method signature
public void Query<T0, T1, ...etc>(QueryDelegate<T0> @delegate, QueryDescription? query = null)

// Method call
_world.Query(static (ref Position pos, ref Velocity vel) => {
    pos.Value += vel.Value;
});

// Method call with state (first arg to query is passed to delegate)
_world.Query(gametime, static (GameTime gametime, ref Position pos, ref Velocity vel) => {
    pos.Value += vel.Value;
});
```

#### Query Enumerable

An enumerable query simply returns results as an enumerable of tuples.

```csharp
// Method signature
public QueryResultEnumerable2<T0, T1> Query<T0, T1, ...etc>(QueryDescription query)

// Method call
foreach (var (e, p, v) in world.Query<Position, Velocity>())
    p.Ref.Value += v.Ref.Value;
```

### Systems

Systems are a completely optional part of `Myriad.ECS`. The library can be used as an in memory database, without any systems running every tick.

#### `ISystem`

All systems must implement `ISystem<TData>`, with an `Update(TData)` method. The `TData` parameter specifies what type will be passed into the `Update` method, for example a `GameTime` object.

#### `ISystemInit`

Adds an `Init` method that is run exactly once, before any other calls.

#### `ISystemBefore`

Adds a `BeforeUpdate(TData)` which is called every tick, just before `Update`.

#### `ISystemAfter`

Adds an `AfterUpdate(TData)` which is called every tick, just after `Update`.

#### `SystemGroup`

Usually you will want to declare a set of systems to run in order every frame. A `SystemGroup` does this, and handles correctly calling all of the above interface methods. A `SystemGroup` is itself a system, so groups can be nested.

A `SystemGroup` exposes a `TotalExecutionTime` property, which is the total time spent in `BeforeUpdate`, `Update` and `AfterUpdate` added together. This can be helpful for diagnosing slow systems.

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

In the above example `CommandBufferSystem`s are created at the start, are passed into various systems, and are scheduled at the end of their respective groups. A `CommandBufferSystem` exposes a `CommandBuffer` and executes the buffer when the systems runs.

This allows multiple systems to share one single `CommandBuffer`, which is executed just once at the end of a group of systems instead of every system making ad-hoc changes.

#### Parallel Systems

`Myriad.ECS` includes 3 parallel system groups, these are all somewhat experimental and should be used carefully.

#### `ParallelSystemGroup`

Runs all systems in each phase using `Parallel.ForEach`. This means all of the systems within the group run in parallel with each other in each phase. If the systems modify the `World` in a non-threadsafe way (for example writing a component in 2 queries) this can cause undefined behaviour.

#### Declarative Parallel Systems

Using a `ParallelSystemGroup` requires carefully manually grouping systems up that can be run in parallel, which is difficult and error prone. `ISystemDeclare` adds a `Declare` method to systems which allows them to declare what components they access:

```csharp
void Declare(ref SystemDeclaration declaration)
{
    declaration.Write<Position>();
    declaration.Read<Velocity>();
    declaration.Read<Acceleration>();
    declaration.Read<Static>();
}
```

This declaration can be used to automatically safely schedule systems in parallel. This is used by three new system groups.

#### `DeclareSystemGroup`

Is a simple serial system group which implement `ISystemDeclare` and groups together declarations from all child systems. This can be used by a wrapper group to schedule this entire group as one item.

#### `PhasedParallelSystemGroup`

This discovers groups of systems which do not "overlap" in the components they write and executes items in the group in parallel. Groups are executed serially. The order of execution of each group is undefined. The only guarantee is that a system will not run in parallel with a another system that is modifying the same component as this one is reading or writing.

Discovering the phasee groups is very quick, but this can only be used when the order of execution of the systems is completely unimportant.

#### `OrderedParallelSystemGroup`

Runs all the systems in the group "in order", but with parallelism where it cannot be "observed". Systems which read a component wait for earlier systems which write that component. Systems which write a component wait for earlier systems which write or read that component. As long as systems only read and write components and do not access any external state this should be identical to running the systems serially.