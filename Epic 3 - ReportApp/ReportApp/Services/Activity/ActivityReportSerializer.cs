using ReportApp.Models.ActivityReport;
using System.Text.Json;

namespace ReportApp.Services.Activity;

public class ActivityReportSerializer : IReportSerializer<ActivityReportModel>
{
    public ActivityReportModel DeserializeReportModel(string path)
    {
        var jsonContent = File.ReadAllText(path);
        return JsonSerializer.Deserialize<ActivityReportModel>(jsonContent);
    }
}
