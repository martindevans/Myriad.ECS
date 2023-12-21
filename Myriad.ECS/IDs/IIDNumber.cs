namespace Myriad.ECS.IDs;

public interface IIDNumber<TSelf>
    : IEquatable<TSelf>
    where TSelf : struct, IIDNumber<TSelf>
{
    static abstract TSelf First();

    static abstract TSelf Next(TSelf value);
}