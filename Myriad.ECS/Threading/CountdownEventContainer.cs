namespace Myriad.ECS.Threading;

internal class CountdownEventContainer
{
    public CountdownEvent Event { get; }

    public CountdownEventContainer()
    {
        Event = new CountdownEvent(0);
    }
}