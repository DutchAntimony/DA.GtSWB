using DA.GtSWB.Common.Types;
using DA.GtSWB.Common.Types.IDs;

namespace DA.GtSWB.Domain.Models.Contributie.NotaOpdrachtRegels;

public record KostenRegel : OpdrachtRegel
{
    public required int Iteratie { get; init; }

    public static KostenRegel Create(string omschrijving, Money bedrag, int iteratie)
    {
        return new KostenRegel()
        {
            Id = OpdrachtRegelId.Create(),
            Bedrag = bedrag,
            Omschrijving = omschrijving,
            Iteratie = iteratie
        };
    }
} 

