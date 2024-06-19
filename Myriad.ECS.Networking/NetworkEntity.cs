using HandySerialization;
using HandySerialization.Extensions;

namespace Myriad.ECS.Networking
{
    //public readonly record struct NetworkEntity(ulong Id)
    //    : IByteSerializable<NetworkEntity>
    //{
    //    public void Write<TWriter>(ref TWriter writer)
    //        where TWriter : struct, IByteWriter
    //    {
    //        writer.Write(Id);
    //    }

    //    public NetworkEntity Read<TReader>(ref TReader reader)
    //        where TReader : struct, IByteReader
    //    {
    //        return new NetworkEntity(reader.ReadUInt64());
    //    }
    //}
}
