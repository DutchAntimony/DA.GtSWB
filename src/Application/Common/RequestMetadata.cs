namespace DA.GtSWB.Application.Common;

public record RequestMetadata
{
    public DateTime Timestamp { get; }
    public string? Gebruiker { get; }

    public RequestMetadata(DateTime timestamp, string gebruiker) =>
        (Timestamp, Gebruiker) = (timestamp, gebruiker.Sanitize());

    internal Result Validate()
    {
        return new ValidationFailureCollection()
            .AddFrom(Timestamp.VerifyNotNull().Bind(Timestamp.VerifyBefore(DateTime.Now)))
            .AddFrom(Gebruiker.VerifyNotEmpty().Bind(Gebruiker.VerifyMaxLength(100)))
            .ToResult();
    }
}
