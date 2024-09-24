using DA.GtSWB.Common.Data;
using DA.GtSWB.Domain.Models.Ledenadministratie;

namespace DA.GtSWB.Domain.ServiceDefinitions;

public interface IUnitOfWork : IDisposable
{
    IRepository<Lid> Leden { get; }
    //IRepository<LidMutatieData> LidMutaties { get; }
    //IRepository<BetaalwijzeData> Betaalwijzes { get; }

    IQueryable<Lid> AllLedenAggregate { get; }

    Task CommitAsync(CancellationToken cancellationToken = default);
}
