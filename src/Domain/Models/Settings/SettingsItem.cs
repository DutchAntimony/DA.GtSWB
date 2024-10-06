using DA.GtSWB.Common.Types.IDs;

namespace DA.GtSWB.Domain.Models.Configuration;
public record ConfiguratieItem
{
    public ConfiguratieItemId Id { get; private init; }
    public string Key { get; private init; } = null!;
    public string Value { get; private set; } = null!;
    public string Type { get; private init; } = null!;

    private ConfiguratieItem() { }

    public void UpdateValue(string value)
    {
        Value = value;
    }

    public static ConfiguratieItem Create(string key, string value, string type)
    {
        return new ConfiguratieItem()
        {
            Id = ConfiguratieItemId.Create(),
            Key = key,
            Value = value,
            Type = type
        };
    }
}
