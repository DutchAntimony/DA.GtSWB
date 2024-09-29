using DA.GtSWB.Common.Types;

namespace DA.GtSWB.Domain.Models.Contributie.NotaOpdrachtRegels;

public class InformatieRegel : OpdrachtRegel
{
    public override required Money Bedrag { get; init; } = Money.Zero;
}

