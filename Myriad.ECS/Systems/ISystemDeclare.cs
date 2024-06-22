using Myriad.ECS.Collections;
using Myriad.ECS.IDs;

namespace Myriad.ECS.Systems;

/// <summary>
/// A system which declares which components is access in the update phase
/// </summary>
/// <typeparam name="TData"></typeparam>
public interface ISystemDeclare<in TData>
    : ISystem<TData>
{
    /// <summary>
    /// Declare which components this system accesses during the update phase
    /// </summary>
    /// <param name="declaration"></param>
    void Declare(ref SystemDeclaration declaration);
}

/// <summary>
/// A declaration of what components a system accesses during update
/// </summary>
public struct SystemDeclaration
{
    private ComponentBloomFilter _reads;
    private ComponentBloomFilter _writes;

    /// <summary>
    /// Declare that the given component is read during the update phase
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void Read<T>()
        where T : IComponent
    {
        _reads.Add(ComponentID<T>.ID);
    }

    /// <summary>
    /// Declare that the given component is read during the update phase
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void Write<T>()
        where T : IComponent
    {
        _writes.Add(ComponentID<T>.ID);
    }

    internal readonly bool Intersects(ref readonly SystemDeclaration other)
    {
        // Cannot read something that is being written
        if (_reads.Intersects(in other._writes))
            return true;

        // Cannot write something that is being read or written
        if (_writes.Intersects(in other._reads))
            return true;
        if (_writes.Intersects(in other._writes))
            return true;

        return false;
    }

    internal void Union(ref readonly SystemDeclaration other)
    {
        _reads.Union(in other._reads);
        _writes.Union(in other._writes);
    }
}

/// <summary>
/// A system group which runs systems serially, but requires that they are declarative systems. The group itself is
/// a declarative system, which is the union of all declarations.
/// </summary>
/// <typeparam name="TData"></typeparam>
/// <param name="name"></param>
/// <param name="systems"></param>
public sealed class DeclareSystemGroup<TData>(string name, params ISystemDeclare<TData>[] systems)
    // ReSharper disable once CoVariantArrayConversion
    : BaseSystemGroup<TData>(name, systems), ISystemDeclare<TData>
{
    protected override void BeforeUpdateInternal(List<SystemGroupItem<TData>> systems, TData data)
    {
        foreach (var item in systems)
            item.BeforeUpdate(data);
    }

    protected override void UpdateInternal(List<SystemGroupItem<TData>> systems, TData data)
    {
        foreach (var item in systems)
            item.Update(data);
    }

    protected override void AfterUpdateInternal(List<SystemGroupItem<TData>> systems, TData data)
    {
        foreach (var item in systems)
            item.AfterUpdate(data);
    }

    public void Declare(ref SystemDeclaration declaration)
    {
        for (var i = 0; i < Systems.Count; i++)
            ((ISystemDeclare<TData>)Systems[i].System).Declare(ref declaration);
    }
}

/// <summary>
/// Run systems in parallel where possible, based on declarations of what components they access. Systems will be
/// run in no particular order, the only guarantee is systems will not run in parallel if they read/write or
/// write/write the same component(s).
/// </summary>
/// <typeparam name="TData"></typeparam>
public sealed class PhasedParallelSystemGroup<TData>
    : BaseSystemGroup<TData>
{
    private SystemDeclaration[] _filters = [ ];

    private Task[] _phase = [ ];
    private readonly List<int> _todo = [ ];

    /// <summary>
    /// Get the number of "phases" which were generated last frame. Each phase runs a number of systems in parallel, but the
    /// phases themselves are run sequentially. Less phases means more parallelism.
    /// </summary>
    public int Phases { get; private set; }

    public PhasedParallelSystemGroup(string name, params ISystemDeclare<TData>[] systems)
        // ReSharper disable once CoVariantArrayConversion
        : base(name, systems)
    {
    }

    protected override void BeforeUpdateInternal(List<SystemGroupItem<TData>> systems, TData data)
    {
        foreach (var item in systems)
            item.BeforeUpdate(data);
    }

    protected override void UpdateInternal(List<SystemGroupItem<TData>> systems, TData data)
    {
        // Ensure arrays are large enough
        if (systems.Count > _filters.Length)
            _filters = new SystemDeclaration[systems.Count];
        if (systems.Count > _phase.Length)
            _phase = new Task[systems.Count];

        // Gather declarations from every system about what it will access
        for (var i = 0; i < systems.Count; i++)
        {
            var decl = (ISystemDeclare<TData>)systems[i].System;
            decl.Declare(ref _filters[i]);
            _todo.Add(i);
        }

        // Keep running work phases while possible
        var phaseCount = 0;
        while (_todo.Count > 0)
        {
            phaseCount++;
            var phaseIdx = 0;
            Array.Fill(_phase, Task.CompletedTask);

            // Find and run all the systems that do not conflict
            var phaseDecl = new SystemDeclaration();
            for (var i = _todo.Count - 1; i >= 0; i--)
            {
                var idx = _todo[i];
                ref var filter = ref _filters[idx];

                // Cannot schedule this system if it overlaps with something else in the phase
                if (filter.Intersects(ref phaseDecl))
                    continue;

                // Union into the phase set
                phaseDecl.Union(ref filter);

                // Start a task running this system
                var sys = systems[idx];
                _phase[phaseIdx++] = Task.Run(() => sys.Update(data));

                // Remove it from remaining work to do
                _todo.RemoveAt(i);
            }

            // Wait for phase to finish
            Task.WaitAll(_phase);
        }

        Phases = phaseCount;
    }

    protected override void AfterUpdateInternal(List<SystemGroupItem<TData>> systems, TData data)
    {
        foreach (var item in systems)
            item.AfterUpdate(data);
    }
}

/// <summary>
/// Execute systems in order, with parallism. Systems which reads a component waits for earlier systems which write
/// that component. Systems which write a component wait for earlier systems which write or read a component.
/// </summary>
/// <typeparam name="TData"></typeparam>
public sealed class OrderedParallelSystemGroup<TData>
    : BaseSystemGroup<TData>
{
    private SystemDeclaration[] _filters = [ ];
    private Task?[] _running = [ ];
    private int[] _depth = [ ];

    /// <summary>
    /// Get the length of the longest dependency chain
    /// </summary>
    public int MaxDependencyChain { get; private set; }

    public OrderedParallelSystemGroup(string name, params ISystemDeclare<TData>[] systems)
        // ReSharper disable once CoVariantArrayConversion
        : base(name, systems)
    {
    }

    protected override void BeforeUpdateInternal(List<SystemGroupItem<TData>> systems, TData data)
    {
        foreach (var item in systems)
            item.BeforeUpdate(data);
    }

    protected override void UpdateInternal(List<SystemGroupItem<TData>> systems, TData data)
    {
        // Ensure arrays are large enough
        if (systems.Count > _filters.Length)
            _filters = new SystemDeclaration[systems.Count];
        if (systems.Count > _running.Length)
            _running = new Task[systems.Count];
        if (systems.Count > _depth.Length)
            _depth = new int[systems.Count];

        // Gather declarations from every system about what it will access
        for (var i = 0; i < systems.Count; i++)
        {
            var decl = (ISystemDeclare<TData>)systems[i].System;
            decl.Declare(ref _filters[i]);
        }

        // Start the first system running
        Array.Clear(_running, 0, _running.Length);
        var sys = systems[0];
        _running[0] = Task.Run(() => sys.Update(data));
        _depth[0] = 0;

        // Run all other systems
        for (var i = 1; i < systems.Count; i++)
            _running[i] = UpdateSystemAsync(i, systems[i], data);

        // Find the longest chain length
        MaxDependencyChain = 0;
        for (var i = 0; i < _depth.Length; i++)
            MaxDependencyChain = Math.Max(MaxDependencyChain, _depth[i]);

        // Wait for all systems to finish
        foreach (var task in _running)
            task?.Wait();
    }

    private async Task UpdateSystemAsync(int index, SystemGroupItem<TData> system, TData data)
    {
        // Wait for any previous tasks which this one depends on
        var depth = 1;
        for (var i = 0; i < index; i++)
        {
            if (_filters[i].Intersects(ref _filters[index]))
            {
                await (_running[i] ?? Task.CompletedTask).ConfigureAwait(false);
                depth = Math.Max(_depth[i] + 1, depth);
            }
        }
        _depth[index] = depth;

        await Task.Run(() => system.Update(data)).ConfigureAwait(false);
    }

    protected override void AfterUpdateInternal(List<SystemGroupItem<TData>> systems, TData data)
    {
        foreach (var item in systems)
            item.AfterUpdate(data);
    }
}