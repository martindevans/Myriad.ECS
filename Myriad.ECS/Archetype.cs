using System.Collections;
using Myriad.ECS.IDs;

namespace Myriad.ECS;

public sealed class Archetype
{
    public IReadOnlySet<ComponentID> Components { get; set; }

    //internal IReadOnlySet<ComponentID> Components
    //{
    //    get => throw new NotImplementedException();
    //}
}