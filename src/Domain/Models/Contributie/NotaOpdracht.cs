using DA.GtSWB.Common.Types.Enums;
using DA.GtSWB.Common.Types.IDs;
using System.Collections.ObjectModel;

namespace DA.GtSWB.Domain.Models.Contributie;

public class NotaOpdracht : BetaalOpdracht
{
    public required NotaTekst NotaTekst { get; init; }
    public override required int Iteratie { get; init; }

    public static NotaOpdracht Create(IEnumerable<OpdrachtRegel> regels, NotaTekst tekst, DateTime aanmaakdatum)
    {
        return new NotaOpdracht()
        {
            Id = BetaalOpdrachtId.Create(),
            AanmaakDatum = aanmaakdatum,
            Status = BetaalStatus.Open,
            OpdrachtRegelCollectie = new ReadOnlyCollection<OpdrachtRegel>(regels.ToList()),
            NotaTekst = tekst,
            Iteratie = 1
        };
    }
}
