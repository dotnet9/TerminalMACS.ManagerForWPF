using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzNETTest
{
	public class TestJob : IJob
	{
		public async Task Execute(IJobExecutionContext context)
		{
			await Console.Out.WriteLineAsync(DateTime.Now.ToString());
		}
	}
}
