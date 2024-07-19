namespace Myriad.ECS.Threading;

internal class CountdownEventContainer
{
    public CountdownEvent Event { get; } = new(0);
}