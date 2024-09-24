using DA.GtSWB.Application.Common;
using DA.GtSWB.Application.Ledenadministratie.Adressen;
using DA.GtSWB.Application.Ledenadministratie.Commands;
using DA.GtSWB.Application.Ledenadministratie.Personen;
using DA.GtSWB.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace DA.GtSWB.Ui.Cli;

internal class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        var host = DependencyInjection.InitializeHost();
        await host.StartAsync();

        InitializeDatabase(host.Services);

        var sender = host.Services.GetRequiredService<ISender>();
        await DoWork(sender);

        await host.StopAsync();
    }

    private static void InitializeDatabase(IServiceProvider services)
    {
        using var serviceScope = services.CreateScope();
        using var dbContext = serviceScope.ServiceProvider.GetRequiredService<LedenAdministratieDbContext>();
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
    }

    private static async Task DoWork(ISender sender)
    {
        var personaliaDto1 = new PersonaliaDto() { Voorletters = "A", Achternaam = "Achternaam", GeslachtInput = "M", Geboortedatum = new DateOnly(2000, 1, 1) };
        var personaliaDto2 = new PersonaliaDto() { Voorletters = string.Empty, Achternaam = "Naamachter", GeslachtInput = "V", Geboortedatum = new DateOnly(1980, 2, 3) };
        var adresDto = new AdresDto() { Straat = "Straat", Huisnummer = "2", Postcode = "1234AB", Woonplaats = "Plaats" };
        var metadata = new RequestMetadata(DateTime.Now, "Ui.Cli");

        var request = new CreateFamilie.Command(adresDto, [personaliaDto1, personaliaDto2], metadata);

        var result = await sender.Send(request);
        Debugger.Break();
    }
}
