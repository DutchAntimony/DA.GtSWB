using DA.GtSWB.Common.Types.IDs;

namespace DA.GtSWB.Domain.Models.Ledenadministratie;

public class Adres
{
    private readonly string? _land;

    public AdresId Id { get; init; } = AdresId.Empty;
    public required string Straat { get; init; }
    public required string Huisnummer { get; init; }
    public required string Postcode { get; init; }
    public required string Woonplaats { get; init; }
    public Option<string> Land
    {
        get => _land.AsOption();
        init => _land = value.AsNullable();
    }

    private Adres() { } // used by ORM and static factory methods

    public static Adres Create(string straat, string huisnummer, string postcode, string woonplaats, Option<string> land)
    {
        return new()
        {
            Id = AdresId.Create(),
            Straat = straat,
            Huisnummer = huisnummer,
            Postcode = postcode,
            Woonplaats = woonplaats,
            Land = land
        };
    }

    public static Adres Empty => new() { Straat = "", Huisnummer = "", Postcode = "", Woonplaats = "" };
}
