using DA.GtSWB.Common.Data;
using DA.GtSWB.Domain.Models.Configuration;
using DA.GtSWB.Domain.Models.Contributie;
using DA.GtSWB.Domain.Models.Ledenadministratie;
using DA.GtSWB.Domain.Models.Ledenadministratie.Betaalwijzes;
using DA.GtSWB.Domain.ServiceDefinitions;
using DA.GtSWB.Persistence.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DA.GtSWB.Persistence;
public class LedenAdministratieDbContext(DbContextOptions<LedenAdministratieDbContext> options)
    : DbContext(options), IUnitOfWork
{
    public DbSet<Lid> LedenDbSet => base.Set<Lid>();
    public DbSet<Betaalwijze> BetaalwijzeSet => base.Set<Betaalwijze>();
    public DbSet<ContributieOpdracht> ContributieOpdrachtSet => base.Set<ContributieOpdracht>();
    public DbSet<ConfiguratieItem> ConfiguratieItemSet => base.Set<ConfiguratieItem>();

    public IRepository<Lid> Leden => new DbSetRepository<Lid>(
    LedenDbSet,
    LedenDbSet //.Where(not uitgeschreven)
        .Include(lid => lid.AdresDataDatabase)
        .Include(lid => lid.Personalia)
        .Include(lid => lid.BetaalwijzeDataDatabase)
        .AsSplitQuery()
        );

    //public IRepository<Adres> Adressen => new DbSetRepository<Adres>(
    //    Set<Adres>(),
    //    Set<Adres>().AsNoTracking());

    public IRepository<ConfiguratieItem> ConfiguratieItems => new DbSetRepository<ConfiguratieItem>(
    base.Set<ConfiguratieItem>(),
    base.Set<ConfiguratieItem>());

    public IRepository<Betaalwijze> Betaalwijzes => new DbSetRepository<Betaalwijze>(
        base.Set<Betaalwijze>(),
        base.Set<Betaalwijze>()
            .Include(bw => bw.Leden)
            .ThenInclude(lid => lid.Personalia)
            .Include(bw => bw.VerantwoordelijkLid)
            .ThenInclude(lid => lid!.AdresDataDatabase)
            .AsSplitQuery()
            .Where(bw => bw.VerantwoordelijkLid != null)
        );

    public IRepository<IncassoBetaalwijze> IncassoBetaalwijzes => new DbSetRepository<IncassoBetaalwijze>(
        base.Set<IncassoBetaalwijze>(),
        base.Set<IncassoBetaalwijze>()
            .Include(bw => bw.Leden)
            .ThenInclude(lid => lid.Personalia)
            .Include(bw => bw.VerantwoordelijkLid)
            .ThenInclude(lid => lid!.AdresDataDatabase)
            .Include(bw => bw.Bankrekening)
            .AsSplitQuery()
            .Where(bw => bw.VerantwoordelijkLid != null)
        );

    public IRepository<NotaBetaalwijze> NotaBetaalwijzes => new DbSetRepository<NotaBetaalwijze>(
        base.Set<NotaBetaalwijze>(),
        base.Set<NotaBetaalwijze>()
            .Include(nb => nb.Leden)
            .ThenInclude(nb => nb.Personalia)
            .Include(bw => bw.VerantwoordelijkLid)
            .ThenInclude(lid => lid!.AdresDataDatabase)
            .AsSplitQuery()
            .Where(nb => nb.VerantwoordelijkLidId != null)
        );

    public IRepository<ContributieOpdracht> ContributieOpdrachten => new DbSetRepository<ContributieOpdracht>(
        ContributieOpdrachtSet,
        ContributieOpdrachtSet
            .Include(co => co.BetaalOpdrachtCollectie)
            .ThenInclude(bo => bo.OpdrachtRegelCollectie)
            .AsSplitQuery());

    public IRepository<IncassoOpdracht> IncassoOpdrachten => new DbSetRepository<IncassoOpdracht>(
        Set<IncassoOpdracht>(),
        Set<IncassoOpdracht>()
            .Include(io => io.Bankrekening)
            .Include(io => io.OpdrachtRegelCollectie));

    public IRepository<NotaOpdracht> NotaOpdrachten => new DbSetRepository<NotaOpdracht>(
        Set<NotaOpdracht>(),
        Set<NotaOpdracht>()
            .Include(no => no.OpdrachtRegelCollectie)
            .Include(no => no.NotaTekst));

    public IRepository<NotaTekst> NotaTeksten => new DbSetRepository<NotaTekst>(Set<NotaTekst>(), Set<NotaTekst>());

    public IQueryable<Lid> AllLedenAggregate => LedenDbSet.AsNoTracking();

    public async Task CommitAsync(CancellationToken cancellationToken = default) =>
        await SaveChangesAsync(cancellationToken);

    public IDbContextTransaction BeginTransaction() => Database.BeginTransaction();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.ApplyConfigurationsFromAssembly(PersistenceAssembly.Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
