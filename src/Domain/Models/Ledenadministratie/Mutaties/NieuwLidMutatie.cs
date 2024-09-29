using DA.GtSWB.Common.Types.IDs;

namespace DA.GtSWB.Domain.Models.Ledenadministratie.Mutaties;

public record NieuwLidMutatie : LidMutatie
{
    public Personalia Personalia { get; init; } = null!;
    public Adres? Adres { get; init; } = null;
    public Betaalwijze? Betaalwijze { get; init; } = null;

    public static NieuwLidMutatie Create(Lid lid, DateTime date, string gebruiker) =>
        new()
        {
            Id = LidMutatieId.Create(),
            Lid = lid,
            Personalia = lid.Personalia,
            Adres = lid.AdresDataDatabase,
            Betaalwijze = lid.BetaalwijzeDataDatabase,
            WijzigingsDatum = date,
            Gebruiker = gebruiker
        };
}
