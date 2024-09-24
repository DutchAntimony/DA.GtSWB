using DA.ApplicationLibrary.Behaviours;
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

        return services;
    }
}

public static class ApplicationAssembly
{
    public static Assembly Assembly => typeof(ApplicationAssembly).Assembly;
}
