using BenchmarkDotNet.Running;
using ByteTest.Core.Test;

// 运行基准测试
BenchmarkRunner.Run<BenchmarkTest>();

// 普通测试
//BenchmarkTest.Test();