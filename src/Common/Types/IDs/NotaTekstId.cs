namespace DA.GtSWB.Common.Types.IDs;

public record struct NotaTekstId(Ulid Value) : IId
{
    public static NotaTekstId Create() => new(Ulid.NewUlid());

    public override readonly string ToString() => Value.ToString();
}