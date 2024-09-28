using DA.GtSWB.Common.Data;
using DA.GtSWB.Domain.Models.Ledenadministratie;
using DA.GtSWB.Domain.ServiceDefinitions;
using DA.GtSWB.Persistence.Common;
using DA.GtSWB.Persistence.Configuration.Tests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DA.GtSWB.Persistence;
public class LedenAdministratieDbContext(DbContextOptions<LedenAdministratieDbContext> options)
    : DbContext(options), IUnitOfWork
{
    public DbSet<Lid> LedenDbSet => base.Set<Lid>();
    public DbSet<Betaalwijze> BetaalwijzeSet => base.Set<Betaalwijze>();

    public IRepository<Lid> Leden => new DbSetRepository<Lid>(
    LedenDbSet,
    LedenDbSet //.Where(not uitgeschreven)
        .Include(lid => lid.AdresDataDatabase)
        .Include(lid => lid.Personalia)
        .Include(lid => lid.BetaalwijzeDataDatabase)
        .AsSplitQuery());

    //public IRepository<Adres> Adressen => new DbSetRepository<Adres>(
    //    Set<Adres>(),
    //    Set<Adres>().AsNoTracking());

    public IRepository<Betaalwijze> Betaalwijzes => new DbSetRepository<Betaalwijze>(
        base.Set<Betaalwijze>(),
        base.Set<Betaalwijze>()
            .Include(bw => bw.Leden));

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
