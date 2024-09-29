using DA.GtSWB.Common.Types;

namespace DA.GtSWB.Domain.Models.Contributie.Settings;

public record IncassantSettings(Iban Iban, string Bic, string TenNameVan, string IncassantId);

