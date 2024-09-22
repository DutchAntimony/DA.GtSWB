namespace DA.GtSWB.Application.Ledenadministratie.Adressen;

public record AdresDto
{
    public string Straat { get; init; } = string.Empty;
    public string Huisnummer { get; init; } = string.Empty;
    public string Postcode { get; init; } = string.Empty;
    public string Woonplaats { get; init; } = string.Empty;
    public string? Land { get; init; }

    internal bool IsSanitized { get; init; } = false;
}
