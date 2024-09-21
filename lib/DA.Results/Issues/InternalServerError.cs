namespace DA.Results.Issues;

public record InternalServerError(Exception Exception) : Error
{
    public override string GetMessage() => Exception.Message;
    public static implicit operator InternalServerError(Exception ex) => new(ex);
}

