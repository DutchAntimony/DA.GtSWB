namespace DA.GtSWB.Tests.DataMigration.AuroraDtos;

public record AuroraRekeningnummerDto
{
    public int Id { get; init; }
    public string? Iban { get; init; }
    public string? Bic { get; init; }
    public string? TenNameVan { get; init; }
}
