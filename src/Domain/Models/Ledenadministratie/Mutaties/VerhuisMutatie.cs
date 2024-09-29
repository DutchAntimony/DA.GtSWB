using DA.GtSWB.Common.Types.IDs;

namespace DA.GtSWB.Domain.Models.Ledenadministratie.Mutaties;

public record VerhuisMutatie : LidMutatie
{
    public Adres? VorigAdres { get; init; } = null;
    public Adres? NieuwAdres { get; init; } = null;

    public static VerhuisMutatie Create(Lid lid, Option<Adres> nieuwAdres, DateTime date, string gebruiker) =>
        new()
        {
            Id = LidMutatieId.Create(),
            Lid = lid,
            VorigAdres = lid.Adres.AsNullable(),
            NieuwAdres = nieuwAdres.AsNullable(),
            WijzigingsDatum = date,
            Gebruiker = gebruiker
        };
}
