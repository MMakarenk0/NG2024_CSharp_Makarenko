using ReportApp.Models.CurrentShopItems;

namespace ReportApp.Services.CurrentShopItems
{
    internal class CurrentShopItemsConfigurationLoaderService : IConfigurationLoader<CurrentShopItemsConfiguration>
    {
        public CurrentShopItemsConfiguration LoadConfiguration(string path)
        {
            var configuration = new CurrentShopItemsConfiguration().LoadFromFile(path);
            return configuration;
        }
    }
}
