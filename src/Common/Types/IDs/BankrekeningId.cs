namespace DA.GtSWB.Common.Types.IDs;

public record struct BankrekeningId(Ulid Value) : IId
{
    public static BankrekeningId Create() => new(Ulid.NewUlid());
    public override readonly string ToString() => Value.ToString();
}