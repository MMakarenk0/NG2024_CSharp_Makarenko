using ReportApp.Models;

namespace ReportApp.Services;

public class ConfigurationLoader
{
    public T LoadConfiguration<T>(string path) where T : ReportConfiguration, new()
    {
        var configuration = new T().LoadFromFile<T>(path);
        return configuration;
    }
}
