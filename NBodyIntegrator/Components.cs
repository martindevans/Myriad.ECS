using Myriad.ECS;
using NBodyIntegrator.Units;

namespace NBodyIntegrator;

public record struct WorldPosition(Metre3 Value) : IComponent;

public readonly record struct EntityArray<T>(T[] Array) : IComponent
{
    public int Length => Array.Length;
}