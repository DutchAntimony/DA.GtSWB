using DA.GtSWB.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace DA.GtSWB.Tests.IntegrationTests;
public sealed class IntegrationTestFixture : IDisposable
{
    public LedenAdministratieDbContext DbContext { get; }
    public ISender Sender { get; }

    public IntegrationTestFixture()
    {
        var services = new ServiceCollection();
        var serviceProvider = services.ConfigureServiceProvider();

        DbContext = serviceProvider.GetRequiredService<LedenAdministratieDbContext>();
        DbContext.Database.EnsureDeleted();
        DbContext.Database.EnsureCreated();

        Sender = serviceProvider.GetRequiredService<ISender>();
    }

    public void Dispose()
    {
        DbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}

[CollectionDefinition("IntegrationTestCollection")]
public class IntegrationTestCollection : ICollectionFixture<IntegrationTestFixture> { }
