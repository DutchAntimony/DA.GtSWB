using DA.GtSWB.Common.ServiceDefinitions;

namespace DA.GtSWB.Infrastructure.Services;

public class SystemDateTimeProvider : IDateTimeProvider
{
    public DateOnly Today => DateOnly.FromDateTime(DateTime.Today);
    public DateTime Now => DateTime.Now;
    public DateTime UtcNow => DateTime.UtcNow;

    public string TodayDutchFormat => Today.ToString("dd-MM-yyyy");
}
