using DA.GtSWB.Common.Data;
using DA.GtSWB.Common.Extensions;
using DA.GtSWB.Domain.ServiceDefinitions;
using DA.GtSWB.Persistence.Common;
using DA.GtSWB.Persistence.ServiceImplementations;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DA.GtSWB.Persistence;
public static class DependencyInjection
{
    public static IServiceCollection RegisterDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var dbName = configuration.GetValue<string>("DbName");
        var baseDir = configuration.GetValue<string>("BaseDir");
        var connection = new SqliteConnection(new SqliteConnectionStringBuilder()
        {
            DataSource = Path.Combine(
                baseDir.EnsureNotEmpty("Configure baseDir in config.json"),
                "Data",
                dbName.EnsureNotEmpty("Configure dbName in config.json"))
        }.ToString());

        services.AddDbContext<LedenAdministratieDbContext>(options =>
            options.UseSqlite(connection, sqliteOptions
                => sqliteOptions.MigrationsAssembly(PersistenceAssembly.Assembly.FullName))
#if DEBUG
            .EnableSensitiveDataLogging(true)
#endif
            );

        services.AddScoped<IUnitOfWork, LedenAdministratieDbContext>();
        services.AddScoped(typeof(ISpecification<>), typeof(QueryableSpecification<>));

        services.AddScoped<ILidnummerProvider, LidnummerProvider>();
        services.AddScoped<IConfiguratieService, ConfiguratieService>();
        return services;
    }
}

public static class PersistenceAssembly
{
    public static Assembly Assembly => typeof(PersistenceAssembly).Assembly;
}

