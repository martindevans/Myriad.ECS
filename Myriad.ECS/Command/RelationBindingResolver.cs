using Myriad.ECS.Allocations;
using Myriad.ECS.Components;
using Myriad.ECS.IDs;

namespace Myriad.ECS.Command;

public partial class CommandBuffer
{
    private abstract class BaseRelationBinder<TKey>
        where TKey : struct
    {
        private readonly Dictionary<ComponentID, IInternalResolver> _resolvers = [ ];

        public void Create<TComponent>(TKey key, BufferedEntity relation)
            where TComponent : IEntityRelationSetter
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

        protected abstract void Apply<TComponent>(Resolver buffer, Dictionary<TKey, BufferedEntity> bindings, ref Blocker blocker)
            where TComponent : IEntityRelationSetter;

        public void Apply(Resolver resolver, ref Blocker blocker)
        {
            if (_resolvers.Count == 0)
                return;
            
            foreach (var internalResolver in _resolvers)
                internalResolver.Value.Apply(resolver, this, ref blocker);
        }

        public void Clear()
        {
            foreach (var internalResolver in _resolvers)
                internalResolver.Value.Recycle();
            _resolvers.Clear();
        }

        private sealed class Binder<TComponent>
            : IInternalResolver
            where TComponent : IEntityRelationSetter
        {
            private readonly Dictionary<TKey, BufferedEntity> _relationships = [];

            public void Add(TKey key, BufferedEntity relation)
            {
                _relationships[key] = relation;
            }

            public void Apply(Resolver buffer, BaseRelationBinder<TKey> binder, ref Blocker blocker)
            {
                binder.Apply<TComponent>(buffer, _relationships, ref blocker);
            }

            public void Recycle()
            {
                _relationships.Clear();
                Pool<Binder<TComponent>>.Return(this);
            }
        }

        private interface IInternalResolver
        {
            void Apply(Resolver buffer, BaseRelationBinder<TKey> binder, ref Blocker blocker);

            void Recycle();
        }
    }

    private class BufferedRelationBinder
        : BaseRelationBinder<BufferedEntity>
    {
        protected override void Apply<TComponent>(Resolver buffer, Dictionary<BufferedEntity, BufferedEntity> bindings, ref Blocker blocker)
        {
            foreach (var (setOnBuf, tgtBuf) in bindings)
            {
                var setOn = setOnBuf.Resolve();
                var tgt = tgtBuf.Resolve();

                // Safe not to block - these are buffered entities, which means we just created them.
                // The archetype must already have been blocked on during creation.
                setOn.GetComponentRef<TComponent>(block:false).Target = tgt;
            }
        }
    }

    private class UnbufferedRelationBinder
        : BaseRelationBinder<Entity>
    {
        protected override void Apply<TComponent>(Resolver buffer, Dictionary<Entity, BufferedEntity> bindings, ref Blocker blocker)
        {
            var cid = ComponentID<TComponent>.ID;
            Span<ComponentID> cidSpan = stackalloc ComponentID[1] { cid };

            foreach (var (setOn, tgtBuf) in bindings)
            {
                var tgt = tgtBuf.Resolve();

                // Get entity info for the destination
                var setOnEntityInfo = setOn.World.GetEntityInfo(setOn);

                // Block on the archetype for the destination entity. We're not doing this through the
                // `blocker` because it allows us to do a more fine-grained block on just this component.
                if (!blocker.HasBlocked(setOnEntityInfo.Chunk.Archetype))
                    setOnEntityInfo.Chunk.Archetype.Block(cidSpan);

                // Set the relation entity into the dest entity
                setOnEntityInfo.GetMutable<TComponent>().Target = tgt;
            }
        }
    }
}