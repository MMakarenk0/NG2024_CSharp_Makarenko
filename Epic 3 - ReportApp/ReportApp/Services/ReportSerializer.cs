using System.Text.Json;

namespace ReportApp.Services;

public class ReportSerializer
{
    public T DeserializeReportModel<T>(string path)
    {
        var jsonContent = File.ReadAllText(path);
        return JsonSerializer.Deserialize<T>(jsonContent);
    }
    public List<T> DeserializeReportModels<T>(string path)
    {
        var jsonContent = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<T>>(jsonContent);
    }
}
