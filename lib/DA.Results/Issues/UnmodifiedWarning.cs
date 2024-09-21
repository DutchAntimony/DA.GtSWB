namespace DA.Results.Issues;

public record UnmodifiedWarning(string Property) : Warning
{
    public override string GetMessage() => $"{Property} is niet aangepast.";
}