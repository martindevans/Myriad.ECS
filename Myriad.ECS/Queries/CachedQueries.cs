using Myriad.ECS.Queries;
using Myriad.ECS.IDs;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedParameter.Global
// ReSharper disable LoopCanBeConvertedToQuery
// ReSharper disable CheckNamespace
// ReSharper disable ArrangeAccessorOwnerBody

namespace Myriad.ECS.Worlds
{
    public partial class World
    {
        private readonly List<(ComponentID[], QueryDescription)> _queryCache1 = [ ];

        internal QueryDescription GetCachedQuery<T0>()
            where T0 : IComponent
        {
            // Find query that matches these types
            var query = default(QueryDescription);
            foreach (var item in _queryCache1)
            {
                if (!item.Item1.Contains(ComponentID<T0>.ID))
                    continue;

                query = item.Item2;
                break;
            }

            // Didn't find one, create it now and store it in the cache
            if (query == null)
            {
                query = new QueryBuilder()
                    .Include<T0>()
                    .Build(this);

                _queryCache1.Add(([
                    ComponentID<T0>.ID,
                ], query));
            }

            return query;
        }
    }
}


namespace Myriad.ECS.Worlds
{
    public partial class World
    {
        private readonly List<(ComponentID[], QueryDescription)> _queryCache2 = [ ];

        internal QueryDescription GetCachedQuery<T0, T1>()
            where T0 : IComponent
            where T1 : IComponent
        {
            // Find query that matches these types
            var query = default(QueryDescription);
            foreach (var item in _queryCache2)
            {
                if (!item.Item1.Contains(ComponentID<T0>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T1>.ID))
                    continue;

                query = item.Item2;
                break;
            }

            // Didn't find one, create it now and store it in the cache
            if (query == null)
            {
                query = new QueryBuilder()
                    .Include<T0>()
                    .Include<T1>()
                    .Build(this);

                _queryCache2.Add(([
                    ComponentID<T0>.ID,
                    ComponentID<T1>.ID,
                ], query));
            }

            return query;
        }
    }
}


namespace Myriad.ECS.Worlds
{
    public partial class World
    {
        private readonly List<(ComponentID[], QueryDescription)> _queryCache3 = [ ];

        internal QueryDescription GetCachedQuery<T0, T1, T2>()
            where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
        {
            // Find query that matches these types
            var query = default(QueryDescription);
            foreach (var item in _queryCache3)
            {
                if (!item.Item1.Contains(ComponentID<T0>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T1>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T2>.ID))
                    continue;

                query = item.Item2;
                break;
            }

            // Didn't find one, create it now and store it in the cache
            if (query == null)
            {
                query = new QueryBuilder()
                    .Include<T0>()
                    .Include<T1>()
                    .Include<T2>()
                    .Build(this);

                _queryCache3.Add(([
                    ComponentID<T0>.ID,
                    ComponentID<T1>.ID,
                    ComponentID<T2>.ID,
                ], query));
            }

            return query;
        }
    }
}


namespace Myriad.ECS.Worlds
{
    public partial class World
    {
        private readonly List<(ComponentID[], QueryDescription)> _queryCache4 = [ ];

        internal QueryDescription GetCachedQuery<T0, T1, T2, T3>()
            where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
        {
            // Find query that matches these types
            var query = default(QueryDescription);
            foreach (var item in _queryCache4)
            {
                if (!item.Item1.Contains(ComponentID<T0>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T1>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T2>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T3>.ID))
                    continue;

                query = item.Item2;
                break;
            }

            // Didn't find one, create it now and store it in the cache
            if (query == null)
            {
                query = new QueryBuilder()
                    .Include<T0>()
                    .Include<T1>()
                    .Include<T2>()
                    .Include<T3>()
                    .Build(this);

                _queryCache4.Add(([
                    ComponentID<T0>.ID,
                    ComponentID<T1>.ID,
                    ComponentID<T2>.ID,
                    ComponentID<T3>.ID,
                ], query));
            }

            return query;
        }
    }
}


namespace Myriad.ECS.Worlds
{
    public partial class World
    {
        private readonly List<(ComponentID[], QueryDescription)> _queryCache5 = [ ];

        internal QueryDescription GetCachedQuery<T0, T1, T2, T3, T4>()
            where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
        {
            // Find query that matches these types
            var query = default(QueryDescription);
            foreach (var item in _queryCache5)
            {
                if (!item.Item1.Contains(ComponentID<T0>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T1>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T2>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T3>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T4>.ID))
                    continue;

                query = item.Item2;
                break;
            }

            // Didn't find one, create it now and store it in the cache
            if (query == null)
            {
                query = new QueryBuilder()
                    .Include<T0>()
                    .Include<T1>()
                    .Include<T2>()
                    .Include<T3>()
                    .Include<T4>()
                    .Build(this);

                _queryCache5.Add(([
                    ComponentID<T0>.ID,
                    ComponentID<T1>.ID,
                    ComponentID<T2>.ID,
                    ComponentID<T3>.ID,
                    ComponentID<T4>.ID,
                ], query));
            }

            return query;
        }
    }
}


namespace Myriad.ECS.Worlds
{
    public partial class World
    {
        private readonly List<(ComponentID[], QueryDescription)> _queryCache6 = [ ];

        internal QueryDescription GetCachedQuery<T0, T1, T2, T3, T4, T5>()
            where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
        {
            // Find query that matches these types
            var query = default(QueryDescription);
            foreach (var item in _queryCache6)
            {
                if (!item.Item1.Contains(ComponentID<T0>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T1>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T2>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T3>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T4>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T5>.ID))
                    continue;

                query = item.Item2;
                break;
            }

            // Didn't find one, create it now and store it in the cache
            if (query == null)
            {
                query = new QueryBuilder()
                    .Include<T0>()
                    .Include<T1>()
                    .Include<T2>()
                    .Include<T3>()
                    .Include<T4>()
                    .Include<T5>()
                    .Build(this);

                _queryCache6.Add(([
                    ComponentID<T0>.ID,
                    ComponentID<T1>.ID,
                    ComponentID<T2>.ID,
                    ComponentID<T3>.ID,
                    ComponentID<T4>.ID,
                    ComponentID<T5>.ID,
                ], query));
            }

            return query;
        }
    }
}


namespace Myriad.ECS.Worlds
{
    public partial class World
    {
        private readonly List<(ComponentID[], QueryDescription)> _queryCache7 = [ ];

        internal QueryDescription GetCachedQuery<T0, T1, T2, T3, T4, T5, T6>()
            where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
        {
            // Find query that matches these types
            var query = default(QueryDescription);
            foreach (var item in _queryCache7)
            {
                if (!item.Item1.Contains(ComponentID<T0>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T1>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T2>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T3>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T4>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T5>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T6>.ID))
                    continue;

                query = item.Item2;
                break;
            }

            // Didn't find one, create it now and store it in the cache
            if (query == null)
            {
                query = new QueryBuilder()
                    .Include<T0>()
                    .Include<T1>()
                    .Include<T2>()
                    .Include<T3>()
                    .Include<T4>()
                    .Include<T5>()
                    .Include<T6>()
                    .Build(this);

                _queryCache7.Add(([
                    ComponentID<T0>.ID,
                    ComponentID<T1>.ID,
                    ComponentID<T2>.ID,
                    ComponentID<T3>.ID,
                    ComponentID<T4>.ID,
                    ComponentID<T5>.ID,
                    ComponentID<T6>.ID,
                ], query));
            }

            return query;
        }
    }
}


namespace Myriad.ECS.Worlds
{
    public partial class World
    {
        private readonly List<(ComponentID[], QueryDescription)> _queryCache8 = [ ];

        internal QueryDescription GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7>()
            where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
        {
            // Find query that matches these types
            var query = default(QueryDescription);
            foreach (var item in _queryCache8)
            {
                if (!item.Item1.Contains(ComponentID<T0>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T1>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T2>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T3>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T4>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T5>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T6>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T7>.ID))
                    continue;

                query = item.Item2;
                break;
            }

            // Didn't find one, create it now and store it in the cache
            if (query == null)
            {
                query = new QueryBuilder()
                    .Include<T0>()
                    .Include<T1>()
                    .Include<T2>()
                    .Include<T3>()
                    .Include<T4>()
                    .Include<T5>()
                    .Include<T6>()
                    .Include<T7>()
                    .Build(this);

                _queryCache8.Add(([
                    ComponentID<T0>.ID,
                    ComponentID<T1>.ID,
                    ComponentID<T2>.ID,
                    ComponentID<T3>.ID,
                    ComponentID<T4>.ID,
                    ComponentID<T5>.ID,
                    ComponentID<T6>.ID,
                    ComponentID<T7>.ID,
                ], query));
            }

            return query;
        }
    }
}


namespace Myriad.ECS.Worlds
{
    public partial class World
    {
        private readonly List<(ComponentID[], QueryDescription)> _queryCache9 = [ ];

        internal QueryDescription GetCachedQuery<T0, T1, T2, T3, T4, T5, T6, T7, T8>()
            where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
            where T8 : IComponent
        {
            // Find query that matches these types
            var query = default(QueryDescription);
            foreach (var item in _queryCache9)
            {
                if (!item.Item1.Contains(ComponentID<T0>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T1>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T2>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T3>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T4>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T5>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T6>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T7>.ID))
                    continue;
                if (!item.Item1.Contains(ComponentID<T8>.ID))
                    continue;

                query = item.Item2;
                break;
            }

            // Didn't find one, create it now and store it in the cache
            if (query == null)
            {
                query = new QueryBuilder()
                    .Include<T0>()
                    .Include<T1>()
                    .Include<T2>()
                    .Include<T3>()
                    .Include<T4>()
                    .Include<T5>()
                    .Include<T6>()
                    .Include<T7>()
                    .Include<T8>()
                    .Build(this);

                _queryCache9.Add(([
                    ComponentID<T0>.ID,
                    ComponentID<T1>.ID,
                    ComponentID<T2>.ID,
                    ComponentID<T3>.ID,
                    ComponentID<T4>.ID,
                    ComponentID<T5>.ID,
                    ComponentID<T6>.ID,
                    ComponentID<T7>.ID,
                    ComponentID<T8>.ID,
                ], query));
            }

            return query;
        }
    }
}



