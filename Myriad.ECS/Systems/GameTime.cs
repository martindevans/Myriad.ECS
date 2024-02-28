namespace Myriad.ECS.Systems;

public record GameTime(double DeltaTime)
{
    public double Time { get; private set; }

    public void Tick()
    {
        Time += DeltaTime;
    }
}