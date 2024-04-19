using Microsoft.VisualStudio.TestTools.UnitTesting;
using Myriad.ECS.Command;
using Myriad.ECS.Queries;
using Myriad.ECS.Worlds;

namespace Myriad.ECS.Tests;

[TestClass]
public class StructQueryTests
{
    [TestMethod]
    public void MatchesWithManagedObject()
    {
        var w = new WorldBuilder().Build();

        var cb = new CommandBuffer(w);
        cb.Create().Set(new ComponentInt64(1));
        cb.Create().Set(new ComponentFloat(2));
        cb.Create().Set(new ComponentObject(new object()));
        cb.Playback().Dispose();

        var l = new List<Entity>();
        w.Execute<PutEntitiesInList<ComponentObject>, ComponentObject>(new PutEntitiesInList<ComponentObject>(l));

        Assert.AreEqual(1, l.Count);
    }

    private readonly struct PutEntitiesInList<T>(List<Entity> entities)
        : IQuery1<T>
        where T : IComponent
    {
        public readonly List<Entity> Entities = entities;

        public void Execute(Entity e, ref T t0)
        {
            Entities.Add(e);
        }
    }
}