using System.Text.Json;

namespace ReportApp.Models.CurrentShopItems;

public class CurrentShopItemsConfiguration
{
    public int DefaultRow { get; set; }
    public int FirstColumn { get; set; }
    public int ReportTitleRow { get; set; }
    public int FirstRowForDynamicGroup { get; set; }
    public int FirstRowForStaticGroup { get; set; }
    public int LastColumn { get; set; }
    public int LastRow { get; set; }

    public CurrentShopItemsConfiguration LoadFromFile(string path)
    {
        var jsonContent = File.ReadAllText(path);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        return JsonSerializer.Deserialize<CurrentShopItemsConfiguration>(jsonContent, options);
    }
}
