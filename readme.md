# Myriad.ECS

[![Nuget](https://img.shields.io/nuget/v/Myriad.ECS?style=for-the-badge)](https://www.nuget.org/packages/Myriad.ECS/)

Myriad.ECS is a high performance Entity Component System (ECS) for C#.

## Benchmarks

[C# ECS Benchmarks](https://github.com/Doraku/Ecs.CSharp.Benchmark).

## Guide

Everything in Myriad happens in a `World`, created with a `WorldBuilder`.

### Components

Entities are just IDs, associated with a set of components. A component can be any type (managed or unmanaged) that implements `IComponent`.

```csharp
public record struct Position(Vector2 Value) : IComponent;
public record struct Velocity(Vector2 Value) : IComponent;
```

`IComponent` is simply a tag that ensures you cannot accidentally attach something to an entity that you didn't mean to. For example adding a `Vector2` to an `Entity` directly, instead of a `Position` or `Velocity` component.

### Phantom Components

Myriad supports "Phantom Components", these are defined by `IPhantomComponent` instead of `IComponent`. When an `Entity` with any phantom components is destroyed the entity is not actually destroyed, instead it becomes a "phantom". Phantom entities are automatically **excluded** from queries, and must be explicitly included with `.Include<Phantom>`.

A phantom entities can be destroyed in two ways:
 - Delete it again.
 - Remove all phantom components.

Phantom components are useful for tracking per-entity state. For example if there is some event that needs to run when an entity is destroyed you can attach a component when the entity is created (`DoTheThing : IPhantomComponent`) and then query for `Include<DoTheThing, Phantom>()`. When you have done whatever is needed you should remove the `DoTheThing` component. Once all of the phantoms have been handled and removed, the entity will be automatically destroyed.

One common case for this is resource disposal, for this you can use `IDisposableComponent` and `DisposableComponentSystem`. Run a `DisposableComponentSystem` every frame for every type of disposable components and it will ensure resources are correctly disposed (even when the world is destroyed).

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

An enumerable query simply returns results as an enumerable of tuples. This requires `dotnet8.0` and is not available in `netstandard2.0`.

```csharp
// Method signature
public QueryResultEnumerable2<T0, T1> Query<T0, T1, ...etc>(QueryDescription query)

// Method call
foreach (var (e, p, v) in world.Query<Position, Velocity>())
    p.Ref.Value += v.Ref.Value;
```