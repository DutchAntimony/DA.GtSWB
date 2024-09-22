namespace DA.GtSWB.Common.Data.IDs;

public record struct LidId(Ulid Value) : IId
{
    public static LidId Empty => new(Ulid.Empty);
    public static LidId Create() => new(Ulid.NewUlid());

    public override readonly string ToString() => Value.ToString();
}
