using System;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace QuartzNETTest;

internal class Program
{
    private static void Main(string[] args)
    {
        Task.Run(async () => await TestJob());
        Console.ReadKey();
    }

    private static async Task TestJob()
    {
        // 创建任务
        var jobDetail = JobBuilder.Create<TestJob>()
            .WithIdentity("TestJob", "Test")
            .Build();

        // 触发条件
        var trigger = TriggerBuilder.Create()
            .WithIdentity("TestJobTrigger", "Test")
            .WithSimpleSchedule(x => { x.WithIntervalInSeconds(3).RepeatForever(); })
            .Build();

        // 启动任务
        var stdSchedulerFactory = new StdSchedulerFactory();
        var scheduler = await stdSchedulerFactory.GetScheduler();
        await scheduler.Start();
        await scheduler.ScheduleJob(jobDetail, trigger);
    }
}