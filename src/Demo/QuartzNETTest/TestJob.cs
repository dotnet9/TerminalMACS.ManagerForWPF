using System;
using System.Threading.Tasks;
using Quartz;

namespace QuartzNETTest;

public class TestJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await Console.Out.WriteLineAsync(DateTime.Now.ToString());
    }
}