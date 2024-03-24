using BenchmarkDotNet.Running;
using Benchmarks;

//var e = new EntityCreateBenchmark();
//e.Setup();
//e.CreateBuffered();

//var summary = BenchmarkRunner.Run<QueryBenchmark>();
var summary = BenchmarkRunner.Run<EntityCreateBenchmark>();

//var b = new QueryBenchmark();
//b.Setup();
//for (var i = 0; i < 125; i++)
//    b.QueryEnumerable();