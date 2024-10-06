using DA.GtSWB.Common.Types;
using DA.GtSWB.Common.Types.Enums;
using DA.GtSWB.Common.Types.IDs;

namespace DA.GtSWB.Domain.Models.Ledenadministratie;

public class Bankrekening
{
    public required BankrekeningId Id { get; init; }
    public required Iban Iban { get; init; }
    public required string Bic { get; init; }
    public required string TenNameVan { get; init; }
    public DateOnly? MandaadDatum { get; init; }
    public MandaadType MandaadType { get; init; } = MandaadType.Geen;
    private Bankrekening() { }

    public static Bankrekening Create(Iban iban, string bic, string tenNameVan, DateOnly? mandaadDatum, MandaadType type)
    {
        return new Bankrekening()
        {
            Id = BankrekeningId.Create(),
            Iban = iban,
            Bic = bic,
            TenNameVan = tenNameVan,
            MandaadDatum = mandaadDatum,
            MandaadType = type
        };
    }
}
