using System.Text.Json;

namespace ReportApp.Models;

public class ReportConfiguration
{
    public int FirstColumn { get; set; }
    public int DefaultRow { get; set; }
    public int ReportTitleRow { get; set; }
    public int FirstRowForDynamicGroup { get; set; }
    public int FirstRowForStaticGroup { get; set; }
    public int LastColumn { get; set; }
    public int LastRow { get; set; }

    public T LoadFromFile<T>(string path) where T : ReportConfiguration
    {
        var jsonContent = File.ReadAllText(path);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        return JsonSerializer.Deserialize<T>(jsonContent, options);
    }
}
