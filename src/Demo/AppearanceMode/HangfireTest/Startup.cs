using Hangfire;
using Hangfire.MemoryStorage;
using HangfireTest.Services;
using LogDashboard;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace HangfireTest
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddLogDashboard();
      services.AddHangfire(x => x.UseStorage(new MemoryStorage()));

      services.AddControllers();
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "HangfireTest", Version = "v1" });
      });
      services.AddSingleton<ITestService, TestService>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      app.UseHangfireServer();
      app.UseHangfireDashboard();
      RecurringJob.AddOrUpdate(() => HangfireTask(), Cron.Minutely());

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HangfireTest v1"));
      }

      app.UseLogDashboard();

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }

    public void HangfireTask()
    {
      Console.WriteLine("测试Hangfire" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
    }
  }
}
