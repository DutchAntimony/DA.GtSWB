using DA.GtSWB.Common.Types.IDs;

namespace DA.GtSWB.Domain.Models.Ledenadministratie;

public abstract record LidMutatie
{
    public required LidMutatieId Id { get; init; }
    public Lid Lid { get; init; } = null!;
    public required DateTime WijzigingsDatum { get; init; }
    public required string Gebruiker { get; init; }
}