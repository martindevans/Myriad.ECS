using Myriad.ECS.IDs;

namespace Myriad.ECS.Tests;

[TestClass]
public class ChunkBitFlagRegistryTests
{
    private struct Flag0 : IChunkBitFlag;
    private struct Flag1 : IChunkBitFlag;
    private struct Flag2 : IChunkBitFlag;
    private struct Flag3 : IChunkBitFlag;

    [TestMethod]
    public void AssignsDistinctIds()
    {
        var ids = new[]
        {
            ChunkBitFlagID<Flag0>.ID,
            ChunkBitFlagRegistry.Get<Flag0>(),
            ChunkBitFlagRegistry.Get(typeof(Flag0)),

            ChunkBitFlagID<Flag1>.ID,
            ChunkBitFlagRegistry.Get<Flag1>(),
            ChunkBitFlagRegistry.Get(typeof(Flag1)),

            ChunkBitFlagID.Get(typeof(Flag2)),

            ChunkBitFlagID.Get(typeof(Flag3)),
        };

        Assert.AreEqual(4, ids.Distinct().Count());

        Assert.AreEqual(typeof(Flag0), ChunkBitFlagID<Flag0>.ID.Type);
    }

    [TestMethod]
    public void DoesNotReassign()
    {
        var id1 = ChunkBitFlagID<Flag0>.ID;
        var id2 = ChunkBitFlagID<Flag0>.ID;

        Assert.AreEqual(id1, id2);
    }

    [TestMethod]
    public void CannotAssignWrongType()
    {
        Assert.ThrowsException<ArgumentException>(() =>
        {
            ChunkBitFlagID.Get(typeof(int));
        });
    }

    [TestMethod]
    public void ThrowsForUnknownId()
    {
        Assert.ThrowsException<InvalidOperationException>(() =>
        {
            var t = default(ChunkBitFlagID).Type;
        });
    }

    [TestMethod]
    public void CompareBitFlags()
    {
        var a = ChunkBitFlagID<Flag0>.ID;
        var b = ChunkBitFlagID<Flag1>.ID;

        var cmpAB = a.CompareTo(b);
        var cmpBA = b.CompareTo(a);

        Assert.AreNotEqual(0, cmpAB);
        Assert.AreNotEqual(0, cmpBA);

        Assert.AreEqual(cmpAB, -cmpBA);
    }

    [TestMethod]
    public void CompareBitFlagsStrings()
    {
        var a = ChunkBitFlagID<Flag0>.ID;
        var b = ChunkBitFlagID<Flag1>.ID;

        var strA = a.ToString();
        var strB = b.ToString();

        Assert.AreNotEqual(strA, strB);
    }
}