using BenchmarkDotNet.Running;
using Benchmarks;

//var summary = BenchmarkRunner.Run<EntityChurnBenchmark>();


var b = new EntityChurnBenchmark();
b.Setup();
b.Churn();