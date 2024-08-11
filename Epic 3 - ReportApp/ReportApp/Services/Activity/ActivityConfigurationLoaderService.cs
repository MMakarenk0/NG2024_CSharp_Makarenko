using ReportApp.Models.ActivityReport;

namespace ReportApp.Services.Activity;

public class ActivityConfigurationLoaderService : IConfigurationLoader<ActivityReportConfiguration>
{
    public ActivityReportConfiguration LoadConfiguration(string path)
    {
        var configuration = new ActivityReportConfiguration().LoadFromFile(path);
        return configuration;
    }
}
