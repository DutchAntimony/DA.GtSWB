namespace DA.GtSWB.Common.Types.IDs;

public record struct BetaalwijzeId(Ulid Value) : IId
{
    public static BetaalwijzeId Empty => new(Ulid.Empty);
    public static BetaalwijzeId Create() => new(Ulid.NewUlid());

    public override readonly string ToString() => Value.ToString();
}
