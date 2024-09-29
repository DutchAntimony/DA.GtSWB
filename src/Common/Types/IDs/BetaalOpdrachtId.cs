namespace DA.GtSWB.Common.Types.IDs;

public record struct BetaalOpdrachtId(Ulid Value) : IId
{
    public static BetaalOpdrachtId Create() => new(Ulid.NewUlid());

    public override readonly string ToString() => Value.ToString();
}