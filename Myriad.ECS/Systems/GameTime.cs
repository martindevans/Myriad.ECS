namespace Myriad.ECS.Systems;

/// <summary>
/// Stores the current simulation time
/// </summary>
public class GameTime
{
    /// <summary>
    /// The current time
    /// </summary>
    public double Time { get; private set; }

    /// <summary>
    /// The change in time since the last frame
    /// </summary>
    public double DeltaTime { get; private set; }

    /// <summary>
    /// The total number of elapsed frames
    /// </summary>
    public ulong Frame { get; private set; }

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

    /// <summary>
    /// Overwrite properties
    /// </summary>
    /// <param name="time"></param>
    /// <param name="deltaTime"></param>
    /// <param name="frameCount"></param>
    public void Set(double time = 0, double deltaTime = 0, ulong frameCount = 0)
    {
        Time = time;
        DeltaTime = deltaTime;
        Frame = frameCount;
    }
}