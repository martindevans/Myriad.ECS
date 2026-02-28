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

        [TestMethod]
        public void DeclareSystemGroup()
        {
            var group = new DeclareSystemGroup<int>(
                "group",
                new DeclareSystemRead([ ComponentID<Component0>.ID, ComponentID<Component1>.ID]),
                new DeclareSystemRead([ ComponentID<Component1>.ID, ComponentID<Component2>.ID]),
                new DeclareSystemWrite<int>([ ComponentID<Component2>.ID, ComponentID<Component3>.ID])
            );

            // Get the declaration from the systems
            SystemDeclaration decl = default;
            group.Declare(ref decl);

            // Another decl that only reads things the first reads
            var x = new SystemDeclaration();
            x.Read<Component0>();
            x.Read<Component1>();
            Assert.IsFalse(x.Intersects(ref decl));

            // Another decl that reads things the first writes
            var y = new SystemDeclaration();
            y.Read<Component0>();
            y.Read<Component2>();
            Assert.IsTrue(y.Intersects(ref decl));

            // Quick check for update phases
            group.BeforeUpdate(0);
            group.Update(1);
            group.AfterUpdate(2);
        }

        private class DeclareSystemRead
            : ISystemDeclare<int>, ISystemBefore<int>, ISystemAfter<int>
        {
            private readonly ComponentID[] _ids;

            public DeclareSystemRead(params ComponentID[] ids)
            {
                _ids = ids;
            }

            public void Declare(ref SystemDeclaration declaration)
            {
                declaration.Read(FrozenOrderedListSet<ComponentID>.Create(_ids));
            }

            public void BeforeUpdate(int data)
            {
                Assert.AreEqual(0, data);
            }

            public void Update(int data)
            {
                Assert.AreEqual(1, data);
            }

            public void AfterUpdate(int data)
            {
                Assert.AreEqual(2, data);
            }
        }

        private class DeclareSystemWrite<TData>
            : ISystemDeclare<TData>
        {
            private readonly ComponentID[] _ids;

            public DeclareSystemWrite(params ComponentID[] ids)
            {
                _ids = ids;
            }

            public void Update(TData data)
            {
            }

            public void Declare(ref SystemDeclaration declaration)
            {
                declaration.Write(FrozenOrderedListSet<ComponentID>.Create(_ids));
            }
        }
    }
}
