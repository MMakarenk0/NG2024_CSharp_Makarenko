using System.Text.Json.Serialization;

namespace ReportApp.Models.CurrentShopItems;

public class CurrentShopItemsModel
{
    [JsonPropertyName("Point Of Purchase")]
    public string PointOfPurchase { get; set; }
    public List<ShopItem> Items { get; set; }
    public string Seller { get; set; }
}
