using DA.GtSWB.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace DataMigration;

internal class Program
{
    static async Task Main(string[] args)
    {
        var connectionString = @"Data Source=C:\Users\romke\AppData\Roaming\Aurora\Data\2024-04-15T07.35.12.sqlite";
        var mandaadFile = @"C:\Users\romke\AppData\Roaming\Aurora\Data\incassoMandaad.csv";

        var dbContext = InitializeDbContext(@"D:\DA.GtSWB\Data\AuroraImport.db");
        await AuroraDataManager.SaveLeden(dbContext, connectionString, mandaadFile);
    }

    private static LedenAdministratieDbContext InitializeDbContext(string file)
    {
        var optionsbuilder = new DbContextOptionsBuilder<LedenAdministratieDbContext>();
        var connection = new SqliteConnection(new SqliteConnectionStringBuilder()
        {
            DataSource = file
        }.ToString());
        optionsbuilder.UseSqlite(connection);
        var dbContext = new LedenAdministratieDbContext(optionsbuilder.Options);
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
        return dbContext;
    }
}
