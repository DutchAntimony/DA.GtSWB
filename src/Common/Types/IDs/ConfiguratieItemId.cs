namespace DA.GtSWB.Common.Types.IDs;

public record struct ConfiguratieItemId(Ulid Value) : IId
{
    public static ConfiguratieItemId Create() => new(Ulid.NewUlid());

    public override readonly string ToString() => Value.ToString();
}
