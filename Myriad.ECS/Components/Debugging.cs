namespace Myriad.ECS.Components;

/// <summary>
/// Friendly name to use when displaying this entity for debugging purposes.
/// Recommended (but not required) to make this unique!
/// </summary>
/// <param name="Name"></param>
// ReSharper disable once NotAccessedPositionalProperty.Global
public record struct DebugDisplayName(string Name) : IComponent;