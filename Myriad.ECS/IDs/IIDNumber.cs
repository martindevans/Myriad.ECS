namespace Myriad.ECS.IDs;

public interface IIDNumber<TSelf>
    : IEquatable<TSelf>, IComparable<TSelf>
    where TSelf : struct, IIDNumber<TSelf>
{
    TSelf Next();
}