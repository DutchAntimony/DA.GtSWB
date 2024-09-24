using DA.GtSWB.Application;
using DA.GtSWB.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;

namespace DA.GtSWB.Tests.IntegrationTests;
internal static class DependencyInjection
{
    public static IServiceProvider ConfigureServiceProvider(this IServiceCollection services)
    {
        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile("secrets.json", optional: true); // The secrets file

        IConfiguration configuration = configurationBuilder.Build();

        var baseDir = configuration.GetValue<string>("BaseDir");

        services.AddSingleton(configuration);
        services.RegisterApplication();
        services.RegisterDatabase(configuration);

        var loggerConfiguration = new LoggerConfiguration()
                .WriteTo.File(Path.Join(baseDir, "Log", "log.txt"), fileSizeLimitBytes: 1048576, rollOnFileSizeLimit: true)
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning);

        var serilogLoggerFactory = new SerilogLoggerFactory(loggerConfiguration.CreateLogger());
        services.AddSingleton<ILoggerFactory>(serilogLoggerFactory);
        services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

        return services.BuildServiceProvider();
    }
}
