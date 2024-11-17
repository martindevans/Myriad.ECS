using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess.NoEmit;
using Benchmarks;

//var e = new QueryBenchmark();
//e.Setup();
//e.ParallelQuery();

//var c = new EntityCreateBenchmark();
//c.Setup();
//c.CreateBuffered();

//var summary = BenchmarkRunner.Run<QueryBenchmark>();
var summary = BenchmarkRunner.Run<EntityCreateBenchmark>();
//var summary = BenchmarkRunner.Run<EntityModifyBenchmark>();
//var summary = BenchmarkRunner.Run<EntityChurnBenchmark>();