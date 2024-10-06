using DA.GtSWB.Domain.Models.Ledenadministratie;
using DA.Options;

namespace DA.GtSWB.Tests.DataMigration.AuroraDtos;

public record AuroraAdresDto
{
    public int Id { get; init; }
    public string? Straat { get; init; }
    public int Huisnummer { get; init; }
    public string? Toevoegsel { get; init; }
    public string? Postcode { get; init; }
    public string? Woonplaats { get; init; }
    public string? Notities { get; init; }

    public Adres ToAdres()
    {
        return Adres.Create(Straat!, $"{Huisnummer} {Toevoegsel}", Postcode!.Replace(" ", ""), Woonplaats!, Option.None);
    }
}
