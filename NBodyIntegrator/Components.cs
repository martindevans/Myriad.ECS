using Myriad.ECS;
using NBodyIntegrator.Units;

namespace NBodyIntegrator;

public record struct WorldPosition(Metre3 Value) : IComponent;