using DA.GtSWB.Common.Formatters;
using DA.GtSWB.Common.Types;
using DA.GtSWB.Common.Types.IDs;
using DA.GtSWB.Domain.Models.Ledenadministratie;

namespace DA.GtSWB.Domain.Models.Contributie.NotaOpdrachtRegels;

public record ContributieRegel : OpdrachtRegel
{
    public required Lid Lid { get; init; }

    public static ContributieRegel Create(Lid lid, LidFormatter formatter, Money contributie)
    {
        return new ContributieRegel()
        {
            Id = OpdrachtRegelId.Create(),
            Bedrag = contributie,
            Omschrijving = formatter(lid),
            Lid = lid
        };
    }
}

