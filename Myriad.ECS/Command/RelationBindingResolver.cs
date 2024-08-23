using Myriad.ECS.Allocations;
using Myriad.ECS.Components;
using Myriad.ECS.Extensions;
using Myriad.ECS.IDs;

namespace Myriad.ECS.Command;

public partial class CommandBuffer
{
    private abstract class BaseRelationBinder<TKey>
        where TKey : struct
    {
        private readonly Dictionary<ComponentID, IInternalResolver> _resolvers = [ ];

        public void Create<TComponent>(TKey key, BufferedEntity relation)
            where TComponent : IEntityRelationComponent
        {
            var c = ComponentID<TComponent>.ID;
            if (!_resolvers.TryGetValue(c, out var resolver))
            {
                resolver = Pool<Binder<TComponent>>.Get();
                _resolvers.Add(c, resolver);
            }

            var concrete = (Binder<TComponent>)resolver;
            concrete.Add(key, relation);
        }

        protected abstract void Apply<TComponent>(Resolver buffer, SortedList<TKey, BufferedEntity> bindings)
            where TComponent : IEntityRelationComponent;

        public void Apply(Resolver resolver)
        {
            foreach (var internalResolver in _resolvers)
                internalResolver.Value.Apply(resolver, this);
        }

        public void Clear()
        {
            foreach (var internalResolver in _resolvers)
                internalResolver.Value.Recycle();
            _resolvers.Clear();
        }

        private sealed class Binder<TComponent>
            : IInternalResolver
            where TComponent : IEntityRelationComponent
        {
            private readonly SortedList<TKey, BufferedEntity> _relationships = [];

            public void Add(TKey key, BufferedEntity relation)
            {
                _relationships[key] = relation;
            }

            public void Apply(Resolver buffer, BaseRelationBinder<TKey> binder)
            {
                binder.Apply<TComponent>(buffer, _relationships);
            }

            public void Recycle()
            {
                _relationships.Clear();
                Pool<Binder<TComponent>>.Return(this);
            }
        }

        private interface IInternalResolver
        {
            void Apply(Resolver buffer, BaseRelationBinder<TKey> binder);

            void Recycle();
        }
    }

    private class BufferedRelationBinder
        : BaseRelationBinder<BufferedEntity>
    {
        protected override void Apply<TComponent>(Resolver buffer, SortedList<BufferedEntity, BufferedEntity> bindings)
        {
            foreach (var (setOnBuf, tgtBuf) in bindings.Enumerable())
            {
                var setOn = setOnBuf.Resolve();
                var tgt = tgtBuf.Resolve();

                setOn.GetComponentRef<TComponent>(buffer.World).Target = tgt;
            }
        }
    }

    private class UnbufferedRelationBinder
        : BaseRelationBinder<Entity>
    {
        protected override void Apply<TComponent>(Resolver buffer, SortedList<Entity, BufferedEntity> bindings)
        {
            foreach (var (setOn, tgtBuf) in bindings.Enumerable())
            {
                var tgt = tgtBuf.Resolve();

                setOn.GetComponentRef<TComponent>(buffer.World).Target = tgt;
            }
        }
    }
}