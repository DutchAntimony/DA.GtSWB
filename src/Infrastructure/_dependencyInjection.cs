using DA.GtSWB.Common.ServiceDefinitions;
using DA.GtSWB.Infrastructure.Services;
using DA.Sepa;
using Microsoft.Extensions.DependencyInjection;

namespace DA.GtSWB.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection RegisterInfraServices(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
        services.AddSepa();
        return services;
    }
}
