﻿using BenchmarkDotNet.Running;
using Benchmarks;

//var e = new EntityModifyBenchmark();
//e.Setup();
//e.Playback();

//var summary = BenchmarkRunner.Run<QueryBenchmark>();
var summary = BenchmarkRunner.Run<EntityCreateBenchmark>();
//var summary = BenchmarkRunner.Run<EntityModifyBenchmark>();

//var b = new QueryBenchmark();
//b.Setup();
//for (var i = 0; i < 125; i++)
//    b.QueryEnumerable();