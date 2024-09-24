namespace DA.GtSWB.Domain.ServiceDefinitions;

public interface ILidnummerProvider
{
    Task<int> GetNextAsync(CancellationToken cancellationToken = default);
}
