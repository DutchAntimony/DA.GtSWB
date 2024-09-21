using DA.Sepa.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DA.Sepa;

public static class DependencyInjection
{
    public static IServiceCollection AddSepa(this IServiceCollection services)
    {
        services.AddSingleton<ISepaXmlWriter, SepaXmlWriter>();
        return services;
    }

}
