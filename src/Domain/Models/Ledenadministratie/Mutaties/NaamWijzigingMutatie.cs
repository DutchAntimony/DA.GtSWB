using DA.GtSWB.Common.Types.IDs;

namespace DA.GtSWB.Domain.Models.Ledenadministratie.Mutaties;

public record NaamWijzigingMutatie : LidMutatie
{
    public Personalia VorigePersonalia { get; init; } = null!;
    public Personalia NieuwPersonalia { get; init; } = null!;

    public static NaamWijzigingMutatie Create(Lid lid, Personalia nieuwePersonalia, DateTime date, string gebruiker) =>
        new()
        {
            Id = LidMutatieId.Create(),
            Lid = lid,
            VorigePersonalia = lid.Personalia,
            NieuwPersonalia = nieuwePersonalia,
            WijzigingsDatum = date,
            Gebruiker = gebruiker
        };
}