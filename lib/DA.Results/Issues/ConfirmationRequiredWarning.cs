namespace DA.Results.Issues;

public record ConfirmationRequiredWarning(string Message) : Warning
{
    public override string GetMessage() => Message;
}
