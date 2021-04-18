using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;
using System.Windows;

namespace WPFWithLogDashboard
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			#region Serilog配置

			string logOutputTemplate = "{Timestamp:HH:mm:ss.fff zzz} || {Level} || {SourceContext:l} || {Message} || {Exception} ||end {NewLine}";
			Log.Logger = new LoggerConfiguration()
			  .MinimumLevel.Debug()
			  .MinimumLevel.Override("Default", LogEventLevel.Information)
			  .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
			  .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
			  .Enrich.FromLogContext()
			  .WriteTo.File($"{AppContext.BaseDirectory}Logs/Dotnet9.log", rollingInterval: RollingInterval.Day, outputTemplate: logOutputTemplate)
			  .CreateLogger();

			#endregion

			Host.CreateDefaultBuilder(e.Args)
				.UseSerilog()
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				}).Build().RunAsync();
		}
	}
}
