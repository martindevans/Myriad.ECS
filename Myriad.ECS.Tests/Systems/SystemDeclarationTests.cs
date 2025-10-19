using Myriad.ECS.Collections;
using Myriad.ECS.IDs;
using Myriad.ECS.Systems;

namespace Myriad.ECS.Tests.Systems
{
    [TestClass]
    public class SystemDeclarationTests
    {
        [TestMethod]
        public void ReadDeclaration()
        {
            var a = new SystemDeclaration();
            a.Read<Component0>();

            var b = new SystemDeclaration();
            b.Read(FrozenOrderedListSet<ComponentID>.Create(new List<ComponentID> { ComponentID<Component0>.ID }));

            // They both READ the same components, that's fine!
            Assert.IsFalse(a.Intersects(in b));
        }

        [TestMethod]
        public void WriteDeclarationIntersects()
        {
            var a = new SystemDeclaration();
            a.Read<Component0>();

            var b = new SystemDeclaration();
            b.Write(FrozenOrderedListSet<ComponentID>.Create(new List<ComponentID> { ComponentID<Component0>.ID }));

            // One READs and one WRITEs, intersect!
            Assert.IsTrue(a.Intersects(in b));
        }

        [TestMethod]
        public void WriteDeclarationNoneIntersects()
        {
            var a = new SystemDeclaration();
            a.Read<Component0>();

            var b = new SystemDeclaration();
            b.Write(FrozenOrderedListSet<ComponentID>.Create(new List<ComponentID> { ComponentID<Component1>.ID }));

            // One READs and one WRITEs, but no intersection because different components
            Assert.IsFalse(a.Intersects(in b));
        }
    }
}
