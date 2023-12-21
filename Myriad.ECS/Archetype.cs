using System.Collections;
using Myriad.ECS.IDs;

namespace Myriad.ECS;

public sealed class Archetype
{
    public HashSet<ComponentID> Components { get; set; }

    //internal IReadOnlySet<ComponentID> Components
    //{
    //    get => throw new NotImplementedException();
    //}
}