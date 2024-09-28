namespace DA.GtSWB.Common.Types.IDs;

public record struct LidMutatieId(Ulid Value) : IId
{
    public static LidMutatieId Empty => new(Ulid.Empty);
    public static LidMutatieId Create() => new(Ulid.NewUlid());

    public override readonly string ToString() => Value.ToString();
}
