namespace DA.GtSWB.Common.Types.IDs;

public record struct ContributieOpdrachtId(Ulid Value) : IId
{
    public static ContributieOpdrachtId Create() => new(Ulid.NewUlid());

    public override readonly string ToString() => Value.ToString();
}
