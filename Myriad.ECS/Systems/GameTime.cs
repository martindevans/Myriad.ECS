namespace Myriad.ECS.Systems;

/// <summary>
/// Stores the current simulation time
/// </summary>
public class GameTime
{
    /// <summary>
    /// The current time
    /// </summary>
    public double Time { get; set; }

    /// <summary>
    /// The change in time since the last frame
    /// </summary>
    public double DeltaTime { get; set; }

    /// <summary>
    /// The total number of elapsed frames
    /// </summary>
    public ulong Frame { get; set; }

    /// <summary>
    /// Advance the time by one step
    /// </summary>
    /// <param name="dt"></param>
    public void Tick(double dt)
    {
        Time += dt;
        DeltaTime = dt;
        Frame++;
    }
}