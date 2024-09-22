namespace DA.GtSWB.Common.Data.IDs;

public interface IId
{
    Ulid Value { get; }
    string ToString();
}
