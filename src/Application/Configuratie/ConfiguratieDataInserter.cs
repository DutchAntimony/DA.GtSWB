using DA.GtSWB.Common.Types;
using DA.GtSWB.Domain.Models.Configuration;

namespace DA.GtSWB.Application.Configuratie;
public static class ConfiguratieDataInserter
{
    public static IEnumerable<ConfiguratieItem> TeInsertenItems()
    {
        return
        [
            ConfiguratieItem.Create("Contributie", new Money(25).ToString(), typeof(Money).FullName!),
            ConfiguratieItem.Create("Kosten.Administratie", new Money(2.5m).ToString(), typeof(Money).FullName!),
            ConfiguratieItem.Create("Kosten.Herinnering", new Money(5).ToString(), typeof(Money).FullName!),
            ConfiguratieItem.Create("Kosten.Aangetekend", new Money(10).ToString(), typeof(Money).FullName!),
            ConfiguratieItem.Create("Vereniging.Iban", new Iban("NL00RABO0000000000").ToString(), typeof(Iban).FullName!),
            ConfiguratieItem.Create("Vereniging.TenNameVan", "", typeof(string).FullName!),
            ConfiguratieItem.Create("Vereniging.Bic", "", typeof(string).FullName!),
            ConfiguratieItem.Create("Vereniging.IncassantId", "", typeof(string).FullName!),
            ConfiguratieItem.Create("Nota.Titel", "", typeof(string).FullName!),
            ConfiguratieItem.Create("Nota.Aanhef", "", typeof(string).FullName!),
            ConfiguratieItem.Create("Nota.Inleiding", "", typeof(string).FullName!),
            ConfiguratieItem.Create("Nota.Afsluiting", "", typeof(string).FullName!),
            ConfiguratieItem.Create("Nota.Afzender", "", typeof(string).FullName!),
            ConfiguratieItem.Create("Nota.Afzender.Woonplaats", "", typeof(string).FullName!),
            ConfiguratieItem.Create("Nota.Nota.Betaaltermijn", 14.ToString(), typeof(int).FullName!),
        ];
    }
}
