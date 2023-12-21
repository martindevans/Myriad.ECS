namespace Myriad.ECS;

public sealed partial class World
{
    public List<Archetype> _archetypes;

    //public Future<Empty> Query<TQ>(TQ query)
    //    where TQ : struct, IQuery
    //{

    //}

    //todo: use source generation to generate a special extension method for every single query.

    public IReadOnlyList<Archetype> Archetypes => _archetypes;
}