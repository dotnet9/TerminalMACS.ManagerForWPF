using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

namespace ByteTestFX47.Benchmark;

public class BenchmarkConfig : ManualConfig
{
    public BenchmarkConfig()
    {
        AddJob(Job.Default
            .WithWarmupCount(0) // 不进行预热
            .WithIterationCount(1)); // 只运行一次
    }
}