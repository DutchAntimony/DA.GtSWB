using DA.GtSWB.Common.Types.Enums;
using DA.GtSWB.Common.Types.IDs;
using DA.GtSWB.Domain.Models.Ledenadministratie;

namespace DA.GtSWB.Domain.Models.Contributie;

public class NotaOpdracht : BetaalOpdracht
{
    public required NotaTekst NotaTekst { get; init; }
    public override required int Iteratie { get; init; }

    public static NotaOpdracht Create(Lid verantwoordelijkLid, IEnumerable<OpdrachtRegel> regels, NotaTekst tekst, DateTime aanmaakdatum)
    {
        return new NotaOpdracht()
        {
            Id = BetaalOpdrachtId.Create(),
            Ident = verantwoordelijkLid.Lidnummer,
            AanmaakDatum = aanmaakdatum,
            Status = BetaalStatus.Open,
            OpdrachtRegelCollectie = new List<OpdrachtRegel>(regels.ToList()),
            NotaTekst = tekst,
            Iteratie = 1
        };
    }
}
