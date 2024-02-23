using BenchmarkDotNet.Running;
using Benchmarks;

var summary = BenchmarkRunner.Run<QueryBenchmark>();
return;

var b = new QueryBenchmark();
b.Setup();
b.Query();