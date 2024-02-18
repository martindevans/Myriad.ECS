using Myriad.ECS.Worlds.Archetypes;
using Myriad.ParallelTasks;

namespace Myriad.ECS.Execution;

public class JobBuilder
{
    public JobBuilder DependsOn(Future predecessor)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// This job requires read access of the given component in the given archetype
    /// </summary>
    /// <typeparam name="T0"></typeparam>
    /// <param name="archetype"></param>
    /// <returns></returns>
    public JobBuilder WithReadAccess<T0>(Archetype archetype)
        where T0 : IComponent
    {
        return this;
    }

    /// <summary>
    /// This job requires write access of the given component in the given archetype
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <param name="archetype"></param>
    /// <returns></returns>
    public JobBuilder WithWriteAccess<T1>(Archetype archetype)
        where T1 : IComponent
    {
        return this;
    }

    public JobBuilder WithReadAll()
    {
        return this;
    }

    public JobBuilder WithWriteAll()
    {
        return this;
    }

    public Future Build()
    {
        return new Future();
    }

    public Future<T> Build<T>(T result)
    {
        return new Future<T>(result);
    }
}