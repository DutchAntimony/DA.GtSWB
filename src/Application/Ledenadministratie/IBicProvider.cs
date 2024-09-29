using DA.GtSWB.Common.Types;

namespace DA.GtSWB.Application.Ledenadministratie;

public interface IBicProvider
{
    Result<string> TryFindBic(Iban iban, string? bic);
}

public class BicProvider : IBicProvider
{
    public Result<string> TryFindBic(Iban iban, string? bic)
    {
        return "INGBNL2U";
    }
}