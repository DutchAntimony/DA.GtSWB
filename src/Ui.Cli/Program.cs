using DA.GtSWB.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace DA.GtSWB.Ui.Cli;

internal class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        var host = DependencyInjection.InitializeHost();
        await host.StartAsync();

        InitializeDatabase(host.Services);

        await host.StopAsync();
    }

    private static void InitializeDatabase(IServiceProvider services)
    {
        using IServiceScope serviceScope = services.CreateScope();
        using LedenAdministratieDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LedenAdministratieDbContext>();
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
    }
}
