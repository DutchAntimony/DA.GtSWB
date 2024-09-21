namespace DA.Results.Issues;

public abstract record Issue
{
    public abstract string GetMessage();
    internal static NonIssue None => new();
}

public static class IssueExtensions
{
    internal static TOut Match<TOut>(this Issue issue,
        TOut successValue,
        TOut warningValue,
        TOut errorValue)
    {
        return issue switch
        {
            NonIssue => successValue,
            Warning => warningValue,
            Error => errorValue,
            _ => throw new NotImplementedException(issue.GetType().Name)
        };
    }

    internal static TOut Match<TOut>(this Issue issue,
        Func<NonIssue, TOut> OnSuccess,
        Func<Warning, TOut> OnWarning,
        Func<Error, TOut> OnError)
    {
        return issue switch
        {
            NonIssue non => OnSuccess(non),
            Warning warn => OnWarning(warn),
            Error error => OnError(error),
            _ => throw new NotImplementedException(issue.GetType().Name)
        };
    }
}
