using DA.GtSWB.Common.Types.IDs;
using DA.GtSWB.Domain.Models.Ledenadministratie;
using DA.GtSWB.Domain.Models.Ledenadministratie.Betaalwijzes;

namespace DA.GtSWB.Domain.Models.Contributie;

public class IncassoOpdracht : BetaalOpdracht
{
    public required Bankrekening Bankrekening { get; init; }
    public required string Omschrijving { get; init; }
    public int IncassoAankondigingsDagen { get; init; } = 10;

    public static IncassoOpdracht Create(IncassoBetaalwijze incassoBetaalwijze, string omschrijving, IEnumerable<OpdrachtRegel> opdrachtRegels, DateTime timestamp)
    {
        incassoBetaalwijze.VerantwoordelijkLid.VerifyNotNull();

        var opdracht = new IncassoOpdracht()
        {
            Id = BetaalOpdrachtId.Create(),
            AanmaakDatum = timestamp,
            Ident = incassoBetaalwijze.VerantwoordelijkLid!.Lidnummer,
            Bankrekening = incassoBetaalwijze.Bankrekening,
            Omschrijving = omschrijving
        };

        opdracht.OpdrachtRegelCollectie.AddRange(opdrachtRegels);

        return opdracht;
    }
}
