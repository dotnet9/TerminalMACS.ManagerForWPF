using BenchmarkDotNet.Running;
using ByteTest.Core.Test;

// 运行基准测试，.net 7\8有问题
BenchmarkRunner.Run<BenchmarkTest>();

// 普通测试
//BenchmarkTest.Test();