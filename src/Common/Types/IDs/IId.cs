namespace DA.GtSWB.Common.Types.IDs;

public interface IId
{
    Ulid Value { get; }
    string ToString();
}
