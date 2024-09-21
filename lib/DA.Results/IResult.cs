namespace DA.Results;

public interface IResult
{
    bool IsSuccess { get; }
    bool HasIssue { get; }
}
