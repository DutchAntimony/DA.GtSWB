using DA.GtSWB.Common.Data;
using DA.GtSWB.Domain.Models.Configuration;
using DA.GtSWB.Domain.Models.Contributie;
using DA.GtSWB.Domain.Models.Ledenadministratie;
using DA.GtSWB.Domain.Models.Ledenadministratie.Betaalwijzes;
using Microsoft.EntityFrameworkCore.Storage;

namespace DA.GtSWB.Domain.ServiceDefinitions;

public interface IUnitOfWork : IDisposable
{
    IRepository<Betaalwijze> Betaalwijzes { get; }
    IRepository<IncassoBetaalwijze> IncassoBetaalwijzes { get; }
    IRepository<NotaBetaalwijze> NotaBetaalwijzes { get; }
    IRepository<Lid> Leden { get; }
    IRepository<ContributieOpdracht> ContributieOpdrachten { get; }
    IRepository<ConfiguratieItem> ConfiguratieItems { get; }
    IRepository<NotaTekst> NotaTeksten { get; }
    IRepository<IncassoOpdracht> IncassoOpdrachten { get; }
    IRepository<NotaOpdracht> NotaOpdrachten { get; }
    //IRepository<LidMutatieData> LidMutaties { get; }

    IQueryable<Lid> AllLedenAggregate { get; }


    Task CommitAsync(CancellationToken cancellationToken = default);

    IDbContextTransaction BeginTransaction();
}