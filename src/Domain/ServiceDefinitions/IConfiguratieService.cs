namespace DA.GtSWB.Domain.ServiceDefinitions;
public interface IConfiguratieService
{
    T GetConfiguratieOrFallback<T>(string key, T fallback);
    void UpdateConfiguratie<T>(string key, T value);
}
