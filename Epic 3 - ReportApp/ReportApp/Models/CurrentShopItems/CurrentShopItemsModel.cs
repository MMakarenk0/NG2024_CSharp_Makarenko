using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ReportApp.Models.CurrentShopItems;

public class CurrentShopItemsModel
{
    [JsonPropertyName("Point Of Purchase")]
    public string PointOfPurchase { get; set; }
    public List<ShopItem> Items { get; set; }
    public string Seller { get; set; }
}
