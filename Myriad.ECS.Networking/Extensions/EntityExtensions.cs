using HandySerialization;

namespace Myriad.ECS.Networking.Extensions;

public static class EntityExtensions
{
    public static void Write<TWriter>(this ref TWriter writer, Entity entity)
        where TWriter : struct, IByteWriter
    {
        throw new NotImplementedException("not generally safe to send Entity over network - IDs are not synced!");
    }
}