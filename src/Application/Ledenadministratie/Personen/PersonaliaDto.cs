namespace DA.GtSWB.Application.Ledenadministratie.Personen;

public record PersonaliaDto
{
    public string Voorletters { get; init; } = string.Empty;
    public string? Tussenvoegsel { get; init; }
    public string Achternaam { get; init; } = string.Empty;
    public string Geslacht { get; init; } = string.Empty;
    public DateOnly Geboortedatum { get; init; } = DateOnly.MinValue;

    internal bool IsSanitized { get; init; } = false;
}
