using DA.GtSWB.Common.Data;
using DA.GtSWB.Domain.Models.Contributie;
using DA.GtSWB.Domain.Models.Ledenadministratie;
using Microsoft.EntityFrameworkCore.Storage;

namespace DA.GtSWB.Domain.ServiceDefinitions;

public interface IUnitOfWork : IDisposable
{
    IRepository<Betaalwijze> Betaalwijzes { get; }
    IRepository<Lid> Leden { get; }
    IRepository<ContributieOpdracht> ContributieOpdrachten { get; }
    //IRepository<LidMutatieData> LidMutaties { get; }

    IQueryable<Lid> AllLedenAggregate { get; }

    Task CommitAsync(CancellationToken cancellationToken = default);

    IDbContextTransaction BeginTransaction();
}