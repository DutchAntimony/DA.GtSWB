namespace DA.GtSWB.Common.Types.IDs;

public record struct AdresId(Ulid Value) : IId
{
    public static AdresId Empty => new(Ulid.Empty);
    public static AdresId Create() => new(Ulid.NewUlid());

    public override readonly string ToString() => Value.ToString();
}
