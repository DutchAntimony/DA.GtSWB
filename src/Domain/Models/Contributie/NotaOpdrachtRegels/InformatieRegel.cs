using DA.GtSWB.Common.Types;
using DA.GtSWB.Common.Types.IDs;

namespace DA.GtSWB.Domain.Models.Contributie.NotaOpdrachtRegels;

public record InformatieRegel : OpdrachtRegel
{
    public override required Money Bedrag { get; init; } = Money.Zero;

    public static InformatieRegel Create(string omschrijving)
    {
        return new InformatieRegel()
        {
            Id = OpdrachtRegelId.Create(),
            Omschrijving = omschrijving,
            Bedrag = Money.Zero
        };
    }
}

