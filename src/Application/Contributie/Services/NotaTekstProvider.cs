using DA.GtSWB.Common.Types;
using DA.GtSWB.Common.Types.IDs;
using DA.GtSWB.Domain.Models.Contributie;
using DA.GtSWB.Domain.ServiceDefinitions;

namespace DA.GtSWB.Application.Contributie.Services;

internal class NotaTekstProvider(IConfiguratieService configuratieService) 
    : INotaTekstProvider
{
    private readonly Lazy<string> _titel = new(() => 
        configuratieService.GetConfiguratieOrFallback("Nota.Titel", "[Titel]"));

    private readonly Lazy<string> _aanhef = new(() =>
        configuratieService.GetConfiguratieOrFallback("Nota.Aanhef", "Geachte "));

    private readonly Lazy<string> _inleiding = new(() =>
        configuratieService.GetConfiguratieOrFallback("Nota.Inleiding", "[Inleiding]"));

    private readonly Lazy<string> _afsluiting = new(() =>
        configuratieService.GetConfiguratieOrFallback("Nota.Afsluiting", "[Afsluiting]"));

    private readonly Lazy<string> _afzender = new(() =>
        configuratieService.GetConfiguratieOrFallback("Nota.Afzender", "Mevr. A. Achternaam\nStraat 123\n1234AB Woonplaats\nT: 0123-456789\nE: test@test.com"));

    private readonly Lazy<string> _afzenderPlaats = new(() =>
        configuratieService.GetConfiguratieOrFallback("Nota.Afzender.Woonplaats", "Wjelsryp"));

    private readonly Lazy<int> _betaaltermijn = new(() =>
        configuratieService.GetConfiguratieOrFallback("Nota.Betaaltermijn", 14));

    private readonly Lazy<Iban> _iban = new(() =>
        configuratieService.GetConfiguratieOrFallback("Vereniging.Iban", new Iban("NL00RABO0000000000")));

    private readonly Lazy<string> _tenNameVan = new(() =>
        configuratieService.GetConfiguratieOrFallback("Vereniging.TenNameVan", "[TenNameVan]"));

    public NotaTekst GetVoorContributieNota(DateOnly verzenddatum)
    {
        return new NotaTekst()
        {
            Id = NotaTekstId.Create(),
            Titel = _titel.Value,
            DatumPlaats = $"{_afzenderPlaats.Value}, {verzenddatum:dd-MM-yyyy}",
            Aanhef = _aanhef.Value,
            Inleiding = _inleiding.Value,
            Afsluiting = _afsluiting.Value
                .Replace("{Iban}", _iban.Value.ToString())
                .Replace("{TenNameVan}", _tenNameVan.Value)
                .Replace("{Betaaltermijn}", _betaaltermijn.Value.ToString()),
            Afzender = _afzender.Value
        };
    }
}