namespace DA.GtSWB.Common.Types.IDs;

public record struct PersonaliaId(Ulid Value) : IId
{
    public static PersonaliaId Empty => new(Ulid.Empty);
    public static PersonaliaId Create() => new(Ulid.NewUlid());

    public override readonly string ToString() => Value.ToString();
}

