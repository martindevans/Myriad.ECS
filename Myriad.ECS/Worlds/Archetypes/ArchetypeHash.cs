using System.Runtime.InteropServices;
using Myriad.ECS.Collections;
using Myriad.ECS.IDs;
using Standart.Hash.xxHash;

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
        unsafe
        {
            // Hash compoent value to smear bits across 64 bit hash space
            var cv = component.Value;
            var v = unchecked((long)xxHash64.ComputeHash(new Span<byte>(&cv, 4), 4));

            // xor this value to tiggle it in the set
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