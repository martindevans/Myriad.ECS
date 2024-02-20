using Myriad.ECS.Collections;
using Myriad.ECS.IDs;

namespace Myriad.ECS.Worlds.Archetypes;

/// <summary>
/// An archetype hash is made by mixing all of the components in an archetype.
/// Components can be "toggled" to update the hash to a new value for an archetype with/without those components.
/// </summary>
internal readonly record struct ArchetypeHash
{
    public long Value { get; private init; }

    /// <summary>
    /// Toggle (add or remove) the given component
    /// </summary>
    /// <param name="component"></param>
    public ArchetypeHash Toggle(ComponentID component)
    {
        return new ArchetypeHash
        {
            Value = Toggle(Value, component),
        };
    }

    private static long Toggle(long value, ComponentID component)
    {
        unchecked
        {
            // Add a (non prime) value then multiply component by a large prime.
            // This should spread the bits around through the hash space.
            var v = ((long)component.Value + 79_528) * 337_190_719_854_678_689;

            // xor this value to add it to the set
            return value ^ v;
        }
    }

    public override string ToString()
    {
        return $"0x{Value:X16}";
    }

    public static ArchetypeHash Create<T>(T componentIds)
        where T : class, IEnumerable<ComponentID>
    {
        var h = new ArchetypeHash();
        foreach (var componentId in componentIds)
            h = h.Toggle(componentId);
        return h;
    }

    internal static ArchetypeHash Create(OrderedListSet<ComponentID> componentIds)
    {
        long l = 0;
        foreach (var componentId in componentIds)
            l = Toggle(l, componentId);

        return new ArchetypeHash { Value = l };
    }
}