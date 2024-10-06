using DA.GtSWB.Domain.Models.Ledenadministratie;
using DA.Options.Extensions;
using DA.GtSWB.Common.Types.Enums;
using DA.Options;

namespace DA.GtSWB.Tests.DataMigration.AuroraDtos;

public record AuroraLidDto
{
    public int Id { get; init; }
    public int Lidnummer { get; init; }
    public string? Voornaam { get; init; }
    public string? Tussenvoegsel { get; init; }
    public string? Achternaam { get; init; }
    public int FamilieId { get; init; }
    public int GeslachtId { get; init; }
    public DateTime Geboortedatum { get; init; }
    public int BetaalwijzeId { get; init; }
    public int UitschrijfredenId { get; init; }
    public DateTime WijzigingsDatum { get; init; }
    public Personalia ToPersonalia()
    {
        return Personalia.Create(Voornaam!, string.IsNullOrEmpty(Tussenvoegsel) ? Option.None : Tussenvoegsel.AsOption(), Achternaam!,
            GeslachtId == 1 ? Geslacht.Man : Geslacht.Vrouw,
            DateOnly.FromDateTime(Geboortedatum));
    }


}
