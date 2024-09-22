using DA.GtSWB.Common.Data;
using DA.GtSWB.Domain.Models.Ledenadministratie;
using DA.GtSWB.Domain.ServiceDefinitions;
using DA.GtSWB.Persistence.Common;
using Microsoft.EntityFrameworkCore;

namespace DA.GtSWB.Persistence;
public class LedenAdministratieDbContext(DbContextOptions<LedenAdministratieDbContext> options)
    : DbContext(options), IUnitOfWork
{
    public DbSet<Lid> LedenDbSet => base.Set<Lid>();

    public IRepository<Lid> Leden => new DbSetRepository<Lid>(
    LedenDbSet,
    LedenDbSet //.Where(not uitgeschreven)
        .Include(lid => lid.AdresDataDatabase)
        .Include(lid => lid.Personalia)
        .AsSplitQuery());

    public async Task CommitAsync() => await SaveChangesAsync();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.ApplyConfigurationsFromAssembly(PersistenceAssembly.Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public IQueryable<Lid> AllLedenAggregate => LedenDbSet.AsNoTracking();
}
