using DA.GtSWB.Common.Types.Enums;
using DA.GtSWB.Common.Types.IDs;

namespace DA.GtSWB.Domain.Models.Ledenadministratie;

public class Personalia
{
    private readonly string? _tussenvoegsel;

    public PersonaliaId Id { get; init; } = PersonaliaId.Empty;
    public required string Voorletters { get; init; }
    public Option<string> Tussenvoegsel
    {
        get => _tussenvoegsel.AsOption();
        init => _tussenvoegsel = value.AsNullable();
    }
    public required string Achternaam { get; init; }
    public required Geslacht Geslacht { get; init; }
    public required DateOnly Geboortedatum { get; init; }

    private Personalia() { } // used by ORM

    public static Personalia Create(string voorletters, Option<string> tussenvoegsel, string achternaam, Geslacht geslacht, DateOnly geboortedatum)
    {
        return new()
        {
            Id = PersonaliaId.Create(),
            Voorletters = voorletters,
            Tussenvoegsel = tussenvoegsel,
            Achternaam = achternaam,
            Geslacht = geslacht,
            Geboortedatum = geboortedatum
        };
    }
}

