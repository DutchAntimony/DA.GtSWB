using DA.GtSWB.Domain.Models.Ledenadministratie;

namespace DA.GtSWB.Domain.Models.Contributie;

public class IncassoOpdracht : BetaalOpdracht
{
    public required Bankrekening Bankrekening { get; init; }

    public required string Omschrijving { get; init; }
    public int IncassoAankondigingsDagen { get; init; } = 10;
}
