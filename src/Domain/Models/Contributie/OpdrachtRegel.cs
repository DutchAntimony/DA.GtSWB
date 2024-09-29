using DA.GtSWB.Common.Types;
using DA.GtSWB.Common.Types.IDs;

namespace DA.GtSWB.Domain.Models.Contributie;

public abstract class OpdrachtRegel
{
    public required OpdrachtRegelId Id { get; init; }
    public BetaalOpdracht BetaalOpdracht { get; init; } = null!;
    public required string Omschrijving { get; init; }
    public virtual required Money Bedrag { get; init; }
}