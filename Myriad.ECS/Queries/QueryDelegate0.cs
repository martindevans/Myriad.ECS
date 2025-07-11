using System.Runtime.CompilerServices;
using Myriad.ECS.Queries;

namespace Myriad.ECS.Queries
{
    /// <summary>
    /// A delegate called for entities in a query. Receives the entity.
    /// </summary>
    public delegate void QueryDelegateEntity(Entity entity);

    /// <summary>
    /// A delegate called for entities in a query. Receives the entity, a data object passed into the query and the components requested.
    /// </summary>
    public delegate void QueryDelegateEntityData<in TData>(TData data, Entity entity);

    internal readonly struct QueryDelegateWrapper0E
        : IQuery
    {
        private readonly QueryDelegateEntity _delegate;

        public QueryDelegateWrapper0E(QueryDelegateEntity @delegate)
        {
            _delegate = @delegate;
        }

        public void Execute(Entity e)
        {
            _delegate(e);
        }
    }

    internal readonly struct QueryDelegateWrapper0ED<TData>
        : IQuery
    {
        private readonly QueryDelegateEntityData<TData> _delegate;
        private readonly TData _data;

        public QueryDelegateWrapper0ED(TData data, QueryDelegateEntityData<TData> @delegate)
        {
            _data = data;
            _delegate = @delegate;
        }

        public void Execute(Entity e)
        {
            _delegate(_data, e);
        }
    }
}

namespace Myriad.ECS.Worlds
{
    public partial class World
    {
		/// <summary>
		/// Execute a delegate for every entity in a query.
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="query"></param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Query(QueryDelegateEntity @delegate, QueryDescription query)
		{
			return Execute(
				new QueryDelegateWrapper0E(@delegate),
				query
			);
		}

        // vv-- TData variants --vv //

        /// <summary>
        /// Execute a delegate for every entity in a query. Passing an object through into every call.
        /// </summary>
        /// <param name="delegate"></param>
        /// <param name="query"></param>
        /// <param name="data">The object passed into every call</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Query<TData>(TData data, QueryDelegateEntityData<TData> @delegate, QueryDescription query)
		{
			return Execute(
				new QueryDelegateWrapper0ED<TData>(data, @delegate),
				query
			);
		}
	}
}