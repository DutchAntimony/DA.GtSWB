using DA.GtSWB.Domain.ServiceDefinitions;
using DA.GtSWB.Persistence.Extensions;

namespace DA.GtSWB.Persistence.ServiceImplementations;
internal class LidnummerProvider(IUnitOfWork unitOfWork) : ILidnummerProvider
{
    private int _next = -1;

    public async Task<int> GetNextAsync(CancellationToken cancellationToken = default)
    {
        if (_next == -1)
        {
            _next = await unitOfWork.AllLedenAggregate.MaxOrDefaultAsync(lid => lid.Lidnummer, 1, cancellationToken);
        }
        return _next++;
    }
}
