using LogDashboard;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace WPFWithLogDashboard;

public class Startup
{
    private ILogger logger;

    public ILogger MyLoger
    {
        get
        {
            if (logger == null) logger = Log.ForContext<Startup>();
            return logger;
        }
    }

    public void ConfigureServices(IServiceCollection services)
    {
        MyLoger.Information("ConfigureServices");

        services.AddLogDashboard();
        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        MyLoger.Information("Configure");

        app.UseLogDashboard();

        app.UseRouting();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}