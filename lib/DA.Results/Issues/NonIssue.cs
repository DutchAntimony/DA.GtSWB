namespace DA.Results.Issues;

internal record NonIssue : Issue
{
    public override string GetMessage() => "Something has gone wrong, this message should never be visible.";
}
