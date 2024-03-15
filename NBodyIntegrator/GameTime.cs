namespace NBodyIntegrator;

public record GameTime(double DeltaTime)
{
    public double Time { get; private set; }
    public ulong Ticks { get; private set; }

    public void Tick()
    {
        Time += DeltaTime;
        Ticks++;
    }
}