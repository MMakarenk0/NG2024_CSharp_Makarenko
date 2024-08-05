using ReportApp.Interfaces;
using ReportApp.Models.CurrentShopItems;
using System.Text.Json;

namespace ReportApp.Services.CurrentShopItems;

public class CurrentShopItemsSerializer : ICollectionReportSerializer<CurrentShopItemsModel>
{
    public List<CurrentShopItemsModel> DeserializeReportModels(string path)
    {
        var jsonContent = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<CurrentShopItemsModel>>(jsonContent);
    }
}