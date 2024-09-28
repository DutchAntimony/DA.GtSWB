using DA.GtSWB.Common.Types;
using DA.GtSWB.Common.Types.IDs;

namespace DA.GtSWB.Domain.Models.Ledenadministratie;

public class Bankrekening
{
    public required BankrekeningId Id { get; init; }
    public required Iban Iban { get; init; }
    public required string Bic { get; init; }
    public required string TenNameVan { get; init; }

    private Bankrekening() { }

    public static Bankrekening Create(Iban iban, string bic, string tenNameVan)
    {
        return new Bankrekening()
        {
            Id = BankrekeningId.Create(),
            Iban = iban,
            Bic = bic,
            TenNameVan = tenNameVan
        };
    }
}
