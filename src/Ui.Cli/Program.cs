using DA.GtSWB.Application.Common;
using DA.GtSWB.Application.Configuratie;
using DA.GtSWB.Application.Contributie.Commands;
using DA.GtSWB.Common.Extensions;
using DA.GtSWB.Domain.ServiceDefinitions;
using DA.GtSWB.Persistence;
using DataMigration;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DA.GtSWB.Ui.Cli;

internal class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        var host = DependencyInjection.InitializeHost();
        await host.StartAsync();

        var sender = host.Services.GetRequiredService<ISender>();
        var config = host.Services.GetRequiredService<IConfiguration>();
        var path = config.GetValue<string>("BaseDir").EnsureNotNull();

        //await Import(host);
        //await ReadSettings(host.Services.GetRequiredService<IUnitOfWork>());
        //await CreateContributieOpdrachten(sender);
        //await CreateIncassoFile(sender, path);
        await CreateNotaFiles(sender, path);
        await host.StopAsync();
    }

    private static async Task ReadSettings(IUnitOfWork unitOfWork)
    {
        var settings = ConfiguratieDataInserter.TeInsertenItems();
        unitOfWork.ConfiguratieItems.AddRange(settings);
        await unitOfWork.CommitAsync();
    }

    private static void InitializeDatabase(IServiceProvider services)
    {
        using var serviceScope = services.CreateScope();
        using var dbContext = serviceScope.ServiceProvider.GetRequiredService<LedenAdministratieDbContext>();
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
    }

    private static async Task Import(IHost host)
    {
        InitializeDatabase(host.Services);

        var connectionString = @"Data Source=C:\Users\romke\AppData\Roaming\Aurora\Data\2024-04-15T07.35.12.sqlite";
        var mandaadFile = @"C:\Users\romke\AppData\Roaming\Aurora\Data\incassoMandaad.csv";

        var dbContext = host.Services.GetRequiredService<LedenAdministratieDbContext>();
        await AuroraDataManager.SaveLeden(dbContext, connectionString, mandaadFile);
    }

    private static async Task CreateContributieOpdrachten(ISender sender)
    {
        var metadata = new RequestMetadata(DateTime.Now, "Ui.Cli");
        await sender.Send(new CreateContributieOpdracht.Command(metadata));
    }

    private static async Task CreateIncassoFile(ISender sender, string path)
    {
        var metadata = new RequestMetadata(DateTime.Now, "Ui.Cli");
        await sender.Send(new CreateIncassoFile.Command(path, metadata));
    }

    private static async Task CreateNotaFiles(ISender sender, string path)
    {
        var metadata = new RequestMetadata(DateTime.Now, "Ui.Cli");
        await sender.Send(new CreateNotaFiles.Command(path, metadata));
    }
}
