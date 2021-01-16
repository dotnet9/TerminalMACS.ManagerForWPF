using Quartz;
using Quartz.Impl;
using System;
using System.Threading.Tasks;

namespace QuartzNETTest
{
	class Program
	{
		static void Main(string[] args)
		{
			Task.Run(async () => await TestJob());
			Console.ReadKey();
		}

		static async Task TestJob()
		{

			// 创建任务
			IJobDetail jobDetail = JobBuilder.Create<TestJob>()
				.WithIdentity("TestJob", "Test")
				.Build();

			// 触发条件
			ITrigger trigger = TriggerBuilder.Create()
				.WithIdentity("TestJobTrigger", "Test")
				.WithSimpleSchedule(x =>
				{
					x.WithIntervalInSeconds(3).RepeatForever();
				})
				.Build();

			// 启动任务
			StdSchedulerFactory stdSchedulerFactory = new StdSchedulerFactory();
			IScheduler scheduler = await stdSchedulerFactory.GetScheduler();
			await scheduler.Start();
			await scheduler.ScheduleJob(jobDetail, trigger);
		}
	}
}
