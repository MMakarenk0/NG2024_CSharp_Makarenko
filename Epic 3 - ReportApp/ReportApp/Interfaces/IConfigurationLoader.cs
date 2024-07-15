namespace ReportApp.Services;

public interface IConfigurationLoader<T>
{
    T LoadConfiguration(string path);
}
