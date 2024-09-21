namespace DA.Results.Issues;

public record InvalidOperationError(string Message) : Error
{
    public override string GetMessage() => Message;
}

