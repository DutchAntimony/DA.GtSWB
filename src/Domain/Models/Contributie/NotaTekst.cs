using DA.GtSWB.Common.Types.IDs;

namespace DA.GtSWB.Domain.Models.Contributie;

public record NotaTekst
{
    public NotaTekstId Id { get; init; }
    public required string Aanhef { get; init; }
    public required string Inleiding { get; init; }
    public required string Afsluiting { get; init; }
    public required string Afzender { get; init; }
}
