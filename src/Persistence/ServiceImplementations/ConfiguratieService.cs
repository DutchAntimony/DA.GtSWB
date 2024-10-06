using DA.GtSWB.Common.Types;
using DA.GtSWB.Domain.Models.Configuration;
using DA.GtSWB.Domain.ServiceDefinitions;

namespace DA.GtSWB.Persistence.ServiceImplementations;

internal class ConfiguratieService(LedenAdministratieDbContext dbContext) : IConfiguratieService
{
    public T GetConfiguratieOrFallback<T>(string key, T fallback)
    {
        var item = dbContext.Set<ConfiguratieItem>().FirstOrDefault(x => x.Key == key);
        if (item is null)
        {
            return fallback;
        }

        if (typeof(T) == typeof(Money))
        {
            return (T)(object)new Money(decimal.Parse(item.Value));
        }

        if (typeof(T) == typeof(Iban))
        {
            return (T)(object)new Iban(item.Value);
        }

        return (T)Convert.ChangeType(item.Value, Type.GetType(item.Type)!);
    }

    public void UpdateConfiguratie<T>(string key, T value)
    {
        ArgumentNullException.ThrowIfNull(value);
        var item = dbContext.Set<ConfiguratieItem>().FirstOrDefault(c => c.Key == key);
        var type = typeof(T).FullName;

        if (item is not null) // update existing value
        {
            item.UpdateValue(value.ToString()!);
        }
        else // insert new item
        {
            dbContext.Set<ConfiguratieItem>().Add(ConfiguratieItem.Create(key, value.ToString()!, type!));
        }

        dbContext.SaveChanges();
    }
}