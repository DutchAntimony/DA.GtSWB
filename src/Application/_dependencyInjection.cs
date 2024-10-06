using DA.GtSWB.Application.Behaviour;
using DA.GtSWB.Application.Contributie.Services;
using DA.GtSWB.Application.Ledenadministratie;
using DA.GtSWB.Application.Ledenadministratie.Services;
using DA.GtSWB.Common.Formatters;
using DA.GtSWB.Domain.Models.Settings;
using DA.GtSWB.Domain.ServiceDefinitions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DA.GtSWB.Application;
public static class DependencyInjection
{
    public static IServiceCollection RegisterApplication(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(ApplicationAssembly.Assembly);
            config.AddOpenBehavior(typeof(LoggingPipelineBehaviour<,>));
            config.AddOpenBehavior(typeof(UnitOfWorkPipelineBehaviour<,>));
            //config.AddOpenBehavior(typeof(ValidationPipelineBehaviour<,>));
        });

        services.AddSingleton<IBicProvider, BicProvider>();
        services.AddSingleton<ILidCreationService, LidCreationService>();
        services.AddSingleton<IBetaalwijzeCreationService, BetaalwijzeCreationService>();
        services.AddSingleton<ICreateContributieOpdrachtService, CreateContributieOpdrachtService>();
        services.AddSingleton<INotaTekstProvider, NotaTekstProvider>();
        services.AddSingleton<Configuraties>();
        services.AddSingleton(DefaultFormatters.VolledigeNaam);
        services.AddSingleton(DefaultFormatters.OpTweeRegels);
        return services;
    }
}

public static class ApplicationAssembly
{
    public static Assembly Assembly => typeof(ApplicationAssembly).Assembly;
}
