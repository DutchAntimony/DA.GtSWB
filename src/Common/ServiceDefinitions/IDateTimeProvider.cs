namespace DA.GtSWB.Common.ServiceDefinitions;

public interface IDateTimeProvider
{
    DateOnly Today { get; }
    DateTime Now { get; }
    DateTime UtcNow { get; }

    string TodayDutchFormat { get; }
}
