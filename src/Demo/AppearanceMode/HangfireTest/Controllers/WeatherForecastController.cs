using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HangfireTest.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerBase
	{
		private static readonly string[] Summaries = new[]
		{
	  "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
	};

		private readonly ILogger<WeatherForecastController> _logger;

		public WeatherForecastController(ILogger<WeatherForecastController> logger)
		{
			_logger = logger;
		}

		[HttpGet]
		public IEnumerable<WeatherForecast> Get()
		{
			var rng = new Random();
			return Enumerable.Range(1, 5).Select(index => new WeatherForecast
			{
				Date = DateTime.Now.AddDays(index),
				TemperatureC = rng.Next(-20, 55),
				Summary = Summaries[rng.Next(Summaries.Length)]
			})
			.ToArray();
		}
	}

	public abstract class TestHangfireBase
	{
		public string Name { get; set; }
		public TestHangfireBase(string name)
		{
			Name = name;
			Console.WriteLine($"基类{DateTime.Now.ToString("HH:mm:ss")}");
			//Name = "我是爹地";
		}
		public void Open()
		{
			Console.WriteLine("注册hangfire任务");
			RecurringJob.AddOrUpdate(() => HangfireTaskEveryMinute(this.Name), Cron.Minutely());
			BackgroundJob.Enqueue(() => HangfireTaskOneTime());
		}

		public static void HangfireTaskEveryMinute(string Name)
		{
			Console.WriteLine($"[{Name}]每分钟触发，控制器内测试Hangfire" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
		}


		public static void HangfireTaskOneTime()
		{
			Console.WriteLine("只执行一次：控制器内测试Hangfire" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
		}
	}

	public class TestHangfireTask : TestHangfireBase
	{
		public TestHangfireTask(string name):base(name)
		{
			Console.WriteLine($"子类{DateTime.Now.ToString("HH:mm:ss")}");
		}
	}
}
