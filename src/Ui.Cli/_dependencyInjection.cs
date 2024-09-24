using DA.GtSWB.Application;
using DA.GtSWB.Infrastructure;
using DA.GtSWB.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace DA.GtSWB.Ui.Cli;

internal static class DependencyInjection
{
    public static IHost InitializeHost()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile("secrets.json", optional: true, reloadOnChange: true)
            .Build();

        var baseDir = configuration.GetValue<string>("BaseDir");

        var host = Host
            .CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.RegisterApplication();
                services.RegisterInfraServices();
                services.RegisterDatabase(configuration);
            })
            .UseSerilog((host, config) => config
                .WriteTo.File(Path.Join(baseDir, "Log", "log.txt"), fileSizeLimitBytes: 1048576, rollOnFileSizeLimit: true)
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            )
            .Build();

        return host;
    }
}
