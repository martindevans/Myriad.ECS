using System.Runtime.CompilerServices;

namespace Myriad.ECS.Execution;

public readonly struct ExecutionSchedule
    : IDisposable
{
    private readonly uint _epoch;
    private readonly ExecutionScheduleState _state;

    public static ExecutionSchedule Create()
    {
        return new ExecutionSchedule(ExecutionScheduleState.Get());
    }

    private ExecutionSchedule(ExecutionScheduleState state)
    {
        _state = state;
        _epoch = state.Epoch;
    }

    public JobBuilder CreateJob()
    {
        CheckEpoch();
        return new JobBuilder();
    }

    public void Dispose()
    {
        CheckEpoch();
        _state.Return();
    }

    private void CheckEpoch([CallerMemberName] string name = "")
    {
        if (_state == null || _epoch != _state.Epoch)
            throw new InvalidOperationException($"Cannot '{name}' - invalid/stale ExecutionContext state");
    }
}

internal class ExecutionScheduleState
{
    public uint Epoch { get; }

    private ExecutionScheduleState()
    {
        
    }

    public void Return()
    {
    }

    public static ExecutionScheduleState Get()
    {
        return new ExecutionScheduleState();
    }
}