namespace DA.GtSWB.Common.Types.IDs;

public record struct OpdrachtRegelId(Ulid Value) : IId
{
    public static OpdrachtRegelId Create() => new(Ulid.NewUlid());

    public override readonly string ToString() => Value.ToString();
}
