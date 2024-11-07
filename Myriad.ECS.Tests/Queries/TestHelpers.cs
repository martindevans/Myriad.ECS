using Myriad.ECS.Command;
using Myriad.ECS.Worlds;

namespace Myriad.ECS.Tests.Queries;

public static class TestHelpers
{
    public static CommandBuffer SetupRandomEntities(World world, uint uniqueComponents = 7, int count = 1_000_000)
    {
        uniqueComponents = Math.Clamp(uniqueComponents, 0, 7);

        var b = new CommandBuffer(world);
        var r = new Random(3452);
        for (var i = 0; i < count; i++)
        {
            var eb = b.Create();

            for (var j = 0; j < 5; j++)
            {
                switch (r.Next(0, checked((int)uniqueComponents)))
                {
                    case 0: eb.Set(new ComponentByte(0), CommandBuffer.DuplicateSet.Overwrite); break;
                    case 1: eb.Set(new ComponentInt16(0), CommandBuffer.DuplicateSet.Overwrite); break;
                    case 2: eb.Set(new ComponentFloat(0), CommandBuffer.DuplicateSet.Overwrite); break;
                    case 3: eb.Set(new ComponentInt32(0), CommandBuffer.DuplicateSet.Overwrite); break;
                    case 4: eb.Set(new ComponentInt64(0), CommandBuffer.DuplicateSet.Overwrite); break;
                    case 5: eb.Set(new Component0(), CommandBuffer.DuplicateSet.Overwrite); break;
                    case 6: eb.Set(new Component1(), CommandBuffer.DuplicateSet.Overwrite); break;
                }
            }
        }

        return b;
    }
}