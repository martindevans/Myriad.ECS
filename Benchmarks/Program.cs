using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess.NoEmit;
using BenchmarkDotNet.Validators;
using Benchmarks;

//var e = new QueryBenchmark();
//e.Setup();
//e.ParallelQuery();

var summary = BenchmarkRunner.Run<QueryBenchmark>();
//var summary = BenchmarkRunner.Run<EntityCreateBenchmark>();
//var summary = BenchmarkRunner.Run<EntityModifyBenchmark>();
//var summary = BenchmarkRunner.Run<EntityChurnBenchmark>();

public class AntiVirusFriendlyConfig
    : ManualConfig
{
    public AntiVirusFriendlyConfig()
    {
        WithOptions(ConfigOptions.DisableOptimizationsValidator);

        AddJob(Job
            .MediumRun
            .WithToolchain(InProcessNoEmitToolchain.Instance)
        );
    }
}