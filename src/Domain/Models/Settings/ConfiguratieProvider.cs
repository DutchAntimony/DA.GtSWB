using DA.GtSWB.Common.Types;
using DA.GtSWB.Domain.ServiceDefinitions;
using Microsoft.Extensions.DependencyInjection;

namespace DA.GtSWB.Domain.Models.Settings;

public class Configuraties(IConfiguratieService service)
{
    public Money Contributie => _contributie.Value;
    private readonly Lazy<Money> _contributie = new(()
        => service.GetConfiguratieOrFallback("Contributie", new Money(25)));

    public Money Administratiekosten => _administratiekosten.Value;
    private readonly Lazy<Money> _administratiekosten = new(()
        => service.GetConfiguratieOrFallback("Kosten.Administratie", new Money(2.5m)));

    public Money Herinneringskosten => _herinneringskosten.Value;
    private readonly Lazy<Money> _herinneringskosten = new(()
        => service.GetConfiguratieOrFallback("Kosten.Herinnering", new Money(5)));

    public Money Aangetekendkosten => _aangetekendkosten.Value;
    private readonly Lazy<Money> _aangetekendkosten = new(()
        => service.GetConfiguratieOrFallback("Kosten.Aangetekend", new Money(10)));

    public Iban VerenigingIban => _verenigingIban.Value;
    private readonly Lazy<Iban> _verenigingIban = new(()
        => service.GetConfiguratieOrFallback("Vereniging.Iban", new Iban("NL00RABO0000000000")));

    public string VerenigingTenNameVan => _verenigingTenNameVan.Value;
    private readonly Lazy<string> _verenigingTenNameVan = new(()
        => service.GetConfiguratieOrFallback("Vereniging.TenNameVan", "[Ten name van]"));

    public string VerenigingBic => _verenigingBic.Value;
    private readonly Lazy<string> _verenigingBic = new(()
        => service.GetConfiguratieOrFallback("Vereniging.Bic", "[Vereniging.Bic]"));

    public string VerenigingIncassantId => _verenigingIncassantId.Value;
    private readonly Lazy<string> _verenigingIncassantId = new(()
        => service.GetConfiguratieOrFallback("Vereniging.IncassantId", "[Vereniging.IncassantId]"));
}