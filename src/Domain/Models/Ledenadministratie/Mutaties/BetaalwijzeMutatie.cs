using DA.GtSWB.Common.Types.IDs;

namespace DA.GtSWB.Domain.Models.Ledenadministratie.Mutaties;

public record BetaalwijzeMutatie : LidMutatie
{
    public Betaalwijze? VorigeBetaalwijze { get; init; } = null;

    public Betaalwijze? NieuweBetaalwijze { get; init; } = null;

    public static BetaalwijzeMutatie Create(Lid lid, Option<Betaalwijze> nieuweBetaalwijze, DateTime date, string gebruiker) =>
        new()
        {
            Id = LidMutatieId.Create(),
            VorigeBetaalwijze = lid.Betaalwijze.AsNullable(),
            NieuweBetaalwijze = nieuweBetaalwijze.AsNullable(),
            WijzigingsDatum = date,
            Gebruiker = gebruiker
        };
}