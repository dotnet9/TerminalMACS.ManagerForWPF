using ByteTestFX47.Benchmark;

namespace ByteTestFX47
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 运行基准测试
            // BenchmarkRunner.Run<BenchmarkTest>();

            // 普通测试
            BenchmarkTest.Test();
        }
    }
}