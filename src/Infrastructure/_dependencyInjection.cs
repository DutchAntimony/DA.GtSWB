using DA.GtSWB.Common.ServiceDefinitions;
using DA.GtSWB.Domain.ServiceDefinitions;
using DA.GtSWB.Infrastructure.Services;
using DA.Sepa;
using Microsoft.Extensions.DependencyInjection;
using QuestPDF.Infrastructure;

namespace DA.GtSWB.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection RegisterInfraServices(this IServiceCollection services)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
        services.AddSingleton<IContributieFileCreator, ContributieFileCreator>();
        services.AddSepa();
        return services;
    }
}
